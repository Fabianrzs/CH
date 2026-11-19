import { Box, Button, CircularProgress, createStyles, Grid, IconButton, makeStyles, Tooltip} from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import MUIDataTable from 'mui-datatables';
import { CloudDownload, CloudDownloadSharp, Search } from '@material-ui/icons';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { ProductState } from '../../../reducers/producto.reducer';
import { CreditState } from '../../../reducers/credit.reducer';
import {startGenerarBalanceCartera,startDescargarBalanceCarteraAux,startLimpiarStateBalanceCartera} from '../../../actions/credit.actions';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import { LoginState } from '../../../reducers/login.reducer';
import { AppState } from '../../../store/rootReducer';
import ContainerTitle from '../../../components/containe-title/container-title';
import { LoadingComponent } from '../../../components/loading/loading.component';
import ApiReportes from '../../../services/api-reportes.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from '../../../services/auth.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import CatalogApi from '../../../services/api-catalog.service';
import { ColumnFormatType } from '../../../enums/ColumnFormatType';
import UtilService from '../../../utils/util.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        totales: {
            textAlign: 'right'
        },
        infoPanel: {
            display: 'grid',
            gridTemplateColumns: '1fr auto',
            gap: '0.5rem 2rem',
            marginTop: '1rem'
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
            marginBottom: '2rem',

        },
        buttonContainer: {
            display: 'flex',
            justifyContent: 'center', // Centra los botones horizontalmente
            marginTop: '1rem'
        }

    }),
);

