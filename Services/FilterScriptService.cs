// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        16.01.2019 15:30
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Exchange.Models;

namespace CMCCore
{
    /// <summary>
    /// FilterScript Service - Reads and writes (import, export) script files
    /// </summary>
    public class FilterScriptService
    {
        /// <summary>
        ///     Reads the Filter Script of the given inputPath
        /// </summary>
        /// <param name="inputPath">Path to the FilterScripts</param>
        /// <returns></returns>
        public List<FilterScriptExt> ReadFilterScripts(string inputPath)
        {
            List<FilterScriptExt> lstFilterScripts = new List<FilterScriptExt>();

            DirectoryInfo d = new DirectoryInfo(inputPath);
            FileInfo[] Files = d.GetFiles("*.mlx"); //Getting MLX Files
            foreach (FileInfo file in Files)
            {
                using (StreamReader streamReader = new StreamReader(file.FullName))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(FilterScript));

                    var fScript = xmlSerializer.Deserialize(streamReader) as FilterScript;
                    var fScriptExt = new FilterScriptExt();
                    fScriptExt.Filename = file.Name;
                    fScriptExt.Items = fScript.Items;
                    fScriptExt.name = fScript.name;

                    lstFilterScripts.Add(fScriptExt);
                }
            }

            return lstFilterScripts;
        }

        /// <summary>
        /// Reads a single FilterScript
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FilterScript ReadFilterScript(string fileName)
        {
            StreamReader streamReader = new StreamReader(fileName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FilterScript));

            var fScript = xmlSerializer.Deserialize(streamReader) as FilterScript;
            streamReader.Close();
            return fScript;
        }

        /// <summary>
        /// Writes the Filterscript to XML Filepath
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public bool WriteFilterScript(string fileName, FilterScript script)
        {
            var settings = new XmlWriterSettings
                           {
                               Indent = true,
                               OmitXmlDeclaration = true
                           };

            var xml = new StringBuilder();

            using (var writer = XmlWriter.Create(xml, settings))
            {
                writer.WriteDocType("FilterScript", null, null, null);

                var nsSerializer = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty});
                
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(FilterScript));
                xmlSerializer.Serialize(writer, script, nsSerializer);
            }

            File.WriteAllText(fileName, xml.ToString());

            return true;
        }
    }
}