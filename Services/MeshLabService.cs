// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        16.04.2019 14:02
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Exchange.Models;

namespace CMCCore
{
    /// <summary>
    /// MeshLabService.cs - Interacting with meshlabserver
    /// </summary>
    public class MeshLabService
    {
        /// <summary>
        ///     Starts a new Meshlab Job
        /// </summary>
        /// <param name="filterOutFile">Outputput file for the filter</param>
        /// <param name="inputFile">Input file</param>
        /// <param name="scriptFile">Script file</param>
        /// <param name="outputFile">Output file</param>
        /// <param name="workingDirectory">Working directory</param>
        /// <param name="write2DebugFile">WriteToDebugfile</param>
        /// <param name="meshLabDirectory">Directory of MeshLab</param>
        public void StartJob(string filterOutFile, string inputFile, string scriptFile, string outputFile, string workingDirectory, bool write2DebugFile, string meshLabDirectory)
        {
            var start = new ProcessStartInfo();

            start.FileName = $"{meshLabDirectory}meshlabserver.exe";
            start.Arguments = $"-l \"{filterOutFile}\" -i \"{inputFile}\" -s \"{scriptFile}\" -o \"{outputFile}\"";
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.WorkingDirectory = workingDirectory;
            
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                using (StreamReader errorReader = process.StandardError)
                {
                    string result = reader.ReadToEnd();
                    string error = errorReader.ReadToEnd();

                    if (write2DebugFile)
                    {
                        result = start.FileName + " " + start.Arguments + Environment.NewLine + Environment.NewLine + result;
                        string logFile = Path.Combine(workingDirectory, "log.txt");

                        if (!string.IsNullOrWhiteSpace(error)) result += "\r\n" + "ERRORS:" + "\r\n" + error;

                        File.WriteAllText(logFile, result);
                    }
                    else
                    {
                        Debug.Write(result);
                    }
                }

                process.WaitForExit();
            }
        }

        /// <summary>
        ///     Returns the Hausdorff Distance out of the filter log. Returns a negative value on fail or hausdorff not found.
        /// </summary>
        /// <param name="text">text</param>
        /// <returns></returns>
        public double ParseHausdorff(string text)
        {
            try
            {
                if (!text.Contains("Hausdorff Distance computed")) return -1;

                var hDSubstring = text.Substring(text.LastIndexOf("Hausdorff Distance computed", StringComparison.Ordinal));
                var valueString = hDSubstring.Substring(hDSubstring.IndexOf("mean", StringComparison.Ordinal) + 6, 8);

                IFormatProvider format = CultureInfo.InvariantCulture;

                if (double.TryParse(valueString, NumberStyles.Any, format, out double hausdorff))
                    return hausdorff;
                return -1;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return -1;
            }
        }

        /// <summary>
        /// Parses the information for the Mesh Info
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public MeshInfo ParseMeshInfo(string text)
        {
            try
            {
                if (!text.Contains(" loaded has "))
                {
                    return null;
                }

                var loadedSubstring = text.Substring(text.LastIndexOf(" loaded has ", StringComparison.Ordinal));
                var start = loadedSubstring.LastIndexOf("has ", StringComparison.Ordinal);
                var end = loadedSubstring.IndexOf("\r\n", StringComparison.Ordinal);

                if (end - start < 1)
                    return null;

                var valuestring = loadedSubstring.Substring(start, end - start);
                var values = valuestring.Split(' ');

                if (!int.TryParse(values[1], out int vertices))
                    return null;

                if (!int.TryParse(values[3], out int faces))
                    return null;

                return new MeshInfo(faces, vertices);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}