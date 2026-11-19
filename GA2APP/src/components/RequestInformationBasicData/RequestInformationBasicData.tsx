import React, { useEffect } from 'react';
import { Grid } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { connect } from 'react-redux';
import { startPostCreditBasicData } from '../../actions/credit.actions';
import { CreditBasicDataRequestModel } from '../../models/requests/credit-basic-data-request.model';
import { CreditBasicDataModel } from '../../models/credit-basic-data.model';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import CHPaper from '../ch-paper/CHPaper';
import CHTextField from '../ch-text-field/CHTextField';
import UtilService from '../../utils/util.service';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import { REQUEST_TYPE_REQUEST } from '../../constants/constants';
import { AppState } from '../../store/rootReducer';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

function RequestInformationBasicData({
    typeRequest,
    startPostCreditBasicData,
    informacionAfiliado,
    creditBasicData,
    procedureRequest,
    isLoading,
    startGetAllPostInformationBasicSeverance,
}: {
    typeRequest?: string;
    startPostCreditBasicData: (data: CreditBasicDataRequestModel) => void;
    informacionAfiliado: any;
    creditBasicData: CreditBasicDataModel;
    procedureRequest: ProcedureRequestModel;
    isLoading: boolean;
    startGetAllPostInformationBasicSeverance: (
        numeroIdentificacion: number,
    ) => void;
}) {

    const classes = useStyles();

    useEffect(() => {
       
        if (REQUEST_TYPE_REQUEST.SEVERANCE === typeRequest) {
            startGetAllPostInformationBasicSeverance(
                procedureRequest.clientIdentificationNumber,
            );
        } else {
            const dataRequest: CreditBasicDataRequestModel = {
                ind: 2,
                soC_ID: procedureRequest.requestID,
            };
            startPostCreditBasicData(dataRequest);
        }
    }, []);

    if (isLoading || creditBasicData === null) {
        return <CHLoadingProcess />;
    }

    return (
        <CHPaper title="Información Básica">
            <Grid container>
            <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Tipo de documento"
                        value={creditBasicData.clientIdentificationType || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Número Documento"
                        value={creditBasicData.sobIDNumber || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Fecha de Expedición"
                        value={UtilService.getDate(
                            creditBasicData.sobExpeditionDate,
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Lugar de Expedición"
                        value={`${creditBasicData.expeditionState} - ${creditBasicData.expeditionCity}`}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Primer Apellido"
                        value={creditBasicData.sobFirstLastName || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Segundo Apellido"
                        value={creditBasicData.sobSecondLastName || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Primer Nombre"
                        value={creditBasicData.sobFirstName || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Segundo Nombre"
                        value={creditBasicData.sobMiddleName || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Fecha Nacimiento"
                        value={UtilService.getDate(
                            creditBasicData.sobBirthDate,
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Lugar Nacimiento"
                        value={`${creditBasicData.bornState} - ${creditBasicData.bornCity}`}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Sexo"
                        value={creditBasicData.sex || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Estado Civil"
                        value={creditBasicData.civilState || ''}
                    />
                </Grid>
                {REQUEST_TYPE_REQUEST.SEVERANCE !== typeRequest ? (
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Nivel Educativo"
                            value={creditBasicData.educationLevel || ''}
                        />
                    </Grid>
                ) : null}

                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Fuerza"
                        value={creditBasicData.force || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Categoría"
                        value={creditBasicData.category || ''}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHTextField
                        isReadOnly
                        label="Grado"
                        value={creditBasicData.grade || ''}
                    />
                </Grid>
                {REQUEST_TYPE_REQUEST.SEVERANCE !== typeRequest ? (
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Profesión"
                            value={creditBasicData.profession || ''}
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                        />
                    </Grid>
                ) : null}
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Cuotas Aportadas"
                            value={informacionAfiliado?.numeroCuotas || ''}
                            InputLabelProps={{ shrink: true }}
                        />
                    </Grid>
            </Grid>
        </CHPaper>
    );
}

const mapStateToProps = (state: AppState) => ({
    isLoadingCredit: state.credit.loading,
    creditBasicData: state.credit.creditBasicData,
    creditProductData: state.credit.creditProductData,
    creditObservationsData: state.credit.creditObservationsData,
    error: state.credit.error,
    responseRequestActivity: state.credit.responseRequestActivity,
    informationBasicSeverance: state.severance.informationBasicSeverance,
    formularioVendedor: state.credit.formularioVendedor,
    creditFilesData: state.credit.creditFilesData,
});

const mapDispatchToProps: any = {
    startPostCreditBasicData,
};

export default connect(mapStateToProps, mapDispatchToProps)(RequestInformationBasicData);