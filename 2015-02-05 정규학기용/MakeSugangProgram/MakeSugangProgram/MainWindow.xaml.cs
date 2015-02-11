using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MakeSugangProgram.Client;
using MakeSugangProgram.Page;

namespace MakeSugangProgram
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			UserLoginData userLoginData = new UserLoginData();
			LoginManager loginManager = new LoginManager();
			
			LoginPage loginPage = new LoginPage(ref window_canvas, ref userLoginData, ref loginManager);
			window_canvas.Children.Add(loginPage);
			(window_canvas.Children[0] as LoginPage).Visibility = Visibility.Visible;
			
			MainPage mainPage = new MainPage(ref window_canvas, ref userLoginData, ref loginManager);
			window_canvas.Children.Add(mainPage);
			(window_canvas.Children[1] as MainPage).Visibility = Visibility.Collapsed;
		}
		
		private void close_btn_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				this.DragMove();
			}
			catch(Exception error)
			{
				
			}
		}
	}
}