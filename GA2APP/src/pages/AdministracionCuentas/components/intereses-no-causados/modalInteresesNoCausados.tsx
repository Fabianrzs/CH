import React, { useEffect, useRef, useState } from 'react';
import moment, { months } from 'moment';
import { toast } from 'react-toastify';

import MUIDataTable from 'mui-datatables';
import {
    Button,
    createStyles,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    Typography,
    CircularProgress,
    FormControl,
    InputLabel,
    MenuItem,
    Select,
    Card,
    DialogTitle
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';

import { InteresNoCausadoDto } from '../../../../types/verificacion.model.type';
import ApiVerificacionDerecho from '../../../../services/api-verificacion.service';

import UtilService from '../../../../utils/util.service';
import CHTextFieldV2 from '../../../../components/ch-text-field/CHTextFieldV2';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHDatePickerV3 from '../../../../components/ch-datepicker/CHDatepickerV3';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import ContainerTitle from '../../../../components/containe-title/container-title';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import CHDatePickerV4 from '../../../../components/ch-datepicker/CHDatePickerV4';
import { LoadingV2Component } from '../../../../components/loading/loadingV2.component';





const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.paper,
    },
    buttonprincipal: {
        width: '250px',
        height: '50px',
        marginLeft: '1rem',
        background: '#FEC917',
    },
    buttonsecundario: {
        width: '250px',
        height: '50px',
        marginLeft: '1rem',
        background: '#43b5f1',
    },
    titleform: {
        width: '100%',
        textAlign: 'center',
        color: '#1C3F6B',
        padding: 1,
        fontWeight: 'bold',
        fontSize: '1rem',
    },
    column: {
        padding: '10px',
        textAlign: 'center'
    },
    select: {

        textAlign: 'left',
        paddingRight: '100px'

    },
}));

function CierreTable({
    data, 
    mostrarTabla
}:{
    data: any[],
    mostrarTabla: any,
}) {
    const columns = [

        {
            name: "numeroSolicitud",
            label: 'ID Solicitud',
            options: {


                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },


        {
            name: 'tipoSolicitud',
            label: 'Tipo Solicitud',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },


        },
        {
            name: 'tipoConcepto',
            label: 'Concepto',
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
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {

                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const sinDecimal = formattedValue; // .split(',')[0]
                    const ctaNumeroModificado = `$${sinDecimal}`;
                    return ctaNumeroModificado;
                }

            },

        },
        {
            name: 'periodo',
            label: 'Periodo',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },

        },
        {
            name: 'solicitudModo',
            label: 'Solicitud Pago/Abono',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },

        },
        {
            name: 'tipoInteres',
            label: 'Causado / No Causado',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },

        },
        {
            name: 'fechaSolicitud',
            label: 'Fecha Solicitud Pago/Abono',
            options: {
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),

            },

        },
    ];

    return (

        mostrarTabla && (
            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    textLabels: {
                        body: {
                            noMatch: 'No se encontraron resultados',
                        },
                        pagination: {
                            displayRows: 'de',
                            next: 'Siguiente',
                            previous: 'Anterior',
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
                }}
            />
        )



    );

}



