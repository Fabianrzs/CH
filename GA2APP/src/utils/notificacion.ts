import LogoCH from '../assets/images/LogoHorizontalCajaHonor.svg';

const notifyMe = (mensaje: string): void => {
    if (!('Notification' in window)) {
        sweetAlert({ text: 'El navegador no soporta notificaciones' });
    } else if (Notification.permission === 'granted') {
        const notification = new Notification('Notificacion GA2', {
            body: mensaje,
            icon: LogoCH,
        });
        setInterval(() => {
            notification.close();
        }, 10000);
    } else if (Notification.permission !== 'denied') {
        Notification.requestPermission().then(permission => {
            if (permission === 'granted') {
                const notification = new Notification('Notificacion GA2', {
                    body: mensaje,
                });
                setInterval(() => {
                    notification.close();
                }, 10000);
            }
        });
    }
};

export default notifyMe;
