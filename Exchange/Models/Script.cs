// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        17.01.2019 09:52
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only


using System;
using System.Collections.Generic;

namespace Exchange.Models
{
    /// <summary>
    /// <para>Script with Parameters</para>
    /// Klasse Script. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class Script
    {
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Filename
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Filters
        /// </summary>
        public List<FilterScriptParameter> LstFilters { get; set; }
    }

    public class FilterScriptParameter
    {
        /// <summary>
        /// Name of the Filter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of Parameters
        /// </summary>
        public List<ScriptParameter> LstParameters { get; set; }
    }
}