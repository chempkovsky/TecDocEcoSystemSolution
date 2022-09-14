// CarShop.Controllers.TecDocRestsController
using CarShop.Models;
using CarShop.Properties;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{
    public class TecDocRestsController : Controller
    {
        private CarShopContext dbCarShop = new CarShopContext();

        private int TecDocSrcType = 1;

        private TecDocContext _db;

        private string CarShopRestContextCatalog;

        private CarShopRestContext _dbRest;

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

        private CarShopRestContext dbRest
        {
            get
            {
                if (_dbRest == null)
                {
                    _dbRest = new CarShopRestContext(CarShopRestContextCatalog);
                }
                return _dbRest;
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

        public async Task<ActionResult> ReSettings()
        {
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                countryId = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            base.ViewBag.sliLANGUAGES = new SelectList(await db.GetLANGUAGESAsync(langId), "LNG_ID", "TEX_TEXT", langId);
            base.ViewBag.sliCOUNTRIES = new SelectList(await db.GetCOUNTRIESAsync(langId), "COU_ID", "TEX_TEXT", countryId);
            return View();
        }

        [HttpPost]
        [ActionName("ReSettings")]
        public async Task<ActionResult> DoReSettings(int? countryId, int? langId)
        {
            countryId = (countryId ?? 249);
            langId = (langId ?? 16);
            base.Session["TECDOCCOUNTRY"] = countryId.Value;
            base.Session["TECDOCLANG"] = langId.Value;
            base.ViewBag.sliLANGUAGES = new SelectList(await db.GetLANGUAGESAsync(langId.Value), "LNG_ID", "TEX_TEXT", langId.Value);
            base.ViewBag.sliCOUNTRIES = new SelectList(await db.GetCOUNTRIESAsync(langId.Value), "COU_ID", "TEX_TEXT", countryId.Value);
            return View();
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Index(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, string artId = null, int? srchTp = null)
        {
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                countryId = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from ee in dbCarShop.EnterpriseUserTDES
                                     where ee.EntUserNic == aName
                                     select ee.EntGuid).FirstOrDefaultAsync());
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            srchTp = (srchTp ?? 0);
            base.ViewBag.slisrchTp = new SelectList(new SelectListItem[4]
            {
            new SelectListItem
            {
                Value = "0",
                Text = Resources.SEARCH_SUPPID,
                Selected = (srchTp.Value == 0)
            },
            new SelectListItem
            {
                Value = "1",
                Text = Resources.SEARCH_TECDOCID,
                Selected = (srchTp.Value == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.MANUF_IDS_INDEX,
                Selected = (srchTp.Value == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.SEARCH_EAN,
                Selected = (srchTp.Value == 3)
            }
            }, "Value", "Text", srchTp.Value);
            if (srchTp.Value != 1)
            {
                if (!string.IsNullOrEmpty(artId))
                {
                    artId = artId.Replace("'", "").Trim();
                    if (srchTp.Value == 3)
                    {
                        artId = artId.Replace(" ", "").Trim();
                    }
                }
                if (string.IsNullOrEmpty(artId))
                {
                    base.ModelState.AddModelError("", Resources.SEARCH_STRING_IS_EMPTY);
                }
            }
            else
            {
                int result = 0;
                if (int.TryParse(artId, out result))
                {
                    artId = result.ToString();
                }
                else
                {
                    base.ModelState.AddModelError("", Resources.CANT_PARS_TO_INT);
                }
            }
            base.ViewBag.srchTp = srchTp.Value;
            base.ViewBag.artId = artId;
            if (base.ModelState.IsValid)
            {
                List<MODELTYPETREEITEMS_TD> modeltypetreeitems = await db.GetArticleByIDAsync(langId, countryId, artId, srchTp.Value);
                if (modeltypetreeitems.Count < 1)
                {
                    List<MODELTYPETREEITEMS_REST_TD> model = new List<MODELTYPETREEITEMS_REST_TD>();
                    return View(model);
                }
                ParameterExpression e = Expression.Parameter(typeof(BranchRestTDES), "e");
                Expression builded = null;
                foreach (MODELTYPETREEITEMS_TD item in modeltypetreeitems)
                {
                    Expression expression = Expression.AndAlso(Expression.Equal(Expression.Property(e, "ART_ARTICLE_NR"), Expression.Constant(item.ART_ARTICLE_NR, typeof(string))), Expression.Equal(Expression.Property(e, "SUP_TEXT"), Expression.Constant(item.SUP_TEXT, typeof(string))));
                    builded = ((builded != null) ? Expression.OrElse(builded, expression) : expression);
                }
                List<BranchRestTDES> branchresttdes = (builded == null) ? new List<BranchRestTDES>() : (await Queryable.Where<BranchRestTDES>(predicate: Expression.Lambda<Func<BranchRestTDES, bool>>(Expression.AndAlso(builded, Expression.Equal(Expression.Property(e, "EntBranchGuid"), Expression.Constant(enterprisebranchtdes.EntBranchGuid, typeof(Guid)))), new ParameterExpression[1]
                {
                e
                }), source: dbRest.BranchRestTDES).ToListAsync());
                List<MODELTYPETREEITEMS_REST_TD> aResult = (from mti in modeltypetreeitems
                                                            join brd in branchresttdes on new
                                                            {
                                                                a = mti.ART_ARTICLE_NR,
                                                                s = mti.SUP_TEXT
                                                            } equals new
                                                            {
                                                                a = brd.ART_ARTICLE_NR,
                                                                s = brd.SUP_TEXT
                                                            } into brdval
                                                            from subbrd in brdval.DefaultIfEmpty()
                                                            select new MODELTYPETREEITEMS_REST_TD
                                                            {
                                                                SUP_ID = mti.SUP_ID,
                                                                SUP_TEXT = mti.SUP_TEXT,
                                                                GA_NR = mti.GA_NR,
                                                                MASTER_BEZ = mti.MASTER_BEZ,
                                                                ART_ARTICLE_NR = mti.ART_ARTICLE_NR,
                                                                ART_ID = mti.ART_ID,
                                                                LA_ID = mti.LA_ID,
                                                                GA_TEXT = mti.GA_TEXT,
                                                                EAN_TEXT = mti.EAN_TEXT,
                                                                ArtAmount = (subbrd?.ArtAmount ?? 0),
                                                                ArtPrice = (subbrd?.ArtPrice ?? 0.0)
                                                            }).ToList();
                return View(aResult);
            }
            List<MODELTYPETREEITEMS_REST_TD> model2 = new List<MODELTYPETREEITEMS_REST_TD>();
            return View(model2);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Details(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, int? artId = null, int? GA_NR = null, string ART_ARTICLE_NR = null)
        {
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                countryId = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from ee in dbCarShop.EnterpriseUserTDES
                                           where ee.EntUserNic == aName
                                           select ee.EntGuid).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            artId = (artId ?? 0);
            GA_NR = (GA_NR ?? 0);
            List<MODELTYPETREEITEMDESCR_TD> modeltypetreeitemdescr = null;
            List<ANALOGOUS_REST_TD> analoglist = null;
            List<FUEL_TD> eans = null;
            List<MODELTYPETREEITEMMANID_TD> manufIds = null;
            if (artId.Value > 0)
            {
                modeltypetreeitemdescr = await db.GetMODELTYPETREEITEMDESCRAsync(langId, countryId, artId.Value);
                analoglist = await db.GetANALOGSAsync(langId, countryId, ART_ARTICLE_NR, GA_NR.Value);
                eans = await db.GetArticleEanAsync(artId.Value);
                manufIds = await db.GetMODELTYPETREEITEMMANIDAsync(langId, countryId, artId.Value);
            }
            base.ViewBag.ManufIds = manufIds;
            base.ViewBag.Modeltypetreeitemdescr = modeltypetreeitemdescr;
            base.ViewBag.Eans = eans;
            base.ViewBag.SearchEntGuid = searchEntGuid;
            base.ViewBag.SearchEntBranchGuid = searchEntBranchGuid;
            base.ViewBag.ART_ID = artId.Value.ToString();
            base.ViewBag.ART_ARTICLE_NR = ART_ARTICLE_NR;
            List<ANALOGOUS_REST_TD> aResult2 = null;
            if (analoglist == null || analoglist.Count < 1)
            {
                return View(aResult2);
            }
            ParameterExpression e = Expression.Parameter(typeof(BranchRestTDES), "e");
            Expression builded = null;
            foreach (ANALOGOUS_REST_TD item in analoglist)
            {
                Expression expression = Expression.AndAlso(Expression.Equal(Expression.Property(e, "ART_ARTICLE_NR"), Expression.Constant(item.ART_ARTICLE_NR, typeof(string))), Expression.Equal(Expression.Property(e, "SUP_TEXT"), Expression.Constant(item.SUP_TEXT, typeof(string))));
                builded = ((builded != null) ? Expression.OrElse(builded, expression) : expression);
            }
            List<BranchRestTDES> branchresttdes = (builded == null) ? new List<BranchRestTDES>() : (await Queryable.Where<BranchRestTDES>(predicate: Expression.Lambda<Func<BranchRestTDES, bool>>(Expression.AndAlso(builded, Expression.Equal(Expression.Property(e, "EntBranchGuid"), Expression.Constant(enterprisebranchtdes.EntBranchGuid, typeof(Guid)))), new ParameterExpression[1]
            {
            e
            }), source: dbRest.BranchRestTDES).ToListAsync());
            aResult2 = (from mti in analoglist
                        join brd in branchresttdes on new
                        {
                            a = mti.ART_ARTICLE_NR,
                            s = mti.SUP_TEXT
                        } equals new
                        {
                            a = brd.ART_ARTICLE_NR,
                            s = brd.SUP_TEXT
                        } into brdval
                        from subbrd in brdval.DefaultIfEmpty()
                        select new ANALOGOUS_REST_TD
                        {
                            ART_ID = mti.ART_ID,
                            ART_ARTICLE_NR = mti.ART_ARTICLE_NR,
                            SUP_TEXT = mti.SUP_TEXT,
                            MASTER_BEZ = mti.MASTER_BEZ,
                            GA_NR = mti.GA_NR,
                            GA_TEXT = mti.GA_TEXT,
                            ArtAmount = (subbrd?.ArtAmount ?? 0),
                            ArtPrice = (subbrd?.ArtPrice ?? 0.0)
                        }).ToList();
            return View(aResult2);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> AppliedTo(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, int? artId = null, string ART_ARTICLE_NR = null, int? page = null)
        {
            page = (page ?? 1);
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                countryId = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from ee in dbCarShop.EnterpriseUserTDES
                                     where ee.EntUserNic == aName
                                     select ee.EntGuid).FirstOrDefaultAsync());
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            IPagedList<MODELTYPE_TD> aResult = null;
            artId = (artId ?? 0);
            base.ViewBag.ART_ID = artId.Value.ToString();
            base.ViewBag.ART_ARTICLE_NR = ART_ARTICLE_NR;
            if (artId.Value > 0)
            {
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await db.GetArticleApplicAsync(langId, countryId, artId.Value, pageNumber, pageSize);
            }
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> Manufact(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null)
        {
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                _ = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                _ = (int)Obj;
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from ee in dbCarShop.EnterpriseUserTDES
                                     where ee.EntUserNic == aName
                                     select ee.EntGuid).FirstOrDefaultAsync());
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            return View(await db.GetBRANDSAsync());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> ModelTypes(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, int? MFA_ID = null)
        {
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                countryId = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from ee in dbCarShop.EnterpriseUserTDES
                                     where ee.EntUserNic == aName
                                     select ee.EntGuid).FirstOrDefaultAsync());
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            MFA_ID = (MFA_ID ?? 0);
            List<MODEL_TD> carmodels = await db.GetMODELSAsync(langId, countryId, MFA_ID.Value);
            base.ViewBag.MFA_ID = MFA_ID.Value;
            return View(carmodels);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> ModelDetais(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null)
        {
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                countryId = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from ee in dbCarShop.EnterpriseUserTDES
                                     where ee.EntUserNic == aName
                                     select ee.EntGuid).FirstOrDefaultAsync());
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            fluelId = (fluelId ?? 71334);
            base.ViewBag.sliFUELS = new SelectList(await db.GetFUELSAsync(langId), "DES_ID", "TEX_TEXT", fluelId);
            MOD_ID = (MOD_ID ?? 0);
            MFA_ID = (MFA_ID ?? 0);
            List<MODELTYPE_TD> carmodeltypes = await db.GetMODELTYPESAsync(langId, countryId, MOD_ID.Value, fluelId);
            base.ViewBag.MFA_ID = MFA_ID.Value;
            base.ViewBag.MOD_ID = MOD_ID.Value;
            base.ViewBag.FluelId = fluelId.Value;
            return View(carmodeltypes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> ModelDetaisTree(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, int? treeKindId = null)
        {
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                _ = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from ee in dbCarShop.EnterpriseUserTDES
                                     where ee.EntUserNic == aName
                                     select ee.EntGuid).FirstOrDefaultAsync());
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            fluelId = (fluelId ?? 71334);
            MFA_ID = (MFA_ID ?? 0);
            MOD_ID = (MOD_ID ?? 0);
            TYP_ID = (TYP_ID ?? 0);
            topicId = (topicId ?? 0);
            treeKindId = (treeKindId ?? 1);
            base.ViewBag.sliTreeKind = new SelectList(new SelectListItem[5]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.TreePassengerCars,
                Selected = (treeKindId.Value == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.TreeCommercialVehicles,
                Selected = (treeKindId.Value == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.TreeMotors,
                Selected = (treeKindId.Value == 3)
            },
            new SelectListItem
            {
                Value = "4",
                Text = Resources.TreeAxles,
                Selected = (treeKindId.Value == 4)
            },
            new SelectListItem
            {
                Value = "7",
                Text = Resources.TreeUniversals,
                Selected = (treeKindId.Value == 7)
            }
            }, "Value", "Text", treeKindId.Value);
            List<MODELTYPESTREE_TD> modeltypetree = await db.GetMODELTYPESTREEAsync(langId, TYP_ID.Value, topicId.Value, treeKindId.Value);
            base.ViewBag.MFA_ID = MFA_ID.Value;
            base.ViewBag.MOD_ID = MOD_ID.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.TYP_ID = TYP_ID.Value;
            base.ViewBag.topicId = topicId.Value;
            base.ViewBag.treeKindId = treeKindId.Value;
            return View(modeltypetree);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> ModelDetaisItems(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, int? tof_assemblyId = null, int? tof_suppliersId = null, int? treeKindId = null)
        {
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                countryId = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from ee in dbCarShop.EnterpriseUserTDES
                                           where ee.EntUserNic == aName
                                           select ee.EntGuid).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            fluelId = (fluelId ?? 71334);
            MFA_ID = (MFA_ID ?? 0);
            MOD_ID = (MOD_ID ?? 0);
            TYP_ID = (TYP_ID ?? 0);
            topicId = (topicId ?? 0);
            tof_suppliersId = (tof_suppliersId ?? 0);
            tof_assemblyId = (tof_assemblyId ?? 0);
            base.ViewBag.searchEntGuid = searchEntGuid;
            base.ViewBag.searchEntBranchGuid = searchEntGuid;
            base.ViewBag.MFA_ID = MFA_ID.Value;
            base.ViewBag.MOD_ID = MOD_ID.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.TYP_ID = TYP_ID.Value;
            base.ViewBag.topicId = topicId.Value;
            base.ViewBag.tof_assemblyId = tof_assemblyId.Value;
            base.ViewBag.tof_suppliersId = tof_suppliersId.Value;
            base.ViewBag.treeKindId = treeKindId;
            int tecDocSrcType = TecDocSrcType;
            List<MODELTYPETREEITEMS_REST_TD> modeltypetreeitems;
            if (tecDocSrcType == 1)
            {
                modeltypetreeitems = (from ee in db.GetMODELTYPETREEITEMS(langId, countryId, TYP_ID.Value, topicId.Value, tof_assemblyId.Value, tof_suppliersId.Value)
                                      orderby ee.GA_NR, ee.SUP_ID
                                      select ee).ToList();
                List<DICT_TD> tof_assembly2 = (from ee in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD ee) => new DICT_TD
                {
                    DictId = ee.GA_NR,
                    DictTitle = ee.GA_TEXT + ":" + ee.GA_NR.ToString()
                }).Distinct()
                                               orderby ee.DictTitle
                                               select ee).ToList();
                tof_assembly2.Insert(0, new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                });
                base.ViewBag.sliTof_assembly = new SelectList(tof_assembly2, "DictId", "DictTitle", tof_assemblyId.Value);
                List<DICT_TD> tof_suppliers2 = (from ee in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD ee) => new DICT_TD
                {
                    DictId = ee.SUP_ID,
                    DictTitle = ee.SUP_TEXT + ":" + ee.SUP_ID.ToString()
                }).Distinct()
                                                orderby ee.DictTitle
                                                select ee).ToList();
                tof_suppliers2.Insert(0, new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                });
                base.ViewBag.sliTof_suppliers = new SelectList(tof_suppliers2, "DictId", "DictTitle", tof_suppliersId.Value);
                if (tof_assemblyId.Value == 0 && tof_suppliersId.Value == 0)
                {
                    return View(modeltypetreeitems);
                }
            }
            else
            {
                DICT_TD tof_assemblyItem = null;
                string tadescr = null;
                List<DICT_TD> tof_assembly2 = await db.GetTof_assemblyAsync(langId, countryId, TYP_ID.Value, topicId.Value);
                if (tof_assemblyId.HasValue)
                {
                    tof_assemblyItem = tof_assembly2.Where((DICT_TD ta) => ta.DictId == tof_assemblyId.Value).FirstOrDefault();
                }
                if (tof_assemblyItem == null)
                {
                    tof_assemblyItem = tof_assembly2.FirstOrDefault();
                }
                if (tof_assemblyItem != null)
                {
                    tof_assemblyId = tof_assemblyItem.DictId;
                    tadescr = tof_assemblyItem.DictTitle;
                }
                else
                {
                    tof_assemblyId = 0;
                }
                modeltypetreeitems = await db.GetMODELTYPETREEITEMS_MSAsync(langId, countryId, TYP_ID.Value, topicId.Value, tof_assemblyId.Value, tadescr, tof_suppliersId.Value);
                base.ViewBag.sliTof_assembly = new SelectList(tof_assembly2, "DictId", "DictTitle", tof_assemblyId.Value);
                List<DICT_TD> tof_suppliers2 = (from ee in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD ee) => new DICT_TD
                {
                    DictId = ee.SUP_ID,
                    DictTitle = ee.SUP_TEXT + ":" + ee.SUP_ID.ToString()
                }).Distinct()
                                                orderby ee.DictTitle
                                                select ee).ToList();
                tof_suppliers2.Insert(0, new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                });
                base.ViewBag.sliTof_suppliers = new SelectList(tof_suppliers2, "DictId", "DictTitle", tof_suppliersId.Value);
                base.ViewBag.tof_assemblyId = tof_assemblyId.Value;
                base.ViewBag.tof_suppliersId = tof_suppliersId.Value;
                if (tof_assemblyId.Value == 0 && tof_suppliersId.Value == 0)
                {
                    return View(modeltypetreeitems);
                }
            }
            ParameterExpression e = Expression.Parameter(typeof(BranchRestTDES), "e");
            Expression builded = null;
            foreach (MODELTYPETREEITEMS_REST_TD item in modeltypetreeitems)
            {
                Expression expression = Expression.AndAlso(Expression.Equal(Expression.Property(e, "ART_ARTICLE_NR"), Expression.Constant(item.ART_ARTICLE_NR, typeof(string))), Expression.Equal(Expression.Property(e, "SUP_TEXT"), Expression.Constant(item.SUP_TEXT, typeof(string))));
                builded = ((builded != null) ? Expression.OrElse(builded, expression) : expression);
            }
            List<BranchRestTDES> branchresttdes = (builded == null) ? new List<BranchRestTDES>() : (await Queryable.Where<BranchRestTDES>(predicate: Expression.Lambda<Func<BranchRestTDES, bool>>(Expression.AndAlso(builded, Expression.Equal(Expression.Property(e, "EntBranchGuid"), Expression.Constant(enterprisebranchtdes.EntBranchGuid, typeof(Guid)))), new ParameterExpression[1]
            {
            e
            }), source: dbRest.BranchRestTDES).ToListAsync());
            List<MODELTYPETREEITEMS_REST_TD> aResult = (from mti in modeltypetreeitems
                                                        join brd in branchresttdes on new
                                                        {
                                                            a = mti.ART_ARTICLE_NR,
                                                            s = mti.SUP_TEXT
                                                        } equals new
                                                        {
                                                            a = brd.ART_ARTICLE_NR,
                                                            s = brd.SUP_TEXT
                                                        } into brdval
                                                        from subbrd in brdval.DefaultIfEmpty()
                                                        select new MODELTYPETREEITEMS_REST_TD
                                                        {
                                                            SUP_ID = mti.SUP_ID,
                                                            SUP_TEXT = mti.SUP_TEXT,
                                                            GA_NR = mti.GA_NR,
                                                            MASTER_BEZ = mti.MASTER_BEZ,
                                                            ART_ARTICLE_NR = mti.ART_ARTICLE_NR,
                                                            ART_ID = mti.ART_ID,
                                                            LA_ID = mti.LA_ID,
                                                            GA_TEXT = mti.GA_TEXT,
                                                            EAN_TEXT = mti.EAN_TEXT,
                                                            ArtAmount = (subbrd?.ArtAmount ?? 0),
                                                            ArtPrice = (subbrd?.ArtPrice ?? 0.0)
                                                        }).ToList();
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> ModelDetaisAnalog(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, string ART_ARTICLE_NR = null, int? GA_NR = null, int? ART_ID = null, int? treeKindId = null, int? tof_assemblyId = null)
        {
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                countryId = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            fluelId = (fluelId ?? 71334);
            MFA_ID = (MFA_ID ?? 0);
            MOD_ID = (MOD_ID ?? 0);
            TYP_ID = (TYP_ID ?? 0);
            topicId = (topicId ?? 0);
            GA_NR = (GA_NR ?? 0);
            ART_ID = (ART_ID ?? 0);
            base.ViewBag.searchEntGuid = searchEntGuid;
            base.ViewBag.searchEntBranchGuid = searchEntGuid;
            base.ViewBag.MFA_ID = MFA_ID.Value;
            base.ViewBag.MOD_ID = MOD_ID.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.TYP_ID = TYP_ID.Value;
            base.ViewBag.topicId = topicId.Value;
            base.ViewBag.GA_NR = GA_NR.Value;
            base.ViewBag.ART_ID = ART_ID.Value;
            base.ViewBag.ART_ARTICLE_NR = ART_ARTICLE_NR;
            base.ViewBag.treeKindId = treeKindId;
            base.ViewBag.tof_assemblyId = tof_assemblyId;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = await (from ee in dbCarShop.EnterpriseUserTDES
                                           where ee.EntUserNic == aName
                                           select ee.EntGuid).FirstOrDefaultAsync();
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            List<ANALOGOUS_REST_TD> analoglist = await db.GetANALOGSAsync(langId, countryId, ART_ARTICLE_NR, GA_NR);
            if (analoglist == null || analoglist.Count < 1)
            {
                return View(analoglist);
            }
            ParameterExpression e = Expression.Parameter(typeof(BranchRestTDES), "e");
            Expression builded = null;
            foreach (ANALOGOUS_REST_TD item in analoglist)
            {
                Expression expression = Expression.AndAlso(Expression.Equal(Expression.Property(e, "ART_ARTICLE_NR"), Expression.Constant(item.ART_ARTICLE_NR, typeof(string))), Expression.Equal(Expression.Property(e, "SUP_TEXT"), Expression.Constant(item.SUP_TEXT, typeof(string))));
                builded = ((builded != null) ? Expression.OrElse(builded, expression) : expression);
            }
            List<BranchRestTDES> branchresttdes = (builded == null) ? new List<BranchRestTDES>() : (await Queryable.Where<BranchRestTDES>(predicate: Expression.Lambda<Func<BranchRestTDES, bool>>(Expression.AndAlso(builded, Expression.Equal(Expression.Property(e, "EntBranchGuid"), Expression.Constant(enterprisebranchtdes.EntBranchGuid, typeof(Guid)))), new ParameterExpression[1]
            {
            e
            }), source: dbRest.BranchRestTDES).ToListAsync());
            base.ViewBag.SearchEntGuid = searchEntGuid;
            base.ViewBag.SearchEntBranchGuid = searchEntBranchGuid;
            List<ANALOGOUS_REST_TD> aResult = (from mti in analoglist
                                               join brd in branchresttdes on new
                                               {
                                                   a = mti.ART_ARTICLE_NR,
                                                   s = mti.SUP_TEXT
                                               } equals new
                                               {
                                                   a = brd.ART_ARTICLE_NR,
                                                   s = brd.SUP_TEXT
                                               } into brdval
                                               from subbrd in brdval.DefaultIfEmpty()
                                               select new ANALOGOUS_REST_TD
                                               {
                                                   ART_ID = mti.ART_ID,
                                                   ART_ARTICLE_NR = mti.ART_ARTICLE_NR,
                                                   SUP_TEXT = mti.SUP_TEXT,
                                                   MASTER_BEZ = mti.MASTER_BEZ,
                                                   GA_NR = mti.GA_NR,
                                                   GA_TEXT = mti.GA_TEXT,
                                                   ArtAmount = (subbrd?.ArtAmount ?? 0),
                                                   ArtPrice = (subbrd?.ArtPrice ?? 0.0)
                                               }).ToList();
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> ModelDetaisAppliedTo(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, string ART_ARTICLE_NR = null, int? GA_NR = null, int? ART_ID = null, int? treeKindId = null, int? tof_assemblyId = null, int? page = null)
        {
            page = (page ?? 1);
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                countryId = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            fluelId = (fluelId ?? 71334);
            MFA_ID = (MFA_ID ?? 0);
            MOD_ID = (MOD_ID ?? 0);
            TYP_ID = (TYP_ID ?? 0);
            topicId = (topicId ?? 0);
            GA_NR = (GA_NR ?? 0);
            ART_ID = (ART_ID ?? 0);
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from ee in dbCarShop.EnterpriseUserTDES
                                     where ee.EntUserNic == aName
                                     select ee.EntGuid).FirstOrDefaultAsync());
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            base.ViewBag.MFA_ID = MFA_ID.Value;
            base.ViewBag.MOD_ID = MOD_ID.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.TYP_ID = TYP_ID.Value;
            base.ViewBag.topicId = topicId.Value;
            base.ViewBag.GA_NR = GA_NR.Value;
            base.ViewBag.ART_ID = ART_ID.Value;
            base.ViewBag.ART_ARTICLE_NR = ART_ARTICLE_NR;
            base.ViewBag.treeKindId = treeKindId;
            base.ViewBag.tof_assemblyId = tof_assemblyId;
            IPagedList<MODELTYPE_TD> aResult = null;
            if (ART_ID.Value > 0)
            {
                int pageSize = 20;
                int pageNumber = page.Value;
                aResult = await db.GetArticleApplicAsync(langId, countryId, ART_ID.Value, pageNumber, pageSize);
            }
            return View(aResult);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> ModelItemDetais(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, string ART_ARTICLE_NR = null, int? GA_NR = null, int? ART_ID = null, int? treeKindId = null, int? tof_assemblyId = null)
        {
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            int countryId = 249;
            int langId = 16;
            object Obj = base.Session["TECDOCCOUNTRY"];
            if (Obj == null)
            {
                base.Session["TECDOCCOUNTRY"] = countryId;
            }
            else
            {
                countryId = (int)Obj;
            }
            Obj = base.Session["TECDOCLANG"];
            if (Obj == null)
            {
                base.Session["TECDOCLANG"] = langId;
            }
            else
            {
                langId = (int)Obj;
            }
            fluelId = (fluelId ?? 71334);
            MFA_ID = (MFA_ID ?? 0);
            MOD_ID = (MOD_ID ?? 0);
            TYP_ID = (TYP_ID ?? 0);
            topicId = (topicId ?? 0);
            GA_NR = (GA_NR ?? 0);
            ART_ID = (ART_ID ?? 0);
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from ee in dbCarShop.EnterpriseUserTDES
                                     where ee.EntUserNic == aName
                                     select ee.EntGuid).FirstOrDefaultAsync());
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            base.ViewBag.MFA_ID = MFA_ID.Value;
            base.ViewBag.MOD_ID = MOD_ID.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.TYP_ID = TYP_ID.Value;
            base.ViewBag.topicId = topicId.Value;
            base.ViewBag.GA_NR = GA_NR.Value;
            base.ViewBag.ART_ID = ART_ID.Value;
            base.ViewBag.ART_ARTICLE_NR = ART_ARTICLE_NR;
            base.ViewBag.treeKindId = treeKindId;
            base.ViewBag.tof_assemblyId = tof_assemblyId;
            List<MODELTYPETREEITEMDESCR_TD> modeltypetreeitemdescr = null;
            List<FUEL_TD> eans = null;
            List<MODELTYPETREEITEMMANID_TD> manufIds = null;
            if (ART_ID.Value > 0)
            {
                modeltypetreeitemdescr = await db.GetMODELTYPETREEITEMDESCRAsync(langId, countryId, ART_ID.Value);
                eans = await db.GetArticleEanAsync(ART_ID.Value);
                manufIds = await db.GetMODELTYPETREEITEMMANIDAsync(langId, countryId, ART_ID.Value);
            }
            base.ViewBag.ManufIds = manufIds;
            base.ViewBag.Modeltypetreeitemdescr = modeltypetreeitemdescr;
            base.ViewBag.Eans = eans;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> IndexSelected(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, string SimpleArticle = null, string DOSELECT_TITLE = null, string DOCANCEL_TITLE = null)
        {
            base.ViewBag.redirecData = redirecData;
            base.ViewBag.redirectContriller = redirectContriller;
            base.ViewBag.redirectAction = redirectAction;
            if (DOSELECT_TITLE == null || DOCANCEL_TITLE != null)
            {
                return RedirectToAction("IndexByRoles", "Home", new
                {

                });
            }
            if (SimpleArticle == null)
            {
                return RedirectToAction("IndexByRoles", "Home", new
                {

                });
            }
            SimpleArticle_REST_TD sa = JsonConvert.DeserializeObject<SimpleArticle_REST_TD>(SimpleArticle);
            string ART_ARTICLE_NR = sa.ART_ARTICLE_NR;
            string SUP_TEXT = sa.SUP_TEXT;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from ee in dbCarShop.EnterpriseUserTDES
                                     where ee.EntUserNic == aName
                                     select ee.EntGuid).FirstOrDefaultAsync());
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View("MakeSelection", null);
            }
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            List<BranchRestTDES> branchresttdes = await (from e in dbRest.BranchRestTDES.Include("BranchRestArticleDescriptionTDES")
                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && e.ART_ARTICLE_NR == ART_ARTICLE_NR && e.SUP_TEXT == SUP_TEXT
                                                         select e).ToListAsync();
            if (branchresttdes == null || branchresttdes.Count < 1)
            {
                base.ModelState.AddModelError("", Resources.NoSuchArticule);
                return View("MakeSelection", branchresttdes);
            }
            if (branchresttdes.Count > 1)
            {
                base.ModelState.AddModelError("", Resources.AmbiguousChoiceOrLackOfChoice);
                return View("MakeSelection", branchresttdes);
            }
            BranchRestTDES item = branchresttdes[0];
            string EntArticle = item.EntBranchArticle;
            string EntBrandNic = item.EntBranchSup;
            string ExternArticle = item.ART_ARTICLE_NR;
            string ExternBrandNic = item.SUP_TEXT;
            string ExternArticleEAN = item.ExternArticleEAN;
            double ArtPrice = item.ArtPrice;
            int EntArticleDescriptionId = item.EntArticleDescriptionId;
            string EntArticleDescription = item.BranchRestArticleDescriptionTDES.EntArticleDescription;
            if (string.IsNullOrEmpty(redirectAction) || string.IsNullOrEmpty(redirectContriller))
            {
                return RedirectToAction("GetArticuleRestForCreate", "SaleBasketArticle", new
                {
                    EntArticle = EntArticle,
                    EntBrandNic = EntBrandNic,
                    EntBranchGuid = searchEntBranchGuid,
                    EntArticleDescription = EntArticleDescription,
                    EntArticleDescriptionId = EntArticleDescriptionId,
                    ExternArticle = ExternArticle,
                    ExternBrandNic = ExternBrandNic,
                    ExternArticleEAN = ExternArticleEAN,
                    ArtPrice = ArtPrice
                });
            }
            return RedirectToAction(redirectAction, redirectContriller, new
            {
                redirecData = redirecData,
                EntArticle = EntArticle,
                EntBrandNic = EntBrandNic,
                EntBranchGuid = searchEntBranchGuid,
                EntArticleDescription = EntArticleDescription,
                EntArticleDescriptionId = EntArticleDescriptionId,
                ExternArticle = ExternArticle,
                ExternBrandNic = ExternBrandNic,
                ExternArticleEAN = ExternArticleEAN,
                ArtPrice = ArtPrice
            });
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public async Task<ActionResult> MakeSelection(string redirecData = null, string redirectContriller = null, string redirectAction = null, Guid? searchEntGuid = null, Guid? searchEntBranchGuid = null, string SimpleBranchRest = null, string DOSELECT_TITLE = null, string DOCANCEL_TITLE = null)
        {
            if (DOSELECT_TITLE == null || DOCANCEL_TITLE != null || SimpleBranchRest == null)
            {
                return RedirectToAction("IndexByRoles", "Home", new
                {

                });
            }
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    new Guid?(await (from ee in dbCarShop.EnterpriseUserTDES
                                     where ee.EntUserNic == aName
                                     select ee.EntGuid).FirstOrDefaultAsync());
                }
                else
                {
                    EnterpriseBranchIdsTDES searchEntBranchIds = await (from ee in dbCarShop.EnterpriseBranchUserTDES
                                                                        where ee.EntUserNic == aName
                                                                        select new EnterpriseBranchIdsTDES
                                                                        {
                                                                            EntGuid = ee.EntGuid,
                                                                            EntBranchGuid = ee.EntBranchGuid
                                                                        }).FirstOrDefaultAsync();
                    if (searchEntBranchIds == null)
                    {
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        new Guid?(searchEntBranchIds.EntGuid);
                        searchEntBranchGuid = searchEntBranchIds.EntBranchGuid;
                    }
                }
            }
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ViewBag.redirecData = redirecData;
                base.ViewBag.redirectContriller = redirectContriller;
                base.ViewBag.redirectAction = redirectAction;
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                return View("MakeSelection", null);
            }
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            string EntArticle = null;
            string EntBrandNic = null;
            Guid EntBranchGuid = Guid.Empty;
            string EntArticleDescription = null;
            string ExternArticle = null;
            string ExternBrandNic = null;
            string ExternArticleEAN = null;
            int EntArticleDescriptionId = 0;
            double ArtPrice = 0.0;
            if (SimpleBranchRest != null)
            {
                BranchRestTmp sa = JsonConvert.DeserializeObject<BranchRestTmp>(SimpleBranchRest);
                EntArticle = sa.EntBranchArticle;
                EntBrandNic = sa.EntBranchSup;
                EntBranchGuid = sa.EntBranchGuid;
                if (CarShopRestContextCatalog != null)
                {
                    BranchRestTDES enterprisearticletdes = await (from e in dbRest.BranchRestTDES.Include("BranchRestArticleDescriptionTDES")
                                                                  where e.EntBranchGuid == sa.EntBranchGuid && e.EntBranchArticle == sa.EntBranchArticle && e.EntBranchSup == sa.EntBranchSup
                                                                  select e).FirstOrDefaultAsync();
                    if (enterprisearticletdes != null)
                    {
                        ExternArticle = enterprisearticletdes.ART_ARTICLE_NR;
                        ExternBrandNic = enterprisearticletdes.SUP_TEXT;
                        ExternArticleEAN = enterprisearticletdes.ExternArticleEAN;
                        ArtPrice = enterprisearticletdes.ArtPrice;
                        EntArticleDescriptionId = enterprisearticletdes.EntArticleDescriptionId;
                        EntArticleDescription = enterprisearticletdes.BranchRestArticleDescriptionTDES.EntArticleDescription;
                    }
                }
            }
            if (string.IsNullOrEmpty(redirectAction) || string.IsNullOrEmpty(redirectContriller))
            {
                return RedirectToAction("GetArticuleRestForCreate", "SaleBasketArticle", new
                {
                    EntArticle,
                    EntBrandNic,
                    EntBranchGuid,
                    EntArticleDescription,
                    EntArticleDescriptionId,
                    ExternArticle,
                    ExternBrandNic,
                    ExternArticleEAN,
                    ArtPrice
                });
            }
            return RedirectToAction("redirectAction", "redirectContriller", new
            {
                redirecData,
                EntArticle,
                EntBrandNic,
                EntBranchGuid,
                EntArticleDescription,
                EntArticleDescriptionId,
                ExternArticle,
                ExternBrandNic,
                ExternArticleEAN,
                ArtPrice
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (_dbRest != null)
            {
                _dbRest.Dispose();
                _dbRest = null;
            }
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