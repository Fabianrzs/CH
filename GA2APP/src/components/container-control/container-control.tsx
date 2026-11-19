import React from 'react';
import './container-control.scss';
import { Grid} from '@material-ui/core';

function CHContainerControl({ children }: { children: any }) {
    return <Grid className="subcontainer">{children}</Grid>  
}

export default CHContainerControl;