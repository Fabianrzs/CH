using AutoMapper;
using Dapper;
using GA2.Application.Dto;
using GA2.Application.Dto.GrupoOpciones;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data.GrupoUsuarios;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class ProcesosEspecialesBusinessLogic : IProcesosEspecialesBusinessLogic
    {
        private readonly IMapper _mapper;
        public ProcesosEspecialesBusinessLogic(
        IMapper mapper)
        {
            _mapper = mapper;

        }

            public Task<Response<bool>> GenerarLiquidacionIntereses(RemuneracionInteresDto remuneracionInteresDto)
        {

           

            throw new System.NotImplementedException();


        }
    }
}
