using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace TecDocEcoSystemDbClassLibrary
{
    public class TecDocContext : IDisposable, ITecDocContext
    {

        protected string GetLANGUAGES_TEXT(int LNG_ID)
        {
            return 
            "SELECT " +
            "TOF_LANGUAGES.LNG_ID,  " +
                //            "TOF_LANGUAGES.LNG_DES_ID,"+
                //            "TOF_DESIGNATIONS.DES_ID,"+
                //            "TOF_DESIGNATIONS.DES_LNG_ID, "+
                //            "TOF_DESIGNATIONS.DES_TEX_ID, "+
                //            "TOF_DES_TEXTS.TEX_ID, "+
            "TOF_DES_TEXTS.TEX_TEXT " +
            "FROM TOF_LANGUAGES " +
            "INNER JOIN TOF_DESIGNATIONS ON TOF_DESIGNATIONS.DES_ID = TOF_LANGUAGES.LNG_DES_ID " +
            "INNER JOIN TOF_DES_TEXTS ON TOF_DESIGNATIONS.DES_TEX_ID = TOF_DES_TEXTS.TEX_ID " +
            "WHERE ((TOF_DESIGNATIONS.DES_LNG_ID = " + LNG_ID.ToString() + "))";
        }
        protected string GetCOUNTRIES_TEXT(int LNG_ID)
        {
            return
            "SELECT " +
            "TOF_COUNTRIES.COU_ID, " +
            "TOF_DES_TEXTS.TEX_TEXT " +
            "FROM TOF_COUNTRIES " +
            "INNER JOIN TOF_DESIGNATIONS ON TOF_DESIGNATIONS.DES_ID = TOF_COUNTRIES.COU_DES_ID " +
            "INNER JOIN TOF_DES_TEXTS ON TOF_DESIGNATIONS.DES_TEX_ID = TOF_DES_TEXTS.TEX_ID " +
            "WHERE ((TOF_DESIGNATIONS.DES_LNG_ID = " + LNG_ID.ToString() + ")) ";
        }
        protected string GetBRANDS_TEXT()
        {
            return
            "SELECT " +
            "MFA_ID, " +
            "MFA_BRAND " +
//            "MFA_PC_MFC " +
            "FROM TOF_MANUFACTURERS WHERE MFA_PC_MFC = 1 ORDER BY MFA_BRAND "; 
        }
        protected string GetMODELS_TEXT(int LNG_ID, int COU_ID, int MFA_ID, int ? MOD_ID = null)
        {
            return
            "SELECT " +
            "TOF_MODELS.MOD_ID, " +
//            "TOF_MODELS.MOD_MFA_ID, " +
//            "TOF_MODELS.MOD_CDS_ID,  " +
//            "TOF_COUNTRY_DESIGNATIONS.CDS_ID,  " +
//            "TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID,  " +
//            "TOF_DES_TEXTS.TEX_ID,  " +
            "TOF_DES_TEXTS.TEX_TEXT  " +
            "FROM TOF_MODELS  " +
            "INNER JOIN TOF_COUNTRY_DESIGNATIONS ON TOF_COUNTRY_DESIGNATIONS.CDS_ID = TOF_MODELS.MOD_CDS_ID  " +
            "INNER JOIN TOF_DES_TEXTS ON TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID = TOF_DES_TEXTS.TEX_ID  " +
            "WHERE ((TOF_MODELS.MOD_MFA_ID = " + MFA_ID.ToString() + " AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + " AND MOD_PC_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1))  " +
            (MOD_ID.HasValue ? (" AND TOF_MODELS.MOD_ID = " + MOD_ID.Value.ToString() ) : ("")  ) +
            " ORDER BY TOF_DES_TEXTS.TEX_TEXT ";
        }
        protected string GetMODELS_EX_TEXT(int LNG_ID, int COU_ID, int MFA_ID, int? MOD_ID = null)
        {
            return
            "SELECT " +
            "TOF_MODELS.MOD_ID, " +
                //            "TOF_MODELS.MOD_MFA_ID, " +
                //            "TOF_MODELS.MOD_CDS_ID,  " +
                //            "TOF_COUNTRY_DESIGNATIONS.CDS_ID,  " +
                //            "TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID,  " +
                //            "TOF_DES_TEXTS.TEX_ID,  " +
            "TOF_DES_TEXTS.TEX_TEXT  " +
            "FROM TOF_MODELS  " +
            "INNER JOIN TOF_COUNTRY_DESIGNATIONS ON TOF_COUNTRY_DESIGNATIONS.CDS_ID = TOF_MODELS.MOD_CDS_ID  " +
            "INNER JOIN TOF_DES_TEXTS ON TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID = TOF_DES_TEXTS.TEX_ID  " +
            "WHERE ((TOF_MODELS.MOD_MFA_ID = " + MFA_ID.ToString() + " AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + "))"+
