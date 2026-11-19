export interface SideMenuItem {
    name: string;
    icon: any;
    link: string;
    permission?:string
    subMenus?: SideMenuItem[];
}
