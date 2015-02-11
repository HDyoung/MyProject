using System.Net;
using System.IO;
using System.Text;
using System;
using System.Windows;

namespace MakeSugangProgram.Client
{
	/// <summary>
	///정규학기 수강신청 때 바꿔야할 부분은 주소. cn1333이 몇으로 바뀔지 모른다. 1303으로바
	/// </summary>
	public class SendToServer
	{				
		static public HttpWebRequest SendData(CookieContainer cookie, string url_credentials, string data, string num)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();
						
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sugang.hongik.ac.kr/cn1303.jsp");
			byte[] postDatas = encoding.GetBytes(data+"chkcnt="+num+"&"+url_credentials);
			
			request.Method ="POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = postDatas.Length;
			request.CookieContainer = cookie;
			request.Referer = "http://sugang.hongik.ac.kr/cn1302.jsp?"+url_credentials+"&campus=1&gubun=1";
			request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			
			using (Stream stream = request.GetRequestStream())
			{
				stream.Write(postDatas,0,postDatas.Length);
				stream.Close();
			}	
			
			return request;
		}
		static public void GetData(HttpWebRequest request)
		{
			HttpWebResponse response;
			response = (HttpWebResponse)request.GetResponse();
			var result = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("EUC-KR")).ReadToEnd();
			
			string code = response.StatusCode.ToString();
			if(code == "OK")
			{
				var fs = new StreamWriter("result.txt");
				fs.WriteLine(result);
				fs.Close();
			}
			else
			{
				MessageBox.Show("응답 없음", "전송 에러", MessageBoxButton.OK);
			}
		}
		static public string MakeData(string haksu, string bunban,string chk)
		{
			string _haksu = "haksu="+haksu+"&";
			string _bunban = "bunban="+bunban+"&";
			string _chk = "chk="+chk+"&";
			string data = _haksu+_bunban+_chk;
			
			return data;
		}
	}
}
