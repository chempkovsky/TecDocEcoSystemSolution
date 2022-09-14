// CarShop.Controllers.ReturnBasketArticleController
using CarShop.Models;
using CarShop.Properties;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class ReturnBasketArticleController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopSalesContextCatalog;

        private CarShopSalesContext _dbSales;

        private CarShopSalesContext dbSales
        {
            get
            {
                if (_dbSales == null)
                {
                    _dbSales = new CarShopSalesContext(CarShopSalesContextCatalog);
                }
                return _dbSales;
            }
        }

        public async Task ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
                base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
                base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
                CarShopSalesContextCatalog = enterprisebranchtdes.SalesCatalog;
                return;
            }
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            EnterpriseTDES enterprises = await (from e in db.EnterpriseTDES
                                                where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
            if (enterprises != null)
            {
                base.ViewBag.EntDescription = enterprises.EntDescription;
                base.ViewBag.SearchEntGuid = enterprises.EntGuid;
            }
            else
            {
                base.ViewBag.EntDescription = Resources.ENTERPRISE_NOT_DEFINED;
                base.ViewBag.SearchEntGuid = null;
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
        }

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            base.ViewBag.IsBranchAdmin = base.User.IsInRole("BranchAdmin");
            base.ViewBag.IsBranchAudit = base.User.IsInRole("BranchAudit");
            base.ViewBag.IsBranchSeller = base.User.IsInRole("BranchSeller");
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchRetBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null, int? page = default(int?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        if ((!base.ViewBag.IsBranchAudit))
                        {
                            searchEntUserNic = aName;
                        }
                    }
                }
            }
            base.ViewBag.SearchEntUserNic = searchEntUserNic;
            IPagedList<ReturnBasketArticleTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                ReturnBasketTDES salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic && (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid
                                                         select e).FirstOrDefaultAsync();
                if (salebaskettdes != null)
                {
                    base.ViewBag.SearchRetBasketGuid = salebaskettdes.RetBasketGuid;
                    base.ViewBag.ReturnBasket = salebaskettdes;
                    IOrderedQueryable<ReturnBasketArticleTDES> salebasketarticletdes = from e in dbSales.ReturnBasketArticleTDES.Include("SaleArticleDescriptionTDES")
                                                                                       where e.RetBasketGuid == salebaskettdes.RetBasketGuid
                                                                                       orderby e.EntArticle, e.EntBrandNic
                                                                                       select e;
                    int pageSize = 20;
                    int pageNumber = page ?? 1;
                    aResult = await salebasketarticletdes.ToPagedListAsync(pageNumber, pageSize);
                }
            }
            return View(aResult);
        }

        protected async Task CreateUpdateCheckUp(ReturnBasketArticleTmp salebasketarticletmp)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            string searchEntUserNic = null;
            Guid? searchRetBasketGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                    searchEntBranchGuid = salebasketarticletmp.EntBranchGuid;
                    searchEntUserNic = salebasketarticletmp.EntUserNic;
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            else
            {
                searchEntBranchGuid = salebasketarticletmp.EntBranchGuid;
                searchEntGuid = salebasketarticletmp.EntGuid;
                searchEntUserNic = salebasketarticletmp.EntUserNic;
            }
            searchRetBasketGuid = salebasketarticletmp.RetBasketGuid;
            if (base.ModelState.IsValid && (searchEntGuid != salebasketarticletmp.EntGuid || searchEntBranchGuid != salebasketarticletmp.EntBranchGuid || searchEntUserNic != salebasketarticletmp.EntUserNic))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return;
            }
            base.ViewBag.SearchEntUserNic = salebasketarticletmp.EntUserNic;
            base.ViewBag.SearchEntBasketGuid = salebasketarticletmp.RetBasketGuid;
            ReturnBasketTDES salebaskettdes = null;
            BranchSpellTDES branchspelltdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return;
            }
            if (CarShopSalesContextCatalog != null)
            {
                if ((!base.ViewBag.IsEcoSystemAdmin))
                {
                    if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                    {
                        salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                                where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
                    }
                    else
                    {
                        salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                                where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                                select e).FirstOrDefaultAsync();
                    }
                }
                else
                {
                    salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                            where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid
                                            select e).FirstOrDefaultAsync();
                }
            }
            if (salebaskettdes == null)
            {
                base.ModelState.AddModelError("", Resources.ReturnBasketTDES_NOTFOUND);
                return;
            }
            if (salebaskettdes.IsPaid || salebaskettdes.IsReverse)
            {
                base.ModelState.AddModelError("", Resources.ReturnBasketTDES_ISPAYED_OR_REVERSED);
                return;
            }
            base.ViewBag.salebaskettdes = salebaskettdes;
            if (CarShopSalesContextCatalog != null)
            {
                branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                         where e.EntBranchGuid == salebaskettdes.EntBranchGuid
                                         select e).FirstOrDefaultAsync();
            }
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_NOTFOUND);
                return;
            }
            if (branchspelltdes.SpellGuid != salebaskettdes.SpellGuid)
            {
                base.ModelState.AddModelError("", Resources.ReturnBasketTDES_ISCREATED_IN_ANOTHERSPELL);
                return;
            }
            if (!branchspelltdes.IsActive || branchspelltdes.IsBlocked)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_ISNOTACTIVE);
                return;
            }
            User2WorkPlaceTDES user2workplacetdes = await (from e in dbSales.User2WorkPlaceTDES
                                                           where e.WorkPlaceGuid == salebaskettdes.WorkPlaceGuid
                                                           select e).FirstOrDefaultAsync();
            if (user2workplacetdes == null)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return;
            }
            if (user2workplacetdes.EntUserNic != salebaskettdes.EntUserNic)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return;
            }
            SaleBasketArticleTDES salebasketarticleTDES = await (from e in dbSales.SaleBasketArticleTDES.Include("SaleArticleDescriptionTDES")
                                                                 where e.EntArticle == salebasketarticletmp.EntArticle && e.EntBrandNic == salebasketarticletmp.EntBrandNic && e.EntBasketGuid == salebasketarticletmp.EntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                 select e).FirstOrDefaultAsync();
            if (salebasketarticleTDES == null)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketArticleTDES_NOTFOUND);
            }
            else if (!salebasketarticleTDES.IsPaid || !salebasketarticleTDES.IsSpellClosed)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketArticleTDES_NOTFOUND);
            }
            else if (salebasketarticleTDES.ReverseAmount >= salebasketarticleTDES.ArtAmount)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketArticleTDES_NOTHINGTOSTORN);
            }
            else if (salebasketarticletmp.ArtAmount > salebasketarticleTDES.ArtAmount - salebasketarticleTDES.ReverseAmount)
            {
                base.ModelState.AddModelError("", Resources.ReturnBasketTDES_AMOUNTINCORRECT);
            }
            else if (salebasketarticletmp.ArtAmount < 0)
            {
                base.ModelState.AddModelError("", Resources.ReturnBasketTDES_AMOUNTLESSZERO);
            }
            else if (salebasketarticletmp.SalePrice < 0.0)
            {
                base.ModelState.AddModelError("", Resources.ReturnBasketTDES_PRICELESSZERO);
            }
            else
            {
                salebasketarticletmp.EntArticleDescriptionId = salebasketarticleTDES.EntArticleDescriptionId;
            }
        }

        protected async Task<ReturnBasketArticleTDES> GetReturnBasketArticleTDES(string EntArticle = null, string EntBrandNic = null, Guid? searchEntBasketGuid = default(Guid?), Guid? searchRetBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            ReturnBasketArticleTDES aResult = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                aResult = await (from e in dbSales.ReturnBasketArticleTDES.Include("SaleArticleDescriptionTDES")
                                 where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                 select e).FirstOrDefaultAsync();
            }
            return aResult;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(string EntArticle = null, string EntBrandNic = null, Guid? searchEntBasketGuid = default(Guid?), Guid? searchRetBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            ReturnBasketArticleTDES salebasketarticletdes = await GetReturnBasketArticleTDES(EntArticle, EntBrandNic, searchEntBasketGuid, searchRetBasketGuid, searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            if (salebasketarticletdes == null)
            {
                return HttpNotFound();
            }
            ReturnBasketArticleTmp salebasketarticletmp = new ReturnBasketArticleTmp();
            salebasketarticletmp.CopyFrom(salebasketarticletdes);
            base.ViewBag.SearchEntUserNic = salebasketarticletdes.EntUserNic;
            base.ViewBag.SearchRetBasketGuid = salebasketarticletdes.RetBasketGuid;
            return View(salebasketarticletmp);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchRetBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            ReturnBasketTDES salebaskettdes = null;
            BranchSpellTDES branchspelltdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                if ((!base.ViewBag.IsEcoSystemAdmin))
                {
                    if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                    {
                        salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                                where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
                    }
                    else
                    {
                        salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                                where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                                select e).FirstOrDefaultAsync();
                    }
                }
                else
                {
                    salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                            where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid
                                            select e).FirstOrDefaultAsync();
                }
            }
            if (salebaskettdes == null)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_NOTDEFINED);
                return View();
            }
            if (salebaskettdes.IsPaid || salebaskettdes.IsReverse)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_ISPAYED_OR_REVERSED);
                return View();
            }
            if (CarShopSalesContextCatalog != null)
            {
                branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                         where e.EntBranchGuid == salebaskettdes.EntBranchGuid
                                         select e).FirstOrDefaultAsync();
            }
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_NOTFOUND);
                return View();
            }
            if (branchspelltdes.SpellGuid != salebaskettdes.SpellGuid)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_ISCREATED_IN_ANOTHERSPELL);
                return View();
            }
            if (!branchspelltdes.IsActive || branchspelltdes.IsBlocked)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_ISNOTACTIVE);
                return View();
            }
            User2WorkPlaceTDES user2workplacetdes = await (from e in dbSales.User2WorkPlaceTDES
                                                           where e.WorkPlaceGuid == salebaskettdes.WorkPlaceGuid
                                                           select e).FirstOrDefaultAsync();
            if (user2workplacetdes == null)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return View();
            }
            if (user2workplacetdes.EntUserNic != salebaskettdes.EntUserNic)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return View();
            }
            ReturnBasketArticleTmp salebasketarticletmp = new ReturnBasketArticleTmp
            {
                EntGuid = salebaskettdes.EntGuid,
                EntBranchGuid = salebaskettdes.EntBranchGuid,
                RetBasketGuid = salebaskettdes.RetBasketGuid,
                EntUserNic = salebaskettdes.EntUserNic,
                WorkPlaceGuid = salebaskettdes.WorkPlaceGuid,
                SpellGuid = salebaskettdes.SpellGuid,
                IsPaid = false,
                PaidAt = DateTime.Now,
                ReverseAmount = 0,
                ArtAmount = 0,
                SalePrice = 0.0,
                IsSpellClosed = false,
                CribFromIncome = 0
            };
            salebasketarticletmp.PaidAt = salebasketarticletmp.PaidAt.AddMilliseconds(-salebasketarticletmp.PaidAt.Millisecond);
            base.ViewBag.SearchEntUserNic = salebaskettdes.EntUserNic;
            base.ViewBag.SearchRetBasketGuid = salebaskettdes.RetBasketGuid;
            return View(salebasketarticletmp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Create(ReturnBasketArticleTmp salebasketarticletmp, string EnterpriseArticleLookUp = null)
        {
            if (EnterpriseArticleLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(salebasketarticletmp);
                return RedirectToAction("LookForSpell", "BranchSpellHst", new
                {
                    redirecData = redirecData,
                    redirectContriller = "ReturnBasketArticle",
                    redirectAction = "GetReturnArticuleForCreate",
                    searchEntGuid = salebasketarticletmp.EntGuid,
                    searchEntBranchGuid = salebasketarticletmp.EntBranchGuid
                });
            }
            await CreateUpdateCheckUp(salebasketarticletmp);
            if (base.ModelState.IsValid)
            {
                ReturnBasketArticleTDES salebasketarticletdes2 = await GetReturnBasketArticleTDES(salebasketarticletmp.EntArticle, salebasketarticletmp.EntBrandNic, salebasketarticletmp.EntBasketGuid, salebasketarticletmp.RetBasketGuid, salebasketarticletmp.EntGuid, salebasketarticletmp.EntBranchGuid, salebasketarticletmp.EntUserNic);
                ReturnBasketTDES salebaskettdes = base.ViewBag.salebaskettdes;
                if (salebasketarticletdes2 != null)
                {
                    salebaskettdes.ArtAmount += salebasketarticletmp.ArtAmount;
                    salebaskettdes.PaymentSum -= (double)salebasketarticletdes2.ArtAmount * salebasketarticletdes2.SalePrice;
                    salebasketarticletdes2.ArtAmount += salebasketarticletmp.ArtAmount;
                    salebasketarticletdes2.SalePrice = salebasketarticletmp.SalePrice;
                    salebaskettdes.PaymentSum += (double)salebasketarticletdes2.ArtAmount * salebasketarticletdes2.SalePrice;
                }
                else
                {
                    salebasketarticletdes2 = new ReturnBasketArticleTDES();
                    salebasketarticletmp.CopyTo(salebasketarticletdes2, doCreateDescr: false);
                    salebasketarticletdes2.PaidAt = DateTime.Now;
                    salebasketarticletdes2.PaidAt = salebasketarticletdes2.PaidAt.AddMilliseconds(-salebasketarticletdes2.PaidAt.Millisecond);
                    salebaskettdes.PaymentSum += (double)salebasketarticletdes2.ArtAmount * salebasketarticletdes2.SalePrice;
                    salebaskettdes.ArtAmount += salebasketarticletmp.ArtAmount;
                    dbSales.ReturnBasketArticleTDES.Add(salebasketarticletdes2);
                }
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchRetBasketGuid = salebasketarticletmp.RetBasketGuid,
                    searchEntGuid = salebasketarticletmp.EntGuid,
                    searchEntBranchGuid = salebasketarticletmp.EntBranchGuid,
                    searchEntUserNic = salebasketarticletmp.EntUserNic
                });
            }
            return View(salebasketarticletmp);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> GetReturnArticuleForCreate(string redirecData, string EntArticle = null, string EntBrandNic = null, Guid? EntBasketGuid = default(Guid?))
        {
            if (string.IsNullOrEmpty(EntArticle) || string.IsNullOrEmpty(EntBrandNic) || !EntBasketGuid.HasValue)
            {
                return RedirectToAction("Index", "ReturnBasket");
            }
            ReturnBasketArticleTmp returnbasketarticletmp;
            if (redirecData != null)
            {
                returnbasketarticletmp = JsonConvert.DeserializeObject<ReturnBasketArticleTmp>(redirecData);
                returnbasketarticletmp.EntArticle = EntArticle;
                returnbasketarticletmp.EntBrandNic = EntBrandNic;
                returnbasketarticletmp.EntBasketGuid = EntBasketGuid.Value;
            }
            else
            {
                returnbasketarticletmp = new ReturnBasketArticleTmp
                {
                    EntArticle = EntArticle,
                    EntBrandNic = EntBrandNic,
                    EntBasketGuid = EntBasketGuid.Value
                };
            }
            returnbasketarticletmp.ArtAmount = 0;
            returnbasketarticletmp.SalePrice = 0.0;
            return View("Create", await DefineOtherField(returnbasketarticletmp));
        }

        protected async Task<ReturnBasketArticleTmp> DefineOtherField(ReturnBasketArticleTmp returnbasketarticletmp)
        {
            UserIsInRoles();
            Guid? searchRetBasketGuid = null;
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            string searchEntUserNic = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    Guid value = await (from e in db.EnterpriseUserTDES
                                        where e.EntUserNic == aName
                                        select e.EntGuid).FirstOrDefaultAsync();
                    searchEntGuid = value;
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from e in db.EnterpriseBranchUserTDES
                                                                        where e.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = e.EntGuid,
                                                                            EntBranchGuid = e.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                        searchEntUserNic = null;
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                        searchEntUserNic = aName;
                    }
                }
            }
            else
            {
                searchEntBranchGuid = returnbasketarticletmp.EntBranchGuid;
                searchEntGuid = returnbasketarticletmp.EntGuid;
                searchEntUserNic = returnbasketarticletmp.EntUserNic;
            }
            searchRetBasketGuid = returnbasketarticletmp.RetBasketGuid;
            if (searchEntBranchGuid.HasValue)
            {
                returnbasketarticletmp.EntBranchGuid = searchEntBranchGuid.Value;
            }
            if (searchEntGuid.HasValue)
            {
                returnbasketarticletmp.EntGuid = searchEntGuid.Value;
            }
            returnbasketarticletmp.EntUserNic = searchEntUserNic;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return returnbasketarticletmp;
            }
            BranchSpellTDES branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                                     where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                     select e).FirstOrDefaultAsync();
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_NOTFOUND);
                return returnbasketarticletmp;
            }
            if (!branchspelltdes.IsActive || branchspelltdes.IsBlocked)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_ISNOTACTIVE);
                return returnbasketarticletmp;
            }
            User2WorkPlaceTDES user2workplacetdes = await (from e in dbSales.User2WorkPlaceTDES
                                                           where e.EntUserNic == returnbasketarticletmp.EntUserNic
                                                           select e).FirstOrDefaultAsync();
            if (user2workplacetdes == null)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return returnbasketarticletmp;
            }
            ReturnBasketTDES salebaskettdes = (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.ReturnBasketTDES
                                                                                                             where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid
                                                                                                             select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.ReturnBasketTDES
                                                                                                                                                                                                                                                  where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                                                                                                                                                                                                                                  select e).FirstOrDefaultAsync()) : (await (from e in dbSales.ReturnBasketTDES
                                                                                                                                                                                                                                                                                             where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                             select e).FirstOrDefaultAsync()));
            if (salebaskettdes != null && (salebaskettdes.WorkPlaceGuid != user2workplacetdes.WorkPlaceGuid || salebaskettdes.EntUserNic != searchEntUserNic || salebaskettdes.SpellGuid != branchspelltdes.SpellGuid || salebaskettdes.EntGuid != branchspelltdes.EntGuid || salebaskettdes.IsPaid))
            {
                salebaskettdes = null;
            }
            if (salebaskettdes == null)
            {
                salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                        where e.SpellGuid == branchspelltdes.SpellGuid && e.EntUserNic == searchEntUserNic && e.IsActive == true && e.IsPaid == false
                                        select e).FirstOrDefaultAsync();
                if (salebaskettdes == null)
                {
                    foreach (ReturnBasketTDES item in await (from e in dbSales.ReturnBasketTDES
                                                             where e.EntUserNic == searchEntUserNic && e.EntGuid == returnbasketarticletmp.EntGuid && e.EntBranchGuid == returnbasketarticletmp.EntBranchGuid && e.SpellGuid == branchspelltdes.SpellGuid && e.IsActive == true
                                                             select e).ToListAsync())
                    {
                        item.IsActive = false;
                        dbSales.Entry(item).State = EntityState.Modified;
                    }
                    await dbSales.SaveChangesAsync();
                    salebaskettdes = new ReturnBasketTDES
                    {
                        RetBasketGuid = Guid.NewGuid(),
                        WorkPlaceGuid = user2workplacetdes.WorkPlaceGuid,
                        EntUserNic = searchEntUserNic,
                        SetAt = user2workplacetdes.SetAt,
                        Description = user2workplacetdes.Description,
                        SpellGuid = branchspelltdes.SpellGuid,
                        EntBranchGuid = branchspelltdes.EntBranchGuid,
                        EntGuid = branchspelltdes.EntGuid,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        IsPaid = false,
                        PaidAt = DateTime.Now.AddSeconds(2.0),
                        IsReverse = false
                    };
                    salebaskettdes.CreatedAt = salebaskettdes.CreatedAt.AddMilliseconds(-salebaskettdes.CreatedAt.Millisecond);
                    salebaskettdes.PaidAt = salebaskettdes.PaidAt.AddMilliseconds(-salebaskettdes.PaidAt.Millisecond);
                    dbSales.ReturnBasketTDES.Add(salebaskettdes);
                    await dbSales.SaveChangesAsync();
                }
            }
            returnbasketarticletmp.RetBasketGuid = salebaskettdes.RetBasketGuid;
            returnbasketarticletmp.EntUserNic = salebaskettdes.EntUserNic;
            returnbasketarticletmp.EntGuid = salebaskettdes.EntGuid;
            returnbasketarticletmp.EntBranchGuid = salebaskettdes.EntBranchGuid;
            returnbasketarticletmp.WorkPlaceGuid = salebaskettdes.WorkPlaceGuid;
            returnbasketarticletmp.SpellGuid = salebaskettdes.SpellGuid;
            base.ViewBag.SearchEntUserNic = salebaskettdes.EntUserNic;
            base.ViewBag.SearchRetBasketGuid = salebaskettdes.RetBasketGuid;
            SaleBasketArticleTDES salebasketarticleTDES = await (from e in dbSales.SaleBasketArticleTDES.Include("SaleArticleDescriptionTDES")
                                                                 where e.EntArticle == returnbasketarticletmp.EntArticle && e.EntBrandNic == returnbasketarticletmp.EntBrandNic && e.EntBasketGuid == returnbasketarticletmp.EntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                 select e).FirstOrDefaultAsync();
            if (salebasketarticleTDES == null)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketArticleTDES_NOTFOUND);
                return returnbasketarticletmp;
            }
            if (!salebasketarticleTDES.IsPaid || !salebasketarticleTDES.IsSpellClosed)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketArticleTDES_NOTFOUND);
                return returnbasketarticletmp;
            }
            if (salebasketarticleTDES.ReverseAmount >= salebasketarticleTDES.ArtAmount)
            {
                base.ModelState.AddModelError("", Resources.SaleBasketArticleTDES_NOTHINGTOSTORN);
                return returnbasketarticletmp;
            }
            returnbasketarticletmp.EntArticleDescriptionId = salebasketarticleTDES.EntArticleDescriptionId;
            returnbasketarticletmp.EntArticleDescription = salebasketarticleTDES.SaleArticleDescriptionTDES.EntArticleDescription;
            if (returnbasketarticletmp.ArtAmount == 0)
            {
                returnbasketarticletmp.ArtAmount = salebasketarticleTDES.ArtAmount - salebasketarticleTDES.ReverseAmount;
            }
            if (returnbasketarticletmp.SalePrice == 0.0)
            {
                returnbasketarticletmp.SalePrice = salebasketarticleTDES.SalePrice;
            }
            return returnbasketarticletmp;
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Edit(string EntArticle = null, string EntBrandNic = null, Guid? searchEntBasketGuid = default(Guid?), Guid? searchRetBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            ReturnBasketArticleTDES salebasketarticletdes = await GetReturnBasketArticleTDES(EntArticle, EntBrandNic, searchEntBasketGuid, searchRetBasketGuid, searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            if (salebasketarticletdes == null)
            {
                return HttpNotFound();
            }
            ReturnBasketTDES salebaskettdes = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                            where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                            select e).FirstOrDefaultAsync();
                }
                else
                {
                    salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                            where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                            select e).FirstOrDefaultAsync();
                }
            }
            else
            {
                salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                        where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid
                                        select e).FirstOrDefaultAsync();
            }
            if (salebaskettdes == null)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_NOTDEFINED);
                return View();
            }
            if (salebaskettdes.IsPaid || salebaskettdes.IsReverse)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_ISPAYED_OR_REVERSED);
                return View();
            }
            BranchSpellTDES branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                                     where e.EntBranchGuid == salebaskettdes.EntBranchGuid
                                                     select e).FirstOrDefaultAsync();
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_NOTFOUND);
                return View();
            }
            if (branchspelltdes.SpellGuid != salebaskettdes.SpellGuid)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_ISCREATED_IN_ANOTHERSPELL);
                return View();
            }
            if (!branchspelltdes.IsActive || branchspelltdes.IsBlocked)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_ISNOTACTIVE);
                return View();
            }
            User2WorkPlaceTDES user2workplacetdes = await (from e in dbSales.User2WorkPlaceTDES
                                                           where e.WorkPlaceGuid == salebaskettdes.WorkPlaceGuid
                                                           select e).FirstOrDefaultAsync();
            if (user2workplacetdes == null)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return View();
            }
            if (user2workplacetdes.EntUserNic != salebaskettdes.EntUserNic)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return View();
            }
            ReturnBasketArticleTmp salebasketarticletmp = new ReturnBasketArticleTmp();
            salebasketarticletmp.CopyFrom(salebasketarticletdes);
            base.ViewBag.SearchEntUserNic = salebasketarticletmp.EntUserNic;
            base.ViewBag.SearchRetBasketGuid = salebasketarticletmp.RetBasketGuid;
            return View(salebasketarticletmp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Edit(ReturnBasketArticleTmp salebasketarticletmp)
        {
            await CreateUpdateCheckUp(salebasketarticletmp);
            if (base.ModelState.IsValid)
            {
                ReturnBasketArticleTDES salebasketarticletdes = await GetReturnBasketArticleTDES(salebasketarticletmp.EntArticle, salebasketarticletmp.EntBrandNic, salebasketarticletmp.EntBasketGuid, salebasketarticletmp.RetBasketGuid, salebasketarticletmp.EntGuid, salebasketarticletmp.EntBranchGuid, salebasketarticletmp.EntUserNic);
                ReturnBasketTDES salebaskettdes = base.ViewBag.salebaskettdes;
                salebaskettdes.ArtAmount = salebaskettdes.ArtAmount + salebasketarticletmp.ArtAmount - salebasketarticletdes.ArtAmount;
                salebaskettdes.PaymentSum = salebaskettdes.PaymentSum + (double)salebasketarticletmp.ArtAmount * salebasketarticletmp.SalePrice - (double)salebasketarticletdes.ArtAmount * salebasketarticletdes.SalePrice;
                salebasketarticletmp.CopyTo(salebasketarticletdes, doCreateDescr: false);
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchRetBasketGuid = salebasketarticletmp.RetBasketGuid,
                    searchEntGuid = salebasketarticletmp.EntGuid,
                    searchEntBranchGuid = salebasketarticletmp.EntBranchGuid,
                    searchEntUserNic = salebasketarticletmp.EntUserNic
                });
            }
            return View(salebasketarticletmp);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Delete(string EntArticle = null, string EntBrandNic = null, Guid? searchEntBasketGuid = default(Guid?), Guid? searchRetBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            ReturnBasketArticleTDES salebasketarticletdes = await GetReturnBasketArticleTDES(EntArticle, EntBrandNic, searchEntBasketGuid, searchRetBasketGuid, searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            if (salebasketarticletdes == null)
            {
                return HttpNotFound();
            }
            ReturnBasketTDES salebaskettdes = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                            where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                            select e).FirstOrDefaultAsync();
                }
                else
                {
                    salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                            where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                            select e).FirstOrDefaultAsync();
                }
            }
            else
            {
                salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                        where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid
                                        select e).FirstOrDefaultAsync();
            }
            if (salebaskettdes == null)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_NOTDEFINED);
                return View();
            }
            if (salebaskettdes.IsPaid || salebaskettdes.IsReverse)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_ISPAYED_OR_REVERSED);
                return View();
            }
            BranchSpellTDES branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                                     where e.EntBranchGuid == salebaskettdes.EntBranchGuid
                                                     select e).FirstOrDefaultAsync();
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_NOTFOUND);
                return View();
            }
            if (branchspelltdes.SpellGuid != salebaskettdes.SpellGuid)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_ISCREATED_IN_ANOTHERSPELL);
                return View();
            }
            if (!branchspelltdes.IsActive || branchspelltdes.IsBlocked)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_ISNOTACTIVE);
                return View();
            }
            User2WorkPlaceTDES user2workplacetdes = await (from e in dbSales.User2WorkPlaceTDES
                                                           where e.WorkPlaceGuid == salebaskettdes.WorkPlaceGuid
                                                           select e).FirstOrDefaultAsync();
            if (user2workplacetdes == null)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return View();
            }
            if (user2workplacetdes.EntUserNic != salebaskettdes.EntUserNic)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return View();
            }
            ReturnBasketArticleTmp salebasketarticletmp = new ReturnBasketArticleTmp();
            salebasketarticletmp.CopyFrom(salebasketarticletdes);
            base.ViewBag.SearchEntUserNic = salebasketarticletmp.EntUserNic;
            base.ViewBag.SearchRetBasketGuid = salebasketarticletmp.RetBasketGuid;
            return View(salebasketarticletmp);
        }

        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> DeleteConfirmed(string EntArticle = null, string EntBrandNic = null, Guid? searchEntBasketGuid = default(Guid?), Guid? searchRetBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            ReturnBasketArticleTDES salebasketarticletdes = await GetReturnBasketArticleTDES(EntArticle, EntBrandNic, searchEntBasketGuid, searchRetBasketGuid, searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            if (salebasketarticletdes == null)
            {
                return HttpNotFound();
            }
            ReturnBasketTDES salebaskettdes = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                            where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                            select e).FirstOrDefaultAsync();
                }
                else
                {
                    salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                            where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                            select e).FirstOrDefaultAsync();
                }
            }
            else
            {
                salebaskettdes = await (from e in dbSales.ReturnBasketTDES
                                        where (Guid)(Guid?)e.RetBasketGuid == (Guid)searchRetBasketGuid
                                        select e).FirstOrDefaultAsync();
            }
            if (salebaskettdes == null)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_NOTDEFINED);
                return View();
            }
            if (salebaskettdes.IsPaid || salebaskettdes.IsReverse)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_ISPAYED_OR_REVERSED);
                return View();
            }
            BranchSpellTDES branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                                     where e.EntBranchGuid == salebaskettdes.EntBranchGuid
                                                     select e).FirstOrDefaultAsync();
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_NOTFOUND);
                return View();
            }
            if (branchspelltdes.SpellGuid != salebaskettdes.SpellGuid)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_ISCREATED_IN_ANOTHERSPELL);
                return View();
            }
            if (!branchspelltdes.IsActive || branchspelltdes.IsBlocked)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_ISNOTACTIVE);
                return View();
            }
            User2WorkPlaceTDES user2workplacetdes = await (from e in dbSales.User2WorkPlaceTDES
                                                           where e.WorkPlaceGuid == salebaskettdes.WorkPlaceGuid
                                                           select e).FirstOrDefaultAsync();
            if (user2workplacetdes == null)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return View();
            }
            if (user2workplacetdes.EntUserNic != salebaskettdes.EntUserNic)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return View();
            }
            if (base.ModelState.IsValid)
            {
                salebaskettdes.ArtAmount -= salebasketarticletdes.ArtAmount;
                salebaskettdes.PaymentSum -= (double)salebasketarticletdes.ArtAmount * salebasketarticletdes.SalePrice;
                dbSales.ReturnBasketArticleTDES.Remove(salebasketarticletdes);
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchRetBasketGuid = salebasketarticletdes.RetBasketGuid,
                    searchEntGuid = salebasketarticletdes.EntGuid,
                    searchEntBranchGuid = salebasketarticletdes.EntBranchGuid,
                    searchEntUserNic = salebasketarticletdes.EntUserNic
                });
            }
            ReturnBasketArticleTmp salebasketarticletmp = new ReturnBasketArticleTmp();
            salebasketarticletmp.CopyFrom(salebasketarticletdes);
            base.ViewBag.SearchEntUserNic = salebasketarticletmp.EntUserNic;
            base.ViewBag.SearchEntBasketGuid = salebasketarticletmp.EntBasketGuid;
            return View(salebasketarticletmp);
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbSales != null)
            {
                _dbSales.Dispose();
                _dbSales = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}