using FTPBuddy.Data;
using FTPBuddy.Pages;

namespace FTPBuddy
{
    public partial class App : Application
    {
        FtpServerDatabase Database = new FtpServerDatabase();

        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Light;

            MainPage = new NavigationPage(new ServersPage(Database))
            {
                BarBackgroundColor = Color.FromHex("#136ebd"),
                BarTextColor = Colors.White,
            };
        }
    }
}
