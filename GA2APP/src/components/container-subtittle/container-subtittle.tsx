import React from 'react';
import {
    Grid,
} from '@material-ui/core';
import './container-subtittle.scss';





function CHContainerSubtitle({
    title,
    subTitle,
    icon,
   
}: {
    title: string;
    subTitle?: string;
    icon?:string
   
}) {

    return (
        <Grid className="container-subtittle">
        <Grid className="flex-l">
          <Grid className="container-ico blue icoMid-6"><span className={`ico ${ icon }`} /></Grid>
          <Grid className="separador separador-marg separador-alt-2" />
          <Grid className="subtittle blue">{ title }<br /><span className="cian">{subTitle}</span></Grid>
        </Grid>
      </Grid>
    );
}

export default CHContainerSubtitle;