import { createStyles, Grid, IconButton, makeStyles } from '@material-ui/core';
import { CloudDownload } from '@material-ui/icons';
import React from 'react';
import CHPaper from '../../../components/ch-paper/CHPaper';

const useStyles = makeStyles(() =>
	createStyles({
        columnIcon: {
			marginRight: '5px',
			textAlign: 'right',
		},
        label: {
            fontFamily: 'Tahoma',
            color: '#808080',
            paddingRight: '5px',
            fontSize: '16px'
        }
	}),
);

const SubsidiosAnulados = () => {
    const classes = useStyles();
    return(
        <>
            <CHPaper title="Subsidios Anulados">
            <Grid container>
                <Grid className={classes.columnIcon} item xs={12} md={12}>
                    <span className={classes.label}>Descargar Excel</span>
                    <IconButton>
                        <CloudDownload fontSize='large'/>
                    </IconButton>
                </Grid>
            </Grid>
            <Grid container>
             <table className="credit-table"    
                    style={{
                        margin: '10px',
                        width: '99%',
                    }}>
                    <thead>
                    <tr>
                        <th style={{ textAlign: 'center', width: '15%' }}>
                           Documento
                        </th>
                        <th style={{ textAlign: 'center' }}>
                           Radicado
                        </th>
                        <th style={{ textAlign: 'center', width: '20%' }}>
                           Afiliado
                        </th>
                        <th style={{ textAlign: 'center' }}>
                           CDP
                        </th>
                        <th style={{ textAlign: 'center', width: '20%' }}>
                           Tercero
                        </th>
                        <th style={{ textAlign: 'center' }}>
                           Factura
                        </th>
                        <th style={{ textAlign: 'center' }}>
                           Monto
                        </th>
                    </tr>
                </thead>
                <tbody>
                        <tr style={{ textAlign: 'center'}}>
							<td>{" "}</td>
							<td>{" "}</td>
							<td>{" "}</td>
                            <td>{" "}</td>
                            <td>{" "}</td>
                            <td>{" "}</td>
                            <td>{" "}</td>
						</tr>
				</tbody>
           </table> 
           </Grid>
        </CHPaper>
        </>
    )
}

export default SubsidiosAnulados;