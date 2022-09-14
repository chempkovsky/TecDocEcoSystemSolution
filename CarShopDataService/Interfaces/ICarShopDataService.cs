using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TecDocEcoSystemDbClassLibrary;

namespace CarShopDataService.Interfaces
{
    public interface ICarShopDataService
    {
        #region DoCreateDb
        void DoCreateDb();
        string GetConnectionStringName();
        void SetConnectionStringName(string connectionStringName);
        #endregion



        #region AddressType
        AddressType AddressTypeInsert(AddressType item);
        void AddressTypeUpdate(AddressType item);
        void AddressTypeDelete(int? id = null);
        AddressType AddressTypeDetail(int? id = null);
        IQueryable<AddressType> AddressTypeIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal);
        int  AddressTypeIndexCount();
        void AddressTypeSaveChanges();
        void AddressTypeReload(AddressType item);
        void AddressTypeInsertOrUpdate(AddressType item);
        #endregion

        #region BranchType
        BranchType BranchTypeInsert(BranchType item);
        void BranchTypeUpdate(BranchType item);
        void BranchTypeDelete(int? id = null);
        BranchType BranchTypeDetail(int? id = null);
        IQueryable<BranchType> BranchTypeIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal);
        int BranchTypeIndexCount();
        void BranchTypeSaveChanges();
        void BranchTypeReload(BranchType item);
        void BranchTypeInsertOrUpdate(BranchType item);
        #endregion

        #region ContactType
        ContactType ContactTypeInsert(ContactType item);
        void ContactTypeUpdate(ContactType item);
        void ContactTypeDelete(int? id = null);
        ContactType ContactTypeDetail(int? id = null);
        IQueryable<ContactType> ContactTypeIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal);
        int ContactTypeIndexCount();
        void ContactTypeSaveChanges();
        void ContactTypeReload(ContactType item);
        void ContactTypeInsertOrUpdate(ContactType item);
        #endregion

        #region Country
        Country CountryInsert(Country item);
        void CountryUpdate(Country item);
        void CountryDelete(int? id = null);
        Country CountryDetail(int? id = null);
        IQueryable<Country> CountryIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal);
        int CountryIndexCount();
        void CountrySaveChanges();
        void CountryReload(Country item);
        void CountryInsertOrUpdate(Country item);
        #endregion

        #region Currency
        Currency CurrencyInsert(Currency item);
        void CurrencyUpdate(Currency item);
        void CurrencyDelete(int? id = null);
        Currency CurrencyDetail(int? id = null);
        IQueryable<Currency> CurrencyIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal);
        int CurrencyIndexCount();
        void CurrencySaveChanges();
        void CurrencyReload(Currency item);
        void CurrencyInsertOrUpdate(Currency item);
        #endregion

        #region SettlementType
        SettlementType SettlementTypeInsert(SettlementType item);
        void SettlementTypeUpdate(SettlementType item);
        void SettlementTypeDelete(int? id = null);
        SettlementType SettlementTypeDetail(int? id = null);
        IQueryable<SettlementType> SettlementTypeIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal);
        int SettlementTypeIndexCount();
        void SettlementTypeSaveChanges();
        void SettlementTypeReload(SettlementType item);
        void SettlementTypeInsertOrUpdate(SettlementType item);
        #endregion

        #region StreetType
        StreetType StreetTypeInsert(StreetType item);
        void StreetTypeUpdate(StreetType item);
        void StreetTypeDelete(int? id = null);
        StreetType StreetTypeDetail(int? id = null);
        IQueryable<StreetType> StreetTypeIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal);
        int StreetTypeIndexCount();
        void StreetTypeSaveChanges();
        void StreetTypeReload(StreetType item);
        void StreetTypeInsertOrUpdate(StreetType item);
        #endregion

        #region Soato
        Soato SoatoInsert(Soato item);
        void SoatoUpdate(Soato item);
        void SoatoDelete(string id = null);
        Soato SoatoDetail(string id = null);
        IQueryable<Soato> SoatoIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal);
        int SoatoIndexCount(string searchColumnVal, string searchFilterVal);
        void SoatoSaveChanges();
        void SoatoReload(Soato item);
        void SoatoInsertOrUpdate(Soato item);
        #endregion

        #region EnterpriseTDES
        EnterpriseTDES EnterpriseTDESInsert(EnterpriseTDES item);
        void EnterpriseTDESUpdate(EnterpriseTDES item);
        void EnterpriseTDESDelete(Guid ? id = null);
        EnterpriseTDES EnterpriseTDESDetail(Guid? id = null);
        IQueryable<EnterpriseTDES> EnterpriseTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal);
        int EnterpriseTDESIndexCount(string searchColumnVal, string searchFilterVal);
        void EnterpriseTDESSaveChanges();
        void EnterpriseTDESReload(EnterpriseTDES item);
        #endregion

        #region EnterpriseUserTDES
        EnterpriseUserTDES EnterpriseUserTDESInsert(EnterpriseUserTDES item);
        void EnterpriseUserTDESUpdate(EnterpriseUserTDES item);
        void EnterpriseUserTDESDelete(string id = null);
        EnterpriseUserTDES EnterpriseUserTDESDetail(string id = null);
        IQueryable<EnterpriseUserTDES> EnterpriseUserTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid);
        int EnterpriseUserTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid);
        void EnterpriseUserTDESSaveChanges();
        void EnterpriseUserTDESReload(EnterpriseUserTDES item);
        #endregion

        #region EnterpriseBranchTDES
        EnterpriseBranchTDES EnterpriseBranchTDESInsert(EnterpriseBranchTDES item);
        void EnterpriseBranchTDESUpdate(EnterpriseBranchTDES item);
        void EnterpriseBranchTDESDelete(Guid? id = null);
        EnterpriseBranchTDES EnterpriseBranchTDESDetail(Guid? id = null);
        IQueryable<EnterpriseBranchTDES> EnterpriseBranchTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid);
        int EnterpriseBranchTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid);
        void EnterpriseBranchTDESSaveChanges();
        void EnterpriseBranchTDESReload(EnterpriseBranchTDES item);
        #endregion

        #region EnterpriseAddressTDES
        EnterpriseAddressTDES EnterpriseAddressTDESInsert(EnterpriseAddressTDES item);
        void EnterpriseAddressTDESUpdate(EnterpriseAddressTDES item);
        void EnterpriseAddressTDESDelete(Guid? id = null);
        EnterpriseAddressTDES EnterpriseAddressTDESDetail(Guid? id = null);
        IQueryable<EnterpriseAddressTDES> EnterpriseAddressTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid);
        int EnterpriseAddressTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid);
        void EnterpriseAddressTDESSaveChanges();
        void EnterpriseAddressTDESReload(EnterpriseAddressTDES item);
        #endregion
        
        #region EnterpriseUserContactTDES
        EnterpriseUserContactTDES EnterpriseUserContactTDESInsert(EnterpriseUserContactTDES item);
        void EnterpriseUserContactTDESUpdate(EnterpriseUserContactTDES item);
        void EnterpriseUserContactTDESDelete(Guid? id = null);
        EnterpriseUserContactTDES EnterpriseUserContactTDESDetail(Guid? id = null);
        IQueryable<EnterpriseUserContactTDES> EnterpriseUserContactTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, string searchEntUserNic);
        int EnterpriseUserContactTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, string searchEntUserNic);
        void EnterpriseUserContactTDESSaveChanges();
        void EnterpriseUserContactTDESReload(EnterpriseUserContactTDES item);
        #endregion

        #region EnterpriseBranchContactsTDES
        EnterpriseBranchContactsTDES EnterpriseBranchContactsTDESInsert(EnterpriseBranchContactsTDES item);
        void EnterpriseBranchContactsTDESUpdate(EnterpriseBranchContactsTDES item);
        void EnterpriseBranchContactsTDESDelete(Guid? id = null);
        EnterpriseBranchContactsTDES EnterpriseBranchContactsTDESDetail(Guid? id = null);
        IQueryable<EnterpriseBranchContactsTDES> EnterpriseBranchContactsTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid);
        int EnterpriseBranchContactsTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid);
        void EnterpriseBranchContactsTDESSaveChanges();
        void EnterpriseBranchContactsTDESReload(EnterpriseBranchContactsTDES item);
        #endregion
        
        #region EnterpriseBranchUserTDES
        EnterpriseBranchUserTDES EnterpriseBranchUserTDESInsert(EnterpriseBranchUserTDES item);
        void EnterpriseBranchUserTDESUpdate(EnterpriseBranchUserTDES item);
        void EnterpriseBranchUserTDESDelete(string id = null);
        EnterpriseBranchUserTDES EnterpriseBranchUserTDESDetail(string id = null);
        IQueryable<EnterpriseBranchUserTDES> EnterpriseBranchUserTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid);
        int EnterpriseBranchUserTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid);
        void EnterpriseBranchUserTDESSaveChanges();
        void EnterpriseBranchUserTDESReload(EnterpriseBranchUserTDES item);
        #endregion
        
        #region EnterpriseBranchUserContactTDES
        EnterpriseBranchUserContactTDES EnterpriseBranchUserContactTDESInsert(EnterpriseBranchUserContactTDES item);
        void EnterpriseBranchUserContactTDESUpdate(EnterpriseBranchUserContactTDES item);
        void EnterpriseBranchUserContactTDESDelete(Guid? id = null);
        EnterpriseBranchUserContactTDES EnterpriseBranchUserContactTDESDetail(Guid? id = null);
        IQueryable<EnterpriseBranchUserContactTDES> EnterpriseBranchUserContactTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, string searchEntUserNic, Guid? searchEntBranchGuid);
        int EnterpriseBranchUserContactTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, string searchEntUserNic, Guid? searchEntBranchGuid);
        void EnterpriseBranchUserContactTDESSaveChanges();
        void EnterpriseBranchUserContactTDESReload(EnterpriseBranchUserContactTDES item);
        #endregion

        #region EnterpriseBranchWorkPlaceTDES
        EnterpriseBranchWorkPlaceTDES EnterpriseBranchWorkPlaceTDESInsert(EnterpriseBranchWorkPlaceTDES item);
        void EnterpriseBranchWorkPlaceTDESUpdate(EnterpriseBranchWorkPlaceTDES item);
        void EnterpriseBranchWorkPlaceTDESDelete(Guid? id = null);
        EnterpriseBranchWorkPlaceTDES EnterpriseBranchWorkPlaceTDESDetail(Guid? id = null);
        IQueryable<EnterpriseBranchWorkPlaceTDES> EnterpriseBranchWorkPlaceTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid);
        int EnterpriseBranchWorkPlaceTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid);
        void EnterpriseBranchWorkPlaceTDESSaveChanges();
        void EnterpriseBranchWorkPlaceTDESReload(EnterpriseBranchWorkPlaceTDES item);
        #endregion

        #region EnterpriseProductCategoryTDES
        EnterpriseProductCategoryTDES EnterpriseProductCategoryTDESInsert(EnterpriseProductCategoryTDES item);
        void EnterpriseProductCategoryTDESUpdate(EnterpriseProductCategoryTDES item);
        void EnterpriseProductCategoryTDESDelete(int id, Guid? entBranchGuid);
        EnterpriseProductCategoryTDES EnterpriseProductCategoryTDESDetail(int id, Guid? entBranchGuid);
        IQueryable<EnterpriseProductCategoryTDES> EnterpriseProductCategoryTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid);
        int EnterpriseProductCategoryTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid);
        void EnterpriseProductCategoryTDESSaveChanges();
        void EnterpriseProductCategoryTDESReload(EnterpriseProductCategoryTDES item);
        #endregion


        #region EnterpriseTecDocSrcTypeTDES
        EnterpriseTecDocSrcTypeTDES EnterpriseTecDocSrcTypeTDESInsert(EnterpriseTecDocSrcTypeTDES item);
        void EnterpriseTecDocSrcTypeTDESUpdate(EnterpriseTecDocSrcTypeTDES item);
        IQueryable<EnterpriseTecDocSrcTypeTDES> EnterpriseTecDocSrcTypeTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal);
        void EnterpriseTecDocSrcTypeTDESInsertOrUpdate(EnterpriseTecDocSrcTypeTDES item);
        #endregion

    }
}
