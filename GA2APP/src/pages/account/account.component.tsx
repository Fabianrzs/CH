import React, { useEffect, useMemo, useState } from 'react';
import { connect } from 'react-redux';
import './account.scss';
import {
    Table,
    TableCell,
    TableHead,
    TableRow,
    TableBody,
    Typography,
    Button,
    Grid,
    createStyles,
    makeStyles,
    Box,
} from '@material-ui/core';

import Accordion from '@material-ui/core/Accordion';
import AccordionSummary from '@material-ui/core/AccordionSummary';
import AccordionDetails from '@material-ui/core/AccordionDetails';

import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { toast } from 'react-toastify';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import { AppState } from '../../store/rootReducer';
import { ClientState } from '../../reducers/client.reducer';
import {
    ClienteDto,
    ConceptosSaldosBloqueos,
    Movimiento,
} from '../../types/client.model.type';
import { startGetMovementsAccountsClient } from '../../actions/client.action';
import MovementsTable from './movements.component';
import UtilService from '../../utils/util.service';
import history from '../../utils/history.router';
import ApiReportes from '../../services/api-reportes.service';
import { ClientForm } from '../curriculum/constants/form.mode.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
            borderRadius: '10%',
            background: '#1C3F8B',
            color: '#FFFFFF',
        },
        restoreButton: {
            width: '250px',
            marginLeft: '.5rem',
            background: '#1C3F8B',
            color: '#FFFFFF',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
        column3: {
            textAlign: 'left',
            width: '50%',
            marginLeft: '250px',
        },
        column4: {
            width: '100%',
            textAlign: 'left',
        },
        titleform: {
            color: '#1C3F8B',
            fontWeight: 'bold',
            fontSize: '1rem',
            margin: 0,
            lineHeight: 1.5,
        },

        clientInfo: {
            color: '#1C3F8B',
            fontWeight: 'bold',
            fontSize: '1rem',
            margin: 0,
            lineHeight: 1.5,
        },


    }),
);
interface dataEmbargo {
    conceptoId: number,
    valor: number,
    tipoEmbargo: any
}

