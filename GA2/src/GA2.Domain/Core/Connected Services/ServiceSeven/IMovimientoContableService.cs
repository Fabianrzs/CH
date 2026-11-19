using GA2.Transversals.Commons;
using ServiceSeven;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Connected_Services.ServiceSeven
{
    public interface IMovimientoContableService
    {
        /// <summary>
        /// Inserta un movimiento contable completo usando TSCnMcont
        /// </summary>
        Task<Response<string>> InsertarMovContableAsync(TSCnMcont dataMovContable, CancellationToken cancellationToken = default);

        /// <summary>
        /// Aplica un movimiento contable
        /// </summary>
        Task<Response<string>> AplicarCnMcontAsync(int empCodi, int mcoCont);

        /// <summary>
        /// Anula un movimiento contable
        /// </summary>
        Task<Response<string>> AnularCnMcontAsync(int empCodi, int mcoCont);

        /// <summary>
        /// Revierte un movimiento contable
        /// </summary>
        Task<Response<string>> RevertirCnMcontAsync(int empCodi, int mcoCont);

        /// <summary>
        /// Crea movimientos contables masivos con operaciones TOPER
        /// </summary>
        Task<Response<TRetToper[]>> SetContabToperAsync(TToper[] movimientos);
    }
}
