import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { AppState } from '../../store/rootReducer';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import { startGetAllTypeDocuments } from '../../actions/catalog.action';
import TableTypeId from './TableTypeId';

const TipoDocumento = (props: props) => {

    const {startGetAllTypeDocuments} = props;

    const load = () => {
        startGetAllTypeDocuments();
    }

    useEffect(() => {
        load();
    },[])

    return (
        <CHPrivateContainer>
            <CHPaper title='Parametrización Tipos Documento'>
                <TableTypeId load={load}/>
            </CHPaper>
        </CHPrivateContainer>
    )
}
  
const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog
})
interface LinkMapDispatchToProps {
    startGetAllTypeDocuments: () => void;
 }

 const mapDispatchToProps = {
    startGetAllTypeDocuments
 }

 type props = LinkMapDispatchToProps;

 export default connect(mapStateToProps,mapDispatchToProps)(TipoDocumento);