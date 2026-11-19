import {
    createStyles,
    Grid,
    makeStyles,
} from '@material-ui/core';
import React, {
    useEffect,
    useMemo,
} from 'react';
import {
    Controller,
    UseFormReturn,
} from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import UtilService from '../../../utils/util.service';
import type { SolicitudSubsidioResolucion } from './ResolucionSubsidios';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        mensajeSinResultados: {
            padding: '20px',
            textAlign: 'center',
        },
    }),
);

type CuadroConsolidado = {
    cuadro: string;
    tipoVivienda: string;
    monto: number;
    cantidad: number;
};

const normalizarTexto = (
    valor: unknown,
): string =>
    String(valor ?? '')
        .trim()
        .toUpperCase()
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '');

const convertirNumero = (
    valor: unknown,
): number => {
    if (
        valor === null ||
        valor === undefined ||
        valor === ''
    ) {
        return 0;
    }

    if (typeof valor === 'number') {
        return Number.isFinite(valor)
            ? valor
            : 0;
    }

    let valorLimpio = String(valor)
        .trim()
        .replace(/[^\d,.-]/g, '');

    if (!valorLimpio) {
        return 0;
    }

    const ultimaComa =
        valorLimpio.lastIndexOf(',');

    const ultimoPunto =
        valorLimpio.lastIndexOf('.');

    if (ultimaComa > ultimoPunto) {
        valorLimpio = valorLimpio
            .replace(/\./g, '')
            .replace(',', '.');
    } else {
        valorLimpio =
            valorLimpio.replace(/,/g, '');
    }

    const numero = Number(valorLimpio);

    return Number.isFinite(numero)
        ? numero
        : 0;
};

const esViviendaNueva = (
    subsidio: SolicitudSubsidioResolucion,
): boolean => {
    const modalidad = normalizarTexto(
        subsidio?.modalidadDescripcion,
    );

    return modalidad.includes(
        'VIVIENDA NUEVA',
    );
};

const esViviendaUsada = (
    subsidio: SolicitudSubsidioResolucion,
): boolean => {
    const modalidad = normalizarTexto(
        subsidio?.modalidadDescripcion,
    );

    return modalidad.includes(
        'VIVIENDA USADA',
    );
};

const calcularMonto = (
    subsidios: SolicitudSubsidioResolucion[],
): number =>
    subsidios.reduce(
        (total, subsidio) =>
            total +
            convertirNumero(
                subsidio?.valorRetirar,
            ),
        0,
    );

const CuadrosAfectados = ({
    formularioCuadrosAfectados,
    subsidios = [],
}: Props) => {
    const classes = useStyles();

    const cuadrosConsolidados =
        useMemo<CuadroConsolidado[]>(() => {
            const subsidiosViviendaNueva =
                subsidios.filter(
                    esViviendaNueva,
                );

            const subsidiosViviendaUsada =
                subsidios.filter(
                    esViviendaUsada,
                );

            return [
                {
                    cuadro: '6181',
                    tipoVivienda:
                        'Vivienda nueva',
                    monto: calcularMonto(
                        subsidiosViviendaNueva,
                    ),
                    cantidad:
                        subsidiosViviendaNueva.length,
                },
                {
                    cuadro: '6181',
                    tipoVivienda:
                        'Vivienda usada',
                    monto: calcularMonto(
                        subsidiosViviendaUsada,
                    ),
                    cantidad:
                        subsidiosViviendaUsada.length,
                },
            ];
        }, [subsidios]);

    const totalSubsidios = useMemo(
        () =>
            cuadrosConsolidados.reduce(
                (total, cuadro) =>
                    total + cuadro.cantidad,
                0,
            ),
        [cuadrosConsolidados],
    );

    const totalMonto = useMemo(
        () =>
            cuadrosConsolidados.reduce(
                (total, cuadro) =>
                    total + cuadro.monto,
                0,
            ),
        [cuadrosConsolidados],
    );

    useEffect(() => {
        formularioCuadrosAfectados.setValue(
            'totalSubsidios',
            totalSubsidios,
        );

        formularioCuadrosAfectados.setValue(
            'totalMonto',
            totalMonto,
        );
    }, [
        formularioCuadrosAfectados,
        totalSubsidios,
        totalMonto,
    ]);

    return (
        <CHPaper title="Cuadros Consolidados Afectados">
            <Grid container>
                <table
                    className="credit-table"
                    style={{
                        margin: '10px',
                        width: '99%',
                    }}
                >
                    <thead>
                        <tr>
                            <th
                                style={{
                                    textAlign:
                                        'center',
                                    width: '20%',
                                }}
                            >
                                Cuadro
                            </th>

                            <th
                                style={{
                                    textAlign:
                                        'center',
                                }}
                            >
                                Tipo de Vivienda
                            </th>

                            <th
                                style={{
                                    textAlign:
                                        'center',
                                }}
                            >
                                Monto
                            </th>

                            <th
                                style={{
                                    textAlign:
                                        'center',
                                    width: '20%',
                                }}
                            >
                                Cantidad Subsidios
                            </th>
                        </tr>
                    </thead>

                    <tbody>
                        {cuadrosConsolidados.map(
                            (row) => (
                                <tr
                                    key={
                                        row.tipoVivienda
                                    }
                                    style={{
                                        textAlign:
                                            'center',
                                    }}
                                >
                                    <td>
                                        {row.cuadro}
                                    </td>

                                    <td>
                                        {
                                            row.tipoVivienda
                                        }
                                    </td>

                                    <td>
                                        {UtilService.formatCurrency(
                                            row.monto,
                                        )}
                                    </td>

                                    <td>
                                        {row.cantidad}
                                    </td>
                                </tr>
                            ),
                        )}
                    </tbody>
                </table>
            </Grid>

            <Grid container>
                <Grid
                    className={classes.column}
                    item
                    xs={12}
                    md={3}
                >
                    <Controller
                        name="totalSubsidios"
                        control={
                            formularioCuadrosAfectados.control
                        }
                        render={({
                            field: {
                                onChange,
                                value,
                            },
                        }) => (
                            <CHTextField
                                isReadOnly
                                value={value}
                                onChange={
                                    onChange
                                }
                                label="Total Subsidios"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
            </Grid>

            <Grid container>
                <Grid
                    className={classes.column}
                    item
                    xs={12}
                    md={3}
                >
                    <Controller
                        name="totalMonto"
                        control={
                            formularioCuadrosAfectados.control
                        }
                        render={({
                            field: {
                                onChange,
                                value,
                            },
                        }) => (
                            <CHTextField
                                isReadOnly
                                currencyMask
                                value={value}
                                onChange={
                                    onChange
                                }
                                label="Total Monto"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
            </Grid>

            <Grid container>
                <Grid
                    className={classes.column}
                    item
                    xs={12}
                    md={3}
                >
                    <Controller
                        name="fechaAdjudicacion"
                        control={
                            formularioCuadrosAfectados.control
                        }
                        render={({
                            field: {
                                onChange,
                                value,
                            },
                        }) => (
                            <CHTextField
                                isReadOnly
                                value={value}
                                onChange={
                                    onChange
                                }
                                label="Fecha Adjudicación"
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
            </Grid>
        </CHPaper>
    );
};

type Props = {
    formularioCuadrosAfectados:
    UseFormReturn<any>;
    subsidios?:
    SolicitudSubsidioResolucion[];
};

export default CuadrosAfectados;