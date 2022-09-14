// CarShop.Controllers.TecDocController
using CarShop.Controllers;
using CarShop.Models;
using CarShop.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;


namespace CarShop.Controllers
{

    public class TecDocController : Controller
    {
        private CarShopContext dbCarShop = new CarShopContext();

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

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
            base.ViewBag.IsBranchAdmin = base.User.IsInRole("BranchAdmin");
            base.ViewBag.IsBranchBooker = base.User.IsInRole("BranchBooker");
        }

        public async Task<ActionResult> Index(int? countryId = null, int? langId = null, int? bandId = null, int? fluelId = null)
        {
            countryId = (countryId ?? 249);
            langId = (langId ?? 16);
            fluelId = (fluelId ?? 71334);
            base.ViewBag.sliLANGUAGES = new SelectList(await db.GetLANGUAGESAsync(langId.Value), "LNG_ID", "TEX_TEXT", langId);
            base.ViewBag.sliCOUNTRIES = new SelectList(await db.GetCOUNTRIESAsync(langId.Value), "COU_ID", "TEX_TEXT", countryId);
            List<BRAND_TD> brands = await db.GetBRANDSAsync();
            if (!bandId.HasValue)
            {
                bandId = ((brands.Count <= 0) ? new int?(0) : new int?(brands[0].MFA_ID));
            }
            base.ViewBag.sliBRANDS = new SelectList(brands, "MFA_ID", "MFA_BRAND", bandId);
            base.ViewBag.sliFUELS = new SelectList(await db.GetFUELSAsync(langId.Value), "DES_ID", "TEX_TEXT", fluelId);
            base.ViewBag.LangId = langId.Value;
            base.ViewBag.CountryId = countryId.Value;
            base.ViewBag.BandId = bandId.Value;
            base.ViewBag.FluelId = fluelId.Value;
            return View(await db.GetMODELSAsync(langId.Value, countryId.Value, bandId.Value));
        }

        public async Task<ActionResult> ModelType(int? modelId = null, int? countryId = null, int? langId = null, int? bandId = null, int? fluelId = null)
        {
            countryId = (countryId ?? 249);
            langId = (langId ?? 16);
            if (!bandId.HasValue)
            {
                bandId = 0;
            }
            modelId = (modelId ?? 0);
            List<MODEL_TD> carmodels = await db.GetMODELSAsync(langId.Value, countryId.Value, bandId.Value, modelId);
            if (carmodels.Count <= 0)
            {
                ViewBag.ModelId = 0;
                ViewBag.ModelDescription = null;
            }
            else
            {
                ViewBag.ModelId = carmodels[0].MOD_ID;
                ViewBag.ModelDescription = carmodels[0].TEX_TEXT;
            }
            ViewBag.LangId = langId.Value;
            ViewBag.CountryId = countryId.Value;
            ViewBag.BandId = bandId.Value;
            ViewBag.FluelId = fluelId;
            ViewBag.sliFUELS = new SelectList(await db.GetFUELSAsync(langId.Value), "DES_ID", "TEX_TEXT", fluelId);
            List<MODELTYPE_TD> carmodeltypes = null;
            if (ViewBag.ModelId != null)
            {
                carmodeltypes = await db.GetMODELTYPESAsync(langId.Value, countryId.Value, base.ViewBag.ModelId, fluelId);
            }
            return View(carmodeltypes);
        }

