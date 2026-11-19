import {createStyles, IconButton, makeStyles } from '@material-ui/core';
import { Edit } from '@material-ui/icons';
import React, { useEffect, useState } from 'react';
import UtilService from '../../../../utils/util.service';

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

const DevolucionRecursos = (props: props) => {

    const classes = useStyles();
    const {listaDevolucionRecursos,
           listaDevolucionRecursosSeleccionada,
           handleOpen,
           setListaDevolucionRecursosSeleccionada} = props;
   
    const [allChecked,setAllChecked]= useState(false);
    const [itemsEstadoEvaluacion,setItemsEstadoEvaluacion]= useState(false);

    useEffect(() => {
        if(listaDevolucionRecursos){
            setItemsEstadoEvaluacion(listaDevolucionRecursos.some((item:any) => item.estadoAfiliao === 'EN EVALUACION'));
        }
    },[listaDevolucionRecursos])
   
    const handleOnChangeAll = (e : any) => {
       if(e.target.checked){
         setAllChecked(true);
         setListaDevolucionRecursosSeleccionada([...listaDevolucionRecursos]);
       }else{
         setAllChecked(false);
         setListaDevolucionRecursosSeleccionada([]);
       }
    }

    const handleOnChange = (e: any, row: any) => {
        const {value, checked} = e.target;
        const afiliado = listaDevolucionRecursos?.find((item: any) => item.numeroDocumento === Number(value));
        if(checked){
            setListaDevolucionRecursosSeleccionada([...listaDevolucionRecursosSeleccionada,afiliado]);
        }else{
            setListaDevolucionRecursosSeleccionada(listaDevolucionRecursosSeleccionada.filter(item => item.numeroDocumento !== Number(value)));
        }
    }

    const totalDevolucionRecursos = () => {
        let valorTotal = 0;
        if(listaDevolucionRecursos){
            listaDevolucionRecursos.forEach((item:any) => {
                    valorTotal += item.valorSolicitado;
            })
        }
        return valorTotal;
    }
    
    return (
    <>
    {listaDevolucionRecursos.length > 0 && (
         <table className="credit-table"    
         style={{
             margin: '20px',
             width: '95%',
         }}>
           <thead>
               <tr>
                    {(listaDevolucionRecursos.length > 0) && (itemsEstadoEvaluacion) && (
                        <th style={{ textAlign: 'center' }}>
                            <input type="checkbox" className={classes.checkBox} onChange={handleOnChangeAll}/>
                        </th>
                    )}
                    {(listaDevolucionRecursos.length > 0) && (itemsEstadoEvaluacion === false) && (
                        <th style={{ textAlign: 'center' }}>
                            {' '}
                        </th>
                    )}
                
                   <th style={{ textAlign: 'center' }}>
                       Cédula del Afiliado
                   </th>
                   <th style={{ textAlign: 'center' }}>
                       Nombres
                   </th>
                   <th style={{ textAlign: 'center' }}>
                       Apellidos
                   </th>
                   <th style={{ textAlign: 'center' }}>
                       Valor Solicitado
                   </th>
                   <th style={{ textAlign: 'center' }}>
                       Estado
                   </th>
                   <th style={{ textAlign: 'center' }}>
                       Acciones
                   </th>
               </tr>
           </thead>
             <tbody>
                {
                    listaDevolucionRecursos?.map((row: any, index: any) => (
                            <tr  key={`table-${index}`}>
                                 <td>
                                {
                                    ((allChecked) && (row?.estadoAfiliao === 'EN EVALUACION') && (
                                        <input type="checkbox" 
                                            className={classes.checkBox}
                                            value={row?.numeroDocumento} 
                                            checked 
                                            onChange={e => {handleOnChange(e, row)}}
                                        />
                                    ))

                                }

                                {
                                    ((!allChecked) && (row?.estadoAfiliao === 'EN EVALUACION') && (
                                        <input type="checkbox" 
                                               className={classes.checkBox}
                                               value={row?.numeroDocumento} 
                                               onChange={e => {handleOnChange(e, row)}}
                                         />
                                    ))

                                }

                                {
                                    row?.estadoAfiliao === 'PROCESADO' && (
                                       ' '
                                    )
                                }

                                </td>
                                <td>{row?.numeroDocumento}</td>
                                <td>{row?.nombres}</td>
                                <td>{row?.apellidos}</td>
                                <td>{UtilService.formatCurrency(row?.valorSolicitado)}</td>
                                <td>{row?.estadoAfiliao}</td>
                                <td>
                                    <IconButton
                                        aria-label="Editar"
                                        color="primary"
                                        onClick={() => handleOpen(row)}
                                    >
                                       <Edit />
                                    </IconButton>
                                </td>
                            </tr>
                        ))
                }
                 <tr>
                     <td colSpan={4}><b>TOTAL DEVOLUCIÓN RECURSOS</b></td>
                     <td><b>{UtilService.formatCurrency(totalDevolucionRecursos())}</b></td>
                     <td colSpan={3}>{' '}</td>
                 </tr>
           </tbody>
       </table> 
    )}
      </>
    );
}

type props = {
    listaDevolucionRecursos: any;
    listaDevolucionRecursosSeleccionada: any[];
    handleOpen: (row: any) => void;
    setListaDevolucionRecursosSeleccionada: (value: any[]) => void;
}

export default DevolucionRecursos;