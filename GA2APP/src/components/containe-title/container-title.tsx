import React from 'react';
import {
    Grid,
} from '@material-ui/core';
import './container-title.scss'


function ContainerTitle({
    title,
   
}: {
    title: string;
   
}) {

    return (
    
     <Grid className="container-25">
        <Grid className="container-ico-t">
            <span className="ico fch-Puntual-0 blue-2 icoLit"/>
            <span className="ico fch-Arrow-0-0 gray-2 icoLit-2 ico-b"/>
            <span className="tittle-tab blue-2 tittle-we"> { title }</span>
        </Grid>
      </Grid>
    );
}

export default ContainerTitle;