// CarShop.Utility.MakeSheetRevaluation
using CarShop.Models;
using CarShop.Utility;
using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Utility
{

    public static class MakeSheetRevaluation
    {
        public static void ClearContexts(MakeSheetRevaluationData aData)
        {
            if (aData.dbIncome != null)
            {
                aData.dbIncome.Dispose();
                aData.dbIncome = null;
            }
            if (aData.dbRest != null)
            {
                aData.dbRest.Dispose();
                aData.dbRest = null;
            }
        }

        public static void DoMakeSheetRevaluation(object makesheetrevaluationdata)
        {
            MakeSheetRevaluationData makeSheetRevaluationData = makesheetrevaluationdata as MakeSheetRevaluationData;
            if (makeSheetRevaluationData == null)
            {
                return;
            }
            if (makeSheetRevaluationData.dbIncome == null || makeSheetRevaluationData.dbRest == null)
            {
                ClearContexts(makeSheetRevaluationData);
                return;
            }
            if (makeSheetRevaluationData.SheetRevaluationTDESGuid == Guid.Empty)
            {
                ClearContexts(makeSheetRevaluationData);
                return;
            }
            CarShopIncomeContext dbIncome = makeSheetRevaluationData.dbIncome;
            CarShopRestContext dbRest = makeSheetRevaluationData.dbRest;
            Guid SheetRevaluationTDESGuid = makeSheetRevaluationData.SheetRevaluationTDESGuid;
            SheetRevaluationTDES sheetRevaluationTDES = (from e in dbIncome.SheetRevaluationTDES
                                                         where e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid
                                                         select e).FirstOrDefault();
            if (sheetRevaluationTDES == null)
            {
                makeSheetRevaluationData.hasError = true;
                makeSheetRevaluationData.ErrorText = "Не найду ведомость для  SheetRevaluationTDESGuid = " + SheetRevaluationTDESGuid.ToString();
                ClearContexts(makeSheetRevaluationData);
                dbIncome = null;
                dbRest = null;
                return;
            }
            if (sheetRevaluationTDES.IsReversed)
            {
                makeSheetRevaluationData.hasError = true;
                makeSheetRevaluationData.ErrorText = "Не могу обработать УЖЕ сторнированную ведомость для  SheetRevaluationTDESGuid = " + SheetRevaluationTDESGuid.ToString();
                ClearContexts(makeSheetRevaluationData);
                dbIncome = null;
                dbRest = null;
                return;
            }
            RevaluationArticleTDES revaluationarticletdes;
            for (revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                           where e.IsProcessed == false && e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState != 0
                                           select e).FirstOrDefault(); revaluationarticletdes != null; revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                                                                                                                 where e.IsProcessed == false && e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState != 0
                                                                                                                                 select e).FirstOrDefault())
            {
                revaluationarticletdes.ProcessedState = 0;
                revaluationarticletdes.Comments = "";
                try
                {
                    dbIncome.SaveChanges();
                }
                catch
                {
                    makeSheetRevaluationData.ErrorText = "Не могу обнулить  флаги состояний т.к. ошибка в БД";
                    makeSheetRevaluationData.hasError = true;
                    dbIncome = null;
                    dbRest = null;
                    ClearContexts(makeSheetRevaluationData);
                    dbIncome = null;
                    dbRest = null;
                    return;
                }
            }
            revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                      where e.IsProcessed == false && e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                      select e).FirstOrDefault();
            while (revaluationarticletdes != null)
            {
                try
                {
                    revaluationarticletdes.ProcessedState = 1;
                    dbIncome.SaveChanges();
                }
                catch
                {
                    makeSheetRevaluationData.ErrorText = "Не могу установить  флаги состояний т.к. ошибка в БД";
                    makeSheetRevaluationData.hasError = true;
                    ClearContexts(makeSheetRevaluationData);
                    dbIncome = null;
                    dbRest = null;
                    return;
                }
                IncomeArticleTDES incomearticletdesToReval = (from e in dbIncome.IncomeArticleTDES
                                                              where e.EntArticle == revaluationarticletdes.EntArticle && e.EntBrandNic == revaluationarticletdes.EntBrandNic && e.IncomePayRollTDESGuid == revaluationarticletdes.IncomePayRollTDESGuid
                                                              select e).FirstOrDefault();
                if (incomearticletdesToReval == null)
                {
                    revaluationarticletdes.Comments = "Не могу найти запись прихода";
                    try
                    {
                        dbIncome.SaveChanges();
                    }
                    catch
                    {
                        makeSheetRevaluationData.ErrorText = "Не могу найти запись прихода (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                        makeSheetRevaluationData.hasError = true;
                        ClearContexts(makeSheetRevaluationData);
                        dbIncome = null;
                        dbRest = null;
                        return;
                    }
                    dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
                    revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                              where e.IsProcessed == false && e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                              select e).FirstOrDefault();
                    continue;
                }
                if (!incomearticletdesToReval.IsProcessed)
                {
                    revaluationarticletdes.Comments = "Не могу обработать НЕОПРИХОДОВАННУЮ запись ведомости прихода";
                    try
                    {
                        dbIncome.SaveChanges();
                    }
                    catch
                    {
                        makeSheetRevaluationData.ErrorText = "Не могу обработать НЕОПРИХОДОВАННУЮ запись ведомости прихода (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                        makeSheetRevaluationData.hasError = true;
                        ClearContexts(makeSheetRevaluationData);
                        dbIncome = null;
                        dbRest = null;
                        return;
                    }
                    dbIncome.Entry(incomearticletdesToReval).State = EntityState.Detached;
                    dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
                    revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                              where e.IsProcessed == false && e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                              select e).FirstOrDefault();
                    continue;
                }
                if (incomearticletdesToReval.IsReversed)
                {
                    revaluationarticletdes.Comments = "Не могу обработать СТОРНИРОВАННУЮ запись ведомости прихода";
                    try
                    {
                        dbIncome.SaveChanges();
                    }
                    catch
                    {
                        makeSheetRevaluationData.ErrorText = "Не могу обработать УЖЕ СТОРНИРОВАННУЮ запись ведомости прихода (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                        makeSheetRevaluationData.hasError = true;
                        ClearContexts(makeSheetRevaluationData);
                        dbIncome = null;
                        dbRest = null;
                        return;
                    }
                    dbIncome.Entry(incomearticletdesToReval).State = EntityState.Detached;
                    dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
                    revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                              where e.IsProcessed == false && e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                              select e).FirstOrDefault();
                    continue;
                }
                if (incomearticletdesToReval.ArtAmountRest < 1)
                {
                    revaluationarticletdes.Comments = "Не могу обработать запись ведомости прихода с нулевым остатком";
                    try
                    {
                        dbIncome.SaveChanges();
                    }
                    catch
                    {
                        makeSheetRevaluationData.ErrorText = "Не могу обработать запись ведомости прихода с нулевым остатком (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                        makeSheetRevaluationData.hasError = true;
                        ClearContexts(makeSheetRevaluationData);
                        dbIncome = null;
                        dbRest = null;
                        return;
                    }
                    dbIncome.Entry(incomearticletdesToReval).State = EntityState.Detached;
                    dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
                    revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                              where e.IsProcessed == false && e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                              select e).FirstOrDefault();
                    continue;
                }
                BranchRestTDES branchRestTDES = (from e in dbRest.BranchRestTDES
                                                 where e.EntBranchArticle == incomearticletdesToReval.EntArticle && e.EntBranchSup == incomearticletdesToReval.EntBrandNic && e.EntBranchGuid == incomearticletdesToReval.EntBranchGuid
                                                 select e).FirstOrDefault();
                try
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        revaluationarticletdes.CurrArtPrice = incomearticletdesToReval.CurrArtPrice;
                        incomearticletdesToReval.CurrArtPrice = revaluationarticletdes.NewArtPrice;
                        revaluationarticletdes.ArtAmountRest = incomearticletdesToReval.ArtAmountRest;
                        revaluationarticletdes.OperSum = (revaluationarticletdes.NewArtPrice - revaluationarticletdes.CurrArtPrice) * (double)revaluationarticletdes.ArtAmountRest;
                        revaluationarticletdes.IsProcessed = true;
                        if (branchRestTDES != null)
                        {
                            branchRestTDES.ArtPrice = incomearticletdesToReval.CurrArtPrice;
                            branchRestTDES.LastUpdated = DateTime.Now;
                            branchRestTDES.LastUpdated = branchRestTDES.LastUpdated.AddMilliseconds(-branchRestTDES.LastUpdated.Millisecond);
                            dbRest.SaveChanges();
                        }
                        dbIncome.SaveChanges();
                        transactionScope.Complete();
                    }
                }
                catch
                {
                    makeSheetRevaluationData.hasError = true;
                    makeSheetRevaluationData.ErrorText = "Не могу обработать запись прихода (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                    ClearContexts(makeSheetRevaluationData);
                    dbIncome = null;
                    dbRest = null;
                    return;
                }
                if (branchRestTDES != null)
                {
                    dbRest.Entry(branchRestTDES).State = EntityState.Detached;
                    branchRestTDES = null;
                }
                dbIncome.Entry(incomearticletdesToReval).State = EntityState.Detached;
                incomearticletdesToReval = null;
                dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
                revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                          where e.IsProcessed == false && e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                          select e).FirstOrDefault();
            }
            int num = (from e in dbIncome.RevaluationArticleTDES
                       where e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.IsProcessed == false
                       select e).Count();
            if (num > 0)
            {
                makeSheetRevaluationData.hasError = true;
                makeSheetRevaluationData.ErrorText = "Не все артикулы удалось переоценить";
            }
            else
            {
                try
                {
                    sheetRevaluationTDES.IsProcessed = true;
                    dbIncome.SaveChanges();
                    makeSheetRevaluationData.isDone = true;
                }
                catch
                {
                    makeSheetRevaluationData.hasError = true;
                    makeSheetRevaluationData.ErrorText = "Не могу изменить состояние ведомости т.к. Ошибка в БД";
                }
            }
            ClearContexts(makeSheetRevaluationData);
            dbIncome = null;
            dbRest = null;
        }

        public static void DoReverseSheetRevaluation(object makesheetrevaluationdata)
        {
            MakeSheetRevaluationData makeSheetRevaluationData = makesheetrevaluationdata as MakeSheetRevaluationData;
            if (makeSheetRevaluationData == null)
            {
                return;
            }
            if (makeSheetRevaluationData.dbIncome == null || makeSheetRevaluationData.dbRest == null)
            {
                ClearContexts(makeSheetRevaluationData);
                return;
            }
            if (makeSheetRevaluationData.SheetRevaluationTDESGuid == Guid.Empty)
            {
                ClearContexts(makeSheetRevaluationData);
                return;
            }
            CarShopIncomeContext dbIncome = makeSheetRevaluationData.dbIncome;
            CarShopRestContext dbRest = makeSheetRevaluationData.dbRest;
            Guid SheetRevaluationTDESGuid = makeSheetRevaluationData.SheetRevaluationTDESGuid;
            SheetRevaluationTDES sheetRevaluationTDES = (from e in dbIncome.SheetRevaluationTDES
                                                         where e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid
                                                         select e).FirstOrDefault();
            if (sheetRevaluationTDES == null)
            {
                makeSheetRevaluationData.hasError = true;
                makeSheetRevaluationData.ErrorText = "Не найду ведомость для  SheetRevaluationTDESGuid = " + SheetRevaluationTDESGuid.ToString();
                ClearContexts(makeSheetRevaluationData);
                dbIncome = null;
                dbRest = null;
                return;
            }
            RevaluationArticleTDES revaluationarticletdes;
            for (revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                           where e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState != 0
                                           select e).FirstOrDefault(); revaluationarticletdes != null; revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                                                                                                                 where e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState != 0
                                                                                                                                 select e).FirstOrDefault())
            {
                revaluationarticletdes.ProcessedState = 0;
                revaluationarticletdes.Comments = "";
                try
                {
                    dbIncome.SaveChanges();
                }
                catch
                {
                    makeSheetRevaluationData.ErrorText = "Не могу найти запись прихода (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                    makeSheetRevaluationData.hasError = true;
                    ClearContexts(makeSheetRevaluationData);
                    dbIncome = null;
                    dbRest = null;
                    return;
                }
                dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
            }
            revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                      where e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                      select e).FirstOrDefault();
            while (revaluationarticletdes != null)
            {
                try
                {
                    revaluationarticletdes.ProcessedState = 1;
                    dbIncome.SaveChanges();
                }
                catch
                {
                    makeSheetRevaluationData.ErrorText = "Не могу установить  флаги состояний т.к. ошибка в БД";
                    makeSheetRevaluationData.hasError = true;
                    ClearContexts(makeSheetRevaluationData);
                    dbIncome = null;
                    dbRest = null;
                    return;
                }
                if (!revaluationarticletdes.IsProcessed)
                {
                    try
                    {
                        revaluationarticletdes.Comments = "Не могу сторнировать т.к. не было переоценки";
                        dbIncome.SaveChanges();
                    }
                    catch
                    {
                        makeSheetRevaluationData.ErrorText = "Не могу обработать запись переоценки (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                        makeSheetRevaluationData.hasError = true;
                        ClearContexts(makeSheetRevaluationData);
                        dbIncome = null;
                        dbRest = null;
                        return;
                    }
                    dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
                    revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                              where e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                              select e).FirstOrDefault();
                    continue;
                }
                IncomeArticleTDES incomearticletdesToReval = (from e in dbIncome.IncomeArticleTDES
                                                              where e.EntArticle == revaluationarticletdes.EntArticle && e.EntBrandNic == revaluationarticletdes.EntBrandNic && e.IncomePayRollTDESGuid == revaluationarticletdes.IncomePayRollTDESGuid
                                                              select e).FirstOrDefault();
                if (incomearticletdesToReval == null)
                {
                    try
                    {
                        revaluationarticletdes.Comments = "Не могу найти запись прихода";
                        dbIncome.SaveChanges();
                    }
                    catch
                    {
                        makeSheetRevaluationData.ErrorText = "Не могу найти запись прихода (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                        makeSheetRevaluationData.hasError = true;
                        ClearContexts(makeSheetRevaluationData);
                        dbIncome = null;
                        dbRest = null;
                        return;
                    }
                    dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
                    revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                              where e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                              select e).FirstOrDefault();
                    continue;
                }
                if (incomearticletdesToReval.ArtAmountRest != revaluationarticletdes.ArtAmountRest)
                {
                    try
                    {
                        revaluationarticletdes.Comments = "Не могу сторнировать, т.к. изменился остаток прихода";
                        dbIncome.SaveChanges();
                    }
                    catch
                    {
                        makeSheetRevaluationData.ErrorText = "Не могу сторнировать, т.к. изменился остаток (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                        makeSheetRevaluationData.hasError = true;
                        ClearContexts(makeSheetRevaluationData);
                        dbIncome = null;
                        dbRest = null;
                        return;
                    }
                    dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
                    revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                              where e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                              select e).FirstOrDefault();
                    continue;
                }
                if (incomearticletdesToReval.CurrArtPrice != revaluationarticletdes.NewArtPrice)
                {
                    try
                    {
                        revaluationarticletdes.Comments = "Не могу сторнировать, т.к. изменилась цена";
                        dbIncome.SaveChanges();
                    }
                    catch
                    {
                        makeSheetRevaluationData.ErrorText = "Не могу сторнировать, т.к. изменился остаток (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                        makeSheetRevaluationData.hasError = true;
                        ClearContexts(makeSheetRevaluationData);
                        dbIncome = null;
                        dbRest = null;
                        return;
                    }
                    dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
                    revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                              where e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                              select e).FirstOrDefault();
                    continue;
                }
                BranchRestTDES branchRestTDES = (from e in dbRest.BranchRestTDES
                                                 where e.EntBranchArticle == incomearticletdesToReval.EntArticle && e.EntBranchSup == incomearticletdesToReval.EntBrandNic && e.EntBranchGuid == incomearticletdesToReval.EntBranchGuid
                                                 select e).FirstOrDefault();
                try
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        incomearticletdesToReval.CurrArtPrice = revaluationarticletdes.CurrArtPrice;
                        revaluationarticletdes.IsProcessed = true;
                        revaluationarticletdes.IsReversed = true;
                        if (branchRestTDES != null)
                        {
                            branchRestTDES.ArtPrice = incomearticletdesToReval.CurrArtPrice;
                            branchRestTDES.LastUpdated = DateTime.Now;
                            branchRestTDES.LastUpdated = branchRestTDES.LastUpdated.AddMilliseconds(-branchRestTDES.LastUpdated.Millisecond);
                            dbRest.SaveChanges();
                        }
                        dbIncome.SaveChanges();
                        transactionScope.Complete();
                    }
                }
                catch
                {
                    makeSheetRevaluationData.ErrorText = "Не могу сторнировать, (т.к.ошибка БД) " + revaluationarticletdes.IncomePayRollTDESGuid.ToString() + " : " + revaluationarticletdes.EntArticle + " : " + revaluationarticletdes.EntBrandNic;
                    makeSheetRevaluationData.hasError = true;
                    ClearContexts(makeSheetRevaluationData);
                    dbIncome = null;
                    dbRest = null;
                    return;
                }
                if (branchRestTDES != null)
                {
                    dbRest.Entry(branchRestTDES).State = EntityState.Detached;
                    branchRestTDES = null;
                }
                dbIncome.Entry(incomearticletdesToReval).State = EntityState.Detached;
                incomearticletdesToReval = null;
                dbIncome.Entry(revaluationarticletdes).State = EntityState.Detached;
                revaluationarticletdes = (from e in dbIncome.RevaluationArticleTDES
                                          where e.IsReversed == false && e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.ProcessedState == 0
                                          select e).FirstOrDefault();
            }
            int num = (from e in dbIncome.RevaluationArticleTDES
                       where e.SheetRevaluationTDESGuid == SheetRevaluationTDESGuid && e.IsReversed == false
                       select e).Count();
            if (num > 0)
            {
                makeSheetRevaluationData.hasError = true;
                makeSheetRevaluationData.ErrorText = "Не все артикулы удалось сторнировать";
            }
            else
            {
                try
                {
                    sheetRevaluationTDES.IsReversed = true;
                    dbIncome.SaveChanges();
                    makeSheetRevaluationData.isDone = true;
                }
                catch
                {
                    makeSheetRevaluationData.hasError = true;
                    makeSheetRevaluationData.ErrorText = "Не могу изменить состояние ведомости т.к. Ошибка в БД";
                }
            }
            ClearContexts(makeSheetRevaluationData);
            dbIncome = null;
            dbRest = null;
        }
    }
}