        public async Task<ActionResult> ModelTypeTree(int? modelTypeId = null, int? modelId = null, int? countryId = null, int? langId = null, int? bandId = null, int? fluelId = null, int? topicId = null)
        {
            List<MODELTYPESTREE_TD> modeltypetree = null;
            countryId = (countryId ?? 249);
            langId = (langId ?? 16);
            bandId = (bandId ?? 0);
            fluelId = (fluelId ?? 71334);
            modelId = (modelId ?? 0);
            topicId = (topicId ?? 0);
            base.ViewBag.LangId = langId.Value;
            base.ViewBag.CountryId = countryId.Value;
            base.ViewBag.BandId = bandId.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.ModelId = modelId.Value;
            if (!modelTypeId.HasValue)
            {
                base.ViewBag.ModelTypeId = 0;
            }
            else
            {
                base.ViewBag.ModelTypeId = modelTypeId.Value;
                List<SIMPLEMODELTYPES_TD> carmodeltypes = await db.GetSIMPLEMODELTYPESAsync(langId.Value, countryId.Value, modelTypeId.Value);
                if (carmodeltypes.Count > 0)
                {
                    base.ViewBag.ModelId = carmodeltypes[0].TYP_MOD_ID;
                    base.ViewBag.ModelDescription = carmodeltypes[0].TEX_TEXT;
                    base.ViewBag.FluelId = carmodeltypes[0].TYP_KV_FUEL;
                    modeltypetree = await db.GetMODELTYPESTREEAsync(langId.Value, modelTypeId.Value, topicId.Value, 7);
                }
            }
            return View(modeltypetree);
        }

