// CarShop.Utility.MakeIncomePayRoll
using CarShop.Models;
using CarShop.Utility;
using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Utility
{

    public class MakeIncomePayRoll
    {
        public static void ClearContexts(MakeIncomePayRollData aData)
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
            if (aData.dbArticle != null)
            {
                aData.dbArticle.Dispose();
                aData.dbArticle = null;
            }
        }

        public static void DoMakeIncomePayRoll(object makeincomepayrolldata)
        {
            MakeIncomePayRollData makeIncomePayRollData = makeincomepayrolldata as MakeIncomePayRollData;
            if (makeIncomePayRollData == null)
            {
                return;
            }
            if (makeIncomePayRollData.dbIncome == null || makeIncomePayRollData.dbRest == null || makeIncomePayRollData.dbArticle == null)
            {
                ClearContexts(makeIncomePayRollData);
                return;
            }
            if (makeIncomePayRollData.IncomePayRollTDESGuid == Guid.Empty)
            {
                ClearContexts(makeIncomePayRollData);
                return;
            }
            CarShopIncomeContext dbIncome = makeIncomePayRollData.dbIncome;
            Guid IncomePayRollTDESGuid = makeIncomePayRollData.IncomePayRollTDESGuid;
            CarShopRestContext dbRest = makeIncomePayRollData.dbRest;
            CarShopArticleContext dbArticle = makeIncomePayRollData.dbArticle;
            IncomePayRollTDES incomePayRollTDES = (from e in dbIncome.IncomePayRollTDES
                                                   where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid
                                                   select e).FirstOrDefault();
            if (incomePayRollTDES == null)
            {
                makeIncomePayRollData.hasError = true;
                makeIncomePayRollData.ErrorText = "Ведомость не найдена для IncomePayRollTDESGuid = " + IncomePayRollTDESGuid.ToString();
                dbIncome = null;
                dbRest = null;
                dbArticle = null;
                ClearContexts(makeIncomePayRollData);
                return;
            }
            if (incomePayRollTDES.IsReversed)
            {
                makeIncomePayRollData.hasError = true;
                makeIncomePayRollData.ErrorText = "Ведомость сторнирована для IncomePayRollTDESGuid = " + IncomePayRollTDESGuid.ToString();
                dbIncome = null;
                dbRest = null;
                dbArticle = null;
                ClearContexts(makeIncomePayRollData);
                return;
            }
            IncomeArticleTDES incomearticletdes;
            for (incomearticletdes = (from e in dbIncome.IncomeArticleTDES
                                      where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == false && e.ProcessedState != 0
                                      select e).FirstOrDefault(); incomearticletdes != null; incomearticletdes = (from e in dbIncome.IncomeArticleTDES
                                                                                                                  where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == false && e.ProcessedState != 0
                                                                                                                  select e).FirstOrDefault())
            {
                try
                {
                    incomearticletdes.Comments = null;
                    incomearticletdes.ProcessedState = 0;
                    dbIncome.SaveChanges();
                }
                catch
                {
                    makeIncomePayRollData.hasError = true;
                    makeIncomePayRollData.ErrorText = "Не могу обнулить  флаги состояний т.к. ошибка в БД";
                    dbIncome = null;
                    dbRest = null;
                    dbArticle = null;
                    ClearContexts(makeIncomePayRollData);
                    return;
                }
                dbIncome.Entry(incomearticletdes).State = EntityState.Detached;
            }
            incomearticletdes = (from e in dbIncome.IncomeArticleTDES
                                 where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == false && e.ProcessedState == 0
                                 select e).FirstOrDefault();
            while (incomearticletdes != null)
            {
                try
                {
                    incomearticletdes.ProcessedState = 1;
                    dbIncome.SaveChanges();
                }
                catch
                {
                    makeIncomePayRollData.hasError = true;
                    makeIncomePayRollData.ErrorText = "Не могу установить флаг состояния т.к. ошибка в БД";
                    dbIncome = null;
                    dbRest = null;
                    dbArticle = null;
                    ClearContexts(makeIncomePayRollData);
                    return;
                }
                BranchRestTDES branchRestTDES = (from e in dbRest.BranchRestTDES
                                                 where e.EntBranchArticle == incomearticletdes.EntArticle && e.EntBranchSup == incomearticletdes.EntBrandNic && e.EntBranchGuid == incomearticletdes.EntBranchGuid
                                                 select e).FirstOrDefault();
                if (branchRestTDES == null)
                {
                    EnterpriseArticleTDES enterprisearticletdes = (from e in dbArticle.EnterpriseArticleTDES.AsNoTracking().Include("EnterpriseArticleDescriptionTDES").AsNoTracking()
                                                                   where e.EntArticle == incomearticletdes.EntArticle && e.EntBrandNic == incomearticletdes.EntBrandNic && e.EntGuid == incomearticletdes.EntGuid
                                                                   select e).FirstOrDefault();
                    if (enterprisearticletdes == null)
                    {
                        try
                        {
                            incomearticletdes.Comments = "Нет описание артикула";
                            dbIncome.SaveChanges();
                        }
                        catch
                        {
                            makeIncomePayRollData.hasError = true;
                            makeIncomePayRollData.ErrorText = "Нет описание артикула для " + incomearticletdes.EntArticle + " : " + incomearticletdes.EntBrandNic;
                            dbIncome = null;
                            dbRest = null;
                            dbArticle = null;
                            ClearContexts(makeIncomePayRollData);
                            return;
                        }
                        dbIncome.Entry(incomearticletdes).State = EntityState.Detached;
                        incomearticletdes = (from e in dbIncome.IncomeArticleTDES
                                             where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == false && e.ProcessedState == 0
                                             select e).FirstOrDefault();
                        continue;
                    }
                    BranchRestArticleDescriptionTDES branchRestArticleDescriptionTDES = (from e in dbRest.BranchRestArticleDescriptionTDES
                                                                                         where e.EntArticleDescription == enterprisearticletdes.EnterpriseArticleDescriptionTDES.EntArticleDescription
                                                                                         select e).FirstOrDefault();
                    if (branchRestArticleDescriptionTDES == null)
                    {
                        BranchRestArticleDescriptionTDES branchRestArticleDescriptionTDES2 = new BranchRestArticleDescriptionTDES();
                        branchRestArticleDescriptionTDES2.EntArticleDescription = enterprisearticletdes.EnterpriseArticleDescriptionTDES.EntArticleDescription;
                        branchRestArticleDescriptionTDES = branchRestArticleDescriptionTDES2;
                        dbRest.BranchRestArticleDescriptionTDES.Add(branchRestArticleDescriptionTDES);
                        try
                        {
                            dbRest.SaveChanges();
                        }
                        catch
                        {
                            makeIncomePayRollData.hasError = true;
                            makeIncomePayRollData.ErrorText = "Не могу вставить запись НАИМЕНОВАНИЯ в каталог остатков для " + incomearticletdes.EntArticle + " : " + incomearticletdes.EntBrandNic;
                            dbIncome = null;
                            dbRest = null;
                            dbArticle = null;
                            ClearContexts(makeIncomePayRollData);
                            return;
                        }
                    }
                    BranchRestTDES branchRestTDES2 = new BranchRestTDES();
                    branchRestTDES2.EntBranchGuid = incomearticletdes.EntBranchGuid;
                    branchRestTDES2.EntBranchArticle = incomearticletdes.EntArticle;
                    branchRestTDES2.EntBranchSup = incomearticletdes.EntBrandNic;
                    branchRestTDES2.ART_ARTICLE_NR = enterprisearticletdes.ExternArticle;
                    branchRestTDES2.SUP_TEXT = enterprisearticletdes.ExternBrandNic;
                    branchRestTDES2.ArtAmount = 0;
                    branchRestTDES2.ArtPrice = 0.0;
                    branchRestTDES2.EntArticleDescriptionId = branchRestArticleDescriptionTDES.EntArticleDescriptionId;
                    branchRestTDES2.ExternArticleEAN = enterprisearticletdes.ExternArticleEAN;
                    branchRestTDES2.LastUpdated = DateTime.Now;
                    branchRestTDES = branchRestTDES2;
                    branchRestTDES.LastUpdated = branchRestTDES.LastUpdated.AddMilliseconds(-branchRestTDES.LastUpdated.Millisecond);
                    branchRestTDES.LastReplicated = branchRestTDES.LastUpdated.AddSeconds(-5.0);
                    dbRest.BranchRestTDES.Add(branchRestTDES);
                    try
                    {
                        dbRest.SaveChanges();
                    }
                    catch
                    {
                        makeIncomePayRollData.hasError = true;
                        makeIncomePayRollData.ErrorText = "Не могу вставить запись в каталог остатков для " + incomearticletdes.EntArticle + " : " + incomearticletdes.EntBrandNic;
                        dbIncome = null;
                        dbRest = null;
                        dbArticle = null;
                        ClearContexts(makeIncomePayRollData);
                        return;
                    }
                }
                try
                {
                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        branchRestTDES.ArtAmount += incomearticletdes.ArtAmountRest;
                        branchRestTDES.ArtPrice = incomearticletdes.CurrArtPrice;
                        incomearticletdes.IsProcessed = true;
                        dbIncome.SaveChanges();
                        dbRest.SaveChanges();
                        transactionScope.Complete();
                    }
                }
                catch
                {
                    makeIncomePayRollData.hasError = true;
                    makeIncomePayRollData.ErrorText = "Не могу обновить остатки (т.к. ошибка в БД) для " + incomearticletdes.EntArticle + " : " + incomearticletdes.EntBrandNic;
                    dbIncome = null;
                    dbRest = null;
                    dbArticle = null;
                    ClearContexts(makeIncomePayRollData);
                    return;
                }
                if (branchRestTDES != null)
                {
                    dbRest.Entry(branchRestTDES).State = EntityState.Detached;
                }
                dbIncome.Entry(incomearticletdes).State = EntityState.Detached;
                incomearticletdes = (from e in dbIncome.IncomeArticleTDES
                                     where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == false && e.ProcessedState == 0
                                     select e).FirstOrDefault();
            }
            int num = (from e in dbIncome.IncomeArticleTDES
                       where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == false && e.IsReversed == false
                       select e).Count();
            if (num > 0)
            {
                makeIncomePayRollData.hasError = true;
                makeIncomePayRollData.ErrorText = "Не все артикулы удалось оприходовать";
            }
            else
            {
                try
                {
                    incomePayRollTDES.IsProcessed = true;
                    dbIncome.SaveChanges();
                    makeIncomePayRollData.isDone = true;
                }
                catch
                {
                    makeIncomePayRollData.hasError = true;
                    makeIncomePayRollData.ErrorText = "Не могу изменить состояние ведомости т.к. Ошибка в БД";
                }
            }
            dbIncome = null;
            dbRest = null;
            dbArticle = null;
            ClearContexts(makeIncomePayRollData);
        }

        public static void DoReverseIncomePayRoll(object makeincomepayrolldata)
        {
            MakeIncomePayRollData makeIncomePayRollData = makeincomepayrolldata as MakeIncomePayRollData;
            if (makeIncomePayRollData == null)
            {
                return;
            }
            if (makeIncomePayRollData.dbIncome == null || makeIncomePayRollData.dbRest == null || makeIncomePayRollData.dbArticle == null)
            {
                ClearContexts(makeIncomePayRollData);
                return;
            }
            if (makeIncomePayRollData.IncomePayRollTDESGuid == Guid.Empty)
            {
                ClearContexts(makeIncomePayRollData);
                return;
            }
            CarShopIncomeContext carShopIncomeContext = makeIncomePayRollData.dbIncome;
            Guid IncomePayRollTDESGuid = makeIncomePayRollData.IncomePayRollTDESGuid;
            CarShopRestContext carShopRestContext = makeIncomePayRollData.dbRest;
            CarShopArticleContext dbArticle = makeIncomePayRollData.dbArticle;
            IncomePayRollTDES incomePayRollTDES = (from e in carShopIncomeContext.IncomePayRollTDES
                                                   where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid
                                                   select e).FirstOrDefault();
            if (incomePayRollTDES == null)
            {
                makeIncomePayRollData.hasError = true;
                makeIncomePayRollData.ErrorText = "Ведомость не найдена для IncomePayRollTDESGuid = " + IncomePayRollTDESGuid.ToString();
                carShopIncomeContext = null;
                carShopRestContext = null;
                ClearContexts(makeIncomePayRollData);
                return;
            }
            if (incomePayRollTDES.IsReversed)
            {
                makeIncomePayRollData.hasError = true;
                makeIncomePayRollData.ErrorText = "Ведомость сторнирована для IncomePayRollTDESGuid = " + IncomePayRollTDESGuid.ToString();
                carShopIncomeContext = null;
                ClearContexts(makeIncomePayRollData);
                return;
            }
            IncomeArticleTDES incomearticletdes;
            for (incomearticletdes = (from e in carShopIncomeContext.IncomeArticleTDES
                                      where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == true && e.IsReversed == false && e.ProcessedState != 0
                                      select e).FirstOrDefault(); incomearticletdes != null; incomearticletdes = (from e in carShopIncomeContext.IncomeArticleTDES
                                                                                                                  where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == true && e.IsReversed == false && e.ProcessedState != 0
                                                                                                                  select e).FirstOrDefault())
            {
                try
                {
                    incomearticletdes.Comments = null;
                    incomearticletdes.ProcessedState = 0;
                    carShopIncomeContext.SaveChanges();
                }
                catch
                {
                    makeIncomePayRollData.hasError = true;
                    makeIncomePayRollData.ErrorText = "Не могу обнулить  флаги состояний т.к. ошибка в БД";
                    carShopIncomeContext = null;
                    carShopRestContext = null;
                    ClearContexts(makeIncomePayRollData);
                }
                carShopIncomeContext.Entry(incomearticletdes).State = EntityState.Detached;
            }
            incomearticletdes = (from e in carShopIncomeContext.IncomeArticleTDES
                                 where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == true && e.IsReversed == false && e.ProcessedState == 0
                                 select e).FirstOrDefault();
            while (incomearticletdes != null)
            {
                try
                {
                    incomearticletdes.ProcessedState = 1;
                    carShopIncomeContext.SaveChanges();
                }
                catch
                {
                    makeIncomePayRollData.hasError = true;
                    makeIncomePayRollData.ErrorText = "Не могу установить флаг состояния т.к. ошибка в БД";
                    carShopIncomeContext = null;
                    carShopRestContext = null;
                    ClearContexts(makeIncomePayRollData);
                }
                RevaluationArticleTDES revaluationArticleTDES = (from e in carShopIncomeContext.RevaluationArticleTDES
                                                                 where e.IsProcessed == true && e.IsReversed == false && e.EntArticle == incomearticletdes.EntArticle && e.IncomePayRollTDESGuid == incomearticletdes.IncomePayRollTDESGuid
                                                                 select e).FirstOrDefault();
                if (revaluationArticleTDES != null)
                {
                    try
                    {
                        incomearticletdes.Comments = "Не могу сторнировать, т.к. нашлась НЕСТОРНИРОВАННАЯ ведомость переоценки " + revaluationArticleTDES.SheetRevaluationTDESGuid.ToString();
                        carShopIncomeContext.SaveChanges();
                    }
                    catch
                    {
                        makeIncomePayRollData.hasError = true;
                        makeIncomePayRollData.ErrorText = "Не могу сторнировать, т.к. нашлась НЕСТОРНИРОВАННАЯ ведомость переоценки и ошибка в БД";
                        carShopIncomeContext = null;
                        carShopRestContext = null;
                        ClearContexts(makeIncomePayRollData);
                    }
                    carShopIncomeContext.Entry(incomearticletdes).State = EntityState.Detached;
                    incomearticletdes = (from e in carShopIncomeContext.IncomeArticleTDES
                                         where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == true && e.IsReversed == false && e.ProcessedState == 0
                                         select e).FirstOrDefault();
                    continue;
                }
                BranchRestTDES branchRestTDES = (from e in carShopRestContext.BranchRestTDES
                                                 where e.EntBranchArticle == incomearticletdes.EntArticle && e.EntBranchSup == incomearticletdes.EntBrandNic && e.EntBranchGuid == incomearticletdes.EntBranchGuid
                                                 select e).FirstOrDefault();
                try
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        if (branchRestTDES != null)
                        {
                            branchRestTDES.ArtAmount -= incomearticletdes.ArtAmountRest;
                        }
                        incomearticletdes.IsReversed = true;
                        if (branchRestTDES != null)
                        {
                            carShopRestContext.SaveChanges();
                        }
                        carShopIncomeContext.SaveChanges();
                        transactionScope.Complete();
                    }
                }
                catch
                {
                    makeIncomePayRollData.hasError = true;
                    makeIncomePayRollData.ErrorText = "Не могу обновить остатки для " + incomearticletdes.EntArticle + " : " + incomearticletdes.EntBrandNic;
                    carShopIncomeContext = null;
                    carShopRestContext = null;
                    ClearContexts(makeIncomePayRollData);
                }
                if (branchRestTDES != null)
                {
                    carShopRestContext.Entry(branchRestTDES).State = EntityState.Detached;
                    branchRestTDES = null;
                }
                carShopIncomeContext.Entry(incomearticletdes).State = EntityState.Detached;
                incomearticletdes = (from e in carShopIncomeContext.IncomeArticleTDES
                                     where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == true && e.IsReversed == false && e.ProcessedState == 0
                                     select e).FirstOrDefault();
            }
            int num = (from e in carShopIncomeContext.IncomeArticleTDES
                       where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsProcessed == true && e.IsReversed == false
                       select e).Count();
            if (num > 0)
            {
                makeIncomePayRollData.hasError = true;
                makeIncomePayRollData.ErrorText = "Не все артикулы удалось сторнировать";
            }
            else
            {
                try
                {
                    incomePayRollTDES.IsReversed = true;
                    carShopIncomeContext.SaveChanges();
                    makeIncomePayRollData.isDone = true;
                }
                catch
                {
                    makeIncomePayRollData.hasError = true;
                    makeIncomePayRollData.ErrorText = "Не могу изменить состояние ведомости т.к. Ошибка в БД";
                }
            }
            carShopIncomeContext = null;
            carShopRestContext = null;
            ClearContexts(makeIncomePayRollData);
        }
    }
}