import { Button, Chip, createStyles, Grid, makeStyles, MenuItem, TablePagination, TablePaginationProps, Typography } from '@material-ui/core';
import { CloudDownload } from '@material-ui/icons';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHTextField from '../../components/ch-text-field/CHTextField';
import { LoadingComponent } from '../../components/loading/loading.component';
import ApiCatalog from '../../services/api-catalog.service';
import ApiReportes from '../../services/api-reportes.service';
import ApiSolicitud from '../../services/api-solicitud.service';
import UtilService from '../../utils/util.service';
import ModalBuscarUsuario from './ModalBuscarUsuario';
import AuditoriaApi from '../../services/api-auditorias.servise';
import { Auditoria } from '../../types/Auditoria.model.type';
import AuthService from '../../services/auth.service';
import CHSelect from '../../components/ch-select/CHSelect';
import ApiFondoSolidaridad from '../../services/api-fondoSolidaridad';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        checkBox: {
            backgroundColor: '#0075ff',
            accentColor: '#1c3f8b',
            width: '18px',
            height: '18px',
            cursor: 'pointer',
        },
        text: {
            fontSize: '.8rem',
            fontFamily: 'OpenSans-Regular',
            color: '#808080',
            fontWeight: 400,
            lineHeight: 1,
            padding: 0,
            marginLeft: '10px',
            marginTop: '15px'
        },
        columnButton: {
            padding: '10px',
            textAlign: 'right',
        },
        formControl: {
            minWidth: 300,
            maxWidth: 400,
        },
        chips: {
            display: 'flex',
            flexWrap: 'wrap',
        },
        chip: {
            margin: 2,
        },
        textTable: {
            textAlign: 'center'
        },
    }),
);

function TableReporteSolicitudes({
    data,
    page,
    setPage,
    onClickExcel
}: {
    data: any;
    page: number;
    setPage: (value: number) => void;
    onClickExcel: (data: any) => void;
}) {

    const classes = useStyles();
    const rowsPerPageOptions = [10, 20, 50, 100];
    const [rowsPerPage, setRowsPerPage] = React.useState(rowsPerPageOptions[0]);

    if(data && data.length === 0){
        return <Typography style={{padding:'20px'}}> <br /> No se encontraron resultados <br /></Typography>;
    }

    const handleChangePage = (
		event: React.MouseEvent<HTMLButtonElement> | null,
		newPage: number,
	) => {
		setPage(newPage);
	};

	const handleChangeRowsPerPage = (
		event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
	) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};

    const labelDisplayedRows: TablePaginationProps['labelDisplayedRows'] = ({ from, to, count }) => `${from}-${to} de ${count}`;

    return (
        <div>
            <table className="client-table">
                <thead>
                    <tr>
                        <th colSpan={14} style={{ backgroundColor: '#f1f2f6', textAlign: 'right' }}>
                            <Button variant="contained" color="primary"
                                onClick={() => onClickExcel(data)}
                                style={{ width: '20%', margin: '10px', marginRight: '20px' }} startIcon={<CloudDownload /> }
                            >
                                Generar Reporte
                            </Button>
                        </th>
                    </tr>
                    <tr>
                        <th className={classes.textTable}>Fuerza</th>
                        <th className={classes.textTable}>Unidad Ejecutora</th>
                        <th className={classes.textTable}>Categoría</th>
                        <th className={classes.textTable}>Grado</th>
                        <th className={classes.textTable}>Radicado</th>
                        <th className={classes.textTable}>Tipo Solicitud</th>
                        <th className={classes.textTable}>Fecha Inicial</th>
                        <th className={classes.textTable}>Fecha Final</th>
                        <th className={classes.textTable}>Días serv</th>
                        <th className={classes.textTable}>Días vcto</th>
                        <th className={classes.textTable}>Afiliado</th>
                        <th className={classes.textTable}>Identificación Afiliado</th>
                        <th className={classes.textTable}>Estado</th>
                        <th className={classes.textTable}>Usuario</th>
                    </tr>
                </thead>
                <tbody>
                {data?.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)?.map((row: any, index: any) => (
                    <tr key={`table-${index}`}>
                        <td className="tab-space-01">{row?.fuerza}</td>
                        <td className="tab-space-01">{row?.unidadEjecutora}</td>
                        <td className="tab-space-01">{row?.categoria}</td>
                        <td className="tab-space-01">{row?.grado}</td>
                        <td className="tab-space-01">{row?.radicado}</td>
                        <td className="tab-space-01">{row?.fuente.toUpperCase()}</td>
                        <td className="tab-space-01">{UtilService.getDate(row?.fechaInicial)}</td>
                        <td className="tab-space-01">{" "}</td>
                        <td className="tab-space-01">{" "}</td>
                        <td className="tab-space-01">{" "}</td>
                        <td className="tab-space-01">{row?.afiliado}</td>
                        <td className="tab-space-01">{row?.identificacionAfiliado}</td>
                        <td className="tab-space-01">{row?.estado}</td>
                        <td className="tab-space-01">{row?.usuario}</td>
                    </tr>
                
                ))}
              </tbody>
            </table>
            {
				data.length > 0 && (
					<TablePagination
						component="div"
						count={data.length}
						page={page}
						onPageChange={handleChangePage}
						rowsPerPage={rowsPerPage}
						labelRowsPerPage="Filas por página"
						labelDisplayedRows={labelDisplayedRows}
						onRowsPerPageChange={handleChangeRowsPerPage}
					/>
				)
			}
        </div>

    );
}

