import { Button, createStyles, Grid, makeStyles } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { connect } from "react-redux";
import CreditApi from "../../services/api-credit.service";
import CHPaper from "../ch-paper/CHPaper";
import CHTextField from "../ch-text-field/CHTextField";
import UtilService from "../../utils/util.service";
import NotificationService from "../../services/notification.service";
import CHLoadingProcess from "../ch-loading-process/CHLoadingProcess";
import ApiReportes from "../../services/api-reportes.service";
import { CatalogState } from "../../reducers/catalog.reducer";
import { AppState } from "../../store/rootReducer";


// Acciones de catálogo
import {
    startGetAllTipoCuenta,
    startGetAllTypeDocuments,
    startGetAllEntidadBancaria,
    startGetAllCatalogoPorId,
} from "../../actions/catalog.action";

// 🎨 Estilos
const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

// 🟢 Interfaces
interface OwnProps {
    procedureRequest: any;
}

interface LinkMapStateToProps {
    catalogState: CatalogState;
}

interface LinkMapDispatchToProps {
    startGetAllTypeDocuments: () => void;
    startGetAllTipoCuenta:()=> void;
    startGetAllEntidadBancaria:()=> void;
    startGetAllCatalogoPorId: any;
}

type Props = OwnProps & LinkMapStateToProps & LinkMapDispatchToProps;

// 🟢 Componente
const FacturaSeven = ({
    procedureRequest,
    catalogState,
    startGetAllTypeDocuments,
    startGetAllTipoCuenta,
    startGetAllEntidadBancaria,
    startGetAllCatalogoPorId,
   
}: Props) => {
    console.log('desde fac s', procedureRequest);

    const [consultaFacturaSeven, setConsultaFacturaSeven] = useState<any>({});
    const [loading, setLoading] = useState<boolean>(false);

    const creditApi = CreditApi();
    const apiReportes = ApiReportes();

    useEffect(() => {
     
        startGetAllTypeDocuments();
        startGetAllTipoCuenta();
        startGetAllEntidadBancaria();
        startGetAllCatalogoPorId(67);
        consultarFacturaSeven();
    }, []);

    const classes = useStyles();

    const consultarFacturaSeven = async () => {
        const request = procedureRequest.requestID;
        try {
            const response = await creditApi.getConsultarFacturaSeven(request);
            if (response?.isSuccess) {
                console.log(response);
                setConsultaFacturaSeven(response.data);
            }
        } catch (error) {
            console.log(error);
        }
    };

    const generarFacturaPdf = async () => {
        try {
            const request = procedureRequest.requestID;
            setLoading(true);
            const data = await apiReportes.postGenerarFacturaSevenCredito(request);
            const ext = UtilService.getFileExtension('.pdf');
            if (ext) {
                const mimeType = UtilService.getMimeTypeFromExtension(ext);
                if (mimeType) {
                    const file = new Blob([data], { type: mimeType });
                    const fileURL = URL.createObjectURL(file);
                    window.open(fileURL);
                }
            }
            setLoading(false);
        } catch (e) {
            await NotificationService.genericAlert(
                'Warning',
                'No se puede generar factura seven.',
                'warning',
            );
            setLoading(false);
        }
    };

    if (loading) {
        return <CHLoadingProcess />;
    }


    const tipoDocumentoNombre = catalogState.IdentificationType?.find(
        item => item.id === consultaFacturaSeven?.faC_TIPO_DOCUMENTO
    )?.descripcion;

    const bancoNombre = catalogState.entidadBancaria?.find(
        item => item.idEntidadBancaria === consultaFacturaSeven?.faC_BANCO_VENDEDOR
    )?.nombreRazonSocial;

    const tipoCuentaNombre = catalogState.valorCatalogo
    ?.find(
        item => Number(item.codigo) === consultaFacturaSeven?.faC_TIPO_CUENTA_VENDEDOR
    )?.descripcion;

    const tipoCreditoNombre = catalogState.creditTypes?.find(
        item => item.id === consultaFacturaSeven?.faC_TIPO_CREDITO
    )?.description;

    return (
        <CHPaper title="Factura Seven">
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Tipo de documento"
                        value={tipoDocumentoNombre || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Número Documento"
                        value={consultaFacturaSeven?.faC_NUMERO_DOCUMENTO || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Primer Nombre"
                        value={consultaFacturaSeven?.faC_NOMBRE_AFILIADO || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Segundo Nombre"
                        value={consultaFacturaSeven?.faC_SEGUNDO_NOMBRE_AFILIADO || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Primer Apellido"
                        value={consultaFacturaSeven?.faC_APELLIDO_AFILIADO || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Segundo Apellido"
                        value={consultaFacturaSeven?.faC_SEGUNDO_APELLIDO_AFILIADO || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Nombre Completo Vendedor"
                        value={consultaFacturaSeven?.faC_NOMBRE_COMPLETO_VENDEDOR || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Tipo Cuenta Vendedor"
                        value= {tipoCuentaNombre || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Número Cuenta Vendedor"
                        value={consultaFacturaSeven?.faC_NUMERO_CUENTA_VENDEDOR || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Banco Vendedor"
                        value= {bancoNombre || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="CDP"
                        value={consultaFacturaSeven?.faC_CDP || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Tipo Crédito"
                        value= {tipoCreditoNombre || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Valor Desembolso"
                        value={UtilService.formatCurrency(
                            consultaFacturaSeven?.faC_VALOR_DESEMBOLSO || 0
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Radicado"
                        value={consultaFacturaSeven?.faC_RADICADO || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Área de Negocio"
                        value='CAJA PROMOTORA DE VIVIENDA MILITAR Y DE POLICÍA'
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Número Factura"
                        value={consultaFacturaSeven?.faC_NUMERO_FACTURA || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Fecha Registro Factura"
                        value={UtilService.getDate(
                            consultaFacturaSeven?.faC_FECHA_REGISTRO_FACTURA
                        )}
                    />
                </Grid>
            </Grid>
            <Grid container justifyContent="center" style={{ marginTop: '20px' }}>
    <Button
        variant="contained"
        color="primary"
        onClick={generarFacturaPdf}
    >
        Generar Factura
    </Button>
</Grid>
        </CHPaper>
    );
};


const mapStateToProps = (state: AppState): LinkMapStateToProps => ({
    catalogState: state.catalog,
});

const mapDispatchToProps: LinkMapDispatchToProps = {
    startGetAllTypeDocuments,
    startGetAllTipoCuenta,
    startGetAllEntidadBancaria,
    startGetAllCatalogoPorId
};

export default connect(mapStateToProps, mapDispatchToProps)(FacturaSeven);
