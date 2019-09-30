// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        10.09.2019 10:09
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only


using System;
using Exchange.Enum;

namespace Exchange.Models
{
    /// <summary>
    /// <para>Model for the JobResultCode</para>
    /// Klasse JobResultCode. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class JobResultCode
    {
        /// <summary>
        /// Result Code (Error or Success)
        /// </summary>
        public JobResultNumber Number { get; set; }

        /// <summary>
        /// Message if something went wrong
        /// </summary>
        public string Message { get; set; }
    }
}