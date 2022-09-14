// CarShop.Utility.ReplyRests
using CarShop.Models;
using CarShop.Utility;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Utility
{

    public class ReplyRests
    {
        public static void ClearContexts(ReplyRestsdData aData)
        {
            if (aData.dbRest != null)
            {
                aData.dbRest.Dispose();
                aData.dbRest = null;
            }
        }

        public static void DoReplyRests(object replyrestsddata)
        {
            ReplyRestsdData replyRestsdData = replyrestsddata as ReplyRestsdData;
            if (replyRestsdData == null)
            {
                return;
            }
            if (replyRestsdData.BranchGuid == Guid.Empty)
            {
                ClearContexts(replyRestsdData);
                return;
            }
            CarShopRestContext dbRest = replyRestsdData.dbRest;
            Guid BranchGuid = replyRestsdData.BranchGuid;
            HttpClient httpClient = replyRestsdData.httpClient;
            string postUriString = replyRestsdData.PostUriString;
            BranchRestTmp branchRestTmp = new BranchRestTmp();
            for (BranchRestTDES branchRestTDES = (from e in dbRest.BranchRestTDES.Include("BranchRestArticleDescriptionTDES")
                                                  where e.EntBranchGuid == BranchGuid && e.IsReplicated > 0
                                                  select e).FirstOrDefault(); branchRestTDES != null; branchRestTDES = (from e in dbRest.BranchRestTDES.Include("BranchRestArticleDescriptionTDES")
                                                                                                                        where e.EntBranchGuid == BranchGuid && e.IsReplicated > 0
                                                                                                                        select e).FirstOrDefault())
            {
                branchRestTDES.LastReplicated = branchRestTDES.LastUpdated;
                branchRestTmp.CopyFrom(branchRestTDES);
                HttpResponseMessage result = httpClient.PostAsXmlAsync(postUriString, branchRestTmp).Result;
                if (!result.IsSuccessStatusCode)
                {
                    replyRestsdData.hasError = true;
                    replyRestsdData.ErrorText = "Ошибка WebApi запроса = " + result.StatusCode.ToString() + " : " + result.ReasonPhrase;
                    dbRest = null;
                    ClearContexts(replyRestsdData);
                    return;
                }
                try
                {
                    dbRest.SaveChanges();
                }
                catch
                {
                    replyRestsdData.hasError = true;
                    replyRestsdData.ErrorText = "Ошибка обновления статуса остатков для = " + branchRestTDES.EntBranchArticle + " : " + branchRestTDES.EntBranchSup;
                    dbRest = null;
                    ClearContexts(replyRestsdData);
                    return;
                }
                dbRest.Entry(branchRestTDES.BranchRestArticleDescriptionTDES).State = EntityState.Detached;
                dbRest.Entry(branchRestTDES).State = EntityState.Detached;
            }
            replyRestsdData.isDone = true;
            replyRestsdData.hasError = false;
            replyRestsdData.ErrorText = "Репликация проведена для  = " + BranchGuid.ToString();
            dbRest = null;
            ClearContexts(replyRestsdData);
        }
    }
}