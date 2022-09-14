using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TecDocEcoSystemDbClassLibrary;

namespace CarShopDataService.Interfaces
{
    public interface ICarShopMsTecDocDataService
    {
        #region DoCreateDb
        void DoCreateDb();
        string GetConnectionStringName();
        void SetConnectionStringName(string connectionStringName);
        #endregion


        #region EnterpriseCarModelFuelTDES
        EnterpriseCarModelFuelTDES EnterpriseCarModelFuelTDESInsert(EnterpriseCarModelFuelTDES item);
        void EnterpriseCarModelFuelTDESUpdate(EnterpriseCarModelFuelTDES item);
        void EnterpriseCarModelFuelTDESInsertOrUpdate(EnterpriseCarModelFuelTDES item);
        #endregion

        #region EnterpriseCarBrandTDES
        EnterpriseCarBrandTDES EnterpriseCarBrandTDESInsert(EnterpriseCarBrandTDES item);
        void EnterpriseCarBrandTDESUpdate(EnterpriseCarBrandTDES item);
        void EnterpriseCarBrandTDESInsertOrUpdate(EnterpriseCarBrandTDES item);
        #endregion

        #region EnterpriseCarModelTypeTDES
        EnterpriseCarModelTypeTDES EnterpriseCarModelTypeTDESInsert(EnterpriseCarModelTypeTDES item);
        void EnterpriseCarModelTypeTDESUpdate(EnterpriseCarModelTypeTDES item);
        void EnterpriseCarModelTypeTDESInsertOrUpdate(EnterpriseCarModelTypeTDES item);
        #endregion

        #region EnterpriseCarModelTDES
        EnterpriseCarModelTDES EnterpriseCarModelTDESInsert(EnterpriseCarModelTDES item);
        void EnterpriseCarModelTDESUpdate(EnterpriseCarModelTDES item);
        void EnterpriseCarModelTDESInsertOrUpdate(EnterpriseCarModelTDES item);
        #endregion

        #region EnterpriseArticleCategoryItemTDES
        EnterpriseArticleCategoryItemTDES EnterpriseArticleCategoryItemTDESInsert(EnterpriseArticleCategoryItemTDES item);
        void EnterpriseArticleCategoryItemTDESUpdate(EnterpriseArticleCategoryItemTDES item);
        void EnterpriseArticleCategoryItemTDESInsertOrUpdate(EnterpriseArticleCategoryItemTDES item);
        #endregion

        #region EnterpriseArticleBrandTDES
        EnterpriseArticleBrandTDES EnterpriseArticleBrandTDESInsert(EnterpriseArticleBrandTDES item);
        void EnterpriseArticleBrandTDESUpdate(EnterpriseArticleBrandTDES item);
        void EnterpriseArticleBrandTDESInsertOrUpdate(EnterpriseArticleBrandTDES item);
        #endregion

        #region EnterpriseArticleLookUpTDES
        EnterpriseArticleLookUpTDES EnterpriseArticleLookUpTDESInsert(EnterpriseArticleLookUpTDES item);
        void EnterpriseArticleLookUpTDESUpdate(EnterpriseArticleLookUpTDES item);
        void EnterpriseArticleLookUpTDESInsertOrUpdate(EnterpriseArticleLookUpTDES item);
        #endregion

        #region EnterpriseArticleApplicTDES
        EnterpriseArticleApplicTDES EnterpriseArticleApplicTDESInsert(EnterpriseArticleApplicTDES item);
        void EnterpriseArticleApplicTDESUpdate(EnterpriseArticleApplicTDES item);
        void EnterpriseArticleApplicTDESInsertOrUpdate(EnterpriseArticleApplicTDES item);
        #endregion

        #region EnterpriseArticleTecDocTDES
        EnterpriseArticleTecDocTDES EnterpriseArticleTecDocTDESInsert(EnterpriseArticleTecDocTDES item);
        void EnterpriseArticleTecDocTDESUpdate(EnterpriseArticleTecDocTDES item);
        void EnterpriseArticleTecDocTDESInsertOrUpdate(EnterpriseArticleTecDocTDES item);
        void EnterpriseArticleTecDocTDESAdd(EnterpriseArticleTecDocTDES item, EnterpriseArticleTecDocDescriptionTDES descr);
        #endregion

        #region EnterpriseCategoryTecDocTDES
        EnterpriseCategoryTecDocTDES EnterpriseCategoryTecDocTDESInsert(EnterpriseCategoryTecDocTDES item);
        void EnterpriseCategoryTecDocTDESUpdate(EnterpriseCategoryTecDocTDES item);
        void EnterpriseCategoryTecDocTDESInsertOrUpdate(EnterpriseCategoryTecDocTDES item);
        #endregion

        #region EnterpriseCategoryItemTecDocTDES
        EnterpriseCategoryItemTecDocTDES EnterpriseCategoryItemTecDocTDESInsert(EnterpriseCategoryItemTecDocTDES item);
        void EnterpriseCategoryItemTecDocTDESUpdate(EnterpriseCategoryItemTecDocTDES item);
        void EnterpriseCategoryItemTecDocTDESInsertOrUpdate(EnterpriseCategoryItemTecDocTDES item);
        void EnterpriseCategoryItemTecDocTDESAdd(EnterpriseCategoryItemTecDocTDES item, EnterpriseCategoryItemTecDocDescriptionTDES descr);
        #endregion

    }
}
