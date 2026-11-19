import React, { useEffect, useState } from 'react';
import {
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    Button,
    Modal,
    Fade,
    Backdrop,
    Checkbox,
    Dialog,
    Box,
    TableContainer,
    Typography,
    Table,
    TableBody,
    TableRow,
    TableCell,
    IconButton,
    TableHead,
} from '@material-ui/core';
import { useHistory } from 'react-router-dom';
import { connect } from 'react-redux';
import { Controller, useForm, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import { Edit } from '@material-ui/icons';
import { AppState } from '../../store/rootReducer';
import history from '../../utils/history.router';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../components/ch-select/CHSelect';
import { startGetAllIdentificationTypes } from '../../actions/catalog.action';
import { IDENTIFICATIONTYPEDTO, 
    CITYDTO,
    DEPARTAMENTODTO, 
} from '../../types/catalog.type';
import { JuzgadoDto } from '../../types/court.model.type';

import CHTextField from '../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../reducers/catalog.reducer';
import { LoginState } from '../../reducers/login.reducer';
import {
    BusquedaEmbargos,
    ConceptoEmbargosDto,
    EmbargoDto,
    EmbargoModelDto,
    BeneficiarioDto,
    TipoEmbargoDto,
    CuentaModelDto,
    TipoProcesoEmbargoDto,
    TipoRetencionDto,
    ConceptoModelDto,
} from '../../types/embargo.model.type';
import { busquedaDesembargoForm, busquedaEmbargosForm } from '../embargos/constants/embargosform.model.type';
import UtilService from '../../utils/util.service';
import ApiEmbargos from '../../services/api-embargos.service';
import Cuentas from '../embargos/cuentasList/Cuentas';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import { convertToConceptos } from '../embargos/constants/conversionesConceptosdeCuentas';



const useStyles = makeStyles((theme) =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        alineationButtons: {
            display: 'flex',
            justifyContent: 'center',
            marginTop: theme.spacing(2),

        },
        modal: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            backgroundColor: 'rgba(0, 0, 0, 0.5)',
            zIndex: 9999,
            '& .MuiPaper-root': {
                margin: 'auto',
                outline: 'none',
                borderRadius: 8,
                width: '50%',
                height: '50%',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                [theme.breakpoints.down('sm')]: {
                    width: '80%',
                    height: '80%',
                },
            },
        },
        paper: {
            backgroundColor: theme.palette.background.paper,
            border: '2px solid #000',
            boxShadow: theme.shadows[5],
            padding: theme.spacing(5, 3, 2),
            width: '70%',
            height: '90%',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            borderRadius: '3%',
        },
        header: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            marginBottom: '1rem',
        },
        title: {
            color: '#1C3F8B',
            textAlign: 'center',
            fontSize: '0.9rem',
            fontWeight: 'bold',
        },
        actions: {
            marginLeft: '2rem',
            width: '20%',
        },
        customCheckbox: {
            color: 'gray',
            '&$checked': {
                color: 'gray',
            },
        },
        alineationHorizontal: {
            display: 'flex',
            justifyContent:"center"
        },
        espaciado: {
            paddingInline: '70px',
            paddingBottom: '10px'
        }
    }),
);

    
const AnularEmbargo = (props: props) => {
    
    const {
        cuentas,
        conceptos,
        numeroCuenta,
        tiposEmbargos,
        tiposRetencion,
        handleOpenModal,
        listaConceptos,
        conceptosData,
        modoActualizar,
        setListaConceptos,
        porcentaje,
        variacionPorcentaje,
        handleOnChangeMostrarPorcentaje,
        handleTotalEmbargos,
        saldos
    } = props;

    const [selectedConceptos, setSelectedConceptos] = useState<any[]>([]);
    const [selectedAll, setSelectedAll] = useState(false);
    
    const classes = useStyles();
    const [isOpen, setIsOpen] = useState<boolean>(false);
    const history = useHistory();
    const [open, setOpen] = useState(false);
    const [isChecked, setIsChecked] = useState(false);
  
    const handleOpen = () => {
      setOpen(true);
    };

    const nuevaListaConceptos = async () => {
        const array = await conceptos.map(item =>
            convertToConceptos(
                item.cuentaId,
                item.id,
                item.conceptoDescripcion,
                item.saldo,
                numeroCuenta,
            ),
        );

        if (modoActualizar) {
            conceptosData.forEach(value => {
                const index = array.findIndex(
                    x => x.conceptoId === value.conceptoId,
                );
                if (index !== -1) {
                    value.saldo = array[index].saldo;
                    value.conceptoDescripcion =
                        array[index].conceptoDescripcion;
                    array[index] = value;
                    array[index].insert = false;
                }
            });
        }
        await setListaConceptos(array);
    };
  
   
    const handleCheck = (event: React.ChangeEvent<HTMLInputElement>) => {
        setIsChecked(event.target.checked);
    }

    const handleOnClickRegresar = () => {
        history.push('/InformacionDesembargo')
    }

    useEffect(() => {
        setOpen(true);
    }, []);

    const handleCheckboxChange = (conceptoId: any) => {
        let updatedConceptos = [...selectedConceptos];

        if (updatedConceptos.includes(conceptoId)) {
            updatedConceptos = updatedConceptos.filter((selectedConceptoId) => selectedConceptoId !== conceptoId);
        } else {
            updatedConceptos.push(conceptoId);
        }
        setSelectedConceptos(updatedConceptos);
    };


    const formularioBusquedaDesembargos = useForm({
        defaultValues: busquedaEmbargosForm,
    });

    const valor = '95ca1858-b89d-4631-9893-22932c925184';

    const valorAplicandoPorcentaje = (item: ConceptoEmbargosDto) => {
        let porcentajeValor = 0;
        if (
            item.tipoEmbargoId === porcentaje ||
            item.tipoEmbargoId === variacionPorcentaje
        ) {
            porcentajeValor = item.saldo * ((item.valor || 0) / 100);
            
        }
        return porcentajeValor;
    };

    const handleSelectAll = (event: any) => {
        const { checked } = event.target;
        setSelectedAll(checked);
        setSelectedConceptos(checked ? listaConceptos.map(concepto => concepto.id) : []);
    };


    return (
        <>
            <Modal
                className={classes.modal}
                open={open}
            >
    <Fade in={open}>
        <div className={classes.paper}>
            <Box mb={0.2}>
                <Typography variant="h6" align="center" className={classes.title}>ANULAR EMBARGO</Typography>
            </Box>
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    <TableContainer>
                        <Table className="credit-table" style={{ marginBottom: '15rem'}}>
                        <TableHead>
                                <TableRow>
                                    <TableCell style={{ textAlign: 'center' }}>
                                    Seleccionar todos
                                    <CHCheckbox
                                        color='primary'
                                        disabled={false}
                                        checked={selectedAll}
                                        indeterminate={selectedConceptos.length > 0 && selectedConceptos.length < selectedConceptos.length}
                                        onChange={handleSelectAll} 
                                        label= ''                                   
                                    />
                                    </TableCell>
                                    <TableCell style={{ textAlign: 'center' }}>
                                        Concepto
                                    </TableCell>
                                    <TableCell style={{ textAlign: 'center' }}>Saldo</TableCell>
                                    <TableCell style={{ textAlign: 'center' }}>
                                        Tipo Embargo
                                    </TableCell>
                                    <TableCell style={{ textAlign: 'center' }}>
                                        Valor Embargo
                                    </TableCell>
                                    <TableCell style={{ textAlign: 'center' }}>
                                        Valor % Aplicado
                                    </TableCell>
                                    <TableCell style={{ textAlign: 'center' }}>
                                        Fecha Inicio
                                    </TableCell>
                                    <TableCell style={{ textAlign: 'center' }}>
                                        Fecha Fin
                                    </TableCell>
                                    <TableCell style={{ textAlign: 'center' }}>
                                        Tipo Retención
                                    </TableCell>
                                    <TableCell style={{ textAlign: 'center' }}>
                                        Indicador Cesantías
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {listaConceptos?.map((concepto) => (
                                    <TableRow key={concepto.id}>
                                        <TableCell>
                                            <CHCheckbox
                                                color='primary'
                                                disabled= {false}
                                                label=''
                                                checked={selectedConceptos.includes(concepto.id)}
                                                onChange={() => handleCheckboxChange(concepto.id)}
                                            />
                                        </TableCell>
                                        <TableCell>{concepto.conceptoDescripcion}</TableCell>
                                        <TableCell>
                                        {concepto.tipoEmbargoId !== porcentaje &&
                                            concepto.tipoEmbargoId !== variacionPorcentaje
                                                ? UtilService.formatCurrency(concepto.valor || 0)
                                                : UtilService.formatCurrency(valorAplicandoPorcentaje(concepto))}   
                                        </TableCell>    
                                        <TableCell>
                                            {concepto.tipoRetencionId}
                                        </TableCell>
                                        <TableCell>
                                            {concepto.tipoEmbargoId !== porcentaje &&
                                            concepto.tipoEmbargoId !== variacionPorcentaje
                                                ? UtilService.formatCurrency(concepto.valor || 0)
                                                : UtilService.formatCurrency(valorAplicandoPorcentaje(concepto))}
                                        </TableCell>
                                        <TableCell>
                                            {concepto.tipoEmbargoId === porcentaje ||
                                                concepto.tipoEmbargoId === variacionPorcentaje
                                                    ? `${concepto.valor}%`
                                                    : ''}
                                        </TableCell>
                                        <TableCell>
                                            {UtilService.getDate(concepto.fechaInicio)}
                                        </TableCell>
                                        <TableCell>
                                            {UtilService.getDate(concepto.fechaFin)}
                                        </TableCell>
                                        <TableCell>
                                            {concepto.indicadorCesantias ? 'SI' : 'NO'}
                                        </TableCell>
                                        <TableCell>
                                            <IconButton
                                                aria-label="Editar"
                                                color="primary"
                                             
                                            >
                                                <Edit />
                                            </IconButton>
                                        </TableCell>
                                    </TableRow>
                                ))}
                                <TableRow>
                                    <TableCell colSpan={3}>
                                        <b>TOTAL CONCEPTOS DESEMBARGADOS</b>
                                    </TableCell>
                                    <TableCell colSpan={1} style={{ textAlign: 'center' }}>
                                        {/* <b>{UtilService.formatCurrency(valorTotal())}</b> */}
                                    </TableCell>
                                </TableRow>
                                    
                            </TableBody>
                        </Table>
                        <Grid className={classes.alineationHorizontal}>
                            <tbody>
                                <td className={classes.espaciado}>
                                    <b>TOTAL CONCEPTOS DESEMBARGADOS</b>
                                </td>
                                <td className={classes.espaciado}>
                                    {/* <b>{UtilService.formatCurrency(valorTotal())}</b> */}
                                </td>
                            </tbody>
                        </Grid>
                    </TableContainer>
                </Grid>
                <Grid item xs={12}>
                    <Box display="flex" justifyContent="center">
                        <Button 
                            className={classes.actions}
                            variant="contained"
                            color='primary'
                        >
                            ANULAR
                        </Button>     
                        <Button 
                            className={classes.actions}
                            variant="contained"
                            color='default'
                            onClick={handleOnClickRegresar}
                        >
                            CANCELAR
                        </Button>
                    </Box>
                </Grid>
            </Grid>
        </div>
    </Fade>
            </Modal>
        </>
    )
};

interface LinkMapDispatchToProps {
    cuentas: CuentaModelDto;
    conceptos: ConceptoModelDto[];
    numeroCuenta: number;
    handleOpenModal: (conceptoId: number) => void;
    listaConceptos: ConceptoEmbargosDto[];
    setListaConceptos: (listaConceptos: ConceptoEmbargosDto[]) => void;
    tiposEmbargos: TipoEmbargoDto[];
    tiposRetencion: TipoRetencionDto[];
    conceptosData: ConceptoEmbargosDto[];
    modoActualizar: boolean;
    porcentaje: string;
    variacionPorcentaje: string;
    handleOnChangeMostrarPorcentaje: (value: boolean) => void;
    handleTotalEmbargos: (totalEmbargos: any) => void;
    saldos: number
}

type props = LinkMapDispatchToProps;


export default AnularEmbargo;