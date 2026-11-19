using CsvHelper;
using GA2.Application.Dto;
using IronOcr;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using Aspose.Cells.Utility;

namespace GA2.Domain.Core
{
    internal abstract class HandlerFileToJson
    {
        public HandlerFileToJson()
        {
        }

        /// <summary>
        /// Lee un archivo excel y lo regresa como un json
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static Task<string> ConvertXLXtoJson(StreamReader stream)
        {
            //Load your source workbook
            Workbook workbook = new Workbook(stream.BaseStream);

            MemoryStream ms = new MemoryStream();
            // convert the workbook to json file.
            workbook.Save(ms, SaveFormat.Json);
            return Task.FromResult(Encoding.UTF8.GetString(ms.ToArray()));

            //try
            //{
            //    DataTable dtTable = new DataTable();
            //    //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
            //    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream.BaseStream, false))
            //    {
            //        //create the object for workbook part  
            //        WorkbookPart workbookPart = doc.WorkbookPart;

            //        Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

            //        //using for each loop to get the sheet from the sheetcollection  
            //        foreach (Sheet thesheet in thesheetcollection.OfType<Sheet>())
            //        {
            //            //statement to get the worksheet object by using the sheet id  
            //            Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;

            //            SheetData thesheetdata = theWorksheet.GetFirstChild<SheetData>();



            //            for (int rCnt = 0; rCnt < thesheetdata.ChildElements.Count(); rCnt++)
            //            {
            //                List<string> rowList = new List<string>();
            //                for (int rCnt1 = 0; rCnt1
            //                    < thesheetdata.ElementAt(rCnt).ChildElements.Count(); rCnt1++)
            //                {

            //                    Cell thecurrentcell = (Cell)thesheetdata.ElementAt(rCnt).ChildElements.ElementAt(rCnt1);
            //                    //statement to take the integer value  
            //                    string currentcellvalue = string.Empty;
            //                    if (thecurrentcell.DataType != null)
            //                    {
            //                        if (thecurrentcell.DataType == CellValues.SharedString)
            //                        {
            //                            int id;
            //                            if (Int32.TryParse(thecurrentcell.InnerText, out id))
            //                            {
            //                                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
            //                                if (item.Text != null)
            //                                {
            //                                    //first row will provide the column name.
            //                                    if (rCnt == 0)
            //                                    {
            //                                        dtTable.Columns.Add(item.Text.Text);
            //                                    }
            //                                    else
            //                                    {
            //                                        rowList.Add(item.Text.Text);
            //                                    }
            //                                }
            //                                else if (item.InnerText != null)
            //                                {
            //                                    currentcellvalue = item.InnerText;
            //                                }
            //                                else if (item.InnerXml != null)
            //                                {
            //                                    currentcellvalue = item.InnerXml;
            //                                }
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (rCnt != 0)//reserved for column values
            //                        {
            //                            rowList.Add(thecurrentcell.InnerText);
            //                        }
            //                    }
            //                }
            //                if (rCnt != 0)//reserved for column values
            //                    dtTable.Rows.Add(rowList.ToArray());

            //            }

            //        }

            //        return Task.FromResult(JsonConvert.SerializeObject(dtTable));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    throw;
            //}
        }

        internal static async Task<string> ConvertCSVtoJson(StreamReader stream)
        {
            TextReader tr = stream;
            var text = (await tr.ReadToEndAsync()).Split(new char[] { '\r', '\n' });

            var texto = new List<string>();
            char caracterVacio = default;
            foreach (var linea in text)
            {
                if (linea != string.Empty)
                {
                    var nuevaLinea = linea.Replace('"', caracterVacio);
                    nuevaLinea = nuevaLinea.Replace("\0", "");
                    texto.Add(nuevaLinea);
                }

            }

            List<PagoMasivoNominaDto> listaMasivo = new List<PagoMasivoNominaDto>();
            texto.RemoveAt(0);
            foreach (var nomina in texto)
            {
                var nuevaNomina = nomina.Split(',');
                var listaNomina = nuevaNomina.ToList();
                listaNomina.RemoveAt(listaNomina.Count - 1);
                listaNomina.RemoveAt(listaNomina.Count - 1);
                var valorPago = string.Empty;

                for (int i = 7; i < listaNomina.Count; i++)
                {
                    valorPago +=  decimal.Parse(listaNomina[i]);
                }
                listaMasivo.Add(new PagoMasivoNominaDto { VALOR = decimal.Parse(valorPago), CEDULA = nuevaNomina[5], FUERZA = nuevaNomina[0] });
            }

            var resultado = JsonConvert.SerializeObject(listaMasivo);

            return resultado;
        }

