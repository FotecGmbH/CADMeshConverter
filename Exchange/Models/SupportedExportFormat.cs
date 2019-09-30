// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        12.09.2019 14:23
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;

namespace Exchange.Models
{
    /// <summary>
    /// <para>Supported Export format</para>
    /// Klasse SupportedExportFormat. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class SupportedFormat
    {
        /// <summary>
        /// Id of the export format
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Description of the export format
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// FileExtension
        /// </summary>
        public string Extension { get; set; }
    }
}