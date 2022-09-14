// CarShop.Controllers.GuestOrderArticleController
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

    public class GuestOrderArticleController : Controller
    {
        private Guid guestEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

        private Guid guestEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);

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

        public async Task ViewBagHelper()
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES
                                                               where e.EntBranchGuid == guestEntBranchGuid && e.EntGuid == guestEntGuid
                                                               select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                if (enterprisebranchtdes.IsActive && enterprisebranchtdes.EnterpriseTDES.IsActive)
                {
                    CarShopOrderContextCatalog = enterprisebranchtdes.OrderCatalog;
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.OrderBranchTDES_ISNOTACTIVE);
                }
            }
            else
            {
                base.ModelState.AddModelError("", Resources.OrderBranchTDES_NOTFOUND);
            }
        }

        [Authorize]
        public async Task<ActionResult> Index(Guid? searchGuestOrderGuid = default(Guid?))
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                base.ViewBag.CanBeModified = false;
                return View();
            }
            object tmpDt = base.TempData["CreateOrUpdate"];
            if (tmpDt != null)
            {
                base.ViewBag.CreateOrUpdate = tmpDt.ToString();
            }
            string aName = base.User.Identity.Name;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.GestUserNic == aName
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes != null)
            {
                base.ViewBag.CanBeModified = (guestordertdes.IsReplicated > 0);
            }
            else
            {
                searchGuestOrderGuid = null;
                base.ViewBag.CanBeModified = false;
            }
            List<GuestOrderArticleTDES> guestorderarticletdes = await (from e in dbOrders.GuestOrderArticleTDES
                                                                       where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid
                                                                       select e).ToListAsync();
            base.ViewBag.GuestOrderGuid = searchGuestOrderGuid;
            return View(guestorderarticletdes);
        }

        [Authorize]
        public async Task<ActionResult> Details(Guid? searchGuestOrderGuid = default(Guid?), string searchEntBranchArticle = null, string searchEntBranchSup = null)
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                base.ViewBag.CanBeModified = false;
                return View();
            }
            string aName = base.User.Identity.Name;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.GestUserNic == aName
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            GuestOrderArticleTDES guestorderarticletdes = await (from e in dbOrders.GuestOrderArticleTDES
                                                                 where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup
                                                                 select e).FirstOrDefaultAsync();
            if (guestorderarticletdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.CanBeModified = (guestordertdes.IsReplicated > 0);
            base.ViewBag.GuestOrderGuid = searchGuestOrderGuid;
            return View(guestorderarticletdes);
        }

        [Authorize]
        public async Task<ActionResult> Create(Guid? searchGuestOrderGuid = default(Guid?))
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            string aName = base.User.Identity.Name;
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.GestUserNic == aName
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                return HttpNotFound();
            }
            GuestOrderArticleTDES guestorderarticletdes = new GuestOrderArticleTDES
            {
                GuestOrderGuid = guestordertdes.GuestOrderGuid,
                EntBranchGuid = guestordertdes.EntBranchGuid,
                LastUpdated = DateTime.Now
            };
            guestorderarticletdes.LastUpdated = guestorderarticletdes.LastUpdated.AddMilliseconds(-guestorderarticletdes.LastUpdated.Millisecond);
            guestorderarticletdes.LastReplicated = guestorderarticletdes.LastUpdated.AddSeconds(-5.0);
            base.ViewBag.GuestOrderGuid = searchGuestOrderGuid;
            return View(guestorderarticletdes);
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(GuestOrderArticleTDES guestorderarticletdes)
        {
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper();
                if (!base.ModelState.IsValid)
                {
                    return View(guestorderarticletdes);
                }
            }
            string aName = base.User.Identity.Name;
            if (base.ModelState.IsValid)
            {
                GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                       where e.GestUserNic == aName && e.GuestOrderGuid == guestorderarticletdes.GuestOrderGuid
                                                       select e).FirstOrDefaultAsync();
                if (guestordertdes == null)
                {
                    base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND + guestorderarticletdes.GuestOrderGuid.ToString());
                    base.ViewBag.GuestOrderGuid = guestorderarticletdes.GuestOrderGuid;
                    return View(guestorderarticletdes);
                }
                if (guestordertdes.IsDone || guestordertdes.IsReplicated <= 0)
                {
                    base.ModelState.AddModelError("", Resources.GuestOrderTDES_ISDONE);
                    base.ViewBag.GuestOrderGuid = guestorderarticletdes.GuestOrderGuid;
                    return View(guestorderarticletdes);
                }
                if (await (from e in dbOrders.GuestOrderArticleTDES
                           where e.GuestOrderGuid == guestorderarticletdes.GuestOrderGuid
                           select e).CountAsync() > 5)
                {
                    base.ModelState.AddModelError("", Resources.GuestOrderArticleTDES_ISMAX);
                    base.ViewBag.GuestOrderGuid = guestorderarticletdes.GuestOrderGuid;
                    return View(guestorderarticletdes);
                }
                if (!guestordertdes.IsActive)
                {
                    foreach (GuestOrderTDES item in await (from e in dbOrders.GuestOrderTDES
                                                           where e.GestUserNic == aName
                                                           select e).ToListAsync())
                    {
                        if (item.IsActive)
                        {
                            item.IsActive = false;
                        }
                    }
                    guestordertdes.IsActive = true;
                    dbOrders.SaveChanges();
                }
                GuestOrderArticleTDES guestorderarticletdesOld = await (from e in dbOrders.GuestOrderArticleTDES
                                                                        where e.GuestOrderGuid == guestorderarticletdes.GuestOrderGuid && e.EntBranchArticle == guestorderarticletdes.EntBranchArticle && e.EntBranchSup == guestorderarticletdes.EntBranchSup
                                                                        select e).FirstOrDefaultAsync();
                if (guestorderarticletdesOld != null)
                {
                    guestorderarticletdesOld.ArtAmount += guestorderarticletdes.ArtAmount;
                }
                else
                {
                    dbOrders.GuestOrderArticleTDES.Add(guestorderarticletdes);
                }
                await dbOrders.SaveChangesAsync();
                base.TempData["CreateOrUpdate"] = "Товар успешно добавлен в корзину";
                return RedirectToAction("Index", new
                {
                    searchGuestOrderGuid = guestorderarticletdes.GuestOrderGuid
                });
            }
            base.ViewBag.GuestOrderGuid = guestorderarticletdes.GuestOrderGuid;
            return View(guestorderarticletdes);
        }

        [Authorize]
        public async Task<ActionResult> Edit(Guid? searchGuestOrderGuid = default(Guid?), string searchEntBranchArticle = null, string searchEntBranchSup = null)
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            string aName = base.User.Identity.Name;
            GuestOrderArticleTDES guestorderarticletdes = await (from e in dbOrders.GuestOrderArticleTDES
                                                                 where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup && e.GuestOrderTDES.GestUserNic == aName
                                                                 select e).FirstOrDefaultAsync();
            if (guestorderarticletdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.GuestOrderGuid = guestorderarticletdes.GuestOrderGuid;
            return View(guestorderarticletdes);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Edit(GuestOrderArticleTDES guestorderarticletdes)
        {
            if (base.ModelState.IsValid)
            {
                await ViewBagHelper();
                if (!base.ModelState.IsValid)
                {
                    return View(guestorderarticletdes);
                }
            }
            string aName = base.User.Identity.Name;
            if (base.ModelState.IsValid)
            {
                GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                       where e.GestUserNic == aName && e.GuestOrderGuid == guestorderarticletdes.GuestOrderGuid
                                                       select e).FirstOrDefaultAsync();
                if (guestordertdes == null)
                {
                    base.ModelState.AddModelError("", Resources.GuestOrderTDES_NOTFOUND + guestorderarticletdes.GuestOrderGuid.ToString());
                    base.ViewBag.GuestOrderGuid = guestorderarticletdes.GuestOrderGuid;
                    return View(guestorderarticletdes);
                }
                if (guestordertdes.IsDone || guestordertdes.IsReplicated <= 0)
                {
                    base.ModelState.AddModelError("", Resources.GuestOrderTDES_ISDONE);
                    base.ViewBag.GuestOrderGuid = guestorderarticletdes.GuestOrderGuid;
                    return View(guestorderarticletdes);
                }
                guestorderarticletdes.LastUpdated = DateTime.Now;
                guestorderarticletdes.LastUpdated = guestorderarticletdes.LastUpdated.AddMilliseconds(-guestorderarticletdes.LastUpdated.Millisecond);
                dbOrders.Entry(guestorderarticletdes).State = EntityState.Modified;
                await dbOrders.SaveChangesAsync();
                base.TempData["CreateOrUpdate"] = "Редакция заказа успешно проведена";
                return RedirectToAction("Index", new
                {
                    searchGuestOrderGuid = guestorderarticletdes.GuestOrderGuid
                });
            }
            base.ViewBag.GuestOrderGuid = guestorderarticletdes.GuestOrderGuid;
            return View(guestorderarticletdes);
        }

        [Authorize]
        public async Task<ActionResult> Delete(Guid? searchGuestOrderGuid = default(Guid?), string searchEntBranchArticle = null, string searchEntBranchSup = null)
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            GuestOrderArticleTDES guestorderarticletdes = await (from e in dbOrders.GuestOrderArticleTDES
                                                                 where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup
                                                                 select e).FirstOrDefaultAsync();
            if (guestorderarticletdes == null)
            {
                return HttpNotFound();
            }
            base.ViewBag.GuestOrderGuid = guestorderarticletdes.GuestOrderGuid;
            return View(guestorderarticletdes);
        }

        [ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(Guid? searchGuestOrderGuid = default(Guid?), string searchEntBranchArticle = null, string searchEntBranchSup = null)
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            string aName = base.User.Identity.Name;
            GuestOrderArticleTDES guestorderarticletdes = await (from e in dbOrders.GuestOrderArticleTDES
                                                                 where (Guid)(Guid?)e.GuestOrderGuid == (Guid)searchGuestOrderGuid && e.EntBranchArticle == searchEntBranchArticle && e.EntBranchSup == searchEntBranchSup && e.GuestOrderTDES.GestUserNic == aName
                                                                 select e).FirstOrDefaultAsync();
            dbOrders.GuestOrderArticleTDES.Remove(guestorderarticletdes);
            await dbOrders.SaveChangesAsync();
            base.TempData["CreateOrUpdate"] = "Удаление товара в корзине успешно проведено";
            return RedirectToAction("Index", new
            {
                searchGuestOrderGuid = guestorderarticletdes.GuestOrderGuid
            });
        }

        [Authorize]
        public async Task<ActionResult> DoCreateArticle(Guid? EntBranchGuid = default(Guid?), string EntBranchArticle = null, string EntBranchSup = null, string ART_ARTICLE_NR = null, string SUP_TEXT = null, int? ArtAmount = default(int?), double? ArtPrice = default(double?), string EntArticleDescription = null, string ExternArticleEAN = null, string BranchName = null)
        {
            await ViewBagHelper();
            if (!base.ModelState.IsValid)
            {
                return RedirectToAction("Index", "GuestOrderArticle");
            }
            string aName = base.User.Identity.Name;
            if (await (from e in dbOrders.GuestProfileTDES
                       where e.GestUserNic == aName
                       select e).FirstOrDefaultAsync() == null)
            {
                return RedirectToAction("Index", "GuestProfile");
            }
            if (!EntBranchGuid.HasValue || string.IsNullOrEmpty(EntBranchArticle) || string.IsNullOrEmpty(EntBranchSup) || string.IsNullOrEmpty(ART_ARTICLE_NR) || string.IsNullOrEmpty(SUP_TEXT) || !ArtAmount.HasValue || !ArtPrice.HasValue || string.IsNullOrEmpty(EntArticleDescription))
            {
                base.ModelState.AddModelError("", Resources.DoSelectArticle_RESTNOTFOUND);
                return View("Create");
            }
            if (string.IsNullOrEmpty(BranchName))
            {
                BranchName = "* * * * *";
            }
            if (await (from e in dbOrders.GuestOrderTDES
                       where e.GestUserNic == aName && e.IsActive == true && e.IsReplicated > 0 && (Guid)(Guid?)e.EntBranchGuid == (Guid)EntBranchGuid && e.GuestOrderArticleTDESes.Count < 20
                       select e).FirstOrDefaultAsync() == null)
            {
                foreach (GuestOrderTDES item in await (from e in dbOrders.GuestOrderTDES
                                                       where e.GestUserNic == aName && e.IsActive == true
                                                       select e).ToListAsync())
                {
                    item.IsActive = false;
                }
                await dbOrders.SaveChangesAsync();
            }
            GuestOrderTDES guestordertdes = await (from e in dbOrders.GuestOrderTDES
                                                   where e.GestUserNic == aName && e.IsReplicated > 0 && (Guid)(Guid?)e.EntBranchGuid == (Guid)EntBranchGuid && e.GuestOrderArticleTDESes.Count < 20
                                                   select e).FirstOrDefaultAsync();
            if (guestordertdes == null)
            {
                if (await (from e in dbOrders.GuestOrderTDES
                           where e.GestUserNic == aName
                           select e).CountAsync() > 7)
                {
                    base.ModelState.AddModelError("", Resources.GuestOrderTDES_ISMAX);
                    return View("Create");
                }
                guestordertdes = new GuestOrderTDES
                {
                    EntBranchGuid = EntBranchGuid.Value,
                    EntBranchDescription = BranchName,
                    GuestOrderGuid = Guid.NewGuid(),
                    GestUserNic = aName,
                    IsActive = true,
                    IsDone = false,
                    LastUpdated = DateTime.Now
                };
                guestordertdes.LastUpdated = guestordertdes.LastUpdated.AddMilliseconds(-guestordertdes.LastUpdated.Millisecond);
                guestordertdes.LastReplicated = guestordertdes.LastUpdated.AddSeconds(-5.0);
                dbOrders.GuestOrderTDES.Add(guestordertdes);
                await dbOrders.SaveChangesAsync();
            }
            else if (!guestordertdes.IsActive)
            {
                guestordertdes.IsActive = true;
                dbOrders.SaveChanges();
            }
            GuestOrderArticleTDES guestorderarticletdes = new GuestOrderArticleTDES
            {
                GuestOrderGuid = guestordertdes.GuestOrderGuid,
                EntBranchArticle = EntBranchArticle,
                EntBranchSup = EntBranchSup,
                EntArticleDescription = EntArticleDescription,
                ART_ARTICLE_NR = ART_ARTICLE_NR,
                SUP_TEXT = SUP_TEXT,
                ExternArticleEAN = ExternArticleEAN,
                ArtAmount = ArtAmount.Value,
                ArtPrice = ArtPrice.Value,
                EntBranchGuid = EntBranchGuid.Value,
                LastUpdated = DateTime.Now
            };
            guestorderarticletdes.LastUpdated = guestorderarticletdes.LastUpdated.AddMilliseconds(-guestorderarticletdes.LastUpdated.Millisecond);
            guestorderarticletdes.LastReplicated = guestorderarticletdes.LastUpdated.AddSeconds(-5.0);
            base.ViewBag.GuestOrderGuid = guestordertdes.GuestOrderGuid;
            return View("Create", guestorderarticletdes);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            if (_dbOrders != null)
            {
                _dbOrders.Dispose();
                _dbOrders = null;
            }
            base.Dispose(disposing);
        }
    }

}