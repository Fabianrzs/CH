import React, {  } from 'react';
import './table-cuentas.scss';
import {
  Grid,
} from '@material-ui/core';

function TablaConceptoDetalleAfiliado() {

  return (
    <>
      <Grid className="marginTablesPopup">

        <Grid>
          <input type="search" className='floatRightButton' placeholder="Buscar.." />
        </Grid>

        <Grid className="container-17">
          <table className="table">
            <thead>

              <th className="tab-space-01">N° Item</th>
              <th className="tab-space-01">Número Cuenta</th>
              <th className="tab-space-01">Descripción</th>
              <th className="tab-space-01">Saldo</th>
              <th className="tab-space-01">Valor</th>
              <th className="tab-space-01">Valor Embargo</th>
              <th className="tab-space-01">Cuenta Bloqueada</th>

            </thead>
            <tbody>

              <tr >
                <td data-label="Descripción" className="tab-space-01"> 1 </td>
                <td data-label="Tasa-EA" className="tab-space-01">254687545</td>
                <td data-label="Tasa-EA" className="tab-space-01">Cuenta de ahorro de vivienda</td>
                <td data-label="Plazo-Mínimo" className="tab-space-01">$ 20.000.00</td>
                <td data-label="Plazo-Mínimo" className="tab-space-01">$ 10.254.123</td>
                <td data-label="Descripción" className="tab-space-01">$ 5.000.00</td>
                <td data-label="Tasa-EA" className="tab-space-01">SI</td>
              </tr>

              <tr >
                <td data-label="Descripción" className="tab-space-01"> 1 </td>
                <td data-label="Tasa-EA" className="tab-space-01">1345678</td>
                <td data-label="Tasa-EA" className="tab-space-01">Cuenta de ahorro de ahorro</td>
                <td data-label="Plazo-Mínimo" className="tab-space-01">$ 5.000.00</td>
                <td data-label="Plazo-Mínimo" className="tab-space-01">$ 5.000.00</td>
                <td data-label="Descripción" className="tab-space-01">$ 0.0</td>
                <td data-label="Tasa-EA" className="tab-space-01">NO</td>
              </tr>

            </tbody>
          </table>
        </Grid>
      </Grid>

    </>

  );
}


export default TablaConceptoDetalleAfiliado;