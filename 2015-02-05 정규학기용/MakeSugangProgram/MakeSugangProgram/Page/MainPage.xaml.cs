using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.ComponentModel;
using MakeSugangProgram.Client;
using System.Threading;

namespace MakeSugangProgram.Page
{
	public class Subject
	{
		public string NameOfSubject {get; set;}
		public string Haksu { get; set;}
		public string Bunban {get; set;}
		public string Chk {get; set;}
	}
	public partial class MainPage : UserControl
	{
		private UserLoginData userLoginData;
		private Canvas window_canvas;
		private List<Subject> subjects;
		private LoginManager loginManager;
		
		private string numberOfsubjects;
		private string perfectData;
		
		internal MainPage(ref Canvas window_canvas, ref UserLoginData userLoginData, ref LoginManager loginManager)
		{
			InitializeComponent();
			this.window_canvas = window_canvas;
			this.userLoginData = userLoginData;
			this.loginManager = loginManager;
			subjects = new List<Subject>();
			perfectData = "";
		}
		public void SetNameOfTitle()
		{
			title_label.Content = userLoginData.userId + " 님 환영합니다";
			subject_textbox.Focus();
		}
		private void Add_btn_Click(object sender, RoutedEventArgs e)
		{
			Subject sub = new Subject();
			sub.Chk = (subjects.Count + 1).ToString();
			sub.Haksu = haksu_textbox.Text;
			sub.Bunban = bunban_textbox.Text;
			sub.NameOfSubject = subject_textbox.Text;
			subjects.Add(sub);
			
			list_table.ItemsSource = null;
			list_table.ItemsSource = subjects;
			
			ClearContentTextbox();
			subject_textbox.Focus();
		}
		private void ClearContentTextbox()
		{
			subject_textbox.Clear();
			bunban_textbox.Clear();
			haksu_textbox.Clear();
		}
		private void Grid_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Return)
				Add_btn_Click(sender, e);
		}
		private void delete_btn_Click(object sender, RoutedEventArgs e)
		{
			int pivot = list_table.SelectedIndex;
			if(pivot != -1)
			{
				subjects.RemoveAt(list_table.SelectedIndex);
				foreach (Subject subject in subjects)
				{
					if(int.Parse(subject.Chk) > pivot+1)
					{
						subject.Chk = (int.Parse(subject.Chk) -1).ToString();
					}
				}
				list_table.Items.Refresh();
			}
		}
		private void manual_btn_Click(object sender, RoutedEventArgs e)
		{
			Window manual_wnd  = new Window();
			
			manual_wnd.Title = "설명서";
			
			manual_wnd.Content = "클래스넷에 들어가 수강신청메뉴에 들어가면 열리는 수강과목들을 볼 수 있습니다 \n"
				+"시간표를 보면 개설학년, 주관학과 .. 비고 까지 나온 테이블이 보일 것입니다. \n\n"
				+"여기서 필요한건 학수번호입니다. 예를 들어 신청하려는 과목이 문학과 인생이고\n"
				+"학수번호가 002011-001이라면 학수번호란에 002011 분반란에 1이라고 입력하면 됩니다."
				+"\n\n 분반은 001이런식으로 학수번 뒤에 따라붙어있는데 입력할때는 앞에 00은 생략하고 1이라고만\n"
				+"입력하면 됩니다.\n\n"
				+"준비가 끝났으면 옆에 완료버튼을 누르고 서버시간을 알아서 보며 대기하면 됩니다 ^^\n"
				+"8시59분55초가 됐을시 옆에 수강신청 버튼을 누르면 끝입니다."
				+"\n\n 아 ! 재수강 과목은 안됩니다. 초수강 과목만 담을것 !!!!!\n\n"
				+"주의사항: 중복 로그인 안되므로 다른곳에서 로그인을 하지말것 !!"
				+"과목명은 편의를 위해서 \n만들어놓음 입력하기 싫으면 안해도 됨 대신 학수번호와 분반은 정확히 입력필요 !!"
				+"\n\n alert('[경고] 313103-1 기업법 동일과목 또는 대체과목을 이미 신청하셨습니다.');라 뜨면 성공하신겁니다"
				+"\n 사람이 많이 몰려 결과가 나올 때 까지 오래 걸릴 수 있으니 느긋하게 기다리세요";
			manual_wnd.Width = 600;
			manual_wnd.Height = 380;
			manual_wnd.Show();
			
		}
		private void apply_btn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				for(int i=0; i < 15; i++)
				{
					if(i == 10)
						SendToServer.GetData(SendToServer.SendData(loginManager.GetCookie(), loginManager.Credentials, perfectData, numberOfsubjects));
					else
						SendToServer.SendData(loginManager.GetCookie(), loginManager.Credentials, perfectData, numberOfsubjects);
					Thread.Sleep(500);
					
				}
				var fs = new StreamReader("result.txt");
				MessageBox.Show(fs.ReadToEnd(), "수강신청 결과", MessageBoxButton.OK);
				fs.Close();
			}
			catch(Exception error)
			{
				MessageBox.Show(error.ToString(), "전송 에러", MessageBoxButton.OK);
			}
		}
		private void check_btn_Click(object sender, RoutedEventArgs e)
		{
			foreach (Subject subject in subjects)
			{
				perfectData = perfectData + SendToServer.MakeData(subject.Haksu, subject.Bunban, subject.Chk);
			}
			numberOfsubjects = subjects.Count.ToString();
			check_btn.Visibility = Visibility.Collapsed;
			apply_btn.Visibility = Visibility.Visible;
			delete_btn.Visibility = Visibility.Collapsed;
			Add_btn.Visibility = Visibility.Collapsed;
			cancle_btn.Visibility = Visibility.Visible;
		}
		private void cancle_btn_Click(object sender, RoutedEventArgs e)
		{
			check_btn.Visibility = Visibility.Visible;
			apply_btn.Visibility = Visibility.Collapsed;
			delete_btn.Visibility = Visibility.Visible;
			Add_btn.Visibility = Visibility.Visible;
			cancle_btn.Visibility = Visibility.Collapsed;
			
			perfectData = "";
		}
	}
}