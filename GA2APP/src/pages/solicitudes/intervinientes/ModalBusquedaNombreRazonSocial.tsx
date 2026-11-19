
import React, { useEffect } from "react";
import { Button, Card, TableHead, CardContent, createStyles, Grid, makeStyles, Modal, Paper, Table, TableCell, TableContainer, TableRow, Typography, TableBody, Tooltip, TextField } from "@material-ui/core";
import { ArrowForward, BackspaceSharp, Block, Search } from "@material-ui/icons";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import CHTextField from "../../../components/ch-text-field/CHTextField";
import SolicitudApi from "../../../services/api-solicitud.service";


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
        },
        modalContent: {
            maxHeight: '70vh',
            overflowY: 'auto',
        },
        buttonBuscar: {
            backgroundColor: '#1565c0',
            '&:hover': {
                backgroundColor: 'black',
            },
        },
        buttonCancelar: {
            backgroundColor: '#2196f3',
            '&:hover': {
                backgroundColor: '#1e88e5',
            },
        },
        buttonLimpiar: {
            backgroundColor: '#64b5f6',
            '&:hover': {
                backgroundColor: '#42a5f5',
            },
        },
        searchField: {
            marginBottom: '20px',
        },
    })
);

const ModalBusquedaNombreRazonSocial = ({
    onClose,
    open,
    buscarUsuarioIngreso
}:
    {
        onClose: any;
        open: boolean;
        buscarUsuarioIngreso: any
    }) => {

    const [listados, setListados] = React.useState<any[]>([]);
    const [filteredData, setFilteredData] = React.useState<any[]>([]);
    const [encontrado, setEncontrado] = React.useState(true);
    const [message, setMessage] = React.useState('');
    const [isLoading, setIsLoading] = React.useState(false);
    const [searchQuery, setSearchQuery] = React.useState('');

    const formulario = useForm({
        defaultValues: {
            nombres: '',
            apellidos: ''
        }
    });

 
    const apiSolicitud = SolicitudApi();
    const classes = useStyles();

    const buscar = async (data: any) => {
        setFilteredData([]);
        setIsLoading(true);
        const request = {
            nombres_usuario: data.nombres,
            apellidos_usuario: data.apellidos
        };
        try {
          
            const response = await apiSolicitud.PostBuscarUsuarioIngreso(request);
            if (response && response.isSuccess && response.data.length > 0) {
                setListados(response.data);
                setFilteredData(response.data);
                setMessage(response.returnMessage);
                setEncontrado(true);
            } else {
                setListados([]);
                setFilteredData([]);
                setMessage('No hay resultados para esta búsqueda');
                setEncontrado(false);
            }
        } catch (error) {
            toast("Error en la búsqueda", { type: toast.TYPE.ERROR });
            setListados([]);
            setFilteredData([]);
            setMessage('Error en la búsqueda');
            setEncontrado(false);
        } finally {
            setIsLoading(false);
        }
    };

 
    const limpiarCampos = () => {
        formulario.reset();
        setSearchQuery('')
        setFilteredData([])
    }

    const enviarBuscar = async (data: any) => {
        await buscarUsuarioIngreso(data);
        onClose(false);
    };



    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
       
        const query = event.target.value.toLowerCase();
        setSearchQuery(query);
        setFilteredData([]);
        if (query === "") {
            
            setFilteredData(listados);
        } else {
            setFilteredData(listados.filter(item =>
                item.identificacioN_USUARIO.toLowerCase() === query ||
                item.nombreS_USUARIO.toLowerCase() === query ||
                item.apellidoS_USUARIO.toLowerCase() === query
            ));
        }
      
    };

    return (
        <Modal
            open={open}
            onClose={onClose}
            aria-labelledby="form-dialog-title"
            style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                height: '90vh',
                width: '100%',
            }}
        >
            <Card style={{ width: '30%', textAlign: 'center' }}>
                <CardContent className={classes.modalContent}>
                    <Grid item>
                        <Typography variant="h5" component="h6">
                            Buscador Nombres/Razón Social
                        </Typography>
                    </Grid>
                    <br />
                    <br />
                    <Grid>
                        <Grid style={{ display: 'flex' }} item xs={12} md={12} lg={12}>
                            <Controller
                                name="nombres"
                                control={formulario.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Nombres"
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid style={{ display: 'flex' }} item xs={12} md={12} lg={12}>
                            <Controller
                                name="apellidos"
                                control={formulario.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Apellidos"
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <br />
                    <br />
                    <Grid container justify="flex-end" spacing={3}>
                        <Grid item>
                            <Tooltip title="Buscar">
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"
                                    onClick={formulario.handleSubmit(buscar)}
                                    disabled={isLoading}
                                    className={classes.buttonBuscar}
                                >
                                    {isLoading ? "Buscando..." : <Search />}
                                </Button>
                            </Tooltip>
                        </Grid>
                        <Grid item>
                            <Tooltip title="Cancelar">
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"
                                    onClick={onClose}
                                    className={classes.buttonCancelar}
                                >
                                    <Block />
                                </Button>
                            </Tooltip>
                        </Grid>
                        <Grid item>
                            <Tooltip title="Limpiar Campos">
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"
                                    onClick={limpiarCampos}
                                    className={classes.buttonLimpiar}
                                >
                                    <BackspaceSharp />
                                </Button>
                            </Tooltip>
                        </Grid>
                    </Grid>
                    <br />
                    <br />

                    {encontrado ? (
                        <TableContainer component={Paper}>
                            <br />
                            <br />
                            {
                            filteredData?.length > 0?
                                <>
                                <h2 style={{ color: '#01579B' }}>  {`${message}.` } </h2>
                                <h4>{`Encontrado: ${filteredData.length}`}</h4>
                                </>:null
                            
                              
                            }
                           
                            <TextField
                                className={classes.searchField}
                                label="Buscar en la tabla"
                                variant="outlined"
                                fullWidth
                                value={searchQuery}
                                onChange={handleSearchChange}
                            />
                            <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                                <Table aria-label="simple table">
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>Identificación</TableCell>
                                            <TableCell>Nombres</TableCell>
                                            <TableCell>Apellidos</TableCell>
                                            <TableCell>Acción</TableCell>
                                        </TableRow>
                                    </TableHead> {
                                         filteredData?.length > 0 ?
                                         <TableBody>

                                        {filteredData.map((option: any) => (
                                            <TableRow key={option.identificacioN_USUARIO}>
                                                <TableCell>{option.identificacioN_USUARIO}</TableCell>
                                                <TableCell>{option.nombreS_USUARIO}</TableCell>
                                                <TableCell>{option.apellidoS_USUARIO}</TableCell>
                                                <TableCell>
                                                    <Button onClick={() => enviarBuscar(option)}>
                                                        <ArrowForward style={{ color: 'blue' }} />
                                                    </Button>
                                                </TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>:''
                                    }
                                    
                                </Table>
                            </Grid>
                        </TableContainer>
                    ) : (
                        <p style={{ color: '#01579B' }}>{message}</p>
                    )}
                </CardContent>
            </Card>
        </Modal>
    );
};

export default ModalBusquedaNombreRazonSocial;


