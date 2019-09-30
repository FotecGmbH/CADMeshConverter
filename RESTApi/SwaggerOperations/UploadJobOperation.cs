// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        16.01.2019 17:28
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.IO;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CMCCloud.SwaggerOperations
{
    /// <summary>
    ///     UploadJobOperation - Converter for the file
    /// </summary>
    public class UploadJobOperation : IOperationFilter
    {
        #region Interface Implementations

        /// <summary>
        /// Apply the operation
        /// 
        /// </summary>
        /// <param name="operation">operation</param>
        /// <param name="context">context</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId.ToLower() == "apiuploadjobpost")
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                                         {
                                             Name = "uploadedFile",
                                             In = "formData",
                                             Description = "Upload File",
                                             Required = true,
                                             Type = "file"
                                         });
                operation.Consumes.Add("multipart/form-data");
            }

            if (context.ApiDescription.SupportedResponseTypes.Any(x => x.Type == typeof(Stream)))
                foreach (var responseType in operation.Responses.Where(x => x.Value.Schema?.Type == "file"))
                    operation.Produces = new[] {"application/octet-stream"};
        }

        #endregion
    }
}