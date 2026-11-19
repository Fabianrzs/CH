import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { AppState } from '../../store/rootReducer';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../components/ch-paper/CHPaper';
import {startGetAllTypesForces} from '../../actions/catalog.action';
import { CatalogState } from '../../reducers/catalog.reducer';
import TableFuerzas from './TableFuerzas';

const Fuerzas = (props: props) => {

    const {catalogState, startGetAllTypesForces} = props;

    const load = () => {
        startGetAllTypesForces();
    }

    useEffect(() => {
        load();
        console.log(catalogState.force)
    },[])

    return (
        <CHPrivateContainer>  
             <CHPaper title="Parametrización Fuerzas"> 
                <TableFuerzas load={load}/>
             </CHPaper>      
        </CHPrivateContainer>
    )
}
  
interface LinkMapStateToProps {
    catalogState: CatalogState;
}
  
const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog
})
interface LinkMapDispatchToProps {
    startGetAllTypesForces: () => void;
 }

 const mapDispatchToProps = {
    startGetAllTypesForces
 }

 type props = LinkMapStateToProps & LinkMapDispatchToProps;

 export default connect(mapStateToProps,mapDispatchToProps)(Fuerzas);

