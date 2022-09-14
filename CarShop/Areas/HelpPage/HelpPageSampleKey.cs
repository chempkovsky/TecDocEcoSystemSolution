// CarShop.Areas.HelpPage.HelpPageSampleKey
using CarShop.Areas.HelpPage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;

namespace CarShop.Areas.HelpPage
{

    public class HelpPageSampleKey
    {
        public string ControllerName
        {
            get;
            private set;
        }

        public string ActionName
        {
            get;
            private set;
        }

        public MediaTypeHeaderValue MediaType
        {
            get;
            private set;
        }

        public HashSet<string> ParameterNames
        {
            get;
            private set;
        }

        public Type ParameterType
        {
            get;
            private set;
        }

        public SampleDirection? SampleDirection
        {
            get;
            private set;
        }

        public HelpPageSampleKey(MediaTypeHeaderValue mediaType, Type type)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            ControllerName = string.Empty;
            ActionName = string.Empty;
            ParameterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            ParameterType = type;
            MediaType = mediaType;
        }

        public HelpPageSampleKey(SampleDirection sampleDirection, string controllerName, string actionName, IEnumerable<string> parameterNames)
        {
            if (!Enum.IsDefined(typeof(SampleDirection), sampleDirection))
            {
                throw new InvalidEnumArgumentException("sampleDirection", (int)sampleDirection, typeof(SampleDirection));
            }
            if (controllerName == null)
            {
                throw new ArgumentNullException("controllerName");
            }
            if (actionName == null)
            {
                throw new ArgumentNullException("actionName");
            }
            if (parameterNames == null)
            {
                throw new ArgumentNullException("parameterNames");
            }
            ControllerName = controllerName;
            ActionName = actionName;
            ParameterNames = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
            SampleDirection = sampleDirection;
        }

        public HelpPageSampleKey(MediaTypeHeaderValue mediaType, SampleDirection sampleDirection, string controllerName, string actionName, IEnumerable<string> parameterNames)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType");
            }
            if (!Enum.IsDefined(typeof(SampleDirection), sampleDirection))
            {
                throw new InvalidEnumArgumentException("sampleDirection", (int)sampleDirection, typeof(SampleDirection));
            }
            if (controllerName == null)
            {
                throw new ArgumentNullException("controllerName");
            }
            if (actionName == null)
            {
                throw new ArgumentNullException("actionName");
            }
            if (parameterNames == null)
            {
                throw new ArgumentNullException("parameterNames");
            }
            ControllerName = controllerName;
            ActionName = actionName;
            MediaType = mediaType;
            ParameterNames = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
            SampleDirection = sampleDirection;
        }

        public override bool Equals(object obj)
        {
            HelpPageSampleKey helpPageSampleKey = obj as HelpPageSampleKey;
            if (helpPageSampleKey == null)
            {
                return false;
            }
            if (string.Equals(ControllerName, helpPageSampleKey.ControllerName, StringComparison.OrdinalIgnoreCase) && string.Equals(ActionName, helpPageSampleKey.ActionName, StringComparison.OrdinalIgnoreCase) && (MediaType == helpPageSampleKey.MediaType || (MediaType != null && MediaType.Equals(helpPageSampleKey.MediaType))) && ParameterType == helpPageSampleKey.ParameterType && SampleDirection == helpPageSampleKey.SampleDirection)
            {
                return ParameterNames.SetEquals(helpPageSampleKey.ParameterNames);
            }
            return false;
        }

        public override int GetHashCode()
        {
            int num = ControllerName.ToUpperInvariant().GetHashCode() ^ ActionName.ToUpperInvariant().GetHashCode();
            if (MediaType != null)
            {
                num ^= MediaType.GetHashCode();
            }
            if (SampleDirection.HasValue)
            {
                num ^= SampleDirection.GetHashCode();
            }
            if (ParameterType != null)
            {
                num ^= ParameterType.GetHashCode();
            }
            foreach (string parameterName in ParameterNames)
            {
                num ^= parameterName.ToUpperInvariant().GetHashCode();
            }
            return num;
        }
    }
}