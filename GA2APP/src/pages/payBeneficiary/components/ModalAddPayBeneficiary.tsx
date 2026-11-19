import React from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import CHSelect from '../../../components/ch-select/CHSelect';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';

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
export const ModalAddPayBeneficiary = (props: any) => {    
    const {
        handleClose,
        open,
        handleOnClickAdd,
        handleOnChange,
        tiposIdentificacion,
        formBeneficiario
    } = props;
    const classes = useStyles();   
    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogContent style={{width: '600px'}}>
                        <Grid item>
                                <br/>
                                    <Typography
                                     className={classes.titleform}
                                     variant="h5"
                                     component="h6"
                                    >
                                        AGREGAR BENEFICIARIO DE PAGO POR EXPEDIENTE                                    
                                    </Typography>
                                </Grid>
                                <br/>
                                <Grid container>                               
                                <Grid item md={4}>
                                <br/>
                                    <Typography 
                                        style={{
                                            width:'400px'
                                          }
                                        }
                                    >
                                        Radicado Work Manager
                                    </Typography>
                                </Grid>
                                    <Grid item md={8}>
                                    <CHTextField 
                                     name="radicadoWorkManager"
                                     value={formBeneficiario?.radicadoWorkManager}
                                     onChange={handleOnChange}
                                     inputProps={{
                                         style: {
                                             width: '400px'
                                         }
                                     }}
                                />
                                </Grid>
                            </Grid>                                                                
                            <Grid container>                               
                                <Grid item md={4}>
                                <br/>
                                    <Typography
                                     style={{
                                        width:'400px'
                                      }
                                    }
                                    >
                                        Tipo de Identificación
                                    </Typography>
                                </Grid>
                                <Grid item md={8}>
                                <CHSelect
                                    name="tipoDocumentoId"
                                    label=''
                                    value={formBeneficiario?.tipoDocumentoId}
                                    onChange={handleOnChange}
                                >
                                {
                                    tiposIdentificacion?.map(
                                    (option: IDENTIFICATIONTYPEDTO) => (
                                            <MenuItem key={option.id} value={option.id}>
                                                {option.descripcion}
                                            </MenuItem>
                                     ),
                                )}
                            </CHSelect>
                                </Grid>
                            </Grid>
                            <Grid container>                               
                                <Grid item md={4}>
                                <br/>
                                    <Typography
                                     style={{
                                        width:'400px'
                                      }
                                    }
                                    >
                                        No. Identificación
                                    </Typography>
                                </Grid>
                                <Grid item md={8}>
                                <CHTextField 
                                     name="numeroIdentificacion"
                                     value={formBeneficiario?.numeroIdentificacion}
                                     onChange={handleOnChange}
                                     inputProps={{
                                         style: {
                                             width: '400px'
                                         }
                                     }}
                                />
                            
                                </Grid>
                            </Grid>    
                            <Grid container>                               
                                <Grid item md={4}>
                                <br/>
                                    <Typography
                                     style={{
                                        width:'400px'
                                      }
                                    }
                                    >
                                        Primer Nombre
                                    </Typography>
                                </Grid>
                                <Grid item md={8}>
                                <CHTextField 
                                     name="primerNombre"
                                     value={formBeneficiario?.primerNombre}
                                     onChange={handleOnChange}
                                     inputProps={{
                                         style: {
                                             width: '400px'
                                         }
                                     }}
                                />
                                </Grid>
                            </Grid>    
                            <Grid container>                               
                                <Grid item md={4}>
                                <br/>
                                    <Typography
                                     style={{
                                        width:'400px'
                                      }
                                    }
                                    >
                                        Segundo Nombre
                                    </Typography>
                                </Grid>
                                <Grid item md={8}>
                                <CHTextField 
                                     name="segundoNombre"
                                     value={formBeneficiario?.segundoNombre}
                                     onChange={handleOnChange}
                                     inputProps={{
                                         style: {
                                             width: '400px'
                                         }
                                     }}
                                />
                                </Grid>
                            </Grid>
                            <Grid container>                               
                                <Grid item md={4}>
                                <br/>
                                    <Typography
                                     style={{
                                        width:'400px'
                                      }
                                    }
                                    >
                                        Primer Apellido
                                    </Typography>
                                </Grid>
                                <Grid item md={8}>
                                <CHTextField 
                                     name="primerApellido"
                                     value={formBeneficiario?.primerApellido}
                                     onChange={handleOnChange}
                                     inputProps={{
                                         style: {
                                             width: '400px'
                                         }
                                     }}
                                />
                                </Grid>
                            </Grid>   
                            <Grid container>                               
                                <Grid item md={4}>
                                <br/>
                                    <Typography
                                     style={{
                                        width:'400px'
                                      }
                                    }
                                    >
                                        Segundo Apellido
                                    </Typography>
                                </Grid>
                                <Grid item md={8}>
                                <CHTextField 
                                     name="segundoApellido"
                                     value={formBeneficiario?.segundoApellido}
                                     onChange={handleOnChange}
                                     inputProps={{
                                         style: {
                                             width: '400px'
                                         }
                                     }}
                                />
                                </Grid>
                            </Grid>       
                         <br/>
                            <Grid container>
                                <Grid item>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                         onClick={handleOnClickAdd}
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
                        <br/>
                </DialogContent>
            </Dialog>
        </div>
    );
};