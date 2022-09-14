// CarShop.Controllers.EnterpriseSupplierAddressController
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

    public class EnterpriseSupplierAddressController : Controller
    {
        private CarShopContext db = new CarShopContext();

        protected void UserIsInRoles()
        {
            base.ViewBag.IsEcoSystemAdmin = base.User.IsInRole("EcoSystemAdmin");
            base.ViewBag.IsEnterpriseAdmin = base.User.IsInRole("EnterpriseAdmin");
            base.ViewBag.IsEnterpriseAudit = base.User.IsInRole("EnterpriseAudit");
        }

        public void ViewBagHelper(Guid? searchEntGuid, string searchEntSupplierId)
        {
            EnterpriseSupplierTDES enterpriseSupplierTDES = (from e in db.EnterpriseSupplierTDES
                                                             where e.EntSupplierId == searchEntSupplierId && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
                                                             select e).Include((EnterpriseSupplierTDES x) => x.EnterpriseTDES).FirstOrDefault();
            if (enterpriseSupplierTDES != null)
            {
                base.ViewBag.EntDescription = enterpriseSupplierTDES.EnterpriseTDES.EntDescription;
                base.ViewBag.EntSupplierDescription = enterpriseSupplierTDES.EntSupplierDescription;
                base.ViewBag.SearchEntGuid = searchEntGuid;
                base.ViewBag.SearchEntSupplierId = searchEntSupplierId;
                return;
            }
            base.ViewBag.EntSupplierDescription = Resources.EnterpriseSupplierTDES_NOTFOUND;
            base.ViewBag.SearchEntSupplierId = null;
            base.ModelState.AddModelError("", Resources.EnterpriseSupplierTDES_NOTFOUND);
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
        public ActionResult Index(Guid? searchEntGuid, string searchEntSupplierId, int? showIsActive, int? showIsVisible, int? showAddressTypeId)
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
            ViewBagHelper(searchEntGuid, searchEntSupplierId);
            IQueryable<EnterpriseSupplierAddressTDES> source = from e in db.EnterpriseSupplierAddressTDES
                                                               where e.EntSupplierId == searchEntSupplierId && (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid
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
            EnterpriseSupplierAddressTDES enterpriseSupplierAddressTDES = null;
            UserIsInRoles();
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
                enterpriseSupplierAddressTDES = (from e in db.EnterpriseSupplierAddressTDES
                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                                 select e).FirstOrDefault();
            }
            else
            {
                enterpriseSupplierAddressTDES = db.EnterpriseSupplierAddressTDES.Find(id);
            }
            if (enterpriseSupplierAddressTDES == null)
            {
                return HttpNotFound();
            }
            searchEntGuid = enterpriseSupplierAddressTDES.EntGuid;
            string entSupplierId = enterpriseSupplierAddressTDES.EntSupplierId;
            ViewBagHelper(searchEntGuid, entSupplierId);
            return View(enterpriseSupplierAddressTDES);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create(Guid? searchEntGuid, string searchEntSupplierId)
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
                    searchEntSupplierId = (from e in db.EnterpriseSupplierTDES
                                           where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && e.EntSupplierId == searchEntSupplierId
                                           select e.EntSupplierId).FirstOrDefault();
                }
            }
            EnterpriseSupplierAddressTDES model = null;
            ViewBagHelper(searchEntGuid, searchEntSupplierId);
            if (base.ViewBag.SearchEntSupplierId != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription");
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryCode2");
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription");
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription");
                EnterpriseSupplierAddressTDES enterpriseSupplierAddressTDES = new EnterpriseSupplierAddressTDES();
                enterpriseSupplierAddressTDES.AddressGuid = Guid.NewGuid();
                enterpriseSupplierAddressTDES.EntSupplierId = searchEntSupplierId;
                enterpriseSupplierAddressTDES.EntGuid = searchEntGuid.Value;
                enterpriseSupplierAddressTDES.AddressValidFrom = DateTime.Now;
                enterpriseSupplierAddressTDES.AddressValidTo = DateTime.Now;
                model = enterpriseSupplierAddressTDES;
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Create(EnterpriseSupplierAddressTDES enterprisesupplieraddresstdes, string SettlementNameLookUp, string SoatoLookUp)
        {
            if (SettlementNameLookUp != null || SoatoLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(enterprisesupplieraddresstdes);
                string text = "";
                string text2 = "";
                if (SettlementNameLookUp != null)
                {
                    text = enterprisesupplieraddresstdes.AddressSettlementName;
                    text2 = "2";
                }
                else
                {
                    text = enterprisesupplieraddresstdes.SoatoId;
                    text2 = "1";
                }
                string redirectContriller = "EnterpriseSupplierAddress";
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
            Guid? searchEntGuid = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                searchEntGuid = enterprisesupplieraddresstdes.EntGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
            }
            if (base.ModelState.IsValid && enterprisesupplieraddresstdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                db.EnterpriseSupplierAddressTDES.Add(enterprisesupplieraddresstdes);
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterprisesupplieraddresstdes.EntGuid,
                    searchEntSupplierId = enterprisesupplieraddresstdes.EntSupplierId
                });
            }
            ViewBagHelper(searchEntGuid, enterprisesupplieraddresstdes.EntSupplierId);
            if (base.ViewBag.SearchEntGuid != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterprisesupplieraddresstdes.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryCode2", enterprisesupplieraddresstdes.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterprisesupplieraddresstdes.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterprisesupplieraddresstdes.StreetTypeId);
            }
            return View(enterprisesupplieraddresstdes);
        }

        protected ActionResult GetSoatoForCreateUpdate(string redirecData, string soatoId, string actionName)
        {
            EnterpriseSupplierAddressTDES enterpriseSupplierAddressTDES = JsonConvert.DeserializeObject<EnterpriseSupplierAddressTDES>(redirecData);
            if (soatoId != null && soatoId.Length == 10)
            {
                Soato soato = db.Soato.Find(soatoId);
                enterpriseSupplierAddressTDES.SoatoId = soato.SoatoId;
                enterpriseSupplierAddressTDES.SettlementTypeId = soato.SettlementTypeId;
                enterpriseSupplierAddressTDES.AddressSettlementName = soato.SoatoSettlementName;
                if (!soatoId.EndsWith("000"))
                {
                    soatoId = soatoId.Substring(0, 7) + "000";
                    soato = db.Soato.Find(soatoId);
                    enterpriseSupplierAddressTDES.AddressRural = soato.SoatoSettlementName;
                }
                else
                {
                    enterpriseSupplierAddressTDES.AddressRural = "";
                }
                if (!soatoId.EndsWith("000000"))
                {
                    soatoId = soatoId.Substring(0, 4) + "000000";
                    soato = db.Soato.Find(soatoId);
                    enterpriseSupplierAddressTDES.AddressDistrict = soato.SoatoSettlementName;
                }
                else
                {
                    enterpriseSupplierAddressTDES.AddressDistrict = "";
                }
                if (!soatoId.EndsWith("000000000"))
                {
                    soatoId = soatoId.Substring(0, 1) + "000000000";
                    soato = db.Soato.Find(soatoId);
                    enterpriseSupplierAddressTDES.AddressRegion = soato.SoatoSettlementName;
                }
                else
                {
                    enterpriseSupplierAddressTDES.AddressRegion = "";
                }
            }
            ViewBagHelper(enterpriseSupplierAddressTDES.EntGuid, enterpriseSupplierAddressTDES.EntSupplierId);
            if (base.ViewBag.SearchEntSupplierId != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterpriseSupplierAddressTDES.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterpriseSupplierAddressTDES.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterpriseSupplierAddressTDES.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterpriseSupplierAddressTDES.StreetTypeId);
            }
            return View(actionName, enterpriseSupplierAddressTDES);
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

        public ActionResult Edit(Guid? id = default(Guid?))
        {
            EnterpriseSupplierAddressTDES enterpriseSupplierAddressTDES = null;
            UserIsInRoles();
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
                enterpriseSupplierAddressTDES = (from e in db.EnterpriseSupplierAddressTDES
                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                                 select e).FirstOrDefault();
            }
            else
            {
                enterpriseSupplierAddressTDES = db.EnterpriseSupplierAddressTDES.Find(id);
            }
            if (enterpriseSupplierAddressTDES == null)
            {
                return HttpNotFound();
            }
            ViewBagHelper(enterpriseSupplierAddressTDES.EntGuid, enterpriseSupplierAddressTDES.EntSupplierId);
            if (base.ViewBag.SearchEntSupplierId != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterpriseSupplierAddressTDES.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterpriseSupplierAddressTDES.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterpriseSupplierAddressTDES.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterpriseSupplierAddressTDES.StreetTypeId);
            }
            return View(enterpriseSupplierAddressTDES);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Edit(EnterpriseSupplierAddressTDES enterprisesupplieraddresstdes, string SettlementNameLookUp, string SoatoLookUp)
        {
            if (SettlementNameLookUp != null || SoatoLookUp != null)
            {
                string redirecData = JsonConvert.SerializeObject(enterprisesupplieraddresstdes);
                string text = "";
                string text2 = "";
                if (SettlementNameLookUp != null)
                {
                    text = enterprisesupplieraddresstdes.AddressSettlementName;
                    text2 = "2";
                }
                else
                {
                    text = enterprisesupplieraddresstdes.SoatoId;
                    text2 = "1";
                }
                string redirectContriller = "EnterpriseSupplierAddress";
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
            Guid? searchEntGuid = null;
            if (!((!base.ViewBag.IsEcoSystemAdmin) ? true : false))
            {
                searchEntGuid = enterprisesupplieraddresstdes.EntGuid;
            }
            else
            {
                string aName = base.User.Identity.Name;
                if (base.ViewBag.IsEnterpriseAdmin)
                {
                    searchEntGuid = (from e in db.EnterpriseUserTDES
                                     where e.EntUserNic == aName
                                     select e.EntGuid).FirstOrDefault();
                }
            }
            if (base.ModelState.IsValid && enterprisesupplieraddresstdes.EntGuid != searchEntGuid.Value)
            {
                base.ModelState.AddModelError("", Resources.ENTERPRISE_NOT_DEFINED);
            }
            if (base.ModelState.IsValid)
            {
                db.Entry(enterprisesupplieraddresstdes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new
                {
                    searchEntGuid = enterprisesupplieraddresstdes.EntGuid,
                    searchEntSupplierId = enterprisesupplieraddresstdes.EntSupplierId
                });
            }
            ViewBagHelper(searchEntGuid, enterprisesupplieraddresstdes.EntSupplierId);
            if (base.ViewBag.SearchEntSupplierId != null)
            {
                base.ViewBag.AddressTypeId = new SelectList(db.AddressType, "AddressTypeId", "AddressTypeDescription", enterprisesupplieraddresstdes.AddressTypeId);
                base.ViewBag.CountryIso = new SelectList(db.Country, "CountryIso", "CountryName", enterprisesupplieraddresstdes.CountryIso);
                base.ViewBag.SettlementTypeId = new SelectList(db.SettlementType, "SettlementTypeId", "SettlementTypeDescription", enterprisesupplieraddresstdes.SettlementTypeId);
                base.ViewBag.StreetTypeId = new SelectList(db.StreetType, "StreetTypeId", "StreetTypeDescription", enterprisesupplieraddresstdes.StreetTypeId);
            }
            return View(enterprisesupplieraddresstdes);
        }

        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        public ActionResult Delete(Guid? id = default(Guid?))
        {
            EnterpriseSupplierAddressTDES enterpriseSupplierAddressTDES = null;
            UserIsInRoles();
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
                enterpriseSupplierAddressTDES = (from e in db.EnterpriseSupplierAddressTDES
                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                                 select e).FirstOrDefault();
            }
            else
            {
                enterpriseSupplierAddressTDES = db.EnterpriseSupplierAddressTDES.Find(id);
            }
            if (enterpriseSupplierAddressTDES == null)
            {
                return HttpNotFound();
            }
            ViewBagHelper(enterpriseSupplierAddressTDES.EntGuid, enterpriseSupplierAddressTDES.EntSupplierId);
            return View(enterpriseSupplierAddressTDES);
        }

        [HttpPost]
        [Authorize(Roles = "EcoSystemAdmin,EnterpriseAdmin")]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            string searchEntSupplierId = null;
            EnterpriseSupplierAddressTDES enterpriseSupplierAddressTDES = null;
            UserIsInRoles();
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
                enterpriseSupplierAddressTDES = (from e in db.EnterpriseSupplierAddressTDES
                                                 where (Guid)(Guid?)e.EntGuid == (Guid)searchEntGuid && (Guid)(Guid?)e.AddressGuid == (Guid)id
                                                 select e).FirstOrDefault();
            }
            else
            {
                enterpriseSupplierAddressTDES = db.EnterpriseSupplierAddressTDES.Find(id);
            }
            if (enterpriseSupplierAddressTDES != null)
            {
                searchEntGuid = enterpriseSupplierAddressTDES.EntGuid;
                searchEntSupplierId = enterpriseSupplierAddressTDES.EntSupplierId;
            }
            db.EnterpriseSupplierAddressTDES.Remove(enterpriseSupplierAddressTDES);
            db.SaveChanges();
            return RedirectToAction("Index", new
            {
                searchEntGuid,
                searchEntSupplierId
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}