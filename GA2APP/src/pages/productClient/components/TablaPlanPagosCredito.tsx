
import React from "react"
import MUIDataTable from "mui-datatables";

const TablaPlanPagosCredito = (props: any) => {

    const {
        obtenerPlanPagosVer
    }= props

    const columns = [
        {
            name: 'plnNroCredito',
            label: 'Número Radicado',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'cuota',
            label: 'Cuotas',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'fechaPago',
            label: 'Fecha Pago',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'saldo',
            label: 'Saldo Inicial',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'capital',
            label: 'Capital',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'capitalPorPagar',
            label: 'Capital Por Pagar',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'intereses',
            label: 'Interes',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'interesesPorPagar',
            label: 'Interes Por Pagar',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'seguroVida',
            label: 'Seguro Vida',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'seguroHogar',
            label: 'Seguro Hogar',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'capitalPorPagar',
            label: 'A Pagar',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'plazoAnos',
            label: 'Plazo',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
        {
            name: 'capitalSaldo',
            label: 'Deuda Remanente',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },
  
        },
     
    ];

console.log({obtenerPlanPagosVer})
return(

    <>
  <MUIDataTable
    title="Plan de pagos"
    data={obtenerPlanPagosVer}
    columns={columns}
    options={{
        selectableRows: 'none',
        responsive: 'vertical',
        filter: false,
        print: false,
        elevation: 0,
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
    }}
/>
    </>
)

}


export default TablaPlanPagosCredito