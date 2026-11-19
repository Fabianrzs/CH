import { createStyles, Grid, makeStyles, Theme, } from "@material-ui/core";
import MaterialTable from "material-table";
import CheckIcon from '@material-ui/icons/Check';
import React from "react";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        checkContainer: {
            width: '100%',
            alignItems: 'center',
            paddingRight: 30,
            color: theme.palette.success.main,
        },
        inactiveContainer: {
            width: '100%',
            alignItems: 'center',
            paddingRight: 10,
            color: '#C44',
        },
    }),
);

function TablasSeguros(props: any) {

 

   const  handleOpenModalEditar = (item: any) => {
    obtenerAseguradorasParaActulizar(item)
   }

    const {
        aseguradoras,
        obtenerAseguradorasParaActulizar,
         } = props;

         const columns: any = [
            {
                title: 'Razon Social',
                field: 'razonSocial',
                align: 'center',
                cellStyle: {
                    border: '1px solid #FFF',
                    color: '#676767',
                },
                headerStyle: {
                    borderTopLeftRadius: '12px',
                    borderTopRightRadius: '0px',
                },
            },
            {
                title: 'Convenio',
                field: 'convenio',
                align: 'center',
                cellStyle: {
                    border: '1.5px solid #FFF',
                    color: '#676767',
                },
                headerStyle: {
                    borderTopRightRadius: '0px',
                },
                render: (rowData: any) => (
                    <>
                        {rowData.convenio === "N" ? (
                            <span className={classes.checkContainer}>
                                <b>
                                     NO
                                </b>
                            </span>
                        ) : (
                            <span className={classes.inactiveContainer}>
                                <b>SI</b>
                            </span>
                        )}
                    </>
                ),
            },
            {
                title: 'Estado',
                field: 'estado',
                align: 'center',
                cellStyle: {
                    border: '1.5px solid #FFF',
                    color: '#676767',
                },
                headerStyle: {
                    borderTopRightRadius: '0px',
                },
                render: (rowData: any) => (
                    <>
                        {rowData.estado === 1 ? (
                            <span className={classes.checkContainer}>
                                <b>
                                    <CheckIcon /> ACTIVO
                                </b>
                            </span>
                        ) : (
                            <span className={classes.inactiveContainer}>
                                <b>INACTIVO</b>
                            </span>
                        )}
                    </>
                ),
            },
            {
                title: 'Tipo de Seguro',
                field: 'ramo',
                align: 'center',
                cellStyle: {
                    border: '1.5px solid #FFF',
                    color: '#676767',
                },
                headerStyle: {
                    borderTopRightRadius: '0px',
                },
                render: (rowData: any) => (
                    <>
                        {rowData.ramo === "V" ? (
                            <span className={classes.checkContainer}>
                                <b>
                                     VIDA
                                </b>
                            </span>
                        ) : (
                            <span className={classes.inactiveContainer}>
                                <b>HOGAR</b>
                            </span>
                        )}
                    </>
                ),
            },
            
        ];

        const intercalar = (index: number) => {
            let color = '#f1f2f6';
            if (index % 2 === 0) {
                color = '#C4C4C4';
            }
    
            return {
                background: color,
                border: '1.5px solid #FFF',
            };
        };
    
        const classes = useStyles();
       
    return (
        <>   <Grid container>
        <Grid item xs={12}>
            <MaterialTable
                style={{
                    borderTopLeftRadius: '12px',
                    borderTopRightRadius: '12px',
                }}
                title="Seguros Hogar y Vida Guardados"
                data={aseguradoras}
                columns={columns}
                actions={[
                  
                    {
                        icon: 'edit',
                        tooltip: 'Editar',
                        onClick: (event, rowData: any) => {
                            handleOpenModalEditar(rowData);
                        },
                    },
                    
                ]}
                options={{
                    actionsColumnIndex: -1,
                    headerStyle: {
                        backgroundColor: '#FEC917',
                        fontWeight: 'bold',
                        color: '#676767',
                        borderColor: '#FFF',
                        border: '1.5px solid #FFF',
                        borderTopRightRadius: '12px',
                    },
                    pageSizeOptions: [10, 20, 30],
                    pageSize: 10,
                    rowStyle: (data, index) => intercalar(index),
                }}
                localization={{
                    header: {
                        actions: 'Acciones',
                    },
                    toolbar: {
                        searchPlaceholder: 'Buscar',
                    },
                }}
            />
        </Grid>
    </Grid> </>
    )
}

export default TablasSeguros;