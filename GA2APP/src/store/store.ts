import { applyMiddleware, createStore } from 'redux';
import thunk, { ThunkMiddleware } from 'redux-thunk';
import { composeWithDevTools } from 'redux-devtools-extension';
import { throttle } from 'lodash';
import { rootReducer } from './rootReducer';
import StorageService from '../services/storage.service';

/** State in localstorage * */
const persistedState = StorageService.loadState();

export const store = createStore(
    rootReducer,
    persistedState,
    composeWithDevTools(applyMiddleware(thunk as ThunkMiddleware)),
);

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

store.subscribe(
    throttle(() => {
        StorageService.saveState({
            ...store.getState(),
        });
    }, 1000),
);
