// CarShop.Areas.HelpPage.HelpPageConfigurationExtensions
using CarShop.Areas.HelpPage;
using CarShop.Areas.HelpPage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;

namespace CarShop.Areas.HelpPage
{

    public static class HelpPageConfigurationExtensions
    {
        private const string ApiModelPrefix = "MS_HelpPageApiModel_";

        public static void SetDocumentationProvider(this HttpConfiguration config, IDocumentationProvider documentationProvider)
        {
            config.Services.Replace(typeof(IDocumentationProvider), documentationProvider);
        }

        public static void SetSampleObjects(this HttpConfiguration config, IDictionary<Type, object> sampleObjects)
        {
            config.GetHelpPageSampleGenerator().SampleObjects = sampleObjects;
        }

        public static void SetSampleRequest(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Request, controllerName, actionName, new string[1]
            {
            "*"
            }), sample);
        }

        public static void SetSampleRequest(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Request, controllerName, actionName, parameterNames), sample);
        }

        public static void SetSampleResponse(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Response, controllerName, actionName, new string[1]
            {
            "*"
            }), sample);
        }

        public static void SetSampleResponse(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Response, controllerName, actionName, parameterNames), sample);
        }

        public static void SetSampleForType(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, Type type)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, type), sample);
        }

        public static void SetActualRequestType(this HttpConfiguration config, Type type, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Request, controllerName, actionName, new string[1]
            {
            "*"
            }), type);
        }

        public static void SetActualRequestType(this HttpConfiguration config, Type type, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Request, controllerName, actionName, parameterNames), type);
        }

        public static void SetActualResponseType(this HttpConfiguration config, Type type, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Response, controllerName, actionName, new string[1]
            {
            "*"
            }), type);
        }

        public static void SetActualResponseType(this HttpConfiguration config, Type type, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Response, controllerName, actionName, parameterNames), type);
        }

        public static HelpPageSampleGenerator GetHelpPageSampleGenerator(this HttpConfiguration config)
        {
            return (HelpPageSampleGenerator)config.Properties.GetOrAdd(typeof(HelpPageSampleGenerator), (object k) => new HelpPageSampleGenerator());
        }

        public static void SetHelpPageSampleGenerator(this HttpConfiguration config, HelpPageSampleGenerator sampleGenerator)
        {
            config.Properties.AddOrUpdate(typeof(HelpPageSampleGenerator), (object k) => sampleGenerator, (object k, object o) => sampleGenerator);
        }

        public static HelpPageApiModel GetHelpPageApiModel(this HttpConfiguration config, string apiDescriptionId)
        {
            string key = "MS_HelpPageApiModel_" + apiDescriptionId;
            if (!config.Properties.TryGetValue(key, out object value))
            {
                Collection<ApiDescription> apiDescriptions = config.Services.GetApiExplorer().ApiDescriptions;
                ApiDescription apiDescription = apiDescriptions.FirstOrDefault((ApiDescription api) => string.Equals(api.GetFriendlyId(), apiDescriptionId, StringComparison.OrdinalIgnoreCase));
                if (apiDescription != null)
                {
                    HelpPageSampleGenerator helpPageSampleGenerator = config.GetHelpPageSampleGenerator();
                    value = GenerateApiModel(apiDescription, helpPageSampleGenerator);
                    config.Properties.TryAdd(key, value);
                }
            }
            return (HelpPageApiModel)value;
        }

        private static HelpPageApiModel GenerateApiModel(ApiDescription apiDescription, HelpPageSampleGenerator sampleGenerator)
        {
            HelpPageApiModel helpPageApiModel = new HelpPageApiModel();
            helpPageApiModel.ApiDescription = apiDescription;
            try
            {
                foreach (KeyValuePair<MediaTypeHeaderValue, object> sampleRequest in sampleGenerator.GetSampleRequests(apiDescription))
                {
                    helpPageApiModel.SampleRequests.Add(sampleRequest.Key, sampleRequest.Value);
                    LogInvalidSampleAsError(helpPageApiModel, sampleRequest.Value);
                }
                foreach (KeyValuePair<MediaTypeHeaderValue, object> sampleResponse in sampleGenerator.GetSampleResponses(apiDescription))
                {
                    helpPageApiModel.SampleResponses.Add(sampleResponse.Key, sampleResponse.Value);
                    LogInvalidSampleAsError(helpPageApiModel, sampleResponse.Value);
                }
                return helpPageApiModel;
            }
            catch (Exception ex)
            {
                helpPageApiModel.ErrorMessages.Add(string.Format(CultureInfo.CurrentCulture, "An exception has occurred while generating the sample. Exception Message: {0}", new object[1]
                {
                ex.Message
                }));
                return helpPageApiModel;
            }
        }

        private static void LogInvalidSampleAsError(HelpPageApiModel apiModel, object sample)
        {
            InvalidSample invalidSample = sample as InvalidSample;
            if (invalidSample != null)
            {
                apiModel.ErrorMessages.Add(invalidSample.ErrorMessage);
            }
        }
    }
}