import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Controller, useForm } from "react-hook-form";
import './account.scss';
import {
    Table,
    TableCell,
    TableHead,
    TableRow,
    TableBody,
    Dialog,
    DialogContent,
    DialogTitle,
    Button,
    DialogActions,
    Grid,
    makeStyles,
    createStyles,
    Box,
    Typography
} from '@material-ui/core';
import { toast } from 'react-toastify';
import { DatePicker } from '@material-ui/pickers';
import { AppState } from '../../store/rootReducer';
import { Movimiento } from '../../types/client.model.type';
import UtilService from '../../utils/util.service';
import CHDatePickerV7 from '../../components/ch-datepicker/CHDatepickerV7';
import ApiReportes from '../../services/api-reportes.service';
import AuthService from '../../services/auth.service';
import { ClientState } from '../../reducers/client.reducer';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        color: {
            color: '#061311'
        },
        dialogTitleContainer: {
            display: 'flex',
            flexDirection: 'column',
            gap: '4px',
        },
        affiliateTitle: {
            fontWeight: 'bold',
            fontSize: '1rem',
            lineHeight: 1.5,
            margin: 0,
            color: '#061311',
        },
        dialogSubtitle: {
            fontSize: '0.875rem',
            lineHeight: 1.4,
            margin: 0,
            color: '#6B7280', // texto secundario
        },
    }),
);

