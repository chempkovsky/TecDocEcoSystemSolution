// CarShop.Utility.ReplyOrdersData
using CarShop.Models;
using System;
using System.Net.Http;

namespace CarShop.Utility
{

    public class ReplyOrdersData
    {
        public CarShopOrdersContext dbOrders
        {
            get;
            set;
        }

        public Guid BranchGuid
        {
            get;
            set;
        }

        public HttpClient httpClient
        {
            get;
            set;
        }

        public string OrderUriString
        {
            get;
            set;
        }

        public string ArticleUriString
        {
            get;
            set;
        }

        public string ProfileUriString
        {
            get;
            set;
        }

        public bool hasError
        {
            get;
            set;
        }

        public string ErrorText
        {
            get;
            set;
        }

        public bool isDone
        {
            get;
            set;
        }
    }
}