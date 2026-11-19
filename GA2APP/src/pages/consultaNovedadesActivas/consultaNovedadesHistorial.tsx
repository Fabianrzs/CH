import React, { useEffect, useState } from 'react';
import {
    Button,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper,
    Checkbox,
    makeStyles,
    createStyles,
    Grid,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';
import history from '../../utils/history.router';
import ContainerTitle from '../../components/containe-title/container-title';
import UtilService from '../../utils/util.service';
import ApiSchedule from '../../services/api-cronograma.service';
import CHTextField from '../../components/ch-text-field/CHTextField';
import NotificationService from '../../services/notification.service';
import AuthService from '../../services/auth.service';
import CatalogApi from '../../services/api-catalog.service';



const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {

            width: 'fit-content',
            marginLeft: '0',
            marginTop: '1.5rem',
            backgroundColor: 'transparent',
            color: '#1C3F8B',
            border: 'none',
            cursor: 'pointer',
        },
        tableContainer: {
            marginTop: '1rem',
        },
        backButton: {
            position: 'fixed',
            bottom: '1rem',
            left: '6rem',
        },
    })
);



const consultaNovedadesHistorial = (props: any) => {

    const classes = useStyles();
    const api = ApiSchedule();
    const user = AuthService.getUserInfo();
    const otroFormato = true;


    const location = useLocation<{ nameUnidadEjecutora: any, archivoName: any, periodoNovedad: any, estadoArchivoNov: any, idCodigoUsuario: any, nombreConceptoNovedad: any, idUnidadEjecutora: any, idConceptoNovedad: any, fechaInicio: any, fechaFin: any, archivoId: any, codigoNovedad: any }>();
    const { nameUnidadEjecutora, archivoName, periodoNovedad, idCodigoUsuario, nombreConceptoNovedad, idUnidadEjecutora, idConceptoNovedad, fechaInicio, fechaFin, estadoArchivoNov } = location.state;

    interface Archivo {
        periodoNovedad: any;
        archivoName: string;
        estado: string;
        nameUnidadEjecutora: string;
        fechaFormateada: any;
    }

    const fechaActual = new Date();
    const [checkBoxMarcado, setCheckBoxMarcado] = useState(false);
    const [archivos, setArchivos] = useState<Archivo[]>([]);
    const [idUser, setIdUser] = useState();
    const [row, setRow] = useState<any>(null);
    const [index, setIndex] = useState<number>(-1);
    const [siglas, setSiglas] = useState();
    const [autorizado, setAutorizado] = useState(false);
    const [archivoId, setArchivoId] = useState<number>();
    const [estadoNovedad, setEstadoNovedad] = useState();

    const fechaFormateada = `${fechaActual.getDate().toString().padStart(2, '0')}/${(fechaActual.getMonth() + 1).toString().padStart(2, '0')}/${fechaActual.getFullYear()}`;

    const handleReturnConsulta = () => {
        history.push('/Consulta')
    };

    const getArchivosDescargaDefinitiva = async () => {

        try {
            const response = await api.ObtenerArchivosDescargaDefinitiva({
                UnidadEjecutoraId: idUnidadEjecutora,
                ConceptoNovedadId: idConceptoNovedad,
                DescargaArchivoId: archivoId,
                NombreArchivo: archivoName,
                NombreUnidadEjecutora: nameUnidadEjecutora,
                NombreConceptoNovedad: nombreConceptoNovedad,
                Periodo: periodoNovedad,
                Estado: estadoNovedad,
                Usuario: idUser === undefined ? user.USR_ID : idUser,
                SiglasUnidadEjecutora: siglas,
                Fecha: fechaActual,
            });

            return response;
        } catch (error) {
            console.error('Error al obtener datos de la API:', error);
            return null;
        }
    };

    useEffect(() => {
        getArchivosDescargaDefinitiva()

    }, []);


    return (
        <Grid container>
            <Grid>
                <ContainerTitle title="Consulta de Novedades a Unidades Ejecutoras" />
            </Grid>
            <table className="table">
                <thead>
                    <tr>
                        <th style={{ textAlign: 'center' }}>Archivo</th>
                        <th style={{ textAlign: 'center' }}>Periodo</th>
                        <th style={{ textAlign: 'center' }}>Unidad Ejecutora</th>
                        <th style={{ textAlign: 'center' }}>Fecha</th>
                        <th style={{ textAlign: 'center' }}>Estado</th>
                    </tr>
                </thead>
                <tbody>

                    <tr key={index}>
                        <td>{archivoName}</td>
                        <td>{periodoNovedad}</td>
                        <td>{nameUnidadEjecutora}</td>
                        <td>{fechaFormateada}</td>
                        <td>{estadoNovedad}</td>
                    </tr>

                </tbody>

            </table>

            <Grid className={classes.backButton}>
                <Button className={classes.button} onClick={handleReturnConsulta}>
                    REGRESAR
                </Button>
            </Grid>
        </Grid>
    );
};

export default consultaNovedadesHistorial;


