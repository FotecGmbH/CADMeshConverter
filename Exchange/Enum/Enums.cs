// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Erstversion vom 17.01.2019 09:55
// Entwickler      Manuel Fasching
// Projekt         CADMeshConverter

using System;

namespace Exchange.Enum
{
    /// <summary>
    /// Enum for Script Parameter Types
    /// </summary>
    public enum ScriptParameterType
    {
        /// <summary>
        /// Integer
        /// </summary>
        Integer,
        /// <summary>
        /// Float
        /// </summary>
        Float,
        /// <summary>
        /// Text
        /// </summary>
        Text,
        /// <summary>
        /// Boolean
        /// </summary>
        Boolean,
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Result-Code for a job
    /// </summary>
    public enum JobResultNumber
    {
        /// <summary>
        /// The job finished successfully 
        /// </summary>
        Success = 0,
        FileRequired = 1,
        FiletypeNotSupported = 2,
        JobFailed = 3,
        OtherError = 99
    }
}