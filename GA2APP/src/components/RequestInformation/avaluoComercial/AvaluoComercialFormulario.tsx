import React, {
  useCallback,
  useEffect,
  useRef,
  useState,
} from "react";
import {
  Box,
  Button,
  CircularProgress,
  Typography,
} from "@material-ui/core";
import { useSelector } from "react-redux";
import swal from "sweetalert";
import ApiClients from "../../../services/api-clients.service";
import CreditApi from "../../../services/api-credit.service";
import CatalogApi from "../../../services/api-catalog.service";
import NotificationService from "../../../services/notification.service";
import {
  AvaluoComercialProps,
  FormErrors,
  FormValues,
} from "./types/avaluoComercial.types";
import useAvaluoComercialStyles from "./styles/avaluoComercial.styles";
import ResumenEjecutivo from "./componentes/ResumenEjecutivo";
import AntecedentesAvaluo from "./componentes/AntecedentesAvaluo";
import AspectosJuridicos from "./componentes/AspectosJuridicos";
import ObservacionesGenerales from "./componentes/ObservacionesGenerales";
import DependenciasInmueble from "./componentes/DependenciasInmueble";
import ConstruccionInmueble from "./componentes/ConstruccionInmueble";
import CuadroValoracion from "./componentes/CuadroValoracion";
import {
  initialValues,
  requiredFields,
} from "./constants/avaluoComercial.constants";

const apiCliente = ApiClients();
const creditApi = CreditApi();
const catalogApi = CatalogApi();

const construirNombreCompleto = (
  cliente: any
): string =>
  [
    cliente?.primerNombre,
    cliente?.segundoNombre,
    cliente?.primerApellido,
    cliente?.segundoApellido,
  ]
    .filter(Boolean)
    .join(" ")
    .trim();

const convertirFechaIso = (
  fecha: string
): string => {
  if (!fecha) {
    return "";
  }

  return `${fecha}T00:00:00`;
};

const convertirFechaInput = (
  fecha: string | null | undefined
): string => {
  if (!fecha) {
    return "";
  }

  return String(fecha).substring(0, 10);
};

const obtenerIdCatalogo = (
  item: any
): string =>
  String(
    item?.codigo ??
      item?.id ??
      item?.departamentoId ??
      item?.ciudadId ??
      item?.idDepartamento ??
      item?.idCiudad ??
      ""
  );

const obtenerNombreCatalogo = (
  item: any
): string =>
  item?.nombre ||
  item?.descripcion ||
  item?.label ||
  item?.nombreDepartamento ||
  item?.nombreCiudad ||
  "";

const convertirMonedaANumero = (
  value: string
): number => {
  const valorLimpio = String(value || "")
    .replace(/\$/g, "")
    .replace(/\s/g, "")
    .replace(/\./g, "")
    .replace(/,/g, "")
    .replace(/[^\d]/g, "");

  return Number(valorLimpio || 0);
};

const formatearMonedaColombiana = (
  value: string
): string => {
  const numero =
    convertirMonedaANumero(value);

  if (!numero) {
    return "";
  }

  return `Valor comercial estimado: $${numero.toLocaleString(
    "es-CO"
  )}`;
};

const obtenerValorGuardado = (
  nuevoValor: any,
  valorActual: string
): string => {
  if (
    nuevoValor === null ||
    nuevoValor === undefined ||
    String(nuevoValor).trim() === ""
  ) {
    return valorActual;
  }

  return String(nuevoValor);
};

const obtenerMensajeError = (
  error: any
): string =>
  error?.response?.data?.returnMessage ||
  error?.response?.data?.message ||
  error?.response?.data?.title ||
  error?.message ||
  "Ocurrió un error al guardar el avalúo comercial.";

const normalizarRespuesta = (
  response: any
): any => {
  if (
    response?.isSuccess !== undefined
  ) {
    return response;
  }

  return response?.data || response || {};
};