function AccountTable({
    data,
    numeroCuenta,
    numeroCuentaCompleta,
    numeroMovimientos,
    dataEmbargos,
    dataConceptos,
    clientState,
    dataEmbargosBloqueos,
    tipoCuenta,
    totalEmbargosHv
}: {
    data: ConceptosSaldosBloqueos[] | null;
    numeroCuenta: string;
    numeroCuentaCompleta: string;
    numeroMovimientos: Movimiento[];
    dataEmbargos: any[];
    dataConceptos: any[];
    clientState: ClientState;
    dataEmbargosBloqueos: ConceptosSaldosBloqueos[] | null;
    tipoCuenta: Movimiento[];
    totalEmbargosHv: any;
}) {
    const porcentaje = 'ae7d141a-fd88-4f57-a199-4db5b5301946';
    const variacionPorcentaje = 'f65c4c4a-174e-4406-8aed-32b5b47cf0b3';
    const valor = '95ca1858-b89d-4631-9893-22932c925184';
    const [open, setOpen] = useState(false);
    const [conceptDescription, setConceptDescription] = useState("");
    const [conceptSaldo, setConceptSaldo] = useState(0);
    const [conceptMovements, setConceptMovements] = useState<Movimiento[]>([]);
    const setterRedirectMovements = (
        dataMovement: Movimiento[],
        concepto: string | undefined,
        saldo: number,
    ) => {
        setOpen(true);
        setConceptMovements(dataMovement);
        setConceptDescription(concepto != null ? concepto : "");
        setConceptSaldo(saldo != null ? saldo : 0)
    };
    const classes = useStyles();

    const handleClose = () => {
        setOpen(false);
    }

    let total = 0;
    let totalDisponible = 0;
    let totalEmbargos = 0;
    // let totalBloqueosEmbargos = 0;
    let valorSolicitado = 0;
    const valorSolicitadoReintegro = 0;
    let embargoConcepto: dataEmbargo[] = []

    console.log(conceptMovements, 'setterRedirectMovements');

    const obtenerValorReintegro = () => {
        if (dataConceptos !== undefined && dataConceptos.length > 0) {
            dataConceptos?.forEach((item: any) => {
                if (item.valorBloqueoEmbargo > 0) {
                    valorSolicitado += item.valorBloqueoEmbargo;
                }
            })
        }
        return valorSolicitado;
    }

    const obtenerValorEmbargo = (idConcepto: number) => {
        obtenerTotal()

        const data = embargoConcepto.filter((x: any) => x.conceptoId === idConcepto)
        let valor = 0;
        if (data) {
            data?.forEach((e: any) => {
                valor += e.valor;
            })
            return valor;
        }
        return 0;
    }

    const obtenerTotal = () => {
        const embargoConceptoE: dataEmbargo[] = [];
        let totalDisponibleLocal = totalDisponible;
        let valorSolicitadoReintegroLocal = 0;
        let tieneBloqueosCuenta = false;
    
        if (dataConceptos !== null && dataConceptos !== undefined && dataConceptos.length > 0) {
            dataConceptos.forEach((item: any) => {
                if (item.valorBloqueoEmbargo > 0) {
                    valorSolicitadoReintegroLocal += item.valorBloqueoEmbargo;
                    tieneBloqueosCuenta = true;
                }
    
                const dataE = dataEmbargos.filter((x: any) => x.conceptoId === item.id);
    
                if (dataE.length > 0) {
                    tieneBloqueosCuenta = true;
    
                    dataE.forEach((embargo: any) => {
                        embargoConceptoE.push({
                            conceptoId: item.conceptoId,
                            valor: embargo.tipoEmbargoId === valor
                                ? embargo.valor
                                : item.saldo * (embargo.valor / 100),
                            tipoEmbargo: embargo.tipoEmbargoId
                        });
                    });
                }
            });
    
            embargoConcepto = embargoConceptoE;
        }
    
        if (data && data.length > 0) {
            const tieneValorAcumulado = data.some((item: any) => (item.valorAcumulado || 0) > 0);
            if (tieneValorAcumulado) {
                tieneBloqueosCuenta = true;
            }
        }
    
        if (valorSolicitadoReintegroLocal > 0) {
            totalDisponibleLocal -= valorSolicitadoReintegroLocal;
        }
    
        if (tieneBloqueosCuenta && totalEmbargosHv > 0) {
            totalDisponibleLocal -= totalEmbargosHv;
        }
    
        return totalDisponibleLocal < 0 ? 0 : totalDisponibleLocal;
    }

    const obtenerPorcentajeEmbargo = (idConcepto: number, saldo: number, valorAcumulado: number) => {
        if (valorAcumulado > 0) {
            return (valorAcumulado * 100) / saldo;
        }
        const data = embargoConcepto.filter((x: any) => x.conceptoId === idConcepto)
        let valor = 0;
        if (data.length > 0) {
            data?.forEach((e: any) => {
                valor += e.valor;
            })
            return (valor * 100) / saldo;
        }
        return 0;
    }

    let totalBloqueosEmbargos = data?.reduce((acum, element) => acum + (element.valorAcumulado + obtenerValorEmbargo(element.idConcepto)), 0) || 0;

    if (total === 0) {
        data?.forEach(element => {
            total += element.saldoConceptoCuenta;
            totalDisponible += element.saldoConceptoCuenta;
        });

        dataEmbargosBloqueos?.forEach(bloqueo => {
            totalBloqueosEmbargos += bloqueo.valorAcumulado;
        });

        dataEmbargos?.forEach(elementEmbargo => {
            totalEmbargos +=
                elementEmbargo.tipoEmbargoId === valor
                    ? elementEmbargo.valor
                    : (dataConceptos?.find((x: any) => x.id === elementEmbargo.conceptoId)?.saldo || 0) *
                    (elementEmbargo.valor / 100);
        });

        totalDisponible -= totalEmbargos;
        totalEmbargos += totalBloqueosEmbargos;
        totalDisponible -= totalBloqueosEmbargos;
        totalDisponible -= valorSolicitado;
    }


    const onClickExcel = async () => {
        const columnName = [
            "Número cuenta",
            "Concepto",
            "Saldo",
            "Bloqueo",
            "% Bloqueo",
            "Número de movimientos"
        ];

        const columnDataList = data?.map((element) => {
            const numeroMovimientosPorConcepto = numeroMovimientos.filter(
                x => x.cnC_ID === element.idConcepto
            ).length;

            return {
                data: [
                    String(numeroCuentaCompleta ?? ''),
                    String(element.concepto ?? ''),
                    String(UtilService.formatCurrency(element.saldoConceptoCuenta) ?? ''),
                    String(UtilService.formatCurrency(element.valorAcumulado + obtenerValorEmbargo(element.idConcepto)) ?? ''),
                    `${UtilService.roundValue(
                        obtenerPorcentajeEmbargo(
                            element.idConcepto,
                            element.saldoConceptoCuenta,
                            element.valorAcumulado
                        ),
                        2
                    )}%`,
                    String(numeroMovimientosPorConcepto)
                ]
            };
        }) ?? [];

        columnDataList.push(
            {
                data: [
                    '',
                    'TOTAL SALDO CONCEPTOS',
                    String(UtilService.formatCurrency(total)),
                    '',
                    '',
                    ''
                ]
            },
            {
                data: [
                    '',
                    'TOTAL BLOQUEOS Y/O EMBARGOS',
                    total === 0 ? '$0' : String(UtilService.formatCurrency(totalEmbargos)),
                    '',
                    '',
                    ''
                ]
            },
            {
                data: [
                    '',
                    'TOTAL BLOQUEOS POR REINTEGROS',
                    String(UtilService.formatCurrency(obtenerValorReintegro())),
                    '',
                    '',
                    ''
                ]
            },
            {
                data: [
                    '',
                    'TOTAL SALDO DISPONIBLE',
                    String(UtilService.formatCurrency(obtenerTotal())),
                    '',
                    '',
                    ''
                ]
            }
        );

        const request = {
            columnName,
            columnDataList
        };

        const apiReportes = ApiReportes();

        try {
            const response = await apiReportes.GenerarExcels(request);

            const blob = new Blob([response], {
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
            });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;


            a.download = `Reporte_Cuenta_${numeroCuentaCompleta}.xlsx`;

            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);


        } catch (error) {
            console.error('Error al generar el Excel:', error);
            toast('Error al descargar el reporte. Verifica la respuesta del servidor.', {
                type: toast.TYPE.ERROR
            });
        }
    };

    // const totalBloqueos = clientState?.client?.cuentas?.map(
    //     cuenta => cuenta?.totalBloqueos
    // );

    const cuentaActual = clientState?.client?.cuentas?.find(
        (cuenta: any) =>
            cuenta.numeroCuenta === numeroCuenta ||
            cuenta.numeroCuentaCompleta === numeroCuentaCompleta
    );

    const saldoDisponibleEmbargos =
        cuentaActual?.saldoDisponibleEmbargos ?? 0;

    return (
        <Grid>

            <Button
                className={classes.restoreButton}
                onClick={() =>
                    setterRedirectMovements(
                        numeroMovimientos,
                        '',
                        obtenerTotal() + totalBloqueosEmbargos
                    )
                }
            >
                Ver movimientos
            </Button>
            <Button onClick={() => onClickExcel()} className={classes.restoreButton} >
                REPORTE
            </Button>
            <Table className="account-table">
                <TableHead>
                    <TableRow>
                        <TableCell>Número cuenta</TableCell>
                        <TableCell>Concepto</TableCell>
                        <TableCell>Saldo</TableCell>
                        <TableCell>Bloqueo</TableCell>
                        <TableCell>% Bloqueo</TableCell>
                        <TableCell>Número de movimientos</TableCell>
                        <TableCell>Acciones</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data?.map((element, index) => (
                        <TableRow key={`accordion-${index}`}>
                            <TableCell>{numeroCuentaCompleta}</TableCell>
                            <TableCell>
                                {
                                    element.concepto
                                }
                            </TableCell>
                            <TableCell>
                                {UtilService.formatCurrency(
                                    element.saldoConceptoCuenta,
                                )}
                            </TableCell>
                            <TableCell>
                                {UtilService.formatCurrency(
                                    element.valorAcumulado + obtenerValorEmbargo(element.idConcepto)
                                )}
                            </TableCell>
                            <TableCell>
                                {UtilService.roundValue(
                                    (obtenerPorcentajeEmbargo(element.idConcepto, element.saldoConceptoCuenta, element.valorAcumulado)), 2
                                )}%
                            </TableCell>
                            <TableCell>
                                {
                                    numeroMovimientos.filter(
                                        x => x.cnC_ID === element.idConcepto,
                                    ).length
                                }
                            </TableCell>
                            <TableCell>
                                <Button
                                    variant="contained"
                                    color="primary"
                                    onClick={() =>
                                        setterRedirectMovements(
                                            numeroMovimientos.filter(
                                                x =>
                                                    x.cnC_ID ===
                                                    element.idConcepto,
                                            ),
                                            element.concepto,
                                            element.saldoConceptoCuenta
                                        )
                                    }
                                >
                                    Ver movimientos
                                </Button>
                            </TableCell>
                        </TableRow>
                    ))}

                    <TableRow key='accordion-total'>
                        <TableCell> </TableCell>
                        <TableCell>TOTAL SALDO CONCEPTOS</TableCell>
                        <TableCell>{UtilService.formatCurrency(total)}</TableCell>
                        <TableCell colSpan={2}> </TableCell>
                        <TableCell> </TableCell>
                        <TableCell> </TableCell>
                    </TableRow>
                    <TableRow key='accordion-total'>
                        <TableCell> </TableCell>
                        <TableCell>TOTAL BLOQUEOS Y/O EMBARGOS</TableCell>
                        <TableCell>
                            {UtilService.formatCurrency(totalBloqueosEmbargos)}
                        </TableCell>

                        <TableCell colSpan={2}> </TableCell>
                        <TableCell> </TableCell>
                        <TableCell> </TableCell>
                    </TableRow>
                    <TableRow key='accordion-total'>
                        <TableCell> </TableCell>
                        <TableCell>TOTAL BLOQUEOS POR REINTEGROS</TableCell>
                        <TableCell>{UtilService.formatCurrency(obtenerValorReintegro())}</TableCell>
                        <TableCell colSpan={2}> </TableCell>
                        <TableCell> </TableCell>
                        <TableCell> </TableCell>
                    </TableRow>
                    <TableRow key='accordion-total'>
                        <TableCell> </TableCell>
                        <TableCell>TOTAL SALDO DISPONIBLE</TableCell>
                        <TableCell>{UtilService.formatCurrency(saldoDisponibleEmbargos)}</TableCell>
                        <TableCell colSpan={2}> </TableCell>
                        <TableCell> </TableCell>
                        <TableCell> </TableCell>

                    </TableRow>
                </TableBody>
            </Table>
            <MovementsTable
                movimientos={conceptMovements}
                onOpen={open}
                concepto={conceptDescription}
                onClose={handleClose}
                saldo={conceptSaldo}
                anioDefault={0}
                clientState={clientState}
                numeroCuentaCompleta={numeroCuentaCompleta}
                tipoCuenta={tipoCuenta}

            />
        </Grid>
    );
}

