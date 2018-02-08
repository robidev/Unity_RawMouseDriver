using System;
using System.Runtime.InteropServices;

namespace RawInputSharp {
/*
 * This file includes all the struct definitions needed for the P/Invoke calls. I'm hardly a P/Invoke expert,
 * so a lot of the data types I picked probably don't make any practical sense -- I'm casting a lot. 
 * (uint when it should be int, etc.). Feel free to change it. :)
 * 
 */

	/// <summary>
	/// Win32 struct for raw input devices in a list. If dwType == RIM_TYPEMOUSE, our
	/// device is a mouse. hDevice is a handle.
	/// </summary>
	public struct RAWINPUTDEVICELIST {
		public IntPtr hDevice;
		public Int32 dwType;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct RID_DEVICE_INFO {
		[FieldOffset(0)] public uint cbSize;
		[FieldOffset(4)] public uint dwType;
		[FieldOffset(8)] public RID_DEVICE_INFO_MOUSE mouse;
		[FieldOffset(8)] public RID_DEVICE_INFO_KEYBOARD keyboard;
		[FieldOffset(8)] public RID_DEVICE_INFO_HID hid;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct RID_DEVICE_INFO_MOUSE {
		[FieldOffset(0)] public uint dwId; 
		[FieldOffset(4)] public uint dwNumberOfButtons; 
		[FieldOffset(8)] public uint dwSampleRate; 
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct RID_DEVICE_INFO_KEYBOARD {
		[FieldOffset(0)] public uint dwType; 
		[FieldOffset(4)] public uint dwSubType; 
		[FieldOffset(8)] public uint dwKeyboardMode; 
		[FieldOffset(12)] public uint dwNumberOfFunctionKeys; 
		[FieldOffset(16)] public uint dwNumberOfIndicators; 
		[FieldOffset(20)] public uint dwNumberOfKeysTotal; 
	}
	[StructLayout(LayoutKind.Explicit)]
	public struct RID_DEVICE_INFO_HID {
		[FieldOffset(0)] public uint dwVendorId; 
		[FieldOffset(4)] public uint dwProductId; 
		[FieldOffset(8)] public uint dwVersionNumber; 
		[FieldOffset(12)] public ushort usUsagePage; 
		[FieldOffset(14)] public ushort usUsage; 
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct RAWINPUTDEVICE {
		[FieldOffset(0)] public ushort usUsagePage; //Toplevel collection UsagePage
		[FieldOffset(2)] public ushort usUsage; //Toplevel collection Usage
		[FieldOffset(4)] public uint dwFlags; 
		[FieldOffset(8)] public uint hwndTarget; // Target hwnd. NULL = follows keyboard focus
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct RAWINPUTHEADER {
		[FieldOffset(0)] public uint dwType;
		[FieldOffset(4)] public uint dwSize;
		[FieldOffset(8)] public uint hDevice;
		[FieldOffset(12)] public uint wParam;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct RAWINPUT {
		[FieldOffset(0)] public RAWINPUTHEADER header;
		[FieldOffset(16)] public RAWMOUSE mouse;
		[FieldOffset(16)] public RAWKEYBOARD keyboard;
		[FieldOffset(16)] public RAWHID hid;
	}

	/// <summary>
	/// I had to play with the layout of this one quite a bit. The usFlags field is listed as a USHORT in winuser.h.
	/// Changing it to a uint makes all the fields line up properly for the WM_INPUT messages.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct RAWMOUSE {
		[FieldOffset(0)] public uint usFlags; //indicator flags
		[FieldOffset(4)] public ushort usButtonFlags;
		[FieldOffset(6)] public ushort usButtonData;
		[FieldOffset(8)] public uint ulRawButtons; //The raw state of the mouse buttons
		[FieldOffset(12)] public int lLastX; //The signed relative or absolute motion in the X direction.
		[FieldOffset(16)] public int lLastY; //The signed relative or absolute motion in the Y direction.
		[FieldOffset(20)] public uint ulExtraInformation; //Device-specific additional information for the event.
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct RAWKEYBOARD {
		[FieldOffset(0)] public ushort MakeCode; //The "make" scan code (key depression).
		[FieldOffset(2)] public ushort Flags; //The flags field indicates a "break" (key release) and other
											  //miscellaneous scan code information defined in ntddkbd.h.
		[FieldOffset(4)] public ushort Reserved;
		[FieldOffset(6)] public ushort VKey; //Windows message compatible information
		[FieldOffset(8)] public uint Message; 
		[FieldOffset(12)] public uint ExtraInformation; //Device-specific additional information for the event.
	}
	
	[StructLayout(LayoutKind.Explicit)]
	public struct RAWHID {
		[FieldOffset(0)] uint dwSizeHid;    // byte size of each report
		[FieldOffset(4)] uint dwCount;      // number of input packed
		[FieldOffset(8)] byte bRawData; //winuser.h has this as BYTE bRawData[1]... should it be
		//uint pbRawData then instead?
	}
}