using FTPBuddy.Data;
using FTPBuddy.Models;

namespace FTPBuddy.Pages;

public partial class AddNewServerPage : ContentPage
{
	FtpServerDatabase Database;

	public AddNewServerPage(FtpServerDatabase database)
	{
		InitializeComponent();
		Database = database;
	}

    private async void AddButton_Clicked(object sender, EventArgs e)
    {
		FtpSavedServer server = new FtpSavedServer
		{
			Domain = entryDomain.Text,
			Username = entryUsername.Text,
			Password = entryPassword.Text
		};

		await Database.SaveFtpServer(server);
		await Navigation.PopToRootAsync();
    }
}