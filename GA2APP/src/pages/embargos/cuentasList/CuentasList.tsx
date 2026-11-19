import React, { useEffect, useState } from 'react';
import { Card } from '@material-ui/core';
import Cuentas from './Cuentas';
import {
    ConceptoEmbargosDto,
    TipoEmbargoDto,
    TipoRetencionDto,
    CuentaModelDto,
    ConceptoModelDto,
} from '../../../types/embargo.model.type';

const CuentasList = (props: props) => {
    const {
        cuentas,
        tiposEmbargos,
        tiposRetencion,
        listaConceptos,
        conceptosData,
        modoActualizar,
        porcentaje,
        variacionPorcentaje,
        handleOpenModal,
        handleCloseModal,
        setListaConceptos,
        handleOnChangeMostrarPorcentaje,
        setCuentas,
        estadoCuentaEmbargo,
        setConcepto,
        soloVisual,
        saldoConcepto
    } = props;

    const [expandedAccountId, setExpandedAccountId] = useState<number | null>(null);
    const handleToggleExpand = (cuentaId: number) => {
        setExpandedAccountId(prevState => (prevState === cuentaId ? null : cuentaId));
    };

    return (
        <>
            {cuentas.map((cuenta: CuentaModelDto) => (
                <Card
                    key={cuenta.cuentaId}
                    style={{ marginBottom: '15px', background: '#f1f2f6' }}
                >
                    <Cuentas
                        listCuentas={cuentas}
                        cuentas={cuenta}
                        key={cuenta.cuentaId}
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
                        handleOnChangeMostrarPorcentaje={
                            handleOnChangeMostrarPorcentaje
                        }
                        setCuentas={setCuentas}
                        estadoCuentaEmbargo={estadoCuentaEmbargo}
                        setConcepto={setConcepto}
                        onToggleExpand={() => handleToggleExpand(cuenta.cuentaId)}
                        expanded={expandedAccountId === cuenta.cuentaId}
                        soloVisual={soloVisual}
                    />
                </Card>
            ))}
        </>
    );
};

interface LinkMapDispatchToProps {
    cuentas: CuentaModelDto[];
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
    soloVisual: boolean;
    saldoConcepto: any;
}

type props = LinkMapDispatchToProps;

export default CuentasList;