const ReportesSolicitudes = () => {

    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiSolicitudes = ApiSolicitud();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const [openModalBuscarUsuario, setOpenModalBuscarUsuario] = useState(false);
    const [tipoSolicitud, setTipoSolicitud] = useState<any []>([]);
    const [infoReporte, setInfoReporte] = useState<any []>([]);
    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [page, setPage] = React.useState(0);
    const [loading, setLoading] = useState(false);

    type FormValues = {
        radicado: string;
        tipoSolicitud: string;
        fechaPresentacionInicio: Date | null;
        fechaPresentacionFinal: Date | null;
        diasVencimiento: Date | null;
        estadoMultiple: string[];
        usuario: string;
        idUsuario: string
    };
    
    const formularioReporteSolicitudes = useForm<FormValues>({
        defaultValues: {
            radicado: '',
            tipoSolicitud: '',
            fechaPresentacionInicio: null,
            fechaPresentacionFinal: null,
            diasVencimiento: null,
            estadoMultiple: [],
            usuario: '',
            idUsuario: ''
        }
    });

    const estadosOptions = [
        'EN TRÁMITE','PENDIENTE', 'EN EVALUACIÓN', 'BLOQUEADO', 'PROCESADA', 'INCONSISTENTE',
        'CANCELADA', 'ANULADA', 'RECHAZADA', 'APROBADO'
    ];
    const seleccionarTodosTexto = 'TODOS';
    useEffect(() => {
        consultarListaTipoSolicitud();
    },[])

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
      };
    
      const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
      };
    
      const enviarIp = async (ip: any) => {
        const convertIp = ip.split('.')[0]
    
        const hexadecimal = convertIp
        await apiCatalog
          .getObtenerIpLocal(hexadecimal)
          .then(async (response: any) => {
    
            if (response) {
              if (response.isSuccess) {
                setPublicIpAddress(response.data);
              }
            }
          })
          .catch(e => {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
          });
    
      }

      useEffect(() => {
        const fetchLocalIPAddress = () => {
          const peerConnection = new RTCPeerConnection({ iceServers: [] });
          peerConnection.createDataChannel('');
          peerConnection.createOffer()
            .then((offer) => peerConnection.setLocalDescription(offer))
            .catch((error) => console.error('Error creating offer:', error));
    
          peerConnection.onicecandidate = (event) => {
            if (event.candidate) {
              const ip: any = event.candidate.address;
              if (isValidIPv4(ip) || isValidIPv6(ip)) {
                enviarIp(ip);
              } else {
                enviarIp(ip);
              }
            }
          };
    
          return () => {
            peerConnection.close();
          };
        };
        fetchLocalIPAddress();
      }, []);

    const openModal = () => {
        setOpenModalBuscarUsuario(true);
        formularioReporteSolicitudes.setValue('usuario', '');
    }

    const closeModal = (event: any, reason: string) => {
        if(reason === 'backdropClick') return;
        setOpenModalBuscarUsuario(false);
    }

    const buscarUsuarioIngreso = (datos: any) => {
		setLoading(true)
		if (datos.nombreS_USUARIO !== undefined && datos.apellidoS_USUARIO !== undefined) {
			formularioReporteSolicitudes.setValue('usuario', `${datos?.nombreS_USUARIO} ${datos?.apellidoS_USUARIO}`)
            formularioReporteSolicitudes.setValue('idUsuario', `${datos?.iD_USR}`)
			setLoading(false)
			setOpenModalBuscarUsuario(false);
		} else {
			formularioReporteSolicitudes.setValue('usuario', "")
            formularioReporteSolicitudes.setValue('idUsuario', "")
			setLoading(false)
			setOpenModalBuscarUsuario(false);
		}
	}

    const consultarListaTipoSolicitud = async () => {
        const TIPO_SOLICITUD_REPORTES = 100;
        try{
          const datosIdSolicitudes =  await apiCatalog.getAllTypeRequest(2);
          const datosFondoSolidaridad =  await apiFondoSolidaridad.getConsultarTipoSolicitudPostulante();

          await apiCatalog
          .getObtenerValoresCatalogosPorIdCatalogo(TIPO_SOLICITUD_REPORTES)
          .then(async (response: any) => {
              if (response) {
                  if (response.isSuccess) {

                      const array = response?.data.filter((item: any) => 
                          item?.descripcion !== 'ABONAR INTERESES AVAC' &&
                          item?.descripcion !== 'ABONAR INTERESES AVAV' &&
                          item?.descripcion !== 'ABONAR INTERESES AVC' &&
                          item?.descripcion !== 'ABONAR INTERESES CESANTIAS' &&
                          item?.descripcion !== 'ABONAR INTERESES SOLUCION DE VIVIENDA' &&
                          item?.descripcion !== 'ABONAR RENDIMIENTOS' &&
                          item?.descripcion !== 'ABONO DE INTERESES INDIVIDUAL' &&
                          item?.descripcion !== 'DEVOLUCION AHORROS AVAC' &&
                          item?.descripcion !== 'DEVOLUCION AHORROS AVAV' &&
                          item?.descripcion !== 'DEVOLUCION SALDO CONVENIO' &&
                          item?.descripcion !== 'DEVOLUCION SALDO CUENTA' &&
                          item?.descripcion !== 'DESEMBARGOS' &&
                          item?.descripcion !== 'EMBARGOS' &&
                          item?.descripcion !== 'EJECUTORIA ACTO ADMINISTRATIVO' &&
                          item?.descripcion !== 'ESTUDIO MODELO SECUESTRADO' &&
                          item?.descripcion !== 'PAGO EMBARGOS' &&
                          item?.descripcion !== 'PAGOS EXTRAORDINARIOS' &&
                          item?.descripcion !== 'PERDIDA DE BENEFICIO FONDO DE SOLIDARIDAD' &&
                          item?.descripcion !== 'PRORROGA FONDO DE SOLIDARIDAD' &&
                          item?.descripcion !== 'PRORROGA VIVIENDA 8' &&
                          item?.descripcion !== 'SOLICTUD DE SUBSIDIO VIVIENDA 14' &&
                          item?.descripcion !== 'RECONOCIMIENTO SUBSIDIO' &&
                          item?.descripcion !== 'RESOLUCION DE ASIGNACION DE SUBSIDIO' &&
                          item?.descripcion !== 'SOLICITUD DE LEASING')

                      const arrayFinal = array?.map((option: any) => {
                          if(option){
                             const tipoSolicitud = datosIdSolicitudes?.data.find((item:any) => item.descripcionTipoSolicitud === option.descripcion);
                             let nuevoItem;
                             if(option?.descripcion === "FONDO DE SOLIDARIDAD"){
                                const solicitudFondoSolidaridad = datosFondoSolidaridad?.data.find((item:any) => item.nombre === 'SOLICITUD POSTULACIÓN FONDO SOLIDARIDAD');
                                nuevoItem = {
                                    ...option,
                                    idValor: solicitudFondoSolidaridad?.id,
                                    idTipoSolicitudes: tipoSolicitud?.idTipoSolicitud || '',
                                }
                             }else if(option?.descripcion === "ADJUDICACION FONDO DE SOLIDARIDAD"){
                                const solicitudFondoSolidaridad = datosFondoSolidaridad?.data.find((item:any) => item.nombre === 'ADJUDICACIÓN DE VIVIENDA');
                                nuevoItem = {
                                    ...option,
                                    idValor: solicitudFondoSolidaridad?.id,
                                    idTipoSolicitudes: tipoSolicitud?.idTipoSolicitud || '',
                                }
                             }else if(option?.descripcion === "PERDIDA DE BENEFICIO FONDO DE SOLIDARIDAD"){
                                const solicitudFondoSolidaridad = datosFondoSolidaridad?.data.find((item:any) => item.nombre === 'PÉRDIDA DE BENEFICIO');
                                nuevoItem = {
                                    ...option,
                                    idValor: solicitudFondoSolidaridad?.id,
                                    idTipoSolicitudes: tipoSolicitud?.idTipoSolicitud || '',
                                }
                             }else{
                                nuevoItem = {
                                    ...option,
                                    idTipoSolicitudes: tipoSolicitud?.idTipoSolicitud || '',
                                }
                             }
                            return nuevoItem;
                          }
                          return null;
                      })
                      const itemEmbargos = {
                        codigo:"SDGDE",
                        descripcion: "SOLICITUDES DE GESTIÓN DE EMBARGOS",
                        esActivo: true,
                        id: 100,
                        idValor: 598
                      }
                      arrayFinal.push(itemEmbargos);
                      const ordenado: any[] = arrayFinal.sort((a:any, b:any) =>
                        a.descripcion.localeCompare(b.descripcion, 'es', { sensitivity: 'base' })
                        );
                      setTipoSolicitud(ordenado);
                  }
              }
          })
          .catch(() => {
              toast('Error al consultar lista tipo solicitud', { type: toast.TYPE.ERROR });
          });
        }catch(e){
            toast('Error al consultar id solicitudes e id postulaciones', { type: toast.TYPE.ERROR });
        }
    };

    const consultarInformacionReporte = async (data: any) => {

        setLoading(true);
        if(!formularioReporteSolicitudes.getValues('usuario')){
            data.idUsuario = ''
        }

        const tipoSolicitudes = data.tipoSolicitud? data.tipoSolicitud.split('_')[0] : null;
        const idTipoSolicitudes = data.tipoSolicitud? data.tipoSolicitud.split('_')[1] : null;

        const request = {
            radicado: data.radicado === ""? null : data.radicado,
            tipoSolicitudes: tipoSolicitudes && (idTipoSolicitudes === "undefined" || idTipoSolicitudes === "")? Number(tipoSolicitudes): null,
            idTipoSolicitudes: idTipoSolicitudes === "undefined" || idTipoSolicitudes === ""? null : idTipoSolicitudes,
            fechaInicio: data.fechaPresentacionInicio === ""? null : data.fechaPresentacionInicio,
            fechaFin: data.fechaPresentacionFinal === ""? null : data.fechaPresentacionFinal,
            diasVencimiento: data.diasVencimiento === ""? null : data.diasVencimiento,
            usuario: data.usuario || '',
            estados: data.estadoMultiple.lenght === 0? null : data.estadoMultiple,
            userId: data.idUsuario === ""? "00000000-0000-0000-0000-000000000000" : data.idUsuario,
        }

        await apiSolicitudes
            .PostConsultaReporteSolcitudesGeneral(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setPage(0);
                        setInfoReporte(response?.data?.dataReporteUnificado);
                        setLoading(false);
                    }else{
                        setLoading(false);
                        toast("No se encontraron resultados");
                    }
                }
            })
            .catch(() => {
                setLoading(false);
                toast('Error al consultar Información Consulta', { type: toast.TYPE.ERROR });
            });
    };

    const onClickExcel = async (data: any) => {

        const columnDataList = data?.map((row: any) => ({
    
          data: [
            row.fuerza,
            row?.unidadEjecutora,
            row?.categoria,
            row?.grado,
            row?.radicado,
            String(row?.fuente).toUpperCase(),
            UtilService.getDate(row?.fechaInicial),
            " ",
            " ",
            " ",
            row?.afiliado,
            row?.identificacionAfiliado,
            row?.estado,
            row?.usuario
          ]
        }));
    
        if (columnDataList) {
          const request = {
            columnDataList: columnDataList.map((item:any) => ({
              data: item.data
            })),
            columnName: ["Fuerza", "Unidad Ejecutora", "Categoría", "Grado", "Radicado", "Tipo Solicitud", "Fecha Inicial", "Fecha Final", "Días serv", "Días vcto", "Afiliado", "Identificación Afiliado", "Estado", "Usuario" ]
          };
    
          const apiReportes = ApiReportes();
    
          try{
            const response = await apiReportes.GenerarExcels(request);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = 'ReporteSolicitudes.xlsx';
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(url);
      
            const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = window.navigator as any;
            const usrAg = nav.userAgentData.brands.find(
              (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand;
      
            const requestAuditoria = {
              navegador: usrAg,
              sistemaOperativo: nav.userAgentData.platform,
              nombreUsuario: user.USR_USERNAME,
              usurioId: user.USR_ID,
              ip: publicIpAddress,
              modulo: 'Reporte Solicitudes',
              hostname: document.location.host,
              tipoAccion: 'DESCARGAS',
              idTipoAccion: 5,
              descripcion: 'Descarga de archivo Excel del Reporte de Solcitudes'
      
            } as Auditoria;
      
            auditoriaApi.InsertAuditoria(requestAuditoria);
          }catch(e:any){
                toast(`Error al generar reporte: ${e.message}`, { type: toast.TYPE.ERROR });
          }
         
        }
      }
  
    return (
        <>
            <LoadingComponent loading={loading} />
            <CHPrivateContainer>
                <CHPageTitle
                    topText="Gestión de Reportes Solicitudes"
                    subText=""
                    title1="Reportes"
                    title2="Solicitudes"
                />
                <CHPaper title="Generación Reporte">
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="radicado"
                                control={formularioReporteSolicitudes.control}
                                rules={{
                                    minLength: { value: 4, message: 'Mínimo 4 caracteres' },
                                    maxLength: { value: 30, message: 'Máximo 30 caracteres' },
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error?.message || ''}
                                        label="Radicado"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={(e) => {
                                            if (e.target.value.length < 30) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[a-zA-Z#./?=!@$%^&*^)(+=_}{":;><|~`]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
							name="tipoSolicitud"
							control={formularioReporteSolicitudes.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									defaultValue=""
									label="Tipo Solicitud"
									value={value}
									onChange={onChange}
								>
									<MenuItem key={0} value={0} >
										SELECCIONE...
									</MenuItem>
									{tipoSolicitud?.map(
										(option: any) => (
											<MenuItem
												key={option?.idValor}
												value={`${option?.idValor}_${option?.idTipoSolicitudes}`}
											>
												{option?.descripcion}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={2}>
                            <Controller
                                name="fechaPresentacionInicio"
                                control={formularioReporteSolicitudes.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHDatePicker
                                        label="Fecha Presentación Inicio"
                                        onChange={onChange}
                                        value={value}
                                        inputProps={{ readOnly: false }}
                                        InputLabelProps={{ shrink: true }}
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={2}>
                            <Controller
                                name="fechaPresentacionFinal"
                                control={formularioReporteSolicitudes.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHDatePicker
                                        label="Fecha Presentación Final"
                                        onChange={onChange}
                                        value={value}
                                        inputProps={{ readOnly: false }}
                                        InputLabelProps={{ shrink: true }}
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={2}>
                            <Controller
                                name="diasVencimiento"
                                control={formularioReporteSolicitudes.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHDatePicker
                                        label="Días Para Vencimiento"
                                        onChange={onChange}
                                        value={value}
                                        inputProps={{ readOnly: false }}
                                        InputLabelProps={{ shrink: true }}
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>

                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={6}>
                            <Controller
                                name="estadoMultiple"
                                control={formularioReporteSolicitudes.control}
                                render={({ field: { onChange, value } }) => {
                                    const handleSelectChange = (event: any) => {
                                        const selected = event.target.value;
                                        if (selected.includes(seleccionarTodosTexto)) {
                                            const allSelected =
                                                value.length === estadosOptions.length ? [] : [...estadosOptions];
                                            onChange(allSelected);
                                        } else {
                                            onChange(selected);
                                        }
                                    };
                                    const isAllSelected = value.length === estadosOptions.length;

                                    return (
                                        <CHTextField
                                            label="Estados"
                                            select
                                            SelectProps={{
                                                multiple: true,
                                                value,
                                                onChange: handleSelectChange,
                                                renderValue: (selected: any) => (
                                                    <div style={{ display: 'flex', flexWrap: 'wrap' }}>
                                                        {selected.map((val: any) => (
                                                            <Chip
                                                                key={val}
                                                                label={val}
                                                                size="small"
                                                                style={{ margin: 2 }}
                                                            />
                                                        ))}
                                                    </div>
                                                ),
                                            }}
                                            InputLabelProps={{ shrink: true }}
                                            fullWidth
                                            variant="standard"
                                        >
                                            <MenuItem value={seleccionarTodosTexto}>
                                                <em>{isAllSelected ? 'Deseleccionar todos' : 'Seleccionar todos'}</em>
                                            </MenuItem>
                                            {estadosOptions.map((estado) => {
                                                const isSelected = value.includes(estado);
                                                return (
                                                    <MenuItem
                                                        key={estado}
                                                        value={estado}
                                                        style={{
                                                            backgroundColor: isSelected ? '#e0dede' : undefined,
                                                            color: isSelected ? '#424242' : undefined,
                                                            fontWeight: isSelected ? 'bold' : undefined,
                                                        }}
                                                    >
                                                        {estado}
                                                    </MenuItem>
                                                );
                                            })}
                                        </CHTextField>
                                    );
                                }}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={6}>
                            <Controller
                                name="usuario"
                                control={formularioReporteSolicitudes.control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error?.message || ''}
                                        label="Usuario Ingreso"
                                        value={value}
                                        onChange={(e) => onChange(e.target.value)}
                                        InputLabelProps={{ shrink: true }}
                                        onClick={openModal}
                                        isReadOnly
                                    />
                                )}
                               
                            />
                        </Grid>
                    </Grid>

                    <Grid container>
                        <Grid className={classes.columnButton} item xs={12} md={12}>
                            <Button variant="contained" color="primary" style={{ width: '20%' }} onClick={formularioReporteSolicitudes.handleSubmit(consultarInformacionReporte)}>
                                Consultar
                            </Button>
                        </Grid>
                    </Grid>
                </CHPaper>
                <TableReporteSolicitudes 
                    data={infoReporte} 
                    onClickExcel={onClickExcel}
                    page={page}
                    setPage={setPage}
                />
            </CHPrivateContainer>
            <ModalBuscarUsuario 
                    open={openModalBuscarUsuario}
                    onClose={closeModal} 
                    buscarUsuarioIngreso={buscarUsuarioIngreso}
            />
        </>
    );
};

export default ReportesSolicitudes;