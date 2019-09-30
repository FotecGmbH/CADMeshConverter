// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        18.02.2019 12:30
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.Collections.Generic;
using Exchange.Models;

namespace CMCWeb.Models
{
    /// <summary>
    /// <para>MeshResultModel - Result of the conversion job</para>
    /// Klasse MeshResultModel. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class MeshResultModel
    {
        /// <summary>
        /// Original model (model from input file)
        /// </summary>
        public MeshDetailModel Original { get; set; }

        /// <summary>
        /// Result model
        /// </summary>
        public MeshDetailModel Result { get; set; }

        /// <summary>
        /// Reduced by percentage
        /// </summary>
        public string ReducedBy { get; set; }
    }

    /// <summary>
    /// Details of the Model
    /// </summary>
    public class MeshDetailModel
    {
        public string FileType
        {
            get
            {
                if (Filename.LastIndexOf('.') < 0) return "";
                var arr = Filename.Split('.');
                var type = arr[arr.Length - 1];
                return type.ToLowerInvariant();
            }
        }

        /// <summary>
        /// Filename
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Relative folder path
        /// </summary>
        public string RelativeFolderPath { get; set; }

        /// <summary>
        /// Preview URL
        /// </summary>
        public string PreviewUrl => $"{RelativeFolderPath}//{Filename}";

        /// <summary>
        /// Filesize
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// Number of vertices
        /// </summary>
        public string NumberOfVertices { get; set; }

        /// <summary>
        /// Number of faces
        /// </summary>
        public string NumberOfFaces { get; set; }

        /// <summary>
        /// Additional Data
        /// </summary>
        public Dictionary<string, string> AdditionalValues { get; set; }
    }
}