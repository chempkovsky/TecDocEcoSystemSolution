// CarShop.Controllers.EnterpriseBranchAddressController
using CarShop.Models;
using CarShop.Properties;
using CarShop.Utility;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Controllers
{

    public class EnterpriseBranchAddressController : Controller
    {
        private CarShopContext db = new CarShopContext();

        public void ViewBagHelper(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchTDES enterpriseBranchTDES = (from e in db.EnterpriseBranchTDES
                                                         where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                         select e).Include((EnterpriseBranchTDES x) => x.EnterpriseTDES).FirstOrDefault();
            if (enterpriseBranchTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseBranchTDES.EnterpriseTDES.EntDescription;
                base.ViewBag.EntBranchDescription = enterpriseBranchTDES.EntBranchDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
                base.ViewBag.SearchEntBranchGuid = searchEntBranchGuid;
                return;
            }
            base.ViewBag.EntBranchDescription = Resources.EnterpriseBranchTDES_NOTFOUND;
            base.ViewBag.SearchEntBranchGuid = null;
            base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            EnterpriseTDES enterpriseTDES = (from e in db.EnterpriseTDES
                                             where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                             select e).FirstOrDefault();
            if (enterpriseTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseTDES.EntDescription;
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
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Index(Guid? searchEntGuid, Guid? searchEntBranchGuid, int? showIsActive, int? showIsVisible, int? showAddressTypeId)
        {
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                }
            }
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            IQueryable<EnterpriseBranchAddressTDES> source = from e in db.EnterpriseBranchAddressTDES
                                                             where (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                             select e;
            showIsActive = (showIsActive ?? 2);
            switch (showIsActive)
            {
                case 2:
                    source = from e in source
                             where e.IsActive == true
                             select e;
                    break;
                case 3:
                    source = from e in source
                             where e.IsActive == false
                             select e;
                    break;
                default:
                    showIsActive = 1;
                    break;
            }
            showIsVisible = (showIsVisible ?? 1);
            switch (showIsVisible)
            {
                case 2:
                    source = from e in source
                             where e.IsVisible == true
                             select e;
                    break;
                case 3:
                    source = from e in source
                             where e.IsVisible == false
                             select e;
                    break;
                default:
                    showIsVisible = 1;
                    break;
            }
            showAddressTypeId = (showAddressTypeId ?? (-1));
            if (showAddressTypeId > -1)
            {
                source = from e in source
                         where (int?)e.AddressTypeId == showAddressTypeId
                         select e;
            }
            base.ViewBag.sliIsActive = SwitcherListUtil.SelectListHelper(showIsActive.Value);
            base.ViewBag.sliIsVisible = SwitcherListUtil.SelectListHelper(showIsVisible.Value);
            var items = (from e in new[]
            {
            new
            {
                AddressTypeId = -1,
                AddressTypeDescription = Resources.SHOWALL
            }
        }
                         select new
                         {
                             e.AddressTypeId,
                             e.AddressTypeDescription
                         }).Union(from e in db.AddressType
                                  select new
                                  {
                                      e.AddressTypeId,
                                      e.AddressTypeDescription
                                  });
            base.ViewBag.sliAddressType = new SelectList(items, "AddressTypeId", "AddressTypeDescription", showAddressTypeId.Value);
            return View(source.ToList());
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Details(Guid? id = default(Guid?))
        {
            EnterpriseBranchAddressTDES enterpriseBranchAddressTDES = null;
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    enterpriseBranchAddressTDES = (from e in db.EnterpriseBranchAddressTDES
                                                   where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                                   select e).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                        enterpriseBranchAddressTDES = (from e in db.EnterpriseBranchAddressTDES
                                                       where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                                       select e).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchAddressTDES = db.EnterpriseBranchAddressTDES.Find(id);
            }
            if (enterpriseBranchAddressTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseBranchAddressTDES.EntGuid;
            searchEntBranchGuid = enterpriseBranchAddressTDES.EntBranchGuid;
            ViewBagHelper(searchEntGuid, searchEntBranchGuid);
            return View(enterpriseBranchAddressTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult Create(Guid? searchEntGuid, Guid? searchEntBranchGuid)
        {
            EnterpriseBranchAddressTDES model = null;
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
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
            if (base.ViewBag.SearchEntBranchGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription");
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName");
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription");
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription");
                EnterpriseBranchAddressTDES enterpriseBranchAddressTDES = new EnterpriseBranchAddressTDES();
                enterpriseBranchAddressTDES.AddressGuid = Guid.NewGuid();
                enterpriseBranchAddressTDES.EntBranchGuid = searchEntBranchGuid.Value;
                enterpriseBranchAddressTDES.EntGuid = searchEntGuid.Value;
                enterpriseBranchAddressTDES.AddressValidFrom = DateTime.Now;
                enterpriseBranchAddressTDES.AddressValidTo = DateTime.Now;
                model = enterpriseBranchAddressTDES;
            }
            return View(model);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(EnterpriseBranchAddressTDES enterprisebranchaddresstdes, string SettlementNameLookUp, string SoatoLookUp)
        {
            if (SettlementNameLookUp != null || SoatoLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(enterprisebranchaddresstdes);
                string text = "";
                string text2 = "";
                if (SettlementNameLookUp != null)
                {
                    text = enterprisebranchaddresstdes.AddressSettlementName;
                    text2 = "2";
                }
                else
                {
                    text = enterprisebranchaddresstdes.SoatoId;
                    text2 = "1";
                }
                string redirectContriller = "EnterpriseBranchAddress";
                string redirectAction = "GetSoatoForCreate";
                return RedirectToAction("LookUpForEnterpriseAddress", "Soato", new
                {
                    redirecData = redirecData,
                    redirectContriller = redirectContriller,
                    redirectAction = redirectAction,
                    searchString = text,
                    searchStringBy = text2
                });
            }
            UserIsInRoles();
            Guid? a = null;
            Guid? guid = null;
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                a = enterprisebranchaddresstdes.EntGuid;
                guid = enterprisebranchaddresstdes.EntBranchGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    a = (from e in db.EnterpriseUserTDES
                         where e.EntUserNic == aName
                         select e.EntGuid).FirstOrDefault();
                    guid = enterprisebranchaddresstdes.EntBranchGuid;
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES != null)
                    {
                        a = enterpriseBranchIdsTDES.EntGuid;
                        guid = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        a = null;
                        guid = null;
                    }
                }
            }
            if (base.ModelState.IsValid && (a != enterprisebranchaddresstdes.EntGuid || guid != enterprisebranchaddresstdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseBranchAddressTDES.Add(enterprisebranchaddresstdes);
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterprisebranchaddresstdes.EntGuid,
                    searchEntBranchGuid = enterprisebranchaddresstdes.EntBranchGuid
                });
            }
            ViewBagHelper(enterprisebranchaddresstdes.EntGuid, enterprisebranchaddresstdes.EntBranchGuid);
            if (base.ViewBag.SearchEntBranchGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterprisebranchaddresstdes.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterprisebranchaddresstdes.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterprisebranchaddresstdes.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterprisebranchaddresstdes.StreetTypeId);
            }
            return View(enterprisebranchaddresstdes);
        }

        protected ActionResult GetSoatoForCreateUpdate(string redirecData, string soatoId, string actionName)
        {
            EnterpriseBranchAddressTDES enterpriseBranchAddressTDES = JsonConvert.DeserializeObject<EnterpriseBranchAddressTDES>(redirecData);
            if (soatoId != null && soatoId.Length == 10)
            {
                Soato soato = db.Soato.Find(soatoId);
                enterpriseBranchAddressTDES.SoatoId = soato.SoatoId;
                enterpriseBranchAddressTDES.SettlementTypeId = soato.SettlementTypeId;
                enterpriseBranchAddressTDES.AddressSettlementName = soato.SoatoSettlementName;
                if (!soatoId.EndsWith("000"))
                {
                    soatoId = soatoId.Substring(0, 7) + "000";
                    soato = db.Soato.Find(soatoId);
                    enterpriseBranchAddressTDES.AddressRural = soato.SoatoSettlementName;
                }
                else
                {
                    enterpriseBranchAddressTDES.AddressRural = "";
                }
                if (!soatoId.EndsWith("000000"))
                {
                    soatoId = soatoId.Substring(0, 4) + "000000";
                    soato = db.Soato.Find(soatoId);
                    enterpriseBranchAddressTDES.AddressDistrict = soato.SoatoSettlementName;
                }
                else
                {
                    enterpriseBranchAddressTDES.AddressDistrict = "";
                }
                if (!soatoId.EndsWith("000000000"))
                {
                    soatoId = soatoId.Substring(0, 1) + "000000000";
                    soato = db.Soato.Find(soatoId);
                    enterpriseBranchAddressTDES.AddressRegion = soato.SoatoSettlementName;
                }
                else
                {
                    enterpriseBranchAddressTDES.AddressRegion = "";
                }
            }
            ViewBagHelper(enterpriseBranchAddressTDES.EntGuid, enterpriseBranchAddressTDES.EntBranchGuid);
            if (base.ViewBag.SearchEntBranchGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterpriseBranchAddressTDES.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterpriseBranchAddressTDES.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterpriseBranchAddressTDES.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterpriseBranchAddressTDES.StreetTypeId);
            }
            return View(actionName, enterpriseBranchAddressTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult GetSoatoForCreate(string redirecData, string soatoId)
        {
            return GetSoatoForCreateUpdate(redirecData, soatoId, "Create");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult GetSoatoForUpdate(string redirecData, string soatoId)
        {
            return GetSoatoForCreateUpdate(redirecData, soatoId, "Edit");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult Edit(Guid? id = default(Guid?))
        {
            EnterpriseBranchAddressTDES enterpriseBranchAddressTDES = null;
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    enterpriseBranchAddressTDES = (from e in db.EnterpriseBranchAddressTDES
                                                   where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                                   select e).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                        enterpriseBranchAddressTDES = (from e in db.EnterpriseBranchAddressTDES
                                                       where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                                       select e).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchAddressTDES = db.EnterpriseBranchAddressTDES.Find(id);
            }
            if (enterpriseBranchAddressTDES == null)
            {
                return HttpNotFound();
            }
            ViewBagHelper(enterpriseBranchAddressTDES.EntGuid, enterpriseBranchAddressTDES.EntBranchGuid);
            if (base.ViewBag.SearchEntBranchGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterpriseBranchAddressTDES.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterpriseBranchAddressTDES.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterpriseBranchAddressTDES.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterpriseBranchAddressTDES.StreetTypeId);
            }
            return View(enterpriseBranchAddressTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EnterpriseBranchAddressTDES enterprisebranchaddresstdes, string SettlementNameLookUp, string SoatoLookUp)
        {
            if (SettlementNameLookUp != null || SoatoLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(enterprisebranchaddresstdes);
                string text = "";
                string text2 = "";
                if (SettlementNameLookUp != null)
                {
                    text = enterprisebranchaddresstdes.AddressSettlementName;
                    text2 = "2";
                }
                else
                {
                    text = enterprisebranchaddresstdes.SoatoId;
                    text2 = "1";
                }
                string redirectContriller = "EnterpriseBranchAddress";
                string redirectAction = "GetSoatoForUpdate";
                return RedirectToAction("LookUpForEnterpriseAddress", "Soato", new
                {
                    redirecData = redirecData,
                    redirectContriller = redirectContriller,
                    redirectAction = redirectAction,
                    searchString = text,
                    searchStringBy = text2
                });
            }
            UserIsInRoles();
            Guid? guid = null;
            Guid? guid2 = null;
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                guid = enterprisebranchaddresstdes.EntGuid;
                guid2 = enterprisebranchaddresstdes.EntBranchGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    guid = (from e in db.EnterpriseUserTDES
                            where e.EntUserNic == aName
                            select e.EntGuid).FirstOrDefault();
                    guid2 = enterprisebranchaddresstdes.EntBranchGuid;
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES != null)
                    {
                        guid = enterpriseBranchIdsTDES.EntGuid;
                        guid2 = enterpriseBranchIdsTDES.EntBranchGuid;
                    }
                    else
                    {
                        guid = null;
                        guid2 = null;
                    }
                }
            }
            if (base.ModelState.IsValid && (guid != enterprisebranchaddresstdes.EntGuid || guid2 != enterprisebranchaddresstdes.EntBranchGuid))
            {
                base.ModelState.AddModelError("", Resources.EnterpriseBranchTDES_NOTFOUND);
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisebranchaddresstdes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterprisebranchaddresstdes.EntGuid,
                    searchEntBranchGuid = enterprisebranchaddresstdes.EntBranchGuid
                });
            }
            ViewBagHelper(guid, guid2);
            if (base.ViewBag.SearchEntBranchGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterprisebranchaddresstdes.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterprisebranchaddresstdes.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterprisebranchaddresstdes.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterprisebranchaddresstdes.StreetTypeId);
            }
            return View(enterprisebranchaddresstdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        public ActionResult Delete(Guid? id = default(Guid?))
        {
            EnterpriseBranchAddressTDES enterpriseBranchAddressTDES = db.EnterpriseBranchAddressTDES.Find(id);
            if (enterpriseBranchAddressTDES == null)
            {
                return HttpNotFound();
            }
            ViewBagHelper(enterpriseBranchAddressTDES.EntGuid, enterpriseBranchAddressTDES.EntBranchGuid);
            return View(enterpriseBranchAddressTDES);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,BranchAdmin")]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            EnterpriseBranchAddressTDES enterpriseBranchAddressTDES = null;
            UserIsInRoles();
            EnterpriseBranchIdsTDES enterpriseBranchIdsTDES = null;
            Guid? searchEntGuid = null;
            Guid? searchEntBranchGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin || base.ViewBag.IsEnterpriseAudit)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                    enterpriseBranchAddressTDES = (from e in db.EnterpriseBranchAddressTDES
                                                   where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                                   select e).FirstOrDefault();
                }
                else
                {
                    enterpriseBranchIdsTDES = (from e in db.EnterpriseBranchUserTDES
                                               where e.EntUserNic == aName
                                               select new EnterpriseBranchIdsTDES
                                               {
                                                   EntGuid = e.EntGuid,
                                                   EntBranchGuid = e.EntBranchGuid
                                               }).FirstOrDefault();
                    if (enterpriseBranchIdsTDES == null)
                    {
                        searchEntGuid = null;
                        searchEntBranchGuid = null;
                    }
                    else
                    {
                        searchEntGuid = enterpriseBranchIdsTDES.EntGuid;
                        searchEntBranchGuid = enterpriseBranchIdsTDES.EntBranchGuid;
                        enterpriseBranchAddressTDES = (from e in db.EnterpriseBranchAddressTDES
                                                       where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.EntBranchGuid == (Guid)searchEntBranchGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                                       select e).FirstOrDefault();
                    }
                }
            }
            else
            {
                enterpriseBranchAddressTDES = db.EnterpriseBranchAddressTDES.Find(id);
            }
            if (enterpriseBranchAddressTDES == null)
            {
                return HttpNotFound();
            }
            Guid? searchEntGuid2 = enterpriseBranchAddressTDES.EntGuid;
            Guid? searchEntBranchGuid2 = enterpriseBranchAddressTDES.EntBranchGuid;
            db.EnterpriseBranchAddressTDES.Remove(enterpriseBranchAddressTDES);
            db.SaveChanges();
            return RedirectToAction("Index", new
            {
                searchEntGuid = searchEntGuid2,
                searchEntBranchGuid = searchEntBranchGuid2
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}