        internal static async Task<(string, string)> ConvertTxtToJson(StreamReader stream)
        {
            var lines = new List<KeyValuePair<string, string>>();
            var units = new List<KeyValuePair<string, string>>();

            do
            {
                string textLine = await stream.ReadLineAsync();
                var split = textLine.Split(';');
                KeyValuePair<string, string> pago = new KeyValuePair<string, string>(int.Parse(split[5]).ToString(), decimal.Parse(split[8]).ToString());
                KeyValuePair<string, string> other = new KeyValuePair<string, string>(split[2].ToString(), split[7].ToString());

                lines.Add(pago);
                units.Add(other);

            } while (stream.Peek() != -1);

            return (JsonConvert.SerializeObject(lines),
                JsonConvert.SerializeObject(units));


        }

        internal static async Task<string> ConvertPdfToJson(Stream baseStream)
        {
            var Ocr = new IronTesseract();
            List<string> text = new();
            using (var Input = new OcrInput(baseStream))
            {
                var Result = Ocr.Read(Input);
                text = (Result.Text).Split(new char[] { '\n', '\r' }).ToList();
            }
            List<List<string>> listafinal = new List<List<string>>();
            List<string> listaAux = new List<string>();
            foreach (var linea in text)
            {
                var lineaSplit = linea.Split(' ');
                if (linea.Contains("LEIDY"))
                {
                    var a = 1;
                }
                if (lineaSplit.Length > 6)
                {
                    if (int.TryParse(lineaSplit[0], out int result) && decimal.TryParse(lineaSplit[lineaSplit.Length - 1], out decimal resultD))
                    {

                        foreach (var splitted in lineaSplit.ToList())
                        {
                            listaAux.Add(splitted);
                        }
                        listafinal.Add(listaAux);
                        listaAux = new();
                    }
                }
            }
            List<KeyValuePair<string, string>> datosPago = new();
            foreach (var registro in listafinal)
            {
                datosPago.Add(new KeyValuePair<string, string>(registro[0], registro[6]));
            }

            return JsonConvert.SerializeObject(datosPago);
        }

        /// <summary>
        /// ConvertTxtToList
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static async Task<List<ArchivoAplicacionPagoTXTDto>> ConvertTxtToList(StreamReader stream)
        {
            List<ArchivoAplicacionPagoTXTDto> list = new List<ArchivoAplicacionPagoTXTDto>();
            List<ArchivoAplicacionPagoTXTDto> listFilter = new List<ArchivoAplicacionPagoTXTDto>();
            ArchivoAplicacionPagoTXTDto archivo = new ArchivoAplicacionPagoTXTDto();
            var including = new int[] { 1350, 1675 };

            do
            {
                string textLine = await stream.ReadLineAsync();
                var split = textLine.Split(';');

                archivo.Documento = split[5].ToString().TrimStart('0');
                archivo.Funcionario = split[6].ToString();
                archivo.ConceptoCod = int.Parse(split[7].ToString());
                archivo.Valor = decimal.Parse(split[8].ToString());

                list.Add(archivo);
                archivo = new ArchivoAplicacionPagoTXTDto();


            } while (stream.Peek() != -1);

            listFilter = list.Where(x => including.Contains(x.ConceptoCod)).ToList();

            return listFilter;
        }

