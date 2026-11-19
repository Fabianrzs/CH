import React from 'react';
import {
    Button,
    Grid,
    makeStyles,
    MenuItem,
    Paper,
    Table,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
    createStyles,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
    }),
);

function IdentificacionPostulantesNucleoFamiliar() {
    const classes = useStyles();
    const postulantesControl = useForm({
        defaultValues: {
            nombres: '',
            apellidos: '',
            tipoDocumento: 0,
            numeroDocumento: '',
            lugarExpedicion: '',
            parentesco: '',
            sexo: '',
            fechaNacimiento: new Date(),
            estadoCivil: '',
            ocupacion: '',
            nivelEducativo: 0,
            afectacionFisica: '',
            afectacionSensorial: '',
            enfermedadEspecifica: '',
            tratamientoMedicoPermanente: '',
            dificultadEscaleras: '',
            medicamentoPermanente: '',
            ayuda: '',
            condicionSalud: '',
        },
    });
    return (
        <>
            <CHPaper title="Identificación de los postulantes y su núcleo familiar">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombres"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Nombres"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="apellidos"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Apellidos"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoDocumento"
                            control={postulantesControl.control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    label="Tipo documento"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={1} value={1}>
                                        CEDULA DE CIUDADANIA
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroDocumento"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número Documento"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="lugarExpedicion"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Lugar Expedición"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="parentesco"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Parentesco"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="sexo"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Sexo"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaNacimiento"
                            control={postulantesControl.control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Nacimiento"
                                    onChange={onChange}
                                    value={value}
                                    autoOk
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="estadoCivil"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Estado Civil"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Consulta Social
                </Typography>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ocupacion"
                            control={postulantesControl.control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    label="Ocupación"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={1} value={1}>
                                        PENSIONADOSSS
                                    </MenuItem>
                                    <MenuItem key={2} value={2}>
                                        EMPLEADO
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nivelEducativo"
                            control={postulantesControl.control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    label="Nivel Educativo"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={1} value={1}>
                                        NO SABE LEER Y/O ESCRIBIR
                                    </MenuItem>
                                    <MenuItem key={2} value={2}>
                                        PRIMARIA
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Condición de salud
                </Typography>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="afectacionFisica"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Afectación Física (partes de cuerpo afectadas"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="afectacionSensorial"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Afectación sensorial (visual, auditiva)"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="enfermedadEspecifica"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Enfermedad Específica"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="tratamientoMedicoPermanente"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Recibe tratamiento médico permanente"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="dificultadEscaleras"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Presenta dificultad para subir y bajar escaleras"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="medicamentoPermanente"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Requiere medicamento permanente"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="ayuda"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Ayuda técnica para la movilidad o ayuda sensorial"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="condicionSalud"
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Describa brevemente su condición de salud en el mismo orden del primer cuadro (Ejemplo: Pérdida de movilidad en pie izquierdo por mina antipersona)"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={false}
                                    multiline
                                    rows={2}
                                    InputLabelProps={{ shrink: true }}
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
                            style={{ marginRight: '10px' }}
                            // onClick={matriculasControl.handleSubmit(
                            //     agregarMatricula,
                            // )}
                        >
                            AGREGAR POSTULANTE
                        </Button>
                        {/* <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            // onClick={cancelarMatricula}
                        >
                            CANCELAR
                        </Button> */}
                    </Grid>
                </Grid>

                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Lista de postulantes
                </Typography>

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
                                    <TableCell>Sexo</TableCell>
                                    <TableCell>Fecha de Nacimiento</TableCell>
                                    <TableCell>Estado Civil</TableCell>
                                    <TableCell align="center">
                                        Acciones
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            {/* <TableBody>
                            {terceroApoderadoModel.map(
                                (option: TerceroApoderadoModel) => (
                                    <TableRow key={option.id}>
                                        <TableCell>
                                            {option.tipoDocObj.descripcion}
                                        </TableCell>
                                        <TableCell>{option.numDoc}</TableCell>
                                        <TableCell>
                                            {option.razonsocial}
                                        </TableCell>
                                        <TableCell>
                                            {option.parentescoObj.id ===
                                            undefined
                                                ? 'N/A'
                                                : option.parentescoObj
                                                      .descripcion}
                                        </TableCell>
                                        <TableCell>
                                            {option.esAutorizado ? 'Si' : 'No'}
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
                  
                </Grid>
            */}
                        </Table>
                    </Grid>
                </TableContainer>
            </CHPaper>
        </>
    );
}

export default IdentificacionPostulantesNucleoFamiliar;