export const ModalInteresesNoCausados = (props: any) => {



    const classes = useStyles();
    const {
        setLoading,
        open,
        onClose,
        responseAdminCuenta,
        formularioInterecesNoCausados,
        idCliente,
        cuentas,
        concept,
        total,
        actualizaDom,
        setActualizaDom


    } = props;


    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [conceptoSeleccionado, setConceptoSeleccionado] = useState('');
    const [filtroTitulo, setFiltroTitulo] = useState('');

    const currentYear = new Date().getFullYear();
    const years = Array.from({ length: currentYear - 1900 + 1 }, (_, index) => currentYear - index);

    const [selectedYear, setSelectedYear] = useState(currentYear);
    const [totalInteresesCausados, setTotalInteresesCausados] = useState(0);
    const [concepto, setConcepto] = useState('');
    const [mostrarTabla, setMostrarTabla] = useState(true);
    const [dataCierreMes, setDataCierreMes] = useState<any[]>([]);
    const ApiVerificacion = ApiVerificacionDerecho();
    const [conceptosFinal, setConceptosFinal] = useState<any[]>([]);
  
    useEffect(() => {
        if(actualizaDom === true ){
            ActualizarSelect()    
            showData()
            setActualizaDom(false)
        }

    }, [ actualizaDom ])

    const showData = async () => {
        
        setIsLocalLoading(true);

        const IDENTIFICACION = responseAdminCuenta.identificacion;

        await ApiVerificacion
            .ObtenerTodoInteresesNoCausadosByIdentificacionRequest(IDENTIFICACION)
            .then(async (response: any) => {

                if (response) {
                    if (response.data !== null) {
                        if (response.isSuccess) {
                            setFiltroTitulo(`COMPLETO`)
                            const dataOrdenada = response.data.sort((a: any, b: any) => b.periodo.localeCompare(a.periodo));
                            const sumaValores = dataOrdenada.reduce((total:any, item:any) => total + item.valor, 0);                           
                            setDataCierreMes(dataOrdenada)
                            formularioInterecesNoCausados.setValue('interesesNoCausados', sumaValores)
                            formularioInterecesNoCausados.setValue('saldoTotalInteresNoCausado', sumaValores + total)
                       

                            setDataCierreMes(dataOrdenada)
                            setMostrarTabla(true)

                        }
                    } else {
                        toast(`No se encontraron resultados`, { type: toast.TYPE.ERROR });
                        setMostrarTabla(false)
                    }

                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            }).finally(() => setIsLocalLoading(false));



    }

    const ActualizarSelect = async () => {

        const concepts: { conceptoId: number, descripcionConcepto: string, tipoCuentaId: number, cuentaId: number }[] = [];

        cuentas.forEach((cuenta: any) => {
            cuenta.conceptos.forEach((concepto: any) => {
                concepts.push({
                    conceptoId: concepto.conceptoId,
                    descripcionConcepto: concepto.descripcionConcepto,
                    tipoCuentaId: cuenta.tipoCuentaId,
                    cuentaId: cuenta.cuentaId
                });
            });
        });

        const datos = [
            { conceptoId: 0, descripcionConcepto: '0', tipoCuentaId: 0 },
            { conceptoId: 9, descripcionConcepto: 'Intereses Aportes', tipoCuentaId: 1 },
            { conceptoId: 10, descripcionConcepto: 'Intereses Cesantías', tipoCuentaId: 1 },
            { conceptoId: 10, descripcionConcepto: 'Intereses Cesantías', tipoCuentaId: 2 },
            { conceptoId: 29, descripcionConcepto: 'Intereses Ahorro Voluntario Contractual', tipoCuentaId: 5 },
            { conceptoId: 26, descripcionConcepto: 'Intereses Ahorro Voluntario Veterano', tipoCuentaId: 7 },
            { conceptoId: 24, descripcionConcepto: 'Intereses Ahorro Voluntario Afiliado Cesantías', tipoCuentaId: 6 },

        ];

        concepts.forEach((concepto: { conceptoId: number, descripcionConcepto: string, tipoCuentaId: number }) => {
            const existe = datos.some(dato =>
                dato.conceptoId === concepto.conceptoId && dato.tipoCuentaId === concepto.tipoCuentaId
            );
        
            if (existe) {
                // Verificar si el concepto ya existe en conceptosFinal antes de agregarlo
                if (!conceptosFinal.some(existingConcepto => existingConcepto.conceptoId === concepto.conceptoId && existingConcepto.tipoCuentaId === concepto.tipoCuentaId)) {
                    setConceptosFinal(prevConceptosFinal => [...prevConceptosFinal, concepto]);
                    conceptosFinal.push(concepto);
                }
            }
        });


    }

    const handleConceptoChange = async (event: any) => {
        setIsLocalLoading(true);

        const selectedValue = event.target.value;
        setConceptoSeleccionado(selectedValue);
        const selectedConcept = Number(selectedValue);
        const selectedConcepto = conceptosFinal.find(concepto => concepto.conceptoId === selectedConcept);
        const selectedCuentaId = selectedConcepto.cuentaId;
        const selectedTipoCuentaId = selectedConcepto.tipoCuentaId;
        const primerCuentaId = selectedCuentaId;
        const primerCuentaIdTipo = selectedTipoCuentaId;     
        setFiltroTitulo(` CONCEPTO: ${selectedConcepto?.descripcionConcepto}`)

        const request = ({

            anio: selectedYear,
            usuarioId: responseAdminCuenta.clienteId,
            concepto: selectedConcept,
            ctaId: primerCuentaId,
            tipoCuentaId: primerCuentaIdTipo,
            saldo: total,
            interesesNoCausados: 0,
            saldoTotalInteresNoCausado: 0
        });

        await ApiVerificacion
            .ObtenerInteresesNoCausadosRequest(request)
            .then(async (response: any) => {

                const datos = {
                    anio: moment(new Date()).year(),
                    usuarioId: response.data.usuarioId,
                    concepto: response.data.concepto,
                    ctaId: response.ctaId,
                    tipoCuentaId: response.data.tipoCuentaId,
                    saldo: response.data.saldo,
                    interesesNoCausados: response.data.interesesNoCausados,
                    saldoTotalInteresNoCausado: response.data.saldoTotalInteresNoCausado
                }

                formularioInterecesNoCausados.reset(datos);

            }).catch((error: any) => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });

            });

        const request1 = ({

            anio: selectedYear,
            usuarioId: responseAdminCuenta.identificacion,
            concepto: selectedConcept,
            ctaId: primerCuentaId,
            tipoCuentaId: primerCuentaIdTipo,
            saldo: total,
            interesesNoCausados: 0,
            saldoTotalInteresNoCausado: 0


        });

      

        await ApiVerificacion
            .ObtenerInteresesNoCausadosPorAnioYConceptoRequest(request1)
            .then(async (response: any) => {

                if (response) {
                    if (response.data !== null) {
                        if (response.isSuccess) {
                            const dataOrdenada = response.data.sort((a: any, b: any) => b.periodo.localeCompare(a.periodo));
                            const sumaValores = dataOrdenada.reduce((total:any, item:any) => total + item.valor, 0);                           
                             setDataCierreMes(dataOrdenada)
                            formularioInterecesNoCausados.setValue('interesesNoCausados', sumaValores)
                            // formularioInterecesNoCausados.setValue('saldoTotalInteresNoCausado', sumaValores + formularioInterecesNoCausados.getValues('saldoTotalInteresNoCausado'))

                            formularioInterecesNoCausados.setValue('interesesNoCausadosRender', '1')
                            setMostrarTabla(true)

                        }
                    } else {
                        toast(`No se encontraron resultados`, { type: toast.TYPE.ERROR });
                        setMostrarTabla(false)
                    }

                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            }).finally(() => setIsLocalLoading(false));





    };

    const handleYearChange = async (event: any) => {
        setIsLocalLoading(true);

        const selectedYear =  moment(event).year()
        setFiltroTitulo(` AÑO: ${selectedYear}`)

        setSelectedYear(selectedYear);
       
        setConceptoSeleccionado("");

        const primerCuentaId = cuentas[0].cuentaId;
        const primerCuentaIdTipo = cuentas[0].tipoCuentaId;


        const request = ({

            anio: selectedYear,
            usuarioId: responseAdminCuenta.identificacion,
            concepto: 0,
            ctaId: primerCuentaId,
            tipoCuentaId: primerCuentaIdTipo,
            saldo: total,
            interesesNoCausados: 0,
            saldoTotalInteresNoCausado: 0


        });

        await ApiVerificacion
            .ObtenerInteresesNoCausadosRequest(request)
            .then(async (response: any) => {

                const datos = {
                    anio: response.data.anio,
                    usuarioId: response.data.usuarioId,
                    concepto: 0,
                    ctaId: response.ctaId,
                    tipoCuentaId: response.data.tipoCuentaId,
                    saldo: response.data.saldo,
                    interesesNoCausados: response.data.interesesNoCausados,
                    saldoTotalInteresNoCausado: response.data.saldoTotalInteresNoCausado
                }

                formularioInterecesNoCausados.reset(datos);
      
            }).catch((error: any) => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });

            });


        await ApiVerificacion
            .ObtenerInteresesNoCausadosPorAnioRequest(request)
            .then(async (response: any) => {

                if (response) {
                    if (response.data !== null) {
                        if (response.isSuccess) {

                            const dataOrdenada = response.data.sort((a: any, b: any) => b.periodo.localeCompare(a.periodo));
                            const sumaValores = dataOrdenada.reduce((total:any, item:any) => total + item.valor, 0);                            
                            setDataCierreMes(dataOrdenada)
                            formularioInterecesNoCausados.setValue('interesesNoCausados', sumaValores)
                            // formularioInterecesNoCausados.setValue('saldoTotalInteresNoCausado', sumaValores + formularioInterecesNoCausados.getValues('saldoTotalInteresNoCausado'))

                            setDataCierreMes(dataOrdenada)
                            setMostrarTabla(true)

                        }
                    } else {
                        toast(`No se encontraron resultados`, { type: toast.TYPE.ERROR });
                        setMostrarTabla(false)
                    }

                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            }).finally(() => setIsLocalLoading(false));


    };

    const handleConsultaInteresesNC = async (event: any) => {
        
        formularioInterecesNoCausados.reset();
        showData()

        // formularioInterecesNoCausados.setValue('saldoTotalInteresNoCausado', total +
        //                                         formularioInterecesNoCausados.getValues('interesesNoCausados'))


    }


 
    return (
        
    <Dialog fullWidth maxWidth="lg" open={open} onClose={onClose} aria-labelledby="form-dialog-title">
    <LoadingV2Component key="loader" loading={isLocalLoading} tittle='Procesando...' />

    <IconTitle title='INTERESES NO CAUSADOS' icon='fch-Buscar-0-1' />
    {/* <DialogTitle>RESUMEN DE PAGO</DialogTitle> */}

        <DialogContent>
            <ContainerTitle title="CONSULTA INTERESES" />
            <Grid container spacing={2} alignItems="center" justify="center">
                <Grid item md={6}>
                    <Controller
                    name="concepto"
                    control={formularioInterecesNoCausados.control}
                    rules={{
                        required: 'Este campo es requerido.',
                    }}
                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                        <CHSelect
                        error={!!error}
                        label="Concepto"
                        helperText={error ? error.message : null}
                        
                        value={value}
                        onChange={(newValue) => {
                            onChange(newValue);
                            handleConceptoChange(newValue); // Make sure this function is correctly updating the state
                        }}
                        
                        // onChange={handleConceptoChange}

                        >
                        {conceptosFinal.map((option) => (
                            <MenuItem key={option.conceptoId} value={option.conceptoId}>
                            {option.descripcionConcepto}
                            </MenuItem>
                        ))}
                        </CHSelect>
                    )}
                    />
                </Grid>
            </Grid>

            <Grid container spacing={2} alignItems="center" justify="center">
                <Grid item md={6}>
                    <Controller
                    name="anios"
                    control={formularioInterecesNoCausados.control}
                    rules={{
                        // required: 'Este campo es requerido.',
                        pattern: {
                        value: /^([1][0-9][2-9][2-9]|[2][0-9][0-9][0-9])([/.]|[-.])([0-9])([0-9])/g,
                        message: 'Fecha no válida',
                        },
                    }}
                    render={({ field: { onChange, value, onBlur }, fieldState: { error } }) => (
                        <CHDatePickerV4
                        views={['year']}
                        error={!!error}
                        invalidDateMessage="Fecha no válida"
                        maxDateMessage="La fecha no debe ser posterior a la fecha máxima"
                        minDateMessage="La fecha no debe ser anterior a la fecha mínima"
                        minDate={moment('1910').format('YYYY')}
                        label="Año "
                        value={value}
                        onChange={(newValue) => {
                            onChange(newValue);
                            handleYearChange(newValue); // Make sure this function is correctly updating the state
                        }}
                        onBlur={onBlur}
                        />
                    )}
                    />
                </Grid>
            </Grid>
            
            <br />

            <Grid container item xs={12} md={12} alignItems="center" justifyContent="center">
                <Button
                    variant="outlined"
                    // onClick={handleConsultaInteresesNC}
                    onClick={formularioInterecesNoCausados.handleSubmit(handleConsultaInteresesNC)}

                    style={{ marginRight: '3%', textAlign: 'center', display: 'block' }}
                >
                    ACTUALIZAR
                </Button>
                <Button
                    variant="outlined"
                    onClick={onClose}
                    style={{ marginRight: '0%', textAlign: 'center', display: 'block' }}
                >
                    CANCELAR
                </Button>
            </Grid>
    
          <ContainerTitle title= {`Historial Cuentas Verificadas ${filtroTitulo} `} />
    
            <Grid container spacing={2}>

                <Grid item md={6} className={classes.column}>
                <Controller
                    name="interesesNoCausados"
                    control={formularioInterecesNoCausados.control}
                    rules={{
                    required: 'Este campo es requerido',
                    }}
                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                    <CHTextField
                        label="Total Intereses No Causados:"
                        currencyMask
                        value={value}
                        onChange={onChange}
                        error={!!error}
                        helperText={error ? error.message : null}
                        InputLabelProps={{ shrink: true }}
                    />
                    )}
                />
                </Grid>
        
                <Grid item md={6} className={classes.column}>
                <Controller
                    name="saldoTotalInteresNoCausado"
                    control={formularioInterecesNoCausados.control}
                    rules={{
                    required: 'Este campo es requerido',
                    }}
                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                    <CHTextField
                        currencyMask
                        label="Saldo Cuenta con Intereses no Causados"
                        value={value}
                        onChange={onChange}
                        error={!!error}
                        helperText={error ? error.message : null}
                        InputLabelProps={{ shrink: true }}
                    />
                    )}
                />
                </Grid>

             </Grid>

            <CierreTable data={dataCierreMes} mostrarTabla={mostrarTabla} />

        </DialogContent>
      </Dialog>
    );
};