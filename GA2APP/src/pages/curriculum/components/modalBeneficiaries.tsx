import React from 'react';
import {
    Button,
    Dialog,
    Grid,
    Typography,
    createStyles,
    makeStyles,
} from '@material-ui/core';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';

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
        button:{
            marginLeft: '.5rem',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        }      
    }),
);

export const ModalBeneficiaries = (props: any) => {     
    const {
        data,
        open,
        handleClose,
        afiliado  
    } = props;
    const classes = useStyles();   
    const stringAfiliado=`Beneficiario de ${String(afiliado).includes('Es Beneficiario de')?'':afiliado}`;
    const stringBeneficiario=`${data!=null?data.identificacion.toString():""}-${data!=null?data.nombre.toString():""}`;
    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <CHPaper title={stringAfiliado}>
                    <Typography className={classes.titleform} variant="h5" component="h6">
                        {stringBeneficiario.toString()}
                    </Typography>
                    <Grid container>
                        <Grid container spacing={3} style={{textAlign:'right'}}>
                            <CHTextField
                                isReadOnly
                                label="Participación"
                                value={data != null ? data.participacion : null}
                            />
                        </Grid>  
                        <Grid container spacing={3} style={{textAlign:'right'}}>
                            <CHTextField
                                isReadOnly
                                label="Fecha adquisición derecho a reclamar"
                                value={data != null ? data.fechaAdquisicion : null}
                             />
                    
                        </Grid>
                        <Grid container spacing={3} style={{textAlign:'right'}}>
                            <CHTextField
                                isReadOnly
                                label="Resolución"
                                value={data != null ? data.resolucion : null}
                            />
                        </Grid>  
                        <Grid container spacing={3} style={{textAlign:'right'}}>
                                <Grid item>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        className={classes.button}
                                        color="default"
                                        onClick={handleClose}
                                        style={{marginLeft: "10px"}}
                                    >
                                        Cancelar
                                    </Button>
                                </Grid>
                         </Grid>
                     </Grid>   
                </CHPaper> 
            
            </Dialog>
        </div>
    );
};