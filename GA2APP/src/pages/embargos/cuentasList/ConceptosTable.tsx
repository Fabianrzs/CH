import React, { useEffect, useRef, useState } from 'react';
import {
    IconButton,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import { Edit } from '@material-ui/icons';
import { Controller, useForm } from 'react-hook-form';
import UtilService from '../../../utils/util.service';
import {
    ConceptoEmbargosDto,
    ConceptoModelDto,
    TipoEmbargoDto,
    TipoRetencionDto,
    CuentaModelDto,
} from '../../../types/embargo.model.type';
import { convertToConceptos } from '../constants/conversionesConceptosdeCuentas';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { Movimiento } from '../../../types/client.model.type';


const ConceptosTable = (props: props) => {
    const {
        conceptos,
        numeroCuenta,
        tiposEmbargos,
        tiposRetencion,
        handleOpenModal,
        handleCloseModal,
        listaConceptos,
        conceptosData,
        modoActualizar,
        setListaConceptos,
        porcentaje,
        variacionPorcentaje,
        handleOnChangeMostrarPorcentaje,
        handleTotalEmbargos,
        saldos,
        setConcepto,
        estadoCuentaEmbargo,
        soloVisual,
        saldoConcepto,

    } = props;

    useEffect(() => {
        console.log('listaConceptos', listaConceptos);

    }, [listaConceptos]);
    const nuevaListaConceptos = async () => {
        const array = await conceptos.map(item =>
            convertToConceptos(
                item.cuentaId,
                item.id,
                item.conceptoDescripcion,
                item.saldo,
                numeroCuenta,
            ),
        );

        if (modoActualizar) {
            conceptosData.forEach(value => {
                const index = array.findIndex(
                    x => x.conceptoId === value.conceptoId,
                );
                if (index !== -1) {
                    value.saldo = array[index].saldo;
                    value.conceptoDescripcion =
                        array[index].conceptoDescripcion;
                    array[index] = value;
                    array[index].insert = false;
                }
            });
        }
        await setListaConceptos(array);
    };

    const toastShown = useRef({
        estadoCuenta: false,
        concepto: false
    });

    const yaNotificoEstadoCuenta = useRef(false);


    const isEmbargoPorcentaje = (item: ConceptoEmbargosDto): boolean =>
        item.tipoEmbargoId === porcentaje || item.tipoEmbargoId === variacionPorcentaje;


    const valorAplicandoPorcentaje = (item: ConceptoEmbargosDto): number => {
        if (!item || item.valor == null || !item.saldo) return 0;

    
        if (!isEmbargoPorcentaje(item)) {
            console.warn(`No aplica porcentaje: ${item.conceptoDescripcion}, tipoEmbargoId: ${item.tipoEmbargoId}`);
            return 0;
        }
    
        const porcentajeDecimal = item.valor / 100;
    
        if (item.tipoRetencionId !== "aae6c046-06ad-40c3-8fd5-9776eddc12e8") {
            return item.saldo * porcentajeDecimal;
        }
    
        const concep = conceptos?.find(c => c.id === item.conceptoId);
        if (!concep) {
            console.warn("No se encontró concepto asociado");
            return item.saldo * porcentajeDecimal;
        }
    
        const fechaInicio = item.fechaInicio ? new Date(item.fechaInicio) : null;
        if (!fechaInicio) return item.saldo * porcentajeDecimal;
    
        fechaInicio.setHours(0, 0, 0, 0);
    
        if (!Array.isArray(estadoCuentaEmbargo) || estadoCuentaEmbargo.length === 0) {
            console.warn("No hay estadoCuentaEmbargo");
            return item.saldo * porcentajeDecimal;
        }
    
        const movimientos = estadoCuentaEmbargo[0].movimientos?.filter((mov: any) =>
            mov.cnC_ID === concep.conceptoId &&
            new Date(mov.mvT_FECHA_PROCESO) >= fechaInicio
        ) ?? [];
    
        if (movimientos.length === 0) {
            console.warn("Sin movimientos: se aplica sobre saldo directamente.");
            return item.saldo * porcentajeDecimal;
        }
    
        const total = movimientos.reduce(
            (acc: number, mov: any) => acc + (mov.mvT_VALOR > 0 ? mov.mvT_VALOR * porcentajeDecimal : 0),
            0
        );
        return total;
    };
    
    

    const [valorTotalCuenta, setValorTotalCuenta] = useState(0);


    const obtenerValorRetenido = (concepto: any): number => {


        if (!concepto.tipoEmbargoId) return 0;

        if (isEmbargoPorcentaje(concepto)) {
            const valor = valorAplicandoPorcentaje(concepto);
            return valor;
        }
    
        const valor = concepto.valor || 0;
        return valor;
    };

    const valorTotal = (conceptosList: any) => {
        let total = 0;

        conceptosList?.forEach((item: any) => {
            total += obtenerValorRetenido(item);
        });

        handleTotalEmbargos(total);
        return total;
    };



    const [total, setTotal] = useState(0);

    const calcularNuevoValor = (concepto: any) => {
        let valorRetenido = 0;

        if (concepto.tipoEmbargoId === porcentaje || concepto.tipoEmbargoId === variacionPorcentaje) {
            valorRetenido = valorAplicandoPorcentaje(concepto);
        } else {
            valorRetenido = concepto.valor || 0;
        }

        return concepto.saldo - valorRetenido;
    };

    useEffect(() => {
        const totalRetenido = valorTotal(listaConceptos);
        setTotal(totalRetenido);
    
        const totalSaldos = listaConceptos.reduce((sum, c) => sum + c.saldo, 0);
        const disponible = totalSaldos - totalRetenido;
    
        setValorTotalCuenta(disponible);
    }, [listaConceptos, porcentaje, variacionPorcentaje]);
    



    useEffect(() => {
        nuevaListaConceptos();
    }, []);



    const [selectedAll, setSelectedAll] = useState(false);
    const [selectedConceptos, setSelectedConceptos] = useState<any[]>([]);

    const handleSelectAll = () => {
        const allSelected = listaConceptos.every(concepto => selectedConceptos.includes(concepto.id));

        if (!allSelected) {
            setSelectedConceptos(listaConceptos.map(concepto => concepto.id));
            setSelectedAll(true);
        } else {
            setSelectedConceptos([]);
            setSelectedAll(false);
        }
    };

    const handleCheckboxChange = (conceptoId: string) => {
        const updatedSelectedConceptos = [...selectedConceptos];

        if (updatedSelectedConceptos.includes(conceptoId)) {
            const index = updatedSelectedConceptos.indexOf(conceptoId);
            if (index !== -1) {
                updatedSelectedConceptos.splice(index, 1);
            }
        } else {
            updatedSelectedConceptos.push(conceptoId);
        }
        setSelectedConceptos(updatedSelectedConceptos);

    };

    const handleApplyAction = () => {
        if (selectedConceptos.length === 0) {
            console.log('No se han seleccionado conceptos.');
            return;
        }

        const conceptosAbiertos = new Set();

        selectedConceptos.forEach((conceptoId, index) => {

            const concepto = listaConceptos.find((concepto) => concepto.id === conceptoId);

            if (concepto && !conceptosAbiertos.has(conceptoId)) {
                handleOpenModal(concepto.conceptoId);
                conceptosAbiertos.add(conceptoId);

                if (index === selectedConceptos.length - 1) {
                    handleCloseModal();

                }
            }
        });
    };

    // const resultadoSaldo = saldos - valorTotal();
    const { control, handleSubmit } = useForm({
        defaultValues: {
            valor: 0
        }
    })

    let totalSum = 0;
    let valorSolicitado = 0;
    let valorDisponible = 0;
    const obtenerValorReintegro = (conceptoId: number) => {
        const movimientos = estadoCuentaEmbargo[0]?.movimientos.filter((item: any) => item.cnC_ID === conceptoId);
        if (movimientos !== undefined && movimientos.length > 0) {
            movimientos?.forEach((item: any) => {
                if (item.mvT_VALOR > 0) {
                    valorSolicitado += item.mvT_VALOR;
                }
            })
        }
        return valorSolicitado;
    }


    return (
        <Table
            className="client-table"
            style={{
                margin: '20px',
                width: '95%',
            }}
        >
            <TableHead>
                <TableRow>
                    <TableCell style={{ textAlign: 'center' }}>
                        Seleccionar todos
                        <CHCheckbox
                            color='primary'
                            disabled={soloVisual}
                            checked={selectedAll}
                            onChange={handleSelectAll}
                            label=''
                            indeterminate={selectedConceptos.length > 0 && selectedConceptos.length < listaConceptos.length}
                        />
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Concepto
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>Saldo</TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Valor retenido
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Porcentaje
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>Saldo Disponible</TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Fecha Inicio
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Fecha Fin
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Tipo Retención
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Condicionado por retiro
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Acciones
                    </TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {listaConceptos.map((concepto) => {
                    totalSum += concepto.saldo;
                    valorDisponible = totalSum - valorTotalCuenta;
                    return (

                        <TableRow key={concepto.conceptoId}>
                            <TableCell>
                                <CHCheckbox
                                    color='primary'
                                    disabled={soloVisual}
                                    checked={selectedConceptos.includes(concepto.id)}
                                    onChange={() => handleCheckboxChange(concepto.id)}
                                    label=''
                                />
                            </TableCell>
                            <TableCell>{concepto.conceptoDescripcion}</TableCell>
                            <TableCell>
                                {UtilService.formatCurrency(concepto.saldo)}
                            </TableCell>
                            <TableCell>
                                {UtilService.formatCurrency(obtenerValorRetenido(concepto))}
                            </TableCell>


                            <TableCell>
                                {concepto.tipoEmbargoId === porcentaje ||
                                    concepto.tipoEmbargoId === variacionPorcentaje
                                    ? `${concepto.valor}%`
                                    : ''}
                            </TableCell>
                            <TableCell>
                                {UtilService.formatCurrency(
                                    calcularNuevoValor(concepto)
                                )
                                }
                            </TableCell>
                            <TableCell>
                                {UtilService.getDate(concepto.fechaInicio)}
                            </TableCell>
                            <TableCell>
                                {concepto.fechaFin && concepto.fechaFin.toString().startsWith("0001-01-01")
                                    ? '-'
                                    : UtilService.getDate(concepto.fechaFin)}
                            </TableCell>
                            <TableCell>
                                {tiposRetencion.map(option => {
                                    if (option.id === concepto.tipoRetencionId) {
                                        return <span>{option.descripcion}</span>;
                                    }
                                    return '';
                                })}
                            </TableCell>
                            <TableCell>
                                {concepto.indicadorCesantias ? 'SI' : 'NO'}
                            </TableCell>
                            <TableCell>
                                <IconButton
                                    aria-label="Editar"
                                    color="primary"
                                    disabled={soloVisual}
                                    onClick={() => {
                                        if (
                                            concepto.tipoEmbargoId === porcentaje ||
                                            concepto.tipoEmbargoId ===
                                            variacionPorcentaje
                                        ) {
                                            handleOnChangeMostrarPorcentaje(true);
                                        } else {
                                            handleOnChangeMostrarPorcentaje(false);
                                        }
                                        handleOpenModal(concepto.conceptoId);
                                    }}
                                >
                                    <Edit />
                                </IconButton>
                            </TableCell>
                        </TableRow>
                    )
                })}
                <TableRow>
                    <TableCell colSpan={2}>
                        <b>TOTAL</b>
                    </TableCell>
                    <TableCell colSpan={1} style={{ textAlign: 'center' }}>
                        <b>{UtilService.formatCurrency(totalSum)}</b>
                    </TableCell>
                    <TableCell colSpan={1}>
                        <b>{UtilService.formatCurrency(total)}</b>
                    </TableCell>
                    <TableCell colSpan={1} style={{ textAlign: 'center' }}>
                        <b>
                            { }
                        </b>
                    </TableCell>

                    <TableCell colSpan={2}>
                        <b>SALDO DISPONIBLE</b>
                    </TableCell>
                    <TableCell colSpan={1} style={{ textAlign: 'center' }}>
                        <b>{UtilService.formatCurrency(valorTotalCuenta)}</b>
                    </TableCell>
                    <TableCell colSpan={2}>
                        <b>EMBARGO MASIVO</b>
                    </TableCell>
                    <TableCell>
                        <IconButton
                            aria-label="Editar"
                            color="secondary"
                            onClick={() => handleOpenModal(1)}
                            disabled={selectedConceptos.length === 0}
                        >
                            <Edit />
                        </IconButton>
                    </TableCell>

                </TableRow>
            </TableBody>
        </Table>
    );
};

interface LinkMapDispatchToProps {

    conceptos: ConceptoModelDto[];
    numeroCuenta: number;
    handleOpenModal: (conceptoId: number) => void;
    handleCloseModal: any;
    listaConceptos: ConceptoEmbargosDto[];
    setListaConceptos: (listaConceptos: ConceptoEmbargosDto[]) => void;
    tiposEmbargos: TipoEmbargoDto[];
    tiposRetencion: TipoRetencionDto[];
    conceptosData: ConceptoEmbargosDto[];
    modoActualizar: boolean;
    porcentaje: string;
    variacionPorcentaje: string;
    handleOnChangeMostrarPorcentaje: (value: boolean) => void;
    handleTotalEmbargos: (totalEmbargos: any) => void;
    saldos: number;
    setConcepto: ConceptoEmbargosDto[];
    estadoCuentaEmbargo: any;
    soloVisual: boolean;
    saldoConcepto: any;
}

type props = LinkMapDispatchToProps;

export default ConceptosTable;
