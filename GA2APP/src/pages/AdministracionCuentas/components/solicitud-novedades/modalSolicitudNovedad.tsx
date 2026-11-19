import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import MUIDataTable from 'mui-datatables';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    Typography,
    FormControl,
    InputLabel,
    MenuItem,
    Select
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHDatePickerV3 from '../../../../components/ch-datepicker/CHDatepickerV3';
import ApiSchedule from '../../../../services/api-cronograma.service';
import AuthService from '../../../../services/auth.service';

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
    actions: {
        marginLeft: '2rem',
        width: '20%',
    },
    loadingOverlay: {
        position: 'fixed',
        top: 0,
        left: 0,
        width: '100%',
        height: '100%',
        background: 'rgba(0, 0, 0, 0.5)',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        zIndex: 9999,
    },
    loadingMessage: {
        background: '#fff',
        padding: '16px',
        borderRadius: '4px',
        textAlign: 'center',
    },
}));



export const ModalSolicitudNovedad = (props: any) => {


    const classes = useStyles();
    const {
        open,
        onClose,
        responseAdminCuenta,
        idConceptoParam,
    } = props;



    const apiCronograma = ApiSchedule();
    const [conceptoNovedad, setConceptoNovedad] = useState('');
    const [modalidad, setModalidad] = useState('');

    const { control, watch, setValue, reset, handleSubmit } = useForm({
        defaultValues: {
            idTipoIdentificacion: 0,
            identificacion: '',
            nombreCliente: '',
            primerNombre: '',
            segundoNombre: '',
            primerApellido: '',
            segundoApellido: '',
            radicado: '',
            periodoNovedad: new Date(),
            conceptoNovedad: '',
            modalidad: '',
            codigoMilitar: '',
            idUnidadEjecutora: 0,
            idConceptoNovedad: 0,
            codigoAlta: 0,
            codigoBaja: 0,
            fechaInicioNovedad: new Date(),
            fechaFinNovedad: new Date(),
            porcentaje: 0,
            valor: 0,
            esAlta: false,
            esBaja: false,
            observaciones: '',
            token: ''
        }
    });


    const [esAltaChecked, setEsAltaChecked] = useState(false);
    const [esBajaChecked, setEsBajaChecked] = useState(false);
    const [showModalidad, setShowModalidad] = useState(true);
    const [fechaInicio, setFechaInicio] = useState<Date>();
    const [porcentaje1, setPorcentaje1] = useState('');
    const [porcentaje2, setPorcentaje2] = useState('');
    const [porcentaje3, setPorcentaje3] = useState('');
    const [porcentaje4, setPorcentaje4] = useState('');

    const showTokenField = (modalidad === "AHORRO VOLUNTARIO ADMINISTRACIÓN CESANTÍAS" && idConceptoParam === 3 || idConceptoParam === 6 || idConceptoParam === 22);
    const unidadEjecutoraId = responseAdminCuenta?.unidadEjecutoraId;


    let idTipoIdentificacion = 0;
    let numeroDocumento = '';
    let idModulo = 1;
    let token = '';


    const guardarNovedad = (data: any) => {
        data.idConceptoNovedad = modalidadIds[modalidad];
        data.esAlta = esAltaChecked;
        data.esBaja = esBajaChecked;
        idTipoIdentificacion = data.responseAdminCuenta?.tipoIdentificacion;
        numeroDocumento = data.identificacion;
        idModulo = data.idModulo;
        token = data.token;


        // const porcentajeSeleccionado = porcentajes[`${conceptoNovedad}-${modalidad}`].find(
        //     (item) => item.value.toString() === data.porcentaje
        // );


        // if (porcentajeSeleccionado) {
        //     data.porcentaje = porcentajeSeleccionado.label;
        // }

        apiCronograma.InsertarNovedadUsuario(data)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        toast('Novedad insertada correctamente');
                        await guardarLog(idTipoIdentificacion, numeroDocumento, fechaInicio, 'SOLICITUD DE NOVEDAD');
                        onClose();
                    } else {
                        toast('La novedad no se ingresó correctamente');
                    }
                }
            });
    };

    const guardarLog = async (tipoDocumento: any, nDocumento: any, fechaInicio: any, modulo: string) => {
        const user = AuthService.getUserInfo();
        await apiCronograma.InsertarAudoriaNovedades({
            idModulo: 1,
            modulo: modulo,
            tipoDocumento: tipoDocumento,
            nDocumento: nDocumento,
            detalleModificacion: `Se ha insertado una solicitud de novedad para el afiliado con número de cédula ${numeroDocumento}`,
            fechaModificacion: new Date(),
            usuarioModificacion: user.USR_ID,
            fechaInicio: fechaInicio,
            fechaFin: new Date(),
            token: token
        });
    };



    const handleConceptoNovedadChange = (event: any) => {
        const selectedValue = event.target.value;

        setEsAltaChecked(selectedValue === 'activacion');
        setEsBajaChecked(selectedValue === 'suspension' || selectedValue === 'suspension-incremento');

        setConceptoNovedad(selectedValue);

    };

    const modalidadIds: { [key: string]: number } = {
        'AHORRO OBLIGATORIO': 1,
        'AHORRO VOLUNTARIO': 2,
        'AHORRO VOLUNTARIO ADMINISTRACIÓN CESANTÍAS': 4,
        'AHORRO VOLUNTARIO AFILIADO VETERANO': 5,
        'APORTE VOLUNTARIO CONTRACTUAL': 6,
    };


    function getModalidadOptions(tipoSolicitud: any, unidadEjecutoraId: number | undefined) {
        const conceptos = [
            { concepto: 'AHORRO OBLIGATORIO', idMod: modalidadIds['AHORRO OBLIGATORIO'] },
            { concepto: 'AHORRO VOLUNTARIO', idMod: modalidadIds['AHORRO VOLUNTARIO'] },
            { concepto: 'AHORRO VOLUNTARIO ADMINISTRACIÓN CESANTÍAS', idMod: modalidadIds['AHORRO VOLUNTARIO ADMINISTRACIÓN CESANTÍAS'] },
            { concepto: 'AHORRO VOLUNTARIO AFILIADO VETERANO', idMod: modalidadIds['AHORRO VOLUNTARIO AFILIADO VETERANO'] },
            { concepto: 'APORTE VOLUNTARIO CONTRACTUAL', idMod: modalidadIds['APORTE VOLUNTARIO CONTRACTUAL'] },
        ];
    
        const excludeModalidad = unidadEjecutoraId !== undefined && ![5, 8, 12, 17].includes(unidadEjecutoraId);
        const excludeAVAC = unidadEjecutoraId === undefined || [5, 8, 12, 17].includes(unidadEjecutoraId);
    
        switch (tipoSolicitud) {
            case 'activacion':
            case 'suspension':
                return conceptos.filter(concepto => {
                    const modalidadCondition = excludeModalidad ? concepto.concepto !== 'AHORRO VOLUNTARIO AFILIADO VETERANO' : true;
                    const avacCondition = excludeAVAC ? concepto.concepto !== 'AHORRO VOLUNTARIO ADMINISTRACIÓN CESANTÍAS' : true;
                    
                    return modalidadCondition && avacCondition;
                });                
            case 'suspension-incremento':
                return conceptos.filter(concepto => concepto.concepto === 'AHORRO VOLUNTARIO');
            case 'novedadFondo':
                return ['Nueva Afiliacion', 'Re Afiliacion', 'No Operó'];
            case 'incremento':
                return ['Incremento De Ahorro', 'Presencial'];
            default:
                return [];
        }
    }
    
    
    type Modalidad =
        | "AHORRO VOLUNTARIO ADMINISTRACIÓN CESANTÍAS"
        | "APORTE VOLUNTARIO CONTRACTUAL"
        | "AHORRO VOLUNTARIO AFILIADO VETERANO"
        | "AHORRO OBLIGATORIO";


    type PorcentajeOption = { value: number; label: string };

    const porcentajesPorModalidad: Record<Modalidad, PorcentajeOption[]> = {
        "AHORRO VOLUNTARIO ADMINISTRACIÓN CESANTÍAS": [
            { value: 8, label: "8%" },
            { value: 10, label: "10%" },
        ],
        "APORTE VOLUNTARIO CONTRACTUAL": [
            { value: 8, label: "8%" },
            { value: 10, label: "10%" },
        ],
        "AHORRO VOLUNTARIO AFILIADO VETERANO": [
            { value: 4.5, label: "4.5%" },
            { value: 8, label: "8%" },
            { value: 10, label: "10%" },
        ],
        "AHORRO OBLIGATORIO": [
            { value: 4.5, label: "4.5%" },
            { value: 7, label: "7%" },
            { value: 8, label: "8%" },
        ],
    };

    const getPorcentajesOptions = (modalidad: Modalidad): PorcentajeOption[] =>
        porcentajesPorModalidad[modalidad] || [];


    const porcentajes: Record<string, { value: number; label: string }[]> = {
        "activacion-AHORRO VOLUNTARIO ADMINISTRACIÓN CESANTÍAS": [
            { value: 8, label: "8%" },
            { value: 10, label: "10%" },
        ],
        "activacion-APORTE VOLUNTARIO CONTRACTUAL": [
            { value: 8, label: "8%" },
            { value: 10, label: "10%" },
        ],
        "activacion-AHORRO VOLUNTARIO AFILIADO VETERANO": [
            { value: 4.5, label: "4.5%" },
            { value: 8, label: "8%" },
            { value: 10, label: "10%" },
        ],
        "activacion-AHORRO OBLIGATORIO": [
            { value: 4.5, label: "4.5%" },
            { value: 7, label: "7%" },
            { value: 8, label: "8%" },
        ],
    };

    const combinacionKey = `${conceptoNovedad}-${modalidad}-${modalidadIds[modalidad]}`;
    const porcentajesParaCombinacion = porcentajes[combinacionKey] || [];

    const validacion = () => {
        if (
            modalidad === "AHORRO VOLUNTARIO AFILIADO VETERANO" &&
            [1, 2, 6, 7, 9, 10, 11, 13, 14, 15, 18, 19, 20, 21, 22].includes(idConceptoParam)
        ) {
            setShowModalidad(false);
        } else {
            setShowModalidad(true);
        }
    };

    useEffect(() => {
        validacion();
    }, [modalidad, idConceptoParam]);



    return (

        <Dialog
            fullWidth
            maxWidth="lg"
            open={open}
            onClose={onClose}
            aria-labelledby="form-dialog-title"
        >
            <DialogContent>
                <Grid item>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        SOLICITUD DE NOVEDAD
                    </Typography>
                </Grid>
                <br />
                <Grid container justify="center">
                    <Grid item xs={12}>
                        <Typography className={classes.titleform} variant="h5">
                            Información del Afiliado
                        </Typography>
                    </Grid>
                </Grid>
                <Grid container spacing={2}>
                    <Grid item xs={6}>
                        <Controller
                            name="idTipoIdentificacion"
                            control={control}
                            render={({ field: { onChange, value } }) => (

                                <CHTextField
                                    label="Tipo de identificación"
                                    isReadOnly
                                    value={responseAdminCuenta.tipoIdentificacion}

                                />
                            )}
                        />

                    </Grid>
                    <Grid item xs={6}>
                        <Controller
                            name="identificacion"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (

                                <CHTextField
                                    label="Numero de identificación"
                                    isReadOnly
                                    value={responseAdminCuenta.identificacion}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <Controller
                            name="primerNombre"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    label="Primer Nombre"
                                    isReadOnly
                                    value={responseAdminCuenta.primerNombre}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <Controller
                            name="segundoNombre"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    label="Segundo Nombre"
                                    isReadOnly
                                    value={responseAdminCuenta.segundoNombre}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <Controller
                            name="primerApellido"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    label="Primer Apellido"
                                    isReadOnly
                                    value={responseAdminCuenta.primerApellido}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <Controller
                            name="segundoApellido"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    label="Segundo Apellido"
                                    isReadOnly
                                    value={responseAdminCuenta.segundoApellido}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item xs={6}>
                        <Controller
                            name="radicado"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (
                                <CHTextField label="Radicado" />
                            )}
                        />
                    </Grid>

                    <Grid item xs={6}>
                        <Controller
                            name="periodoNovedad"
                            control={control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePickerV3
                                    views={['month', 'year']}
                                    invalidDateMessage=""
                                    label="Periodo de Novedad"
                                    value={value}
                                    onChange={onChange}
                                    format="MM/yyyy"

                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <Controller
                            name="conceptoNovedad"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (

                                <FormControl className={classes.select} style={{ width: '115%', paddingRight: '100px', marginBottom: '20px' }}>
                                    <InputLabel>Tipo Solicitud</InputLabel>
                                    <Select
                                        value={conceptoNovedad}
                                        onChange={handleConceptoNovedadChange}
                                    >
                                        <MenuItem value="activacion">Activación Ahorro</MenuItem>
                                        <MenuItem value="suspension">Suspensión Ahorro</MenuItem>
                                        <MenuItem value="suspension-incremento">Suspensión de Incremento de Ahorro</MenuItem>
                                        <MenuItem value="novedadFondo">Novedad Fondo Solidaridad</MenuItem>
                                        <MenuItem value="incremento">Incremento de Ahorro</MenuItem>
                                    </Select>
                                </FormControl>
                            )}
                        />
                    </Grid>

                    <Grid item xs={6}>
                        <Controller
                            name="idConceptoNovedad"
                            control={control}
                            render={({ field: { onChange, value } }) => (

                                <FormControl className={classes.select} style={{ width: '115%', paddingRight: '100px', marginBottom: '20px' }}>
                                    <InputLabel>Modalidad</InputLabel>
                                    <Select
                                        value={modalidad}
                                        onChange={(event) => {
                                            const selectedModalidad = event.target.value as string;
                                            setModalidad(selectedModalidad);
                                        }}
                                    >
                                        {getModalidadOptions(conceptoNovedad, unidadEjecutoraId).map((option) => (
                                            <MenuItem key={typeof option === 'string' ? option : option.idMod} value={typeof option === 'string' ? option : option.concepto}>
                                                {typeof option === 'string' ? option : option.concepto}
                                            </MenuItem>
                                        ))}

                                    </Select>
                                </FormControl>
                            )}
                        />
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item xs={6}>
                            {(porcentajesParaCombinacion.length > 0) && (
                                <FormControl className={classes.select} style={{ width: '115%', paddingRight: '100px', marginBottom: '20px' }}>
                                    <InputLabel>Porcentaje</InputLabel>
                                    <Select
                                        value={porcentaje1}
                                        onChange={(event) => setPorcentaje1(event.target.value as string)}
                                    >
                                        {porcentajesParaCombinacion.map((porcentajeItem) => (
                                            <MenuItem key={porcentajeItem.value} value={porcentajeItem.value}>
                                                {porcentajeItem.label}
                                            </MenuItem>
                                        ))}
                                    </Select>

                                </FormControl>
                            )}
                        </Grid>
                        {(conceptoNovedad === "activacion" && typeof modalidad === 'string') && (
                            <Grid item xs={6}>
                                <FormControl className={classes.select} style={{ width: '115%', paddingRight: '100px', marginBottom: '20px' }}>
                                    <InputLabel>Porcentaje</InputLabel>
                                    <Select
                                        value={porcentaje2}
                                        onChange={(event) => setPorcentaje2(event.target.value as string)}
                                    >
                                        {getPorcentajesOptions(modalidad as Modalidad).map(option => (
                                            <MenuItem key={option.value} value={option.value}>{option.label}</MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Grid>
                        )}


                        {(conceptoNovedad === "incremento" && modalidad === "Incremento De Ahorro" || modalidad === "Presencial") && (
                            <Grid item xs={6}>
                                <FormControl className={classes.select} style={{ width: '115%', paddingRight: '100px', marginBottom: '20px' }}>
                                    <InputLabel>Porcentaje</InputLabel>
                                    <Select
                                        value={porcentaje3}
                                        onChange={(event) => setPorcentaje3(event.target.value as string)}
                                    >
                                        <MenuItem value={2}>2%</MenuItem>
                                        <MenuItem value={3}>3%</MenuItem>
                                    </Select>
                                </FormControl>
                            </Grid>
                        )}
                        {(conceptoNovedad === "novedadFondo" && modalidad === "Nueva Afiliacion" || modalidad === 'Re Afiliacion' || modalidad === 'No Operó') && (
                            <Grid item xs={6}>
                                <FormControl className={classes.select} style={{ width: '115%', paddingRight: '100px', marginBottom: '20px' }}>
                                    <InputLabel>Porcentaje</InputLabel>
                                    <Select
                                        value={porcentaje4}
                                        onChange={(event) => setPorcentaje4(event.target.value as string)}
                                    >
                                        <MenuItem value={7}>7%</MenuItem>
                                    </Select>
                                </FormControl>
                            </Grid>
                        )}
                        {showTokenField && (
                            <Grid className={classes.column} item xs={6}>
                                <Controller
                                    name="token"
                                    control={control}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                            label="Token"
                                            value={value}
                                            onChange={onChange}
                                        />
                                    )}
                                />
                                <Button variant="contained" color="primary">
                                    TOKEN
                                </Button>
                            </Grid>
                        )}
                    </Grid>
                    <Grid item xs={6}>
                        <Controller
                            name="observaciones"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    label="Observaciones"
                                    fullWidth
                                    multiline
                                    rows={4}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container justify="flex-end" spacing={2}>
                    <Grid item>
                        <Button className={classes.buttonprincipal} onClick={onClose}>Regresar</Button>
                    </Grid>
                    <Grid item>
                        <Button className={classes.buttonsecundario} onClick={handleSubmit(guardarNovedad)}>Aceptar</Button>
                    </Grid>
                </Grid>
            </DialogContent>

        </Dialog >
    )

};