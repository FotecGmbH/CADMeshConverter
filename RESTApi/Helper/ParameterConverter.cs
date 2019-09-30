// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        17.01.2019 10:15
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only


using System;
using Exchange.Enum;

namespace CMCCloud.Helper
{
    /// <summary>
    /// <para>Converts the Parameter type</para>
    /// Klasse ParameterConverter. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public static class ParameterConverter
    {
        /// <summary>
        /// Converts the Parameter Type
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static ScriptParameterType ConvertParameter(string parameterName)
        {
            switch (parameterName)
            {
                case "RichInt":
                    return ScriptParameterType.Integer;
                case "RichFloat":
                    return ScriptParameterType.Float;
                case "RichBool":
                    return ScriptParameterType.Boolean;
                case "Text": //???
                    return ScriptParameterType.Text;
                default:
                    return ScriptParameterType.Unknown;
            }
        }
            
    }
}