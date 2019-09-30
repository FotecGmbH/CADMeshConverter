// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        16.04.2019 13:33
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;

namespace CMCCloud.Helper
{
    /// <summary>
    /// Application Settings
    /// </summary>
    public class AppSettings
    {
        #region Properties

        /// <summary>
        ///     Working directory
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        ///     Script directory
        /// </summary>
        public string ScriptDirectory { get; set; }

        /// <summary>
        ///     MeshLab directory
        /// </summary>
        public string MeshLabDirectory { get; set; }

        /// <summary>
        /// Folder for the temporary files that are uploaded -> Will be generated at Root-Level of the hosting application
        /// </summary>
        public string UploadDirectoryName { get; set; }

        /// <summary>
        /// ResultFileName Ending
        /// </summary>
        public string ResultFileNameEnding { get; set; }

        /// <summary>
        /// Name of the filterouptut file
        /// </summary>
        public string FilterOutputFilename { get; set; }

        #endregion
    }
}