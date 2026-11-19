using AutoMapper;
using GA2.Application.Main;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class IntegracionesGA2FenixBusinessLogic : IIntegracionesGA2FenixBusinessLogic
    {
        private readonly IIntegracionesGA2Fenix _integraciones;

        public IntegracionesGA2FenixBusinessLogic(IIntegracionesGA2Fenix integraciones)
        {
            _integraciones = integraciones;
        }

        public async Task<object> ValidarBiometria(Uri url, int request, Guid userId)
        {
            var response = new Response<object>();
            var result = await _integraciones.ValidarBiometria<object>(url, request, response.GetCorrelation(), userId);
            response.Data = JsonConvert.SerializeObject(result);
            return response;
        }
    }
}
