import { Button, Card, CardContent, Checkbox, CircularProgress, createStyles, Grid, IconButton, ListItemText, makeStyles, MenuItem, OutlinedInput, Select, Tooltip } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable, { MUIDataTableColumnDef } from 'mui-datatables';
import moment from 'moment';
import { CloudDownload } from '@material-ui/icons';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import Header from '../../components/header/Header';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import CreditApi from '../../services/api-credit.service';
import UtilService from '../../utils/util.service';  
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import CHTextField from '../../components/ch-text-field/CHTextField';
import ApiproductoClients from '../../services/api-producto.service';
import CHAutoComplete from '../../components/ch-auto-complete/CHAutoComplete';

const useStyles = makeStyles(() =>
    createStyles({
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917',
        },
        columnDate: {
            minWidth: '110px',
        },

        tablaCierre: {
            overflow: 'auto !important'
        },
        buttonsecundario: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#43b5f1',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
            alignItems: 'flex-end',
            justifyContent: 'center',
            display: 'flex',
        },
        nombreCuentaContable: {
            minHeight: '200px'
        },
        errorMessage: {
            color: 'red'
        }
    }),
);

interface tableProps {
    data: any[],
    mostrarTabla: boolean,
    handleExportToExcel: () => Promise<void>
}


function TablaReporteContable(props: tableProps) {

    const { data, mostrarTabla, handleExportToExcel } = props;
    const classes = useStyles();
    const [downloading, setDownloading] = useState(false);

    const columns: MUIDataTableColumnDef[] = [
        {
            name: 'fechaMovimiento',
            label: 'Fecha Movimiento',
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                    style: {
                        textAlign: 'left',
                    },
                    
                }),
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
               
            },
        
        },

        {
            name: 'comprobante',
            label: 'Comprobante',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },

        {
            name: 'tipoComprobante',
            label: 'Tipo Comprobante',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },

        {
            name: 'detalle',
            label: 'Detalle',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },

        {
            name: 'movimiento',
            label: 'Movimiento',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },

        {
            name: 'valor',
            label: 'Valor',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right'
                           
                        },
                    }),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            }
        },

        {
            name: 'saldo',
            label: 'Saldo',
            options: {
                customBodyRender: (value: number | bigint) =>
                value ? UtilService.formatCurrency(value) : null,
                    setCellProps: () => ({
                        style: {
                            textAlign: 'right'
                           
                        },
                    }),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            }
        },
        {
            name: 'tercero',
            label: 'T.Ben',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            }
        },
        {
            name: 'tipoIdentificacion',
            label: 'Tipo Identificación',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            }
        },
        {
            name: 'numeroIdentificacion',
            label: 'Número Identificación',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            }
        },
        {
            name: 'nombreTercero',
            label: 'Nombre',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                  setCellProps: () => ({
                    style: {minWidth: "200px"}
                })
            }
        },
        {
            name: 'numeroCuentaContable',
            label: 'Número Cuenta Contable',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas',
                }),
            }
        },
        {
            name: 'nombreCuentaContable',
            label: 'Nombre Cuenta Contable',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                setCellProps: () => ({
                    style: {minWidth: "300px"}
                })
            }
        },


        
    ];

    const handleClick = async () => {
        try {
            setDownloading(true);
            await handleExportToExcel();
        } finally {
            setDownloading(false);
        }
    }
 
    if(mostrarTabla) {
        return  <MUIDataTable
                    title=""
                    data={data}
                    columns={columns}

                    options={{
                        selectableRows: 'none',
                        filter: false,
                        print: false,
                        elevation: 2,
                        filterType: 'dropdown',
                        responsive: 'vertical',
                        fixedHeader: true,
                        fixedSelectColumn: true,
                        tableBodyHeight: '470px',
                        rowsPerPageOptions: [5, 15, 20, 100, 200, 500 ],
                        textLabels: {
                            body: {
                                noMatch: 'No se encontraron resultados',
                            },
                            pagination: {
                                rowsPerPage: 'Filas por página',
                                jumpToPage: '',
                            },
                            toolbar: {
                                downloadCsv: 'Descargar CSV',
                                search: 'Buscar',
                                viewColumns: 'Mostrar Columnas',
                            },
                            viewColumns: {
                                title: 'Mostrar Columnas',
                            },
                        },
                        download: false,
                        customToolbar: () => (
                            
                                <Tooltip title="Descargar Excel">
                                    {
                                        downloading
                                            ? <CircularProgress size={20} style={{'verticalAlign': 'middle', 'marginLeft': '14px', 'marginRight': '14px'}}/>
                                            
                                            :  <IconButton onClick={handleClick} >
                                                    <CloudDownload/>
                                                </IconButton>
                                    }
                                </Tooltip>
                            
                        )
                    }}
                />
    }

    return <></>;
}

