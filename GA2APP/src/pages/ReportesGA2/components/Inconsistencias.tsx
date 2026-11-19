import { React, useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    MenuItem
} from '@material-ui/core';
import moment from 'moment';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import UtilService from '../../../utils/util.service';
import CHSelect from '../../../components/ch-select/CHSelect';
import { LoadingComponent } from '../../../components/loading/loading.component';
import ApiClients from '../../../services/api-clients.service';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { startGetAllGrades, startGetAllForces, startGetCategory, startGetAllExecutingUnits, startGetAllIdentificationTypes } from '../../../actions/catalog.action';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import ApiReportes from '../../../services/api-reportes.service';
import { FORCEDTO } from '../../../types/catalog.type';
import { LoginState } from '../../../reducers/login.reducer';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        button2: {
            width: '100%',
            marginTop: '1.5rem',
            color: '#fff',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px px',
        },
        accountButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 8px',
        },

        buttonNoObl: {
            width: '17rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
        buttonTotalSaldos: {
            width: '17rem',
            marginLeft: '31rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
        buttonTotalProvision: {
            width: '17rem',
            marginLeft: '1.5rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
    }),
);

const api = ApiClients();
const apiReportes = ApiReportes();

// function ConsolidadoFuerzasTable({
//     data,
//     catalogState,
//     dataStatus
// }: {
//     data: any;
//     catalogState: CatalogState;
//     dataStatus: any
// }) {
//     const forcesId: number[] = Array.from(new Set(data?.map((x: any) => x.idFuerza)))
//     return (
//         <div>
//             {data.length > 0 ? (
//                 <table className="client-table">
//                     <thead>
//                         <tr>
//                             <th>Fuerza</th>
//                             <th>Unidad Ejecutora</th>
//                         </tr>
//                     </thead>
//                     <tbody>
//                         {catalogState.Force.filter(obj => forcesId.includes(obj.id)).map((force: FORCEDTO, index) => {
//                             const statusCount = dataStatus?.length ?? 0; 
//                             return dataStatus?.map((status: any, statusIndex: any) => {
//                                 if (statusIndex === 0) {
//                                     return (
//                                         <tr key={`table-${index}`}>
//                                             <td data-label="Número Cuenta" className="tab-space-01" rowSpan={statusCount}>{force.descripcion}</td>
//                                             <td data-label="Número Cuenta" className="tab-space-01">{status.unidadEjecutora}</td>
//                                         </tr>
//                                     );
//                                 }
//                                 return (
//                                     <tr key={`table-${index}`}>
//                                         <td data-label="Número Cuenta" className="tab-space-01">{status.unidadEjecutora}</td>
//                                     </tr>
//                                 );
//                             });
//                         })}
//                     </tbody>
//                 </table>
//             ) : null}
//         </div>
//     )
// }

const Inconsistencias = ({
    catalogState,
    loginState,
    startGetAllForces,
    startGetAllExecutingUnits
} : {
    catalogState : CatalogState;
    loginState: LoginState;
    startGetAllForces: (props: string) => void;
    startGetAllExecutingUnits: (props: string) => void;
}) => {
    const classes = useStyles();

    const {control} = useForm({
        defaultValues:{
            radicado: '',
            liderProceso: 0,
            tipoIdentificacionSolicitante: 0,
            identificacionSolicitante: '',
            periodo: '',
            
        }
    })	

    useEffect(() => {
        startGetAllForces(loginState.claims)
        startGetAllExecutingUnits(loginState.claims)
    }, [])

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);

    const handleSearchButton = async () => {
        setLoading(true)
        apiReportes.getReporteConsolidadoFuerza()
        .then((response) => {
            setData(response.data)
        })
        setLoading(false)
    };

    // const GenerarExcel = async () => {
    //     const columnDataList = data?.map((row: any) => ({
	// 		data: [				
	// 			String(catalogState?.IdentificationType.find(x => x.id === row.idTipoIdentificacion)?.descripcion) || '',
	// 			String(row.identificacion ) || '',
	// 			String(`${row.primerNombre} ${row.primerApellido}` ) || '',
	// 			String(catalogState.Force.find(x => x.id === row.idFuerza)?.descripcion )|| '',
	// 			String(catalogState.ExecutingUnit.find(x => x.id === row.idUnidadEjecutora)?.nombre )|| '',
	// 			String(catalogState.Grade.find(x => x.id === row.idGrado)?.descripcion )|| ''
	// 		]

	// 	}));

    //     const request = {
    //         columnDataList: columnDataList.map(item => ({
    //             data: item.data
    //         })),
    //         columnName: [
    //         "Tipo Identificación", "Número identificación", 
    //         "Nombre", "Fuerza","Unidad Ejecutora", "Grado" 
    //          ]
    //     };
    //     const response = await apiReportes.GenerarExcels(request);
    //     const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    //     const url = window.URL.createObjectURL(blob);
    //     const link = document.createElement('a');
    //     link.href = url;
    //     link.download = `Consulta-Afiliados:${moment().format()}.csv`;
    //     document.body.appendChild(link);
    //     link.click();
    //     window.URL.revokeObjectURL(url);
    // }

    return (
        <div>
            <CHPaper title="Inconsistencias">
                <LoadingComponent key="loader" loading={loading} />
                <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHTextField
                                    defaultValue=""
                                    label="Radicado"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="radicado"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Lider proceso"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="liderProceso"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo identificación solicitante"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="tipoIdentificacionSolicitante"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHTextField
                                    defaultValue=""
                                    label="Identificación solicitante"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="identificacionSolicitante"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Período"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="periodo"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={handleSearchButton}
                        >
                            CONSULTAR 
                        </Button>
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            // onClick={GenerarExcel}
                        >
                            GENERAR EXCEL 
                        </Button>
                    </Grid>
                </Grid>
            </CHPaper>
            {/* <ConsolidadoFuerzasTable
                data={data}
                catalogState={catalogState}
            /> */}
        </div>
    );
};

const mapStateToProps = (state: AppState) => ({
	clientState: state.client,
	catalogState: state.catalog,
	loginState: state.login,
});

const mapDispatchToProps = {
	startGetAllGrades,
	startGetAllForces,
	startGetCategory,
	startGetAllExecutingUnits,
    startGetAllIdentificationTypes
};

export default connect(mapStateToProps, mapDispatchToProps)(Inconsistencias);
