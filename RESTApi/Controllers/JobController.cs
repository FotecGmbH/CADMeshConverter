// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        16.01.2019 16:23
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Assimp;
using CMCCloud.Helper;
using CMCCore;
using Exchange.Enum;
using Exchange.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using FFile = System.IO.File;

namespace CMCCloud.Controllers
{
    /// <summary>
    ///     Job Controller - API for Upload Job and Download a Job Result
    /// </summary>
    [ApiController]
    public class JobController : ControllerBase
    {
        /// <summary>
        ///     Appsettings from appsettings.json
        /// </summary>
        private readonly AppSettings _appSettings;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="appSettings">AppSettings</param>
        public JobController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        /// <summary>
        ///     Upload a new Job
        /// </summary>
        /// <param name="file">Input File</param>
        /// <param name="fScriptParams">Parameters for the script</param>
        /// <param name="scriptFilename">Script to use</param>
        /// <param name="outputFileFormat">format of the file output</param>
        /// <returns>Job Result</returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        [Route("api/UploadJob")]
        [Produces(typeof(JobResult))]
        [STAThread]
        public async Task<JobResult> UploadJob(IFormFile file, [FromForm] string fScriptParams, [FromForm] string scriptFilename, [FromForm] string outputFileFormat = "1")
        {
            //Result of the job
            JobResult res = new JobResult();

            if (String.IsNullOrEmpty(outputFileFormat))
                outputFileFormat = "1"; //Standard OBJ

            try
            {
                List<ScriptParameterFilter> filterScriptParams = new List<ScriptParameterFilter>();

                //Deserialize parameters for the script
                if (fScriptParams != null) filterScriptParams = JsonConvert.DeserializeObject<List<ScriptParameterFilter>>(fScriptParams);

                string folderName = _appSettings.UploadDirectoryName;
                string webRootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string newPath = Path.Combine(webRootPath, folderName);

                if (!Directory.Exists(newPath))
                {
                    try
                    {
                        //Create direcotry if it doesn't exits
                        Directory.CreateDirectory(newPath);
                    }
                    catch (Exception e)
                    {
                        res.ResultCode = new JobResultCode(){Message = e.ToString(), Number = JobResultNumber.OtherError};
                    }
                }
                
                if (file.Length > 0) //Check if filelength greater than zero
                {
                    //Remove apostroph
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');

                    string fullPath = Path.Combine(newPath, fileName);

                    try
                    {
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                    catch (Exception e)
                    {
                        res.ResultCode = new JobResultCode() { Message = e.ToString(), Number = JobResultNumber.OtherError };
                    }
                    
                    //Generates a ramdom job ID
                    string jobId = Guid.NewGuid().ToString("N");
                    res.JobId = jobId;

                    string pathToWorkJob = Path.Combine(_appSettings.WorkingDirectory, jobId);

                    try
                    {
                        Directory.CreateDirectory(pathToWorkJob);
                    }
                    catch (Exception e)
                    {
                        res.ResultCode = new JobResultCode() {Message = e.ToString(), Number = JobResultNumber.OtherError};
                        return res;
                    }
                    
                    var workJobFileInput = Path.Combine(pathToWorkJob, CorrectFileName(fileName));

                    FFile.Copy(fullPath, workJobFileInput);
                    string workJobScript = Path.Combine(pathToWorkJob, scriptFilename);
                    FFile.Copy(Path.Combine(_appSettings.ScriptDirectory, scriptFilename), workJobScript);

                    FilterScriptService fManager = new FilterScriptService();
                    var fScript = fManager.ReadFilterScript(workJobScript);
                    foreach (var filter in filterScriptParams)
                    {
                        var filterScriptXml = fScript.Items.FirstOrDefault(a => a.GetType() == typeof(FilterScriptXmlfilter) && ((FilterScriptXmlfilter) a).name == filter.FilterName);
                        var filterScript = fScript.Items.FirstOrDefault(a => a.GetType() == typeof(FilterScriptFilter) && ((FilterScriptFilter) a).name == filter.FilterName);

                        if (filterScript != null)
                            foreach (var filterParameter in filter.FilterParameter)
                            {
                                var param = ((FilterScriptFilter) filterScript).Param?.ToList().FirstOrDefault(a => a.name == filterParameter.Key);
                                if (param != null) param.value = filterParameter.Value;
                            }

                        if (filterScriptXml != null)
                            foreach (var filterParameter in filter.FilterParameter)
                            {
                                var param = ((FilterScriptXmlfilter) filterScriptXml).xmlparam?.ToList().FirstOrDefault(a => a.name == filterParameter.Key);
                                if (param != null) param.value = filterParameter.Value;
                            }
                    }

                    fManager.WriteFilterScript(workJobScript, fScript);
                    var index = CheckFileExtension(workJobFileInput);

                    if (index < 0) //Filetype is not supported
                    {
                        res.ResultCode = new JobResultCode()
                                         {
                                             Message = "Filetype is not supported. See supported filetypes.",
                                             Number = JobResultNumber.FiletypeNotSupported
                                         };
                        return res;
                    }

                    var oFileF = int.Parse(outputFileFormat);

                    //If gltf, do job with obj an then convert it to gltf
                    if (outputFileFormat == "5")
                    {
                        oFileF = 1;
                    }

                    string jobResultFilename = $"{workJobFileInput.Substring(0, index)}{_appSettings.ResultFileNameEnding}.{ParameterController.LstSupportedExportFormats.FirstOrDefault(a => a.Id == oFileF).Extension}";
                    
                    var outputFile = Path.Combine(pathToWorkJob, jobResultFilename);
                    string filterOutputFile = Path.Combine(pathToWorkJob, $"{_appSettings.FilterOutputFilename}.txt");

                    MeshLabService mService = new MeshLabService();

                    try
                    {
                        mService.StartJob(filterOutputFile, workJobFileInput, workJobScript, outputFile, pathToWorkJob, true, _appSettings.MeshLabDirectory);
                    }
                    catch (Exception e)
                    {
                        res.ResultCode = new JobResultCode()
                                         {
                                             Number = JobResultNumber.JobFailed,
                                             Message = e.ToString()
                                         };
                        return res;
                    }

                    if (!FFile.Exists(outputFile))
                    {
                        res.ResultCode = new JobResultCode()
                        {
                                Number = JobResultNumber.JobFailed,
                                Message = FFile.ReadAllText(filterOutputFile)
                        };
                        return res;
                    }

                    if (outputFileFormat == "5")
                    {
                        try
                        {
                            using (AssimpContext cont = new AssimpContext())
                            {
                                var tmpDir = Path.Combine(pathToWorkJob, "output");
                                Directory.CreateDirectory(tmpDir);

                                var x = cont.ConvertFromFileToFile(jobResultFilename, Path.Combine(tmpDir, "output.gltf"), "gltf2");
                            }
                        }
                        catch(Exception){}
                    }

                    res.FileResultName = Path.GetFileName(jobResultFilename);

                    var hausdorff = -1.0;
                    MeshInfo meshInfo = null;
                    
                    if (FFile.Exists(filterOutputFile))
                    {
                        var foFile = FFile.ReadAllText(filterOutputFile);
                        hausdorff = mService.ParseHausdorff(foFile);
                        meshInfo = mService.ParseMeshInfo(foFile);
                    }

                    FileInfo fiResult   = new FileInfo(outputFile);
                    FileInfo fiOriginal = new FileInfo(workJobFileInput);

                    res.AdditionalData = new Dictionary<string, string>();
                    if (hausdorff > 0)
                    {
                        res.AdditionalData.Add("Hausdorff", hausdorff.ToString(CultureInfo.InvariantCulture));
                    }

                    try
                    {
                        using (AssimpContext importer = new AssimpContext())
                        { 
                            var model = importer.ImportFile(workJobFileInput);
                            long cntVertex = 0;
                            long cntFaces = 0;

                            foreach (var modelMesh in model.Meshes)
                            {
                                cntVertex += modelMesh.VertexCount;
                                cntFaces += modelMesh.FaceCount;
                            }

                            res.AdditionalData.Add("Number of Vertices of original model", cntVertex.ToString("N0"));
                            res.AdditionalData.Add("Number of Faces of original model", cntFaces.ToString("N0"));
                            importer.Dispose();
                        }
                    }
                    catch (Exception)
                    {}

                    try
                    {
                        using (AssimpContext importer = new AssimpContext())
                        {
                            var model = importer.ImportFile(outputFile);
                            long cntVertex = 0;
                            long cntFaces = 0;

                            foreach (var modelMesh in model.Meshes)
                            {
                                cntVertex += modelMesh.VertexCount;
                                cntFaces += modelMesh.FaceCount;
                            }

                            res.AdditionalData.Add("Number of Vertices of result model", cntVertex.ToString("N0"));
                            res.AdditionalData.Add("Number of Faces of result model", cntFaces.ToString("N0"));
                            importer.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    
                    res.FileInputSize = GetFileSizeInBytes(fiOriginal.Length);
                    if (outputFileFormat == "5")
                    {
                        try
                        {
                            using (AssimpContext cont = new AssimpContext())
                            {
                                var tmpDir = Path.Combine(pathToWorkJob, "output");
                                ZipFile.CreateFromDirectory(tmpDir, Path.Combine(pathToWorkJob, "output.zip"));
                                jobResultFilename = Path.Combine(pathToWorkJob, "output.zip");
                                res.FileResultName = "output.zip";
                                DirectoryInfo info = new DirectoryInfo(tmpDir);
                                long totalSize = info.EnumerateFiles().Sum(f => f.Length);
                                res.FileResultSize = GetFileSizeInBytes(totalSize);
                                res.ReducedBy = (100.0 - (double)totalSize / fiOriginal.Length * 100.0).ToString("F") + "%";
                            }
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        res.FileResultSize = GetFileSizeInBytes(fiResult.Length);
                        res.ReducedBy = (100.0 - (double)fiResult.Length / fiOriginal.Length * 100.0).ToString("F") + "%";
                    }

                    res.ReducedBy = (100.0 - (double) fiResult.Length / fiOriginal.Length * 100.0).ToString("F") + "%";
                    
                    res.ResultCode = new JobResultCode()
                                     {
                                         Message = "Success",
                                         Number = JobResultNumber.Success
                                     };

                    return res;
                }
                else
                {
                    res.ResultCode = new JobResultCode()
                                     {
                                         Number = JobResultNumber.FileRequired,
                                         Message = "File is required!"
                                     };
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResultCode = new JobResultCode()
                                 {
                                     Number = JobResultNumber.OtherError,
                                     Message = ex.ToString()
                                 };
                return res;
            }
        }

        /// <summary>
        ///     Download converted file of the job
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="jobId">Job Id</param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        [Route("api/DownloadJobFileResult")]
        [ProducesResponseType(typeof(Stream), 200)]
        public async Task<IActionResult> DownloadJobFileResult([FromForm] string filename, [FromForm] string jobId)
        {
            string pathToWorkJob = Path.Combine(_appSettings.WorkingDirectory, jobId);
            var outputFile = Path.Combine(pathToWorkJob, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(outputFile, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory, "APPLICATION/octet-stream", Path.GetFileName(outputFile));
        }

        /// <summary>
        /// Downloads the log file of the convertion
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/DownloadLogForJob/{jobId}")]
        public async Task<IActionResult> DownloadLogForJob(string jobId)
        {
            string pathToWorkJob = Path.Combine(_appSettings.WorkingDirectory, jobId);
            var filterOutput = Path.Combine(pathToWorkJob, $"{_appSettings.FilterOutputFilename}.txt");

            var memory = new MemoryStream();

            if (!FFile.Exists(filterOutput))
                return null;

            using (var stream = new FileStream(filterOutput, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory, "APPLICATION/octet-stream", Path.GetFileName(filterOutput));
        }


        /// <summary>
        /// Format Filezise
        /// </summary>
        /// <param name="TotalBytes">Count of Bytes</param>
        /// <returns></returns>
        private string GetFileSizeInBytes(long TotalBytes)
        {
            if (TotalBytes >= 1073741824) //Giga Bytes
            {
                Decimal fileSize = Decimal.Divide(TotalBytes, 1073741824);
                return $"{fileSize:##.##} GB";
            }
            else if (TotalBytes >= 1048576) //Mega Bytes
            {
                Decimal fileSize = Decimal.Divide(TotalBytes, 1048576);
                return $"{fileSize:##.##} MB";
            }
            else if (TotalBytes >= 1024) //Kilo Bytes
            {
                Decimal fileSize = Decimal.Divide(TotalBytes, 1024);
                return $"{fileSize:##.##} KB";
            }
            else if (TotalBytes > 0)
            {
                Decimal fileSize = TotalBytes;
                return $"{fileSize:##.##} Bytes";
            }
            else
            {
                return "0 Bytes";
            }
        }

        /// <summary>
        ///     Checks if the file extension is valid.
        /// </summary>
        /// <param name="filename">
        ///     Returns the index of the extension separator ('.') or a negative number when the extension is
        ///     invalid.
        /// </param>
        /// <returns></returns>
        private int CheckFileExtension(string filename)
        {
            if (!filename.Contains(".")) return -1;

            var dotIndex = filename.LastIndexOf(".", StringComparison.Ordinal);

            if (dotIndex == filename.Length - 1 || dotIndex == 0) return -1;

            if (Regex.IsMatch(filename, @"([^\s]+(\.(?i)(stl|obj|fbx|3ds|ply|qobj|off|ptx|vmi|bre|dae|ctm|pts|apts|xyz|pdb|tri|asc|txt|x3d|x3dv|wrl))$)", RegexOptions.IgnoreCase)) return dotIndex;
            return -1;
        }

        /// <summary>
        ///     Removes problematic characters from the filename.
        /// </summary>
        /// <param name="filename">The original filename.</param>
        /// <returns>A cleaned filename</returns>
        private string CorrectFileName(string filename)
        {
            string corrected = string.Empty;

            foreach (var character in filename)
                if (!Constants.ProblematicCharacters.Contains(character))
                    corrected += character;

            return corrected;
        }
    }
}