import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import {
    Button,
    createStyles,
    Grid,
    IconButton,
    makeStyles,
    MenuItem,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { Delete, Edit } from '@material-ui/icons';
import { toast } from 'react-toastify';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import { AppState } from '../../../../store/rootReducer';
import {
    startGetAllCatalogoParentesco,
    startGetAllTypeDocuments,
} from '../../../../actions/catalog.action';
import { CatalogState } from '../../../../reducers/catalog.reducer';
import GenerarGuid from '../funciones/guid.funtions';
import NotificationService from '../../../../services/notification.service';
import {
    ENTIDADEDUCATIVA,
    IDENTIFICATIONTYPEDTO,
    NIVELEDUCATIVO,
    PROGRAMAEDUCATIVO,
    VALORCATALOGO,
} from '../../../../types/catalog.type';
import { TerceroBeneficiarioEstudioModel } from '../../../../models/tercero-beneficiario-estudio.model';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';

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
    }),
);

function TerceroBeneficiarioEstudio({
    catalogState,
    listaBeneficiarioEstudio,
    obtenerTipoDocumento,
    tercerosBeneficiarioEstudioAdicionados,
    obtenerParentesco,
    obtenerEntidadEducativa,
    obtenerProgramaEducativo,
    obtenerNivelEducativo,
    responseSolicitud,
}: {
    catalogState: CatalogState;
    listaBeneficiarioEstudio: TerceroBeneficiarioEstudioModel[];
    obtenerTipoDocumento: () => void;
    tercerosBeneficiarioEstudioAdicionados: (
        terceros: TerceroBeneficiarioEstudioModel[],
    ) => void;
    obtenerParentesco: (catalogoId: number) => void;
    obtenerEntidadEducativa: () => void;
    obtenerProgramaEducativo: (idEntidad: string) => void;
    obtenerNivelEducativo: (idPrograma: string) => void;
    responseSolicitud: ResponseSeveranceModel | undefined;
}) {
    const classes = useStyles();
    const [
        terceroBeneficiarioEstudioModel,
        setTerceroBeneficiarioEstudioModel,
    ] = useState<TerceroBeneficiarioEstudioModel[]>([]);
    const [descEstadoBoton, setDescEstadoBoton] = useState<string>(
        'AGREGAR BENEFICIARIO',
    );
    const [numDocAnterior, setNumDocAnterior] = useState<string>('');
    const [idAnterior, setIdAnterior] = useState<string>('');

    useEffect(() => {
        obtenerTipoDocumento();
        obtenerParentesco(3);
        if (
            listaBeneficiarioEstudio !== null &&
            listaBeneficiarioEstudio !== undefined
        ) {
            if (listaBeneficiarioEstudio.length > 0) {
                setTerceroBeneficiarioEstudioModel(listaBeneficiarioEstudio);
            }
        }
        obtenerEntidadEducativa();

        if (responseSolicitud?.solicitudTerceroEntidadEducativaDto !== null) {
            const dato = String(
                responseSolicitud?.solicitudTerceroEntidadEducativaDto
                    .razonSocial,
            );
            const idEntidad = dato.split('_')[0];
            obtenerProgramaEducativo(idEntidad);

            obtenerNivelEducativo(
                String(
                    responseSolicitud?.solicitudTerceroEntidadEducativaDto
                        .idEntidad,
                ),
            );

            // controlApoderado.setValue("razonSocial", String(responseSolicitud?.solicitudTerceroEntidadEducativaDto.razonSocial));
            // controlApoderado.setValue("programaentidad", String(responseSolicitud?.solicitudTerceroEntidadEducativaDto.idEntidad));
            // controlApoderado.setValue("nivelEducativo", String(responseSolicitud?.solicitudTerceroEntidadEducativaDto.idNivel));
            // controlApoderado.setValue("numReciboPago", String(responseSolicitud?.solicitudTerceroEntidadEducativaDto.reciboPago));
            // controlApoderado.setValue("fechaLimitePago", new Date(responseSolicitud?.solicitudTerceroEntidadEducativaDto.fechalimite as any | string));
        }
    }, []);

    const controlApoderado = useForm({
        defaultValues: {
            parentescoEstudioObj: '',
            tipoDocEstudioObj: '',
            numDocEstudio: '',
            razonsocialEstudio: '',
            razonSocial: '',
            programaEntidad: '',
            nivelEducativo: '',
            reciboPago: '',
            valor: '',
            fechalimite: '',
        },
    });

    const cancelarTerceroApoderado = async () => {
        setDescEstadoBoton('AGREGAR BENEFICIARIO');
        controlApoderado.setValue('parentescoEstudioObj', '');
        controlApoderado.setValue('tipoDocEstudioObj', '');
        controlApoderado.setValue('numDocEstudio', '');
        controlApoderado.setValue('razonsocialEstudio', '');
        controlApoderado.setValue('razonSocial', '');
        controlApoderado.setValue('programaEntidad', '');
        controlApoderado.setValue('nivelEducativo', '');
        controlApoderado.setValue('reciboPago', '');
        controlApoderado.setValue('valor', '');
        controlApoderado.setValue('fechalimite', '');
        controlApoderado.reset();
    };

    const agregarTerceroApoderado = async (data: any) => {
        const datoEncontrado = terceroBeneficiarioEstudioModel.findIndex(
            x =>
                x.numDocEstudio === data.numDocEstudio &&
                x.numDocEstudio !== numDocAnterior,
        );
        if (datoEncontrado === -1) {
            const guid = GenerarGuid();

            const terceroNuevo = {
                id: guid.newGuid(),
                tipoDocEstudio: data.tipoDocEstudioObj.split('_')[0],
                tipoDocEstudioObj: {
                    id: data.tipoDocEstudioObj.split('_')[0],
                    descripcion: data.tipoDocEstudioObj.split('_')[1],
                },
                parentescoEstudio:
                    data.parentescoEstudioObj.split('_')[0] === ''
                        ? null
                        : data.parentescoEstudioObj.split('_')[0],
                parentescoEstudioObj: {
                    id: data.parentescoEstudioObj.split('_')[0],
                    descripcion: data.parentescoEstudioObj.split('_')[1],
                },
                numDocEstudio: data.numDocEstudio,
                razonsocialEstudio: data.razonsocialEstudio,
                esAutorizado: data.esAutorizado,
            };

            let terceros: TerceroBeneficiarioEstudioModel[] = [];
            if (descEstadoBoton === 'AGREGAR BENEFICIARIO') {
                setTerceroBeneficiarioEstudioModel(oldArray => [
                    ...oldArray,
                    terceroNuevo,
                ]);
                terceros = [...terceroBeneficiarioEstudioModel];
                terceros.push(terceroNuevo);
            } else {
                const temp = [...terceroBeneficiarioEstudioModel];
                const posicionActual = temp.findIndex(
                    x => x.numDocEstudio === numDocAnterior,
                );
                temp.splice(posicionActual, 1);
                terceroNuevo.id = idAnterior;
                temp.splice(posicionActual, 0, terceroNuevo);
                terceros = temp;
                setTerceroBeneficiarioEstudioModel(temp);
            }

            tercerosBeneficiarioEstudioAdicionados(terceros);
            setNumDocAnterior('');
            setIdAnterior('');
            await cancelarTerceroApoderado();
        } else {
            toast(
                'Ya existe un apoderado con este mismo número de documento en la lista',
                { type: toast.TYPE.ERROR },
            );
        }
    };

    const eliminarRegistroTercero = async (item: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...terceroBeneficiarioEstudioModel];
            temp.splice(
                temp.findIndex(x => x.id === item),
                1,
            );
            setTerceroBeneficiarioEstudioModel(temp);
            tercerosBeneficiarioEstudioAdicionados(temp);
        }
    };

    const seleccionarRegistroTercero = async (
        item: TerceroBeneficiarioEstudioModel,
    ) => {
        setNumDocAnterior(item.numDocEstudio);
        setIdAnterior(item.id);
        controlApoderado.setValue(
            'tipoDocEstudioObj',
            `${item.tipoDocEstudioObj.id}_${item.tipoDocEstudioObj.descripcion}`,
        );
        if (item.parentescoEstudio != null) {
            controlApoderado.setValue(
                'parentescoEstudioObj',
                `${item.parentescoEstudioObj.id}_${item.parentescoEstudioObj.descripcion}`,
            );
        }
        controlApoderado.setValue('numDocEstudio', item.numDocEstudio);
        controlApoderado.setValue(
            'razonsocialEstudio',
            item.razonsocialEstudio,
        );
        setDescEstadoBoton('EDITAR BENEFICIARIO');
    };

    return (
        <>
            <CHPaper title="Beneficiario de estudio">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoDocEstudioObj"
                            control={controlApoderado.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo de Documento"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={value => {
                                        onChange(value);
                                    }}
                                >
                                    {catalogState.IdentificationType?.map(
                                        (option: IDENTIFICATIONTYPEDTO) => {
                                            const dato = `${option.id}_${option.descripcion}`;
                                            return (
                                                <MenuItem
                                                    key={option.id}
                                                    value={dato}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            );
                                        },
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numDocEstudio"
                            control={controlApoderado.control}
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
                                    isReadOnly={false}
                                    label="Número de Documento"
                                    value={value}
                                    onChange={value => {
                                        onChange(value);
                                    }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="razonsocialEstudio"
                            control={controlApoderado.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    label="Nombre / Razón Social"
                                    value={value}
                                    onChange={value => {
                                        onChange(value);
                                    }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="parentescoEstudioObj"
                            control={controlApoderado.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Parentesco"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={value => {
                                        onChange(value);
                                    }}
                                >
                                    {catalogState.parentescoCatalogo?.map(
                                        (option: VALORCATALOGO) => {
                                            const dato = `${option.idValor}_${option.descripcion}`;
                                            return (
                                                <MenuItem
                                                    key={option.idValor}
                                                    value={dato}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            );
                                        },
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="razonSocial"
                            control={controlApoderado.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Razón Social"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={value => {
                                        const dato = value.target
                                            .value as string;
                                        const idEntidad = dato.split('_')[0];
                                        obtenerProgramaEducativo(idEntidad);
                                        controlApoderado.setValue(
                                            'programaEntidad',
                                            '',
                                        );
                                        onChange(value);
                                    }}
                                >
                                    {catalogState.entidadEducativa?.map(
                                        (option: ENTIDADEDUCATIVA) => {
                                            const dato = `${option.idEntidad}_${option.nit}_${option.razonSocial}`;
                                            return (
                                                <MenuItem
                                                    key={option.idEntidad}
                                                    value={dato}
                                                >
                                                    {option.razonSocial}
                                                </MenuItem>
                                            );
                                        },
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="programaEntidad"
                            control={controlApoderado.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Programa Entidad"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={value => {
                                        const dato = value.target
                                            .value as string;
                                        obtenerNivelEducativo(dato);
                                        controlApoderado.setValue(
                                            'nivelEducativo',
                                            '',
                                        );
                                        onChange(value);
                                    }}
                                >
                                    {catalogState.programaEducativo?.map(
                                        (option: PROGRAMAEDUCATIVO) => (
                                            <MenuItem
                                                key={option.id}
                                                value={option.id}
                                            >
                                                {option.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nivelEducativo"
                            control={controlApoderado.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Nivel Educativo"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={value => {
                                        onChange(value);
                                    }}
                                >
                                    {catalogState.nivelEducativo?.map(
                                        (option: NIVELEDUCATIVO) => (
                                            <MenuItem
                                                key={option.id}
                                                value={option.id}
                                            >
                                                {option.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="reciboPago"
                            control={controlApoderado.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    label="Número Recibo De Pago"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valor"
                            control={controlApoderado.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    label="Valor"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechalimite"
                            control={controlApoderado.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    label="Fecha Límite De Pago"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>

                <Grid container>
                    <Grid
                        className={classes.columnButtonAdd}
                        item
                        xs={12}
                        md={12}
                    >
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={controlApoderado.handleSubmit(
                                agregarTerceroApoderado,
                            )}
                        >
                            {descEstadoBoton}
                        </Button>

                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={cancelarTerceroApoderado}
                        >
                            CANCELAR
                        </Button>
                    </Grid>
                </Grid>

                <h3>Lista de Apoderados</h3>

                <TableContainer component={Paper}>
                    <Grid
                        className={classes.columnButtonAdd}
                        item
                        xs={12}
                        md={12}
                    >
                        <Table aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Tipo de Identificación</TableCell>
                                    <TableCell>Número de Identificación</TableCell>
                                    <TableCell>Nombre / Razón Social</TableCell>
                                    <TableCell>Parentesco</TableCell>
                                    <TableCell align="center">
                                        Acciones
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {terceroBeneficiarioEstudioModel.map(
                                    (
                                        option: TerceroBeneficiarioEstudioModel,
                                    ) => (
                                        <TableRow key={option.id}>
                                            <TableCell>
                                                {
                                                    option.tipoDocEstudioObj
                                                        .descripcion
                                                }
                                            </TableCell>
                                            <TableCell>
                                                {option.numDocEstudio}
                                            </TableCell>
                                            <TableCell>
                                                {option.razonsocialEstudio}
                                            </TableCell>
                                            <TableCell>
                                                {option.parentescoEstudioObj
                                                    .id === undefined
                                                    ? 'N/A'
                                                    : option
                                                          .parentescoEstudioObj
                                                          .descripcion}
                                            </TableCell>
                                            <TableCell align="center">
                                                <IconButton
                                                    aria-label="Editar"
                                                    color="primary"
                                                    onClick={() => {
                                                        seleccionarRegistroTercero(
                                                            option,
                                                        );
                                                    }}
                                                >
                                                    <Edit />
                                                </IconButton>
                                                <IconButton
                                                    aria-label="Eliminar"
                                                    color="secondary"
                                                    onClick={() => {
                                                        eliminarRegistroTercero(
                                                            option.id,
                                                        );
                                                    }}
                                                >
                                                    <Delete />
                                                </IconButton>
                                            </TableCell>
                                        </TableRow>
                                    ),
                                )}
                            </TableBody>
                        </Table>
                    </Grid>
                </TableContainer>
            </CHPaper>
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
});

const mapDispatchToProps = (dispatch: any) => ({
    obtenerTipoDocumento: () => dispatch(startGetAllTypeDocuments()),
    obtenerParentesco: (catalogoId: number) =>
        dispatch(startGetAllCatalogoParentesco(catalogoId)),
});

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(TerceroBeneficiarioEstudio);
