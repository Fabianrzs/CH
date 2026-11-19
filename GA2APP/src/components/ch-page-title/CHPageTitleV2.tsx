import React, { useEffect } from 'react';
import './CHPageTitleV2.scss';
import { Divider, Icon } from '@material-ui/core';
import { Brightness1 } from '@material-ui/icons';


function CHPageTitleV2(props:any) {

    const {   topText,
        title1,
        icono,
        topText2,
        title2} = props
  
    return (
        <div className="contenido-titulo">
            <div className="top-information">
                <Brightness1 className="arrow-icon" /> {topText}
                {topText2 !== undefined ? <Brightness1 className="arrow-icon" /> : null} {topText2}
            </div>
            <Divider />

            <div className="contenido">
                <Icon className="icon-title">{icono}</Icon>
                <Divider orientation="vertical" flexItem />
                <div className="title-page1">
                    {title1} <div className="title-page2">
                        {title2}
                    </div>
                </div>
            </div>

        </div>
    );
}

export default CHPageTitleV2;
