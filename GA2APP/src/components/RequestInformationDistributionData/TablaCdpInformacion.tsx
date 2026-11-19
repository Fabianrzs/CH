
import { Paper, Table, TableCell, TableContainer, TableRow, TableHead, TableBody, Typography, Button, makeStyles, Theme, createStyles } from "@material-ui/core"

import React, { useEffect, useState } from "react"
import { toast } from "react-toastify";
import CreditApi from "../../services/api-credit.service";
import NotificationService from "../../services/notification.service";
import UtilService from "../../utils/util.service";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
       
        button: {
          width:'200px',
            marginLeft: '.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
      
    }),
);

const TablaCpdInformacion = ({ procedureRequest, numeroCDP }: { procedureRequest: any; numeroCDP: any }) => {
    const creditApi = CreditApi();
    console.log({ numeroCDP })

    const [listVendedor, setListaVendedor] = useState([])


    useEffect(() => {
        const request = {
            Soc_Id: procedureRequest.requestID,
            Sls_Id: '00000000-0000-0000-0000-000000000000'
        }
        creditApi
            .postObtenerVendedor(request)
            .then(response => {
                if (response) {
                    if (response.isSuccess) {
                        setListaVendedor(response.data)

                    }
                }
            })


            .catch(() => {

                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, [])

   

    const classes = useStyles();
    return (
        <>
            <br />
            <div>
                <Typography>
                    Se ha generado correctamente la solicitud de CDP con el número <strong>{numeroCDP}</strong>, por valor de <strong>{UtilService.formatCurrency(procedureRequest.creditValue)}</strong>, con la siguiente información:
                </Typography>
            </div>
    
            <br />
            <br />
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow style={{ backgroundColor: '#1C3F8B' }}>
                            <TableCell style={{ color: 'white', width: '33%' }}><strong>Nombres</strong></TableCell>
                            <TableCell style={{ color: 'white', width: '33%' }}><strong>Tipo Tercero</strong></TableCell>
                            <TableCell style={{ color: 'white', width: '33%' }}><strong>No. Identificación</strong></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        <TableRow>
                            <TableCell>{procedureRequest?.clientName}</TableCell>
                            <TableCell>Afiliado</TableCell>
                            <TableCell>{procedureRequest?.clientIdentificationNumber}</TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </TableContainer>
    
            <br />
            <br />
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow style={{ backgroundColor: '#1C3F8B' }}>
                            <TableCell style={{ color: 'white', width: '33%' }}><strong>Nombres</strong></TableCell>
                            <TableCell style={{ color: 'white', width: '33%' }}><strong>Tipo Tercero</strong></TableCell>
                            <TableCell style={{ color: 'white', width: '33%' }}><strong>No. Identificación</strong></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {listVendedor.map((row: any) => (
                            <TableRow key={row.id}>
                                <TableCell>{row.nombre_Vendedor}</TableCell>
                                <TableCell>Beneficiario</TableCell>
                                <TableCell>{row.numero_Doc}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
    


}

export default TablaCpdInformacion