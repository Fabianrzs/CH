import React from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    TextField,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    MenuItem,
    Checkbox,
} from '@material-ui/core';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import { TipoEmbargoDto, TipoRetencionDto } from '../../../types/embargo.model.type';

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
export const ModalEditarEmbargoConcepto = (props: any) => {    
    const {
        handleClose,
        open,
        handleOnClickEdit,
        handleOnChange,
        tiposEmbargos,
        tiposRetencion,
        handleOnChangeFechaInicio,
        handleOnChangeFechaFin,
        handleOnChecked,
        formEmbargo
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
                                        ACTUALIZAR EMBARGO A CONCEPTO                                     
                                    </Typography>
                                </Grid>
                                <br/>
                                <Grid container>                               
                                <Grid item md={5}>
                                    <Typography 
                                        style={{
                                            width:'350px'
                                          }
                                        }
                                    >
                                        Radicado Work Manager
                                    </Typography>
                                </Grid>
                                    <Grid item md={6}>
                                    <TextField
                                        name="radicadoWorkManager"
                                        onChange={handleOnChange}
                                        value={formEmbargo?.radicadoWorkManager}     
                                        inputProps={{
                                            style: {
                                                width: '280px'
                                            }
                                        }}                                      
                                    />
                                </Grid>
                            </Grid>                                                                
                            <Grid container>                               
                                <Grid item md={5}>
                                    <Typography
                                     style={{
                                        width:'350px'
                                      }
                                    }
                                    >
                                        Cuenta
                                    </Typography>
                                </Grid>
                                <Grid item md={6}>
                                <TextField
                                        name="numeroCuenta"
                                        onChange={handleOnChange}
                                        value={formEmbargo?.numeroCuenta}     
                                        inputProps={{
                                            style: {
                                                width: '280px'
                                            }
                                        }}                                      
                                    />
                                </Grid>
                            </Grid>  
                            <Grid container>                               
                                <Grid item md={5}>
                                <br/>
                                    <Typography
                                     style={{
                                        width:'350px'
                                      }
                                    }
                                    >
                                        Tipo Embargo
                                    </Typography>
                                </Grid>
                                <Grid item md={6}>
                                    <CHSelect
                                        label=""
                                        name="tipoEmbargoId"
                                        onChange={handleOnChange}
                                        value={formEmbargo?.tipoEmbargoId}
                                    >
                                       {
                                            tiposEmbargos.map((option: TipoEmbargoDto) =>(
                                                <MenuItem key={option.id} 
                                                          value={option.id}
                                                 >{option.nombre}
                                                </MenuItem>
                                            ))
                                        }
                                    </CHSelect>
                                </Grid>
                            </Grid> 
                         <Grid container>
                                <Grid item md={5}>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Valor embargo</Typography>
                                </Grid>
                            <Grid item md={6}>
                                 <TextField
                                        name="valor"
                                        type="number"
                                        onChange={handleOnChange}
                                        value={formEmbargo?.valor}     
                                        inputProps={{
                                            style: {
                                                width: '280px'
                                            }
                                        }}                                      
                                    />
                            </Grid>
                         </Grid>
                         <Grid container>
                                <Grid item md={5}>
                               <br/>
                                    <Typography
                                     style={{
                                        width:'350px'
                                      }
                                    }
                                    
                                    >Fecha de Inicio Embargo</Typography>
                                </Grid>
                            <Grid item md={6}>
                                <CHDatePicker
                                    label=""
                                    name="fechaInicio"
                                    value={formEmbargo?.fechaInicio as Date}
                                    onChange={handleOnChangeFechaInicio}
                                />
                            </Grid>
                           </Grid>
                           <Grid container>
                                <Grid item md={5}>
                               <br/>
                                    <Typography
                                     style={{
                                        width:'350px'
                                      }
                                    }
                                    
                                    >Fecha de Fin Embargo</Typography>
                                </Grid>
                            <Grid item md={6}>
                                <CHDatePicker
                                    label=""
                                    name='fechaFin'
                                    value={formEmbargo?.fechaFin}
                                    onChange={handleOnChangeFechaFin}
                                />
                            </Grid>
                           </Grid>
                           <Grid container>                               
                                <Grid item md={5}>
                                <br/>
                                    <Typography
                                     style={{
                                        width:'350px'
                                      }
                                    }
                                    >
                                        Tipo de Retención
                                    </Typography>
                                </Grid>
                                <Grid item md={6}>
                                <CHSelect
                                        label=""
                                        name="tipoRetencionId"
                                        onChange={handleOnChange}
                                        value={formEmbargo?.tipoRetencionId}
                                    >
                                       {
                                            tiposRetencion.map((option: TipoRetencionDto) =>(
                                                <MenuItem key={option.id} 
                                                          value={option.id}
                                                 >{option.nombre}
                                                </MenuItem>
                                            ))
                                        }
                                    </CHSelect>
                                </Grid>
                            </Grid> 
                            <Grid container>                               
                                <Grid item md={5}>
                                <br/>
                                    <Typography
                                     style={{
                                        width:'350px'
                                      }
                                    }
                                    >
                                        Indicador Cesantías
                                    </Typography>
                                </Grid>
                                <br/>
                                <br/>
                                <Grid item md={6}>
                                    <Checkbox 
                                        onChange={handleOnChecked}
                                        checked={formEmbargo?.indicadorCesantias}
                                        color='primary'
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
                                         onClick={handleOnClickEdit}
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
                        <br/>
                </DialogContent>
            </Dialog>
        </div>
    );
};