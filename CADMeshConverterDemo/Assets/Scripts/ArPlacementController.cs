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

using Dummiesman;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// This class is used to place objects on a detected plane and to change the appearance of the object when the mesh gets updated;
/// </summary>
public class ArPlacementController : MonoBehaviour
{
    private Camera _camera;
    private Pose? _lastPose;
    private string _meshFile;
    private bool _initialized;
    private bool _updateMesh;
    private ARSessionOrigin _origin;
    private ARRaycastManager _manager;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private GameObject _placed;
    public GameObject PlacementPrefab;
    public Material MaterialToUse;
    public GameObject LoadingArea;

    /// <summary>
    /// Should the Input be active.
    /// </summary>
    public bool Active { get; set; } = true;

    /// <summary>
    /// The path to the model/mesh file to load.
    /// </summary>
    public string MeshFile
    {
        set
        {
            if (value != null)
            {
                _meshFile = value;
                _updateMesh = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _manager = FindObjectOfType<ARRaycastManager>();
        _origin = FindObjectOfType<ARSessionOrigin>();
        if (_manager != null && _origin != null && _camera != null)
            _initialized = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!_initialized || !Active)
            return;

        if (_updateMesh)
        {
            UpdateMesh();
        }
            

        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Ended)
                return;

            _hits.Clear();

            if (_manager.Raycast(touch.position, _hits, TrackableType.PlaneWithinPolygon) && _hits.Count > 0)
            {
                _lastPose = _hits[0].pose;

                if (_placed == null)
                {
                    _placed = Instantiate(PlacementPrefab, _lastPose.Value.position, _lastPose.Value.rotation);
                }

                _placed.transform.position = _lastPose.Value.position;
                _placed.transform.rotation = _lastPose.Value.rotation;
            }
        }
    }

    /// <summary>
    /// The size of an imported mesh is unknown. This method resizes the presentation of the mesh to a maximums size of 1.
    /// The resizing process keeps the aspect ratio. 
    /// </summary>
    /// <param name="sizeOfObject"></param>
    private void ResizeImportedObject(Vector3 sizeOfObject)
    {
        if (_placed == null || sizeOfObject.Equals(Vector3.zero))
            return;

        var biggest = 0f;

        if (Math.Abs(sizeOfObject.x) >= Math.Abs(sizeOfObject.y) && Math.Abs(sizeOfObject.x) >= Math.Abs(sizeOfObject.z))
            biggest = Math.Abs(sizeOfObject.x);

        if (Math.Abs(sizeOfObject.y) > Math.Abs(sizeOfObject.x) && Math.Abs(sizeOfObject.y) >= Math.Abs(sizeOfObject.z))
            biggest = Math.Abs(sizeOfObject.y);

        if (Math.Abs(sizeOfObject.z) > Math.Abs(sizeOfObject.x) && Math.Abs(sizeOfObject.z) > Math.Abs(sizeOfObject.y))
            biggest = Math.Abs(sizeOfObject.z);

        if (biggest.Equals(0f))
            return;

        _placed.transform.localScale /= biggest;
    }

    /// <summary>
    /// Updates the mesh after a converted model has been downloaded.
    /// </summary>
    private void UpdateMesh()
    {
        try
        {
           

            LoadingArea.SetActive(true);

            if (_placed != null)
                DestroyImmediate(_placed);

            _placed = new OBJLoader().Load(_meshFile);

            var renderers = _placed.GetComponentsInChildren<MeshRenderer>();
            var size = Vector3.zero;

            foreach (var mrenderer in renderers)
            {
                if (MaterialToUse != null)
                    mrenderer.material = MaterialToUse;

                var msize = mrenderer.bounds.size;

                if (msize.sqrMagnitude > size.sqrMagnitude)
                    size = msize;
            }

            ResizeImportedObject(size);

            if (_lastPose.HasValue)
            {
                _placed.transform.position = _lastPose.Value.position;
                _placed.transform.rotation = _lastPose.Value.rotation;
            }
            else
            {
                _placed.transform.position = _camera.transform.parent.position + Vector3.forward * 3;
            }

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        finally
        {
            _updateMesh = false;
            LoadingArea.SetActive(false);
        }
    }

}
