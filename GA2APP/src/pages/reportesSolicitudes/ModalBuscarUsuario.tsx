import React from "react";
import { Button, Card, CardContent, createStyles, Grid, makeStyles, Modal, Typography, Tooltip, TextField } from "@material-ui/core";
import { ArrowForward } from "@material-ui/icons";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import ApiSolicitudes from '../../services/api-solicitud.service';
import CHTextField from "../../components/ch-text-field/CHTextField";
import { LoadingComponent } from "../../components/loading/loading.component";

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '19px',
        },
        modalContent: {
            maxHeight: '70vh',
            overflowY: 'auto',
        },
        searchField: {
            marginBottom: '20px',
        },
        columnButton: {
            padding: '10px',
            textAlign: 'right',
        },
    })
);

const ModalBuscarUsuario = ({
    open,
    onClose,
    buscarUsuarioIngreso
}:
    {
        open: any;
        onClose: any;
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


    const apiSolicitud = ApiSolicitudes();
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
                const mensaje = response.returnMessage.split("nombre ")
                setMessage(`${mensaje[0]} nombre "${mensaje[1]}"`);
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
        <>
            <LoadingComponent loading={isLoading} />
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
                <Card style={{ width: '50%', textAlign: 'center' }}>
                    <CardContent className={classes.modalContent}>
                        <Grid item>
                            <Typography
                                className={classes.titleform}
                                variant="h5"
                                component="h6"
                            >
                                BÚSQUEDA USUARIO
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
                        <Grid container>
                            <Grid className={classes.columnButton} item xs={12} md={12}>
                                <Button variant="contained" color="primary" style={{ width: '16%' }} onClick={formulario.handleSubmit(buscar)}>
                                    Consultar
                                </Button>
                                <Button variant="contained" color="primary" style={{ width: '16%', marginLeft: '10px' }} onClick={limpiarCampos}>
                                    Cancelar
                                </Button>
                                <Button variant="contained" color="inherit" style={{ width: '16%', marginLeft: '10px' }} onClick={onClose}>
                                    Regresar
                                </Button>
                            </Grid>
                        </Grid>
                        {encontrado ? (
                            <>
                                <br />
                                <br />
                                {
                                    filteredData?.length > 0 ?
                                        <>
                                            <h2 className={classes.titleform}>  {`${message}`} </h2>
                                            <h4 style={{color: '#1C3F6B'}}>{`Total Encontrados: ${filteredData.length}`}</h4>
                                        </> : null
                                }
                              
                                        <TextField
                                            className={classes.searchField}
                                            label="Buscar en la tabla"
                                            variant="outlined"
                                            fullWidth
                                            value={searchQuery}
                                            onChange={handleSearchChange}
                                        />
                               
                                        <table className="client-table">
                                            <thead>
                                                <tr>
                                                    <th style={{ width: '300px', textAlign: 'center' }}>Identificación</th>
                                                    <th style={{ width: '300px', textAlign: 'center' }}>Nombres</th>
                                                    <th style={{ width: '300px', textAlign: 'center' }}>Apellidos</th>
                                                    <th style={{ width: '200px', textAlign: 'center' }}>Acción</th>
                                                </tr>
                                            </thead> {
                                                filteredData?.length > 0 ?
                                                    <tbody>

                                                        {filteredData.map((option: any) => (
                                                            <tr key={option.identificacioN_USUARIO}>
                                                                <td>{option.identificacioN_USUARIO}</td>
                                                                <td>{option.nombreS_USUARIO}</td>
                                                                <td>{option.apellidoS_USUARIO}</td>
                                                                <td>
                                                                 <Tooltip title="Agregar">
                                                                    <Button onClick={() => enviarBuscar(option)}>
                                                                        <ArrowForward color="primary"/>
                                                                    </Button>
                                                                 </Tooltip>
                                                                </td>
                                                            </tr>
                                                        ))}
                                                    </tbody> : ''
                                            }

                                        </table>
                            </>
                        ) : (
                            <p style={{ color: '#1C3F6B' }}>{message}</p>
                        )}
                    </CardContent>
                </Card>
            </Modal>
        </>
    );
};

export default ModalBuscarUsuario;