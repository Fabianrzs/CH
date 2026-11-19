import React from 'react';
import Nav from '../navs/Nav';
import Notify from '../notify/Notify';

export const Layout: React.FC = (props?: any) => {
    const { children } = props;
    return (
        <div>
            <Notify />
            <Nav pageshow={children} />
        </div>
    );
};
