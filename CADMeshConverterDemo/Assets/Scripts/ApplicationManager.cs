// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        19.09.2019 06:41
// Developer:      Georg Wernitznig (GWe)
// Project:        CADMeshConverterDemo
//
// Released under GPL-3.0-only

using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using System.Globalization;
using System.IO;
using Assets.Scripts;
using Assets.Scripts.Abstract;
using Newtonsoft.Json;
using SimpleFileBrowser;

public class ApplicationManager : MonoBehaviour
{
    private Task _loadingTask;
    private string _persistentPath;
    private RestAccessClient _client;
    private ArPlacementController _controller;
    private string _fileToReduce;
    private ICollection<Script> _scripts;
    private bool _showFilter;
    private SnackbarManager _snackBar;

    public UiArea FilterArea;
    public GameObject AreaContent;
    public GameObject LoadingArea;


    // Start is called before the first frame update
    void Start()
    {
        _controller = FindObjectOfType<ArPlacementController>();
        _client = new RestAccessClient(Constants.RestEndpoint, new HttpClient());
        _persistentPath = Application.persistentDataPath;
        _snackBar = FindObjectOfType<SnackbarManager>();
        FileBrowser.SetFilters(true, Constants.InputFormats);
    }

    // Update is called once per frame
    void Update()
    {
        if (_loadingTask != null)
        {
            if (LoadingArea.activeSelf && (_loadingTask.IsCompleted || _loadingTask.IsCanceled || _loadingTask.IsFaulted))
            {
                _controller.Active = true;
                LoadingArea.SetActive(false);
            }

            if (!LoadingArea.activeSelf &&
                _loadingTask.Status == TaskStatus.Running ||
                _loadingTask.Status == TaskStatus.WaitingForActivation ||
                _loadingTask.Status == TaskStatus.WaitingToRun)
            {
                _controller.Active = false;
                LoadingArea.SetActive(true);
            }
        }

        if (_showFilter)
        {
            AreaContent.SetActive(true);
            FilterArea.gameObject.SetActive(true);
            FilterArea.CloseArea += FilterArea_CloseArea;
            _showFilter = false;
        }
    }

    /// <summary>
    /// Opens the file explorer.
    /// </summary>
    public void ShowFiles()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    /// <summary>
    /// Called when the filter parametrisation is finished.
    /// </summary>
    /// <param name="sender">Is null.</param>
    /// <param name="args">Is null.</param>
    private void FilterArea_CloseArea(object sender, System.EventArgs args)
    {
        if (!(FilterArea.AreaResult is float value))
            return;

        FilterArea.CloseArea -= FilterArea_CloseArea;
        AreaContent.SetActive(false);
        FilterArea.gameObject.SetActive(false);
        _loadingTask = Task.Run(async () =>
        {
            try
            {
                if (_scripts == null || !_scripts.Any())
                {
                    _snackBar.ShowError("Connectivity error");
                    return;
                }

                _snackBar.ClearError();

                var targetFile = Path.Combine(_persistentPath, "reduced.obj");
                //var scriptToUse = _scripts.FirstOrDefault(s => s.FileName.Equals("QuadricEdgeCollapseDecimation.mlx")).LstFilters.FirstOrDefault(f => f.Name.Equals("Simplification: Quadric Edge Collapse Decimation"));
                var scriptToUse = _scripts.FirstOrDefault(s => s.LstFilters.Any(f => f.Name.Equals("Simplification: Quadric Edge Collapse Decimation")));

                if (scriptToUse == null)
                {
                    _snackBar.ShowError("No suitable script on server!");
                    return;
                }

                var filter = scriptToUse.LstFilters.FirstOrDefault(f => f.Name.Equals("Simplification: Quadric Edge Collapse Decimation"));

                if (filter == null)
                {
                    _snackBar.ShowError("No suitable script on server!");
                    return;
                }

                var param = filter.LstParameters.FirstOrDefault(p => p.Name.Equals("TargetPerc"));

                if (param == null)
                {
                    _snackBar.ShowError("No suitable script on server!");
                    return;
                }
                else
                {
                    _snackBar.ClearError();
                }

                var paramValue = (double)value;
                paramValue = Math.Round(paramValue, 2);
                param.DefaultValue = paramValue.ToString(CultureInfo.InvariantCulture);
                var stream = File.Open(_fileToReduce, FileMode.Open);
                var fileName = _fileToReduce.Substring(_fileToReduce.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                FileParameter file = new FileParameter(stream, fileName);
                var paramList = new List<ScriptParameterFilter>();
                var dictionary = new Dictionary<string, string> { { param.Name, param.DefaultValue } };
                paramList.Add(new ScriptParameterFilter
                {
                    FilterName = filter.Name,
                    FilterParameter = dictionary
                });
                var paramJson = JsonConvert.SerializeObject(paramList);
                var jobResult = await _client.UploadJobAsync(file, paramJson, scriptToUse.FileName, "1");

                if (jobResult == null || jobResult.ResultCode.Number != 0)
                {
                    _snackBar.ShowError("Connectivity error");
                    return;
                }

                _snackBar.ClearError();
                _snackBar.UpdateInfo(jobResult);

                var reducedResult = await _client.DownloadJobFileResultAsync(jobResult.FileResultName, jobResult.JobId);

                if (reducedResult == null)
                {
                    _snackBar.ShowError("Connectivity error");
                    return;
                }

                if (File.Exists(targetFile))
                {
                    File.Delete(targetFile);
                }

                using (reducedResult.Stream)
                using (var resultStream = File.Open(targetFile, FileMode.CreateNew))
                {
                    await reducedResult.Stream.CopyToAsync(resultStream);
                    _controller.MeshFile = targetFile;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Exception Occurred: {e}");
            }
        });
    }

    private IEnumerator ShowLoadDialogCoroutine()
    {
        _controller.Active = false;
        yield return FileBrowser.WaitForLoadDialog(title: "Select Model");

        if (FileBrowser.Success)
        {
            _fileToReduce = FileBrowser.Result;
            _loadingTask = Task.Run(async () =>
            {
                _scripts = await _client.GetScriptsAsync();

                if (_scripts == null || !_scripts.Any())
                    _snackBar.ShowError("Connectivity problems!");
                else
                    _showFilter = true;
            });
        }
        _controller.Active = true;
    }
}
