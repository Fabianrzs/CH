import React from 'react';
import { Button, createStyles, makeStyles } from '@material-ui/core';
// import { UseFormHandleSubmit } from 'react-hook-form';
import { BOTONERA_SOLICITUD } from '../../../../constants/constants';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        columnLabel: {
            padding: '10px',
        },
    }),
);

function BotoneraInformacionSolicitud({
    onItemGoBack,
    onSubmit,
    onSubmitAprobar,
    onSubmitSubsanar,
    onSubmitRechazar,
    tipoAccion,
}: // handleSubmit,
{
    onItemGoBack: () => void;
    onSubmit: (value: any) => Promise<void>;
    onSubmitAprobar: () => void;
    onSubmitSubsanar: () => void;
    onSubmitRechazar: () => void;
    tipoAccion: number;
    // handleSubmit: UseFormHandleSubmit<any>;
    // handleSubmit: UseFormHandleSubmit<any> | undefined;
}) {
    const classes = useStyles();
    return tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR ? (
        <>
            <div className="buttons">
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onSubmit}
                >
                    ACTUALIZAR
                </Button>
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onSubmitAprobar}
                > 
                    APROBAR 
                </Button>

                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onSubmitSubsanar}
                >
                    SUBSANAR
                </Button>

                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onSubmitRechazar}
                >
                    RECHAZARR 
                </Button>
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onItemGoBack}
                >
                    REGRESAR
                </Button>
            </div>
        </>
    ) : null;
}

export default BotoneraInformacionSolicitud;
