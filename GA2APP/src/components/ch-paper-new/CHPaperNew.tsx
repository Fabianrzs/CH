import { Paper, withStyles } from '@material-ui/core';
import React from 'react';
import './CHPaperNew.scss';

const CustomPaper = withStyles({
    root: {
        marginBottom: '1rem',
        flex: 1,
    },
})(Paper);

function CHPaper({ children }: { children: any; }) {
    return (
        <CustomPaper>
            <div className="paper-content">{children}</div>
        </CustomPaper>
    );
}

export default CHPaper;
