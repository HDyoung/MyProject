using System;
using System.Windows;
using System.Data;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using MakeSugangProgram.Client;

namespace MakeSugangProgram
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private int exitCode_Normal = 0;
		
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			if(!MutexManager.IsExistMutex())
			{
				RunProgram();
				string context = "10분 전 부터 로그인 해주시길 바랍니다.\n" + "로그인 후 다른 곳에서 로그인하면 안됩니다.\n"
					+"다른곳에서 로그인 했을 시 프로그램을 다시 껏다가 실행시키세요";
				
				MessageBox.Show(context, "알림", MessageBoxButton.OK);
			}
			
			else
			{
				AlreadyRun();
			}
		}
		
		private void RunProgram()
		{
			System.Net.ServicePointManager.DefaultConnectionLimit = 10000000;
			RunMainWindow();
		}
		
		private void AlreadyRun()
		{
			MessageBox.Show("프로그램이 이미 실행중입니다!");
			App.Current.Shutdown(exitCode_Normal);
		}
		
		private void RunMainWindow()
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
		}
	}
}