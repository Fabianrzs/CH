import React, { useState } from 'react';
import { connect } from 'react-redux';
import { Grid, makeStyles, createStyles, Button, Typography } from '@material-ui/core';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { ClienteDto, Dependiente } from '../../../types/client.model.type';
import { ModalBeneficiaries } from './modalBeneficiaries';
import afiliadoApi from '../../../services/api-afiliado.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
            marginBottom: '1px'
        },
        button: {
            marginLeft: '.5rem',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        }
    }),
);

function Beneficiaries({
    catalogState,
    clientModel,

}: {
    catalogState: CatalogState;
    clientModel: ClienteDto;

}) {
    const classes = useStyles();
    const [open, setOpen] = useState(false);
    const [dependiente, setDependiente] = useState<Dependiente>();
    const [afiliado, setAfiliado] = useState<string>();
    const setterRedirectDependiente = (
        dataDependiente: Dependiente,
        afiliado: string
    ) => {
        setOpen(true);
        setDependiente(dataDependiente);
        if (clientModel?.esBeneficiario) {
            setAfiliado('Es Beneficiario de: ');
        } else {
            setAfiliado(afiliado);
        }
    };
    const handleClose = () => {
        setOpen(false);
    }
    const stringAfiliado=`${clientModel?.esBeneficiario?'Es beneficiario de:':'Beneficiarios'}`;

    return (
        <CHPaper title={stringAfiliado}>
            {clientModel?.dependientes && clientModel.dependientes.length > 0 ? (
                clientModel.dependientes.map((element, index) => (
                    <Grid key={`tableB-${index}`} container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                label="Tipo de Identificación"
                                value={catalogState.IdentificationType.find(
                                    x => x.codigo === element.identificacion.split('-')[0]
                                )?.descripcion || ''}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                label="Número de Identificación"
                                value={element.identificacion.split('-')[1] || ''}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <CHTextField
                                isReadOnly
                                label="Nombre"
                                value={element.nombre || ''}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={2}>
                            <CHTextField
                                isReadOnly
                                label="Participación"
                                value={element.participacion || ''}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={1}>
                            <Button
                                onClick={() =>
                                    setterRedirectDependiente(element, `${clientModel?.identificacion}-${clientModel?.primerNombre} ${clientModel?.primerApellido}`)
                                } className={classes.button}
                            >
                                Detalle
                            </Button>
                        </Grid>
                    </Grid>
                ))
            ) : (
                <Typography variant="body1">El afiliado no cuenta con beneficiarios</Typography>
            )}
            <ModalBeneficiaries
                data={dependiente}
                open={open}
                handleClose={handleClose}
                afiliado={afiliado}
            />
        </CHPaper>
    ); 
}
export default connect()(Beneficiaries);
