// CarShop.Utility.ReplyOrders
using CarShop.Models;
using CarShop.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web;
using TecDocEcoSystemDbClassLibrary;

namespace CarShop.Utility
{

    public class ReplyOrders
    {
        public static void ClearContexts(ReplyOrdersData aData)
        {
            if (aData.dbOrders != null)
            {
                aData.dbOrders.Dispose();
                aData.dbOrders = null;
            }
        }

        public static void DoReplyOrders(object replyrestsddata)
        {
            ReplyOrdersData replyOrdersData = replyrestsddata as ReplyOrdersData;
            if (replyOrdersData == null)
            {
                return;
            }
            if (replyOrdersData.BranchGuid == Guid.Empty)
            {
                ClearContexts(replyOrdersData);
                return;
            }
            CarShopOrdersContext carShopOrdersContext = replyOrdersData.dbOrders;
            Guid branchGuid = replyOrdersData.BranchGuid;
            HttpClient httpClient = replyOrdersData.httpClient;
            string orderUriString = replyOrdersData.OrderUriString;
            string articleUriString = replyOrdersData.ArticleUriString;
            string profileUriString = replyOrdersData.ProfileUriString;
            string requestUri = orderUriString + "?searchEntBranchGuid=" + HttpUtility.UrlEncode(branchGuid.ToString());
            string text = null;
            string text2 = null;
            GuestOrderArticleTDES guestOrderArticleTDES = null;
            GuestProfileTDES guestProfileTDES = null;
            HttpResponseMessage result = httpClient.GetAsync(requestUri).Result;
            while (result.IsSuccessStatusCode)
            {
                bool flag = false;
                List<GuestOrderTmp> result2 = result.Content.ReadAsAsync<List<GuestOrderTmp>>().Result;
                foreach (GuestOrderTmp guestordertmp in result2)
                {
                    flag = true;
                    GuestOrderTDES guestOrderTDES = (from e in carShopOrdersContext.GuestOrderTDES
                                                     where e.GuestOrderGuid == guestordertmp.GuestOrderGuid
                                                     select e).FirstOrDefault();
                    bool flag2 = guestOrderTDES == null;
                    if (!flag2)
                    {
                        guestordertmp.LastReplicated = guestOrderTDES.LastReplicated;
                        if (guestordertmp.LastReplicated < guestordertmp.LastUpdated)
                        {
                            guestordertmp.LastReplicated = guestordertmp.LastUpdated;
                        }
                    }
                    else
                    {
                        guestProfileTDES = (from e in carShopOrdersContext.GuestProfileTDES
                                            where e.GestUserNic == guestordertmp.GestUserNic
                                            select e).FirstOrDefault();
                        text = profileUriString + "?searchEntBranchGuid=" + HttpUtility.UrlEncode(branchGuid.ToString()) + "&searchGestUserNic=" + HttpUtility.UrlEncode(guestordertmp.GestUserNic);
                        HttpResponseMessage result3 = httpClient.GetAsync(text).Result;
                        if (!result3.IsSuccessStatusCode)
                        {
                            replyOrdersData.hasError = true;
                            replyOrdersData.ErrorText = "Ошибка WebApi запроса GET (" + profileUriString + ") = " + result3.StatusCode.ToString() + " : " + result3.ReasonPhrase;
                            carShopOrdersContext = null;
                            ClearContexts(replyOrdersData);
                            return;
                        }
                        GuestProfileTDES result4 = result3.Content.ReadAsAsync<GuestProfileTDES>().Result;
                        if (guestProfileTDES == null)
                        {
                            guestProfileTDES = result4;
                            carShopOrdersContext.GuestProfileTDES.Add(guestProfileTDES);
                            result4 = null;
                        }
                        else
                        {
                            guestProfileTDES.FirstName = result4.FirstName;
                            guestProfileTDES.LastName = result4.LastName;
                            guestProfileTDES.MiddleName = result4.MiddleName;
                            guestProfileTDES.Contact = result4.Contact;
                            guestProfileTDES.Address = result4.Address;
                        }
                        carShopOrdersContext.SaveChanges();
                        try
                        {
                            carShopOrdersContext.SaveChanges();
                        }
                        catch
                        {
                            replyOrdersData.hasError = true;
                            replyOrdersData.ErrorText = "Ошибка вставки гостевого профиля для = " + guestProfileTDES.GestUserNic;
                            carShopOrdersContext = null;
                            ClearContexts(replyOrdersData);
                            return;
                        }
                        guestordertmp.LastReplicated = DateTime.Now;
                        guestordertmp.LastReplicated = guestordertmp.LastReplicated.AddMilliseconds(-guestordertmp.LastReplicated.Millisecond);
                        if (guestordertmp.LastReplicated < guestordertmp.LastUpdated)
                        {
                            guestordertmp.LastReplicated = guestordertmp.LastUpdated;
                        }
                        guestOrderTDES = new GuestOrderTDES();
                        guestordertmp.CopyTo(guestOrderTDES);
                    }
                    if (flag2)
                    {
                        carShopOrdersContext.GuestOrderTDES.Add(guestOrderTDES);
                        try
                        {
                            carShopOrdersContext.SaveChanges();
                        }
                        catch
                        {
                            replyOrdersData.hasError = true;
                            replyOrdersData.ErrorText = "Ошибка вставки гостевого заказа для = " + guestOrderTDES.GuestOrderGuid.ToString() + " и пользователя " + guestOrderTDES.GestUserNic;
                            carShopOrdersContext = null;
                            ClearContexts(replyOrdersData);
                            return;
                        }
                    }
                    text2 = articleUriString + "?searchEntBranchGuid=" + HttpUtility.UrlEncode(branchGuid.ToString()) + "&searchGuestOrderGuid=" + HttpUtility.UrlEncode(guestOrderTDES.GuestOrderGuid.ToString());
                    HttpResponseMessage result5 = httpClient.GetAsync(text2).Result;
                    if (!result5.IsSuccessStatusCode)
                    {
                        replyOrdersData.hasError = true;
                        replyOrdersData.ErrorText = "Ошибка WebApi запроса GET (" + articleUriString + ") = " + result5.StatusCode.ToString() + " : " + result5.ReasonPhrase;
                        carShopOrdersContext = null;
                        ClearContexts(replyOrdersData);
                        return;
                    }
                    List<GuestOrderArticleTmp> result6 = result5.Content.ReadAsAsync<List<GuestOrderArticleTmp>>().Result;
                    foreach (GuestOrderArticleTmp guestorderarticletmp in result6)
                    {
                        guestOrderArticleTDES = (from e in carShopOrdersContext.GuestOrderArticleTDES
                                                 where e.GuestOrderGuid == guestorderarticletmp.GuestOrderGuid && e.EntBranchArticle == guestorderarticletmp.EntBranchArticle && e.EntBranchSup == guestorderarticletmp.EntBranchSup
                                                 select e).FirstOrDefault();
                        if (guestOrderArticleTDES == null)
                        {
                            guestOrderArticleTDES = new GuestOrderArticleTDES();
                            guestorderarticletmp.CopyTo(guestOrderArticleTDES);
                            guestOrderArticleTDES.LastReplicated = DateTime.Now;
                            guestOrderArticleTDES.LastReplicated = guestOrderArticleTDES.LastReplicated.AddMilliseconds(-guestOrderArticleTDES.LastReplicated.Millisecond);
                            if (guestOrderArticleTDES.LastReplicated < guestOrderArticleTDES.LastUpdated)
                            {
                                guestOrderArticleTDES.LastReplicated = guestOrderArticleTDES.LastUpdated;
                            }
                            carShopOrdersContext.GuestOrderArticleTDES.Add(guestOrderArticleTDES);
                            try
                            {
                                carShopOrdersContext.SaveChanges();
                                carShopOrdersContext.Entry(guestOrderArticleTDES).State = EntityState.Detached;
                            }
                            catch
                            {
                                replyOrdersData.hasError = true;
                                replyOrdersData.ErrorText = "Ошибка вставки гостевого артикула для = " + guestOrderTDES.GuestOrderGuid.ToString() + " и пользователя " + guestOrderTDES.GestUserNic + ":" + guestorderarticletmp.EntBranchArticle + ":" + guestorderarticletmp.EntBranchSup;
                                carShopOrdersContext = null;
                                ClearContexts(replyOrdersData);
                                return;
                            }
                        }
                        carShopOrdersContext.Entry(guestOrderArticleTDES).State = EntityState.Detached;
                        guestorderarticletmp.LastReplicated = guestOrderArticleTDES.LastReplicated;
                        if (guestorderarticletmp.LastReplicated < guestorderarticletmp.LastUpdated)
                        {
                            guestorderarticletmp.LastReplicated = guestorderarticletmp.LastUpdated;
                        }
                        text2 = articleUriString + "?searchEntBranchGuid=" + HttpUtility.UrlEncode(branchGuid.ToString()) + "&searchGuestOrderGuid=" + HttpUtility.UrlEncode(guestOrderTDES.GuestOrderGuid.ToString()) + "&searchEntBranchArticle=" + HttpUtility.UrlEncode(guestorderarticletmp.EntBranchArticle) + "&searchEntBranchSup=" + HttpUtility.UrlEncode(guestorderarticletmp.EntBranchSup);
                        HttpResponseMessage result7 = httpClient.PutAsXmlAsync(text2, guestorderarticletmp).Result;
                        if (!result7.IsSuccessStatusCode)
                        {
                            replyOrdersData.hasError = true;
                            replyOrdersData.ErrorText = "Ошибка WebApi запроса PUT (" + articleUriString + ") = " + result7.StatusCode.ToString() + " : " + result7.ReasonPhrase;
                            carShopOrdersContext = null;
                            ClearContexts(replyOrdersData);
                            return;
                        }
                    }
                    carShopOrdersContext.Entry(guestOrderTDES).State = EntityState.Detached;
                    if (guestProfileTDES != null)
                    {
                        carShopOrdersContext.Entry(guestProfileTDES).State = EntityState.Detached;
                        guestProfileTDES = null;
                    }
                    string requestUri2 = orderUriString + "?searchEntBranchGuid=" + HttpUtility.UrlEncode(branchGuid.ToString()) + "&searchGuestOrderGuid=" + HttpUtility.UrlEncode(guestordertmp.GuestOrderGuid.ToString());
                    HttpResponseMessage result8 = httpClient.PutAsXmlAsync(requestUri2, guestordertmp).Result;
                    if (!result8.IsSuccessStatusCode)
                    {
                        replyOrdersData.hasError = true;
                        replyOrdersData.ErrorText = "Ошибка WebApi запроса PUT (" + orderUriString + ") = " + result8.StatusCode.ToString() + " : " + result8.ReasonPhrase;
                        carShopOrdersContext = null;
                        ClearContexts(replyOrdersData);
                        return;
                    }
                }
                if (!flag)
                {
                    break;
                }
                result = httpClient.GetAsync(requestUri).Result;
            }
            if (!result.IsSuccessStatusCode)
            {
                replyOrdersData.hasError = true;
                replyOrdersData.ErrorText = "Ошибка WebApi запроса GET (" + orderUriString + ") = " + result.StatusCode.ToString() + " : " + result.ReasonPhrase;
                carShopOrdersContext = null;
                ClearContexts(replyOrdersData);
            }
            else
            {
                replyOrdersData.isDone = true;
                replyOrdersData.hasError = false;
                replyOrdersData.ErrorText = "Репликация стола заказов проведена для  = " + branchGuid.ToString();
                carShopOrdersContext = null;
                ClearContexts(replyOrdersData);
            }
        }
    }
}