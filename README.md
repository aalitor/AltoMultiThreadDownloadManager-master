# AltoMultiThreadDownloadManager-master
 Download accelerator library and demo. You can make your own Download Accelerator like IDM, FlashGet etc. using this library.

This library provides multithreading download over Http. Using multithreading download speed can be 8x much faster.

<h2>Features:</h2>
<ul>
 <li><b>Dynamic partition:</b> If one thread completes, one of the active threads is divided into 2 and thread count remains the same.</li>
	<li><b>All cases handled:</b> Download cases such as Resume-Not-Supported, Unknown-Content-Length, Expired url cases handled in library side.</li>
	<li>Pause, resume support</li>
	<li>Chunked download support over Google Drive or other sites using chunked stream</li>
	<li><b>Download informations provided:</b> Resumeability, Content-Size, Speed, Progress, ServerFileName</li>
	<li>Due to event based download management is so easy</li>
	<li>Native messaging methods are provided in library</li>
	<li>Chrome extension is included to messaging and capturing downloads</li>
</ul>

<h2>What's next:</h2>
FTP download will be supported when it is ready

<h2>Code sample</h2>
<pre><code>
void InitAndStart()
{
	var url = "http://ipv4.download.thinkbroadband.com/100MB.zip";
	var saveFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
	var saveFileName = "default.unknown";
	var chunkFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.AppData);
	var nofMaxThread = 8;
	var downloader = new MultiThreadDownloadOrganizer(url, saveFolder, saveFileName, chunkFilesFolder, nofMaxThread);
	downloader.ProgressChanged += downloader_DownloadInfoReceived;
	downloader.DownloadInfoReceived += downloader_ProgressChanged;
	downloader.Completed += downloader_Completed;
	downloader.StatusChanged += downloader_StatusChanged;
	downloader.Start();
}
void downloader_DownloadInfoReceived(object sender, EventArgs e)
{
	var downloader = (MultiThreadDownloadOrganizer)sender;
	//You can change the save filename after informations received
	downloader.SaveFileName = downloader.Info.ServerFileName;
	lblContentSize.Text = downloader.Info.ContentSize.ToHumanReadableSize();
	lblServerFileName.Text = downloader.Info.ServerFileName;
	lblResumeability.Text = downloader.Info.AcceptRanges ? "Yes" : "No";
	lblNofActiveThreads.Text = downloader.NofActiveThreads.ToString();
}
void downloader_StatusChanged(object sender, StatusChangedEventArgs e)
{
	switch(e.CurrentStatus)
	{
		case Status.Stopped:
			//all threads stopped
			//disable pause button
			break;
		case Status.Downloading:
			//disable resume button
			break;
	}
}
void downloader_ProgressChanged(object sender, ProgressChangedEventArgs e)
{
	progressBar1.Value = (int)e.Progress;
	lblSpeed.Text = downloader.Speed.ToHumanReadableSize() + "/s";
}
void downloader_Completed(object sender, EventArgs e)
{
	MessageBox.Show("Download Completed");
}
</code></pre>

<h2>Demo Application using native messaging</h2>

<img src="https://i.imgur.com/774e6Qp.gif"></img>
