# Offline PS4 Remote Play

Ever wanted to use your PS4 on your local network but couldn't because you didn't have an Internet connection available?  
Here's the solution!

## Before we get started, a little message for Sony

Hello Sony,  
If you ever happen to read this, don't you think it would be nice to have this feature built-in?  
Think about it like this for a second: you are going on holiday with your PS4 and your laptop because you have no other display available for it. Most laptops aren't shipped with an HDMI input port so you can't directly connect your PS4 to your laptop's screen. However, what you can do is connect your laptop and your PS4 with an ethernet cable and use Remote Play... until you realize that you need an Internet connection to start the software.  
Looking for updates is nice, but is there really a point in preventing players from using their console because they don't have an Internet connection available all the time?  
Incidentally, if you have any problem with me posting this, please don't hesitate to contact me.

## Supported versions of the Remote Play

You can find older versions of the Remote Play on the Internet Archive: Wayback Machine!
- 1.0.0.15181 - Hash : 694659629815D82CDCD62A95FA771237
- 1.5.0.8251 - Hash : 53DF9F442EEC309D95BE88D28CC21E18
- 2.0.0.2211 - Hash : FCD0DD66996B399F6A1A9A254F7E85B3
- 2.6.0.2270 - Hash : 2E4C4CA5ECFD3138CB734ED603958A1A
- 2.7.0.7270 - Hash : 9961E4475745881FAC537D58B1BCD5FB (seems to work without even logging to the Playstation Network)

## Getting started

For various reasons, I can't give you the patched executable. However, I can still tell you how to patch it yourself.

### [FULLY AUTOMATED PATCHER]

You can just download and run the latest version of the [PS4 Remote Play Patcher](https://github.com/MysteryDash/Offline-PS4-Remote-Play/releases/latest).  
If the software can't find the Remote Play by itself it'll ask you to specify its location.  
Once that is done, the software will patch the Remote Play (with the latest patch available) and create a backup of your original Remote Play if you want.  
Don't forget that, to use the PS4 Remote Play, you still need a local network (connect your PS4 to your computer using an ethernet cable or a local Wi-Fi network hosted on your computer)!

### [MANUAL PATCH]

You will need to download [BsPatch](BsPatch) and the [Patches.zip](Patches.zip) file. 
Extract the patch of your choice and execute the following command :  

        bsdiff RemotePlay.exe RemotePlay-Patched.exe mypatch
		
Then you have to start RemotePlay-Patched.exe to enjoy your PS4 Remote Play offline experience.
I'll describe below another method to do the same thing without having to change RemotePlay.exe itself.

## Will Remote Play work as usual?

Yes, every aspect of the original Remote Play have been kept, except for the updates window, which will not even appear with the latest version of the patch.

## I don't trust those patches thrown at me like this...

... and that's perfectly fine.  
Here are the steps so that you can reproduce the patch (the first version) by yourself:
* Throw your RemotePlay.exe at [de4dot](https://github.com/0xd4d/de4dot) to remove the obfuscation.
* Open your brand new RemotePlay-cleaned.exe using your favorite decompiler (ILSpy, .NET Reflector, etc...). In my case, I am using [dnSpy](https://github.com/0xd4d/dnSpy). Navigate to the class RemoteplayUI.CheckUpdate and look for the properties NeedUpdate and CancelRequest. Force them to return false.
* There's a second edit to do. Look this time for the class called Class2 (this class is not in a namespace).
* In the Main method, get rid of ```checkUpdate.ShowDialog();```.
* And that's it, you can now export your brand new Remote Play executable (using the Save Module... function on dnSpy) and enjoy playing on your PS4 anywhere.

If you wonder why we aren't just removing ShowDialog and setting the two assignments under to false directly, that's because it triggers my antivirus for some reason.

## Alternative method, almost 0 programming skill required.

1. Download and install [Fiddler](http://www.telerik.com/fiddler).
2. [Enable HTTPS decryption in Fiddler](https://www.fiddlerbook.com/fiddler/help/httpsdecryption.asp).
3. a. With Fiddler running, start Remote Play and wait for the request to https://remoteplay.dl.playstation.net/remoteplay/module/win/rp-version-win.json.
3. b. You might also see a request to https://remoteplay.dl.playstation.net/remoteplay/module/pplist_v2.json, handle it the same way as the previous one.
4. Select the AutoResponder tab, enable the rules, enable unmatched requests passthrough.
5. Drag & Drop the request made to remoteplay.dl.playstation.net into the rules list.
6. Select the FiddlerScript tab and look for this line:
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
10. There's still something to do. Currently, if you open Remote Play, it'll tell you that (if you are truly disconnected) there is not network connection available. To fix this, create an access point on your phone and connect to it (there's no need to have an Internet connection available on your phone, otherwise it would defeat the point of having done everything mentionned above). Note that you can also use any free hotspot you may have around you.
11. It's already done ! When you'll start the Remote Play, it'll think that an Internet connection is available (when it's merely connected to a hotspot WITHOUT Internet) and when it'll try to look for updates Fiddler will take care of it.
12. Enjoy the offline PS4 Remote Play.

## Contributing

I usually appreciate contributions, however, this time, I will decline every pull request containing an executable or a patch.  
Suggestions are welcome!
