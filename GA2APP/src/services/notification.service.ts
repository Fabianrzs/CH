import { toast } from 'react-toastify';
import swal from 'sweetalert';
import {
  NotificationCardModel,
  NotificationCardModelEnum,
} from '../models/requests/notification-cards-model';
import { NotificationMediaCard } from '../components/notificacion-messages/notification-card';

export default class NotificationService {
  static async confirmAlert(
    title: string,
    message = '',
    icon = 'warning',
    cancelButton = 'Cancelar',
    okButton = 'Confirmar',
  ): Promise<any> {
    return swal({
      title,
      text: message,
      icon,
      buttons: [cancelButton, okButton],
      dangerMode: true,
    });
  }

  static async messageAlert(
    title: string,
    message = '',
    icon = 'warning',
  ): Promise<any> {
    return swal({
      title,
      text: message,
      icon,
      dangerMode: true,
    });
  }



  static async genericAlert(
    title: string,
    message: string,
    icon = 'success',
  ): Promise<void> {
    return swal({
      title,
      text: message,
      icon,
    });
  }

  static async genericAlertError(
    title: string,
    message: string,
    icon = 'error',
  ): Promise<void> {
    return swal({
      title,
      text: message,
      icon,
    });
  }

  static async genericAlwarning(
    title: string,
    message: string,
    icon = 'warning',
  ): Promise<void> {
    return swal({
      title,
      text: message,
      icon,
    });
  }

  static async showNotification(
    notificationCard: NotificationCardModel,
  ): Promise<any> {
    switch (notificationCard.type) {
      case NotificationCardModelEnum.warn:
        return toast.warn(NotificationMediaCard(notificationCard));
      case NotificationCardModelEnum.success:
        return toast.success(NotificationMediaCard(notificationCard));
      case NotificationCardModelEnum.error:
        return toast.error(NotificationMediaCard(notificationCard));
      case NotificationCardModelEnum.info:
        return toast.info(NotificationMediaCard(notificationCard));
      case NotificationCardModelEnum.dark:
        return toast.dark(NotificationMediaCard(notificationCard));
      default:
        break;
    }
    return null;
  }
}
