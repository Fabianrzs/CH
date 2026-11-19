using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Transversals.Commons.Extensions
{
    public static class StreamExtension
    {
        public async static Task<Encoding> GetEncoding(this Stream stream)
        {
            var encodingByBOM = await GetEncodingByBOM(stream);
            if (encodingByBOM != null)
                return encodingByBOM;

            var encodingByParsingUTF8 = await GetEncodingByParsing(stream, Encoding.UTF8);
            if (encodingByParsingUTF8 != null)
                return encodingByParsingUTF8;

            var encodingByParsingLatin1 = await GetEncodingByParsing(stream, Encoding.GetEncoding("iso-8859-1"));
            if (encodingByParsingLatin1 != null)
                return encodingByParsingLatin1;

            var encodingByParsingUTF7 = await GetEncodingByParsing(stream, Encoding.UTF7);
            if (encodingByParsingUTF7 != null)
                return encodingByParsingUTF7;

            return null;
        }

        private async static Task<Encoding> GetEncodingByBOM(Stream stream)
        {
            try
            {
                var byteOrderMark = new byte[4];

                stream.Seek(0, SeekOrigin.Begin);
                await stream.ReadAsync(byteOrderMark.AsMemory(0, 4));
                
                if (byteOrderMark[0] == 0x2b && byteOrderMark[1] == 0x2f && byteOrderMark[2] == 0x76) return Encoding.UTF7;
                if (byteOrderMark[0] == 0xef && byteOrderMark[1] == 0xbb && byteOrderMark[2] == 0xbf) return Encoding.UTF8;
                if (byteOrderMark[0] == 0xff && byteOrderMark[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
                if (byteOrderMark[0] == 0xfe && byteOrderMark[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
                if (byteOrderMark[0] == 0 && byteOrderMark[1] == 0 && byteOrderMark[2] == 0xfe && byteOrderMark[3] == 0xff) return Encoding.UTF32;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return null;
        }

        private async static Task<Encoding> GetEncodingByParsing(Stream stream, Encoding encoding)
        {
            var encodingVerifier = Encoding.GetEncoding(encoding.BodyName, new EncoderExceptionFallback(), new DecoderExceptionFallback());

            try
            {
                stream.Seek(0, SeekOrigin.Begin);
                using var textReader = new StreamReader(stream, encodingVerifier, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
                while (!textReader.EndOfStream)
                {
                    await textReader.ReadLineAsync();
                }
                return textReader.CurrentEncoding;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return null;
        }
    }
}
