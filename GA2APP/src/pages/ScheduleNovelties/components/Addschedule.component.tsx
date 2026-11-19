
import React from 'react';
import { Button, createStyles, Dialog, DialogContent, DialogTitle, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { AVERAGECOSTDTO, EXECUTINGUNITDTO, FORCEDTO, FORMATDTO } from '../../../types/catalog.type';
import { AppState } from '../../../store/rootReducer';
import { UnidadEjecutoraCronogramaDTO } from '../../../types/cronograma.type';
import { startAddExecutingUnitSchedule } from '../../../actions/schedule.action';
import CHSelectV2 from '../../../components/ch-select/CHSelectV2';
import CHDatePickerV2 from '../../../components/ch-datepicker/CHDatepickerV2';
import CHTextFieldV2 from '../../../components/ch-text-field/CHTextFieldV2';

const useStyles = makeStyles(() =>
    createStyles({
        button: {
            width: '120px',
            marginLeft: '37rem',
            background: '#42b7ef',
            color: '#fff',
            textAlign: 'center',
            borderRadius: '0%',

        },
        button2: {
            width: '120px',
            marginLeft: '.5rem',
            background: '#fec917',
            color: '#1c3b90',
            textAlign: 'center',
            borderRadius: '0%',

        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },

        icon: {
            padding: '10px',
            textAlign: 'inherit',
            color: '#1C3F8B',
        },

        title: {
            color: '#42b7ef',
            textAlign: 'center',
        }

    }),
);

function AddSchedule({
    catalogState,
    isOpen,
    onClose,
    startAddExecutingUnitSchedule,
}:
    {
        catalogState: CatalogState;
        isOpen: boolean;
        onClose: (value: any) => void;
        startAddExecutingUnitSchedule: (agregarUnidadEjecutora: UnidadEjecutoraCronogramaDTO) => void;
    }): any {
    const classes = useStyles();
    const handleClose = () => {
        const value: any = {};
        onClose(value);
    };

    const { control, watch, handleSubmit } =
        useForm<UnidadEjecutoraCronogramaDTO>({
            defaultValues: {
                descripcionFormato: '',
                descripcionMedioEnvio: '',
                fechaFin: new Date(),
                fechaInicio: new Date(),
                fechaReporte: new Date(),
                fuerzaDescripcion: '',
                id: 0,
                nombreUnidadEjecutora: '',
                periodo: new Date(),
                idFormato: 0,
                idMedioEnvio: 0,
            }
        });

    const addExecutingUnitSchedule = async (addExecutingUnitSchedule: UnidadEjecutoraCronogramaDTO) => {
        startAddExecutingUnitSchedule(addExecutingUnitSchedule)
    }

    return (
        <Dialog
            fullWidth
            maxWidth="md"
            open={isOpen}
            aria-labelledby="form-dialog-title"
        >
            <DialogTitle className={classes.title} id="form-dialog-title">AGREGAR UNIDAD EJECUTORA</DialogTitle>
            <DialogContent>
                <Grid container>
                    <Grid container md={12}>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="fechaFin"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHSelectV2
                                            defaultValue=""
                                            label="Fuerza"
                                            onChange={onChange}
                                            value={value}
                                        >
                                            {catalogState.Force?.map(
                                                (option: FORCEDTO) => (
                                                    <MenuItem
                                                        key={option.id}
                                                        value={option.id}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelectV2>
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="idFormato"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHSelectV2
                                            defaultValue=""
                                            label="Unidad Ejecutora"
                                            onChange={onChange}
                                            value={value}
                                        >
                                            {catalogState.ExecutingUnit?.filter(x=>x.fuerza === +watch('fechaFin')).map(
                                                (option: EXECUTINGUNITDTO) => (
                                                    <MenuItem
                                                        key={option.id}
                                                        value={option.id}
                                                    >
                                                        {option.nombre}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelectV2>
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid container md={12}>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="fechaReporte"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHDatePickerV2
                                            label="Fecha De Reporte a UE"
                                            onChange={onChange}
                                            value={value}
                                            invalidDateMessage="Fecha inválida"
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="fechaInicio"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHDatePickerV2
                                            label="Fecha De Inicio"
                                            onChange={onChange}
                                            value={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid container md={12}>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="fechaFin"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHDatePickerV2
                                            label="Fecha Final"
                                            onChange={onChange}
                                            value={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="periodo"
                                    control={control}
                                    render={({ field: { value, onChange } }) => (
                                        <CHTextFieldV2
                                            isReadOnly
                                            label="Periodo"
                                            onChange={onChange}
                                            value={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid container md={12}>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="idFormato"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHSelectV2
                                            label="Formato"
                                            onChange={onChange}
                                            value={value}
                                        >
                                            {catalogState.Formato?.map(
                                                (option: FORMATDTO) => (
                                                    <MenuItem
                                                        key={option.id}
                                                        value={option.id}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelectV2>
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="idMedioEnvio"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHSelectV2
                                            label="Medio De Envío"
                                            onChange={onChange}
                                            value={value}
                                        >
                                            {catalogState.MediosDeEnvio?.map(
                                                (option: AVERAGECOSTDTO) => (
                                                    <MenuItem
                                                        key={option.id}
                                                        value={option.id}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelectV2>
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Grid >
                        <Button
                            className={classes.button}
                            type="button"
                            onClick={handleClose}
                        >
                            REGRESAR
                        </Button>
                        <Button
                            className={classes.button2}
                            onClick={handleSubmit(addExecutingUnitSchedule)}
                            type="submit"
                        >
                            AGREGAR
                        </Button>
                    </Grid>
                </Grid>
            </DialogContent>
        </Dialog>
    );
}
const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
});

const mapDispatchToProps = {
    startAddExecutingUnitSchedule,
};
export default connect(mapStateToProps, mapDispatchToProps)(AddSchedule);


