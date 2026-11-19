import React, { useEffect, useState } from 'react';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    Typography,
} from '@material-ui/core';
import { connect } from 'react-redux';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';
import { AppState } from '../../store/rootReducer';
import NotificationService from '../../services/notification.service';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import TablePayBeneficiary from './TablePayBeneficiary';
import { startGetAllTypeDocuments } from '../../actions/catalog.action';
import { startGetAllBeneficiariosEmbargo } from '../../actions/embargos.action';
import { CatalogState } from '../../reducers/catalog.reducer';
import InformationPayBeneficiary from './InformationPayBeneficiary';
import { EmbargoBeneficiariosDto } from '../../types/embargo.model.type';
import { beneficiarioForm } from '../embargos/constants/embargosform.model.type';
import ApiEmbargos from '../../services/api-embargos.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '70%',
            marginLeft: '.5rem',
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

const PayBeneficiary = (props: props) => {
    const classes = useStyles();
    const api = ApiEmbargos();

    const {
        catalogState,
        startGetAllTypeDocuments,
        startGetAllBeneficiariosEmbargo,
    } = props;

    const [modoActualizar, setModoActualizar] = useState(false);

    const { handleSubmit, control, setValue, reset } =
        useForm<EmbargoBeneficiariosDto>({
            defaultValues: beneficiarioForm,
        });

    const load = async () => {
        await startGetAllBeneficiariosEmbargo();
    };

    const loadTypesDocuments = async () => {
        await startGetAllTypeDocuments();
    };

    useEffect(() => {
        loadTypesDocuments();
        load();
    }, []);

    const handleOnChangeModo = (value: boolean) => setModoActualizar(value);

    const handleOnChangeEdit = async (value: any) => {
        setValue('id', value.id);
        setValue('radicadoWorkManager', value.radicadoWorkManager);
        setValue('tipoDocumentoId', value.tipoDocumentoId);
        setValue('tipoIdentificacion', value.tipoIdentificacion);
        setValue('numeroIdentificacion', value.numeroIdentificacion);
        setValue('primerNombre', value.primerNombre);
        setValue('segundoNombre', value.segundoNombre);
        setValue('primerApellido', value.primerApellido);
        setValue('segundoApellido', value.segundoApellido);
    };

    const handleOnClickCancelar = async (value: boolean) => {
        await handleOnChangeModo(value);
        reset();
    };

    const insertarEmbargoBeneficiario = (
        embargoBeneficiario: EmbargoBeneficiariosDto,
    ) => api.postInsertEmbargoBeneficiario(embargoBeneficiario);

    const actualizarEmbargoBeneficiario = (
        embargoBeneficiario: EmbargoBeneficiariosDto,
    ) => api.postUpdateEmbargoBeneficiario(embargoBeneficiario);

    const handleOnClick = async (data: EmbargoBeneficiariosDto) => {
        if (
            data?.radicadoWorkManager.trim() === '' ||
            data?.tipoDocumentoId === 0 ||
            data?.numeroIdentificacion.trim() === '' ||
            data?.primerNombre.trim() === '' ||
            data?.primerApellido.trim() === ''
        ) {
            toast('Deben diligenciarse todos los campos.');
        } else if (!modoActualizar) {
            const respuesta = await insertarEmbargoBeneficiario(data);
            if (respuesta?.isSuccess === false) {
                swal({
                    text: respuesta.returnMessage,
                    icon: 'warning',
                });
            } else {
                reset();
                load();
                await NotificationService.genericAlert(
                    'Guardado',
                    'Se ha guardado correctamente el beneficiario',
                );
            }
        } else {
            const respuesta = await actualizarEmbargoBeneficiario(data);
            if (respuesta?.isSuccess === false) {
                swal({
                    text: respuesta.returnMessage,
                    icon: 'warning',
                });
            } else {
                await handleOnChangeModo(false);
                reset();
                load();
                await NotificationService.genericAlert(
                    'Actualizado con éxito',
                    'Se realizó con éxito la actualización.',
                );
            }
        }
    };

    return (
        <CHPrivateContainer>
            <CHPaper title="Beneficiarios de pago por expediente">
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Información Beneficiarios
                </Typography>
                <InformationPayBeneficiary
                    tiposIdentificacion={catalogState.IdentificationType}
                    control={control}
                />
                <Grid container>
                    <div style={{ marginLeft: '75%' }}>
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="primary"
                            style={{ width: '140px' }}
                            onClick={handleSubmit(handleOnClick)}
                        >
                            {!modoActualizar ? 'AGREGAR' : 'ACTUALIZAR'}
                        </Button>
                    </div>
                    {modoActualizar ? (
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="default"
                            style={{ width: '140px' }}
                            onClick={() => handleOnClickCancelar(false)}
                        >
                            CANCELAR
                        </Button>
                    ) : null}
                </Grid>
                <br />
                <TablePayBeneficiary
                    load={load}
                    handleOnChangeEdit={handleOnChangeEdit}
                    handleOnChangeModo={handleOnChangeModo}
                />
            </CHPaper>
        </CHPrivateContainer>
    );
};

interface LinkMapStateToProps {
    catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
    embargoState: state.embargo,
    catalogState: state.catalog,
});
interface LinkMapDispatchToProps {
    startGetAllBeneficiariosEmbargo: () => void;
    startGetAllTypeDocuments: () => void;
}

const mapDispatchToProps = {
    startGetAllBeneficiariosEmbargo,
    startGetAllTypeDocuments,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(PayBeneficiary);
