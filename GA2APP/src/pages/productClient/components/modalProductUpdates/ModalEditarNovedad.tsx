
import React from 'react';
import { Button, Card, 
         CardContent,
         Container, 
         createStyles, 
         Dialog, 
         Grid, 
         makeStyles, 
         MenuItem, 
         Typography} from '@material-ui/core';
import CHSelect from '../../../../components/ch-select/CHSelect';

const useStyles = makeStyles(() =>
    createStyles({   
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },       
    }),
);

export const ModalEditarNovedad = (props: any) => {
    const {
        open,
        handleClose,
        handleOnChange,
        handleEditarNovedad,
        tipoNovedad
    } = props;

    const classes = useStyles();
    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="md">
                    <Card>
                        <CardContent>
                            <Grid item>
                                <Typography
                                    className={classes.titleform}
                                    variant="h5"
                                    component="h6"
                                >
                                    ACTUALIZAR NOVEDAD                                      
                                </Typography>
                            </Grid> 
                            <br/>  
                            <Grid container spacing={3}>
                                <Grid item xs={3}>
                                    <br />
                                    <Typography>Novedad</Typography>
                                </Grid>
                            <Grid item xs={9}>
                                <CHSelect
                                    label=""
                                    name="estado"
                                    onChange={handleOnChange}   
                                    value={tipoNovedad} 
                                    inputProps={{
                                        style: {
                                            width:"500px"
                                        }
                                    }}
                                >
                                     <MenuItem value="1">ACELERACIÓN DE LA DEUDA</MenuItem>
                                    <MenuItem value="2">REFERIDO DE LA DEUDA</MenuItem>
                                    <MenuItem value="3">REESTRUCTURACIÓN DE LA DEUDA</MenuItem>
                                    <MenuItem value="4">NORMALIZACIONES</MenuItem>
                                </CHSelect>
                            </Grid>
                            </Grid>
                            <br/>  
                            <Grid container spacing={3}>
                                <Grid item>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={handleEditarNovedad}
                                    >
                                        Actualizar
                                    </Button>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="default"
                                        onClick={handleClose}   
                                        style={{marginLeft: "10px"}}                                    
                                    >
                                        Cancelar
                                    </Button>       
                                </Grid>
                            </Grid>
                        </CardContent>
                    </Card>
                </Container>
            </Dialog>
        </div>
    );
};
