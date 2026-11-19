import { Button, Grid, MenuItem } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHSelect from '../../components/ch-select/CHSelect';
import CHTextField from '../../components/ch-text-field/CHTextField';
import DatosAliadosEstrategicos from '../../components/DatosAliadosEstrategicos/DatosAliadosEstrategicos';


function  Prueba ()  {

const [arr, setArr] = useState([ {
  "id":1,
  "value":"pasto"
},
{
  "id":2,
  "value":"cali"
}])

    const formularioPrueba=useForm({
        defaultValues:{
            nombre:"",
            apellido:"",
            ciudad:0
        }
    })

    const [datos, setDatos] = useState({
    
    })


    const guardar = (data:any) =>{
    
      setDatos(data)
    }
    
    // useEffect(()=>{
    //   arr?.map((x:any,index)=>{
    //     console.log(x,index)
    //   });
    //   formularioPrueba.setValue("nombre",datos.nombre);    
    //   formularioPrueba.setValue("apellido",datos.apellido);
    //   formularioPrueba.setValue("ciudad",datos.ciudad);
    // },[])

  

 

 

    return (
        <>
            <CHPaper title="Prueba">
                <Grid container>
                  <Grid  item xs={12} md={3}>
                    <Controller
                      name="nombre"
                      control={formularioPrueba.control}

                      rules={{
                        required: {
                          value: true,
                          message: 'Este campo es requerido',
                        },
                        
                  
                      }}

                      render={({
                        field: { onChange, value },
                        fieldState: { error },
                      }) => (
                        
                          <CHTextField
                            error={!!error}
                            helperText={error ? error.message : null}
                            label="nombre"
                            required
                            value={value}
                            onChange={onChange}
                          />

                
                      )}
                    />
                  </Grid>
                  <Grid  item xs={12} md={3}>
                    <Controller
                      name="apellido"
                      control={formularioPrueba.control}

                      rules={{
                        required: {
                          value: true,
                          message: 'Este campo es requerido',
                        },
                        
                  
                      }}

                      render={({
                        field: { onChange, value },
                        fieldState: { error },
                      }) => (
                        
                          <CHTextField
                            error={!!error}
                            helperText={error ? error.message : null}
                            label="apellido"
                            required
                            value={value}
                            onChange={onChange}
                          />

                
                      )}
                    />
                  </Grid>
                  <Grid item xs={12} md={4}>
                        <Controller
                            name="ciudad"
                            control={formularioPrueba.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Verificación Desembolso *"
                                    onChange={onChange}
                                    value={value}
                                >
                                  {arr.map((x:any)=>(
                                    <MenuItem key={x.id} value={x.id}> 
                                    {x.value}
                                    </MenuItem>
                                  ))}
                               
                                </CHSelect>
                            )}
                        />
                    </Grid>

                  <Button
                    onClick={formularioPrueba.handleSubmit(guardar)}
                  >
                    CANCELAR
                  </Button>
                </Grid>
                <PruebaDos
                  datos={datos}
                />
            </CHPaper>
            
        </>
    )
} 

function PruebaDos ({datos}:{datos:any}){
  // console.log(datos)
  return (<>
    <table className="credit-table">
    <thead>
					<tr>
						<th>nombre</th>
						<th>apellido</th>				
						<th>ciudad</th>				
					</tr>
				</thead>
        <tbody>
         <tr key={2}>
          <td>{datos.nombre}</td>
					<td>{datos.apellido}</td>
					<td>{datos.ciudad}</td>
         </tr>
        </tbody>
    </table>
  </>)
}

export default Prueba 