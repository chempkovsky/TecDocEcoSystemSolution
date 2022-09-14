using CarShop.Models;
using CarShopDataService;
using CarShopDataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
//using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TecDocEcoSystemDbClassLibrary;

//
// dbContext.Entry(entity).State = EntityState.Detached;
//
namespace CarShopDBDataServiceModule
{
    public class CarShopDBDataService : ICarShopDataService, IDisposable
    {
        protected string connectionstringname = null;
        private CarShopContext _db = null;

        protected CarShopContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new CarShopContext(connectionstringname);
                }
                return _db;
            }
        }

        public string GetConnectionStringName()
        {
            return connectionstringname;
        }
        public void SetConnectionStringName(string connectionStringName)
        {
            if (connectionStringName == null)
            {
                if (connectionstringname != null)
                {
                    if (_db != null)
                    {
                        _db.Dispose();
                        _db = null;
                    }
                }
                connectionstringname = connectionStringName;
            }
            else
            {
                if (!connectionStringName.Equals(connectionstringname))
                {
                    if (_db != null)
                    {
                        _db.Dispose();
                        _db = null;
                    }
                }
                connectionstringname = connectionStringName;
            }
        }
        public void DoCreateDb()
        {
            System.Data.Entity.Database.SetInitializer<CarShopContext>(new CarShopContextInitializer());
            using (CarShopContext db2 = new CarShopContext(connectionstringname))
            {
                db2.Database.Initialize(true);
            } //db.Dispose();
        }


        private bool disposed = false;
        
        #region AddressType
        public AddressType AddressTypeInsert(AddressType item)
        {
            AddressType addresstype = item;
            try
            {
                addresstype = db.AddressType.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(addresstype).State = EntityState.Detached;
            }
            return addresstype;
        }
        public void AddressTypeUpdate(AddressType item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void AddressTypeDelete(int? id = null)
        {
            AddressType addresstype = db.AddressType.Find(id);
            db.AddressType.Remove(addresstype);
            db.SaveChanges();
        }
        public AddressType AddressTypeDetail(int? id = null)
        {
            AddressType addresstype = db.AddressType.Find(id);
            db.Entry(addresstype).State = EntityState.Detached;
            return addresstype;
        }
        public IQueryable<AddressType> AddressTypeIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal)
        {
            IQueryable<AddressType> result = db.AddressType.AsNoTracking();
            ascendingVal = ascendingVal ?? true;
            //Expression<Func<AddressType, object>> keySelector = null;
            //var keySelector = null;
            switch (sortColumnVal)
            {
                case "AddressTypeDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressTypeDescription);
                        else
                            result = result.OrderByDescending(v => v.AddressTypeDescription);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressTypeId);
                        else
                            result = result.OrderByDescending(v => v.AddressTypeId);
//                        var keySelector = v => v.AddressTypeId;
                        break;
                    }
            }
