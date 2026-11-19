function generateRandomHexString(length: any) {
    const buf = new Uint16Array(length);
    window.crypto.getRandomValues(buf);

    let hexString = '';
    for (let i = 0; i < length; i+=1) {
        hexString += buf[i].toString(16).padStart(4, '0'); 
    }
    return hexString;
}

const GenerarGuid = () => {

    const newGuid = () => {
    let result: string;
    result = "";
    const part1 = generateRandomHexString(2); 
    const part2 = generateRandomHexString(1);
    const part3 = generateRandomHexString(1);
    const part4 = generateRandomHexString(1);
    const part5 = generateRandomHexString(3); 
    result = `${part1}-${part2}-${part3}-${part4}-${part5}`;
    return result
    }
    return {
        newGuid
    };
};


export default GenerarGuid;