// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.TecDocContext
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShop.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using TecDocEcoSystemDbClassLibrary;

public class TecDocContext : IDisposable
{
    private CarShopMsTecDocContext mstecdoccontext;

    private int tecdocsrctype = 1;

    protected OdbcConnection odbcConnectionTD;

    protected OdbcCommand odbcCommandTD;

    public CarShopMsTecDocContext msTDContext
    {
        get
        {
            if (mstecdoccontext == null)
            {
                mstecdoccontext = new CarShopMsTecDocContext();
            }
            return mstecdoccontext;
        }
    }

    protected OdbcConnection ConnectionTD
    {
        get
        {
            if (odbcConnectionTD == null)
            {
                odbcConnectionTD = new OdbcConnection("Dsn=TECDOC");
            }
            return odbcConnectionTD;
        }
    }

    protected OdbcCommand OdbcCommandTD
    {
        get
        {
            if (odbcCommandTD == null)
            {
                odbcCommandTD = new OdbcCommand();
                odbcCommandTD.Connection = ConnectionTD;
            }
            return odbcCommandTD;
        }
    }

    public TecDocContext(int TecDocSrcType)
    {
        tecdocsrctype = TecDocSrcType;
    }

    public TecDocContext()
    {
    }

    protected string GetLANGUAGES_TEXT(int LNG_ID)
    {
        return "SELECT TOF_LANGUAGES.LNG_ID,  TOF_DES_TEXTS.TEX_TEXT FROM TOF_LANGUAGES INNER JOIN TOF_DESIGNATIONS ON TOF_DESIGNATIONS.DES_ID = TOF_LANGUAGES.LNG_DES_ID INNER JOIN TOF_DES_TEXTS ON TOF_DESIGNATIONS.DES_TEX_ID = TOF_DES_TEXTS.TEX_ID WHERE ((TOF_DESIGNATIONS.DES_LNG_ID = " + LNG_ID.ToString() + "))";
    }

    protected string GetCOUNTRIES_TEXT(int LNG_ID)
    {
        return "SELECT TOF_COUNTRIES.COU_ID, TOF_DES_TEXTS.TEX_TEXT FROM TOF_COUNTRIES INNER JOIN TOF_DESIGNATIONS ON TOF_DESIGNATIONS.DES_ID = TOF_COUNTRIES.COU_DES_ID INNER JOIN TOF_DES_TEXTS ON TOF_DESIGNATIONS.DES_TEX_ID = TOF_DES_TEXTS.TEX_ID WHERE ((TOF_DESIGNATIONS.DES_LNG_ID = " + LNG_ID.ToString() + ")) ";
    }

    protected string GetBRANDS_TEXT()
    {
        return "SELECT MFA_ID, MFA_BRAND FROM TOF_MANUFACTURERS WHERE MFA_PC_MFC = 1 ORDER BY MFA_BRAND ";
    }

