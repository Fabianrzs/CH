import React, { useEffect, useState } from 'react';
import { IconButton } from '@material-ui/core';
import { AddCircle } from '@material-ui/icons';
import ConceptosTable from './ConceptosTable';
import UtilService from '../../../utils/util.service';
import {
    TipoEmbargoDto,
    TipoRetencionDto,
    CuentaModelDto,
    ConceptoEmbargosDto,
} from '../../../types/embargo.model.type';

const Cuentas = (props: props) => {
    const {
        listCuentas,
        cuentas,
        tiposEmbargos,
        tiposRetencion,
        handleOpenModal,
        handleCloseModal,
        listaConceptos,
        setListaConceptos,
        conceptosData,
        modoActualizar,
        porcentaje,
        variacionPorcentaje,
        handleOnChangeMostrarPorcentaje,
        setCuentas,
        estadoCuentaEmbargo,
        setConcepto,
        expanded, 
        onToggleExpand,
        soloVisual,
        saldoConcepto,
    } = props;
    const [showConcept, setShowConcept] = useState(false);
    const [totalEmbargos, setTotalEmbargos] = useState(0)
    

    const handleClick = () => {
        setShowConcept(!showConcept);
    };

    const handleTotalEmbargos = (totalEmbargosMetodo: any) => {
        setTotalEmbargos(totalEmbargosMetodo)
    }

    useEffect(() => {
        let total = 0;
        conceptosData?.forEach((element, index) => {
            if (
                element.tipoEmbargoId === porcentaje ||
                element.tipoEmbargoId === variacionPorcentaje
            ) {
                const porcentajeValor = (cuentas.conceptos.find(x => x.id === element.conceptoId)?.saldo || 0) * ((element.valor || 0) / 100);
                total += porcentajeValor;
            } else {
                total += element.valor || 0;
            }
        })

        setTotalEmbargos(total)
    }, [])



    return (
        <>
            <div>
                
                <span style={{ marginLeft: '15px' }}>
                    <b> CUENTA: </b>
                </span>
                {cuentas.nombreCuenta}
                <span>
                    <b> NÚMERO CUENTA: </b>
                </span>
                {cuentas.numeroCuenta}
                <span>
                    <b> SALDO: </b>
                </span>
                {UtilService.formatCurrency(cuentas.saldo)}
                <span>
                    <b> ESTADO DE CUENTA:{cuentas.estadoCuenta} </b>
                </span>
                {estadoCuentaEmbargo && estadoCuentaEmbargo.estadoCuenta} 
               
                <IconButton onClick={onToggleExpand}>
                    <AddCircle />
                </IconButton>
            </div>
            <div>
                {expanded  ? (
                    <ConceptosTable
                        numeroCuenta={cuentas.numeroCuenta}
                        conceptos={cuentas.conceptos}
                        handleOpenModal={handleOpenModal}
                        handleCloseModal={handleCloseModal}
                        listaConceptos={listaConceptos}
                        setListaConceptos={setListaConceptos}
                        tiposEmbargos={tiposEmbargos}
                        tiposRetencion={tiposRetencion}
                        conceptosData={conceptosData}
                        modoActualizar={modoActualizar}
                        porcentaje={porcentaje}
                        variacionPorcentaje={variacionPorcentaje}
                        handleOnChangeMostrarPorcentaje={handleOnChangeMostrarPorcentaje}
                        handleTotalEmbargos={handleTotalEmbargos}
                        saldos={cuentas.saldo - totalEmbargos}
                        setConcepto={setConcepto}
                        estadoCuentaEmbargo={estadoCuentaEmbargo}
                        soloVisual={soloVisual}

                    />
                ) : null}
            </div>
        </>
    );
};

interface LinkMapDispatchToProps {
    listCuentas: CuentaModelDto[];
    cuentas: CuentaModelDto;
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
    setCuentas: (data: any) => void;
    estadoCuentaEmbargo: any;
    setConcepto: ConceptoEmbargosDto[];
    expanded: boolean;
    onToggleExpand: () => void;
    soloVisual: boolean;
    saldoConcepto: any;
}

type props = LinkMapDispatchToProps;

export default Cuentas;
