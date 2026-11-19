import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { AppState } from '../../store/rootReducer';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../components/ch-paper/CHPaper';
import TableCategoria from './TableCategoria';
import { startGetAllCategories } from '../../actions/catalog.action';
import { CatalogState } from '../../reducers/catalog.reducer';
import { LoadingComponent } from '../../components/loading/loading.component';

const Categoria = (props: props) => {

    const { catalogState,
        startGetAllCategories} = props

    const load = () => {
        startGetAllCategories();
    }

    useEffect(() => {
        load();
    }, [])
    

    return (
        <CHPrivateContainer>  
             <CHPaper title="Parametrizacón Categorias"> 
                <LoadingComponent loading={catalogState?.loading} />
                <TableCategoria load={load} />
             </CHPaper>      
        </CHPrivateContainer>
    )

}

interface LinkMapStateToProps {
    catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
});

interface LinkMapDispatchToProps {
    startGetAllCategories: () => void;
}

const mapDispatchToProps = {
    startGetAllCategories
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Categoria);