import React, { ChangeEvent, useEffect, useState } from 'react';
import './table-cuentas.scss';
import {
  Grid, IconButton,
} from '@material-ui/core';
import { Edit } from '@material-ui/icons';
import { AdminCuentasAfiliado } from '../../../../types/client.model.type';
import UtilService from '../../../../utils/util.service';
import ModalActualizarCuotas from './ModalActualizarCuotas';



function TablaCategoriaAfiliado({
  responseAdminCuenta
}: {
  responseAdminCuenta: AdminCuentasAfiliado;
}) {

  useEffect(() => {
    setItems(responseAdminCuenta.aportes)
  }, [responseAdminCuenta])

  const [items, setItems] = useState<any[]>(responseAdminCuenta.aportes);

  const handleChangeBuscar = (event: ChangeEvent<HTMLInputElement>) => {

    if (event.target.value.toUpperCase() === '') {
      setItems(responseAdminCuenta.aportes);
    } else {
      setItems(filterItems(event.target.value));
    }

  }

  const filterItems = (value: string) => {
    const filter = responseAdminCuenta.aportes.filter(
      (x) =>
        x.apC_CUOTAS_ACUMULADAS.toString().includes(value) ||
        x.apC_FECHA_PRIMERA_CUOTA.toString().includes(value) ||
        x.apC_FECHA_ULTIMA_COUTA.toString().includes(value)
    );
    return filter;
  };

  const [openModal, setOpenModal] = useState(false);
  const [selectedRow, setSelectedRow] = useState<any>(null);

  const handleEdit = (row: any) => {
    setSelectedRow(row);
    setOpenModal(true);
  };

  const handleCloseModal = () => {
    setOpenModal(false);
    setSelectedRow(null);
  };




  return (
    <Grid>

      <Grid className="container-24">
        <input type="search" placeholder="Buscar.." onChange={handleChangeBuscar} />
      </Grid>

      <Grid className="container-17">
        <table className="table">
          <thead>
            <th className="tab-space-01">Categoría</th>
            <th className="tab-space-01">Número cuotas</th>
            <th className="tab-space-01">Fecha primer aporte</th>
            <th className="tab-space-01">Fecha último aporte</th>
            <th className="tab-space-01">Acciones</th>
          </thead>
          <tbody>

            {items?.map((row, index) => (
              <tr key={`table-${index}`}>
                <td data-label="Descripción" className="tab-space-01"> {row?.ctG_DESCRIPCION} </td>
                <td data-label="Coutas" className="tab-space-01">{row.apC_CUOTAS_ACUMULADAS}</td>
                <td data-label="Primer Aporte" className="tab-space-01">{UtilService.getDate(row.apC_FECHA_PRIMERA_CUOTA)}</td>
                <td data-label="Ultimo Aporte" className="tab-space-01">{UtilService.getDate(row.apC_FECHA_ULTIMA_COUTA)}</td>
                <td className="tab-space-01">
                  <IconButton
                    aria-label="edit"
                    color="primary"
                    size="medium"
                    onClick={() => handleEdit(row)}
                  >
                    <Edit fontSize="small" />
                  </IconButton>
                </td>

              </tr>
            ))}

          </tbody>
        </table>
      </Grid>
      <ModalActualizarCuotas
        open={openModal}
        onClose={handleCloseModal}
        data={selectedRow}
      />
    </Grid>

  )
}

export default TablaCategoriaAfiliado;