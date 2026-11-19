import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import { Provider } from 'react-redux';
import {Router } from 'react-router-dom';
import history from './utils/history.router';
import App from './App';
import './assets/fonts/LibIcons/v1.2.0/style.css';
import './assets/fonts/LibIcons/v1.3.0/style.css';
import reportWebVitals from './reportWebVitals';
import { store } from './store/store';

ReactDOM.render(
    <React.StrictMode>
        <Provider store={store}>
            <Router history={history}>
                <App />
            </Router>
        </Provider>
    </React.StrictMode>,
    document.getElementById('root')
);

reportWebVitals();
