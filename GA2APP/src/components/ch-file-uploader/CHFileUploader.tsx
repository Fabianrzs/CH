import React, { useRef } from 'react';
import { Button } from '@material-ui/core';

function CHFileUploader({
    id,
    onChangeInput,
    filesUploaded,
    disabled,
    file
}: {
    id: any;
    onChangeInput: (id: any) => (file: any) => void;
    filesUploaded: number[];
    disabled?: boolean;
    file: any;
}) {
    const hiddenFileInput = useRef<HTMLInputElement>(null);

    const handleClick = () => {
        hiddenFileInput?.current?.click();
    };

    const isUploaded = filesUploaded.some(fileID => fileID === id);

   

    if (!isUploaded && disabled) {
        return null;
    }

    return (
        <>
            {(file?.carga === 'S' ) && (
            <Button onClick={handleClick}>Cargar</Button>
            )}
            <input
                type="file"
                hidden
                ref={hiddenFileInput}
                onChange={onChangeInput(id)}
            />
        </>
    );
}

export default CHFileUploader;
