using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using MakeSugangProgram.Client;
using System.Windows.Media.Animation;

namespace MakeSugangProgram.Page
{
	/// <summary>
	/// Interaction logic for LoginPage.xaml
	/// </summary>
	public partial class LoginPage : UserControl
	{
		private Canvas window_canvas;
		private DoubleAnimation slideOutAnimation;
		private DoubleAnimation slideInAnimation;
		private BackgroundWorker backgroundWorker;
		private UserLoginData userLoginData;
		private LoginManager loginManager;
				
		internal LoginPage(ref Canvas window_canvas, ref UserLoginData userLoginData, ref LoginManager loginManager)			
		{
			InitializeComponent();
			this.window_canvas = window_canvas;
			this.userLoginData = userLoginData;
			this.loginManager = loginManager;
			
			InitializePageTransitionAnimation();					
		}
		
		private void login_btn_Click(object sender, RoutedEventArgs e)
		{			
			if(login_btn.IsEnabled)
			{
				LoginInBackgroundWorker();
			}
		}
		
		private void LoginInBackgroundWorker()
		{
			using(backgroundWorker = new BackgroundWorker())
			{
				InitializeBackgroundWorker();
				
				if(CheckLoginEnable())
					DoLoginInBackgroundWorker();
			}
		
		}
		private bool CheckLoginEnable()
		{
			if(backgroundWorker.IsBusy != true)
			{
				login_btn.IsEnabled = false;
				return true;
			}
			return false;
		}
		private void DoLoginInBackgroundWorker()
		{
			List<string> arguments = new List<string>();
			arguments.Add(id_textbox.Text);
			arguments.Add(pwd_passwordBox.Password);
			backgroundWorker.RunWorkerAsync(arguments);
		}
		private void id_textbox_Loaded(object sender, RoutedEventArgs e)
		{
			id_textbox.Focus();
		}
		private void InitializeBackgroundWorker()
		{
			backgroundWorker.WorkerSupportsCancellation = false;
			backgroundWorker.DoWork += new DoWorkEventHandler(TryLogin);
			backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompletedLogin);
		}
		private void TryLogin(object sender, DoWorkEventArgs e)
		{
			List<string> arguments = e.Argument as List<string>;
			e.Result = Login(arguments[0], arguments[1]);			
		}
		private bool Login(string userId, string password)
		{
			try
			{
				userLoginData.IsLogined = loginManager.Login(userId, password);
				if(userLoginData.IsLogined)
				{
					userLoginData.userId = userId;
					userLoginData.password = password;
				}
				return userLoginData.IsLogined;
			}
			catch (Exception e)
			{
				return false;
			}
		}
		private void CompletedLogin(object sender, RunWorkerCompletedEventArgs e)
		{
			if(e.Error != null)
			{
				
			}
			else if(e.Cancelled)
			{
				
			}
			else
			{
				if((bool)e.Result)
				{
					PrepareMainPage();
				}
				else
				{
					login_btn.IsEnabled = true;
					MessageBox.Show("로그인 할 수 없습니다", "알림", MessageBoxButton.OK);
				}
			}
		}
		private void PrepareMainPage()
		{
			PageTransition_With_SlideEffect_To_MainPage();
		}
		private void PageTransition_With_SlideEffect_To_MainPage()
		{
			(window_canvas.Children[1] as MainPage).SetNameOfTitle();
			(window_canvas.Children[1] as MainPage).Visibility = Visibility.Visible;
			
			slideOutAnimation.Completed += delegate(object sender, EventArgs e1)
			{
				CleanTextboxes();
				(window_canvas.Children[0] as LoginPage).Visibility = Visibility.Collapsed;
			};
			
			(window_canvas.Children[0] as LoginPage).BeginAnimation(Canvas.LeftProperty, slideOutAnimation);
			(window_canvas.Children[1] as MainPage).BeginAnimation(Canvas.LeftProperty, slideInAnimation);
		}
		private void CleanTextboxes()
		{
			id_textbox.Text = string.Empty;
			pwd_passwordBox.Password = string.Empty;
		}
		private void InitializePageTransitionAnimation()
		{
			Duration duration = new Duration((new TimeSpan(0,0,0,0,600)));
			slideOutAnimation = new DoubleAnimation(0.0, -500, duration, FillBehavior.HoldEnd);
			slideInAnimation = new DoubleAnimation(500, 0.0, duration, FillBehavior.HoldEnd);
		}
		private void Grid_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Return)
				login_btn_Click(sender,e);
		}
	}
}