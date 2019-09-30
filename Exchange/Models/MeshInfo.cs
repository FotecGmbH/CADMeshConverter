// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        11.09.2019 13:53
// Developer:      Georg Wernitznig (GWe)
// Project         CADMeshConverter
//
// Released under GPL-3.0-only


using System;

namespace Exchange.Models
{
    /// <summary>
    /// <para>Vertex and face information for a mesh.</para>
    /// Klasse MeshInfo. (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class MeshInfo
    {
        public MeshInfo(int faces, int vertices)
        {
            Faces = faces;
            Vertices = vertices;
        }

        /// <summary>
        /// The number of faces of the mesh.
        /// </summary>
        public int Faces { get; private set; }

        /// <summary>
        /// The number of vertices of the mesh.
        /// </summary>
        public int Vertices { get; private set; }

        public override string ToString()
        {
            return $"{Vertices} vertices, {Faces} faces";
        }
    }
}