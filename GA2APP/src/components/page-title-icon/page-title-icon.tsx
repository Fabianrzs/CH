import React from 'react';
import {
    Grid,
} from '@material-ui/core';
import './page-title-icon.scss';


function IconTitle({
    title,
    subTitle,
    icon,
   
}: {
    title: string;
    subTitle?: string;
    icon?:string
   
}) {

    return (
    
      <Grid className="container-3 flex">
        <Grid className="container-ico"><span className={`ico yellow icoBig ${ icon }`} /> </Grid>
        <Grid className="separador separador-marg-2 separador-alt" />
        <Grid className="tittle blue">{title}<br /><span className="yellow">{ subTitle }</span></Grid>
      </Grid>
    );
}

export default IconTitle;