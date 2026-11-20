import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import {
    Button,
    Grid,
    makeStyles,
    TextField,
    Theme,
    createStyles,
    InputAdornment,
    IconButton,
} from '@material-ui/core';

import './login.style.scss';
import { useForm, Controller } from 'react-hook-form';
import { Visibility, VisibilityOff } from '@material-ui/icons';
import { startLoginUser } from '../../actions/login.action';
import { LoginState } from '../../reducers/login.reducer';

import HeaderLogin from '../../components/header/Header';
import { Login, LoginInitial } from '../../types/login.type';
import { AppState } from '../../store/rootReducer';
import { LoadingComponent } from '../../components/loading/loading.component';
import { authContext } from './LoginContextAzureAD';
import StorageService from '../../services/storage.service';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            flexGrow: 1,
        },
        errorscolors: {
            color: 'red',
        },
        boxForm: {
            textAlign: 'center',
            margin: theme.spacing(20, 0, 20, 0),
            // background:'red',
        },
        boxRules: {
            textAlign: 'left',
            margin: theme.spacing(25, 0, 20, 0),
        },
        formSpacing: {
            '& .MuiTextField-root': {
                margin: theme.spacing(1),
            },
            '& .MuiButton-root': {
                margin: theme.spacing(1),
            },
        },
    }),
);

