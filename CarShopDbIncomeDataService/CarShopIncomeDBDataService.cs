using CarShop.Models;
using CarShopDataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarShopDbIncomeDataService
{
    public class CarShopIncomeDBDataService : ICarShopIncomeDataService
    {
        #region DoCreateDb
        protected string connectionstringname = null;
        protected CarShopIncomeContext _db = null;
        protected CarShopIncomeContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new CarShopIncomeContext(connectionstringname);
                }
                return _db;
            }
        }
        public void DoCreateDb()
        {
            System.Data.Entity.Database.SetInitializer<CarShopIncomeContext>(new CarShopIncomeContextInitializer());
            using (CarShopIncomeContext db2 = new CarShopIncomeContext(connectionstringname))
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
