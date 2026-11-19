import { Dispatch } from 'redux';
import { ScheduleNoveltiesModel } from '../models/requests/Schedule-Novelties.Model';
import { AppActions } from '../types/appactions.type';
import AuthService from '../services/auth.service';
import {
    POST_SHEDULE_NOVELTIES_REQUEST,
    ON_ERROR,
} from '../types/schedulenovelties.actions.type';

const PostScheduleNovelties = (): AppActions => ({
    type: POST_SHEDULE_NOVELTIES_REQUEST,
});

const SetError = (error: any): AppActions => ({
    type: ON_ERROR,
    error,
});

export const startPostProcedureRequest =
    (data: ScheduleNoveltiesModel) =>
    async (dispatch: Dispatch<AppActions>): Promise<void> => {
        try {
            const user = AuthService.getUserInfo();
            data.UserId = user.USR_ID;
            dispatch(PostScheduleNovelties());
        } catch (e) {
           
            dispatch(SetError('server-error'));
        }
    };
