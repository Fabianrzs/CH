import { Paper, withStyles } from '@material-ui/core';
import React from 'react';
import './CHPaperV2.scss';

const CustomPaper = withStyles({
    root: {
        marginBottom: '1rem',
        flex: 1,
    },
})(Paper);

function CHPaperV2({ children }: { children: any; }) {
    return (
        <CustomPaper>
            <div className="paper-content">{children}</div>
        </CustomPaper>
    );
}

export default CHPaperV2;
