// CarShop.Controllers.EnterpriseAddressController
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

    public class EnterpriseAddressController : Controller
    {
        private CarShopContext db = new CarShopContext();

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
        }

        protected void ViewBagHelper(Guid? searchEntGuid)
        {
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

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin,EnterpriseAudit,BranchAdmin,BranchAudit,BranchSeller,BranchBooker")]
        public ActionResult Index(Guid? searchEntGuid, int? showIsActive, int? showIsVisible, int? showAddressTypeId)
        {
            UserIsInRoles();
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
                    searchEntGuid = (from e in db.EnterpriseBranchUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
            }
            ViewBagHelper(searchEntGuid);
            IQueryable<EnterpriseAddressTDES> source = from e in db.EnterpriseAddressTDES
                                                       where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
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
            UserIsInRoles();
            EnterpriseAddressTDES enterpriseAddressTDES = null;
            Guid? searchEntGuid = null;
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
                    searchEntGuid = (from e in db.EnterpriseBranchUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseAddressTDES = (from e in db.EnterpriseAddressTDES
                                         where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                         select e).FirstOrDefault();
            }
            else
            {
                enterpriseAddressTDES = db.EnterpriseAddressTDES.Find(id);
            }
            if (enterpriseAddressTDES == null)
            {
                return HttpNotFound();
            }
            ViewBagHelper(enterpriseAddressTDES.EntGuid);
            return View(enterpriseAddressTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create(Guid? searchEntGuid = default(Guid?))
        {
            UserIsInRoles();
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
            }
            EnterpriseAddressTDES enterpriseAddressTDES = null;
            ViewBagHelper(searchEntGuid);
            if (base.ViewBag.SearchEntGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription");
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName");
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription");
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription");
                if (enterpriseAddressTDES == null)
                {
                    EnterpriseAddressTDES enterpriseAddressTDES2 = new EnterpriseAddressTDES();
                    enterpriseAddressTDES2.AddressGuid = Guid.NewGuid();
                    enterpriseAddressTDES2.EntGuid = searchEntGuid.Value;
                    enterpriseAddressTDES2.AddressValidFrom = DateTime.Now;
                    enterpriseAddressTDES2.AddressValidTo = DateTime.Now;
                    enterpriseAddressTDES = enterpriseAddressTDES2;
                }
            }
            return View(enterpriseAddressTDES);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [HttpPost]
        public ActionResult Create(EnterpriseAddressTDES enterpriseaddresstdes, string SettlementNameLookUp, string SoatoLookUp)
        {
            if (SettlementNameLookUp != null || SoatoLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(enterpriseaddresstdes);
                string text = "";
                string text2 = "";
                if (SettlementNameLookUp != null)
                {
                    text = enterpriseaddresstdes.AddressSettlementName;
                    text2 = "2";
                }
                else
                {
                    text = enterpriseaddresstdes.SoatoId;
                    text2 = "1";
                }
                string redirectContriller = "EnterpriseAddress";
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
            Guid? guid = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                guid = enterpriseaddresstdes.EntGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    guid = (from e in db.EnterpriseUserTDES
                            where e.EntUserNic == aName
                            select e.EntGuid).FirstOrDefault();
                }
            }
            if (base.ModelState.IsValid && enterpriseaddresstdes.EntGuid != guid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseAddressTDES.Add(enterpriseaddresstdes);
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterpriseaddresstdes.EntGuid
                });
            }
            ViewBagHelper(enterpriseaddresstdes.EntGuid);
            if (base.ViewBag.SearchEntGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterpriseaddresstdes.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterpriseaddresstdes.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterpriseaddresstdes.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterpriseaddresstdes.StreetTypeId);
            }
            return View(enterpriseaddresstdes);
        }

        protected ActionResult GetSoatoForCreateUpdate(string redirecData, string soatoId, string actionName)
        {
            EnterpriseAddressTDES enterpriseAddressTDES = JsonConvert.DeserializeObject<EnterpriseAddressTDES>(redirecData);
            if (soatoId != null && soatoId.Length == 10)
            {
                Soato soato = db.Soato.Find(soatoId);
                enterpriseAddressTDES.SoatoId = soato.SoatoId;
                enterpriseAddressTDES.SettlementTypeId = soato.SettlementTypeId;
                enterpriseAddressTDES.AddressSettlementName = soato.SoatoSettlementName;
                if (!soatoId.EndsWith("000"))
                {
                    soatoId = soatoId.Substring(0, 7) + "000";
                    soato = db.Soato.Find(soatoId);
                    enterpriseAddressTDES.AddressRural = soato.SoatoSettlementName;
                }
                else
                {
                    enterpriseAddressTDES.AddressRural = "";
                }
                if (!soatoId.EndsWith("000000"))
                {
                    soatoId = soatoId.Substring(0, 4) + "000000";
                    soato = db.Soato.Find(soatoId);
                    enterpriseAddressTDES.AddressDistrict = soato.SoatoSettlementName;
                }
                else
                {
                    enterpriseAddressTDES.AddressDistrict = "";
                }
                if (!soatoId.EndsWith("000000000"))
                {
                    soatoId = soatoId.Substring(0, 1) + "000000000";
                    soato = db.Soato.Find(soatoId);
                    enterpriseAddressTDES.AddressRegion = soato.SoatoSettlementName;
                }
                else
                {
                    enterpriseAddressTDES.AddressRegion = "";
                }
            }
            ViewBagHelper(enterpriseAddressTDES.EntGuid);
            if (base.ViewBag.SearchEntGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterpriseAddressTDES.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterpriseAddressTDES.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterpriseAddressTDES.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterpriseAddressTDES.StreetTypeId);
            }
            return View(actionName, enterpriseAddressTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult GetSoatoForCreate(string redirecData, string soatoId)
        {
            return GetSoatoForCreateUpdate(redirecData, soatoId, "Create");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult GetSoatoForUpdate(string redirecData, string soatoId)
        {
            return GetSoatoForCreateUpdate(redirecData, soatoId, "Edit");
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Edit(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            EnterpriseAddressTDES enterpriseAddressTDES = null;
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseAddressTDES = (from e in db.EnterpriseAddressTDES
                                         where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                         select e).FirstOrDefault();
            }
            else
            {
                enterpriseAddressTDES = db.EnterpriseAddressTDES.Find(id);
            }
            if (enterpriseAddressTDES == null)
            {
                return HttpNotFound();
            }
            ViewBagHelper(enterpriseAddressTDES.EntGuid);
            if (base.ViewBag.SearchEntGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterpriseAddressTDES.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterpriseAddressTDES.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterpriseAddressTDES.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterpriseAddressTDES.StreetTypeId);
            }
            return View(enterpriseAddressTDES);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EnterpriseAddressTDES enterpriseaddresstdes, string SettlementNameLookUp, string SoatoLookUp)
        {
            if (SettlementNameLookUp != null || SoatoLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(enterpriseaddresstdes);
                string text = "";
                string text2 = "";
                if (SettlementNameLookUp != null)
                {
                    text = enterpriseaddresstdes.AddressSettlementName;
                    text2 = "2";
                }
                else
                {
                    text = enterpriseaddresstdes.SoatoId;
                    text2 = "1";
                }
                string redirectContriller = "EnterpriseAddress";
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
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                guid = enterpriseaddresstdes.EntGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    guid = (from e in db.EnterpriseUserTDES
                            where e.EntUserNic == aName
                            select e.EntGuid).FirstOrDefault();
                }
            }
            if (base.ModelState.IsValid && enterpriseaddresstdes.EntGuid != guid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterpriseaddresstdes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterpriseaddresstdes.EntGuid
                });
            }
            ViewBagHelper(enterpriseaddresstdes.EntGuid);
            if (base.ViewBag.SearchEntGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterpriseaddresstdes.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterpriseaddresstdes.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterpriseaddresstdes.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterpriseaddresstdes.StreetTypeId);
            }
            return View(enterpriseaddresstdes);
        }

        public ActionResult Delete(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            EnterpriseAddressTDES enterpriseAddressTDES = null;
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseAddressTDES = (from e in db.EnterpriseAddressTDES
                                         where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                         select e).FirstOrDefault();
            }
            else
            {
                enterpriseAddressTDES = db.EnterpriseAddressTDES.Find(id);
            }
            if (enterpriseAddressTDES == null)
            {
                return HttpNotFound();
            }
            ViewBagHelper(enterpriseAddressTDES.EntGuid);
            return View(enterpriseAddressTDES);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid? id = default(Guid?))
        {
            UserIsInRoles();
            EnterpriseAddressTDES enterpriseAddressTDES = null;
            Guid? searchEntGuid = null;
            if ((!base.ViewBag.IsEcoSystemAdmin))
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
                enterpriseAddressTDES = (from e in db.EnterpriseAddressTDES
                                         where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                         select e).FirstOrDefault();
            }
            else
            {
                enterpriseAddressTDES = db.EnterpriseAddressTDES.Find(id);
                searchEntGuid = enterpriseAddressTDES.EntGuid;
            }
            db.EnterpriseAddressTDES.Remove(enterpriseAddressTDES);
            db.SaveChanges();
            return RedirectToAction("Index", new
            {
                searchEntGuid
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}