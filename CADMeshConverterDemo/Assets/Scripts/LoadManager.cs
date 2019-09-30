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

#if PLATFORM_ANDROID

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// This class is used to check the availability of ARCore on a device.
/// </summary>
public class LoadManager : MonoBehaviour
{
    private bool _quitApp;
    public GameObject ErrorArea;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            StartCoroutine(CheckAvailable());

            if (ARSession.state == ARSessionState.Unsupported)
            {
                ErrorArea.SetActive(true);
                StartCoroutine(QuitApp());
            }
            else
            {
                SceneManager.LoadScene("CadMeshConverter");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator CheckAvailable()
    {
        yield return ARSession.CheckAvailability();
    }

    private IEnumerator QuitApp()
    {
        yield return new WaitForSecondsRealtime(5f);
        Application.Quit();
    }
}
#endif
