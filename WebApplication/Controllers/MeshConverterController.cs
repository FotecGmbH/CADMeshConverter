// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        21.01.2019 16:08
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CMCWeb.Helper;
using CMCWeb.Models;
using CMCWeb.ServiceAccess;
using Exchange.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JobResult = CMCWeb.ServiceAccess.JobResult;

namespace CMCWeb.Controllers
{
    /// <summary>
    /// <para>MechConverterController - UI for processing meshes</para>
    /// Class MechConverterController. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class MeshConverterController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// Controller for the Meshconverter
        /// </summary>
        /// <param name="env">Environment</param>
        /// <param name="appSettings">Application settings</param>
        public MeshConverterController(IHostingEnvironment env, IOptions<AppSettings> appSettings)
        {
            _env = env;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>return Index View</returns>
        public async Task<IActionResult> Index()
        {
            Client c = new Client(_appSettings.RestApiUrl, new HttpClient());

            MeshModel mm = new MeshModel();

            try
            {
                //Load scripts from API
                mm.Scripts = await c.GetScriptsAsync();
                //Load supported export formats from the api
                mm.SupportedExportFormats = await c.GetSupportedExportFormatsAsync();
            }
            catch (Exception e)
            {
                ViewBag.Error = $"API {_appSettings.RestApiUrl} currently not available.";
                ViewBag.Code = "";

                return View("Error");
            }


            return View(mm);
        }

        /// <summary>
        /// Index form submit
        /// </summary>
        /// <param name="file">Input file</param>
        /// <param name="form">Form</param>
        /// <returns>Result as partial view</returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Index(IFormFile file, IFormCollection form)
        {
            MeshResultModel model = new MeshResultModel();
            model.Original = new MeshDetailModel();
            model.Result = new MeshDetailModel();

            string webRootPath = _env.WebRootPath;
            string randomFilePath = Path.GetRandomFileName().Replace(".", "");
            string relativeFolderPath = Path.Combine("models", "obj", randomFilePath);
            string newPath = Path.Combine(webRootPath, relativeFolderPath);

            if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);

            var urlPath = relativeFolderPath.Replace("\\", "/");
            model.Original.RelativeFolderPath = urlPath;
            model.Result.RelativeFolderPath = urlPath;

            List<ScriptParameterFilter> lstScriptParams = new List<ScriptParameterFilter>();

            if (form != null && form.Any())
                foreach (var keyValuePair in form.ToList())
                {
                    var kp = keyValuePair.Key.Split("#");
                    if (kp.Length == 2)
                    {
                        var sp = lstScriptParams.FirstOrDefault(a => a.FilterName == kp[0]);
                        if (sp == null)
                        {
                            sp = new ScriptParameterFilter();
                            sp.FilterName = kp[0];
                            lstScriptParams.Add(sp);
                        }

                        if (sp.FilterParameter == null)
                            sp.FilterParameter = new Dictionary<string, string>();

                        sp.FilterParameter.Add(kp[1], keyValuePair.Value);
                    }
                }

            if (file?.Length > 0)
            {
                model.Original.Filename = file.FileName;
                var originalFilePath = await saveOriginalFile(file, newPath);
                HttpClient hc = new HttpClient();
                hc.Timeout = TimeSpan.FromHours(2);

                Client c = new Client(_appSettings.RestApiUrl, hc);
                FileParameter fileParameter = new FileParameter(file.OpenReadStream(), file.FileName, file.ContentType);

                JobResult jobResult = null;

                try
                {
                    jobResult = await c.UploadJobAsync(fileParameter, JsonConvert.SerializeObject(lstScriptParams), form.FirstOrDefault().Value, form.FirstOrDefault(a => a.Key == "selectedOutputFormat").Value);
                }
                catch (Exception e)
                {
                    //TODO: Schöner
                    ViewBag.Error = "Error at uploading.";
                    ViewBag.Code = e.ToString();

                    return View("Error");
                }
                
                if(jobResult != null && jobResult.ResultCode.Number != JobResultCodeNumber._0)
                {
                    //Job wasn't finished successfull
                    ViewBag.Error = jobResult.ResultCode.Message;
                    ViewBag.Code = jobResult.ResultCode.Number;

                    return View("Error");
                }
                else if (jobResult != null)
                {
                    var resultFile = await c.DownloadJobFileResultAsync(jobResult.FileResultName, jobResult.JobId, newPath);
                    model.Result.Filename = resultFile;
                    model.Result.AdditionalValues = new Dictionary<string, string>(jobResult.AdditionalData);
                    model.Result.FileSize = jobResult.FileResultSize;
                    model.Result.NumberOfFaces = jobResult.ResultNumberOfFaces;
                    model.Result.NumberOfVertices = jobResult.ResultNumberOfVertices;

                    model.ReducedBy = jobResult.ReducedBy;
                    model.Original.FileSize = jobResult.FileInputSize;
                    ViewBag.Link = $"{_appSettings.RestApiUrl}/api/DownloadLogForJob/{jobResult.JobId}";
                }
            }
            else
            {
                //no file selected
                ViewBag.Error = "Please select file";
                ViewBag.Code = "";

                return View("Error");
            }

            return View("~/Views/Visualization/ModelResultPreview.cshtml", model);
        }

        /// <summary>
        /// Saves the file
        /// </summary>
        /// <param name="file">File</param>
        /// <param name="path">Path for the file</param>
        /// <returns></returns>
        private async Task<string> saveOriginalFile(IFormFile file, string path)
        {
            var filePath = Path.Combine(path, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return filePath;
        }
    }
}