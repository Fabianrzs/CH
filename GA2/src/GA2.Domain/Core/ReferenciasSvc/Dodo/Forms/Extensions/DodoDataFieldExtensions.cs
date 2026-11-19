using GA2.Domain.Core.Dodo.Common;
using GA2.Domain.Core.Dodo.Forms.Constants;
using System.Collections.Generic;

namespace GA2.Domain.Core.Dodo.Forms.Extensions
{
    public static class DodoDataFieldExtensions
    {
        public static List<DodoDataField> Add(
            this List<DodoDataField> list,
            string field,
            string value)
        {
            list.Add(new DodoDataField { Field = field, Value = value });
            return list;
        }

        // --- Formulario 361 ---
        public static List<DodoDataField> AddFlujo361Data(
            this List<DodoDataField> list,
            string puntoAtencion,
            string medioRecepcion,
            string tipoCredito,
            string medioCreditoVivienda,
            string numFolios,
            string tipoDocumento,
            string numeroDocumento,
            string nombreCliente,
            string unidadEjecutora,
            string categoria,
            string departamento,
            string ciudad,
            string direccion,
            string telefono,
            string correo)
        {
            return list
                .Add(Fields361.PuntoAtencion, puntoAtencion)
                .Add(Fields361.MedioRecepcion, medioRecepcion)
                .Add(Fields361.TipoSolicitud, $"SOLICITUD {tipoCredito}")
                .Add(Fields361.TipoProspeccion, $"PROSPECCIÓN {tipoCredito}")
                .Add(Fields361.MedioCreditoVivienda, medioCreditoVivienda)
                .Add(Fields361.NumFolios, numFolios)
                .Add(Fields361.TipoDocumento, tipoDocumento)
                .Add(Fields361.NumeroDocumento, numeroDocumento)
                .Add(Fields361.NombreCliente, nombreCliente)
                .Add(Fields361.UnidadEjecutora, unidadEjecutora)
                .Add(Fields361.Categoria, categoria)
                .Add(Fields361.DocumentoNombre, $"{numeroDocumento} | {nombreCliente}")
                .Add(Fields361.Departamento, departamento)
                .Add(Fields361.Ciudad, ciudad)
                .Add(Fields361.Direccion, direccion)
                .Add(Fields361.Telefono, telefono)
                .Add(Fields361.Correo, correo);
        }

        // --- Formulario 02 ---
        public static List<DodoDataField> AddFlujo02Data(
            this List<DodoDataField> list,
            string identificacion,
            string nombre,
            string tipoDocumento,
            string direccion,
            string telefonoFijo,
            string telefonoCelular,
            string correo,
            string unidadEjecutora,
            string gradoCategoria,
            string barrio,
            string departamento,
            string ciudad)
        {
            return list
                .Add(Fields02.Titulo, $"Expediente Padre: {identificacion} {nombre}")
                .Add(Fields02.Identificacion, identificacion)
                .Add(Fields02.Nombre, nombre)
                .Add(Fields02.TipoDocumento, tipoDocumento)
                .Add(Fields02.Direccion, direccion)
                .Add(Fields02.TelefonoFijo, telefonoFijo)
                .Add(Fields02.TelefonoCelular, telefonoCelular)
                .Add(Fields02.Correo, correo)
                .Add(Fields02.UnidadEjecutora, unidadEjecutora)
                .Add(Fields02.GradoCategoria, gradoCategoria)
                .Add(Fields02.DireccionAlt, direccion)
                .Add(Fields02.Barrio, barrio)
                .Add(Fields02.Pais, "Colombia")
                .Add(Fields02.Departamento, departamento)
                .Add(Fields02.Ciudad, ciudad);
        }

        // --- Formulario 219 ---
        public static List<DodoDataField> AddFlujo219Data(
            this List<DodoDataField> list,
            string numeroDocumento,
            string nombreCompleto,
            string fuerza,
            string fechaAfiliacion,
            string categoria,
            string departamento,
            string direccion,
            string correo,
            string telefono,
            string tipoDocumento,
            string fechaExpedicion,
            string ciudadExpedicion,
            string fechaNacimiento)
        {
            return list
                .Add(Fields219.NumeroDocumento, numeroDocumento)
                .Add(Fields219.NombreCompleto, nombreCompleto)
                .Add(Fields219.Fuerza, fuerza)
                .Add(Fields219.FechaAfiliacion, fechaAfiliacion)
                .Add(Fields219.Categoria, categoria)
                .Add(Fields219.Departamento, departamento)
                .Add(Fields219.Ciudad, "BOGOTA")
                .Add(Fields219.Direccion, direccion)
                .Add(Fields219.Correo, correo)
                .Add(Fields219.Telefono, telefono)
                .Add(Fields219.TipoDocumento, tipoDocumento)
                .Add(Fields219.FechaExpedicion, fechaExpedicion)
                .Add(Fields219.CiudadExpedicion, ciudadExpedicion)
                .Add(Fields219.FechaNacimiento, fechaNacimiento);
        }

        // --- Formulario 83 ---
        public static List<DodoDataField> AddFlujo83Data(
            this List<DodoDataField> list,
            string identificacion,
            string nombreCompleto,
            string tipoDocumento,
            string correo,
            string prestamo,
            string plazo,
            string valorCuota,
            string tipoCredito)
        {
            return list
                .Add(Fields83.DocumentoNombre, $"{identificacion} | {nombreCompleto}")
                .Add(Fields83.Dependencia, "Grupo Originación de Crédito")
                .Add(Fields83.TipoComunicacion, "Comunicación oficial")
                .Add(Fields83.TipoDocumento, tipoDocumento)
                .Add(Fields83.Identificacion, identificacion)
                .Add(Fields83.NombreCompleto, nombreCompleto)
                .Add(Fields83.Correo, correo)
                .Add(Fields83.TipoAfiliado, "AFILIADO")
                .Add(Fields83.Cargo, "Analista")
                .Add(Fields83.ValorPrestamo, prestamo)
                .Add(Fields83.Plazo, plazo)
                .Add(Fields83.ValorCuota, valorCuota)
                .Add(Fields83.GrupoDestino, "Grupo Originación de Crédito")
                .Add(Fields83.TipoCredito, tipoCredito)
                .Add(Fields83.Aprobado, "NO")
                .Add(Fields83.Contador, "0");
        }

        // --- Formulario 133 ---
        public static List<DodoDataField> AddFlujo133Data(
            this List<DodoDataField> list,
            string numeroComite,
            string fechaComite,
            string tema1Desc, string tema1Obs,
            string tema2Desc, string tema2Obs,
            string tema3Desc, string tema3Obs,
            bool requiereFirmaGerencia,
            string archivoAdjunto)
        {
            return list
                .Add(Fields133.NumeroComite, numeroComite)
                .Add(Fields133.FechaComite, fechaComite)
                .Add(Fields133.Temas, "TEMAS")
                .Add(Fields133.Tema1Descripcion, tema1Desc)
                .Add(Fields133.Tema1Observacion, tema1Obs)
                .Add(Fields133.Tema2Descripcion, tema2Desc)
                .Add(Fields133.Tema2Observacion, tema2Obs)
                .Add(Fields133.Tema3Descripcion, tema3Desc)
                .Add(Fields133.Tema3Observacion, tema3Obs)
                .Add(Fields133.RequiereFirmaGerencia, $"{requiereFirmaGerencia}")
                .Add(Fields133.ArchivoAdjunto, archivoAdjunto);
        }
    }
}