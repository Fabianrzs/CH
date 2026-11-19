import React, { useState } from 'react';
import { Grid, IconButton } from '@material-ui/core';
import { connect } from 'react-redux';
import { useForm, UseFormReturn } from 'react-hook-form';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import MaterialTable from 'material-table';
import moment, { months } from 'moment';
import { Edit as EditIcon, Delete as DeleteIcon, Check } from '@material-ui/icons';
import { AppState } from '../../store/rootReducer';
import { CourtsState } from '../../reducers/courts.reducer';
import { CITYDTO, DEPARTAMENTODTO } from '../../types/catalog.type';
import { DespachosJudiciales, JuzgadoDto } from '../../types/court.model.type';
import {
    startPostUpdateCourt,
    startDeleteCourt,
    startPostInsertCourt,
} from '../../actions/courts.action';
import { juzgadoForm } from './constants/courtform.model.type';
import apiCatalog from '../../services/api-catalog.service';
import { ModalCourtAdd } from './components/ModalCourtAdd';
import { GUID_DEFAULT } from '../../constants/constants';
import ApiCourt from '../../services/api-courts.service'
import NotificationService from '../../services/notification.service';




const TableCourt = (props: props) => {
    const api = apiCatalog();

    const {
        courtsState,
        informacion,
        startPostUpdateCourt,
        startPostInsertCourt,
        startDeleteCourt,
        obtenerCiudades,
        obtenerDespachosJudiciales,
        formularioDespachosJudiciales,
        allTipoActor,
        estadoJuzgado
    } = props;


    const formulario = useForm<JuzgadoDto>({
        defaultValues: juzgadoForm,
    });

    const [openModalEdit, setOpenModalEdit] = useState(false);
    const [allDepartaments, setAllDepartaments] = useState<DEPARTAMENTODTO[]>(
        [],
    );

    const columns: any = [

        {
            title: 'Tipo Actor',
            field: 'tipoActorDescripcion',
            align: 'center',
            cellStyle: {
                border: '1px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopLeftRadius: '12px',
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Nombre',
            field: 'nombre',
            align: 'center',
            cellStyle: {
                border: '1px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopLeftRadius: '12px',
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Código',
            field: 'codigoInterno',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Oficina de destino',
            field: 'codigoOficina',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Cuenta de Depósito',
            field: 'nroCuenta',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Departamento',
            field: 'departamento',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Municipio',
            field: 'ciudad',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Correo Electrónico',
            field: 'correoElectronico',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
    ];


    const [noJuzgado, setNoJuzgado] = useState(true);
    const [nuevoTipoActorEnabled, setNuevoTipoActorEnabled] = useState(true);
    const [estado, setEstado] = useState([]);
    const [departamentValue, setDepartamentValue] = useState<DEPARTAMENTODTO | null>(null);
    const [nullDepartamentValue, setNullDepartamentValue] = useState(false);
    const [cityValue, setCityValue] = useState<CITYDTO | null>(null);
    const [nullCityValue, setNullCityValue] = useState(false);
    const [informacio, setInformacio] = useState<JuzgadoDto>();

    const apiCourt = ApiCourt();




    const handleOpenModalEdit = async (data: JuzgadoDto) => {
        let departamento: any = null;
        let ciudad: any = null;
        obtenerDepartamentos();
        formulario.setValue('idJuzgado', data.idJuzgado);
        formulario.setValue('tipoActor', data.tipoActor);
        formulario.setValue('nombre', data.nombre);
        formulario.setValue('codigoInterno', data.codigoInterno);
        formulario.setValue('codigoOficina', data.codigoOficina);
        formulario.setValue('nroCuenta', data.nroCuenta);
        formulario.setValue('idDepartamento', data.idDepartamento);
        formulario.setValue('idCiudad', data.idCiudad);
        formulario.setValue('correoElectronico', data.correoElectronico);
        setNullDepartamentValue(false);

        await api
            .getAllStates()
            .then(async (response) => {
                if (response.data) {
                    departamento = response.data?.find(
                        (item: any) => item.id === data.idDepartamento
                    );
                }
            })
            .catch(() => {
                toast('Error al obtener los departamentos');
            });

        setDepartamentValue((prevDepartamento) => departamento || prevDepartamento);
        obtenerCiudades(data.idDepartamento);

        await api
            .getCityOfState(data.idDepartamento.toString())
            .then(async (response) => {
                if (response.data) {
                    ciudad = response.data?.find(
                        (item: any) => item.id === data.idCiudad
                    );
                }
            })
            .catch(() => {
                toast('Error al obtener las ciudades');
            });

        setNullCityValue(false);
        setCityValue((prevCiudad) => ciudad || prevCiudad);
        const selectedOption = allTipoActor.find(
            (member: any) => member.idTipoActorEmbargo === data.tipoActor
        );
        setNuevoTipoActorEnabled(selectedOption?.idTipoActorEmbargo === 7);
        setNoJuzgado(selectedOption?.idTipoActorEmbargo === 5);
        setEstado({
            DepartamentValue: departamento || null,
            setCityValue: ciudad || null,
        });
        setOpenModalEdit(true);
        setInformacio(data);
    };


    const handleCloseModalEdit = () => {
        setOpenModalEdit(false);
    };

    const obtenerDepartamentos = async () => {
        await api
            .getAllStates()
            .then(async response => {
                const datos = response.data as DEPARTAMENTODTO[];
                const array = datos.filter(item => item.id !== 0);
                setAllDepartaments(array);
            })
            .catch(() => {
                toast('Error al obtener los departamentos');
            });
    };

    const handleEdit = async (data: JuzgadoDto) => {
        const codigoYDeposito = {
            codigoInterno: data.codigoInterno,
            nCuenta: data.nroCuenta
        };
        const apiCourt = ApiCourt();
        const response = await apiCourt.postAllCourtsByCodeAndAccountnumber(codigoYDeposito);
        const datos = response.data as JuzgadoDto[];

        if (datos.length === 0 || data.idJuzgado === datos[0]?.idJuzgado && datos.length === 1) {

            handleCloseModalEdit();
            await startPostUpdateCourt(data);
            handleCloseModalEdit();

            const valoresBusqueda = formularioDespachosJudiciales.getValues();
            obtenerDespachosJudiciales(valoresBusqueda);
        } else {
            datos.map((dato) => {
                if (data.codigoInterno === dato?.codigoInterno && data.idJuzgado !== dato?.idJuzgado) {
                    swal({
                        text: 'Código interno existente, por favor verificar.',
                        icon: 'warning',
                    });
                }
                if (data.nroCuenta === dato?.nroCuenta && data.idJuzgado !== dato?.idJuzgado) {
                    swal({
                        text: 'Numero de cuenta existente, por favor verificar.',
                        icon: 'warning',
                    });
                }
                return null;
            })
        }

    };

    const handleAdd = async (data: JuzgadoDto) => {
        data.idJuzgado = GUID_DEFAULT;
        startPostInsertCourt(data);

        const valoresBusqueda = formularioDespachosJudiciales.getValues();
        handleCloseModalEdit();
        obtenerDespachosJudiciales(valoresBusqueda);
    };

    const deleteState = async (juzgado: JuzgadoDto) => {
        await startDeleteCourt(juzgado);
        toast.success("Estado modificado con éxito", { autoClose: 5000 });
        const valoresBusqueda = formularioDespachosJudiciales.getValues();
        obtenerDespachosJudiciales(valoresBusqueda);
    };



    const [informaciones, setInformacion] = useState<JuzgadoDto[]>([]);


    const handleDelete = (rowData: any) => {
        const juzgadoDelete: JuzgadoDto = rowData as JuzgadoDto;
        if (estadoJuzgado) {
            swal({
                text: 'El estado del registro se modificará como INACTIVO. ¿Desea realizar esta acción?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
                closeOnClickOutside: false, 
            }).then((respuesta) => {
                if (respuesta === null) {  
                    return;
                }

                deleteState(juzgadoDelete)
                    .then(() => {
                        swal('Registro eliminado exitosamente', {
                            icon: 'success',
                        }).then(() => {
                            const newData = informacion.filter((data: any) => data.id !== juzgadoDelete.idJuzgado);
                            setInformacion(newData);

                            const valoresBusqueda = formularioDespachosJudiciales.getValues();
                            obtenerDespachosJudiciales(valoresBusqueda);
                        });
                    })
                    .catch((error) => {
                        swal('Ha ocurrido un error eliminando el registro', {
                            icon: 'error',
                        });
                    });
            });
        } else {
            swal({
                text: 'El estado del registro se modificará como ACTIVO. ¿Desea realizar esta acción?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
                closeOnClickOutside: false, 
            }).then((respuesta) => {
                if (respuesta === null) { 
                    return;
                }

                deleteState(juzgadoDelete)
                    .then(() => {
                        swal('Registro activado exitosamente', {
                            icon: 'success',
                        }).then(() => {
                            const newData = informacion.filter((data: any) => data.id !== juzgadoDelete.idJuzgado);
                            setInformacion(newData);

                            const valoresBusqueda = formularioDespachosJudiciales.getValues();
                            obtenerDespachosJudiciales(valoresBusqueda);
                        });
                    })
                    .catch((error) => {
                        swal('Ha ocurrido un error activando el registro', {
                            icon: 'error',
                        });
                    });
            });
        }
    };


    const intercalar = (index: number) => {
        let color = '#f1f2f6';
        if (index % 2 === 0) {
            color = '#C4C4C4';
        }

        return {
            background: color,
            border: '1.5px solid #FFF',
        };
    };

    return (
        <>
            <Grid container>
                <Grid item xs={12}>
                    <MaterialTable
                        title=""
                        data={informacion}
                        columns={columns}
                        actions={[
                            {
                                icon: () => (
                                    <IconButton >
                                        <EditIcon />
                                    </IconButton>
                                ),
                                tooltip: 'Editar',
                                onClick: (event, rowData: any) => {
                                    handleOpenModalEdit(rowData);
                                },
                                hidden: !estadoJuzgado
                            },
                            {
                                icon: () => (
                                    <IconButton style={{ color: 'red' }}>
                                        <DeleteIcon />
                                    </IconButton>
                                ),
                                tooltip: 'Eliminar',
                                onClick: (event, rowData: any) => {
                                    handleDelete(rowData);
                                },
                                hidden: !estadoJuzgado
                            },
                            {
                                icon: () => (
                                    <IconButton style={{ color: 'green' }} >
                                        <Check />
                                    </IconButton>
                                ),
                                tooltip: 'Activar',
                                onClick: (event, rowData: any) => {
                                    handleDelete(rowData);
                                },
                                hidden: estadoJuzgado
                            },

                        ]}
                        options={{
                            actionsColumnIndex: -1,
                            headerStyle: {
                                backgroundColor: '#FEC917',
                                fontWeight: 'bold',
                                color: '#676767',
                                borderColor: '#FFF',
                                border: '1.5px solid #FFF',
                                borderTopRightRadius: '12px',
                            },
                            search: true,
                            toolbar: true,
                            rowStyle: (data, index) => intercalar(index),
                            
                        }}
                        localization={{
                            header: {
                                actions: 'Acciones',
                            },
                            toolbar: {
                                searchPlaceholder: 'Buscar',
                            },
                            pagination: {
                                labelRowsSelect: 'filas',
                                labelDisplayedRows: '{from}-{to} de {count} filas',
                                labelRowsPerPage: 'Filas por página:',
                            },
                            body: {
                                emptyDataSourceMessage: 'No hay registros para mostrar', // Aquí cambias el mensaje
                            },
                        }}
                    />
                </Grid>
            </Grid>
            <ModalCourtAdd
                open={openModalEdit}
                formulario={formulario}
                allDepartaments={allDepartaments}
                onClose={handleCloseModalEdit}
                handleAdd={handleAdd}
                handleEdit={handleEdit}
                allTipoActor={allTipoActor}
                estado={estado}
                informacion={informacio}
            />
        </>
    );
};
interface LinkMapStateToProps {
    courtsState: CourtsState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    courtsState: state.courts,
});
interface LinkMapDispatchToProps {
    informacion: JuzgadoDto[];
    startPostUpdateCourt: (juzgado: JuzgadoDto) => void;
    startDeleteCourt: (juzgado: JuzgadoDto) => void;
    startPostInsertCourt: (juzgado: JuzgadoDto) => void;
    obtenerCiudades: (idDepartamento: number) => void;
    obtenerDespachosJudiciales: (valoresBusqueda: DespachosJudiciales) => void;
    formularioDespachosJudiciales: UseFormReturn<DespachosJudiciales>;
    allTipoActor: any;
    estadoJuzgado: any;
}

const mapDispatchToProps = {
    startPostUpdateCourt,
    startDeleteCourt,
    startPostInsertCourt,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(TableCourt);
function fechaInicio(idModulo: number, USR_ID: any, fechaInicio: any) {
    throw new Error('Function not implemented.');
}