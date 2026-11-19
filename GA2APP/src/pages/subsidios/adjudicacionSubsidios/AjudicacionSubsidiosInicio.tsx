import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import React, { useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import swal from 'sweetalert';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import RealizacionSubsidios from './ResolucionSubsidios';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '400px',
			marginLeft: '.5rem',
		},
		processButton: {
			backgroundColor: '#1C3F8B',
			color: '#fff',
			margin: '8px 0',
		},
	}),
);

function SubsidiosTable ({
    data,
    onClickItem
}:{
    data: any[] | null;
    onClickItem: any;
}){

    const classes = useStyles();

    if (data === null) {
        return <div>No se encontraron resultados</div>;
    }

    return(
        <>
             <table className="credit-table">
              <thead>
                  <tr>
                      <th style={{ textAlign: 'center' }}>
                          Procesar
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          No. Resolución
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Proceso
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Fecha de Apertura
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Fecha de Cierre
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Funcionario
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Estado
                      </th>
                  </tr>
              </thead>
              <tbody>
              {data?.map((row, index) => (
                      <tr key={`table-${index}`}>
                          <td>
                              <Button
                                  className={classes.processButton}
                                  onClick={() => onClickItem(row)}
                              >
                                 CONTINUAR
                              </Button>
                          </td>
                          <td>{row.resolucion}</td>
                          <td>{row.proceso}</td>
                          <td>{row.fechaRecepcion}</td>
                          <td>{row.fechaRecepcion}</td>
                          <td>{row.funcionario}</td>
                          <td>{row.estado}</td>
                      </tr>
                  ))}
              </tbody>
          </table>
        </>
    )
}

const AjudicacionSubsidiosInicio = () => {
    const classes = useStyles();
    const [selectData, onClickItem] = useState(null);
    const [isLoading, setIsLoadind] = useState(false);
    const [mostrarTabla, setMostrarTabla] = useState(false);
    

    const formularioConsulta = useForm ({
        defaultValues: {
            radicado: '',
            resolucion: '',
            fechaDesde: null,
            fechaHasta: null,
        }
    })

    const dataPrueba = [
        {
            id: 1,
            resolucion: "2024-01-124343254",
            proceso: "Realización cuadros de adjudicación",
            fechaRecepcion: "2024-08-27 09:28AM",
            funcionario: "PRUEBA PERITO",
            estado: "EN EVALUACIÓN"
        }
    ]

    const regresar = () => onClickItem(null);

    const consultar = () => {
     
        setIsLoadind(true)
        if(formularioConsulta.getValues('radicado') === ''){
            setTimeout(() => {
                swal({
                    text: 'No se encontraron subsidios radicados',
                    icon: 'warning',
                  
                  });
                  setIsLoadind(false)
                  setMostrarTabla(true)
            }, 10000); 
        }else {
            setTimeout(() => {
                swal({
                    text: `No se encontro un subsidio para el número del radicado  ${formularioConsulta.getValues('radicado')}` ,
                    icon: 'warning',
                  
                  });
                  setIsLoadind(false)
                  setMostrarTabla(true)
            }, 10000); 
        }
    
      };
      

    if(selectData){
        return(
            <RealizacionSubsidios
                regresar={regresar}
            />
        )
    }

    if(isLoading){  return <CHLoadingProcess />;}

    return(<>
        <CHPrivateContainer>
			<CHPageTitle
				topText="Gestión de subsidios"
				subText=""
				title1="Resolución"
				title2="de Subsidios"
			/>
            <CHPaper title="Información Solicitud">
            <Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="radicado"
							control={formularioConsulta.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									InputLabelProps={{ shrink: true }}
									label="No. Radicado"
									value={value}
									onChange={e => {
										if(e.target.value.length < 30) {
											onChange(
												e.target.value.replace(
													/[a-zA-Z#./?=!@$%^&*^)(+=_}{":;><|~`]/g,
													'',
												),
											);
										}
									}}
								/>
							)}
						/>
					</Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaDesde"
                            control={formularioConsulta.control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Desde"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: false }}
                                    InputLabelProps={{ shrink: true }}
                                    autoOk
                                />
                            )}
                        />
					</Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaHasta"
                            control={formularioConsulta.control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Hasta"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: false }}
                                    InputLabelProps={{ shrink: true }}
                                    autoOk
                                />
                            )}
                        />
					</Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="resolucion"
							control={formularioConsulta.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									InputLabelProps={{ shrink: true }}
									label="No. Resolución"
									value={value}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
									onChange={e => {
										if(e.target.value.length < 30) {
											onChange(
												e.target.value.replace(
													/[a-zA-Z#./?=!@$%^&*^)(+=_}{":;><|~`]/g,
													'',
												),
											);
										}
									}}
								/>
							)}
						/>
					</Grid>
                </Grid>
            </CHPaper>
            <div className="buttons">
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={()=> {consultar()}}
                >
                    CONSULTAR
                </Button>
            </div>
        {  mostrarTabla &&  <SubsidiosTable
               data={dataPrueba}
               onClickItem={onClickItem}
            />}
        </CHPrivateContainer>
    </>)
}

export default AjudicacionSubsidiosInicio;
