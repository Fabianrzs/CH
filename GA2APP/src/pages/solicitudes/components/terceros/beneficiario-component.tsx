import { Button, createStyles, Grid, IconButton, makeStyles, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@material-ui/core';
import { Delete, Edit } from '@material-ui/icons';
import React, { useState, useEffect } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { BeneficiarioModel } from '../../../../models/tercero-beneficiario.model';
import NotificationService from '../../../../services/notification.service';
import { IDENTIFICATIONTYPEDTO, VALORCATALOGO } from '../../../../types/catalog.type';
import GenerarGuid from '../funciones/guid.funtions';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
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
    }),
);

const Beneficiarios = (props: PropsType) => {

    const classes = useStyles();
    const { listaBeneficiarios, 
            listaTipoDocumentos, 
            listaParentesco, 
            campoSoloLecturaRadicacionAprobada, 
            beneficiariosAdicionados } = props;
    const [beneficiariosModel, setBeneficiariosModel] = useState<any[]>([]);
    const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR BENEFICIARIO');
    const formularioBeneficiarios = useForm({
        defaultValues: {
            id: '',
            tipoDocumento: '',
            numeroDocumento: '',
            nombres: '',
            apellidos: '',
            porcentaje: '',
            parentesco: ''
        },
    });

    useEffect(() => {
        if (listaBeneficiarios && listaBeneficiarios.length > 0) {
            setBeneficiariosModel(listaBeneficiarios);
            beneficiariosAdicionados(listaBeneficiarios);
        }
	}, []);

    const agregarBeneficiario = async (data: any) => {

        let arrayBeneficiarios: any[] = [];
        const guid = GenerarGuid();
        const nuevoBeneficiario = {
            id: data.id === '' ? guid.newGuid() : data.id,
            tipoDoc: data.tipoDocumento.split('_')[0],
            tipoDocObj: {
                id: data.tipoDocumento.split('_')[0],
                descripcion: data.tipoDocumento.split('_')[1],
            },
            parentesco: parseInt(data.parentesco.split('_')[0], 10),
            parentescoObj: data.parentesco.split('_')[1]
                ? {
                    id: Number(String(data.parentesco).split('_')[0]),
                    descripcion: String(data.parentesco).split('_')[1]
                }
                : null,
            numDoc: data.numeroDocumento,
            primerNombre: data?.nombres.toUpperCase() || '',
            segundoNombre: '',
            primerApellido: data?.apellidos.toUpperCase() || '',
            segundoApellido: '',
            valorRetirarDos: Number(data.porcentaje.replace('%','')),
        }
        arrayBeneficiarios = [...beneficiariosModel];

        if (descEstadoBoton === 'AGREGAR BENEFICIARIO') {

            const registroEncontrado = arrayBeneficiarios.find((item: any) => item?.numeroDocumento === data.numeroDocumento);
            if (registroEncontrado) {
                toast('Ya existe un Beneficiario con el mismo número de documento en la lista', {
                    type: toast.TYPE.ERROR,
                });
                return;
            }

            const total = arrayBeneficiarios.reduce((acum, item) => acum + Number(typeof item?.valorRetirarDos === 'string'? item?.valorRetirarDos.replace('%', ''):item?.valorRetirarDos), 0);
            const totalPorcentaje = total + Number(data?.porcentaje.replace('%', ''));

            if (totalPorcentaje > 100) {
                toast('El porcentaje no puede superar el 100%', {
                    type: toast.TYPE.ERROR,
                });
                return;
            }

            arrayBeneficiarios.push(nuevoBeneficiario);
            setBeneficiariosModel(arrayBeneficiarios);
            beneficiariosAdicionados(arrayBeneficiarios);
            cancelar();
        } else {

            const index = arrayBeneficiarios.findIndex((item: any) => item.id === data.id);
            arrayBeneficiarios.splice(index, 1, nuevoBeneficiario);
            setBeneficiariosModel(arrayBeneficiarios);
            beneficiariosAdicionados(arrayBeneficiarios);
            cancelar();
            await NotificationService.genericAlert(
                'Actualizó con éxito',
                'Se actualizó el beneficiario con éxito.',
            );

        }

    }

    const eliminarRegistro = async (item: string) => {

        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...beneficiariosModel];
            temp.splice(
                temp.findIndex(x => x.id === item),
                1,
            );
            setBeneficiariosModel(temp);
            beneficiariosAdicionados(temp);
            cancelar();
        }

    }

    const seleccionarRegistro = (data: any) => {

        setDescEstadoBoton('EDITAR BENEFICIARIO');
        formularioBeneficiarios.setValue('id', data.id);
        formularioBeneficiarios.setValue('tipoDocumento',  `${data.tipoDocObj?.id}_${data.tipoDocObj?.descripcion}`);
        formularioBeneficiarios.setValue('numeroDocumento', data.numDoc);
        formularioBeneficiarios.setValue('nombres', data.primerNombre);
        formularioBeneficiarios.setValue('apellidos', data.primerApellido);
        formularioBeneficiarios.setValue('porcentaje',`${data.valorRetirarDos}%`);
        formularioBeneficiarios.setValue('parentesco', `${data.parentescoObj?.id}_${data.parentescoObj?.descripcion}`);
    }

    const cancelar = () => {
        setDescEstadoBoton('AGREGAR BENEFICIARIO');
        formularioBeneficiarios.reset();
    }

    return (
        <>
            <CHPaper title="Información Beneficiarios">
                {campoSoloLecturaRadicacionAprobada? null : (
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoDocumento"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo de Documento *"
                                    required
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {listaTipoDocumentos?.map(
                                        (option: IDENTIFICATIONTYPEDTO) =>
                                            <MenuItem
                                                key={option.id}
                                                value={`${option.id}_${option.descripcion}`}
                                            >
                                                {option.descripcion}
                                            </MenuItem>

                                    )}
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
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    numberOnlyMask
                                    required
                                    label="Número de documento"
                                    value={value}
                                    inputProps={{ maxLength: 12 }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombres"
                            control={formularioBeneficiarios.control}
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
                                    required
                                    label="Nombres"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{ style: { textTransform: 'uppercase', }, maxLength: 25 }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="apellidos"
                            control={formularioBeneficiarios.control}
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
                                    required
                                    label="Apellidos"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{ style: { textTransform: 'uppercase', }, maxLength: 25 }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="porcentaje"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                validate: (value) => {
                                    const numero = Number(value.replace('%',''));
                                    return numero <= 100 || 'El porcentaje no puede ser mayor a 100%.';
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    percentMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    required
                                    label="Porcentaje"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{ style: { textTransform: 'uppercase', }, maxLength: 50 }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
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
                                    error={!!error}
                                    label="Parentesco *"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={value => {
                                        onChange(value);
                                    }}
                                >
                                    {listaParentesco?.map(
                                        (option: VALORCATALOGO) => {
                                            if (option.idValor === 137 ||
                                                option.idValor === 163 ||
                                                option.idValor === 170 ||
                                                option.idValor === 176 ||
                                                option.idValor === 198) {
                                                const dato = `${option.idValor}_${option.descripcion}`;
                                                return (
                                                    <MenuItem
                                                        key={option.idValor}
                                                        value={dato}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                );
                                            }
                                            return null;
                                        },
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                )}
                {campoSoloLecturaRadicacionAprobada? null : (
                <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                    <Button
                        className={classes.buttonAdd}
                        variant="outlined"
                        onClick={formularioBeneficiarios.handleSubmit(agregarBeneficiario)}
                    >
                        {descEstadoBoton}
                    </Button>

                    <Button
                        className={classes.buttonAdd}
                        variant="outlined"
                        onClick={cancelar}
                    >
                        CANCELAR
                    </Button>
                </Grid>
                 )}
                <h3>Lista de Beneficiarios</h3>
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
                                    <TableCell>Nombres y Apellidos</TableCell>
                                    <TableCell>Porcentaje</TableCell>
                                    <TableCell>Parentesco</TableCell>
                                    {campoSoloLecturaRadicacionAprobada? null : (
                                        <TableCell align="center">
                                            Acciones
                                        </TableCell>
                                    )}
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {beneficiariosModel?.map(
                                    (option: any) => (
                                        <TableRow key={option.id}>
                                            <TableCell>
                                                {option.tipoDocObj?.descripcion}
                                            </TableCell>
                                            <TableCell>
                                                {option.numDoc}
                                            </TableCell>
                                            <TableCell>
                                                {`${option.primerNombre} ${option.primerApellido}`}
                                            </TableCell>
                                            <TableCell>
                                                {`${option.valorRetirarDos}%`}
                                            </TableCell>
                                            <TableCell>
                                                {option.parentescoObj?.descripcion}
                                            </TableCell>
                                            {campoSoloLecturaRadicacionAprobada? null : (
                                            <TableCell align="center">
                                                <IconButton
                                                    aria-label="Editar"
                                                    color="primary"
                                                    onClick={() => {
                                                        seleccionarRegistro(
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
                                                        eliminarRegistro(
                                                            option.id,
                                                        );
                                                    }}
                                                >
                                                    <Delete />
                                                </IconButton>
                                            </TableCell>
                                            )}
                                        </TableRow>
                                    ),
                                )}
                            </TableBody>
                        </Table>
                    </Grid>
                </TableContainer>
            </CHPaper>
        </>
    )

}

type PropsType = {
    listaBeneficiarios: BeneficiarioModel[];
    listaTipoDocumentos: IDENTIFICATIONTYPEDTO[];
    listaParentesco:VALORCATALOGO[];
    campoSoloLecturaRadicacionAprobada: boolean;
    beneficiariosAdicionados:(beneficiarios: any[]) => void;
}

export default Beneficiarios;