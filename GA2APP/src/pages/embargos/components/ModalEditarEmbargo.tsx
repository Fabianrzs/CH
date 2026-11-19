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
} from '@material-ui/core';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import { CITYDTO, DEPARTAMENTODTO, IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import { JuzgadoDto } from '../../../types/court.model.type';
import { TipoEmbargoDto, TipoProcesoEmbargoDto } from '../../../types/embargo.model.type';

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
export const ModalEditarEmbargo = (props: any) => {    
    const {
        handleClose,
        open,
        handleOnClickEdit,
        tiposIdentificacion,
        allDepartaments,
        allCities,
        handleOnChange,
        handleOnChangeDepartament,
        handleOnChangeCity,
        handleOnChangeDateWorkManager,
        handleOnChangeDateJuzgado,
        tiposEmbargo,
        tiposProcesos,
        juzgados,
        formEmbargo 
    } = props;
    const classes = useStyles();   
    console.log(juzgados);
    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogContent>
                        <Grid item>
                                <br/>
                                    <Typography
                                     className={classes.titleform}
                                     variant="h5"
                                     component="h6"
                                    >
                                        ACTUALIZAR EMBARGO                                      
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
                                        Tipo de Identificación
                                    </Typography>
                                </Grid>
                                <Grid item md={7}>
                                <CHSelect
                                    label=""
                                    name="clienteTipoIdentificacion"
                                    value={formEmbargo?.clienteTipoIdentificacion === undefined? 1 : 
                                           formEmbargo?.clienteTipoIdentificacion}
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
                                    <Typography 
                                        style={{
                                            width:'350px'
                                          }
                                        }
                                    >
                                        No. identificación afiliado
                                    </Typography>
                                </Grid>
                                <Grid item md={7}>
                                    <TextField
                                           name="identificacionCliente"
                                           onChange={handleOnChange}
                                           value={formEmbargo?.identificacionCliente}   
                                            inputProps={{
                                                style: {
                                                    width: '500px'
                                                }
                                            }}                                     
                                        />
                                </Grid>
                            </Grid>                                                                
                            <Grid container>                               
                                <Grid item md={4}>
                                    <Typography
                                     style={{
                                        width:'350px'
                                      }
                                    }
                                    >
                                        No. Radicado Work Manager
                                    </Typography>
                                </Grid>
                                <Grid item md={7}>
                                    <TextField
                                          name="radicadoWorkManager"
                                          onChange={handleOnChange}
                                          value={formEmbargo?.radicadoWorkManager}     
                                        inputProps={{
                                            style: {
                                                width: '500px'
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
                                        width:'350px'
                                      }
                                    }
                                    
                                    >Fecha Radicado Work Manager</Typography>
                                </Grid>
                            <Grid item md={7}>
                                <CHDatePicker
                                       label=""
                                       name="fechaRadicadoWorkManager"
                                       value={formEmbargo?.fechaRadicadoWorkManager as Date}
                                       onChange={handleOnChangeDateWorkManager}
                                />
                            </Grid>
                           </Grid>
                            <Grid container>                               
                                <Grid item md={4}>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>
                                        No. Radicado Oficio Juzgado
                                    </Typography>
                                </Grid>
                                <Grid item md={7}>
                                    <TextField
                                         name="radicadoJuzgado"
                                         onChange={handleOnChange}
                                         value={formEmbargo?.radicadoJuzgado}    
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    />
                                </Grid>
                            </Grid>
                            <Grid container>
                                <Grid item md={4}>
                                <br/>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Fecha Radicado Oficio Juzgado</Typography>
                                </Grid>
                            <Grid item md={7}>
                                <CHDatePicker
                                    label=""
                                    name="fechaRadicadoJuzgado"
                                    value={formEmbargo?.fechaRadicadoJuzgado}
                                    onChange={handleOnChangeDateJuzgado}
                                />
                            </Grid>
                           </Grid>
                           <Grid container>                               
                                <Grid item md={4}>
                                <br/>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>
                                        Departamento Juzgado
                                    </Typography>
                                </Grid>
                                <Grid item md={7}>
                                <CHSelect
                                    label=""
                                    name="idDepartamento"
                                    onChange={handleOnChangeDepartament}
                                    // value={formJuzgado?.idDepartamento}
                                >
                                    {
                                        allDepartaments?.map(
                                            (option: DEPARTAMENTODTO) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.nombre}
                                                </MenuItem>
                                            ))
                                    }
                                 </CHSelect>
                                </Grid>
                            </Grid> 
                            <Grid container>                               
                                <Grid item md={4}>
                                <br/>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>
                                        Municipio Juzgado
                                    </Typography>
                                </Grid>
                                <Grid item md={7}>
                                <CHSelect
                                    label=""
                                    name="idCiudad"
                                    onChange={handleOnChangeCity}
                                    // value={formEmbargo?}
                                >
                                    {
                                        allCities?.map(
                                            (option: CITYDTO) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.codigo}
                                                >
                                                    {option.nombre}
                                                </MenuItem>
                                            ))
                                    }
                                 </CHSelect>
                                </Grid>
                            </Grid>   
                            <Grid container>                               
                                <Grid item md={4}>
                                <br/>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>
                                        Juzgado
                                    </Typography>
                                </Grid>
                                <Grid item md={7}>
                                    <CHSelect
                                      label=""
                                      name="juzgadoId"
                                      onChange={handleOnChange}
                                      value={formEmbargo?.juzgadoId}      
                                                                          
                                    >
                                       {
                                            juzgados?.map(
                                                (option: JuzgadoDto) => (
                                                    <MenuItem
                                                        key={option.idJuzgado}
                                                        value={option.idJuzgado}
                                                    >
                                                        {option.nombre}
                                                    </MenuItem>
                                                ))
                                       }
                                    </CHSelect>
                                </Grid>
                            </Grid>  
                            <Grid container>
                                <Grid item md={4}>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Email Respuesta</Typography>
                                </Grid>
                            <Grid item md={7}>
                                 <TextField
                                      name="emailRespuesta"
                                      onChange={handleOnChange}
                                      value={formEmbargo?.emailRespuesta}         
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    />
                            </Grid>
                         </Grid>   
                         <Grid container>
                                <Grid item md={4}>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Dirección Respuesta</Typography>
                                </Grid>
                            <Grid item md={7}>
                                 <TextField
                                        name="direccionRespuesta"
                                        onChange={handleOnChange}
                                        value={formEmbargo?.direccionRespuesta}   
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    />
                            </Grid>
                         </Grid> 
                         <Grid container>
                                <Grid item md={4}>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Nombre Demandante</Typography>
                                </Grid>
                            <Grid item md={7}>
                                 <TextField
                                       name="nombreDemandante"
                                       onChange={handleOnChange}
                                       value={formEmbargo?.nombreDemandante}  
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    />
                            </Grid>
                         </Grid>
                         <Grid container>
                                <Grid item md={4}>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }> Nombre Demandado</Typography>
                                </Grid>
                            <Grid item md={7}>
                                 <TextField
                                         name="nombreDemandado"
                                         onChange={handleOnChange}
                                         value={formEmbargo?.nombreDemandado}   
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    />
                            </Grid>
                         </Grid>
                         <Grid container>
                                <Grid item md={4}>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>No. Expediente Banco Agrario</Typography>
                                </Grid>
                            <Grid item md={7}>
                                 <TextField
                                       name="expedienteBancoAgrario"
                                       onChange={handleOnChange}
                                       value={formEmbargo?.expedienteBancoAgrario}    
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    />
                            </Grid>
                         </Grid>
                         <Grid container>
                                <Grid item md={4}>
                                <br/>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Tipo de proceso</Typography>
                                </Grid>
                                <Grid item md={7}>
                                 <CHSelect
                                        label=""
                                        name="procesoId"
                                        onChange={handleOnChange}
                                        value={formEmbargo?.procesoId}       
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    >
                                        {
                                            tiposProcesos.map((option: TipoProcesoEmbargoDto) =>(
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
                                <Grid item md={4}>
                                <br/>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Tipo de embargo</Typography>
                                </Grid>
                                <Grid item md={7}>
                                 <CHSelect
                                        label=""
                                        name="tipoEmbargoId"
                                        onChange={handleOnChange}
                                        value={formEmbargo?.tipoEmbargoId}      
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    >
                                        {
                                            tiposEmbargo.map((option: TipoEmbargoDto) =>(
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
                                <Grid item md={4}>
                                <br/>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Valor embargo</Typography>
                                </Grid>
                            <Grid item md={7}>
                                 <TextField
                                         name="valor"
                                         type="number"
                                         onChange={handleOnChange}
                                         value={formEmbargo?.valor}   
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    />
                            </Grid>
                         </Grid>
                         <Grid container>
                                <Grid item md={4}>
                                <br/>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Remanente</Typography>
                                </Grid>
                            <Grid item md={7}>
                                 <TextField
                                        name="remanente"
                                        onChange={handleOnChange}
                                        value={formEmbargo?.remanente}  
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    />
                            </Grid>
                         </Grid>
                         <Grid container>
                                <Grid item md={4}>
                                <br/>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Observaciones</Typography>
                                </Grid>
                            <Grid item md={7}>
                                 <TextField
                                        id="outlined-multiline-flexible"
                                        name="observaciones"
                                        multiline
                                        maxRows={4}
                                        onChange={handleOnChange}
                                        value={formEmbargo?.observaciones}     
                                        inputProps={{
                                            style: {
                                                width: '500px'
                                            }
                                        }}                                      
                                    />
                            </Grid>
                         </Grid>
                         <Grid container>
                                <Grid item md={4}>
                                <br/>
                                    <Typography  style={{
                                            width:'350px'
                                          }
                                        }>Respuesta</Typography>
                                </Grid>
                            <Grid item md={7}>
                                 <TextField
                                        id="outlined-multiline-flexible"
                                        name="respuesta"
                                        multiline
                                        maxRows={5}
                                        onChange={handleOnChange}
                                        value={formEmbargo?.respuesta}       
                                        inputProps={{
                                            style: {
                                                width: '500px'
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