using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;

using Newtonsoft.Json;

using ApplicationBoilerplate.Events;

namespace MVCBootstrap.Web.Events {

	public class AsyncWebTask : AsyncTask {
		private readonly IAsyncConfiguration configuration;

		public AsyncWebTask(IAsyncConfiguration configuration)
			: base() {
			this.configuration = configuration;
		}

		public override void Execute(String key, Object data) {
			String site = this.configuration.SiteUrl();
			String endPoint = this.configuration.EndPoint();

			String listener = this.GetListener(key);
			// Get the configuration, we need the site URL.
			//String site = this.configuration.SiteUrl();
			if (!site.EndsWith("/")) {
				site += "/";
			}
			//String endPoint = this.configuration.EndPoint();
			if (endPoint.StartsWith("/")) {
				endPoint = endPoint.Substring(1);
			}
			Uri actionURL = new Uri(String.Format("{0}{1}", site, endPoint));

			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);

			JsonSerializer seri = new JsonSerializer();
			seri.PreserveReferencesHandling = PreserveReferencesHandling.None;
			seri.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			//seri.ReferenceResolver = new defa
			seri.Serialize(new JsonTextWriter(sw), data);
			AsyncWebTask.Post(new Dictionary<String, Object> { { "listener", listener }, { "data", sb.ToString() }, { "type", data.GetType().FullName }, { "assembly", data.GetType().Assembly.FullName } }, actionURL);
		}

		private static String BuildSet(String key, String value) {
			return String.Format("{0}={1}&", key, HttpUtility.UrlEncode(value));
		}

		private static String BuildParameterString(Dictionary<String, Object> parameters) {
			String output = String.Empty;
			foreach (KeyValuePair<string, object> pair in parameters) {
				if (pair.Value == null) {
					output += BuildSet(pair.Key, "");
				}
				else if (pair.Value is String) {
					output += BuildSet(pair.Key, (String)pair.Value);
				}
				else if (pair.Value is Int32) {
					output += BuildSet(pair.Key, ((Int32)pair.Value).ToString());
				}
				else if (pair.Value is Int32) {
					output += BuildSet(pair.Key, ((Int32)pair.Value).ToString());
				}
				else if (pair.Value is Enum) {
					output += BuildSet(pair.Key, ((Enum)pair.Value).ToString());
				}
				else if (pair.Value.GetType().IsGenericType && pair.Value is List<String>) {
					foreach (String value in (List<String>)pair.Value) {
						output += BuildSet(pair.Key, value);
					}
				}
			}

			return (String.IsNullOrEmpty(output) ? "" : output.Substring(0, output.Length - 1));
		}

		/// <summary>
		/// Post the parameters to the given URI.
		/// </summary>
		/// <param name="parameters">Parameters that needs to be posted.</param>
		/// <param name="uri">The recipient of the POST.</param>
		public static String Post(Dictionary<String, Object> parameters, Uri uri) {
			// We're using UTF8 to support all characters/letters!
			UTF8Encoding encoding = new UTF8Encoding();
			String postData = String.Empty;
			// Any parameters?
			if (parameters != null && parameters.Count > 0) {
				// Yes, let's build the parameter string then!
				postData = BuildParameterString(parameters);
			}

			// We need the parameters as a byte array.
			Byte[] data = encoding.GetBytes(postData);

			// Create the web request
			WebRequest request = (WebRequest)WebRequest.Create(uri);
			// We're posting!
			request.Method = "POST";
			// Let's set a high timeout, the site might be busy!
			request.Timeout = 30000;
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = data.Length;
			// Get the stream!
			Stream stream = request.GetRequestStream();
			// Write the data to the stream!
			stream.Write(data, 0, data.Length);
			// Close the stream, we're ready!
			stream.Close();

			String output = String.Empty;
			try {
				using (WebResponse response = (WebResponse)request.GetResponse()) {
					// Any response from the server, get it!
					using (Stream readStream = response.GetResponseStream()) {
						StreamReader responseStream = new StreamReader(readStream, Encoding.UTF8);

						output = responseStream.ReadToEnd();

						// We're done, close the response stream!
						responseStream.Close();
					}
				}
			}
			catch (Exception ex) {
				// TODO:
			}
			return output;
		}
	}
}