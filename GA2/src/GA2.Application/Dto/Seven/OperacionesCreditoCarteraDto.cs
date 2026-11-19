using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Seven
{

    /// <summary>
    /// InsertarTercero
    /// </summary>
    public class InsertarTerceroDto
    {
        public int Emp_codi { get; set; }
        public int Tip_codi { get; set; }
        public string Ter_coda { get; set; }
        public int Ter_dive { get; set; }
        public string Ter_nomb { get; set; }
        public string Ter_apel { get; set; }
        public string Ter_nocm { get; set; }
        public int Mod_codi { get; set; }
        public int Pai_codi { get; set; }
        public int Dep_codi { get; set; }
        public int Mun_codi { get; set; }
        public string Ter_dire { get; set; }
        public string Ter_ntel { get; set; }
        public string Ter_mail { get; set; }
        public string Ter_nfax { get; set; }
        public string Arb_clte { get; set; }
        public string Ter_audp { get; set; }
    }

    /// <summary>
    /// AnularFactura
    /// </summary>
    public class AnularFacturaDto
    {
        public int Emp_Codi { get; set; }
        public int Fac_Cont { get; set; }
    }

    /// <summary>
    /// AnularPoFactu
    /// </summary>
    public class AnularPoFactuDto
    {
        public int Emp_Codi { get; set; }
        public int Fac_Cont { get; set; }
    }

    /// <summary>
    /// AnularCDP
    /// </summary>
    public class AnularCDPDto
    {
        public int Emp_codi { get; set; }
        public int Mpr_cont { get; set; }
    }

    /// <summary>
    /// AplicarCDP
    /// </summary>
    public class AplicarCDPDto
    {
        public int Emp_codi { get; set; }

        public int Mpr_cont { get; set; }
    }

    /// <summary>
    /// CrearCDPDto
    /// </summary>
    public class CrearCDPDto
    {
        public VDetalleCrearCDP vDetalle { get; set; }
        public int Emp_codi { get; set; }
        public int Top_codi { get; set; }
        public int Mpr_nume { get; set; }
        public DateTime Mpr_fech { get; set; }
        public string Mpr_desc { get; set; }
        public string Arb_csuc { get; set; }
        public string Ter_coda { get; set; }
        public string Mpr_tdos { get; set; }
        public int Mpr_ndos { get; set; }
        public int Uni_codi { get; set; }
    } 
    /// <summary>
    /// CrearCDPDto
    /// </summary>
    public class CrearCuotaDto
    {
        public VDetalleCrearCDP vDetalle { get; set; }
        public int Emp_codi { get; set; }
        public int Top_codi { get; set; }
        public int Mpr_nume { get; set; }
        public DateTime Mpr_fech { get; set; }
        public string Mpr_desc { get; set; }
        public string Arb_csuc { get; set; }
        public string Ter_coda { get; set; }
        public string Mpr_tdos { get; set; }
        public int Mpr_ndos { get; set; }
        public int Uni_codi { get; set; }
    }

    public class TSPgDmpre
    {
        public string Rub_codi { get; set; }
        public string Pro_codi { get; set; }
        public string Arb_codc { get; set; }
        public string Arb_coda { get; set; }
        public string Arb_codp { get; set; }
        public string Dmp_valo { get; set; }
        public string Dmp_desc { get; set; }
    }

    public class VDetalleCrearCDP
    {
        public List<TSPgDmpre> TSPgDmpre { get; set; }
    }


    /// <summary>
    /// InsertarCDPDto
    /// </summary>
    public class InsertarCDPDto
    {
        public int Emp_codi { get; set; }
        public int Top_codi { get; set; }
        public int Mpr_nume { get; set; }
        public DateTime Mpr_fech { get; set; }
        public string Mpr_desc { get; set; }
        public string Arb_cods { get; set; }
        public string Ter_coda { get; set; }
        public string Mpr_tdos { get; set; }
        public int Mpr_ndos { get; set; }
        public int Uni_codi { get; set; }
    }

    public class InsertarDetCDPDto
    {
        public int Emp_codi { get; set; }
        public int Mpr_cont { get; set; }
        public string Rub_codi { get; set; }
        public string Pro_codi { get; set; }
        public string Arb_codc { get; set; }
        public string Arb_codp { get; set; }
        public string Arb_coda { get; set; }
        public double Dmp_valo { get; set; }
        public string Dmp_desc { get; set; }
    }

    /// <summary>
    /// CuentaContable
    /// </summary>
    public class CuentaContableDto
    {
        public int Emp_codi { get; set; }
        public string Pro_codi { set; get; }
        public string Dsp_codi { get; set; }
    }

    /// <summary>
    /// CuentaContableTo
    /// </summary>
    public class CuentaContableToDto
    {
        public int Emp_codi { get; set; }
        public int Top_codi { set; get; }

    }

    /// <summary>
    /// ActModalidadCupo
    /// </summary>
    public class ActModalidadCupoDto
    {
        public int Emp_codi { get; set; }
        public string Cli_coda { get; set; }
        public string Arb_codc { get; set; }
        public string Ite_moda { get; set; }
        public double Dcl_cupo { get; set; }
    }

    /// <summary>
    /// CambiarNit
    /// </summary>
    public class ChangeNitDto
    {
        public int Emp_codi { get; set; }
        public string Ter_coda { get; set; }
        public string Ter_codn { get; set; }
        public int Tip_codn { get; set; }
    }

    /// <summary>
    /// EmpleadoProveedor
    /// </summary>
    public class SyncEmpleadoPvdorDto
    {
        public int Emp_codi { get; set; }
        public int Tip_codi { get; set; }
        public string Ter_coda { get; set; }
        public int Cli_dive { get; set; }
        public string Cli_nomb { get; set; }
        public string Cli_apel { get; set; }
        public string Cli_noco { get; set; }
        public int Mod_codi { get; set; }
        public int Pai_codi { get; set; }
        public int Dep_codi { get; set; }
        public int Mun_codi { get; set; }
        public string Cli_coda { get; set; }
        public int Tcl_codi { get; set; }
        public int Cal_codi { get; set; }
        public int Coc_codi { get; set; }
        public int Cim_codi { get; set; }
        public string Arb_csuc { get; set; }
        public string Dcl_dire { get; set; }
        public string Dcl_ntel { get; set; }
        public string Dcl_mail { get; set; }
        public string Dcl_nfax { get; set; }
        public string Arb_clte { get; set; }
        public string Cli_inna { get; set; }
        public double Ven_codi { get; set; }
        public string Arb_ccec { get; set; }
        public int Lis_codi { get; set; }
        public string Dcl_obse { get; set; }
        public string Dcl_apar { get; set; }
        public string Dcl_cloc { get; set; }
        public string Cli_anex { get; set; }
        public string Cli_hpra { get; set; }
        public int Cli_fecm { get; set; }
        public int Cli_feca { get; set; }
        public string Cli_audp { get; set; }
        public string Pvd_coda { get; set; }
        public int Pvd_dive { get; set; }
        public string Pvd_nomb { get; set; }
        public string Pvd_apel { get; set; }
        public string Pvd_noco { get; set; }
        public string Cup_nume { get; set; }
        public int Ban_codi { get; set; }
        public int Sub_codi { get; set; }
        public string Cup_tipo { get; set; }
        public string Dep_dire { get; set; }
        public string Dep_ntel { get; set; }
        public string Dep_mail { get; set; }
    }

    /// <summary>
    /// SyncFaClienDto
    /// </summary>
    public class SyncFaClienDto
    {
        public int Emp_codi { get; set; }
        public int Tip_codi { get; set; }
        public string Ter_coda { get; set; }
        public int Cli_dive { get; set; }
        public string Cli_nomb { get; set; }
        public string Cli_apel { get; set; }
        public string Cli_noco { get; set; }
        public int Mod_codi { get; set; }
        public int Pai_codi { get; set; }
        public int Dep_codi { get; set; }
        public int Mun_codi { get; set; }
        public string Cli_coda { get; set; }
        public int Tcl_codi { get; set; }
        public int Cal_codi { get; set; }
        public int Coc_codi { get; set; }
        public int Cim_codi { get; set; }
        public string Arb_csuc { get; set; }
        public string Dcl_dire { get; set; }
        public string Dcl_ntel { get; set; }
        public string Dcl_mail { get; set; }
        public string Dcl_nfax { get; set; }
        public string Arb_clte { get; set; }
        public string Cli_inna { get; set; }
        public double Ven_codi { get; set; }
        public string Arb_ccec { get; set; }
        public int Lis_codi { get; set; }
        public string Dcl_obse { get; set; }
        public string Dcl_apar { get; set; }
        public string Dcl_cloc { get; set; }
        public string Cli_anex { get; set; }
        public string Cli_hpra { get; set; }
        public int Cli_fecm { get; set; }
        public int Cli_feca { get; set; }
        public string Cli_audp { get; set; }
        public string Pvd_coda { get; set; }
        public int Pvd_dive { get; set; }
        public string Pvd_nomb { get; set; }
        public string Pvd_apel { get; set; }
        public string Pvd_noco { get; set; }
        public string Cup_nume { get; set; }
        public int Ban_codi { get; set; }
        public int Sub_codi { get; set; }
        public string Cup_tipo { get; set; }
        public string Dep_dire { get; set; }
        public string Dep_ntel { get; set; }
        public string Dep_mail { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SyncFaContaDto
    {
        public int Emp_codi { get; set; }
        public string Cli_coda { get; set; }
        public int Dcl_codd { get; set; }
        public int Con_codi { get; set; }
        public string Con_nomb { get; set; }
        public string Con_carg { get; set; }
        public string Con_dire { get; set; }
        public string Con_dibo { get; set; }
        public string Con_tele { get; set; }
        public string Con_nufa { get; set; }
        public string Con_mail { get; set; }
        public DateTime Con_fcum { get; set; }
        public string Con_afic { get; set; }

    }

    /// <summary>
    /// SyncFaClienDto
    /// </summary>
    public class SyncFaClienComplexDto
    {
        public int Emp_codi { get; set; }
        public int Tip_codi { get; set; }
        public string Ter_coda { get; set; }
        public int Cli_dive { get; set; }
        public string Cli_nomb { get; set; }
        public string Cli_apel { get; set; }
        public string Cli_noco { get; set; }
        public int Mod_codi { get; set; }
        public int Pai_codi { get; set; }
        public int Dep_codi { get; set; }
        public int Mun_codi { get; set; }
        public string Cli_coda { get; set; }
        public int Tcl_codi { get; set; }
        public int Cal_codi { get; set; }
        public int Coc_codi { get; set; }
        public int Cim_codi { get; set; }
        public string Arb_csuc { get; set; }
        public string Dcl_dire { get; set; }
        public string Dcl_ntel { get; set; }
        public string Dcl_mail { get; set; }
        public string Dcl_nfax { get; set; }
        public string Arb_clte { get; set; }
        public string Cli_inna { get; set; }
        public double Ven_codi { get; set; }
        public string Arb_ccec { get; set; }
        public int Lis_codi { get; set; }
        public string Dcl_obse { get; set; }
        public string Dcl_apar { get; set; }
        public string Dcl_cloc { get; set; }
        public string Cli_anex { get; set; }
        public string Cli_hpra { get; set; }
        public int Cli_fecm { get; set; }
        public int Cli_feca { get; set; }
        public string Cli_audp { get; set; }
        public string Arb_cpro { get; set; }
        public string Arb_care { get; set; }
        public string Act_codi { get; set; }
    }

    public class SyncGnTerceDto
    {
        public string Emp_codi { get; set; }
        public string Tip_codi { get; set; }
        public string Ter_coda { get; set; }
        public string Ter_dive { get; set; }
        public string Ter_nomb { get; set; }
        public string Ter_apel { get; set; }
        public string Ter_noco { get; set; }
        public string Mod_codi { get; set; }
        public string Pai_codi { get; set; }
        public string Dep_codi { get; set; }
        public string Mun_codi { get; set; }
        public string Ter_dire { get; set; }
        public string Ter_ntel { get; set; }
        public string Ter_mail { get; set; }
        public string Ter_nfax { get; set; }
        public string Ter_audp { get; set; }
    }

    public class SyncPoPvdorDto
    {
        public string Emp_codi { get; set; }
        public string Tip_codi { get; set; }
        public string pvd_coda { get; set; }
        public string pvd_dive { get; set; }
        public string pvd_nomb { get; set; }
        public string pvd_apel { get; set; }
        public string pvd_noco { get; set; }
        public string Pai_codi { get; set; }
        public string Dep_codi { get; set; }
        public string Mun_codi { get; set; }
        public string tpr_codi { get; set; }
        public string pvd_clad { get; set; }
        public string arb_csuc { get; set; }
        public string cal_codi { get; set; }
        public string coc_codi { get; set; }
        public string cim_codi { get; set; }
        public string act_codi { get; set; }
        public string mon_codi { get; set; }
        public string cup_nume { get; set; }
        public string cup_idba { get; set; }
        public string ban_codi { get; set; }
        public string sub_codi { get; set; }
        public string cup_tipo { get; set; }
        public string pvr_riva { get; set; }
        public string pvr_auto { get; set; }
        public string pvr_reau { get; set; }
        public string cup_tdba { get; set; }
        public string cup_tcta { get; set; }
        public string cup_ib04 { get; set; }
        public string cup_ib05 { get; set; }
        public string cup_ib06 { get; set; }
        public string cup_ib07 { get; set; }
        public string cup_ib08 { get; set; }
        public string cup_ib09 { get; set; }
        public string cup_ib10 { get; set; }
        public string Dep_dire { get; set; }
        public string Dep_ntel { get; set; }
        public string Dep_mail { get; set; }
        public string Dep_codd { get; set; }
        public string cup_numi { get; set; }
        public string cup_nomi { get; set; }
        public string ban_coin { get; set; }
        public string ban_dire { get; set; }
        public string cup_coab { get; set; }
        public string cup_coib { get; set; }
        public string cup_coas { get; set; }
        public string cup_cobi { get; set; }
        public string cup_clas { get; set; }
        public string Dep_apar { get; set; }
        public string Dep_nfax { get; set; }
        public string Dep_cloc { get; set; }
        public string Dep_nomb { get; set; }
        public string cup_esta { get; set; }
        public string pvd_audp { get; set; }
    }

    /// <summary>
    /// ValidarAportes
    /// </summary>
    public class ValidarAportesDto
    {
        public int emp_codi { get; set; }
        public int top_codi { get; set; }
        public int mte_nume { get; set; }
        public int mte_anop { get; set; }
        public string ter_coda { get; set; }
        public string arb_coda { get; set; }
        public int cie_codi { get; set; }
        public string ind_clas { get; set; }
        public string ind_tipo { get; set; }
    }

    /// <summary>
    /// AnularMovimientoContable
    /// </summary>
    public class AnularCnMcontDto
    {
        public int emp_codi { get; set; }
        public int mco_cont { get; set; }
    }

    /// <summary>
    /// AplicarCnMcontDto
    /// </summary>
    public class AplicarCnMcontDto
    {
        public int emp_codi { get; set; }
        public int mco_cont { get; set; }
    }

    /// <summary>
    /// RevertirMovimientoContable
    /// </summary>
    public class RevertirCnMcontDto
    {
        public int emp_codi { get; set; }
        public int mco_cont { get; set; }
    }

    /// <summary>
    /// DetalleMovimientoContable
    /// </summary>
    public class InsertarCnDmconDto
    {
        public int emp_codi { get; set; }
        public int mco_cont { get; set; }
        public string cue_codi { get; set; }
        public string dmc_desc { get; set; }
        public double dmc_cant { get; set; }
        public double dmc_acti { get; set; }
        public string dmc_refe { get; set; }
        public decimal dmc_vadb { get; set; }
        public decimal dmc_vacr { get; set; }
        public decimal dmc_vaba { get; set; }
        public string ter_coda { get; set; }
        public string arb_codc { get; set; }
        public string arb_codp { get; set; }
        public string arb_coda { get; set; }
        public string arb_cods { get; set; }
    }

    /// <summary>
    /// EncabezadoMovimientoContable
    /// </summary>
    public class InsertarCnMcontDto
    {
        public int Emp_codi { get; set; }
        public int Top_codi { get; set; }
        public int Mco_nume { get; set; }
        public DateTime Mco_fech { get; set; }
        public int Mod_codi { get; set; }
        public string Arb_cods { get; set; }
        public string Mco_desc { get; set; }
    }

    /// <summary>
    /// InsertarMovContableDto
    /// </summary>
    public class InsertarMovContableDto
    {
        public VDetalleMovContable vDetalle { get; set; }
        public int Emp_codi { get; set; }
        public int Top_codi { get; set; }
        public int Mco_nume { get; set; }
        public DateTime Mco_fech { get; set; }
        public int Mod_codi { get; set; }
        public string Arb_csuc { get; set; }
        public string Mco_desc { get; set; }
    }

    public class TSCnDmcon
    {
        public string Cue_codi { get; set; }
        public string Dmc_desc { get; set; }
        public string Dmc_cant { get; set; }
        public string Dmc_acti { get; set; }
        public string Dmc_refe { get; set; }
        public string Dmc_vadb { get; set; }
        public string Dmc_vacr { get; set; }
        public string Dmc_vaba { get; set; }
        public string Ter_coda { get; set; }
        public string Arb_codc { get; set; }
        public string Arb_coda { get; set; }
        public string Arb_codp { get; set; }
        public string Arb_cods { get; set; }
        public string Ter_codm { get; set; }
    }

    public class VDetalleMovContable
    {
        public List<TSCnDmcon> TSCnDmcon { get; set; }
    }

    /// <summary>
    /// InsertarFacturaP
    /// </summary>
    public class InsertarFacturaPDto
    {
        public VDetalle vDetalle { get; set; }
        public int Emp_codi { get; set; }
        public int Top_codi { get; set; }
        public int Fac_nume { get; set; }
        public DateTime Fac_fech { get; set; }
        public string Arb_cods { get; set; }
        public string Fac_desc { get; set; }
        public string Pvd_coda { get; set; }
        public int Dep_codd { get; set; }
        public string Fac_pref { get; set; }
        public string Fac_nfap { get; set; }
        public int Mon_codi { get; set; }
        public int Mpr_cont { get; set; }
    }
    public class TSPoDfact
    {
        public string Pro_codi { get; set; }
        public string Dfa_dest { get; set; }
        public string Dfa_valo { get; set; }
        public string Arb_codc { get; set; }
        public string Arb_coda { get; set; }
        public string Arb_codp { get; set; }
    }
    public class VDetalle
    {
        public List<TSPoDfact> TSPoDfact { get; set; }
    }
    public class InsertarPoFactuDto
    {
        public VDetallePoFactu vDetalle { get; set; }
        public VCAdic vCAdic { get; set; }
        public VAnticipo vAnticipo { get; set; }
        public VCruceAut vCruceAut { get; set; }
        public string Emp_codi { get; set; }
        public string Top_codi { get; set; }
        public string Fac_nume { get; set; }
        public string Fac_fech { get; set; }
        public string Arb_cods { get; set; }
        public string Fac_desc { get; set; }
        public string Pvd_coda { get; set; }
        public string Dep_codd { get; set; }
        public string Fac_tipo { get; set; }
        public string Fac_pref { get; set; }
        public string Fac_nfap { get; set; }
        public string Fac_esta { get; set; }
        public string Mon_codi { get; set; }
        public string Fac_base { get; set; }
        public string Bir_cont { get; set; }
        public string Fac_auto { get; set; }
        public string Fac_mulc { get; set; }
        public string Cal_impu { get; set; }
        public string Fac_mfev { get; set; }
        public string Fac_feve { get; set; }
        public string Fac_tdis { get; set; }
    }
    public class TSPoCruat1
    {
        public string Top_ccru { get; set; }
        public string Fac_ncru { get; set; }
        public string Fac_fcru { get; set; }
        public string Suc_ccru { get; set; }
        public string Cru_valo { get; set; }
        public string Cxp_cont { get; set; }
    }
    public class TSPoDanti1
    {
        public VDAntic vDAntic { get; set; }
        public string Top_cant { get; set; }
        public string Fac_nant { get; set; }
        public string Fac_fant { get; set; }
        public string Suc_cant { get; set; }
        public string Tip_lega { get; set; }
    }
    public class TSPoDcoad1
    {
        public string Coa_codi { get; set; }
        public string Coa_valo { get; set; }
    }
    public class TSPoDfact1
    {
        public string Bod_codi { get; set; }
        public string Pro_codi { get; set; }
        public string Dfa_dest { get; set; }
        public string Dfa_cant { get; set; }
        public string Dfa_valo { get; set; }
        public string Dfa_desc { get; set; }
        public string Ter_coda { get; set; }
        public string Dfa_refe { get; set; }
        public string Pvd_coda { get; set; }
        public string Dep_codd { get; set; }
        public VDistrib vDistrib { get; set; }
        public VDistribM VDistribM { get; set; }
    }
    public class VAnticipo
    {
        public List<TSPoDanti1> TSPoDanti1 { get; set; }
    }
    public class VCAdic
    {
        public List<TSPoDcoad1> TSPoDcoad1 { get; set; }
    }
    public class VCruceAut
    {
        public List<TSPoCruat1> TSPoCruat1 { get; set; }
    }
    public class VDAntic
    {
        [JsonProperty("xsi:nil")]
        public string XsiNil { get; set; }
    }
    public class VDetallePoFactu
    {
        public List<TSPoDfact1> TSPoDfact1 { get; set; }
    }
    public class VDistrib
    {
        [JsonProperty("xsi:nil")]
        public string XsiNil { get; set; }
    }
    public class VDistribM
    {
        [JsonProperty("xsi:nil")]
        public string XsiNil { get; set; }
    }

    /// <summary>
    /// InsertarPoFactuP
    /// </summary>
    public class InsertarPoFactuPDto
    {
        public VDetallePoFactuP vDetalle { get; set; }
        public VCAdicPoFactuP vCAdicPoFactuP { get; set; }
        public VAnticipoPoFactuP vAnticipoPoFactuP { get; set; }
        public VCruceAut vCruceAut { get; set; }
        public VDocBasePoFactuP vDocBase { get; set; }
        public VDocPresPoFactuP vDocPres { get; set; }
        public int Emp_codi { get; set; }
        public int Top_codi { get; set; }
        public int Fac_nume { get; set; }
        public string Fac_fech { get; set; }
        public string Arb_cods { get; set; }
        public string Fac_desc { get; set; }
        public string Pvd_coda { get; set; }
        public int Dep_codd { get; set; }
        public string Fac_tipo { get; set; }
        public string Fac_orig { get; set; }
        public string Fac_pref { get; set; }
        public string Fac_nfap { get; set; }
        public string Fac_esta { get; set; }
        public int Mon_codi { get; set; }
        public double Bir_cont { get; set; }
        public string Fac_auto { get; set; }
        public string Fac_refe { get; set; }
        public string Fac_mulc { get; set; }
        public int Cal_impu { get; set; }
        public string Fac_mfev { get; set; }
        public DateTime Fac_feve { get; set; }
        public DateTime Fac_fpse { get; set; }
    }
    public class TSCmRdoba2
    {
        public string Mpr_cont { get; set; }
        public string Dmp_cont { get; set; }
        public string Dmp_valo { get; set; }
    }
    public class TSPoCruat2
    {
        public string Top_ccru { get; set; }
        public string Fac_ncru { get; set; }
        public string Fac_fcru { get; set; }
        public string Suc_ccru { get; set; }
        public string Cru_valo { get; set; }
        public string Cxp_cont { get; set; }
    }
    public class TSPoCtrlo2
    {
        public string Top_base { get; set; }
        public string Bas_nume { get; set; }
        public string Bas_fech { get; set; }
        public string Bas_csuc { get; set; }
        public string Bas_cont { get; set; }
    }
    public class TSPoDanti2
    {
        public VDAntic vDAntic { get; set; }
        public string Top_cant { get; set; }
        public string Fac_nant { get; set; }
        public string Fac_fant { get; set; }
        public string Suc_cant { get; set; }
        public string Tip_lega { get; set; }
    }
    public class TSPoDcoad2
    {
        public string Coa_codi { get; set; }
        public string Coa_valo { get; set; }
    }
    public class TSPoDfact2
    {
        public VDistribPoFactuP vDistrib { get; set; }
        public VDistribmPoFactuP vDistribm { get; set; }
        public string Bod_codi { get; set; }
        public string Pro_codi { get; set; }
        public string Dfa_dest { get; set; }
        public string Dfa_cant { get; set; }
        public string Dfa_valo { get; set; }
        public string Dfa_desc { get; set; }
        public string Ter_coda { get; set; }
        public string Dfa_refe { get; set; }
        public string Ctr_cont { get; set; }
        public string Pvd_coda { get; set; }
        public string Dep_codd { get; set; }
    }
    public class VAnticipoPoFactuP
    {
        public List<TSPoDanti2> TSPoDanti2 { get; set; }
    }
    public class VCAdicPoFactuP
    {
        public List<TSPoDcoad2> TSPoDcoad2 { get; set; }
    }
    public class VCruceAutPoFactuP
    {
        public List<TSPoCruat2> TSPoCruat2 { get; set; }
    }
    public class VDAnticPoFactuP
    {
        [JsonProperty("xsi:nil")]
        public string XsiNil { get; set; }
    }
    public class VDetallePoFactuP
    {
        public List<TSPoDfact2> TSPoDfact2 { get; set; }
    }
    public class VDistribPoFactuP
    {
        [JsonProperty("xsi:nil")]
        public string XsiNil { get; set; }
    }
    public class VDistribmPoFactuP
    {
        [JsonProperty("xsi:nil")]
        public string XsiNil { get; set; }
    }
    public class VDocBasePoFactuP
    {
        public TSPoCtrlo2 TSPoCtrlo2 { get; set; }
    }
    public class VDocPresPoFactuP
    {
        public TSCmRdoba2 TSCmRdoba2 { get; set; }
    }

}