function AvaluoComercial({
  procedureRequest,
  isLoading,
}: AvaluoComercialProps) {
  const classes =
    useAvaluoComercialStyles();

  const catalogState = useSelector(
    (state: any) =>
      state?.catalog ||
      state?.catalogState ||
      {}
  );

  const [values, setValues] =
    useState<FormValues>(initialValues);

  const [errors, setErrors] =
    useState<FormErrors>({});

  const [
    cargandoCliente,
    setCargandoCliente,
  ] = useState(false);

  const [
    cargandoAvaluo,
    setCargandoAvaluo,
  ] = useState(false);

  const [guardando, setGuardando] =
    useState(false);

  const [
    ciudadesEscritura,
    setCiudadesEscritura,
  ] = useState<any[]>([]);

  const [
    cargandoCiudadesEscritura,
    setCargandoCiudadesEscritura,
  ] = useState(false);

  const ultimaConsultaRef = useRef("");

  const obtenerListaDepartamentos =
    useCallback(
      (): any[] =>
        catalogState?.departments || [],
      [catalogState?.departments]
    );

  const obtenerListaCiudades =
    useCallback(
      (): any[] =>
        catalogState?.City || [],
      [catalogState?.City]
    );

  const obtenerSocId = useCallback(
    (): string =>
      procedureRequest?.socId ||
      procedureRequest?.soC_ID ||
      procedureRequest?.socID ||
      procedureRequest?.requestID ||
      "",
    [procedureRequest]
  );

  const obtenerNombreDepartamentoPorId =
    useCallback(
      (departamentoId: any): string => {
        const departamento =
          obtenerListaDepartamentos().find(
            (item: any) =>
              Number(
                obtenerIdCatalogo(item)
              ) ===
              Number(departamentoId)
          );

        return obtenerNombreCatalogo(
          departamento
        );
      },
      [obtenerListaDepartamentos]
    );

  const obtenerNombreCiudadPorId =
    useCallback(
      (
        ciudadId: any,
        departamentoId: any
      ): string => {
        const ciudad =
          obtenerListaCiudades().find(
            (item: any) =>
              Number(
                obtenerIdCatalogo(item)
              ) ===
                Number(ciudadId) &&
              Number(
                item?.idDepartamento ??
                  item?.departamentoId
              ) ===
                Number(departamentoId)
          );

        return obtenerNombreCatalogo(
          ciudad
        );
      },
      [obtenerListaCiudades]
    );

  const obtenerNombreDepartamento =
    useCallback(
      (cliente: any): string =>
        obtenerNombreDepartamentoPorId(
          cliente?.idDepartamentoResidencia
        ),
      [obtenerNombreDepartamentoPorId]
    );

  const obtenerNombreCiudad =
    useCallback(
      (cliente: any): string =>
        obtenerNombreCiudadPorId(
          cliente?.idCiudadResidencia,
          cliente?.idDepartamentoResidencia
        ),
      [obtenerNombreCiudadPorId]
    );

  const handleChange = (
    field: keyof FormValues,
    value: string
  ) => {
    let nuevoValor = value;

    if (field === "notaria") {
      nuevoValor = String(value || "").replace(
        /\D/g,
        ""
      );
    }

    setValues((previousValues) => {
      const nuevosValores: FormValues = {
        ...previousValues,
        [field]: nuevoValor,
      };

      if (
        field === "garaje" &&
        String(nuevoValor || "").toUpperCase() !==
          "SI"
      ) {
        nuevosValores.descripcionGaraje = "";
      }

      return nuevosValores;
    });

    setErrors((previousErrors) => {
      const nuevosErrores: FormErrors = {
        ...previousErrors,
        [field]: "",
      };

      if (
        field === "garaje" &&
        String(nuevoValor || "").toUpperCase() !==
          "SI"
      ) {
        nuevosErrores.descripcionGaraje = "";
      }

      return nuevosErrores;
    });
  };

  const validate = (): boolean => {
    const newErrors: FormErrors = {};

    requiredFields.forEach((field) => {
      if (field === "descripcionGaraje") {
        return;
      }
    
      const fieldValue = values[field];
    
      const isEmpty =
        fieldValue === null ||
        fieldValue === undefined ||
        String(fieldValue).trim() === "";
    
      if (isEmpty) {
        newErrors[field] =
          "Este campo es obligatorio";
      }
    });

    if (!values.departamentoId) {
      newErrors.departamento =
        "No se encontró el departamento del cliente";
    }

    if (!values.ciudadId) {
      newErrors.ciudad =
        "No se encontró la ciudad del cliente";
    }

    if (
      !values.departamentoEscrituraId
    ) {
      newErrors.departamentoEscritura =
        "Debe seleccionar el departamento de la escritura";
    }

    if (!values.ciudadEscrituraId) {
      newErrors.ciudadEscritura =
        "Debe seleccionar la ciudad de la escritura";
    }

    if (
      convertirMonedaANumero(
        values.inmueble
      ) <= 0
    ) {
      newErrors.inmueble =
        "Debe ingresar un valor válido";
    }

    if (
      values.notaria &&
      !/^\d+$/.test(
        String(values.notaria)
      )
    ) {
      newErrors.notaria =
        "La notaría solamente permite números";
    }

    if (
      String(
        values.garaje || ""
      ).toUpperCase() === "SI" &&
      !String(
        values.descripcionGaraje || ""
      ).trim()
    ) {
      newErrors.descripcionGaraje =
        "Debe ingresar la descripción del garaje";
    }

    setErrors(newErrors);

    return (
      Object.keys(newErrors).length === 0
    );
  };

  const consultarCliente = useCallback(
    async (
      tipoDocumentoId: number,
      numeroDocumento: string
    ): Promise<void> => {
      try {
        setCargandoCliente(true);

        const response =
          await apiCliente.getClientByIdentificationTypeIdentificationNumberRequest(
            tipoDocumentoId,
            numeroDocumento
          );

        const cliente =
          response?.data || response || {};

        const nombreDepartamento =
          obtenerNombreDepartamento(cliente);

        const nombreCiudad =
          obtenerNombreCiudad(cliente);

        setValues(
          (previousValues) => ({
            ...previousValues,

            cliente:
              construirNombreCompleto(
                cliente
              ),

            cedulaCliente:
              cliente?.identificacion ||
              numeroDocumento,

            direccion:
              cliente?.direccion || "",

            sector:
              cliente?.sector ||
              previousValues.sector ||
              "",

            barrio:
              cliente?.barrio || "",

            departamento:
              nombreDepartamento,

            ciudad:
              nombreCiudad,

            departamentoId:
              cliente?.idDepartamentoResidencia
                ? String(
                    cliente.idDepartamentoResidencia
                  )
                : "",

            ciudadId:
              cliente?.idCiudadResidencia
                ? String(
                    cliente.idCiudadResidencia
                  )
                : "",
          })
        );
      } catch (error) {
        setValues(
          (previousValues) => ({
            ...previousValues,
            cliente: "",
            cedulaCliente:
              numeroDocumento,
            direccion: "",
            sector:
              previousValues.sector ||
              "",
            barrio: "",
            departamento: "",
            ciudad: "",
            departamentoId: "",
            ciudadId: "",
          })
        );
      } finally {
        setCargandoCliente(false);
      }
    },
    [
      obtenerNombreCiudad,
      obtenerNombreDepartamento,
    ]
  );

  const obtenerCiudadesDepartamento =
    useCallback(
      async (
        departamentoId: string
      ): Promise<any[]> => {
        if (!departamentoId) {
          return [];
        }

        const response =
          await catalogApi.getCitiesByDepartment(
            Number(departamentoId)
          );

        const responseNormalizada =
          normalizarRespuesta(response);

        const ciudadesResponse =
          responseNormalizada?.data ||
          responseNormalizada?.result ||
          responseNormalizada ||
          [];

        return Array.isArray(
          ciudadesResponse
        )
          ? ciudadesResponse
          : [];
      },
      []
    );

  const consultarCiudadesEscritura =
    async (
      departamentoId: string
    ): Promise<void> => {
      setCiudadesEscritura([]);

      if (!departamentoId) {
        return;
      }

      try {
        setCargandoCiudadesEscritura(
          true
        );

        const listaCiudades =
          await obtenerCiudadesDepartamento(
            departamentoId
          );

        setCiudadesEscritura(
          listaCiudades
        );

        if (
          listaCiudades.length === 1
        ) {
          const unicaCiudad =
            listaCiudades[0];

          setValues(
            (previousValues) => ({
              ...previousValues,

              ciudadEscrituraId:
                obtenerIdCatalogo(
                  unicaCiudad
                ),

              ciudadEscritura:
                obtenerNombreCatalogo(
                  unicaCiudad
                ),
            })
          );

          setErrors(
            (previousErrors) => ({
              ...previousErrors,
              ciudadEscritura: "",
            })
          );
        }
      } catch (error) {
        setCiudadesEscritura([]);

        setValues(
          (previousValues) => ({
            ...previousValues,
            ciudadEscrituraId: "",
            ciudadEscritura: "",
          })
        );

        await NotificationService.genericAlert(
          "Atención",
          "No fue posible consultar las ciudades del departamento seleccionado.",
          "warning"
        );
      } finally {
        setCargandoCiudadesEscritura(
          false
        );
      }
    };

  const handleDepartamentoEscrituraChange =
    async (
      departamento: any | null
    ) => {
      const departamentoId =
        departamento
          ? obtenerIdCatalogo(
              departamento
            )
          : "";

      const departamentoNombre =
        departamento
          ? obtenerNombreCatalogo(
              departamento
            )
          : "";

      setValues(
        (previousValues) => ({
          ...previousValues,

          departamentoEscrituraId:
            departamentoId,

          departamentoEscritura:
            departamentoNombre,

          ciudadEscrituraId: "",

          ciudadEscritura: "",
        })
      );

      setErrors(
        (previousErrors) => ({
          ...previousErrors,
          departamentoEscritura: "",
          ciudadEscritura: "",
        })
      );

      setCiudadesEscritura([]);

      if (!departamentoId) {
        return;
      }

      await consultarCiudadesEscritura(
        departamentoId
      );
    };

  const handleCiudadEscrituraChange = (
    ciudad: any | null
  ) => {
    setValues(
      (previousValues) => ({
        ...previousValues,

        ciudadEscrituraId: ciudad
          ? obtenerIdCatalogo(ciudad)
          : "",

        ciudadEscritura: ciudad
          ? obtenerNombreCatalogo(ciudad)
          : "",
      })
    );

    setErrors(
      (previousErrors) => ({
        ...previousErrors,
        ciudadEscritura: "",
      })
    );
  };

  const obtenerDatosAvaluoComercial =
    useCallback(
      async (): Promise<void> => {
        const socId = obtenerSocId();

        if (!socId) {
          return;
        }

        try {
          setCargandoAvaluo(true);

          const response =
            await creditApi.getConsultarAvaluoComercialCredito(
              socId
            );

          const responseNormalizada =
            normalizarRespuesta(response);

          if (
            responseNormalizada
              ?.isSuccess !== true ||
            !responseNormalizada?.data
          ) {
            /*
             * Si todavía no existe avalúo,
             * se conservan los datos consultados
             * desde el cliente.
             */
            return;
          }

          const avaluo =
            responseNormalizada.data;

          const resumen =
            avaluo?.resumenEjecutivo ||
            {};

          const antecedentes =
            avaluo?.antecedentes || {};

          const aspectosJuridicos =
            avaluo?.aspectosJuridicos ||
            {};

          const observaciones =
            avaluo
              ?.observacionesGenerales ||
            {};

          const dependencias =
            avaluo?.dependencias || {};

          const construccion =
            avaluo?.construccion || {};

          const cuadroValoracion =
            avaluo?.cuadroValoracion ||
            {};

          const departamentoEscrituraId =
            aspectosJuridicos
              ?.departamentoEscrituraId;

          const ciudadEscrituraId =
            aspectosJuridicos
              ?.ciudadEscrituraId;

          let nombreCiudadEscritura = "";

          if (
            departamentoEscrituraId
          ) {
            const listaCiudades =
              await obtenerCiudadesDepartamento(
                String(
                  departamentoEscrituraId
                )
              );

            setCiudadesEscritura(
              listaCiudades
            );

            const ciudadEncontrada =
              listaCiudades.find(
                (item: any) =>
                  Number(
                    obtenerIdCatalogo(
                      item
                    )
                  ) ===
                  Number(
                    ciudadEscrituraId
                  )
              );

            nombreCiudadEscritura =
              obtenerNombreCatalogo(
                ciudadEncontrada
              );
          } else {
            setCiudadesEscritura([]);
          }

          const nombreDepartamentoCliente =
            obtenerNombreDepartamentoPorId(
              resumen?.departamentoId
            );

          const nombreCiudadCliente =
            obtenerNombreCiudadPorId(
              resumen?.ciudadId,
              resumen?.departamentoId
            );

          const nombreDepartamentoEscritura =
            obtenerNombreDepartamentoPorId(
              departamentoEscrituraId
            );

          setValues(
            (previousValues) => ({
              ...previousValues,

              /*
               * Resumen ejecutivo.
               * Si el GET trae el dato,
               * tiene prioridad.
               */
              cliente:
                obtenerValorGuardado(
                  resumen?.cliente,
                  previousValues.cliente
                ),

              cedulaCliente:
                obtenerValorGuardado(
                  resumen?.numeroDocumento,
                  previousValues
                    .cedulaCliente
                ),

              direccion:
                obtenerValorGuardado(
                  resumen?.direccion,
                  previousValues.direccion
                ),

              sector:
                obtenerValorGuardado(
                  resumen?.sector,
                  previousValues.sector
                ),

              barrio:
                obtenerValorGuardado(
                  resumen?.barrio,
                  previousValues.barrio
                ),

              departamentoId:
                obtenerValorGuardado(
                  resumen?.departamentoId,
                  previousValues
                    .departamentoId
                ),

              departamento:
                obtenerValorGuardado(
                  nombreDepartamentoCliente,
                  previousValues
                    .departamento
                ),

              ciudadId:
                obtenerValorGuardado(
                  resumen?.ciudadId,
                  previousValues.ciudadId
                ),

              ciudad:
                obtenerValorGuardado(
                  nombreCiudadCliente,
                  previousValues.ciudad
                ),

              /*
               * Antecedentes.
               */
              fechaInforme:
                obtenerValorGuardado(
                  convertirFechaInput(
                    antecedentes
                      ?.fechaInforme
                  ),
                  previousValues
                    .fechaInforme
                ),

              vetustez:
                obtenerValorGuardado(
                  antecedentes?.vetustez,
                  previousValues.vetustez
                ),

              fechaPagoAvaluo:
                obtenerValorGuardado(
                  convertirFechaInput(
                    antecedentes
                      ?.fechaPagoAvaluo
                  ),
                  previousValues
                    .fechaPagoAvaluo
                ),

              fechaVisita:
                obtenerValorGuardado(
                  convertirFechaInput(
                    antecedentes
                      ?.fechaVisita
                  ),
                  previousValues
                    .fechaVisita
                ),

              /*
               * Aspectos jurídicos.
               */
              propietario:
                obtenerValorGuardado(
                  aspectosJuridicos
                    ?.propietario,
                  previousValues
                    .propietario
                ),

              escritura:
                obtenerValorGuardado(
                  aspectosJuridicos
                    ?.escritura,
                  previousValues.escritura
                ),

              notaria:
                obtenerValorGuardado(
                  aspectosJuridicos
                    ?.notaria,
                  previousValues.notaria
                ),

              fechaEscritura:
                obtenerValorGuardado(
                  convertirFechaInput(
                    aspectosJuridicos
                      ?.fechaEscritura
                  ),
                  previousValues
                    .fechaEscritura
                ),

              departamentoEscrituraId:
                obtenerValorGuardado(
                  departamentoEscrituraId,
                  previousValues
                    .departamentoEscrituraId
                ),

              departamentoEscritura:
                obtenerValorGuardado(
                  nombreDepartamentoEscritura,
                  previousValues
                    .departamentoEscritura
                ),

              ciudadEscrituraId:
                obtenerValorGuardado(
                  ciudadEscrituraId,
                  previousValues
                    .ciudadEscrituraId
                ),

              ciudadEscritura:
                obtenerValorGuardado(
                  nombreCiudadEscritura,
                  previousValues
                    .ciudadEscritura
                ),

              cedulaCatastral:
                obtenerValorGuardado(
                  aspectosJuridicos
                    ?.cedulaCatastral,
                  previousValues
                    .cedulaCatastral
                ),

              licenciaConstruccion:
                obtenerValorGuardado(
                  aspectosJuridicos
                    ?.licenciaConstruccion,
                  previousValues
                    .licenciaConstruccion
                ),

              nombreConjunto:
                obtenerValorGuardado(
                  aspectosJuridicos
                    ?.nombreConjunto,
                  previousValues
                    .nombreConjunto
                ),

              coeficienteCopropiedad:
                obtenerValorGuardado(
                  aspectosJuridicos
                    ?.coeficienteCopropiedad,
                  previousValues
                    .coeficienteCopropiedad
                ),

              /*
               * Observaciones generales.
               */
              distribucionInmueble:
                obtenerValorGuardado(
                  observaciones
                    ?.distribucionInmueble,
                  previousValues
                    .distribucionInmueble
                ),

              caracteristicasAdicionales:
                obtenerValorGuardado(
                  observaciones
                    ?.caracteristicasAdicionales,
                  previousValues
                    .caracteristicasAdicionales
                ),

              /*
               * Dependencias.
               */
              descripcion:
                obtenerValorGuardado(
                  dependencias
                    ?.descripcion,
                  previousValues
                    .descripcion
                ),

              garaje:
                obtenerValorGuardado(
                  dependencias?.garaje,
                  previousValues.garaje
                ),

              descripcionGaraje:
                obtenerValorGuardado(
                  dependencias
                    ?.descripcionGaraje,
                  previousValues
                    .descripcionGaraje
                ),

              /*
               * Construcción.
               */
              salaAreaConstruida:
                obtenerValorGuardado(
                  construccion
                    ?.salaAreaConstruida,
                  previousValues
                    .salaAreaConstruida
                ),

              areaPrivada:
                obtenerValorGuardado(
                  construccion
                    ?.areaPrivada,
                  previousValues
                    .areaPrivada
                ),

              /*
               * El GET devuelve:
               * "Valor comercial estimado: $420.000.000"
               *
               * El campo currencyMask recibe:
               * "420000000"
               */
              inmueble:
                cuadroValoracion
                  ?.inmueble
                  ? String(
                      convertirMonedaANumero(
                        cuadroValoracion
                          .inmueble
                      )
                    )
                  : previousValues
                      .inmueble,
            })
          );

          setErrors({});
        } catch (error) {
          await NotificationService.genericAlert(
            "Atención",
            "No fue posible consultar la información guardada del avalúo comercial.",
            "warning"
          );
        } finally {
          setCargandoAvaluo(false);
        }
      },
      [
        obtenerCiudadesDepartamento,
        obtenerNombreCiudadPorId,
        obtenerNombreDepartamentoPorId,
        obtenerSocId,
      ]
    );

  useEffect(() => {
    const tipoDocumentoId =
      Number(
        procedureRequest?.tipoDocumentoId
      ) || 1;

    const numeroDocumento =
      procedureRequest
        ?.clientIdentificationNumber ||
      procedureRequest?.numeroDocumento;

    const socId = obtenerSocId();

    if (
      !numeroDocumento ||
      !socId
    ) {
      return;
    }

    const departamentos =
      obtenerListaDepartamentos();

    const ciudades =
      obtenerListaCiudades();

    const claveConsulta = [
      tipoDocumentoId,
      numeroDocumento,
      socId,
      departamentos.length,
      ciudades.length,
    ].join("-");

    if (
      ultimaConsultaRef.current ===
      claveConsulta
    ) {
      return;
    }

    ultimaConsultaRef.current =
      claveConsulta;

    const cargarInformacionFormulario =
      async (): Promise<void> => {
      
        await consultarCliente(
          tipoDocumentoId,
          String(numeroDocumento)
        );

      
        await obtenerDatosAvaluoComercial();
      };

    cargarInformacionFormulario().catch(
      () => {
        /*
         * Cada consulta maneja internamente
         * sus errores.
         */
      }
    );
  }, [
    procedureRequest?.tipoDocumentoId,
    procedureRequest
      ?.clientIdentificationNumber,
    procedureRequest?.numeroDocumento,
    procedureRequest?.requestID,
    procedureRequest?.socId,
    catalogState?.departments,
    catalogState?.City,
    consultarCliente,
    obtenerDatosAvaluoComercial,
    obtenerListaCiudades,
    obtenerListaDepartamentos,
    obtenerSocId,
  ]);

  const handleSave = async () => {
    if (guardando) {
      return;
    }

    if (!validate()) {
      await NotificationService.genericAlert(
        "Campos pendientes",
        "Debe completar todos los campos obligatorios antes de guardar el avalúo comercial.",
        "warning"
      );

      return;
    }

    const socId = obtenerSocId();

    if (!socId) {
      await NotificationService.genericAlert(
        "No fue posible guardar",
        "No se encontró el identificador de la solicitud.",
        "error"
      );

      return;
    }

    const confirmarGuardado = await swal({
      title:
        "Guardar avalúo comercial",
      text:
        "¿Está seguro de guardar la información del avalúo comercial?",
      icon: "warning",
      buttons: {
        cancel: {
          text: "Cancelar",
          value: false,
          visible: true,
          closeModal: true,
        },
        confirm: {
          text: "Sí, guardar",
          value: true,
          visible: true,
          closeModal: true,
        },
      },
      dangerMode: false,
    });

    if (!confirmarGuardado) {
      return;
    }

    const request = {
      socId,

      resumenEjecutivo: {
        cliente:
          values.cliente,

        numeroDocumento:
          values.cedulaCliente,

        direccion:
          values.direccion,

        sector:
          values.sector,

        barrio:
          values.barrio,

        departamentoId:
          Number(
            values.departamentoId
          ),

        ciudadId:
          Number(values.ciudadId),
      },

      antecedentes: {
        fechaInforme:
          convertirFechaIso(
            values.fechaInforme
          ),

        vetustez:
          values.vetustez,

        fechaPagoAvaluo:
          convertirFechaIso(
            values.fechaPagoAvaluo
          ),

        fechaVisita:
          convertirFechaIso(
            values.fechaVisita
          ),
      },

      aspectosJuridicos: {
        propietario:
          values.propietario,

        escritura:
          values.escritura,

        notaria:
          values.notaria,

        fechaEscritura:
          convertirFechaIso(
            values.fechaEscritura
          ),

        departamentoEscrituraId:
          Number(
            values.departamentoEscrituraId
          ),

        ciudadEscrituraId:
          Number(
            values.ciudadEscrituraId
          ),

        cedulaCatastral:
          values.cedulaCatastral,

        licenciaConstruccion:
          values.licenciaConstruccion,

        nombreConjunto:
          values.nombreConjunto,

        coeficienteCopropiedad:
          values.coeficienteCopropiedad,
      },

      observacionesGenerales: {
        distribucionInmueble:
          values.distribucionInmueble,

        caracteristicasAdicionales:
          values.caracteristicasAdicionales,
      },

      dependencias: {
        descripcion:
          values.descripcion,

        garaje:
          values.garaje,

        descripcionGaraje:
          String(
            values.garaje || ""
          ).toUpperCase() === "SI"
            ? values.descripcionGaraje
            : "",
      },

      construccion: {
        salaAreaConstruida:
          values.salaAreaConstruida,

        areaPrivada:
          values.areaPrivada,
      },

      cuadroValoracion: {
        inmueble:
          formatearMonedaColombiana(
            values.inmueble
          ),
      },
    };

    try {
      setGuardando(true);

      const response =
        await creditApi.insertarActualizarAvaluoComercialCredito(
          request
        );

      const responseNormalizada =
        normalizarRespuesta(response);

      if (
        responseNormalizada
          ?.isSuccess !== true
      ) {
        await NotificationService.genericAlert(
          "No fue posible guardar",
          responseNormalizada
            ?.returnMessage ||
            "No fue posible guardar el avalúo comercial.",
          "error"
        );

        return;
      }

      await NotificationService.genericAlert(
        "Avalúo guardado",
        responseNormalizada
          ?.returnMessage ||
          "La información del avalúo comercial fue guardada correctamente.",
        "success"
      );

      /*
       * Se vuelve a consultar para cargar
       * exactamente lo almacenado en backend.
       */
      await obtenerDatosAvaluoComercial();
    } catch (error) {
      await NotificationService.genericAlert(
        "Error al guardar",
        obtenerMensajeError(error),
        "error"
      );
    } finally {
      setGuardando(false);
    }
  };

  if (
    isLoading ||
    cargandoAvaluo
  ) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        p={4}
      >
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box className={classes.root}>
      <Box className={classes.header}>
        AVALÚO COMERCIAL
      </Box>

      <Typography
        className={classes.subtitle}
      >
        Resumen Ejecutivo
      </Typography>

      <Box px={2}>
        <ResumenEjecutivo
          values={values}
          errors={errors}
          cargandoCliente={
            cargandoCliente
          }
          onChange={handleChange}
        />

        <AntecedentesAvaluo
          values={values}
          errors={errors}
          onChange={handleChange}
        />

        <AspectosJuridicos
          values={values}
          errors={errors}
          departamentos={
            obtenerListaDepartamentos()
          }
          ciudadesEscritura={
            ciudadesEscritura
          }
          cargandoCiudadesEscritura={
            cargandoCiudadesEscritura
          }
          onChange={handleChange}
          onDepartamentoChange={
            handleDepartamentoEscrituraChange
          }
          onCiudadChange={
            handleCiudadEscrituraChange
          }
        />

        <ObservacionesGenerales
          values={values}
          errors={errors}
          onChange={handleChange}
        />

        <DependenciasInmueble
          values={values}
          errors={errors}
          onChange={handleChange}
        />

        <ConstruccionInmueble
          values={values}
          errors={errors}
          onChange={handleChange}
        />

        <CuadroValoracion
          values={values}
          errors={errors}
          onChange={handleChange}
        />

        <Box
          className={classes.actions}
        >
          <Button
            className={
              classes.saveButton
            }
            variant="contained"
            onClick={handleSave}
            disabled={
              guardando ||
              cargandoCliente ||
              cargandoAvaluo ||
              cargandoCiudadesEscritura
            }
          >
            {guardando
              ? "Guardando..."
              : "Guardar"}
          </Button>
        </Box>
      </Box>
    </Box>
  );
}

export default AvaluoComercial;