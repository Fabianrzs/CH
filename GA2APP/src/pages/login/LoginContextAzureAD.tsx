import AuthenticationContext from 'react-adal/lib/adal';
import { routeReactApp } from '../../utils/baserouteapi.route';

const config = {
    clientId: 'bf0f363c-e471-4f15-a129-1b7e68ab42f5',
    cacheLocation: 'localStorage',
    redirectUri: routeReactApp
  };

const adalContext = new AuthenticationContext(config);

export const authContext = adalContext;

export const getToken = () => adalContext.getCachedToken(config.clientId);

export const login = () => {
  adalContext.login();
};

export const logout = () => {
  adalContext.logOut();
};

export const isLoggedIn = () => !!adalContext.getCachedToken(config.clientId);

export const getCachedUser = () => {
    const user = adalContext.getCachedUser();
    return user;
  };