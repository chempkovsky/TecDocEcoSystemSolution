using CarShop.Models;
using CarShopDataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarShopDbRestDataService
{
    public class CarShopRestDBDataService : ICarShopRestDataService
    {
        #region DoCreateDb
        protected string connectionstringname = null;
        protected CarShopRestContext _db = null;
        protected CarShopRestContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new CarShopRestContext(connectionstringname);
                }
                return _db;
            }
        }
        public void DoCreateDb()
        {
            System.Data.Entity.Database.SetInitializer<CarShopRestContext>(new CarShopRestContextInitializer());
            using (CarShopRestContext db2 = new CarShopRestContext(connectionstringname))
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
