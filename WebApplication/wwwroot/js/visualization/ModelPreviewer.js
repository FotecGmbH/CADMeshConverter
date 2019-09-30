// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        15.03.2019 10:14
// Developer:      Sabine Pölzlbauer (SPö)
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

'use strict';

var ModelPreviewer = function (elementToBindTo) {
    this.renderer = null;
    this.canvas = elementToBindTo;
    this.aspectRatio = 1;
    this.recalcAspectRatio();

    this.scene = null;
    this.cameraDefaults = {
        posCamera: new THREE.Vector3(0.0, 175.0, 500.0),
        posCameraTarget: new THREE.Vector3(0, 0, 0),
        near: 0.1,
        far: 10000,
        fov: 45
    };
    this.camera = null;
    this.cameraTarget = this.cameraDefaults.posCameraTarget;

    this.controls = null;
};

ModelPreviewer.prototype = {

    constructor: ModelPreviewer,

    initGL: function () {

        this.renderer = new THREE.WebGLRenderer({
            canvas: this.canvas,
            antialias: true,
            autoClear: true
        });

        this.renderer.setClearColor(0x050505);

        this.scene = new THREE.Scene();

        this.camera = new THREE.PerspectiveCamera(this.cameraDefaults.fov, this.aspectRatio, this.cameraDefaults.near, this.cameraDefaults.far);
        this.resetCamera();
        this.controls = new THREE.SyncedTrackballControls(this.camera, this.renderer.domElement);

        var ambientLight = new THREE.AmbientLight(0x404040);
        var directionalLight1 = new THREE.DirectionalLight(0xC0C090);
        var directionalLight2 = new THREE.DirectionalLight(0xC0C090);

        directionalLight1.position.set(-100, -50, 100);
        directionalLight2.position.set(100, 50, -100);

        this.scene.add(directionalLight1);
        this.scene.add(directionalLight2);
        this.scene.add(ambientLight);

        var helper = new THREE.GridHelper(1200, 60, 0xFF4444, 0x404040);
        this.scene.add(helper);
    },

    initContent: function (modelName, modelPath, fileType, materialPath) {

	    materialPath = '/models/materials/default.mtl';

        this._reportProgress({ detail: { text: 'Loading: ' + modelName + ' from Path: ' + modelPath } });

        var scope = this;

        console.log("Init Content with Type: " + fileType + " for Model: " + modelName);

        if (fileType === 'stl') {

            var stlloader = new THREE.STLLoader();



            stlloader.load(modelPath, function (geometry) {

                var defaultMaterial = new THREE.MeshPhongMaterial();
                defaultMaterial.opacity = 1;
                defaultMaterial.color = new THREE.Color(0xcccccc);
                defaultMaterial.shininess = 154;
                defaultMaterial.specular = new THREE.Color(0x2a2a2a);

                var mesh = new THREE.Mesh(geometry, defaultMaterial);
                scope.scene.add(mesh);
            });

        } else if (fileType === 'obj') {

            var objLoader = new THREE.OBJLoader2();
            var callbackOnLoad = function (event) {
                scope.scene.add(event.detail.loaderRootNode);
                console.log('Loading complete: ' + event.detail.modelName);
                scope._reportProgress({ detail: { text: '' } });
            };

            var onLoadMtl = function (materials) {
                objLoader.setModelName(modelName);
                objLoader.setMaterials(materials);
                objLoader.setLogging(true, true);
                objLoader.load(modelPath, callbackOnLoad, null, null, null, false);
            };
            objLoader.loadMtl(materialPath, null, onLoadMtl);

        } else {
            console.warn("FileType " + fileType + " is not supported!");
        }
    },

    _reportProgress: function (event) {
        var output = THREE.LoaderSupport.Validator.verifyInput(event.detail.text, '');
        console.log('Progress: ' + output);
        //TODO: feedback per canvas as overlay???
        document.getElementById('feedback').innerHTML = output;
    },

    resizeDisplayGL: function () {
        this.controls.handleResize();

        this.recalcAspectRatio();
        this.renderer.setSize(this.canvas.offsetWidth, this.canvas.offsetHeight, false);

        this.updateCamera();
    },

    recalcAspectRatio: function () {
        this.aspectRatio = (this.canvas.offsetHeight === 0) ? 1 : this.canvas.offsetWidth / this.canvas.offsetHeight;
    },

    resetCamera: function () {
        this.camera.position.copy(this.cameraDefaults.posCamera);
        this.cameraTarget.copy(this.cameraDefaults.posCameraTarget);

        this.updateCamera();
    },

    updateCamera: function () {
        this.camera.aspect = this.aspectRatio;
        this.camera.lookAt(this.cameraTarget);
        this.camera.updateProjectionMatrix();
    },

    render: function () {
        if (!this.renderer.autoClear) this.renderer.clear();
        this.controls.update();
        this.renderer.render(this.scene, this.camera);
    }

};