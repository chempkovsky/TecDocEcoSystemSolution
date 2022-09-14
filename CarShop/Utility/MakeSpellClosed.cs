// CarShop.Utility.MakeSpellClosed
using CarShop.Models;
using CarShop.Utility;
using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Utility
{

    public class MakeSpellClosed
    {
        public static void ClearContexts(MakeSpellClosedData aData)
        {
            if (aData.dbIncome != null)
            {
                aData.dbIncome.Dispose();
                aData.dbIncome = null;
            }
            if (aData.dbSales != null)
            {
                aData.dbSales.Dispose();
                aData.dbSales = null;
            }
        }

        public static void DoMakeSpellClosed(object makespellcloseddata)
        {
            MakeSpellClosedData makeSpellClosedData = makespellcloseddata as MakeSpellClosedData;
            if (makeSpellClosedData == null)
            {
                return;
            }
            if (makeSpellClosedData.dbIncome == null || makeSpellClosedData.dbSales == null)
            {
                ClearContexts(makeSpellClosedData);
                return;
            }
            if (makeSpellClosedData.SpellGuid == Guid.Empty)
            {
                ClearContexts(makeSpellClosedData);
                return;
            }
            CarShopIncomeContext dbIncome = makeSpellClosedData.dbIncome;
            Guid SpellGuid = makeSpellClosedData.SpellGuid;
            CarShopSalesContext dbSales = makeSpellClosedData.dbSales;
            BranchSpellHstTDES branchSpellHstTDES = (from e in dbSales.BranchSpellHstTDES
                                                     where e.SpellGuid == SpellGuid
                                                     select e).FirstOrDefault();
            if (branchSpellHstTDES == null)
            {
                makeSpellClosedData.hasError = true;
                makeSpellClosedData.ErrorText = "Смена не найдена для SpellGuid = " + SpellGuid.ToString();
                dbIncome = null;
                dbSales = null;
                ClearContexts(makeSpellClosedData);
                return;
            }
            if (branchSpellHstTDES.IsActive)
            {
                makeSpellClosedData.hasError = true;
                makeSpellClosedData.ErrorText = "Смена не закрыта для SpellGuid = " + SpellGuid.ToString();
                dbIncome = null;
                dbSales = null;
                ClearContexts(makeSpellClosedData);
                return;
            }
            if (!branchSpellHstTDES.IsBlocked)
            {
                makeSpellClosedData.hasError = true;
                makeSpellClosedData.ErrorText = "Смена не заблокирована для SpellGuid = " + SpellGuid.ToString();
                dbIncome = null;
                dbSales = null;
                ClearContexts(makeSpellClosedData);
                return;
            }
            SaleBasketArticleTDES salebasketarticletdes;
            for (salebasketarticletdes = (from e in dbSales.SaleBasketArticleTDES
                                          where e.SpellGuid == SpellGuid && e.IsSpellClosed == true
                                          select e).FirstOrDefault(); salebasketarticletdes != null; salebasketarticletdes = (from e in dbSales.SaleBasketArticleTDES
                                                                                                                              where e.SpellGuid == SpellGuid && e.IsSpellClosed == true
                                                                                                                              select e).FirstOrDefault())
            {
                try
                {
                    salebasketarticletdes.IsSpellClosed = false;
                    dbSales.SaveChanges();
                    dbSales.Entry(salebasketarticletdes).State = EntityState.Detached;
                }
                catch
                {
                    makeSpellClosedData.hasError = true;
                    makeSpellClosedData.ErrorText = "Ошибка расстановки флагов готовности для SpellGuid = " + SpellGuid.ToString();
                    dbIncome = null;
                    dbSales = null;
                    ClearContexts(makeSpellClosedData);
                    return;
                }
            }
            ReturnBasketArticleTDES returnbasketarticletdes;
            for (returnbasketarticletdes = (from e in dbSales.ReturnBasketArticleTDES
                                            where e.SpellGuid == SpellGuid && e.IsSpellClosed == true
                                            select e).FirstOrDefault(); returnbasketarticletdes != null; returnbasketarticletdes = (from e in dbSales.ReturnBasketArticleTDES
                                                                                                                                    where e.SpellGuid == SpellGuid && e.IsSpellClosed == true
                                                                                                                                    select e).FirstOrDefault())
            {
                try
                {
                    returnbasketarticletdes.IsSpellClosed = false;
                    dbSales.SaveChanges();
                    dbSales.Entry(returnbasketarticletdes).State = EntityState.Detached;
                }
                catch
                {
                    makeSpellClosedData.hasError = true;
                    makeSpellClosedData.ErrorText = "Ошибка расстановки флагов готовности для возвратов для SpellGuid = " + SpellGuid.ToString();
                    dbIncome = null;
                    dbSales = null;
                    ClearContexts(makeSpellClosedData);
                    return;
                }
            }
            salebasketarticletdes = (from e in dbSales.SaleBasketArticleTDES
                                     where e.SpellGuid == SpellGuid && e.IsSpellClosed == false && e.IsPaid == true
                                     select e).FirstOrDefault();
            while (salebasketarticletdes != null)
            {
                if (salebasketarticletdes.ArtAmount - salebasketarticletdes.ReverseAmount - salebasketarticletdes.CribFromIncome < 1)
                {
                    try
                    {
                        salebasketarticletdes.IsSpellClosed = true;
                        dbSales.SaveChanges();
                        dbSales.Entry(salebasketarticletdes).State = EntityState.Detached;
                    }
                    catch
                    {
                        makeSpellClosedData.hasError = true;
                        makeSpellClosedData.ErrorText = "Ошибка расстановки флагов готовности для SpellGuid = " + SpellGuid.ToString();
                        dbIncome = null;
                        dbSales = null;
                        ClearContexts(makeSpellClosedData);
                        return;
                    }
                    salebasketarticletdes = (from e in dbSales.SaleBasketArticleTDES
                                             where e.SpellGuid == SpellGuid && e.IsSpellClosed == false && e.IsPaid == true
                                             select e).FirstOrDefault();
                    continue;
                }
                for (IncomeArticleTDES incomeArticleTDES = (from e in dbIncome.IncomeArticleTDES
                                                            where e.EntBranchGuid == salebasketarticletdes.EntBranchGuid && e.EntGuid == salebasketarticletdes.EntGuid && e.EntArticle == salebasketarticletdes.EntArticle && e.EntBrandNic == salebasketarticletdes.EntBrandNic && e.IsProcessed == true && e.IsReversed == false && e.ArtAmountRest > 0
                                                            select e).FirstOrDefault(); incomeArticleTDES != null; incomeArticleTDES = (from e in dbIncome.IncomeArticleTDES
                                                                                                                                        where e.EntBranchGuid == salebasketarticletdes.EntBranchGuid && e.EntGuid == salebasketarticletdes.EntGuid && e.EntArticle == salebasketarticletdes.EntArticle && e.EntBrandNic == salebasketarticletdes.EntBrandNic && e.IsProcessed == true && e.IsReversed == false && e.ArtAmountRest > 0
                                                                                                                                        select e).FirstOrDefault())
                {
                    int artAmountRest = incomeArticleTDES.ArtAmountRest;
                    int num = salebasketarticletdes.ArtAmount - salebasketarticletdes.ReverseAmount - salebasketarticletdes.CribFromIncome;
                    if (artAmountRest >= num)
                    {
                        salebasketarticletdes.CribFromIncome += num;
                        salebasketarticletdes.IsSpellClosed = true;
                        incomeArticleTDES.ArtAmountRest -= num;
                    }
                    else
                    {
                        salebasketarticletdes.CribFromIncome += artAmountRest;
                        incomeArticleTDES.ArtAmountRest -= artAmountRest;
                    }
                    try
                    {
                        using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                        {
                            dbIncome.SaveChanges();
                            dbSales.SaveChanges();
                            transactionScope.Complete();
                        }
                    }
                    catch
                    {
                        makeSpellClosedData.hasError = true;
                        makeSpellClosedData.ErrorText = "Ошибка выполнения распределенной транзакции для SpellGuid = " + SpellGuid.ToString();
                        dbIncome = null;
                        dbSales = null;
                        ClearContexts(makeSpellClosedData);
                        return;
                    }
                    dbIncome.Entry(incomeArticleTDES).State = EntityState.Detached;
                    if (salebasketarticletdes.IsSpellClosed)
                    {
                        break;
                    }
                }
                if (!salebasketarticletdes.IsSpellClosed)
                {
                    try
                    {
                        salebasketarticletdes.IsSpellClosed = true;
                        dbSales.SaveChanges();
                        dbSales.Entry(salebasketarticletdes).State = EntityState.Detached;
                    }
                    catch
                    {
                        makeSpellClosedData.hasError = true;
                        makeSpellClosedData.ErrorText = "Ошибка расстановки флага обработки для SpellGuid = " + SpellGuid.ToString();
                        dbIncome = null;
                        dbSales = null;
                        ClearContexts(makeSpellClosedData);
                        return;
                    }
                }
                else
                {
                    dbSales.Entry(salebasketarticletdes).State = EntityState.Detached;
                }
                salebasketarticletdes = (from e in dbSales.SaleBasketArticleTDES
                                         where e.SpellGuid == SpellGuid && e.IsSpellClosed == false && e.IsPaid == true
                                         select e).FirstOrDefault();
            }
            returnbasketarticletdes = (from e in dbSales.ReturnBasketArticleTDES
                                       where e.SpellGuid == SpellGuid && e.IsSpellClosed == false && e.IsPaid == true
                                       select e).FirstOrDefault();
            while (returnbasketarticletdes != null)
            {
                if (returnbasketarticletdes.ArtAmount - returnbasketarticletdes.ReverseAmount - returnbasketarticletdes.CribFromIncome < 1)
                {
                    try
                    {
                        returnbasketarticletdes.IsSpellClosed = true;
                        dbSales.SaveChanges();
                        dbSales.Entry(returnbasketarticletdes).State = EntityState.Detached;
                    }
                    catch
                    {
                        makeSpellClosedData.hasError = true;
                        makeSpellClosedData.ErrorText = "Ошибка расстановки флагов готовности для возврта для SpellGuid = " + SpellGuid.ToString();
                        dbIncome = null;
                        dbSales = null;
                        ClearContexts(makeSpellClosedData);
                        return;
                    }
                    returnbasketarticletdes = (from e in dbSales.ReturnBasketArticleTDES
                                               where e.SpellGuid == SpellGuid && e.IsSpellClosed == false && e.IsPaid == true
                                               select e).FirstOrDefault();
                    continue;
                }
                for (IncomeArticleTDES incomeArticleTDES2 = (from e in dbIncome.IncomeArticleTDES
                                                             where e.EntBranchGuid == returnbasketarticletdes.EntBranchGuid && e.EntGuid == returnbasketarticletdes.EntGuid && e.EntArticle == returnbasketarticletdes.EntArticle && e.EntBrandNic == returnbasketarticletdes.EntBrandNic && e.IsProcessed == true && e.IsReversed == false && e.ArtAmountRest < e.ArtAmount
                                                             select e).FirstOrDefault(); incomeArticleTDES2 != null; incomeArticleTDES2 = (from e in dbIncome.IncomeArticleTDES
                                                                                                                                           where e.EntBranchGuid == returnbasketarticletdes.EntBranchGuid && e.EntGuid == returnbasketarticletdes.EntGuid && e.EntArticle == returnbasketarticletdes.EntArticle && e.EntBrandNic == returnbasketarticletdes.EntBrandNic && e.IsProcessed == true && e.IsReversed == false && e.ArtAmountRest < e.ArtAmount
                                                                                                                                           select e).FirstOrDefault())
                {
                    int num2 = incomeArticleTDES2.ArtAmount - incomeArticleTDES2.ArtAmountRest;
                    int num3 = returnbasketarticletdes.ArtAmount - returnbasketarticletdes.ReverseAmount - returnbasketarticletdes.CribFromIncome;
                    if (num2 >= num3)
                    {
                        returnbasketarticletdes.CribFromIncome += num3;
                        returnbasketarticletdes.IsSpellClosed = true;
                        incomeArticleTDES2.ArtAmountRest += num3;
                    }
                    else
                    {
                        returnbasketarticletdes.CribFromIncome += num2;
                        incomeArticleTDES2.ArtAmountRest += num2;
                    }
                    try
                    {
                        using (TransactionScope transactionScope2 = new TransactionScope(TransactionScopeOption.Required))
                        {
                            dbIncome.SaveChanges();
                            dbSales.SaveChanges();
                            transactionScope2.Complete();
                        }
                    }
                    catch
                    {
                        makeSpellClosedData.hasError = true;
                        makeSpellClosedData.ErrorText = "Ошибка выполнения распределенной транзакции для SpellGuid = " + SpellGuid.ToString();
                        dbIncome = null;
                        dbSales = null;
                        ClearContexts(makeSpellClosedData);
                        return;
                    }
                    dbIncome.Entry(incomeArticleTDES2).State = EntityState.Detached;
                    if (returnbasketarticletdes.IsSpellClosed)
                    {
                        break;
                    }
                }
                if (!returnbasketarticletdes.IsSpellClosed)
                {
                    try
                    {
                        returnbasketarticletdes.IsSpellClosed = true;
                        dbSales.SaveChanges();
                        dbSales.Entry(returnbasketarticletdes).State = EntityState.Detached;
                    }
                    catch
                    {
                        makeSpellClosedData.hasError = true;
                        makeSpellClosedData.ErrorText = "Ошибка расстановки флага обработки для SpellGuid = " + SpellGuid.ToString();
                        dbIncome = null;
                        dbSales = null;
                        ClearContexts(makeSpellClosedData);
                        return;
                    }
                }
                else
                {
                    dbSales.Entry(returnbasketarticletdes).State = EntityState.Detached;
                }
                returnbasketarticletdes = (from e in dbSales.ReturnBasketArticleTDES
                                           where e.SpellGuid == SpellGuid && e.IsSpellClosed == false && e.IsPaid == true
                                           select e).FirstOrDefault();
            }
            try
            {
                branchSpellHstTDES.IsCribFromIncome = (!dbSales.ReturnBasketArticleTDES.Any((ReturnBasketArticleTDES e) => e.SpellGuid == SpellGuid && e.IsPaid == true && e.ArtAmount > e.ReverseAmount + e.CribFromIncome) && !dbSales.SaleBasketArticleTDES.Any((SaleBasketArticleTDES e) => e.SpellGuid == SpellGuid && e.IsPaid == true && e.ArtAmount > e.ReverseAmount + e.CribFromIncome));
                dbSales.SaveChanges();
            }
            catch
            {
                makeSpellClosedData.hasError = true;
                makeSpellClosedData.ErrorText = "Ошибка изменения статуса смены SpellGuid = " + SpellGuid.ToString();
                dbIncome = null;
                dbSales = null;
                ClearContexts(makeSpellClosedData);
                return;
            }
            makeSpellClosedData.isDone = true;
            makeSpellClosedData.hasError = false;
            makeSpellClosedData.ErrorText = "Смена обработана для SpellGuid = " + SpellGuid.ToString();
            dbIncome = null;
            dbSales = null;
            ClearContexts(makeSpellClosedData);
        }
    }
}