// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        12.09.2019 15:39
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
    /// <para>Mesh Model - Model for the Mesh (Scripts and supported export formats)</para>
    /// Klasse MeshModel. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class MeshModel
    {
        /// <summary>
        /// Scripts to choose with its parameters
        /// </summary>
        public ICollection<ServiceAccess.Script> Scripts { get; set; }

        /// <summary>
        /// Unterstützte Ausgabeformate
        /// </summary>
        public ICollection<ServiceAccess.SupportedExportFormat> SupportedExportFormats { get; set; }
    }
}