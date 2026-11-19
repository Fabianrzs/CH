import { IconButton } from '@material-ui/core';
import { Edit } from '@material-ui/icons';
import React from 'react';
import UtilService from '../../../../utils/util.service';

const Bloqueos = (props: props) => {

    const {listaBloqueos,
           handleOpen} = props;

    const totalBloqueos = () => {
        let valorTotal = 0;
        if(listaBloqueos){
            listaBloqueos.forEach((item:any) => {
                valorTotal += item.valorSolicitado;
            })
        }
        return valorTotal;
    }

    return (
      <>
      {listaBloqueos.length > 0 && (
        <table className="credit-table"    
        style={{
            margin: '20px',
            width: '95%',
        }}>
          <thead>
              <tr>
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
                   listaBloqueos?.map((row: any, index: any) => (
                           <tr  key={`table-${index}`}>
                               <td>{row?.numeroDocumento}</td>
                               <td>{row?.nombres}</td>
                               <td>{row?.apellidos}</td>
                               <td>{row?.valorSolicitado? (UtilService.formatCurrency(row?.valorSolicitado)) : ('$0')}</td>
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
                    <td colSpan={3}><b>TOTAL BLOQUEOS</b></td>
                    <td><b>{UtilService.formatCurrency(totalBloqueos())}</b></td>
                    <td colSpan={3}>{' '}</td>
                </tr>
          </tbody>
      </table>
      )}
      </>
    );
}

type props = {
    listaBloqueos: any[];
    handleOpen: (value: boolean) => void;
}

export default Bloqueos;