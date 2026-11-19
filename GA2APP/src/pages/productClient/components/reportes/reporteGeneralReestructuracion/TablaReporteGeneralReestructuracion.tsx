import React, { useEffect, useState } from 'react';
import moment from 'moment';
import MUIDataTable from 'mui-datatables';
import { toast } from 'react-toastify';
import UtilService from '../../../../../utils/util.service';
import { ColumnFormatType } from '../../../../../enums/ColumnFormatType';
import handleExportToExcel from '../../../../Reports/helpers/handleExportToExcel';
import DownloadToolbar from '../componentes/DownloadToolbar';

import CatalogApi from '../../../../../services/api-catalog.service';

const TablaReporteGeneralReestructuracion = ({data, periodo}: { data: any[], periodo?: Date | null }) => {

    const defaultCellOptions = {
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
            },
        }),
    };

    const defaultCellDateOptions = {
        customBodyRender: (value: Date) => UtilService.getDate(value),
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
            },
        }),
    };

    const defaultCellCurrencyOptions = {
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
                textAlign: 'right',
            },
        }),
        customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value ?? 0),
        setCellProps: () => ({
            style: {
                textAlign: 'right',
            },
        }),
    };

    const columns = [
        {
            name: 'numeroCredito',
            label: 'Radicación',
            options: defaultCellOptions,
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: defaultCellOptions,
        },
        {
            name: 'identificacion',
            label: 'Identificación',
            options: defaultCellOptions,
        },
        {
            name: 'nombreCompleto',
            label: 'Apellidos y Nombres',
            options: defaultCellOptions,
        },
        {
            name: 'fuerza',
            label: 'Fuerza',
            options: defaultCellOptions,
        },
        // {
        //     name: 'categoria',
        //     label: 'Categoría',
        //     options: defaultCellOptions,
        // },
        {
            name: 'unidadejecutora',
            label: 'Nombre Pagaduría',
            options: defaultCellOptions,
        },
        {
            name: 'fechaDesembolso',
            label: 'Apertura',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaFinalizaPagos',
            label: 'Terminación',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'lineaCredito',
            label: 'Línea de Descuento',
            options: defaultCellOptions,
        },
        {
            name: 'tasaCredito',
            label: 'Tasa de Línea',
            options: {
                ...defaultCellCurrencyOptions, 
                customBodyRender: (value: number | bigint) => UtilService.formatPercent(value ?? 0)
            },
            formatType: ColumnFormatType.Porcentaje,
        },
        {
            name: 'valorDesembolso',
            label: 'Monto',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'saldoCapital',
            label: 'Saldo',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorCuota',
            label: 'Cuota',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'fechaUltimoPago',
            label: 'Ultimo Pago',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaProximoPago',
            label: 'Proximo Pago',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'valorMoraAnterior',
            label: 'Mora Anterior',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'estado_Descripcion',
            label: 'Estado',
            options: defaultCellOptions,
        },
        {
            name: 'diasMora',
            label: 'Dias Mora',
            options: defaultCellOptions,
        },
        {
            name: 'valorIntereses',
            label: 'Interés',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorSeguros',
            label: 'Seguros',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorCapital',
            label: 'Capital',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorTotalPagar',
            label: 'Total Pagar',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorVivienda',
            label: 'Valor del Inmueble',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'plazoTotal',
            label: 'Plazo de la   Obligación',
            options: defaultCellOptions,
        },
        {
            name: 'numeroCuotasFaltantes',
            label: 'Numero de cuotas faltantes',
            options: defaultCellOptions,
        },
        {
            name: 'calificacion',
            label: 'Calificación',
            options: defaultCellOptions,
        },
        {
            name: 'cre_Cartera_Castigada',
            label: 'Cartera Castigada',
            options: defaultCellOptions,
        },
        {
            name: 'valorOpcionCompra',
            label: 'Valor opción de compra',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'canonExtraordinarioFinal',
            label: ' Valor Cuota Extraordinaria',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorDeudaRemanente',
            label: 'Interés Remanente',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'provisionTotal',
            label: 'Provisión Total',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'descripcionCiudad',
            label: 'Ciudad Inmueble',
            options: defaultCellOptions,
        },
        {
            name: 'tipoVivienda',
            label: 'Tipo Vivienda',
            options: defaultCellOptions,
        },
        {
            name: 'puntaje',
            label: 'Puntaje Scoring',
            options: defaultCellOptions,
        },
    ];

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi  = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip:any) => {
        const  convertIp = ip.split('.')[0]
       const  hexadecimal = convertIp
        await catalogApi 
            .getObtenerIpLocal(hexadecimal)
            .then(async (response: any) => {
    
                if (response) {
                    if (response.isSuccess) {
                        setPublicIpAddress(response.data);
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    
    }

    useEffect(() => {
        const fetchLocalIPAddress = () => {
            const peerConnection = new RTCPeerConnection({ iceServers: [] });
            peerConnection.createDataChannel('');
            peerConnection.createOffer()
                .then((offer) => peerConnection.setLocalDescription(offer))
                .catch((error) => console.error('Error creating offer:', error));

            peerConnection.onicecandidate = (event) => {
                if (event.candidate) {
                    const ip: any = event.candidate.address;
                    if (isValidIPv4(ip) || isValidIPv6(ip)) {
                        enviarIp(ip);
                    } else {
                        enviarIp(ip);
                    }
                }
            };

            return () => {
                peerConnection.close();
            };
        };
        fetchLocalIPAddress();

    }, []);

    const handleDownload = async () => {
        const format = periodo ? moment(periodo).format('YYYY-MM'): 'Actual';
        const fileName = `Reporte general de reestructuración - Periodo ${format}.xlsx`;
        await handleExportToExcel(data, columns, fileName, publicIpAddress);
    }

    return (
        <div style={{width: '100%', 'display': 'table', 'tableLayout': 'fixed'}}>
            <MUIDataTable
                    title=""
                    data={data}
                    columns={columns}
                    options={{
                        rowsPerPage: 10,
                        tableBodyHeight: '500px',
                        selectableRows: 'none',
                        responsive: 'standard',
                        filter: false,
                        print: false,
                        elevation: 2,
                        filterType: 'dropdown',
                        fixedHeader: true,
                        fixedSelectColumn: true,
                        textLabels: {
                            body: {
                                noMatch: 'No se encontraron resultados',
                            },
                            pagination: {
                                displayRows: 'de',
                                next: 'Siguiente',
                                previous: 'Anterior',
                                rowsPerPage: 'Filas por página',
                                jumpToPage: '',
                            },
                            toolbar: {
                                downloadCsv: 'Descargar CSV',
                                search: 'Buscar',
                                viewColumns: 'Mostrar Columnas',
                            },
                            viewColumns: {
                                title: 'Mostrar Columnas',
                            },
                        },
                        download: false,
                        customToolbar: () => <DownloadToolbar  downloadAction={handleDownload} />
                    }}
                />
        </div>
    );
}

export default TablaReporteGeneralReestructuracion;