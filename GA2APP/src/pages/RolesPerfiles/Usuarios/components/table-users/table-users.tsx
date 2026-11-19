import React, { ChangeEvent, useEffect, useState } from 'react';
import swal from 'sweetalert';
import './table-users.scss';
import { Grid } from '@material-ui/core';
import history from '../../../../../utils/history.router';
import { User } from '../../../../../types/user.type';
import ContainerTitle from '../../../../../components/containe-title/container-title';

function TableUsers({
  users,
  setEditUser,
  deleteUser,
}: {
  users: User[];
  setEditUser: (user: User) => void;
  deleteUser: (user: User) => void;
}) {
  const [items, setItems] = useState<User[]>();

  const onClickEliminar = (user: User) => (event: React.MouseEvent) => {
    const segundoNombre = user.segundoNombre === null ? '' : user.segundoNombre;
    const segundoApellido =
      user.segundoApellido === null ? '' : user.segundoApellido;
    swal({
      text: `¿Seguro que quiere eliminar al usuario ${user.nombre} ${segundoNombre} ${user.apellido} ${segundoApellido}?`,
      icon: 'warning',
      buttons: ['No', 'Sí'],
    }).then(respuesta => {
      if (respuesta) {
        deleteUser(user);
        setItems(items?.filter(x => x.id !== user.id));
      }
    });

    event.preventDefault();
  };

  useEffect(() => {
    setItems(users);
  }, [users]);

  const onClickCreaUsurio = () => (event: React.MouseEvent) => {
    history.push('/CrearUsuario');
    event.preventDefault();
  };

  const onClickEditarUsuario = (user: User) => (event: React.MouseEvent) => {
    setEditUser(user);
    event.preventDefault();
  };

  const handleChangeBuscar = (event: ChangeEvent<HTMLInputElement>) => {
    if (event.target.value.toUpperCase() === '') {
      setItems(users);
    } else {
      setItems(filterItems(event.target.value.toUpperCase()));
    }
  };

  const filterItems = (value: string) => {
    const filter = users.filter(
      x =>
        x.nombre?.toUpperCase().includes(value) ||
        x.apellido?.toUpperCase().includes(value) ||
        x.segundoNombre?.toUpperCase().includes(value) ||
        x.segundoApellido?.toUpperCase().includes(value) ||
        x.correo?.toUpperCase().includes(value) ||
        x.identificacion?.toUpperCase().includes(value) ||
        x.nombreUsuario?.toUpperCase().includes(value),
    );
    return filter;
  };

  return (
    <Grid>
      <ContainerTitle title="Administración de Usuarios" />

      <Grid className="container-24">
        <input
          type="search"
          placeholder="Buscar.."
          onChange={handleChangeBuscar}
        />
      </Grid>

      <Grid className="container-23">
        <Grid>
          <button
            type="button"
            className="bgcian button-i blue flex"
            onClick={onClickCreaUsurio()}
          >
            <span className="ico fch-Agregar-0-1 icoMid-2 separador-marg-6" />
            AGREGAR
          </button>
        </Grid>
      </Grid>
      <Grid className="container-17">
        <table className="table">
          <thead>
            <th className="tab-space-01">Nombres y Apellidos</th>
            <th className="tab-space-01">Tipo Identificación</th>
            <th className="tab-space-01">Identificación</th>
            <th className="tab-space-01">Correo Electrónico</th>
            <th className="tab-space-01">Usuario</th>
            <th className="tab-space-01">Estado</th>
            <th className="tab-space-01">Acciones</th>
          </thead>
          <tbody>
            {items?.map((row, index) => (
              <tr key={`table-${index}`}>
                <td data-label="Descripción" className="tab-space-01">
                  {' '}
                  {row.nombre} {row.segundoNombre} {row.apellido}{' '}
                  {row.segundoApellido}
                </td>
                <td data-label="Tasa-EA" className="tab-space-01">
                  {row.tipoIdentificationCode}
                </td>
                <td data-label="Tasa-EA" className="tab-space-01">
                  {row.identificacion}
                </td>
                <td data-label="Plazo-Mínimo" className="tab-space-01">
                  {row.correo}
                </td>
                <td data-label="Plazo-Máximo" className="tab-space-01">
                  {row.nombreUsuario}
                </td>
                <td data-label="Estado" className="tab-space-01">
                  {' '}
                  {row.estado ? 'Activo' : 'Inactivo'}{' '}
                </td>
                <td data-label="Estado" className="tab-space-01 actions">
                  <Grid>
                    <span
                      aria-label="Editar"
                      onClick={onClickEditarUsuario(row)}
                    >
                      <span className="ico fch-Editar-0-1 icoMid-3" />
                    </span>
                    <span aria-label="Eliminar" onClick={onClickEliminar(row)}>
                      <span className="ico fch-Eliminar-0-1 icoMid-3" />
                    </span>
                  </Grid>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </Grid>
    </Grid>
  );
}

export default TableUsers;