const GenerarBalanceCartera = (props: Props) => {

    const classes = useStyles();
    const apiReportes = ApiReportes();

    const {
        catalogState,
        loginState,
        productState,
        creditState,
        startGenerarBalanceCartera,
        startDescargarBalanceCarteraAux,
        startLimpiarStateBalanceCartera
    } = props;

    const { control, clearErrors, register, getValues, setValue, handleSubmit, reset, ...rest } = useForm({
        defaultValues: {
            fechaInicio: new Date(),
        }
    });

    const columns = [
        {
            name: 'cuentaContable',
            label: 'Cuenta Contable',
            options: {

                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'cuentasNif',
            label: 'Cuenta NIF',
            options: {

                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'descripcionCuenta',
            label: 'Descripcion de Cuenta',
            options: {

                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'saldoInicial',
            label: `Saldo Inicial ${moment(getValues('fechaInicio')).format("MM-YYYY") || ''}`, 
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'debitos',
            label: 'Debito',
            options: {

                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'creditos',
            label: 'Credito',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'saldoFinal',
            label: 'Saldo Final',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },
    ];

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi  = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip:any) => {
        const  convertIp = ip.split('.')[0]
       const  hexadecimal = convertIp
        await catalogApi 
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

    const [downloading, setDownloading] = useState(false);

    useEffect(() => {
        startLimpiarStateBalanceCartera()
    }, [])


    const generarBalance = (data: any) => {
        const request = {
            fechaCorte: moment(data.fechaInicio).format("DD-MM-YYYY"),
        };

        startGenerarBalanceCartera(request);

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand || 'Desconocido';

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData?.platform || 'Desconocido',
            nombreUsuario: user?.USR_USERNAME ?? 'Usuario desconocido',
            usurioId: user?.USR_ID ?? 'ID desconocido',
            ip: publicIpAddress,
            modulo: 'Generación de Balance',
            hostname: document.location.host,
            tipoAccion: 'CONSULTA',
            idTipoAccion: 3,
            descripcion: `Se generó un balance de cartera con la siguiente información:
            \n Fecha de corte: ${request.fechaCorte}`,
        } as Auditoria;


        auditoriaApi.InsertAuditoria(requestAuditoria);
    };


    const descargarBalanceAux = (data: any) => {

        const request = {
            fechaCorte: moment(data.fechaInicio).format("DD-MM-YYYY")
        }
        startDescargarBalanceCarteraAux(request)

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand || 'Desconocido';

        try {
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform || 'Desconocido',
                nombreUsuario: user?.USR_USERNAME ?? 'Usuario desconocido',
                usurioId: user?.USR_ID ?? 'ID desconocido',
                ip: publicIpAddress,
                modulo: 'Generar Balance Cartera',
                hostname: document.location.host,
                tipoAccion: 'DESCARGAS',
                idTipoAccion: 5,
                descripcion: `Se generó archivo auxiliar contable cartera:
                \n Fecha de corte: ${request.fechaCorte}`,
            } as Auditoria;
    
            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            const descripcionError = `Error al descargar Excel.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta de cartera',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast('Error al consultar el cierre mensual contable', { type: 'error' });
        }
    };

    const handleClick = async () => {
        try {
            setDownloading(true);
            await handleExportToExcel();
        } finally {
            setDownloading(false);
        }
    }

    const handleExportToExcel = async () => {

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {

            const columnDataList = creditState.balanceCartera?.map((row: any) => ({
                data: columns.map(x => row[x.name]?.toString() ?? '')
            }));
            
            if (columnDataList) {
                const request = {
                    columnData: columnDataList.map((item: any) => item.data),
                    columnsInfo: columns.map((x:any) => ({
                        name: x.label,
                        formatType: x.formatType
                    }))
                };

                const response = await apiReportes.GenerarExcelInvariantCulture(request);
                const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = url;
                link.download = `Balance Cartera  ${moment(getValues('fechaInicio')).format("DD-MM-YYYY") || ''}.xlsx`;
                document.body.appendChild(link);
                link.click();
                window.URL.revokeObjectURL(url);
                document.body.removeChild(link);

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
                    modulo: 'Generar balance cartera',
                    hostname: document.location.host,
                    tipoAccion: 'EXPORTACIÓN',
                    idTipoAccion: 4,
                    descripcion: `Se exportó un archivo Excel de comprobantes.`,
                } as Auditoria;

                auditoriaApi.InsertAuditoria(requestAuditoria);
            }
        } catch (error: any) {
            const descripcionError = `Error al exportar archivo.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Generar balance cartera',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);

            toast("Ocurrió un error al intentar exportar a Excel. Por favor, inténtalo de nuevo.");
        }
    };

    return (
        <CHPrivateContainer>
            <PageIndicators
                topTextNivel1="Balance Prueba"
                topTextNivel2="Cartera"
            />

            <LoadingComponent key="loader" loading={creditState.loading} />

            <IconTitle
                title="Generar"
                subTitle="Balance Cartera"
                icon="fch-Aprobar-Cargue-Nomina-0-1"
            />

            <CHPaper title="Datos Balance">
                <ContainerTitle title="GENERAR BALANCE" />

                <Grid container spacing={3}>
                    <Grid item xs={12} md={4} className={classes.column}>
                    <Controller
                        name="fechaInicio"
                        rules={{ required: 'Este campo es requerido' }}
                        control={control}
                        render={({ field: { onChange, value } }) => (
                            <CHDatePicker
                                label="Fecha Inicial"
                                inputProps={{ readOnly: true }}
                                views={['year', 'month']}
                                onChange={(date) => {
                                    if (date) {
                                        const lastDayOfMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                                        onChange(lastDayOfMonth); // Establece el último día del mes en el estado
                                    }
                                }}
                                value={value || null}
                                format="MM/yyyy" // Formato de visualización
                            />
                        )}
                    />

                    </Grid>

                    <Grid
                        container
                        justifyContent="center"
                        className={classes.buttonContainer}
                    >
                        <Button
                            className={classes.buttonAdd}
                            type="submit"
                            variant="outlined"
                            onClick={handleSubmit(generarBalance)}
                            startIcon={<Search />}
                            style={{
                                backgroundColor: '#FFD33A',
                                color: 'black',
                            }}
                        >
                            Generar Balance
                        </Button>

                        <Button
                            className={classes.buttonAdd}
                            type="submit"
                            variant="outlined"
                            onClick={handleSubmit(descargarBalanceAux)}
                            startIcon={<CloudDownloadSharp />}
                            style={{
                                backgroundColor: '#FFD33A',
                                color: 'black',
                            }}
                        >
                            Descargar Auxiliar
                        </Button>
                    </Grid>
                </Grid>
                <ContainerTitle title="BALANCE PRUEBA" />
                <Grid container>
                    <Box style={{ width: '100%' }}>
                        <MUIDataTable
                            title=" "
                            data={creditState.balanceCartera}
                            columns={columns}
                            options={{
                                selectableRows: 'none',
                                filter: false,
                                print: false,
                                search: true,
                                elevation: 2,
                                filterType: 'dropdown',
                                responsive: 'standard',
                                fixedHeader: false,
                                textLabels: {
                                    body: {
                                        noMatch: 'No se encontraron resultados',
                                    },
                                    pagination: {
                                        rowsPerPage: 'Filas por página',
                                    },
                                    toolbar: {
                                        downloadCsv: 'Descargar CSV',
                                        search: 'Buscar',
                                        viewColumns: 'Mostrar Columnas',
                                    },
                                    viewColumns: {
                                        title: 'Mostrar Columnas',
                                    },
                                },
                                download: false,
                                customToolbar: () => (
                                    <Tooltip title="Descargar Excel">
                                        {downloading ? (
                                            <CircularProgress
                                                size={20}
                                                style={{
                                                    verticalAlign: 'middle',
                                                    marginLeft: '14px',
                                                    marginRight: '14px',
                                                }}
                                            />
                                        ) : (
                                            <IconButton onClick={handleClick}>
                                                <CloudDownload />
                                            </IconButton>
                                        )}
                                    </Tooltip>
                                ),
                                pagination: false,
                            }}
                        />
                    </Box>
                </Grid>
            </CHPaper>
        </CHPrivateContainer>
    );


}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
    productState: ProductState;
    creditState: CreditState

}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    productState: state.producto,
    creditState: state.credit,


});

interface LinkMapDispatchToProps {
    startGenerarBalanceCartera: (data: any) => void;
    startDescargarBalanceCarteraAux: (data: any) => void;
    startLimpiarStateBalanceCartera: () => void;
    
}

const mapDispatchToProps = {
    startGenerarBalanceCartera,
    startDescargarBalanceCarteraAux,
    startLimpiarStateBalanceCartera,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(GenerarBalanceCartera);

