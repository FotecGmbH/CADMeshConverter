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
using Exchange.Enum;

namespace Exchange.Models
{
    /// <summary>
    /// <para>Parameter of a Script</para>
    /// Klasse ScriptParameters. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ScriptParameter
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Tooltip
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public ScriptParameterType Type { get; set; }
    }
}