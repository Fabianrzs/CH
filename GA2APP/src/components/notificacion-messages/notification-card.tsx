import React from 'react';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Typography from '@material-ui/core/Typography';
import { NotificationCardModel } from '../../models/requests/notification-cards-model';

export const NotificationMediaCard = (
    notificationCard: NotificationCardModel,
): any => {
    const { image, message } = notificationCard;
    const ImageContentCard = () => {
        if (image !== null) {
            return (
                <div>
                    <CardMedia
                        component="img"
                        alt="Contemplative Reptile"
                        height="140"
                        image="/static/images/cards/contemplative-reptile.jpg"
                        title="Contemplative Reptile"
                    />
                </div>
            );
        }
        return <></>;
    };

    return (
        <div className="width: 365">
            <CardActionArea>
                {ImageContentCard()}
                <CardContent>
                    <Typography gutterBottom variant="body1" component="h6">
                        GA2 Notificaciones
                    </Typography>
                    {message}
                </CardContent>
            </CardActionArea>
        </div>
    );
};
