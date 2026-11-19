import React from 'react';
import './CHPageTitleV3.scss';
import { Divider, Icon } from '@material-ui/core';
import { Brightness1 } from '@material-ui/icons';


function CHPageTitleV3({
    topText,
    topText2,
    title1,
    title2,
    icono
}: {
    topText: string;
    topText2?: string;
    title1: string;
    title2?: string;
    icono: string;
}) {
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

export default CHPageTitleV3;
