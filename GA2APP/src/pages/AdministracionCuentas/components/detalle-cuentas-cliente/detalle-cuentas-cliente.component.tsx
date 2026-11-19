import React from 'react';
import { createStyles, Grid, makeStyles, Card, CardContent } from "@material-ui/core";
import ContainerTitle from '../../../../components/containe-title/container-title'
import TablaDetalleCuentaAfiliado from '../tables-cuentas-afiliado/table-detalle-cuenta-afiliado'
import { AdminCuentasAfiliado } from '../../../../types/client.model.type';
import { CAUSALBLOQUEOCUENTA, IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';

const useStyles = makeStyles(() =>
    createStyles({

        marginTops: {
            marginTop: '1rem'
        },
        root: {
            flexGrow: 1,
        }

    }),
);


function DetalleCuentascliente({
    responseAdminCuenta,
    tipoBloqueo,
    actualizarObjetoRespuesta,  
    tiposIdentificacion
}: {
    responseAdminCuenta: AdminCuentasAfiliado;
    tipoBloqueo: CAUSALBLOQUEOCUENTA[];
    actualizarObjetoRespuesta: (data: any) => void;
    tiposIdentificacion: IDENTIFICATIONTYPEDTO[];
}) {
    const classes = useStyles();

    return (
        <>
            <div className={classes.root}>
                <Grid item xs={12}>
                    <Card>
                        <ContainerTitle
                            title="Detalle de las cuentas del afiliado"
                        />
                        <CardContent>
                            <TablaDetalleCuentaAfiliado
                                responseAdminCuenta={responseAdminCuenta}
                                tipoBloqueo={tipoBloqueo}
                                actualizarObjetoRespuesta={actualizarObjetoRespuesta} 
                                tiposIdentificacion={tiposIdentificacion} 
                            />
                        </CardContent>
                    </Card>
                </Grid>
            </div>
            <div className={classes.marginTops}>
                {' '}
            </div>
        </>
    )
}

export default DetalleCuentascliente;