


import React, { useEffect, useState } from 'react';


import { Accordion, AccordionDetails, AccordionSummary, Button, Card, CardContent, createStyles, Grid, makeStyles, Modal, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@material-ui/core';
import { toast } from 'react-toastify';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import UtilService from '../../../../utils/util.service';
import ApiClients from '../../../../services/api-clients.service';
import SolicitudApi from '../../../../services/api-solicitud.service';


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
        fullScreenDialog: {
            width: '50%',
        },
        formRow: {
            display: 'flex',
            flexDirection: 'row',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        columnLabel: {
            padding: '10px',
        },

        inline: {
            display: 'inline',
        }
    })
);

const arrayIteresCalculado = [{
    'cuenta':12566,
    'concepto': 'INTERESES APORTE',
    'no_abonado': '0',
    'interes_acumulado': '0',
},
{
    'cuenta':12577,
    'concepto': 'INTERESES CESANTÍAS',
    'no_abonado': '0',
    'interes_acumulado': '0',
},
{
    'cuenta':12588,
    'concepto': 'INTERESES AVC',
    'no_abonado': '0',
    'interes_acumulado': '0',
},
{
    'cuenta':12599,
    'concepto': 'INTERESES AVAC',
    'no_abonado': '0',
    'interes_acumulado': '0',
},
{
    'cuenta':12599,
    'concepto': 'INTERESES AVAV',
    'no_abonado': '0',
    'interes_acumulado': '0',
},

]

const ModalCuentasAfectadas = (props: any) => {

    
    const apiSolicitud =  SolicitudApi()

    const { responseSolicitud } = props
    const [open, setOpen] = useState(false);

    const [cuentasAfectadas, setCuentasAfectadas] = useState<any[]>([])
    const [cuentasAfectadasDos, setCuentasAfectadasDos] = useState<any[]>([])
   
   

    const openModal = () => {

        setOpen(true)
    }
    const handleClose = () => {
        setOpen(false);
    };

    

    useEffect(()=> {
        obtenerCuentasPorIdentificacion()
        obtenerCuentasAfectadasCliente()
    },[])

    const obtenerCuentasAfectadasCliente = async () => {
        try {
            const request = {
                solicitudId: responseSolicitud.solicitudId,
                identificacion: responseSolicitud.identificacionCliente,
            };
    
            const response = await apiSolicitud.PostObtenerCuentasAfectadasCliente(request);
    
            if (response && response.isSuccess) {
                
                let cuentasAfectadas:any = [];
                if (responseSolicitud.nombreTarea === 'Verificación cuenta individual' || 
                    responseSolicitud.nombreTarea === 'Verificación cuenta individual Giro de recursos') {
                    const nombreCuenta = "CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS";
                    const nombreCuentaAvac = "AHORRO VOLUNTARIO AFILIADO CESANTÍAS";
                    const nombreCuentaCisv = "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA";
    
                    const cuentasIndividuales = response?.data.filter((x:any) => x?.descripcioN_CUENTA === nombreCuenta);
                    const cuentasAvac = response.data.filter((x:any) => x?.descripcioN_CUENTA === nombreCuentaAvac);
                    const cuentasCisv = response.data.filter((x:any) => x?.descripcioN_CUENTA === nombreCuentaCisv);
    
                    if (cuentasIndividuales?.length > 0 && cuentasAvac?.length > 0 ) {
                        cuentasAfectadas = [cuentasIndividuales[0], cuentasAvac[0]];
                    }else if(cuentasIndividuales?.length > 0 && cuentasCisv?.length > 0){
                        cuentasAfectadas = [cuentasIndividuales[0], cuentasCisv[0]];
                    }else {
                        cuentasAfectadas = [cuentasIndividuales[0]]
                    }
                } else {
                    const nombreCuenta = "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA";
                    const cuentas = response?.data.filter((x:any) => x?.descripcioN_CUENTA === nombreCuenta);
    
                    if (cuentas.length > 0) {
                        cuentasAfectadas = [cuentas[0]];
                    }
                }
    
                setCuentasAfectadas(cuentasAfectadas);
            } else {
                toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
            }
        } catch (error) {
            toast("Error del sistema", { type: toast.TYPE.ERROR });
        }
    };
    


    const obtenerCuentasPorIdentificacion = () => {
            const request = {
                solicitudId:  responseSolicitud.solicitudId,
                identificacion: responseSolicitud.identificacionCliente,
              }
        apiSolicitud
            .PostObtenerConceptosAfectadosCliente(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response?.data !=='') {
                            
                        if(responseSolicitud?.nombreTarea === 'Verificación cuenta individual' ||
                           responseSolicitud?.nombreTarea === 'Verificación cuenta individual Giro de recursos'){
                           
                            setCuentasAfectadasDos(response?.data)
                        }else{
                            setCuentasAfectadasDos(response?.data)
                           
                        }
                        
                    } else {
                        toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
                    }
                }
            })
            .catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            });
    };
    
    return (
        <>
            <Button
                variant="contained" color="primary"
                onClick={() => openModal()}
            >Cuentas Afectadas
            </Button>


            <ModalCuentas
                open={open}
                onClose={handleClose}
                cuentasAfectadas={cuentasAfectadas}        
                cuentasAfectadasDos={cuentasAfectadasDos}        
                 />

        </>

    )
}