    protected string GetMODELS_TEXT(int LNG_ID, int COU_ID, int MFA_ID, int? MOD_ID = default(int?))
    {
        return "SELECT TOF_MODELS.MOD_ID, TOF_DES_TEXTS.TEX_TEXT  FROM TOF_MODELS  INNER JOIN TOF_COUNTRY_DESIGNATIONS ON TOF_COUNTRY_DESIGNATIONS.CDS_ID = TOF_MODELS.MOD_CDS_ID  INNER JOIN TOF_DES_TEXTS ON TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID = TOF_DES_TEXTS.TEX_ID  WHERE ((TOF_MODELS.MOD_MFA_ID = " + MFA_ID.ToString() + " AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + " AND MOD_PC_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1))  " + (MOD_ID.HasValue ? (" AND TOF_MODELS.MOD_ID = " + MOD_ID.Value.ToString()) : "") + " ORDER BY TOF_DES_TEXTS.TEX_TEXT ";
    }

    protected string GetMODELTYPES_TEXT(int LNG_ID, int COU_ID, int MOD_ID, int? fluelId = default(int?), int? modelTypeId = default(int?))
    {
        return "SELECT TOF_TYPES.TYP_ID, TOF_DES_TEXTS.TEX_TEXT AS TEX_TEXT, TOF_DES_TEXTS2.TEX_TEXT AS TYP_KV_BODY_DESCR, TOF_TYPES.TYP_PCON_START,TOF_TYPES.TYP_PCON_END,TOF_TYPES.TYP_KW_FROM,TOF_TYPES.TYP_KW_UPTO,TOF_TYPES.TYP_HP_FROM,TOF_TYPES.TYP_HP_UPTO,TOF_TYPES.TYP_CCM,TOF_TYPES.TYP_VALVES,TOF_TYPES.TYP_CYLINDERS,TOF_TYPES.TYP_DOORS,TOF_DES_TEXTS3.TEX_TEXT AS TYP_KV_ABS_DESCR, TOF_DES_TEXTS4.TEX_TEXT AS TYP_KV_ASR_DESCR, TOF_DES_TEXTS5.TEX_TEXT AS TYP_KV_BRAKE_TYPE_DESCR, TOF_DES_TEXTS6.TEX_TEXT AS TYP_KV_BRAKE_SYST_DESCR, TOF_DES_TEXTS7.TEX_TEXT AS TYP_KV_FUEL_DES_ID_DESCR, TOF_DES_TEXTS8.TEX_TEXT AS TYP_KV_FUEL_SUPPLY_DES_ID_DESCR, TOF_DES_TEXTS9.TEX_TEXT AS TYP_KV_CATALYST_DES_ID_DESCR, TOF_DES_TEXTS1.TEX_TEXT AS TYP_KV_TRANS_DES_ID_DESCR,  TOF_ENGINES.ENG_CODE AS ENG_CODE FROM TOF_TYPES INNER JOIN TOF_COUNTRY_DESIGNATIONS ON TOF_COUNTRY_DESIGNATIONS.CDS_ID = TOF_TYPES.TYP_MMT_CDS_ID AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + " INNER JOIN TOF_DES_TEXTS ON TOF_DES_TEXTS.TEX_ID = TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS2 ON TOF_DESIGNATIONS2.DES_ID = TOF_TYPES.TYP_KV_BODY_DES_ID AND TOF_DESIGNATIONS2.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS2 ON TOF_DES_TEXTS2.TEX_ID = TOF_DESIGNATIONS2.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS3 ON TOF_DESIGNATIONS3.DES_ID = TOF_TYPES.TYP_KV_ABS_DES_ID AND TOF_DESIGNATIONS3.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS3 ON TOF_DES_TEXTS3.TEX_ID = TOF_DESIGNATIONS3.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS4 ON TOF_DESIGNATIONS4.DES_ID = TOF_TYPES.TYP_KV_ASR_DES_ID AND TOF_DESIGNATIONS4.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS4 ON TOF_DES_TEXTS4.TEX_ID = TOF_DESIGNATIONS4.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS5 ON TOF_DESIGNATIONS5.DES_ID = TOF_TYPES.TYP_KV_BRAKE_TYPE_DES_ID AND TOF_DESIGNATIONS5.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS5 ON TOF_DES_TEXTS5.TEX_ID = TOF_DESIGNATIONS5.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS6 ON TOF_DESIGNATIONS6.DES_ID = TOF_TYPES.TYP_KV_BRAKE_SYST_DES_ID AND TOF_DESIGNATIONS6.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS6 ON TOF_DES_TEXTS6.TEX_ID = TOF_DESIGNATIONS6.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS7 ON TOF_DESIGNATIONS7.DES_ID = TOF_TYPES.TYP_KV_FUEL_DES_ID AND TOF_DESIGNATIONS7.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS7 ON TOF_DES_TEXTS7.TEX_ID = TOF_DESIGNATIONS7.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS8 ON TOF_DESIGNATIONS8.DES_ID = TOF_TYPES.TYP_KV_FUEL_SUPPLY_DES_ID AND TOF_DESIGNATIONS8.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS8 ON TOF_DES_TEXTS8.TEX_ID = TOF_DESIGNATIONS8.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS9 ON TOF_DESIGNATIONS9.DES_ID = TOF_TYPES.TYP_KV_CATALYST_DES_ID AND TOF_DESIGNATIONS9.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS9 ON TOF_DES_TEXTS9.TEX_ID = TOF_DESIGNATIONS9.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS1 ON TOF_DESIGNATIONS1.DES_ID = TOF_TYPES.TYP_KV_TRANS_DES_ID AND TOF_DESIGNATIONS1.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS1 ON TOF_DES_TEXTS1.TEX_ID = TOF_DESIGNATIONS1.DES_TEX_ID LEFT JOIN TOF_LINK_TYP_ENG ON LTE_TYP_ID = TYP_ID LEFT JOIN TOF_ENGINES ON ENG_ID = LTE_ENG_ID  WHERE " + (modelTypeId.HasValue ? (" TOF_TYPES.TYP_ID = " + modelTypeId.Value.ToString()) : (" TOF_TYPES.TYP_MOD_ID = " + MOD_ID.ToString() + (fluelId.HasValue ? (" AND TOF_TYPES.TYP_KV_FUEL_DES_ID = " + fluelId.Value.ToString()) : ""))) + " AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + " AND TOF_TYPES.TYP_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1  ORDER BY TOF_TYPES.TYP_SORT";
    }

    protected string GetSIMPLEMODELTYPES_TEXT(int LNG_ID, int COU_ID, int modelTypeId)
    {
        return "SELECT TOF_TYPES.TYP_ID, TOF_DES_TEXTS.TEX_TEXT AS TEX_TEXT, TOF_TYPES.TYP_KV_FUEL_DES_ID, TOF_TYPES.TYP_MOD_ID FROM TOF_TYPES INNER JOIN TOF_COUNTRY_DESIGNATIONS ON TOF_COUNTRY_DESIGNATIONS.CDS_ID = TOF_TYPES.TYP_MMT_CDS_ID INNER JOIN TOF_DES_TEXTS ON TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID = TOF_DES_TEXTS.TEX_ID  WHERE  TOF_TYPES.TYP_ID = " + modelTypeId.ToString() + " AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + " AND TOF_TYPES.TYP_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1  ORDER BY TOF_TYPES.TYP_SORT";
    }

    protected string GetFUELS_TEXT(int LNG_ID)
    {
        return "select DES_ID, TEX_TEXT  from TOF_DESIGNATIONS INNER JOIN TOF_DES_TEXTS ON TOF_DES_TEXTS.TEX_ID = TOF_DESIGNATIONS.DES_TEX_ID where (TOF_DESIGNATIONS.DES_LNG_ID = " + LNG_ID.ToString() + " and TOF_DESIGNATIONS.DES_ID in (select distinct TYP_KV_FUEL_DES_ID from TOF_TYPES where TYP_KV_FUEL_DES_ID is not null)) ";
    }

    protected string GetMODELTYPESTREE_TEXT(int LNG_ID, int TYP_ID)
    {
        return "select str_id, str_level, str_sort, tex_text, str_id_parent  from tof_search_tree  join tof_designations  on str_des_id nljoin des_id and  des_lng_id = " + LNG_ID.ToString() + " join tof_des_texts  on des_tex_id nljoin tex_id  where 1 < 3 and  str_type = 1 and  str_level > 1 and  (select nvl(max(lgs_ga_id), 0)  from tof_link_ga_str  join tof_link_la_typ  on lgs_ga_id nljoin lat_ga_id and  lat_typ_id = " + TYP_ID.ToString() + " where lgs_str_id = str_id and (-1 < 0 or lgs_ga_id in (-1))) > 0  order by str_level, str_sort";
    }

    protected string GetMODELTYPETREEITEMS_TEXT(int LNG_ID, int COU_ID, int TYP_ID, int STR_ID, int tof_assemblyId, int tof_suppliersId)
    {
        string text = " select distinct lat_sup_id sup_id,  nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier,  ga_nr,  tof_des_texts.TEX_TEXT masterbez,  ART_ARTICLE_NR,  ART_ID,  LA_ID,  ga_assembly_tex.TEX_TEXT GA_TEXT from tof_link_la_typ  join tof_generic_articles  on ga_id = lat_ga_id  join tof_designations  on des_id = ga_des_id and  des_lng_id = " + LNG_ID.ToString() + " join tof_des_texts  on tex_id = des_tex_id  left outer join tof_suppliers sup_cou  on sup_cou.sup_id = lat_sup_id and  sup_cou.sup_cou_id = " + COU_ID.ToString() + " left outer join tof_suppliers sup_null  on sup_null.sup_id = lat_sup_id and  sup_null.sup_cou_id is null  left outer join tof_retail_filters trf_equal  on trf_equal.trf_ga_id = ga_id and  trf_equal.trf_sup_id = lat_sup_id and  trf_equal.trf_tsd_id IS NULL  left outer join tof_retail_filters trf_null  on trf_null.trf_ga_id is null and  trf_null.trf_sup_id = lat_sup_id and  trf_null.trf_tsd_id IS NULL  join tof_link_art  on lat_la_id nljoin la_id  join tof_articles  on la_art_id nljoin art_id and  art_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1  left outer join tof_designations ga_assembly_des on ga_des_id_assembly nljoin ga_assembly_des.des_id and " + LNG_ID.ToString() + " nljoin ga_assembly_des.des_lng_id  left outer join tof_des_texts ga_assembly_tex  on ga_assembly_des.des_tex_id nljoin ga_assembly_tex.tex_id  where lat_typ_id = " + TYP_ID.ToString() + " and lat_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1  and (-1 < 0 or lat_ga_id in (-1)) and  lat_ga_id in ( select lgs_ga_id  from tof_link_ga_str  where lgs_str_id = " + STR_ID.ToString() + ") " + ((tof_assemblyId != 0) ? (" AND ga_nr = " + tof_assemblyId.ToString()) : "") + ((tof_suppliersId != 0) ? (" AND lat_sup_id = " + tof_suppliersId.ToString()) : "");
        if (text != "")
        {
            return text;
        }
        text = text + " select distinct lam_sup_id sup_id,  nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier,  ga_nr,  tex_text masterbez,  ART_ARTICLE_NR,  ART_ID ,  LA_ID,  TEX_TEXT  from tof_link_la_mrk  join tof_generic_articles  on ga_id = lam_ga_id  join tof_designations  on des_id = ga_des_id and  des_lng_id = " + LNG_ID.ToString() + " join tof_des_texts  on tex_id = des_tex_id  left outer join tof_suppliers sup_cou  on sup_cou.sup_id = lam_sup_id and  sup_cou.sup_cou_id = " + COU_ID.ToString() + " left outer join tof_suppliers sup_null  on sup_null.sup_id = lam_sup_id and  sup_null.sup_cou_id is null  left outer join tof_retail_filters trf_equal  on trf_equal.trf_ga_id = ga_id and  trf_equal.trf_sup_id = lam_sup_id and  trf_equal.trf_tsd_id IS NULL  left outer join tof_retail_filters trf_null  on trf_null.trf_ga_id is null and  trf_null.trf_sup_id = lam_sup_id and  trf_null.trf_tsd_id IS NULL  join tof_link_art  on lam_la_id nljoin la_id  join tof_articles  on la_art_id nljoin art_id and  art_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1  where lam_mrk_id = " + TYP_ID.ToString() + " and lam_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1  and 1 = 2 and  1 = 6 and  (-1 < 0 or lam_ga_id in (-1)) and  lam_ga_id in ( select lgs_ga_id  from tof_link_ga_str  where lgs_str_id = " + STR_ID.ToString() + ") UNION ";
        text = text + " select distinct lae_sup_id sup_id,  nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier,  ga_nr,  tex_text masterbez,  ART_ARTICLE_NR,  ART_ID ,  LA_ID,  TEX_TEXT  from tof_link_la_eng  join tof_generic_articles  on ga_id = lae_ga_id  join tof_designations  on des_id = ga_des_id and  des_lng_id = " + LNG_ID.ToString() + " join tof_des_texts   on tex_id = des_tex_id  left outer join tof_suppliers sup_cou  on sup_cou.sup_id = lae_sup_id and  sup_cou.sup_cou_id = " + COU_ID.ToString() + " left outer join tof_suppliers sup_null  on sup_null.sup_id = lae_sup_id and  sup_null.sup_cou_id is null  left outer join tof_retail_filters trf_equal  on trf_equal.trf_ga_id = ga_id and  trf_equal.trf_sup_id = lae_sup_id and  trf_equal.trf_tsd_id IS NULL  left outer join tof_retail_filters trf_null  on trf_null.trf_ga_id is null and  trf_null.trf_sup_id = lae_sup_id and  trf_null.trf_tsd_id IS NULL  join tof_link_art  on lae_la_id nljoin la_id  join tof_articles  on la_art_id nljoin art_id and  art_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1  where lae_eng_id = " + TYP_ID.ToString() + " and lae_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1  and 1 = 3 and  (-1 < 0 or lae_ga_id in (-1)) and  lae_ga_id in ( select lgs_ga_id  from tof_link_ga_str  where lgs_str_id = " + STR_ID.ToString() + ") UNION ";
        text = text + " select distinct laa_sup_id sup_id,   nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier,   ga_nr,   tex_text masterbez,   ART_ARTICLE_NR,  ART_ID ,  LA_ID,  TEX_TEXT  from tof_link_la_axl   join tof_generic_articles   on ga_id = laa_ga_id   join tof_designations   on des_id = ga_des_id and   des_lng_id = " + LNG_ID.ToString() + " join tof_des_texts  on tex_id = des_tex_id  left outer join tof_suppliers sup_cou  on sup_cou.sup_id = laa_sup_id and  sup_cou.sup_cou_id = " + COU_ID.ToString() + " left outer join tof_suppliers sup_null  on sup_null.sup_id = laa_sup_id and  sup_null.sup_cou_id is null  left outer join tof_retail_filters trf_equal  on trf_equal.trf_ga_id = ga_id and  trf_equal.trf_sup_id = laa_sup_id and  trf_equal.trf_tsd_id IS NULL  left outer join tof_retail_filters trf_null  on trf_null.trf_ga_id is null and  trf_null.trf_sup_id = laa_sup_id and  trf_null.trf_tsd_id IS NULL  join tof_link_art  on laa_la_id nljoin la_id  join tof_articles  on la_art_id nljoin art_id and  art_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1  where laa_axl_id = " + TYP_ID.ToString() + " and laa_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1  and 1 = 5 and  (-1 < 0 or laa_ga_id in (-1)) and  laa_ga_id in ( select lgs_ga_id  from tof_link_ga_str  where lgs_str_id = " + STR_ID.ToString() + ") ";
        if (text != null)
        {
            return text;
        }
        return text + " select distinct art_sup_id sup_id,  nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier,  ga_nr,  tex_text masterbez,  ART_ARTICLE_NR,  ART_ID  ,  LA_ID,  TEX_TEXT  from tof_generic_articles  join tof_designations  on des_id = ga_des_id and  des_lng_id = " + LNG_ID.ToString() + " join tof_des_texts  on tex_id = des_tex_id  join tof_link_art_ga  on lag_ga_id = ga_id  join tof_articles  on art_id = lag_art_id and  art_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1  left outer join tof_suppliers sup_cou  on sup_cou.sup_id = art_sup_id and  sup_cou.sup_cou_id = " + COU_ID.ToString() + " left outer join tof_suppliers sup_null  on sup_null.sup_id = art_sup_id and  sup_null.sup_cou_id is null  left outer join tof_retail_filters trf_equal  on trf_equal.trf_ga_id = ga_id and  trf_equal.trf_sup_id = art_sup_id and  trf_equal.trf_tsd_id IS NULL  left outer join tof_retail_filters trf_null  on trf_null.trf_ga_id is null and  trf_null.trf_sup_id = art_sup_id and  trf_null.trf_tsd_id IS NULL  where 1 = 4 and  ga_universal = 1 and  (-1 < 0 or ga_id in (-1)) and  ga_id in ( select lgs_ga_id  from tof_link_ga_str  where lgs_str_id = " + STR_ID.ToString() + ") ";
    }

    protected string GetMODELTYPETREEITEMDESCR_TEXT(int LNG_ID, int COU_ID, int ART_ID)
    {
        return " select  cri_tex.tex_text crit_designation,  nvl(acr_value, value_tex.tex_text) crit_value,  nvl(' '||unit_tex.tex_text, '') crit_unit  from tof_article_criteria  join tof_criteria  on acr_cri_id = cri_id  left outer join tof_designations cri_des  on cri_des.des_id = cri_short_des_id and  cri_des.des_lng_id = " + LNG_ID.ToString() + " left outer join tof_des_texts cri_tex  on cri_tex.tex_id = cri_des.des_tex_id  left outer join tof_designations value_des  on value_des.des_id = acr_kv_des_id and  value_des.des_lng_id = " + LNG_ID.ToString() + " left outer join tof_des_texts value_tex  on value_tex.tex_id = value_des.des_tex_id  left outer join tof_designations unit_des  on unit_des.des_id = cri_unit_des_id and  unit_des.des_lng_id = " + LNG_ID.ToString() + " left outer join tof_des_texts unit_tex  on unit_tex.tex_id = unit_des.des_tex_id  where acr_art_id in (" + ART_ID.ToString() + ") and  acr_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 and  acr_display = 1 ";
    }

    protected string GetMODELTYPETREEITEMMANID_TEXT(int LNG_ID, int COU_ID, int ART_ID, int? MFA_ID = default(int?))
    {
        return " SELECT  TOF_ART_LOOKUP.ARL_KIND,  TOF_BRANDS.BRA_BRAND,  TOF_ART_LOOKUP.ARL_DISPLAY_NR  FROM TOF_ART_LOOKUP  INNER JOIN TOF_BRANDS ON TOF_BRANDS.BRA_ID = TOF_ART_LOOKUP.ARL_BRA_ID  WHERE TOF_ART_LOOKUP.ARL_ART_ID = " + ART_ID.ToString() + " AND TOF_ART_LOOKUP.ARL_KIND = '3' " + (MFA_ID.HasValue ? (" AND TOF_BRANDS.BRA_ID = " + MFA_ID.Value.ToString()) : "") + " ORDER BY BRA_BRAND, ARL_DISPLAY_NR ";
    }

    protected string GetANALOGS_TEXT(int LNG_ID, int COU_ID, string ART_ARTICLE_NR, int? GA_NR)
    {
        return " select distinct  art_id,  tof_articles.ART_ARTICLE_NR,  ga_id,  ga_tex.tex_text MASTER_BEZ,  ga_assembly_tex.tex_text ga_assembly,  SUP.SUP_BRAND  from tof_art_lookup  join tof_articles  on arl_art_id nljoin art_id and  1 nljoin art_ctm subrange(" + COU_ID.ToString() + " cast integer)  JOIN TOF_SUPPLIERS SUP ON (SUP.SUP_ID = tof_articles.ART_SUP_ID)  JOIN TOF_DESIGNATIONS DES ON (DES.DES_ID = tof_articles.ART_COMPLETE_DES_ID)  JOIN TOF_DES_TEXTS TEX ON (DES.DES_TEX_ID = TEX.TEX_ID)  join tof_link_art_ga  on lag_art_id = art_id  join tof_generic_articles  on ga_id = lag_ga_id and  ((ga_universal = 0 and  ga_id = ga_nr) or ga_universal = 1)  join tof_designations ga_des  on ga_des_id nljoin ga_des.des_id and  ga_des.des_lng_id = " + LNG_ID.ToString() + " join tof_des_texts ga_tex on ga_des.des_tex_id nljoin ga_tex.tex_id left outer join tof_designations ga_assembly_des on ga_des_id_assembly nljoin ga_assembly_des.des_id and " + LNG_ID.ToString() + " nljoin ga_assembly_des.des_lng_id  left outer join tof_des_texts ga_assembly_tex  on ga_assembly_des.des_tex_id nljoin ga_assembly_tex.tex_id  where arl_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 and  arl_kind in ('1','2','3','4','5') and   ( (0 = 1 and arl_search_number like '" + ART_ARTICLE_NR + "' ) or  ( 0 = 0 and arl_search_number = '" + ART_ARTICLE_NR + "' ) ) and " + (GA_NR.HasValue ? (" ga_id = " + GA_NR.Value.ToString()) : " (-1 = -1  or  ga_id = -1 ) ") + " UNION ALL  select distinct   art_id,  tof_articles.ART_ARTICLE_NR,  ga_id,   ga_tex.tex_text MASTER_BEZ,   ga_assembly_tex.tex_text ga_assembly,  SUP.SUP_BRAND  from tof_tecsel_dealers   join tof_tecsel_prices   on tsd_id nljoin tsp_tsd_id and   tsp_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 and   ( (0 = 1 and tsp_search_number like '" + ART_ARTICLE_NR + "' ) or  ( 0 = 0 and  tsp_search_number = '" + ART_ARTICLE_NR + "' ) )   join tof_articles  on tsp_art_id nljoin art_id and  1 nljoin art_ctm subrange(" + COU_ID.ToString() + " cast integer)  JOIN TOF_SUPPLIERS SUP ON (SUP.SUP_ID = tof_articles.ART_SUP_ID)  JOIN TOF_DESIGNATIONS DES ON (DES.DES_ID = tof_articles.ART_COMPLETE_DES_ID)  JOIN TOF_DES_TEXTS TEX ON (DES.DES_TEX_ID = TEX.TEX_ID)  join tof_link_art_ga  on lag_art_id = art_id  join tof_generic_articles  on ga_id = lag_ga_id and  ((ga_universal = 0 and  ga_id = ga_nr) or ga_universal = 1)  join tof_designations ga_des  on ga_des_id nljoin ga_des.des_id and  ga_des.des_lng_id = " + LNG_ID.ToString() + " join tof_des_texts ga_tex  on ga_des.des_tex_id nljoin ga_tex.tex_id  left outer join tof_designations ga_assembly_des  on ga_des_id_assembly nljoin ga_assembly_des.des_id and " + LNG_ID.ToString() + " nljoin ga_assembly_des.des_lng_id  left outer join tof_des_texts ga_assembly_tex  on ga_assembly_des.des_tex_id nljoin ga_assembly_tex.tex_id  where tsd_id = -1 and " + (GA_NR.HasValue ? (" ga_id = " + GA_NR.Value.ToString()) : " (-1 = -1  or  ga_id = -1 )") + " order by ga_id  ";
    }

    protected string GetArticleByID_TEXT(int LNG_ID, int COU_ID, string ART_ARTICLE_NR, int searchType)
    {
        if (searchType < 2)
        {
            if (searchType == 0)
            {
                return "SELECT DISTINCT TOF_ARTICLES.ART_ID AS ART_ID, TOF_ARTICLES.ART_ARTICLE_NR AS ART_ARTICLE_NR, TOF_SUPPLIERS.SUP_ID AS ART_SUP_ID, TOF_SUPPLIERS.SUP_BRAND SUPPLIER, TOF_DES_TEXTS.TEX_TEXT AS ARTICLE_DESCR, TOF_LINK_ART_GA.LAG_GA_ID AS LAG_GA_ID FROM TOF_ART_LOOKUP LEFT JOIN TOF_BRANDS ON TOF_BRANDS.BRA_ID = TOF_ART_LOOKUP.ARL_BRA_ID INNER JOIN TOF_ARTICLES ON TOF_ARTICLES.ART_ID = TOF_ART_LOOKUP.ARL_ART_ID INNER JOIN  TOF_LINK_ART_GA ON TOF_LINK_ART_GA.LAG_ART_ID = TOF_ARTICLES.ART_ID INNER JOIN TOF_SUPPLIERS ON TOF_SUPPLIERS.SUP_ID = TOF_ARTICLES.ART_SUP_ID INNER JOIN TOF_DESIGNATIONS ON TOF_DESIGNATIONS.DES_ID = TOF_ARTICLES.ART_COMPLETE_DES_ID INNER JOIN TOF_DES_TEXTS ON TOF_DES_TEXTS.TEX_ID = TOF_DESIGNATIONS.DES_TEX_ID WHERE  (TOF_ART_LOOKUP.ARL_SEARCH_NUMBER = '" + ART_ARTICLE_NR + "') AND  (TOF_ART_LOOKUP.ARL_KIND ='1') AND (TOF_DESIGNATIONS.DES_LNG_ID = " + LNG_ID.ToString() + ")";
            }
            return " select  ART_ID,  ART_ARTICLE_NR,  ART_SUP_ID,  nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier,  art_tex.tex_text ARTICLE_DESCR,  lag_ga_id  from tof_articles  join tof_link_art_ga  on lag_art_id = art_id  left outer join tof_designations art_des  on ART_COMPLETE_DES_ID nljoin art_des.des_id  and " + LNG_ID.ToString() + " nljoin art_des.des_lng_id  left outer join tof_des_texts art_tex  on art_des.des_tex_id nljoin art_tex.tex_id  left outer join tof_suppliers sup_cou  on sup_cou.sup_id = ART_SUP_ID and  sup_cou.sup_cou_id = " + LNG_ID.ToString() + " left outer join tof_suppliers sup_null  on sup_null.sup_id = ART_SUP_ID and  sup_null.sup_cou_id is null  where  ART_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1   AND " + ((searchType == 0) ? (" ART_ARTICLE_NR = '" + ART_ARTICLE_NR + "' ") : ("ART_ID=" + ART_ARTICLE_NR));
        }
        return " select  ART_ID,  ART_ARTICLE_NR,  ART_SUP_ID,  nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier,  art_tex.tex_text ARTICLE_DESCR,  lag_ga_id  from tof_art_lookup  inner join tof_articles on art_id = ARL_ART_ID  join tof_link_art_ga  on lag_art_id = art_id  left outer join tof_designations art_des  on ART_COMPLETE_DES_ID nljoin art_des.des_id  and " + LNG_ID.ToString() + " nljoin art_des.des_lng_id  left outer join tof_des_texts art_tex  on art_des.des_tex_id nljoin art_tex.tex_id  left outer join tof_suppliers sup_cou  on sup_cou.sup_id = ART_SUP_ID and  sup_cou.sup_cou_id = " + LNG_ID.ToString() + " left outer join tof_suppliers sup_null  on sup_null.sup_id = ART_SUP_ID and  sup_null.sup_cou_id is null  where " + ((searchType != 2) ? " arl_kind in ('5') and " : " arl_kind in ('3') and arl_search_number=arl_display_nr and ") + " arl_search_number='" + ART_ARTICLE_NR + "'";
    }

    protected string GetAllArticle_TEXT(int LNG_ID, int COU_ID)
    {
        return " select  ART_ID,  ART_ARTICLE_NR,  ART_SUP_ID,  nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier,  art_tex.tex_text ARTICLE_DESCR,  ARL_DISPLAY_NR  from tof_articles  left outer join tof_designations art_des  on ART_COMPLETE_DES_ID nljoin art_des.des_id  and " + LNG_ID.ToString() + " nljoin art_des.des_lng_id  left outer join tof_des_texts art_tex  on art_des.des_tex_id nljoin art_tex.tex_id  left outer join tof_suppliers sup_cou  on sup_cou.sup_id = ART_SUP_ID and  sup_cou.sup_cou_id = " + LNG_ID.ToString() + " left outer join tof_suppliers sup_null  on sup_null.sup_id = ART_SUP_ID and  sup_null.sup_cou_id is null  left outer join TOF_ART_LOOKUP TFARTLKP  on TFARTLKP.ARL_ART_ID = ART_ID and TFARTLKP.arl_kind in ('5')  where  ART_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1  ";
    }

    protected string GetArticleApplic_TEXT(int LNG_ID, int COU_ID, int ART_ID)
    {
        return "SELECT TOF_TYPES.TYP_ID, TOF_DES_TEXTS.TEX_TEXT AS TEX_TEXT, TOF_DES_TEXTS2.TEX_TEXT AS TYP_KV_BODY_DESCR, TOF_TYPES.TYP_PCON_START,TOF_TYPES.TYP_PCON_END,TOF_TYPES.TYP_KW_FROM,TOF_TYPES.TYP_KW_UPTO,TOF_TYPES.TYP_HP_FROM,TOF_TYPES.TYP_HP_UPTO,TOF_TYPES.TYP_CCM,TOF_TYPES.TYP_VALVES,TOF_TYPES.TYP_CYLINDERS,TOF_TYPES.TYP_DOORS,TOF_DES_TEXTS3.TEX_TEXT AS TYP_KV_ABS_DESCR, TOF_DES_TEXTS4.TEX_TEXT AS TYP_KV_ASR_DESCR, TOF_DES_TEXTS5.TEX_TEXT AS TYP_KV_BRAKE_TYPE_DESCR, TOF_DES_TEXTS6.TEX_TEXT AS TYP_KV_BRAKE_SYST_DESCR, TOF_DES_TEXTS7.TEX_TEXT AS TYP_KV_FUEL_DES_ID_DESCR, TOF_DES_TEXTS8.TEX_TEXT AS TYP_KV_FUEL_SUPPLY_DES_ID_DESCR, TOF_DES_TEXTS9.TEX_TEXT AS TYP_KV_CATALYST_DES_ID_DESCR, TOF_DES_TEXTS1.TEX_TEXT AS TYP_KV_TRANS_DES_ID_DESCR,  TOF_ENGINES.ENG_CODE AS ENG_CODE FROM TOF_LINK_ART INNER JOIN TOF_LINK_LA_TYP ON LAT_LA_ID = LA_ID INNER JOIN TOF_TYPES ON TYP_ID = LAT_TYP_ID INNER JOIN TOF_COUNTRY_DESIGNATIONS ON TOF_COUNTRY_DESIGNATIONS.CDS_ID = TOF_TYPES.TYP_MMT_CDS_ID AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + " INNER JOIN TOF_DES_TEXTS ON TOF_DES_TEXTS.TEX_ID = TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS2 ON TOF_DESIGNATIONS2.DES_ID = TOF_TYPES.TYP_KV_BODY_DES_ID AND TOF_DESIGNATIONS2.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS2 ON TOF_DES_TEXTS2.TEX_ID = TOF_DESIGNATIONS2.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS3 ON TOF_DESIGNATIONS3.DES_ID = TOF_TYPES.TYP_KV_ABS_DES_ID AND TOF_DESIGNATIONS3.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS3 ON TOF_DES_TEXTS3.TEX_ID = TOF_DESIGNATIONS3.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS4 ON TOF_DESIGNATIONS4.DES_ID = TOF_TYPES.TYP_KV_ASR_DES_ID AND TOF_DESIGNATIONS4.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS4 ON TOF_DES_TEXTS4.TEX_ID = TOF_DESIGNATIONS4.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS5 ON TOF_DESIGNATIONS5.DES_ID = TOF_TYPES.TYP_KV_BRAKE_TYPE_DES_ID AND TOF_DESIGNATIONS5.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS5 ON TOF_DES_TEXTS5.TEX_ID = TOF_DESIGNATIONS5.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS6 ON TOF_DESIGNATIONS6.DES_ID = TOF_TYPES.TYP_KV_BRAKE_SYST_DES_ID AND TOF_DESIGNATIONS6.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS6 ON TOF_DES_TEXTS6.TEX_ID = TOF_DESIGNATIONS6.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS7 ON TOF_DESIGNATIONS7.DES_ID = TOF_TYPES.TYP_KV_FUEL_DES_ID AND TOF_DESIGNATIONS7.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS7 ON TOF_DES_TEXTS7.TEX_ID = TOF_DESIGNATIONS7.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS8 ON TOF_DESIGNATIONS8.DES_ID = TOF_TYPES.TYP_KV_FUEL_SUPPLY_DES_ID AND TOF_DESIGNATIONS8.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS8 ON TOF_DES_TEXTS8.TEX_ID = TOF_DESIGNATIONS8.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS9 ON TOF_DESIGNATIONS9.DES_ID = TOF_TYPES.TYP_KV_CATALYST_DES_ID AND TOF_DESIGNATIONS9.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS9 ON TOF_DES_TEXTS9.TEX_ID = TOF_DESIGNATIONS9.DES_TEX_ID LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS1 ON TOF_DESIGNATIONS1.DES_ID = TOF_TYPES.TYP_KV_TRANS_DES_ID AND TOF_DESIGNATIONS1.DES_LNG_ID = " + LNG_ID.ToString() + " LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS1 ON TOF_DES_TEXTS1.TEX_ID = TOF_DESIGNATIONS1.DES_TEX_ID LEFT JOIN TOF_LINK_TYP_ENG ON LTE_TYP_ID = TYP_ID LEFT JOIN TOF_ENGINES ON ENG_ID = LTE_ENG_ID  WHERE  LA_ART_ID =  " + ART_ID + "  AND  LA_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1  AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + " AND TOF_TYPES.TYP_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1  ORDER BY TOF_TYPES.TYP_SORT";
    }

    protected string GetArticleEan_TEXT(int ART_ID)
    {
        return " select ARL_DISPLAY_NR  from TOF_ART_LOOKUP  WHERE arl_kind in ('5') and ARL_ART_ID = " + ART_ID.ToString();
    }

    protected string GetPhoto_TEXT(int GRD_FLD, int GRD_ID)
    {
        return "SELECT GRD_GRAPHIC FROM TOF_GRA_DATA_" + GRD_FLD.ToString() + " WHERE GRD_ID = " + GRD_ID.ToString();
    }

    protected string GetArticleGroup_text()
    {
        return " select   art_id,  ga_nr  from  tof_articles  join tof_link_art_ga  on lag_art_id = art_id  join tof_generic_articles  on ga_id = lag_ga_id and  ((ga_universal = 0 and  ga_id = ga_nr) or ga_universal = 1) ";
    }

    protected string GetArticleBrand_TEXT()
    {
        return " SELECT  BRA_ID,  BRA_BRAND,  FROM TOF_BRANDS ";
    }

    protected void DoOpenConnectionTD()
    {
        if (ConnectionTD.State == ConnectionState.Closed)
        {
            ConnectionTD.Open();
        }
    }

    public List<LANGUAGES_TD> GetLANGUAGES(int LNG_ID)
    {
        List<LANGUAGES_TD> list = new List<LANGUAGES_TD>();
        if (tecdocsrctype == 1)
        {
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetLANGUAGES_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                while (odbcDataReader.Read())
                {
                    LANGUAGES_TD lANGUAGES_TD = new LANGUAGES_TD();
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        lANGUAGES_TD.LNG_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                    }
                    if (!odbcDataReader.IsDBNull(1))
                    {
                        lANGUAGES_TD.TEX_TEXT = odbcDataReader.GetString(1);
                    }
                    list.Add(lANGUAGES_TD);
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        list.Add(new LANGUAGES_TD
        {
            LNG_ID = 16,
            TEX_TEXT = "Русский"
        });
        return list;
    }

    public async Task<List<LANGUAGES_TD>> GetLANGUAGESAsync(int LNG_ID)
    {
        List<LANGUAGES_TD> aResult = new List<LANGUAGES_TD>();
        if (tecdocsrctype == 1)
        {
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetLANGUAGES_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                while (await rd.ReadAsync())
                {
                    LANGUAGES_TD lANGUAGES_TD = new LANGUAGES_TD();
                    if (!rd.IsDBNull(0))
                    {
                        lANGUAGES_TD.LNG_ID = Convert.ToInt32(rd.GetValue(0));
                    }
                    if (!rd.IsDBNull(1))
                    {
                        lANGUAGES_TD.TEX_TEXT = rd.GetString(1);
                    }
                    aResult.Add(lANGUAGES_TD);
                }
                return aResult;
            }
            finally
            {
                rd.Close();
            }
        }
        aResult.Add(new LANGUAGES_TD
        {
            LNG_ID = 16,
            TEX_TEXT = "Русский"
        });
        return aResult;
    }

    public List<COUNTRIES_TD> GetCOUNTRIES(int LNG_ID)
    {
        List<COUNTRIES_TD> list = new List<COUNTRIES_TD>();
        if (tecdocsrctype == 1)
        {
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetCOUNTRIES_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                while (odbcDataReader.Read())
                {
                    COUNTRIES_TD cOUNTRIES_TD = new COUNTRIES_TD();
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        cOUNTRIES_TD.COU_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                    }
                    if (!odbcDataReader.IsDBNull(1))
                    {
                        cOUNTRIES_TD.TEX_TEXT = odbcDataReader.GetString(1);
                    }
                    list.Add(cOUNTRIES_TD);
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        list.Add(new COUNTRIES_TD
        {
            COU_ID = 249,
            TEX_TEXT = "dummy for all countries by dd1gj"
        });
        return list;
    }

    public async Task<List<COUNTRIES_TD>> GetCOUNTRIESAsync(int LNG_ID)
    {
        List<COUNTRIES_TD> aResult = new List<COUNTRIES_TD>();
        if (tecdocsrctype == 1)
        {
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetCOUNTRIES_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                while (await rd.ReadAsync())
                {
                    COUNTRIES_TD cOUNTRIES_TD = new COUNTRIES_TD();
                    if (!rd.IsDBNull(0))
                    {
                        cOUNTRIES_TD.COU_ID = Convert.ToInt32(rd.GetValue(0));
                    }
                    if (!rd.IsDBNull(1))
                    {
                        cOUNTRIES_TD.TEX_TEXT = rd.GetString(1);
                    }
                    aResult.Add(cOUNTRIES_TD);
                }
                return aResult;
            }
            finally
            {
                rd.Close();
            }
        }
        aResult.Add(new COUNTRIES_TD
        {
            COU_ID = 249,
            TEX_TEXT = "dummy for all countries by dd1gj"
        });
        return aResult;
    }

    public List<BRAND_TD> GetBRANDS()
    {
        List<BRAND_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<BRAND_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetBRANDS_TEXT();
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                while (odbcDataReader.Read())
                {
                    BRAND_TD bRAND_TD = new BRAND_TD();
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        bRAND_TD.MFA_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                    }
                    if (!odbcDataReader.IsDBNull(1))
                    {
                        bRAND_TD.MFA_BRAND = odbcDataReader.GetString(1);
                    }
                    list.Add(bRAND_TD);
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            return (from e in msTDContext.EnterpriseCarBrandTDES
                    select new BRAND_TD
                    {
                        MFA_ID = e.EnterpriseCarBrandId,
                        MFA_BRAND = e.EnterpriseCarBrandName
                    }).ToList();
        }
        return new List<BRAND_TD>();
    }

    public async Task<List<BRAND_TD>> GetBRANDSAsync()
    {
        if (tecdocsrctype == 1)
        {
            List<BRAND_TD> aResult = new List<BRAND_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetBRANDS_TEXT();
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                while (await rd.ReadAsync())
                {
                    BRAND_TD bRAND_TD = new BRAND_TD();
                    if (!rd.IsDBNull(0))
                    {
                        bRAND_TD.MFA_ID = Convert.ToInt32(rd.GetValue(0));
                    }
                    if (!rd.IsDBNull(1))
                    {
                        bRAND_TD.MFA_BRAND = rd.GetString(1);
                    }
                    aResult.Add(bRAND_TD);
                }
            }
            finally
            {
                rd.Close();
            }
            return (from e in aResult
                    orderby e.MFA_BRAND
                    select e).ToList();
        }
        if (tecdocsrctype == 2)
        {
            return await (from e in msTDContext.EnterpriseCarBrandTDES
                          select new BRAND_TD
                          {
                              MFA_ID = e.EnterpriseCarBrandId,
                              MFA_BRAND = e.EnterpriseCarBrandName
                          } into eee
                          orderby eee.MFA_BRAND
                          select eee).ToListAsync();
        }
        return new List<BRAND_TD>();
    }

    public List<MODEL_TD> GetMODELS(int LNG_ID, int COU_ID, int MFA_ID, int? MOD_ID = default(int?))
    {
        List<MODEL_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<MODEL_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELS_TEXT(LNG_ID, COU_ID, MFA_ID, MOD_ID);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                while (odbcDataReader.Read())
                {
                    MODEL_TD mODEL_TD = new MODEL_TD();
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        mODEL_TD.MOD_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                    }
                    if (!odbcDataReader.IsDBNull(1))
                    {
                        mODEL_TD.TEX_TEXT = odbcDataReader.GetString(1);
                    }
                    list.Add(mODEL_TD);
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            if (MOD_ID.HasValue && MOD_ID.Value > 0)
            {
                return (from e in msTDContext.EnterpriseCarModelTypeTDES
                        where e.EnterpriseCarBrandId == MFA_ID && e.EnterpriseCarModelTypeId == MOD_ID.Value
                        orderby e.EnterpriseCarModelTypeName
                        select new MODEL_TD
                        {
                            MOD_ID = e.EnterpriseCarModelTypeId,
                            TEX_TEXT = e.EnterpriseCarModelTypeName
                        }).ToList();
            }
            return (from e in msTDContext.EnterpriseCarModelTypeTDES
                    where e.EnterpriseCarBrandId == MFA_ID
                    orderby e.EnterpriseCarModelTypeName
                    select new MODEL_TD
                    {
                        MOD_ID = e.EnterpriseCarModelTypeId,
                        TEX_TEXT = e.EnterpriseCarModelTypeName
                    }).ToList();
        }
        return new List<MODEL_TD>();
    }

    public async Task<List<MODEL_TD>> GetMODELSAsync(int LNG_ID, int COU_ID, int MFA_ID, int? MOD_ID = default(int?))
    {
        if (tecdocsrctype == 1)
        {
            List<MODEL_TD> aResult = new List<MODEL_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELS_TEXT(LNG_ID, COU_ID, MFA_ID, MOD_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                while (await rd.ReadAsync())
                {
                    MODEL_TD mODEL_TD = new MODEL_TD();
                    if (!rd.IsDBNull(0))
                    {
                        mODEL_TD.MOD_ID = Convert.ToInt32(rd.GetValue(0));
                    }
                    if (!rd.IsDBNull(1))
                    {
                        mODEL_TD.TEX_TEXT = rd.GetString(1);
                    }
                    aResult.Add(mODEL_TD);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        if (tecdocsrctype == 2)
        {
            if (MOD_ID.HasValue && MOD_ID.Value > 0)
            {
                return await (from e in msTDContext.EnterpriseCarModelTypeTDES
                              where e.EnterpriseCarBrandId == MFA_ID && e.EnterpriseCarModelTypeId == MOD_ID.Value
                              orderby e.EnterpriseCarModelTypeName
                              select new MODEL_TD
                              {
                                  MOD_ID = e.EnterpriseCarModelTypeId,
                                  TEX_TEXT = e.EnterpriseCarModelTypeName
                              }).ToListAsync();
            }
            return await (from e in msTDContext.EnterpriseCarModelTypeTDES
                          where e.EnterpriseCarBrandId == MFA_ID
                          orderby e.EnterpriseCarModelTypeName
                          select new MODEL_TD
                          {
                              MOD_ID = e.EnterpriseCarModelTypeId,
                              TEX_TEXT = e.EnterpriseCarModelTypeName
                          }).ToListAsync();
        }
        return new List<MODEL_TD>();
    }

    public List<MODELTYPE_TD> GetMODELTYPES(int LNG_ID, int COU_ID, int MOD_ID, int? fluelId = default(int?), int? modelTypeId = default(int?))
    {
        List<MODELTYPE_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<MODELTYPE_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPES_TEXT(LNG_ID, COU_ID, MOD_ID, fluelId, modelTypeId);
            DoOpenConnectionTD();
            int num = 0;
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                int num2 = 0;
                while (odbcDataReader.Read())
                {
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        num2 = Convert.ToInt32(odbcDataReader.GetValue(0));
                    }
                    if (num2 != num)
                    {
                        num = num2;
                        MODELTYPE_TD mODELTYPE_TD = new MODELTYPE_TD();
                        mODELTYPE_TD.TYP_ID = num;
                        if (!odbcDataReader.IsDBNull(1))
                        {
                            mODELTYPE_TD.TEX_TEXT = Convert.ToString(odbcDataReader.GetValue(1));
                        }
                        if (!odbcDataReader.IsDBNull(2))
                        {
                            mODELTYPE_TD.TYP_KV_BODY = Convert.ToString(odbcDataReader.GetValue(2));
                        }
                        if (!odbcDataReader.IsDBNull(3))
                        {
                            mODELTYPE_TD.TYP_PCON_START = Convert.ToString(odbcDataReader.GetValue(3));
                            if (!string.IsNullOrEmpty(mODELTYPE_TD.TYP_PCON_START) && mODELTYPE_TD.TYP_PCON_START.Length > 4)
                            {
                                mODELTYPE_TD.TYP_PCON_START = mODELTYPE_TD.TYP_PCON_START.Substring(0, 4) + "-" + mODELTYPE_TD.TYP_PCON_START.Substring(4);
                            }
                        }
                        if (!odbcDataReader.IsDBNull(4))
                        {
                            mODELTYPE_TD.TYP_PCON_END = Convert.ToString(odbcDataReader.GetValue(4));
                            if (!string.IsNullOrEmpty(mODELTYPE_TD.TYP_PCON_END) && mODELTYPE_TD.TYP_PCON_END.Length > 4)
                            {
                                mODELTYPE_TD.TYP_PCON_END = mODELTYPE_TD.TYP_PCON_END.Substring(0, 4) + "-" + mODELTYPE_TD.TYP_PCON_END.Substring(4);
                            }
                        }
                        if (!odbcDataReader.IsDBNull(5))
                        {
                            mODELTYPE_TD.TYP_KW_FROM = Convert.ToString(odbcDataReader.GetValue(5));
                        }
                        if (!odbcDataReader.IsDBNull(6))
                        {
                            mODELTYPE_TD.TYP_KW_UPTO = Convert.ToString(odbcDataReader.GetValue(6));
                        }
                        if (!odbcDataReader.IsDBNull(7))
                        {
                            mODELTYPE_TD.TYP_HP_FROM = Convert.ToString(odbcDataReader.GetValue(7));
                        }
                        if (!odbcDataReader.IsDBNull(8))
                        {
                            mODELTYPE_TD.TYP_HP_UPTO = Convert.ToString(odbcDataReader.GetValue(8));
                        }
                        if (!odbcDataReader.IsDBNull(9))
                        {
                            mODELTYPE_TD.TYP_CCM = Convert.ToString(odbcDataReader.GetValue(9));
                        }
                        if (!odbcDataReader.IsDBNull(10))
                        {
                            mODELTYPE_TD.TYP_VALVES = Convert.ToString(odbcDataReader.GetValue(10));
                        }
                        if (!odbcDataReader.IsDBNull(11))
                        {
                            mODELTYPE_TD.TYP_CYLINDERS = Convert.ToString(odbcDataReader.GetValue(11));
                        }
                        if (!odbcDataReader.IsDBNull(12))
                        {
                            mODELTYPE_TD.TYP_DOORS = Convert.ToString(odbcDataReader.GetValue(12));
                        }
                        if (!odbcDataReader.IsDBNull(13))
                        {
                            mODELTYPE_TD.TYP_KV_ABS = Convert.ToString(odbcDataReader.GetValue(13));
                        }
                        if (!odbcDataReader.IsDBNull(14))
                        {
                            mODELTYPE_TD.TYP_KV_ASR = Convert.ToString(odbcDataReader.GetValue(14));
                        }
                        if (!odbcDataReader.IsDBNull(15))
                        {
                            mODELTYPE_TD.TYP_KV_BRAKE_TYPE = Convert.ToString(odbcDataReader.GetValue(15));
                        }
                        if (!odbcDataReader.IsDBNull(16))
                        {
                            mODELTYPE_TD.TYP_KV_BRAKE_SYST = Convert.ToString(odbcDataReader.GetValue(16));
                        }
                        if (!odbcDataReader.IsDBNull(17))
                        {
                            mODELTYPE_TD.TYP_KV_FUEL = Convert.ToString(odbcDataReader.GetValue(17));
                        }
                        if (!odbcDataReader.IsDBNull(18))
                        {
                            mODELTYPE_TD.TYP_KV_FUEL_SUPPLY = Convert.ToString(odbcDataReader.GetValue(18));
                        }
                        if (!odbcDataReader.IsDBNull(19))
                        {
                            mODELTYPE_TD.TYP_KV_CATALYST = Convert.ToString(odbcDataReader.GetValue(19));
                        }
                        if (!odbcDataReader.IsDBNull(20))
                        {
                            mODELTYPE_TD.TYP_KV_TRANS = Convert.ToString(odbcDataReader.GetValue(20));
                        }
                        if (!odbcDataReader.IsDBNull(21))
                        {
                            mODELTYPE_TD.TYP_KV_ENGINE = Convert.ToString(odbcDataReader.GetValue(21));
                        }
                        list.Add(mODELTYPE_TD);
                    }
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            if (modelTypeId.HasValue && modelTypeId.Value > 0)
            {
                return (from e in msTDContext.EnterpriseCarModelTDES.Include("EnterpriseCarModelFuelTDES")
                        where e.EnterpriseCarModelId == modelTypeId.Value && e.EnterpriseCarModelTypeId == MOD_ID
                        select new MODELTYPE_TD
                        {
                            TYP_ID = e.EnterpriseCarModelId,
                            TEX_TEXT = e.EnterpriseCarModelName,
                            TYP_KV_BODY = e.EnterpriseCarModelBody,
                            TYP_PCON_START = e.EnterpriseCarModelProductDateStart,
                            TYP_PCON_END = e.EnterpriseCarModelProductDateTil,
                            TYP_KW_FROM = e.EnterpriseCarModelPowerKW,
                            TYP_HP_FROM = e.EnterpriseCarModelPowerHP,
                            TYP_CCM = e.EnterpriseCarModelEngCap,
                            TYP_VALVES = e.EnterpriseCarModelVALVES,
                            TYP_CYLINDERS = e.EnterpriseCarModelCYLINDERS,
                            TYP_KV_ABS = e.EnterpriseCarModelABS,
                            TYP_KV_ASR = e.EnterpriseCarModelASR,
                            TYP_KV_BRAKE_TYPE = e.EnterpriseCarModelBrakeType,
                            TYP_KV_BRAKE_SYST = e.EnterpriseCarModelBrakeSys,
                            TYP_KV_FUEL = e.EnterpriseCarModelFuelTDES.FuelName,
                            TYP_KV_FUEL_SUPPLY = e.EnterpriseCarModelFUELSUPPLY,
                            TYP_KV_CATALYST = e.EnterpriseCarModelCATALYST,
                            TYP_KV_TRANS = e.EnterpriseCarModelTRANS,
                            TYP_KV_ENGINE = e.EnterpriseCarModelENGCODE
                        }).ToList();
            }
            if (fluelId.HasValue && fluelId.Value > 0)
            {
                return (from e in msTDContext.EnterpriseCarModelTDES.Include("EnterpriseCarModelFuelTDES")
                        where e.EnterpriseCarModelTypeId == MOD_ID && e.FUELId == fluelId.Value
                        select new MODELTYPE_TD
                        {
                            TYP_ID = e.EnterpriseCarModelId,
                            TEX_TEXT = e.EnterpriseCarModelName,
                            TYP_KV_BODY = e.EnterpriseCarModelBody,
                            TYP_PCON_START = e.EnterpriseCarModelProductDateStart,
                            TYP_PCON_END = e.EnterpriseCarModelProductDateTil,
                            TYP_KW_FROM = e.EnterpriseCarModelPowerKW,
                            TYP_HP_FROM = e.EnterpriseCarModelPowerHP,
                            TYP_CCM = e.EnterpriseCarModelEngCap,
                            TYP_VALVES = e.EnterpriseCarModelVALVES,
                            TYP_CYLINDERS = e.EnterpriseCarModelCYLINDERS,
                            TYP_KV_ABS = e.EnterpriseCarModelABS,
                            TYP_KV_ASR = e.EnterpriseCarModelASR,
                            TYP_KV_BRAKE_TYPE = e.EnterpriseCarModelBrakeType,
                            TYP_KV_BRAKE_SYST = e.EnterpriseCarModelBrakeSys,
                            TYP_KV_FUEL = e.EnterpriseCarModelFuelTDES.FuelName,
                            TYP_KV_FUEL_SUPPLY = e.EnterpriseCarModelFUELSUPPLY,
                            TYP_KV_CATALYST = e.EnterpriseCarModelCATALYST,
                            TYP_KV_TRANS = e.EnterpriseCarModelTRANS,
                            TYP_KV_ENGINE = e.EnterpriseCarModelENGCODE
                        }).ToList();
            }
            return (from e in msTDContext.EnterpriseCarModelTDES.Include("EnterpriseCarModelFuelTDES")
                    where e.EnterpriseCarModelTypeId == MOD_ID
                    select new MODELTYPE_TD
                    {
                        TYP_ID = e.EnterpriseCarModelId,
                        TEX_TEXT = e.EnterpriseCarModelName,
                        TYP_KV_BODY = e.EnterpriseCarModelBody,
                        TYP_PCON_START = e.EnterpriseCarModelProductDateStart,
                        TYP_PCON_END = e.EnterpriseCarModelProductDateTil,
                        TYP_KW_FROM = e.EnterpriseCarModelPowerKW,
                        TYP_HP_FROM = e.EnterpriseCarModelPowerHP,
                        TYP_CCM = e.EnterpriseCarModelEngCap,
                        TYP_VALVES = e.EnterpriseCarModelVALVES,
                        TYP_CYLINDERS = e.EnterpriseCarModelCYLINDERS,
                        TYP_KV_ABS = e.EnterpriseCarModelABS,
                        TYP_KV_ASR = e.EnterpriseCarModelASR,
                        TYP_KV_BRAKE_TYPE = e.EnterpriseCarModelBrakeType,
                        TYP_KV_BRAKE_SYST = e.EnterpriseCarModelBrakeSys,
                        TYP_KV_FUEL = e.EnterpriseCarModelFuelTDES.FuelName,
                        TYP_KV_FUEL_SUPPLY = e.EnterpriseCarModelFUELSUPPLY,
                        TYP_KV_CATALYST = e.EnterpriseCarModelCATALYST,
                        TYP_KV_TRANS = e.EnterpriseCarModelTRANS,
                        TYP_KV_ENGINE = e.EnterpriseCarModelENGCODE
                    }).ToList();
        }
        return new List<MODELTYPE_TD>();
    }

    public async Task<List<MODELTYPE_TD>> GetMODELTYPESAsync(int LNG_ID, int COU_ID, int MOD_ID, int? fluelId = default(int?), int? modelTypeId = default(int?))
    {
        if (tecdocsrctype == 1)
        {
            List<MODELTYPE_TD> aResult = new List<MODELTYPE_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPES_TEXT(LNG_ID, COU_ID, MOD_ID, fluelId, modelTypeId);
            DoOpenConnectionTD();
            int CurrentId = 0;
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                int itemId = 0;
                while (await rd.ReadAsync())
                {
                    if (!rd.IsDBNull(0))
                    {
                        itemId = Convert.ToInt32(rd.GetValue(0));
                    }
                    if (itemId != CurrentId)
                    {
                        CurrentId = itemId;
                        MODELTYPE_TD mODELTYPE_TD = new MODELTYPE_TD();
                        mODELTYPE_TD.TYP_ID = CurrentId;
                        if (!rd.IsDBNull(1))
                        {
                            mODELTYPE_TD.TEX_TEXT = Convert.ToString(rd.GetValue(1));
                        }
                        if (!rd.IsDBNull(2))
                        {
                            mODELTYPE_TD.TYP_KV_BODY = Convert.ToString(rd.GetValue(2));
                        }
                        if (!rd.IsDBNull(3))
                        {
                            mODELTYPE_TD.TYP_PCON_START = Convert.ToString(rd.GetValue(3));
                            if (!string.IsNullOrEmpty(mODELTYPE_TD.TYP_PCON_START) && mODELTYPE_TD.TYP_PCON_START.Length > 4)
                            {
                                mODELTYPE_TD.TYP_PCON_START = mODELTYPE_TD.TYP_PCON_START.Substring(0, 4) + "-" + mODELTYPE_TD.TYP_PCON_START.Substring(4);
                            }
                        }
                        if (!rd.IsDBNull(4))
                        {
                            mODELTYPE_TD.TYP_PCON_END = Convert.ToString(rd.GetValue(4));
                            if (!string.IsNullOrEmpty(mODELTYPE_TD.TYP_PCON_END) && mODELTYPE_TD.TYP_PCON_END.Length > 4)
                            {
                                mODELTYPE_TD.TYP_PCON_END = mODELTYPE_TD.TYP_PCON_END.Substring(0, 4) + "-" + mODELTYPE_TD.TYP_PCON_END.Substring(4);
                            }
                        }
                        if (!rd.IsDBNull(5))
                        {
                            mODELTYPE_TD.TYP_KW_FROM = Convert.ToString(rd.GetValue(5));
                        }
                        if (!rd.IsDBNull(6))
                        {
                            mODELTYPE_TD.TYP_KW_UPTO = Convert.ToString(rd.GetValue(6));
                        }
                        if (!rd.IsDBNull(7))
                        {
                            mODELTYPE_TD.TYP_HP_FROM = Convert.ToString(rd.GetValue(7));
                        }
                        if (!rd.IsDBNull(8))
                        {
                            mODELTYPE_TD.TYP_HP_UPTO = Convert.ToString(rd.GetValue(8));
                        }
                        if (!rd.IsDBNull(9))
                        {
                            mODELTYPE_TD.TYP_CCM = Convert.ToString(rd.GetValue(9));
                        }
                        if (!rd.IsDBNull(10))
                        {
                            mODELTYPE_TD.TYP_VALVES = Convert.ToString(rd.GetValue(10));
                        }
                        if (!rd.IsDBNull(11))
                        {
                            mODELTYPE_TD.TYP_CYLINDERS = Convert.ToString(rd.GetValue(11));
                        }
                        if (!rd.IsDBNull(12))
                        {
                            mODELTYPE_TD.TYP_DOORS = Convert.ToString(rd.GetValue(12));
                        }
                        if (!rd.IsDBNull(13))
                        {
                            mODELTYPE_TD.TYP_KV_ABS = Convert.ToString(rd.GetValue(13));
                        }
                        if (!rd.IsDBNull(14))
                        {
                            mODELTYPE_TD.TYP_KV_ASR = Convert.ToString(rd.GetValue(14));
                        }
                        if (!rd.IsDBNull(15))
                        {
                            mODELTYPE_TD.TYP_KV_BRAKE_TYPE = Convert.ToString(rd.GetValue(15));
                        }
                        if (!rd.IsDBNull(16))
                        {
                            mODELTYPE_TD.TYP_KV_BRAKE_SYST = Convert.ToString(rd.GetValue(16));
                        }
                        if (!rd.IsDBNull(17))
                        {
                            mODELTYPE_TD.TYP_KV_FUEL = Convert.ToString(rd.GetValue(17));
                        }
                        if (!rd.IsDBNull(18))
                        {
                            mODELTYPE_TD.TYP_KV_FUEL_SUPPLY = Convert.ToString(rd.GetValue(18));
                        }
                        if (!rd.IsDBNull(19))
                        {
                            mODELTYPE_TD.TYP_KV_CATALYST = Convert.ToString(rd.GetValue(19));
                        }
                        if (!rd.IsDBNull(20))
                        {
                            mODELTYPE_TD.TYP_KV_TRANS = Convert.ToString(rd.GetValue(20));
                        }
                        if (!rd.IsDBNull(21))
                        {
                            mODELTYPE_TD.TYP_KV_ENGINE = Convert.ToString(rd.GetValue(21));
                        }
                        aResult.Add(mODELTYPE_TD);
                    }
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        if (tecdocsrctype == 2)
        {
            if (modelTypeId.HasValue && modelTypeId.Value > 0)
            {
                return await (from e in msTDContext.EnterpriseCarModelTDES.Include("EnterpriseCarModelFuelTDES")
                              where e.EnterpriseCarModelId == modelTypeId.Value && e.EnterpriseCarModelTypeId == MOD_ID
                              select new MODELTYPE_TD
                              {
                                  TYP_ID = e.EnterpriseCarModelId,
                                  TEX_TEXT = e.EnterpriseCarModelName,
                                  TYP_KV_BODY = e.EnterpriseCarModelBody,
                                  TYP_PCON_START = e.EnterpriseCarModelProductDateStart,
                                  TYP_PCON_END = e.EnterpriseCarModelProductDateTil,
                                  TYP_KW_FROM = e.EnterpriseCarModelPowerKW,
                                  TYP_HP_FROM = e.EnterpriseCarModelPowerHP,
                                  TYP_CCM = e.EnterpriseCarModelEngCap,
                                  TYP_VALVES = e.EnterpriseCarModelVALVES,
                                  TYP_CYLINDERS = e.EnterpriseCarModelCYLINDERS,
                                  TYP_KV_ABS = e.EnterpriseCarModelABS,
                                  TYP_KV_ASR = e.EnterpriseCarModelASR,
                                  TYP_KV_BRAKE_TYPE = e.EnterpriseCarModelBrakeType,
                                  TYP_KV_BRAKE_SYST = e.EnterpriseCarModelBrakeSys,
                                  TYP_KV_FUEL = e.EnterpriseCarModelFuelTDES.FuelName,
                                  TYP_KV_FUEL_SUPPLY = e.EnterpriseCarModelFUELSUPPLY,
                                  TYP_KV_CATALYST = e.EnterpriseCarModelCATALYST,
                                  TYP_KV_TRANS = e.EnterpriseCarModelTRANS,
                                  TYP_KV_ENGINE = e.EnterpriseCarModelENGCODE
                              }).ToListAsync();
            }
            if (fluelId.HasValue && fluelId.Value > 0)
            {
                return await (from e in msTDContext.EnterpriseCarModelTDES.Include("EnterpriseCarModelFuelTDES")
                              where e.EnterpriseCarModelTypeId == MOD_ID && e.FUELId == fluelId.Value
                              select new MODELTYPE_TD
                              {
                                  TYP_ID = e.EnterpriseCarModelId,
                                  TEX_TEXT = e.EnterpriseCarModelName,
                                  TYP_KV_BODY = e.EnterpriseCarModelBody,
                                  TYP_PCON_START = e.EnterpriseCarModelProductDateStart,
                                  TYP_PCON_END = e.EnterpriseCarModelProductDateTil,
                                  TYP_KW_FROM = e.EnterpriseCarModelPowerKW,
                                  TYP_HP_FROM = e.EnterpriseCarModelPowerHP,
                                  TYP_CCM = e.EnterpriseCarModelEngCap,
                                  TYP_VALVES = e.EnterpriseCarModelVALVES,
                                  TYP_CYLINDERS = e.EnterpriseCarModelCYLINDERS,
                                  TYP_KV_ABS = e.EnterpriseCarModelABS,
                                  TYP_KV_ASR = e.EnterpriseCarModelASR,
                                  TYP_KV_BRAKE_TYPE = e.EnterpriseCarModelBrakeType,
                                  TYP_KV_BRAKE_SYST = e.EnterpriseCarModelBrakeSys,
                                  TYP_KV_FUEL = e.EnterpriseCarModelFuelTDES.FuelName,
                                  TYP_KV_FUEL_SUPPLY = e.EnterpriseCarModelFUELSUPPLY,
                                  TYP_KV_CATALYST = e.EnterpriseCarModelCATALYST,
                                  TYP_KV_TRANS = e.EnterpriseCarModelTRANS,
                                  TYP_KV_ENGINE = e.EnterpriseCarModelENGCODE
                              }).ToListAsync();
            }
            return await (from e in msTDContext.EnterpriseCarModelTDES.Include("EnterpriseCarModelFuelTDES")
                          where e.EnterpriseCarModelTypeId == MOD_ID
                          select new MODELTYPE_TD
                          {
                              TYP_ID = e.EnterpriseCarModelId,
                              TEX_TEXT = e.EnterpriseCarModelName,
                              TYP_KV_BODY = e.EnterpriseCarModelBody,
                              TYP_PCON_START = e.EnterpriseCarModelProductDateStart,
                              TYP_PCON_END = e.EnterpriseCarModelProductDateTil,
                              TYP_KW_FROM = e.EnterpriseCarModelPowerKW,
                              TYP_HP_FROM = e.EnterpriseCarModelPowerHP,
                              TYP_CCM = e.EnterpriseCarModelEngCap,
                              TYP_VALVES = e.EnterpriseCarModelVALVES,
                              TYP_CYLINDERS = e.EnterpriseCarModelCYLINDERS,
                              TYP_KV_ABS = e.EnterpriseCarModelABS,
                              TYP_KV_ASR = e.EnterpriseCarModelASR,
                              TYP_KV_BRAKE_TYPE = e.EnterpriseCarModelBrakeType,
                              TYP_KV_BRAKE_SYST = e.EnterpriseCarModelBrakeSys,
                              TYP_KV_FUEL = e.EnterpriseCarModelFuelTDES.FuelName,
                              TYP_KV_FUEL_SUPPLY = e.EnterpriseCarModelFUELSUPPLY,
                              TYP_KV_CATALYST = e.EnterpriseCarModelCATALYST,
                              TYP_KV_TRANS = e.EnterpriseCarModelTRANS,
                              TYP_KV_ENGINE = e.EnterpriseCarModelENGCODE
                          }).ToListAsync();
        }
        return new List<MODELTYPE_TD>();
    }

    public List<FUEL_TD> GetFUELS(int LNG_ID)
    {
        List<FUEL_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<FUEL_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetFUELS_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                while (odbcDataReader.Read())
                {
                    FUEL_TD fUEL_TD = new FUEL_TD();
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        fUEL_TD.DES_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                    }
                    if (!odbcDataReader.IsDBNull(1))
                    {
                        fUEL_TD.TEX_TEXT = Convert.ToString(odbcDataReader.GetValue(1));
                    }
                    list.Add(fUEL_TD);
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            return (from e in msTDContext.EnterpriseCarModelFuelTDES
                    select new FUEL_TD
                    {
                        DES_ID = e.FUELId,
                        TEX_TEXT = e.FuelName
                    }).ToList();
        }
        return new List<FUEL_TD>();
    }

    public async Task<List<FUEL_TD>> GetFUELSAsync(int LNG_ID)
    {
        if (tecdocsrctype == 1)
        {
            List<FUEL_TD> aResult = new List<FUEL_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetFUELS_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                while (await rd.ReadAsync())
                {
                    FUEL_TD fUEL_TD = new FUEL_TD();
                    if (!rd.IsDBNull(0))
                    {
                        fUEL_TD.DES_ID = Convert.ToInt32(rd.GetValue(0));
                    }
                    if (!rd.IsDBNull(1))
                    {
                        fUEL_TD.TEX_TEXT = Convert.ToString(rd.GetValue(1));
                    }
                    aResult.Add(fUEL_TD);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        if (tecdocsrctype == 2)
        {
            return await (from e in msTDContext.EnterpriseCarModelFuelTDES
                          select new FUEL_TD
                          {
                              DES_ID = e.FUELId,
                              TEX_TEXT = e.FuelName
                          }).ToListAsync();
        }
        return new List<FUEL_TD>();
    }

    public List<SIMPLEMODELTYPES_TD> GetSIMPLEMODELTYPES(int LNG_ID, int COU_ID, int modelTypeId)
    {
        List<SIMPLEMODELTYPES_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<SIMPLEMODELTYPES_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetSIMPLEMODELTYPES_TEXT(LNG_ID, COU_ID, modelTypeId);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                while (odbcDataReader.Read())
                {
                    SIMPLEMODELTYPES_TD sIMPLEMODELTYPES_TD = new SIMPLEMODELTYPES_TD();
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        sIMPLEMODELTYPES_TD.TYP_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                    }
                    if (!odbcDataReader.IsDBNull(1))
                    {
                        sIMPLEMODELTYPES_TD.TEX_TEXT = Convert.ToString(odbcDataReader.GetValue(1));
                    }
                    if (!odbcDataReader.IsDBNull(2))
                    {
                        sIMPLEMODELTYPES_TD.TYP_KV_FUEL = Convert.ToInt32(odbcDataReader.GetValue(2));
                    }
                    if (!odbcDataReader.IsDBNull(3))
                    {
                        sIMPLEMODELTYPES_TD.TYP_MOD_ID = Convert.ToInt32(odbcDataReader.GetValue(3));
                    }
                    list.Add(sIMPLEMODELTYPES_TD);
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            return (from e in msTDContext.EnterpriseCarModelTDES.Include("EnterpriseCarModelFuelTDES")
                    where e.EnterpriseCarModelId == modelTypeId
                    select new SIMPLEMODELTYPES_TD
                    {
                        TYP_ID = e.EnterpriseCarModelId,
                        TEX_TEXT = e.EnterpriseCarModelName,
                        TYP_KV_FUEL = e.FUELId,
                        TYP_MOD_ID = e.EnterpriseCarModelId
                    }).ToList();
        }
        return new List<SIMPLEMODELTYPES_TD>();
    }

    public async Task<List<SIMPLEMODELTYPES_TD>> GetSIMPLEMODELTYPESAsync(int LNG_ID, int COU_ID, int modelTypeId)
    {
        if (tecdocsrctype == 1)
        {
            List<SIMPLEMODELTYPES_TD> aResult = new List<SIMPLEMODELTYPES_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetSIMPLEMODELTYPES_TEXT(LNG_ID, COU_ID, modelTypeId);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                while (await rd.ReadAsync())
                {
                    SIMPLEMODELTYPES_TD sIMPLEMODELTYPES_TD = new SIMPLEMODELTYPES_TD();
                    if (!rd.IsDBNull(0))
                    {
                        sIMPLEMODELTYPES_TD.TYP_ID = Convert.ToInt32(rd.GetValue(0));
                    }
                    if (!rd.IsDBNull(1))
                    {
                        sIMPLEMODELTYPES_TD.TEX_TEXT = Convert.ToString(rd.GetValue(1));
                    }
                    if (!rd.IsDBNull(2))
                    {
                        sIMPLEMODELTYPES_TD.TYP_KV_FUEL = Convert.ToInt32(rd.GetValue(2));
                    }
                    if (!rd.IsDBNull(3))
                    {
                        sIMPLEMODELTYPES_TD.TYP_MOD_ID = Convert.ToInt32(rd.GetValue(3));
                    }
                    aResult.Add(sIMPLEMODELTYPES_TD);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        if (tecdocsrctype == 2)
        {
            return await (from e in msTDContext.EnterpriseCarModelTDES.Include("EnterpriseCarModelFuelTDES")
                          where e.EnterpriseCarModelId == modelTypeId
                          select new SIMPLEMODELTYPES_TD
                          {
                              TYP_ID = e.EnterpriseCarModelId,
                              TEX_TEXT = e.EnterpriseCarModelName,
                              TYP_KV_FUEL = e.FUELId,
                              TYP_MOD_ID = e.EnterpriseCarModelId
                          }).ToListAsync();
        }
        return new List<SIMPLEMODELTYPES_TD>();
    }

    protected MODELTYPESTREE_TD GetMODELTYPESTREE_Parent(ICollection<MODELTYPESTREE_TD> aResult, int prntId, bool isOpen)
    {
        if (aResult == null)
        {
            return null;
        }
        MODELTYPESTREE_TD mODELTYPESTREE_TD = (from e in aResult
                                               where e.STR_ID == prntId
                                               select e).FirstOrDefault();
        if (mODELTYPESTREE_TD != null)
        {
            if (isOpen)
            {
                mODELTYPESTREE_TD.isOpen = isOpen;
            }
            return mODELTYPESTREE_TD;
        }
        foreach (MODELTYPESTREE_TD item in aResult)
        {
            mODELTYPESTREE_TD = GetMODELTYPESTREE_Parent(item.Subitems, prntId, isOpen);
            if (mODELTYPESTREE_TD != null)
            {
                if (isOpen)
                {
                    item.isOpen = isOpen;
                }
                return mODELTYPESTREE_TD;
            }
        }
        return null;
    }

    public List<MODELTYPESTREE_TD> GetMODELTYPESTREE(int LNG_ID, int TYP_ID, int topicId, int treeKindId)
    {
        List<MODELTYPESTREE_TD> list = new List<MODELTYPESTREE_TD>();
        if (tecdocsrctype == 1)
        {
            list = new List<MODELTYPESTREE_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPESTREE_TEXT(LNG_ID, TYP_ID);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                bool flag = true;
                int num = 0;
                MODELTYPESTREE_TD mODELTYPESTREE_TD = null;
                while (odbcDataReader.Read())
                {
                    MODELTYPESTREE_TD mODELTYPESTREE_TD2 = new MODELTYPESTREE_TD();
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        mODELTYPESTREE_TD2.STR_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                    }
                    if (!odbcDataReader.IsDBNull(1))
                    {
                        mODELTYPESTREE_TD2.TEX_TEXT = Convert.ToString(odbcDataReader.GetValue(3));
                    }
                    mODELTYPESTREE_TD2.isOpen = (topicId == mODELTYPESTREE_TD2.STR_ID);
                    int num2 = Convert.ToInt32(odbcDataReader.GetValue(1));
                    if (flag)
                    {
                        flag = false;
                        num = num2;
                        list.Add(mODELTYPESTREE_TD2);
                        mODELTYPESTREE_TD = mODELTYPESTREE_TD2;
                    }
                    else if (num == num2)
                    {
                        list.Add(mODELTYPESTREE_TD2);
                    }
                    else
                    {
                        int prntId = Convert.ToInt32(odbcDataReader.GetValue(4));
                        mODELTYPESTREE_TD = GetMODELTYPESTREE_Parent(list, prntId, mODELTYPESTREE_TD2.isOpen);
                        if (mODELTYPESTREE_TD != null)
                        {
                            if (mODELTYPESTREE_TD.Subitems == null)
                            {
                                mODELTYPESTREE_TD.Subitems = new List<MODELTYPESTREE_TD>();
                            }
                            mODELTYPESTREE_TD.Subitems.Add(mODELTYPESTREE_TD2);
                        }
                        else
                        {
                            list.Add(mODELTYPESTREE_TD2);
                        }
                    }
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            bool flag2 = true;
            MODELTYPESTREE_TD mODELTYPESTREE_TD3 = null;
            int aLower = treeKindId * 10000;
            int aUpper = aLower + 10000;
            IQueryable<EnterpriseCategoryTecDocTDES> queryable = from e in msTDContext.EnterpriseCategoryTecDocTDES
                                                                 join b in msTDContext.EnterpriseCategoryApplicTDES on e.CategoryId equals b.CategoryId
                                                                 where e.CategoryId >= aLower && e.CategoryId < aUpper && b.EnterpriseCarModelId == TYP_ID
                                                                 orderby e.CategoryParent, e.CategoryId
                                                                 select e;
            {
                foreach (EnterpriseCategoryTecDocTDES item in queryable)
                {
                    MODELTYPESTREE_TD mODELTYPESTREE_TD4 = new MODELTYPESTREE_TD();
                    mODELTYPESTREE_TD4.STR_ID = item.CategoryId;
                    mODELTYPESTREE_TD4.TEX_TEXT = item.CategoryDescription;
                    mODELTYPESTREE_TD4.isOpen = (topicId == item.CategoryId);
                    mODELTYPESTREE_TD4.Parent = item.CategoryParent;
                    MODELTYPESTREE_TD mODELTYPESTREE_TD5 = mODELTYPESTREE_TD4;
                    if (flag2)
                    {
                        flag2 = false;
                        list.Add(mODELTYPESTREE_TD5);
                        mODELTYPESTREE_TD3 = mODELTYPESTREE_TD5;
                    }
                    else
                    {
                        int categoryParent = item.CategoryParent;
                        mODELTYPESTREE_TD3 = GetMODELTYPESTREE_Parent(list, categoryParent, mODELTYPESTREE_TD5.isOpen);
                        if (mODELTYPESTREE_TD3 != null)
                        {
                            if (mODELTYPESTREE_TD3.Subitems == null)
                            {
                                mODELTYPESTREE_TD3.Subitems = new List<MODELTYPESTREE_TD>();
                            }
                            mODELTYPESTREE_TD3.Subitems.Add(mODELTYPESTREE_TD5);
                        }
                        else
                        {
                            list.Add(mODELTYPESTREE_TD5);
                        }
                        for (int num3 = list.Count - 1; num3 > 0; num3--)
                        {
                            MODELTYPESTREE_TD mODELTYPESTREE_TD6 = list[num3];
                            if (mODELTYPESTREE_TD6.Parent == item.CategoryId)
                            {
                                if (mODELTYPESTREE_TD5.Subitems == null)
                                {
                                    mODELTYPESTREE_TD5.Subitems = new List<MODELTYPESTREE_TD>();
                                }
                                mODELTYPESTREE_TD5.Subitems.Add(mODELTYPESTREE_TD6);
                                list.RemoveAt(num3);
                            }
                        }
                    }
                }
                return list;
            }
        }
        return list;
    }

    public async Task<List<MODELTYPESTREE_TD>> GetMODELTYPESTREEAsync(int LNG_ID, int TYP_ID, int topicId, int treeKindId)
    {
        List<MODELTYPESTREE_TD> aResult = new List<MODELTYPESTREE_TD>();
        if (tecdocsrctype == 1)
        {
            aResult = new List<MODELTYPESTREE_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPESTREE_TEXT(LNG_ID, TYP_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                bool isFirst = true;
                int currLevel = 0;
                while (await rd.ReadAsync())
                {
                    MODELTYPESTREE_TD mODELTYPESTREE_TD = new MODELTYPESTREE_TD();
                    if (!rd.IsDBNull(0))
                    {
                        mODELTYPESTREE_TD.STR_ID = Convert.ToInt32(rd.GetValue(0));
                    }
                    if (!rd.IsDBNull(1))
                    {
                        mODELTYPESTREE_TD.TEX_TEXT = Convert.ToString(rd.GetValue(3));
                    }
                    mODELTYPESTREE_TD.isOpen = (topicId == mODELTYPESTREE_TD.STR_ID);
                    int num = Convert.ToInt32(rd.GetValue(1));
                    if (isFirst)
                    {
                        isFirst = false;
                        currLevel = num;
                        aResult.Add(mODELTYPESTREE_TD);
                    }
                    else if (currLevel == num)
                    {
                        aResult.Add(mODELTYPESTREE_TD);
                    }
                    else
                    {
                        int prntId = Convert.ToInt32(rd.GetValue(4));
                        MODELTYPESTREE_TD prntItm2 = GetMODELTYPESTREE_Parent(aResult, prntId, mODELTYPESTREE_TD.isOpen);
                        if (prntItm2 != null)
                        {
                            if (prntItm2.Subitems == null)
                            {
                                prntItm2.Subitems = new List<MODELTYPESTREE_TD>();
                            }
                            prntItm2.Subitems.Add(mODELTYPESTREE_TD);
                        }
                        else
                        {
                            aResult.Add(mODELTYPESTREE_TD);
                        }
                    }
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        if (tecdocsrctype == 2)
        {
            bool isFirst2 = true;
            int aLower = treeKindId * 10000;
            int aUpper = aLower + 10000;
            foreach (EnterpriseCategoryTecDocTDES item in await (from e in msTDContext.EnterpriseCategoryTecDocTDES
                                                                 join b in msTDContext.EnterpriseCategoryApplicTDES on e.CategoryId equals b.CategoryId
                                                                 where e.CategoryId >= aLower && e.CategoryId < aUpper && b.EnterpriseCarModelId == TYP_ID
                                                                 orderby e.CategoryParent, e.CategoryId
                                                                 select e).ToListAsync())
            {
                MODELTYPESTREE_TD mODELTYPESTREE_TD2 = new MODELTYPESTREE_TD();
                mODELTYPESTREE_TD2.STR_ID = item.CategoryId;
                mODELTYPESTREE_TD2.TEX_TEXT = item.CategoryDescription;
                mODELTYPESTREE_TD2.isOpen = (topicId == item.CategoryId);
                mODELTYPESTREE_TD2.Parent = item.CategoryParent;
                MODELTYPESTREE_TD mODELTYPESTREE_TD3 = mODELTYPESTREE_TD2;
                if (isFirst2)
                {
                    isFirst2 = false;
                    aResult.Add(mODELTYPESTREE_TD3);
                }
                else
                {
                    int categoryParent = item.CategoryParent;
                    MODELTYPESTREE_TD prntItm = GetMODELTYPESTREE_Parent(aResult, categoryParent, mODELTYPESTREE_TD3.isOpen);
                    if (prntItm != null)
                    {
                        if (prntItm.Subitems == null)
                        {
                            prntItm.Subitems = new List<MODELTYPESTREE_TD>();
                        }
                        prntItm.Subitems.Add(mODELTYPESTREE_TD3);
                    }
                    else
                    {
                        aResult.Add(mODELTYPESTREE_TD3);
                    }
                    for (int num2 = aResult.Count - 1; num2 > 0; num2--)
                    {
                        MODELTYPESTREE_TD mODELTYPESTREE_TD4 = aResult[num2];
                        if (mODELTYPESTREE_TD4.Parent == item.CategoryId)
                        {
                            if (mODELTYPESTREE_TD3.Subitems == null)
                            {
                                mODELTYPESTREE_TD3.Subitems = new List<MODELTYPESTREE_TD>();
                            }
                            mODELTYPESTREE_TD3.Subitems.Add(mODELTYPESTREE_TD4);
                            aResult.RemoveAt(num2);
                        }
                    }
                }
            }
            return aResult;
        }
        return aResult;
    }

    public List<MODELTYPETREEITEMS_REST_TD> GetMODELTYPETREEITEMS(int LNG_ID, int COU_ID, int TYP_ID, int STR_ID, int tof_assemblyId, int tof_suppliersId)
    {
        List<MODELTYPETREEITEMS_REST_TD> list = new List<MODELTYPETREEITEMS_REST_TD>();
        OdbcCommandTD.CommandType = CommandType.Text;
        OdbcCommandTD.CommandText = GetMODELTYPETREEITEMS_TEXT(LNG_ID, COU_ID, TYP_ID, STR_ID, tof_assemblyId, tof_suppliersId);
        DoOpenConnectionTD();
        OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
        try
        {
            while (odbcDataReader.Read())
            {
                MODELTYPETREEITEMS_REST_TD mODELTYPETREEITEMS_REST_TD = new MODELTYPETREEITEMS_REST_TD();
                if (!odbcDataReader.IsDBNull(0))
                {
                    mODELTYPETREEITEMS_REST_TD.SUP_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                }
                if (!odbcDataReader.IsDBNull(1))
                {
                    mODELTYPETREEITEMS_REST_TD.SUP_TEXT = Convert.ToString(odbcDataReader.GetValue(1));
                }
                if (!odbcDataReader.IsDBNull(2))
                {
                    mODELTYPETREEITEMS_REST_TD.GA_NR = Convert.ToInt32(odbcDataReader.GetValue(2));
                }
                if (!odbcDataReader.IsDBNull(3))
                {
                    mODELTYPETREEITEMS_REST_TD.MASTER_BEZ = Convert.ToString(odbcDataReader.GetValue(3));
                }
                if (!odbcDataReader.IsDBNull(4))
                {
                    mODELTYPETREEITEMS_REST_TD.ART_ARTICLE_NR = Convert.ToString(odbcDataReader.GetValue(4));
                }
                if (!odbcDataReader.IsDBNull(5))
                {
                    mODELTYPETREEITEMS_REST_TD.ART_ID = Convert.ToInt32(odbcDataReader.GetValue(5));
                }
                if (!odbcDataReader.IsDBNull(6))
                {
                    mODELTYPETREEITEMS_REST_TD.LA_ID = Convert.ToInt32(odbcDataReader.GetValue(6));
                }
                if (!odbcDataReader.IsDBNull(7))
                {
                    mODELTYPETREEITEMS_REST_TD.GA_TEXT = Convert.ToString(odbcDataReader.GetValue(7));
                }
                list.Add(mODELTYPETREEITEMS_REST_TD);
            }
            return list;
        }
        finally
        {
            odbcDataReader.Close();
        }
    }

    public async Task<List<MODELTYPETREEITEMS_REST_TD>> GetMODELTYPETREEITEMSAsync(int LNG_ID, int COU_ID, int TYP_ID, int STR_ID, int tof_assemblyId, int tof_suppliersId)
    {
        List<MODELTYPETREEITEMS_REST_TD> aResult = new List<MODELTYPETREEITEMS_REST_TD>();
        OdbcCommandTD.CommandType = CommandType.Text;
        OdbcCommandTD.CommandText = GetMODELTYPETREEITEMS_TEXT(LNG_ID, COU_ID, TYP_ID, STR_ID, tof_assemblyId, tof_suppliersId);
        DoOpenConnectionTD();
        OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
        try
        {
            while (await rd.ReadAsync())
            {
                MODELTYPETREEITEMS_REST_TD mODELTYPETREEITEMS_REST_TD = new MODELTYPETREEITEMS_REST_TD();
                if (!rd.IsDBNull(0))
                {
                    mODELTYPETREEITEMS_REST_TD.SUP_ID = Convert.ToInt32(rd.GetValue(0));
                }
                if (!rd.IsDBNull(1))
                {
                    mODELTYPETREEITEMS_REST_TD.SUP_TEXT = Convert.ToString(rd.GetValue(1));
                }
                if (!rd.IsDBNull(2))
                {
                    mODELTYPETREEITEMS_REST_TD.GA_NR = Convert.ToInt32(rd.GetValue(2));
                }
                if (!rd.IsDBNull(3))
                {
                    mODELTYPETREEITEMS_REST_TD.MASTER_BEZ = Convert.ToString(rd.GetValue(3));
                }
                if (!rd.IsDBNull(4))
                {
                    mODELTYPETREEITEMS_REST_TD.ART_ARTICLE_NR = Convert.ToString(rd.GetValue(4));
                }
                if (!rd.IsDBNull(5))
                {
                    mODELTYPETREEITEMS_REST_TD.ART_ID = Convert.ToInt32(rd.GetValue(5));
                }
                if (!rd.IsDBNull(6))
                {
                    mODELTYPETREEITEMS_REST_TD.LA_ID = Convert.ToInt32(rd.GetValue(6));
                }
                if (!rd.IsDBNull(7))
                {
                    mODELTYPETREEITEMS_REST_TD.GA_TEXT = Convert.ToString(rd.GetValue(7));
                }
                aResult.Add(mODELTYPETREEITEMS_REST_TD);
            }
            return aResult;
        }
        finally
        {
            rd.Close();
        }
    }

    public List<DICT_TD> GetTof_assembly(int LNG_ID, int COU_ID, int TYP_ID, int STR_ID)
    {
        return (from e in msTDContext.EnterpriseCategoryItemTecDocTDES.Include("EnterpriseCategoryItemTecDocDescription")
                where e.CategoryId == STR_ID
                select new DICT_TD
                {
                    DictId = e.CategoryItemId,
                    DictTitle = e.EnterpriseCategoryItemTecDocDescription.EntCategoryItemDescription
                }).ToList();
    }

    public async Task<List<DICT_TD>> GetTof_assemblyAsync(int LNG_ID, int COU_ID, int TYP_ID, int STR_ID)
    {
        return await (from e in msTDContext.EnterpriseCategoryItemTecDocTDES.Include("EnterpriseCategoryItemTecDocDescription")
                      where e.CategoryId == STR_ID
                      select new DICT_TD
                      {
                          DictId = e.CategoryItemId,
                          DictTitle = e.EnterpriseCategoryItemTecDocDescription.EntCategoryItemDescription
                      }).ToListAsync();
    }

    public List<MODELTYPETREEITEMS_REST_TD> GetMODELTYPETREEITEMS_MS(int LNG_ID, int COU_ID, int TYP_ID, int STR_ID, int tof_assemblyId, string tof_assemblyItemDescr, int tof_suppliersId)
    {
        if (tof_assemblyId != 0)
        {
            if (tof_suppliersId == 0)
            {
                return (from artCtg in msTDContext.EnterpriseArticleCategoryItemTDES
                        join art in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on artCtg.ArticleId equals art.ArticleId
                        join applic in msTDContext.EnterpriseArticleApplicTDES on artCtg.ArticleId equals applic.ArticleId
                        where artCtg.CategoryItemId == tof_assemblyId && applic.EnterpriseCarModelId == TYP_ID
                        select new MODELTYPETREEITEMS_REST_TD
                        {
                            SUP_ID = art.ArticleBrandId,
                            SUP_TEXT = art.ExternBrandNic,
                            GA_NR = artCtg.CategoryItemId,
                            MASTER_BEZ = art.EnterpriseArticleTecDocDescription.EntArticleDescription,
                            ART_ARTICLE_NR = art.ExternArticle,
                            ART_ID = art.ArticleId,
                            LA_ID = artCtg.CategoryItemId,
                            GA_TEXT = tof_assemblyItemDescr,
                            EAN_TEXT = art.ExternArticleEAN
                        }).ToList();
            }
            return (from artCtg in msTDContext.EnterpriseArticleCategoryItemTDES
                    join art in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on artCtg.ArticleId equals art.ArticleId
                    join applic in msTDContext.EnterpriseArticleApplicTDES on artCtg.ArticleId equals applic.ArticleId
                    where artCtg.CategoryItemId == tof_assemblyId && art.ArticleBrandId == tof_suppliersId && applic.EnterpriseCarModelId == TYP_ID
                    select new MODELTYPETREEITEMS_REST_TD
                    {
                        SUP_ID = art.ArticleBrandId,
                        SUP_TEXT = art.ExternBrandNic,
                        GA_NR = artCtg.CategoryItemId,
                        MASTER_BEZ = art.EnterpriseArticleTecDocDescription.EntArticleDescription,
                        ART_ARTICLE_NR = art.ExternArticle,
                        ART_ID = art.ArticleId,
                        LA_ID = artCtg.CategoryItemId,
                        GA_TEXT = tof_assemblyItemDescr,
                        EAN_TEXT = art.ExternArticleEAN
                    }).ToList();
        }
        return new List<MODELTYPETREEITEMS_REST_TD>();
    }

    public async Task<List<MODELTYPETREEITEMS_REST_TD>> GetMODELTYPETREEITEMS_MSAsync(int LNG_ID, int COU_ID, int TYP_ID, int STR_ID, int tof_assemblyId, string tof_assemblyItemDescr, int tof_suppliersId)
    {
        if (tof_assemblyId != 0)
        {
            if (tof_suppliersId == 0)
            {
                return await (from artCtg in msTDContext.EnterpriseArticleCategoryItemTDES
                              join art in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on artCtg.ArticleId equals art.ArticleId
                              join applic in msTDContext.EnterpriseArticleApplicTDES on artCtg.ArticleId equals applic.ArticleId
                              where artCtg.CategoryItemId == tof_assemblyId && applic.EnterpriseCarModelId == TYP_ID
                              select new MODELTYPETREEITEMS_REST_TD
                              {
                                  SUP_ID = art.ArticleBrandId,
                                  SUP_TEXT = art.ExternBrandNic,
                                  GA_NR = artCtg.CategoryItemId,
                                  MASTER_BEZ = art.EnterpriseArticleTecDocDescription.EntArticleDescription,
                                  ART_ARTICLE_NR = art.ExternArticle,
                                  ART_ID = art.ArticleId,
                                  LA_ID = artCtg.CategoryItemId,
                                  GA_TEXT = tof_assemblyItemDescr,
                                  EAN_TEXT = art.ExternArticleEAN
                              }).ToListAsync();
            }
            return await (from artCtg in msTDContext.EnterpriseArticleCategoryItemTDES
                          join art in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on artCtg.ArticleId equals art.ArticleId
                          join applic in msTDContext.EnterpriseArticleApplicTDES on artCtg.ArticleId equals applic.ArticleId
                          where artCtg.CategoryItemId == tof_assemblyId && art.ArticleBrandId == tof_suppliersId && applic.EnterpriseCarModelId == TYP_ID
                          select new MODELTYPETREEITEMS_REST_TD
                          {
                              SUP_ID = art.ArticleBrandId,
                              SUP_TEXT = art.ExternBrandNic,
                              GA_NR = artCtg.CategoryItemId,
                              MASTER_BEZ = art.EnterpriseArticleTecDocDescription.EntArticleDescription,
                              ART_ARTICLE_NR = art.ExternArticle,
                              ART_ID = art.ArticleId,
                              LA_ID = artCtg.CategoryItemId,
                              GA_TEXT = tof_assemblyItemDescr,
                              EAN_TEXT = art.ExternArticleEAN
                          }).ToListAsync();
        }
        return new List<MODELTYPETREEITEMS_REST_TD>();
    }

    public List<MODELTYPETREEITEMDESCR_TD> GetMODELTYPETREEITEMDESCR(int LNG_ID, int COU_ID, int ART_ID)
    {
        List<MODELTYPETREEITEMDESCR_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<MODELTYPETREEITEMDESCR_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPETREEITEMDESCR_TEXT(LNG_ID, COU_ID, ART_ID);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                while (odbcDataReader.Read())
                {
                    MODELTYPETREEITEMDESCR_TD mODELTYPETREEITEMDESCR_TD = new MODELTYPETREEITEMDESCR_TD();
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        mODELTYPETREEITEMDESCR_TD.TEX_TEXT = Convert.ToString(odbcDataReader.GetValue(0));
                    }
                    if (!odbcDataReader.IsDBNull(1))
                    {
                        mODELTYPETREEITEMDESCR_TD.TEX_VALUE = Convert.ToString(odbcDataReader.GetValue(1));
                    }
                    if (!odbcDataReader.IsDBNull(2))
                    {
                        mODELTYPETREEITEMDESCR_TD.TEX_UNIT = Convert.ToString(odbcDataReader.GetValue(2));
                    }
                    list.Add(mODELTYPETREEITEMDESCR_TD);
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        int tecdocsrctype2 = tecdocsrctype;
        return new List<MODELTYPETREEITEMDESCR_TD>();
    }

    public async Task<List<MODELTYPETREEITEMDESCR_TD>> GetMODELTYPETREEITEMDESCRAsync(int LNG_ID, int COU_ID, int ART_ID)
    {
        if (tecdocsrctype == 1)
        {
            List<MODELTYPETREEITEMDESCR_TD> aResult = new List<MODELTYPETREEITEMDESCR_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPETREEITEMDESCR_TEXT(LNG_ID, COU_ID, ART_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                while (await rd.ReadAsync())
                {
                    MODELTYPETREEITEMDESCR_TD mODELTYPETREEITEMDESCR_TD = new MODELTYPETREEITEMDESCR_TD();
                    if (!rd.IsDBNull(0))
                    {
                        mODELTYPETREEITEMDESCR_TD.TEX_TEXT = Convert.ToString(rd.GetValue(0));
                    }
                    if (!rd.IsDBNull(1))
                    {
                        mODELTYPETREEITEMDESCR_TD.TEX_VALUE = Convert.ToString(rd.GetValue(1));
                    }
                    if (!rd.IsDBNull(2))
                    {
                        mODELTYPETREEITEMDESCR_TD.TEX_UNIT = Convert.ToString(rd.GetValue(2));
                    }
                    aResult.Add(mODELTYPETREEITEMDESCR_TD);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        int tecdocsrctype2 = tecdocsrctype;
        return new List<MODELTYPETREEITEMDESCR_TD>();
    }

    public List<MODELTYPETREEITEMMANID_TD> GetMODELTYPETREEITEMMANID(int LNG_ID, int COU_ID, int ART_ID, int? MFA_ID = default(int?))
    {
        List<MODELTYPETREEITEMMANID_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<MODELTYPETREEITEMMANID_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPETREEITEMMANID_TEXT(LNG_ID, COU_ID, ART_ID, MFA_ID);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                while (odbcDataReader.Read())
                {
                    MODELTYPETREEITEMMANID_TD mODELTYPETREEITEMMANID_TD = new MODELTYPETREEITEMMANID_TD();
                    if (!odbcDataReader.IsDBNull(1))
                    {
                        mODELTYPETREEITEMMANID_TD.SUP_TEXT = Convert.ToString(odbcDataReader.GetValue(1));
                    }
                    if (!odbcDataReader.IsDBNull(2))
                    {
                        mODELTYPETREEITEMMANID_TD.TEX_VALUE = Convert.ToString(odbcDataReader.GetValue(2));
                    }
                    list.Add(mODELTYPETREEITEMMANID_TD);
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            if (MFA_ID.HasValue && MFA_ID.Value > 0)
            {
                return (from lup in msTDContext.EnterpriseArticleLookUpTDES
                        join br in msTDContext.EnterpriseArticleBrandTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleBrandId equals br.ArticleBrandId
                        where lup.ArticleId == ART_ID && lup.ArticleSearchKind == 3 && lup.ArticleBrandId == MFA_ID.Value
                        select new MODELTYPETREEITEMMANID_TD
                        {
                            SUP_TEXT = br.ArticleBrandNic,
                            TEX_VALUE = lup.ArticleDysplay
                        }).ToList();
            }
            return (from lup in msTDContext.EnterpriseArticleLookUpTDES
                    join br in msTDContext.EnterpriseArticleBrandTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleBrandId equals br.ArticleBrandId
                    where lup.ArticleId == ART_ID && lup.ArticleSearchKind == 3
                    select new MODELTYPETREEITEMMANID_TD
                    {
                        SUP_TEXT = br.ArticleBrandNic,
                        TEX_VALUE = lup.ArticleDysplay
                    }).ToList();
        }
        return new List<MODELTYPETREEITEMMANID_TD>();
    }

    public async Task<List<MODELTYPETREEITEMMANID_TD>> GetMODELTYPETREEITEMMANIDAsync(int LNG_ID, int COU_ID, int ART_ID, int? MFA_ID = default(int?))
    {
        if (tecdocsrctype == 1)
        {
            List<MODELTYPETREEITEMMANID_TD> aResult = new List<MODELTYPETREEITEMMANID_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPETREEITEMMANID_TEXT(LNG_ID, COU_ID, ART_ID, MFA_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                while (await rd.ReadAsync())
                {
                    MODELTYPETREEITEMMANID_TD mODELTYPETREEITEMMANID_TD = new MODELTYPETREEITEMMANID_TD();
                    if (!rd.IsDBNull(1))
                    {
                        mODELTYPETREEITEMMANID_TD.SUP_TEXT = Convert.ToString(rd.GetValue(1));
                    }
                    if (!rd.IsDBNull(2))
                    {
                        mODELTYPETREEITEMMANID_TD.TEX_VALUE = Convert.ToString(rd.GetValue(2));
                    }
                    aResult.Add(mODELTYPETREEITEMMANID_TD);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        if (tecdocsrctype == 2)
        {
            if (MFA_ID.HasValue && MFA_ID.Value > 0)
            {
                return await (from lup in msTDContext.EnterpriseArticleLookUpTDES
                              join br in msTDContext.EnterpriseArticleBrandTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleBrandId equals br.ArticleBrandId
                              where lup.ArticleId == ART_ID && lup.ArticleSearchKind == 3 && lup.ArticleBrandId == MFA_ID.Value
                              select new MODELTYPETREEITEMMANID_TD
                              {
                                  SUP_TEXT = br.ArticleBrandNic,
                                  TEX_VALUE = lup.ArticleDysplay
                              }).ToListAsync();
            }
            return await (from lup in msTDContext.EnterpriseArticleLookUpTDES
                          join br in msTDContext.EnterpriseArticleBrandTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleBrandId equals br.ArticleBrandId
                          where lup.ArticleId == ART_ID && lup.ArticleSearchKind == 3
                          select new MODELTYPETREEITEMMANID_TD
                          {
                              SUP_TEXT = br.ArticleBrandNic,
                              TEX_VALUE = lup.ArticleDysplay
                          }).ToListAsync();
        }
        return new List<MODELTYPETREEITEMMANID_TD>();
    }

    public List<ANALOGOUS_REST_TD> GetANALOGS(int LNG_ID, int COU_ID, string ART_ARTICLE_NR, int? GA_NR)
    {
        List<ANALOGOUS_REST_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<ANALOGOUS_REST_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetANALOGS_TEXT(LNG_ID, COU_ID, ART_ARTICLE_NR, GA_NR);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                while (odbcDataReader.Read())
                {
                    ANALOGOUS_REST_TD aNALOGOUS_REST_TD = new ANALOGOUS_REST_TD();
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        aNALOGOUS_REST_TD.ART_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                    }
                    if (!odbcDataReader.IsDBNull(1))
                    {
                        aNALOGOUS_REST_TD.ART_ARTICLE_NR = Convert.ToString(odbcDataReader.GetValue(1));
                    }
                    if (!odbcDataReader.IsDBNull(2))
                    {
                        aNALOGOUS_REST_TD.GA_NR = Convert.ToInt32(odbcDataReader.GetValue(2));
                    }
                    if (!odbcDataReader.IsDBNull(3))
                    {
                        aNALOGOUS_REST_TD.MASTER_BEZ = Convert.ToString(odbcDataReader.GetValue(3));
                    }
                    if (!odbcDataReader.IsDBNull(4))
                    {
                        aNALOGOUS_REST_TD.GA_TEXT = Convert.ToString(odbcDataReader.GetValue(4));
                    }
                    if (!odbcDataReader.IsDBNull(5))
                    {
                        aNALOGOUS_REST_TD.SUP_TEXT = Convert.ToString(odbcDataReader.GetValue(5));
                    }
                    list.Add(aNALOGOUS_REST_TD);
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            if (!string.IsNullOrEmpty(ART_ARTICLE_NR))
            {
                ART_ARTICLE_NR = ART_ARTICLE_NR.Replace(".", "").Replace("-", "").Replace(" ", "")
                    .Replace("/", "")
                    .Replace("\\", "");
            }
            if (GA_NR.HasValue && GA_NR.Value != 0)
            {
                int gnr = GA_NR.Value;
                return (from lup in msTDContext.EnterpriseArticleLookUpTDES
                        join e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleId equals e.ArticleId
                        join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                        where lup.ArticleSearch == ART_ARTICLE_NR && ee.CategoryItemId == gnr
                        select new ANALOGOUS_REST_TD
                        {
                            ART_ID = e.ArticleId,
                            ART_ARTICLE_NR = e.ExternArticle,
                            SUP_TEXT = e.ExternBrandNic,
                            MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                            GA_NR = ee.CategoryItemId,
                            GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                        }).Distinct().ToList();
            }
            return (from lup in msTDContext.EnterpriseArticleLookUpTDES
                    join e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleId equals e.ArticleId
                    join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                    where lup.ArticleSearch == ART_ARTICLE_NR
                    select new ANALOGOUS_REST_TD
                    {
                        ART_ID = e.ArticleId,
                        ART_ARTICLE_NR = e.ExternArticle,
                        SUP_TEXT = e.ExternBrandNic,
                        MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                        GA_NR = ee.CategoryItemId,
                        GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                    }).Distinct().ToList();
        }
        return new List<ANALOGOUS_REST_TD>();
    }

    public async Task<List<ANALOGOUS_REST_TD>> GetANALOGSAsync(int LNG_ID, int COU_ID, string ART_ARTICLE_NR, int? GA_NR)
    {
        if (tecdocsrctype == 1)
        {
            List<ANALOGOUS_REST_TD> aResult = new List<ANALOGOUS_REST_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetANALOGS_TEXT(LNG_ID, COU_ID, ART_ARTICLE_NR, GA_NR);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                while (await rd.ReadAsync())
                {
                    ANALOGOUS_REST_TD aNALOGOUS_REST_TD = new ANALOGOUS_REST_TD();
                    if (!rd.IsDBNull(0))
                    {
                        aNALOGOUS_REST_TD.ART_ID = Convert.ToInt32(rd.GetValue(0));
                    }
                    if (!rd.IsDBNull(1))
                    {
                        aNALOGOUS_REST_TD.ART_ARTICLE_NR = Convert.ToString(rd.GetValue(1));
                    }
                    if (!rd.IsDBNull(2))
                    {
                        aNALOGOUS_REST_TD.GA_NR = Convert.ToInt32(rd.GetValue(2));
                    }
                    if (!rd.IsDBNull(3))
                    {
                        aNALOGOUS_REST_TD.MASTER_BEZ = Convert.ToString(rd.GetValue(3));
                    }
                    if (!rd.IsDBNull(4))
                    {
                        aNALOGOUS_REST_TD.GA_TEXT = Convert.ToString(rd.GetValue(4));
                    }
                    if (!rd.IsDBNull(5))
                    {
                        aNALOGOUS_REST_TD.SUP_TEXT = Convert.ToString(rd.GetValue(5));
                    }
                    aResult.Add(aNALOGOUS_REST_TD);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        if (tecdocsrctype == 2)
        {
            if (!string.IsNullOrEmpty(ART_ARTICLE_NR))
            {
                ART_ARTICLE_NR = ART_ARTICLE_NR.Replace(".", "").Replace("-", "").Replace(" ", "")
                    .Replace("/", "")
                    .Replace("\\", "");
            }
            if (GA_NR.HasValue && GA_NR.Value != 0)
            {
                int gnr = GA_NR.Value;
                return await (from lup in msTDContext.EnterpriseArticleLookUpTDES
                              join e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleId equals e.ArticleId
                              join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                              where lup.ArticleSearch == ART_ARTICLE_NR && ee.CategoryItemId == gnr
                              select new ANALOGOUS_REST_TD
                              {
                                  ART_ID = e.ArticleId,
                                  ART_ARTICLE_NR = e.ExternArticle,
                                  SUP_TEXT = e.ExternBrandNic,
                                  MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                                  GA_NR = ee.CategoryItemId,
                                  GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                              }).Distinct().ToListAsync();
            }
            return await (from lup in msTDContext.EnterpriseArticleLookUpTDES
                          join e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleId equals e.ArticleId
                          join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                          where lup.ArticleSearch == ART_ARTICLE_NR
                          select new ANALOGOUS_REST_TD
                          {
                              ART_ID = e.ArticleId,
                              ART_ARTICLE_NR = e.ExternArticle,
                              SUP_TEXT = e.ExternBrandNic,
                              MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                              GA_NR = ee.CategoryItemId,
                              GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                          }).Distinct().ToListAsync();
        }
        return new List<ANALOGOUS_REST_TD>();
    }

    public List<MODELTYPETREEITEMS_TD> GetArticleByID(int LNG_ID, int COU_ID, string ART_ARTICLE_NR, int searchType)
    {
        List<MODELTYPETREEITEMS_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<MODELTYPETREEITEMS_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleByID_TEXT(LNG_ID, COU_ID, ART_ARTICLE_NR, searchType);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            int num = 0;
            try
            {
                int num2 = 0;
                while (odbcDataReader.Read())
                {
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        num2 = Convert.ToInt32(odbcDataReader.GetValue(0));
                        if (num2 != num)
                        {
                            num = num2;
                            MODELTYPETREEITEMS_TD mODELTYPETREEITEMS_TD = new MODELTYPETREEITEMS_TD();
                            mODELTYPETREEITEMS_TD.ART_ID = num2;
                            if (!odbcDataReader.IsDBNull(1))
                            {
                                mODELTYPETREEITEMS_TD.ART_ARTICLE_NR = Convert.ToString(odbcDataReader.GetValue(1));
                            }
                            if (!odbcDataReader.IsDBNull(2))
                            {
                                mODELTYPETREEITEMS_TD.SUP_ID = Convert.ToInt32(odbcDataReader.GetValue(2));
                            }
                            if (!odbcDataReader.IsDBNull(3))
                            {
                                mODELTYPETREEITEMS_TD.SUP_TEXT = Convert.ToString(odbcDataReader.GetValue(3));
                            }
                            if (!odbcDataReader.IsDBNull(4))
                            {
                                mODELTYPETREEITEMS_TD.MASTER_BEZ = Convert.ToString(odbcDataReader.GetValue(4));
                            }
                            if (!odbcDataReader.IsDBNull(5))
                            {
                                mODELTYPETREEITEMS_TD.GA_NR = Convert.ToInt32(odbcDataReader.GetValue(5));
                            }
                            list.Add(mODELTYPETREEITEMS_TD);
                        }
                    }
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            switch (searchType)
            {
                case 0:
                    if (!string.IsNullOrEmpty(ART_ARTICLE_NR))
                    {
                        ART_ARTICLE_NR = ART_ARTICLE_NR.Replace(" ", "").Replace("/", "").Replace("\\", "")
                            .Replace(".", "")
                            .Replace("-", "");
                    }
                    return (from lup in msTDContext.EnterpriseArticleLookUpTDES
                            join e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleId equals e.ArticleId
                            join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                            where lup.ArticleSearch == ART_ARTICLE_NR && lup.ArticleSearchKind == 1
                            select new MODELTYPETREEITEMS_TD
                            {
                                ART_ID = e.ArticleId,
                                ART_ARTICLE_NR = e.ExternArticle,
                                SUP_ID = e.ArticleBrandId,
                                SUP_TEXT = e.ExternBrandNic,
                                MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                                GA_NR = ee.CategoryItemId,
                                GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                            }).ToList();
                case 1:
                    {
                        int aFilter = 0;
                        if (!int.TryParse(ART_ARTICLE_NR, out aFilter))
                        {
                            aFilter = 0;
                        }
                        return (from e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription")
                                join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                                where e.ArticleId == aFilter
                                select new MODELTYPETREEITEMS_TD
                                {
                                    ART_ID = e.ArticleId,
                                    ART_ARTICLE_NR = e.ExternArticle,
                                    SUP_ID = e.ArticleBrandId,
                                    SUP_TEXT = e.ExternBrandNic,
                                    MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                                    GA_NR = ee.CategoryItemId,
                                    GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                                }).ToList();
                    }
                case 2:
                    if (!string.IsNullOrEmpty(ART_ARTICLE_NR))
                    {
                        ART_ARTICLE_NR = ART_ARTICLE_NR.Replace(" ", "").Replace("/", "").Replace("\\", "")
                            .Replace(".", "")
                            .Replace("-", "");
                    }
                    return (from lup in msTDContext.EnterpriseArticleLookUpTDES
                            join e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleId equals e.ArticleId
                            join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                            where lup.ArticleSearch == ART_ARTICLE_NR && lup.ArticleSearchKind == 3
                            select new MODELTYPETREEITEMS_TD
                            {
                                ART_ID = e.ArticleId,
                                ART_ARTICLE_NR = e.ExternArticle,
                                SUP_ID = e.ArticleBrandId,
                                SUP_TEXT = e.ExternBrandNic,
                                MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                                GA_NR = ee.CategoryItemId,
                                GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                            }).ToList();
                default:
                    return (from e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription")
                            join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                            where e.ExternArticleEAN == ART_ARTICLE_NR
                            select new MODELTYPETREEITEMS_TD
                            {
                                ART_ID = e.ArticleId,
                                ART_ARTICLE_NR = e.ExternArticle,
                                SUP_ID = e.ArticleBrandId,
                                SUP_TEXT = e.ExternBrandNic,
                                MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                                GA_NR = ee.CategoryItemId,
                                GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                            }).ToList();
            }
        }
        return new List<MODELTYPETREEITEMS_TD>();
    }

    public async Task<List<MODELTYPETREEITEMS_TD>> GetArticleByIDAsync(int LNG_ID, int COU_ID, string ART_ARTICLE_NR, int searchType)
    {
        if (tecdocsrctype == 1)
        {
            List<MODELTYPETREEITEMS_TD> aResult = new List<MODELTYPETREEITEMS_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleByID_TEXT(LNG_ID, COU_ID, ART_ARTICLE_NR, searchType);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            int CurrentId = 0;
            try
            {
                while (await rd.ReadAsync())
                {
                    if (!rd.IsDBNull(0))
                    {
                        int itemId = Convert.ToInt32(rd.GetValue(0));
                        if (itemId != CurrentId)
                        {
                            CurrentId = itemId;
                            MODELTYPETREEITEMS_TD mODELTYPETREEITEMS_TD = new MODELTYPETREEITEMS_TD();
                            mODELTYPETREEITEMS_TD.ART_ID = itemId;
                            if (!rd.IsDBNull(1))
                            {
                                mODELTYPETREEITEMS_TD.ART_ARTICLE_NR = Convert.ToString(rd.GetValue(1));
                            }
                            if (!rd.IsDBNull(2))
                            {
                                mODELTYPETREEITEMS_TD.SUP_ID = Convert.ToInt32(rd.GetValue(2));
                            }
                            if (!rd.IsDBNull(3))
                            {
                                mODELTYPETREEITEMS_TD.SUP_TEXT = Convert.ToString(rd.GetValue(3));
                            }
                            if (!rd.IsDBNull(4))
                            {
                                mODELTYPETREEITEMS_TD.MASTER_BEZ = Convert.ToString(rd.GetValue(4));
                            }
                            if (!rd.IsDBNull(5))
                            {
                                mODELTYPETREEITEMS_TD.GA_NR = Convert.ToInt32(rd.GetValue(5));
                            }
                            aResult.Add(mODELTYPETREEITEMS_TD);
                        }
                    }
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        if (tecdocsrctype == 2)
        {
            switch (searchType)
            {
                case 0:
                    if (!string.IsNullOrEmpty(ART_ARTICLE_NR))
                    {
                        ART_ARTICLE_NR = ART_ARTICLE_NR.Replace(" ", "").Replace("/", "").Replace("\\", "")
                            .Replace(".", "")
                            .Replace("-", "");
                    }
                    return await (from lup in msTDContext.EnterpriseArticleLookUpTDES
                                  join e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleId equals e.ArticleId
                                  join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                                  where lup.ArticleSearch == ART_ARTICLE_NR && lup.ArticleSearchKind == 1
                                  select new MODELTYPETREEITEMS_TD
                                  {
                                      ART_ID = e.ArticleId,
                                      ART_ARTICLE_NR = e.ExternArticle,
                                      SUP_ID = e.ArticleBrandId,
                                      SUP_TEXT = e.ExternBrandNic,
                                      MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                                      GA_NR = ee.CategoryItemId,
                                      GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                                  }).ToListAsync();
                case 1:
                    {
                        int aFilter = 0;
                        if (!int.TryParse(ART_ARTICLE_NR, out aFilter))
                        {
                            aFilter = 0;
                        }
                        return await (from e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription")
                                      join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                                      where e.ArticleId == aFilter
                                      select new MODELTYPETREEITEMS_TD
                                      {
                                          ART_ID = e.ArticleId,
                                          ART_ARTICLE_NR = e.ExternArticle,
                                          SUP_ID = e.ArticleBrandId,
                                          SUP_TEXT = e.ExternBrandNic,
                                          MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                                          GA_NR = ee.CategoryItemId,
                                          GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                                      }).ToListAsync();
                    }
                case 2:
                    if (!string.IsNullOrEmpty(ART_ARTICLE_NR))
                    {
                        ART_ARTICLE_NR = ART_ARTICLE_NR.Replace(" ", "").Replace("/", "").Replace("\\", "")
                            .Replace(".", "")
                            .Replace("-", "");
                    }
                    return await (from lup in msTDContext.EnterpriseArticleLookUpTDES
                                  join e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription") on lup.ArticleId equals e.ArticleId
                                  join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                                  where lup.ArticleSearch == ART_ARTICLE_NR && lup.ArticleSearchKind == 3
                                  select new MODELTYPETREEITEMS_TD
                                  {
                                      ART_ID = e.ArticleId,
                                      ART_ARTICLE_NR = e.ExternArticle,
                                      SUP_ID = e.ArticleBrandId,
                                      SUP_TEXT = e.ExternBrandNic,
                                      MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                                      GA_NR = ee.CategoryItemId,
                                      GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                                  }).ToListAsync();
                default:
                    return await (from e in msTDContext.EnterpriseArticleTecDocTDES.Include("EnterpriseArticleTecDocDescription")
                                  join ee in msTDContext.EnterpriseArticleCategoryItemTDES on e.ArticleId equals ee.ArticleId
                                  where e.ExternArticleEAN == ART_ARTICLE_NR
                                  select new MODELTYPETREEITEMS_TD
                                  {
                                      ART_ID = e.ArticleId,
                                      ART_ARTICLE_NR = e.ExternArticle,
                                      SUP_ID = e.ArticleBrandId,
                                      SUP_TEXT = e.ExternBrandNic,
                                      MASTER_BEZ = e.EnterpriseArticleTecDocDescription.EntArticleDescription,
                                      GA_NR = ee.CategoryItemId,
                                      GA_TEXT = e.EnterpriseArticleTecDocDescription.EntArticleDescription
                                  }).ToListAsync();
            }
        }
        return new List<MODELTYPETREEITEMS_TD>();
    }

    public List<MODELTYPE_TD> GetArticleApplic(int LNG_ID, int COU_ID, int ART_ID)
    {
        List<MODELTYPE_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<MODELTYPE_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleApplic_TEXT(LNG_ID, COU_ID, ART_ID);
            DoOpenConnectionTD();
            int num = 0;
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                int num2 = 0;
                while (odbcDataReader.Read())
                {
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        num2 = Convert.ToInt32(odbcDataReader.GetValue(0));
                    }
                    if (num2 != num)
                    {
                        num = num2;
                        MODELTYPE_TD mODELTYPE_TD = new MODELTYPE_TD();
                        mODELTYPE_TD.TYP_ID = num;
                        if (!odbcDataReader.IsDBNull(1))
                        {
                            mODELTYPE_TD.TEX_TEXT = Convert.ToString(odbcDataReader.GetValue(1));
                        }
                        if (!odbcDataReader.IsDBNull(2))
                        {
                            mODELTYPE_TD.TYP_KV_BODY = Convert.ToString(odbcDataReader.GetValue(2));
                        }
                        if (!odbcDataReader.IsDBNull(3))
                        {
                            mODELTYPE_TD.TYP_PCON_START = Convert.ToString(odbcDataReader.GetValue(3));
                            if (!string.IsNullOrEmpty(mODELTYPE_TD.TYP_PCON_START) && mODELTYPE_TD.TYP_PCON_START.Length > 4)
                            {
                                mODELTYPE_TD.TYP_PCON_START = mODELTYPE_TD.TYP_PCON_START.Substring(0, 4) + "-" + mODELTYPE_TD.TYP_PCON_START.Substring(4);
                            }
                        }
                        if (!odbcDataReader.IsDBNull(4))
                        {
                            mODELTYPE_TD.TYP_PCON_END = Convert.ToString(odbcDataReader.GetValue(4));
                            if (!string.IsNullOrEmpty(mODELTYPE_TD.TYP_PCON_END) && mODELTYPE_TD.TYP_PCON_END.Length > 4)
                            {
                                mODELTYPE_TD.TYP_PCON_END = mODELTYPE_TD.TYP_PCON_END.Substring(0, 4) + "-" + mODELTYPE_TD.TYP_PCON_END.Substring(4);
                            }
                        }
                        if (!odbcDataReader.IsDBNull(5))
                        {
                            mODELTYPE_TD.TYP_KW_FROM = Convert.ToString(odbcDataReader.GetValue(5));
                        }
                        if (!odbcDataReader.IsDBNull(6))
                        {
                            mODELTYPE_TD.TYP_KW_UPTO = Convert.ToString(odbcDataReader.GetValue(6));
                        }
                        if (!odbcDataReader.IsDBNull(7))
                        {
                            mODELTYPE_TD.TYP_HP_FROM = Convert.ToString(odbcDataReader.GetValue(7));
                        }
                        if (!odbcDataReader.IsDBNull(8))
                        {
                            mODELTYPE_TD.TYP_HP_UPTO = Convert.ToString(odbcDataReader.GetValue(8));
                        }
                        if (!odbcDataReader.IsDBNull(9))
                        {
                            mODELTYPE_TD.TYP_CCM = Convert.ToString(odbcDataReader.GetValue(9));
                        }
                        if (!odbcDataReader.IsDBNull(10))
                        {
                            mODELTYPE_TD.TYP_VALVES = Convert.ToString(odbcDataReader.GetValue(10));
                        }
                        if (!odbcDataReader.IsDBNull(11))
                        {
                            mODELTYPE_TD.TYP_CYLINDERS = Convert.ToString(odbcDataReader.GetValue(11));
                        }
                        if (!odbcDataReader.IsDBNull(12))
                        {
                            mODELTYPE_TD.TYP_DOORS = Convert.ToString(odbcDataReader.GetValue(12));
                        }
                        if (!odbcDataReader.IsDBNull(13))
                        {
                            mODELTYPE_TD.TYP_KV_ABS = Convert.ToString(odbcDataReader.GetValue(13));
                        }
                        if (!odbcDataReader.IsDBNull(14))
                        {
                            mODELTYPE_TD.TYP_KV_ASR = Convert.ToString(odbcDataReader.GetValue(14));
                        }
                        if (!odbcDataReader.IsDBNull(15))
                        {
                            mODELTYPE_TD.TYP_KV_BRAKE_TYPE = Convert.ToString(odbcDataReader.GetValue(15));
                        }
                        if (!odbcDataReader.IsDBNull(16))
                        {
                            mODELTYPE_TD.TYP_KV_BRAKE_SYST = Convert.ToString(odbcDataReader.GetValue(16));
                        }
                        if (!odbcDataReader.IsDBNull(17))
                        {
                            mODELTYPE_TD.TYP_KV_FUEL = Convert.ToString(odbcDataReader.GetValue(17));
                        }
                        if (!odbcDataReader.IsDBNull(18))
                        {
                            mODELTYPE_TD.TYP_KV_FUEL_SUPPLY = Convert.ToString(odbcDataReader.GetValue(18));
                        }
                        if (!odbcDataReader.IsDBNull(19))
                        {
                            mODELTYPE_TD.TYP_KV_CATALYST = Convert.ToString(odbcDataReader.GetValue(19));
                        }
                        if (!odbcDataReader.IsDBNull(20))
                        {
                            mODELTYPE_TD.TYP_KV_TRANS = Convert.ToString(odbcDataReader.GetValue(20));
                        }
                        if (!odbcDataReader.IsDBNull(21))
                        {
                            mODELTYPE_TD.TYP_KV_ENGINE = Convert.ToString(odbcDataReader.GetValue(21));
                        }
                        list.Add(mODELTYPE_TD);
                    }
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            return (from e in msTDContext.EnterpriseArticleApplicTDES
                    join mm in msTDContext.EnterpriseCarModelTDES on e.EnterpriseCarModelId equals mm.EnterpriseCarModelId
                    where e.ArticleId == ART_ID
                    select new MODELTYPE_TD
                    {
                        TYP_ID = mm.EnterpriseCarModelId,
                        TEX_TEXT = mm.EnterpriseCarModelName,
                        TYP_KV_BODY = mm.EnterpriseCarModelBody,
                        TYP_PCON_START = mm.EnterpriseCarModelProductDateStart,
                        TYP_PCON_END = mm.EnterpriseCarModelProductDateTil,
                        TYP_KW_FROM = mm.EnterpriseCarModelPowerKW,
                        TYP_HP_FROM = mm.EnterpriseCarModelPowerHP,
                        TYP_CCM = mm.EnterpriseCarModelEngCap,
                        TYP_VALVES = mm.EnterpriseCarModelVALVES,
                        TYP_CYLINDERS = mm.EnterpriseCarModelCYLINDERS,
                        TYP_KV_ABS = mm.EnterpriseCarModelABS,
                        TYP_KV_ASR = mm.EnterpriseCarModelASR,
                        TYP_KV_BRAKE_TYPE = mm.EnterpriseCarModelBrakeType,
                        TYP_KV_BRAKE_SYST = mm.EnterpriseCarModelBrakeSys,
                        FUEL_ID = mm.FUELId,
                        TYP_KV_FUEL_SUPPLY = mm.EnterpriseCarModelFUELSUPPLY,
                        TYP_KV_CATALYST = mm.EnterpriseCarModelCATALYST,
                        TYP_KV_TRANS = mm.EnterpriseCarModelTRANS,
                        TYP_KV_ENGINE = mm.EnterpriseCarModelENGCODE
                    }).ToList();
        }
        return new List<MODELTYPE_TD>();
    }

    public async Task<IPagedList<MODELTYPE_TD>> GetArticleApplicAsync(int LNG_ID, int COU_ID, int ART_ID, int pageNumber, int pageSize)
    {
        List<MODELTYPE_TD> aResult2;
        if (tecdocsrctype == 1)
        {
            aResult2 = new List<MODELTYPE_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleApplic_TEXT(LNG_ID, COU_ID, ART_ID);
            DoOpenConnectionTD();
            int CurrentId = 0;
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                int itemId = 0;
                while (await rd.ReadAsync())
                {
                    if (!rd.IsDBNull(0))
                    {
                        itemId = Convert.ToInt32(rd.GetValue(0));
                    }
                    if (itemId != CurrentId)
                    {
                        CurrentId = itemId;
                        MODELTYPE_TD mODELTYPE_TD = new MODELTYPE_TD();
                        mODELTYPE_TD.TYP_ID = CurrentId;
                        if (!rd.IsDBNull(1))
                        {
                            mODELTYPE_TD.TEX_TEXT = Convert.ToString(rd.GetValue(1));
                        }
                        if (!rd.IsDBNull(2))
                        {
                            mODELTYPE_TD.TYP_KV_BODY = Convert.ToString(rd.GetValue(2));
                        }
                        if (!rd.IsDBNull(3))
                        {
                            mODELTYPE_TD.TYP_PCON_START = Convert.ToString(rd.GetValue(3));
                            if (!string.IsNullOrEmpty(mODELTYPE_TD.TYP_PCON_START) && mODELTYPE_TD.TYP_PCON_START.Length > 4)
                            {
                                mODELTYPE_TD.TYP_PCON_START = mODELTYPE_TD.TYP_PCON_START.Substring(0, 4) + "-" + mODELTYPE_TD.TYP_PCON_START.Substring(4);
                            }
                        }
                        if (!rd.IsDBNull(4))
                        {
                            mODELTYPE_TD.TYP_PCON_END = Convert.ToString(rd.GetValue(4));
                            if (!string.IsNullOrEmpty(mODELTYPE_TD.TYP_PCON_END) && mODELTYPE_TD.TYP_PCON_END.Length > 4)
                            {
                                mODELTYPE_TD.TYP_PCON_END = mODELTYPE_TD.TYP_PCON_END.Substring(0, 4) + "-" + mODELTYPE_TD.TYP_PCON_END.Substring(4);
                            }
                        }
                        if (!rd.IsDBNull(5))
                        {
                            mODELTYPE_TD.TYP_KW_FROM = Convert.ToString(rd.GetValue(5));
                        }
                        if (!rd.IsDBNull(6))
                        {
                            mODELTYPE_TD.TYP_KW_UPTO = Convert.ToString(rd.GetValue(6));
                        }
                        if (!rd.IsDBNull(7))
                        {
                            mODELTYPE_TD.TYP_HP_FROM = Convert.ToString(rd.GetValue(7));
                        }
                        if (!rd.IsDBNull(8))
                        {
                            mODELTYPE_TD.TYP_HP_UPTO = Convert.ToString(rd.GetValue(8));
                        }
                        if (!rd.IsDBNull(9))
                        {
                            mODELTYPE_TD.TYP_CCM = Convert.ToString(rd.GetValue(9));
                        }
                        if (!rd.IsDBNull(10))
                        {
                            mODELTYPE_TD.TYP_VALVES = Convert.ToString(rd.GetValue(10));
                        }
                        if (!rd.IsDBNull(11))
                        {
                            mODELTYPE_TD.TYP_CYLINDERS = Convert.ToString(rd.GetValue(11));
                        }
                        if (!rd.IsDBNull(12))
                        {
                            mODELTYPE_TD.TYP_DOORS = Convert.ToString(rd.GetValue(12));
                        }
                        if (!rd.IsDBNull(13))
                        {
                            mODELTYPE_TD.TYP_KV_ABS = Convert.ToString(rd.GetValue(13));
                        }
                        if (!rd.IsDBNull(14))
                        {
                            mODELTYPE_TD.TYP_KV_ASR = Convert.ToString(rd.GetValue(14));
                        }
                        if (!rd.IsDBNull(15))
                        {
                            mODELTYPE_TD.TYP_KV_BRAKE_TYPE = Convert.ToString(rd.GetValue(15));
                        }
                        if (!rd.IsDBNull(16))
                        {
                            mODELTYPE_TD.TYP_KV_BRAKE_SYST = Convert.ToString(rd.GetValue(16));
                        }
                        if (!rd.IsDBNull(17))
                        {
                            mODELTYPE_TD.TYP_KV_FUEL = Convert.ToString(rd.GetValue(17));
                        }
                        if (!rd.IsDBNull(18))
                        {
                            mODELTYPE_TD.TYP_KV_FUEL_SUPPLY = Convert.ToString(rd.GetValue(18));
                        }
                        if (!rd.IsDBNull(19))
                        {
                            mODELTYPE_TD.TYP_KV_CATALYST = Convert.ToString(rd.GetValue(19));
                        }
                        if (!rd.IsDBNull(20))
                        {
                            mODELTYPE_TD.TYP_KV_TRANS = Convert.ToString(rd.GetValue(20));
                        }
                        if (!rd.IsDBNull(21))
                        {
                            mODELTYPE_TD.TYP_KV_ENGINE = Convert.ToString(rd.GetValue(21));
                        }
                        aResult2.Add(mODELTYPE_TD);
                    }
                }
            }
            finally
            {
                rd.Close();
            }
            return (from zzz in aResult2
                    orderby zzz.TYP_ID
                    select zzz).ToPagedList(pageNumber, pageSize);
        }
        if (tecdocsrctype == 2)
        {
            return await (from e in msTDContext.EnterpriseArticleApplicTDES
                          join mm in msTDContext.EnterpriseCarModelTDES on e.EnterpriseCarModelId equals mm.EnterpriseCarModelId
                          where e.ArticleId == ART_ID
                          select new MODELTYPE_TD
                          {
                              TYP_ID = mm.EnterpriseCarModelId,
                              TEX_TEXT = mm.EnterpriseCarModelName,
                              TYP_KV_BODY = mm.EnterpriseCarModelBody,
                              TYP_PCON_START = mm.EnterpriseCarModelProductDateStart,
                              TYP_PCON_END = mm.EnterpriseCarModelProductDateTil,
                              TYP_KW_FROM = mm.EnterpriseCarModelPowerKW,
                              TYP_HP_FROM = mm.EnterpriseCarModelPowerHP,
                              TYP_CCM = mm.EnterpriseCarModelEngCap,
                              TYP_VALVES = mm.EnterpriseCarModelVALVES,
                              TYP_CYLINDERS = mm.EnterpriseCarModelCYLINDERS,
                              TYP_KV_ABS = mm.EnterpriseCarModelABS,
                              TYP_KV_ASR = mm.EnterpriseCarModelASR,
                              TYP_KV_BRAKE_TYPE = mm.EnterpriseCarModelBrakeType,
                              TYP_KV_BRAKE_SYST = mm.EnterpriseCarModelBrakeSys,
                              FUEL_ID = mm.FUELId,
                              TYP_KV_FUEL_SUPPLY = mm.EnterpriseCarModelFUELSUPPLY,
                              TYP_KV_CATALYST = mm.EnterpriseCarModelCATALYST,
                              TYP_KV_TRANS = mm.EnterpriseCarModelTRANS,
                              TYP_KV_ENGINE = mm.EnterpriseCarModelENGCODE
                          } into eee
                          orderby eee.TYP_ID
                          select eee).ToPagedListAsync(pageNumber, pageSize);
        }
        aResult2 = new List<MODELTYPE_TD>();
        return aResult2.ToPagedList(pageNumber, pageSize);
    }

    public List<FUEL_TD> GetArticleEan(int ART_ID)
    {
        List<FUEL_TD> list = null;
        if (tecdocsrctype == 1)
        {
            list = new List<FUEL_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleEan_TEXT(ART_ID);
            DoOpenConnectionTD();
            OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
            try
            {
                while (odbcDataReader.Read())
                {
                    FUEL_TD fUEL_TD = new FUEL_TD();
                    if (!odbcDataReader.IsDBNull(0))
                    {
                        fUEL_TD.TEX_TEXT = Convert.ToString(odbcDataReader.GetValue(0));
                    }
                    list.Add(fUEL_TD);
                }
                return list;
            }
            finally
            {
                odbcDataReader.Close();
            }
        }
        if (tecdocsrctype == 2)
        {
            list = (from e in msTDContext.EnterpriseArticleTecDocTDES
                    where e.ArticleId == ART_ID
                    select new FUEL_TD
                    {
                        TEX_TEXT = e.ExternArticleEAN
                    }).ToList();
            if (list.Count > 0)
            {
                for (int num = list.Count - 1; num > -1; num--)
                {
                    FUEL_TD fUEL_TD2 = list[num];
                    if (string.IsNullOrEmpty(fUEL_TD2.TEX_TEXT))
                    {
                        list.RemoveAt(num);
                    }
                }
            }
            return list;
        }
        return new List<FUEL_TD>();
    }

    public async Task<List<FUEL_TD>> GetArticleEanAsync(int ART_ID)
    {
        if (tecdocsrctype == 1)
        {
            List<FUEL_TD> aResult2 = new List<FUEL_TD>();
            OdbcCommandTD.CommandType = CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleEan_TEXT(ART_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
            try
            {
                while (await rd.ReadAsync())
                {
                    FUEL_TD fUEL_TD = new FUEL_TD();
                    if (!rd.IsDBNull(0))
                    {
                        fUEL_TD.TEX_TEXT = Convert.ToString(rd.GetValue(0));
                    }
                    aResult2.Add(fUEL_TD);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult2;
        }
        if (tecdocsrctype == 2)
        {
            List<FUEL_TD> aResult2 = await (from e in msTDContext.EnterpriseArticleTecDocTDES
                                            where e.ArticleId == ART_ID
                                            select new FUEL_TD
                                            {
                                                TEX_TEXT = e.ExternArticleEAN
                                            }).ToListAsync();
            if (aResult2.Count > 0)
            {
                for (int num = aResult2.Count - 1; num > -1; num--)
                {
                    FUEL_TD fUEL_TD2 = aResult2[num];
                    if (string.IsNullOrEmpty(fUEL_TD2.TEX_TEXT))
                    {
                        aResult2.RemoveAt(num);
                    }
                }
            }
            return aResult2;
        }
        return new List<FUEL_TD>();
    }

    public byte[] GetPhoto(int GRD_FLD, int GRD_ID)
    {
        byte[] result = null;
        OdbcCommandTD.CommandType = CommandType.Text;
        OdbcCommandTD.CommandText = GetPhoto_TEXT(GRD_FLD, GRD_ID);
        DoOpenConnectionTD();
        OdbcDataReader odbcDataReader = OdbcCommandTD.ExecuteReader();
        try
        {
            while (odbcDataReader.Read())
            {
                if (!odbcDataReader.IsDBNull(0))
                {
                    long bytes = odbcDataReader.GetBytes(0, 0L, null, 0, 128000);
                    if (bytes > 0)
                    {
                        result = new byte[bytes];
                        odbcDataReader.GetBytes(0, 0L, result, 0, (int)bytes);
                        return result;
                    }
                }
            }
            return result;
        }
        finally
        {
            odbcDataReader.Close();
        }
    }

    public async Task<byte[]> GetPhotoAsync(int GRD_FLD, int GRD_ID)
    {
        byte[] aResult2 = null;
        OdbcCommandTD.CommandType = CommandType.Text;
        OdbcCommandTD.CommandText = GetPhoto_TEXT(GRD_FLD, GRD_ID);
        DoOpenConnectionTD();
        OdbcDataReader rd = (OdbcDataReader)(await OdbcCommandTD.ExecuteReaderAsync());
        try
        {
            while (await rd.ReadAsync())
            {
                if (!rd.IsDBNull(0))
                {
                    long bytes = rd.GetBytes(0, 0L, null, 0, 128000);
                    if (bytes > 0)
                    {
                        aResult2 = new byte[bytes];
                        rd.GetBytes(0, 0L, aResult2, 0, (int)bytes);
                        return aResult2;
                    }
                }
            }
            return aResult2;
        }
        finally
        {
            rd.Close();
        }
    }

    public object GetAllArticle_READER(int LNG_ID, int COU_ID)
    {
        OdbcCommandTD.CommandType = CommandType.Text;
        OdbcCommandTD.CommandText = GetAllArticle_TEXT(LNG_ID, COU_ID);
        DoOpenConnectionTD();
        return OdbcCommandTD.ExecuteReader();
    }

    public MODELTYPETREEITEMS_TD GetAllArticle_NEXT(object reader)
    {
        if (reader == null)
        {
            return null;
        }
        if (!(reader is OdbcDataReader))
        {
            return null;
        }
        OdbcDataReader odbcDataReader = reader as OdbcDataReader;
        MODELTYPETREEITEMS_TD mODELTYPETREEITEMS_TD = null;
        while (odbcDataReader.Read())
        {
            if (!odbcDataReader.IsDBNull(0))
            {
                mODELTYPETREEITEMS_TD = new MODELTYPETREEITEMS_TD();
                mODELTYPETREEITEMS_TD.ART_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                if (!odbcDataReader.IsDBNull(1))
                {
                    mODELTYPETREEITEMS_TD.ART_ARTICLE_NR = Convert.ToString(odbcDataReader.GetValue(1));
                }
                if (!odbcDataReader.IsDBNull(2))
                {
                    mODELTYPETREEITEMS_TD.SUP_ID = Convert.ToInt32(odbcDataReader.GetValue(2));
                }
                if (!odbcDataReader.IsDBNull(3))
                {
                    mODELTYPETREEITEMS_TD.SUP_TEXT = Convert.ToString(odbcDataReader.GetValue(3));
                }
                if (!odbcDataReader.IsDBNull(4))
                {
                    mODELTYPETREEITEMS_TD.MASTER_BEZ = Convert.ToString(odbcDataReader.GetValue(4));
                }
                if (!odbcDataReader.IsDBNull(5))
                {
                    mODELTYPETREEITEMS_TD.EAN_TEXT = Convert.ToString(odbcDataReader.GetValue(5));
                }
                return mODELTYPETREEITEMS_TD;
            }
        }
        odbcDataReader.Close();
        return null;
    }

    public object GetArticleGroup()
    {
        OdbcCommandTD.CommandType = CommandType.Text;
        OdbcCommandTD.CommandText = GetArticleGroup_text();
        DoOpenConnectionTD();
        return OdbcCommandTD.ExecuteReader();
    }

    public ArticleGroup_TD GetArticleGroup_NEXT(object reader)
    {
        if (reader == null)
        {
            return null;
        }
        if (!(reader is OdbcDataReader))
        {
            return null;
        }
        OdbcDataReader odbcDataReader = reader as OdbcDataReader;
        ArticleGroup_TD articleGroup_TD = null;
        while (odbcDataReader.Read())
        {
            if (!odbcDataReader.IsDBNull(0))
            {
                articleGroup_TD = new ArticleGroup_TD();
                articleGroup_TD.ART_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                articleGroup_TD.GROUP_ID = Convert.ToInt32(odbcDataReader.GetValue(1));
                return articleGroup_TD;
            }
        }
        odbcDataReader.Close();
        return null;
    }

    public object GetArticleBrand()
    {
        OdbcCommandTD.CommandType = CommandType.Text;
        OdbcCommandTD.CommandText = GetArticleBrand_TEXT();
        DoOpenConnectionTD();
        return OdbcCommandTD.ExecuteReader();
    }

    public BRAND_TD GetArticleBrand_NEXT(object reader)
    {
        if (reader == null)
        {
            return null;
        }
        if (!(reader is OdbcDataReader))
        {
            return null;
        }
        OdbcDataReader odbcDataReader = reader as OdbcDataReader;
        BRAND_TD bRAND_TD = null;
        while (odbcDataReader.Read())
        {
            if (!odbcDataReader.IsDBNull(0))
            {
                bRAND_TD = new BRAND_TD();
                bRAND_TD.MFA_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                bRAND_TD.MFA_BRAND = Convert.ToString(odbcDataReader.GetValue(1));
                return bRAND_TD;
            }
        }
        odbcDataReader.Close();
        return null;
    }

    protected string GetArticleLookUp_TEXT()
    {
        return "select ARL_ART_ID,ARL_SEARCH_NUMBER,ARL_KIND,ARL_BRA_ID,ARL_DISPLAY_NR from TOF_ART_LOOKUP ";
    }

    public object GetArticleLookUp()
    {
        OdbcCommandTD.CommandType = CommandType.Text;
        OdbcCommandTD.CommandText = GetArticleLookUp_TEXT();
        DoOpenConnectionTD();
        return OdbcCommandTD.ExecuteReader();
    }

    public ARTICLE_LOOKUP_TD GetArticleLookUp_NEXT(object reader)
    {
        if (reader == null)
        {
            return null;
        }
        if (!(reader is OdbcDataReader))
        {
            return null;
        }
        OdbcDataReader odbcDataReader = reader as OdbcDataReader;
        ARTICLE_LOOKUP_TD aRTICLE_LOOKUP_TD = null;
        while (odbcDataReader.Read())
        {
            if (!odbcDataReader.IsDBNull(0))
            {
                aRTICLE_LOOKUP_TD = new ARTICLE_LOOKUP_TD();
                aRTICLE_LOOKUP_TD.ARL_ART_ID = Convert.ToInt32(odbcDataReader.GetValue(0));
                aRTICLE_LOOKUP_TD.ARL_SEARCH_NUMBER = Convert.ToString(odbcDataReader.GetValue(1));
                switch (Convert.ToChar(odbcDataReader.GetValue(2)))
                {
                    case '1':
                        aRTICLE_LOOKUP_TD.ARL_KIND = 1;
                        break;
                    case '2':
                        aRTICLE_LOOKUP_TD.ARL_KIND = 2;
                        break;
                    case '3':
                        aRTICLE_LOOKUP_TD.ARL_KIND = 3;
                        break;
                    case '4':
                        aRTICLE_LOOKUP_TD.ARL_KIND = 4;
                        break;
                    case '5':
                        aRTICLE_LOOKUP_TD.ARL_KIND = 5;
                        break;
                    default:
                        aRTICLE_LOOKUP_TD.ARL_KIND = 0;
                        break;
                }
                aRTICLE_LOOKUP_TD.ARL_BRA_ID = Convert.ToInt32(odbcDataReader.GetValue(3));
                aRTICLE_LOOKUP_TD.ARL_DISPLAY_NR = Convert.ToString(odbcDataReader.GetValue(4));
                return aRTICLE_LOOKUP_TD;
            }
        }
        odbcDataReader.Close();
        return null;
    }

    public void Dispose()
    {
        if (mstecdoccontext != null)
        {
            mstecdoccontext.Dispose();
            mstecdoccontext = null;
        }
        if (odbcCommandTD != null)
        {
            odbcCommandTD.Dispose();
        }
        if (odbcConnectionTD != null)
        {
            odbcConnectionTD.Dispose();
        }
    }
}
