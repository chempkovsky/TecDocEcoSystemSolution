// CarShop.Areas.HelpPage.XmlDocumentationProvider
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Xml.XPath;

namespace CarShop.Areas.HelpPage
{

    public class XmlDocumentationProvider : IDocumentationProvider
    {
        private const string TypeExpression = "/doc/members/member[@name='T:{0}']";

        private const string MethodExpression = "/doc/members/member[@name='M:{0}']";

        private const string ParameterExpression = "param[@name='{0}']";

        private XPathNavigator _documentNavigator;

        public XmlDocumentationProvider(string documentPath)
        {
            if (documentPath == null)
            {
                throw new ArgumentNullException("documentPath");
            }
            XPathDocument xPathDocument = new XPathDocument(documentPath);
            _documentNavigator = xPathDocument.CreateNavigator();
        }

        public string GetDocumentation(HttpControllerDescriptor controllerDescriptor)
        {
            XPathNavigator typeNode = GetTypeNode(controllerDescriptor);
            return GetTagValue(typeNode, "summary");
        }

        public virtual string GetDocumentation(HttpActionDescriptor actionDescriptor)
        {
            XPathNavigator methodNode = GetMethodNode(actionDescriptor);
            return GetTagValue(methodNode, "summary");
        }

        public virtual string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
        {
            ReflectedHttpParameterDescriptor reflectedHttpParameterDescriptor = parameterDescriptor as ReflectedHttpParameterDescriptor;
            if (reflectedHttpParameterDescriptor != null)
            {
                XPathNavigator methodNode = GetMethodNode(reflectedHttpParameterDescriptor.ActionDescriptor);
                if (methodNode != null)
                {
                    string name = reflectedHttpParameterDescriptor.ParameterInfo.Name;
                    XPathNavigator xPathNavigator = methodNode.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "param[@name='{0}']", new object[1]
                    {
                    name
                    }));
                    if (xPathNavigator != null)
                    {
                        return xPathNavigator.Value.Trim();
                    }
                }
            }
            return null;
        }

        public string GetResponseDocumentation(HttpActionDescriptor actionDescriptor)
        {
            XPathNavigator methodNode = GetMethodNode(actionDescriptor);
            return GetTagValue(methodNode, "returns");
        }

        private XPathNavigator GetMethodNode(HttpActionDescriptor actionDescriptor)
        {
            ReflectedHttpActionDescriptor reflectedHttpActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;
            if (reflectedHttpActionDescriptor != null)
            {
                string xpath = string.Format(CultureInfo.InvariantCulture, "/doc/members/member[@name='M:{0}']", new object[1]
                {
                GetMemberName(reflectedHttpActionDescriptor.MethodInfo)
                });
                return _documentNavigator.SelectSingleNode(xpath);
            }
            return null;
        }

        private static string GetMemberName(MethodInfo method)
        {
            string text = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[2]
            {
            method.DeclaringType.FullName,
            method.Name
            });
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length != 0)
            {
                string[] value = parameters.Select((ParameterInfo param) => GetTypeName(param.ParameterType)).ToArray();
                text += string.Format(CultureInfo.InvariantCulture, "({0})", new object[1]
                {
                string.Join(",", value)
                });
            }
            return text;
        }

        private static string GetTagValue(XPathNavigator parentNode, string tagName)
        {
            if (parentNode != null)
            {
                XPathNavigator xPathNavigator = parentNode.SelectSingleNode(tagName);
                if (xPathNavigator != null)
                {
                    return xPathNavigator.Value.Trim();
                }
            }
            return null;
        }

        private static string GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                Type[] genericArguments = type.GetGenericArguments();
                string fullName = genericTypeDefinition.FullName;
                fullName = fullName.Substring(0, fullName.IndexOf('`'));
                string[] value = genericArguments.Select((Type t) => GetTypeName(t)).ToArray();
                return string.Format(CultureInfo.InvariantCulture, "{0}{{{1}}}", new object[2]
                {
                fullName,
                string.Join(",", value)
                });
            }
            return type.FullName;
        }

        private XPathNavigator GetTypeNode(HttpControllerDescriptor controllerDescriptor)
        {
            Type controllerType = controllerDescriptor.ControllerType;
            string text = controllerType.FullName;
            if (controllerType.IsNested)
            {
                text = text.Replace("+", ".");
            }
            string xpath = string.Format(CultureInfo.InvariantCulture, "/doc/members/member[@name='T:{0}']", new object[1]
            {
            text
            });
            return _documentNavigator.SelectSingleNode(xpath);
        }
    }
}