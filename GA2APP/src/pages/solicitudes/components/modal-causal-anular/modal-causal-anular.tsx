import { Card, createStyles, CardContent, Grid, makeStyles, Modal, Typography, Button, MenuItem } from '@material-ui/core';
import React from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import CHSelect from '../../../../components/ch-select/CHSelect';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1.5rem',

        },
        fullScreenDialog: {
            width: '100%',
        },
        formRow: {
            display: 'flex',
            flexDirection: 'row',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        card: {
            width: '70%'
        },
        columnLabel: {
            padding: '10px',
        },
        texfield: {
            width: '100%',
            fontWeight: 'bold',
            fontSize: 'large'
        },
        inline: {
            display: 'inline',
        },
        column: {
            padding: '10px',
            textAlign: 'center'
        },
    })
);

const ModalCausalAnularSolicitud = (props: Props) => {

    const classes = useStyles();
    const {open, formulario, onClose, funcionAnular} = props;

   return(
    <>
        <Modal
            open={open}
            onClose={onClose}
            aria-labelledby="form-dialog-title"
            style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                height: '90vh',
            }}
        >
        <Card className={classes.card}
            style={{
                width: '600px',     
                maxWidth: '90%',    
            }}
        >
            <CardContent>
                <br />
                <br />
                    <>
                        <Grid item>
                            <Typography
                                className={classes.titleform}
                                variant="h5"
                                component="h6"
                            >
                                ANULAR SOLICITUD
                            </Typography>
                        </Grid>
                        <Grid container>
                            <Grid className={classes.column} item xs={12} md={12}>
                                    <Controller
                                        name="causal"
                                        control={formulario.control}
                                        render={({ field: { value } }) => (
                                            <CHSelect
                                                label="Causal"
                                                value={value}
                                                defaultValue='0'
                                            >
                                                <MenuItem
                                                    key='0'
                                                    value='0'
                                                >
                                                    Lista de selección de Causales parametrizable
                                                </MenuItem>
                                            </CHSelect>
                                        )}
                                    />
                            </Grid>
                        </Grid>
                        <Grid container justify="flex-end" spacing={3}>
                            <Grid item>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"
                                    style={{width:"120px"}}
                                    onClick={() => funcionAnular()}
                                >
                                    Confirmar
                                </Button>
                            </Grid>
                            <Grid item>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="default"
                                    onClick={onClose}
                                >
                                    Cancelar
                                </Button>
                            </Grid>
                        </Grid>
                    </>
                </CardContent>             
            </Card>                         
        </Modal>
    </>
   )
}

interface Props {
    open: boolean;
    formulario: UseFormReturn<any>;
    onClose: () => void;
    funcionAnular: () => void;
}

export default ModalCausalAnularSolicitud;