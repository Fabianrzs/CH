import React, { useEffect, useRef, useState } from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import { useHistory } from 'react-router-dom';
import { Collapse } from '@material-ui/core';
import { ExpandLess, ExpandMore } from '@material-ui/icons';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { toast } from 'react-toastify';
import { SideMenuItem } from '../../models/side-menu-item.model';
import StorageService from '../../services/storage.service';
import { authContext } from '../../pages/login/LoginContextAzureAD';
import Api from '../../services/api.core.service';
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import AuditoriaApi from '../../services/api-auditorias.servise';
import CatalogApi from '../../services/api-catalog.service';
import { useIp } from '../../shared/context/IpContext';


const api = Api();

const useStyles = makeStyles(() =>
    createStyles({
        listItem: {
            color: '#fff',
        },
        subItem: {
            marginLeft: '.5rem',
            color: '#fff',
        },
        subItemText: {
            fontSize: '1rem',
        },
    }),
);

function CHCollapsable({
    isOpen,
    subMenus,
    onRedirect,
    isSubItem = false,
}: {
    isOpen: boolean;
    subMenus: SideMenuItem[];
    onRedirect: () => void;
    isSubItem?: boolean;
}) {
    const history = useHistory();
    const [itemOpen, setItemOpen] = useState<number | null>(null);
    const handleClick = (menuLink: string) => () => {
        history.push(menuLink);
        onRedirect();
    };
    return (
        <Collapse in={isOpen} timeout="auto">
            {subMenus.map((menu, key) => {
                if(menu.subMenus != null) {
                    return <CHSideMenuItem
                            isOpen={itemOpen === key}
                            setOpen={setItemOpen}
                            id={key}
                            onRedirect={onRedirect}
                            menu={menu}
                            isSubItem
                            key={`subitem-${key}`} />
                }

                return <Item
                            isSubItem
                            item={menu}
                            key={`subitem-${key}`}
                            handleClick={handleClick(menu.link)}
                        />
            })}
        </Collapse>
    );
}

function Item({
    item,
    handleClick,
    firstLevel = false,
    isOpen = false,
    isSubItem,
}: {
    item: SideMenuItem;
    handleClick: () => void;
    firstLevel?: boolean;
    isOpen?: boolean;
    isSubItem?: boolean;
}) {
    const classes = useStyles();
    const Expand = isOpen ? <ExpandLess /> : <ExpandMore />;
    return (
        
        <ListItem
            classes={{
                root: isSubItem ? classes.subItem : classes.listItem,
            }}
            button
            onClick={handleClick}
        >
            <ListItemIcon
                classes={{
                    root: classes.listItem,
                }}
            >
                {item.icon}
            </ListItemIcon>
            <ListItemText
                disableTypography={isSubItem}
                primary={item.name}
                classes={{
                    root: isSubItem ? classes.subItemText : '',
                }}
            />
            {firstLevel && Expand}
        </ListItem>
    );
}

function CHSideMenuItem({
    id,
    menu,
    onRedirect,
    isOpen,
    setOpen,
    isSubItem = false,
}: {
    id: number;
    menu: SideMenuItem;
    onRedirect: () => void;
    isOpen: boolean;
    setOpen: (id: number | null) => void;
    isSubItem?: boolean;
}) {
    const history = useHistory();

    const { publicIp } = useIp();
    
    const handleClick = () => {
        if (menu.subMenus) {
            if (isOpen) {
                setOpen(null);
            } else {
                setOpen(id);
            }
        } else {

            if(menu.name==="Cerrar sesión"){
                const auditoriaApi= AuditoriaApi();
                const user = AuthService.getUserInfo();
                const nav =(window.navigator as any);
                const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;
                const requestAuditoria={
                    navegador:usrAg,
                    sistemaOperativo:nav.userAgentData.platform,
                    nombreUsuario:user.USR_USERNAME, 
                    usurioId:user.USR_ID,
                    ip: publicIp,
                    modulo:'Logout',
                     hostname:document.location.host
                } as Auditoria;
                api.Logout(user.USR_ID)               
                .then(() => {
                    window.localStorage.clear()                    
                    handleCloseSesion();
                    requestAuditoria.tipoAccion='INICIO DE SESIÓN';
                    requestAuditoria.idTipoAccion=1;
                    requestAuditoria.descripcion=`Cierre de sesión:
                         \n Usuario:  ${user.USR_USERNAME}`;
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                }).catch((error:any)=> {
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                   requestAuditoria.descripcion=`Cierre de sesión:
                    \n Usuario:  ${user.USR_USERNAME}                 
                    \n Error:  ${error}
                    \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                }) 
                
                return
            }
              history.push(menu.link);
              handleRedirect();
            
        }
    };

    const handleRedirect = () => {
        onRedirect();
    };

    const handleCloseSesion = () => {
        StorageService.removeStorageFromLogout();
        history.push('/');
    };

    return (
        <div>
            <Item
                firstLevel={menu.subMenus != null}
                isOpen={isOpen}
                item={menu}
                handleClick={handleClick}
                isSubItem={isSubItem}
            />
            {menu.subMenus && (
                <CHCollapsable
                    onRedirect={handleRedirect}
                    isOpen={isOpen}
                    subMenus={menu.subMenus}
                />
            )}
        </div>
    );
}

export default CHSideMenuItem;
