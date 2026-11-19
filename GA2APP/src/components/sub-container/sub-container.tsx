import React from 'react';
import './sub-container.scss';
import { Grid} from '@material-ui/core';

function CHSubContainer({ children }: { children: any }) {
    return <Grid className="container-2">{children}</Grid>  
}

export default CHSubContainer;