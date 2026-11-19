import React, { useEffect, useState } from 'react';
import {
    Dialog,
    DialogContent, Typography, Button, makeStyles, createStyles, DialogActions,
    Grid, CardContent, Card
} from '@material-ui/core';
import { format } from 'date-fns';
import { AdminCuentasAfiliado } from '../../../../types/client.model.type';
import UtilService from '../../../../utils/util.service';


function ModalDetalleDeCuenta({
    responseAdminCuenta,
    handleClose,
    open,
    cuentaId,
    bloqueosManuales = {}
}: {
    responseAdminCuenta: AdminCuentasAfiliado;
    handleClose: any;
    open: any;
    cuentaId: any;
    bloqueosManuales?: Record<string, number>;
}) {

    const useStyles = makeStyles(() =>
        createStyles({
            dialogPaper: {
                width: '80%',
                height: '85%'
            },
            dialogTitles: {
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between'
            },
            buttonClose: {
                position: 'absolute',
                right: '8',
                top: '8'
            },
            root: {
                flexGrow: 1,
            },
            gridPadding: {
                padding: '1rem'
            },
            cardContent: {
                margin: '0',
                padding: '0rem'
            },
            dialogRoot: {
                padding: '.5rem'
            },
            marginTops: {
                marginTop: '1rem'
            }
        })
    );
    const classes = useStyles();
    const valor = '95ca1858-b89d-4631-9893-22932c925184';
    const [showVisible, visibleDetalleConcepto] = useState<boolean>();

    const [nombreConceptoSelect, setNombreConceptoSelect] = useState<string>();
    const [detalleConceptoFilter, setDetalleConcepto] = useState<any[]>();
    let totalEmbargos = 0;
    const conceptosCuenta = () => {
        const filter = responseAdminCuenta.cuentas.filter(
            (x) =>
                x.cuentaId.toString().includes(cuentaId)
        );
        
        return filter;
    };

    const detalleConceptoFil = (idConcepto: string, idCuenta: number, detalleConcepto: any) => {
        const filter = detalleConcepto?.filter(
            (x: any) =>
                x.cuentaid.toString().includes(idCuenta.toString()) ||
                x.conceptoId.toString().includes(idConcepto.toString())
        );

        return filter;
    };

    const valorEmbargo = (cuentaId: string, conceptoId: string) => {

        const filter = responseAdminCuenta.cuentas.filter(
            (x) =>
                x.cuentaId.toString().includes(cuentaId)
        );
        const concepto = filter[0].conceptos?.find(x => x.conceptoId === conceptoId);
        if (concepto) {
            const embargos = filter[0].listaEmbargos?.filter(x => x.conceptoId === concepto.id);
            if (embargos.length > 0) {
                let total = 0;
                embargos.forEach((e: any) => {
                    total += e.tipoEmbargoId === valor ? e.valor : concepto?.saldo * e?.valor / 100;
                })
                totalEmbargos += total;
                return total;
            }
        }
        return 0;
    };

    const porcentajeEmbargo = (cuentaId: string, conceptoId: string) => {
        const filter = responseAdminCuenta.cuentas.filter(
            (x) =>
                x.cuentaId.toString().includes(cuentaId)
        );
        const concepto = filter[0].conceptos?.find(x => x.conceptoId === conceptoId);
        if (concepto) {
            const embargos = filter[0].listaEmbargos?.filter(x => x.conceptoId === concepto.id);
            if (embargos.length > 0) {
                let total = 0;
                embargos.forEach((e: any) => {
                    total += e.tipoEmbargoId === valor ? e.valor * 100 / concepto?.saldo : e.valor;
                })
                return total;
            }
        }
        return 0;
    };


    const handleEventDetalleConcepto = (idConcepto: string, idCuenta: number, detalleConcepto: any, descripcionConcepto: string) => (event: React.MouseEvent) => {
        visibleDetalleConcepto(true);
        setNombreConceptoSelect(descripcionConcepto);
        setDetalleConcepto(detalleConceptoFil(idConcepto, idCuenta, detalleConcepto));
        event.preventDefault();
    }

    let total = 0;
    const totalDisponible = () => {
        if (total === 0) {
            conceptosCuenta()?.forEach(element => {
                total += element.saldoDisponible
            });
            total -= totalEmbargos;
            return total
        }
        return 0;
    }

    return (
        <>
            <div>
                <Dialog classes={{ paper: classes.dialogPaper }}
                    open={open}
                    onClose={handleClose}
                    maxWidth="lg"

                >
                    <div >
                        <Typography
                            variant="h5"
                            component="h5"
                            className={classes.dialogTitles}
                        >
                            Conceptos y Detalles De La Cuenta
                        </Typography>
                    </div>
                    <div className={classes.marginTops}>
                        {' '}
                    </div>
                    <div >
                        <Grid item xs={12} >
                            <Card >

                                <Grid className="container-ico-t marginTopContainerPopup">
                                    <span className="ico fch-Puntual-0 blue-2 icoLit" />
                                    <span className="ico fch-Arrow-0-0 gray-2 icoLit-2 ico-b" />
                                    <span className="tittle-tab blue-2 tittle-we">Conceptos De la Cuenta</span>
                                </Grid>
                                <CardContent className={classes.cardContent}>

                                    <DialogContent className={classes.cardContent}>
                                        <Grid className="marginTablesPopup">
                                            <Grid className="container-17">
                                                <table className="table" >
                                                    <thead>

                                                        <th style={{ position: "sticky" }}>Concepto</th>
                                                        <th className="tab-space-01" style={{ position: "sticky" }}>Saldo</th>
                                                        <th className="tab-space-01" style={{ position: "sticky" }}>% Embargo</th>
                                                        <th className="tab-space-01" style={{ position: "sticky" }}>Valor Bloqueo/Embargo</th>
                                                        <th className="tab-space-01" style={{ position: "sticky" }}>Valor Reintegro</th>
                                                        <th className="tab-space-01" style={{ position: "sticky" }}>Saldo Disponible</th>
                                                        <th className="tab-space-01" style={{ position: "sticky" }}>N° registros</th>
                                                        <th className="tab-space-01" style={{ position: "sticky" }}>Acciones</th>
                                                    </thead>
                                                    <tbody>

                                                        {conceptosCuenta()?.map((account) => (
                                                            account.conceptos?.map((concepto, index) => {
                                                               
                                                                const numeroCuentaStr = account.numeroCuenta?.toString() || account.cuentaId?.toString() || '';

                                                                const valorEmb = valorEmbargo(String(concepto.cuentaId), concepto.conceptoId);

                                                                const claveBloqueoConcepto = `${numeroCuentaStr}-${concepto.id}`;

                                                                const valorBloqueoManual = bloqueosManuales[claveBloqueoConcepto] || 0;
                               
                                                                const valorReintegro = concepto.valorDevolver || 0;

                                                                
                                                                const saldoDisponibleActualizado = concepto.saldo - (valorEmb + valorBloqueoManual + valorReintegro);

                                                                return (
                                                                    <tr key={`table-${index}`}>
                                                                        <td data-label="Id Concepto">{concepto.descripcionConcepto}</td>
                                                                        <td data-label="Saldo">{UtilService.formatCurrency(concepto.saldo)}</td>
                                                                        <td data-label="% Embargo">{`${porcentajeEmbargo(String(concepto.cuentaId), concepto.conceptoId)}%`}</td>

                                                                        {/* Mostrar embargo + bloqueo manual */}
                                                                        <td data-label="Valor Bloqueo/Embargo">
                                                                            {UtilService.formatCurrency(valorEmb + valorBloqueoManual)}
                                                                        </td>

                                                                        <td data-label="Valor Reintegro">{UtilService.formatCurrency(valorReintegro)}</td>

                                                                        {/* NUEVO: Saldo Disponible Actualizado */}
                                                                        <td data-label="Saldo Disponible Actualizado">
                                                                            {UtilService.formatCurrency(saldoDisponibleActualizado)}
                                                                        </td>

                                                                        <td data-label="N° registros">{concepto.ingresos}</td>
                                                                        <td data-label="Detalle Concepto" className="actions">
                                                                            <Grid>
                                                                                <span aria-label="Detalle Concepto" onClick={handleEventDetalleConcepto(concepto.conceptoId, concepto.cuentaId, concepto.movimientos, concepto.descripcionConcepto)} >
                                                                                    <span className="ico fch-Excepciones-1-1 icoMid-3" />
                                                                                </span>
                                                                            </Grid>
                                                                        </td>
                                                                    </tr>
                                                                );
                                                            })
                                                        ))}
                                                        <tr>
                                                            <td><b>TOTAL SALDO CONCEPTOS</b></td>
                                                            <td><b>{UtilService.formatCurrency(totalDisponible())}</b></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </Grid>
                                        </Grid>

                                    </DialogContent>

                                </CardContent>

                            </Card>
                            <div className={classes.marginTops}>
                                {' '}
                            </div>
                            {showVisible && (
                                <Card >

                                    <Grid className="container-ico-t marginTopContainerPopup">
                                        <span className="ico fch-Puntual-0 blue-2 icoLit" />
                                        <span className="ico fch-Arrow-0-0 gray-2 icoLit-2 ico-b" />
                                        <span className="tittle-tab blue-2 tittle-we">Detalle Del Concepto : {nombreConceptoSelect}</span>
                                    </Grid>
                                    <CardContent className={classes.cardContent}>
                                        <DialogContent className={classes.cardContent}>
                                            <Grid className="marginTablesPopup">
                                                <Grid className="container-17">
                                                    <table className="table">
                                                        <thead>
                                                            <th className="tab-space-01">Id documento</th>
                                                            <th className="tab-space-01">Período</th>
                                                            <th className="tab-space-01">Concepto</th>
                                                            <th className="tab-space-01">Ingreso</th>
                                                            <th className="tab-space-01">Egreso</th>
                                                            <th className="tab-space-01">Fecha movimiento</th>
                                                        </thead>
                                                        <tbody>

                                                            {detalleConceptoFilter?.map((row, index) => (
                                                                <tr key={`table-${index}`}>
                                                                    <td data-label="Concepto Id" className="tab-space-01">{row.idDocumento}</td>
                                                                    <td data-label="Fecha Movimiento" className="tab-space-01">{row.periodoAno}</td>
                                                                    <td data-label="Fecha Proceso" className="tab-space-01">{nombreConceptoSelect}</td>
                                                                    <td data-label="Perioro" className="tab-space-01">{row.tipoMovimiento === 'C' ? UtilService.formatCurrency(row.valor) : null}</td>
                                                                    <td data-label="Valor" className="tab-space-01">{row.tipoMovimiento === 'D' ? UtilService.formatCurrency(row.valor) : null}</td>
                                                                    <td data-label="Tipo Movimiento" className="tab-space-01">{UtilService.getDate(row.fechaProceso)}</td>
                                                                </tr>
                                                            ))}
                                                        </tbody>
                                                    </table>
                                                </Grid>
                                            </Grid>

                                        </DialogContent>

                                    </CardContent>

                                </Card>
                            )}
                        </Grid>

                    </div>
                    <DialogActions className='maginButtonAddClose'>
                        <Button onClick={handleClose} variant="contained">
                            Cerrar
                        </Button>
                        { /* <Button onClick={handleClose} variant="contained">
                            Guardar
                                                            </Button> */ }
                    </DialogActions>
                </Dialog>
            </div>
        </>
    );
}


export default ModalDetalleDeCuenta;