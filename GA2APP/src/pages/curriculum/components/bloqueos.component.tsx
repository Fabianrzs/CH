import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Grid, makeStyles, createStyles, Typography, Button, } from '@material-ui/core';
import { toast } from 'react-toastify';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { ClienteDto } from '../../../types/client.model.type';
import UtilService from '../../../utils/util.service';
import AuthService from '../../../services/auth.service';
import ApiReportes from '../../../services/api-reportes.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'right'
        },
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        titleCampo: {
            fontSize: '10px',
        },
        restoreButton: {
            width: '250px',
            marginRight: '.5rem',
            background: '#1C3F8B',
            color: '#FFFFFF',
        },
    }),
);

function Bloqueos({
    catalogState,
    clientModel,
}: {
    catalogState: CatalogState;
    clientModel: ClienteDto;
}) {

    const [bloqueos, setBloqueos] = useState<boolean>(false);
    const classes = useStyles();
    const apiReportes = ApiReportes();
    useEffect(() => {
        const validateBloqueos = () => {
            if (clientModel?.cuentas?.length !== 0) {
                clientModel?.cuentas?.forEach((element, index) => {
                    if (element.conceptosSaldosBloqueos?.length !== 0) {
                        setBloqueos(true)
                    }
                })
            }
        }

        validateBloqueos()
    });

    const onClickExcel = async () => {

        const columnDataList =
            clientModel?.cuentas?.flatMap((cuenta) =>
                cuenta.conceptosSaldosBloqueos?.flatMap((bloqueo: any) => {
                    if (bloqueo.valorAcumulado > 0 && bloqueo.idCuenta > 0) {
                        return [{
                            data: [
                                cuenta.numeroCuenta,
                                cuenta.numeroCuenta,
                                "PROCESADA",
                                bloqueo.estadoBloqueo,
                                bloqueo.concepto,
                                cuenta.numeroCuenta,
                                "",
                                UtilService.formatCurrency(bloqueo.valorAcumulado),
                            ].map((v) => String(v ?? "")),
                        }];
                    }
                    return [];
                }) ?? []
            ) ?? [];

        const request = {
            columnDataList: columnDataList.map((item: any) => ({
                data: item.data
            })),
            columnName: [
                "Radicado",
                "Solicitud",
                "Estado Solicitud",
                "Estado Bloqueo",
                "Demandante",
                "Cuenta",
                "Razón",
                "Monto/Porcentaje"
            ]
        };

        const apiReportes = ApiReportes();

        try {
            const response = await apiReportes.GenerarExcels(request);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `Bloqueos-${clientModel?.identificacion}.xlsx`;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);

        } catch (error) {
            toast('Error al descargar el reporte', { type: toast.TYPE.ERROR });
        }
    }

    return (
        <CHPaper title="Bloqueos">
            <Typography className={classes.titleform} variant="h5" component="h6">
                Bloqueos
            </Typography>
    
            {bloqueos ? (
                <table className="table">
                    <thead>
                        <tr>
                            <th>Radicado</th>
                            <th>Solicitud</th>
                            <th>Estado solicitud</th>
                            <th>Estado bloqueo</th>
                            <th>Demandante</th>
                            <th>Cuenta</th>
                            <th>Razón</th>
                            <th>Monto/Porcentaje</th>
                        </tr>
                    </thead>
    
                    <tbody>
                        {clientModel?.cuentas?.map((element) =>
                            element.conceptosSaldosBloqueos
                                ?.filter(
                                    (bloqueo: any) =>
                                        bloqueo.valorAcumulado > 0 &&
                                        bloqueo.idCuenta > 0 &&
                                        bloqueo.razon !== 'EMBARGO JUDICIAL'
                                )
                                .map((elementBloqueos: any, index1: number) => (
                                    <tr key={index1}>
                                        <td className="dateReport3">{elementBloqueos.nroExpd}</td>
                                        <td className="dateReport3">{elementBloqueos.solicitud}</td>
                                        <td className="dateReport3">PROCESADA</td>
                                        <td className="dateReport3">{elementBloqueos.estadoBloqueo}</td>
                                        <td className="dateReport3">{elementBloqueos.demandante}</td>
                                        <td className="dateReport3">{elementBloqueos.tipo_cuenta}</td>
                                        <td className="dateReport3">{elementBloqueos.razon}</td>
                                        <td className="dateReport3">
                                            {UtilService.formatCurrency(elementBloqueos.valorAcumulado)}
                                        </td>
                                    </tr>
                                ))
                        )}
                    </tbody>
                </table>
            ) : (
                'El afiliado no cuenta con bloqueos'
            )}
    
            <br />
    
            <Grid className={classes.column} item xs={12} md={12}>
                <Button onClick={() => onClickExcel()} className={classes.restoreButton}>
                    Generar Excel
                </Button>
            </Grid>
        </CHPaper>
    );
}


export default connect()(Bloqueos);