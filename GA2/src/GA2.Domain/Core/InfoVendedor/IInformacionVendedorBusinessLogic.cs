using GA2.Application.Dto.InfoVendedor;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.InfoVendedor
{
    public interface IInformacionVendedorBusinessLogic
    {
        Task<Response<InformacionVendedorDto>> CrearInformacionVendedor(InformacionVendedorDto informacionVendedorDto);
        Task<Response<IEnumerable<InformacionVendedorDto>>> EliminarInformacionVendedor(InformacionVendedorDto informacionVendedorDto);
        Task<Response<IEnumerable<InformacionVendedorDto>>> ObtenerInformacionVendedor(consultarVendedorDto request);
    }
}
