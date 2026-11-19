import React from 'react';
import { makeStyles, createStyles, Grid, Button } from '@material-ui/core';
import MaterialTable from 'material-table';
import CHPaper from '../../components/ch-paper/CHPaper';
import UtilService from '../../utils/util.service';
import { ModalEditarEmbargosaConceptos } from './components/ModalEditarEmbargoaConcepto';
import {
    EmbargoConceptoDto,
    TipoEmbargoDto,
    TipoRetencionDto,
} from '../../types/embargo.model.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '90%',
            marginLeft: '.5rem',
            marginRight: '150px',
            marginTop: '1.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

const DetalleEmbargosConcepto = (props: props) => {
    const classes = useStyles();
    const {
        openModalEditar,
        tiposEmbargos,
        tiposRetencion,
        control,
        porcentaje,
        valor,
        mostrarPorcentaje,
        errorPorcentaje,
        mensaje,
        conceptosEditar,
        handleSubmit,
        onItemGoBack,
        handleOpenModalEditar,
        handleCloseModalEditar,
        setErrorPorcentaje,
        setMensaje,
        handleOnChangeMostrarPorcentaje,
        handleOnChangeModo,
        handleOnClickValidation,
    } = props;

    const columns: any = [
        {
            title: 'Tipo Embargo',
            field: 'tipoembargo',
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
            title: 'Tipo Retención',
            field: 'tipoRetencion',
            align: 'center',
            cellStyle: {
                border: '1px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
        },
        {
            title: 'Valor',
            field: 'valor',
            align: 'center',
            cellStyle: {
                border: '1.5px solid #FFF',
                color: '#676767',
            },
            headerStyle: {
                borderTopRightRadius: '0px',
            },
            render: (rowData: any) =>
                rowData.tipoembargo === 'Valor' ? (
                    UtilService.formatCurrency(rowData.valor)
                ) : (
                    <span>{`${rowData.valor}%`}</span>
                ),
        },
    ];

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
            <CHPaper title="Información Conceptos">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <MaterialTable
                            style={{
                                borderTopLeftRadius: '12px',
                                borderTopRightRadius: '12px',
                                margin: '20px',
                                width: '95%',
                            }}
                            title=""
                            data={conceptosEditar}
                            columns={columns}
                            options={{
                                toolbar: false,
                                search: false,
                                actionsColumnIndex: -1,
                                selection: false,
                                showEmptyDataSourceMessage: false,
                                showTextRowsSelected: false,
                                headerSelectionProps: {
                                    color: 'primary',
                                },
                                selectionProps: {
                                    color: 'primary',
                                },
                                headerStyle: {
                                    backgroundColor: '#FEC917',
                                    fontWeight: 'bold',
                                    color: '#676767',
                                    borderColor: '#FFF',
                                    border: '1.5px solid #FFF',
                                    borderTopRightRadius: '12px',
                                },
                                filterCellStyle: {
                                    width: '50%',
                                },
                                rowStyle: (data, index) => intercalar(index),
                            }}
                            actions={[
                                {
                                    icon: 'edit',
                                    tooltip: 'Editar Concepto',
                                    onClick: (event, rowData: any) => {
                                       
                                        console.log('editar', rowData);
                                        handleOnChangeModo(true);
                                        handleOpenModalEditar(rowData);
                                    },
                                },
                            ]}
                            localization={{
                                header: {
                                    actions: 'Acciones',
                                },
                            }}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={9} />
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="default"
                            onClick={onItemGoBack}
                        >
                            REGRESAR
                        </Button>
                    </Grid>
                </Grid>
                <ModalEditarEmbargosaConceptos
                    open={openModalEditar}
                    handleClose={handleCloseModalEditar}
                    tiposEmbargos={tiposEmbargos}
                    tiposRetencion={tiposRetencion}
                    porcentaje={porcentaje}
                    valor={valor}
                    control={control}
                    handleOnChangeMostrarPorcentaje={
                        handleOnChangeMostrarPorcentaje
                    }
                    mostrarPorcentaje={mostrarPorcentaje}
                    setErrorPorcentaje={setErrorPorcentaje}
                    setMensaje={setMensaje}
                    errorPorcentaje={errorPorcentaje}
                    mensaje={mensaje}
                    handleSubmit={handleSubmit}
                    handleOnClickUpdate={handleOnClickValidation}
                />
            </CHPaper>
        </>
    );
};

interface LinkMapDispatchToProps {
    openModalEditar: boolean;
    tiposEmbargos: TipoEmbargoDto[];
    tiposRetencion: TipoRetencionDto[];
    control: any;
    porcentaje: string;
    valor: string;
    mostrarPorcentaje: boolean;
    errorPorcentaje: boolean;
    mensaje: string;
    conceptosEditar: any[];
    handleSubmit: any;
    onItemGoBack: () => void;
    setErrorPorcentaje: (value: boolean) => void;
    setMensaje: (value: string) => void;
    handleOpenModalEditar: (value: any) => void;
    handleCloseModalEditar: () => void;
    handleOnChangeMostrarPorcentaje: (value: boolean) => void;
    handleOnChangeModo: (value: boolean) => void;
    handleOnClickValidation: (value: EmbargoConceptoDto) => void;
}

type props = LinkMapDispatchToProps;

export default DetalleEmbargosConcepto;
