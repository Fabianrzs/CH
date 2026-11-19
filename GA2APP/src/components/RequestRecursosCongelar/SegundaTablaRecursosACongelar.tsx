import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@material-ui/core";
import React from "react";
import UtilService from "../../utils/util.service";

const SegundaTablaRecursosACongelar = ({ crearMensaje, listadoRecursosCongeladosTablaDos }: { crearMensaje: any; listadoRecursosCongeladosTablaDos: any }) => {
    const ahorros = listadoRecursosCongeladosTablaDos.filter((item: any) =>
        ["AHORROS OBLIGATORIOS", "AHORROS RETROACTIVOS", "AHORROS VOLUNTARIOS", "INTERESES APORTES"].includes(item.coN_DESCRIPCION)
    );

    const cesantias = listadoRecursosCongeladosTablaDos.filter((item: any) =>
        ["CESANTÍAS", "CESANTÍAS RETROACTIVOS", "INTERESES CESANTÍAS"].includes(item.coN_DESCRIPCION)
    );

    return (
        <>
            <Typography style={{ color: 'black' }}>
                Se solicita el bloqueo de los recursos de la cuenta individual, según el porcentaje relacionado y los valores actualizados a la fecha, dispuestos en la siguiente tabla:
            </Typography>
            <br />
            <br />

            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow style={{ backgroundColor: '#1C3F8B' }}>
                            <TableCell style={{ color: 'white' }}>Descripción</TableCell>
                            <TableCell style={{ color: 'white' }} align="right">Valor</TableCell>
                            <TableCell style={{ color: 'white' }} align="right">Porcentaje</TableCell>
                            <TableCell style={{ color: 'white' }} align="right">Valor Calculado</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {/* Ahorros Subtitle */}
                        <TableRow>
                            <TableCell colSpan={4}>
                                <Typography variant="h6" style={{ fontWeight: 'bold', color: '#1C3F8B' }}>Ahorros</Typography>
                            </TableCell>
                        </TableRow>
                        {ahorros.map((item: any) => (
                            <TableRow key={item.coN_ID}>
                                <TableCell>{item.coN_DESCRIPCION}</TableCell>
                                <TableCell align="right">{UtilService.formatCurrencyTres(item.coN_VALOR)}</TableCell>
                                <TableCell align="right">{item.coN_PORCENTAJE} %</TableCell>
                                <TableCell align="right">{UtilService.formatCurrencyTres(item.coN_VALOR_FORMAT)}</TableCell>
                            </TableRow>
                        ))}

                        {/* Cesantías Subtitle */}
                        <TableRow>
                            <TableCell colSpan={4}>
                                <Typography variant="h6" style={{ fontWeight: 'bold', color: '#1C3F8B' }}>Cesantías</Typography>
                            </TableCell>
                        </TableRow>
                        
                        {cesantias.map((item: any) => (
                            <TableRow key={item.coN_ID}>
                                <TableCell>{item.coN_DESCRIPCION}</TableCell>
                                <TableCell align="right">{UtilService.formatCurrencyTres(item.coN_VALOR)}</TableCell>
                                <TableCell align="right">{item.coN_PORCENTAJE} %</TableCell>
                                <TableCell align="right">{UtilService.formatCurrencyTres(item.coN_VALOR_FORMAT)}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}

export default SegundaTablaRecursosACongelar;
