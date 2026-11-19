using GA2.Application.Dto;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface  IProcesosEspecialesBusinessLogic
    {
        Task<Response<bool>> GenerarLiquidacionIntereses(RemuneracionInteresDto remuneracionInteresDto);
    }
}
