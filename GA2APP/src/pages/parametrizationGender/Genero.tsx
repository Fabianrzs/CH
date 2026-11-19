import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { AppState } from '../../store/rootReducer';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../components/ch-paper/CHPaper';
import TableGenero from './TableGenero';
import {startGetAllGeneros} from '../../actions/catalog.action';

const Genero = (props: props) => {

    const { startGetAllGeneros} = props;

    const load = () => {
        startGetAllGeneros();
    }

    useEffect(() => {
        load();
    },[])

    return (
        <CHPrivateContainer>  
             <CHPaper title="Parametrización Géneros"> 
                <TableGenero load={load}/>
             </CHPaper>      
        </CHPrivateContainer>
    )

}
  
const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog
})
interface LinkMapDispatchToProps {
    startGetAllGeneros: () => void;
 }

 const mapDispatchToProps = {
    startGetAllGeneros
 }

 type props = LinkMapDispatchToProps;

 export default connect(mapStateToProps,mapDispatchToProps)(Genero);

