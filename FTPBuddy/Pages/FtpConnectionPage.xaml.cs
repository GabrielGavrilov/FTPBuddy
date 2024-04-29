using FluentFTP;
using FTPBuddy.Models;

namespace FTPBuddy.Pages;

public partial class FtpConnectionPage : ContentPage
{
	FtpClient Client;

	public FtpConnectionPage(FtpClient client)
	{
		InitializeComponent();

		Client = client;
		Client.AutoConnect();
		Title = Client.Host;

		FtpItemListView.ItemsSource = GetFilesInFtpDirectory("/");
		MobileFileSystemListView.ItemsSource = GetFilesInMobileDirectory("/storage/emulated/0/");
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
	}

	private List<MobileFileDirectoryModel> GetFilesInMobileDirectory(string directory)
	{
		List<MobileFileDirectoryModel> filesInDirectory = new List<MobileFileDirectoryModel>();
		DirectoryInfo dir = new DirectoryInfo(directory);
		LocalFilesDirectoryEntry.Text = dir.FullName;

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
		FtpServerFilesDirectoryEntry.Text = directory;

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
		Client.DownloadFile(LocalFilesDirectoryEntry.Text + "/" + ftpFileDirectory.Name, ftpFileDirectory.FullName);
		await DisplayAlert("", "File downloaded", "Ok");
		MobileFileSystemListView.ItemsSource = GetFilesInMobileDirectory(LocalFilesDirectoryEntry.Text);
	}

	private async void UploadFile(MobileFileDirectoryModel mobileFileDirectory)
	{
		Client.UploadFile(mobileFileDirectory.FullName, FtpServerFilesDirectoryEntry.Text + "/" + mobileFileDirectory.Name);
		await DisplayAlert("", "File uploaded", "Ok");
		FtpItemListView.ItemsSource = GetFilesInFtpDirectory(FtpServerFilesDirectoryEntry.Text);
	}

    private void FtpItemListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
		FtpFileDirectoryModel ftpFileDirectoryModel = (FtpFileDirectoryModel)e.Item;

		if (ftpFileDirectoryModel.IsFile)
			DownloadFile(ftpFileDirectoryModel);

		else
			FtpItemListView.ItemsSource = GetFilesInFtpDirectory(ftpFileDirectoryModel.FullName);

		FtpServerFilesDirectoryEntry.Text = ftpFileDirectoryModel.FullName;
	}

    private void MobileFileSystemListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
		MobileFileDirectoryModel item = (MobileFileDirectoryModel)e.Item;

		if (!item.IsDirectory)
			UploadFile(item);

		else
			MobileFileSystemListView.ItemsSource = GetFilesInMobileDirectory(item.FullName);
		
		LocalFilesDirectoryEntry.Text = item.FullName;
	}
}