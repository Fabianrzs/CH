import React from 'react';
import { Grid } from '@material-ui/core';
import { connect } from 'react-redux';
import { AppState } from '../../../store/rootReducer';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { startDeletePropietaryHistory, startUpdatePropietaryHistory, startGetBasicInformation } from '../../../actions/realStateRegistrations.action';
import UtilService from '../../../utils/util.service';
import { EducationalEntitiesState } from '../../../reducers/educationalEntities.reducer';

const ChangeHistory = ({
  educationalEntities
}: {
  educationalEntities: EducationalEntitiesState;
}): JSX.Element => (
 
    <Grid container>
      <CHPaperNew>
        <table className="table">
          <thead>
            <tr>
              <th className='column2'>Pestaña</th>
              <th className='column2'>Campo</th>
              <th className='column2'>Valor Anterior</th>
              <th className='column2'>Valor Actualizacion</th>
              <th className='column2'>Usuario</th>
              <th className='column2'>Fecha Modificacion</th>
            </tr>
          </thead>
          <tbody>
            {educationalEntities?.historialCambios && educationalEntities?.historialCambios.map((row: any, index: number) => (
              <tr key={`table-${index}`}>
                <td className='tab-space-01'>{row.pestaña}</td>
                <td className='tab-space-01'>{row.campo}</td>
                <td className='tab-space-01'>{row.vlrAnterior}</td>
                <td className='tab-space-01'>{row.vlrNuevo}</td>
                <td>{row.nombreUsuario}</td>
                <td className='tab-space-01'> {UtilService.getFullDateWithTimeDirection(new Date(String(row?.fechaModificacion).toString()))}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </CHPaperNew>
    </Grid>
  )

const mapStateToProps = (state: AppState) => ({
  catalogState: state.catalog,
  loginState: state.login,
  realStateRegistrationsState: state.realStateRegistrations,
  informacionBasica: state.informacionBasica,
  educationalEntities: state.educationalEntities,
});

const mapDispatchToProps = {
  startDeletePropietaryHistory,
  startUpdatePropietaryHistory,
  startGetBasicInformation
};

export default connect(mapStateToProps, mapDispatchToProps)(ChangeHistory);