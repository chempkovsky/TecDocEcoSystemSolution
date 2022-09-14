using CarShop.Models;
using CarShopDataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarShopDbSalesDataService
{
    class CarShopSalesDBDataService : ICarShopSalesDataService
    {
        #region DoCreateDb
        protected string connectionstringname = null;
        protected CarShopSalesContext _db = null;
        protected CarShopSalesContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new CarShopSalesContext(connectionstringname);
                }
                return _db;
            }
        }
        public void DoCreateDb()
        {
            System.Data.Entity.Database.SetInitializer<CarShopSalesContext>(new CarShopSalesContextInitializer());
            using (CarShopSalesContext db2 = new CarShopSalesContext(connectionstringname))
            {
                db2.Database.Initialize(true);
            } //db.Dispose();
        }

        public string GetConnectionStringName()
        {
            return connectionstringname;
        }
        public void SetConnectionStringName(string connectionStringName)
        {
            if (connectionStringName == null)
            {
                if (connectionstringname != null)
                {
                    if (_db != null)
                    {
                        _db.Dispose();
                        _db = null;
                    }
                }
                connectionstringname = connectionStringName;
            }
            else
            {
                if (!connectionStringName.Equals(connectionstringname))
                {
                    if (_db != null)
                    {
                        _db.Dispose();
                        _db = null;
                    }
                }
                connectionstringname = connectionStringName;
            }
        }
        #endregion
    }
}
