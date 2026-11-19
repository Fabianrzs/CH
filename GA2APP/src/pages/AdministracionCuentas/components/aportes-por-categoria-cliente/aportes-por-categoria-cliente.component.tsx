import React, { } from 'react';
import { createStyles, Grid, makeStyles, Card, CardContent } from "@material-ui/core";
import ContainerTitle from '../../../../components/containe-title/container-title';
import TableCategoriaAfiliado from '../tables-cuentas-afiliado/table-categoria-afiliado';
import { AdminCuentasAfiliado } from '../../../../types/client.model.type';


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


function AportesCategoriaPorcliente({
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
                            title="Aportes por categoría del afiliado"
                        />
                        <CardContent>

                            <TableCategoriaAfiliado responseAdminCuenta={responseAdminCuenta} />

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

export default AportesCategoriaPorcliente;