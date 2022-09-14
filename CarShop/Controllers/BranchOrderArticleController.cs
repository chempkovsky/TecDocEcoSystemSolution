// CarShop.Controllers.BranchOrderArticleController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class BranchOrderArticleController : Controller
    {
        private CarShopContext db = new CarShopContext();

        private string CarShopOrderContextCatalog;

        private CarShopOrdersContext _dbOrders;

        private CarShopOrdersContext dbOrders
        {
            get
            {
                if (_dbOrders == null)
                {
                    _dbOrders = new CarShopOrdersContext(CarShopOrderContextCatalog);
                }
                return _dbOrders;
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
                CarShopOrderContextCatalog = enterprisebranchtdes.OrderCatalog;
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
                base.ViewBag.SearchEntGuid = searchEntGuid;
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
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchGuestOrderGuid = default(Guid?))
        {
            base.ViewBag.CanBeModified = false;
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
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.SearchGestUserNic = guestordertdes.GestUserNic;
            base.ViewBag.SearchGuestOrderGuid = guestordertdes.GuestOrderGuid;
            base.ViewBag.CanBeModified = !guestordertdes.IsDone;
            return View(await (from e in dbOrders.GuestOrderArticleTDES
                               where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid
                               select e).ToListAsync());
        }

        public async Task<ActionResult> Details(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchGuestOrderGuid = default(Guid?), string searchEntBranchArticle = null, string searchEntBranchSup = null)
        {
            base.ViewBag.CanBeModified = false;
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
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.SearchGestUserNic = guestordertdes.GestUserNic;
            base.ViewBag.SearchGuestOrderGuid = guestordertdes.GuestOrderGuid;
            base.ViewBag.CanBeModified = !guestordertdes.IsDone;
            GuestOrderArticleTDES guestorderarticletdes = await (from e in dbOrders.GuestOrderArticleTDES
                                                                 where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup
                                                                 select e).FirstOrDefaultAsync();
            if (guestorderarticletdes == null)
            {
                return HttpNotFound();
            }
            return View(guestorderarticletdes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GuestOrderArticleTDES guestorderarticletdes)
        {
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if (guestorderarticletdes != null)
            {
                searchEntBranchGuid = guestorderarticletdes.EntBranchGuid;
            }
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
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (string.IsNullOrEmpty(CarShopOrderContextCatalog))
            {
                return View(guestorderarticletdes);
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where e.GuestOrderGuid == guestorderarticletdes.GuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return View(guestorderarticletdes);
            }
            base.ViewBag.SearchGestUserNic = guestordertdes.GestUserNic;
            base.ViewBag.SearchGuestOrderGuid = guestordertdes.GuestOrderGuid;
            if (base.ModelState.IsValid)
            {
                GuestOrderArticleTDES oldItem = await (from e in dbOrders.GuestOrderArticleTDES
                                                       where e.GuestOrderGuid == guestorderarticletdes.GuestOrderGuid && e.EntBranchArticle == guestorderarticletdes.EntBranchArticle && e.EntBranchSup == guestorderarticletdes.EntBranchSup
                                                       select e).FirstOrDefaultAsync();
                if (oldItem != null)
                {
                    oldItem.ArtAmount += guestorderarticletdes.ArtAmount;
                }
                else
                {
                    dbOrders.GuestOrderArticleTDES.Add(guestorderarticletdes);
                }
                await dbOrders.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = searchEntGuid,
                    searchEntBranchGuid = searchEntBranchGuid,
                    searchGuestOrderGuid = guestorderarticletdes.GuestOrderGuid
                });
            }
            return View(guestorderarticletdes);
        }

        public async Task<ActionResult> Edit(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchGuestOrderGuid = default(Guid?), string searchEntBranchArticle = null, string searchEntBranchSup = null)
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
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.SearchGestUserNic = guestordertdes.GestUserNic;
            base.ViewBag.SearchGuestOrderGuid = guestordertdes.GuestOrderGuid;
            base.ViewBag.CanBeModified = !guestordertdes.IsDone;
            GuestOrderArticleTDES guestorderarticletdes = await (from e in dbOrders.GuestOrderArticleTDES
                                                                 where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup
                                                                 select e).FirstOrDefaultAsync();
            if (guestorderarticletdes == null)
            {
                return HttpNotFound();
            }
            return View(guestorderarticletdes);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(GuestOrderArticleTDES guestorderarticletdes = null)
        {
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if (guestorderarticletdes != null)
            {
                searchEntBranchGuid = guestorderarticletdes.EntBranchGuid;
            }
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
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (string.IsNullOrEmpty(CarShopOrderContextCatalog))
            {
                return View(guestorderarticletdes);
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where e.GuestOrderGuid == guestorderarticletdes.GuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return View(guestorderarticletdes);
            }
            base.ViewBag.SearchGestUserNic = guestordertdes.GestUserNic;
            base.ViewBag.SearchGuestOrderGuid = guestordertdes.GuestOrderGuid;
            if (base.ModelState.IsValid)
            {
                dbOrders.Entry(guestorderarticletdes).State = EntityState.Modified;
                await dbOrders.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = searchEntGuid,
                    searchEntBranchGuid = searchEntBranchGuid,
                    searchGuestOrderGuid = guestorderarticletdes.GuestOrderGuid
                });
            }
            return View(guestorderarticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Delete(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchGuestOrderGuid = default(Guid?), string searchEntBranchArticle = null, string searchEntBranchSup = null)
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
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.SearchGestUserNic = guestordertdes.GestUserNic;
            base.ViewBag.SearchGuestOrderGuid = guestordertdes.GuestOrderGuid;
            base.ViewBag.CanBeModified = !guestordertdes.IsDone;
            GuestOrderArticleTDES guestorderarticletdes = await (from e in dbOrders.GuestOrderArticleTDES
                                                                 where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup
                                                                 select e).FirstOrDefaultAsync();
            if (guestorderarticletdes == null)
            {
                return HttpNotFound();
            }
            return View(guestorderarticletdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchSeller,BranchBooker")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), Guid? searchGuestOrderGuid = default(Guid?), string searchEntBranchArticle = null, string searchEntBranchSup = null)
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
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.SearchGestUserNic = guestordertdes.GestUserNic;
            base.ViewBag.SearchGuestOrderGuid = guestordertdes.GuestOrderGuid;
            base.ViewBag.CanBeModified = !guestordertdes.IsDone;
            GuestOrderArticleTDES guestorderarticletdes = await (from e in dbOrders.GuestOrderArticleTDES
                                                                 where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup
                                                                 select e).FirstOrDefaultAsync();
            dbOrders.GuestOrderArticleTDES.Remove(guestorderarticletdes);
            await dbOrders.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                searchEntBranchGuid,
                searchGuestOrderGuid
            });
        }

        protected async Task<ActionResult> GetArticuleRestForCreateUpdate(string redirecData = null, string EntArticle = null, string EntBrandNic = null, Guid? EntBranchGuid = default(Guid?), string EntArticleDescription = null, int? EntArticleDescriptionId = default(int?), string ExternArticle = null, string ExternBrandNic = null, string ExternArticleEAN = null, double? ArtPrice = default(double?), string actionName = null)
        {
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
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
                    }
                    else
                    {
                        searchEntGuid = searchEntBranchIds.EntGuid;
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (!base.ModelState.IsValid)
            {
                return View(actionName);
            }
            if (string.IsNullOrEmpty(redirecData))
            {
                base.ModelState.AddModelError("", Resources.GestUserNic_NOT_DEFINED);
                return View(actionName);
            }
            if (await (from e in dbOrders.GuestProfileTDES
                       where e.GestUserNic == redirecData
                       select e).FirstOrDefaultAsync() == null)
            {
                base.ModelState.AddModelError("", Resources.GestUserNic_NOT_DEFINED);
                return View(actionName);
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where e.GestUserNic == redirecData && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.IsDone == false && e.IsActive == true
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                        where e.GestUserNic == redirecData && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.IsDone == false && e.IsActive == false
                                        select e).FirstOrDefaultAsync();
            }
            List<GuestOrderTDES> itms = (guestordertdes == null) ? (await (from e in dbOrders.GuestOrderTDES
                                                                           where e.GestUserNic == redirecData && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.IsActive == true
                                                                           select e).ToListAsync()) : (await (from e in dbOrders.GuestOrderTDES
                                                                                                              where e.GestUserNic == redirecData && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.GuestOrderGuid != guestordertdes.GuestOrderGuid && e.IsActive == true
                                                                                                              select e).ToListAsync());
            foreach (GuestOrderTDES item in itms)
            {
                item.IsActive = false;
            }
            await dbOrders.SaveChangesAsync();
            if (guestordertdes == null)
            {
                guestordertdes = new GuestOrderTDES
                {
                    GuestOrderGuid = Guid.NewGuid(),
                    GestUserNic = redirecData,
                    EntBranchGuid = searchEntBranchGuid.Value,
                    EntBranchDescription = (base.ViewBag.EntBranchDescription as string),
                    IsActive = true,
                    IsDone = false,
                    LastUpdated = DateTime.Now
                };
                guestordertdes.LastUpdated = guestordertdes.LastUpdated.AddMilliseconds(-guestordertdes.LastUpdated.Millisecond);
                guestordertdes.LastReplicated = guestordertdes.LastUpdated.AddSeconds(-5.0);
                dbOrders.GuestOrderTDES.Add(guestordertdes);
            }
            else
            {
                guestordertdes.IsActive = true;
                guestordertdes.LastUpdated = DateTime.Now;
                guestordertdes.LastUpdated = guestordertdes.LastUpdated.AddMilliseconds(-guestordertdes.LastUpdated.Millisecond);
            }
            await dbOrders.SaveChangesAsync();
            ArtPrice = (ArtPrice ?? 0.0);
            GuestOrderArticleTDES guestorderarticletdes = new GuestOrderArticleTDES
            {
                GuestOrderGuid = guestordertdes.GuestOrderGuid,
                EntBranchArticle = EntArticle,
                EntBranchSup = EntBrandNic,
                EntArticleDescription = EntArticleDescription,
                ART_ARTICLE_NR = ExternArticle,
                SUP_TEXT = ExternBrandNic,
                ExternArticleEAN = ExternArticleEAN,
                ArtAmount = 1,
                ArtPrice = ArtPrice.Value,
                EntBranchGuid = searchEntBranchGuid.Value,
                LastUpdated = guestordertdes.LastUpdated,
                LastReplicated = guestordertdes.LastUpdated.AddSeconds(-5.0)
            };
            base.ViewBag.SearchGestUserNic = redirecData;
            base.ViewBag.SearchGuestOrderGuid = guestordertdes.GuestOrderGuid;
            return View(actionName, guestorderarticletdes);
        }

        public async Task<ActionResult> GetArticuleRestForCreate(string redirecData = null, string EntArticle = null, string EntBrandNic = null, Guid? EntBranchGuid = default(Guid?), string EntArticleDescription = null, int? EntArticleDescriptionId = default(int?), string ExternArticle = null, string ExternBrandNic = null, string ExternArticleEAN = null, double? ArtPrice = default(double?))
        {
            return await GetArticuleRestForCreateUpdate(redirecData, EntArticle, EntBrandNic, EntBranchGuid, EntArticleDescription, EntArticleDescriptionId, ExternArticle, ExternBrandNic, ExternArticleEAN, ArtPrice, "Create");
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbOrders != null)
            {
                _dbOrders.Dispose();
                _dbOrders = null;
            }
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}