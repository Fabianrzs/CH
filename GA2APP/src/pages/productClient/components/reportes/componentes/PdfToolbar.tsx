import React from "react";
import { IconButton, Tooltip } from "@material-ui/core";
import PictureAsPdfIcon from "@material-ui/icons/PictureAsPdf";

const PdfToolbar = ({ pdfAction }: { pdfAction: () => void }) => (
    <Tooltip title="Generar PDF">
        <IconButton onClick={pdfAction}>
            <PictureAsPdfIcon />
        </IconButton>
    </Tooltip>
);

export default PdfToolbar;