const LoginPage = (props: any) => {
    const { startLoginUser, loginState } = props;
    const [showPassword, setShowPassword] = useState(false);

    const togglePasswordVisibility = () => {
        setShowPassword(prev => !prev);
    };
    const classes = useStyles();

    const [cacheKey, setCacheKey] = useState('');

    // useEffect(() => {
    //   // Lógica para borrar la caché y el localStorage aquí
    //   window.localStorage.clear();
    // }, [cacheKey]);
    // useEffect(() => {
    //     clearCache()
    // }, []);

    const {
        handleSubmit,
        control,
        formState: { errors },
    } = useForm<Login>({
        defaultValues: LoginInitial,
        criteriaMode: 'all',
    });

    const onLogin = (login: Login) => {
        startLoginUser(login);
    };

    const onSubmit = handleSubmit(data => {
        const dataInfo = {
            credential: data?.credential,
            isActiveDirectory: true,
            userName: data.userName,
        };
        onLogin(dataInfo);
    });

    const clearCache = () => {
        // Actualiza el valor de cacheKey para borrar la caché
        setCacheKey(Date.now().toString());
    };

    // AZURE ACTIVE DIRECTORY
    const login = () => {
        authContext.login();
    };

    useEffect(() => {
        // Handle the authentication callback from Azure AD
        authContext.handleWindowCallback();
        const storedToast = localStorage.getItem('toastMessage');
        if (storedToast === null) {
            toast.dismiss(storedToast || undefined);
        }
        window.localStorage.clear();
        StorageService.removeStorageFromLogout();

        // Retrieve user data
        const authenticatedUser = authContext.getCachedUser();

        if (authenticatedUser) {
            onLogin({
                userName: authenticatedUser.userName,
                isActiveDirectory: true,
            } as Login);
        }
    }, []);

    return (
        <div className={classes.root}>
            <HeaderLogin />
            <LoadingComponent key="loader" loading={loginState.loading} />
            <Grid container spacing={4}>
                <Grid
                    item
                    xs={12}
                    sm={6}
                    className={`container-login ${classes.boxForm}`}
                >
                    <Grid container spacing={1}>
                        <Grid item xs={12}>
                            <Grid className="subtittle-2 blue">Bienvenido</Grid>
                            <Grid className="blue-3 icoBig-2">
                                <span className="ico fch-Caja-Honor-0-1" />
                            </Grid>
                            <Grid className="subtittle-2 blue">
                                SISTEMA GA<span className="yellow">2+</span>
                                <br />
                                <span className="yellow">
                                    Gestión de Aportes y Afiliados
                                </span>
                            </Grid>
                        </Grid>
                        <Grid item xs={12}>
                            <Grid className="container-11">
                                <form
                                    onSubmit={onSubmit}
                                    className={classes.formSpacing}
                                >
                                    <Controller
                                        control={control}
                                        rules={{
                                            required:
                                                'Este campo es requerido.',
                                        }}
                                        name="userName"
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <TextField
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                variant="outlined"
                                                fullWidth
                                                onChange={onChange}
                                                value={value}
                                                label="Usuario"
                                                id="userName"
                                                type="text"
                                            />
                                        )}
                                    />

                                    {errors.userName && (
                                        <div className={classes.errorscolors}>
                                            Debe ingrear un correo valido
                                        </div>
                                    )}
                                    <Controller
                                        control={control}
                                        rules={{
                                            required:
                                                'Este campo es requerido.',
                                            minLength: {
                                                value: 8,
                                                message:
                                                    'La contraseña debe tener al menos 8 caracteres.',
                                            },
                                            pattern: {
                                                value: /^(?=.*[a-zA-Z])(?=.*[!@#$%^&*(),.?":{}|<>])(?!.*(.)\1{3,}).{8,}$/,
                                                message:
                                                    'La contraseña debe incluir al menos una letra, un carácter especial y no repetir un mismo carácter más de 3 veces seguidas.',
                                            },
                                        }}
                                        name="credential"
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <TextField
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                variant="outlined"
                                                fullWidth
                                                label="Contraseña"
                                                onChange={onChange}
                                                value={value}
                                                id="password"
                                                type={
                                                    showPassword
                                                        ? 'text'
                                                        : 'password'
                                                }
                                                InputProps={{
                                                    endAdornment: (
                                                        <InputAdornment position="end">
                                                            <IconButton
                                                                onClick={
                                                                    togglePasswordVisibility
                                                                }
                                                                edge="end"
                                                            >
                                                                {showPassword ? (
                                                                    <VisibilityOff />
                                                                ) : (
                                                                    <Visibility />
                                                                )}
                                                            </IconButton>
                                                        </InputAdornment>
                                                    ),
                                                }}
                                            />
                                        )}
                                    />

                                    {/* {errors.credential && (
                                        <div className={classes.errorscolors}>
                                            {errors.credential.message}
                                        </div>
                                    )} */}

                                    <Button
                                        fullWidth
                                        type="submit"
                                        variant="contained"
                                        color="primary"
                                        className="button-h bgcian"
                                    >
                                        Ingresar
                                    </Button>

                                    <Button
                                        fullWidth
                                        variant="contained"
                                        color="primary"
                                        className="button-h bgcian"
                                        onClick={() => login()}
                                    >
                                        Ingresar a directorio activo (NO USAR)
                                    </Button>

                                    <Grid className="a-box">
                                        <a href="/RecuperacionContrasena">
                                            ¿Olvidó su contraseña?
                                        </a>
                                    </Grid>
                                </form>
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
                <Grid item xs={12} sm={6} className={classes.boxRules}>
                    <Grid item xs={12}>
                        <Grid className="container-6 md-e">
                            <Grid className="container-7 md-k">
                                <Grid>
                                    <span className="ico fch-Tips-0-1 icoBig-2 yellow" />
                                </Grid>
                                <Grid className="separador-h separador-marg-4" />
                                <Grid className="tittle blue">
                                    Tips
                                    <br />
                                    <span className="yellow">
                                        de&nbsp;seguridad
                                    </span>
                                </Grid>
                            </Grid>

                            <Grid className="container-8 md-k bwhite">
                                <Grid className="container-9 icoMid white bgblue">
                                    <span className="ico fch-Certificado-SSl-0-1" />
                                </Grid>
                                <Grid className="text-1 purple">
                                    Su información es muy valiosa. No acceda a
                                    sitios virtuales sin el certificado SSL{' '}
                                    <span className="yellow">
                                        (cifrado de datos)
                                    </span>
                                    .
                                </Grid>
                            </Grid>

                            <Grid className="container-8 md-k bwhite">
                                <Grid className="container-9 icoMid white bgblue-2">
                                    <span className="ico fch-Informacion-Personal-0-1" />
                                </Grid>
                                <Grid className="text-1 purple">
                                    No entregue Información personal y claves de
                                    acceso a terceros. Pone en riesgo los
                                    beneficios como afiliado que ofrece Caja
                                    Honor.
                                </Grid>
                            </Grid>

                            <Grid className="container-8 md-k bwhite">
                                <Grid className="container-9 icoMid white bgblue-2">
                                    <span className="ico fch-Datos-Personales-0-1" />
                                </Grid>
                                <Grid className="text-1 purple">
                                    Sus datos personales son únicos e
                                    intransferibles, proteja su información.
                                </Grid>
                            </Grid>

                            <Grid className="container-8 md-k bwhite">
                                <Grid className="container-9 icoMid white bgblue-2">
                                    <span className="ico fch-Verifique-Docuementos-0-1" />
                                </Grid>
                                <Grid className="text-1 purple">
                                    Siempre verifique los documentos adjuntos en
                                    un correo electrónico.
                                </Grid>
                            </Grid>

                            <Grid className="container-8 md-k bwhite">
                                <Grid className="container-9 icoMid white bgblue-2">
                                    <span className="ico fch-Enlace-0-1" />
                                </Grid>
                                <Grid className="text-1 purple">
                                    No abra ningún enlace en el que le pidan
                                    información o haya alguna oferta.
                                </Grid>
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </div>
    );
};

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
});

const mapDispatchToProps = {
    startLoginUser,
};

type Props = {
    loginState: LoginState;
    startLoginUser: (login: Login) => void;
};

export default connect(mapStateToProps, mapDispatchToProps)(LoginPage);