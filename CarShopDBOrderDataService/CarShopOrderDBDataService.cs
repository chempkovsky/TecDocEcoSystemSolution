using CarShopDataService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarShopDBOrderDataService
{
    public class CarShopOrderDBDataService : ICarShopOrderDataService
    {
        #region DoCreateDb
        protected string connectionstringname = null;
        protected CarShopOrdersContext _db = null;
        protected CarShopOrdersContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new CarShopOrdersContext(connectionstringname);
                }
                return _db;
            }
        }
        public void DoCreateDb()
        {
            System.Data.Entity.Database.SetInitializer<CarShopOrdersContext>(new CarShopOrdersContextInitializer());
            using (CarShopOrdersContext db2 = new CarShopOrdersContext(connectionstringname))
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
