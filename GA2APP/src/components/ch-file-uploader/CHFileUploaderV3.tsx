import React, { useRef } from 'react';
import { Button } from '@material-ui/core';
import CloudUploadIcon from '@material-ui/icons/CloudUpload';


function CHFileUploaderV3({
    id,
    onChangeInput,
    filesUploaded,
    disabled,
    formato,
    multipleArchivo
}: {
    id: any;
    onChangeInput: (id: any) => (file: any) => void;
    filesUploaded: number[];
    disabled?: boolean;
    formato?: string;
    multipleArchivo?: boolean;
}) {

    const hiddenFileInput = useRef<HTMLInputElement>(null);

    const handleClick = () => {
        hiddenFileInput?.current?.click();
    };

    const isUploaded = filesUploaded.some(fileID => fileID === id);

    if (isUploaded) {
        return <>Cargado</>;
    }

    if (!isUploaded && disabled) {
        return null;
    }

    return (
        <>
            <Button
                onClick={handleClick}
                variant="outlined"
                startIcon={<CloudUploadIcon />}
            >Cargar</Button>
            {multipleArchivo ?
                <input
                    type="file"
                    hidden
                    ref={hiddenFileInput}
                    multiple
                    onChange={onChangeInput(id)}
                    accept = {formato}
                />
                :
                <input
                    type="file"
                    hidden
                    ref={hiddenFileInput}
                    onChange={onChangeInput(id)}
                    accept = {formato}
                />
            }

        </>
    );
}

export default CHFileUploaderV3;
