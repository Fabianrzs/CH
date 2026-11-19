export interface Response<T> {
    isSuccess: boolean;
    returnMessage: string;
    data: T;
}
