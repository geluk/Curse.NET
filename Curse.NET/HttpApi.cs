using System.IO;
using System.Net;
using Curse.NET.Model;
using Newtonsoft.Json;

namespace Curse.NET
{
	internal class HttpApi
	{
		public string AuthToken { get; set; }


		public T Post<T>(string url, string payload)
		{
			var rq = WebRequest.CreateHttp(url);
			rq.Method = "POST";
			rq.ContentType = "application/x-www-form-urlencoded";
			rq.CookieContainer = new CookieContainer();
			using (var writer = new StreamWriter(rq.GetRequestStream()))
			{
				writer.Write(payload);
			}
			var response = rq.GetResponse();
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
			}
		}


		public string Post(string url, RequestObject obj)
		{
			var rq = WebRequest.CreateHttp(url);
			rq.Method = "POST";
			rq.ContentType = "application/json";
			rq.Headers["AuthenticationToken"] = AuthToken;
			using (var writer = new StreamWriter(rq.GetRequestStream()))
			{
				var text = JsonConvert.SerializeObject(obj);
				writer.Write(text);
			}
			var response = rq.GetResponse();
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				return reader.ReadToEnd();
			}
		}

		public T Post<T>(string url, RequestObject obj)
		{
			return JsonConvert.DeserializeObject<T>(Post(url, obj));
		}

		public T Get<T>(string url)
		{
			return JsonConvert.DeserializeObject<T>(Get(url));
		}

		public string Get(string url)
		{
			var rq = WebRequest.CreateHttp(url);
			rq.Method = "GET";
			rq.ContentType = "application/json";
			rq.Headers["AuthenticationToken"] = AuthToken;
			var response = rq.GetResponse();
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				var responseText = reader.ReadToEnd();
				return responseText;
			}
		}

		public void Delete(string url)
		{
			var rq = WebRequest.CreateHttp(url);
			rq.Method = "DELETE";
			rq.Headers["AuthenticationToken"] = AuthToken;
			var response = rq.GetResponse();
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				var responseText = reader.ReadToEnd();
				;
				//return responseText;
			}
		}
	}


	internal class HttpResponseObject
	{
		public HttpResponseObject(string responseText, HttpWebResponse response)
		{
			ResponseText = responseText;
			Response = response;
		}

		public HttpWebResponse Response { get; }
		public string ResponseText { get; }
	}
}
