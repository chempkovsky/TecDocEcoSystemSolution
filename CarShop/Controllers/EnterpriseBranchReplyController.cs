// CarShop.Controllers.EnterpriseBranchReplyController
using CarShop.Models;
using CarShop.Properties;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseBranchReplyController : Controller
    {
        private CarShopContext db = new CarShopContext();

        public async Task ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterprisebranchtdes = await (from e in db.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefaultAsync();
            if (enterprisebranchtdes != null)
            {
                base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
                base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
                base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
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

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Index(Guid? searchEntGuid, Guid? searchEntBranchGuid)
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
            IQueryable<EnterpriseBranchReplyTDES> enterprisebranchreplytdes = from e in db.EnterpriseBranchReplyTDES
                                                                              where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                              select e;
            return View(await enterprisebranchreplytdes.ToListAsync());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Details(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), int? searchReplyType = default(int?))
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
            EnterpriseBranchReplyTDES enterprisebranchreplytdes = await (from e in db.EnterpriseBranchReplyTDES
                                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (int?)e.ReplyType == searchReplyType
                                                                         select e).FirstOrDefaultAsync();
            if (enterprisebranchreplytdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisebranchreplytdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?))
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
            EnterpriseBranchReplyTDES enterprisebranchreplytdes = new EnterpriseBranchReplyTDES
            {
                EntBranchGuid = searchEntBranchGuid.Value,
                ReplyType = 1,
                HttpLoginUrl = "api/BranchLogInSvc",
                HttpGetUrl = "api/BranchRestSvc",
                HttpPostUrl = "api/BranchRestSvc",
                HttpPutUrl = "api/BranchRestSvc",
                HttpDeleteUrl = "api/BranchRestSvc"
            };
            base.ViewBag.sliReplyTypes = new SelectList(new SelectListItem[4]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.ReplyRests,
                Selected = true
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.ReplyProfile,
                Selected = false
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.ReplyOrders,
                Selected = false
            },
            new SelectListItem
            {
                Value = "4",
                Text = Resources.ReplyArticles,
                Selected = false
            }
            }, "Value", "Text", 1);
            return View(enterprisebranchreplytdes);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Create(EnterpriseBranchReplyTDES enterprisebranchreplytdes)
        {
            Guid? searchEntGuid = null;
            UserIsInRoles();
            Guid? searchEntBranchGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                    searchEntBranchGuid = enterprisebranchreplytdes.EntBranchGuid;
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
            else
            {
                searchEntBranchGuid = enterprisebranchreplytdes.EntBranchGuid;
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            enterprisebranchreplytdes.EntBranchGuid = searchEntBranchGuid.Value;
            if (base.ModelState.IsValid)
            {
                db.EnterpriseBranchReplyTDES.Add(enterprisebranchreplytdes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchEntBranchGuid = (object)base.ViewBag.SearchEntBranchGuid
                });
            }
            base.ViewBag.sliReplyTypes = new SelectList(new SelectListItem[4]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.ReplyRests,
                Selected = (1 == enterprisebranchreplytdes.ReplyType)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.ReplyProfile,
                Selected = (1 == enterprisebranchreplytdes.ReplyType)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.ReplyOrders,
                Selected = (1 == enterprisebranchreplytdes.ReplyType)
            },
            new SelectListItem
            {
                Value = "4",
                Text = Resources.ReplyArticles,
                Selected = (1 == enterprisebranchreplytdes.ReplyType)
            }
            }, "Value", "Text", enterprisebranchreplytdes.ReplyType);
            return View(enterprisebranchreplytdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Edit(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), int? searchReplyType = default(int?))
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
            EnterpriseBranchReplyTDES enterprisebranchreplytdes = await (from e in db.EnterpriseBranchReplyTDES
                                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (int?)e.ReplyType == searchReplyType
                                                                         select e).FirstOrDefaultAsync();
            if (enterprisebranchreplytdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisebranchreplytdes);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [HttpPost]
        public async Task<ActionResult> Edit(EnterpriseBranchReplyTDES enterprisebranchreplytdes)
        {
            Guid? searchEntGuid = null;
            UserIsInRoles();
            Guid? searchEntBranchGuid;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from e in db.EnterpriseUserTDES
                                           where e.EntUserNic == aName
                                           select e.EntGuid).FirstOrDefaultAsync();
                    searchEntBranchGuid = enterprisebranchreplytdes.EntBranchGuid;
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
            else
            {
                searchEntBranchGuid = enterprisebranchreplytdes.EntBranchGuid;
            }
            await ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisebranchreplytdes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = (object)base.ViewBag.SearchEntGuid,
                    searchEntBranchGuid = (object)base.ViewBag.SearchEntBranchGuid
                });
            }
            return View(enterprisebranchreplytdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        public async Task<ActionResult> Delete(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), int? searchReplyType = default(int?))
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
            EnterpriseBranchReplyTDES enterprisebranchreplytdes = await (from e in db.EnterpriseBranchReplyTDES
                                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (int?)e.ReplyType == searchReplyType
                                                                         select e).FirstOrDefaultAsync();
            if (enterprisebranchreplytdes == null)
            {
                return HttpNotFound();
            }
            return View(enterprisebranchreplytdes);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin,BranchBooker")]
        [ActionName("Delete")]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(Guid? searchEntGuid = default(Guid?), Guid? searchEntBranchGuid = default(Guid?), int? searchReplyType = default(int?))
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
            EnterpriseBranchReplyTDES enterprisebranchreplytdes = await (from e in db.EnterpriseBranchReplyTDES
                                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (int?)e.ReplyType == searchReplyType
                                                                         select e).FirstOrDefaultAsync();
            db.EnterpriseBranchReplyTDES.Remove(enterprisebranchreplytdes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                searchEntBranchGuid
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}