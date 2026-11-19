import React from 'react';
import { Button, createStyles, Grid, makeStyles, } from '@material-ui/core';
import { connect } from 'react-redux';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import UtilService from '../../../utils/util.service';

const useStyles = makeStyles(() =>
    createStyles({
        button: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            width: '18%',
            marginLeft: '80%',
        },
    }),
);

function TramitesIntervinientes({
    data,
    listaTipoIdentificacion
}: {
    data: any;
    listaTipoIdentificacion: any;
}): any {
    const classes = useStyles();

    return (
        <Grid container>
            <CHPaperNew>
                <table className="table">
                    <thead>
                        <tr>
                            <th className='column2'>Tipo de Documento</th>
                            <th className='column2'>N° Documento</th>
                            <th className='column2'>Afiliado</th>
                            <th className='column2'>N° Radicado</th>
                            <th className='column2'>Tipo Solicitud</th>
                            <th className='column2'>Modalidad</th>
                            <th className='column2'>Fecha Solicitud</th>
                            <th className='column2'>Valor Solicitud</th>
                            <th className='column2'>Estado</th>
                            <th className='column2'>Tipo Actor</th>
                        </tr>
                    </thead>
                    <tbody>
                        {data?.map((row: any, index: number) => (
                            <tr key={`table-${index}`}>
                                <td className='tab-space-01'>{ listaTipoIdentificacion.filter((tipo:any) => tipo.id === 1)
                                    .map((tipo:any)=> tipo.descripcion)}</td>
                                <td className='tab-space-01'>{row?.soL_CEDULA}</td>
                                <td className='tab-space-01'>{row?.prP_NOMBRE_RAZON_SOCIAL}</td>
                                <td className='tab-space-01'>{row?.soL_RADICADO_GD}</td>
                                <td className='tab-space-01'>{row?.tpS_SUB_DESCRIPCION}</td>
                                <td className='tab-space-01'>{row?.tiM_DESCRIPCION}</td>
                                <td className='column4'>{UtilService.getFullDateWithTimeDirection(new Date(String(row?.soL_FECHA_SOLICITUD).toString()))}</td>  
                                <td className='tab-space-01'>{UtilService.formatCurrency(row?.soL_VALOR_A_RETIRAR)}</td>
                                <td className='tab-space-01'>{row?.esS_DESCRIPCION}</td>
                                <td className='tab-space-01'>{String(row?.itA_NOMBRE).toUpperCase()}</td>
                            </tr>
                         ))}
                    </tbody>
                </table>
                <div className="buttons">
                    <Button
                        className={classes.button}
                        variant="outlined"
                        type="submit"
                        // onClick={() => onClickExcel()}
                    >
                        EXCEL
                    </Button>
                </div>
                
                
            </CHPaperNew>
        </Grid>
    );
};


export default connect()(TramitesIntervinientes);