const InformacionContable = () => {

    const classes = useStyles();
    const api = ApiproductoClients();
    const [request, setRequest] = useState<any>(null);
    const [isProccessing, setProcessing] = useState(false);
    const [mostrarTabla, setMostrarTabla] = useState(false);
    const [data, setData] = useState([]);
    const [cuentasContables, setCuentasContables] = useState<any[]>([]);

    const defaultValues: {
        fechaInicial: Date,
        fechaFinal: Date,
        cuentaContable: string
    } = {
        fechaInicial:  new Date(),
        fechaFinal: new Date(),
        cuentaContable: ''
    };

    
    useEffect(() => {
        api.getEquivalenciaCuentasContables().then(x => {
            if(x.isSuccess) {
                setCuentasContables(x.data);
            }
            console.log(x.data);
        });
    }, []);

    const formControls = useForm({
        'mode': 'onChange',
        defaultValues: defaultValues,
    });

    const validateDate = (value: any) =>
        !moment(value).isValid()
            ? 'Seleccione una fecha valida'
            : undefined;

    const onSubmit = async (request: any) => {
        if(formControls.formState.isValid) {            
            setProcessing(true);
            setMostrarTabla(false);

            api.getReporteInformacionContable(
                request.fechaInicial,
                request.fechaFinal,
                request.cuentaContable?.cuentaContableSeven,
            )
            .then(x => {
                console.log(x);
                setData(x.data);
                setMostrarTabla(true);
                setRequest(request);
            })
            .finally(() => setProcessing(false));
        }
    };

    const handleExportToExcel = async () => {

        const response = await api.generarExcelReporteInformacionContable(
            request.fechaInicial,
            request.fechaFinal,
            request.cuentaContable?.cuentaContableSeven,
        );

        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `Reporte Informacion Contable - Fecha ${moment(request.fechaInicial).format('YYYY-MM-DD')} a ${moment(request.fechaFinal).format('YYYY-MM-DD')}.xlsx`;
        document.body.appendChild(link);
        link.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(link);
    }

    return (<div  
        style={{
            display: 'table',
            tableLayout: 'fixed',
            width: '100%',
            
        }}>
         <Header />
         <CHPrivateContainer>
            <IconTitle
                title="Información Contable"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid container>
                <Grid item xs={12} >
                    <Card>
                        <CardContent>
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="fechaInicial"
                                        control={formControls.control}
                                        rules={{
                                            required: 'Se requiere la Fecha Inicial',
                                            validate: validateDate
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <div>
                                                <CHDatePicker
                                                    error={!!error}
                                                    label="Fecha inicial"
                                                    value={value}
                                                    onChange={onChange}
                                                />
                                                <span className={classes.errorMessage}>{error?.message}</span>
                                            </div>                                         
                                        )}                                        
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="fechaFinal"
                                        control={formControls.control}
                                        rules={{
                                            required: 'Se requiere la Fecha Final',
                                            validate: validateDate
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <div>
                                                <CHDatePicker
                                                    error={!!error}
                                                    label="Fecha final"
                                                    value={value}
                                                    onChange={onChange}
                                                />  
                                                <span className={classes.errorMessage}>{error?.message}</span>
                                            </div>                               
                                        )}                                        
                                    />
                                </Grid>
                                <Grid item xs={5}>
                                    <br />
                                    <Controller
                                            name="cuentaContable"
                                            control={formControls.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHAutoComplete
                                                    autoComplete
                                                    options={cuentasContables}
                                                    value={value || null}
                                                    getOptionSelected={(a, b) => a?.cuentaContableSeven === b?.cuentaContableSeven}
                                                    onChange={(event, item) => onChange(item)}
                                                    getOptionLabel={(option) => option.cuentaContableSeven ? `${option.cuentaContableSeven} - ${option.descripcionCuenta}` : ''}
                                                    renderInput={(params) =>
                                                        <CHTextField
                                                            {...params}
                                                            label="Cuenta Contable"
                                                            error={!!error}
                                                            helperText={error ? error.message : null}
                                                        />
                                                    }
                                                />
                                            )}
                                        />
                                </Grid>
                            </Grid>
                            <br />
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
            <Grid className="buttonssolicitud">
                <Button
                    type='submit'
                    className={classes.buttonprincipal}
                    variant="outlined"
                    disabled={!formControls.formState.isValid}
                    onClick={formControls.handleSubmit(onSubmit)}>
                    Buscar
                </Button>
            </Grid>
            <br />
            {
                isProccessing && <CHLoadingProcess />
            }
            <div className='MuiTableCell-stickyHeader; jss86'>
                <TablaReporteContable {...{data, mostrarTabla, handleExportToExcel}}/>
            </div>
            </CHPrivateContainer>
        </div>)
}

export default InformacionContable;