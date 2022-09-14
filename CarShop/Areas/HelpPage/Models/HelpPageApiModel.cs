// CarShop.Areas.HelpPage.Models.HelpPageApiModel
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Web.Http.Description;

namespace CarShop.Areas.HelpPage.Models
{

    public class HelpPageApiModel
    {
        public ApiDescription ApiDescription
        {
            get;
            set;
        }

        public IDictionary<MediaTypeHeaderValue, object> SampleRequests
        {
            get;
            private set;
        }

        public IDictionary<MediaTypeHeaderValue, object> SampleResponses
        {
            get;
            private set;
        }

        public Collection<string> ErrorMessages
        {
            get;
            private set;
        }

        public HelpPageApiModel()
        {
            SampleRequests = new Dictionary<MediaTypeHeaderValue, object>();
            SampleResponses = new Dictionary<MediaTypeHeaderValue, object>();
            ErrorMessages = new Collection<string>();
        }
    }
}