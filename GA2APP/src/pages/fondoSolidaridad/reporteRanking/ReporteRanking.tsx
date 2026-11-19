import { makeStyles, createStyles, Grid, MenuItem, Button } from '@material-ui/core';
import React from 'react';
import { Controller, useForm } from 'react-hook-form';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '70%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
    }),
);

function InformacionReporte({
    data,
    // onClickItem,
}: {
    data: any[] | null;
    // onClickItem: (data: EmbargoDto) => void;
}) {
    const classes = useStyles();

    if (data === null) {
        return <div>No se encontraron resultados</div>;
    }

    return (
        <>
            <table className="credit-table">
                <thead>
                    <tr>
                        <th style={{ textAlign: 'center' }}>
                            Nombre Convocatoria
                        </th>
                        <th style={{ textAlign: 'center' }}>Detalle</th>
                    </tr>
                </thead>
                {/* <tbody>
                    {data?.map((row, index) => (
                        <tr key={`table-${index}`}>
                            <td>{row.tipoIdentificacion}</td>
                            <td>{row.identificacionCliente}</td>
                            <td>{row.juzgado}</td>
                            <td>{row.radicadoWorkManager}</td>
                            <td>
                                {UtilService.getDate(
                                    row.fechaRadicadoWorkManager,
                                )}
                            </td>
                            <td>{row.tipoProceso}</td>
                            <td>
                                <Button
                                    className={classes.processButton}
                                    onClick={() => onClickItem(row)}
                                >
                                    Detalle
                                </Button>
                            </td>
                        </tr>
                    ))}
                </tbody> */}
            </table>
        </>
    );
}

const ReporteRanking = () => {
    const classes = useStyles();

    const formularioReporte = useForm({
        defaultValues: {
           convocatoria: '',
        },
    });

    return (
        <>
           <CHPrivateContainer>
                <CHPageTitle
                    topText="Gestión Reporte Ranking"
                    subText=""
                    title1="Reporte Ranking"
                    title2="Fondo Solidaridad"
                />
                <CHPaper title="Información Reporte">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="convocatoria"
                            control={formularioReporte.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Convocatoria *"
                                    value={value}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={onChange}
                            >
                                   <MenuItem>
                                           Convocatoria
                                   </MenuItem>
                                {/* {garaje?.map(
                                    (item: CATALOGODTO) => (
                                        <MenuItem
                                            key={item.codigo}
                                            value={item.codigo}
                                        >
                                            {item.descripcion}
                                        </MenuItem>
                                    ),
                                )} */}
                            </CHSelect>
                            )}
                        />
                    </Grid>
                <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="primary"
                            // style={{ width: '200px' }}
                            // onClick={handleOnClickRespuesta}
                        >
                           RANKING
                        </Button>
                </Grid>
              </Grid>
            </CHPaper>
        </CHPrivateContainer>
        </>
    )
}

export default ReporteRanking;