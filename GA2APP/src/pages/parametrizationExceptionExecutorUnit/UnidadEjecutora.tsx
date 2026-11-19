import React, { useEffect, useState } from 'react';
import { Container, 
         createStyles,
         Grid, 
         makeStyles, 
         MenuItem, 
         Typography} from '@material-ui/core';
import { connect } from 'react-redux';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../components/ch-select/CHSelect';
import TableExcepUnidadEjecutora from './TableExcepUnidadEjecutora';
import { SimuladorState } from '../../reducers/simulador.reducer';
import { AppState } from '../../store/rootReducer';
import { SimuladorDto } from '../../types/simulador.model.type';
import { startGetAllSimuladorTasas,
         startPostAllSimuladorUniEjTasas } from '../../actions/simulador.action';
import { LoadingComponent } from '../../components/loading/loading.component';

const useStyles = makeStyles(() =>
    createStyles({        
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        }, 
        column: {
            padding: '10px',
            textAlign: 'center',
        },      
    }),
);

const UnidadEjecutora = (props: Props) => {
    const classes = useStyles(); 
    const {simuladorState,
           startGetAllSimuladorTasas,
           startPostAllSimuladorUniEjTasas} = props;
    const [idSimulacion, setIdSimulacion] = useState(1);

    const loadSimulador = () => {    
        startGetAllSimuladorTasas();
    };

    const load = () => {       
        startPostAllSimuladorUniEjTasas(idSimulacion); 
    };
    
    useEffect(() => {
        loadSimulador();     
    }, []);

    useEffect(() => {
        load();
    }, [idSimulacion]);

    const handleOnChange = (target: any) => {        
        const valueNumber = target.value;
        setIdSimulacion(valueNumber);   
    }   

    return (
        <CHPrivateContainer>    
            <CHPaper title="Parámetros de Crédito Hipotecario">          
            <Grid container>
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Excepciones por Unidad Ejecutora
                </Typography>
            </Grid>
            <hr/>          
            <Grid container>
              <Grid className={classes.column} item xs={12} md={4}> 
              <CHSelect
                label=""
                name="simulador"
                value= {idSimulacion}   
                onChange={e => handleOnChange(e.target)}                  
                >
                  {simuladorState.simuladorGeneral?.map(
                        (option: SimuladorDto) => (
                            <MenuItem
                                key={option.idSimulacion}
                                value={option.idSimulacion}
                            >
                                {option.descripcion}
                            </MenuItem>
                        ),
                    )}
              </CHSelect>
              </Grid>
              </Grid>
              <Container>
                <LoadingComponent loading={simuladorState?.loading} />
                <Grid container spacing={3}>
                    <Grid item xs={6} sm={12}>  
                           <TableExcepUnidadEjecutora load={load} selectedSimulacion={idSimulacion}/>
                    </Grid>
                </Grid>
            </Container> 
           </CHPaper>  
        </CHPrivateContainer>
    )
}
interface LinkMapStateToProps {
    simuladorState: SimuladorState;
}

const mapStateToProps = (state: AppState) => ({
    simuladorState: state.simulador,
});

interface LinkMapDispatchToProps {
    startGetAllSimuladorTasas: () => void;
    startPostAllSimuladorUniEjTasas: (idSimulacion: number) => void;
}    

const mapDispatchToProps = {
    startGetAllSimuladorTasas,
    startPostAllSimuladorUniEjTasas
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default  connect(mapStateToProps, mapDispatchToProps)(UnidadEjecutora);
