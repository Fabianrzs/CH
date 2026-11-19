import React, { useEffect, useRef, useState } from 'react';
import {
    createStyles,
    makeStyles,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';
import {
    ConceptoEmbargosDto,
    CuentaModelDto,
    EmbargoDto,
} from '../../../types/embargo.model.type';
import CHPaper from '../../../components/ch-paper/CHPaper';
import UtilService from '../../../utils/util.service';
import apiClient from '../../../services/api-clients.service';

import { embargoForm } from '../../embargos/constants/embargosform.model.type';
import apiEmbargos from '../../../services/api-embargos.service';




const Expedientes = (props: props) => {

    const {
        embargoModel,
        cuentas,
        listaConceptos,
        onValorCalculado
    } = props;

    const porcentaje = 'ae7d141a-fd88-4f57-a199-4db5b5301946';
    const variacionPorcentaje = 'f65c4c4a-174e-4406-8aed-32b5b47cf0b3';
    const apiCliente = apiClient();
    const apiEmbargo = apiEmbargos();
    const formularioEmbargos = useForm<EmbargoDto>({
        defaultValues: embargoForm,
    });


    const [concepto, setConcepto] = useState([]);
    const [loading, setLoading] = useState(false);

    const consultarCliente = async (idTipoIdentificacion: number, identificacion: string) => {
        if (idTipoIdentificacion) {
            await apiCliente
                .getClientByIdentificationTypeIdentificationNumberRequest(idTipoIdentificacion, identificacion)
                .then(async (response) => {
                    if (response.data != null) {
                        formularioEmbargos.setValue(
                            'clienteNombre',
                            `${response.data.PrimerNombre} ${response.data.SegundoNombre} ${response.data.PrimerApellido} ${response.data.SegundoApellido}`
                        );

                        setEstadoCuentaEmbargo(response.data.cuentas);
                    }
                })
                .catch(() => {
                    toast('Error al consultar el cliente.');
                });

            await apiEmbargo.getObtenerEmbargosByIdentificacion(identificacion)
                .then(async (response) => {
                    if (response.data != null) {
                        const allConceptos = [];
                        response.data.forEach((item) => {
                            if (item.embargo && item.embargo.remanente === false) {
                                allConceptos.push(...(item.conceptos || []));
                            }
                        });
                        setConcepto(allConceptos);
                    }
                })
                .catch(() => {
                    toast('Error al consultar los embargos.');
                });

            setLoading(false);
        } else {
            toast('Ingrese el tipo de documento');
        }
    };

    useEffect(() => {
        const { tipoIdentificacionId, identificacionCliente } = embargoModel?.embargo || {};

        if (tipoIdentificacionId && identificacionCliente) {
            consultarCliente(tipoIdentificacionId, identificacionCliente);
        }
    }, [embargoModel?.embargo?.tipoIdentificacionId, embargoModel?.embargo?.identificacionCliente]);

    interface Movimiento {
        cnC_ID: string;
        mvT_FECHA_PROCESO: string;
        mvT_VALOR: number;
    }

    interface EstadoCuenta {
        movimientos: Movimiento[];
    }

    const [estadoCuentaEmbargo, setEstadoCuentaEmbargo] = useState<EstadoCuenta[]>([]);


    const valorAplicandoPorcentaje = (concepto: any): number => {
        const porcentaje = Number(concepto?.valor || 0);
        const conceptoId = Number(concepto?.conceptoId);
        const cuentaId = Number(concepto?.cuentaId);

        const cuenta = cuentas.find(c => Number(c.cuentaId) === cuentaId);
        const conceptoEnCuenta = cuenta?.conceptos?.find(c => Number(c.id) === conceptoId);

        const saldo = Number(conceptoEnCuenta?.saldo || 0);

        if (concepto?.tipoEmbargo === "PORCENTAJE" && saldo > 0 && porcentaje > 0) {
            return saldo * (porcentaje / 100);
        }

        return 0;
    };


    useEffect(() => {
        if (
            embargoModel?.conceptos?.length > 0 &&
            cuentas?.length > 0 &&
            listaConceptos?.length > 0
        ) {
            const concepto = embargoModel.conceptos[0];

            const esPorcentaje =
                concepto.tipoEmbargoId === porcentaje ||
                concepto.tipoEmbargoId === variacionPorcentaje;

            const valorCalculado = esPorcentaje
                ? valorAplicandoPorcentaje(concepto)
                : concepto?.valor || 0;

            console.log('valorCalculado', valorCalculado);



            // onValorCalculado(UtilService.formatCurrency(valorCalculado));
            onValorCalculado(valorCalculado);

        }
    }, [embargoModel, cuentas, listaConceptos]);

    const conceptoData = embargoModel.conceptos[0];

    const esPorcentaje = conceptoData?.tipoEmbargoId === porcentaje || conceptoData?.tipoEmbargoId === variacionPorcentaje;

    const valorCalculado = esPorcentaje
        ? valorAplicandoPorcentaje(conceptoData)
        : conceptoData?.valor || 0;


    const valorMostrado = UtilService.formatCurrency(valorCalculado);
    onValorCalculado(valorCalculado);

    const cuentaEncontrada = cuentas.find(option =>
        option.conceptos?.some(concepto => concepto.id === embargoModel.conceptos[0]?.conceptoId)
    );

    return (
        <CHPaper title="Expedientes">
            <Table aria-label="simple table">
                <TableHead>
                    <TableRow>
                        <TableCell>No. Expediente</TableCell>
                        <TableCell>Razon Social</TableCell>
                        <TableCell>Concepto</TableCell>
                        <TableCell>Valor Acumulado</TableCell>
                        <TableCell>Valor a pagar</TableCell>
                        <TableCell>Porcentaje</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {embargoModel && embargoModel.embargo && embargoModel.conceptos && embargoModel.conceptos[0] && cuentas && (
                        <TableRow key={embargoModel.embargo.id}>
                            <TableCell>{embargoModel.embargo.expedienteBancoAgrario}</TableCell>
                            <TableCell>{embargoModel.embargo.juzgado}</TableCell>
                            <TableCell>{embargoModel.conceptos[0]?.tipoEmbargo}</TableCell>
                            {/* <TableCell>
                                {
                                    cuentas.find(option => embargoModel.conceptos[0]?.conceptoId === option.conceptos[0]?.id)
                                        ?.conceptos[0]?.saldo !== undefined
                                        ? UtilService.formatCurrency(
                                            cuentas.find(option => embargoModel.conceptos[0]?.conceptoId === option.conceptos[0]?.id)
                                                ?.conceptos[0]?.saldo || 0
                                        )
                                        : '-'
                                }
                            </TableCell> */}

                            <TableCell>
                                {
                                    cuentaEncontrada
                                        ? UtilService.formatCurrency(
                                            cuentaEncontrada.conceptos.find(c => c.id === embargoModel.conceptos[0]?.conceptoId)?.saldo || 0
                                        )
                                        : '-'
                                }
                            </TableCell>
                            <TableCell>
                                {valorMostrado}
                            </TableCell>

                            <TableCell>
                                {embargoModel.conceptos[0]?.tipoEmbargo === 'PORCENTAJE'
                                    ? `${embargoModel.conceptos[0]?.valor}%`
                                    : ''}
                            </TableCell>
                        </TableRow>
                    )}
                </TableBody>
            </Table>
        </CHPaper>
    )
};

interface LinkMapDispatchToProps {
    embargoModel: {
        embargo: {
            id: string;
            expedienteBancoAgrario: string;
            nombreDemandado: string;
            juzgado: string;
            tipoIdentificacionId: any;
            identificacionCliente: any;
        };
        beneficiarios: Array<{
            id: string;
        }>;
        conceptos: Array<{
            tipoEmbargoId: string;
            id: string;
            tipoEmbargo: string;
            valor: number;
            saldo: number;
            conceptoId: any;
        }>
    };
    cuentas: CuentaModelDto[];
    onValorCalculado: (valor: any) => void;
    listaConceptos: ConceptoEmbargosDto[];
}
type props = LinkMapDispatchToProps;

export default Expedientes;