import { Paper, withStyles } from '@material-ui/core';
import React from 'react';
import './CHPaper.scss';

const CustomPaper = withStyles({
    root: {
        borderRadius: '14px',
        marginBottom: '1rem',
        flex: 1,
    },
})(Paper);

function CHPaper({ children, title }: { children: any; title: string }) {


    return (
        <CustomPaper>
            <div className="paper-title">{title}</div>
            <div className="paper-content">{children}</div>
        </CustomPaper>
    );
}

export default CHPaper;
