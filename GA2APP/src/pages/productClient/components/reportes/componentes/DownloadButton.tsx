import React, { useState } from "react";
import { CircularProgress, createStyles, makeStyles, Button } from "@material-ui/core";
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

const DownloadButton = ({downloadAction}: {downloadAction: () => Promise<void>}) => {
    const classes = useStyles();
    const [downloading, setDownloading] = useState(false);
    
    const handleClick = () => {
        setDownloading(true);
        downloadAction().finally(() => setDownloading(false));
    }

    return (
        <Button variant='outlined' size='medium' onClick={handleClick} startIcon={
            downloading
            ? <CircularProgress size={20} color='inherit'/>
            : <CloudDownload />
        } disabled={downloading}>
            Descargar Excel
        </Button>
    );
}

export default DownloadButton;