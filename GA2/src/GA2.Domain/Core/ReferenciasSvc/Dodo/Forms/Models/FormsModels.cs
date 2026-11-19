using GA2.Domain.Core.Dodo.Common;
using GA2.Domain.Core.ReferenciasSvc.Dodo.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Forms.Models
{
    // ─────────────────────────────────────────────────────────────────────────────
    // Form_GetParameters
    // ─────────────────────────────────────────────────────────────────────────────

    public class FormGetParametersRequest : IDodoRequest
    {
        public string FormCode { get; set; }
        public int? FormId { get; set; }
        public DodoHeader Header { get; set; } = new();
    }

    public class FormDefinition
    {
        public int IdFormulario { get; set; }
        public string Tabla { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? IdPadre { get; set; }
        public int Estado { get; set; }
        public string Codigo { get; set; }
        public string? Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Host { get; set; }
        public bool ArchivoRequerido { get; set; }
        public bool WorkflowUnico { get; set; }
    }

    public class FormGetParametersResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public FormDefinition? Form { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // Form_Insert
    // ─────────────────────────────────────────────────────────────────────────────

    public class FormInsertRequest : IDodoRequest
    {
        public string OperationUser { get; set; }
        public string FormCode { get; set; }
        public string OfficeCode { get; set; }
        public string? NextStepUser { get; set; }
        public int? ProcessId { get; set; }
        public List<DodoDataField> Data { get; set; } = new();
        public List<DodoFileItem>? Files { get; set; }
        public bool? CheckWorkflow { get; set; }
        public DodoHeader Header { get; set; } = new();
    }

    public class FormInsertResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? BarCode { get; set; }
        public int? ProcessId { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // Form_Update
    // ─────────────────────────────────────────────────────────────────────────────

    public class FormUpdateRequest : IDodoRequest
    {
        public string OperationUser { get; set; }
        public string FormCode { get; set; }
        public List<DodoDataField> Data { get; set; } = new();
        public DodoFilterParameters FilterParameters { get; set; }
        public string? Filter { get; set; }
        public DodoHeader Header { get; set; } = new();
    }

    public class FormUpdateResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Rows { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // Form_Delete
    // ─────────────────────────────────────────────────────────────────────────────

    public class FormDeleteRequest : IDodoRequest
    {
        public string OperationUser { get; set; }
        public string FormCode { get; set; }
        public DodoFilterParameters FilterParameters { get; set; }
        public string? Filter { get; set; }
        public DodoHeader Header { get; set; } = new();
    }

    public class FormDeleteResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Rows { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // Form_GetData
    // ─────────────────────────────────────────────────────────────────────────────

    public class FormGetDataRequest : IDodoRequest
    {
        public string FormCode { get; set; }
        public DodoFilterParameters FilterParameters { get; set; }
        public string? Filter { get; set; }
        public string? OrderColumn { get; set; }
        public string? OrderType { get; set; }
        public int? Top { get; set; }
        public DodoHeader Header { get; set; } = new();
    }

    public class FormGetDataResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<DodoDataField> Data { get; set; } = new();
        public int Rows { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // Form_AttachFiles
    // ─────────────────────────────────────────────────────────────────────────────

    public class FormAttachFilesRequest : IDodoRequest
    {
        public string OperationUser { get; set; }
        public string BarCode { get; set; }
        public List<DodoFileItem> Files { get; set; } = new();
        public bool? CheckWorkflow { get; set; }
        public DodoHeader Header { get; set; } = new();
    }

    public class FormAttachFilesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // Form_GetFileCodes
    // ─────────────────────────────────────────────────────────────────────────────

    public class FormGetFileCodesRequest : IDodoRequest
    {
        public string BarCode { get; set; }
        public string? DirectoryCode { get; set; }
        public DodoHeader Header { get; set; } = new();
    }

    public class FormGetFileCodesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<DodoFileProperty> FileProperties { get; set; } = new();
        public int Rows { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // Form_GetFilesDirectories
    // ─────────────────────────────────────────────────────────────────────────────

    public class FormGetFilesDirectoriesRequest : IDodoRequest
    {
        public string FormCode { get; set; }
        public DodoHeader Header { get; set; } = new();
    }

    public class FormDirectoryItem
    {
        public int IdDirectorio { get; set; }
        public int IdFormulario { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int IdEstado { get; set; }
    }

    public class FormGetFilesDirectoriesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<FormDirectoryItem> Data { get; set; } = new();
        public int Rows { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // Form_GetBase64StringFile
    // ─────────────────────────────────────────────────────────────────────────────

    public class FormGetBase64StringFileRequest : IDodoRequest
    {
        public string FileCode { get; set; }
        public DodoHeader Header { get; set; } = new();
    }

    public class FormGetBase64StringFileResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DodoFileProperty? File { get; set; }
    }

    public static class FileResultExtensions
    {
        public static async Task<string> ToBase64Async(this FileResult fileResult)
        {
            byte[] fileBytes;

            switch (fileResult)
            {
                case FileContentResult fileContentResult:
                    fileBytes = fileContentResult.FileContents;
                    break;

                case FileStreamResult fileStreamResult:
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileStreamResult.FileStream.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }
                    break;

                default:
                    throw new NotSupportedException(
                        $"Tipo de FileResult no soportado: {fileResult.GetType().Name}");
            }

            return Convert.ToBase64String(fileBytes);
        }
    }
}