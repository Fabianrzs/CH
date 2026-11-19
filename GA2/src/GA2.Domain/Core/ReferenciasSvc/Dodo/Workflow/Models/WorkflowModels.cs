using GA2.Domain.Core.Dodo.Common;
using System;
using System.Collections.Generic;

namespace GA2.Domain.Core.Dodo.Workflow.Models
{
    // ─── Workflow_Start ────────────────────────────────────────────────────────

    public class WorkflowStartRequest
    {
        public string OperationUser { get; set; }
        public string? NextStepUser { get; set; }
        public string BarCode { get; set; }
        public int ProcessId { get; set; }
        public List<DodoFileItem>? Files { get; set; }
        public DodoHeader Header { get; set; }
    }

    public class WorkflowStartResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int WorkflowId { get; set; }
        public List<string> FileCodes { get; set; } = new();
    }

    // ─── Workflow_RunTask ──────────────────────────────────────────────────────

    public class WorkflowRunTaskRequest
    {
        public string? BarCode { get; set; }
        public int? ProcessId { get; set; }
        public int? WorkflowId { get; set; }
        public int Step { get; set; }
        public string? Comment { get; set; }
        public string? NextStepUser { get; set; }
        public DodoHeader Header { get; set; }
    }

    public class WorkflowSimpleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    // ─── Workflow_ReturnTask ───────────────────────────────────────────────────

    public class WorkflowReturnTaskRequest
    {
        public DodoHeader Header { get; set; }
        public int WorkflowId { get; set; }
        public bool SealMail { get; set; }
        public int ReturnStep { get; set; }
        public string OperationUser { get; set; }
        public string Comment { get; set; }
    }

    // ─── Workflow_AbortProcess ─────────────────────────────────────────────────

    public class WorkflowAbortProcessRequest
    {
        public int WorkflowId { get; set; }
        public string OperationUser { get; set; }
        public string Comment { get; set; }
        public DodoHeader Header { get; set; }
    }

    // ─── Workflow_AttachFiles ──────────────────────────────────────────────────

    public class WorkflowAttachFilesRequest
    {
        public string OperationUser { get; set; }
        public int WorkflowId { get; set; }
        public bool? CopyToForm { get; set; }
        public List<DodoFileItem> Files { get; set; } = new();
        public DodoHeader Header { get; set; }
    }

    // ─── Workflow_GetCases ─────────────────────────────────────────────────────

    public class WorkflowGetCasesRequest
    {
        public string FormCode { get; set; }
        public int ProcessId { get; set; }
        public DodoFilterParameters FilterParameters { get; set; }
        public DodoHeader Header { get; set; }
    }

    public class WorkflowCaseItem
    {
        public int WorkflowId { get; set; }
        public string State { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int ProcessId { get; set; }
        public int RecordId { get; set; }
    }

    public class WorkflowGetCasesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<WorkflowCaseItem> WorkflowCases { get; set; } = new();
        public int Rows { get; set; }
    }

    // ─── Workflow_GetTasks ─────────────────────────────────────────────────────

    public class WorkflowGetTasksRequest
    {
        public int WorkflowId { get; set; }
        public DodoHeader Header { get; set; }
    }

    public class WorkflowTaskItem
    {
        public int Step { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string? Priority { get; set; }
        public string? OwnerUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class WorkflowGetTasksResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<WorkflowTaskItem> Tasks { get; set; } = new();
        public int Rows { get; set; }
    }

    // ─── Workflow_GetFileCodes ─────────────────────────────────────────────────

    public class WorkflowGetFileCodesRequest
    {
        public int WorkflowId { get; set; }
        public DodoHeader Header { get; set; }
    }

    public class WorkflowGetFileCodesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<DodoFileProperty> FileProperties { get; set; } = new();
        public int Rows { get; set; }
    }

    // ─── Workflow_StepsForReturn ───────────────────────────────────────────────

    public class WorkflowStepsForReturnRequest
    {
        public int WorkflowId { get; set; }
        public DodoHeader Header { get; set; }
    }

    public class WorkflowStepItem
    {
        public int Paso { get; set; }
        public string Descripcion { get; set; }
        public int IdProceso { get; set; }
    }

    public class WorkflowStepsForReturnResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<WorkflowStepItem> StepList { get; set; } = new();
    }

    // ─── Process_Parameters ───────────────────────────────────────────────────

    public class ProcessParametersRequest
    {
        public DodoHeader Header { get; set; }
        public int ProcessId { get; set; }
    }

    public class ProcessDefinition
    {
        public int IdProceso { get; set; }
        public string Proceso { get; set; }
        public string? Descripcion { get; set; }
        public int Estado { get; set; }
        public string? Version { get; set; }
        public string? Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Host { get; set; }
        public int IdFormulario { get; set; }
    }

    public class ProcessParametersResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ProcessDefinition? Parameters { get; set; }
    }
}
