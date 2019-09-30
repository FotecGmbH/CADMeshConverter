// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        13.09.2019 10:15
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only


using System;

namespace CMCWeb.Models
{
    /// <summary>
    /// <para>Error View Model - Model for displaying error messages</para>
    /// Class ErrorViewModel. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Requested ID
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Should show the request ID
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}