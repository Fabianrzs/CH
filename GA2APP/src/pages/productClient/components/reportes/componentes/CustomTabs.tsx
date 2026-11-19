import React from "react";
import { Tabs, Theme, createStyles, withStyles, Tab } from "@material-ui/core";

export interface StyledTabProps {
    label: string;
    value: any;
}

export const CustomTabs = withStyles({
    flexContainer: {
        display: 'flex',
        flexWrap: 'wrap',
    },
    root: {
        borderBottom: '1px solid #e8e8e8',
        marginBottom: '1rem',
    },
    indicator: {
        backgroundColor: '#FEC917',
    },
})(Tabs);


export const CustomTabItems = withStyles((theme: Theme) =>
    createStyles({
        root: {
            textTransform: 'none',
            margin: 0,
            marginRight: theme.spacing(0.5),
            '&$selected': {
                color: 'white',
                fontWeight: theme.typography.fontWeightMedium,
                background: '#1C3F8B'
            },
        },
        selected: {},
    }),
)((props: StyledTabProps) => <Tab disableRipple {...props} />);