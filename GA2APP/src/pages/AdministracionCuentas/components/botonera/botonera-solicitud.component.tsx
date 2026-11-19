import React from 'react';
import {  Button, createStyles, makeStyles } from "@material-ui/core";
import { UseFormHandleSubmit } from 'react-hook-form';
import { BOTONERA_SOLICITUD } from '../../../../constants/constants';
import { LoadingComponent } from '../../../../components/loading/loading.component';



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


function BotoneraSolicitud({
    onItemGoBack,
    onSubmit,
    onSubmitAprobar,
    onSubmitRechazar,
    tipoAccion,
    handleSubmit,
}: {
    onItemGoBack: () => void;
    onSubmit: () => void;
    onSubmitAprobar: () => void;
    onSubmitRechazar: () => void;
    tipoAccion: number;
    handleSubmit: UseFormHandleSubmit<any> | undefined;
}) {
    const classes = useStyles();
    return (
        <>
            <div className="buttons">
                {handleSubmit !== undefined ?
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={handleSubmit(onSubmit)}
                >
                    {tipoAccion === BOTONERA_SOLICITUD.GUARDAR ? 'GUARDAR': 'ACTUALIZAR'}
                </Button>
                    : <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onSubmit}
                >
                    {tipoAccion === BOTONERA_SOLICITUD.GUARDAR ? 'GUARDAR': 'ACTUALIZAR'}
                </Button> }
                {tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR ?
                    <Button
                        className={classes.button}
                        variant="outlined"
                        onClick={onSubmitAprobar}
                    > 
                        APROBAR
                    </Button>
                    :
                    ''
                }
                {tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR ?
                    <Button
                        className={classes.button}
                        variant="outlined"
                        onClick={onSubmitRechazar}
                    >
                        RECHAZAR
                    </Button>
                    :
                    ''
                }
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onItemGoBack}
                >
                    REGRESAR
                </Button>
            </div>
        </>
    )
}


export default (BotoneraSolicitud);
