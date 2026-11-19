import React from 'react';
import {
    Grid,
} from '@material-ui/core';
import './page-navigate-indicator.scss'



function PageIndicators({
    topTextNivel1,
    topTextNivel2,
   
}: {
    topTextNivel1: string;
    topTextNivel2?: string;
   
}) {

    return (
       <Grid item >
            <ul className="breadcrumb">
                <li><span className="ico fch-Point-0 gray" /></li>
                <li><a href="###">{ topTextNivel1 }</a></li>
                <li><span className="ico fch-Point-0 blue" /></li>
                <li><a href="###">{ topTextNivel2 } </a></li>
            </ul>
       </Grid>
   
    );
}

export default PageIndicators;