        /// <summary>
        /// ConvertCSVToList
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static async Task<List<PagoMasivoNominaDto>> ConvertCSVToList(StreamReader stream)
        {
            TextReader tr = stream;
            var text = (await tr.ReadToEndAsync()).Split(new char[] { '\r', '\n' });
            var including = new string[] { "973D", "986V" };

            var texto = new List<string>();
            char caracterVacio = default;

            //text
            int counter = 0;
            foreach (var linea in text)
            {
                if (linea != string.Empty)
                {
                    if (counter == 0)
                    {
                        if(validateHeader(linea, caracterVacio) == null)
                        {
                            return null;
                        }
                    }
                    var nuevaLinea = linea.Replace('"', caracterVacio);
                    nuevaLinea = nuevaLinea.Replace("\0", "");
                    texto.Add(nuevaLinea);
                }
                counter++;
            }

            List<PagoMasivoNominaDto> listaMasivo = new List<PagoMasivoNominaDto>();
            List<PagoMasivoNominaDto> listaFilter = new List<PagoMasivoNominaDto>();
            texto.RemoveAt(0);
            foreach (var nomina in texto)
            {
                var nuevaNomina = nomina.Split(',');
                var listaNomina = nuevaNomina.ToList();
                listaNomina.RemoveAt(listaNomina.Count - 1);
                listaNomina.RemoveAt(listaNomina.Count - 1);
                var valorPago = string.Empty;
                for (int i = 7; i < listaNomina.Count; i++)
                {
                    valorPago += listaNomina[i];
                }


                listaMasivo.Add(new PagoMasivoNominaDto
                {
                    FUERZA = nuevaNomina[0],
                    TIPO_NOMINA = nuevaNomina[1],
                    CONCEPTO_COD = nuevaNomina[2],
                    CONCEPTO = nuevaNomina[3],
                    FUNCIONARIO = nuevaNomina[4],
                    CEDULA = nuevaNomina[5],
                    CODIGO_MILITAR = nuevaNomina[6],
                    VALOR = decimal.Parse(valorPago),
                    FECHA_DESDE = nuevaNomina.Length > 11 ? nuevaNomina[10] : nuevaNomina[9],
                    FECHA_HASTA = nuevaNomina.Length > 11 ? nuevaNomina[11] : nuevaNomina[10]
                }); ;
            }

            listaFilter = listaMasivo.Where(x => including.Contains(x.CONCEPTO_COD)).ToList();

            return listaFilter;
        }

        /// <summary>
        /// ConvertCSVToList
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static async Task<List<CargueArchivoListaInterna>> ConvertCSVToInternList(StreamReader stream)
        {
            TextReader tr = stream;
            var text = (await tr.ReadToEndAsync()).Split(new char[] { '\r', '\n' });
            var including = new string[] { "973D", "986V" };

            var texto = new List<string>();
            char caracterVacio = default;

            //text
            foreach (var linea in text)
            {
                if (linea != string.Empty)
                { 
                    var nuevaLinea = linea.Replace('"', caracterVacio);
                    nuevaLinea = nuevaLinea.Replace("\0", "");
                    texto.Add(nuevaLinea);
                }
            }

            List<CargueArchivoListaInterna> listaMasivo = new();
            List<CargueArchivoListaInterna> listaFilter = new();
            texto.RemoveAt(0);
            foreach (var nomina in texto)
            {
                var nuevaNomina = nomina.Split(',');
                var listaNomina = nuevaNomina.ToList();
                listaNomina.RemoveAt(listaNomina.Count - 1);
                listaNomina.RemoveAt(listaNomina.Count - 1);
                var valorPago = string.Empty;
                for (int i = 7; i < listaNomina.Count; i++)
                {
                    valorPago += listaNomina[i];
                }


                listaMasivo.Add(new CargueArchivoListaInterna
                {
                    FUERZA = nuevaNomina[0],
                    TIPO_NOMINA = nuevaNomina[1],
                    CONCEPTO_COD = nuevaNomina[2],
                    CONCEPTO = nuevaNomina[3],
                    FUNCIONARIO = nuevaNomina[4],
                    CEDULA = nuevaNomina[5],
                    CODIGO_MILITAR = nuevaNomina[6],
                    VALOR = decimal.Parse(valorPago, CultureInfo.InvariantCulture),
                    FECHA_DESDE = nuevaNomina[8],
                    FECHA_HASTA = nuevaNomina[9]
                });
            }

            listaFilter = listaMasivo.Where(x => including.Contains(x.CONCEPTO_COD)).ToList();

            return listaFilter;
        }

        /// <summary>
        /// Lee un archivo excel y lo regresa como un json
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static ArchivoAplicacionPagoExcelDto ConvertXLXtoList(StreamReader stream)
        {
            //Load your source workbook
            Workbook workbook = new Workbook(stream.BaseStream);

            MemoryStream ms = new MemoryStream();
            // convert the workbook to json file.
            workbook.Save(ms, SaveFormat.Json);
            return JsonConvert.DeserializeObject<ArchivoAplicacionPagoExcelDto>(Task.FromResult(Encoding.UTF8.GetString(ms.ToArray())).Result);
        }

        public static string validateHeader(string linea, char caracterVacio)
        {
            var nuevaLinea = linea.Replace('"', caracterVacio);
            nuevaLinea = nuevaLinea.Replace("\0", "");

            var headerArray = nuevaLinea.Split(',');

            if (!headerArray.ToList().Any(x => x == "Documento"))
                return null;

            if (!headerArray.ToList().Any(x => x.Contains("Concepto C")))
                return null;

            if (!headerArray.ToList().Any(x => x == "Valor"))
                return null;

            return nuevaLinea;
        }

    }
}