//!!!       " AND MOD_PC_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1))  " +
            (MOD_ID.HasValue ? (" AND TOF_MODELS.MOD_ID = " + MOD_ID.Value.ToString()) : ("")) +
            " ORDER BY TOF_DES_TEXTS.TEX_TEXT ";
        }
        protected string GetMODELTYPES_TEXT(int LNG_ID, int COU_ID, int MOD_ID, int? fluelId = null, int? modelTypeId = null)
        {
            return
            "SELECT " + 
            "TOF_TYPES.TYP_ID, "                                        +   /* 0  ключ */
            "TOF_DES_TEXTS.TEX_TEXT AS TEX_TEXT, "                      +   /* 1  Наименование модели полное если поле TYP_MMT_CDS_ID и краткое если поле TYP_CDS_ID */

            "TOF_DES_TEXTS2.TEX_TEXT AS TYP_KV_BODY_DESCR, "            +   /* 2  кузов */

            "TOF_TYPES.TYP_PCON_START,"                                 +   /* 3  первые 4 цифры: год начала выпуска модели, последние две месяц начала выпуска модели. */
            "TOF_TYPES.TYP_PCON_END,"                                   +   /* 4  первые 4 цифры: год окончания выпуска модели, последние две месяц окончания выпуска модели. */
                                                                  
            "TOF_TYPES.TYP_KW_FROM,"                                    +   /* 5  Техническая информация/Мощность двигателя(кВ) (от) */
            "TOF_TYPES.TYP_KW_UPTO,"                                    +   /* 6  Техническая информация/Мощность двигателя(кВ) (до) */
            "TOF_TYPES.TYP_HP_FROM,"                                    +   /* 7  Техническая информация/Мощность двигателя (ЛС) (от) */
            "TOF_TYPES.TYP_HP_UPTO,"                                    +   /* 8  Техническая информация/Мощность двигателя (ЛС) (до) */
                                                                  
            "TOF_TYPES.TYP_CCM,"                                        +   /* 9  Техническая информация/Тех. Объем куб. см. */
            "TOF_TYPES.TYP_VALVES,"                                     +   /* 10 Техническая информация/Количество клапанов на одну камеру сгорания */
            "TOF_TYPES.TYP_CYLINDERS,"                                  +   /* 11 Техническая информация/Цилиндр (Количество цилиндров) */
            "TOF_TYPES.TYP_DOORS,"                                      +   /* 12 Конструкция/Количество дверей. */

            "TOF_DES_TEXTS3.TEX_TEXT AS TYP_KV_ABS_DESCR, "             +   /* 13 ABS */
            "TOF_DES_TEXTS4.TEX_TEXT AS TYP_KV_ASR_DESCR, "             +   /* 14 ASR */
            "TOF_DES_TEXTS5.TEX_TEXT AS TYP_KV_BRAKE_TYPE_DESCR, "      +   /* 15 TYP_KV_BRAKE_TYPE Вид тормозов */ 
            "TOF_DES_TEXTS6.TEX_TEXT AS TYP_KV_BRAKE_SYST_DESCR, "      +   /* 16 TYP_KV_BRAKE_SYST Система тормозов */ 

            "TOF_DES_TEXTS7.TEX_TEXT AS TYP_KV_FUEL_DES_ID_DESCR, "     +   /* 17 TYP_KV_FUEL Вид горючего */ 
            "TOF_DES_TEXTS8.TEX_TEXT AS TYP_KV_FUEL_SUPPLY_DES_ID_DESCR, "     +   /* 18 TYP_KV_FUEL_SUPPLY впрыск */ 

            "TOF_DES_TEXTS9.TEX_TEXT AS TYP_KV_CATALYST_DES_ID_DESCR, "     +   /* 19 TYP_KV_CATALYST катализатор */ 

            "TOF_DES_TEXTS1.TEX_TEXT AS TYP_KV_TRANS_DES_ID_DESCR,  "     +   /* 20 TYP_KV_TRANS трансмиссия */

//            "TOF_DES_TEXTS_A.TEX_TEXT AS TYP_KV_ENGINE_DES_ID_DESCR  " +   /* 21 TYP_KV_ENGINE мотор */ 
            "TOF_ENGINES.ENG_CODE AS ENG_CODE " + /* 21 TYP_KV_ENGINE мотор */ 
            
//            "TOF_TYPES.TYP_MOD_ID " + /* модель  */

            "FROM TOF_TYPES " +
            "INNER JOIN TOF_COUNTRY_DESIGNATIONS ON TOF_COUNTRY_DESIGNATIONS.CDS_ID = TOF_TYPES.TYP_MMT_CDS_ID AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + " " + // 
            "INNER JOIN TOF_DES_TEXTS ON TOF_DES_TEXTS.TEX_ID = TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID " +
//            "INNER JOIN TOF_TYPE_NUMBERS TYPES ON TOF_TYPE_NUMBERS.TYN_TYP_ID TOF_TYPES.TYP_ID " +


            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS2 ON TOF_DESIGNATIONS2.DES_ID = TOF_TYPES.TYP_KV_BODY_DES_ID AND TOF_DESIGNATIONS2.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* вид конструкции */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS2 ON TOF_DES_TEXTS2.TEX_ID = TOF_DESIGNATIONS2.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS3 ON TOF_DESIGNATIONS3.DES_ID = TOF_TYPES.TYP_KV_ABS_DES_ID AND TOF_DESIGNATIONS3.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* abs */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS3 ON TOF_DES_TEXTS3.TEX_ID = TOF_DESIGNATIONS3.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS4 ON TOF_DESIGNATIONS4.DES_ID = TOF_TYPES.TYP_KV_ASR_DES_ID AND TOF_DESIGNATIONS4.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* asr */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS4 ON TOF_DES_TEXTS4.TEX_ID = TOF_DESIGNATIONS4.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS5 ON TOF_DESIGNATIONS5.DES_ID = TOF_TYPES.TYP_KV_BRAKE_TYPE_DES_ID AND TOF_DESIGNATIONS5.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* TYP_KV_BRAKE_TYPE */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS5 ON TOF_DES_TEXTS5.TEX_ID = TOF_DESIGNATIONS5.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS6 ON TOF_DESIGNATIONS6.DES_ID = TOF_TYPES.TYP_KV_BRAKE_SYST_DES_ID AND TOF_DESIGNATIONS6.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* тормозная система */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS6 ON TOF_DES_TEXTS6.TEX_ID = TOF_DESIGNATIONS6.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS7 ON TOF_DESIGNATIONS7.DES_ID = TOF_TYPES.TYP_KV_FUEL_DES_ID AND TOF_DESIGNATIONS7.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* вид топлива */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS7 ON TOF_DES_TEXTS7.TEX_ID = TOF_DESIGNATIONS7.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS8 ON TOF_DESIGNATIONS8.DES_ID = TOF_TYPES.TYP_KV_FUEL_SUPPLY_DES_ID AND TOF_DESIGNATIONS8.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* впрыск */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS8 ON TOF_DES_TEXTS8.TEX_ID = TOF_DESIGNATIONS8.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS9 ON TOF_DESIGNATIONS9.DES_ID = TOF_TYPES.TYP_KV_CATALYST_DES_ID AND TOF_DESIGNATIONS9.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* вид катализатора */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS9 ON TOF_DES_TEXTS9.TEX_ID = TOF_DESIGNATIONS9.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS1 ON TOF_DESIGNATIONS1.DES_ID = TOF_TYPES.TYP_KV_TRANS_DES_ID AND TOF_DESIGNATIONS1.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* трансмиссия */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS1 ON TOF_DES_TEXTS1.TEX_ID = TOF_DESIGNATIONS1.DES_TEX_ID " +

//            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS_A ON TOF_DESIGNATIONS_A.DES_ID = TOF_TYPES.TYP_KV_ENGINE_DES_ID AND TOF_DESIGNATIONS_A.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* мотор */
//            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS_A ON TOF_DES_TEXTS_A.TEX_ID = TOF_DESIGNATIONS_A.DES_TEX_ID " +

            "LEFT JOIN TOF_LINK_TYP_ENG ON LTE_TYP_ID = TYP_ID " +
            "LEFT JOIN TOF_ENGINES ON ENG_ID = LTE_ENG_ID " +


            " WHERE " +
            (modelTypeId.HasValue ? (" TOF_TYPES.TYP_ID = " + modelTypeId.Value.ToString()) 
                                  : (" TOF_TYPES.TYP_MOD_ID = " + MOD_ID.ToString()) +
                                    (fluelId.HasValue ? (" AND TOF_TYPES.TYP_KV_FUEL_DES_ID = " + fluelId.Value.ToString()) : (""))                      
                                  ) +

            " AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + 
            " AND TOF_TYPES.TYP_CTM subrange(" + COU_ID.ToString() +" cast integer) = 1 " + 
            " ORDER BY TOF_TYPES.TYP_SORT";
        }
        protected string GetMODELTYPES_EX_TEXT(int LNG_ID, int COU_ID, int MOD_ID, int? fluelId = null, int? modelTypeId = null)
        {
            return
            "SELECT " +
            "TOF_TYPES.TYP_ID, " +   /* 0  ключ */
            "TOF_DES_TEXTS.TEX_TEXT AS TEX_TEXT, " +   /* 1  Наименование модели полное если поле TYP_MMT_CDS_ID и краткое если поле TYP_CDS_ID */

            "TOF_DES_TEXTS2.TEX_TEXT AS TYP_KV_BODY_DESCR, " +   /* 2  кузов */

            "TOF_TYPES.TYP_PCON_START," +   /* 3  первые 4 цифры: год начала выпуска модели, последние две месяц начала выпуска модели. */
            "TOF_TYPES.TYP_PCON_END," +   /* 4  первые 4 цифры: год окончания выпуска модели, последние две месяц окончания выпуска модели. */

            "TOF_TYPES.TYP_KW_FROM," +   /* 5  Техническая информация/Мощность двигателя(кВ) (от) */
            "TOF_TYPES.TYP_KW_UPTO," +   /* 6  Техническая информация/Мощность двигателя(кВ) (до) */
            "TOF_TYPES.TYP_HP_FROM," +   /* 7  Техническая информация/Мощность двигателя (ЛС) (от) */
            "TOF_TYPES.TYP_HP_UPTO," +   /* 8  Техническая информация/Мощность двигателя (ЛС) (до) */

            "TOF_TYPES.TYP_CCM," +   /* 9  Техническая информация/Тех. Объем куб. см. */
            "TOF_TYPES.TYP_VALVES," +   /* 10 Техническая информация/Количество клапанов на одну камеру сгорания */
            "TOF_TYPES.TYP_CYLINDERS," +   /* 11 Техническая информация/Цилиндр (Количество цилиндров) */
            "TOF_TYPES.TYP_DOORS," +   /* 12 Конструкция/Количество дверей. */

            "TOF_DES_TEXTS3.TEX_TEXT AS TYP_KV_ABS_DESCR, " +   /* 13 ABS */
            "TOF_DES_TEXTS4.TEX_TEXT AS TYP_KV_ASR_DESCR, " +   /* 14 ASR */
            "TOF_DES_TEXTS5.TEX_TEXT AS TYP_KV_BRAKE_TYPE_DESCR, " +   /* 15 TYP_KV_BRAKE_TYPE Вид тормозов */
            "TOF_DES_TEXTS6.TEX_TEXT AS TYP_KV_BRAKE_SYST_DESCR, " +   /* 16 TYP_KV_BRAKE_SYST Система тормозов */

            "TOF_DES_TEXTS7.TEX_TEXT AS TYP_KV_FUEL_DES_ID_DESCR, " +   /* 17 TYP_KV_FUEL Вид горючего */
            "TOF_DES_TEXTS8.TEX_TEXT AS TYP_KV_FUEL_SUPPLY_DES_ID_DESCR, " +   /* 18 TYP_KV_FUEL_SUPPLY впрыск */

            "TOF_DES_TEXTS9.TEX_TEXT AS TYP_KV_CATALYST_DES_ID_DESCR, " +   /* 19 TYP_KV_CATALYST катализатор */

            "TOF_DES_TEXTS1.TEX_TEXT AS TYP_KV_TRANS_DES_ID_DESCR,  " +   /* 20 TYP_KV_TRANS трансмиссия */

//            "TOF_DES_TEXTS_A.TEX_TEXT AS TYP_KV_ENGINE_DES_ID_DESCR  " +   /* 21 TYP_KV_ENGINE мотор */ 
            "TOF_ENGINES.ENG_CODE AS ENG_CODE, " + /* 21 TYP_KV_ENGINE мотор */

//            "TOF_TYPES.TYP_MOD_ID " + /* модель  */

            "TOF_TYPES.TYP_KV_FUEL_DES_ID FUEL_DES_ID " +  /* 22 ID топлива */


            "FROM TOF_TYPES " +
            "INNER JOIN TOF_COUNTRY_DESIGNATIONS ON TOF_COUNTRY_DESIGNATIONS.CDS_ID = TOF_TYPES.TYP_MMT_CDS_ID AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + " " + // 
            "INNER JOIN TOF_DES_TEXTS ON TOF_DES_TEXTS.TEX_ID = TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID " +
                //            "INNER JOIN TOF_TYPE_NUMBERS TYPES ON TOF_TYPE_NUMBERS.TYN_TYP_ID TOF_TYPES.TYP_ID " +


            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS2 ON TOF_DESIGNATIONS2.DES_ID = TOF_TYPES.TYP_KV_BODY_DES_ID AND TOF_DESIGNATIONS2.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* вид конструкции */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS2 ON TOF_DES_TEXTS2.TEX_ID = TOF_DESIGNATIONS2.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS3 ON TOF_DESIGNATIONS3.DES_ID = TOF_TYPES.TYP_KV_ABS_DES_ID AND TOF_DESIGNATIONS3.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* abs */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS3 ON TOF_DES_TEXTS3.TEX_ID = TOF_DESIGNATIONS3.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS4 ON TOF_DESIGNATIONS4.DES_ID = TOF_TYPES.TYP_KV_ASR_DES_ID AND TOF_DESIGNATIONS4.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* asr */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS4 ON TOF_DES_TEXTS4.TEX_ID = TOF_DESIGNATIONS4.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS5 ON TOF_DESIGNATIONS5.DES_ID = TOF_TYPES.TYP_KV_BRAKE_TYPE_DES_ID AND TOF_DESIGNATIONS5.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* TYP_KV_BRAKE_TYPE */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS5 ON TOF_DES_TEXTS5.TEX_ID = TOF_DESIGNATIONS5.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS6 ON TOF_DESIGNATIONS6.DES_ID = TOF_TYPES.TYP_KV_BRAKE_SYST_DES_ID AND TOF_DESIGNATIONS6.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* тормозная система */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS6 ON TOF_DES_TEXTS6.TEX_ID = TOF_DESIGNATIONS6.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS7 ON TOF_DESIGNATIONS7.DES_ID = TOF_TYPES.TYP_KV_FUEL_DES_ID AND TOF_DESIGNATIONS7.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* вид топлива */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS7 ON TOF_DES_TEXTS7.TEX_ID = TOF_DESIGNATIONS7.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS8 ON TOF_DESIGNATIONS8.DES_ID = TOF_TYPES.TYP_KV_FUEL_SUPPLY_DES_ID AND TOF_DESIGNATIONS8.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* впрыск */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS8 ON TOF_DES_TEXTS8.TEX_ID = TOF_DESIGNATIONS8.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS9 ON TOF_DESIGNATIONS9.DES_ID = TOF_TYPES.TYP_KV_CATALYST_DES_ID AND TOF_DESIGNATIONS9.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* вид катализатора */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS9 ON TOF_DES_TEXTS9.TEX_ID = TOF_DESIGNATIONS9.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS1 ON TOF_DESIGNATIONS1.DES_ID = TOF_TYPES.TYP_KV_TRANS_DES_ID AND TOF_DESIGNATIONS1.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* трансмиссия */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS1 ON TOF_DES_TEXTS1.TEX_ID = TOF_DESIGNATIONS1.DES_TEX_ID " +

//            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS_A ON TOF_DESIGNATIONS_A.DES_ID = TOF_TYPES.TYP_KV_ENGINE_DES_ID AND TOF_DESIGNATIONS_A.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* мотор */
                //            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS_A ON TOF_DES_TEXTS_A.TEX_ID = TOF_DESIGNATIONS_A.DES_TEX_ID " +

            "LEFT JOIN TOF_LINK_TYP_ENG ON LTE_TYP_ID = TYP_ID " +
            "LEFT JOIN TOF_ENGINES ON ENG_ID = LTE_ENG_ID " +


            " WHERE " +
            (modelTypeId.HasValue ? (" TOF_TYPES.TYP_ID = " + modelTypeId.Value.ToString())
                                  : (" TOF_TYPES.TYP_MOD_ID = " + MOD_ID.ToString()) +
                                    (fluelId.HasValue ? (" AND TOF_TYPES.TYP_KV_FUEL_DES_ID = " + fluelId.Value.ToString()) : (""))
                                  ) +

            " AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() +
// !!!            " AND TOF_TYPES.TYP_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1 " +
            " ORDER BY TOF_TYPES.TYP_SORT";
        }
        protected string GetSIMPLEMODELTYPES_TEXT(int LNG_ID, int COU_ID, int modelTypeId)
        {
            return
            "SELECT " + 
            "TOF_TYPES.TYP_ID, "                                        +   /* 0  ключ */
            "TOF_DES_TEXTS.TEX_TEXT AS TEX_TEXT, "                      +   /* 1  Наименование модели полное если поле TYP_MMT_CDS_ID и краткое если поле TYP_CDS_ID */
            "TOF_TYPES.TYP_KV_FUEL_DES_ID, "                            +   /* 2  топливо */
            "TOF_TYPES.TYP_MOD_ID "                                     +   /* 3  модель  */
            "FROM TOF_TYPES " +

            "INNER JOIN TOF_COUNTRY_DESIGNATIONS ON TOF_COUNTRY_DESIGNATIONS.CDS_ID = TOF_TYPES.TYP_MMT_CDS_ID " +
            "INNER JOIN TOF_DES_TEXTS ON TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID = TOF_DES_TEXTS.TEX_ID " +

            " WHERE " +

            " TOF_TYPES.TYP_ID = " + modelTypeId.ToString() +
            " AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + 
            " AND TOF_TYPES.TYP_CTM subrange(" + COU_ID.ToString() +" cast integer) = 1 " + 
            " ORDER BY TOF_TYPES.TYP_SORT";
        }
        protected string GetFUELS_TEXT(int LNG_ID)
        {
            return 
            "select " +
            "DES_ID, " + 
            "TEX_TEXT  " + 
            "from TOF_DESIGNATIONS " + 
            "INNER JOIN TOF_DES_TEXTS ON TOF_DES_TEXTS.TEX_ID = TOF_DESIGNATIONS.DES_TEX_ID " +
            "where (TOF_DESIGNATIONS.DES_LNG_ID = " + LNG_ID.ToString() + " and " + 
            "TOF_DESIGNATIONS.DES_ID in (select distinct TYP_KV_FUEL_DES_ID from TOF_TYPES where TYP_KV_FUEL_DES_ID is not null)) "; 
        }
        protected string GetMODELTYPESTREE_TEXT(int LNG_ID, int TYP_ID)
        {
            return
            "select str_id, " +
            "str_level, " +
            "str_sort, " +
            "tex_text, " +
            "str_id_parent  " +

            "from tof_search_tree  " +
            "join tof_designations  " +
            "on str_des_id nljoin des_id and  " +
            "des_lng_id = " + LNG_ID.ToString() + " " +
            "join tof_des_texts  " +
            "on des_tex_id nljoin tex_id  " +

            "where 1 < 3 and  " +
            "str_type = 1 and  " +
            "str_level > 1 and  " +
            "(select nvl(max(lgs_ga_id), 0)  " +
            "from tof_link_ga_str  " +
            "join tof_link_la_typ  " +
            "on lgs_ga_id nljoin lat_ga_id and  " +
            "lat_typ_id = " + TYP_ID.ToString() + " " +
            "where lgs_str_id = str_id and " +
            "(-1 < 0 or lgs_ga_id in (-1))) > 0  "+
            "order by str_level, str_sort";
        }
        protected string GetAllMODELTYPESTREE_TEXT(int LNG_ID) {
            return
            "select str_id, " +
            "tex_text, " +
            "str_id_parent  " +
            "from tof_search_tree  " +
            "join tof_designations  " +
            "on str_des_id nljoin des_id and  " +
            "des_lng_id = " + LNG_ID.ToString() + " " +
            "join tof_des_texts  " +
            "on des_tex_id nljoin tex_id  " ;
        }

        protected string GetMODELTYPETREEITEMS_TEXT(int LNG_ID, int COU_ID, int TYP_ID, int STR_ID, int tof_assemblyId, int tof_suppliersId)
        {
            String ret = 

                " select distinct lat_sup_id sup_id, " +
                " nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier, " +
                " ga_nr, " +
                " tof_des_texts.TEX_TEXT masterbez, " +
                " ART_ARTICLE_NR, " +
                " ART_ID, " +
                " LA_ID, " +
                " ga_assembly_tex.TEX_TEXT GA_TEXT" +

//                " 'column' col, " +
//                " nvl(trf_equal.trf_abc, trf_null.trf_abc) trf_abc, " +
//                " nvl(trf_equal.trf_sort, trf_null.trf_sort) trf_sort," +
//                " 1 firstgr, " +
//                " 1 lastgr, " +
//                " 0 special_filter " +
                " from tof_link_la_typ " +
                " join tof_generic_articles " +
                " on ga_id = lat_ga_id " +
                " join tof_designations " +
                " on des_id = ga_des_id and " +
                " des_lng_id = " + LNG_ID.ToString() +
                " join tof_des_texts " +
                " on tex_id = des_tex_id " +
                " left outer join tof_suppliers sup_cou " +
                " on sup_cou.sup_id = lat_sup_id and " +
                " sup_cou.sup_cou_id = " + COU_ID.ToString() +
                " left outer join tof_suppliers sup_null " +
                " on sup_null.sup_id = lat_sup_id and " +
                " sup_null.sup_cou_id is null " +
                " left outer join tof_retail_filters trf_equal " +
                " on trf_equal.trf_ga_id = ga_id and " +
                " trf_equal.trf_sup_id = lat_sup_id and " +
                " trf_equal.trf_tsd_id IS NULL " +
                " left outer join tof_retail_filters trf_null " +
                " on trf_null.trf_ga_id is null and " +
                " trf_null.trf_sup_id = lat_sup_id and " +
                " trf_null.trf_tsd_id IS NULL " +

                " join tof_link_art " +
                " on lat_la_id nljoin la_id " +
                " join tof_articles " +
                " on la_art_id nljoin art_id and " +
                " art_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 " +


                " left outer join tof_designations ga_assembly_des" +
                " on ga_des_id_assembly nljoin ga_assembly_des.des_id and " +
                LNG_ID.ToString() + " nljoin ga_assembly_des.des_lng_id " +
                " left outer join tof_des_texts ga_assembly_tex " +
                " on ga_assembly_des.des_tex_id nljoin ga_assembly_tex.tex_id " +



                " where lat_typ_id = " + TYP_ID.ToString() +
                " and lat_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 " +
//                " and 1 < 3 and " +
//                " 1 = 1 and " +
                " and (-1 < 0 or lat_ga_id in (-1)) and " +
                " lat_ga_id in ( select lgs_ga_id " +
                " from tof_link_ga_str " +
                " where lgs_str_id = " + STR_ID.ToString() + ") " + 
                ( (tof_assemblyId != 0) ? (" AND ga_nr = " + tof_assemblyId.ToString() ) : ("")  ) +
                ( (tof_suppliersId != 0) ? (" AND lat_sup_id = " + tof_suppliersId.ToString() ) : ("")  ) 
                
                ; // UNION ";

            if (ret != "") return ret;

            // subquery below do not return resultset since criteriar like " and 1 = 2 and "  

            ret =  ret +
                " select distinct lam_sup_id sup_id, " +
                " nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier, " +
                " ga_nr, " +
                " tex_text masterbez, " +
                " ART_ARTICLE_NR, " +
                " ART_ID , " +
                " LA_ID, " +
                " TEX_TEXT " +
//                " 'column' col, " +
//                " nvl(trf_equal.trf_abc, trf_null.trf_abc) trf_abc, " +
//                " nvl(trf_equal.trf_sort, trf_null.trf_sort) trf_sort, " +
//                " 1 firstgr, " +
//                " 1 lastgr, " +
//                " 0 special_filter " +
                " from tof_link_la_mrk " +
                " join tof_generic_articles " +
                " on ga_id = lam_ga_id " +
                " join tof_designations " +
                " on des_id = ga_des_id and " +
                " des_lng_id = " + LNG_ID.ToString() +
                " join tof_des_texts " +
                " on tex_id = des_tex_id " +
                " left outer join tof_suppliers sup_cou " +
                " on sup_cou.sup_id = lam_sup_id and " +
                " sup_cou.sup_cou_id = " + COU_ID.ToString() +
                " left outer join tof_suppliers sup_null " +
                " on sup_null.sup_id = lam_sup_id and " +
                " sup_null.sup_cou_id is null " +
                " left outer join tof_retail_filters trf_equal " +
                " on trf_equal.trf_ga_id = ga_id and " +
                " trf_equal.trf_sup_id = lam_sup_id and " +
                " trf_equal.trf_tsd_id IS NULL " +
                " left outer join tof_retail_filters trf_null " +
                " on trf_null.trf_ga_id is null and " +
                " trf_null.trf_sup_id = lam_sup_id and " +
                " trf_null.trf_tsd_id IS NULL " +

                " join tof_link_art " +
                " on lam_la_id nljoin la_id " +
                " join tof_articles " +
                " on la_art_id nljoin art_id and " +
                " art_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 " +


                " where lam_mrk_id = " + TYP_ID.ToString() +
                " and lam_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 " +
                " and 1 = 2 and " +
                " 1 = 6 and " +
                " (-1 < 0 or lam_ga_id in (-1)) and " +
                " lam_ga_id in ( select lgs_ga_id " +
                " from tof_link_ga_str " +
                " where lgs_str_id = " + STR_ID.ToString() + ") UNION ";

            

            ret = ret +
                " select distinct lae_sup_id sup_id, " +
                " nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier, " +
                " ga_nr, " +
                " tex_text masterbez, " +
                " ART_ARTICLE_NR, " +
                " ART_ID , " +
                " LA_ID, " +
                " TEX_TEXT " +

//                " 'column' col, " +
//                " nvl(trf_equal.trf_abc, trf_null.trf_abc) trf_abc, " +
//                " nvl(trf_equal.trf_sort, trf_null.trf_sort) trf_sort, " +
//                " 1 firstgr, " +
//                " 1 lastgr, " +
//                " 0 special_filter " +
                " from tof_link_la_eng " +
                " join tof_generic_articles " +
                " on ga_id = lae_ga_id " +
                " join tof_designations " +
                " on des_id = ga_des_id and " +
                " des_lng_id = " + LNG_ID.ToString() +
                " join tof_des_texts  " +
                " on tex_id = des_tex_id " +
                " left outer join tof_suppliers sup_cou " +
                " on sup_cou.sup_id = lae_sup_id and " +
                " sup_cou.sup_cou_id = " + COU_ID.ToString() +
                " left outer join tof_suppliers sup_null " +
                " on sup_null.sup_id = lae_sup_id and " +
                " sup_null.sup_cou_id is null " +
                " left outer join tof_retail_filters trf_equal " +
                " on trf_equal.trf_ga_id = ga_id and " +
                " trf_equal.trf_sup_id = lae_sup_id and " +
                " trf_equal.trf_tsd_id IS NULL " +
                " left outer join tof_retail_filters trf_null " +
                " on trf_null.trf_ga_id is null and " +
                " trf_null.trf_sup_id = lae_sup_id and " +
                " trf_null.trf_tsd_id IS NULL " +

                " join tof_link_art " +
                " on lae_la_id nljoin la_id " +
                " join tof_articles " +
                " on la_art_id nljoin art_id and " +
                " art_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 " +

                " where lae_eng_id = " + TYP_ID.ToString() +
                " and lae_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 " +
                " and 1 = 3 and " +
                " (-1 < 0 or lae_ga_id in (-1)) and " +
                " lae_ga_id in ( select lgs_ga_id " +
                " from tof_link_ga_str " +
                " where lgs_str_id = " + STR_ID.ToString() + ") UNION ";
            
            ret = ret +
                " select distinct laa_sup_id sup_id,  " +
                " nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier,  " +
                " ga_nr,  " +
                " tex_text masterbez,  " +
                " ART_ARTICLE_NR, " +
                " ART_ID , " +
                " LA_ID, " +
                " TEX_TEXT " +

//                " 'column' col,  " +
//                " nvl(trf_equal.trf_abc, trf_null.trf_abc) trf_abc,  " +
//                " nvl(trf_equal.trf_sort, trf_null.trf_sort) trf_sort,  " +
//                " 1 firstgr,  " +
//                " 1 lastgr,  " +
//                " 0 special_filter  " +
                " from tof_link_la_axl  " +
                " join tof_generic_articles  " +
                " on ga_id = laa_ga_id  " +
                " join tof_designations  " +
                " on des_id = ga_des_id and  " +
                " des_lng_id = " + LNG_ID.ToString() +
                " join tof_des_texts " +
                " on tex_id = des_tex_id " +
                " left outer join tof_suppliers sup_cou " +
                " on sup_cou.sup_id = laa_sup_id and " +
                " sup_cou.sup_cou_id = " + COU_ID.ToString() +
                " left outer join tof_suppliers sup_null " +
                " on sup_null.sup_id = laa_sup_id and " +
                " sup_null.sup_cou_id is null " +
                " left outer join tof_retail_filters trf_equal " +
                " on trf_equal.trf_ga_id = ga_id and " +
                " trf_equal.trf_sup_id = laa_sup_id and " +
                " trf_equal.trf_tsd_id IS NULL " +
                " left outer join tof_retail_filters trf_null " +
                " on trf_null.trf_ga_id is null and " +
                " trf_null.trf_sup_id = laa_sup_id and " +
                " trf_null.trf_tsd_id IS NULL " +

                " join tof_link_art " +
                " on laa_la_id nljoin la_id " +
                " join tof_articles " +
                " on la_art_id nljoin art_id and " +
                " art_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 " +

                " where laa_axl_id = " + TYP_ID.ToString() +
                " and laa_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 " +
                " and 1 = 5 and " +
                " (-1 < 0 or laa_ga_id in (-1)) and " +
                " laa_ga_id in ( select lgs_ga_id " +
                " from tof_link_ga_str " +
                " where lgs_str_id = " + STR_ID.ToString() + ") "; // UNION ";
            if (ret != null) return ret;
            ret =  ret +
                " select distinct art_sup_id sup_id, " +
                " nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier, " +
                " ga_nr, " +
                " tex_text masterbez, " +
                " ART_ARTICLE_NR, " +
                " ART_ID  , " +
                " LA_ID, " +   // <----------------------- no such a fild in this subquery
                " TEX_TEXT " +

//                " 'column' col, " +
//                " nvl(trf_equal.trf_abc, trf_null.trf_abc) trf_abc, " +
//                " nvl(trf_equal.trf_sort, trf_null.trf_sort) trf_sort, " +
//                " 1 firstgr, " +
//                " 1 lastgr, " +
//                " 0 special_filter " +
                " from tof_generic_articles " +
                " join tof_designations " +
                " on des_id = ga_des_id and " +
                " des_lng_id = " + LNG_ID.ToString() +
                " join tof_des_texts " +
                " on tex_id = des_tex_id " +
                " join tof_link_art_ga " +
                " on lag_ga_id = ga_id " +
                " join tof_articles " +
                " on art_id = lag_art_id and " +
                " art_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 " +
                " left outer join tof_suppliers sup_cou " +
                " on sup_cou.sup_id = art_sup_id and " +
                " sup_cou.sup_cou_id = " + COU_ID.ToString() +
                " left outer join tof_suppliers sup_null " +
                " on sup_null.sup_id = art_sup_id and " +
                " sup_null.sup_cou_id is null " +
                " left outer join tof_retail_filters trf_equal " +
                " on trf_equal.trf_ga_id = ga_id and " +
                " trf_equal.trf_sup_id = art_sup_id and " +
                " trf_equal.trf_tsd_id IS NULL " +
                " left outer join tof_retail_filters trf_null " +
                " on trf_null.trf_ga_id is null and " +
                " trf_null.trf_sup_id = art_sup_id and " +
                " trf_null.trf_tsd_id IS NULL " +
                " where 1 = 4 and " +
                " ga_universal = 1 and " +
                " (-1 < 0 or ga_id in (-1)) and " +
                " ga_id in ( select lgs_ga_id " +
                " from tof_link_ga_str " +
                " where lgs_str_id = " + STR_ID.ToString() + ") ";

            return ret;

        }
        protected string GetALLMODELTYPETREEITEMS_TEXT(int LNG_ID) 
        {
            return
            "select " +
            "lgs_str_id, " +
            "tex_text, " +
            "lgs_ga_id " +
            " from tof_link_ga_str " +
            " join tof_generic_articles " +
            " on ga_id = lgs_ga_id " +
            " join tof_designations " +
            " on des_id = ga_des_id and " +
            " des_lng_id = " + LNG_ID.ToString() +
            " join tof_des_texts " +
            " on tex_id = des_tex_id ";
        }

        
        protected string GetMODELTYPETREEITEMDESCR_TEXT(int LNG_ID, int COU_ID, int ART_ID) // , int LA_ID)
        {
            return
/*
                " select "+
                " cri_tex.tex_text crit_designation, "+
                " nvl(lac_value, value_tex.tex_text) crit_value, "+
                " nvl(' '||unit_tex.tex_text, '') crit_unit, "+

                " from tof_la_criteria "+
                " join tof_link_art "+
                " on la_id = lac_la_id "+
                " join tof_criteria "+
                " on cri_id = lac_cri_id "+
                " left outer join tof_designations cri_des "+
                " on cri_des.des_id = cri_short_des_id and "+
                " cri_des.des_lng_id = " + LNG_ID.ToString() + 
                " left outer join tof_des_texts cri_tex "+
                " on cri_tex.tex_id = cri_des.des_tex_id "+
                " left outer join tof_designations value_des "+
                " on value_des.des_id = lac_kv_des_id and "+
                " value_des.des_lng_id = " + LNG_ID.ToString() + 
                " left outer join tof_des_texts value_tex "+
                " on value_tex.tex_id = value_des.des_tex_id "+
                " left outer join tof_designations unit_des "+
                " on unit_des.des_id = cri_unit_des_id and "+
                " unit_des.des_lng_id = " + LNG_ID.ToString() + 
                " left outer join tof_des_texts unit_tex "+
                " on unit_tex.tex_id = unit_des.des_tex_id "+
                " where lac_la_id in (" + LA_ID.ToString() + ") and " + 
                " lac_display = 1 and "+
                " lac_ctm subrange("+ COU_ID.ToString() +" cast integer) = 1 "+

                " UNION ALL "+
 */ 
                " select "+
                " cri_tex.tex_text crit_designation, "+
                " nvl(acr_value, value_tex.tex_text) crit_value, "+
                " nvl(' '||unit_tex.tex_text, '') crit_unit "+

                " from tof_article_criteria "+
                " join tof_criteria "+
                " on acr_cri_id = cri_id "+
                " left outer join tof_designations cri_des "+
                " on cri_des.des_id = cri_short_des_id and "+
                " cri_des.des_lng_id = " + LNG_ID.ToString() + 
                " left outer join tof_des_texts cri_tex "+
                " on cri_tex.tex_id = cri_des.des_tex_id "+
                " left outer join tof_designations value_des "+
                " on value_des.des_id = acr_kv_des_id and "+
                " value_des.des_lng_id = " + LNG_ID.ToString() +  
                " left outer join tof_des_texts value_tex "+
                " on value_tex.tex_id = value_des.des_tex_id "+
                " left outer join tof_designations unit_des "+
                " on unit_des.des_id = cri_unit_des_id and "+
                " unit_des.des_lng_id = " + LNG_ID.ToString() +   
                " left outer join tof_des_texts unit_tex "+
                " on unit_tex.tex_id = unit_des.des_tex_id "+
                " where acr_art_id in (" + ART_ID.ToString() + ") and " +
                " acr_ctm subrange("+ COU_ID.ToString() +" cast integer) = 1 and "+
                " acr_display = 1 ";

        }
        protected string GetMODELTYPETREEITEMMANID_TEXT(int LNG_ID, int COU_ID, int ART_ID, int ? MFA_ID = null) // , int LA_ID)
        {
            return
                " SELECT " +
                " TOF_ART_LOOKUP.ARL_KIND, " +
                " TOF_BRANDS.BRA_BRAND, " +
                " TOF_ART_LOOKUP.ARL_DISPLAY_NR " +
                " FROM TOF_ART_LOOKUP " +
                " INNER JOIN TOF_BRANDS ON TOF_BRANDS.BRA_ID = TOF_ART_LOOKUP.ARL_BRA_ID " +
                " WHERE TOF_ART_LOOKUP.ARL_ART_ID = " + ART_ID.ToString() +
                " AND TOF_ART_LOOKUP.ARL_KIND = '3' " +
                (MFA_ID.HasValue ? (" AND TOF_BRANDS.BRA_ID = " + MFA_ID.Value.ToString()) : "") +


                " ORDER BY BRA_BRAND, ARL_DISPLAY_NR ";
        }
        protected string GetANALOGS_TEXT(int LNG_ID, int COU_ID, string ART_ARTICLE_NR, int? GA_NR)
        {
            return
                " select distinct " +
                " art_id, " +
                " tof_articles.ART_ARTICLE_NR, " +
                " ga_id, " +
                " ga_tex.tex_text MASTER_BEZ, " +
                " ga_assembly_tex.tex_text ga_assembly, " +
                " SUP.SUP_BRAND " +
                
                " from tof_art_lookup " +
                " join tof_articles " +
                " on arl_art_id nljoin art_id and " +
                " 1 nljoin art_ctm subrange(" + COU_ID.ToString() + " cast integer) " +
                " JOIN TOF_SUPPLIERS SUP ON (SUP.SUP_ID = tof_articles.ART_SUP_ID) " +
                " JOIN TOF_DESIGNATIONS DES ON (DES.DES_ID = tof_articles.ART_COMPLETE_DES_ID) " +
                " JOIN TOF_DES_TEXTS TEX ON (DES.DES_TEX_ID = TEX.TEX_ID) " +
                " join tof_link_art_ga " +
                " on lag_art_id = art_id " +
                " join tof_generic_articles " +
                " on ga_id = lag_ga_id and " +
                " ((ga_universal = 0 and  ga_id = ga_nr) or ga_universal = 1) " +
                " join tof_designations ga_des " +
                " on ga_des_id nljoin ga_des.des_id and " +
                " ga_des.des_lng_id = " + LNG_ID.ToString() +
                " join tof_des_texts ga_tex" +
                " on ga_des.des_tex_id nljoin ga_tex.tex_id" +
                " left outer join tof_designations ga_assembly_des" +
                " on ga_des_id_assembly nljoin ga_assembly_des.des_id and " +
                LNG_ID.ToString() + " nljoin ga_assembly_des.des_lng_id " +
                " left outer join tof_des_texts ga_assembly_tex " +
                " on ga_assembly_des.des_tex_id nljoin ga_assembly_tex.tex_id " +
                " where arl_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 and " +
                " arl_kind in ('1','2','3','4','5') and  " +
                " ( (0 = 1 and arl_search_number like '" + ART_ARTICLE_NR + "' ) or " +
                " ( 0 = 0 and arl_search_number = '" + ART_ARTICLE_NR + "' ) ) and " +
                (GA_NR.HasValue ? (" ga_id = " + GA_NR.Value.ToString()) : " (-1 = -1  or  ga_id = -1 ) ") +


                " UNION ALL " +
                " select distinct  " +
                " art_id, " +
                " tof_articles.ART_ARTICLE_NR, " +
                " ga_id,  " +
                " ga_tex.tex_text MASTER_BEZ,  " +
                " ga_assembly_tex.tex_text ga_assembly, " +
                " SUP.SUP_BRAND " +
                " from tof_tecsel_dealers  " +
                " join tof_tecsel_prices  " +
                " on tsd_id nljoin tsp_tsd_id and  " +
                " tsp_ctm subrange(" + COU_ID.ToString() + " cast integer) = 1 and  " +
                " ( (0 = 1 and tsp_search_number like '" + ART_ARTICLE_NR + "' ) or " +
                " ( 0 = 0 and  tsp_search_number = '" + ART_ARTICLE_NR + "' ) )  " +
                " join tof_articles " +
                " on tsp_art_id nljoin art_id and " +
                " 1 nljoin art_ctm subrange(" + COU_ID.ToString() + " cast integer) " +
                " JOIN TOF_SUPPLIERS SUP ON (SUP.SUP_ID = tof_articles.ART_SUP_ID) " +
                " JOIN TOF_DESIGNATIONS DES ON (DES.DES_ID = tof_articles.ART_COMPLETE_DES_ID) " +
                " JOIN TOF_DES_TEXTS TEX ON (DES.DES_TEX_ID = TEX.TEX_ID) " +
                " join tof_link_art_ga " +
                " on lag_art_id = art_id " +
                " join tof_generic_articles " +
                " on ga_id = lag_ga_id and " +
                " ((ga_universal = 0 and  ga_id = ga_nr) or ga_universal = 1) " +
                " join tof_designations ga_des " +
                " on ga_des_id nljoin ga_des.des_id and " +
                " ga_des.des_lng_id = " + LNG_ID.ToString() +
                " join tof_des_texts ga_tex " +
                " on ga_des.des_tex_id nljoin ga_tex.tex_id " +
                " left outer join tof_designations ga_assembly_des " +
                " on ga_des_id_assembly nljoin ga_assembly_des.des_id and " +
                LNG_ID.ToString() + " nljoin ga_assembly_des.des_lng_id " +
                " left outer join tof_des_texts ga_assembly_tex " +
                " on ga_assembly_des.des_tex_id nljoin ga_assembly_tex.tex_id " +
                " where tsd_id = -1 and " +
                (GA_NR.HasValue ? (" ga_id = " + GA_NR.Value.ToString()) : " (-1 = -1  or  ga_id = -1 )") +
                " order by ga_id  ";
       }
        protected string GetArticleByID_TEXT(int LNG_ID, int COU_ID, string ART_ARTICLE_NR, int searchType) 
        {
            if (searchType < 2)
                return
                    " select " +
                    " ART_ID, " +
                    " ART_ARTICLE_NR, " +
                    " ART_SUP_ID, " +
                    " nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier, " +
                    " art_tex.tex_text ARTICLE_DESCR, " +
                    " lag_ga_id " +
                    " from tof_articles " +

                    " join tof_link_art_ga " +
                    " on lag_art_id = art_id " +
//                " join tof_generic_articles " +
//                " on ga_id = lag_ga_id and " +
//                " ((ga_universal = 0 and  ga_id = ga_nr) or ga_universal = 1) " +



                    " left outer join tof_designations art_des " +
                    " on ART_COMPLETE_DES_ID nljoin art_des.des_id  and " +
                    LNG_ID.ToString() + " nljoin art_des.des_lng_id " +
                    " left outer join tof_des_texts art_tex " +
                    " on art_des.des_tex_id nljoin art_tex.tex_id " +
                    " left outer join tof_suppliers sup_cou " +
                    " on sup_cou.sup_id = ART_SUP_ID and " +
                    " sup_cou.sup_cou_id = " + LNG_ID.ToString() +
                    " left outer join tof_suppliers sup_null " +
                    " on sup_null.sup_id = ART_SUP_ID and " +
                    " sup_null.sup_cou_id is null " +
                    " where " +
                    " ART_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1   AND " +
                    (searchType == 0 ? " ART_ARTICLE_NR = '" + ART_ARTICLE_NR + "' " : "ART_ID=" + ART_ARTICLE_NR);
            else
                return
                   " select " +
                   " ART_ID, " +
                   " ART_ARTICLE_NR, " +
                   " ART_SUP_ID, " +
                   " nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier, " +
                   " art_tex.tex_text ARTICLE_DESCR, " +
                   " lag_ga_id " +
                   " from tof_art_lookup " +
                   " inner join tof_articles on art_id = ARL_ART_ID " +


                    " join tof_link_art_ga " +
                    " on lag_art_id = art_id " +
//                " join tof_generic_articles " +
//                " on ga_id = lag_ga_id and " +
//                " ((ga_universal = 0 and  ga_id = ga_nr) or ga_universal = 1) " +


                   " left outer join tof_designations art_des " +
                   " on ART_COMPLETE_DES_ID nljoin art_des.des_id  and " +
                   LNG_ID.ToString() + " nljoin art_des.des_lng_id " +
                   " left outer join tof_des_texts art_tex " +
                   " on art_des.des_tex_id nljoin art_tex.tex_id " +
                   " left outer join tof_suppliers sup_cou " +
                   " on sup_cou.sup_id = ART_SUP_ID and " +
                   " sup_cou.sup_cou_id = " + LNG_ID.ToString() +
                   " left outer join tof_suppliers sup_null " +
                   " on sup_null.sup_id = ART_SUP_ID and " +
                   " sup_null.sup_cou_id is null " +


                   " where " +
                   (searchType != 2 ? " arl_kind in ('5') and " : " arl_kind in ('3') and arl_search_number=arl_display_nr and " ) +
                   " arl_search_number='" + ART_ARTICLE_NR + "'";
        }

        protected string GetAllArticle_TEXT(int LNG_ID, int COU_ID, int TecDocFrom, int TecDocTil)
        {
                string aResult =
                    " select " +
                    " ART_ID, " +
                    " ART_ARTICLE_NR, " +
                    " ART_SUP_ID, " +
                    " nvl(sup_cou.sup_brand, sup_null.sup_brand) supplier, " +
                    " art_tex.tex_text ARTICLE_DESCR, " +
                    " ARL_DISPLAY_NR " +
                    //" lag_ga_id " +
                    " from tof_articles " +

                    //" join tof_link_art_ga " +
                    //" on lag_art_id = art_id " +

                    " left outer join tof_designations art_des " +
                    " on ART_COMPLETE_DES_ID nljoin art_des.des_id  and " +
                    LNG_ID.ToString() + " nljoin art_des.des_lng_id " +
                    " left outer join tof_des_texts art_tex " +
                    " on art_des.des_tex_id nljoin art_tex.tex_id " +
                    " left outer join tof_suppliers sup_cou " +
                    " on sup_cou.sup_id = ART_SUP_ID and " +
                    " sup_cou.sup_cou_id = " + LNG_ID.ToString() +
                    " left outer join tof_suppliers sup_null " +
                    " on sup_null.sup_id = ART_SUP_ID and " +
                    " sup_null.sup_cou_id is null " +
                    " left outer join TOF_ART_LOOKUP TFARTLKP " +
                    " on TFARTLKP.ARL_ART_ID = ART_ID and TFARTLKP.arl_kind in ('5') " +
                    " where " +
                    " ART_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1  " +
                    " AND (ART_ID >= " + TecDocFrom.ToString() +")";
                if (TecDocTil > 0)
                {
                    aResult = aResult + " AND (ART_ID <= " + TecDocTil.ToString() + ")";
                }
                return aResult;
        }
        protected string GetAllArticleMax_TEXT()
        {
            return
                " select " +
                " max(ART_ID) " +
                " from tof_articles ";
        }

        protected string GetArticleGroup_text(int ArticleCategoryFrom, int ArticleCategoryTil)
        {
            string aResult =
                " select  " +
                " art_id, " +
                " ga_nr " +
                " from " +
                " tof_articles " +
                " join tof_link_art_ga " +
                " on lag_art_id = art_id " +
                " join tof_generic_articles " +
                " on ga_id = lag_ga_id and " +
                " ((ga_universal = 0 and  ga_id = ga_nr) or ga_universal = 1) " +
                " where (art_id >= " + ArticleCategoryFrom.ToString() + ") ";
            if (ArticleCategoryTil > 0) {
                aResult = aResult + " and (art_id <= " + ArticleCategoryTil.ToString() + ")";
            }

            return aResult;
        }
        protected string GetArticleGroupMax_TEXT()
        {
            return
            " select  " +
            " max(art_id) " +
            " from " +
            " tof_articles ";
        }

        protected string GetArticleBrand_TEXT()
        {
            return
                " SELECT " +
                " BRA_ID, " +
                " BRA_BRAND " +
                " FROM TOF_BRANDS ";
        }
        protected string GetArticleBrandMax_TEXT()
        {
            return
                "select " +
                "max(ARL_ART_ID)" +
                "from TOF_ART_LOOKUP";
        }

        protected string GetArticleLookUp_TEXT(int articlelookupfrom, int articlelookuptil)
        {
            string aResult =
                "select " +
                "ARL_ART_ID," +
                "ARL_SEARCH_NUMBER," +
                "ARL_KIND," +
                "ARL_BRA_ID," +
                "ARL_DISPLAY_NR " +
                "from TOF_ART_LOOKUP where (ARL_ART_ID >= " + articlelookupfrom.ToString() + ")";
            if (articlelookuptil >= 0) {
                aResult = aResult + " and (ARL_ART_ID <= "+articlelookuptil.ToString()+")";
            }
            return aResult;
        }
        protected string GetArticleLookUpMax_TEXT()
        {
            return
                "select " +
                "max(ARL_ART_ID)" +
                "from TOF_ART_LOOKUP";
        }


        protected string GetArticleApplic_TEXT(int LNG_ID, int COU_ID, int ART_ID)
        {
            return 
            "SELECT " +
            "TOF_TYPES.TYP_ID, " +   /* 0  ключ */
            "TOF_DES_TEXTS.TEX_TEXT AS TEX_TEXT, " +   /* 1  Наименование модели полное если поле TYP_MMT_CDS_ID и краткое если поле TYP_CDS_ID */

            "TOF_DES_TEXTS2.TEX_TEXT AS TYP_KV_BODY_DESCR, " +   /* 2  кузов */

            "TOF_TYPES.TYP_PCON_START," + /* 3  первые 4 цифры: год начала выпуска модели, последние две месяц начала выпуска модели. */
            "TOF_TYPES.TYP_PCON_END," +   /* 4  первые 4 цифры: год окончания выпуска модели, последние две месяц окончания выпуска модели. */

            "TOF_TYPES.TYP_KW_FROM," +   /* 5  Техническая информация/Мощность двигателя(кВ) (от) */
            "TOF_TYPES.TYP_KW_UPTO," +   /* 6  Техническая информация/Мощность двигателя(кВ) (до) */
            "TOF_TYPES.TYP_HP_FROM," +   /* 7  Техническая информация/Мощность двигателя (ЛС) (от) */
            "TOF_TYPES.TYP_HP_UPTO," +   /* 8  Техническая информация/Мощность двигателя (ЛС) (до) */

            "TOF_TYPES.TYP_CCM," +   /* 9  Техническая информация/Тех. Объем куб. см. */
            "TOF_TYPES.TYP_VALVES," +   /* 10 Техническая информация/Количество клапанов на одну камеру сгорания */
            "TOF_TYPES.TYP_CYLINDERS," +   /* 11 Техническая информация/Цилиндр (Количество цилиндров) */
            "TOF_TYPES.TYP_DOORS," +   /* 12 Конструкция/Количество дверей. */

            "TOF_DES_TEXTS3.TEX_TEXT AS TYP_KV_ABS_DESCR, " +   /* 13 ABS */
            "TOF_DES_TEXTS4.TEX_TEXT AS TYP_KV_ASR_DESCR, " +   /* 14 ASR */
            "TOF_DES_TEXTS5.TEX_TEXT AS TYP_KV_BRAKE_TYPE_DESCR, " +   /* 15 TYP_KV_BRAKE_TYPE Вид тормозов */
            "TOF_DES_TEXTS6.TEX_TEXT AS TYP_KV_BRAKE_SYST_DESCR, " +   /* 16 TYP_KV_BRAKE_SYST Система тормозов */

            "TOF_DES_TEXTS7.TEX_TEXT AS TYP_KV_FUEL_DES_ID_DESCR, " +   /* 17 TYP_KV_FUEL Вид горючего */
            "TOF_DES_TEXTS8.TEX_TEXT AS TYP_KV_FUEL_SUPPLY_DES_ID_DESCR, " +   /* 18 TYP_KV_FUEL_SUPPLY впрыск */

            "TOF_DES_TEXTS9.TEX_TEXT AS TYP_KV_CATALYST_DES_ID_DESCR, " +   /* 19 TYP_KV_CATALYST катализатор */

            "TOF_DES_TEXTS1.TEX_TEXT AS TYP_KV_TRANS_DES_ID_DESCR,  " +   /* 20 TYP_KV_TRANS трансмиссия */

//            "TOF_DES_TEXTS_A.TEX_TEXT AS TYP_KV_ENGINE_DES_ID_DESCR  " +   /* 21 TYP_KV_ENGINE мотор */ 
            "TOF_ENGINES.ENG_CODE AS ENG_CODE " + /* 21 TYP_KV_ENGINE мотор */

//            "TOF_TYPES.TYP_MOD_ID " + /* модель  */

            "FROM TOF_LINK_ART " +
            "INNER JOIN TOF_LINK_LA_TYP ON LAT_LA_ID = LA_ID " +
            "INNER JOIN TOF_TYPES ON TYP_ID = LAT_TYP_ID " +
	


            "INNER JOIN TOF_COUNTRY_DESIGNATIONS ON TOF_COUNTRY_DESIGNATIONS.CDS_ID = TOF_TYPES.TYP_MMT_CDS_ID AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() + " " + // 
            "INNER JOIN TOF_DES_TEXTS ON TOF_DES_TEXTS.TEX_ID = TOF_COUNTRY_DESIGNATIONS.CDS_TEX_ID " +
                //            "INNER JOIN TOF_TYPE_NUMBERS TYPES ON TOF_TYPE_NUMBERS.TYN_TYP_ID TOF_TYPES.TYP_ID " +


            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS2 ON TOF_DESIGNATIONS2.DES_ID = TOF_TYPES.TYP_KV_BODY_DES_ID AND TOF_DESIGNATIONS2.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* вид конструкции */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS2 ON TOF_DES_TEXTS2.TEX_ID = TOF_DESIGNATIONS2.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS3 ON TOF_DESIGNATIONS3.DES_ID = TOF_TYPES.TYP_KV_ABS_DES_ID AND TOF_DESIGNATIONS3.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* abs */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS3 ON TOF_DES_TEXTS3.TEX_ID = TOF_DESIGNATIONS3.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS4 ON TOF_DESIGNATIONS4.DES_ID = TOF_TYPES.TYP_KV_ASR_DES_ID AND TOF_DESIGNATIONS4.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* asr */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS4 ON TOF_DES_TEXTS4.TEX_ID = TOF_DESIGNATIONS4.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS5 ON TOF_DESIGNATIONS5.DES_ID = TOF_TYPES.TYP_KV_BRAKE_TYPE_DES_ID AND TOF_DESIGNATIONS5.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* TYP_KV_BRAKE_TYPE */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS5 ON TOF_DES_TEXTS5.TEX_ID = TOF_DESIGNATIONS5.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS6 ON TOF_DESIGNATIONS6.DES_ID = TOF_TYPES.TYP_KV_BRAKE_SYST_DES_ID AND TOF_DESIGNATIONS6.DES_LNG_ID = " + LNG_ID.ToString() + " " +   /* тормозная система */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS6 ON TOF_DES_TEXTS6.TEX_ID = TOF_DESIGNATIONS6.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS7 ON TOF_DESIGNATIONS7.DES_ID = TOF_TYPES.TYP_KV_FUEL_DES_ID AND TOF_DESIGNATIONS7.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* вид топлива */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS7 ON TOF_DES_TEXTS7.TEX_ID = TOF_DESIGNATIONS7.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS8 ON TOF_DESIGNATIONS8.DES_ID = TOF_TYPES.TYP_KV_FUEL_SUPPLY_DES_ID AND TOF_DESIGNATIONS8.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* впрыск */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS8 ON TOF_DES_TEXTS8.TEX_ID = TOF_DESIGNATIONS8.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS9 ON TOF_DESIGNATIONS9.DES_ID = TOF_TYPES.TYP_KV_CATALYST_DES_ID AND TOF_DESIGNATIONS9.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* вид катализатора */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS9 ON TOF_DES_TEXTS9.TEX_ID = TOF_DESIGNATIONS9.DES_TEX_ID " +

            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS1 ON TOF_DESIGNATIONS1.DES_ID = TOF_TYPES.TYP_KV_TRANS_DES_ID AND TOF_DESIGNATIONS1.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* трансмиссия */
            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS1 ON TOF_DES_TEXTS1.TEX_ID = TOF_DESIGNATIONS1.DES_TEX_ID " +

//            "LEFT JOIN TOF_DESIGNATIONS TOF_DESIGNATIONS_A ON TOF_DESIGNATIONS_A.DES_ID = TOF_TYPES.TYP_KV_ENGINE_DES_ID AND TOF_DESIGNATIONS_A.DES_LNG_ID = " + LNG_ID.ToString() + " " +  /* мотор */
                //            "LEFT JOIN TOF_DES_TEXTS TOF_DES_TEXTS_A ON TOF_DES_TEXTS_A.TEX_ID = TOF_DESIGNATIONS_A.DES_TEX_ID " +

            "LEFT JOIN TOF_LINK_TYP_ENG ON LTE_TYP_ID = TYP_ID " +
            "LEFT JOIN TOF_ENGINES ON ENG_ID = LTE_ENG_ID " +


            " WHERE " +
            " LA_ART_ID =  " + ART_ID + "  AND " +
            " LA_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1 " + 
	
/*
            (modelTypeId.HasValue ? (" TOF_TYPES.TYP_ID = " + modelTypeId.Value.ToString())
                                  : (" TOF_TYPES.TYP_MOD_ID = " + MOD_ID.ToString()) +
                                    (fluelId.HasValue ? (" AND TOF_TYPES.TYP_KV_FUEL_DES_ID = " + fluelId.Value.ToString()) : (""))
                                  ) +
 */ 

            " AND TOF_COUNTRY_DESIGNATIONS.CDS_LNG_ID = " + LNG_ID.ToString() +
            " AND TOF_TYPES.TYP_CTM subrange(" + COU_ID.ToString() + " cast integer) = 1 " +
            " ORDER BY TOF_TYPES.TYP_SORT";

        }
        protected string GetArticleEan_TEXT(int ART_ID)
        {
            return
                " select ARL_DISPLAY_NR " +
                " from TOF_ART_LOOKUP  WHERE arl_kind in ('5') and ARL_ART_ID = " + ART_ID.ToString();
        }
        protected string GetPhoto_TEXT(int GRD_FLD, int GRD_ID)
        {
            return
                "SELECT GRD_GRAPHIC FROM TOF_GRA_DATA_" + GRD_FLD.ToString() +
                " WHERE GRD_ID = " + GRD_ID.ToString();

        }


        protected string GetAllArticleApplic_TEXT(int LNG_ID, int COU_ID, int articleApplicfrom, int articleApplictil)
        {
            string aResult =
            "SELECT " +
            "LA_ART_ID, " +
            "LAT_TYP_ID " +
//             "TYP_ID " +

            " from TOF_LINK_LA_TYP " +
            " join tof_link_art " +
            " on lat_la_id nljoin la_id " +

//            "FROM TOF_LINK_ART " +
//            "INNER JOIN TOF_LINK_LA_TYP ON LA_ID = LAT_LA_ID  "+
//            " AND LA_GA_ID = LAT_GA_ID " +

            //"FROM TOF_TYPES " +
            //"INNER JOIN TOF_LINK_LA_TYP ON TYP_ID = LAT_TYP_ID " +
            //"INNER JOIN TOF_LINK_ART ON LAT_LA_ID = LA_ID " +

            //"FROM TOF_LINK_ART " +
            //"INNER JOIN TOF_LINK_LA_TYP ON LAT_LA_ID = LA_ID " +
            //"INNER JOIN TOF_TYPES ON TYP_ID = LAT_TYP_ID " +


            " WHERE " +
            " (LAT_TYP_ID >=  " + articleApplicfrom.ToString() + ") ";

            if (articleApplictil > 0) {
                aResult = aResult + " AND (LAT_TYP_ID <=  " + articleApplictil.ToString() + " )";
            }
  


            return aResult;
        }
        
                
        protected OdbcConnection odbcConnectionTD = null;

        protected OdbcConnection ConnectionTD {
            get {
                if (odbcConnectionTD == null)
                {
                    odbcConnectionTD = new System.Data.Odbc.OdbcConnection("Dsn=TECDOC");
                }
                return odbcConnectionTD;
            }
        }
        protected void DoOpenConnectionTD() {
            if (ConnectionTD.State != ConnectionState.Closed)
            {
                ConnectionTD.Close();
            }
            if ( ConnectionTD.State==ConnectionState.Closed) {
                ConnectionTD.Open();
            }
        }
        protected OdbcCommand odbcCommandTD = null;
        protected OdbcCommand OdbcCommandTD {
            get {
                if ( odbcCommandTD == null) {
                    odbcCommandTD = new OdbcCommand();
                    odbcCommandTD.Connection = ConnectionTD;
                }
                return odbcCommandTD;
            }
        }
        public List<LANGUAGES_TD> GetLANGUAGES(int LNG_ID)
        {
            List<LANGUAGES_TD> aResult = new List<LANGUAGES_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetLANGUAGES_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try {
                while (rd.Read()) {
                    LANGUAGES_TD itm = new LANGUAGES_TD();
                    if (!rd.IsDBNull(0)) itm.LNG_ID = Convert.ToInt32( rd.GetValue(0) );
                    if (!rd.IsDBNull(1)) itm.TEX_TEXT = rd.GetString(1);
                    aResult.Add(itm);
                }
            } finally {
                rd.Close();
            }
            return aResult;
        }
        public List<COUNTRIES_TD> GetCOUNTRIES(int LNG_ID)
        {
            List<COUNTRIES_TD> aResult = new List<COUNTRIES_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetCOUNTRIES_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    COUNTRIES_TD itm = new COUNTRIES_TD();
                     if (!rd.IsDBNull(0))
                         itm.COU_ID = Convert.ToInt32(rd.GetValue(0));  // GetAsInt32(rd, 0); // (int)rd.GetInt16(0);
                    if (!rd.IsDBNull(1)) itm.TEX_TEXT = rd.GetString(1);
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        public List<BRAND_TD> GetBRANDS()
        {
            List<BRAND_TD> aResult = new List<BRAND_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetBRANDS_TEXT();
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    BRAND_TD itm = new BRAND_TD();
                    if (!rd.IsDBNull(0))
                        itm.MFA_ID = Convert.ToInt32(rd.GetValue(0)); // GetAsInt32(rd, 0); // rd.GetInt32(0);
                    if (!rd.IsDBNull(1)) itm.MFA_BRAND = rd.GetString(1);
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        public List<MODEL_TD> GetMODELS(int LNG_ID, int COU_ID, int MFA_ID, int? MOD_ID = null)
        {
            List<MODEL_TD> aResult = new List<MODEL_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELS_TEXT(LNG_ID, COU_ID, MFA_ID, MOD_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    MODEL_TD itm = new MODEL_TD();
                    if (!rd.IsDBNull(0))
                        itm.MOD_ID = Convert.ToInt32(rd.GetValue(0)); // rd.GetInt32(0);
                    if (!rd.IsDBNull(1)) itm.TEX_TEXT = rd.GetString(1);
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        public List<MODEL_TD> GetMODELS_EX(int LNG_ID, int COU_ID, int MFA_ID, int? MOD_ID = null)
        {
            List<MODEL_TD> aResult = new List<MODEL_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELS_EX_TEXT(LNG_ID, COU_ID, MFA_ID, MOD_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    MODEL_TD itm = new MODEL_TD();
                    if (!rd.IsDBNull(0))
                        itm.MOD_ID = Convert.ToInt32(rd.GetValue(0)); // rd.GetInt32(0);
                    if (!rd.IsDBNull(1)) itm.TEX_TEXT = rd.GetString(1);
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        public List<MODELTYPE_TD> GetMODELTYPES(int LNG_ID, int COU_ID, int MOD_ID, int? fluelId = null, int? modelTypeId = null)
        {
            List<MODELTYPE_TD> aResult = new List<MODELTYPE_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPES_TEXT(LNG_ID, COU_ID, MOD_ID, fluelId, modelTypeId);
            DoOpenConnectionTD();
            int CurrentId = 0;
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                int itemId = 0;
                while (rd.Read())
                {
                    if (!rd.IsDBNull(0)) itemId = Convert.ToInt32(rd.GetValue(0));
                    if (itemId == CurrentId) continue;
                    CurrentId = itemId;
                    MODELTYPE_TD itm = new MODELTYPE_TD();
                    itm.TYP_ID = CurrentId; //rd.GetInt32(0);

                      if (!rd.IsDBNull(1)  )     itm.TEX_TEXT            =  Convert.ToString( rd.GetValue(1) );
                      if (!rd.IsDBNull(2)  )     itm.TYP_KV_BODY         =  Convert.ToString( rd.GetValue(2) );
                      if (!rd.IsDBNull(3))
                      {
                          itm.TYP_PCON_START = Convert.ToString(rd.GetValue(3));
                          if ((!string.IsNullOrEmpty(itm.TYP_PCON_START)) && (itm.TYP_PCON_START.Length > 4))
                          {
                              itm.TYP_PCON_START = itm.TYP_PCON_START.Substring(0, 4) + "-" + itm.TYP_PCON_START.Substring(4);
                          }
                      }
                      if (!rd.IsDBNull(4))
                      {
                          itm.TYP_PCON_END = Convert.ToString(rd.GetValue(4));
                          if ((!string.IsNullOrEmpty(itm.TYP_PCON_END)) && (itm.TYP_PCON_END.Length > 4))
                          {
                              itm.TYP_PCON_END = itm.TYP_PCON_END.Substring(0, 4) + "-" + itm.TYP_PCON_END.Substring(4);
                          }
                      }
                      if (!rd.IsDBNull(5)  )     itm.TYP_KW_FROM         =  Convert.ToString( rd.GetValue(5) );
                      if (!rd.IsDBNull(6)  )     itm.TYP_KW_UPTO         =  Convert.ToString( rd.GetValue(6) );
                      if (!rd.IsDBNull(7)  )     itm.TYP_HP_FROM         =  Convert.ToString( rd.GetValue(7) );
                      if (!rd.IsDBNull(8)  )     itm.TYP_HP_UPTO         =  Convert.ToString( rd.GetValue(8) );
                      if (!rd.IsDBNull(9)  )     itm.TYP_CCM             =  Convert.ToString( rd.GetValue(9) );
                      if (!rd.IsDBNull(10) )     itm.TYP_VALVES          =  Convert.ToString( rd.GetValue(10) );
                      if (!rd.IsDBNull(11) )     itm.TYP_CYLINDERS       =  Convert.ToString( rd.GetValue(11) );
                      if (!rd.IsDBNull(12) )     itm.TYP_DOORS           =  Convert.ToString( rd.GetValue(12) );
                      if (!rd.IsDBNull(13) )     itm.TYP_KV_ABS          =  Convert.ToString( rd.GetValue(13) );
                      if (!rd.IsDBNull(14) )     itm.TYP_KV_ASR          =  Convert.ToString( rd.GetValue(14) );
                                                                            
                      if (!rd.IsDBNull(15) )     itm.TYP_KV_BRAKE_TYPE   =  Convert.ToString( rd.GetValue(15) );
                      if (!rd.IsDBNull(16) )     itm.TYP_KV_BRAKE_SYST   =  Convert.ToString( rd.GetValue(16) );
                                                                                             
                      if (!rd.IsDBNull(17) )     itm.TYP_KV_FUEL         =  Convert.ToString( rd.GetValue(17) );
                      if (!rd.IsDBNull(18) )     itm.TYP_KV_FUEL_SUPPLY  =  Convert.ToString( rd.GetValue(18) );
                      if (!rd.IsDBNull(19) )     itm.TYP_KV_CATALYST     =  Convert.ToString( rd.GetValue(19) );
                                                                                                    
                      if (!rd.IsDBNull(20))      itm.TYP_KV_TRANS        = Convert.ToString( rd.GetValue(20)  );
                      if (!rd.IsDBNull(21))      itm.TYP_KV_ENGINE       = Convert.ToString(rd.GetValue(21));
                    
                    aResult.Add(itm);                                                              
                }                          
            }                              
            finally                        
            {                              
                rd.Close();                
            }
            return aResult;
        }
        public List<MODELTYPE_TD> GetMODELTYPES_EX(int LNG_ID, int COU_ID, int MOD_ID, int? fluelId = null, int? modelTypeId = null)
        {
            List<MODELTYPE_TD> aResult = new List<MODELTYPE_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPES_EX_TEXT(LNG_ID, COU_ID, MOD_ID, fluelId, modelTypeId);
            DoOpenConnectionTD();
            int CurrentId = 0;
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                int itemId = 0;
                while (rd.Read())
                {
                    if (!rd.IsDBNull(0)) itemId = Convert.ToInt32(rd.GetValue(0));
                    // if (itemId == CurrentId) continue;
                    CurrentId = itemId;
                    MODELTYPE_TD itm = new MODELTYPE_TD();
                    itm.TYP_ID = CurrentId; //rd.GetInt32(0);

                      if (!rd.IsDBNull(1)  )     itm.TEX_TEXT            =  Convert.ToString( rd.GetValue(1) );
                      if (!rd.IsDBNull(2)  )     itm.TYP_KV_BODY         =  Convert.ToString( rd.GetValue(2) );
                      if (!rd.IsDBNull(3))
                      {
                          itm.TYP_PCON_START = Convert.ToString(rd.GetValue(3));
                          if ((!string.IsNullOrEmpty(itm.TYP_PCON_START)) && (itm.TYP_PCON_START.Length > 4))
                          {
                              itm.TYP_PCON_START = itm.TYP_PCON_START.Substring(0, 4) + "-" + itm.TYP_PCON_START.Substring(4);
                          }
                      }
                      if (!rd.IsDBNull(4))
                      {
                          itm.TYP_PCON_END = Convert.ToString(rd.GetValue(4));
                          if ((!string.IsNullOrEmpty(itm.TYP_PCON_END)) && (itm.TYP_PCON_END.Length > 4))
                          {
                              itm.TYP_PCON_END = itm.TYP_PCON_END.Substring(0, 4) + "-" + itm.TYP_PCON_END.Substring(4);
                          }
                      }
                      if (!rd.IsDBNull(5)  )     itm.TYP_KW_FROM         =  Convert.ToString( rd.GetValue(5) );
                      if (!rd.IsDBNull(6)  )     itm.TYP_KW_UPTO         =  Convert.ToString( rd.GetValue(6) );
                      if (!rd.IsDBNull(7)  )     itm.TYP_HP_FROM         =  Convert.ToString( rd.GetValue(7) );
                      if (!rd.IsDBNull(8)  )     itm.TYP_HP_UPTO         =  Convert.ToString( rd.GetValue(8) );
                      if (!rd.IsDBNull(9)  )     itm.TYP_CCM             =  Convert.ToString( rd.GetValue(9) );
                      if (!rd.IsDBNull(10) )     itm.TYP_VALVES          =  Convert.ToString( rd.GetValue(10) );
                      if (!rd.IsDBNull(11) )     itm.TYP_CYLINDERS       =  Convert.ToString( rd.GetValue(11) );
                      if (!rd.IsDBNull(12) )     itm.TYP_DOORS           =  Convert.ToString( rd.GetValue(12) );
                      if (!rd.IsDBNull(13) )     itm.TYP_KV_ABS          =  Convert.ToString( rd.GetValue(13) );
                      if (!rd.IsDBNull(14) )     itm.TYP_KV_ASR          =  Convert.ToString( rd.GetValue(14) );
                                                                            
                      if (!rd.IsDBNull(15) )     itm.TYP_KV_BRAKE_TYPE   =  Convert.ToString( rd.GetValue(15) );
                      if (!rd.IsDBNull(16) )     itm.TYP_KV_BRAKE_SYST   =  Convert.ToString( rd.GetValue(16) );
                                                                                             
                      if (!rd.IsDBNull(17) )     itm.TYP_KV_FUEL         =  Convert.ToString( rd.GetValue(17) );
                      if (!rd.IsDBNull(18) )     itm.TYP_KV_FUEL_SUPPLY  =  Convert.ToString( rd.GetValue(18) );
                      if (!rd.IsDBNull(19) )     itm.TYP_KV_CATALYST     =  Convert.ToString( rd.GetValue(19) );
                                                                                                    
                      if (!rd.IsDBNull(20))      itm.TYP_KV_TRANS        = Convert.ToString( rd.GetValue(20)  );
                      if (!rd.IsDBNull(21))      itm.TYP_KV_ENGINE       = Convert.ToString(rd.GetValue(21));
                      if (!rd.IsDBNull(22))      itm.FUEL_ID             = Convert.ToInt32(rd.GetValue(22));

                    
                    
                    aResult.Add(itm);                                                              
                }                          
            }                              
            finally                        
            {                              
                rd.Close();                
            }
            return aResult;
        }
        public List<FUEL_TD> GetFUELS(int LNG_ID)
        {
            List<FUEL_TD> aResult = new List<FUEL_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetFUELS_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {

                    FUEL_TD itm = new FUEL_TD();
                    if (!rd.IsDBNull(0)) itm.DES_ID = Convert.ToInt32(rd.GetValue(0)); //rd.GetInt32(0);
                    if (!rd.IsDBNull(1)) itm.TEX_TEXT = Convert.ToString(rd.GetValue(1));
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }
        public List<SIMPLEMODELTYPES_TD> GetSIMPLEMODELTYPES(int LNG_ID, int COU_ID, int modelTypeId)
        {
            List<SIMPLEMODELTYPES_TD> aResult = new List<SIMPLEMODELTYPES_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetSIMPLEMODELTYPES_TEXT(LNG_ID, COU_ID, modelTypeId);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    SIMPLEMODELTYPES_TD itm = new SIMPLEMODELTYPES_TD();
                    if (!rd.IsDBNull(0)) itm.TYP_ID = Convert.ToInt32(rd.GetValue(0)); //rd.GetInt32(0);
                    if (!rd.IsDBNull(1)) itm.TEX_TEXT = Convert.ToString(rd.GetValue(1));
                    if (!rd.IsDBNull(2)) itm.TYP_KV_FUEL = Convert.ToInt32(rd.GetValue(2));
                    if (!rd.IsDBNull(3)) itm.TYP_MOD_ID = Convert.ToInt32(rd.GetValue(3));
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }

        protected MODELTYPESTREE_TD GetMODELTYPESTREE_Parent(ICollection<MODELTYPESTREE_TD> aResult, int prntId, bool isOpen)
        {
            if (aResult == null) return null;
            MODELTYPESTREE_TD aRet = (from e in aResult where e.STR_ID == prntId select e).FirstOrDefault();
            if (aRet != null)
            {
                if (isOpen)
                {
                    aRet.isOpen = isOpen;
                }
                return aRet;
            }
            foreach (var itm in aResult)
            {
                aRet = GetMODELTYPESTREE_Parent(itm.Subitems, prntId, isOpen);
                if (aRet != null)
                {
                    if (isOpen)
                    {
                        itm.isOpen = isOpen;
                    }
                    return aRet;
                }
            }
            return null;
        }

        public List<MODELTYPESTREE_TD> GetMODELTYPESTREE(int LNG_ID, int TYP_ID, int topicId)
        {
            List<MODELTYPESTREE_TD> aResult = new List<MODELTYPESTREE_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPESTREE_TEXT(LNG_ID, TYP_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                bool isFirst = true;
                int currLevel = 0;
                MODELTYPESTREE_TD prntItm = null;
                while (rd.Read())
                {

                    MODELTYPESTREE_TD itm = new MODELTYPESTREE_TD();
                    if (!rd.IsDBNull(0)) itm.STR_ID = Convert.ToInt32(rd.GetValue(0)); 
                    if (!rd.IsDBNull(1)) itm.TEX_TEXT = Convert.ToString(rd.GetValue(3));
                    itm.isOpen = (topicId == itm.STR_ID);
                    int itmLevel = Convert.ToInt32(rd.GetValue(1));
                    if (isFirst)
                    {
                        isFirst = false;
                        currLevel = itmLevel;
                        aResult.Add(itm);
                        prntItm = itm;
                    }
                    else
                    {
                        if (currLevel == itmLevel)
                        {
                            aResult.Add(itm);
                        }
                        else
                        {
                            int prntId = Convert.ToInt32(rd.GetValue(4));
                            prntItm = GetMODELTYPESTREE_Parent(aResult, prntId, itm.isOpen);
                            if (prntItm != null)
                            {
                                if (prntItm.Subitems == null) prntItm.Subitems = new List<MODELTYPESTREE_TD>();
                                prntItm.Subitems.Add(itm);
                            }
                            else
                            {
                                aResult.Add(itm);
                            }
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

        public List<MODELTYPETREEITEMS_REST_TD> GetMODELTYPETREEITEMS(int LNG_ID, int COU_ID, int TYP_ID, int STR_ID, int tof_assemblyId, int tof_suppliersId)
        {
            List<MODELTYPETREEITEMS_REST_TD> aResult = new List<MODELTYPETREEITEMS_REST_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPETREEITEMS_TEXT(LNG_ID, COU_ID, TYP_ID, STR_ID, tof_assemblyId, tof_suppliersId);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    MODELTYPETREEITEMS_REST_TD itm = new MODELTYPETREEITEMS_REST_TD();
                    if (!rd.IsDBNull(0)) itm.SUP_ID = Convert.ToInt32(rd.GetValue(0)); //rd.GetInt32(0);
                    if (!rd.IsDBNull(1)) itm.SUP_TEXT = Convert.ToString(rd.GetValue(1));
                    if (!rd.IsDBNull(2)) itm.GA_NR = Convert.ToInt32(rd.GetValue(2));
                    if (!rd.IsDBNull(3)) itm.MASTER_BEZ = Convert.ToString(rd.GetValue(3));
                    if (!rd.IsDBNull(4)) itm.ART_ARTICLE_NR = Convert.ToString(rd.GetValue(4));
                    if (!rd.IsDBNull(5)) itm.ART_ID = Convert.ToInt32(rd.GetValue(5));
                    if (!rd.IsDBNull(6)) itm.LA_ID = Convert.ToInt32(rd.GetValue(6));
                    if (!rd.IsDBNull(7)) itm.GA_TEXT = Convert.ToString(rd.GetValue(7));
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }

        public List<MODELTYPETREEITEMDESCR_TD> GetMODELTYPETREEITEMDESCR(int LNG_ID, int COU_ID, int ART_ID)
        {
            List<MODELTYPETREEITEMDESCR_TD> aResult = new List<MODELTYPETREEITEMDESCR_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPETREEITEMDESCR_TEXT(LNG_ID, COU_ID, ART_ID); //, LA_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    MODELTYPETREEITEMDESCR_TD itm = new MODELTYPETREEITEMDESCR_TD();
                    if (!rd.IsDBNull(0)) itm.TEX_TEXT = Convert.ToString(rd.GetValue(0));
                    if (!rd.IsDBNull(1)) itm.TEX_VALUE = Convert.ToString(rd.GetValue(1));
                    if (!rd.IsDBNull(2)) itm.TEX_UNIT = Convert.ToString(rd.GetValue(2));
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }

        public List<MODELTYPETREEITEMMANID_TD> GetMODELTYPETREEITEMMANID(int LNG_ID, int COU_ID, int ART_ID, int ? MFA_ID = null) // , int LA_ID)
        {
            List<MODELTYPETREEITEMMANID_TD> aResult = new List<MODELTYPETREEITEMMANID_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetMODELTYPETREEITEMMANID_TEXT(LNG_ID, COU_ID, ART_ID, MFA_ID); //, LA_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    MODELTYPETREEITEMMANID_TD itm = new MODELTYPETREEITEMMANID_TD();
                    if (!rd.IsDBNull(1)) itm.SUP_TEXT = Convert.ToString(rd.GetValue(1));
                    if (!rd.IsDBNull(2)) itm.TEX_VALUE = Convert.ToString(rd.GetValue(2));
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;

        }

        public List<ANALOGOUS_REST_TD> GetANALOGS(int LNG_ID, int COU_ID, string ART_ARTICLE_NR, int? GA_NR)
        {
            List<ANALOGOUS_REST_TD> aResult = new List<ANALOGOUS_REST_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetANALOGS_TEXT(LNG_ID, COU_ID, ART_ARTICLE_NR, GA_NR); 
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    ANALOGOUS_REST_TD itm = new ANALOGOUS_REST_TD();
                    if (!rd.IsDBNull(0)) itm.ART_ID = Convert.ToInt32(rd.GetValue(0));
                    if (!rd.IsDBNull(1)) itm.ART_ARTICLE_NR = Convert.ToString(rd.GetValue(1));
                    if (!rd.IsDBNull(2)) itm.GA_NR = Convert.ToInt32(rd.GetValue(2));
                    if (!rd.IsDBNull(3)) itm.MASTER_BEZ = Convert.ToString(rd.GetValue(3));
                    if (!rd.IsDBNull(4)) itm.GA_TEXT = Convert.ToString(rd.GetValue(4));
                    if (!rd.IsDBNull(5)) itm.SUP_TEXT = Convert.ToString(rd.GetValue(5));
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;

        }

        public List<MODELTYPETREEITEMS_TD> GetArticleByID(int LNG_ID, int COU_ID, string ART_ARTICLE_NR, int searchType)
        {
            List<MODELTYPETREEITEMS_TD> aResult = new List<MODELTYPETREEITEMS_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleByID_TEXT(LNG_ID, COU_ID, ART_ARTICLE_NR, searchType);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            int CurrentId = 0;
            try
            {
                int itemId = 0;
                while (rd.Read())
                {
                    if (rd.IsDBNull(0)) continue;
                    itemId = Convert.ToInt32(rd.GetValue(0));
                    if (itemId == CurrentId) continue;
                    CurrentId = itemId;
                    MODELTYPETREEITEMS_TD itm = new MODELTYPETREEITEMS_TD();
                    itm.ART_ID = itemId; 
                    if (!rd.IsDBNull(1)) itm.ART_ARTICLE_NR = Convert.ToString(rd.GetValue(1));
                    if (!rd.IsDBNull(2)) itm.SUP_ID = Convert.ToInt32(rd.GetValue(2)); 
                    if (!rd.IsDBNull(3)) itm.SUP_TEXT = Convert.ToString(rd.GetValue(3));
                    if (!rd.IsDBNull(4)) itm.MASTER_BEZ = Convert.ToString(rd.GetValue(4));
                    if (!rd.IsDBNull(5)) itm.GA_NR = Convert.ToInt32(rd.GetValue(5));
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }

        public List<MODELTYPE_TD> GetArticleApplic(int LNG_ID, int COU_ID, int ART_ID)
        {
            List<MODELTYPE_TD> aResult = new List<MODELTYPE_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleApplic_TEXT(LNG_ID, COU_ID, ART_ID);
            DoOpenConnectionTD();
            int CurrentId = 0;
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                int itemId = 0;
                while (rd.Read())
                {
                    if (!rd.IsDBNull(0)) itemId = Convert.ToInt32(rd.GetValue(0));
                    if (itemId == CurrentId) continue;
                    CurrentId = itemId;
                    MODELTYPE_TD itm = new MODELTYPE_TD();
                    itm.TYP_ID = CurrentId; //rd.GetInt32(0);

                    if (!rd.IsDBNull(1)) itm.TEX_TEXT = Convert.ToString(rd.GetValue(1));
                    if (!rd.IsDBNull(2)) itm.TYP_KV_BODY = Convert.ToString(rd.GetValue(2));
                    if (!rd.IsDBNull(3))
                    {
                        itm.TYP_PCON_START = Convert.ToString(rd.GetValue(3));
                        if ((!string.IsNullOrEmpty(itm.TYP_PCON_START)) && (itm.TYP_PCON_START.Length > 4))
                        {
                            itm.TYP_PCON_START = itm.TYP_PCON_START.Substring(0, 4) + "-" + itm.TYP_PCON_START.Substring(4);
                        }
                    }
                    if (!rd.IsDBNull(4))
                    {
                        itm.TYP_PCON_END = Convert.ToString(rd.GetValue(4));
                        if ((!string.IsNullOrEmpty(itm.TYP_PCON_END)) && (itm.TYP_PCON_END.Length > 4))
                        {
                            itm.TYP_PCON_END = itm.TYP_PCON_END.Substring(0, 4) + "-" + itm.TYP_PCON_END.Substring(4);
                        }
                    }
                    if (!rd.IsDBNull(5)) itm.TYP_KW_FROM = Convert.ToString(rd.GetValue(5));
                    if (!rd.IsDBNull(6)) itm.TYP_KW_UPTO = Convert.ToString(rd.GetValue(6));
                    if (!rd.IsDBNull(7)) itm.TYP_HP_FROM = Convert.ToString(rd.GetValue(7));
                    if (!rd.IsDBNull(8)) itm.TYP_HP_UPTO = Convert.ToString(rd.GetValue(8));
                    if (!rd.IsDBNull(9)) itm.TYP_CCM = Convert.ToString(rd.GetValue(9));
                    if (!rd.IsDBNull(10)) itm.TYP_VALVES = Convert.ToString(rd.GetValue(10));
                    if (!rd.IsDBNull(11)) itm.TYP_CYLINDERS = Convert.ToString(rd.GetValue(11));
                    if (!rd.IsDBNull(12)) itm.TYP_DOORS = Convert.ToString(rd.GetValue(12));
                    if (!rd.IsDBNull(13)) itm.TYP_KV_ABS = Convert.ToString(rd.GetValue(13));
                    if (!rd.IsDBNull(14)) itm.TYP_KV_ASR = Convert.ToString(rd.GetValue(14));

                    if (!rd.IsDBNull(15)) itm.TYP_KV_BRAKE_TYPE = Convert.ToString(rd.GetValue(15));
                    if (!rd.IsDBNull(16)) itm.TYP_KV_BRAKE_SYST = Convert.ToString(rd.GetValue(16));

                    if (!rd.IsDBNull(17)) itm.TYP_KV_FUEL = Convert.ToString(rd.GetValue(17));
                    if (!rd.IsDBNull(18)) itm.TYP_KV_FUEL_SUPPLY = Convert.ToString(rd.GetValue(18));
                    if (!rd.IsDBNull(19)) itm.TYP_KV_CATALYST = Convert.ToString(rd.GetValue(19));

                    if (!rd.IsDBNull(20)) itm.TYP_KV_TRANS = Convert.ToString(rd.GetValue(20));
                    if (!rd.IsDBNull(21)) itm.TYP_KV_ENGINE = Convert.ToString(rd.GetValue(21));

                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }

        public List<FUEL_TD> GetArticleEan(int ART_ID)
        {
            List<FUEL_TD> aResult = new List<FUEL_TD>();
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleEan_TEXT(ART_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    FUEL_TD itm = new FUEL_TD();
                    if (!rd.IsDBNull(0)) itm.TEX_TEXT = Convert.ToString(rd.GetValue(0));
                    aResult.Add(itm);
                }
            }
            finally
            {
                rd.Close();
            }
            return aResult;
        }

        public byte[] GetPhoto(int GRD_FLD, int GRD_ID)
        {
            byte[] aResult = null;
            
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetPhoto_TEXT(GRD_FLD, GRD_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            try
            {
                while (rd.Read())
                {
                    if (!rd.IsDBNull(0)) {
                        long bufLen = rd.GetBytes(0,0L,null,0, 128000);
                        if (bufLen > 0) {
                            aResult = new byte[bufLen];
                            rd.GetBytes(0, 0L, aResult, 0, (int)bufLen);
                            break;
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

        public object GetAllArticle_READER(int LNG_ID, int COU_ID, int TecDocFrom, int TecDocTil)
        {
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetAllArticle_TEXT(LNG_ID, COU_ID, TecDocFrom, TecDocTil);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            return rd;
        }
        public MODELTYPETREEITEMS_TD GetAllArticle_NEXT(object reader)
        {
            if (reader == null) return null;

            if (!(reader is OdbcDataReader)) return null;

            OdbcDataReader rd = reader as OdbcDataReader;
            MODELTYPETREEITEMS_TD itm = null;

            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;
                itm = new MODELTYPETREEITEMS_TD();
                itm.ART_ID = Convert.ToInt32(rd.GetValue(0));
                if (!rd.IsDBNull(1)) itm.ART_ARTICLE_NR = Convert.ToString(rd.GetValue(1));
                if (!rd.IsDBNull(2)) itm.SUP_ID = Convert.ToInt32(rd.GetValue(2));
                if (!rd.IsDBNull(3)) itm.SUP_TEXT = Convert.ToString(rd.GetValue(3));
                if (!rd.IsDBNull(4)) itm.MASTER_BEZ = Convert.ToString(rd.GetValue(4));
                if (!rd.IsDBNull(5)) itm.EAN_TEXT = Convert.ToString(rd.GetValue(5));
                return itm;
            }
            rd.Close();
            return null;

        }
        public int GetAllArticleMax()
        {
            int aResult = 0;
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetAllArticleMax_TEXT();
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;
                aResult = Convert.ToInt32(rd.GetValue(0));
            }
            return aResult;
        }


        public object GetArticleGroup(int ArticleCategoryFrom, int ArticleCategoryTil)
        {
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleGroup_text(ArticleCategoryFrom, ArticleCategoryTil);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            return rd;
        }
        public ArticleGroup_TD GetArticleGroup_NEXT(object reader)
        {

            if (reader == null) return null;

            if (!(reader is OdbcDataReader)) return null;

            OdbcDataReader rd = reader as OdbcDataReader;
            ArticleGroup_TD item = null;
            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;
                item = new ArticleGroup_TD();
                item.ART_ID = Convert.ToInt32(rd.GetValue(0));
                item.GROUP_ID = Convert.ToInt32(rd.GetValue(1));
                return item;
            }
            rd.Close();
            return null;
        }
        public int GetArticleGroupMax()
        {
            int aResult = 0;
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleGroupMax_TEXT();
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;
                aResult = Convert.ToInt32(rd.GetValue(0));
            }
            return aResult;
        }


        public object GetArticleBrand()
        {
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleBrand_TEXT();
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            return rd;
        }
        public BRAND_TD GetArticleBrand_NEXT(object reader)
        {

            if (reader == null) return null;

            if (!(reader is OdbcDataReader)) return null;

            OdbcDataReader rd = reader as OdbcDataReader;
            BRAND_TD item = null;
            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;
                item = new BRAND_TD();
                item.MFA_ID = Convert.ToInt32(rd.GetValue(0));
                item.MFA_BRAND = Convert.ToString(rd.GetValue(1));
                return item;
            }
            rd.Close();
            return null;
        }
        public int GetArticleBrandMax()
        {
            int aResult = 0;
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleBrandMax_TEXT();
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;
                aResult = Convert.ToInt32(rd.GetValue(0));
            }
            return aResult;
        }


        public object GetArticleLookUp(int articlelookupfrom, int articlelookuptil)
        {
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleLookUp_TEXT(articlelookupfrom, articlelookuptil);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            return rd;
        }
        public ARTICLE_LOOKUP_TD GetArticleLookUp_NEXT(object reader)
        {

            if (reader == null) return null;

            if (!(reader is OdbcDataReader)) return null;

            OdbcDataReader rd = reader as OdbcDataReader;
            ARTICLE_LOOKUP_TD item = null;
            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;
                if (rd.IsDBNull(1)) continue;
                
                item = new ARTICLE_LOOKUP_TD();
                item.ARL_ART_ID = Convert.ToInt32(rd.GetValue(0));
                if (rd.IsDBNull(0)) continue;
                item.ARL_SEARCH_NUMBER = Convert.ToString(rd.GetValue(1));
                if (rd.IsDBNull(2))
                {
                    item.ARL_KIND = 0;
                }
                else
                {
                    char s = Convert.ToChar(rd.GetValue(2));
                    switch (s)
                    {
                        case '1': // неоригинальный (артикульный) номер, к которому относятся аналоги
                            item.ARL_KIND = 1;
                            break;
                        case '2': // торговый номер (номер пользователя)
                            item.ARL_KIND = 2;
                            break;
                        case '3': // оригинальный (конструкционный) номер
                            item.ARL_KIND = 3;
                            break;
                        case '4': // неоригинальный аналог
                            item.ARL_KIND = 4;
                            break;
                        case '5': // штрих-код (номер EAN)
                            item.ARL_KIND = 5;
                            break;
                        default:
                            item.ARL_KIND = 0;
                            break;
                    }
                }
                if (! rd.IsDBNull(3)) 
                    item.ARL_BRA_ID = Convert.ToInt32(rd.GetValue(3));
                if (! rd.IsDBNull(4))
                    item.ARL_DISPLAY_NR = Convert.ToString(rd.GetValue(4));
                return item;
            }
            rd.Close();
            return null;
        }
        public int GetArticleLookUpMax()
        {
            int aResult = 0;
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetArticleLookUpMax_TEXT();
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;
                aResult = Convert.ToInt32(rd.GetValue(0));
            }
            return aResult;
        }

        public object GetAllArticleApplic(int LNG_ID, int COU_ID, int articleApplicfrom, int articleApplictil) {
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetAllArticleApplic_TEXT(LNG_ID, COU_ID, articleApplicfrom, articleApplictil);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            return rd;
        }
        public ArticleGroup_TD GetAllArticleApplic_NEXT(object reader)
        {
            if (reader == null) return null;

            if (!(reader is OdbcDataReader)) return null;

            OdbcDataReader rd = reader as OdbcDataReader;
            ArticleGroup_TD item = null;
            while (rd.Read())
            {
                if (rd.IsDBNull(0) || rd.IsDBNull(1)) continue;
                item = new ArticleGroup_TD();
                item.ART_ID = Convert.ToInt32(rd.GetValue(0));
                item.GROUP_ID = Convert.ToInt32(rd.GetValue(1));
                return item;
            }
            rd.Close();
            return null;
        }
        public int GetArticleApplicMax()
        {
            int aResult = 0;
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = 
                "select " +
                "max(TYP_ID)" +
                "from TOF_TYPES";
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;
                aResult = Convert.ToInt32(rd.GetValue(0));
            }
            return aResult;
        }

        public object GetAllMODELTYPESTREE(int LNG_ID)
        {
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetAllMODELTYPESTREE_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            return rd;
        }
        public MODELTYPESTREE_PARENT_TD GetAllMODELTYPESTREE_NEXT(object reader)
        {
            if (reader == null) return null;

            if (!(reader is OdbcDataReader)) return null;

            OdbcDataReader rd = reader as OdbcDataReader;
            MODELTYPESTREE_PARENT_TD item = null;
            while (rd.Read())
            {
                if (rd.IsDBNull(0))  continue;
                item = new MODELTYPESTREE_PARENT_TD();
                item.STR_ID = Convert.ToInt32(rd.GetValue(0));
                item.TEX_TEXT = Convert.ToString(rd.GetValue(1));
                if (! rd.IsDBNull(2))
                    item.PARENT_ID= Convert.ToInt32(rd.GetValue(2));
                return item;
            }
            rd.Close();
            return null;
        }

        public object GetALLMODELTYPETREEITEMS(int LNG_ID) 
        {
            OdbcCommandTD.CommandType = System.Data.CommandType.Text;
            OdbcCommandTD.CommandText = GetALLMODELTYPETREEITEMS_TEXT(LNG_ID);
            DoOpenConnectionTD();
            OdbcDataReader rd = OdbcCommandTD.ExecuteReader();
            return rd;
        }
        public MODELTYPESTREE_PARENT_TD GetALLMODELTYPETREEITEMS_NEXT(object reader)
        {
            if (reader == null) return null;

            if (!(reader is OdbcDataReader)) return null;

            OdbcDataReader rd = reader as OdbcDataReader;
            MODELTYPESTREE_PARENT_TD item = null;
            while (rd.Read())
            {
                if (rd.IsDBNull(0)) continue;
                item = new MODELTYPESTREE_PARENT_TD();
                item.PARENT_ID = Convert.ToInt32(rd.GetValue(0));
                item.TEX_TEXT = Convert.ToString(rd.GetValue(1));
                if (!rd.IsDBNull(2))
                    item.STR_ID = Convert.ToInt32(rd.GetValue(2));
                return item;
            }
            rd.Close();
            return null;
        }

        public void DoCloseReader(object reader)
        {
            if (reader == null) return;
            if (!(reader is OdbcDataReader)) return;
            OdbcDataReader rd = reader as OdbcDataReader;
            if (!rd.IsClosed) rd.Close();
        }

        // MODELTYPESTREE_TD

        


        public void Dispose()
        {
            if (odbcCommandTD != null) odbcCommandTD.Dispose();
            if (odbcConnectionTD != null) odbcConnectionTD.Dispose();
        }
    }
}




