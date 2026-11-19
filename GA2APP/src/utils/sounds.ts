const notificacionSonido = (uri: string): void => {
    const audio = new Audio(uri);
    audio.play();
};

export default notificacionSonido;
