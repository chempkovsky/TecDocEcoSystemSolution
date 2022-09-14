using CarShop.Models;
using CarShopDataService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecDocEcoSystemDbClassLibrary;
using CarShopDataService.Interfaces;
using System.Data.Entity;

namespace CarShopDBArticleDataService
{
    class CarShopArticleDBDataService : ICarShopArticleDataService
    {
        protected string connectionstringname = null;
        protected CarShopArticleContext _db = null;
        protected CarShopArticleContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new CarShopArticleContext(connectionstringname);
                }
                return _db;
            }
        }

        #region DoCreateDb
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
            System.Data.Entity.Database.SetInitializer<CarShopArticleContext>(new CarShopArticleContextInitializer());
            using (CarShopArticleContext db2 = new CarShopArticleContext(connectionstringname))
            {
                db2.Database.Initialize(true);
            } //db.Dispose();
        }

        #endregion

        #region EnterpriseCategoryItemDescriptionTDES
        public EnterpriseCategoryItemDescriptionTDES EnterpriseCategoryItemDescriptionTDESInsert(EnterpriseCategoryItemDescriptionTDES item)
        {
            EnterpriseCategoryItemDescriptionTDES enterprisecategoryitemdescriptiontdes = null;
            try
            {
                enterprisecategoryitemdescriptiontdes = (from e in db.EnterpriseCategoryItemDescriptionTDES
                                                         where e.EntCategoryItemDescription == item.EntCategoryItemDescription
                                                         select e).FirstOrDefault();
                if (enterprisecategoryitemdescriptiontdes == null)
                {
                    enterprisecategoryitemdescriptiontdes = new EnterpriseCategoryItemDescriptionTDES()
                    {
                        EntCategoryItemDescription = item.EntCategoryItemDescription
                    };
                    db.EnterpriseCategoryItemDescriptionTDES.Add(enterprisecategoryitemdescriptiontdes);
                    db.SaveChanges();
                }
            }
            finally
            {
                if (enterprisecategoryitemdescriptiontdes != null)
                {
                    db.Entry(enterprisecategoryitemdescriptiontdes).State = EntityState.Detached;
                }
            }
            return enterprisecategoryitemdescriptiontdes;
        }
        public void EnterpriseCategoryItemDescriptionTDESUpdate(EnterpriseCategoryItemDescriptionTDES item)
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
        public void EnterpriseCategoryItemDescriptionTDESDelete(int? id = null)
        {
            EnterpriseCategoryItemDescriptionTDES
            enterprisecategoryitemdescriptiontdes = (from e in db.EnterpriseCategoryItemDescriptionTDES
                                                     where e.EntCategoryItemDescriptionId == id
                                                     select e).FirstOrDefault();
            if (enterprisecategoryitemdescriptiontdes != null)
            {
                db.EnterpriseCategoryItemDescriptionTDES.Remove(enterprisecategoryitemdescriptiontdes);
                db.SaveChanges();
            }
        }
        public EnterpriseCategoryItemDescriptionTDES EnterpriseCategoryItemDescriptionTDESDetail(int? id = null)
        {
            EnterpriseCategoryItemDescriptionTDES enterprisecategoryitemdescriptiontdes = null;
            try
            {
                enterprisecategoryitemdescriptiontdes = (from e in db.EnterpriseCategoryItemDescriptionTDES
                                                         where e.EntCategoryItemDescriptionId == id
                                                         select e).FirstOrDefault();
            }
            finally
            {
                if (enterprisecategoryitemdescriptiontdes != null)
                    db.Entry(enterprisecategoryitemdescriptiontdes).State = EntityState.Detached;
            }

            return enterprisecategoryitemdescriptiontdes;
        }
        public IQueryable<EnterpriseCategoryItemDescriptionTDES> EnterpriseCategoryItemDescriptionTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal)
        {
            IQueryable<EnterpriseCategoryItemDescriptionTDES> result = db.EnterpriseCategoryItemDescriptionTDES.AsNoTracking();

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntCategoryItemDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntCategoryItemDescriptionId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            result = result.Where(v => v.EntCategoryItemDescription.StartsWith(searchFilterVal));
                            break;
                        }
                }
            }


            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntCategoryItemDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntCategoryItemDescription);
                        else
                            result = result.OrderByDescending(v => v.EntCategoryItemDescription);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntCategoryItemDescriptionId);
                        else
                            result = result.OrderByDescending(v => v.EntCategoryItemDescriptionId);
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
        public int EnterpriseCategoryItemDescriptionTDESCount(string searchColumnVal, string searchFilterVal)
        {
            IQueryable<EnterpriseCategoryItemDescriptionTDES> result = db.EnterpriseCategoryItemDescriptionTDES.AsNoTracking();
            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntCategoryItemDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntCategoryItemDescriptionId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            result = result.Where(v => v.EntCategoryItemDescription.StartsWith(searchFilterVal));
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseCategoryItemDescriptionTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseCategoryItemDescriptionTDESReload(EnterpriseCategoryItemDescriptionTDES item)
        {
            db.Entry(item).Reload();
        }
        public void EnterpriseCategoryItemDescriptionTDESInsertOrUpdate(EnterpriseCategoryItemDescriptionTDES item)
        {
            if (db.EnterpriseCategoryItemDescriptionTDES.AsNoTracking().Any(e => e.EntCategoryItemDescriptionId == item.EntCategoryItemDescriptionId))
            {
                EnterpriseCategoryItemDescriptionTDESUpdate(item);
            }
            else
            {
                EnterpriseCategoryItemDescriptionTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseCategoryDescriptionTDES
        public EnterpriseCategoryDescriptionTDES EnterpriseCategoryDescriptionTDESInsert(EnterpriseCategoryDescriptionTDES item)
        {
            EnterpriseCategoryDescriptionTDES enterprisecategorydescriptiontdes = null;
            try
            {
                enterprisecategorydescriptiontdes = (from e in db.EnterpriseCategoryDescriptionTDES
                                                     where e.EntCategoryDescription == item.EntCategoryDescription
                                                     select e).FirstOrDefault();
                if (enterprisecategorydescriptiontdes == null)
                {
                    enterprisecategorydescriptiontdes = new EnterpriseCategoryDescriptionTDES()
                    {
                        EntCategoryDescription = item.EntCategoryDescription
                    };
                    db.EnterpriseCategoryDescriptionTDES.Add(enterprisecategorydescriptiontdes);
                    db.SaveChanges();
                }
            }
            finally
            {
                if (enterprisecategorydescriptiontdes != null)
                {
                    db.Entry(enterprisecategorydescriptiontdes).State = EntityState.Detached;
                }
            }
            return enterprisecategorydescriptiontdes;

        }
        public void EnterpriseCategoryDescriptionTDESUpdate(EnterpriseCategoryDescriptionTDES item)
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
        public void EnterpriseCategoryDescriptionTDESDelete(int? id = null)
        {
            EnterpriseCategoryDescriptionTDES
            enterprisecategorydescriptiontdes = (from e in db.EnterpriseCategoryDescriptionTDES
                                                     where e.EntCategoryDescriptionId == id
                                                     select e).FirstOrDefault();
            if (enterprisecategorydescriptiontdes != null)
            {
                db.EnterpriseCategoryDescriptionTDES.Remove(enterprisecategorydescriptiontdes);
                db.SaveChanges();
            }
        }
        public EnterpriseCategoryDescriptionTDES EnterpriseCategoryDescriptionTDESDetail(int? id = null)
        {
            EnterpriseCategoryDescriptionTDES enterprisecategorydescriptiontdes = null;
            try
            {
                enterprisecategorydescriptiontdes = (from e in db.EnterpriseCategoryDescriptionTDES
                                                     where e.EntCategoryDescriptionId == id
                                                     select e).FirstOrDefault();
            }
            finally
            {
                if (enterprisecategorydescriptiontdes != null)
                    db.Entry(enterprisecategorydescriptiontdes).State = EntityState.Detached;
            }
            return enterprisecategorydescriptiontdes;
        }
        public IQueryable<EnterpriseCategoryDescriptionTDES> EnterpriseCategoryDescriptionTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal)
        {
            IQueryable<EnterpriseCategoryDescriptionTDES> result = db.EnterpriseCategoryDescriptionTDES.AsNoTracking();
            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntCategoryDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntCategoryDescriptionId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            result = result.Where(v => v.EntCategoryDescription.StartsWith(searchFilterVal));
                            break;
                        }
                }
            }

            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntCategoryDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntCategoryDescription);
                        else
                            result = result.OrderByDescending(v => v.EntCategoryDescription);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntCategoryDescriptionId);
                        else
                            result = result.OrderByDescending(v => v.EntCategoryDescriptionId);
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
        public int EnterpriseCategoryDescriptionTDESCount(string searchColumnVal, string searchFilterVal)
        {
            IQueryable<EnterpriseCategoryDescriptionTDES> result = db.EnterpriseCategoryDescriptionTDES.AsNoTracking();
            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntCategoryDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntCategoryDescriptionId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            result = result.Where(v => v.EntCategoryDescription.StartsWith(searchFilterVal));
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseCategoryDescriptionTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseCategoryDescriptionTDESReload(EnterpriseCategoryDescriptionTDES item)
        {
            db.Entry(item).Reload();
        }
        public void EnterpriseCategoryDescriptionTDESInsertOrUpdate(EnterpriseCategoryDescriptionTDES item)
        {
            if (db.EnterpriseCategoryDescriptionTDES.AsNoTracking().Any(e => e.EntCategoryDescriptionId == item.EntCategoryDescriptionId))
            {
                EnterpriseCategoryDescriptionTDESUpdate(item);
            }
            else
            {
                EnterpriseCategoryDescriptionTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseCategoryTDES
        public EnterpriseCategoryTDES EnterpriseCategoryTDESInsert(EnterpriseCategoryTDES item)
        {
            EnterpriseCategoryTDES enterprisecategorytdes = item;
            try
            {
                enterprisecategorytdes = db.EnterpriseCategoryTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisecategorytdes).State = EntityState.Detached;
            }
            return enterprisecategorytdes;
        }
        public void EnterpriseCategoryTDESUpdate(EnterpriseCategoryTDES item)
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
        public void EnterpriseCategoryTDESDelete(int? id = null, Guid? searchEntGuid = null)
        {
            EnterpriseCategoryTDES enterprisecategorytdes =
                (from e in db.EnterpriseCategoryTDES
                 where (e.CategoryId == id) && (e.EntGuid == searchEntGuid)
                 select e).FirstOrDefault();
            db.EnterpriseCategoryTDES.Remove(enterprisecategorytdes);
            db.SaveChanges();
        }
        public EnterpriseCategoryTDES EnterpriseCategoryTDESDetail(int? id = null, Guid? searchEntGuid = null)
        {
            EnterpriseCategoryTDES enterprisecategorytdes =
                (from e in db.EnterpriseCategoryTDES.AsNoTracking()
                 where (e.CategoryId == id) && (e.EntGuid == searchEntGuid)
                 select e).FirstOrDefault();
            return enterprisecategorytdes;
        }
        public IQueryable<EnterpriseCategoryTDES> EnterpriseCategoryTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid = null)
        {
            IQueryable<EnterpriseCategoryTDES> result = db.EnterpriseCategoryTDES.AsNoTracking();
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
                    case "CategoryId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.CategoryId == intFilter);
                            }
                            break;
                        }
                    case "CategoryDescription":
                        {
                            result = result.Where(v => v.CategoryDescription.StartsWith(searchFilterVal));
                            break;
                        }
                    case "CategoryParent":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.CategoryParent == intFilter);
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
                case "CategoryDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CategoryDescription);
                        else
                            result = result.OrderByDescending(v => v.CategoryDescription);
                        break;
                    }
                case "CategoryId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CategoryId);
                        else
                            result = result.OrderByDescending(v => v.CategoryId);
                        break;
                    }
                case "CategoryParent":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CategoryParent);
                        else
                            result = result.OrderByDescending(v => v.CategoryParent);
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
        public int EnterpriseCategoryTDESCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid)
        {
            IQueryable<EnterpriseCategoryTDES> result = db.EnterpriseCategoryTDES.AsNoTracking();
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
                    case "CategoryId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.CategoryId == intFilter);
                            }
                            break;
                        }
                    case "CategoryDescription":
                        {
                            result = result.Where(v => v.CategoryDescription.StartsWith(searchFilterVal));
                            break;
                        }
                    case "CategoryParent":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.CategoryParent == intFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseCategoryTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseCategoryTDESReload(EnterpriseCategoryTDES item)
        {
            db.Entry(item).Reload();
        }
        public void EnterpriseCategoryTDESInsertOrUpdate(EnterpriseCategoryTDES item)
        {
            if (db.EnterpriseCategoryTDES.AsNoTracking().Any(e => (e.CategoryId == item.CategoryId) && (e.EntGuid == item.EntGuid)))
            {
                EnterpriseCategoryTDESUpdate(item);
            }
            else
            {
                EnterpriseCategoryTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseCategoryItemTDES
        public EnterpriseCategoryItemTDES EnterpriseCategoryItemTDESInsert(EnterpriseCategoryItemTDES item)
        {
            EnterpriseCategoryItemDescriptionTDES 
                enterprisecategoryitemdescriptiontdes = (from e in db.EnterpriseCategoryItemDescriptionTDES
                                                         where e.EntCategoryItemDescriptionId == item.EntCategoryItemDescriptionId
                                                         select e).FirstOrDefault();
            EnterpriseCategoryDescriptionTDES 
                enterprisecategorydescriptiontdes = (from e in db.EnterpriseCategoryDescriptionTDES
                                                     where e.EntCategoryDescriptionId == item.EntCategoryDescriptionId
                                                     select e).FirstOrDefault();
            EnterpriseCategoryTDES enterprisecategorytdes =
                (from e in db.EnterpriseCategoryTDES
                 where (e.CategoryId == item.CategoryId) && (e.EntGuid == item.EntGuid)
                 select e).FirstOrDefault();



            EnterpriseCategoryItemTDES enterprisecategoryitemtdes = item;
            try
            {
                enterprisecategoryitemtdes = db.EnterpriseCategoryItemTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisecategoryitemtdes).State = EntityState.Detached;
                db.Entry(enterprisecategoryitemdescriptiontdes).State = EntityState.Detached;
                db.Entry(enterprisecategorydescriptiontdes).State = EntityState.Detached;
                db.Entry(enterprisecategorytdes).State = EntityState.Detached;
            }
            return enterprisecategoryitemtdes;

        }
        public void EnterpriseCategoryItemTDESUpdate(EnterpriseCategoryItemTDES item)
        {
            EnterpriseCategoryItemDescriptionTDES
                enterprisecategoryitemdescriptiontdes = (from e in db.EnterpriseCategoryItemDescriptionTDES
                                                         where e.EntCategoryItemDescriptionId == item.EntCategoryItemDescriptionId
                                                         select e).FirstOrDefault();
            EnterpriseCategoryDescriptionTDES
                enterprisecategorydescriptiontdes = (from e in db.EnterpriseCategoryDescriptionTDES
                                                     where e.EntCategoryDescriptionId == item.EntCategoryDescriptionId
                                                     select e).FirstOrDefault();
            EnterpriseCategoryTDES enterprisecategorytdes =
                (from e in db.EnterpriseCategoryTDES
                 where (e.CategoryId == item.CategoryId) && (e.EntGuid == item.EntGuid)
                 select e).FirstOrDefault();

            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
                db.Entry(enterprisecategoryitemdescriptiontdes).State = EntityState.Detached;
                db.Entry(enterprisecategorydescriptiontdes).State = EntityState.Detached;
                db.Entry(enterprisecategorytdes).State = EntityState.Detached;
            }
        }
        public void EnterpriseCategoryItemTDESDelete(int? id = null, Guid? searchEntGuid = null, int? searchCategoryId = null)
        {
            EnterpriseCategoryItemTDES enterprisecategoryitemtdes =
                (from e in db.EnterpriseCategoryItemTDES
                 where (e.EntGuid == searchEntGuid) && (e.CategoryItemId == id) && (e.CategoryId == searchCategoryId)
                 select e).First();
            db.EnterpriseCategoryItemTDES.Remove(enterprisecategoryitemtdes);
            db.SaveChanges();
        }
        public EnterpriseCategoryItemTDES EnterpriseCategoryItemTDESDetail(int? id = null, Guid? searchEntGuid = null, int? searchCategoryId = null)
        {
            EnterpriseCategoryItemTDES enterprisecategoryitemtdes =
                (from e in db.EnterpriseCategoryItemTDES.AsNoTracking()
                 where (e.EntGuid == searchEntGuid) && (e.CategoryItemId == id) && (e.CategoryId == searchCategoryId)
                 select e).First();
            return enterprisecategoryitemtdes;
        }
        public IQueryable<EnterpriseCategoryItemTDES> EnterpriseCategoryItemTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid = null, int? searchCategoryId = null)
        {
            IQueryable<EnterpriseCategoryItemTDES> result = db.EnterpriseCategoryItemTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (searchCategoryId.HasValue)
            {
                result = result.Where(v => v.CategoryId == searchCategoryId.Value);
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
                    case "CategoryId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.CategoryId == intFilter);
                            }
                            break;
                        }
                    case "CategoryItemId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.CategoryItemId == intFilter);
                            }
                            break;
                        }
                    case "EntCategoryItemDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntCategoryItemDescriptionId == intFilter);
                            }
                            break;
                        }
                    case "EntCategoryDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntCategoryDescriptionId == intFilter);
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
                case "CategoryId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CategoryId);
                        else
                            result = result.OrderByDescending(v => v.CategoryId);
                        break;
                    }
                case "CategoryItemId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.CategoryItemId);
                        else
                            result = result.OrderByDescending(v => v.CategoryItemId);
                        break;
                    }
                case "EntCategoryItemDescriptionId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntCategoryItemDescriptionId);
                        else
                            result = result.OrderByDescending(v => v.EntCategoryItemDescriptionId);
                        break;
                    }
                case "EntCategoryDescriptionId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntCategoryDescriptionId);
                        else
                            result = result.OrderByDescending(v => v.EntCategoryDescriptionId);
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
        public int EnterpriseCategoryItemTDESCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, int? searchCategoryId = null)
        {
            IQueryable<EnterpriseCategoryItemTDES> result = db.EnterpriseCategoryItemTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (searchCategoryId.HasValue)
            {
                result = result.Where(v => v.CategoryId == searchCategoryId.Value);
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
                    case "CategoryId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.CategoryId == intFilter);
                            }
                            break;
                        }
                    case "CategoryItemId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.CategoryItemId == intFilter);
                            }
                            break;
                        }
                    case "EntCategoryItemDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntCategoryItemDescriptionId == intFilter);
                            }
                            break;
                        }
                    case "EntCategoryDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntCategoryDescriptionId == intFilter);
                            }
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseCategoryItemTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseCategoryItemTDESReload(EnterpriseCategoryItemTDES item)
        {
            db.Entry(item).Reload();
        }
        public void EnterpriseCategoryItemTDESInsertOrUpdate(EnterpriseCategoryItemTDES item)
        {
            if (db.EnterpriseCategoryItemTDES.AsNoTracking().Any(e => (e.CategoryId == item.CategoryId) && (e.EntGuid == item.EntGuid) && (e.CategoryItemId == item.CategoryItemId)))
            {
                EnterpriseCategoryItemTDESUpdate(item);
            }
            else
            {
                EnterpriseCategoryItemTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseBrandTDES
        public EnterpriseBrandTDES EnterpriseBrandTDESInsert(EnterpriseBrandTDES item)
        {
            EnterpriseBrandTDES enterprisebrandtdes = item;
            try
            {
                enterprisebrandtdes = db.EnterpriseBrandTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisebrandtdes).State = EntityState.Detached;
            }
            return enterprisebrandtdes;
        }
        public void EnterpriseBrandTDESUpdate(EnterpriseBrandTDES item)
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
        public void EnterpriseBrandTDESDelete(string id = null, Guid? searchEntGuid = null)
        {
            EnterpriseBrandTDES enterprisebrandtdes =
                (from e in db.EnterpriseBrandTDES
                 where e.EntBrandNic == id && e.EntGuid == searchEntGuid
                 select e).First();
            db.EnterpriseBrandTDES.Remove(enterprisebrandtdes);
            db.SaveChanges();
        }
        public EnterpriseBrandTDES EnterpriseBrandTDESDetail(string id = null, Guid? searchEntGuid = null)
        {
            EnterpriseBrandTDES enterprisebrandtdes =
                (from e in db.EnterpriseBrandTDES.AsNoTracking()
                 where e.EntBrandNic == id && e.EntGuid == searchEntGuid
                 select e).First();
            // db.Entry(enterprisebrandtdes).State = EntityState.Detached;  -- .AsNoTracking()
            return enterprisebrandtdes;
        }
        public IQueryable<EnterpriseBrandTDES> EnterpriseBrandTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid)
        {
            IQueryable<EnterpriseBrandTDES> result = db.EnterpriseBrandTDES.AsNoTracking();
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
                    case "EntBrandNic":
                        {
                            result = result.Where(v => v.EntBrandNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "EntBrandDescription":
                        {
                            result = result.Where(v => v.EntBrandDescription.StartsWith(searchFilterVal));
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
                case "EntBrandNic":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBrandNic);
                        else
                            result = result.OrderByDescending(v => v.EntBrandNic);
                        break;
                    }
                case "EntBrandDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBrandDescription);
                        else
                            result = result.OrderByDescending(v => v.EntBrandDescription);
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
        public int EnterpriseBrandTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid)
        {
            IQueryable<EnterpriseBrandTDES> result = db.EnterpriseBrandTDES.AsNoTracking();
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
                    case "EntBrandNic":
                        {
                            result = result.Where(v => v.EntBrandNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "EntBrandDescription":
                        {
                            result = result.Where(v => v.EntBrandDescription.StartsWith(searchFilterVal));
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseBrandTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseBrandTDESReload(EnterpriseBrandTDES item)
        {
            db.Entry(item).Reload();
        }
        public void EnterpriseBrandTDESInsertOrUpdate(EnterpriseBrandTDES item)
        {
            if (db.EnterpriseBrandTDES.AsNoTracking().Any(e => (e.EntBrandNic == item.EntBrandNic) && (e.EntGuid == item.EntGuid)))
            {
                EnterpriseBrandTDESUpdate(item);
            }
            else
            {
                EnterpriseBrandTDESInsert(item);
            }

        }
        #endregion

        #region EnterpriseArticleDescriptionTDES
        public EnterpriseArticleDescriptionTDES EnterpriseArticleDescriptionTDESInsert(EnterpriseArticleDescriptionTDES item)
        {
            EnterpriseArticleDescriptionTDES enterprisearticledescriptiontdes = null;
            try
            {
                enterprisearticledescriptiontdes = (from e in db.EnterpriseArticleDescriptionTDES
                                                    where e.EntArticleDescription == item.EntArticleDescription
                                                    select e).FirstOrDefault();
                if (enterprisearticledescriptiontdes == null)
                {
                    enterprisearticledescriptiontdes = new EnterpriseArticleDescriptionTDES()
                    {
                        EntArticleDescription = item.EntArticleDescription
                    };
                    db.EnterpriseArticleDescriptionTDES.Add(enterprisearticledescriptiontdes);
                    db.SaveChanges();
                }
            }
            finally
            {
                if (enterprisearticledescriptiontdes != null)
                {
                    db.Entry(enterprisearticledescriptiontdes).State = EntityState.Detached;
                }
            }
            return enterprisearticledescriptiontdes;
        }
        public void EnterpriseArticleDescriptionTDESUpdate(EnterpriseArticleDescriptionTDES item)
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
        public void EnterpriseArticleDescriptionTDESDelete(int? id = null)
        {
            EnterpriseArticleDescriptionTDES
            enterprisearticledescriptiontdes = (from e in db.EnterpriseArticleDescriptionTDES
                                                where e.EntArticleDescriptionId == id
                                                select e).FirstOrDefault();
            if (enterprisearticledescriptiontdes != null)
            {
                db.EnterpriseArticleDescriptionTDES.Remove(enterprisearticledescriptiontdes);
                db.SaveChanges();
            }
        }
        public EnterpriseArticleDescriptionTDES EnterpriseArticleDescriptionTDESDetail(int? id = null)
        {
            EnterpriseArticleDescriptionTDES enterprisearticledescriptiontdes = null;
            try
            {
                enterprisearticledescriptiontdes = (from e in db.EnterpriseArticleDescriptionTDES
                                                    where e.EntArticleDescriptionId == id
                                                    select e).FirstOrDefault();
            }
            finally
            {
                if (enterprisearticledescriptiontdes != null)
                    db.Entry(enterprisearticledescriptiontdes).State = EntityState.Detached;
            }

            return enterprisearticledescriptiontdes;
        }
        public IQueryable<EnterpriseArticleDescriptionTDES> EnterpriseArticleDescriptionTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal)
        {
            IQueryable<EnterpriseArticleDescriptionTDES> result = db.EnterpriseArticleDescriptionTDES.AsNoTracking();

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntArticleDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntArticleDescriptionId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            result = result.Where(v => v.EntArticleDescription.StartsWith(searchFilterVal));
                            break;
                        }
                }
            }


            ascendingVal = ascendingVal ?? true;
            switch (sortColumnVal)
            {
                case "EntArticleDescription":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntArticleDescription);
                        else
                            result = result.OrderByDescending(v => v.EntArticleDescription);
                        break;
                    }
                default:
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntArticleDescriptionId);
                        else
                            result = result.OrderByDescending(v => v.EntArticleDescriptionId);
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
        public int EnterpriseArticleDescriptionTDESIndexCount(string searchColumnVal, string searchFilterVal)
        {
            IQueryable<EnterpriseArticleDescriptionTDES> result = db.EnterpriseArticleDescriptionTDES.AsNoTracking();

            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntArticleDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntArticleDescriptionId == intFilter);
                            }
                            break;
                        }
                    default:
                        {
                            result = result.Where(v => v.EntArticleDescription.StartsWith(searchFilterVal));
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseArticleDescriptionTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseArticleDescriptionTDESReload(EnterpriseArticleDescriptionTDES item)
        {
            db.Entry(item).Reload();
        }
        public void EnterpriseArticleDescriptionTDESInsertOrUpdate(EnterpriseArticleDescriptionTDES item)
        {
            if (db.EnterpriseArticleDescriptionTDES.AsNoTracking().Any(e => e.EntArticleDescriptionId == item.EntArticleDescriptionId))
            {
                EnterpriseArticleDescriptionTDESUpdate(item);
            }
            else
            {
                EnterpriseArticleDescriptionTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseArticleTDES
        public EnterpriseArticleTDES EnterpriseArticleTDESInsert(EnterpriseArticleTDES item)
        {
            EnterpriseArticleTDES enterprisearticletdes = item;
            try
            {
                enterprisearticletdes = db.EnterpriseArticleTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisearticletdes).State = EntityState.Detached;
            }
            return enterprisearticletdes;
        }
        public void EnterpriseArticleTDESUpdate(EnterpriseArticleTDES item)
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
        public void EnterpriseArticleTDESDelete(string EntArticle = null, string EntBrandNic = null, Guid? searchEntGuid = null)
        {
            EnterpriseArticleTDES enterprisearticletdes =
                (from e in db.EnterpriseArticleTDES
                 where e.EntBrandNic == EntBrandNic && e.EntGuid == searchEntGuid && (e.EntArticle == EntArticle)
                 select e).First();
            db.EnterpriseArticleTDES.Remove(enterprisearticletdes);
            db.SaveChanges();
        }
        public EnterpriseArticleTDES EnterpriseArticleTDESDetail(string EntArticle = null, string EntBrandNic = null, Guid? searchEntGuid = null)
        {
            EnterpriseArticleTDES enterprisearticletdes =
                (from e in db.EnterpriseArticleTDES.AsNoTracking()
                 where e.EntBrandNic == EntBrandNic && e.EntGuid == searchEntGuid && (e.EntArticle == EntArticle)
                 select e).First();
            return enterprisearticletdes;
        }
        public IQueryable<EnterpriseArticleTDES> EnterpriseArticleTDESIndex(int? startVal, int? itemCountVal, string sortColumnVal, bool? ascendingVal, string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, string EntBrandNic)
        {
            IQueryable<EnterpriseArticleTDES> result = db.EnterpriseArticleTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (!string.IsNullOrEmpty(EntBrandNic))
            {
                result = result.Where(v => v.EntBrandNic == EntBrandNic);
            }
            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntArticle":
                        {
                            result = result.Where(v => v.EntArticle.StartsWith(searchFilterVal));
                            break;
                        }
                    case "EntBrandNic":
                        {
                            result = result.Where(v => v.EntBrandNic.StartsWith(searchFilterVal));
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
                    case "EntArticleDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntArticleDescriptionId == intFilter);
                            }
                            break;
                        }
                    case "ExternArticle":
                        {
                            result = result.Where(v => v.ExternArticle.StartsWith(searchFilterVal));
                            break;
                        }
                    case "ExternBrandNic":
                        {
                            result = result.Where(v => v.ExternBrandNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "ExternArticleEAN":
                        {
                            result = result.Where(v => v.ExternArticleEAN.StartsWith(searchFilterVal));
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
                case "EntArticle":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntArticle);
                        else
                            result = result.OrderByDescending(v => v.EntArticle);
                        break;
                    }
                case "EntBrandNic":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntBrandNic);
                        else
                            result = result.OrderByDescending(v => v.EntBrandNic);
                        break;
                    }
                case "EntArticleDescriptionId":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.EntArticleDescriptionId);
                        else
                            result = result.OrderByDescending(v => v.EntArticleDescriptionId);
                        break;
                    }
                case "ExternArticle":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ExternArticle);
                        else
                            result = result.OrderByDescending(v => v.ExternArticle);
                        break;
                    }
                case "ExternBrandNic":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ExternBrandNic);
                        else
                            result = result.OrderByDescending(v => v.ExternBrandNic);
                        break;
                    }
                case "ExternArticleEAN":
                    {
                        if (ascendingVal.Value)
                            result = result.OrderBy(v => v.ExternArticleEAN);
                        else
                            result = result.OrderByDescending(v => v.ExternArticleEAN);
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
        public int EnterpriseArticleTDESIndexCount(string searchColumnVal, string searchFilterVal, Guid? searchEntGuid, string EntBrandNic)
        {
            IQueryable<EnterpriseArticleTDES> result = db.EnterpriseArticleTDES.AsNoTracking();
            if (searchEntGuid.HasValue)
            {
                result = result.Where(v => v.EntGuid == searchEntGuid.Value);
            }
            if (!string.IsNullOrEmpty(EntBrandNic))
            {
                result = result.Where(v => v.EntBrandNic == EntBrandNic);
            }
            if (!string.IsNullOrEmpty(searchFilterVal))
            {
                switch (searchColumnVal)
                {
                    case "EntArticle":
                        {
                            result = result.Where(v => v.EntArticle.StartsWith(searchFilterVal));
                            break;
                        }
                    case "EntBrandNic":
                        {
                            result = result.Where(v => v.EntBrandNic.StartsWith(searchFilterVal));
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
                    case "EntArticleDescriptionId":
                        {
                            int intFilter;
                            if (int.TryParse(searchFilterVal, out intFilter))
                            {
                                result = result.Where(v => v.EntArticleDescriptionId == intFilter);
                            }
                            break;
                        }
                    case "ExternArticle":
                        {
                            result = result.Where(v => v.ExternArticle.StartsWith(searchFilterVal));
                            break;
                        }
                    case "ExternBrandNic":
                        {
                            result = result.Where(v => v.ExternBrandNic.StartsWith(searchFilterVal));
                            break;
                        }
                    case "ExternArticleEAN":
                        {
                            result = result.Where(v => v.ExternArticleEAN.StartsWith(searchFilterVal));
                            break;
                        }
                }
            }
            return result.Count();
        }
        public void EnterpriseArticleTDESSaveChanges()
        {
            db.SaveChanges();
        }
        public void EnterpriseArticleTDESReload(EnterpriseArticleTDES item)
        {
            db.Entry(item).Reload();
        }
        public void EnterpriseArticleTDESInsertOrUpdate(EnterpriseArticleTDES item)
        {
            if (db.EnterpriseArticleTDES.AsNoTracking().Any(e => (e.EntArticle == item.EntArticle) && (e.EntBrandNic == item.EntBrandNic) && (e.EntGuid == item.EntGuid)))
            {
                EnterpriseArticleTDESUpdate(item);
            }
            else
            {
                EnterpriseArticleTDESInsert(item);
            }
        }
        public void EnterpriseArticleTDESAdd(EnterpriseArticleTDES item, EnterpriseArticleDescriptionTDES descr)
        {
            EnterpriseArticleDescriptionTDES enterprisearticledescriptiontdes = null;
            EnterpriseBrandTDES enterprisebrandtdes = null;
            EnterpriseArticleTDES art = null;
            try
            {

                art = (from e in db.EnterpriseArticleTDES.AsNoTracking()
                       where (e.EntArticle == item.EntArticle) && (e.EntBrandNic == item.EntBrandNic) && (e.EntGuid == item.EntGuid)
                       select e).FirstOrDefault();
                if (art == null)
                {
                    if (descr.EntArticleDescription.Length > 120) descr.EntArticleDescription = descr.EntArticleDescription.Substring(0, 120);

                    enterprisearticledescriptiontdes = (from e in db.EnterpriseArticleDescriptionTDES
                                                        where e.EntArticleDescription == descr.EntArticleDescription
                                                        select e).FirstOrDefault();
                    if (enterprisearticledescriptiontdes == null)
                    {
                        enterprisearticledescriptiontdes = new EnterpriseArticleDescriptionTDES()
                        {
                            EntArticleDescription = descr.EntArticleDescription
                        };
                        db.EnterpriseArticleDescriptionTDES.Add(enterprisearticledescriptiontdes);
                        db.SaveChanges();
                    }

                    enterprisebrandtdes = (from e in db.EnterpriseBrandTDES
                                           where e.EntBrandNic == item.ExternBrandNic && e.EntGuid == item.EntGuid
                                           select e).FirstOrDefault();
                    if (enterprisebrandtdes == null)
                    {
                        enterprisebrandtdes = new EnterpriseBrandTDES()
                        {
                            EntBrandNic = item.ExternBrandNic,
                            EntBrandDescription = item.ExternBrandNic,
                            EntGuid = item.EntGuid

                        };
                        db.EnterpriseBrandTDES.Add(enterprisebrandtdes);
                        db.SaveChanges();
                    }

                    if (item.ExternArticleEAN != null)
                        item.ExternArticleEAN = item.ExternArticleEAN.Replace(" ", "");


                    item.EntArticleDescriptionId = enterprisearticledescriptiontdes.EntArticleDescriptionId;
                    db.EnterpriseArticleTDES.Add(item);
                    db.SaveChanges();
                }
                //else
                //{
                //    art.EntArticleDescriptionId = enterprisearticledescriptiontdes.EntArticleDescriptionId;
                //    art.ExternArticle = item.ExternArticle;
                //    art.ExternBrandNic = item.ExternBrandNic;
                //    art.ExternArticleEAN = item.ExternArticleEAN;
                //}
                //db.SaveChanges();
            }
            finally
            {
                if (enterprisearticledescriptiontdes != null)
                    db.Entry(enterprisearticledescriptiontdes).State = EntityState.Detached;
                if (enterprisebrandtdes != null)
                    db.Entry(enterprisebrandtdes).State = EntityState.Detached;
                if (art == null)
                //if (art != null)
                //{
                //    db.Entry(art).State = EntityState.Detached;
                //}
                //else
                {
                    db.Entry(item).State = EntityState.Detached;
                }
            }
        }
        #endregion


    }
}
