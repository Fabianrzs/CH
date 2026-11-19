import React, { useState } from "react";
import { CircularProgress, IconButton, createStyles, makeStyles, Tooltip } from "@material-ui/core";
import { CloudDownload } from "@material-ui/icons";


const useStyles = makeStyles(() =>
    createStyles({
        circularProgress: {
            'verticalAlign': 'middle',
            'marginLeft': '14px',
            'marginRight': '14px'
        }
    }),
);

const DownloadToolbar = ({downloadAction}: {downloadAction: () => Promise<void>}) => {
    const classes = useStyles();
    const [downloading, setDownloading] = useState(false);
    
    const handleClick = () => {
        setDownloading(true);
        downloadAction().finally(() => setDownloading(false));
    }

    return (
        <Tooltip title="Descargar Excel">
            {
                downloading
                ? <CircularProgress size={20} className={classes.circularProgress}/>
                : <IconButton onClick={handleClick} >
                    <CloudDownload/>
                  </IconButton>
            }
        </Tooltip>
    );
}

export default DownloadToolbar;