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

using System;
using UnityEngine;
using UnityEngine.UI;

public class SnackbarManager : MonoBehaviour
{
    private JobResult _result;
    private string _errorText;
    private bool _updateError;
    private bool _updateInfo;

    public Text OriginalModelText;
    public Text ReducedModelText;
    public GameObject ErrorArea;

    // Start is called before the first frame update
    void Start()
    {
        OriginalModelText.text = string.Empty;
        ReducedModelText.text = string.Empty;
        ErrorArea.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_updateInfo)
        {
            var processed = ProcessJobInfo();
            OriginalModelText.text = processed.Item1;
            ReducedModelText.text = processed.Item2;
            _updateInfo = false;
        }

        if (_updateError)
        {
            if (string.IsNullOrWhiteSpace(_errorText) && ErrorArea.activeSelf)
            {
                ErrorArea.SetActive(false);
                _updateError = false;
                return;
            }

            ErrorArea.SetActive(true);
            var errorText = ErrorArea.GetComponentInChildren<Text>(true);
            errorText.text = _errorText;
            _updateError = false;
        }
    }

    public void UpdateInfo(JobResult result)
    {
        if (result != null)
        {
            _result = result;
            _updateInfo = true;
        }
    }

    public void ShowError(string text)
    {
        _errorText = text;
        _updateError = true;
    }

    public void ClearError()
    {
        _errorText = string.Empty;
        _updateError = true;
    }

    private Tuple<string,string> ProcessJobInfo()
    {
        var originalModel = "Original Model:\r\n";
        var reducedModel = "Reduced Model:\r\n";

        foreach (var item in _result.AdditionalData)
        {
            if (item.Key.Equals("Number of Vertices of original model"))
                originalModel += $"Vertex count: {item.Value}\r\n";

            if (item.Key.Equals("Number of Faces of original model"))
                originalModel += $"Face count: {item.Value}\r\n";

            if (item.Key.Equals("Number of Vertices of result model"))
                reducedModel += $"Vertex count: {item.Value}\r\n";

            if (item.Key.Equals("Number of Faces of result model"))
                reducedModel += $"Face count: {item.Value}\r\n";
        }

        if (!string.IsNullOrWhiteSpace(_result.FileInputSize))
            originalModel += $"File size: {_result.FileInputSize}";


        if (!string.IsNullOrWhiteSpace(_result.FileResultSize))
            reducedModel += $"File size: {_result.FileResultSize}";
        
        return new Tuple<string, string>(originalModel, reducedModel);
    }

}