/*
            if (keySelector != null)
            {
                ascendingVal = ascendingVal ?? true;
                if (ascendingVal.Value)
                    result = result.OrderBy(keySelector);
                else
                    result = result.OrderByDescending(keySelector);
            }
*/
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }
            return result;
        }
        public int AddressTypeIndexCount()
        {
            return db.AddressType.Count();
        }
        public void AddressTypeSaveChanges()
        {
            db.SaveChanges();
        }
        public void AddressTypeReload(AddressType item)
        {
            db.Entry(item).Reload();
            db.Entry(item).State = EntityState.Detached;
        }
        public void AddressTypeInsertOrUpdate(AddressType item)
        {
            if (db.AddressType.Any(e => e.AddressTypeId == item.AddressTypeId))
            {
                AddressTypeUpdate(item);
            }
            else
            {
                AddressTypeInsert(item);
            }
        }
        #endregion

        #region BranchType
        public BranchType BranchTypeInsert(BranchType item)
        {
            BranchType branchtype = item;
            try
            {

                branchtype = db.BranchType.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(branchtype).State = EntityState.Detached;
            }

            return branchtype;
        }
        public void BranchTypeUpdate(BranchType item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void BranchTypeDelete(int? id = null)
        {
            BranchType branchtype = db.BranchType.Find(id);
            db.BranchType.Remove(branchtype);
            db.SaveChanges();
        }
        public BranchType BranchTypeDetail(int? id = null)
        {
            BranchType branchtype = db.BranchType.Find(id);
            db.Entry(branchtype).State = EntityState.Detached;
            return branchtype;
        }
        public IQueryable<BranchType> BranchTypeIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal)
        {
            IQueryable<BranchType> result = db.BranchType.AsNoTracking();
            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "BranchTypeDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.BranchTypeDescription);
                        else
                            result = result.OrderByDescending(v => v.BranchTypeDescription);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.BranchTypeId);
                        else
                            result = result.OrderByDescending(v => v.BranchTypeId);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }
            return result;
        }
        public int BranchTypeIndexCount()
        {
            return db.BranchType.Count();
        }
        public void BranchTypeSaveChanges()
        {
            db.SaveChanges();
        }
        public void BranchTypeReload(BranchType item)
        {
            db.Entry(item).Reload();
            db.Entry(item).State = EntityState.Detached;
        }
        public void BranchTypeInsertOrUpdate(BranchType item)
        {
            if (db.BranchType.Any(e => e.BranchTypeId == item.BranchTypeId))
            {
                BranchTypeUpdate(item);
            }
            else
            {
                BranchTypeInsert(item);
            }
        }
        #endregion

        #region ContactType
        public ContactType ContactTypeInsert(ContactType item)
        {
            ContactType contacttype = item;
            try
            {
                contacttype = db.ContactType.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(contacttype).State = EntityState.Detached;
            }
            return contacttype;
        }
        public void ContactTypeUpdate(ContactType item)
        {
            try
            {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void ContactTypeDelete(int? id = null)
        {
            ContactType contacttype = db.ContactType.Find(id);
            db.ContactType.Remove(contacttype);
            db.SaveChanges();
        }
        public ContactType ContactTypeDetail(int? id = null)
        {
            ContactType contacttype = db.ContactType.Find(id);
            db.Entry(contacttype).State = EntityState.Detached;
            return contacttype;
        }
        public IQueryable<ContactType> ContactTypeIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal)
        {
            IQueryable<ContactType> result = db.ContactType.AsNoTracking();
            ascendingVal = ascendingVal ?? true;
            //Expression<Func<AddressType, object>> keySelector = null;
            //var keySelector = null;
            switch (sortColumnVal)
            {
                case "ContactTypeDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactTypeDescription);
                        else
                            result = result.OrderByDescending(v => v.ContactTypeDescription);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactTypeId);
                        else
                            result = result.OrderByDescending(v => v.ContactTypeId);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }
            return result;
        }
        public int ContactTypeIndexCount()
        {
            return db.ContactType.Count();
        }
        public void ContactTypeSaveChanges()
        {
            db.SaveChanges();
        }
        public void ContactTypeReload(ContactType item)
        {
            db.Entry(item).Reload();
            db.Entry(item).State = EntityState.Detached;
        }
        public void ContactTypeInsertOrUpdate(ContactType item)
        {
            if (db.ContactType.Any(e => e.ContactTypeId == item.ContactTypeId))
            {
                ContactTypeUpdate(item);
            }
            else
            {
                ContactTypeInsert(item);
            }
        }

        #endregion

        #region Country
        public Country CountryInsert(Country item)
        {
            Country country = item;
            try
            {
                country = db.Country.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(country).State = EntityState.Detached;
            }
            return country;
        }
        public void CountryUpdate(Country item)
        {
            try
            {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void CountryDelete(int? id = null)
        {
            Country country = db.Country.Find(id);
            db.Country.Remove(country);
            db.SaveChanges();
        }
        public Country CountryDetail(int? id = null)
        {
            Country country = db.Country.Find(id);
            db.Entry(country).State = EntityState.Detached;
            return country;
        }
        public IQueryable<Country> CountryIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal)
        {
            IQueryable<Country> result = db.Country.AsNoTracking();
            ascendingVal = ascendingVal ?? true;
            //Expression<Func<AddressType, object>> keySelector = null;
            //var keySelector = null;
            switch (sortColumnVal)
            {
                case "CountryCapital":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CountryCapital);
                        else
                            result = result.OrderByDescending(v => v.CountryCapital);
                        break;
                    }
                case "CountryCode2":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CountryCode2);
                        else
                            result = result.OrderByDescending(v => v.CountryCode2);
                        break;
                    }
                case "CountryCode3":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CountryCode3);
                        else
                            result = result.OrderByDescending(v => v.CountryCode3);
                        break;
                    }
                case "CountryEngName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CountryEngName);
                        else
                            result = result.OrderByDescending(v => v.CountryEngName);
                        break;
                    }
                case "CountryIso":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CountryIso);
                        else
                            result = result.OrderByDescending(v => v.CountryIso);
                        break;
                    }
                case "CountryName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CountryName);
                        else
                            result = result.OrderByDescending(v => v.CountryName);
                        break;
                    }
                case "CountryPhone":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CountryPhone);
                        else
                            result = result.OrderByDescending(v => v.CountryPhone);
                        break;
                    }

                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CountryIso);
                        else
                            result = result.OrderByDescending(v => v.CountryIso);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }
            
            return result;
        }
        public int CountryIndexCount()
        {
            return db.Country.Count();
        }
        public void CountrySaveChanges()
        {
            db.SaveChanges();
        }
        public void CountryReload(Country item)
        {
            db.Entry(item).Reload();
            db.Entry(item).State = EntityState.Detached;
        }
        public void CountryInsertOrUpdate(Country item)
        {
            if (db.Country.Any(e => e.CountryIso == item.CountryIso))
            {
                CountryUpdate(item);
            }
            else
            {
                CountryInsert(item);
            }
        }
        #endregion

        #region Currency
        public Currency CurrencyInsert(Currency item)
        {
            Currency сurrency = item;
            try
            {

                сurrency = db.Currency.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(сurrency).State = EntityState.Detached;
            }
            return сurrency;
        }
        public void CurrencyUpdate(Currency item)
        {
            try
            {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void CurrencyDelete(int? id = null)
        {
            Currency сurrency = db.Currency.Find(id);
            db.Currency.Remove(сurrency);
            db.SaveChanges();
        }
        public Currency CurrencyDetail(int? id = null)
        {
            Currency сurrency = db.Currency.Find(id);
            db.Entry(сurrency).State = EntityState.Detached;
            return сurrency;
        }
        public IQueryable<Currency> CurrencyIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal)
        {
            IQueryable<Currency> result = db.Currency.AsNoTracking();
            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "CurrencyCode3":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CurrencyCode3);
                        else
                            result = result.OrderByDescending(v => v.CurrencyCode3);
                        break;
                    }
                case "CurrencyIso":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CurrencyIso);
                        else
                            result = result.OrderByDescending(v => v.CurrencyIso);
                        break;
                    }
                case "CurrencyName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CurrencyName);
                        else
                            result = result.OrderByDescending(v => v.CurrencyName);
                        break;
                    }
                case "FractionalUnit":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.FractionalUnit);
                        else
                            result = result.OrderByDescending(v => v.FractionalUnit);
                        break;
                    }
                case "FractionalUnitName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.FractionalUnitName);
                        else
                            result = result.OrderByDescending(v => v.FractionalUnitName);
                        break;
                    }
                case "IsNational":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsNational);
                        else
                            result = result.OrderByDescending(v => v.IsNational);
                        break;
                    }
                case "IsOperational":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsOperational);
                        else
                            result = result.OrderByDescending(v => v.IsOperational);
                        break;
                    }
                case "IssuerName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IssuerName);
                        else
                            result = result.OrderByDescending(v => v.IssuerName);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CurrencyIso);
                        else
                            result = result.OrderByDescending(v => v.CurrencyIso);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }
            
            return result;
        }
        public int CurrencyIndexCount()
        {
            return db.Currency.Count();
        }
        public void CurrencySaveChanges()
        {
            db.SaveChanges();
        }
        public void CurrencyReload(Currency item)
        {
            db.Entry(item).Reload();
            db.Entry(item).State = EntityState.Detached;
        }
        public void CurrencyInsertOrUpdate(Currency item)
        {
            if (db.Currency.Any(e => e.CurrencyIso == item.CurrencyIso))
            {
                CurrencyUpdate(item);
            }
            else
            {
                CurrencyInsert(item);
            }
        }
        #endregion

        #region SettlementType
        public SettlementType SettlementTypeInsert(SettlementType item)
        {
            SettlementType settlementtype = item;
            try
            {
                settlementtype = db.SettlementType.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(settlementtype).State = EntityState.Detached;
            }
            return settlementtype;
        }
        public void SettlementTypeUpdate(SettlementType item)
        {
            try
            {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void SettlementTypeDelete(int? id = null)
        {
            SettlementType settlementtype = db.SettlementType.Find(id);
            db.SettlementType.Remove(settlementtype);
            db.SaveChanges();
        }
        public SettlementType SettlementTypeDetail(int? id = null)
        {
            SettlementType settlementtype = db.SettlementType.Find(id);
            db.Entry(settlementtype).State = EntityState.Detached;
            return settlementtype;
        }
        public IQueryable<SettlementType> SettlementTypeIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal)
        {
            IQueryable<SettlementType> result = db.SettlementType.AsNoTracking();
            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "SettlementTypeDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.SettlementTypeDescription);
                        else
                            result = result.OrderByDescending(v => v.SettlementTypeDescription);
                        break;
                    }
                case "SettlementTypeShortDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.SettlementTypeShortDescription);
                        else
                            result = result.OrderByDescending(v => v.SettlementTypeShortDescription);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.SettlementTypeId);
                        else
                            result = result.OrderByDescending(v => v.SettlementTypeId);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }
            return result;
        }
        public int SettlementTypeIndexCount()
        {
            return db.SettlementType.Count();
        }
        public void SettlementTypeSaveChanges()
        {
            db.SaveChanges();
        }
        public void SettlementTypeReload(SettlementType item)
        {
            db.Entry(item).Reload();
            db.Entry(item).State = EntityState.Detached;
        }
        public void SettlementTypeInsertOrUpdate(SettlementType item)
        {
            if (db.SettlementType.Any(e => e.SettlementTypeId == item.SettlementTypeId))
            {
                SettlementTypeUpdate(item);
            }
            else
            {
                SettlementTypeInsert(item);
            }
        }
        #endregion

        #region StreetType
        public StreetType StreetTypeInsert(StreetType item)
        {
            StreetType streettype = item;
            try
            {
                streettype = db.StreetType.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(streettype).State = EntityState.Detached;
            }

            return streettype;
        }
        public void StreetTypeUpdate(StreetType item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void StreetTypeDelete(int? id = null)
        {
            StreetType streettype = db.StreetType.Find(id);
            db.StreetType.Remove(streettype);
            db.SaveChanges();
        }
        public StreetType StreetTypeDetail(int? id = null)
        {
            StreetType streettype = db.StreetType.Find(id);
            db.Entry(streettype).State = EntityState.Detached;
            return streettype;
        }
        public IQueryable<StreetType> StreetTypeIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal)
        {
            IQueryable<StreetType> result = db.StreetType.AsNoTracking();
            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "StreetTypeDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.StreetTypeDescription);
                        else
                            result = result.OrderByDescending(v => v.StreetTypeDescription);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.StreetTypeId);
                        else
                            result = result.OrderByDescending(v => v.StreetTypeId);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int StreetTypeIndexCount()
        {
            return db.StreetType.Count();
        }
        public void StreetTypeSaveChanges()
        {
            db.SaveChanges();
        }
        public void StreetTypeReload(StreetType item)
        {
            db.Entry(item).Reload();
            db.Entry(item).State = EntityState.Detached;
        }
        public void StreetTypeInsertOrUpdate(StreetType item)
        {
            if (db.StreetType.Any(e => e.StreetTypeId == item.StreetTypeId))
            {
                StreetTypeUpdate(item);
            }
            else
            {
                StreetTypeInsert(item);
            }
        }
        #endregion

        #region Soato
        public Soato SoatoInsert(Soato item)
        {
            Soato soato = item;
            try
            {
                soato = db.Soato.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(soato).State = EntityState.Detached;
            }
            return soato;
        }
        public void SoatoUpdate(Soato item)
        {
            try
            {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void SoatoDelete(string id = null)
        {
            Soato soato = db.Soato.Find(id);
            db.Soato.Remove(soato);
            db.SaveChanges();
        }
        public Soato SoatoDetail(string id = null)
        {
            Soato soato = db.Soato.Find(id);
            db.Entry(soato).State = EntityState.Detached;
            return soato;
        }
        public IQueryable<Soato> SoatoIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal)
        {
            IQueryable<Soato> result = db.Soato.AsNoTracking();

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "SoatoSettlementName":
                        {
                            result = result.Where(v => v.SoatoSettlementName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "SoatoId":
                        {
                            result = result.Where(v => v.SoatoId.StartsWith(searchFilterVal));
                            break;
                        }
                    case "SettlementTypeId":
                        {
                            int intFilter;
                            if ( int.TryParse(searchFilterVal, out intFilter) ) {
                                result = result.Where(v => v.SettlementTypeId == intFilter);
                            }
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "SoatoSettlementName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.SoatoSettlementName);
                        else
                            result = result.OrderByDescending(v => v.SoatoSettlementName);
                        break;
                    }
                case "SettlementTypeId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.SettlementTypeId);
                        else
                            result = result.OrderByDescending(v => v.SettlementTypeId);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.SoatoId);
                        else
                            result = result.OrderByDescending(v => v.SoatoId);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int SoatoIndexCount(string searchColumnVal, string searchFilterVal)
        {
            IQueryable<Soato> result = db.Soato.AsNoTracking();
            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "SoatoSettlementName":
                        {
                            result = result.Where(v => v.SoatoSettlementName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "SoatoId":
                        {
                            result = result.Where(v => v.SoatoId.StartsWith(searchFilterVal));
                            break;
                        }
                    case "SettlementTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.SettlementTypeId == intFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void SoatoSaveChanges()
        {
            db.SaveChanges();
        }
        public void SoatoReload(Soato item)
        {
            db.Entry(item).Reload();
        }
        public void SoatoInsertOrUpdate(Soato item)
        {
            if (db.Soato.Any(e => e.SoatoId == item.SoatoId))
            {
                SoatoUpdate(item);
            }
            else
            {
                SoatoInsert(item);
            }
        }
        #endregion

        #region EnterpriseTDES
        public EnterpriseTDES EnterpriseTDESInsert(EnterpriseTDES item)
        {
            EnterpriseTDES enterpriseTDES = item;
            try
            {
                enterpriseTDES = db.EnterpriseTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterpriseTDES).State = EntityState.Detached;
            }
            return enterpriseTDES;
        }
        public void EnterpriseTDESUpdate(EnterpriseTDES item)
        {
            try
            {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void EnterpriseTDESDelete(Guid ? id = null)
        {
            EnterpriseTDES enterpriseTDES = db.EnterpriseTDES.Find(id);
            db.EnterpriseTDES.Remove(enterpriseTDES);
            db.SaveChanges();
        }
        public EnterpriseTDES EnterpriseTDESDetail(Guid? id = null)
        {
            EnterpriseTDES enterpriseTDES = db.EnterpriseTDES.Find(id);
            db.Entry(enterpriseTDES).State = EntityState.Detached;
            return enterpriseTDES;
        }
        public IQueryable<EnterpriseTDES> EnterpriseTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal)
        {
            IQueryable<EnterpriseTDES> result = db.EnterpriseTDES.AsNoTracking();

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntDescription":
                        {
                            result = result.Where(v => v.EntDescription.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "ArticleCatalog":
                        {
                            result = result.Where(v => v.ArticleCatalog.StartsWith(searchFilterVal));
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
                case "EntDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntDescription);
                        else
                            result = result.OrderByDescending(v => v.EntDescription);
                        break;
                    }
                case "IsActive":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsActive);
                        else
                            result = result.OrderByDescending(v => v.IsActive);
                        break;
                    }
                case "ArticleCatalog":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ArticleCatalog);
                        else
                            result = result.OrderByDescending(v => v.ArticleCatalog);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int EnterpriseTDESIndexCount(string searchColumnVal, string searchFilterVal)
        {
            IQueryable<EnterpriseTDES> result = db.EnterpriseTDES.AsNoTracking();
            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntDescription":
                        {
                            result = result.Where(v => v.EntDescription.StartsWith(searchFilterVal));
                            break;
                        }
                    case "ArticleCatalog":
                        {
                            result = result.Where(v => v.ArticleCatalog.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseTDESReload(EnterpriseTDES item)
        {
            db.Entry(item).Reload();
        }
        #endregion

        #region EnterpriseUserTDES
        public EnterpriseUserTDES EnterpriseUserTDESInsert(EnterpriseUserTDES item)
        {
            EnterpriseUserTDES enterpriseUserTDES = item;
            try
            {
                enterpriseUserTDES = db.EnterpriseUserTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterpriseUserTDES).State = EntityState.Detached;
            }
            return enterpriseUserTDES;
        }
        public void EnterpriseUserTDESUpdate(EnterpriseUserTDES item)
        {
            try
            {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void EnterpriseUserTDESDelete(string id = null)
        {
            EnterpriseUserTDES enterpriseUserTDES = db.EnterpriseUserTDES.Find(id);
            db.EnterpriseUserTDES.Remove(enterpriseUserTDES);
            db.SaveChanges();
        }
        public EnterpriseUserTDES EnterpriseUserTDESDetail(string id = null)
        {
            EnterpriseUserTDES enterpriseUserTDES = db.EnterpriseUserTDES.Find(id);
            db.Entry(enterpriseUserTDES).State = EntityState.Detached;
            return enterpriseUserTDES;
        }
        public IQueryable<EnterpriseUserTDES> EnterpriseUserTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid ? searchEntGuid)
        {
            IQueryable<EnterpriseUserTDES> result = db.EnterpriseUserTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntUserNic":
                        {
                            result = result.Where(v => v.EntUserNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "Password":
                        {
                            result = result.Where(v => v.Password.StartsWith(searchFilterVal));
                            break;
                        }
                    case "FirstName":
                        {
                            result = result.Where(v => v.FirstName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "LastName":
                        {
                            result = result.Where(v => v.LastName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "MiddleName":
                        {
                            result = result.Where(v => v.MiddleName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsAdmin":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsAdmin == boolFilter);
                            }
                            break;
                        }
                    case "IsAudit":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsAudit == boolFilter);
                            }
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
                case "EntUserNic":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntUserNic);
                        else
                            result = result.OrderByDescending(v => v.EntUserNic);
                        break;
                    }
                case "Password":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.Password);
                        else
                            result = result.OrderByDescending(v => v.Password);
                        break;
                    }
                case "FirstName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.FirstName);
                        else
                            result = result.OrderByDescending(v => v.FirstName);
                        break;
                    }
                case "LastName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.LastName);
                        else
                            result = result.OrderByDescending(v => v.LastName);
                        break;
                    }
                case "MiddleName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.MiddleName);
                        else
                            result = result.OrderByDescending(v => v.MiddleName);
                        break;
                    }


                case "IsActive":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsActive);
                        else
                            result = result.OrderByDescending(v => v.IsActive);
                        break;
                    }
                case "IsAdmin":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsAdmin);
                        else
                            result = result.OrderByDescending(v => v.IsAdmin);
                        break;
                    }
                case "IsAudit":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsAudit);
                        else
                            result = result.OrderByDescending(v => v.IsAudit);
                        break;
                    }

                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntUserNic);
                        else
                            result = result.OrderByDescending(v => v.EntUserNic);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int EnterpriseUserTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid ? searchEntGuid)
        {
            IQueryable<EnterpriseUserTDES> result = db.EnterpriseUserTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntUserNic":
                        {
                            result = result.Where(v => v.EntUserNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "Password":
                        {
                            result = result.Where(v => v.Password.StartsWith(searchFilterVal));
                            break;
                        }
                    case "FirstName":
                        {
                            result = result.Where(v => v.FirstName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "LastName":
                        {
                            result = result.Where(v => v.LastName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "MiddleName":
                        {
                            result = result.Where(v => v.MiddleName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsAdmin":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsAdmin == boolFilter);
                            }
                            break;
                        }
                    case "IsAudit":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsAudit == boolFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseUserTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseUserTDESReload(EnterpriseUserTDES item)
        {
            db.Entry(item).Reload();
        }
        #endregion

        #region EnterpriseBranchTDES
        public EnterpriseBranchTDES EnterpriseBranchTDESInsert(EnterpriseBranchTDES item)
        {
            EnterpriseBranchTDES enterpriseBranchTDES = item;
            try
            {
                enterpriseBranchTDES = db.EnterpriseBranchTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterpriseBranchTDES).State = EntityState.Detached;
            }
            return enterpriseBranchTDES;
        }
        public void EnterpriseBranchTDESUpdate(EnterpriseBranchTDES item)
        {
            try
            {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void EnterpriseBranchTDESDelete(Guid? id = null)
        {
            EnterpriseBranchTDES enterpriseBranchTDES = db.EnterpriseBranchTDES.Find(id);
            db.EnterpriseBranchTDES.Remove(enterpriseBranchTDES);
            db.SaveChanges();
        }
        public EnterpriseBranchTDES EnterpriseBranchTDESDetail(Guid? id = null)
        {
            EnterpriseBranchTDES enterpriseBranchTDES = db.EnterpriseBranchTDES.Find(id);
            db.Entry(enterpriseBranchTDES).State = EntityState.Detached;
            return enterpriseBranchTDES;
        }
        public IQueryable<EnterpriseBranchTDES> EnterpriseBranchTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid ? searchEntGuid)
        {
            IQueryable<EnterpriseBranchTDES> result = db.EnterpriseBranchTDES.AsNoTracking();
            if (searchEntGuid.HasValue) {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchDescription":
                        {
                            result = result.Where(v => v.EntBranchDescription.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsVisible":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsVisible == boolFilter);
                            }
                            break;
                        }
                    case "BranchTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.BranchTypeId == intFilter);
                            }
                            break;
                        }
                    case "TecDocCatalog":
                        {
                            result = result.Where(v => v.TecDocCatalog.StartsWith(searchFilterVal));
                            break;
                        }
                    case "SalesCatalog":
                        {
                            result = result.Where(v => v.SalesCatalog.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IncomeCatalog":
                        {
                            result = result.Where(v => v.IncomeCatalog.StartsWith(searchFilterVal));
                            break;
                        }
                    case "OrderCatalog":
                        {
                            result = result.Where(v => v.OrderCatalog.StartsWith(searchFilterVal));
                            break;
                        }


                        
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
                case "EntBranchGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBranchGuid);
                        else
                            result = result.OrderByDescending(v => v.EntBranchGuid);
                        break;
                    }
                case "EntBranchDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBranchDescription);
                        else
                            result = result.OrderByDescending(v => v.EntBranchDescription);
                        break;
                    }
                case "TecDocCatalog":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.TecDocCatalog);
                        else
                            result = result.OrderByDescending(v => v.TecDocCatalog);
                        break;
                    }
                case "SalesCatalog":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.SalesCatalog);
                        else
                            result = result.OrderByDescending(v => v.SalesCatalog);
                        break;
                    }
                case "IncomeCatalog":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IncomeCatalog);
                        else
                            result = result.OrderByDescending(v => v.IncomeCatalog);
                        break;
                    }
                case "OrderCatalog":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.OrderCatalog);
                        else
                            result = result.OrderByDescending(v => v.OrderCatalog);
                        break;
                    }
                    

                case "IsActive":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsActive);
                        else
                            result = result.OrderByDescending(v => v.IsActive);
                        break;
                    }
                case "IsVisible":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsVisible);
                        else
                            result = result.OrderByDescending(v => v.IsVisible);
                        break;
                    }
                case "BranchTypeId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.BranchTypeId);
                        else
                            result = result.OrderByDescending(v => v.BranchTypeId);
                        break;
                    }


                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBranchGuid);
                        else
                            result = result.OrderByDescending(v => v.EntBranchGuid);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int EnterpriseBranchTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid ? searchEntGuid)
        {
            IQueryable<EnterpriseBranchTDES> result = db.EnterpriseBranchTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchDescription":
                        {
                            result = result.Where(v => v.EntBranchDescription.StartsWith(searchFilterVal));
                            break;
                        }
                    case "TecDocCatalog":
                        {
                            result = result.Where(v => v.TecDocCatalog.StartsWith(searchFilterVal));
                            break;
                        }
                    case "SalesCatalog":
                        {
                            result = result.Where(v => v.SalesCatalog.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IncomeCatalog":
                        {
                            result = result.Where(v => v.IncomeCatalog.StartsWith(searchFilterVal));
                            break;
                        }
                    case "OrderCatalog":
                        {
                            result = result.Where(v => v.OrderCatalog.StartsWith(searchFilterVal));
                            break;
                        }
                        

                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsVisible":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsVisible == boolFilter);
                            }
                            break;
                        }
                    case "BranchTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.BranchTypeId == intFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseBranchTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseBranchTDESReload(EnterpriseBranchTDES item)
        {
            db.Entry(item).Reload();
        }
        #endregion

        #region EnterpriseAddressTDES
        public EnterpriseAddressTDES EnterpriseAddressTDESInsert(EnterpriseAddressTDES item)
        {
            EnterpriseAddressTDES enterpriseAddressTDES = item;
            try
            {
                enterpriseAddressTDES = db.EnterpriseAddressTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterpriseAddressTDES).State = EntityState.Detached;
            }
            return enterpriseAddressTDES;
        }
        public void EnterpriseAddressTDESUpdate(EnterpriseAddressTDES item)
        {
            try
            {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void EnterpriseAddressTDESDelete(Guid? id = null)
        {
            EnterpriseAddressTDES enterpriseAddressTDES = db.EnterpriseAddressTDES.Find(id);
            db.EnterpriseAddressTDES.Remove(enterpriseAddressTDES);
            db.SaveChanges();
        }
        public EnterpriseAddressTDES EnterpriseAddressTDESDetail(Guid? id = null)
        {
            EnterpriseAddressTDES enterpriseAddressTDES = db.EnterpriseAddressTDES.Find(id);
            db.Entry(enterpriseAddressTDES).State = EntityState.Detached;
            return enterpriseAddressTDES;
        }
        public IQueryable<EnterpriseAddressTDES> EnterpriseAddressTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid ? searchEntGuid)
        {
            IQueryable<EnterpriseAddressTDES> result = db.EnterpriseAddressTDES.AsNoTracking();
            if (searchEntGuid.HasValue) {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "AddressGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.AddressGuid == guidFilter);
                            }
                            break;
                        }
                    case "AddressTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.AddressTypeId == intFilter);
                            }
                            break;
                        }
                    case "CountryIso":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.CountryIso == intFilter);
                            }
                            break;
                        }
                    case "AddressRegion":
                        {
                            result = result.Where(v => v.AddressRegion.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressDistrict":
                        {
                            result = result.Where(v => v.AddressDistrict.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressRural":
                        {
                            result = result.Where(v => v.AddressRural.StartsWith(searchFilterVal));
                            break;
                        }
                    case "SettlementTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.SettlementTypeId == intFilter);
                            }
                            break;
                        }
                    case "AddressSettlementName":
                        {
                            result = result.Where(v => v.AddressSettlementName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "SoatoId":
                        {
                            result = result.Where(v => v.SoatoId.StartsWith(searchFilterVal));
                            break;
                        }
                    case "StreetTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.StreetTypeId == intFilter);
                            }
                            break;
                        }
                    case "AddressStreetName":
                        {
                            result = result.Where(v => v.AddressStreetName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressHouse":
                        {
                            result = result.Where(v => v.AddressHouse.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressBuilding":
                        {
                            result = result.Where(v => v.AddressBuilding.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressOffice":
                        {
                            result = result.Where(v => v.AddressOffice.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressPostCode":
                        {
                            result = result.Where(v => v.AddressPostCode.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressValidFrom":
                        {
                            DateTime dateFilter;
                            if (DateTime.TryParse(searchFilterVal, out dateFilter))
                            {
                                result = result.Where(v => v.AddressValidFrom == dateFilter);
                            }
                            break;
                        }
                    case "AddressValidTo":
                        {
                            DateTime dateFilter;
                            if (DateTime.TryParse(searchFilterVal, out dateFilter))
                            {
                                result = result.Where(v => v.AddressValidTo == dateFilter);
                            }
                            break;
                        }


                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsVisible":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsVisible == boolFilter);
                            }
                            break;
                        }
                    case "AddressLongitude":
                        {
                            double doubleFilter;
                            if (double.TryParse(searchFilterVal, out doubleFilter))
                            {
                                result = result.Where(v => v.AddressLongitude == doubleFilter);
                            }
                            break;
                        }
                    case "AddressLatitude":
                        {
                            double doubleFilter;
                            if (double.TryParse(searchFilterVal, out doubleFilter))
                            {
                                result = result.Where(v => v.AddressLatitude == doubleFilter);
                            }
                            break;
                        }
                    default:
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.AddressGuid == guidFilter);
                            }
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
                case "AddressGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressGuid);
                        else
                            result = result.OrderByDescending(v => v.AddressGuid);
                        break;
                    }
                case "AddressTypeId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressTypeId);
                        else
                            result = result.OrderByDescending(v => v.AddressTypeId);
                        break;
                    }
                case "CountryIso":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CountryIso);
                        else
                            result = result.OrderByDescending(v => v.CountryIso);
                        break;
                    }
                case "AddressRegion":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressRegion);
                        else
                            result = result.OrderByDescending(v => v.AddressRegion);
                        break;
                    }
                case "AddressDistrict":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressDistrict);
                        else
                            result = result.OrderByDescending(v => v.AddressDistrict);
                        break;
                    }
                case "AddressRural":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressRural);
                        else
                            result = result.OrderByDescending(v => v.AddressRural);
                        break;
                    }
                case "SettlementTypeId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.SettlementTypeId);
                        else
                            result = result.OrderByDescending(v => v.SettlementTypeId);
                        break;
                    }
                case "AddressSettlementName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressSettlementName);
                        else
                            result = result.OrderByDescending(v => v.AddressSettlementName);
                        break;
                    }
                case "SoatoId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.SoatoId);
                        else
                            result = result.OrderByDescending(v => v.SoatoId);
                        break;
                    }
                case "StreetTypeId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.StreetTypeId);
                        else
                            result = result.OrderByDescending(v => v.StreetTypeId);
                        break;
                    }
                case "AddressStreetName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressStreetName);
                        else
                            result = result.OrderByDescending(v => v.AddressStreetName);
                        break;
                    }
                case "AddressHouse":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressHouse);
                        else
                            result = result.OrderByDescending(v => v.AddressHouse);
                        break;
                    }
                case "AddressBuilding":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressBuilding);
                        else
                            result = result.OrderByDescending(v => v.AddressBuilding);
                        break;
                    }
                case "AddressOffice":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressOffice);
                        else
                            result = result.OrderByDescending(v => v.AddressOffice);
                        break;
                    }
                case "AddressPostCode":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressPostCode);
                        else
                            result = result.OrderByDescending(v => v.AddressPostCode);
                        break;
                    }
                case "AddressValidFrom":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressValidFrom);
                        else
                            result = result.OrderByDescending(v => v.AddressValidFrom);
                        break;
                    }
                case "AddressValidTo":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressValidTo);
                        else
                            result = result.OrderByDescending(v => v.AddressValidTo);
                        break;
                    }
                case "IsActive":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsActive);
                        else
                            result = result.OrderByDescending(v => v.IsActive);
                        break;
                    }
                case "IsVisible":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsVisible);
                        else
                            result = result.OrderByDescending(v => v.IsVisible);
                        break;
                    }
                case "AddressLongitude":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressLongitude);
                        else
                            result = result.OrderByDescending(v => v.AddressLongitude);
                        break;
                    }
                case "AddressLatitude":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressLatitude);
                        else
                            result = result.OrderByDescending(v => v.AddressLatitude);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.AddressGuid);
                        else
                            result = result.OrderByDescending(v => v.AddressGuid);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int EnterpriseAddressTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid ? searchEntGuid)
        {
            IQueryable<EnterpriseAddressTDES> result = db.EnterpriseAddressTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "AddressGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.AddressGuid == guidFilter);
                            }
                            break;
                        }
                    case "AddressTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.AddressTypeId == intFilter);
                            }
                            break;
                        }
                    case "CountryIso":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.CountryIso == intFilter);
                            }
                            break;
                        }
                    case "AddressRegion":
                        {
                            result = result.Where(v => v.AddressRegion.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressDistrict":
                        {
                            result = result.Where(v => v.AddressDistrict.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressRural":
                        {
                            result = result.Where(v => v.AddressRural.StartsWith(searchFilterVal));
                            break;
                        }
                    case "SettlementTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.SettlementTypeId == intFilter);
                            }
                            break;
                        }
                    case "AddressSettlementName":
                        {
                            result = result.Where(v => v.AddressSettlementName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "SoatoId":
                        {
                            result = result.Where(v => v.SoatoId.StartsWith(searchFilterVal));
                            break;
                        }
                    case "StreetTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.StreetTypeId == intFilter);
                            }
                            break;
                        }
                    case "AddressStreetName":
                        {
                            result = result.Where(v => v.AddressStreetName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressHouse":
                        {
                            result = result.Where(v => v.AddressHouse.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressBuilding":
                        {
                            result = result.Where(v => v.AddressBuilding.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressOffice":
                        {
                            result = result.Where(v => v.AddressOffice.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressPostCode":
                        {
                            result = result.Where(v => v.AddressPostCode.StartsWith(searchFilterVal));
                            break;
                        }
                    case "AddressValidFrom":
                        {
                            DateTime dateFilter;
                            if (DateTime.TryParse(searchFilterVal, out dateFilter))
                            {
                                result = result.Where(v => v.AddressValidFrom == dateFilter);
                            }
                            break;
                        }
                    case "AddressValidTo":
                        {
                            DateTime dateFilter;
                            if (DateTime.TryParse(searchFilterVal, out dateFilter))
                            {
                                result = result.Where(v => v.AddressValidTo == dateFilter);
                            }
                            break;
                        }


                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsVisible":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsVisible == boolFilter);
                            }
                            break;
                        }
                    case "AddressLongitude":
                        {
                            double doubleFilter;
                            if (double.TryParse(searchFilterVal, out doubleFilter))
                            {
                                result = result.Where(v => v.AddressLongitude == doubleFilter);
                            }
                            break;
                        }
                    case "AddressLatitude":
                        {
                            double doubleFilter;
                            if (double.TryParse(searchFilterVal, out doubleFilter))
                            {
                                result = result.Where(v => v.AddressLatitude == doubleFilter);
                            }
                            break;
                        }
                    default:
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.AddressGuid == guidFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseAddressTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseAddressTDESReload(EnterpriseAddressTDES item)
        {
            db.Entry(item).Reload();
        }
        #endregion

        #region EnterpriseUserContactTDES
        public EnterpriseUserContactTDES EnterpriseUserContactTDESInsert(EnterpriseUserContactTDES item)
        {
            EnterpriseUserContactTDES enterpriseUserContactTDES = item;
            try
            {
                enterpriseUserContactTDES = db.EnterpriseUserContactTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterpriseUserContactTDES).State = EntityState.Detached;
            }
            return enterpriseUserContactTDES;
        }
        public void EnterpriseUserContactTDESUpdate(EnterpriseUserContactTDES item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void EnterpriseUserContactTDESDelete(Guid? id = null)
        {
            EnterpriseUserContactTDES enterpriseUserContactTDES = db.EnterpriseUserContactTDES.Find(id);
            db.EnterpriseUserContactTDES.Remove(enterpriseUserContactTDES);
            db.SaveChanges();
        }
        public EnterpriseUserContactTDES EnterpriseUserContactTDESDetail(Guid? id = null)
        {
            EnterpriseUserContactTDES enterpriseUserContactTDES = db.EnterpriseUserContactTDES.Find(id);
            db.Entry(enterpriseUserContactTDES).State = EntityState.Detached;
            return enterpriseUserContactTDES;
        }
        public IQueryable<EnterpriseUserContactTDES> EnterpriseUserContactTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid ? searchEntGuid, string searchEntUserNic)
        {
            IQueryable<EnterpriseUserContactTDES> result = db.EnterpriseUserContactTDES.AsNoTracking();
            if (searchEntGuid.HasValue) {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (!string.IsNullOrEmpty(searchEntUserNic))
            {
                result = result.Where(v => v.EntUserNic == searchEntUserNic);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "ContactGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntUserNic":
                        {
                            result = result.Where(v => v.EntUserNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "Contact":
                        {
                            result = result.Where(v => v.Contact.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsVisible":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsVisible == boolFilter);
                            }
                            break;
                        }
                    case "Description":
                        {
                            result = result.Where(v => v.Description.StartsWith(searchFilterVal));
                            break;
                        }

                    case "ContactTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.ContactTypeId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid==guidFilter);
                            }
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
                case "EntUserNic":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntUserNic);
                        else
                            result = result.OrderByDescending(v => v.EntUserNic);
                        break;
                    }
                case "ContactGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactGuid);
                        else
                            result = result.OrderByDescending(v => v.ContactGuid);
                        break;
                    }
                case "Contact":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.Contact);
                        else
                            result = result.OrderByDescending(v => v.Contact);
                        break;
                    }
                case "IsActive":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsActive);
                        else
                            result = result.OrderByDescending(v => v.IsActive);
                        break;
                    }
                case "IsVisible":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsVisible);
                        else
                            result = result.OrderByDescending(v => v.IsVisible);
                        break;
                    }
                case "Description":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.Description);
                        else
                            result = result.OrderByDescending(v => v.Description);
                        break;
                    }

                case "ContactTypeId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactTypeId);
                        else
                            result = result.OrderByDescending(v => v.ContactTypeId);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactGuid);
                        else
                            result = result.OrderByDescending(v => v.ContactGuid);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int EnterpriseUserContactTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid ? searchEntGuid, string searchEntUserNic)
        {
            IQueryable<EnterpriseUserContactTDES> result = db.EnterpriseUserContactTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (!string.IsNullOrEmpty(searchEntUserNic))
            {
                result = result.Where(v => v.EntUserNic == searchEntUserNic);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "ContactGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntUserNic":
                        {
                            result = result.Where(v => v.EntUserNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "Contact":
                        {
                            result = result.Where(v => v.Contact.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsVisible":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsVisible == boolFilter);
                            }
                            break;
                        }
                    case "Description":
                        {
                            result = result.Where(v => v.Description.StartsWith(searchFilterVal));
                            break;
                        }

                    case "ContactTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.ContactTypeId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseUserContactTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseUserContactTDESReload(EnterpriseUserContactTDES item)
        {
            db.Entry(item).Reload();
        }
        #endregion

        #region EnterpriseBranchContactsTDES
        public EnterpriseBranchContactsTDES EnterpriseBranchContactsTDESInsert(EnterpriseBranchContactsTDES item)
        {
            EnterpriseBranchContactsTDES enterpriseBranchContactsTDES = item;
            try
            {
                enterpriseBranchContactsTDES = db.EnterpriseBranchContactsTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterpriseBranchContactsTDES).State = EntityState.Detached;
            }
            return enterpriseBranchContactsTDES;
        }
        public void EnterpriseBranchContactsTDESUpdate(EnterpriseBranchContactsTDES item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void EnterpriseBranchContactsTDESDelete(Guid? id = null)
        {
            EnterpriseBranchContactsTDES enterpriseBranchContactsTDES = db.EnterpriseBranchContactsTDES.Find(id);
            db.EnterpriseBranchContactsTDES.Remove(enterpriseBranchContactsTDES);
            db.SaveChanges();
        }
        public EnterpriseBranchContactsTDES EnterpriseBranchContactsTDESDetail(Guid? id = null)
        {
            EnterpriseBranchContactsTDES enterpriseBranchContactsTDES = db.EnterpriseBranchContactsTDES.Find(id);
            db.Entry(enterpriseBranchContactsTDES).State = EntityState.Detached;
            return enterpriseBranchContactsTDES;
        }
        public IQueryable<EnterpriseBranchContactsTDES> EnterpriseBranchContactsTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            IQueryable<EnterpriseBranchContactsTDES> result = db.EnterpriseBranchContactsTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (searchEntBranchGuid.HasValue)
            {
                result = result.Where(v => v.EntBranchGuid == searchEntBranchGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "ContactGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "Contact":
                        {
                            result = result.Where(v => v.Contact.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsVisible":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsVisible == boolFilter);
                            }
                            break;
                        }
                    case "Description":
                        {
                            result = result.Where(v => v.Description.StartsWith(searchFilterVal));
                            break;
                        }

                    case "ContactTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.ContactTypeId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
                case "EntBranchGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBranchGuid);
                        else
                            result = result.OrderByDescending(v => v.EntBranchGuid);
                        break;
                    }
                case "ContactGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactGuid);
                        else
                            result = result.OrderByDescending(v => v.ContactGuid);
                        break;
                    }
                case "Contact":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.Contact);
                        else
                            result = result.OrderByDescending(v => v.Contact);
                        break;
                    }
                case "IsActive":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsActive);
                        else
                            result = result.OrderByDescending(v => v.IsActive);
                        break;
                    }
                case "IsVisible":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsVisible);
                        else
                            result = result.OrderByDescending(v => v.IsVisible);
                        break;
                    }
                case "Description":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.Description);
                        else
                            result = result.OrderByDescending(v => v.Description);
                        break;
                    }

                case "ContactTypeId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactTypeId);
                        else
                            result = result.OrderByDescending(v => v.ContactTypeId);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactGuid);
                        else
                            result = result.OrderByDescending(v => v.ContactGuid);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int EnterpriseBranchContactsTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            IQueryable<EnterpriseBranchContactsTDES> result = db.EnterpriseBranchContactsTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (searchEntBranchGuid.HasValue)
            {
                result = result.Where(v => v.EntBranchGuid == searchEntBranchGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "ContactGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "Contact":
                        {
                            result = result.Where(v => v.Contact.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsVisible":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsVisible == boolFilter);
                            }
                            break;
                        }
                    case "Description":
                        {
                            result = result.Where(v => v.Description.StartsWith(searchFilterVal));
                            break;
                        }

                    case "ContactTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.ContactTypeId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseBranchContactsTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseBranchContactsTDESReload(EnterpriseBranchContactsTDES item)
        {
            db.Entry(item).Reload();
        }
        #endregion

        #region EnterpriseBranchUserTDES
        public EnterpriseBranchUserTDES EnterpriseBranchUserTDESInsert(EnterpriseBranchUserTDES item)
        {
            EnterpriseBranchUserTDES enterpriseBranchUserTDES = item;
            try
            {
                enterpriseBranchUserTDES = db.EnterpriseBranchUserTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterpriseBranchUserTDES).State = EntityState.Detached;
            }
            return enterpriseBranchUserTDES;
        }
        public void EnterpriseBranchUserTDESUpdate(EnterpriseBranchUserTDES item)
        {
            try
            {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void EnterpriseBranchUserTDESDelete(string id = null)
        {
            EnterpriseBranchUserTDES enterpriseBranchUserTDES = db.EnterpriseBranchUserTDES.Find(id);
            db.EnterpriseBranchUserTDES.Remove(enterpriseBranchUserTDES);
            db.SaveChanges();
        }
        public EnterpriseBranchUserTDES EnterpriseBranchUserTDESDetail(string id = null)
        {
            EnterpriseBranchUserTDES enterpriseBranchUserTDES = db.EnterpriseBranchUserTDES.Find(id);
            db.Entry(enterpriseBranchUserTDES).State = EntityState.Detached;
            return enterpriseBranchUserTDES;
        }
        public IQueryable<EnterpriseBranchUserTDES> EnterpriseBranchUserTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            IQueryable<EnterpriseBranchUserTDES> result = db.EnterpriseBranchUserTDES.AsNoTracking();
            if (searchEntGuid.HasValue) {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (searchEntBranchGuid.HasValue)
            {
                result = result.Where(v => v.EntBranchGuid == searchEntBranchGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntUserNic":
                        {
                            result = result.Where(v => v.EntUserNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "Password":
                        {
                            result = result.Where(v => v.Password.StartsWith(searchFilterVal));
                            break;
                        }
                    case "FirstName":
                        {
                            result = result.Where(v => v.FirstName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "LastName":
                        {
                            result = result.Where(v => v.LastName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "MiddleName":
                        {
                            result = result.Where(v => v.MiddleName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsAdmin":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsAdmin == boolFilter);
                            }
                            break;
                        }
                    case "IsAudit":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsAudit == boolFilter);
                            }
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
                case "EntBranchGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBranchGuid);
                        else
                            result = result.OrderByDescending(v => v.EntBranchGuid);
                        break;
                    }
                case "EntUserNic":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntUserNic);
                        else
                            result = result.OrderByDescending(v => v.EntUserNic);
                        break;
                    }
                case "Password":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.Password);
                        else
                            result = result.OrderByDescending(v => v.Password);
                        break;
                    }
                case "FirstName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.FirstName);
                        else
                            result = result.OrderByDescending(v => v.FirstName);
                        break;
                    }
                case "LastName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.LastName);
                        else
                            result = result.OrderByDescending(v => v.LastName);
                        break;
                    }
                case "MiddleName":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.MiddleName);
                        else
                            result = result.OrderByDescending(v => v.MiddleName);
                        break;
                    }


                case "IsActive":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsActive);
                        else
                            result = result.OrderByDescending(v => v.IsActive);
                        break;
                    }
                case "IsAdmin":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsAdmin);
                        else
                            result = result.OrderByDescending(v => v.IsAdmin);
                        break;
                    }
                case "IsAudit":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsAudit);
                        else
                            result = result.OrderByDescending(v => v.IsAudit);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntUserNic);
                        else
                            result = result.OrderByDescending(v => v.EntUserNic);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int EnterpriseBranchUserTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid ? searchEntBranchGuid)
        {
            IQueryable<EnterpriseBranchUserTDES> result = db.EnterpriseBranchUserTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (searchEntBranchGuid.HasValue)
            {
                result = result.Where(v => v.EntBranchGuid == searchEntBranchGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntUserNic":
                        {
                            result = result.Where(v => v.EntUserNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "Password":
                        {
                            result = result.Where(v => v.Password.StartsWith(searchFilterVal));
                            break;
                        }
                    case "FirstName":
                        {
                            result = result.Where(v => v.FirstName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "LastName":
                        {
                            result = result.Where(v => v.LastName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "MiddleName":
                        {
                            result = result.Where(v => v.MiddleName.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsAdmin":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsAdmin == boolFilter);
                            }
                            break;
                        }
                    case "IsAudit":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsAudit == boolFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseBranchUserTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseBranchUserTDESReload(EnterpriseBranchUserTDES item)
        {
            db.Entry(item).Reload();
        }
        #endregion

        #region EnterpriseBranchUserContactTDES
        public EnterpriseBranchUserContactTDES EnterpriseBranchUserContactTDESInsert(EnterpriseBranchUserContactTDES item)
        {
            EnterpriseBranchUserContactTDES enterpriseBranchUserContactTDES = item;
            try
            {
                enterpriseBranchUserContactTDES = db.EnterpriseBranchUserContactTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterpriseBranchUserContactTDES).State = EntityState.Detached;
            }
            return enterpriseBranchUserContactTDES;
        }
        public void EnterpriseBranchUserContactTDESUpdate(EnterpriseBranchUserContactTDES item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void EnterpriseBranchUserContactTDESDelete(Guid? id = null)
        {
            EnterpriseBranchUserContactTDES enterpriseBranchUserContactTDES = db.EnterpriseBranchUserContactTDES.Find(id);
            db.EnterpriseBranchUserContactTDES.Remove(enterpriseBranchUserContactTDES);
            db.SaveChanges();
        }
        public EnterpriseBranchUserContactTDES EnterpriseBranchUserContactTDESDetail(Guid? id = null)
        {
            EnterpriseBranchUserContactTDES enterpriseBranchUserContactTDES = db.EnterpriseBranchUserContactTDES.Find(id);
            db.Entry(enterpriseBranchUserContactTDES).State = EntityState.Detached;
            return enterpriseBranchUserContactTDES;
        }
        public IQueryable<EnterpriseBranchUserContactTDES> EnterpriseBranchUserContactTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, string searchEntUserNic, Guid? searchEntBranchGuid)
        {
            IQueryable<EnterpriseBranchUserContactTDES> result = db.EnterpriseBranchUserContactTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (!string.IsNullOrEmpty(searchEntUserNic))
            {
                result = result.Where(v => v.EntUserNic == searchEntUserNic);
            }
            if (searchEntBranchGuid.HasValue)
            {
                result = result.Where(v => v.EntBranchGuid == searchEntBranchGuid.Value);
            }
            

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "ContactGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntUserNic":
                        {
                            result = result.Where(v => v.EntUserNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "Contact":
                        {
                            result = result.Where(v => v.Contact.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsVisible":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsVisible == boolFilter);
                            }
                            break;
                        }
                    case "Description":
                        {
                            result = result.Where(v => v.Description.StartsWith(searchFilterVal));
                            break;
                        }

                    case "ContactTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.ContactTypeId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
                case "EntBranchGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBranchGuid);
                        else
                            result = result.OrderByDescending(v => v.EntBranchGuid);
                        break;
                    }
                case "EntUserNic":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntUserNic);
                        else
                            result = result.OrderByDescending(v => v.EntUserNic);
                        break;
                    }
                case "ContactGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactGuid);
                        else
                            result = result.OrderByDescending(v => v.ContactGuid);
                        break;
                    }
                case "Contact":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.Contact);
                        else
                            result = result.OrderByDescending(v => v.Contact);
                        break;
                    }
                case "IsActive":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsActive);
                        else
                            result = result.OrderByDescending(v => v.IsActive);
                        break;
                    }
                case "IsVisible":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsVisible);
                        else
                            result = result.OrderByDescending(v => v.IsVisible);
                        break;
                    }
                case "Description":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.Description);
                        else
                            result = result.OrderByDescending(v => v.Description);
                        break;
                    }

                case "ContactTypeId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactTypeId);
                        else
                            result = result.OrderByDescending(v => v.ContactTypeId);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ContactGuid);
                        else
                            result = result.OrderByDescending(v => v.ContactGuid);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int EnterpriseBranchUserContactTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, string searchEntUserNic, Guid? searchEntBranchGuid)
        {
            IQueryable<EnterpriseBranchUserContactTDES> result = db.EnterpriseBranchUserContactTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (!string.IsNullOrEmpty(searchEntUserNic))
            {
                result = result.Where(v => v.EntUserNic == searchEntUserNic);
            }
            if (searchEntBranchGuid.HasValue) 
            {
                result = result.Where(v => v.EntBranchGuid == searchEntBranchGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "ContactGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntUserNic":
                        {
                            result = result.Where(v => v.EntUserNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "Contact":
                        {
                            result = result.Where(v => v.Contact.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }
                    case "IsVisible":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsVisible == boolFilter);
                            }
                            break;
                        }
                    case "Description":
                        {
                            result = result.Where(v => v.Description.StartsWith(searchFilterVal));
                            break;
                        }

                    case "ContactTypeId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.ContactTypeId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.ContactGuid == guidFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseBranchUserContactTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseBranchUserContactTDESReload(EnterpriseBranchUserContactTDES item)
        {
            db.Entry(item).Reload();
        }
        #endregion

        #region EnterpriseBranchWorkPlaceTDES
        public EnterpriseBranchWorkPlaceTDES EnterpriseBranchWorkPlaceTDESInsert(EnterpriseBranchWorkPlaceTDES item)
        {
            EnterpriseBranchWorkPlaceTDES enterpriseBranchWorkPlaceTDES = item;
            try
            {
                enterpriseBranchWorkPlaceTDES = db.EnterpriseBranchWorkPlaceTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterpriseBranchWorkPlaceTDES).State = EntityState.Detached;
            }
            return enterpriseBranchWorkPlaceTDES;
        }
        public void EnterpriseBranchWorkPlaceTDESUpdate(EnterpriseBranchWorkPlaceTDES item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void EnterpriseBranchWorkPlaceTDESDelete(Guid? id = null)
        {
            EnterpriseBranchWorkPlaceTDES enterpriseBranchWorkPlaceTDES = db.EnterpriseBranchWorkPlaceTDES.Find(id);
            db.EnterpriseBranchWorkPlaceTDES.Remove(enterpriseBranchWorkPlaceTDES);
            db.SaveChanges();
        }
        public EnterpriseBranchWorkPlaceTDES EnterpriseBranchWorkPlaceTDESDetail(Guid? id = null)
        {
            EnterpriseBranchWorkPlaceTDES enterpriseBranchWorkPlaceTDES = db.EnterpriseBranchWorkPlaceTDES.Find(id);
            db.Entry(enterpriseBranchWorkPlaceTDES).State = EntityState.Detached;
            return enterpriseBranchWorkPlaceTDES;
        }
        public IQueryable<EnterpriseBranchWorkPlaceTDES> EnterpriseBranchWorkPlaceTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            IQueryable<EnterpriseBranchWorkPlaceTDES> result = db.EnterpriseBranchWorkPlaceTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (searchEntBranchGuid.HasValue)
            {
                result = result.Where(v => v.EntBranchGuid == searchEntBranchGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "WorkPlaceGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.WorkPlaceGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                            result = result.Where(v => v.EntBranchGuid==guidFilter);
                            }
                            break;
                        }
                    case "Description":
                        {
                            result = result.Where(v => v.Description.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }

                    default:
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.WorkPlaceGuid == guidFilter);
                            }
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
                case "EntBranchGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBranchGuid);
                        else
                            result = result.OrderByDescending(v => v.EntBranchGuid);
                        break;
                    }
                case "WorkPlaceGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.WorkPlaceGuid);
                        else
                            result = result.OrderByDescending(v => v.WorkPlaceGuid);
                        break;
                    }
                case "IsActive":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.IsActive);
                        else
                            result = result.OrderByDescending(v => v.IsActive);
                        break;
                    }
                case "Description":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.Description);
                        else
                            result = result.OrderByDescending(v => v.Description);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.WorkPlaceGuid);
                        else
                            result = result.OrderByDescending(v => v.WorkPlaceGuid);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int EnterpriseBranchWorkPlaceTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            IQueryable<EnterpriseBranchWorkPlaceTDES> result = db.EnterpriseBranchWorkPlaceTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (searchEntBranchGuid.HasValue)
            {
                result = result.Where(v => v.EntBranchGuid == searchEntBranchGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "WorkPlaceGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.WorkPlaceGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "Description":
                        {
                            result = result.Where(v => v.Description.StartsWith(searchFilterVal));
                            break;
                        }
                    case "IsActive":
                        {
                            bool boolFilter;
                            if (bool.TryParse(searchFilterVal, out boolFilter))
                            {
                                result = result.Where(v => v.IsActive == boolFilter);
                            }
                            break;
                        }

                    default:
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.WorkPlaceGuid == guidFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseBranchWorkPlaceTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseBranchWorkPlaceTDESReload(EnterpriseBranchWorkPlaceTDES item)
        {
            db.Entry(item).Reload();
        }
        #endregion

        #region EnterpriseProductCategoryTDES
        public EnterpriseProductCategoryTDES EnterpriseProductCategoryTDESInsert(EnterpriseProductCategoryTDES item)
        {
            EnterpriseProductCategoryTDES enterpriseProductCategoryTDES = item;
            try
            {
                enterpriseProductCategoryTDES = db.EnterpriseProductCategoryTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterpriseProductCategoryTDES).State = EntityState.Detached;
            }
            return enterpriseProductCategoryTDES;
        }
        public void EnterpriseProductCategoryTDESUpdate(EnterpriseProductCategoryTDES item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public void EnterpriseProductCategoryTDESDelete(int id, Guid? entBranchGuid)
        {

            EnterpriseProductCategoryTDES enterpriseProductCategoryTDES = 
                (from e in db.EnterpriseProductCategoryTDES
                 where e.PCId == id && e.EntBranchGuid == entBranchGuid select e).First();
            db.EnterpriseProductCategoryTDES.Remove(enterpriseProductCategoryTDES);
            db.SaveChanges();

        }
        public EnterpriseProductCategoryTDES EnterpriseProductCategoryTDESDetail(int id, Guid? entBranchGuid)
        {
            EnterpriseProductCategoryTDES enterpriseProductCategoryTDES =
                (from e in db.EnterpriseProductCategoryTDES.AsNoTracking()
                 where e.PCId == id && e.EntBranchGuid == entBranchGuid
                 select e).First();
            // db.Entry(enterpriseProductCategoryTDES).State = EntityState.Detached; // since  AsNoTracking()
            return enterpriseProductCategoryTDES;
        }
        public IQueryable<EnterpriseProductCategoryTDES> EnterpriseProductCategoryTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            IQueryable<EnterpriseProductCategoryTDES> result = db.EnterpriseProductCategoryTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (searchEntBranchGuid.HasValue)
            {
                result = result.Where(v => v.EntBranchGuid == searchEntBranchGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "PCId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.PCId == intFilter);
                            }
                            break;
                        }
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "PCDescription":
                        {
                            result = result.Where(v => v.PCDescription.StartsWith(searchFilterVal));
                            break;
                        }
                    default:
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.PCId == intFilter);
                            }
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntGuid);
                        else
                            result = result.OrderByDescending(v => v.EntGuid);
                        break;
                    }
                case "EntBranchGuid":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBranchGuid);
                        else
                            result = result.OrderByDescending(v => v.EntBranchGuid);
                        break;
                    }
                case "PCId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.PCId);
                        else
                            result = result.OrderByDescending(v => v.PCId);
                        break;
                    }
                case "PCDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.PCDescription);
                        else
                            result = result.OrderByDescending(v => v.PCDescription);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.PCId);
                        else
                            result = result.OrderByDescending(v => v.PCId);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }

            return result;
        }
        public int EnterpriseProductCategoryTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            IQueryable<EnterpriseProductCategoryTDES> result = db.EnterpriseProductCategoryTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (searchEntBranchGuid.HasValue)
            {
                result = result.Where(v => v.EntBranchGuid == searchEntBranchGuid.Value);
            }

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "PCId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.PCId == intFilter);
                            }
                            break;
                        }
                    case "EntGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntGuid == guidFilter);
                            }
                            break;
                        }
                    case "EntBranchGuid":
                        {
                            Guid guidFilter;
                            if (Guid.TryParse(searchFilterVal, out guidFilter))
                            {
                                result = result.Where(v => v.EntBranchGuid == guidFilter);
                            }
                            break;
                        }
                    case "PCDescription":
                        {
                            result = result.Where(v => v.PCDescription.StartsWith(searchFilterVal));
                            break;
                        }
                    default:
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.PCId == intFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseProductCategoryTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseProductCategoryTDESReload(EnterpriseProductCategoryTDES item)
        {
            db.Entry(item).Reload();
        }
        #endregion

        #region EnterpriseTecDocSrcTypeTDES
        public EnterpriseTecDocSrcTypeTDES EnterpriseTecDocSrcTypeTDESInsert(EnterpriseTecDocSrcTypeTDES item)
        {
            EnterpriseTecDocSrcTypeTDES enterprisetecdocSrcTypeTDES = item;
            try
            {
                enterprisetecdocSrcTypeTDES = db.EnterpriseTecDocSrcTypeTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisetecdocSrcTypeTDES).State = EntityState.Detached;
            }
            return enterprisetecdocSrcTypeTDES;
        }

        public void EnterpriseTecDocSrcTypeTDESUpdate(EnterpriseTecDocSrcTypeTDES item)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
            }
        }
        public IQueryable<EnterpriseTecDocSrcTypeTDES> EnterpriseTecDocSrcTypeTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal)
        {
            IQueryable<EnterpriseTecDocSrcTypeTDES> result = db.EnterpriseTecDocSrcTypeTDES.AsNoTracking();
            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "TecDocSrcTypeDescr":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.TecDocSrcTypeDescr);
                        else
                            result = result.OrderByDescending(v => v.TecDocSrcTypeDescr);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.TecDocSrcTypeId);
                        else
                            result = result.OrderByDescending(v => v.TecDocSrcTypeId);
                        break;
                    }
            }
            if (startVal.HasValue)
            {
                if (startVal.Value > 0)
                {
                    int doskip = startVal.Value;
                    result = result.Skip(doskip);
                }
            }
            if (itemCountVal.HasValue)
            {
                if (itemCountVal.Value > 0)
                {
                    int doTake = itemCountVal.Value;
                    result = result.Take(doTake);
                }
            }
            return result;
        }
        public void EnterpriseTecDocSrcTypeTDESInsertOrUpdate(EnterpriseTecDocSrcTypeTDES item)
        {
            if (db.EnterpriseTecDocSrcTypeTDES.Any(e => e.TecDocSrcTypeId == item.TecDocSrcTypeId))
            {
                EnterpriseTecDocSrcTypeTDESUpdate(item);
            }
            else
            {
                EnterpriseTecDocSrcTypeTDESInsert(item);
            }
        }
        #endregion


        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    db.Dispose();
                }
                // Note disposing has been done.
                disposed = true;
            }
        }
        #endregion

        ~CarShopDBDataService()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
            Dispose(false);
        }

    }
}