        public async Task<ActionResult> ModelTypeTreeItems(int? topicId, int? modelTypeId = null, int? modelId = null, int? countryId = null, int? langId = null, int? bandId = null, int? fluelId = null, int? tof_assemblyId = null, int? tof_suppliersId = null)
        {
            List<MODELTYPETREEITEMS_REST_TD> modeltypetreeitems = null;
            countryId = (countryId ?? 249);
            langId = (langId ?? 16);
            bandId = (bandId ?? 0);
            fluelId = (fluelId ?? 71334);
            modelId = (modelId ?? 0);
            modelTypeId = (modelTypeId ?? 0);
            base.ViewBag.LangId = langId.Value;
            base.ViewBag.CountryId = countryId.Value;
            base.ViewBag.BandId = bandId.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.ModelId = modelId.Value;
            tof_assemblyId = (tof_assemblyId ?? 0);
            tof_suppliersId = (tof_suppliersId ?? 0);
            base.ViewBag.Tof_assemblyId = tof_assemblyId.Value;
            base.ViewBag.Tof_suppliersId = tof_suppliersId.Value;
            if (!modelTypeId.HasValue || !topicId.HasValue)
            {
                base.ViewBag.ModelTypeId = 0;
                base.ViewBag.TopicId = 0;
            }
            else
            {
                base.ViewBag.ModelTypeId = modelTypeId.Value;
                List<SIMPLEMODELTYPES_TD> carmodeltypes = await db.GetSIMPLEMODELTYPESAsync(langId.Value, countryId.Value, modelTypeId.Value);
                if (carmodeltypes.Count > 0)
                {
                    base.ViewBag.ModelId = carmodeltypes[0].TYP_MOD_ID;
                    base.ViewBag.ModelDescription = carmodeltypes[0].TEX_TEXT;
                    base.ViewBag.FluelId = carmodeltypes[0].TYP_KV_FUEL;
                    base.ViewBag.TopicId = topicId.Value;
                    base.ViewBag.ModelTypeId = modelTypeId.Value;
                    List<MODELTYPETREEITEMS_REST_TD> list = (from e in await db.GetMODELTYPETREEITEMSAsync(langId.Value, countryId.Value, modelTypeId.Value, topicId.Value, tof_assemblyId.Value, tof_suppliersId.Value)
                                                             orderby e.GA_NR, e.SUP_ID
                                                             select e).ToList();
                    modeltypetreeitems = list;
                    List<DICT_TD> tof_assembly = (from e in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD e) => new DICT_TD
                    {
                        DictId = e.GA_NR,
                        DictTitle = e.GA_TEXT + ":" + e.GA_NR.ToString()
                    }).Distinct()
                                                  orderby e.DictTitle
                                                  select e).ToList();
                    tof_assembly.Insert(0, new DICT_TD
                    {
                        DictId = 0,
                        DictTitle = Resources.SHOWALL
                    });
                    base.ViewBag.sliTof_assembly = new SelectList(tof_assembly, "DictId", "DictTitle", tof_assemblyId.Value);
                    List<DICT_TD> tof_suppliers = (from e in modeltypetreeitems.Select((MODELTYPETREEITEMS_REST_TD e) => new DICT_TD
                    {
                        DictId = e.SUP_ID,
                        DictTitle = e.SUP_TEXT + ":" + e.SUP_ID.ToString()
                    }).Distinct()
                                                   orderby e.DictTitle
                                                   select e).ToList();
                    tof_suppliers.Insert(0, new DICT_TD
                    {
                        DictId = 0,
                        DictTitle = Resources.SHOWALL
                    });
                    base.ViewBag.sliTof_suppliers = new SelectList(tof_suppliers, "DictId", "DictTitle", tof_suppliersId.Value);
                }
            }
            if (base.ViewBag.sliTof_suppliers == null)
            {
                base.ViewBag.sliTof_suppliers = new SelectList(new List<DICT_TD>
            {
                new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                }
            }, "DictId", "DictTitle", tof_suppliersId.Value);
            }
            if (base.ViewBag.sliTof_assembly == null)
            {
                base.ViewBag.sliTof_assembly = new SelectList(new List<DICT_TD>
            {
                new DICT_TD
                {
                    DictId = 0,
                    DictTitle = Resources.SHOWALL
                }
            }, "DictId", "DictTitle", tof_assemblyId.Value);
            }
            return View(modeltypetreeitems);
        }

        public async Task<ActionResult> ModelTypeTreeItemDetails(int? laId = null, int? artId = null, int? topicId = null, int? modelTypeId = null, int? modelId = null, int? countryId = null, int? langId = null, int? bandId = null, int? fluelId = null, int? tof_assemblyId = null, int? tof_suppliersId = null)
        {
            List<MODELTYPETREEITEMDESCR_TD> modeltypetreeitemdescr = null;
            tof_assemblyId = (tof_assemblyId ?? 0);
            base.ViewBag.Tof_assemblyId = tof_assemblyId.Value;
            tof_suppliersId = (tof_suppliersId ?? 0);
            base.ViewBag.Tof_suppliersId = tof_suppliersId.Value;
            countryId = (countryId ?? 249);
            langId = (langId ?? 16);
            bandId = (bandId ?? 0);
            fluelId = (fluelId ?? 71334);
            modelId = (modelId ?? 0);
            modelTypeId = (modelTypeId ?? 0);
            base.ViewBag.LangId = langId.Value;
            base.ViewBag.CountryId = countryId.Value;
            base.ViewBag.BandId = bandId.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.ModelId = modelId.Value;
            artId = (artId ?? 0);
            laId = (laId ?? 0);
            base.ViewBag.ArtId = artId.Value;
            base.ViewBag.LaId = laId.Value;
            if (!modelTypeId.HasValue || !topicId.HasValue)
            {
                base.ViewBag.ModelTypeId = 0;
                base.ViewBag.TopicId = 0;
            }
            else
            {
                base.ViewBag.ModelTypeId = modelTypeId.Value;
                base.ViewBag.TopicId = topicId.Value;
                List<SIMPLEMODELTYPES_TD> carmodeltypes = await db.GetSIMPLEMODELTYPESAsync(langId.Value, countryId.Value, modelTypeId.Value);
                if (carmodeltypes.Count > 0)
                {
                    base.ViewBag.ModelId = carmodeltypes[0].TYP_MOD_ID;
                    base.ViewBag.ModelDescription = carmodeltypes[0].TEX_TEXT;
                    base.ViewBag.FluelId = carmodeltypes[0].TYP_KV_FUEL;
                    modeltypetreeitemdescr = await db.GetMODELTYPETREEITEMDESCRAsync(langId.Value, countryId.Value, artId.Value);
                    List<MODELTYPETREEITEMMANID_TD> manufIds = await db.GetMODELTYPETREEITEMMANIDAsync(langId.Value, countryId.Value, artId.Value, bandId);
                    base.ViewBag.ManufIds = manufIds;
                    List<FUEL_TD> eans = db.GetArticleEan(artId.Value);
                    base.ViewBag.Eans = eans;
                }
            }
            return View(modeltypetreeitemdescr);
        }

        public async Task<ActionResult> ModelTypeTreeItemAnalogs(string artNr, int? gaNr = null, int? laId = null, int? artId = null, int? topicId = null, int? modelTypeId = null, int? modelId = null, int? countryId = null, int? langId = null, int? bandId = null, int? fluelId = null, int? tof_assemblyId = null, int? tof_suppliersId = null)
        {
            List<ANALOGOUS_REST_TD> modeltypetreeitemdescr = null;
            tof_assemblyId = (tof_assemblyId ?? 0);
            base.ViewBag.Tof_assemblyId = tof_assemblyId.Value;
            tof_suppliersId = (tof_suppliersId ?? 0);
            base.ViewBag.Tof_suppliersId = tof_suppliersId.Value;
            countryId = (countryId ?? 249);
            langId = (langId ?? 16);
            bandId = (bandId ?? 0);
            fluelId = (fluelId ?? 71334);
            modelId = (modelId ?? 0);
            modelTypeId = (modelTypeId ?? 0);
            base.ViewBag.LangId = langId.Value;
            base.ViewBag.CountryId = countryId.Value;
            base.ViewBag.BandId = bandId.Value;
            base.ViewBag.FluelId = fluelId.Value;
            base.ViewBag.ModelId = modelId.Value;
            artId = (artId ?? 0);
            laId = (laId ?? 0);
            base.ViewBag.ArtId = artId.Value;
            base.ViewBag.LaId = laId.Value;
            if (!modelTypeId.HasValue || !topicId.HasValue)
            {
                base.ViewBag.ModelTypeId = 0;
                base.ViewBag.TopicId = 0;
            }
            else
            {
                base.ViewBag.ModelTypeId = modelTypeId.Value;
                base.ViewBag.TopicId = topicId.Value;
                List<SIMPLEMODELTYPES_TD> carmodeltypes = await db.GetSIMPLEMODELTYPESAsync(langId.Value, countryId.Value, modelTypeId.Value);
                if (carmodeltypes.Count > 0)
                {
                    base.ViewBag.ModelId = carmodeltypes[0].TYP_MOD_ID;
                    base.ViewBag.ModelDescription = carmodeltypes[0].TEX_TEXT;
                    base.ViewBag.FluelId = carmodeltypes[0].TYP_KV_FUEL;
                    modeltypetreeitemdescr = await db.GetANALOGSAsync(langId.Value, countryId.Value, artNr, gaNr);
                }
            }
            return View(modeltypetreeitemdescr);
        }

        public async Task<ActionResult> ArticleByID(int? countryId = null, int? langId = null, string artId = null, int? srchTp = null)
        {
            countryId = (countryId ?? 249);
            langId = (langId ?? 16);
            srchTp = (srchTp ?? 0);
            base.ViewBag.sliLANGUAGES = new SelectList(await db.GetLANGUAGESAsync(langId.Value), "LNG_ID", "TEX_TEXT", langId.Value);
            base.ViewBag.sliCOUNTRIES = new SelectList(await db.GetCOUNTRIESAsync(langId.Value), "COU_ID", "TEX_TEXT", countryId.Value);
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
            List<MODELTYPETREEITEMS_TD> modeltypetreeitems = (!base.ModelState.IsValid) ? new List<MODELTYPETREEITEMS_TD>() : (await db.GetArticleByIDAsync(langId.Value, countryId.Value, artId, srchTp.Value));
            base.ViewBag.countryId = countryId.Value;
            base.ViewBag.langId = langId.Value;
            base.ViewBag.srchTp = srchTp.Value;
            base.ViewBag.artId = artId;
            List<MODELTYPETREEITEMDESCR_TD> modeltypetreeitemdescr;
            List<MODELTYPETREEITEMMANID_TD> manufIds;
            List<ANALOGOUS_REST_TD> analoglist;
            List<MODELTYPE_TD> automobiles;
            List<FUEL_TD> eans;
            if (modeltypetreeitems.Count > 0)
            {
                modeltypetreeitemdescr = await db.GetMODELTYPETREEITEMDESCRAsync(langId.Value, countryId.Value, modeltypetreeitems[0].ART_ID);
                manufIds = await db.GetMODELTYPETREEITEMMANIDAsync(langId.Value, countryId.Value, modeltypetreeitems[0].ART_ID);
                analoglist = await db.GetANALOGSAsync(langId.Value, countryId.Value, modeltypetreeitems[0].ART_ARTICLE_NR, modeltypetreeitems[0].GA_NR);
                automobiles = db.GetArticleApplic(langId.Value, countryId.Value, modeltypetreeitems[0].ART_ID);
                eans = await db.GetArticleEanAsync(modeltypetreeitems[0].ART_ID);
            }
            else
            {
                modeltypetreeitemdescr = new List<MODELTYPETREEITEMDESCR_TD>();
                manufIds = new List<MODELTYPETREEITEMMANID_TD>();
                analoglist = new List<ANALOGOUS_REST_TD>();
                automobiles = new List<MODELTYPE_TD>();
                eans = new List<FUEL_TD>();
            }
            base.ViewBag.ManufIds = manufIds;
            base.ViewBag.Modeltypetreeitemdescr = modeltypetreeitemdescr;
            base.ViewBag.Analoglist = analoglist;
            base.ViewBag.Modeltypetreeitems = modeltypetreeitems;
            base.ViewBag.Eans = eans;
            return View(automobiles);
        }

        public async Task<ActionResult> GetImage(int? GRD_FLD, int? GRD_ID)
        {
            return File(await db.GetPhotoAsync(GRD_FLD.Value, GRD_ID.Value), "image/tiff");
        }

        public async Task<ActionResult> LookUpArticleByID(string redirecData, string redirectContriller, string redirectAction, int? countryId = null, int? langId = null, string artId = null, int? srchTp = null)
        {
            base.ViewBag.RedirecData = redirecData;
            base.ViewBag.RedirectContriller = redirectContriller;
            base.ViewBag.RedirectAction = redirectAction;
            Guid? searchEntBranchGuid = null;
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    EnterpriseUserTDES aUser = await (from ee in dbCarShop.EnterpriseUserTDES.Include("EnterpriseTDES")
                                                      where ee.EntUserNic == aName
                                                      select ee).FirstOrDefaultAsync();
                    if (aUser == null)
                    {
                        base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                        return View();
                    }
                    TecDocSrcType = aUser.EnterpriseTDES.TecDocSrcTypeId;
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
                    EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                                       where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                       select ee).FirstOrDefaultAsync();
                    if (enterprisebranchtdes == null)
                    {
                        base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                        return View();
                    }
                    TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
                }
            }
            countryId = (countryId ?? 249);
            langId = (langId ?? 16);
            srchTp = (srchTp ?? 0);
            base.ViewBag.sliLANGUAGES = new SelectList(await db.GetLANGUAGESAsync(langId.Value), "LNG_ID", "TEX_TEXT", langId.Value);
            base.ViewBag.sliCOUNTRIES = new SelectList(await db.GetCOUNTRIESAsync(langId.Value), "COU_ID", "TEX_TEXT", countryId.Value);
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
            List<MODELTYPETREEITEMS_TD> modeltypetreeitems = (!base.ModelState.IsValid) ? new List<MODELTYPETREEITEMS_TD>() : (await db.GetArticleByIDAsync(langId.Value, countryId.Value, artId, srchTp.Value));
            base.ViewBag.countryId = countryId.Value;
            base.ViewBag.langId = langId.Value;
            base.ViewBag.srchTp = srchTp.Value;
            base.ViewBag.artId = artId;
            List<MODELTYPETREEITEMDESCR_TD> modeltypetreeitemdescr;
            List<MODELTYPETREEITEMMANID_TD> manufIds;
            List<ANALOGOUS_REST_TD> analoglist;
            List<FUEL_TD> eans;
            if (modeltypetreeitems.Count > 0)
            {
                modeltypetreeitemdescr = await db.GetMODELTYPETREEITEMDESCRAsync(langId.Value, countryId.Value, modeltypetreeitems[0].ART_ID);
                manufIds = await db.GetMODELTYPETREEITEMMANIDAsync(langId.Value, countryId.Value, modeltypetreeitems[0].ART_ID);
                analoglist = await db.GetANALOGSAsync(langId.Value, countryId.Value, modeltypetreeitems[0].ART_ARTICLE_NR, modeltypetreeitems[0].GA_NR);
                eans = await db.GetArticleEanAsync(modeltypetreeitems[0].ART_ID);
            }
            else
            {
                modeltypetreeitemdescr = new List<MODELTYPETREEITEMDESCR_TD>();
                manufIds = new List<MODELTYPETREEITEMMANID_TD>();
                analoglist = new List<ANALOGOUS_REST_TD>();
                eans = new List<FUEL_TD>();
            }
            base.ViewBag.ManufIds = manufIds;
            base.ViewBag.Modeltypetreeitemdescr = modeltypetreeitemdescr;
            base.ViewBag.Analoglist = analoglist;
            base.ViewBag.Modeltypetreeitems = modeltypetreeitems;
            base.ViewBag.Eans = eans;
            return View(modeltypetreeitems);
        }

        [HttpPost]
        public async Task<ActionResult> LookUpArticleByIDSelected(string redirecData, string redirectContriller, string redirectAction, string SimpleArticle, string DOSELECT_TITLE, string DOCANCEL_TITLE)
        {
            if (DOSELECT_TITLE != null)
            {
                Guid? searchEntBranchGuid = null;
                UserIsInRoles();
                if ((!base.ViewBag.IsEcoSystemAdmin))
                {
                    string aName = base.User.Identity.Name;
                    if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                    {
                        EnterpriseUserTDES aUser = await (from ee in dbCarShop.EnterpriseUserTDES.Include("EnterpriseTDES")
                                                          where ee.EntUserNic == aName
                                                          select ee).FirstOrDefaultAsync();
                        if (aUser == null)
                        {
                            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                            return View();
                        }
                        TecDocSrcType = aUser.EnterpriseTDES.TecDocSrcTypeId;
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
                        EnterpriseBranchTDES enterprisebranchtdes = await (from ee in dbCarShop.EnterpriseBranchTDES.Include("EnterpriseTDES")
                                                                           where (Guid)(Guid?)ee.EntBranchGuid == (Guid)searchEntBranchGuid
                                                                           select ee).FirstOrDefaultAsync();
                        if (enterprisebranchtdes == null)
                        {
                            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
                            return View();
                        }
                        TecDocSrcType = enterprisebranchtdes.EnterpriseTDES.TecDocSrcTypeId;
                    }
                }
                string ART_ARTICLE_NR = null;
                string SUP_TEXT = null;
                string MASTER_BEZ = null;
                string EAN = null;
                if (SimpleArticle != null)
                {
                    SimpleArticle_TD sa = JsonConvert.DeserializeObject<SimpleArticle_TD>(SimpleArticle);
                    ART_ARTICLE_NR = sa.ART_ARTICLE_NR;
                    SUP_TEXT = sa.SUP_TEXT;
                    MASTER_BEZ = sa.MASTER_BEZ;
                    List<FUEL_TD> eans = await db.GetArticleEanAsync(sa.ART_ID);
                    if (eans != null && eans.Count > 0)
                    {
                        EAN = eans[0].TEX_TEXT;
                    }
                }
                return RedirectToAction(redirectAction, redirectContriller, new
                {
                    redirecData,
                    ART_ARTICLE_NR,
                    SUP_TEXT,
                    MASTER_BEZ,
                    EAN
                });
            }
            return RedirectToAction(redirectAction, redirectContriller, new
            {
                redirecData
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
            dbCarShop.Dispose();
            base.Dispose(disposing);
        }
    }

}