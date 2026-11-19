import React, {useEffect, useState}  from 'react';
import { Box, Collapse, IconButton, makeStyles, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@material-ui/core';
import KeyboardArrowDownIcon from '@material-ui/icons/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import { toast } from 'react-toastify';
import { groupBy } from 'lodash';
import { Visibility } from '@material-ui/icons';
import Delete from '@material-ui/icons/Delete';
import CHPaper from "../../../../components/ch-paper/CHPaper";
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import SolicitudApi from '../../../../services/api-solicitud.service';
import { DocumentosPorSolicitudModel } from '../../../../models/documentos-por-solicitud.model';
import NotificationService from '../../../../services/notification.service';
import { DocumentosEliminarModel } from '../../../../models/requests/documento-eliminar.model';
import UtilService from '../../../../utils/util.service';



let rows: any[] = [];
const api = SolicitudApi();


const useRowStyles = makeStyles({
    root: {
      '& > *': {
        borderBottom: 'unset',
      },
    },
  });

  function createData(
    name: string,
    total: number,
    history: DocumentosPorSolicitudModel[]
  ) {

    return {
      name,
      total,
      history,
    };
  }



function Row(
    {
        row, 
        registrosActualizados,
        siEsAprobadaSolcitud,

    }: {
        row: ReturnType<typeof createData>, 
        registrosActualizados: (rows: any[]) => void;
        siEsAprobadaSolcitud:boolean;
    
    }) {

    const [open, setOpen] = React.useState(false);
    const classes = useRowStyles();

  

    const eliminarRegistro = async (data: DocumentosPorSolicitudModel, registro: any) => {
        const confirmAlert = await NotificationService.confirmAlert(
            `¿Eliminando registro ${data.nombreArchivo}?`,
        );

        if (confirmAlert) {

            const modelEliminarDocumento: DocumentosEliminarModel = {
                idArchivo: data.idArchivo,
                rutaStorage: data.rutaStorage
            };
            await api.PostEliminarDocumento(modelEliminarDocumento)
            .then(async ()  => {
                
                setOpen(false);
                toast(`Archivo ${data.nombreArchivo}, eliminado correctamente`,  { type: toast.TYPE.SUCCESS});
                const temp = [...registro.history];
                temp.splice(temp.findIndex(x => x.idArchivo === data.idArchivo), 1);
                rows.find(x => x.name === registro.name).history = temp;
                registrosActualizados(rows);
                setOpen(true);
            })
            .catch(() => {
                toast("Error del sistema",  { type: toast.TYPE.ERROR});
            });
        }
    }

    const descargarArchivo = async (data: DocumentosPorSolicitudModel) => {
         const archivo = await api.GetDescargarArchivoSolicitud(data.rutaStorage);
        const ext = UtilService.getFileExtension(data.extension);
        if (ext) {
            const mimeType = UtilService.getMimeTypeFromExtension(ext);
            if (mimeType) {
                const file = new Blob([archivo], { type: mimeType });
                const fileURL = URL.createObjectURL(file);
                window.open(fileURL);
            }
        }
    }

    return (
        <>
        {row.history.length > 0 ?
            <React.Fragment key={`fracmento_${row.name}`}>
                <TableRow className={classes.root}>
                <TableCell>
                    <IconButton aria-label="expand row" size="small" onClick={() => setOpen(!open)}>
                    {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
                    </IconButton>
                </TableCell>
                <TableCell component="th" scope="row">
                    {row.name}
                </TableCell>
                <TableCell component="th" scope="row">
                    {row.history.length}
                </TableCell>
                </TableRow>
                <TableRow>
                <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={6}>
                    <Collapse in={open} timeout="auto" unmountOnExit>
                    <Box margin={1}>
                        <Typography variant="h6" gutterBottom component="div">
                        Archivos
                        </Typography>
                        <br />
                        <Table size="small" aria-label="purchases">
                        <TableHead>
                            <TableRow>
                            <TableCell>Nombre Archivo</TableCell>
                            <TableCell>Extensión</TableCell>
                            <TableCell align="center">Acciones</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {row.history.map((historyRow) => (
                            <TableRow key={historyRow.idArchivo}>
                               
                                <TableCell component="th" scope="row">
                                {historyRow.nombreArchivo}
                               
                                </TableCell>
                                <TableCell>{historyRow.extension}</TableCell>
                                <TableCell align="center">
                                    <IconButton aria-label="Editares" color="primary" onClick={() => {
                                        descargarArchivo(historyRow);
                                    }}>
                                        <Visibility />
                                    </IconButton>
                                    <IconButton aria-label="Eliminar" color="secondary" disabled={!siEsAprobadaSolcitud} onClick={() => {
                                            eliminarRegistro(historyRow, row);
                                    }} >
                                    {/* <IconButton aria-label="Eliminar" color="secondary" disabled={historyRow.trA_ORDEN!==2  && historyRow.trA_ORDEN ===4} onClick={() => {
                                            eliminarRegistro(historyRow, row);
                                    }} > */}
                                        <Delete />
                                    </IconButton> 
                                </TableCell>
                            </TableRow>
                            ))}
                        </TableBody>
                        </Table>
                        <br />
                    </Box>
                    </Collapse>
                </TableCell>
                </TableRow>
            </React.Fragment>
            :
            null
            }
        </>
    );
  }





function VerDocumentosSolicitud({
    responseSolicitud,
    siEsAprobadaSolcitud
}:{
    responseSolicitud : ResponseSeveranceModel | undefined;
    siEsAprobadaSolcitud: boolean;
}) {
    const [listaDocumentos, setListadocumentos] = useState<any[]>([]);

    const registrosActualizados = (registros: any[]) => {
        setListadocumentos(registros);
    }

    const consultarDocumentosCargados = async () => {
        rows = [];
        await api.GetConsultarDocumentosPorSolicitud(responseSolicitud?.solicitudId as string)
            .then(async (respuesta)  => {
                const datos =  groupBy(Array.from(respuesta.data), 'tipoArchivo');
                Object.entries(datos).forEach((x) => {
                    rows.push(createData(x[0], x[1].length, x[1] as DocumentosPorSolicitudModel[]));
                });
                setListadocumentos(rows);
            })
            .catch(() => {
                toast("Error del sistema",  { type: toast.TYPE.ERROR});
            });
    }

    useEffect(() => {
        consultarDocumentosCargados();
    }, []);

     return (
        <>
            <CHPaper title="Ver documentos cargados">

                <TableContainer component={Paper}>
                    <Table aria-label="collapsible table">
                        <TableHead>
                        <TableRow>
                            <TableCell />
                            <TableCell align="left">Tipo De Archivo </TableCell>
                            <TableCell align="left">Número De Archivos</TableCell>
                        </TableRow>
                        </TableHead>
                        <TableBody>

                        {listaDocumentos.length > 0 && listaDocumentos.map((row) => (
                            <Row
                             key={row.name} row={row}
                             registrosActualizados={registrosActualizados}
                             siEsAprobadaSolcitud= {siEsAprobadaSolcitud} />
                        ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </CHPaper>
        </>
    )
}


export default VerDocumentosSolicitud;
