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
using Assets.Scripts.Abstract;
using UnityEngine.UI;

/// <summary>
/// This class contains the logic for the filter parametrization. 
/// </summary>
public class FilterManager : UiArea
{
    private bool _updateUi;

    /// <summary>
    /// A slider in the range of 0..1
    /// </summary>
    public Slider ValueSlider;

    /// <summary>
    /// A text used to display the value of the slider in percent.
    /// </summary>
    public Text PercentageText;

    /// <summary>
    /// A button that triggers the <see cref="UploadModel"/> method.
    /// </summary>
    public Button UploadButton;

    public override event EventHandler CloseArea;

    public override object AreaResult { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        ValueSlider.onValueChanged.AddListener(value => { _updateUi = true; });
        _updateUi = true;

        UploadButton.onClick.AddListener(UploadModel);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_updateUi)
            return;

        PercentageText.text = $"{ValueSlider.value * 100} %";
        _updateUi = false;
    }

    /// <summary>
    /// Sets the result and invokes the close event.
    /// </summary>
    private void UploadModel()
    {
        AreaResult = ValueSlider.value;
        CloseArea?.Invoke(null, null);
    }

  
}
