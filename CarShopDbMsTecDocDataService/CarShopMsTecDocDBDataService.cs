using CarShop.Models;
using CarShopDataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TecDocEcoSystemDbClassLibrary;
using System.Data.Entity;

namespace CarShopDbMsTecDocDataService
{
    public class CarShopMsTecDocDBDataService : ICarShopMsTecDocDataService
    {

        protected string connectionstringname = null;
        protected CarShopMsTecDocContext _db = null;
        protected CarShopMsTecDocContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new CarShopMsTecDocContext(connectionstringname);
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
                if (this.connectionstringname != null)
                {
                    if (_db != null)
                    {
                        _db.Dispose();
                        _db = null;
                    }
                }
                this.connectionstringname = connectionStringName;
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
            System.Data.Entity.Database.SetInitializer<CarShopMsTecDocContext>(new CarShopMsTecDocContextInitializer());
            using (CarShopMsTecDocContext db2 = new CarShopMsTecDocContext(connectionstringname))
            {
                db2.Database.Initialize(true);
            } //db.Dispose();
        }

        #endregion


        #region EnterpriseArticleCategoryItemTDES
        public EnterpriseArticleCategoryItemTDES EnterpriseArticleCategoryItemTDESInsert(EnterpriseArticleCategoryItemTDES item)
        {
            EnterpriseArticleCategoryItemTDES enterprisearticlecategoryitemTDES = item;
            try
            {
                enterprisearticlecategoryitemTDES = db.EnterpriseArticleCategoryItemTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisearticlecategoryitemTDES).State = EntityState.Detached;
            }
            return enterprisearticlecategoryitemTDES;
        }
        public void EnterpriseArticleCategoryItemTDESUpdate(EnterpriseArticleCategoryItemTDES item)
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
        public void EnterpriseArticleCategoryItemTDESInsertOrUpdate(EnterpriseArticleCategoryItemTDES item)
        {
            if (db.EnterpriseArticleCategoryItemTDES.AsNoTracking().Any(e => (e.ArticleId == item.ArticleId) && (e.CategoryItemId == item.CategoryItemId)))
            {
                // EnterpriseArticleCategoryItemTDESUpdate(item);
            }
            else
            {
                EnterpriseArticleCategoryItemTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseCarModelFuelTDES
        public EnterpriseCarModelFuelTDES EnterpriseCarModelFuelTDESInsert(EnterpriseCarModelFuelTDES item)
        {
            EnterpriseCarModelFuelTDES enterprisecarmodelfuelTDES = item;
            try
            {
                enterprisecarmodelfuelTDES = db.EnterpriseCarModelFuelTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisecarmodelfuelTDES).State = EntityState.Detached;
            }
            return enterprisecarmodelfuelTDES;
        }
        public void EnterpriseCarModelFuelTDESUpdate(EnterpriseCarModelFuelTDES item)
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
        public void EnterpriseCarModelFuelTDESInsertOrUpdate(EnterpriseCarModelFuelTDES item)
        {
            if (db.EnterpriseCarModelFuelTDES.AsNoTracking().Any(e => (e.FUELId == item.FUELId)))
            {
                EnterpriseCarModelFuelTDESUpdate(item);
            }
            else
            {
                EnterpriseCarModelFuelTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseCarBrandTDES
        public EnterpriseCarBrandTDES EnterpriseCarBrandTDESInsert(EnterpriseCarBrandTDES item)
        {
            EnterpriseCarBrandTDES enterprisecarbrandTDES = item;
            try
            {
                enterprisecarbrandTDES = db.EnterpriseCarBrandTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisecarbrandTDES).State = EntityState.Detached;
            }
            return enterprisecarbrandTDES;
        }
        public void EnterpriseCarBrandTDESUpdate(EnterpriseCarBrandTDES item)
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
        public void EnterpriseCarBrandTDESInsertOrUpdate(EnterpriseCarBrandTDES item)
        {
            if (db.EnterpriseCarBrandTDES.AsNoTracking().Any(e => (e.EnterpriseCarBrandId == item.EnterpriseCarBrandId)))
            {
                EnterpriseCarBrandTDESUpdate(item);
            }
            else
            {
                EnterpriseCarBrandTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseCarModelTypeTDES
        public EnterpriseCarModelTypeTDES EnterpriseCarModelTypeTDESInsert(EnterpriseCarModelTypeTDES item)
        {
            EnterpriseCarBrandTDES enterprisecarbrandTDES = (from e in db.EnterpriseCarBrandTDES
                                                             where e.EnterpriseCarBrandId == item.EnterpriseCarBrandId
                                                             select e).FirstOrDefault();
            EnterpriseCarModelTypeTDES enterprisecarmodeltypeTDES = item;
            try
            {
                enterprisecarmodeltypeTDES = db.EnterpriseCarModelTypeTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisecarbrandTDES).State = EntityState.Detached;
                db.Entry(enterprisecarmodeltypeTDES).State = EntityState.Detached;
            }
            return enterprisecarmodeltypeTDES;
        }
        public void EnterpriseCarModelTypeTDESUpdate(EnterpriseCarModelTypeTDES item)
        {
            EnterpriseCarBrandTDES enterprisecarbrandTDES = (from e in db.EnterpriseCarBrandTDES
                                                             where e.EnterpriseCarBrandId == item.EnterpriseCarBrandId
                                                             select e).FirstOrDefault();
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
                db.Entry(enterprisecarbrandTDES).State = EntityState.Detached;
            }
        }
        public void EnterpriseCarModelTypeTDESInsertOrUpdate(EnterpriseCarModelTypeTDES item)
        {
            if (db.EnterpriseCarModelTypeTDES.AsNoTracking().Any(e => (e.EnterpriseCarBrandId == item.EnterpriseCarBrandId) && (e.EnterpriseCarModelTypeId == item.EnterpriseCarModelTypeId)))
            {
                EnterpriseCarModelTypeTDESUpdate(item);
            }
            else
            {
                EnterpriseCarModelTypeTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseCarModelTDES
        public EnterpriseCarModelTDES EnterpriseCarModelTDESInsert(EnterpriseCarModelTDES item)
        {
            EnterpriseCarModelFuelTDES enterprisecarmodelfuelTDES = (from e in db.EnterpriseCarModelFuelTDES
                                                                     where e.FUELId == item.FUELId
                                                                     select e).FirstOrDefault();
            EnterpriseCarModelTypeTDES enterprisecarmodeltypeTDES = (from e in db.EnterpriseCarModelTypeTDES
                                                                     where e.EnterpriseCarBrandId == item.EnterpriseCarBrandId
                                                                     select e).FirstOrDefault();
            EnterpriseCarModelTDES enterprisecarmodelTDES = item;
            try
            {
                enterprisecarmodelTDES = db.EnterpriseCarModelTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisecarmodelTDES).State = EntityState.Detached;
                db.Entry(enterprisecarmodeltypeTDES).State = EntityState.Detached;
                db.Entry(enterprisecarmodelfuelTDES).State = EntityState.Detached;
            }
            return enterprisecarmodelTDES;
        }
        public void EnterpriseCarModelTDESUpdate(EnterpriseCarModelTDES item)
        {
            EnterpriseCarModelFuelTDES enterprisecarmodelfuelTDES = (from e in db.EnterpriseCarModelFuelTDES
                                                                     where e.FUELId == item.FUELId
                                                                     select e).FirstOrDefault();
            EnterpriseCarModelTypeTDES enterprisecarmodeltypeTDES = (from e in db.EnterpriseCarModelTypeTDES
                                                                     where e.EnterpriseCarBrandId == item.EnterpriseCarBrandId
                                                                     select e).FirstOrDefault();
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            finally
            {
                db.Entry(item).State = EntityState.Detached;
                db.Entry(enterprisecarmodelfuelTDES).State = EntityState.Detached;
                db.Entry(enterprisecarmodeltypeTDES).State = EntityState.Detached;
            }

        }
        public void EnterpriseCarModelTDESInsertOrUpdate(EnterpriseCarModelTDES item)
        {
            if (db.EnterpriseCarModelTDES.AsNoTracking().Any(e => (e.EnterpriseCarBrandId == item.EnterpriseCarBrandId) &&
                (e.EnterpriseCarModelTypeId == item.EnterpriseCarModelTypeId) && (e.EnterpriseCarModelId == item.EnterpriseCarModelId)))
            {
                EnterpriseCarModelTDESUpdate(item);
            }
            else
            {
                EnterpriseCarModelTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseArticleBrandTDES
        public EnterpriseArticleBrandTDES EnterpriseArticleBrandTDESInsert(EnterpriseArticleBrandTDES item)
        {
            EnterpriseArticleBrandTDES enterprisearticlebrandTDES = item;
            try
            {
                enterprisearticlebrandTDES = db.EnterpriseArticleBrandTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisearticlebrandTDES).State = EntityState.Detached;
            }
            return enterprisearticlebrandTDES;
        }
        public void EnterpriseArticleBrandTDESUpdate(EnterpriseArticleBrandTDES item)
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
        public void EnterpriseArticleBrandTDESInsertOrUpdate(EnterpriseArticleBrandTDES item)
        {
            if (db.EnterpriseArticleBrandTDES.AsNoTracking().Any(e => (e.ArticleBrandId == item.ArticleBrandId)))
            {
                EnterpriseArticleBrandTDESUpdate(item);
            }
            else
            {
                EnterpriseArticleBrandTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseArticleLookUpTDES
        public EnterpriseArticleLookUpTDES EnterpriseArticleLookUpTDESInsert(EnterpriseArticleLookUpTDES item)
        {
            EnterpriseArticleLookUpTDES enterprisearticlelookupTDES = item;
            try
            {
                enterprisearticlelookupTDES = db.EnterpriseArticleLookUpTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisearticlelookupTDES).State = EntityState.Detached;
            }
            return enterprisearticlelookupTDES;
        }
        public void EnterpriseArticleLookUpTDESUpdate(EnterpriseArticleLookUpTDES item)
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
        public void EnterpriseArticleLookUpTDESInsertOrUpdate(EnterpriseArticleLookUpTDES item)
        {
            if (string.IsNullOrEmpty(item.ArticleSearch))
            {
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(item.ArticleSearch.Trim()))
                {
                    return;
                }
            }

            if (db.EnterpriseArticleLookUpTDES.AsNoTracking().Any(e =>
                (e.ArticleSearch == item.ArticleSearch) &&
                (e.ArticleId == item.ArticleId) &&
                (e.ArticleBrandId == item.ArticleBrandId) &&
                (e.ArticleSearchKind == item.ArticleSearchKind)
                ))
            {
                //EnterpriseArticleLookUpTDESUpdate(item);
            }
            else
            {
                EnterpriseArticleLookUpTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseArticleApplicTDES
        public EnterpriseArticleApplicTDES EnterpriseArticleApplicTDESInsert(EnterpriseArticleApplicTDES item)
        {
            EnterpriseArticleApplicTDES enterprisearticleapplicTDES = item;
            try
            {
                enterprisearticleapplicTDES = db.EnterpriseArticleApplicTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisearticleapplicTDES).State = EntityState.Detached;
            }
            return enterprisearticleapplicTDES;
        }
        public void EnterpriseArticleApplicTDESUpdate(EnterpriseArticleApplicTDES item)
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
        public void EnterpriseArticleApplicTDESInsertOrUpdate(EnterpriseArticleApplicTDES item)
        {
            if (db.EnterpriseArticleApplicTDES.AsNoTracking().Any(e =>
                (e.ArticleId == item.ArticleId) &&
                (e.EnterpriseCarModelId == item.EnterpriseCarModelId)
                ))
            {
                //EnterpriseArticleLookUpTDESUpdate(item);
            }
            else
            {
                EnterpriseArticleApplicTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseArticleTecDocTDES
        public EnterpriseArticleTecDocTDES EnterpriseArticleTecDocTDESInsert(EnterpriseArticleTecDocTDES item)
        {
            EnterpriseArticleTecDocTDES enterprisearticletecdocTDES = item;
            try
            {
                enterprisearticletecdocTDES = db.EnterpriseArticleTecDocTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisearticletecdocTDES).State = EntityState.Detached;
            }
            return enterprisearticletecdocTDES;
        }
        public void EnterpriseArticleTecDocTDESUpdate(EnterpriseArticleTecDocTDES item)
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
        public void EnterpriseArticleTecDocTDESInsertOrUpdate(EnterpriseArticleTecDocTDES item)
        {
            if (db.EnterpriseArticleTecDocTDES.AsNoTracking().Any(e =>
                (e.ArticleId == item.ArticleId)
                ))
            {
                //EnterpriseArticleLookUpTDESUpdate(item);
            }
            else
            {
                EnterpriseArticleTecDocTDESInsert(item);
            }
        }
        public void EnterpriseArticleTecDocTDESAdd(EnterpriseArticleTecDocTDES item, EnterpriseArticleTecDocDescriptionTDES descr)
        {
            EnterpriseArticleTecDocDescriptionTDES enterprisearticledescriptiontdes = null;
            EnterpriseArticleTecDocTDES art = null;
            try
            {

                art = (from e in db.EnterpriseArticleTecDocTDES // .AsNoTracking()
                       where (e.ArticleId == item.ArticleId)
                       select e).FirstOrDefault();
//??                if (art == null)
//??                {
                    if (descr.EntArticleDescription.Length > 120) descr.EntArticleDescription = descr.EntArticleDescription.Substring(0, 120);

                    enterprisearticledescriptiontdes = (from e in db.EnterpriseArticleTecDocDescriptionTDES
                                                        where e.EntArticleDescription == descr.EntArticleDescription
                                                        select e).FirstOrDefault();
                    if (enterprisearticledescriptiontdes == null)
                    {
                        enterprisearticledescriptiontdes = new EnterpriseArticleTecDocDescriptionTDES()
                        {
                            EntArticleDescription = descr.EntArticleDescription
                        };
                        db.EnterpriseArticleTecDocDescriptionTDES.Add(enterprisearticledescriptiontdes);
                        db.SaveChanges();
                    }

                    if (item.ExternArticleEAN != null)
                        item.ExternArticleEAN = item.ExternArticleEAN.Replace(" ", "");


                    item.EntArticleDescriptionId = enterprisearticledescriptiontdes.EntArticleDescriptionId;
                    if (art == null)
                    {
                        db.EnterpriseArticleTecDocTDES.Add(item);
                    }
                    else
                    {
                        db.Entry(art).State = EntityState.Modified;
                        art.ExternArticle = item.ExternArticle;
                        art.ExternBrandNic = item.ExternBrandNic;
                        art.ExternArticleEAN = item.ExternArticleEAN;
                        art.EntArticleDescriptionId = item.EntArticleDescriptionId;
                        art.ArticleBrandId = item.ArticleBrandId;
                    }

                    db.SaveChanges();
//??                }

            }
            finally
            {
                if (enterprisearticledescriptiontdes != null)
                    db.Entry(enterprisearticledescriptiontdes).State = EntityState.Detached;
                if (art == null)
                {
                    db.Entry(item).State = EntityState.Detached;
                }
                else
                {
                    db.Entry(art).State = EntityState.Detached;
                }
            }
        }
        #endregion

        #region EnterpriseCategoryTecDocTDES
        public EnterpriseCategoryTecDocTDES EnterpriseCategoryTecDocTDESInsert(EnterpriseCategoryTecDocTDES item)
        {
            EnterpriseCategoryTecDocTDES enterprisecategorytecdocTDES = item;
            try
            {
                enterprisecategorytecdocTDES = db.EnterpriseCategoryTecDocTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisecategorytecdocTDES).State = EntityState.Detached;
            }
            return enterprisecategorytecdocTDES;
        }
        public void EnterpriseCategoryTecDocTDESUpdate(EnterpriseCategoryTecDocTDES item)
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
        public void EnterpriseCategoryTecDocTDESInsertOrUpdate(EnterpriseCategoryTecDocTDES item)
        {
            if (db.EnterpriseCategoryTecDocTDES.AsNoTracking().Any(e =>
                (e.CategoryId == item.CategoryId)
                ))
            {
                //EnterpriseArticleLookUpTDESUpdate(item);
            }
            else
            {
                EnterpriseCategoryTecDocTDESInsert(item);
            }
        }
        #endregion

        #region EnterpriseCategoryItemTecDocTDES
        public EnterpriseCategoryItemTecDocTDES EnterpriseCategoryItemTecDocTDESInsert(EnterpriseCategoryItemTecDocTDES item)
        {
            EnterpriseCategoryItemTecDocTDES enterprisecategoryitemtecdocTDES = item;
            try
            {
                EnterpriseCategoryItemTecDocDescriptionTDES
                    enterprisecategoryitemdescriptiontdes = (from e in db.EnterpriseCategoryItemTecDocDescriptionTDES
                                                             where e.EntCategoryItemDescriptionId == item.EntCategoryItemDescriptionId
                                                             select e).FirstOrDefault();
                EnterpriseCategoryTecDocTDES enterprisecategorytdes =
                    (from e in db.EnterpriseCategoryTecDocTDES
                     where (e.CategoryId == item.CategoryId)
                     select e).FirstOrDefault();



                enterprisecategoryitemtecdocTDES = db.EnterpriseCategoryItemTecDocTDES.Add(item);
                db.SaveChanges();
            }
            finally
            {
                db.Entry(enterprisecategoryitemtecdocTDES).State = EntityState.Detached;
            }
            return enterprisecategoryitemtecdocTDES;
        }
        public void EnterpriseCategoryItemTecDocTDESUpdate(EnterpriseCategoryItemTecDocTDES item)
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
        public void EnterpriseCategoryItemTecDocTDESInsertOrUpdate(EnterpriseCategoryItemTecDocTDES item)
        {
            if (db.EnterpriseCategoryItemTecDocTDES.AsNoTracking().Any(e =>
                (e.CategoryId == item.CategoryId) && (e.CategoryItemId == item.CategoryItemId)
                ))
            {
                //EnterpriseArticleLookUpTDESUpdate(item);
            }
            else
            {
                EnterpriseCategoryItemTecDocTDESInsert(item);
            }
        }
        public void EnterpriseCategoryItemTecDocTDESAdd(EnterpriseCategoryItemTecDocTDES item, EnterpriseCategoryItemTecDocDescriptionTDES descr)
        {


            if (!db.EnterpriseCategoryItemTecDocTDES.AsNoTracking().Any(e =>
                (e.CategoryId == item.CategoryId) && (e.CategoryItemId == item.CategoryItemId)
                ))
            {
                EnterpriseCategoryItemTecDocDescriptionTDES ecid = null;
                EnterpriseCategoryTecDocTDES categ = null;
                try
                {

                    categ = (from e in db.EnterpriseCategoryTecDocTDES
                             where e.CategoryId == item.CategoryId
                             select e).FirstOrDefault();
                    ecid =
                          (from e in db.EnterpriseCategoryItemTecDocDescriptionTDES
                           where e.EntCategoryItemDescription == descr.EntCategoryItemDescription
                           select e).FirstOrDefault();

                    if (ecid == null)
                    {
                        ecid = new EnterpriseCategoryItemTecDocDescriptionTDES()
                        {
                            EntCategoryItemDescription = descr.EntCategoryItemDescription
                        };
                        db.EnterpriseCategoryItemTecDocDescriptionTDES.Add(ecid);
                        db.SaveChanges();
                    }

                    item.EntCategoryItemDescriptionId = ecid.EntCategoryItemDescriptionId;
                    db.EnterpriseCategoryItemTecDocTDES.Add(item);
                    db.SaveChanges();
                }
                finally
                {
                    db.Entry(item).State = EntityState.Detached;
                    if (ecid != null)
                        db.Entry(ecid).State = EntityState.Detached;
                    if (categ != null)
                        db.Entry(categ).State = EntityState.Detached;
                }
            }

        }
        #endregion

    }
}
