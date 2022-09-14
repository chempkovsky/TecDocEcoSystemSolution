﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarShopDataService.Interfaces
{
    public interface ICarShopRestDataService
    {
        #region DoCreateDb
        void DoCreateDb();
        string GetConnectionStringName();
        void SetConnectionStringName(string connectionStringName);
        #endregion

    }
}
