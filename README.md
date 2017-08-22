# Offline PS4 Remote Play (Current Version of the Remote Play :  2.0.0.2211)

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](http://paypal.me/MysteryDash)

Ever wanted to use your PS4 on your local network but couldn't because you didn't have an Internet connection available ?  
Here's the solution !

## Before we get started, a little message for Sony

Hello Sony,  
If you ever happen to read this, don't you think it would be nice to have this feature built-in ?  
Think about it like this for a second : you are going on holiday with your PS4 and your laptop because you have no other display available for it. Most laptops aren't shipped with an HDMI input port so you can't directly connect your PS4 to your laptop's screen. However, what you can do is connect your laptop and your PS4 with an ethernet cable and use RemotePlay... until you realize that you need an Internet connection to start the software.  
Looking for updates is nice, but is there really a point in preventing players from using their console because they don't have an Internet connection available all the time ?  
Incidentally, if you have any problem with me posting this, please don't hesitate to contact me.

## Getting started

For various reasons, I can't give you the patched executable. However, I can still tell you how to patch it yourself.
You will need to download [xdelta](https://github.com/jmacd/xdelta-gpl/releases) as well as the [RemotePlay.patch](RemotePlay.patch) and put them next to your RemotePlay.exe.  
Execute the following command :  

        xdelta -d -s RemotePlay.exe RemotePlay.patch RemotePlay-Patched.exe
		
Then you have to start RemotePlay-Patched.exe to enjoy your PS4 Remote Play offline experience.
I'll describe below another method to do the same thing without having to change RemotePlay.exe itself.

## Will RemotePlay work as usual ?

Yes, every aspect of the original RemotePlay have been kept, except the error messages (only those concerning the updates, of course).  
The software will still ask you if you want to update it, however, you can now decline and still use the Remote Play !

## I don't trust those patches thrown at me like this...

... and that's perfectly fine.  
Here are the steps so that you can reproduce the patch (the first version) by yourself :
* Throw your RemotePlay.exe at [de4dot](https://github.com/0xd4d/de4dot) to remove the obfuscation.
* Open your brand new RemotePlay-cleaned.exe using your favorite decompiler (ILSpy, .NET Reflector, etc...). In my case, I am using [dnSpy](https://github.com/0xd4d/dnSpy). Navigate to the class RemoteplayUI.CheckUpdate and look for the method taking an object and an EventArgs as arguments.
Here's the original obfuscated but commented method :
```csharp
private void \uE000(object \uE00F, EventArgs \uE010)
{
	this.\uE005 = new Timer();
	this.\uE005.Interval = \uE005.\uE000(19);
	this.\uE005.Tick += new EventHandler(this.\uE002);
	this.\uE005.Start();
	if (!NetworkInterface.GetIsNetworkAvailable())
	{
		NormalMessage normalMessage = new NormalMessage(RpUIres.\uE03A, (MESSAGE_DIALOG_SHOW_TYPE)\uE005.\uE000(0));
		normalMessage.ShowDialog();
		normalMessage.Dispose();
		// Those three lines below are the lines we want to get rid of.
		// Removing the whole if statement works fine too, but you will not have the error message anymore.
		this.\uE00A = (\uE005.\uE000(2) != 0);
		base.Close();
		return;
	}
	this.\uE000();
}
```
And here's the unobfuscated edited method :
```csharp
private void CheckUpdate_Shown(object sender, EventArgs e)
{
	this.timer_0 = new Timer();
	this.timer_0.Interval = Class8.smethod_0(19);
	this.timer_0.Tick += new EventHandler(this.timer_0_Tick);
	this.timer_0.Start();
	if (!NetworkInterface.GetIsNetworkAvailable())
	{
		NormalMessage expr_56 = new NormalMessage(RpUIres.String_58, (MESSAGE_DIALOG_SHOW_TYPE)Class8.smethod_0(0));
		expr_56.ShowDialog();
		expr_56.Dispose();
	}
	this.method_0();
}
```
* There's a second edit to do. Look this time for the method taking an object and an AsyncCompletedEventArgs as arguments.
Here's the original (shortened a bit) obfuscated but commented method :
```csharp
private void \uE000(object \uE019, AsyncCompletedEventArgs \uE01A)
{
	if (\uE01A.Cancelled)
	{
		this.\uE00A = (\uE005.\uE000(2) != 0);
	}
	else if (\uE01A.Error != null)
	{
		NormalMessage normalMessage = new NormalMessage(RpUIres.\uE039, (MESSAGE_DIALOG_SHOW_TYPE)\uE005.\uE000(0));
		normalMessage.ShowDialog();
		normalMessage.Dispose();
		this.\uE00A = (\uE005.\uE000(2) != 0); // We want to remove this.
	}
	else
	{
		// Deserialization stuff no one cares about
	}
	this.\uE007 = (\uE005.\uE000(2) != 0); // We want this to be always true}
}
```
And here is the unobfuscated edited method (still shortened) :
```csharp
private void webClient_0_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
{
	if (e.Cancelled)
	{
		this.bool_3 = (Class8.smethod_0(2) != 0);
	}
	else if (e.Error != null)
	{
		NormalMessage expr_34 = new NormalMessage(RpUIres.String_57, (MESSAGE_DIALOG_SHOW_TYPE)Class8.smethod_0(0));
		expr_34.ShowDialog();
		expr_34.Dispose();
	}
	else
	{	
	    // Deserialization stuff no one cares about
	}
	this.bool_1 = true;
}
```
* And that's it, you can now export your brand new RemotePlay executable (using the Save Module... function on dnSpy) and enjoy playing on your PS4 anywhere.

## Alternative method, almost 0 programming skill required.

1. Download and install [Fiddler](http://www.telerik.com/fiddler).
2. [Enable HTTPS decryption in Fiddler](https://www.fiddlerbook.com/fiddler/help/httpsdecryption.asp).
3. With Fiddler running, start RemotePlay and wait for the request to https://remoteplay.dl.playstation.net/remoteplay/module/win/rp-version-win.json.
4. Select the AutoResponder tab, enable the rules, enable unmatched requests passthrough.
5. Drag & Drop the request made to remoteplay.dl.playstation.net into the rules list.
6. Select the FiddlerScript tab and look for this line :
```csharp
static function OnBeforeRequest(oSession: Session) {
```
7. Add the following content under the line you found earlier :
```csharp
if (oSession.HTTPMethodIs("CONNECT"))
{
    oSession.oFlags["x-replywithtunnel"] = "GenerateTunnel";
    return;
}
```
8. Click on Save Script.
9. That's it for Fiddler. You have nothing to save by yourself. The only thing that matters now is that Fiddler must be up and running when you want to play without Internet.
10. There's still something to do. Currently, if you open RemotePlay, it'll tell you that (if you are truly disconnected) there is not network connection available. To fix this, create an access point on your phone and connect to it (there's no need to have an Internet connection available on your phone, otherwise it would defeat the point of having done everything mentionned above). Note that you can also use any free hotspot you may have around you.
11. It's already done ! When you'll start RemotePlay, it'll think that an Internet connection is available (when it's merely connected to a hotspot WITHOUT Internet) and when it'll try to look for updates Fiddler will take care of it.
12. Enjoy the offline PS4 Remote Play.

## Contributing

I usually appreciate contributions, however, this time, I will decline every pull request containing an executable or a patch.