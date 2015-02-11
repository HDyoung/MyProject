using System;
using System.Net;
using System.IO;
using System.Text;

namespace MakeSugangProgram.Client
{
	/// <summary>
	/// Description of LoginManager.
	/// </summary>
	public class LoginManager
	{
		private CookieContainer cookieContainer = new CookieContainer();
		private string credentials;
		
		public bool Login(string userId, string password)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] postDataBytes = encoding.GetBytes("p_userid="+userId+"&p_passwd="+password);
			
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sugang.hongik.ac.kr/login_ok.jsp");
			request.CookieContainer = new System.Net.CookieContainer();
			
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = postDataBytes.Length;
			request.AllowAutoRedirect = false;
			
			using(Stream stream = request.GetRequestStream())
			{
				stream.Write(postDataBytes, 0, postDataBytes.Length);
				stream.Close();
			}
			
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			
			if(CheckLoginResult(response.Headers)) // 
			{
				foreach (Cookie cookie in response.Cookies)
				{
					cookieContainer.Add(cookie);
				}
				response.Close();
				return true;
			}
			else
				return false;
		 }
		
		private bool CheckLoginResult(WebHeaderCollection headerCollection)
		{					
			var headers = headerCollection.Get("Location");							
			string checkLogin = headers.Substring(38,8);			
		
			if(checkLogin == "p_contm=")
			{
				credentials = headers.Substring(38);				
				return true;
			}
			else
			{			
				return false;
			}
		}
		
		public CookieContainer GetCookie()
		{
			return cookieContainer;
		}
		public string Credentials
		{
			get
			{
				return credentials;
			}
		
		}		
	}
}
