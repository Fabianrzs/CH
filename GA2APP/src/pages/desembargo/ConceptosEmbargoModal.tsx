import React, { useEffect, useMemo, useState } from 'react';
import {
    Box,
    Button,
    createStyles,
    Fade,
    Grid,
    makeStyles,
    Modal,
    TableCell,
    TableRow,
    Typography,
} from '@material-ui/core';
import UtilService from '../../utils/util.service';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import ApiEmbargos from '../../services/api-embargos.service';
import ApiSolicitud from '../../services/api-solicitud.service';

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
            margin: '8px 8px',
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
            justifyContent: 'center',
        },
        espaciado: {
            paddingInline: '70px',
            paddingBottom: '10px',
        },
        encabezados: {
            textAlign: 'center',
            position: 'sticky',
            top: 0,
            background: '#FEC917',
            zIndex: 1,
        },
    }),
);

const ConceptosEmbargoModal = ({
    open,
    handleClose,
    conceptos,
    data,
    embargo,
    tipoDocumentoId,
    numeroDocumento,
    onSelectedConceptoChange,
    onProcesarTotal,
    cuentas,
    estadoEmbargo,
    datoEstado
}: {
    open: boolean;
    handleClose: () => void;
    conceptos: any;
    data:any;
    embargo: any;
    onSelectedConceptoChange: (selectedConcepto: any) => void;
    onProcesarTotal: (total: number) => void;
    cuentas: any;
    estadoEmbargo: any;
    tipoDocumentoId: any;
    numeroDocumento: any;
    datoEstado: any;
}) => {
    const classes = useStyles();
    const porcentaje = 'ae7d141a-fd88-4f57-a199-4db5b5301946';
    const variacionPorcentaje = 'f65c4c4a-174e-4406-8aed-32b5b47cf0b3';
    const [selectedRows, setSelectedRows] = useState<any>([]);
    const [selectedConcepto, setSelectedConcepto] = useState<any>([]);
    const [wasProcessed, setWasProcessed] = useState(false);

    const handleProcesarClick = async () => {
        onSelectedConceptoChange(selectedConcepto);
    };


    const handleCheckboxChange = (id: any) => {
        const matchingConcepto = embargo?.[0]?.conceptos.find(
            (option: any) => option.conceptoId === id
        );

        if (!matchingConcepto) return;

        let updatedRows: any[] = [];
        let updatedConceptos: any[] = [];

        if (selectedRows.includes(id)) {
            updatedRows = selectedRows.filter((selectedId: any) => selectedId !== id);
            updatedConceptos = selectedConcepto.filter(
                (selectedId: any) => selectedId !== matchingConcepto.id
            );
        } else {
            updatedRows = [...selectedRows, id];
            updatedConceptos = selectedConcepto.includes(matchingConcepto.id)
                ? selectedConcepto
                : [...selectedConcepto, matchingConcepto.id];
        }

        setSelectedRows(updatedRows);
        setSelectedConcepto(updatedConceptos);
    };




    const handleSelectAll = () => {
        const validConceptos = conceptos.filter((concepto: any) =>
            embargo[0]?.conceptos.some(
                (option: any) => option.conceptoId === concepto.id
            )
        );

        if (areAllSelected) {
            setSelectedRows([]);
            setSelectedConcepto([]);
        } else {
            setSelectedRows(validConceptos.map((concepto: any) => concepto.id));
            setSelectedConcepto(
                validConceptos.map(
                    (concepto: any) =>
                        embargo[0]?.conceptos.find(
                            (option: any) => option.conceptoId === concepto.id
                        ).id
                )
            );
        }
    };


    const areAllSelected =
        Array.isArray(conceptos) &&
        conceptos.length > 0 &&
        conceptos.filter((concepto: any) =>
            embargo[0]?.conceptos.some(
                (option: any) => option.conceptoId === concepto.id
            )
        ).length > 0 &&
        conceptos.filter((concepto: any) =>
            embargo[0]?.conceptos.some(
                (option: any) => option.conceptoId === concepto.id
            )
        ).every((concepto: any) => selectedRows.includes(concepto.id));




    const handleSelectAllChange = () => {
        handleSelectAll();
    };

    const handleReturn = () => {
        handleClose();
    };

    function renderValor(valor: any) {
        return `${valor}%`;
    }


    const totalConceptosDesembargados = useMemo(() => {
        if (!embargo || embargo.length === 0 || !embargo[0]?.conceptos) return 0;

        return cuentas?.flatMap((cuenta: any) => cuenta.conceptos || [])
            .filter((concepto: any) => {
                if (datoEstado === "ANULADA") {
                    return concepto.valor > 0;
                }

                return selectedRows.includes(concepto.id);
            })
            .reduce((total: number, concepto: any) => {
                const sumaConcepto = embargo[0]?.conceptos
                    .filter((option: any) => option.conceptoId === concepto.id)
                    .reduce((acc: number, option: any) => {
                        const valorCalculado =
                            option.tipoEmbargoId === porcentaje || option.tipoEmbargoId === variacionPorcentaje
                                ? (concepto.saldo * option.valor) / 100
                                : option.valor || 0;
                        return acc + valorCalculado;
                    }, 0);

                return total + sumaConcepto;
            }, 0);
    }, [embargo, cuentas, selectedRows, porcentaje, variacionPorcentaje, datoEstado]);


    useEffect(() => {
        if (!open && !wasProcessed) {
            setSelectedRows([]);
            setSelectedConcepto([]);
            setWasProcessed(false);
        }
    }, [open, wasProcessed]);

    useEffect(() => {
        if (datoEstado === "PROCESADA" && Array.isArray(embargo) && embargo.length > 0) {
            const conceptosMarcados = embargo[0].conceptos
                ?.filter((option: any) => option.valor > 0)
                .map((option: any) => option.conceptoId) || [];

            setSelectedRows(conceptosMarcados);
            setSelectedConcepto(conceptosMarcados);

            onSelectedConceptoChange(conceptosMarcados);

            const total = embargo[0].conceptos
                ?.filter((option: any) => conceptosMarcados.includes(option.conceptoId))
                .reduce((acc: number, curr: any) => acc + curr.valor, 0) || 0;

            onProcesarTotal(total);
        }
    }, [datoEstado, embargo]);




    return (
        <>
            <Modal className={classes.modal} open={open} onClose={handleClose}>
                <Fade in={open}>
                    <>
                        <div className={classes.paper}>
                            <Box mb={0.2}>
                                <Typography variant="h6" align="center" className={classes.title}>
                                    ANULAR EMBARGO
                                </Typography>
                            </Box>
                            <br />
                            <Grid container alignItems="center" justify="center">
                                <Grid item xs={12}>
                                    <div style={{ maxHeight: '70vh', overflow: 'auto', width: '100%' }}>
                                        <table className="client-table" style={{ width: '100%' }}>
                                            <thead>
                                                <tr>
                                                    <th className={classes.encabezados}>
                                                        <CHCheckbox
                                                            color="primary"
                                                            disabled={false}
                                                            label=""
                                                            checked={areAllSelected}
                                                            onChange={handleSelectAllChange}
                                                        />
                                                    </th>
                                                    <th className={classes.encabezados}>Concepto</th>
                                                    <th className={classes.encabezados}>Saldo</th>
                                                    <th className={classes.encabezados}>Tipo Embargo</th>
                                                    <th className={classes.encabezados}>Valor Retenido</th>
                                                    <th className={classes.encabezados}>Porcentaje</th>
                                                    <th className={classes.encabezados}>Fecha Inicio</th>
                                                    <th className={classes.encabezados}>Fecha Fin</th>
                                                    <th className={classes.encabezados}>Tipo Retención</th>
                                                    <th className={classes.encabezados}>Indicador de cesantías</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {cuentas?.map((cuenta: any) => (
                                                    cuenta.conceptos?.length > 0
                                                        ? cuenta.conceptos?.map((concepto: any) => (
                                                            <tr key={concepto.id}>
                                                                <td>
                                                                    <CHCheckbox
                                                                        color="primary"
                                                                        label=""
                                                                        checked={
                                                                            datoEstado === "ANULADA"
                                                                                ? embargo[0]?.conceptos.some(
                                                                                    (option: any) =>
                                                                                        option.conceptoId === concepto.id && option.valor > 0
                                                                                )
                                                                                : selectedRows.includes(concepto.id)
                                                                        }
                                                                        disabled={
                                                                            datoEstado === "ANULADA" &&
                                                                            embargo[0]?.conceptos.some(
                                                                                (option: any) =>
                                                                                    option.conceptoId === concepto.id && option.valor > 0
                                                                            )
                                                                        }
                                                                        onChange={() => handleCheckboxChange(concepto.id)}
                                                                    />
                                                                </td>




                                                                <td>{concepto.conceptoDescripcion}</td>
                                                                <td>{UtilService.formatCurrency(concepto.saldo)}</td>
                                                                <td>
                                                                    {embargo[0]?.conceptos.map((option: any) =>
                                                                        (option.estado === true && embargo[0]?.embargo.estado === true) && option.conceptoId === concepto.id ? <span>{option.tipoEmbargo}</span> : null
                                                                    )}
                                                                </td>
                                                                <td>
                                                                    {embargo[0]?.conceptos.map((option: any) => {
                                                                        if (option.conceptoId === concepto.id) {
                                                                            const valorCalculado =
                                                                                option.tipoEmbargoId === porcentaje || option.tipoEmbargoId === variacionPorcentaje
                                                                                    ? (concepto.saldo * option.valor) / 100
                                                                                    : option.valor || 0;


                                                                            return <span>{UtilService.formatCurrency(valorCalculado)}</span>;
                                                                        }
                                                                        return null;
                                                                    })}
                                                                </td>
                                                                <td>
                                                                    {embargo[0]?.conceptos.map((option: any) => {
                                                                        if (option.conceptoId === concepto.id) {
                                                                            if ((option.estado === true && embargo[0]?.embargo.estado === true) && option.tipoEmbargoId === porcentaje || option.tipoEmbargoId === variacionPorcentaje) {
                                                                                return renderValor(option.valor);
                                                                            }
                                                                            return null;
                                                                        }
                                                                        return null;
                                                                    })}
                                                                </td>
                                                                <td>
                                                                    {embargo[0]?.conceptos.map((option: any) => {
                                                                        if ((option.estado === true && embargo[0]?.embargo.estado === true) && option.conceptoId === concepto.id) {
                                                                            const fecha = new Date(option.fechaInicio);
                                                                            const mes = (fecha.getMonth() + 1).toString().padStart(2, '0');
                                                                            const dia = fecha.getDate();
                                                                            const año = fecha.getFullYear();
                                                                            return <span>{`${dia}-${mes}-${año}`}</span>;
                                                                        }
                                                                        return null;
                                                                    })}
                                                                </td>
                                                                <td>-</td>
                                                                <td>
                                                                    {embargo[0]?.conceptos.map((option: any) =>
                                                                        option.estado === true && option.conceptoId === concepto.id ? <span>{option.tipoRetencion}</span> : null
                                                                    )}
                                                                </td>
                                                                <td>{concepto.condicionadoPorRetiro ? 'SI' : 'NO'}</td>
                                                            </tr>
                                                        ))
                                                        : null
                                                ))}
                                            </tbody>
                                        </table>
                                    </div>
                                    <tbody>
                                        <TableRow>
                                            <TableCell colSpan={2}>
                                                <b>TOTAL CONCEPTOS DESEMBARGADOS</b>
                                            </TableCell>
                                            <TableCell colSpan={1} style={{ textAlign: "center", fontSize: "18px" }}>
                                                <b>{UtilService.formatCurrency(totalConceptosDesembargados) || "0"}</b>
                                            </TableCell>

                                        </TableRow>
                                    </tbody>
                                </Grid>
                            </Grid>
                            <div className={classes.alineationHorizontal}>
                                {estadoEmbargo === "True" && (
                                    <Button
                                        className={classes.processButton}
                                        onClick={() => {
                                            const conceptosSeleccionados = embargo?.[0]?.conceptos.filter((option: any) =>
                                                selectedRows.includes(option.conceptoId)
                                            );

                                            onProcesarTotal(totalConceptosDesembargados);
                                            onSelectedConceptoChange(conceptosSeleccionados);

                                            setWasProcessed(true);
                                            handleClose();
                                        }}
                                        disabled={(selectedRows?.length ?? 0) === 0}
                                    >
                                        Procesar
                                    </Button>
                                )}
                                <Button
                                    className={classes.processButton}
                                    onClick={handleReturn}

                                >
                                    Regresar
                                </Button>
                            </div>
                        </div>


                    </>
                </Fade>

            </Modal>
        </>
    );
};

export default ConceptosEmbargoModal;
