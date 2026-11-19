import { Grid } from '@material-ui/core';
import React from 'react';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';

const InformacionPagoIntervinientes = (props: propsType) => {

    const {datosLlenarTablaPagos,listaEntidadesBancarias} = props;

    return (
        <Grid container>
        <CHPaperNew>
        <table className="table">
            <thead>
                <tr>
                  <th className='column2'>Tipo de cuenta</th>
                  <th className='column2'>Número de cuenta bancaria</th>
                  <th className='column2'>Entidad Bancaria</th>
                </tr>
          </thead>
            <tbody>
              <tr key="table-">
                  <td className='tab-space-01'>{datosLlenarTablaPagos?.itR_TIPO_CUENTA?.split('_')[1]}</td>
                  <td className='tab-space-01'>{datosLlenarTablaPagos?.itR_NUMERO_CUENTA_BANCARIA}</td>
                  <td className='tab-space-01'>{listaEntidadesBancarias
                .filter((tipo: any) => tipo.codigoBanco === Number(datosLlenarTablaPagos?.itR_ENTIDAD_BANCARIA))
                .map((tipo: any) => tipo.nombreRazonSocial.toUpperCase())
                .shift() || 'Descripción no encontrada'}</td>
              </tr>
            </tbody>
        </table>
        </CHPaperNew>
        </Grid>
    )

}

export default InformacionPagoIntervinientes;

type propsType = {
    datosLlenarTablaPagos: any;
    listaEntidadesBancarias: any[];
}