function Account({
    clientState,
    startGetMovementsAccountsClient,
    clientModel,
}: {
    clientState: ClientState;
    startGetMovementsAccountsClient: (identification: string) => void;
    clientModel: ClienteDto;
}) {
    const classes = useStyles();
    const [searchData, setSearchData] = useState(false);
    const [detailCurriculum, setDetailCurriculum] = useState(false);
    const [saldoDisponible, setSaldoDisponible] = useState<number>(0);

    useEffect(() => {
        setToResponseClient(clientState.client);
    }, []);

    const [clientInfo, setClientInfo] = React.useState(ClientForm);

    const setToResponseClient = (client: ClienteDto | null) => {
        setClientInfo({ ...clientInfo, ...client });
    };

    const embargosFiltrados = clientInfo?.solicitudes?.filter(s => {
        const nombre = s.tipoSolicitud?.toUpperCase() ?? '';
        return nombre.includes('EMBARGOS') || nombre.includes('DESEMBARGOS') || nombre.includes('EMBARGO');
    });

    const toNumber = (v: any): number => {
        if (v == null) return 0;
        if (typeof v === 'number') return Number.isFinite(v) ? v : 0;

        const s = String(v).trim();

        const cleaned = s.replace(/[^\d.,-]/g, '');

        if (cleaned.includes(',') && cleaned.includes('.')) {
            const normalized = cleaned.replace(/\./g, '').replace(',', '.');
            const n = Number(normalized);
            return Number.isFinite(n) ? n : 0;
        }

        if (cleaned.includes(',') && !cleaned.includes('.')) {
            const n = Number(cleaned.replace(',', '.'));
            return Number.isFinite(n) ? n : 0;
        }

        const n = Number(cleaned.replace(/,/g, ''));
        return Number.isFinite(n) ? n : 0;
    };

    const totalEmbargosHv = embargosFiltrados.reduce(
        (acc: number, s: any) => acc + toNumber(s.valor),
        0
    );


    if (searchData) {
        setSearchData(false);
        history.push('/HojaDeVida')
    }

    if (detailCurriculum) {
        setDetailCurriculum(false)
        history.push('/InformacionCliente')
    }

    useEffect(() => {
        const identification =
            clientState.client?.identificacion != null
                ? clientState.client?.identificacion
                : '';
        startGetMovementsAccountsClient(identification);
    }, []);

    useEffect(() => {
        if (clientState.client?.saldoDisponible !== undefined) {
            setSaldoDisponible(Number(clientState.client.saldoDisponible));
        }
    }, [clientState.client?.saldoDisponible]);

    const {
        client: {
            nombreTipoIdentificacion,
            identificacion,
            primerNombre,
            segundoNombre,
            primerApellido,
            segundoApellido,
        } = {},
    } = clientState;

    const nombreCompleto = [primerNombre, segundoNombre, primerApellido, segundoApellido]
        .filter(Boolean)
        .join(' ');

    const clienteInfo =
        nombreTipoIdentificacion && identificacion && nombreCompleto
            ? `${nombreTipoIdentificacion}: ${identificacion} - ${nombreCompleto}`
            : '';


    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Información Cuentas"
                subText=""
                title1="Cuentas del"
                title2="Afiliado"
            />

            <Box
                display="flex"
                justifyContent="space-between"
                alignItems="center"
                width="100%"
            >
                <Typography
                    variant="body1"
                    className={classes.clientInfo}
                >
                    {clienteInfo}
                </Typography>

                <Typography
                    variant="body1"
                    className={classes.titleform}
                >
                    Saldo total disponible:{' '}
                    {UtilService.formatCurrency(
                        Math.abs(Number(saldoDisponible))
                    )}
                </Typography>
            </Box>


            {clientState?.client?.cuentas?.map((element, index) => (
                <Accordion key={`key-${index}`}>

                    <AccordionSummary
                        expandIcon={<ExpandMoreIcon />}
                        aria-controls="panel1a-content"
                        id="panel1a-header"
                    >
                        <Grid className={`item-containerModal ${classes.column2}`} >
                            <Grid className='subcontainerModal'>
                                <Grid container >
                                    <Typography>
                                        <p>Cuenta No.: {element.numeroCuentaCompleta}</p>
                                    </Typography>
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid className={`item-containerModal ${classes.column4}`} >
                            <Grid className='subcontainerModal'>
                                <Grid container >
                                    <Typography>
                                        <p>{element.tipoCuenta}</p>
                                    </Typography>
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid className={`item-containerModal ${classes.column2}`} >
                            <Grid className='subcontainerModal'>
                                <Grid container >
                                    <Typography>
                                        <p>Fecha creación: {UtilService.getDate(element.fechaCreacion)}</p>
                                    </Typography>
                                </Grid>
                            </Grid>
                        </Grid>
                        <Grid className={`item-containerModal ${classes.column2}`} >
                            <Grid className='subcontainerModal'>
                                <Grid container >
                                    <p>Estado de Cuenta: {element.estadoCuenta} </p>
                                </Grid>
                            </Grid>
                        </Grid>

                    </AccordionSummary>

                    <AccordionDetails>
                        <AccountTable
                            data={element.conceptosSaldosBloqueos}
                            dataEmbargosBloqueos={element.conceptosEmbargosBloqueos}
                            dataEmbargos={element.embargos}
                            numeroCuenta={element.numeroCuenta.toString()}
                            numeroCuentaCompleta={element.numeroCuentaCompleta.toString()}
                            numeroMovimientos={element.movimientos}
                            dataConceptos={element.conceptos}
                            clientState={clientState}
                            tipoCuenta={element.tipoCuenta.toString()}
                            totalEmbargosHv={totalEmbargosHv}
                        />
                    </AccordionDetails>

                </Accordion>
            ))}
            <div className="buttons">
                <Button
                    className={classes.restoreButton}
                    variant="outlined"
                    onClick={() => setDetailCurriculum(true)}
                >
                    DETALLE
                </Button>
                <Button
                    className={classes.restoreButton}
                    variant="outlined"
                    type="submit"
                    onClick={() => setSearchData(true)}
                >
                    NUEVA CONSULTA
                </Button>
            </div>
        </CHPrivateContainer>
    );
}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
});

const mapDispatchToProps = {
    startGetMovementsAccountsClient,
};

export default connect(mapStateToProps, mapDispatchToProps)(Account);
