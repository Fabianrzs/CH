using AutoMapper;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSWintipro;

namespace GA2.Transversals.Mappers.GA2.Credito.Seven
{
    public class SevenMappers : Profile
    {
        public SevenMappers()
        {
            // CDP
            CreateMap<AnularCDPRequest, AnularCDPDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.emp_codi))
                .ForMember(source => source.Mpr_cont, map => map.MapFrom(destino => destino.mpr_cont))
                .ReverseMap();

            CreateMap<AplicarCDPRequest, AplicarCDPDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.emp_codi))
                .ForMember(source => source.Mpr_cont, map => map.MapFrom(destino => destino.mpr_cont))
                .ReverseMap();

            CreateMap<CrearCDPRequest, CrearCDPDto>()
                .ForMember(source => source.vDetalle, map => map.MapFrom(destino => destino.pCDP.vDetalle))
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.pCDP.Emp_codi))
                .ForMember(source => source.Top_codi, map => map.MapFrom(destino => destino.pCDP.Top_codi))
                .ForMember(source => source.Mpr_nume, map => map.MapFrom(destino => destino.pCDP.Mpr_nume))
                .ForMember(source => source.Mpr_fech, map => map.MapFrom(destino => destino.pCDP.Mpr_fech))
                .ForMember(source => source.Mpr_desc, map => map.MapFrom(destino => destino.pCDP.Mpr_desc))
                .ForMember(source => source.Arb_csuc, map => map.MapFrom(destino => destino.pCDP.Arb_csuc))
                .ForMember(source => source.Ter_coda, map => map.MapFrom(destino => destino.pCDP.Ter_coda))
                .ForMember(source => source.Mpr_tdos, map => map.MapFrom(destino => destino.pCDP.Mpr_tdos))
                .ForMember(source => source.Mpr_ndos, map => map.MapFrom(destino => destino.pCDP.Mpr_ndos))
                .ForMember(source => source.Uni_codi, map => map.MapFrom(destino => destino.pCDP.Uni_codi))
                .ReverseMap();

            /// SyncFaClienDto
            CreateMap<SyncFaClienRequest, SyncFaClienDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.emp_codi))
                .ForMember(source => source.Tip_codi, map => map.MapFrom(destino => destino.tip_codi))
                .ForMember(source => source.Ter_coda, map => map.MapFrom(destino => destino.ter_coda))
                .ForMember(source => source.Cli_dive, map => map.MapFrom(destino => destino.cli_dive))
                .ForMember(source => source.Cli_nomb, map => map.MapFrom(destino => destino.cli_nomb))
                .ForMember(source => source.Cli_apel, map => map.MapFrom(destino => destino.cli_apel))
                .ForMember(source => source.Cli_noco, map => map.MapFrom(destino => destino.cli_noco))
                .ForMember(source => source.Mod_codi, map => map.MapFrom(destino => destino.mod_codi))
                .ForMember(source => source.Pai_codi, map => map.MapFrom(destino => destino.pai_codi))
                .ForMember(source => source.Dep_codi, map => map.MapFrom(destino => destino.dep_codi))
                .ForMember(source => source.Mun_codi, map => map.MapFrom(destino => destino.mun_codi))
                .ForMember(source => source.Cli_coda, map => map.MapFrom(destino => destino.cli_coda))
                .ForMember(source => source.Tcl_codi, map => map.MapFrom(destino => destino.tcl_codi))
                .ForMember(source => source.Cal_codi, map => map.MapFrom(destino => destino.cal_codi))
                .ForMember(source => source.Coc_codi, map => map.MapFrom(destino => destino.coc_codi))
                .ForMember(source => source.Cim_codi, map => map.MapFrom(destino => destino.cim_codi))
                .ForMember(source => source.Arb_csuc, map => map.MapFrom(destino => destino.arb_csuc))
                .ForMember(source => source.Dcl_dire, map => map.MapFrom(destino => destino.dcl_dire))
                .ForMember(source => source.Dcl_ntel, map => map.MapFrom(destino => destino.dcl_ntel))
                .ForMember(source => source.Dcl_mail, map => map.MapFrom(destino => destino.dcl_mail))
                .ForMember(source => source.Dcl_nfax, map => map.MapFrom(destino => destino.dcl_nfax))
                .ForMember(source => source.Arb_clte, map => map.MapFrom(destino => destino.arb_clte))
                .ForMember(source => source.Cli_inna, map => map.MapFrom(destino => destino.cli_inna))
                .ForMember(source => source.Ven_codi, map => map.MapFrom(destino => destino.ven_codi))
                .ForMember(source => source.Arb_ccec, map => map.MapFrom(destino => destino.arb_ccec))
                .ForMember(source => source.Lis_codi, map => map.MapFrom(destino => destino.lis_codi))
                .ForMember(source => source.Dcl_obse, map => map.MapFrom(destino => destino.dcl_obse))
                .ForMember(source => source.Dcl_apar, map => map.MapFrom(destino => destino.dcl_apar))
                .ForMember(source => source.Dcl_cloc, map => map.MapFrom(destino => destino.dcl_cloc))
                .ForMember(source => source.Cli_anex, map => map.MapFrom(destino => destino.cli_anex))
                .ForMember(source => source.Cli_hpra, map => map.MapFrom(destino => destino.cli_hpra))
                .ForMember(source => source.Cli_fecm, map => map.MapFrom(destino => destino.cli_fecm))
                .ForMember(source => source.Cli_feca, map => map.MapFrom(destino => destino.cli_feca))
                .ForMember(source => source.Cli_audp, map => map.MapFrom(destino => destino.cli_audp))
                .ReverseMap();

            /// SyncFaContaDto
            CreateMap<SyncFaContaRequest, SyncFaContaDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.emp_codi))
                .ForMember(source => source.Cli_coda, map => map.MapFrom(destino => destino.cli_coda))
                .ForMember(source => source.Dcl_codd, map => map.MapFrom(destino => destino.dcl_codd))
                .ForMember(source => source.Con_codi, map => map.MapFrom(destino => destino.con_codi))
                .ForMember(source => source.Con_nomb, map => map.MapFrom(destino => destino.con_nomb))
                .ForMember(source => source.Con_carg, map => map.MapFrom(destino => destino.con_carg))
                .ForMember(source => source.Con_dire, map => map.MapFrom(destino => destino.con_dire))
                .ForMember(source => source.Con_dibo, map => map.MapFrom(destino => destino.con_dibo))
                .ForMember(source => source.Con_tele, map => map.MapFrom(destino => destino.con_tele))
                .ForMember(source => source.Con_nufa, map => map.MapFrom(destino => destino.con_nufa))
                .ForMember(source => source.Con_mail, map => map.MapFrom(destino => destino.con_mail))
                .ForMember(source => source.Con_fcum, map => map.MapFrom(destino => destino.con_fcum))
                .ForMember(source => source.Con_afic, map => map.MapFrom(destino => destino.con_afic))
                .ReverseMap();

            /// SyncFaClienComplexDto
            CreateMap<SyncFaClienComplexRequest, SyncFaClienComplexDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.toFaClient.emp_codi))
                .ForMember(source => source.Tip_codi, map => map.MapFrom(destino => destino.toFaClient.tip_codi))
                .ForMember(source => source.Ter_coda, map => map.MapFrom(destino => destino.toFaClient.ter_coda))
                .ForMember(source => source.Cli_dive, map => map.MapFrom(destino => destino.toFaClient.cli_dive))
                .ForMember(source => source.Cli_nomb, map => map.MapFrom(destino => destino.toFaClient.cli_nomb))
                .ForMember(source => source.Cli_apel, map => map.MapFrom(destino => destino.toFaClient.cli_apel))
                .ForMember(source => source.Cli_noco, map => map.MapFrom(destino => destino.toFaClient.cli_noco))
                .ForMember(source => source.Mod_codi, map => map.MapFrom(destino => destino.toFaClient.mod_codi))
                .ForMember(source => source.Pai_codi, map => map.MapFrom(destino => destino.toFaClient.pai_codi))
                .ForMember(source => source.Dep_codi, map => map.MapFrom(destino => destino.toFaClient.dep_codi))
                .ForMember(source => source.Mun_codi, map => map.MapFrom(destino => destino.toFaClient.mun_codi))
                .ForMember(source => source.Cli_coda, map => map.MapFrom(destino => destino.toFaClient.cli_coda))
                .ForMember(source => source.Tcl_codi, map => map.MapFrom(destino => destino.toFaClient.tcl_codi))
                .ForMember(source => source.Cal_codi, map => map.MapFrom(destino => destino.toFaClient.cal_codi))
                .ForMember(source => source.Coc_codi, map => map.MapFrom(destino => destino.toFaClient.coc_codi))
                .ForMember(source => source.Cim_codi, map => map.MapFrom(destino => destino.toFaClient.cim_codi))
                .ForMember(source => source.Arb_csuc, map => map.MapFrom(destino => destino.toFaClient.arb_csuc))
                .ForMember(source => source.Dcl_dire, map => map.MapFrom(destino => destino.toFaClient.dcl_dire))
                .ForMember(source => source.Dcl_ntel, map => map.MapFrom(destino => destino.toFaClient.dcl_ntel))
                .ForMember(source => source.Dcl_mail, map => map.MapFrom(destino => destino.toFaClient.dcl_mail))
                .ForMember(source => source.Dcl_nfax, map => map.MapFrom(destino => destino.toFaClient.dcl_nfax))
                .ForMember(source => source.Arb_clte, map => map.MapFrom(destino => destino.toFaClient.arb_clte))
                .ForMember(source => source.Cli_inna, map => map.MapFrom(destino => destino.toFaClient.cli_inna))
                .ForMember(source => source.Ven_codi, map => map.MapFrom(destino => destino.toFaClient.ven_codi))
                .ForMember(source => source.Arb_ccec, map => map.MapFrom(destino => destino.toFaClient.arb_ccec))
                .ForMember(source => source.Lis_codi, map => map.MapFrom(destino => destino.toFaClient.lis_codi))
                .ForMember(source => source.Dcl_obse, map => map.MapFrom(destino => destino.toFaClient.dcl_obse))
                .ForMember(source => source.Dcl_apar, map => map.MapFrom(destino => destino.toFaClient.dcl_apar))
                .ForMember(source => source.Dcl_cloc, map => map.MapFrom(destino => destino.toFaClient.dcl_cloc))
                .ForMember(source => source.Cli_anex, map => map.MapFrom(destino => destino.toFaClient.cli_anex))
                .ForMember(source => source.Cli_hpra, map => map.MapFrom(destino => destino.toFaClient.cli_hpra))
                .ForMember(source => source.Cli_fecm, map => map.MapFrom(destino => destino.toFaClient.cli_fecm))
                .ForMember(source => source.Cli_feca, map => map.MapFrom(destino => destino.toFaClient.cli_feca))
                .ForMember(source => source.Cli_audp, map => map.MapFrom(destino => destino.toFaClient.cli_audp))
                .ForMember(source => source.Arb_cpro, map => map.MapFrom(destino => destino.toFaClient.arb_cpro))
                .ForMember(source => source.Arb_care, map => map.MapFrom(destino => destino.toFaClient.arb_care))
                .ForMember(source => source.Act_codi, map => map.MapFrom(destino => destino.toFaClient.act_codi))
                .ReverseMap();

            /// ValidarAportesDto
            CreateMap<GetAporteRequest, ValidarAportesDto>()
                .ForMember(source => source.emp_codi, map => map.MapFrom(destino => destino.Body.emp_codi))
                .ForMember(source => source.top_codi, map => map.MapFrom(destino => destino.Body.top_codi))
                .ForMember(source => source.mte_nume, map => map.MapFrom(destino => destino.Body.mte_nume))
                .ForMember(source => source.mte_anop, map => map.MapFrom(destino => destino.Body.mte_anop))
                .ForMember(source => source.ter_coda, map => map.MapFrom(destino => destino.Body.ter_coda))
                .ForMember(source => source.arb_coda, map => map.MapFrom(destino => destino.Body.arb_coda))
                .ForMember(source => source.cie_codi, map => map.MapFrom(destino => destino.Body.cie_codi))
                .ForMember(source => source.ind_clas, map => map.MapFrom(destino => destino.Body.ind_clas))
                .ForMember(source => source.ind_tipo, map => map.MapFrom(destino => destino.Body.ind_tipo))
                .ReverseMap();

            /// AnularCnMcontDto
            CreateMap<AnularCnMcontRequest, AnularCnMcontDto>()
                .ForMember(source => source.emp_codi, map => map.MapFrom(destino => destino.Body.emp_codi))
                .ForMember(source => source.mco_cont, map => map.MapFrom(destino => destino.Body.mco_cont))
                .ReverseMap();

            /// AplicarCnMcontDto
            CreateMap<AplicarCnMcontRequest, AplicarCnMcontDto>()
                .ForMember(source => source.emp_codi, map => map.MapFrom(destino => destino.Body.emp_codi))
                .ForMember(source => source.mco_cont, map => map.MapFrom(destino => destino.Body.mco_cont))
                .ReverseMap();

            /// RevertirCnMcontDto
            CreateMap<RevertirCnMcontRequest, RevertirCnMcontDto>()
                .ForMember(source => source.emp_codi, map => map.MapFrom(destino => destino.Body.emp_codi))
                .ForMember(source => source.mco_cont, map => map.MapFrom(destino => destino.Body.mco_cont))
                .ReverseMap();

            /// InsertarCnDmconDto
            CreateMap<InsertarCnDmconRequest, InsertarCnDmconDto>()
                .ForMember(source => source.emp_codi, map => map.MapFrom(destino => destino.Body.emp_codi))
                .ForMember(source => source.mco_cont, map => map.MapFrom(destino => destino.Body.mco_cont))
                .ForMember(source => source.cue_codi, map => map.MapFrom(destino => destino.Body.cue_codi))
                .ForMember(source => source.dmc_desc, map => map.MapFrom(destino => destino.Body.dmc_desc))
                .ForMember(source => source.dmc_cant, map => map.MapFrom(destino => destino.Body.dmc_cant))
                .ForMember(source => source.dmc_acti, map => map.MapFrom(destino => destino.Body.dmc_acti))
                .ForMember(source => source.dmc_refe, map => map.MapFrom(destino => destino.Body.dmc_refe))
                .ForMember(source => source.dmc_vadb, map => map.MapFrom(destino => destino.Body.dmc_vadb))
                .ForMember(source => source.dmc_vacr, map => map.MapFrom(destino => destino.Body.dmc_vacr))
                .ForMember(source => source.dmc_vaba, map => map.MapFrom(destino => destino.Body.dmc_vaba))
                .ForMember(source => source.ter_coda, map => map.MapFrom(destino => destino.Body.ter_coda))
                .ForMember(source => source.arb_codc, map => map.MapFrom(destino => destino.Body.arb_codc))
                .ForMember(source => source.arb_codp, map => map.MapFrom(destino => destino.Body.arb_codp))
                .ForMember(source => source.arb_coda, map => map.MapFrom(destino => destino.Body.arb_coda))
                .ForMember(source => source.arb_cods, map => map.MapFrom(destino => destino.Body.arb_cods))
                .ReverseMap();




            /// InsertarCDPDto
            CreateMap<InsertarCDPRequest, InsertarCDPDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.emp_codi))
                .ForMember(source => source.Top_codi, map => map.MapFrom(destino => destino.top_codi))
                .ForMember(source => source.Mpr_nume, map => map.MapFrom(destino => destino.mpr_nume))
                .ForMember(source => source.Mpr_fech, map => map.MapFrom(destino => destino.mpr_fech))
                .ForMember(source => source.Mpr_desc, map => map.MapFrom(destino => destino.mpr_desc))
                .ForMember(source => source.Arb_cods, map => map.MapFrom(destino => destino.arb_cods))
                .ForMember(source => source.Ter_coda, map => map.MapFrom(destino => destino.ter_coda))
                .ForMember(source => source.Mpr_tdos, map => map.MapFrom(destino => destino.mpr_tdos))
                .ForMember(source => source.Mpr_ndos, map => map.MapFrom(destino => destino.mpr_ndos))
                .ForMember(source => source.Uni_codi, map => map.MapFrom(destino => destino.uni_codi))
                .ReverseMap();

            CreateMap<InsertarDetCDPRequest, InsertarDetCDPDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.emp_codi))
                .ForMember(source => source.Mpr_cont, map => map.MapFrom(destino => destino.mpr_cont))
                .ForMember(source => source.Rub_codi, map => map.MapFrom(destino => destino.rub_codi))
                .ForMember(source => source.Pro_codi, map => map.MapFrom(destino => destino.pro_codi))
                .ForMember(source => source.Arb_codc, map => map.MapFrom(destino => destino.arb_codc))
                .ForMember(source => source.Arb_codp, map => map.MapFrom(destino => destino.arb_codp))
                .ForMember(source => source.Arb_coda, map => map.MapFrom(destino => destino.arb_coda))
                .ForMember(source => source.Dmp_valo, map => map.MapFrom(destino => destino.dmp_valo))
                .ForMember(source => source.Dmp_desc, map => map.MapFrom(destino => destino.dmp_desc))
                .ReverseMap();

            /// Cuenta Contable
            CreateMap<InsertarCnMcontRequest, InsertarCnMcontDto>()
                .ForMember(source => source.Mco_nume, map => map.MapFrom(destino => destino.Body.mco_nume))
                .ForMember(source => source.Mco_desc, map => map.MapFrom(destino => destino.Body.mco_desc))
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.Body.emp_codi))
                .ForMember(source => source.Mco_desc, map => map.MapFrom(destino => destino.Body.mco_desc))
                .ForMember(source => source.Mco_fech, map => map.MapFrom(destino => destino.Body.mco_fech))
                .ForMember(source => source.Arb_cods, map => map.MapFrom(destino => destino.Body.arb_cods))
                .ForMember(source => source.Mod_codi, map => map.MapFrom(destino => destino.Body.mod_codi))
                .ReverseMap();

            /// InsertarMovContableDto
            CreateMap<InsertarCnMcontRequest, InsertarCnMcontDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.Body.emp_codi))
                .ForMember(source => source.Top_codi, map => map.MapFrom(destino => destino.Body.top_codi))
                .ForMember(source => source.Mco_nume, map => map.MapFrom(destino => destino.Body.mco_nume))
                .ForMember(source => source.Mco_fech, map => map.MapFrom(destino => destino.Body.mco_fech))
                .ForMember(source => source.Mod_codi, map => map.MapFrom(destino => destino.Body.mod_codi))
                .ForMember(source => source.Arb_cods, map => map.MapFrom(destino => destino.Body.arb_cods))
                .ForMember(source => source.Mco_desc, map => map.MapFrom(destino => destino.Body.mco_desc))
                .ReverseMap();

            /// InsertarFacturaPDto
            CreateMap<InsertarFacturaPRequest, InsertarFacturaPDto>()
                .ForMember(source => source.vDetalle, map => map.MapFrom(destino => destino.Body.pFactura.vDetalle))
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.Body.pFactura.Emp_codi))
                .ForMember(source => source.Top_codi, map => map.MapFrom(destino => destino.Body.pFactura.Top_codi))
                .ForMember(source => source.Fac_nume, map => map.MapFrom(destino => destino.Body.pFactura.Fac_nume))
                .ForMember(source => source.Fac_fech, map => map.MapFrom(destino => destino.Body.pFactura.Fac_fech))
                .ForMember(source => source.Arb_cods, map => map.MapFrom(destino => destino.Body.pFactura.Arb_cods))
                .ForMember(source => source.Fac_desc, map => map.MapFrom(destino => destino.Body.pFactura.Fac_desc))
                .ForMember(source => source.Pvd_coda, map => map.MapFrom(destino => destino.Body.pFactura.Pvd_coda))
                .ForMember(source => source.Dep_codd, map => map.MapFrom(destino => destino.Body.pFactura.Dep_codd))
                .ForMember(source => source.Fac_pref, map => map.MapFrom(destino => destino.Body.pFactura.Fac_pref))
                .ForMember(source => source.Fac_nfap, map => map.MapFrom(destino => destino.Body.pFactura.Fac_nfap))
                .ForMember(source => source.Mon_codi, map => map.MapFrom(destino => destino.Body.pFactura.Mon_codi))
                .ForMember(source => source.Mpr_cont, map => map.MapFrom(destino => destino.Body.pFactura.Mpr_cont))
                .ReverseMap();

            /// InsertarPoFactuDto
            CreateMap<InsertarPoFactuRequest, InsertarPoFactuDto>()
                .ForMember(source => source.vDetalle, map => map.MapFrom(destino => destino.Body.pFactura.vDetalle))
                .ForMember(source => source.vCAdic, map => map.MapFrom(destino => destino.Body.pFactura.vCAdic))
                .ForMember(source => source.vAnticipo, map => map.MapFrom(destino => destino.Body.pFactura.vAnticipo))
                .ForMember(source => source.vCruceAut, map => map.MapFrom(destino => destino.Body.pFactura.vCruceAut))
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.Body.pFactura.Emp_codi))
                .ForMember(source => source.Top_codi, map => map.MapFrom(destino => destino.Body.pFactura.Top_codi))
                .ForMember(source => source.Fac_nume, map => map.MapFrom(destino => destino.Body.pFactura.Fac_nume))
                .ForMember(source => source.Fac_fech, map => map.MapFrom(destino => destino.Body.pFactura.Fac_fech))
                .ForMember(source => source.Arb_cods, map => map.MapFrom(destino => destino.Body.pFactura.Arb_cods))
                .ForMember(source => source.Fac_desc, map => map.MapFrom(destino => destino.Body.pFactura.Fac_desc))
                .ForMember(source => source.Pvd_coda, map => map.MapFrom(destino => destino.Body.pFactura.Pvd_coda))
                .ForMember(source => source.Dep_codd, map => map.MapFrom(destino => destino.Body.pFactura.Dep_codd))
                .ForMember(source => source.Fac_tipo, map => map.MapFrom(destino => destino.Body.pFactura.Fac_tipo))
                .ForMember(source => source.Fac_pref, map => map.MapFrom(destino => destino.Body.pFactura.Fac_pref))
                .ForMember(source => source.Fac_nfap, map => map.MapFrom(destino => destino.Body.pFactura.Fac_nfap))
                .ForMember(source => source.Fac_esta, map => map.MapFrom(destino => destino.Body.pFactura.Fac_esta))
                .ForMember(source => source.Mon_codi, map => map.MapFrom(destino => destino.Body.pFactura.Mon_codi))
                .ForMember(source => source.Fac_base, map => map.MapFrom(destino => destino.Body.pFactura.Fac_base))
                .ForMember(source => source.Bir_cont, map => map.MapFrom(destino => destino.Body.pFactura.Bir_cont))
                .ForMember(source => source.Fac_auto, map => map.MapFrom(destino => destino.Body.pFactura.Fac_auto))
                .ForMember(source => source.Fac_mulc, map => map.MapFrom(destino => destino.Body.pFactura.Fac_mulc))
                .ForMember(source => source.Cal_impu, map => map.MapFrom(destino => destino.Body.pFactura.Cal_impu))
                .ForMember(source => source.Fac_mfev, map => map.MapFrom(destino => destino.Body.pFactura.Fac_mfev))
                .ForMember(source => source.Fac_feve, map => map.MapFrom(destino => destino.Body.pFactura.Fac_feve))
                .ForMember(source => source.Fac_tdis, map => map.MapFrom(destino => destino.Body.pFactura.Fac_tdis))
                .ReverseMap();

            /// InsertarPoFactuPDto
            CreateMap<InsertarPoFactuPRequest, InsertarPoFactuPDto>()
                .ForMember(source => source.vDetalle, map => map.MapFrom(destino => destino.Body.pFactura.vDetalle))
                .ForMember(source => source.vCAdicPoFactuP, map => map.MapFrom(destino => destino.Body.pFactura.vCAdic))
                .ForMember(source => source.vAnticipoPoFactuP, map => map.MapFrom(destino => destino.Body.pFactura.vAnticipo))
                .ForMember(source => source.vCruceAut, map => map.MapFrom(destino => destino.Body.pFactura.vCruceAut))
                .ForMember(source => source.vDocBase, map => map.MapFrom(destino => destino.Body.pFactura.vDocBase))
                .ForMember(source => source.vDocPres, map => map.MapFrom(destino => destino.Body.pFactura.vDocPres))
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.Body.pFactura.Emp_codi))
                .ForMember(source => source.Top_codi, map => map.MapFrom(destino => destino.Body.pFactura.Top_codi))
                .ForMember(source => source.Fac_nume, map => map.MapFrom(destino => destino.Body.pFactura.Fac_nume))
                .ForMember(source => source.Fac_fech, map => map.MapFrom(destino => destino.Body.pFactura.Fac_fech))
                .ForMember(source => source.Arb_cods, map => map.MapFrom(destino => destino.Body.pFactura.Arb_cods))
                .ForMember(source => source.Fac_desc, map => map.MapFrom(destino => destino.Body.pFactura.Fac_desc))
                .ForMember(source => source.Pvd_coda, map => map.MapFrom(destino => destino.Body.pFactura.Pvd_coda))
                .ForMember(source => source.Dep_codd, map => map.MapFrom(destino => destino.Body.pFactura.Dep_codd))
                .ForMember(source => source.Fac_tipo, map => map.MapFrom(destino => destino.Body.pFactura.Fac_tipo))
                .ForMember(source => source.Fac_orig, map => map.MapFrom(destino => destino.Body.pFactura.Fac_orig))
                .ForMember(source => source.Fac_pref, map => map.MapFrom(destino => destino.Body.pFactura.Fac_pref))
                .ForMember(source => source.Fac_nfap, map => map.MapFrom(destino => destino.Body.pFactura.Fac_nfap))
                .ForMember(source => source.Fac_esta, map => map.MapFrom(destino => destino.Body.pFactura.Fac_esta))
                .ForMember(source => source.Mon_codi, map => map.MapFrom(destino => destino.Body.pFactura.Mon_codi))
                .ForMember(source => source.Bir_cont, map => map.MapFrom(destino => destino.Body.pFactura.Bir_cont))
                .ForMember(source => source.Fac_auto, map => map.MapFrom(destino => destino.Body.pFactura.Fac_auto))
                .ForMember(source => source.Fac_refe, map => map.MapFrom(destino => destino.Body.pFactura.Fac_refe))
                .ForMember(source => source.Fac_mulc, map => map.MapFrom(destino => destino.Body.pFactura.Fac_mulc))
                .ForMember(source => source.Cal_impu, map => map.MapFrom(destino => destino.Body.pFactura.Cal_impu))
                .ForMember(source => source.Fac_mfev, map => map.MapFrom(destino => destino.Body.pFactura.Fac_mfev))
                .ForMember(source => source.Fac_feve, map => map.MapFrom(destino => destino.Body.pFactura.Fac_feve))
                .ForMember(source => source.Fac_fpse, map => map.MapFrom(destino => destino.Body.pFactura.Fac_fpse))
                .ReverseMap();




            /// CuentaContableToDto
            CreateMap<GetCuentaContableTORequest, CuentaContableToDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.Body.emp_codi))
                .ForMember(source => source.Top_codi, map => map.MapFrom(destino => destino.Body.top_codi))
                .ReverseMap();

            // ActModalidadCupoDto
            CreateMap<ActModalidadCupoRequest, ActModalidadCupoDto>()
                .ForMember(source => source.Dcl_cupo, map => map.MapFrom(destino => destino.dcl_cupo))
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.emp_codi))
                .ForMember(source => source.Cli_coda, map => map.MapFrom(destino => destino.cli_coda))
                .ForMember(source => source.Arb_codc, map => map.MapFrom(destino => destino.arb_codc))
                .ForMember(source => source.Cli_coda, map => map.MapFrom(destino => destino.cli_coda))
                .ReverseMap();

            /// ChangeNit
            CreateMap<ChangeNitRequest, ChangeNitDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.emp_codi))
                .ForMember(source => source.Ter_coda, map => map.MapFrom(destino => destino.ter_coda))
                .ForMember(source => source.Ter_codn, map => map.MapFrom(destino => destino.ter_codn))
                .ForMember(source => source.Tip_codn, map => map.MapFrom(destino => destino.tip_codn))
                .ReverseMap();

            /// SyncEmpleadoPvdor
            CreateMap<SyncEmpleadoPvdorRequest, SyncEmpleadoPvdorDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.emp_codi))
                .ForMember(source => source.Tip_codi, map => map.MapFrom(destino => destino.tip_codi))
                .ForMember(source => source.Pai_codi, map => map.MapFrom(destino => destino.pai_codi))
                .ForMember(source => source.Dep_codi, map => map.MapFrom(destino => destino.dep_codi))
                .ForMember(source => source.Mun_codi, map => map.MapFrom(destino => destino.mun_codi))
                .ForMember(source => source.Arb_csuc, map => map.MapFrom(destino => destino.arb_csuc))
                .ForMember(source => source.Pvd_coda, map => map.MapFrom(destino => destino.pvd_coda))
                .ForMember(source => source.Pvd_dive, map => map.MapFrom(destino => destino.pvd_dive))
                .ForMember(source => source.Pvd_nomb, map => map.MapFrom(destino => destino.pvd_nomb))
                .ForMember(source => source.Pvd_apel, map => map.MapFrom(destino => destino.pvd_apel))
                .ForMember(source => source.Pvd_noco, map => map.MapFrom(destino => destino.pvd_noco))
                .ForMember(source => source.Cup_nume, map => map.MapFrom(destino => destino.cup_nume))
                .ForMember(source => source.Ban_codi, map => map.MapFrom(destino => destino.ban_codi))
                .ForMember(source => source.Sub_codi, map => map.MapFrom(destino => destino.sub_codi))
                .ForMember(source => source.Cup_tipo, map => map.MapFrom(destino => destino.cup_tipo))
                .ForMember(source => source.Dep_dire, map => map.MapFrom(destino => destino.dep_dire))
                .ForMember(source => source.Dep_ntel, map => map.MapFrom(destino => destino.dep_ntel))
                .ForMember(source => source.Dep_mail, map => map.MapFrom(destino => destino.dep_mail))
                .ReverseMap();

            /// Tercero
            CreateMap<InsertarGnTerceRequest, InsertarTerceroDto>()
                .ForMember(source => source.Emp_codi, map => map.MapFrom(destino => destino.emp_codi))
                .ForMember(source => source.Tip_codi, map => map.MapFrom(destino => destino.tip_codi))
                .ForMember(source => source.Ter_coda, map => map.MapFrom(destino => destino.ter_coda))
                .ForMember(source => source.Ter_dive, map => map.MapFrom(destino => destino.ter_dive))
                .ForMember(source => source.Ter_nomb, map => map.MapFrom(destino => destino.ter_nomb))
                .ForMember(source => source.Ter_apel, map => map.MapFrom(destino => destino.ter_apel))
                .ForMember(source => source.Ter_nocm, map => map.MapFrom(destino => destino.ter_nocm))
                .ForMember(source => source.Mod_codi, map => map.MapFrom(destino => destino.mod_codi))
                .ForMember(source => source.Pai_codi, map => map.MapFrom(destino => destino.pai_codi))
                .ForMember(source => source.Dep_codi, map => map.MapFrom(destino => destino.dep_codi))
                .ForMember(source => source.Mun_codi, map => map.MapFrom(destino => destino.mun_codi))
                .ForMember(source => source.Ter_dire, map => map.MapFrom(destino => destino.ter_dire))
                .ForMember(source => source.Ter_ntel, map => map.MapFrom(destino => destino.ter_ntel))
                .ForMember(source => source.Ter_mail, map => map.MapFrom(destino => destino.ter_mail))
                .ForMember(source => source.Ter_nfax, map => map.MapFrom(destino => destino.ter_nfax))
                .ForMember(source => source.Arb_clte, map => map.MapFrom(destino => destino.arb_clte))
                .ForMember(source => source.Ter_audp, map => map.MapFrom(destino => destino.ter_audp))
                .ReverseMap();

            /// Factura
            CreateMap<AnularFacturaRequest, AnularFacturaDto>()
                .ForMember(source => source.Fac_Cont, map => map.MapFrom(destino => destino.Body.fac_cont))
                .ForMember(source => source.Emp_Codi, map => map.MapFrom(destino => destino.Body.emp_codi))
                .ReverseMap();

            CreateMap<AnularPoFactuRequest, AnularPoFactuDto>()
                .ForMember(source => source.Fac_Cont, map => map.MapFrom(destino => destino.Body.fac_cont))
                .ForMember(source => source.Emp_Codi, map => map.MapFrom(destino => destino.Body.emp_codi))
                .ReverseMap();





        }
    }
}
