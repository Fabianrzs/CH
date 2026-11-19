import React from 'react';
import { createStyles, Grid, Card, CardContent, makeStyles } from "@material-ui/core";
import ContainerTitle from '../../../../components/containe-title/container-title';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { AdminCuentasAfiliado } from '../../../../types/client.model.type';
import UtilService from '../../../../utils/util.service';



const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        columnLabel: {
            padding: '10px',
        },
        root: {
            flexGrow: 1,
        },
        marginTops:{
            marginTop: '1rem'
        }
    }),
);


function DatosPersonales({
    responseAdminCuenta

}: {
    responseAdminCuenta: AdminCuentasAfiliado;
}) {
    const classes = useStyles();
    
    return (
        <>
            <div className={classes.root}>
                <Grid item xs={12}>
                    <Card>
                        <ContainerTitle
                            title="Datos básicos del afiliado"
                        />
                        <CardContent>

                            <Grid container>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        isReadOnly={false}
                                        label="Nombres"
                                        value={`${ responseAdminCuenta.primerNombre} ${ responseAdminCuenta?.segundoNombre || ''}`  }     
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        isReadOnly={false}
                                        label="Apellidos"
                                        value={`${ responseAdminCuenta.primerApellido} ${ responseAdminCuenta?.segundoApellido || ''}`  } 
                                    />
                                </Grid>

                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        isReadOnly={false}
                                        label="Tipo Documento"
                                        value={ responseAdminCuenta.tipoIdentificacion}
                                    />
                                </Grid>

                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        numberMask
                                        isReadOnly={false}
                                        label="Número De Documento"
                                        value={ responseAdminCuenta.identificacion  }
                                    />
                                </Grid>

                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        isReadOnly={false}
                                        label="Unidad Ejecutora"
                                        value={ responseAdminCuenta.unidadEjecutora}
                                    />
                                </Grid>

                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        isReadOnly={false}
                                        label="Fuerza"
                                        value={responseAdminCuenta?.fuerza }
                                    />
                                </Grid>

                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        isReadOnly={false}
                                        label="Grado"
                                        value={responseAdminCuenta?.grado }
                                    />
                                </Grid>

                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        isReadOnly={false}
                                        label="Categoría"
                                        value={responseAdminCuenta?.categoria}
                                    />
                                </Grid>

                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        isReadOnly={false}
                                        label="Fecha Inscripción"
                                        value={ UtilService.getDate(responseAdminCuenta.fechaInscripcion)}
                                    />
                                </Grid>

                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        isReadOnly={false}
                                        label="Fecha Alta"
                                        value={UtilService.getDate(responseAdminCuenta.fechaAlta) } 
                                    />
                                </Grid>

                                <Grid className={classes.column} item xs={12} md={3}>
                                    <CHTextField
                                        isReadOnly={false}
                                        label="Tipo Afiliado"
                                        value={responseAdminCuenta?.tipoAfiliacion}
                                    />
                                </Grid>
                            </Grid>
                        </CardContent>
                    </Card>
                </Grid>
            </div>
            <div className={classes.marginTops}/>
              
        </>
    )
}

export default DatosPersonales;