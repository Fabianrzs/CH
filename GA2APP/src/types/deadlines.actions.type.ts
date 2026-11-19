export const YEARMIN = '';
export const MONTHMIN = '';
export const YEARMAX = '';
export const MONTHMAX = '';

export interface SetYearMin {
    type: typeof YEARMIN;
    login: number;
}

export interface SetMonthMin {
    type: typeof MONTHMIN;
    claims: number;
}

export interface SetYearMax {
    type: typeof YEARMAX;
    claims: number;
}

export interface SetMonthMax {
    type: typeof MONTHMAX;
    claims: number;
}

export type DeadlineActionsTypes =
    | SetYearMin
    | SetMonthMin
    | SetYearMax
    | SetMonthMax;
