import { createStyles,
         makeStyles,
         Table, 
         TableBody, 
         TableCell, 
         TableHead, 
         TableRow } from '@material-ui/core';
import React, { useEffect, useRef } from 'react';
import { ConceptoDto } from '../../../../models/requests/reintegros-request-request.model';
import UtilService from '../../../../utils/util.service';
import { TIPO_REINTEGRO } from './constants/reintegros-constants';

const useStyles = makeStyles(() =>
    createStyles({
        checkBox: {
            backgroundColor: '#0075ff',
            accentColor: '#1c3f8b',
            width:'18px',
            height: '18px',
            cursor: 'pointer',
        },
    }),
);

const ListaConceptosReintegros = (props: props) => {
    const classes = useStyles();
    const {conceptos,
           cuentaId,
           tipoReintegro,
           estadoAfiliadoEnEvaluacion,
           listaConceptosSeleccionada,
           funcionAplicada,
           setListaConceptosSeleccionada} = props;

    const elementosCheck = useRef<HTMLInputElement[]>([]);

    useEffect(() => {
        if(listaConceptosSeleccionada.length === 0){
            elementosCheck.current.forEach((check: HTMLInputElement | null) => {
                if (check) {
                  check.checked = false;
                }
            });
        }
      }, [listaConceptosSeleccionada]);

    useEffect(() => {
        elementosCheck.current.forEach((check: HTMLInputElement | null) => {
            if (check) {
              check.checked = false;
            }
        });
      }, [funcionAplicada]);

    const handleOnChange = (e: any, cuentaId: number) => {
        const {value, checked} = e.target;
        const concepto = conceptos?.find(item => item.conceptoId === Number(value) && item.cuentaId ===cuentaId);
      
        if(checked){
            setListaConceptosSeleccionada([...listaConceptosSeleccionada,concepto]);
            }else{
            setListaConceptosSeleccionada(listaConceptosSeleccionada.filter(item => item.conceptoId !== Number(value)));
        }
    }

    const mostrarValor = (valorNewSaldo: number, valorBloqueo: number, saldo: number) => {
        let valorMostrar = 0;
        if(tipoReintegro === TIPO_REINTEGRO.BLOQUEOS && valorNewSaldo === 0 && valorBloqueo === 0){
            valorMostrar = saldo;
        }else if(tipoReintegro === TIPO_REINTEGRO.BLOQUEOS && valorBloqueo > 0){
            valorMostrar = valorNewSaldo;
        }
        else if(tipoReintegro === TIPO_REINTEGRO.DESBLOQUEOS && valorBloqueo > 0 ){
            valorMostrar = valorNewSaldo;
        }
        else if(tipoReintegro === TIPO_REINTEGRO.DESBLOQUEOS && valorBloqueo === 0 ){
            valorMostrar = saldo;
        }
        else if(tipoReintegro === TIPO_REINTEGRO.DEVOLUCION_DE_RECURSOS && valorNewSaldo === 0){
            valorMostrar = saldo;
        }
        else if(tipoReintegro === TIPO_REINTEGRO.DEVOLUCION_DE_RECURSOS && valorNewSaldo > 0){
            valorMostrar = valorNewSaldo;
        }
        return valorMostrar;
    }

    return(
        <>
            <Table
                className="client-table"
                style={{
                    margin: '20px',
                    width: '95%',
                }}
            >
            <TableHead>
                <TableRow>
                    <TableCell>{" "}</TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Concepto
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Valor Bloqueo
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Valor Disponible
                    </TableCell>
                    <TableCell style={{ textAlign: 'center' }}>
                        Valor a Devolver
                    </TableCell>
                </TableRow>
            </TableHead>
            <TableBody> 
            {conceptos.map((item, index) => {
                if(item.cuentaId === cuentaId){
                    return (
                        <TableRow key={index}>
                            <TableCell>
                            {(tipoReintegro === TIPO_REINTEGRO.BLOQUEOS) 
                          && (estadoAfiliadoEnEvaluacion) 
                          && (!(String(item.conceptoDescripcion).includes('INTERESES'))) &&
                             (item.valorBloqueoEmbargo === 0) && (
                                <input type="checkbox" 
                                    ref={ref => {elementosCheck.current[index] = ref as HTMLInputElement}}
                                    className={classes.checkBox}
                                    value={item?.conceptoId} 
                                    onChange={e => {
                                        handleOnChange(e,cuentaId)
                                    }}
                                />
                            )}
                            {(tipoReintegro === TIPO_REINTEGRO.DESBLOQUEOS) 
                            && (estadoAfiliadoEnEvaluacion) 
                            && (!(String(item.conceptoDescripcion).includes('INTERESES'))) &&
                             (item.valorBloqueoEmbargo > 0) && (
                                <input type="checkbox" 
                                    ref={ref => {elementosCheck.current[index] = ref as HTMLInputElement}}
                                    className={classes.checkBox}
                                    value={item?.conceptoId} 
                                    onChange={e => {
                                        handleOnChange(e,cuentaId)
                                    }}
                                />
                            )}
                            {(tipoReintegro === TIPO_REINTEGRO.DEVOLUCION_DE_RECURSOS) 
                            && (estadoAfiliadoEnEvaluacion) 
                            && (!(String(item.conceptoDescripcion).includes('INTERESES'))) &&
                             (item.valorDevolver === 0) && (
                                <input type="checkbox" 
                                    ref={ref => {elementosCheck.current[index] = ref as HTMLInputElement}}
                                    className={classes.checkBox}
                                    value={item?.conceptoId} 
                                    onChange={e => {
                                        handleOnChange(e,cuentaId)
                                    }}
                                />
                            )}
                            </TableCell>
                            <TableCell>{item.conceptoDescripcion}</TableCell>
                            <TableCell>{item.conceptoDescripcion.includes('INTERESES')? '' : UtilService.formatCurrency(item.valorBloqueoEmbargo)}</TableCell>
                            <TableCell>{UtilService.formatCurrency(mostrarValor(item.valorNewSaldo,item.valorBloqueoEmbargo,item.saldo))}</TableCell>
                            <TableCell>{item.conceptoDescripcion.includes('INTERESES')? '' : UtilService.formatCurrency(tipoReintegro === TIPO_REINTEGRO.BLOQUEOS || tipoReintegro === TIPO_REINTEGRO.DESBLOQUEOS?  0 : item.valorDevolver)}</TableCell>
                        </TableRow>
                    )
                }
                return null;
            })}
            </TableBody>
            </Table>
        </>
    )
}

type props = {
    conceptos: ConceptoDto[];
    cuentaId: number;
    tipoReintegro: number;
    estadoAfiliadoEnEvaluacion: boolean;
    listaConceptosSeleccionada: any[];
    funcionAplicada: number;
    setListaConceptosSeleccionada: (value: any[]) => void;
}

export default ListaConceptosReintegros;