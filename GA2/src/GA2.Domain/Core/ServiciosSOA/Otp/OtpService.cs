//using AutoMapper;
//using IntegracionesKriterion.Core.BaseEndpoints.Otp;
//using IntegracionesKriterion.Core.Commons;
//using IntegracionesKriterion.Core.ReferenciasSvc;
//using ServicesOtpSvc;
//using System.Threading;
//using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Otp
{
    /// <summary>
    /// Otp Service 
    /// </summary>
    public class OtpService // : IOtpService
    {
        ///// <summary>
        ///// Interface consumo otp service
        ///// </summary>
        //private readonly IConsumoServiceOtp _consumoServiceOtp;
        ///// <summary>
        ///// Mapper otp service
        ///// </summary>
        //private readonly IMapper _mapper;


        ///// <summary>
        ///// Constructor service 
        ///// </summary>
        ///// <param name="consumoServiceOtp">consume instance</param>
        ///// <param name="mapper">mapper instance</param>
        //public OtpService(IConsumoServiceOtp consumoServiceOtp, IMapper mapper)
        //{
        //    _consumoServiceOtp = consumoServiceOtp;
        //    _mapper = mapper;
        //}

        ///// <summary>
        ///// Generar Otp async
        ///// </summary>
        ///// <param name="generarOtpRequest">Parametros al generar otp</param>
        ///// <returns>Otp generado</returns>
        //public async Task<Response<GenerarOtpResponse>> GenerarOtpAsync(GenerarOtpRequest generarOtpRequest, CancellationToken cancellationToken)
        //{
        //    var response = new Response<GenerarOtpResponse>();
        //    var otp = await _consumoServiceOtp.GenerarOtpAsync(_mapper.Map<CrearOTPRequest>(generarOtpRequest), cancellationToken);
        //    response.Data = otp != null ? _mapper.Map<GenerarOtpResponse>(otp) : null;
        //    return response;
        //}

        ///// <summary>
        ///// Valida Otp 
        ///// </summary>
        ///// <param name="validaOtpRequest">codigo otp generado</param>
        ///// <returns>Otp valido</returns>
        //public async Task<Response<ValidaOtpResponse>> ValidarOtpAsync(ValidaOtpRequest validaOtpRequest, CancellationToken cancellationToken)
        //{
        //    var response = new Response<ValidaOtpResponse>();
        //    var otp = await _consumoServiceOtp.ValidarOtpAsync(_mapper.Map<ValidaOTPRequest>(validaOtpRequest), cancellationToken);
        //    response.Data = otp != null ? _mapper.Map<ValidaOtpResponse>(otp) : null;
        //    return response;
        //}
    }
}
