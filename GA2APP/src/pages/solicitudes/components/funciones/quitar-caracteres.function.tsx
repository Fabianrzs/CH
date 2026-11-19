

const QuitarCaracteres = () => {
    const limpiar = (dato: string) => {
        let datoConvertir =  dato.toLowerCase();
        datoConvertir = datoConvertir.replaceAll(" ", "");
        datoConvertir = datoConvertir
        .replaceAll("á","a")
        .replaceAll("é","e")
        .replaceAll("í","i")
        .replaceAll("ó","o")
        .replaceAll("ú","u");

        return datoConvertir;
    }

    return {
        limpiar,
    };
};

export default QuitarCaracteres;
