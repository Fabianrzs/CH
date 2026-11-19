import React from 'react';
import './header.style.scss';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import CssBaseline from '@material-ui/core/CssBaseline';
import Typography from '@material-ui/core/Typography';
import LogoCH from '../../assets/images/LogoHorizontalCajaHonor.svg';

const useStyles = makeStyles(() =>
    createStyles({
        root: {
            display: 'fixed',
            flexGrow: 1,
        },
        logo: {
            flexGrow: 1,
        },
    }),
);

export default function HeaderLogin() {
    const classes = useStyles();
    return (
        <div className={classes.root}>
            <CssBaseline />
            <AppBar position="fixed">
                <Toolbar className="fondo-header">
                    <Typography variant="h6" noWrap className={classes.logo} />
                    <a href="/Inicio">
                        <img src={LogoCH} alt="" />
                    </a>
                </Toolbar>
            </AppBar>
        </div>
    );
}
