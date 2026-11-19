import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Grid, makeStyles, createStyles,Typography, } from '@material-ui/core';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { ClienteDto } from '../../../types/client.model.type';


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center'
        }, 
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        }, 
        titleCampo: {
            fontSize: '10px',
           
        },       
    }),
);

function Historial({
    catalogState,
    clientModel,
}: {
    catalogState: CatalogState;
    clientModel: ClienteDto;
}){    
    
    const [historial, setHistorial] = useState<boolean>(false);
    const classes = useStyles(); 
    useEffect(() => {
        const validateHistorial = () => {
            if(clientModel?.cuentas?.length !== 0) {
               setHistorial(true);
            }
        }

        validateHistorial()
    });
    
        return(
            <CHPaper title="Historial causal cambio de estado en cuentas">
                
                    <Typography className={classes.titleform} variant="h5" component="h6">
                        Historial causales
                    </Typography>
                    {
                        historial ? (
                            
                        <table className="table">
                            <thead>
                                <tr>
                                    <th>Cuenta</th>
                                    <th>Concepto</th>
                                    <th>Causal Estado</th>  
                                </tr>
                            </thead>
    
                            <tbody>
                                {clientModel?.cuentas?.map((element, index) => (
                                    <tr key={`table-schedule${index}`}>
                                        <td className='dateReport3'>{element.causalEstado}</td>
                                        <td className='dateReport3'>{element.causalEstado}</td>
                                        <td className='dateReport3'>{element.causalEstado}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                        ) : 'El afiliado no cuenta con historial de causales de bloqueo'
                    }  
            </CHPaper>   )
    }
    export default connect()(Historial);