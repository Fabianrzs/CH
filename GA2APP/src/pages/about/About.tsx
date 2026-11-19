import { Card, CardContent, CardHeader, Grid } from '@material-ui/core';
import React from 'react';

export const AboutPage: React.FC = () => (
    <Grid container className="Centrado">
        <Grid item xs={12}>
            <Grid container>
                <Grid item xs={12}>
                    <Card>
                        <CardHeader title="Acerca de." />
                        <CardContent>
                            Este contenido es exclusivo para el acerca de. de la
                            aplicación GA2.
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
        </Grid>
    </Grid>
);
