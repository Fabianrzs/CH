using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace GA2.Transversals.Commons.Extensions
{
    /// <summary>
    /// Extensión de generación archivos
    /// </summary>
    public static class FileManagerExtension
    {
        public static byte[] GenerateCsvBytes<T>(this IEnumerable<T> data, string fileName, string separator = ";")
        {
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Configura la codificación UTF-8
                Delimiter = ";" // Configura el separador deseado (punto y coma en este caso)
            };

            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream, csvConfiguration.Encoding))
            using (var csvWriter = new CsvWriter(streamWriter, csvConfiguration))
            {
                csvWriter.WriteRecords(data);
                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        public static byte[] GenerateCsvBytes<T>(this IEnumerable<T> data, Dictionary<string, string> propertiesToShow, string fileName, string separator = ";")
        {
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8, // Configura la codificación UTF-8
                Delimiter = ";" // Configura el separador deseado (punto y coma en este caso)
            };

            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream, csvConfiguration.Encoding))
            using (var csvWriter = new CsvWriter(streamWriter, csvConfiguration))
            {
                foreach (var propName in propertiesToShow.Keys)
                {
                    // Se Agrega las columnas configuradas como visibles del valor que tenga la key en el diccionario
                    csvWriter.WriteField(propertiesToShow[propName]); 
                }
                csvWriter.NextRecord();

                foreach (var item in data)
                {
                    foreach (var propName in propertiesToShow.Keys)
                    {
                        var propInfo = item.GetType().GetProperty(propName);
                        if (propInfo != null)
                        {
                            csvWriter.WriteField(propInfo.GetValue(item)); // Escribe el valor de la propiedad
                        }
                    }

                    csvWriter.NextRecord();
                }

                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }
    }
}
