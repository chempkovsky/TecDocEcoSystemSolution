// CarShop.Utility.CreateSheetRevaluation
using CarShop.Models;
using CarShop.Utility;
using System;
using System.Data.Entity;
using System.Linq;
using TecDocEcoSystemDbClassLibrary;


namespace CarShop.Utility
{
    public class CreateSheetRevaluation
    {
        public static void ClearContexts(CreateSheetRevaluationData aData)
        {
            if (aData.dbIncome != null)
            {
                aData.dbIncome.Dispose();
                aData.dbIncome = null;
            }
        }

        public static void DoCreateSheetRevaluation(object createsheetrevaluationdata)
        {
            CreateSheetRevaluationData createSheetRevaluationData = createsheetrevaluationdata as CreateSheetRevaluationData;
            if (createSheetRevaluationData == null || createSheetRevaluationData.dbIncome == null)
            {
                return;
            }
            if (createSheetRevaluationData.IncomePayRollTDESGuid == Guid.Empty)
            {
                ClearContexts(createSheetRevaluationData);
                return;
            }
            CarShopIncomeContext dbIncome = createSheetRevaluationData.dbIncome;
            Guid IncomePayRollTDESGuid = createSheetRevaluationData.IncomePayRollTDESGuid;
            IncomePayRollTDES incomePayRollTDES = (from e in dbIncome.IncomePayRollTDES
                                                   where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid
                                                   select e).FirstOrDefault();
            if (incomePayRollTDES == null)
            {
                createSheetRevaluationData.hasError = true;
                createSheetRevaluationData.ErrorText = "Не найду ведомость с ID = " + IncomePayRollTDESGuid.ToString();
                dbIncome = null;
                ClearContexts(createSheetRevaluationData);
                return;
            }
            IncomeArticleTDES incomearticletdes;
            for (incomearticletdes = (from e in dbIncome.IncomeArticleTDES
                                      where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsRevaluate == true && e.ProcessedState != 0
                                      select e).FirstOrDefault(); incomearticletdes != null; incomearticletdes = (from e in dbIncome.IncomeArticleTDES
                                                                                                                  where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsRevaluate == true && e.ProcessedState != 0
                                                                                                                  select e).FirstOrDefault())
            {
                try
                {
                    incomearticletdes.ProcessedState = 0;
                    dbIncome.SaveChanges();
                }
                catch
                {
                    createSheetRevaluationData.hasError = true;
                    createSheetRevaluationData.ErrorText = "Не могу обнулить  флаги состояний т.к. ошибка в БД";
                    ClearContexts(createSheetRevaluationData);
                    return;
                }
                dbIncome.Entry(incomearticletdes).State = EntityState.Detached;
                incomearticletdes = null;
            }
            SheetRevaluationTDES sheetrevaluationtdes = (from e in dbIncome.SheetRevaluationTDES
                                                         where e.SheetRevaluationTDESGuid == IncomePayRollTDESGuid
                                                         select e).FirstOrDefault();
            if (sheetrevaluationtdes == null)
            {
                sheetrevaluationtdes = new SheetRevaluationTDES
                {
                    SheetRevaluationTDESGuid = IncomePayRollTDESGuid,
                    Description = incomePayRollTDES.Description,
                    EntBranchGuid = incomePayRollTDES.EntBranchGuid,
                    EntGuid = incomePayRollTDES.EntGuid,
                    EntUserNic = incomePayRollTDES.EntUserNic,
                    CreatedAt = DateTime.Now,
                    IsProcessed = false,
                    IsReversed = false
                };
                sheetrevaluationtdes.CreatedAt = sheetrevaluationtdes.CreatedAt.AddMilliseconds(-sheetrevaluationtdes.CreatedAt.Millisecond);
                try
                {
                    dbIncome.SheetRevaluationTDES.Add(sheetrevaluationtdes);
                    dbIncome.SaveChanges();
                }
                catch
                {
                    createSheetRevaluationData.ErrorText = "Не могу создать заголовок ведомости переоценки т.к. ошибка в БД";
                    createSheetRevaluationData.hasError = true;
                    ClearContexts(createSheetRevaluationData);
                    return;
                }
            }
            for (incomearticletdes = (from e in dbIncome.IncomeArticleTDES
                                      where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsRevaluate == true && e.ProcessedState == 0
                                      select e).FirstOrDefault(); incomearticletdes != null; incomearticletdes = (from e in dbIncome.IncomeArticleTDES
                                                                                                                  where e.IncomePayRollTDESGuid == IncomePayRollTDESGuid && e.IsRevaluate == true && e.ProcessedState == 0
                                                                                                                  select e).FirstOrDefault())
            {
                try
                {
                    incomearticletdes.ProcessedState = 1;
                    dbIncome.SaveChanges();
                }
                catch
                {
                    createSheetRevaluationData.ErrorText = "Не могу установить флаг старта обработки т.к. ошибка в БД";
                    createSheetRevaluationData.hasError = true;
                    ClearContexts(createSheetRevaluationData);
                    return;
                }
                for (IncomeArticleTDES incomeArticleTDES = (from e in dbIncome.IncomeArticleTDES
                                                            where e.IsProcessed == true && e.IsReversed == false && e.CurrArtPrice != incomearticletdes.CurrArtPrice && e.ArtAmountRest > 0 && e.EntArticle == incomearticletdes.EntArticle && e.EntBrandNic == incomearticletdes.EntBrandNic && e.EntBranchGuid == incomearticletdes.EntBranchGuid && e.EntGuid == incomearticletdes.EntGuid && e.IncomePayRollTDESGuid != incomearticletdes.IncomePayRollTDESGuid && !dbIncome.RevaluationArticleTDES.Any((RevaluationArticleTDES r) => r.EntArticle == e.EntArticle && r.EntBrandNic == e.EntBrandNic && r.IncomePayRollTDESGuid == e.IncomePayRollTDESGuid && r.SheetRevaluationTDESGuid == sheetrevaluationtdes.SheetRevaluationTDESGuid)
                                                            select e).FirstOrDefault(); incomeArticleTDES != null; incomeArticleTDES = (from e in dbIncome.IncomeArticleTDES
                                                                                                                                        where e.IsProcessed == true && e.IsReversed == false && e.CurrArtPrice != incomearticletdes.CurrArtPrice && e.ArtAmountRest > 0 && e.EntArticle == incomearticletdes.EntArticle && e.EntBrandNic == incomearticletdes.EntBrandNic && e.EntBranchGuid == incomearticletdes.EntBranchGuid && e.EntGuid == incomearticletdes.EntGuid && e.IncomePayRollTDESGuid != incomearticletdes.IncomePayRollTDESGuid && !dbIncome.RevaluationArticleTDES.Any((RevaluationArticleTDES r) => r.EntArticle == e.EntArticle && r.EntBrandNic == e.EntBrandNic && r.IncomePayRollTDESGuid == e.IncomePayRollTDESGuid && r.SheetRevaluationTDESGuid == sheetrevaluationtdes.SheetRevaluationTDESGuid)
                                                                                                                                        select e).FirstOrDefault())
                {
                    RevaluationArticleTDES revaluationArticleTDES = new RevaluationArticleTDES();
                    revaluationArticleTDES.EntArticle = incomearticletdes.EntArticle;
                    revaluationArticleTDES.EntBrandNic = incomearticletdes.EntBrandNic;
                    revaluationArticleTDES.IncomePayRollTDESGuid = incomeArticleTDES.IncomePayRollTDESGuid;
                    revaluationArticleTDES.SheetRevaluationTDESGuid = sheetrevaluationtdes.SheetRevaluationTDESGuid;
                    revaluationArticleTDES.EntArticleDescription = incomeArticleTDES.EntArticleDescription;
                    revaluationArticleTDES.EntBranchGuid = incomearticletdes.EntBranchGuid;
                    revaluationArticleTDES.EntGuid = incomearticletdes.EntGuid;
                    revaluationArticleTDES.IsProcessed = false;
                    revaluationArticleTDES.IsReversed = false;
                    revaluationArticleTDES.CurrArtPrice = incomeArticleTDES.CurrArtPrice;
                    revaluationArticleTDES.NewArtPrice = incomearticletdes.CurrArtPrice;
                    revaluationArticleTDES.ArtAmountRest = incomeArticleTDES.ArtAmountRest;
                    revaluationArticleTDES.OperSum = (incomearticletdes.CurrArtPrice - incomeArticleTDES.CurrArtPrice) * (double)incomeArticleTDES.ArtAmountRest;
                    RevaluationArticleTDES entity = revaluationArticleTDES;
                    dbIncome.RevaluationArticleTDES.Add(entity);
                    try
                    {
                        dbIncome.SaveChanges();
                    }
                    catch
                    {
                        createSheetRevaluationData.ErrorText = "Не могу вставить запись в ведомость переоценки, т.к. ошибка в БД";
                        createSheetRevaluationData.hasError = true;
                        ClearContexts(createSheetRevaluationData);
                        return;
                    }
                    dbIncome.Entry(entity).State = EntityState.Detached;
                    dbIncome.Entry(incomeArticleTDES).State = EntityState.Detached;
                }
                dbIncome.Entry(incomearticletdes).State = EntityState.Detached;
            }
            createSheetRevaluationData.isDone = true;
            dbIncome.Dispose();
        }
    }
}