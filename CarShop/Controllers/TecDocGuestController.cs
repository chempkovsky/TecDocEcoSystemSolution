// CarShop.Controllers.TecDocGuestController
using CarShop.Models;
using CarShop.Properties;
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
    public class TecDocGuestController : Controller
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

        public async Task<ActionResult> Index(string artId = null, int? srchTp = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
                Text = Resources.GUEST_SEARCH_SUPPID,
                Selected = (srchTp.Value == 0)
            },
            new SelectListItem
            {
                Value = "1",
                Text = Resources.GUEST_SEARCH_TECDOCID,
                Selected = (srchTp.Value == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.GUEST_MANUF_IDS_INDEX,
                Selected = (srchTp.Value == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.GUEST_SEARCH_EAN,
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
                modeltypetreeitems = (from i in modeltypetreeitems
                                      group i by i.ART_ID into @group
                                      select @group.First()).ToList();
                ParameterExpression e = Expression.Parameter(typeof(BranchRestTDES), "e");
                Expression builded = null;
                foreach (MODELTYPETREEITEMS_TD item in modeltypetreeitems)
                {
                    Expression expression = Expression.AndAlso(Expression.Equal(Expression.Property(e, "ART_ARTICLE_NR"), Expression.Constant(item.ART_ARTICLE_NR, typeof(string))), Expression.Equal(Expression.Property(e, "SUP_TEXT"), Expression.Constant(item.SUP_TEXT, typeof(string))));
                    builded = ((builded != null) ? Expression.OrElse(builded, expression) : expression);
                }
                List<BranchRestSumTDES> branchresttdes = (builded == null) ? new List<BranchRestSumTDES>() : (await (from gt in dbRest.BranchRestTDES.Where(Expression.Lambda<Func<BranchRestTDES, bool>>(builded, new ParameterExpression[1]
                     {
                    e
                     }))
                                                                                                                     where gt.ArtAmount > 0
                                                                                                                     select gt into ppp
                                                                                                                     group ppp by new
                                                                                                                     {
                                                                                                                         ppp.ART_ARTICLE_NR,
                                                                                                                         ppp.SUP_TEXT
                                                                                                                     } into zzz
                                                                                                                     select new BranchRestSumTDES
                                                                                                                     {
                                                                                                                         ART_ARTICLE_NR = zzz.Key.ART_ARTICLE_NR,
                                                                                                                         SUP_TEXT = zzz.Key.SUP_TEXT,
                                                                                                                         ArtAmount = zzz.Sum((BranchRestTDES aaa) => aaa.ArtAmount),
                                                                                                                         ArtPrice = zzz.Max((BranchRestTDES bbb) => bbb.ArtPrice),
                                                                                                                         MinArtPrice = zzz.Min((BranchRestTDES ccc) => ccc.ArtPrice)
                                                                                                                     }).ToListAsync());
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
                                                                ArtPrice = (subbrd?.ArtPrice ?? 0.0),
                                                                MinArtPrice = (subbrd?.MinArtPrice ?? 0.0)
                                                            } into m
                                                            orderby m.ArtAmount descending
                                                            select m).ToList();
                return View(aResult);
            }
            List<MODELTYPETREEITEMS_REST_TD> model2 = new List<MODELTYPETREEITEMS_REST_TD>();
            return View(model2);
        }

        public async Task<ActionResult> IndexUTO(string artId = null, int? srchTp = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
                Text = Resources.GUEST_SEARCH_SUPPID,
                Selected = (srchTp.Value == 0)
            },
            new SelectListItem
            {
                Value = "1",
                Text = Resources.GUEST_SEARCH_TECDOCID,
                Selected = (srchTp.Value == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.GUEST_MANUF_IDS_INDEX,
                Selected = (srchTp.Value == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.GUEST_SEARCH_EAN,
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
                modeltypetreeitems = (from i in modeltypetreeitems
                                      group i by i.ART_ID into @group
                                      select @group.First()).ToList();
                ParameterExpression e = Expression.Parameter(typeof(SuppRestTDES), "e");
                Expression builded = null;
                foreach (MODELTYPETREEITEMS_TD item in modeltypetreeitems)
                {
                    Expression expression = Expression.AndAlso(Expression.Equal(Expression.Property(e, "ART_ARTICLE_NR"), Expression.Constant(item.ART_ARTICLE_NR, typeof(string))), Expression.Equal(Expression.Property(e, "SUP_TEXT"), Expression.Constant(item.SUP_TEXT, typeof(string))));
                    builded = ((builded != null) ? Expression.OrElse(builded, expression) : expression);
                }
                List<BranchRestSumTDES> branchresttdes = (builded == null) ? new List<BranchRestSumTDES>() : (await (from gt in dbRest.SuppRestTDES.Where(Expression.Lambda<Func<SuppRestTDES, bool>>(builded, new ParameterExpression[1]
                     {
                    e
                     }))
                                                                                                                     where gt.ArtAmount > 0
                                                                                                                     select gt into ppp
                                                                                                                     group ppp by new
                                                                                                                     {
                                                                                                                         ppp.ART_ARTICLE_NR,
                                                                                                                         ppp.SUP_TEXT
                                                                                                                     } into zzz
                                                                                                                     select new BranchRestSumTDES
                                                                                                                     {
                                                                                                                         ART_ARTICLE_NR = zzz.Key.ART_ARTICLE_NR,
                                                                                                                         SUP_TEXT = zzz.Key.SUP_TEXT,
                                                                                                                         ArtAmount = zzz.Sum((SuppRestTDES aaa) => aaa.ArtAmount),
                                                                                                                         ArtPrice = zzz.Max((SuppRestTDES bbb) => bbb.ArtPrice),
                                                                                                                         MinArtPrice = zzz.Min((SuppRestTDES ccc) => ccc.ArtPrice)
                                                                                                                     }).ToListAsync());
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
                                                                ArtPrice = (subbrd?.ArtPrice ?? 0.0),
                                                                MinArtPrice = (subbrd?.MinArtPrice ?? 0.0)
                                                            } into m
                                                            orderby m.ArtAmount descending
                                                            select m).ToList();
                return View(aResult);
            }
            List<MODELTYPETREEITEMS_REST_TD> model2 = new List<MODELTYPETREEITEMS_REST_TD>();
            return View(model2);
        }

        public async Task<ActionResult> Details(int? artId = null, int? GA_NR = null, string ART_ARTICLE_NR = null)
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
            List<BranchRestSumTDES> branchresttdes = (builded == null) ? new List<BranchRestSumTDES>() : (await (from gt in dbRest.BranchRestTDES.Where(Expression.Lambda<Func<BranchRestTDES, bool>>(builded, new ParameterExpression[1]
                 {
                e
                 }))
                                                                                                                 where gt.ArtAmount > 0
                                                                                                                 select gt into ppp
                                                                                                                 group ppp by new
                                                                                                                 {
                                                                                                                     ppp.ART_ARTICLE_NR,
                                                                                                                     ppp.SUP_TEXT
                                                                                                                 } into zzz
                                                                                                                 select new BranchRestSumTDES
                                                                                                                 {
                                                                                                                     ART_ARTICLE_NR = zzz.Key.ART_ARTICLE_NR,
                                                                                                                     SUP_TEXT = zzz.Key.SUP_TEXT,
                                                                                                                     ArtAmount = zzz.Sum((BranchRestTDES aaa) => aaa.ArtAmount),
                                                                                                                     ArtPrice = zzz.Max((BranchRestTDES bbb) => bbb.ArtPrice),
                                                                                                                     MinArtPrice = zzz.Min((BranchRestTDES ccc) => ccc.ArtPrice)
                                                                                                                 }).ToListAsync());
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
                            ArtPrice = (subbrd?.ArtPrice ?? 0.0),
                            MinArtPrice = (subbrd?.MinArtPrice ?? 0.0)
                        } into fff
                        orderby fff.ArtAmount descending
                        select fff).ToList();
            return View(aResult2);
        }

        public async Task<ActionResult> DetailsUTO(int? artId = null, int? GA_NR = null, string ART_ARTICLE_NR = null)
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
            ParameterExpression e = Expression.Parameter(typeof(SuppRestTDES), "e");
            Expression builded = null;
            foreach (ANALOGOUS_REST_TD item in analoglist)
            {
                Expression expression = Expression.AndAlso(Expression.Equal(Expression.Property(e, "ART_ARTICLE_NR"), Expression.Constant(item.ART_ARTICLE_NR, typeof(string))), Expression.Equal(Expression.Property(e, "SUP_TEXT"), Expression.Constant(item.SUP_TEXT, typeof(string))));
                builded = ((builded != null) ? Expression.OrElse(builded, expression) : expression);
            }
            List<BranchRestSumTDES> branchresttdes = (builded == null) ? new List<BranchRestSumTDES>() : (await (from gt in dbRest.SuppRestTDES.Where(Expression.Lambda<Func<SuppRestTDES, bool>>(builded, new ParameterExpression[1]
                 {
                e
                 }))
                                                                                                                 where gt.ArtAmount > 0
                                                                                                                 select gt into ppp
                                                                                                                 group ppp by new
                                                                                                                 {
                                                                                                                     ppp.ART_ARTICLE_NR,
                                                                                                                     ppp.SUP_TEXT
                                                                                                                 } into zzz
                                                                                                                 select new BranchRestSumTDES
                                                                                                                 {
                                                                                                                     ART_ARTICLE_NR = zzz.Key.ART_ARTICLE_NR,
                                                                                                                     SUP_TEXT = zzz.Key.SUP_TEXT,
                                                                                                                     ArtAmount = zzz.Sum((SuppRestTDES aaa) => aaa.ArtAmount),
                                                                                                                     ArtPrice = zzz.Max((SuppRestTDES bbb) => bbb.ArtPrice),
                                                                                                                     MinArtPrice = zzz.Min((SuppRestTDES ccc) => ccc.ArtPrice)
                                                                                                                 }).ToListAsync());
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
                            ArtPrice = (subbrd?.ArtPrice ?? 0.0),
                            MinArtPrice = (subbrd?.MinArtPrice ?? 0.0)
                        } into fff
                        orderby fff.ArtAmount descending
                        select fff).ToList();
            return View(aResult2);
        }

        public async Task<ActionResult> AppliedTo(int? artId = null, string ART_ARTICLE_NR = null, int? page = null)
        {
            page = (page ?? 1);
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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

        public async Task<ActionResult> AppliedToUTO(int? artId = null, string ART_ARTICLE_NR = null, int? page = null)
        {
            page = (page ?? 1);
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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

        public async Task<ActionResult> Manufact()
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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

        public async Task<ActionResult> ModelTypes(int? MFA_ID = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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

        public async Task<ActionResult> ModelDetais(int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
            base.ViewBag.MFA_ID = MFA_ID.Value;
            base.ViewBag.MOD_ID = MOD_ID.Value;
            base.ViewBag.FluelId = fluelId.Value;
            return View(await db.GetMODELTYPESAsync(langId, countryId, MOD_ID.Value, fluelId));
        }

        public async Task<ActionResult> ModelDetaisTree(int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, int? treeKindId = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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

        public async Task<ActionResult> ModelDetaisItems(int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, int? tof_assemblyId = null, int? tof_suppliersId = null, int? treeKindId = null)
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
                List<MODELTYPETREEITEMS_REST_TD> list = (from ee in await db.GetMODELTYPETREEITEMSAsync(langId, countryId, TYP_ID.Value, topicId.Value, tof_assemblyId.Value, tof_suppliersId.Value)
                                                         orderby ee.GA_NR, ee.SUP_ID
                                                         select ee).ToList();
                modeltypetreeitems = list;
                List<DICT_TD> tof_assembly = (from ee in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD ee) => new DICT_TD
                {
                    DictId = ee.GA_NR,
                    DictTitle = ee.GA_TEXT + ":" + ee.GA_NR.ToString()
                }).Distinct()
                                              orderby ee.DictTitle
                                              select ee).ToList();
                tof_assembly.Insert(0, new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                });
                base.ViewBag.sliTof_assembly = new SelectList(tof_assembly, "DictId", "DictTitle", tof_assemblyId.Value);
                List<DICT_TD> tof_suppliers = (from ee in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD ee) => new DICT_TD
                {
                    DictId = ee.SUP_ID,
                    DictTitle = ee.SUP_TEXT + ":" + ee.SUP_ID.ToString()
                }).Distinct()
                                               orderby ee.DictTitle
                                               select ee).ToList();
                tof_suppliers.Insert(0, new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                });
                base.ViewBag.sliTof_suppliers = new SelectList(tof_suppliers, "DictId", "DictTitle", tof_suppliersId.Value);
                if (tof_assemblyId.Value == 0 && tof_suppliersId.Value == 0)
                {
                    return View(modeltypetreeitems);
                }
            }
            else
            {
                DICT_TD tof_assemblyItem = null;
                string tadescr = null;
                List<DICT_TD> tof_assembly = await db.GetTof_assemblyAsync(langId, countryId, TYP_ID.Value, topicId.Value);
                if (tof_assemblyId.HasValue)
                {
                    tof_assemblyItem = tof_assembly.Where((DICT_TD ta) => ta.DictId == tof_assemblyId.Value).FirstOrDefault();
                }
                if (tof_assemblyItem == null)
                {
                    tof_assemblyItem = tof_assembly.FirstOrDefault();
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
                base.ViewBag.sliTof_assembly = new SelectList(tof_assembly, "DictId", "DictTitle", tof_assemblyId.Value);
                List<DICT_TD> tof_suppliers = (from ee in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD ee) => new DICT_TD
                {
                    DictId = ee.SUP_ID,
                    DictTitle = ee.SUP_TEXT + ":" + ee.SUP_ID.ToString()
                }).Distinct()
                                               orderby ee.DictTitle
                                               select ee).ToList();
                tof_suppliers.Insert(0, new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                });
                base.ViewBag.sliTof_suppliers = new SelectList(tof_suppliers, "DictId", "DictTitle", tof_suppliersId.Value);
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
            List<BranchRestSumTDES> branchresttdes = (builded == null) ? new List<BranchRestSumTDES>() : (from gt in dbRest.BranchRestTDES.Where(Expression.Lambda<Func<BranchRestTDES, bool>>(builded, new ParameterExpression[1]
                {
                e
                }))
                                                                                                          where gt.ArtAmount > 0
                                                                                                          select gt into ppp
                                                                                                          group ppp by new
                                                                                                          {
                                                                                                              ppp.ART_ARTICLE_NR,
                                                                                                              ppp.SUP_TEXT
                                                                                                          } into zzz
                                                                                                          select new BranchRestSumTDES
                                                                                                          {
                                                                                                              ART_ARTICLE_NR = zzz.Key.ART_ARTICLE_NR,
                                                                                                              SUP_TEXT = zzz.Key.SUP_TEXT,
                                                                                                              ArtAmount = zzz.Sum((BranchRestTDES aaa) => aaa.ArtAmount),
                                                                                                              ArtPrice = zzz.Max((BranchRestTDES bbb) => bbb.ArtPrice),
                                                                                                              MinArtPrice = zzz.Min((BranchRestTDES ccc) => ccc.ArtPrice)
                                                                                                          }).ToList();
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
                                                            ArtPrice = (subbrd?.ArtPrice ?? 0.0),
                                                            MinArtPrice = (subbrd?.MinArtPrice ?? 0.0)
                                                        } into sss
                                                        orderby sss.ArtAmount descending
                                                        select sss).ToList();
            return View(aResult);
        }

        public async Task<ActionResult> ModelDetaisItemsUTO(int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, int? tof_assemblyId = null, int? tof_suppliersId = null, int? treeKindId = null)
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
                List<MODELTYPETREEITEMS_REST_TD> list = (from ee in await db.GetMODELTYPETREEITEMSAsync(langId, countryId, TYP_ID.Value, topicId.Value, tof_assemblyId.Value, tof_suppliersId.Value)
                                                         orderby ee.GA_NR, ee.SUP_ID
                                                         select ee).ToList();
                modeltypetreeitems = list;
                List<DICT_TD> tof_assembly = (from ee in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD ee) => new DICT_TD
                {
                    DictId = ee.GA_NR,
                    DictTitle = ee.GA_TEXT + ":" + ee.GA_NR.ToString()
                }).Distinct()
                                              orderby ee.DictTitle
                                              select ee).ToList();
                tof_assembly.Insert(0, new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                });
                base.ViewBag.sliTof_assembly = new SelectList(tof_assembly, "DictId", "DictTitle", tof_assemblyId.Value);
                List<DICT_TD> tof_suppliers = (from ee in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD ee) => new DICT_TD
                {
                    DictId = ee.SUP_ID,
                    DictTitle = ee.SUP_TEXT + ":" + ee.SUP_ID.ToString()
                }).Distinct()
                                               orderby ee.DictTitle
                                               select ee).ToList();
                tof_suppliers.Insert(0, new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                });
                base.ViewBag.sliTof_suppliers = new SelectList(tof_suppliers, "DictId", "DictTitle", tof_suppliersId.Value);
                if (tof_assemblyId.Value == 0 && tof_suppliersId.Value == 0)
                {
                    return View(modeltypetreeitems);
                }
            }
            else
            {
                DICT_TD tof_assemblyItem = null;
                string tadescr = null;
                List<DICT_TD> tof_assembly = await db.GetTof_assemblyAsync(langId, countryId, TYP_ID.Value, topicId.Value);
                if (tof_assemblyId.HasValue)
                {
                    tof_assemblyItem = tof_assembly.Where((DICT_TD ta) => ta.DictId == tof_assemblyId.Value).FirstOrDefault();
                }
                if (tof_assemblyItem == null)
                {
                    tof_assemblyItem = tof_assembly.FirstOrDefault();
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
                base.ViewBag.sliTof_assembly = new SelectList(tof_assembly, "DictId", "DictTitle", tof_assemblyId.Value);
                List<DICT_TD> tof_suppliers = (from ee in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD ee) => new DICT_TD
                {
                    DictId = ee.SUP_ID,
                    DictTitle = ee.SUP_TEXT + ":" + ee.SUP_ID.ToString()
                }).Distinct()
                                               orderby ee.DictTitle
                                               select ee).ToList();
                tof_suppliers.Insert(0, new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                });
                base.ViewBag.sliTof_suppliers = new SelectList(tof_suppliers, "DictId", "DictTitle", tof_suppliersId.Value);
                base.ViewBag.tof_assemblyId = tof_assemblyId.Value;
                base.ViewBag.tof_suppliersId = tof_suppliersId.Value;
                if (tof_assemblyId.Value == 0 && tof_suppliersId.Value == 0)
                {
                    return View(modeltypetreeitems);
                }
            }
            ParameterExpression e = Expression.Parameter(typeof(SuppRestTDES), "e");
            Expression builded = null;
            foreach (MODELTYPETREEITEMS_REST_TD item in modeltypetreeitems)
            {
                Expression expression = Expression.AndAlso(Expression.Equal(Expression.Property(e, "ART_ARTICLE_NR"), Expression.Constant(item.ART_ARTICLE_NR, typeof(string))), Expression.Equal(Expression.Property(e, "SUP_TEXT"), Expression.Constant(item.SUP_TEXT, typeof(string))));
                builded = ((builded != null) ? Expression.OrElse(builded, expression) : expression);
            }
            List<BranchRestSumTDES> branchresttdes = (builded == null) ? new List<BranchRestSumTDES>() : (await (from gt in dbRest.SuppRestTDES.Where(Expression.Lambda<Func<SuppRestTDES, bool>>(builded, new ParameterExpression[1]
                 {
                e
                 }))
                                                                                                                 where gt.ArtAmount > 0
                                                                                                                 select gt into ppp
                                                                                                                 group ppp by new
                                                                                                                 {
                                                                                                                     ppp.ART_ARTICLE_NR,
                                                                                                                     ppp.SUP_TEXT
                                                                                                                 } into zzz
                                                                                                                 select new BranchRestSumTDES
                                                                                                                 {
                                                                                                                     ART_ARTICLE_NR = zzz.Key.ART_ARTICLE_NR,
                                                                                                                     SUP_TEXT = zzz.Key.SUP_TEXT,
                                                                                                                     ArtAmount = zzz.Sum((SuppRestTDES aaa) => aaa.ArtAmount),
                                                                                                                     ArtPrice = zzz.Max((SuppRestTDES bbb) => bbb.ArtPrice),
                                                                                                                     MinArtPrice = zzz.Min((SuppRestTDES ccc) => ccc.ArtPrice)
                                                                                                                 }).ToListAsync());
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
                                                            ArtPrice = (subbrd?.ArtPrice ?? 0.0),
                                                            MinArtPrice = (subbrd?.MinArtPrice ?? 0.0)
                                                        } into sss
                                                        orderby sss.ArtAmount descending
                                                        select sss).ToList();
            return View(aResult);
        }

        public async Task<ActionResult> ModelDetaisAnalog(int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, string ART_ARTICLE_NR = null, int? GA_NR = null, int? ART_ID = null, int? treeKindId = null, int? tof_assemblyId = null)
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
            List<BranchRestSumTDES> branchresttdes = (builded == null) ? new List<BranchRestSumTDES>() : (await (from gt in dbRest.BranchRestTDES.Where(Expression.Lambda<Func<BranchRestTDES, bool>>(builded, new ParameterExpression[1]
                 {
                e
                 }))
                                                                                                                 where gt.ArtAmount > 0
                                                                                                                 select gt into ppp
                                                                                                                 group ppp by new
                                                                                                                 {
                                                                                                                     ppp.ART_ARTICLE_NR,
                                                                                                                     ppp.SUP_TEXT
                                                                                                                 } into zzz
                                                                                                                 select new BranchRestSumTDES
                                                                                                                 {
                                                                                                                     ART_ARTICLE_NR = zzz.Key.ART_ARTICLE_NR,
                                                                                                                     SUP_TEXT = zzz.Key.SUP_TEXT,
                                                                                                                     ArtAmount = zzz.Sum((BranchRestTDES aaa) => aaa.ArtAmount),
                                                                                                                     ArtPrice = zzz.Max((BranchRestTDES bbb) => bbb.ArtPrice),
                                                                                                                     MinArtPrice = zzz.Min((BranchRestTDES ccc) => ccc.ArtPrice)
                                                                                                                 }).ToListAsync());
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
                                                   ArtPrice = (subbrd?.ArtPrice ?? 0.0),
                                                   MinArtPrice = (subbrd?.MinArtPrice ?? 0.0)
                                               } into ddd
                                               orderby ddd.ArtAmount descending
                                               select ddd).ToList();
            return View(aResult);
        }

        public async Task<ActionResult> ModelDetaisAnalogUTO(int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, string ART_ARTICLE_NR = null, int? GA_NR = null, int? ART_ID = null, int? treeKindId = null, int? tof_assemblyId = null)
        {
            Guid? searchEntGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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
            ParameterExpression e = Expression.Parameter(typeof(SuppRestTDES), "e");
            Expression builded = null;
            foreach (ANALOGOUS_REST_TD item in analoglist)
            {
                Expression expression = Expression.AndAlso(Expression.Equal(Expression.Property(e, "ART_ARTICLE_NR"), Expression.Constant(item.ART_ARTICLE_NR, typeof(string))), Expression.Equal(Expression.Property(e, "SUP_TEXT"), Expression.Constant(item.SUP_TEXT, typeof(string))));
                builded = ((builded != null) ? Expression.OrElse(builded, expression) : expression);
            }
            List<BranchRestSumTDES> branchresttdes = (builded == null) ? new List<BranchRestSumTDES>() : (await (from gt in dbRest.SuppRestTDES.Where(Expression.Lambda<Func<SuppRestTDES, bool>>(builded, new ParameterExpression[1]
                 {
                e
                 }))
                                                                                                                 where gt.ArtAmount > 0
                                                                                                                 select gt into ppp
                                                                                                                 group ppp by new
                                                                                                                 {
                                                                                                                     ppp.ART_ARTICLE_NR,
                                                                                                                     ppp.SUP_TEXT
                                                                                                                 } into zzz
                                                                                                                 select new BranchRestSumTDES
                                                                                                                 {
                                                                                                                     ART_ARTICLE_NR = zzz.Key.ART_ARTICLE_NR,
                                                                                                                     SUP_TEXT = zzz.Key.SUP_TEXT,
                                                                                                                     ArtAmount = zzz.Sum((SuppRestTDES aaa) => aaa.ArtAmount),
                                                                                                                     ArtPrice = zzz.Max((SuppRestTDES bbb) => bbb.ArtPrice),
                                                                                                                     MinArtPrice = zzz.Min((SuppRestTDES ccc) => ccc.ArtPrice)
                                                                                                                 }).ToListAsync());
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
                                                   ArtPrice = (subbrd?.ArtPrice ?? 0.0),
                                                   MinArtPrice = (subbrd?.MinArtPrice ?? 0.0)
                                               } into ddd
                                               orderby ddd.ArtAmount descending
                                               select ddd).ToList();
            return View(aResult);
        }

        public async Task<ActionResult> ModelDetaisAppliedTo(int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, string ART_ARTICLE_NR = null, int? GA_NR = null, int? ART_ID = null, int? treeKindId = null, int? tof_assemblyId = null, int? page = null)
        {
            page = (page ?? 1);
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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

        public async Task<ActionResult> ModelDetaisAppliedToUTO(int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, string ART_ARTICLE_NR = null, int? GA_NR = null, int? ART_ID = null, int? treeKindId = null, int? tof_assemblyId = null, int? page = null)
        {
            page = (page ?? 1);
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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

        public async Task<ActionResult> ModelItemDetais(int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, string ART_ARTICLE_NR = null, int? GA_NR = null, int? ART_ID = null, int? treeKindId = null, int? tof_assemblyId = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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

        public async Task<ActionResult> ModelItemDetaisUTO(int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, string ART_ARTICLE_NR = null, int? GA_NR = null, int? ART_ID = null, int? treeKindId = null, int? tof_assemblyId = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
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
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
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

        public async Task<ActionResult> ArticleShopIndex(string ART_ARTICLE_NR = null, string SUP_TEXT = null, string artId = null, int? srchTp = null, int? page = null, string searchTown = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            page = (page ?? 1);
            int pageSize = 20;
            int pageNumber = page.Value;
            if (!string.IsNullOrEmpty(searchTown))
            {
                searchTown = searchTown.Trim();
            }
            List<MODELTYPETREEITEMMANID_TD> searchTowns = await (from uu in dbRest.GuestBranchTDES.Select((GuestBranchTDES st) => new MODELTYPETREEITEMMANID_TD
            {
                SUP_TEXT = st.AddressSettlementName,
                TEX_VALUE = st.AddressSettlementName
            }).Distinct()
                                                                 orderby uu.SUP_TEXT
                                                                 select uu).ToListAsync();
            searchTowns.Insert(0, new MODELTYPETREEITEMMANID_TD
            {
                SUP_TEXT = "",
                TEX_VALUE = Resources.SHOWALL
            });
            base.ViewBag.slisearchTown = new SelectList(searchTowns, "SUP_TEXT", "TEX_VALUE", searchTown);
            base.ViewBag.searchTown = searchTown;
            IPagedList<GuestBranchSumTDES> aResult = (!string.IsNullOrEmpty(searchTown)) ? (await (from r in dbRest.BranchRestTDES
                                                                                                   where r.ART_ARTICLE_NR == ART_ARTICLE_NR && r.SUP_TEXT == SUP_TEXT && r.ArtAmount > 0
                                                                                                   group r by r.EntBranchGuid into dbRestgrp
                                                                                                   select new
                                                                                                   {
                                                                                                       EntBranchGuid = dbRestgrp.Key,
                                                                                                       ArtAmount = dbRestgrp.Sum((BranchRestTDES r) => r.ArtAmount),
                                                                                                       ArtPrice = dbRestgrp.Min((BranchRestTDES r) => r.ArtPrice)
                                                                                                   } into dbRestgroupt
                                                                                                   join b in dbRest.GuestBranchTDES on dbRestgroupt.EntBranchGuid equals b.EntBranchGuid into rbs
                                                                                                   from rb in rbs
                                                                                                   where rb.AddressSettlementName == searchTown
                                                                                                   orderby dbRestgroupt.ArtAmount
                                                                                                   select new GuestBranchSumTDES
                                                                                                   {
                                                                                                       EntBranchGuid = rb.EntBranchGuid,
                                                                                                       EntBranchDescription = rb.EntBranchDescription,
                                                                                                       AddressPostCode = rb.AddressPostCode,
                                                                                                       AddressRegion = rb.AddressRegion,
                                                                                                       AddressSettlementName = rb.AddressSettlementName,
                                                                                                       AddressStreetName = rb.AddressStreetName,
                                                                                                       AddressLongitude = rb.AddressLongitude,
                                                                                                       AddressLatitude = rb.AddressLatitude,
                                                                                                       WorkingDays = rb.WorkingDays,
                                                                                                       WorkingTime = rb.WorkingTime,
                                                                                                       ArtAmount = dbRestgroupt.ArtAmount,
                                                                                                       ArtPrice = Math.Round(dbRestgroupt.ArtPrice * rb.ExchRate * rb.Multiplexer / rb.Rounding) * rb.Rounding,
                                                                                                       Phones = rb.Phones,
                                                                                                       SiteUrl = rb.SiteUrl,
                                                                                                       ShopLicense = rb.ShopLicense,
                                                                                                       ShopSupply = rb.ShopSupply
                                                                                                   }).ToPagedListAsync(pageNumber, pageSize)) : (await (from r in dbRest.BranchRestTDES
                                                                                                                                                        where r.ART_ARTICLE_NR == ART_ARTICLE_NR && r.SUP_TEXT == SUP_TEXT && r.ArtAmount > 0
                                                                                                                                                        group r by r.EntBranchGuid into dbRestgrp
                                                                                                                                                        select new
                                                                                                                                                        {
                                                                                                                                                            EntBranchGuid = dbRestgrp.Key,
                                                                                                                                                            ArtAmount = dbRestgrp.Sum((BranchRestTDES r) => r.ArtAmount),
                                                                                                                                                            ArtPrice = dbRestgrp.Min((BranchRestTDES r) => r.ArtPrice)
                                                                                                                                                        } into dbRestgroupt
                                                                                                                                                        join b in dbRest.GuestBranchTDES on dbRestgroupt.EntBranchGuid equals b.EntBranchGuid into rbs
                                                                                                                                                        from rb in rbs
                                                                                                                                                        orderby dbRestgroupt.ArtAmount
                                                                                                                                                        select new GuestBranchSumTDES
                                                                                                                                                        {
                                                                                                                                                            EntBranchGuid = rb.EntBranchGuid,
                                                                                                                                                            EntBranchDescription = rb.EntBranchDescription,
                                                                                                                                                            AddressPostCode = rb.AddressPostCode,
                                                                                                                                                            AddressRegion = rb.AddressRegion,
                                                                                                                                                            AddressSettlementName = rb.AddressSettlementName,
                                                                                                                                                            AddressStreetName = rb.AddressStreetName,
                                                                                                                                                            AddressLongitude = rb.AddressLongitude,
                                                                                                                                                            AddressLatitude = rb.AddressLatitude,
                                                                                                                                                            WorkingDays = rb.WorkingDays,
                                                                                                                                                            WorkingTime = rb.WorkingTime,
                                                                                                                                                            ArtAmount = dbRestgroupt.ArtAmount,
                                                                                                                                                            ArtPrice = Math.Round(dbRestgroupt.ArtPrice * rb.ExchRate * rb.Multiplexer / rb.Rounding) * rb.Rounding,
                                                                                                                                                            Phones = rb.Phones,
                                                                                                                                                            SiteUrl = rb.SiteUrl,
                                                                                                                                                            ShopLicense = rb.ShopLicense,
                                                                                                                                                            ShopSupply = rb.ShopSupply
                                                                                                                                                        }).ToPagedListAsync(pageNumber, pageSize));
            base.ViewBag.ART_ARTICLE_NR = ART_ARTICLE_NR;
            base.ViewBag.SUP_TEXT = SUP_TEXT;
            base.ViewBag.artId = artId;
            base.ViewBag.srchTp = srchTp;
            return View(aResult);
        }

        public async Task<ActionResult> ArticleShopIndexUTO(string ART_ARTICLE_NR = null, string SUP_TEXT = null, string artId = null, int? srchTp = null, int? page = null, string searchTown = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            page = (page ?? 1);
            int pageSize = 20;
            int pageNumber = page.Value;
            if (!string.IsNullOrEmpty(searchTown))
            {
                searchTown = searchTown.Trim();
            }
            List<MODELTYPETREEITEMMANID_TD> searchTowns = await (from uu in dbRest.GuestBranchTDES.Select((GuestBranchTDES st) => new MODELTYPETREEITEMMANID_TD
            {
                SUP_TEXT = st.AddressSettlementName,
                TEX_VALUE = st.AddressSettlementName
            }).Distinct()
                                                                 orderby uu.SUP_TEXT
                                                                 select uu).ToListAsync();
            searchTowns.Insert(0, new MODELTYPETREEITEMMANID_TD
            {
                SUP_TEXT = "",
                TEX_VALUE = Resources.SHOWALL
            });
            base.ViewBag.slisearchTown = new SelectList(searchTowns, "SUP_TEXT", "TEX_VALUE", searchTown);
            base.ViewBag.searchTown = searchTown;
            IPagedList<GuestBranchSumTDES> aResult = (!string.IsNullOrEmpty(searchTown)) ? (await (from r in dbRest.SuppRestTDES
                                                                                                   where r.ART_ARTICLE_NR == ART_ARTICLE_NR && r.SUP_TEXT == SUP_TEXT && r.ArtAmount > 0
                                                                                                   group r by r.EntSupplierId into dbRestgrp
                                                                                                   select new
                                                                                                   {
                                                                                                       EntSupplierId = dbRestgrp.Key,
                                                                                                       ArtAmount = dbRestgrp.Sum((SuppRestTDES r) => r.ArtAmount),
                                                                                                       ArtPrice = dbRestgrp.Min((SuppRestTDES r) => r.ArtPrice)
                                                                                                   } into dbRestgroupt
                                                                                                   join b in dbRest.BranchSuppTDES on dbRestgroupt.EntSupplierId equals b.EntSupplierId
                                                                                                   join c in dbRest.GuestBranchTDES on b.EntBranchGuid equals c.EntBranchGuid into rbs
                                                                                                   from rb in rbs
                                                                                                   where rb.AddressSettlementName == searchTown
                                                                                                   orderby dbRestgroupt.ArtAmount
                                                                                                   select new GuestBranchSumTDES
                                                                                                   {
                                                                                                       EntBranchGuid = rb.EntBranchGuid,
                                                                                                       EntBranchDescription = rb.EntBranchDescription,
                                                                                                       AddressPostCode = rb.AddressPostCode,
                                                                                                       AddressRegion = rb.AddressRegion,
                                                                                                       AddressSettlementName = rb.AddressSettlementName,
                                                                                                       AddressStreetName = rb.AddressStreetName,
                                                                                                       AddressLongitude = rb.AddressLongitude,
                                                                                                       AddressLatitude = rb.AddressLatitude,
                                                                                                       WorkingDays = rb.WorkingDays,
                                                                                                       WorkingTime = rb.WorkingTime,
                                                                                                       ArtAmount = dbRestgroupt.ArtAmount,
                                                                                                       ArtPrice = Math.Round(dbRestgroupt.ArtPrice * b.ExchRate * b.Multiplexer / b.Rounding) * b.Rounding,
                                                                                                       SuppTime = b.SuppTime,
                                                                                                       Phones = rb.Phones,
                                                                                                       SiteUrl = rb.SiteUrl,
                                                                                                       ShopLicense = rb.ShopLicense,
                                                                                                       ShopSupply = rb.ShopSupply
                                                                                                   }).ToPagedListAsync(pageNumber, pageSize)) : (await (from r in dbRest.SuppRestTDES
                                                                                                                                                        where r.ART_ARTICLE_NR == ART_ARTICLE_NR && r.SUP_TEXT == SUP_TEXT && r.ArtAmount > 0
                                                                                                                                                        group r by r.EntSupplierId into dbRestgrp
                                                                                                                                                        select new
                                                                                                                                                        {
                                                                                                                                                            EntSupplierId = dbRestgrp.Key,
                                                                                                                                                            ArtAmount = dbRestgrp.Sum((SuppRestTDES r) => r.ArtAmount),
                                                                                                                                                            ArtPrice = dbRestgrp.Min((SuppRestTDES r) => r.ArtPrice)
                                                                                                                                                        } into dbRestgroupt
                                                                                                                                                        join b in dbRest.BranchSuppTDES on dbRestgroupt.EntSupplierId equals b.EntSupplierId
                                                                                                                                                        join c in dbRest.GuestBranchTDES on b.EntBranchGuid equals c.EntBranchGuid into rbs
                                                                                                                                                        from rb in rbs
                                                                                                                                                        orderby dbRestgroupt.ArtAmount
                                                                                                                                                        select new GuestBranchSumTDES
                                                                                                                                                        {
                                                                                                                                                            EntBranchGuid = rb.EntBranchGuid,
                                                                                                                                                            EntBranchDescription = rb.EntBranchDescription,
                                                                                                                                                            AddressPostCode = rb.AddressPostCode,
                                                                                                                                                            AddressRegion = rb.AddressRegion,
                                                                                                                                                            AddressSettlementName = rb.AddressSettlementName,
                                                                                                                                                            AddressStreetName = rb.AddressStreetName,
                                                                                                                                                            AddressLongitude = rb.AddressLongitude,
                                                                                                                                                            AddressLatitude = rb.AddressLatitude,
                                                                                                                                                            WorkingDays = rb.WorkingDays,
                                                                                                                                                            WorkingTime = rb.WorkingTime,
                                                                                                                                                            ArtAmount = dbRestgroupt.ArtAmount,
                                                                                                                                                            ArtPrice = Math.Round(dbRestgroupt.ArtPrice * b.ExchRate * b.Multiplexer / b.Rounding) * b.Rounding,
                                                                                                                                                            SuppTime = b.SuppTime,
                                                                                                                                                            Phones = rb.Phones,
                                                                                                                                                            SiteUrl = rb.SiteUrl,
                                                                                                                                                            ShopLicense = rb.ShopLicense,
                                                                                                                                                            ShopSupply = rb.ShopSupply
                                                                                                                                                        }).ToPagedListAsync(pageNumber, pageSize));
            base.ViewBag.ART_ARTICLE_NR = ART_ARTICLE_NR;
            base.ViewBag.SUP_TEXT = SUP_TEXT;
            base.ViewBag.artId = artId;
            base.ViewBag.srchTp = srchTp;
            return View(aResult);
        }

        public async Task<ActionResult> ArticleShopDetaisItems(string ART_ARTICLE_NR = null, string SUP_TEXT = null, int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, int? tof_assemblyId = null, int? tof_suppliersId = null, int? treeKindId = null, int? page = null, string searchTown = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            fluelId = (fluelId ?? 71334);
            MFA_ID = (MFA_ID ?? 0);
            MOD_ID = (MOD_ID ?? 0);
            TYP_ID = (TYP_ID ?? 0);
            topicId = (topicId ?? 0);
            tof_suppliersId = (tof_suppliersId ?? 0);
            tof_assemblyId = (tof_assemblyId ?? 0);
            base.ViewBag.MFA_ID = MFA_ID.Value;
            base.ViewBag.MOD_ID = MOD_ID.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.TYP_ID = TYP_ID.Value;
            base.ViewBag.topicId = topicId.Value;
            base.ViewBag.tof_assemblyId = tof_assemblyId.Value;
            base.ViewBag.tof_suppliersId = tof_suppliersId.Value;
            base.ViewBag.treeKindId = treeKindId;
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            page = (page ?? 1);
            int pageSize = 20;
            int pageNumber = page.Value;
            if (!string.IsNullOrEmpty(searchTown))
            {
                searchTown = searchTown.Trim();
            }
            List<MODELTYPETREEITEMMANID_TD> searchTowns = await (from uu in dbRest.GuestBranchTDES.Select((GuestBranchTDES st) => new MODELTYPETREEITEMMANID_TD
            {
                SUP_TEXT = st.AddressSettlementName,
                TEX_VALUE = st.AddressSettlementName
            }).Distinct()
                                                                 orderby uu.SUP_TEXT
                                                                 select uu).ToListAsync();
            searchTowns.Insert(0, new MODELTYPETREEITEMMANID_TD
            {
                SUP_TEXT = "",
                TEX_VALUE = Resources.SHOWALL
            });
            base.ViewBag.slisearchTown = new SelectList(searchTowns, "SUP_TEXT", "TEX_VALUE", searchTown);
            base.ViewBag.searchTown = searchTown;
            IPagedList<GuestBranchSumTDES> aResult = (!string.IsNullOrEmpty(searchTown)) ? (await (from r in dbRest.BranchRestTDES
                                                                                                   where r.ART_ARTICLE_NR == ART_ARTICLE_NR && r.SUP_TEXT == SUP_TEXT && r.ArtAmount > 0
                                                                                                   join b in dbRest.GuestBranchTDES on r.EntBranchGuid equals b.EntBranchGuid into rbs
                                                                                                   from rb in rbs
                                                                                                   where rb.AddressSettlementName == searchTown
                                                                                                   orderby r.ArtAmount
                                                                                                   select new GuestBranchSumTDES
                                                                                                   {
                                                                                                       EntBranchGuid = rb.EntBranchGuid,
                                                                                                       EntBranchDescription = rb.EntBranchDescription,
                                                                                                       AddressPostCode = rb.AddressPostCode,
                                                                                                       AddressRegion = rb.AddressRegion,
                                                                                                       AddressSettlementName = rb.AddressSettlementName,
                                                                                                       AddressStreetName = rb.AddressStreetName,
                                                                                                       AddressLongitude = rb.AddressLongitude,
                                                                                                       AddressLatitude = rb.AddressLatitude,
                                                                                                       WorkingDays = rb.WorkingDays,
                                                                                                       WorkingTime = rb.WorkingTime,
                                                                                                       ArtAmount = r.ArtAmount,
                                                                                                       ArtPrice = Math.Round(r.ArtPrice * rb.ExchRate * rb.Multiplexer / rb.Rounding) * rb.Rounding,
                                                                                                       Phones = rb.Phones,
                                                                                                       SiteUrl = rb.SiteUrl,
                                                                                                       ShopLicense = rb.ShopLicense,
                                                                                                       ShopSupply = rb.ShopSupply
                                                                                                   }).ToPagedListAsync(pageNumber, pageSize)) : (await (from r in dbRest.BranchRestTDES
                                                                                                                                                        where r.ART_ARTICLE_NR == ART_ARTICLE_NR && r.SUP_TEXT == SUP_TEXT && r.ArtAmount > 0
                                                                                                                                                        join b in dbRest.GuestBranchTDES on r.EntBranchGuid equals b.EntBranchGuid into rbs
                                                                                                                                                        from rb in rbs
                                                                                                                                                        orderby r.ArtAmount
                                                                                                                                                        select new GuestBranchSumTDES
                                                                                                                                                        {
                                                                                                                                                            EntBranchGuid = rb.EntBranchGuid,
                                                                                                                                                            EntBranchDescription = rb.EntBranchDescription,
                                                                                                                                                            AddressPostCode = rb.AddressPostCode,
                                                                                                                                                            AddressRegion = rb.AddressRegion,
                                                                                                                                                            AddressSettlementName = rb.AddressSettlementName,
                                                                                                                                                            AddressStreetName = rb.AddressStreetName,
                                                                                                                                                            AddressLongitude = rb.AddressLongitude,
                                                                                                                                                            AddressLatitude = rb.AddressLatitude,
                                                                                                                                                            WorkingDays = rb.WorkingDays,
                                                                                                                                                            WorkingTime = rb.WorkingTime,
                                                                                                                                                            ArtAmount = r.ArtAmount,
                                                                                                                                                            ArtPrice = Math.Round(r.ArtPrice * rb.ExchRate * rb.Multiplexer / rb.Rounding) * rb.Rounding,
                                                                                                                                                            Phones = rb.Phones,
                                                                                                                                                            SiteUrl = rb.SiteUrl,
                                                                                                                                                            ShopLicense = rb.ShopLicense,
                                                                                                                                                            ShopSupply = rb.ShopSupply
                                                                                                                                                        }).ToPagedListAsync(pageNumber, pageSize));
            base.ViewBag.ART_ARTICLE_NR = ART_ARTICLE_NR;
            base.ViewBag.SUP_TEXT = SUP_TEXT;
            return View(aResult);
        }

        public async Task<ActionResult> ArticleShopDetaisItemsUTO(string ART_ARTICLE_NR = null, string SUP_TEXT = null, int? MFA_ID = null, int? MOD_ID = null, int? fluelId = null, int? TYP_ID = null, int? topicId = null, int? tof_assemblyId = null, int? tof_suppliersId = null, int? treeKindId = null, int? page = null, string searchTown = null)
        {
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            fluelId = (fluelId ?? 71334);
            MFA_ID = (MFA_ID ?? 0);
            MOD_ID = (MOD_ID ?? 0);
            TYP_ID = (TYP_ID ?? 0);
            topicId = (topicId ?? 0);
            tof_suppliersId = (tof_suppliersId ?? 0);
            tof_assemblyId = (tof_assemblyId ?? 0);
            base.ViewBag.MFA_ID = MFA_ID.Value;
            base.ViewBag.MOD_ID = MOD_ID.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.TYP_ID = TYP_ID.Value;
            base.ViewBag.topicId = topicId.Value;
            base.ViewBag.tof_assemblyId = tof_assemblyId.Value;
            base.ViewBag.tof_suppliersId = tof_suppliersId.Value;
            base.ViewBag.treeKindId = treeKindId;
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            page = (page ?? 1);
            int pageSize = 20;
            int pageNumber = page.Value;
            if (!string.IsNullOrEmpty(searchTown))
            {
                searchTown = searchTown.Trim();
            }
            List<MODELTYPETREEITEMMANID_TD> searchTowns = await (from uu in dbRest.GuestBranchTDES.Select((GuestBranchTDES st) => new MODELTYPETREEITEMMANID_TD
            {
                SUP_TEXT = st.AddressSettlementName,
                TEX_VALUE = st.AddressSettlementName
            }).Distinct()
                                                                 orderby uu.SUP_TEXT
                                                                 select uu).ToListAsync();
            searchTowns.Insert(0, new MODELTYPETREEITEMMANID_TD
            {
                SUP_TEXT = "",
                TEX_VALUE = Resources.SHOWALL
            });
            base.ViewBag.slisearchTown = new SelectList(searchTowns, "SUP_TEXT", "TEX_VALUE", searchTown);
            base.ViewBag.searchTown = searchTown;
            IPagedList<GuestBranchSumTDES> aResult = (!string.IsNullOrEmpty(searchTown)) ? (await (from r in dbRest.SuppRestTDES
                                                                                                   where r.ART_ARTICLE_NR == ART_ARTICLE_NR && r.SUP_TEXT == SUP_TEXT && r.ArtAmount > 0
                                                                                                   group r by r.EntSupplierId into dbRestgrp
                                                                                                   select new
                                                                                                   {
                                                                                                       EntSupplierId = dbRestgrp.Key,
                                                                                                       ArtAmount = dbRestgrp.Sum((SuppRestTDES r) => r.ArtAmount),
                                                                                                       ArtPrice = dbRestgrp.Min((SuppRestTDES r) => r.ArtPrice)
                                                                                                   } into dbRestgroupt
                                                                                                   join b in dbRest.BranchSuppTDES on dbRestgroupt.EntSupplierId equals b.EntSupplierId
                                                                                                   join c in dbRest.GuestBranchTDES on b.EntBranchGuid equals c.EntBranchGuid into rbs
                                                                                                   from rb in rbs
                                                                                                   where rb.AddressSettlementName == searchTown
                                                                                                   orderby dbRestgroupt.ArtAmount
                                                                                                   select new GuestBranchSumTDES
                                                                                                   {
                                                                                                       EntBranchGuid = rb.EntBranchGuid,
                                                                                                       EntBranchDescription = rb.EntBranchDescription,
                                                                                                       AddressPostCode = rb.AddressPostCode,
                                                                                                       AddressRegion = rb.AddressRegion,
                                                                                                       AddressSettlementName = rb.AddressSettlementName,
                                                                                                       AddressStreetName = rb.AddressStreetName,
                                                                                                       AddressLongitude = rb.AddressLongitude,
                                                                                                       AddressLatitude = rb.AddressLatitude,
                                                                                                       WorkingDays = rb.WorkingDays,
                                                                                                       WorkingTime = rb.WorkingTime,
                                                                                                       ArtAmount = dbRestgroupt.ArtAmount,
                                                                                                       ArtPrice = Math.Round(dbRestgroupt.ArtPrice * b.ExchRate * b.Multiplexer / b.Rounding) * b.Rounding,
                                                                                                       SuppTime = b.SuppTime,
                                                                                                       Phones = rb.Phones,
                                                                                                       SiteUrl = rb.SiteUrl,
                                                                                                       ShopLicense = rb.ShopLicense,
                                                                                                       ShopSupply = rb.ShopSupply
                                                                                                   }).ToPagedListAsync(pageNumber, pageSize)) : (await (from r in dbRest.SuppRestTDES
                                                                                                                                                        where r.ART_ARTICLE_NR == ART_ARTICLE_NR && r.SUP_TEXT == SUP_TEXT && r.ArtAmount > 0
                                                                                                                                                        group r by r.EntSupplierId into dbRestgrp
                                                                                                                                                        select new
                                                                                                                                                        {
                                                                                                                                                            EntSupplierId = dbRestgrp.Key,
                                                                                                                                                            ArtAmount = dbRestgrp.Sum((SuppRestTDES r) => r.ArtAmount),
                                                                                                                                                            ArtPrice = dbRestgrp.Min((SuppRestTDES r) => r.ArtPrice)
                                                                                                                                                        } into dbRestgroupt
                                                                                                                                                        join b in dbRest.BranchSuppTDES on dbRestgroupt.EntSupplierId equals b.EntSupplierId
                                                                                                                                                        join c in dbRest.GuestBranchTDES on b.EntBranchGuid equals c.EntBranchGuid into rbs
                                                                                                                                                        from rb in rbs
                                                                                                                                                        orderby dbRestgroupt.ArtAmount
                                                                                                                                                        select new GuestBranchSumTDES
                                                                                                                                                        {
                                                                                                                                                            EntBranchGuid = rb.EntBranchGuid,
                                                                                                                                                            EntBranchDescription = rb.EntBranchDescription,
                                                                                                                                                            AddressPostCode = rb.AddressPostCode,
                                                                                                                                                            AddressRegion = rb.AddressRegion,
                                                                                                                                                            AddressSettlementName = rb.AddressSettlementName,
                                                                                                                                                            AddressStreetName = rb.AddressStreetName,
                                                                                                                                                            AddressLongitude = rb.AddressLongitude,
                                                                                                                                                            AddressLatitude = rb.AddressLatitude,
                                                                                                                                                            WorkingDays = rb.WorkingDays,
                                                                                                                                                            WorkingTime = rb.WorkingTime,
                                                                                                                                                            ArtAmount = dbRestgroupt.ArtAmount,
                                                                                                                                                            ArtPrice = Math.Round(dbRestgroupt.ArtPrice * b.ExchRate * b.Multiplexer / b.Rounding) * b.Rounding,
                                                                                                                                                            SuppTime = b.SuppTime,
                                                                                                                                                            Phones = rb.Phones,
                                                                                                                                                            SiteUrl = rb.SiteUrl,
                                                                                                                                                            ShopLicense = rb.ShopLicense,
                                                                                                                                                            ShopSupply = rb.ShopSupply
                                                                                                                                                        }).ToPagedListAsync(pageNumber, pageSize));
            base.ViewBag.ART_ARTICLE_NR = ART_ARTICLE_NR;
            base.ViewBag.SUP_TEXT = SUP_TEXT;
            return View(aResult);
        }

        [Authorize]
        public async Task<ActionResult> DoSelectArticle(string ART_ARTICLE_NR = null, string SUP_TEXT = null, Guid? EntBranchGuid = null)
        {
            if (ART_ARTICLE_NR == null || SUP_TEXT == null || !EntBranchGuid.HasValue)
            {
                base.ModelState.AddModelError("", Resources.DoSelectArticle_Error);
                return View();
            }
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            bool hasFound = true;
            BranchRestTDES branchresttdes = await (from e in dbRest.BranchRestTDES.Include("BranchRestArticleDescriptionTDES")
                                                   where e.ART_ARTICLE_NR == ART_ARTICLE_NR && e.SUP_TEXT == SUP_TEXT && (Guid)(Guid?)e.EntBranchGuid == (Guid)EntBranchGuid
                                                   select e).FirstOrDefaultAsync();
            if (branchresttdes == null)
            {
                hasFound = false;
                var xxx = await (from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                 where e.ART_ARTICLE_NR == ART_ARTICLE_NR && e.SUP_TEXT == SUP_TEXT
                                 join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                 where (Guid)(Guid?)b.EntBranchGuid == (Guid)EntBranchGuid
                                 select new
                                 {
                                     EntBranchGuid = b.EntBranchGuid,
                                     EntBranchArticle = e.EntBranchArticle,
                                     EntBranchSup = e.EntBranchSup,
                                     ART_ARTICLE_NR = e.ART_ARTICLE_NR,
                                     SUP_TEXT = e.SUP_TEXT,
                                     ArtAmount = e.ArtAmount,
                                     ArtPrice = e.ArtPrice,
                                     ExternArticleEAN = e.ExternArticleEAN,
                                     ExchRate = b.ExchRate,
                                     Rounding = b.Rounding,
                                     Multiplexer = b.Multiplexer,
                                     brad = e.BranchRestArticleDescriptionTDES
                                 }).FirstOrDefaultAsync();
                if (xxx != null)
                {
                    branchresttdes = new BranchRestTDES
                    {
                        EntBranchGuid = xxx.EntBranchGuid,
                        EntBranchArticle = xxx.EntBranchArticle,
                        EntBranchSup = xxx.EntBranchSup,
                        ART_ARTICLE_NR = xxx.ART_ARTICLE_NR,
                        SUP_TEXT = xxx.SUP_TEXT,
                        ArtAmount = xxx.ArtAmount,
                        ArtPrice = Math.Round(xxx.ArtPrice * xxx.Multiplexer * xxx.ExchRate / xxx.Rounding) * xxx.Rounding,
                        ExternArticleEAN = xxx.ExternArticleEAN
                    };
                    branchresttdes.BranchRestArticleDescriptionTDES = xxx.brad;
                }
            }
            if (branchresttdes == null)
            {
                base.ModelState.AddModelError("", Resources.DoSelectArticle_RESTNOTFOUND);
                return View();
            }
            GuestBranchTDES guestbranchtdes = await dbRest.GuestBranchTDES.Where((GuestBranchTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)EntBranchGuid).FirstOrDefaultAsync();
            string BranchName = "Не задан";
            if (guestbranchtdes != null)
            {
                BranchName = guestbranchtdes.EntBranchDescription;
                if (hasFound)
                {
                    branchresttdes.ArtPrice = Math.Round(branchresttdes.ArtPrice * guestbranchtdes.Multiplexer * guestbranchtdes.ExchRate / guestbranchtdes.Rounding) * guestbranchtdes.Rounding;
                }
            }
            BranchRestTmp branchresttmp = new BranchRestTmp();
            branchresttmp.CopyFrom(branchresttdes);
            return RedirectToAction("DoCreateArticle", "GuestOrderArticle", new
            {
                EntBranchGuid = branchresttdes.EntBranchGuid,
                EntBranchArticle = branchresttdes.EntBranchArticle,
                EntBranchSup = branchresttdes.EntBranchSup,
                ART_ARTICLE_NR = branchresttdes.ART_ARTICLE_NR,
                SUP_TEXT = branchresttdes.SUP_TEXT,
                ArtAmount = 1,
                ArtPrice = branchresttdes.ArtPrice,
                EntArticleDescription = branchresttdes.BranchRestArticleDescriptionTDES.EntArticleDescription,
                ExternArticleEAN = branchresttdes.ExternArticleEAN,
                BranchName = BranchName
            });
        }

        [Authorize]
        public async Task<ActionResult> DoSelectArticleUTO(string ART_ARTICLE_NR = null, string SUP_TEXT = null, Guid? EntBranchGuid = null)
        {
            if (ART_ARTICLE_NR == null || SUP_TEXT == null || !EntBranchGuid.HasValue)
            {
                base.ModelState.AddModelError("", Resources.DoSelectArticle_Error);
                return View();
            }
            new Guid?(new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17));
            Guid? searchEntBranchGuid = new Guid(0, 0, 0, 17, 17, 17, 17, 17, 17, 17, 17);
            EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                               where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                               select ee).FirstOrDefaultAsync();
            if (enterprisebranchtdes == null)
            {
                base.ModelState.AddModelError("", Resources.GuestEnterpriseBranchTDES_NOTFOUND);
                return View();
            }
            base.ViewBag.EntDescription = enterprisebranchtdes.EnterpriseTDES.EntDescription;
            base.ViewBag.EntBranchDescription = enterprisebranchtdes.EntBranchDescription;
            base.ViewBag.SearchEntGuid = enterprisebranchtdes.EntGuid;
            base.ViewBag.SearchEntBranchGuid = enterprisebranchtdes.EntBranchGuid;
            CarShopRestContextCatalog = enterprisebranchtdes.TecDocCatalog;
            TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
            BranchRestTDES branchresttdes = await (from e in dbRest.SuppRestTDES.Include("BranchRestArticleDescriptionTDES")
                                                   join b in dbRest.BranchSuppTDES on e.EntSupplierId equals b.EntSupplierId
                                                   where e.ART_ARTICLE_NR == ART_ARTICLE_NR && e.SUP_TEXT == SUP_TEXT && (Guid)(Guid?)b.EntBranchGuid == (Guid)EntBranchGuid
                                                   select new BranchRestTDES
                                                   {
                                                       EntBranchGuid = b.EntBranchGuid,
                                                       EntBranchArticle = e.EntBranchArticle,
                                                       EntBranchSup = e.EntBranchSup,
                                                       ART_ARTICLE_NR = e.ART_ARTICLE_NR,
                                                       SUP_TEXT = e.SUP_TEXT,
                                                       ArtAmount = e.ArtAmount,
                                                       ArtPrice = e.ArtPrice
                                                   }).FirstOrDefaultAsync();
            if (branchresttdes == null)
            {
                base.ModelState.AddModelError("", Resources.DoSelectArticle_RESTNOTFOUND);
                return View();
            }
            GuestBranchTDES guestbranchtdes = await dbRest.GuestBranchTDES.Where((GuestBranchTDES e) => (Guid)(Guid?)e.EntBranchGuid == (Guid)EntBranchGuid).FirstOrDefaultAsync();
            string BranchName = "Не задан";
            if (guestbranchtdes != null)
            {
                BranchName = guestbranchtdes.EntBranchDescription;
            }
            BranchRestTmp branchresttmp = new BranchRestTmp();
            branchresttmp.CopyFrom(branchresttdes);
            return RedirectToAction("DoCreateArticle", "GuestOrderArticle", new
            {
                EntBranchGuid = branchresttdes.EntBranchGuid,
                EntBranchArticle = branchresttdes.EntBranchArticle,
                EntBranchSup = branchresttdes.EntBranchSup,
                ART_ARTICLE_NR = branchresttdes.ART_ARTICLE_NR,
                SUP_TEXT = branchresttdes.SUP_TEXT,
                ArtAmount = 1,
                ArtPrice = branchresttdes.ArtPrice,
                EntArticleDescription = branchresttdes.BranchRestArticleDescriptionTDES.EntArticleDescription,
                ExternArticleEAN = branchresttdes.ExternArticleEAN,
                BranchName = BranchName
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