using AutoMapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Integraciones;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using static GA2.Domain.Core.IAuditoriaServicesBusinessLogic;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace GA2.Domain.Core
{
    public class AuditoriaServicesBusinessLogic : IAuditoriaServicesBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IAuditoriaRepository _auditoriaRepository;

        public AuditoriaServicesBusinessLogic(IMapper mapper, IAuditoriaRepository auditoriaRepository)
        {
            _mapper = mapper;
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<TResult> EjecutarServicio<T, TResult>(MetodoServicio<T, Task<TResult>> funcion, T parametro, ReferenciaSvc referenciaSvc)
        {
            // Aquí puedes ejecutar la función con el parámetro.
            var id = Guid.NewGuid();
            var requestSerializado = Serializar(parametro);


            await _auditoriaRepository.InsertReportIntegraciones(new ReportIntegracion
            {
                ID_INTEGRACIONES = id,
                TIPOSERVICIO = referenciaSvc.ToString(),
                REQUEST = Serializar(parametro),
            });
            //await GuardarRequestEnArchivoAsync(id, referenciaSvc.ToString(), requestSerializado);


            var response = await funcion(parametro);

            await _auditoriaRepository.UpdateReportIntegraciones(new ReportIntegracion
            {
                ID_INTEGRACIONES = id,
                RESPONSE = Serializar(response),
            });
            return response;
        }



        private async Task GuardarRequestEnArchivoAsync(Guid id, string tipoServicio, string requestSerializado)
        {
            var carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogsRequests");

            // Crea la carpeta si no existe
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            // Nombre del archivo: ejemplo => 20250404_ReferenciaSvc_GUID.txt
            var nombreArchivo = $"{DateTime.Now:yyyyMMdd_HHmmss}_{tipoServicio}_{id}.txt";
            var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            await File.WriteAllTextAsync(rutaCompleta, requestSerializado, Encoding.UTF8);
        }


        private string Serializar(object obj)
        {
            XmlSerializer x = new XmlSerializer(obj.GetType());
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Encoding = Encoding.Unicode }))
                {
                    x.Serialize(writer, obj);
                    string xmlString = stringWriter.ToString();
                    return xmlString;
                }
            }
        }
    }
}
