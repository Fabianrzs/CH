import React from 'react';
import './CHPrivateContainer.scss';

function CHPrivateContainer({ children }: { children: any }) {
    return <div className="container">{children}</div>;
}

export default CHPrivateContainer;
