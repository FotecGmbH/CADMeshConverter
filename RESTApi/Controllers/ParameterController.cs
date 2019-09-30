// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        17.01.2019 09:48
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMCCloud.Helper;
using CMCCore;
using Exchange.Enum;
using Exchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CMCCloud.Controllers
{
    /// <summary>
    /// Parameter Controller - API for getting the scripts with the parameters
    /// </summary>
    [ApiController]
    public class ParameterController : ControllerBase
    {
        /// <summary>
        /// Application Settings from appsettings.json
        /// </summary>
        private readonly AppSettings _appSettings;

        /// <summary>
        /// List of supported output formats
        /// </summary>
        public static List<SupportedFormat> LstSupportedExportFormats = new List<SupportedFormat>()
                                                                              {
                                                                                  new SupportedFormat(){Description = "Alias Wavefront Object (*.obj)", Extension = "obj", Id = 1},
                                                                                  new SupportedFormat(){Description = "X3D File Format - XML encoding (*.x3d)", Extension = "x3d", Id = 2},
                                                                                  new SupportedFormat(){Description = "STL File Format (*.stl)", Extension = "stl", Id = 3},
                                                                                  new SupportedFormat(){Description = "JavaScript JSON (*.json)", Extension = "json", Id = 4},
                                                                                  new SupportedFormat(){Description = "GL Transmission Format 2.0 (*.gltf)", Extension = "gltf", Id = 5},
                                                                                  new SupportedFormat(){Description = "Collada File Format (*.dae)", Extension = "dae", Id = 6}
                                                                              };

        /// <summary>
        /// List of supported input formats
        /// </summary>
        public static List<SupportedFormat> LstSupportedInputFormats = new List<SupportedFormat>()
                                                                              {
                                                                                  new SupportedFormat(){Description = "3D Studio File Format (*.3ds)", Extension = "3ds", Id = 1},
                                                                                  new SupportedFormat(){Description = "Stanford Polygon File Format (*.ply)", Extension = "ply", Id = 2},
                                                                                  new SupportedFormat(){Description = "STL File Format (*.stl)", Extension = "stl", Id = 3},
                                                                                  new SupportedFormat(){Description = "Alias Wavefront Object (*.obj)", Extension = "obj", Id = 4},
                                                                                  new SupportedFormat(){Description = "Quad Object (*.qobj)", Extension = "qobj", Id = 5},
                                                                                  new SupportedFormat(){Description = "Object File Format (*.off)", Extension = "off", Id = 6},
                                                                                  new SupportedFormat(){Description = "PTX File Format (*.ptx)", Extension = "ptx", Id = 7},
                                                                                  new SupportedFormat(){Description = "VCG Dump File Format (*.vmi)", Extension = "vmi", Id = 8},
                                                                                  new SupportedFormat(){Description = "FBX Autodesk Interchange Format (*.fbx)", Extension = "fbx", Id = 9},
                                                                                  new SupportedFormat(){Description = "Breuckmann File Format (*.bre)", Extension = "bre", Id = 10},
                                                                                  new SupportedFormat(){Description = "Collada File Format (*.dae)", Extension = "dae", Id = 11},
                                                                                  new SupportedFormat(){Description = "OpenCTM compressed format (*.ctm)", Extension = "ctm", Id = 12},
                                                                                  new SupportedFormat(){Description = "Expe's point set - binary (*.pts)", Extension = "pts", Id = 13},
                                                                                  new SupportedFormat(){Description = "Expe's point set - ascii (*.apts)", Extension = "apts", Id = 14},
                                                                                  new SupportedFormat(){Description = "XYZ Point Cloud - with and without normal (*.xyz)", Extension = "xyz", Id = 15},
                                                                                  new SupportedFormat(){Description = "Protein Data Bank (*.pdb)", Extension = "pdb", Id = 16},
                                                                                  new SupportedFormat(){Description = "TRI (photogrammetric reconstructions) (*.tri)", Extension = "tri", Id = 17},
                                                                                  new SupportedFormat(){Description = "ASC (ascii triplets of points) (*.asc)", Extension = "asc", Id = 18},
                                                                                  new SupportedFormat(){Description = "TXT (Generic ASCII point list) (*.txt)", Extension = "txt", Id = 19},
                                                                                  new SupportedFormat(){Description = "X3D File Format - XML encoding (*.x3d)", Extension = "x3d", Id = 20},
                                                                                  new SupportedFormat(){Description = "X3D File Format - VRML encoding (*.x3dv)", Extension = "x3dv", Id = 21},
                                                                                  new SupportedFormat(){Description = "VRML 2.0 File Format (*.wrl)", Extension = "wrl", Id = 22}
                                                                              };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appSettings">Application Settings</param>
        public ParameterController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Get the supported export formats
        /// </summary>
        [HttpGet]
        [Route("api/GetSupportedExportFormats")]
        public List<SupportedFormat> GetSupportedExportFormats()
        {
            return LstSupportedExportFormats;
        }

        /// <summary>
        /// Get the supported export formats
        /// </summary>
        [HttpGet]
        [Route("api/GetSupportedInputFormats")]
        public List<SupportedFormat> GetSupportedInputFormats()
        {
            return LstSupportedInputFormats;
        }

        /// <summary>
        /// Get the scripts with its parameters
        /// </summary>
        /// <returns>List of Scripts</returns>
        [Route("api/GetScripts")]
        [HttpGet]
        public List<Script> GetScripts()
        {
            List<Script> result = new List<Script>();

            FilterScriptService manager = new FilterScriptService();
            var filterScripts = manager.ReadFilterScripts(_appSettings.ScriptDirectory);

            if (filterScripts != null && filterScripts.Any())
                foreach (var filterScript in filterScripts)
                {
                    Script s = new Script();
                    s.FileName = filterScript.Filename;
                    s.Description = filterScript.name;
                    if (filterScript.Items != null)
                    {
                        s.LstFilters = new List<FilterScriptParameter>();
                        foreach (var item in filterScript.Items)
                        {
                            FilterScriptParameter fsp = new FilterScriptParameter();

                            if (item.GetType() == typeof(FilterScriptXmlfilter))
                            {
                                var filterScriptXmlItem = (FilterScriptXmlfilter)item;
                                fsp.LstParameters = new List<ScriptParameter>();
                                fsp.Name = filterScriptXmlItem.name;
                                if (filterScriptXmlItem.xmlparam != null)
                                    foreach (var param in filterScriptXmlItem?.xmlparam)
                                    {
                                        ScriptParameter p = new ScriptParameter();
                                        p.DefaultValue = param.value;
                                        p.Name = param.name;
                                        p.Type = ScriptParameterType.Text;
                                        fsp.LstParameters.Add(p);
                                    }

                                s.LstFilters.Add(fsp);
                            }

                            if (item.GetType() == typeof(FilterScriptFilter))
                            {
                                var filterScriptXmlItem = (FilterScriptFilter)item;
                                fsp.LstParameters = new List<ScriptParameter>();
                                fsp.Name = filterScriptXmlItem.name;
                                if (filterScriptXmlItem.Param != null)
                                    foreach (var param in filterScriptXmlItem?.Param)
                                    {
                                        ScriptParameter p = new ScriptParameter();
                                        p.Description = param.description;
                                        p.DefaultValue = param.value;
                                        p.Name = param.name;
                                        p.Tooltip = param.tooltip;
                                        p.Type = ParameterConverter.ConvertParameter(param.type);
                                        fsp.LstParameters.Add(p);
                                    }

                                s.LstFilters.Add(fsp);
                            }
                        }
                    }

                    result.Add(s);
                }

            return result;
        }
    }
}