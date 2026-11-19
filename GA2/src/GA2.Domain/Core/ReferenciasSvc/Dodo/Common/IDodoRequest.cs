using GA2.Domain.Core.Dodo.Common;

namespace GA2.Domain.Core.ReferenciasSvc.Dodo.Common
{
    public interface IDodoRequest
    {
        DodoHeader Header { get; set; }
    }
}