interface Props {
    open: boolean;
    onClose: () => void;
    cuentasAfectadas:any;
    cuentasAfectadasDos:any;
   

}

const ModalCuentas = (props: Props) => {
    const { open, onClose,  cuentasAfectadas, cuentasAfectadasDos } = props;

    const classes = useStyles();
  

 
    return (
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

                <Card>
                    <CardContent>
                        <Grid item>
                            <Typography
                                className={classes.titleform}
                                variant="h5"
                                component="h6"
                            >
                                CUENTAS AFECTADAS
                            </Typography>
                        </Grid>

                        <br />
                        <br />

                <div>
                <Accordion>
    <AccordionSummary
        expandIcon={<ExpandMoreIcon />}
        aria-controls="panel1a-content"
        id="panel1a-header"
    >
        <Typography>Cuentas Afectadas</Typography>
    </AccordionSummary>
    <AccordionDetails>
        <TableContainer component={Paper}>
            <Grid
                className={classes.columnButtonAdd}
                item
                xs={12}
                md={12}
            >
                <Table aria-label="simple table">
                                                <TableHead style={{ backgroundColor:'#1C3F8B' }}>
                                                    <TableRow >
                                                        <TableCell style={{color:'#fff', textAlign:'center'}}>Numero de cuenta</TableCell>
                                                        <TableCell style={{color:'#fff', textAlign:'center'}}>Cuenta</TableCell>
                                                        <TableCell style={{color:'#fff', textAlign:'center'}} align="right">Valor pago</TableCell>
                                                    </TableRow>
                                                </TableHead>
                    <TableBody>
                        {cuentasAfectadas?.map((option:any, index:any) => (
                            <TableRow key={option?.tipoCuenta} style={{ backgroundColor: index % 2 === 0 ? '#f0f0f0' : 'inherit' }}>
                                <TableCell>
                                    {option?.numerO_CUENTA}
                                </TableCell>
                                <TableCell>
                                    {option?.descripcioN_CUENTA}
                                </TableCell>
                                <TableCell align="right">
                                    {UtilService.formatCurrency(
                                        Number(option?.totaL_DESCONTADO_CUENTA)
                                    )}
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
           
                <Grid container justify="flex-end" style={{ marginRight: '80px' }}>
                    <Typography variant="subtitle1" align="right">
                        Total valor a pagar: {' '}
                        <strong>
                            {UtilService.formatCurrency(
                                Number(cuentasAfectadas[0]?.totaL_DESCONTADO)
                            )}
                        </strong>
                    </Typography>
                </Grid>
            </Grid>
        </TableContainer>
    </AccordionDetails>
</Accordion>



                  
                    <Accordion>
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls="panel2a-content"
                            id="panel2a-header"
                        >
                            <Typography>Conceptos Afectados</Typography> 
                        </AccordionSummary>
                        <AccordionDetails>
                            
                        <TableContainer component={Paper}>
                            <Grid
                                className={classes.columnButtonAdd}
                                item
                                xs={12}
                                md={12}
                            >
                                <Table aria-label="simple table">
                                    <TableHead style={{ backgroundColor:'#1C3F8B' }}>
                                        <TableRow>
                                            <TableCell style={{color:'#fff', textAlign:'center'}}>Cuenta</TableCell>
                                            <TableCell style={{color:'#fff', textAlign:'center'}}>Concepto</TableCell>
                                            <TableCell style={{color:'#fff', textAlign:'center'}}>Disponible</TableCell>
                                            <TableCell style={{color:'#fff', textAlign:'center'}}>Monto</TableCell>
                                            <TableCell style={{color:'#fff', textAlign:'center'}}>Interés acumulado</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>

                                        {cuentasAfectadasDos?.map(
                                            (option: any, index:any) => (
                                                <TableRow key={option?.ctC_DESCRIPCION} style={{ backgroundColor: index % 2 === 0 ? '#f0f0f0' : 'inherit' }}>
                                                    <TableCell>
                                                        {option?.numerO_CUENTA}
                                                    </TableCell>
                                                    <TableCell>
                                                        {option?.ctC_DESCRIPCION}
                                                    </TableCell>
                                                   
                                                    <TableCell align="right">
                                                        {UtilService.formatCurrency(
                                                            Number(
                                                                option?.caC_SALDO
                                                            ),
                                                        )}
                                                    </TableCell>
                                                    <TableCell align="right">
                                                        {UtilService.formatCurrency(
                                                            Number(
                                                                option?.caC_VALOR_DESCONTADO
                                                            ),
                                                        )}
                                                    </TableCell>
                                                    <TableCell align="right">
                                                        {UtilService.formatCurrency(
                                                            Number(
                                                                '0',
                                                            ),
                                                        )}
                                                    </TableCell>
                                         
                                                </TableRow>
                                                
                                            ),
                                        )}
                                       
                                    </TableBody>
                                   
                                </Table>
                       
                            </Grid>
                        </TableContainer>
                        </AccordionDetails>
                    </Accordion>
                    <Accordion>
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls="panel3a-content"
                            id="panel3a-header"
                        >
                            <Typography>Interés Calculado</Typography>
                        </AccordionSummary>
                        <AccordionDetails>
                            
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
                                            <TableCell>Cuenta</TableCell>
                                            <TableCell>Concepto</TableCell>
                                            <TableCell>No abonado</TableCell>
                                            <TableCell>Interés acumulado</TableCell>
                                            
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>

                                        {arrayIteresCalculado?.map(
                                            (option: any) => (
                                                <TableRow key={option.cuenta}>
                                                    <TableCell>
                                                        {option.cuenta}
                                                    </TableCell>
                                                    <TableCell>
                                                    {option.concepto}
                                                    </TableCell>
                                                   
                                                    <TableCell>
                                                      {option.no_abonado}
                                                    </TableCell>
                                                    <TableCell>
                                                        {option.interes_acumulado}
                                                    </TableCell>
                                               
                                         
                                                </TableRow>
                                                
                                            ),
                                        )}
                                       
                                    </TableBody>
                                   
                                </Table>
                             
                            </Grid>
                        </TableContainer>
                        </AccordionDetails>
                    </Accordion>
                 
                </div>
                <br />
                        <br />
                        <Grid container justify="flex-end" spacing={2}>
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
                </CardContent>
                </Card>
            </Modal>
        </>
    );
};



export default ModalCuentasAfectadas