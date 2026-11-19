import swal from "sweetalert";

type AlertType = 1 | 2 | 3 | 4 | 5;

export const showAlert = (type: AlertType) => {
  const alertConfig: Record<AlertType, { title: string; text: string; icon: "error" | "warning" | "info" }> = {
    1: {
      title: 'Error de conexión',
      text: 'No se pudo establecer conexión con el servidor. Verifica tu red e intenta de nuevo.',
      icon: 'error'
    },
   2: {
      title: 'Datos inválidos',
      text: 'Algunos campos contienen errores o están incompletos. Por favor verifica la información.',
      icon: 'warning'
    },
    3: {
      title: 'Acceso denegado',
      text: 'No tienes permisos para realizar esta acción. Contacta con el administrador.',
      icon: 'error'
    },
    4: {
      title: 'Alerta',
      text: 'Funcionalidad no aplicable para este proceso',
      icon: 'warning'
    },
    5: {
      title: 'Alerta',
      text: 'No aplica para la función solicitada',
      icon: 'warning'
    }
  };

  const { title, text, icon } = alertConfig[type];

  swal({
    title,
    text,
    icon,
    buttons: {
      confirm: {
        text: 'Entendido',
        value: true,
        visible: true,
        className: '',
        closeModal: true
      }
    }
  });
};
