using FluentFTP;
using FTPBuddy.Models;
using Microsoft.Maui.Controls;

namespace FTPBuddy.Pages;

public partial class ServerConnectionPage : ContentPage
{
	FtpClient Client;

	public ServerConnectionPage(FtpClient client)
	{
		InitializeComponent();

		Client = client;
		Client.AutoConnect();
		Title = String.Format("{0}@{1}", Client.Credentials.UserName, Client.Host);

		ServerDirectoryListView.ItemsSource = GetFilesInFtpDirectory("/");
		LocalDirectoryListView.ItemsSource = GetFilesInMobileDirectory("/storage/emulated/0");
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
	}

	private List<MobileFileDirectoryModel> GetFilesInMobileDirectory(string directory)
	{
		List<MobileFileDirectoryModel> filesInDirectory = new List<MobileFileDirectoryModel>();
		DirectoryInfo dir = new DirectoryInfo(directory);
		LocalDirectoryEntry.Text = dir.FullName;

		if(directory == "/storage/emulated/0")
			LocalDirectoryGoBack.IsVisible = false;
		else
			LocalDirectoryGoBack.IsVisible = true;

		foreach(FileSystemInfo info in dir.GetFileSystemInfos())
		{
			filesInDirectory.Add(new MobileFileDirectoryModel
			{
				DirectoryFileSystem = info
			});
		}

		return filesInDirectory;
	}

	private List<FtpFileDirectoryModel> GetFilesInFtpDirectory(string directory)
	{
		List<FtpFileDirectoryModel> filesInDirectory = new List<FtpFileDirectoryModel>();
		ServerDirectoryEntry.Text = directory;

		if(directory == "/")
			ServerDirectoryGoBack.IsVisible = false; 
		else
			ServerDirectoryGoBack.IsVisible = true;

		foreach(FtpListItem item in Client.GetListing(directory))
		{
			filesInDirectory.Add(new FtpFileDirectoryModel
			{
				DirectoryListItem = item
			});
		}

		return filesInDirectory;
	}

	private async void DownloadFile(FtpFileDirectoryModel ftpFileDirectory)
	{
		Client.DownloadFile(LocalDirectoryEntry.Text + "/" + ftpFileDirectory.Name, ftpFileDirectory.FullName);
		await DisplayAlert("", "File downloaded", "Ok");
		LocalDirectoryListView.ItemsSource = GetFilesInMobileDirectory(LocalDirectoryEntry.Text);
	}

	private async void UploadFile(MobileFileDirectoryModel mobileFileDirectory)
	{
		Client.UploadFile(mobileFileDirectory.FullName, ServerDirectoryEntry.Text + "/" + mobileFileDirectory.Name);
		await DisplayAlert("", "File uploaded", "Ok");
		ServerDirectoryListView.ItemsSource = GetFilesInFtpDirectory(ServerDirectoryEntry.Text);
	}

	private string GetParentDirectory(string directory)
	{
		List<string> SplitDirectory = new List<string>(directory.Split("/"));
		SplitDirectory.RemoveAt(SplitDirectory.Count() - 1);
		return String.Join("/", SplitDirectory);
	}

    private void ServerDirectoryGoBack_Tapped(object sender, TappedEventArgs e)
    {
		string ParentDirectory = GetParentDirectory(ServerDirectoryEntry.Text);
		ServerDirectoryListView.ItemsSource = GetFilesInFtpDirectory(ParentDirectory);
		ServerDirectoryEntry.Text = ParentDirectory;
    }

    private void LocalDirectoryGoBack_Tapped(object sender, TappedEventArgs e)
    {
		string ParentDirectory = GetParentDirectory(LocalDirectoryEntry.Text);
        LocalDirectoryListView.ItemsSource = GetFilesInMobileDirectory(ParentDirectory);
		LocalDirectoryEntry.Text = ParentDirectory;
    }

    private async void ServerDirectoryListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
		FtpFileDirectoryModel file = (FtpFileDirectoryModel)e.Item;

		if (file.IsFile)
		{
            var action = await DisplayActionSheet("", "Cancel", null, "Download", "Delete");
			
			switch(action)
			{
				case "Download":
					DownloadFile(file);
					break;
				case "Delete":
					Client.DeleteFile(file.FullName);
					await DisplayAlert("", "File deleted", "Ok");
					ServerDirectoryListView.ItemsSource = GetFilesInFtpDirectory(ServerDirectoryEntry.Text);
					break;
			}
		}
		else
		{
            ServerDirectoryListView.ItemsSource = GetFilesInFtpDirectory(file.FullName);
            ServerDirectoryEntry.Text = file.FullName;
        }
	}

    private async void LocalDirectoryListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
		MobileFileDirectoryModel file = (MobileFileDirectoryModel)e.Item;

		if (!file.IsDirectory)
		{
            var action = await DisplayActionSheet("", "Cancel", null, "Upload", "Delete");

			switch (action)
			{
				case "Upload":
					UploadFile(file);
					break;
				case "Delete":
					file.DirectoryFileSystem.Delete();
					await DisplayAlert("", "File deleted", "Ok");
					LocalDirectoryListView.ItemsSource = GetFilesInMobileDirectory(LocalDirectoryEntry.Text);
					break;
			}

		}
		else
		{
            LocalDirectoryListView.ItemsSource = GetFilesInMobileDirectory(file.FullName);
            LocalDirectoryEntry.Text = file.FullName;
        }
	}
}