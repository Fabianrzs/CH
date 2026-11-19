import React, { useEffect } from 'react';
import {
    Button,
    Dialog,
    TextField,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    Checkbox,
} from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { TablaTitleParametroDto } from '../../../types/tablaParametro.model.type';
import {
    ModalTablaParametroForm,
} from '../constants/tablaParametroform.model.type';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center'
        },
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        titleCampo: {
            fontSize: '10px',

        },
        table: {
            color: '#f1c3b90ff',
        },
    }),
);

export const ModalAgregar = (props: any) => {
    const {
        data,
        handleClose,
        open,
        handleOnClickAgregar,
        handleOnChange,

    } = props;
    const classes = useStyles();
    const formularioModal = useForm({
        defaultValues: ModalTablaParametroForm,
    });

    

        const transformName = (columnName: string) => {
            const transformedName = columnName.replace(/_/g, ' ').replace(/\b\w/g, char => char.toUpperCase());
        
    
            switch (columnName) {
                case 'ENT_CODIGO_BANCO':
                    return 'CÓDIGO DEL BANCO';
                case 'ENT_NOMBRE_RAZON_SOCIAL':
                    return 'NOMBRE Y/O RAZÓN SOCIAL';
                default:
                   
                    return transformedName.replace(/^Ent /, '');
            }
        };
        

    return (
        <Dialog
            open={open}
            onClose={handleClose}
            className={classes.table}
            aria-labelledby="form-dialog-title"

        >
            <CHPaper title="Agregar">
                <Grid container>
                    <Typography className={classes.titleform} variant="h5" component="h6">
                        Nuevo registro
                    </Typography>
                    {data.filter((campo: TablaTitleParametroDto) => campo.tipoDato === 'varchar').map((campo: TablaTitleParametroDto) => (
                        <><br /><Grid key={campo.nombreColumna} container spacing={3}>

                            <Grid item xs={10}>
                                <TextField
                                    name={campo.nombreColumna}
                                    label={transformName(campo.nombreColumna)}
                                    onChange={handleOnChange}
                                    inputProps={{
                                        style: {
                                            width: '400px',
                                            textTransform: 'uppercase',
                                        }
                                    }}


                                />
                            </Grid>
                        </Grid></>
                    ))}
                    <br />
                    <br />
                    <br />
                    {data.filter((campo: TablaTitleParametroDto) => campo.tipoDato === 'int').map((campo: TablaTitleParametroDto) => (
                        <><br /><Grid key={campo.nombreColumna} container spacing={4}>

                            <Grid item xs={10}>
                                <TextField
                                    name={campo.nombreColumna}
                                    label={transformName(campo.nombreColumna)}
                                    type='number'
                                    onChange={handleOnChange}
                                    inputProps={{
                                        style: {
                                            width: '400px',
                                            textTransform: 'uppercase',
                                        }
                                    }} />
                            </Grid>
                        </Grid></>
                    ))}
                    <br />
                    <br />
                    <br />
                    <Grid container spacing={3} style={{ textAlign: 'right' }}>
                        <Grid item>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={() => handleOnClickAgregar}

                            >
                                Agregar
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"
                                onClick={handleClose}
                                style={{ marginLeft: "10px" }}
                            >
                                Cancelar
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </CHPaper>
        </Dialog>
    );
};