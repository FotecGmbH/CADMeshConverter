// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        13.09.2019 08:04
// Developer:      Georg Wernitznig (GWe)
// Project:        CADMeshConverterDemo
//
// Released under GPL-3.0-only

using System.Collections.Generic;

namespace Assets.Scripts
{
    /// <summary>
    /// <para>Constants</para>
    /// Klasse Constants. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// File extensions supported by the mesh converter. .*fbx only when using Meshlab Version 2018.04 or higher.
        /// </summary>
        public static List<string> InputFormats = new List<string>
                                                  {
                                                      ".3ds",
                                                      ".ply",
                                                      ".stl",
                                                      ".obj",
                                                      ".qobj",
                                                      ".off",
                                                      ".ptx",
                                                      ".vmi",
                                                      ".bre",
                                                      ".dae",
                                                      ".ctm",
                                                      ".pts",
                                                      ".apts",
                                                      ".xyz",
                                                      ".pdb",
                                                      ".tri",
                                                      ".asc",
                                                      ".txt",
                                                      ".x3d",
                                                      ".x3dv",
                                                      ".wrl",
                                                      ".fbx"
                                                  };

        public static string RestEndpoint = "http://meshapi.fotec.at:8091";

    }
}