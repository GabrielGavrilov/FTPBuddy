using FluentFTP;
using FTPBuddy.Data;
using FTPBuddy.Models;

namespace FTPBuddy.Pages;

public partial class ServersPage : ContentPage
{
	FtpServerDatabase Database;

	public ServersPage(FtpServerDatabase database)
	{
		InitializeComponent();
        Database = database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ServersListView.ItemsSource = await Database.GetAllFtpServers();
    }

    private async void AddServerToolbarItem_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddNewServerPage(Database));
    }

    private async void ServersListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        FtpSavedServer server = (FtpSavedServer)e.Item;
        var action = await DisplayActionSheet(server.Domain, "Cancel", null, "Connect", "Delete");
    
        switch(action)
        {
            case "Connect":
                FtpClient client = new FtpClient(server.Domain, server.Username, server.Password);
                await Navigation.PushAsync(new ServerConnectionPage(client));
                break;
            case "Delete":
                await Database.DeleteFtpServer(server);
                ServersListView.ItemsSource = await Database.GetAllFtpServers();
                break;
        }
    
    }
}