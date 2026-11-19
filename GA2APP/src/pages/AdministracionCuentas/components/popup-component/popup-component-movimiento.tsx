import React from 'react';
import {
    Dialog,
    DialogContent, Typography, Button, makeStyles, createStyles, DialogActions,
    Grid, CardContent, Card
} from '@material-ui/core';

import { AdminCuentasAfiliado } from '../../../../types/client.model.type';
import UtilService from '../../../../utils/util.service';

const concepto = [
    {id: 1	, descripcion: 'AHORROS OBLIGATORIOS'},
    {id: 2	, descripcion: 'AHORROS VOLUNTARIOS'},
    {id: 3	, descripcion: 'CESANTÍAS'},
    {id: 4	, descripcion: 'AHORROS RETROACTIVOS'},
    {id: 5	, descripcion: 'CESANTÍAS RETROACTIVOS'},
    {id: 6	, descripcion: 'FONDO SOLIDARIDAD'},
    {id: 7	, descripcion: 'CONCILIACIONES'},
    {id: 8	, descripcion: 'POR LEGALIZAR'},
    {id: 9	, descripcion: 'INTERESES APORTES'},
    {id: 10	, descripcion: 'INTERESES CESANTÍAS'},
    {id: 11	, descripcion: 'SUBSIDIO'},
    {id: 12	, descripcion: 'EXCEDENTES FINANCIEROS'},
    {id: 13	, descripcion: 'COMPENSACIONES'},
    {id: 14	, descripcion: 'PROVISIÓN'},
    {id: 15	, descripcion: 'CESANTÍAS DOCEAVAS'},
    {id: 16	, descripcion: 'RETROACTIVOS FONDO SOLIDARIDAD'},
    {id: 17	, descripcion: 'RENDIMIENTOS CESANTÍAS'},
    {id: 19	, descripcion: 'CI MENOR CUANTIA'},
    {id: 22	, descripcion: 'FONDO NUEVAS CONVOCATORIAS'},
    {id: 23	, descripcion: 'AHORRO AVAC'},
    {id: 24	, descripcion: 'INTERESES AHORRO AVAC'},
]

function ModalMoviemientoDeCuenta({
    responseAdminCuenta,
    handleClose,
    open,
    cuentaId
}: {
    responseAdminCuenta: AdminCuentasAfiliado;
    handleClose: any;
    open: any;
    cuentaId: any;
}) {

    const useStyles = makeStyles(() =>
        createStyles({
            dialogPaper: {
                width: '60%',
                height: '50%'
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


    const conceptosCuenta = () => {
        const filter = responseAdminCuenta.cuentas.filter(
            (x) =>
                x.cuentaId.toString().includes(cuentaId)
        );
        return filter;
    };


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
                            Movimientos De La Cuenta
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
                                    <span className="tittle-tab blue-2 tittle-we">Detalle De Los Movimientos</span>
                                </Grid>
                                <CardContent className={classes.cardContent}>

                                    <DialogContent className={classes.cardContent}>
                                        <Grid className="marginTablesPopup">

                                            <Grid>
                                                <input type="search" className='floatRightButton' placeholder="Buscar.." />
                                            </Grid>

                                            <Grid className="container-17">
                                                <table className="table">
                                                    <thead>
                                                        <th className="tab-space-01">Id documento</th>
                                                        <th className="tab-space-01">Período</th>
                                                        <th className="tab-space-01">Concepto</th>
                                                        <th className="tab-space-01">Ingreso</th>
                                                        <th className="tab-space-01">Egreso</th>
                                                        <th className="tab-space-01">Fecha proceso</th>
                                                    </thead>
                                                    <tbody>

                                                        {conceptosCuenta()?.map((row) => (
                                                            row.movimientos?.map((row, index) => (
                                                                <tr key={`table-${index}`}>
                                                                    <td data-label="Concepto Id" className="tab-space-01">{row.documentoId}</td>
                                                                    <td data-label="Fecha Movimiento" className="tab-space-01">{row.periodoAno}</td>
                                                                    <td data-label="Fecha Proceso" className="tab-space-01">{concepto.find(x => x.id === row.conceptoId)?.descripcion}</td>
                                                                    <td data-label="Perioro" className="tab-space-01">{row.tipoMovimiento === 'C' ? UtilService.formatCurrency(row.valor): null}</td>
                                                                    <td data-label="Valor" className="tab-space-01">{row.tipoMovimiento === 'D' ? UtilService.formatCurrency(row.valor): null}</td>
                                                                    <td data-label="Tipo Movimiento" className="tab-space-01">{UtilService.getDate(row.fechaProceso)}</td>
                                                                </tr>
                                                            ))
                                                        ))}
                                                    </tbody>
                                                </table>
                                            </Grid>
                                        </Grid>

                                    </DialogContent>

                                </CardContent>

                            </Card>
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


export default ModalMoviemientoDeCuenta;