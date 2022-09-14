// CarShop.Utility.Rest2Income
using CarShop.Models;
using CarShop.Utility;
using System;
using System.Data.Entity;
using System.Linq;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Utility
{

    public class Rest2Income
    {
        public static void ClearContexts(Rest2IncomeData aData)
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

        public static void DoMakeRest2Income(object rest2incomedata)
        {
            Rest2IncomeData rest2IncomeData = rest2incomedata as Rest2IncomeData;
            if (rest2IncomeData == null)
            {
                return;
            }
            if (rest2IncomeData.dbIncome == null || rest2IncomeData.dbRest == null || rest2IncomeData.dbArticle == null)
            {
                ClearContexts(rest2IncomeData);
                return;
            }
            if (rest2IncomeData.IncomePayRollTDESGuid == Guid.Empty)
            {
                ClearContexts(rest2IncomeData);
                return;
            }
            CarShopIncomeContext dbIncome = rest2IncomeData.dbIncome;
            Guid IncomePayRollTDESGuid = rest2IncomeData.IncomePayRollTDESGuid;
            CarShopRestContext dbRest = rest2IncomeData.dbRest;
            CarShopArticleContext dbArticle = rest2IncomeData.dbArticle;
            IncomePayRollTDES incomepayrolltdes = (from e in dbIncome.IncomePayRollTDES
                                                   where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid
                                                   select e).FirstOrDefault();
            if (incomepayrolltdes == null)
            {
                rest2IncomeData.hasError = true;
                rest2IncomeData.ErrorText = "Ведомость не найдена для IncomePayRollTDESGuid = " + IncomePayRollTDESGuid.ToString();
                dbIncome = null;
                dbRest = null;
                dbArticle = null;
                ClearContexts(rest2IncomeData);
                return;
            }
            if (incomepayrolltdes.IsReversed)
            {
                rest2IncomeData.hasError = true;
                rest2IncomeData.ErrorText = "Ведомость сторнирована для IncomePayRollTDESGuid = " + IncomePayRollTDESGuid.ToString();
                dbIncome = null;
                dbRest = null;
                dbArticle = null;
                ClearContexts(rest2IncomeData);
                return;
            }
            if (incomepayrolltdes.IsProcessed)
            {
                rest2IncomeData.hasError = true;
                rest2IncomeData.ErrorText = "Ведомость проведена для IncomePayRollTDESGuid = " + IncomePayRollTDESGuid.ToString();
                dbIncome = null;
                dbRest = null;
                dbArticle = null;
                ClearContexts(rest2IncomeData);
                return;
            }
            BranchRestTDES branchresttdes;
            for (branchresttdes = (from e in dbRest.BranchRestTDES
                                   where e.EntBranchGuid == incomepayrolltdes.EntBranchGuid && e.IsReplicated > 0
                                   select e).FirstOrDefault(); branchresttdes != null; branchresttdes = (from e in dbRest.BranchRestTDES
                                                                                                         where e.EntBranchGuid == incomepayrolltdes.EntBranchGuid && e.IsReplicated > 0
                                                                                                         select e).FirstOrDefault())
            {
                branchresttdes.LastReplicated = branchresttdes.LastUpdated;
                try
                {
                    dbRest.SaveChanges();
                }
                catch
                {
                    rest2IncomeData.hasError = true;
                    rest2IncomeData.ErrorText = "Не могу обнулить  флаги состояний остатков т.к. ошибка в БД";
                    dbIncome = null;
                    dbRest = null;
                    dbArticle = null;
                    ClearContexts(rest2IncomeData);
                    return;
                }
                dbRest.Entry(branchresttdes).State = EntityState.Detached;
            }
            for (branchresttdes = (from e in dbRest.BranchRestTDES
                                   where e.EntBranchGuid == incomepayrolltdes.EntBranchGuid && e.IsReplicated == 0
                                   select e).FirstOrDefault(); branchresttdes != null; branchresttdes = (from e in dbRest.BranchRestTDES
                                                                                                         where e.EntBranchGuid == incomepayrolltdes.EntBranchGuid && e.IsReplicated == 0
                                                                                                         select e).FirstOrDefault())
            {
                branchresttdes.LastUpdated = DateTime.Now;
                branchresttdes.LastUpdated = branchresttdes.LastUpdated.AddMilliseconds(-branchresttdes.LastUpdated.Millisecond);
                EnterpriseArticleTDES enterpriseArticleTDES = null;
                if (branchresttdes.ArtAmount > 0)
                {
                    enterpriseArticleTDES = (from e in dbArticle.EnterpriseArticleTDES.AsNoTracking().Include("EnterpriseArticleDescriptionTDES").AsNoTracking()
                                             where e.EntGuid == incomepayrolltdes.EntGuid && e.EntArticle == branchresttdes.EntBranchArticle && e.EntBrandNic == branchresttdes.EntBranchSup
                                             select e).FirstOrDefault();
                    if (enterpriseArticleTDES == null)
                    {
                        dbRest.Entry(branchresttdes).State = EntityState.Detached;
                        rest2IncomeData.hasError = true;
                        rest2IncomeData.ErrorText = "Не найду артикул предприятия " + branchresttdes.EntBranchArticle + " : " + branchresttdes.EntBranchSup;
                        dbIncome = null;
                        dbRest = null;
                        dbArticle = null;
                        ClearContexts(rest2IncomeData);
                        return;
                    }
                    IncomeArticleTDES incomeArticleTDES = new IncomeArticleTDES();
                    incomeArticleTDES.SupArticle = enterpriseArticleTDES.ExternArticle;
                    incomeArticleTDES.SupBrandNic = enterpriseArticleTDES.ExternBrandNic;
                    incomeArticleTDES.IncomePayRollTDESGuid = incomepayrolltdes.IncomePayRollTDESGuid;
                    incomeArticleTDES.EntArticle = enterpriseArticleTDES.EntArticle;
                    incomeArticleTDES.EntBrandNic = enterpriseArticleTDES.EntBrandNic;
                    incomeArticleTDES.EntArticleDescription = enterpriseArticleTDES.EnterpriseArticleDescriptionTDES.EntArticleDescription;
                    incomeArticleTDES.IsProcessed = true;
                    incomeArticleTDES.IsReversed = false;
                    incomeArticleTDES.ArtAmount = branchresttdes.ArtAmount;
                    incomeArticleTDES.ArtAmountRest = branchresttdes.ArtAmount;
                    incomeArticleTDES.PurchasePrice = branchresttdes.ArtPrice;
                    incomeArticleTDES.ArtPrice = branchresttdes.ArtPrice;
                    incomeArticleTDES.CurrArtPrice = branchresttdes.ArtPrice;
                    incomeArticleTDES.IsRevaluate = false;
                    incomeArticleTDES.EntBranchGuid = incomepayrolltdes.EntBranchGuid;
                    incomeArticleTDES.EntGuid = incomepayrolltdes.EntGuid;
                    incomeArticleTDES.Comments = "";
                    incomeArticleTDES.ProcessedState = 0;
                    IncomeArticleTDES entity = incomeArticleTDES;
                    try
                    {
                        dbIncome.IncomeArticleTDES.Add(entity);
                        dbIncome.SaveChanges();
                        dbIncome.Entry(entity).State = EntityState.Detached;
                    }
                    catch
                    {
                        dbRest.Entry(branchresttdes).State = EntityState.Detached;
                        rest2IncomeData.hasError = true;
                        rest2IncomeData.ErrorText = "Не могу вставить в БД прихода артикул " + branchresttdes.EntBranchArticle + " : " + branchresttdes.EntBranchSup;
                        dbIncome = null;
                        dbRest = null;
                        dbArticle = null;
                        ClearContexts(rest2IncomeData);
                        return;
                    }
                }
                try
                {
                    dbRest.SaveChanges();
                }
                catch
                {
                    dbRest.Entry(branchresttdes).State = EntityState.Detached;
                    rest2IncomeData.hasError = true;
                    rest2IncomeData.ErrorText = "Не могу обновить БД остатков для артикула " + branchresttdes.EntBranchArticle + " : " + branchresttdes.EntBranchSup;
                    dbIncome = null;
                    dbRest = null;
                    dbArticle = null;
                    ClearContexts(rest2IncomeData);
                    return;
                }
                dbRest.Entry(branchresttdes).State = EntityState.Detached;
            }
            branchresttdes = (from e in dbRest.BranchRestTDES
                              where e.EntBranchGuid == incomepayrolltdes.EntBranchGuid && e.IsReplicated == 0
                              select e).FirstOrDefault();
            if (branchresttdes != null)
            {
                rest2IncomeData.hasError = true;
                rest2IncomeData.ErrorText = "Не все артикулы удалось скопировать в приходную ведомость";
            }
            else
            {
                incomepayrolltdes.IsProcessed = true;
                try
                {
                    dbIncome.SaveChanges();
                    dbIncome.Entry(incomepayrolltdes).State = EntityState.Detached;
                    rest2IncomeData.isDone = true;
                    rest2IncomeData.hasError = false;
                }
                catch
                {
                    rest2IncomeData.hasError = true;
                    rest2IncomeData.ErrorText = "Не могу обновить состояние ведомости прихода";
                    dbIncome = null;
                    dbRest = null;
                    dbArticle = null;
                    ClearContexts(rest2IncomeData);
                    return;
                }
            }
            for (branchresttdes = (from e in dbRest.BranchRestTDES
                                   where e.EntBranchGuid == incomepayrolltdes.EntBranchGuid && e.IsReplicated > 0
                                   select e).FirstOrDefault(); branchresttdes != null; branchresttdes = (from e in dbRest.BranchRestTDES
                                                                                                         where e.EntBranchGuid == incomepayrolltdes.EntBranchGuid && e.IsReplicated > 0
                                                                                                         select e).FirstOrDefault())
            {
                branchresttdes.LastReplicated = branchresttdes.LastUpdated;
                try
                {
                    dbRest.SaveChanges();
                }
                catch
                {
                    rest2IncomeData.hasError = true;
                    rest2IncomeData.ErrorText = "Не могу повторно обнулить  флаги состояний остатков т.к. ошибка в БД";
                    dbIncome = null;
                    dbRest = null;
                    dbArticle = null;
                    ClearContexts(rest2IncomeData);
                    return;
                }
                dbRest.Entry(branchresttdes).State = EntityState.Detached;
            }
            dbIncome = null;
            dbRest = null;
            dbArticle = null;
            ClearContexts(rest2IncomeData);
        }
    }
}