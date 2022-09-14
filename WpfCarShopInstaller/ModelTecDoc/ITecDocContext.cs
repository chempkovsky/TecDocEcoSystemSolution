using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecDocEcoSystemDbClassLibrary
{
    public interface  ITecDocContext
    {
        object GetAllArticle_READER(int LNG_ID, int COU_ID, int TecDocFrom, int TecDocTil);
        MODELTYPETREEITEMS_TD GetAllArticle_NEXT(object reader);
        int GetAllArticleMax();

        List<FUEL_TD> GetFUELS(int LNG_ID);
        List<BRAND_TD> GetBRANDS();
        List<MODEL_TD> GetMODELS(int LNG_ID, int COU_ID, int MFA_ID, int? MOD_ID = null);
        List<MODEL_TD> GetMODELS_EX(int LNG_ID, int COU_ID, int MFA_ID, int? MOD_ID = null);
        List<MODELTYPE_TD> GetMODELTYPES_EX(int LNG_ID, int COU_ID, int MOD_ID, int? fluelId = null, int? modelTypeId = null);
        object GetArticleGroup(int ArticleCategoryFrom, int ArticleCategoryTil);
        ArticleGroup_TD GetArticleGroup_NEXT(object reader);
        int GetArticleGroupMax();

        object GetArticleBrand();
        BRAND_TD GetArticleBrand_NEXT(object reader);
        int GetArticleBrandMax();

        object GetArticleLookUp(int articlelookupfrom, int articlelookuptil);
        ARTICLE_LOOKUP_TD GetArticleLookUp_NEXT(object reader);
        int GetArticleLookUpMax();

        object GetAllArticleApplic(int LNG_ID, int COU_ID, int articleApplicfrom, int articleApplictil);
        ArticleGroup_TD GetAllArticleApplic_NEXT(object reader);
        int GetArticleApplicMax();


        object GetAllMODELTYPESTREE(int LNG_ID);
        MODELTYPESTREE_PARENT_TD GetAllMODELTYPESTREE_NEXT(object reader);

        object GetALLMODELTYPETREEITEMS(int LNG_ID);
        MODELTYPESTREE_PARENT_TD GetALLMODELTYPETREEITEMS_NEXT(object reader);

        void DoCloseReader(object reader);
    }
}
