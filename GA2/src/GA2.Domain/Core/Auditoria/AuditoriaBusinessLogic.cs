using AutoMapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class AuditoriaBusinessLogic : IAuditoriaBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;
        private readonly IUsersRepository _usersRepository;

        public AuditoriaBusinessLogic(IMapper mapper, IAuditoriaRepository auditoriaRepository, ICatalogosBusinessLogic catalogosBusinessLogic, IUsersRepository usersRepository)
        {
            _mapper = mapper;
            _auditoriaRepository = auditoriaRepository;
            _catalogosBusinessLogic = catalogosBusinessLogic;
            _usersRepository = usersRepository; 
        }

        public async Task<Response<listaAuditorias>> ObtenerlogAuditoria(ConsultaAuditoriaDto parametros)
        {
            ConsultaAuditoria param = new ConsultaAuditoria();
            param.AUR_USR_ID = parametros.UserId;
            param.AUR_VER_DETALLE = parametros.VerDetalle;
            param.AUR_FECHA_FILTRO_INICIO = parametros.FechaInicio;
            param.AUR_FECHA_FILTRO_FIN = parametros.FechaFin;
            param.AUR_AGRUPADOR = parametros.Agrupador;
            param.AUR_NOM_TABLE = parametros.NombreTabla;
            param.PAGENUM = parametros.PageNum;
            param.PAGESIZE = parametros.PageSize;
            param.ID_TIPO_ACCION = string.IsNullOrEmpty(parametros.TipoAccion) ? 0 : Convert.ToInt32(parametros.TipoAccion);
            param.USUARIO = parametros.Usuario;
            param.DESCRIPCION = parametros.Descripcion;
            param.MODULO = parametros.Modulo;
            //int numero = await _auditoriaRepository.ObtenerNumeroRegistrologAuditoria(param);

            //double numeropaginas = Convert.ToDouble(numero)/Convert.ToDouble(parametros.PageSize);
            IEnumerable<AuditoriaDto> auditorias = _mapper.Map<IEnumerable<AuditoriaDto>>(await _auditoriaRepository.ObtenerlogAuditoria(param));

            listaAuditorias lista = new listaAuditorias();
            lista.NumeroPaginas = Convert.ToInt32(10);
            lista.TotalRegistros = auditorias.Count();
            lista.Auditorias = (List<AuditoriaDto>)auditorias;

            return new Response<listaAuditorias> { Data = lista };

        }

        public async Task<Response<IEnumerable<TablasAuditoriaDto>>> ObtenerTablasAuditoria()
        {

            IEnumerable<TablasAuditoriaDto> tables = _mapper.Map<IEnumerable<TablasAuditoriaDto>>(await _auditoriaRepository.ObtenerTablasAuditoria());
            return new Response<IEnumerable<TablasAuditoriaDto>> { Data = tables };


        }

        public async Task<Response<AuditoriaDto>> InsertarLogAuditoria(AuditoriaDto request)
        {
            var responseIp = await _catalogosBusinessLogic.ObtenerIpLocal();
            if (responseIp.IsSuccess)
            {
                if (!string.IsNullOrWhiteSpace(responseIp.Data))
                {
                    var result = responseIp.Data.Split("**");
                    request.Ip = result[0] == "::1" ? "Ip desde localhost" : result[0];
                    request.Hostname = result[1];
                }
            }
            else
            {
                request.Ip = responseIp.ReturnMessage;
            }
            AuditoriaDto response = _mapper.Map<AuditoriaDto>(await _auditoriaRepository.InsertarLogAuditoria(request));
            return new Response<AuditoriaDto> { Data = response };
        }

        public static bool IsInteger(double number)
        {
            return number == Math.Truncate(number);

        }


        public async Task LogAuditAsync<T>(int transtation, T oldEntity, T newEntity, string userId) where T : class
        {
            var userName = await _usersRepository.ObtenerUsuarioPorId(Guid.Parse(userId));
            var typeAction = await _auditoriaRepository.ObtenerTipoAcionAuditoria(transtation);
            var auditLog = new AuditLog();

            if (newEntity is null)
            {
                var changedFields = DataDeleteOrSave(oldEntity, typeAction);
                auditLog = new AuditLog
                {
                    TranstationId = transtation,
                    ChangedFields = JsonConvert.SerializeObject(changedFields),
                    UserName = userName.USR_EMAIL,
                    Timestamp = DateTime.UtcNow
                };

                await _auditoriaRepository.InsertarAuditoriaDetalle(auditLog);
            }
            else
            {
                var changedFields = CompareEntities(oldEntity, newEntity);

                if (changedFields.Any())
                {
                    auditLog = new AuditLog
                    {
                        TranstationId = transtation,
                        ChangedFields = JsonConvert.SerializeObject(changedFields),
                        UserName = userName.USR_EMAIL,
                        Timestamp = DateTime.UtcNow
                    };

                    await _auditoriaRepository.InsertarAuditoriaDetalle(auditLog);
                }
            }
        }

        private List<FieldChange> DataDeleteOrSave<T>(T entity, int typeAction)
        {
            var changedFields = new List<FieldChange>();

            foreach (var prop in typeof(T).GetProperties())
            {
                try
                {
                    var value = prop.GetValue(entity);
                    string valueStr;

                    if (value is IEnumerable && !(value is string))
                    {
                        valueStr = string.Join(",", ((IEnumerable)value).Cast<object>());
                    }
                    else
                    {
                        valueStr = value?.ToString() ?? "null";
                    }

                    changedFields.Add(new FieldChange
                    {
                        Field = prop.Name,
                        OldValue = typeAction == 7 ? valueStr : string.Empty,
                        NewValue = typeAction == 6 ? valueStr : string.Empty
                    });
                }
                catch (Exception e)
                {
                    // Log si es necesario
                }
            }

            return changedFields;
        }

        private List<FieldChange> CompareEntities<T>(T oldEntity, T newEntity)
        {
            var changedFields = new List<FieldChange>();

            foreach (var prop in typeof(T).GetProperties())
            {
                try
                {
                    var oldValue = prop.GetValue(oldEntity);
                    var newValue = prop.GetValue(newEntity);

                    string oldValueStr = ConvertToString(oldValue);
                    string newValueStr = ConvertToString(newValue);

                    if (oldValueStr != newValueStr)
                    {
                        changedFields.Add(new FieldChange
                        {
                            Field = prop.Name,
                            OldValue = oldValueStr,
                            NewValue = newValueStr
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Log si es necesario
                }
            }

            return changedFields;
        }

        private string ConvertToString(object value)
        {
            if (value is null) return "null";
            if (value is string str) return str;
            if (value is IEnumerable enumerable)
                return string.Join(",", enumerable.Cast<object>());
            return value.ToString();
        }

        public UserAgentInfo ParseUserAgent(string userAgent)
        {
            string browserPattern = @"(Chrome|Firefox|Safari|Opera|Edge)/(\d+\.\d+\.\d+\.\d+)";
            string osPattern = @"\((Windows NT \d+\.\d+);.*\)";

            var browserMatch = Regex.Match(userAgent, browserPattern);
            var osMatch = Regex.Match(userAgent, osPattern);

            string browser = "Unknown";
            string browserVersion = "Unknown";
            string os = "Unknown";
            string osVersion = "Unknown";

            if (browserMatch.Success)
            {
                browser = browserMatch.Groups[1].Value;
                browserVersion = browserMatch.Groups[2].Value;
            }

            if (osMatch.Success)
            {
                os = osMatch.Groups[1].Value;
                osVersion = osMatch.Groups[1].Value;
            }

            return new UserAgentInfo
            {
                Browser = browser,
                BrowserVersion = browserVersion,
                OperatingSystem = os,
                OsVersion = osVersion
            };
        }
        public string Ip()
        {
            string ip;
            var ipList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            ip = ipList[1].ToString();

            return ip;
        }
    }
}
