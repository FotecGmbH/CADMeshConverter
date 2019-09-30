// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        15.02.2019 10:29
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only


using System;
using System.Collections.Generic;
using Exchange.Enum;

namespace Exchange.Models
{
    /// <summary>
    /// <para>Ergebniss eines Jobs</para>
    /// Klasse JobResult. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class JobResult
    {
        /// <summary>
        /// Job ID
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// File Result Name
        /// </summary>
        public string FileResultName { get; set; }

        /// <summary>
        /// Result Code with message
        /// </summary>
        public JobResultCode ResultCode { get; set; }

        /// <summary>
        /// Filesize of the result file
        /// </summary>
        public string FileResultSize { get; set; }

        /// <summary>
        /// Number of vertices of the result
        /// </summary>
        public string ResultNumberOfVertices { get; set; }

        /// <summary>
        /// Number of faces of the result
        /// </summary>
        public string ResultNumberOfFaces { get; set; }

        /// <summary>
        /// File size of the input file
        /// </summary>
        public string FileInputSize { get; set; }

        /// <summary>
        /// Reduced by percentage
        /// </summary>
        public string ReducedBy { get; set; }

        /// <summary>
        /// Additional Data
        /// </summary>
        public Dictionary<string,string> AdditionalData { get; set; }
    }
}