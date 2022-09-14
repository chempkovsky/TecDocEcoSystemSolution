// CarShop.Utility.ReplyRestsdData
using CarShop.Models;
using System;
using System.Net.Http;

namespace CarShop.Utility
{

    public class ReplyRestsdData
    {
        public CarShopRestContext dbRest
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

        public string PostUriString
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