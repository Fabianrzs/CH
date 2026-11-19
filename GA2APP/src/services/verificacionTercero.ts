
type SN = 'S' | 'N';


export const buildPayloadVerificacion = (cedula: string) => ({
  pe_Origen: 'CORE GA2',
  pe_Dato: cedula,         
  pe_TipoVerificacion: '1', 
  pe_Porcentaje: '100',    
});


const normalizeEncontrado = (res: any): SN => {
  const val: SN | undefined =
    (res?.data?.encontrado as SN) ??
    (res?.encontrado as SN);

  return val === 'S' ? 'S' : 'N';
};


const quienReportado = (afiliado: SN, vendedor: SN): string => {
  if (afiliado === 'S' && vendedor === 'S') return 'el afiliado y el vendedor';
  if (afiliado === 'S') return 'el afiliado';
  if (vendedor === 'S') return 'el vendedor';
  return '';
};


export const verificarDocumentoReportado = async (
  cedula: string,
  PostVerificacionTerc: (req: any) => Promise<any>
): Promise<SN> => {
  const req = buildPayloadVerificacion(cedula);
  const res = await PostVerificacionTerc(req);
  return normalizeEncontrado(res);
};


export const validarAfiliadoYVendedor = async (
  cedulaAfiliado: string,
  cedulaVendedor: string,
  PostVerificacionTerc: (req: any) => Promise<any>,
  NotificationService: {
    genericAlertError: (title: string, msg: string, type: 'warning' | 'error' | 'info') => void;
  }
): Promise<boolean> => {
  try {
    const [afiliado, vendedor] = await Promise.all([
      verificarDocumentoReportado(cedulaAfiliado, PostVerificacionTerc),
      verificarDocumentoReportado(cedulaVendedor, PostVerificacionTerc),
    ]);

    const quien = quienReportado(afiliado, vendedor);
    if (quien) {
      NotificationService.genericAlertError(
        'No permitido',
        `No se puede continuar: ${quien} está reportado.`,
        'warning'
      );
      return false;
    }

    return true; 
  } catch {
    NotificationService.genericAlertError(
      'Error de verificación',
      'No se pudo validar los documentos. Intenta nuevamente.',
      'error'
    );
    return false;
  }
};
