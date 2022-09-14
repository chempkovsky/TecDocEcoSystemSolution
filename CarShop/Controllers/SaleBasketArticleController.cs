// CarShop.Controllers.SaleBasketArticleController
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

    public class SaleBasketArticleController : Controller
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

        public async Task<ActionResult> Index(Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null, int? page = default(int?))
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
            IPagedList<SaleBasketArticleTDES> aResult = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                SaleBasketTDES salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                                       where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic && (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
                                                       select e).FirstOrDefaultAsync();
                if (salebaskettdes != null)
                {
                    base.ViewBag.SearchEntBasketGuid = salebaskettdes.EntBasketGuid;
                    base.ViewBag.SaleBasket = salebaskettdes;
                    IOrderedQueryable<SaleBasketArticleTDES> salebasketarticletdes = from e in dbSales.SaleBasketArticleTDES.Include("SaleArticleDescriptionTDES")
                                                                                     where e.EntBasketGuid == salebaskettdes.EntBasketGuid
                                                                                     orderby e.EntArticle, e.EntBrandNic
                                                                                     select e;
                    int pageSize = 20;
                    int pageNumber = page ?? 1;
                    aResult = await salebasketarticletdes.ToPagedListAsync(pageNumber, pageSize);
                }
            }
            return View(aResult);
        }

        protected async Task<SaleBasketArticleTDES> GetSaleBasketArticleTDES(string EntArticle = null, string EntBrandNic = null, Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            SaleBasketArticleTDES aResult = null;
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
                aResult = await (from e in dbSales.SaleBasketArticleTDES.Include("SaleArticleDescriptionTDES")
                                 where e.EntArticle == EntArticle && e.EntBrandNic == EntBrandNic && (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                 select e).FirstOrDefaultAsync();
            }
            return aResult;
        }

        protected async Task CreateUpdateCheckUp(SaleBasketArticleTmp salebasketarticletmp)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            string searchEntUserNic = null;
            Guid? searchEntBasketGuid = null;
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
            searchEntBasketGuid = salebasketarticletmp.EntBasketGuid;
            if (base.ModelState.IsValid && (searchEntGuid != salebasketarticletmp.EntGuid || searchEntBranchGuid != salebasketarticletmp.EntBranchGuid || searchEntUserNic != salebasketarticletmp.EntUserNic))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return;
            }
            base.ViewBag.SearchEntUserNic = salebasketarticletmp.EntUserNic;
            base.ViewBag.SearchEntBasketGuid = salebasketarticletmp.EntBasketGuid;
            SaleBasketTDES salebaskettdes = null;
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
                        salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                                where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
                    }
                    else
                    {
                        salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                                where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                                select e).FirstOrDefaultAsync();
                    }
                }
                else
                {
                    salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                            where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
                                            select e).FirstOrDefaultAsync();
                }
            }
            if (salebaskettdes == null)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_NOTDEFINED);
                return;
            }
            if (salebaskettdes.IsPaid || salebaskettdes.IsReverse)
            {
                base.ModelState.AddModelError("", Resources.SaleBasket_ISPAYED_OR_REVERSED);
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
                base.ModelState.AddModelError("", Resources.SaleBasket_ISCREATED_IN_ANOTHERSPELL);
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
            }
            else if (user2workplacetdes.EntUserNic != salebaskettdes.EntUserNic)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
            }
        }

        public async Task<ActionResult> Details(string EntArticle = null, string EntBrandNic = null, Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            SaleBasketArticleTDES salebasketarticletdes = await GetSaleBasketArticleTDES(EntArticle, EntBrandNic, searchEntBasketGuid, searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            if (salebasketarticletdes == null)
            {
                return HttpNotFound();
            }
            SaleBasketArticleTmp salebasketarticletmp = new SaleBasketArticleTmp();
            salebasketarticletmp.CopyFrom(salebasketarticletdes);
            base.ViewBag.SearchEntUserNic = salebasketarticletdes.EntUserNic;
            base.ViewBag.SearchEntBasketGuid = salebasketarticletdes.EntBasketGuid;
            return View(salebasketarticletmp);
        }

        public async Task<ActionResult> Create(Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
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
            SaleBasketTDES salebaskettdes = null;
            BranchSpellTDES branchspelltdes = null;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (CarShopSalesContextCatalog != null)
            {
                if ((!base.ViewBag.IsEcoSystemAdmin))
                {
                    if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                    {
                        salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                                where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                select e).FirstOrDefaultAsync();
                    }
                    else
                    {
                        salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                                where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                                select e).FirstOrDefaultAsync();
                    }
                }
                else
                {
                    salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                            where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
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
            SaleBasketArticleTmp salebasketarticletmp = new SaleBasketArticleTmp
            {
                EntGuid = salebaskettdes.EntGuid,
                EntBranchGuid = salebaskettdes.EntBranchGuid,
                EntBasketGuid = salebaskettdes.EntBasketGuid,
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
            base.ViewBag.SearchEntBasketGuid = salebaskettdes.EntBasketGuid;
            return View(salebasketarticletmp);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(SaleBasketArticleTmp salebasketarticletmp, string EnterpriseArticleLookUp = null, string TecDocArticleLookUp = null, string TecDocTreeLookUp = null)
        {
            if (TecDocArticleLookUp != null)
            {
                return RedirectToAction("Index", "TecDocRests", new
                {

                });
            }
            if (TecDocTreeLookUp != null)
            {
                return RedirectToAction("Manufact", "TecDocRests", new
                {

                });
            }
            if (EnterpriseArticleLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(salebasketarticletmp);
                return RedirectToAction("Index", "EnterpriseArticleAndRest", new
                {
                    redirecData = redirecData,
                    redirectContriller = "SaleBasketArticle",
                    redirectAction = "GetArticuleRestForCreate",
                    searchEntGuid = salebasketarticletmp.EntGuid,
                    searchEntBranchGuid = salebasketarticletmp.EntBranchGuid,
                    searchString = salebasketarticletmp.EntArticle,
                    searchStringBy = 1
                });
            }
            await CreateUpdateCheckUp(salebasketarticletmp);
            if (base.ModelState.IsValid)
            {
                SaleBasketArticleTDES salebasketarticletdes = await GetSaleBasketArticleTDES(salebasketarticletmp.EntArticle, salebasketarticletmp.EntBrandNic, salebasketarticletmp.EntBasketGuid, salebasketarticletmp.EntGuid, salebasketarticletmp.EntBranchGuid, salebasketarticletmp.EntUserNic);
                SaleBasketTDES salebaskettdes = base.ViewBag.salebaskettdes;
                if (salebasketarticletdes != null)
                {
                    salebaskettdes.ArtAmount += salebasketarticletmp.ArtAmount;
                    salebaskettdes.PaymentSum -= (double)salebasketarticletdes.ArtAmount * salebasketarticletdes.SalePrice;
                    salebasketarticletdes.ArtAmount += salebasketarticletmp.ArtAmount;
                    salebasketarticletdes.SalePrice = salebasketarticletmp.SalePrice;
                    salebaskettdes.PaymentSum += (double)salebasketarticletdes.ArtAmount * salebasketarticletdes.SalePrice;
                }
                else
                {
                    salebasketarticletdes = new SaleBasketArticleTDES();
                    salebasketarticletmp.CopyTo(salebasketarticletdes, doCreateDescr: false);
                    SaleArticleDescriptionTDES desr = await (from e in dbSales.SaleArticleDescriptionTDES
                                                             where e.EntArticleDescription == salebasketarticletmp.EntArticleDescription
                                                             select e).FirstOrDefaultAsync();
                    if (desr == null)
                    {
                        desr = new SaleArticleDescriptionTDES
                        {
                            EntArticleDescription = salebasketarticletmp.EntArticleDescription
                        };
                        dbSales.SaleArticleDescriptionTDES.Add(desr);
                        await dbSales.SaveChangesAsync();
                    }
                    salebasketarticletdes.EntArticleDescriptionId = desr.EntArticleDescriptionId;
                    salebaskettdes.PaymentSum += (double)salebasketarticletdes.ArtAmount * salebasketarticletdes.SalePrice;
                    salebaskettdes.ArtAmount += salebasketarticletmp.ArtAmount;
                    dbSales.SaleBasketArticleTDES.Add(salebasketarticletdes);
                }
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntBasketGuid = salebasketarticletmp.EntBasketGuid,
                    searchEntGuid = salebasketarticletmp.EntGuid,
                    searchEntBranchGuid = salebasketarticletmp.EntBranchGuid,
                    searchEntUserNic = salebasketarticletmp.EntUserNic
                });
            }
            return View(salebasketarticletmp);
        }

        public async Task<ActionResult> Edit(string EntArticle = null, string EntBrandNic = null, Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            SaleBasketArticleTDES salebasketarticletdes = await GetSaleBasketArticleTDES(EntArticle, EntBrandNic, searchEntBasketGuid, searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            if (salebasketarticletdes == null)
            {
                return HttpNotFound();
            }
            SaleBasketTDES salebaskettdes = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                            where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                            select e).FirstOrDefaultAsync();
                }
                else
                {
                    salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                            where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                            select e).FirstOrDefaultAsync();
                }
            }
            else
            {
                salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                        where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
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
            SaleBasketArticleTmp salebasketarticletmp = new SaleBasketArticleTmp();
            salebasketarticletmp.CopyFrom(salebasketarticletdes);
            base.ViewBag.SearchEntUserNic = salebasketarticletmp.EntUserNic;
            base.ViewBag.SearchEntBasketGuid = salebasketarticletmp.EntBasketGuid;
            return View(salebasketarticletmp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SaleBasketArticleTmp salebasketarticletmp)
        {
            await CreateUpdateCheckUp(salebasketarticletmp);
            if (base.ModelState.IsValid)
            {
                SaleBasketArticleTDES salebasketarticletdes = await GetSaleBasketArticleTDES(salebasketarticletmp.EntArticle, salebasketarticletmp.EntBrandNic, salebasketarticletmp.EntBasketGuid, salebasketarticletmp.EntGuid, salebasketarticletmp.EntBranchGuid, salebasketarticletmp.EntUserNic);
                SaleBasketTDES salebaskettdes = base.ViewBag.salebaskettdes;
                SaleArticleDescriptionTDES desr = await (from e in dbSales.SaleArticleDescriptionTDES
                                                         where e.EntArticleDescription == salebasketarticletmp.EntArticleDescription
                                                         select e).FirstOrDefaultAsync();
                if (desr == null)
                {
                    desr = new SaleArticleDescriptionTDES
                    {
                        EntArticleDescription = salebasketarticletmp.EntArticleDescription
                    };
                    dbSales.SaleArticleDescriptionTDES.Add(desr);
                    await dbSales.SaveChangesAsync();
                }
                salebaskettdes.ArtAmount = salebaskettdes.ArtAmount + salebasketarticletmp.ArtAmount - salebasketarticletdes.ArtAmount;
                salebaskettdes.PaymentSum = salebaskettdes.PaymentSum + (double)salebasketarticletmp.ArtAmount * salebasketarticletmp.SalePrice - (double)salebasketarticletdes.ArtAmount * salebasketarticletdes.SalePrice;
                salebasketarticletmp.CopyTo(salebasketarticletdes, doCreateDescr: false);
                salebasketarticletdes.EntArticleDescriptionId = desr.EntArticleDescriptionId;
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntBasketGuid = salebasketarticletmp.EntBasketGuid,
                    searchEntGuid = salebasketarticletmp.EntGuid,
                    searchEntBranchGuid = salebasketarticletmp.EntBranchGuid,
                    searchEntUserNic = salebasketarticletmp.EntUserNic
                });
            }
            return View(salebasketarticletmp);
        }

        public async Task<ActionResult> Delete(string EntArticle = null, string EntBrandNic = null, Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            SaleBasketArticleTDES salebasketarticletdes = await GetSaleBasketArticleTDES(EntArticle, EntBrandNic, searchEntBasketGuid, searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            if (salebasketarticletdes == null)
            {
                return HttpNotFound();
            }
            SaleBasketTDES salebaskettdes = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                            where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                            select e).FirstOrDefaultAsync();
                }
                else
                {
                    salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                            where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                            select e).FirstOrDefaultAsync();
                }
            }
            else
            {
                salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                        where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
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
            SaleBasketArticleTmp salebasketarticletmp = new SaleBasketArticleTmp();
            salebasketarticletmp.CopyFrom(salebasketarticletdes);
            base.ViewBag.SearchEntUserNic = salebasketarticletmp.EntUserNic;
            base.ViewBag.SearchEntBasketGuid = salebasketarticletmp.EntBasketGuid;
            return View(salebasketarticletmp);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string EntArticle = null, string EntBrandNic = null, Guid? searchEntBasketGuid = default(Guid?), Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), string searchEntUserNic = null)
        {
            SaleBasketArticleTDES salebasketarticletdes = await GetSaleBasketArticleTDES(EntArticle, EntBrandNic, searchEntBasketGuid, searchEntGuid, searchEntBranchGuid, searchEntUserNic);
            if (salebasketarticletdes == null)
            {
                return HttpNotFound();
            }
            SaleBasketTDES salebaskettdes = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                            where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                            select e).FirstOrDefaultAsync();
                }
                else
                {
                    salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                            where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                            select e).FirstOrDefaultAsync();
                }
            }
            else
            {
                salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                        where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
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
                dbSales.SaleBasketArticleTDES.Remove(salebasketarticletdes);
                await dbSales.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntBasketGuid = salebasketarticletdes.EntBasketGuid,
                    searchEntGuid = salebasketarticletdes.EntGuid,
                    searchEntBranchGuid = salebasketarticletdes.EntBranchGuid,
                    searchEntUserNic = salebasketarticletdes.EntUserNic
                });
            }
            SaleBasketArticleTmp salebasketarticletmp = new SaleBasketArticleTmp();
            salebasketarticletmp.CopyFrom(salebasketarticletdes);
            base.ViewBag.SearchEntUserNic = salebasketarticletmp.EntUserNic;
            base.ViewBag.SearchEntBasketGuid = salebasketarticletmp.EntBasketGuid;
            return View(salebasketarticletmp);
        }

        protected async Task<ActionResult> GetArticuleRestForCreateUpdate(string redirecData = null, string EntArticle = null, string EntBrandNic = null, Guid? EntBranchGuid = default(Guid?), string EntArticleDescription = null, int? EntArticleDescriptionId = default(int?), string ExternArticle = null, string ExternBrandNic = null, string ExternArticleEAN = null, double? ArtPrice = default(double?), string actionName = null)
        {
            SaleBasketArticleTmp salebasketarticletmp;
            if (redirecData != null)
            {
                try
                {
                    salebasketarticletmp = JsonConvert.DeserializeObject<SaleBasketArticleTmp>(redirecData);
                }
                catch
                {
                    salebasketarticletmp = new SaleBasketArticleTmp
                    {
                        ReverseAmount = 0,
                        ArtAmount = 0,
                        PaidAt = DateTime.Now
                    };
                }
            }
            else
            {
                salebasketarticletmp = new SaleBasketArticleTmp
                {
                    ReverseAmount = 0,
                    ArtAmount = 0,
                    PaidAt = DateTime.Now
                };
            }
            if (EntArticle != null)
            {
                salebasketarticletmp.EntArticle = EntArticle;
            }
            if (EntBrandNic != null)
            {
                salebasketarticletmp.EntBrandNic = EntBrandNic;
            }
            if (EntBranchGuid.HasValue)
            {
                salebasketarticletmp.EntBranchGuid = EntBranchGuid.Value;
            }
            if (EntArticleDescription != null)
            {
                salebasketarticletmp.EntArticleDescription = EntArticleDescription;
            }
            if (EntArticleDescriptionId.HasValue)
            {
                salebasketarticletmp.EntArticleDescriptionId = EntArticleDescriptionId.Value;
            }
            if (ExternArticle != null)
            {
                salebasketarticletmp.ExternArticle = ExternArticle;
            }
            if (ExternBrandNic != null)
            {
                salebasketarticletmp.ExternBrandNic = ExternBrandNic;
            }
            if (ExternArticleEAN != null)
            {
                salebasketarticletmp.ExternArticleEAN = ExternArticleEAN;
            }
            if (ArtPrice.HasValue)
            {
                salebasketarticletmp.SalePrice = ArtPrice.Value;
            }
            return View(actionName, await DefineOtherField(salebasketarticletmp));
        }

        protected async Task<SaleBasketArticleTmp> DefineOtherField(SaleBasketArticleTmp salebasketarticletmp)
        {
            UserIsInRoles();
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            string searchEntUserNic = null;
            Guid? searchEntBasketGuid = null;
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
            searchEntBasketGuid = salebasketarticletmp.EntBasketGuid;
            if (searchEntBranchGuid.HasValue)
            {
                salebasketarticletmp.EntBranchGuid = searchEntBranchGuid.Value;
            }
            if (searchEntGuid.HasValue)
            {
                salebasketarticletmp.EntGuid = searchEntGuid.Value;
            }
            salebasketarticletmp.EntUserNic = searchEntUserNic;
            base.ViewBag.SearchEntUserNic = salebasketarticletmp.EntUserNic;
            base.ViewBag.SearchEntBasketGuid = salebasketarticletmp.EntBasketGuid;
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return salebasketarticletmp;
            }
            BranchSpellTDES branchspelltdes = await (from e in dbSales.BranchSpellTDES
                                                     where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                     select e).FirstOrDefaultAsync();
            if (branchspelltdes == null)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_NOTFOUND);
                return salebasketarticletmp;
            }
            if (!branchspelltdes.IsActive || branchspelltdes.IsBlocked)
            {
                base.ModelState.AddModelError("", Resources.BranchSpellTDES_ISNOTACTIVE);
                return salebasketarticletmp;
            }
            User2WorkPlaceTDES user2workplacetdes = await (from e in dbSales.User2WorkPlaceTDES
                                                           where e.EntUserNic == salebasketarticletmp.EntUserNic
                                                           select e).FirstOrDefaultAsync();
            if (user2workplacetdes == null)
            {
                base.ModelState.AddModelError("", Resources.User2WorkPlaceTDES_NOTFOUND);
                return salebasketarticletmp;
            }
            SaleBasketTDES salebaskettdes = (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false)) ? (await (from e in dbSales.SaleBasketTDES
                                                                                                           where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid
                                                                                                           select e).FirstOrDefaultAsync()) : ((!((base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit) ? true : false)) ? (await (from e in dbSales.SaleBasketTDES
                                                                                                                                                                                                                                                where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.EntUserNic == searchEntUserNic
                                                                                                                                                                                                                                                select e).FirstOrDefaultAsync()) : (await (from e in dbSales.SaleBasketTDES
                                                                                                                                                                                                                                                                                           where (Guid)(Guid?)e.EntBasketGuid == (Guid)searchEntBasketGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                                                                                                                                                                                                                                                           select e).FirstOrDefaultAsync()));
            if (salebaskettdes != null && (salebaskettdes.WorkPlaceGuid != user2workplacetdes.WorkPlaceGuid || salebaskettdes.EntUserNic != searchEntUserNic || salebaskettdes.SpellGuid != branchspelltdes.SpellGuid || salebaskettdes.EntGuid != branchspelltdes.EntGuid || salebaskettdes.IsPaid))
            {
                salebaskettdes = null;
            }
            if (salebaskettdes == null)
            {
                salebaskettdes = await (from e in dbSales.SaleBasketTDES
                                        where e.SpellGuid == branchspelltdes.SpellGuid && e.EntUserNic == searchEntUserNic && e.IsActive == true && e.IsPaid == false
                                        select e).FirstOrDefaultAsync();
                if (salebaskettdes == null)
                {
                    IQueryable<SaleBasketTDES> ActiveSalebaskettdes = from e in dbSales.SaleBasketTDES
                                                                      where e.EntUserNic == salebasketarticletmp.EntUserNic && e.EntGuid == salebasketarticletmp.EntGuid && e.EntBranchGuid == salebasketarticletmp.EntBranchGuid && e.SpellGuid == branchspelltdes.SpellGuid && e.IsActive == true
                                                                      select e;
                    foreach (SaleBasketTDES item in ActiveSalebaskettdes)
                    {
                        item.IsActive = false;
                        dbSales.Entry(item).State = EntityState.Modified;
                    }
                    await dbSales.SaveChangesAsync();
                    salebaskettdes = new SaleBasketTDES
                    {
                        EntBasketGuid = Guid.NewGuid(),
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
                    dbSales.SaleBasketTDES.Add(salebaskettdes);
                    await dbSales.SaveChangesAsync();
                }
            }
            salebasketarticletmp.EntBasketGuid = salebaskettdes.EntBasketGuid;
            salebasketarticletmp.EntUserNic = salebaskettdes.EntUserNic;
            salebasketarticletmp.EntGuid = salebaskettdes.EntGuid;
            salebasketarticletmp.EntBranchGuid = salebaskettdes.EntBranchGuid;
            salebasketarticletmp.WorkPlaceGuid = salebaskettdes.WorkPlaceGuid;
            salebasketarticletmp.SpellGuid = salebaskettdes.SpellGuid;
            base.ViewBag.SearchEntUserNic = salebasketarticletmp.EntUserNic;
            base.ViewBag.SearchEntBasketGuid = salebasketarticletmp.EntBasketGuid;
            return salebasketarticletmp;
        }

        public async Task<ActionResult> GetArticuleRestForCreate(string redirecData = null, string EntArticle = null, string EntBrandNic = null, Guid? EntBranchGuid = default(Guid?), string EntArticleDescription = null, int? EntArticleDescriptionId = default(int?), string ExternArticle = null, string ExternBrandNic = null, string ExternArticleEAN = null, double? ArtPrice = default(double?))
        {
            return await GetArticuleRestForCreateUpdate(redirecData, EntArticle, EntBrandNic, EntBranchGuid, EntArticleDescription, EntArticleDescriptionId, ExternArticle, ExternBrandNic, ExternArticleEAN, ArtPrice, "Create");
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