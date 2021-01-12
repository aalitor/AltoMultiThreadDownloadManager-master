# AltoMultiThreadDownloadManager-master
 Download accelerator library and demo

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


<h2>Demo Application using native messaging</h2>

<img src="https://i.imgur.com/774e6Qp.gif"></img>
