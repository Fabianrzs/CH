import React from 'react';
import {
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { AppState } from '../../../store/rootReducer';
import { startDeleteRol, startGetAllRoles } from '../../../actions/rol.action';
import { ROLDTO } from '../../../types/rol.type';

export const TableRoles = (props?: any) => {
    const { rolState } = props;

    // const deleteRolHandler = (roleID: number) => {
    //     console.log('not empty');
    // };

    return (
        <div>
            <h1>Usuarios</h1>
            <br />
            <div className="div-in-table">
                <TableContainer component={Paper} className="table-users">
                    <Table aria-label="simple table">
                        <TableHead>
                            <TableRow>
                                <TableCell>Descripción</TableCell>
                                <TableCell>Fecha Creación</TableCell>
                                <TableCell>Eliminar</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {!rolState.roles
                                ? 'no hay datos'
                                : rolState.roles.map((rol: ROLDTO) => (
                                      <TableRow key={rol.id}>
                                          <TableCell component="th" scope="row">
                                              {rol.descripcion}
                                          </TableCell>
                                          <TableCell>
                                              {rol.fechaCreacion}
                                          </TableCell>
                                          <TableCell>
                                              {/* <Button
                                                  onClick={deleteRolHandler(
                                                      rol.id,
                                                  )}
                                                  variant="contained"
                                              >
                                                  Eliminar
                                              </Button> */}
                                          </TableCell>
                                      </TableRow>
                                  ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
        </div>
    );
};

const mapStateToProps = (state: AppState) => ({
    rolState: state.rol,
});

const mapDispatchToProps = {
    startGetAllRoles,
    startDeleteRol,
};

export default connect(mapStateToProps, mapDispatchToProps)(TableRoles);