function MovementsTable({
    movimientos,
    onOpen,
    concepto,
    saldo,
    onClose,
    anioDefault,
    clientState,
    numeroCuentaCompleta,
    tipoCuenta

}: {
    movimientos: Movimiento[];
    onOpen: boolean;
    concepto: string;
    onClose: () => void;
    saldo: number;
    anioDefault: number;
    clientState: ClientState;
    numeroCuentaCompleta: string;
    tipoCuenta: string;

}) {
    const [anio, setAnio] = useState<Date | null>();
    const [movimientosFiltrados, setMovimientos] = useState<Movimiento[]>(movimientos);
    const today = new Date();
    const classes = useStyles();

    useEffect(() => {
        if (onOpen) {
            setFrom(null);
            setTo(null);
            setUseMonth(false);
        }
    }, [onOpen]);

    const obtenersaldo = () => {
        let saldoAcumulado = saldo;
        const listSaldo: number[] = [];
        listSaldo.push(saldo);

        movimientosFiltrados?.forEach((item: Movimiento) => {
            if (item.caT_TIPO_MOVIMIENTO === 'C') {
                saldoAcumulado -= item.mvT_VALOR;
            } else {
                saldoAcumulado += item.mvT_VALOR;
            }
            listSaldo.push(saldoAcumulado);
        });

        return listSaldo;
    };

    const saldos = obtenersaldo();

    const { control, setValue, handleSubmit, reset } = useForm({
        defaultValues: {
            fecha: null,
            tipo: '',
        },
    });


    const onClickExcel = async () => {

        const columnDataList = movimientosFiltrados.map((element, index) => ({
            data: [
                element.dcT_ID,
                element.mvT_ID,
                element.cnC_CONCEPTO,
                element.mvT_ANO_PERIODO,
                element.caT_TIPO_MOVIMIENTO === 'C' ? UtilService.formatCurrency(element.mvT_VALOR) : '',
                element.caT_TIPO_MOVIMIENTO === 'D' ? UtilService.formatCurrency(element.mvT_VALOR) : '',
                UtilService.getFullDateWithTimeDirection(element.mvT_FECHA_PROCESO),
                UtilService.formatCurrency(saldos[index])
            ].map(value => String(value ?? ''))
        }));

        const request = {
            columnDataList: columnDataList.map(item => ({
                data: item.data
            })),
            columnName: [
                "Documento",
                "Identificador",
                "Concepto",
                "Periodo",
                "Ingreso",
                "Egreso",
                "Fecha Movimiento",
                "Saldo"
            ]
        };



        const apiReportes = ApiReportes();

        try {
            const response = await apiReportes.GenerarExcels(request);

            console.log('Es un Blob:', response instanceof Blob);
            console.log('Tipo de respuesta:', response);


            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `Movimientos - ${concepto === '' ? 'cuenta' : concepto} - Saldo ${UtilService.formatCurrency(saldo)}.xlsx`;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);


            const user = AuthService.getUserInfo();
            const nav = window.navigator as any;
            const usrAg = nav.userAgentData.brands.find(
                (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand;




        } catch (error) {
            console.error('Error al generar el Excel:', error);
            toast('Error al descargar el reporte. Verifica la respuesta del servidor.', { type: toast.TYPE.ERROR });
        }
    };

    const handleDetalleMovimientos = () => {
        console.log('.');
    }

    useEffect(() => {
        if (!onOpen) {
            setAnio(null);
        }
    }, [onOpen]);

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


    const [from, setFrom] = useState<Date | null>(null);
    const [to, setTo] = useState<Date | null>(null);
    const [useMonth, setUseMonth] = useState(false);

    const startOfYear = (d: Date) => new Date(d.getFullYear(), 0, 1, 0, 0, 0, 0);
    const startOfNextYear = (d: Date) => new Date(d.getFullYear() + 1, 0, 1, 0, 0, 0, 0);

    const startOfMonth = (d: Date) => new Date(d.getFullYear(), d.getMonth(), 1, 0, 0, 0, 0);
    const startOfNextMonth = (d: Date) => new Date(d.getFullYear(), d.getMonth() + 1, 1, 0, 0, 0, 0);

    function buildRange(from: Date | null, to: Date | null, useMonth: boolean) {
        if (!from && !to) return { fechaDesde: null as Date | null, fechaHasta: null as Date | null };

        const f = from ?? to!;
        const t = to ?? from!;

        let fechaDesde = useMonth ? startOfMonth(f) : startOfYear(f);
        let fechaHasta = useMonth ? startOfNextMonth(t) : startOfNextYear(t);


        if (fechaHasta.getTime() < fechaDesde.getTime()) {
            fechaDesde = useMonth ? startOfMonth(t) : startOfYear(t);
            fechaHasta = useMonth ? startOfNextMonth(f) : startOfNextYear(f);
        }

        return { fechaDesde, fechaHasta };
    }


    useEffect(() => {

        if (!from && !to) {
            setMovimientos(movimientos);
            return;
        }

        const { fechaDesde, fechaHasta } = buildRange(from, to, useMonth);

        const filtrados = movimientos.filter((x) => {
            const fechaMov = new Date(x.mvT_FECHA_PROCESO);
            return (
                (!fechaDesde || fechaMov.getTime() >= fechaDesde.getTime()) &&
                (!fechaHasta || fechaMov.getTime() < fechaHasta.getTime())
            );
        });

        setMovimientos(filtrados);
    }, [from, to, useMonth, movimientos]);

    type MovimientoConSaldo = typeof movimientosFiltrados[number] & {
        saldo: number;
    };
    
    const movimientosConSaldo = movimientosFiltrados.reduce<MovimientoConSaldo[]>(
        (acc, mov) => {
            const saldoAnterior = acc.length ? acc[acc.length - 1].saldo : 0;
    
            const saldo =
                mov.caT_TIPO_MOVIMIENTO === 'C'
                    ? saldoAnterior + mov.mvT_VALOR
                    : saldoAnterior - mov.mvT_VALOR;
    
            acc.push({
                ...mov,
                saldo
            });
    
            return acc;
        },
        []
    );
    
    const movimientosParaMostrar = movimientosConSaldo;
    const saldosInvertidos = [...movimientosConSaldo.map(item => item.saldo)].reverse();


    return (
        <Dialog
            fullWidth
            maxWidth="md"
            open={onOpen}
            aria-labelledby="form-dialog-title"
        >
            <DialogTitle id="form-dialog-title">
                <div className={classes.dialogTitleContainer}>
                    <Typography
                        variant="subtitle1"
                        className={classes.affiliateTitle}
                    >
                        {clienteInfo}
                    </Typography>

                    <Typography variant="body2" color="textSecondary">
                        Cuenta No.: {numeroCuentaCompleta}
                    </Typography>


                    <Typography variant="body2" color="textSecondary">
                        {tipoCuenta}
                    </Typography>


                    <Typography
                        variant="body2"
                        className={classes.dialogSubtitle}
                    >
                        Movimientos – {concepto || 'cuenta'} – Saldo:{' '}
                        {UtilService.formatCurrency(saldo)}
                    </Typography>
                </div>
            </DialogTitle>
            <DialogContent>
                <Grid container spacing={2} className={classes.column}>
                    <Grid item xs={12} md={3}>
                        <CHDatePickerV7
                            label="Desde"
                            views={useMonth ? ['year', 'month'] : ['year']}
                            onChange={(e) => setFrom(e)}
                            value={from ?? null}
                            format={useMonth ? 'yyyy/MM' : 'yyyy'}
                            inputProps={{ readOnly: false }}
                            InputLabelProps={{ shrink: true }}
                            autoOk
                            minDate={new Date('1980-01-01')}
                            maxDate={today}
                            invalidDateMessage={useMonth ? 'Formato inválido. Usa AAAA/MM' : 'Formato inválido. Usa AAAA'}
                        />
                    </Grid>
                    <Grid item xs={12} md={3}>
                        <CHDatePickerV7
                            label="Hasta"
                            views={useMonth ? ['year', 'month'] : ['year']}
                            onChange={(e) => setTo(e)}
                            value={to ?? null}
                            format={useMonth ? 'yyyy/MM' : 'yyyy'}
                            inputProps={{ readOnly: false }}
                            InputLabelProps={{ shrink: true }}
                            autoOk
                            minDate={new Date('1980-01-01')}
                            maxDate={today}
                            invalidDateMessage={useMonth ? 'Formato inválido. Usa AAAA/MM' : 'Formato inválido. Usa AAAA'}
                        />
                    </Grid>

                    <Grid item xs={12} md={3}>
                        <label
                            htmlFor="useMonth"
                            style={{ display: 'flex', gap: 8, alignItems: 'center', cursor: 'pointer' }}
                        >
                            <input
                                id="useMonth"
                                type="checkbox"
                                checked={useMonth}
                                onChange={(e) => setUseMonth(e.target.checked)}
                            />
                            <span>Incluir mes (opcional)</span>
                        </label>
                    </Grid>


                </Grid>

                <Table className="account-table" stickyHeader>
                    <TableHead>
                        <TableRow>
                            <TableCell>Documento</TableCell>
                            <TableCell>Identificador</TableCell>
                            <TableCell>Concepto</TableCell>
                            <TableCell>Periodo</TableCell>
                            <TableCell>Ingreso</TableCell>
                            <TableCell>Egreso</TableCell>
                            <TableCell>Fecha Movimiento</TableCell>
                            <TableCell>Saldo</TableCell>
                            <TableCell>Accion</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {movimientosParaMostrar.map((element, index) => (

                            <TableRow key={`accordion-${index}`}>
                                <TableCell>{element.dcT_ID}</TableCell>
                                <TableCell>{element.mvT_ID}</TableCell>
                                <TableCell>{element.cnC_CONCEPTO}</TableCell>
                                <TableCell>{element.mvT_ANO_PERIODO}</TableCell>
                                <TableCell>{element.caT_TIPO_MOVIMIENTO === 'C' ? UtilService.formatCurrency(element.mvT_VALOR) : null}</TableCell>
                                <TableCell>
                                    {element.caT_TIPO_MOVIMIENTO === 'D' ? UtilService.formatCurrency(element.mvT_VALOR) : null}
                                </TableCell>
                                <TableCell>
                                    {UtilService.getFullDateWithTimeDirection(element.mvT_FECHA_PROCESO)}
                                </TableCell>
                                <TableCell>
                                    {/* {UtilService.formatCurrency(element.saldo)} */}
                                    {UtilService.formatCurrency(saldosInvertidos[index])}
                                </TableCell>
                                <TableCell>
                                    <span aria-label="Detalle" onClick={handleDetalleMovimientos}>
                                        <span className="ico fch-Hoja-Vida-0-1 icoMid-3" />
                                    </span>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </DialogContent>
            <DialogActions className='maginButtonAddClose'>
                <Button onClick={onClose} variant="contained">
                    Regresar
                </Button>

                <Button onClick={handleSubmit(onClickExcel)} variant="outlined" >
                    REPORTE
                </Button>

            </DialogActions>
        </Dialog>
    );
}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
});

export default connect(mapStateToProps)(MovementsTable);
