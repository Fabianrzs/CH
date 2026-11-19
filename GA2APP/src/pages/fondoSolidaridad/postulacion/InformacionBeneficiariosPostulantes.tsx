import React, {useEffect, useState} from 'react';
import {
    Button,
    createStyles,
    Grid,
    IconButton,
    makeStyles,
    MenuItem,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    Typography,
} from '@material-ui/core';
import { Controller, useForm} from 'react-hook-form';
import { toast } from 'react-toastify';
import { sortBy } from 'lodash';
import { Delete, Edit } from '@material-ui/icons';
import SearchIcon from '@material-ui/icons/Search';
import moment from 'moment';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { IDENTIFICATIONTYPEDTO, PARENTESCO } from '../../../types/catalog.type';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import { Postulante } from '../constants/fondosolidaridadform.model.type';
import NotificationService from '../../../services/notification.service';
import ApiClient from '../../../services/api-clients.service';
import { LoadingComponent } from '../../../components/loading/loading.component';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        buttonAdd: {
            width: '200px',
            marginRight: '15px',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        buttonAddEdit: {
            width: '300px',
            marginLeft: '.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

const InformacionBeneficiariosPostulantes = (props: props) =>{

    const classes = useStyles();
    const apiCliente = ApiClient();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const {catalogState, 
           setBeneficiariosPostulantesPostulacion,
           beneficiariosPostulantesPostulacion,
           camposSolicitudPostulacionSoloLectura} = props;
    const [listaParentesco, setListaParentesco] = useState<PARENTESCO[]>([]);
    const [estadoBotonBeneficiarios, setEstadoBotonBeneficiarios] = useState<string>('AGREGAR BENEFICIARIO');
    const [beneficiarioPostulanteModel, setBeneficiarioPostulanteModel] = useState<Postulante[]>([]);
    const [beneficiarioNuevo, setBeneficiarioNuevo] = useState<Postulante | null>(null);
    const [beneficiarioAnterior, setBeneficiarioAnterior] = useState<number>(0);
    const [idAnteriorBeneficiario, setIdAnteriorBeneficiario] = useState<number>(0);
    const [deshabilitarCampoParentesco, setDeshabilitarCampoParentesco] = useState<boolean>(true);
    const [deshabilitarCamposNombreYApellido, setDeshabilitarCamposNombreYApellido] = useState<boolean>(true);
    const [loading,setLoading] = useState(false);
    const formularioBeneficiarios = useForm ({
        defaultValues: {
            tipoDocumentoId: '',
            numeroDocumento:'',
            nombre: '',
            apellido:'',
            parentesco:'',
        }
    })

    useEffect(() => {
        consultarCatalogoListaParentesco();
    }, []);

    useEffect(() => {
        if( beneficiariosPostulantesPostulacion && beneficiariosPostulantesPostulacion.length > 0){
            setBeneficiarioPostulanteModel(beneficiariosPostulantesPostulacion);
        }

    }, [beneficiariosPostulantesPostulacion]);

    const consultarCatalogoListaParentesco = async () => {
        await apiFondoSolidaridad
            .getObtenerTipoParentesco()
            .then(async (response: any) => {
                if (response.data) {
                        const datosOrdenados = sortBy(
                            response.data,
                            'descripcion',
                        );
                        setListaParentesco(datosOrdenados);
                }
            })
            .catch(() => {
                toast('Error al consultar parentesco en Postulación', { type: toast.TYPE.ERROR });
            });
    };

    const agregarBeneficiarioPostulante = async (data: any) => {
        const datoEncontrado = beneficiarioPostulanteModel.findIndex(
            item =>
                String(item.numeroDocumento) === String(data.numeroDocumento) &&
                item.numeroDocumento !== beneficiarioAnterior,
        );
        if (datoEncontrado === -1) {
            await agregarRegistroBeneficiarioPostulante(data);
        } else {
            toast('El beneficiario ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
    };

    const consultarExisteBeneficiario = async() => {

        const data = formularioBeneficiarios.getValues();
        if(data?.tipoDocumentoId === '' ){
            formularioBeneficiarios.setError('tipoDocumentoId', { type: "focus", message: "Este campo es requerido." });
            return;
        }

        if(data?.tipoDocumentoId){
            formularioBeneficiarios.clearErrors('tipoDocumentoId');
        }

        if(data?.numeroDocumento === ''){
            formularioBeneficiarios.setError('numeroDocumento', { type: "focus", message: "Este campo es requerido." });
            return;
        }
        
        if(data?.numeroDocumento){
            formularioBeneficiarios.clearErrors('numeroDocumento');
        }
        setLoading(true);
        await apiCliente.getClientByIdentificationTypeIdentificationNumberRequest(
            Number(data?.tipoDocumentoId),
            data?.numeroDocumento,
        ).then(response => {
            if(response.data){
                setDeshabilitarCampoParentesco(false);
                setDeshabilitarCamposNombreYApellido(true);
                formularioBeneficiarios.setValue('nombre',`${response?.data?.primerNombre} ${response?.data?.segundoNombre}`);
                formularioBeneficiarios.setValue('apellido',`${response?.data?.primerApellido} ${response?.data?.segundoApellido}`);
                const beneficiario = {
                    tipoDocumentoId: response?.data?.idTipoIdentificacion,
                    numeroDocumento: Number(response?.data?.identificacion),
                    nombre: `${response?.data?.primerNombre} ${response?.data?.segundoNombre}`,
                    apellido: `${response?.data?.primerApellido} ${response?.data?.segundoApellido}`,
                    departamentoExpedicion: response?.data?.idDepartamentoExpedicionDocumento,
                    ciudadExpedicion: response?.data?.idCiudadExpedicionDocumento,
                    parentesco: '0',
                    sexo: response?.data?.sexo,
                    fechaNacimiento:  moment(response?.data?.fechaNacimiento).format('YYYY-MM-DD'),
                    estadoCivil: response?.data?.estadoCivil,
                    ocupacion: response?.data?.idProfesion,
                    nivelEducativo: null,
                    afectionFisica: null,
                    afectacionSensorial: null,
                    enfermedadEspecifica: null,
                    tratamientoMedico: null,
                    dificultadEscaleras: null,
                    medicoPermanente: null,
                    ayudaTecnica: null,
                    tipoIntegranteId: null,
                    tipoParentescoId: null,
                    tipAyudaTecnicaId: null,
                    tipoDiscapacidadId: null,
                    condicionSalud: null,
                    tieneCuenta: 'SI',
                }
                setBeneficiarioNuevo(beneficiario as Postulante);
                setLoading(false);
            }
            else{
                setDeshabilitarCampoParentesco(false);
                setDeshabilitarCamposNombreYApellido(false);
                setLoading(false);
                NotificationService.genericAlert(
                    'Información',
                    `El beneficiario no tiene cuenta creada`,
                    'warning'
                );
            }
        }).catch((e) => {
            setLoading(false);
            toast(`Error al consultar beneficiario postulacion ${e.message}`, { type: toast.TYPE.ERROR });
        });
    }

    const agregarRegistroBeneficiarioPostulante = async (data: any) => {
        let nuevoBeneficiario: Postulante;

        if(beneficiarioNuevo){
            nuevoBeneficiario = {
                ...beneficiarioNuevo,
                tipoParentescoId: data.parentesco,
            } 
        }else{
            nuevoBeneficiario = {
                tipoDocumentoId: Number(data.tipoDocumentoId),
                numeroDocumento: Number(data.numeroDocumento),
                nombre: String(data.nombre).toUpperCase(),
                apellido: String(data.apellido).toUpperCase(),
                departamentoExpedicion: null,
                ciudadExpedicion: null,
                parentesco: '0',
                sexo: null,
                fechaNacimiento:  null,
                estadoCivil: null,
                ocupacion: null,
                nivelEducativo: null,
                afectionFisica: null,
                afectacionSensorial: null,
                enfermedadEspecifica: null,
                tratamientoMedico: null,
                dificultadEscaleras: null,
                medicoPermanente: null,
                ayudaTecnica: null,
                tipoIntegranteId: null,
                tipoParentescoId: data.parentesco,
                tipAyudaTecnicaId: null,
                tipoDiscapacidadId: null,
                condicionSalud: null,
                tieneCuenta: 'NO',
            }
        }

        let beneficiarios: Postulante[] = [];
        if (nuevoBeneficiario && estadoBotonBeneficiarios === 'AGREGAR BENEFICIARIO') {

            setBeneficiarioPostulanteModel(oldArray => [
                ...oldArray,
                nuevoBeneficiario,
            ]);
            beneficiarios = [...beneficiarioPostulanteModel];
            beneficiarios.push(nuevoBeneficiario);
        } else if(nuevoBeneficiario && estadoBotonBeneficiarios === 'EDITAR BENEFICIARIO'){

            const temp = [...beneficiarioPostulanteModel];
            const posicionActual = temp.findIndex(
                item => item.numeroDocumento === beneficiarioAnterior,
            );

            nuevoBeneficiario.numeroDocumento = temp[posicionActual].numeroDocumento;

            if (
                Number(nuevoBeneficiario.numeroDocumento) !==
                temp[posicionActual].numeroDocumento
            ) {
                temp[posicionActual].numeroDocumento =
                nuevoBeneficiario.numeroDocumento;
            } else {
                nuevoBeneficiario.numeroDocumento = idAnteriorBeneficiario;
            }
            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, nuevoBeneficiario);
            beneficiarios = temp;
            setBeneficiarioPostulanteModel(temp);
        }
        setBeneficiariosPostulantesPostulacion(beneficiarios);
        setBeneficiarioAnterior(0);
        setBeneficiarioNuevo(null);
        resetBeneficiarioNuevo();
    };

    const seleccionarRegistroBeneficiario = async (item: Postulante) => {
        setDeshabilitarCampoParentesco(false);
        if(item?.tieneCuenta === 'SI'){
            setDeshabilitarCamposNombreYApellido(true);
        }

        if(item?.tieneCuenta === 'NO'){
            setDeshabilitarCamposNombreYApellido(false);
        }
        setBeneficiarioAnterior(item.numeroDocumento);
        setIdAnteriorBeneficiario(item.numeroDocumento);
        formularioBeneficiarios.setValue('tipoDocumentoId', String(item.tipoDocumentoId));
        formularioBeneficiarios.setValue('numeroDocumento', String(item.numeroDocumento));
        formularioBeneficiarios.setValue('nombre', item.nombre);
        formularioBeneficiarios.setValue('apellido', item.apellido);
        formularioBeneficiarios.setValue('parentesco', item.tipoParentescoId || '');
        setEstadoBotonBeneficiarios('EDITAR BENEFICIARIO');
    };

    const eliminarRegistroBeneficiario = async (numero: number) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...beneficiarioPostulanteModel];
            temp.splice(
                temp.findIndex(item => item.numeroDocumento === numero),
                1,
            );
            setBeneficiarioPostulanteModel(temp);
            setBeneficiariosPostulantesPostulacion(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
            resetBeneficiarioNuevo();
        }
    };

    const resetBeneficiarioNuevo = async () => {
        setEstadoBotonBeneficiarios('AGREGAR BENEFICIARIO');
        setDeshabilitarCampoParentesco(true);
        setDeshabilitarCamposNombreYApellido(true);
        formularioBeneficiarios.clearErrors();
        formularioBeneficiarios.setValue('tipoDocumentoId', '');
        formularioBeneficiarios.setValue('numeroDocumento', '');
        formularioBeneficiarios.setValue('nombre', '');
        formularioBeneficiarios.setValue('apellido', '');
        formularioBeneficiarios.setValue('parentesco', '');
    };

    const mostrarTipoIdentificacion = (numeroTipoDocumento: number) => {
        const tipoDocumento = catalogState.IdentificationType.find(item => item.id === numeroTipoDocumento);
        return tipoDocumento?.descripcion;
    }

    const mostrarParentesco = (parentesco: string) => {
        const tipoParentesco = listaParentesco.find(item => item.codigo === parentesco);
        return tipoParentesco?.descripcion;
    }

    return (
        <>
            <LoadingComponent loading={loading} />
            <CHPaper title="Información Beneficiarios Postulantes">
             {!camposSolicitudPostulacionSoloLectura && (
            <Grid container>
                <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="tipoDocumentoId"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={camposSolicitudPostulacionSoloLectura}
                                    label="Tipo documento *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={
                                    error ? error.message : null
                                }
                            >
                                {catalogState?.IdentificationType.map(
                                    (option: IDENTIFICATIONTYPEDTO) => {
                                        if(option.descripcion !== "CÓDIGO JUZGADO" &&
                                           option.descripcion !== "CODIGO MILITAR" &&
                                           option.descripcion !== "NÚMERO DE IDENTIFICACIÓN TRIBUTARIA"){
                                            return (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                 {option.descripcion}
                                                </MenuItem>
                                            )
                                        }
                                        return null;
                                })}
                            </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroDocumento"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly={camposSolicitudPostulacionSoloLectura}
                                    label="Número Documento *"
                                    value={value}
                                    onChange={e => {
                                        if(e.target.value.length <= 12){
                                            onChange(e);
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                         <Grid item xs={1}>
                         <IconButton
                             onClick={() => consultarExisteBeneficiario()}
                         >
                             <SearchIcon />
                         </IconButton>
                     </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="parentesco"
                                control={formularioBeneficiarios.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        readOnly={camposSolicitudPostulacionSoloLectura}
                                        label="Parentesco *"
                                        value={value}
                                        onChange={onChange}
                                        disabled={deshabilitarCampoParentesco}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                    >
                                        {listaParentesco?.map((option: PARENTESCO) => (
                                            <MenuItem
                                                key={option.codigo}
                                                value={option.codigo}
                                            >
                                                {option.descripcion}
                                            </MenuItem>
                                        ))}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                )}
                  {!camposSolicitudPostulacionSoloLectura && (
                    <Grid container>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="nombre"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                isReadOnly={deshabilitarCamposNombreYApellido || camposSolicitudPostulacionSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Nombres *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 25){
                                            onChange(
                                                e.target.value.replace(
                                                    /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                    '')
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="apellido"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={deshabilitarCamposNombreYApellido || camposSolicitudPostulacionSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Apellidos *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 25){
                                            onChange(
                                                e.target.value.replace(
                                                    /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                   '',),
                                           );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                )}
                <Grid container>
                    <Grid
                        className={classes.columnButtonAdd}
                        item
                        xs={12}
                        md={12}
                    >
                        {!camposSolicitudPostulacionSoloLectura && (
                           <Button
                               className={classes.buttonAddEdit}
                               variant="outlined"
                               style={{ marginRight: '10px' }}
                               onClick={formularioBeneficiarios.handleSubmit(agregarBeneficiarioPostulante)}
                           >
                               {estadoBotonBeneficiarios}
                           </Button>
                        )}
                        {!camposSolicitudPostulacionSoloLectura && (
                            <Button
                                className={classes.buttonAdd}
                                variant="outlined"
                                onClick={resetBeneficiarioNuevo}
                            >
                                CANCELAR
                            </Button>
                        )}
                    </Grid>
                </Grid>
                {!camposSolicitudPostulacionSoloLectura && (
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Lista de Beneficiarios
                </Typography>)}
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Tipo de Identificación</TableCell>
                            <TableCell>Número de Identificación</TableCell>
                            <TableCell>Nombres</TableCell>
                            <TableCell>Apellidos</TableCell>
                            <TableCell>Parentesco</TableCell>
                            {!camposSolicitudPostulacionSoloLectura && (
                                <TableCell align="center">
                                    Acciones
                                </TableCell>
                            )}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {beneficiarioPostulanteModel.map(
                            (option: Postulante) => (
                                <TableRow key={option.numeroDocumento}>
                                    <TableCell>{mostrarTipoIdentificacion(option.tipoDocumentoId)}</TableCell>
                                    <TableCell>{option.numeroDocumento}</TableCell>
                                    <TableCell>{option.nombre}</TableCell>
                                    <TableCell>{option.apellido}</TableCell>
                                    <TableCell>{mostrarParentesco(option.tipoParentescoId || '')}</TableCell>
                                    {!camposSolicitudPostulacionSoloLectura && (
                                    <TableCell align="center" style={{ width: '200px' }}>
                                        <IconButton
                                            aria-label="Editar"
                                            color="primary"
                                            size="small"
                                            onClick={() => {
                                                seleccionarRegistroBeneficiario(
                                                    option,
                                                );
                                            }}
                                        >
                                            <Edit />
                                        </IconButton>
                                        {!camposSolicitudPostulacionSoloLectura &&(
                                            <IconButton
                                                aria-label="Eliminar"
                                                color="primary"
                                                size="small"
                                                onClick={() => {
                                                    eliminarRegistroBeneficiario(
                                                        option.numeroDocumento,
                                                    );
                                                }}
                                            >
                                                <Delete />
                                            </IconButton>
                                        )}
                                    </TableCell>
                                    )}
                                </TableRow>
                            )
                        )}
                    </TableBody>
                  </Table>
            </CHPaper>
        </>
    )
}

interface LinkMapDispatchToProps {
    catalogState: CatalogState;
    setBeneficiariosPostulantesPostulacion: (value: Postulante[]) => void;
    beneficiariosPostulantesPostulacion: Postulante[];
    camposSolicitudPostulacionSoloLectura: boolean;
}

type props = LinkMapDispatchToProps;

export default InformacionBeneficiariosPostulantes;