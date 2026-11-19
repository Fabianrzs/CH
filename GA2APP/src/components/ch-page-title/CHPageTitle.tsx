import ArrowForwardIosIcon from '@material-ui/icons/ArrowForwardIos';
import React from 'react';
import './CHPageTitle.scss';

function CHPageTitle({
    topText,
    subText,
    title1,
    title2,
}: {
    topText: string;
    subText: string;
    title1: string;
    title2: string;
}) {

    return (
        <div>
            <div className="top-information">
                <ArrowForwardIosIcon className="arrow-icon" /> {topText}
            </div>
            <div className="subtext-information">{subText}</div>
            <div className="title-page">
                {title1}
                <br />
                <span>{title2}</span>
            </div>
        </div>
    );
}

export default CHPageTitle;
