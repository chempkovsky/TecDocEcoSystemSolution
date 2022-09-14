// CarShop.Areas.MsTecDoc.Controllers.MsNoTecDocSvcController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Areas.MsTecDoc.Controllers
{

    public class MsNoTecDocSvcController : ApiController
    {
        private CarShopContext dbCarShop = new CarShopContext();

        private bool ViewBag_IsEcoSystemAdmin;

        private bool ViewBag_IsEnterpriseAdmin;

        private Guid EntGuid = default(Guid);

        private int TecDocSrcType = 1;

        private TecDocContext _db;

        private TecDocContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new TecDocContext(TecDocSrcType);
                }
                return _db;
            }
        }

        private void UserIsInRoles()
        {
            ViewBag_IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            ViewBag_IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
        }

        private void ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterpriseBranchTDES = (from e in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                         select e).FirstOrDefault();
            if (enterpriseBranchTDES != null)
            {
                if (!enterpriseBranchTDES.IsActive || !enterpriseBranchTDES.EnterpriseTDES.IsActive)
                {
                    base.ModelState.AddModelError("", Resources.SendEnterpriseBranchTDES_ISNOTACTIVE);
                    return;
                }
                TecDocSrcType = enterpriseBranchTDES.EnterpriseTDES.TecDocSrcTypeId;
                EntGuid = enterpriseBranchTDES.EnterpriseTDES.EntGuid;
            }
            else
            {
                base.ModelState.AddModelError("", Resources.SendEnterpriseBranchTDES_NOTFOUND);
            }
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public HttpResponseMessage PostMsNoTecDoc(Guid? searchEntBranchGuid = null, string CustArticleCode = null, string CustArticleBrand = null, string CustArticleName = null, string CustArticleEAN = null, string CustArticleOems = null)
        {
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            UserIsInRoles();
            Guid? searchEntGuid = null;
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if (!ViewBag_IsEcoSystemAdmin)
            {
                string aName = base.User.Identity.Name;
                if (ViewBag_IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in dbCarShop.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in dbCarShop.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES != null)
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                }
            }
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            if (searchEntGuid.HasValue && EntGuid.CompareTo(searchEntGuid.Value) != 0)
            {
                base.ModelState.AddModelError("", "Указано не свое подразделение");
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            if (TecDocSrcType == 1)
            {
                base.ModelState.AddModelError("", "Тип источника TecDoc не равен MSTecDoc");
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            CarShopMsTecDocContext msTDContext = db.msTDContext;
            object value = DBNull.Value;
            object value2 = DBNull.Value;
            object value3 = DBNull.Value;
            object value4 = DBNull.Value;
            object value5 = DBNull.Value;
            if (!string.IsNullOrEmpty(CustArticleCode))
            {
                value = CustArticleCode;
            }
            if (!string.IsNullOrEmpty(CustArticleBrand))
            {
                value2 = CustArticleBrand;
            }
            if (!string.IsNullOrEmpty(CustArticleEAN))
            {
                value3 = CustArticleEAN;
            }
            if (!string.IsNullOrEmpty(CustArticleName))
            {
                value4 = CustArticleName;
            }
            if (!string.IsNullOrEmpty(CustArticleOems))
            {
                value5 = CustArticleOems;
            }
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@CustArticleCodeP";
            sqlParameter.Value = value;
            sqlParameter.Direction = ParameterDirection.Input;
            SqlParameter sqlParameter2 = sqlParameter;
            SqlParameter sqlParameter3 = new SqlParameter();
            sqlParameter3.ParameterName = "@CustArticleBrandP";
            sqlParameter3.Value = value2;
            sqlParameter3.Direction = ParameterDirection.Input;
            SqlParameter sqlParameter4 = sqlParameter3;
            SqlParameter sqlParameter5 = new SqlParameter();
            sqlParameter5.ParameterName = "@CustArticleNameP";
            sqlParameter5.Value = value4;
            sqlParameter5.Direction = ParameterDirection.Input;
            SqlParameter sqlParameter6 = sqlParameter5;
            SqlParameter sqlParameter7 = new SqlParameter();
            sqlParameter7.ParameterName = "@CustArticleEANP";
            sqlParameter7.Value = value3;
            sqlParameter7.Direction = ParameterDirection.Input;
            SqlParameter sqlParameter8 = sqlParameter7;
            SqlParameter sqlParameter9 = new SqlParameter();
            sqlParameter9.ParameterName = "@CustArticleOemsP";
            sqlParameter9.Value = value5;
            sqlParameter9.Direction = ParameterDirection.Input;
            SqlParameter sqlParameter10 = sqlParameter9;
            try
            {
                msTDContext.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "exec [dbo].[AddNewCustArticleSP] @CustArtCode = @CustArticleCodeP, @CustBrandNic  = @CustArticleBrandP , @CustArtDescr = @CustArticleNameP, @CustEAN = @CustArticleEANP, @CustOems = @CustArticleOemsP ", sqlParameter2, sqlParameter4, sqlParameter6, sqlParameter8, sqlParameter10);
            }
            catch (Exception ex)
            {
                base.ModelState.AddModelError("", "Ошибка БД:" + ex.Message);
                return base.Request.CreateErrorResponse(HttpStatusCode.BadRequest, base.ModelState);
            }
            return base.Request.CreateResponse(HttpStatusCode.OK);
        }

        protected override void Dispose(bool disposing)
        {
            dbCarShop.Dispose();
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
            base.Dispose(disposing);
        }
    }
}