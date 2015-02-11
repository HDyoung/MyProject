using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace MakeSugangProgram.Client
{
	/// <summary>
	/// Description of UserLoginData.
	/// </summary>
	internal class UserLoginData
	{
		private string _userId;
		private string _password;
		private bool isLogined = false;
		
		internal string userId
		{
			get{return _userId;}
			set{_userId = value;}
		}
		
		internal string password
		{
			get{ return _password;}
			set{_password = value;}
			
		}
		internal bool IsLogined
		{
			get { return isLogined;}
			set { isLogined = value; }
			
		}
		internal void CleanLoginData()
		{
			userId = string.Empty;
			_password = string.Empty;
			isLogined = false;
		}
	}
	
}
