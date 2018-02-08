https://forum.unity.com/threads/small-mono-csharp-script-based-driver-for-multiple-mice-support-in-unity.266174/

RawMouseDriver -- readme.txt.

RawMouseDriver is a C# library that can be used by unity to access multiple mice 
independently. I based the code on the example found here: 
http://jstookey.com/arcade/rawmouse/ from Peter Brumblay (peter@tyrconsulting.com).
I created it for myself to be able to use multiple mice in a unity project. I'm 
releasing this code and the code used from Peter Brumblay as a package in the hope
that it can be useful for others as well.

Instalation:
copy the RawInputSharp.dll and RawMouseDriver.dll into the assets directory of Unity.
then you can reference them from the script like so:

using RawMouseDriver;
using RawInputSharp;

public class example : MonoBehaviour 
{
	private RawMouseDriver.RawMouseDriver mousedriver;
	private RawMouse mouse1;
	private RawMouse mouse2;
	
	private float moveY = 0.0f;	
	private float moveX = 0.0f;

	void Start () 
	{	
		mousedriver = new RawMouseDriver.RawMouseDriver ();
	}
	void Update()
	{
		mousedriver.GetMouse (0, ref mouse1);
		mousedriver.GetMouse (1, ref mouse1);
		
		moveY += mouse1.YDelta;
		transform.Translate(Vector3.forward * moveY);
		
		moveX += mouse2.XDelta;
		transform.Translate(Vector3.forward * moveX);
	}	
	void OnApplicationQuit()
	{
		mousedriver.Dispose ();
	}
}

By using "GetMouse" with an index it is possible to acces the data of the individual 
mice connected to the system, and retrieve the respective data. In the background the 
RawMouseDriver.dll will create a window that is used to capture the mouse data. I hid 
this window from view, but if you experience any sort of buggy behavior or need to 
troubleshoot, you can access this window by pressing alt-tab and selecting 
"Connected Mice". Here you see a list of detected mice, and when you select one, you 
can see the X and Y coordinates at the bottom that should update when you move the 
respective mouse.

Compilation:
include both csproj files in a solution, and you should compile them as .net 2.0, in 
32bit format. In 64bit I ran into trouble with retrieving data from the mice.
No other special dependencies should be needed afaik.

This code and software is provided as-is with absolutely no warranty whatsoever. But
feel free to contact me when questions arise on robin.dev@gmail.com. Please respect
the names mentioned in this document, and give credit where credit is due :)

Best regards,

Robin Massink
robin.dev@gmail.com

(copied from old zipfile:)

RawInputSharp -- readme.txt.

RawInputSharp is a C# port of the raw_mouse code from Jake Stookey. It uses Platform
Invoke calls to mimic the behavior in raw_mouse, and has everything reorganized into
what I hope is a sensible object-oriented design.

The code:
RawInput.cs contains all the P/Invoke prototypes and the constants needed from winuser.h
RawInputStructDefs.cs contains all the C# struture definitions that the P/Invoke calls
use.

RawMouseInput.cs is the helper class that you will use to read raw_mouse calls.
RawMouse.cs is a state container for a given mouse.

Caveats:
-absolute mice not currently supported. I don't have one so I didn't care.
-system mouse and RDP mouse ignored. I had no need for these either.

Support for the above would be easy to add. You'd have to modify RawMouseInput... 

Have fun.

Peter Brumblay
