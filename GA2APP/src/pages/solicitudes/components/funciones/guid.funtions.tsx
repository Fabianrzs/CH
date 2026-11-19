import { v4 as uuid } from 'uuid';

const GenerarGuid = () => {
    const newGuid = () => {
        const id: string = uuid();
        return id;
    }

    return {
        newGuid,
    };
};

export default GenerarGuid;
