import { ScheduleNoveltiesModel } from '../models/requests/Schedule-Novelties.Model';

export const POST_SHEDULE_NOVELTIES_REQUEST = 'POST_SHEDULE_NOVELTIES_REQUEST';

export const POST_SHEDULE_NOVELTIES_REQUEST_SUCCESS =
    'POST_SHEDULE_NOVELTIES_REQUEST_SUCCESS';

export const ON_ERROR = 'ON_ERROR';

interface PostScheduleNoveltiesRequestSuccess {
    type: 'POST_SHEDULE_NOVELTIES_REQUEST_SUCCESS';
    schedulNovelties: ScheduleNoveltiesModel[];
}

interface PostScheduleNoveltiesRequest {
    type: 'POST_SHEDULE_NOVELTIES_REQUEST';
}

export type ScheduleNoveltiesActionsType =
    | PostScheduleNoveltiesRequestSuccess
    | PostScheduleNoveltiesRequest;
