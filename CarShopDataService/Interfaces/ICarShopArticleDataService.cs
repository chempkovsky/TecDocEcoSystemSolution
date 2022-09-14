using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TecDocEcoSystemDbClassLibrary;

namespace CarShopDataService.Interfaces
{
    public interface  ICarShopArticleDataService
    {
        #region DoCreateDb
        void DoCreateDb();
        string GetConnectionStringName();
        void SetConnectionStringName(string connectionStringName);
        #endregion


        #region EnterpriseCategoryItemDescriptionTDES
        EnterpriseCategoryItemDescriptionTDES EnterpriseCategoryItemDescriptionTDESInsert(EnterpriseCategoryItemDescriptionTDES item);
        void EnterpriseCategoryItemDescriptionTDESUpdate(EnterpriseCategoryItemDescriptionTDES item);
        void EnterpriseCategoryItemDescriptionTDESDelete(int? id = null);
        EnterpriseCategoryItemDescriptionTDES EnterpriseCategoryItemDescriptionTDESDetail(int? id = null);
        IQueryable<EnterpriseCategoryItemDescriptionTDES> EnterpriseCategoryItemDescriptionTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal);
        int EnterpriseCategoryItemDescriptionTDESCount(string searchColumnVal, string searchFilterVal);
        void EnterpriseCategoryItemDescriptionTDESSaveChanges();
        void EnterpriseCategoryItemDescriptionTDESReload(EnterpriseCategoryItemDescriptionTDES item);
        void EnterpriseCategoryItemDescriptionTDESInsertOrUpdate(EnterpriseCategoryItemDescriptionTDES item);
        #endregion

        #region EnterpriseCategoryDescriptionTDES
        EnterpriseCategoryDescriptionTDES EnterpriseCategoryDescriptionTDESInsert(EnterpriseCategoryDescriptionTDES item);
        void EnterpriseCategoryDescriptionTDESUpdate(EnterpriseCategoryDescriptionTDES item);
        void EnterpriseCategoryDescriptionTDESDelete(int? id = null);
        EnterpriseCategoryDescriptionTDES EnterpriseCategoryDescriptionTDESDetail(int? id = null);
        IQueryable<EnterpriseCategoryDescriptionTDES> EnterpriseCategoryDescriptionTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal);
        int EnterpriseCategoryDescriptionTDESCount(string searchColumnVal, string searchFilterVal);
        void EnterpriseCategoryDescriptionTDESSaveChanges();
        void EnterpriseCategoryDescriptionTDESReload(EnterpriseCategoryDescriptionTDES item);
        void EnterpriseCategoryDescriptionTDESInsertOrUpdate(EnterpriseCategoryDescriptionTDES item);
        #endregion

        #region EnterpriseCategoryTDES
        EnterpriseCategoryTDES EnterpriseCategoryTDESInsert(EnterpriseCategoryTDES item);
        void EnterpriseCategoryTDESUpdate(EnterpriseCategoryTDES item);
        void EnterpriseCategoryTDESDelete(int? id = null, Guid? searchEntGuid=null);
        EnterpriseCategoryTDES EnterpriseCategoryTDESDetail(int? id = null, Guid? searchEntGuid = null);
        IQueryable<EnterpriseCategoryTDES> EnterpriseCategoryTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid = null);
        int EnterpriseCategoryTDESCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid);
        void EnterpriseCategoryTDESSaveChanges();
        void EnterpriseCategoryTDESReload(EnterpriseCategoryTDES item);
        void EnterpriseCategoryTDESInsertOrUpdate(EnterpriseCategoryTDES item);
        #endregion

        #region EnterpriseCategoryItemTDES
        EnterpriseCategoryItemTDES EnterpriseCategoryItemTDESInsert(EnterpriseCategoryItemTDES item);
        void EnterpriseCategoryItemTDESUpdate(EnterpriseCategoryItemTDES item);
        void EnterpriseCategoryItemTDESDelete(int? id = null, Guid? searchEntGuid = null, int? searchCategoryId = null);
        EnterpriseCategoryItemTDES EnterpriseCategoryItemTDESDetail(int? id = null, Guid? searchEntGuid = null, int? searchCategoryId = null);
        IQueryable<EnterpriseCategoryItemTDES> EnterpriseCategoryItemTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid = null, int? searchCategoryId = null);
        int EnterpriseCategoryItemTDESCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, int? searchCategoryId = null);
        void EnterpriseCategoryItemTDESSaveChanges();
        void EnterpriseCategoryItemTDESReload(EnterpriseCategoryItemTDES item);
        void EnterpriseCategoryItemTDESInsertOrUpdate(EnterpriseCategoryItemTDES item);
        #endregion

        #region EnterpriseBrandTDES
        EnterpriseBrandTDES EnterpriseBrandTDESInsert(EnterpriseBrandTDES item);
        void EnterpriseBrandTDESUpdate(EnterpriseBrandTDES item);
        void EnterpriseBrandTDESDelete(string id = null, Guid? searchEntGuid = null);
        EnterpriseBrandTDES EnterpriseBrandTDESDetail(string id = null, Guid? searchEntGuid = null);
        IQueryable<EnterpriseBrandTDES> EnterpriseBrandTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid);
        int EnterpriseBrandTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid);
        void EnterpriseBrandTDESSaveChanges();
        void EnterpriseBrandTDESReload(EnterpriseBrandTDES item);
        void EnterpriseBrandTDESInsertOrUpdate(EnterpriseBrandTDES item);
        #endregion

        #region EnterpriseArticleDescriptionTDES
        EnterpriseArticleDescriptionTDES EnterpriseArticleDescriptionTDESInsert(EnterpriseArticleDescriptionTDES item);
        void EnterpriseArticleDescriptionTDESUpdate(EnterpriseArticleDescriptionTDES item);
        void EnterpriseArticleDescriptionTDESDelete(int ? id = null);
        EnterpriseArticleDescriptionTDES EnterpriseArticleDescriptionTDESDetail(int ? id = null);
        IQueryable<EnterpriseArticleDescriptionTDES> EnterpriseArticleDescriptionTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal);
        int EnterpriseArticleDescriptionTDESIndexCount(string searchColumnVal, string searchFilterVal);
        void EnterpriseArticleDescriptionTDESSaveChanges();
        void EnterpriseArticleDescriptionTDESReload(EnterpriseArticleDescriptionTDES item);
        void EnterpriseArticleDescriptionTDESInsertOrUpdate(EnterpriseArticleDescriptionTDES item);
        
        #endregion

        #region EnterpriseArticleTDES
        EnterpriseArticleTDES EnterpriseArticleTDESInsert(EnterpriseArticleTDES item);
        void EnterpriseArticleTDESUpdate(EnterpriseArticleTDES item);
        void EnterpriseArticleTDESDelete(string EntArticle = null, string EntBrandNic = null, Guid? searchEntGuid = null);
        EnterpriseArticleTDES EnterpriseArticleTDESDetail(string EntArticle = null, string EntBrandNic=null, Guid? searchEntGuid = null);
        IQueryable<EnterpriseArticleTDES> EnterpriseArticleTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, string EntBrandNic);
        int EnterpriseArticleTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, string EntBrandNic);
        void EnterpriseArticleTDESSaveChanges();
        void EnterpriseArticleTDESReload(EnterpriseArticleTDES item);
        void EnterpriseArticleTDESInsertOrUpdate(EnterpriseArticleTDES item);
        void EnterpriseArticleTDESAdd(EnterpriseArticleTDES item, EnterpriseArticleDescriptionTDES descr);
        #endregion


        
    }
}
