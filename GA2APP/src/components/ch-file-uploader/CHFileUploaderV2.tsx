import React, { useEffect, useRef, useState } from 'react';
import { Button } from '@material-ui/core';
import CloudUploadIcon from '@material-ui/icons/CloudUpload';
import { Autorenew, Check } from '@material-ui/icons';


function CHFileUploaderV2({
    id,
    onChangeInput,
    filesUploaded,
    disabled,
    formato,
    multipleArchivo,
    numeroFolio,
    option,
}: {
    id: any;
    onChangeInput: (id: any) => (file: any) => void;
    filesUploaded: number[];
    disabled?: boolean;
    formato?: string;
    multipleArchivo?: boolean;
    numeroFolio:string;
    option:any
}) {

    const [bloqueo , setBloqueo] = useState(false)





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
    const verificarDodo = (data: any) => {
   
        console.log(data)
    }

    return (
        <>
      
        {
            option.nombreArchivo !== 'OTRO' || option.nombreArchivo !== 'ANEXOS' ?
            <Button
            onClick={handleClick}
            variant="outlined"
            startIcon={<CloudUploadIcon  />}
            disabled = {false}
        >Cargar
        
        </Button>:
         <Button
         onClick={() => verificarDodo}
         variant="outlined"
         startIcon={<Autorenew  />}
         disabled = {false}
     >Verificar
     
     </Button>
        }
           
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

export default CHFileUploaderV2;
