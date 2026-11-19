import convert from 'convert-string-bytes';

export function dataExcel(bytesExcel:any)
{
    const data=atob(convert.bytesToString(bytesExcel))
            const sliceSize=1024
            const bytesLength= data.length
            const slicesCount= Math.ceil(bytesLength / sliceSize)
            const byteArrays= new Array(slicesCount)
            for(let sliceIndex=0; sliceIndex < slicesCount; sliceIndex+=1)
            {
                const begin = sliceIndex * sliceSize
                const end = Math.min(begin+sliceSize,bytesLength)
                const bytes = new Array(end - begin)
                for(let offset=begin,i=0;offset < end; i+=1,offset+=1)
                {
                    bytes[i]= data[offset].charCodeAt(0)
                }
                byteArrays[sliceIndex] = new Uint8Array(bytes)
            }
    return byteArrays

}

