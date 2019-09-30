// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        21.01.2019 13:59
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.Collections.Generic;

namespace Exchange.Models
{
    /// <summary>
    /// <para>ScriptParameterFilter</para>
    /// Klasse ScriptParameterList. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ScriptParameterFilter
    {
        /// <summary>
        /// Name of the filter
        /// </summary>
        public string FilterName { get; set; }

        /// <summary>
        /// Filter Parameters
        /// </summary>
        public Dictionary<string, string> FilterParameter { get; set; }

    }
}