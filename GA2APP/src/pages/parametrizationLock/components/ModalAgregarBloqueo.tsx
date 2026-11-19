import React from 'react';
import {
    Button,
    Dialog,
    TextField,
    Card,
    CardContent,
    Container,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    Checkbox,
} from '@material-ui/core';

const useStyles = makeStyles(() =>
    createStyles({   
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },       
    }),
);
export const ModalAgregarBloqueo = (props: any) => {    
    const {
        handleClose,
        open,
        handleOnClickAgregar,
        handleOnChange,
        handleOnCheckedReversible,
        handleOnCheckedAceleracionDeuda,
        formBloqueo   
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
                                        AGREGAR BLOQUEO                                      
                                    </Typography>
                                </Grid>
                                <br/>
                                <Grid container spacing={3}>                               
                                <Grid item xs={4}>
                                    <Typography>
                                        Código
                                    </Typography>
                                </Grid>
                                <Grid item xs={8}>
                                    <TextField
                                            name="codigo"
                                            type="number"
                                            onChange={handleOnChange}
                                            value={formBloqueo?.codigo}      
                                            inputProps={{
                                                style: {
                                                    width: '400px'
                                                }
                                            }}                                     
                                        />
                                </Grid>
                            </Grid>                                                                
                            <Grid container spacing={3}>                               
                                <Grid item xs={4}>
                                    <Typography>
                                        Concepto
                                    </Typography>
                                </Grid>
                                <Grid item xs={8}>
                                    <TextField
                                        name="concepto"
                                        onChange={handleOnChange}
                                        value={formBloqueo?.concepto}     
                                        inputProps={{
                                            style: {
                                                width: '400px'
                                            }
                                        }}                                      
                                    />
                                </Grid>
                            </Grid>                          
                            <Grid container spacing={3}>
                                <Grid item xs={4}>
                                <br/>
                                    <Typography>Reversible</Typography>
                                </Grid>
                            <Grid item xs={8}>
                                   <Checkbox 
                                        onChange={handleOnCheckedReversible}
                                        checked={formBloqueo?.reversible}
                                        color='primary'
                                   />
                            </Grid>
                         </Grid>
                         <Grid container spacing={3}>
                                <Grid item xs={4}>
                                <br/>
                                    <Typography>Aceleración Deuda</Typography>
                                </Grid>
                            <Grid item xs={8}>
                                    <Checkbox 
                                        onChange={handleOnCheckedAceleracionDeuda}
                                        checked={formBloqueo?.aceleracionDeuda}
                                        color='primary'
                                   />
                            </Grid>
                         </Grid>
                            <Grid container spacing={3}>
                                <Grid item>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={handleOnClickAgregar}
                                    >
                                        Agregar
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