using System;
using System.Runtime.InteropServices;
using System.Collections;

namespace RawInputSharp {

	/// <summary>
	/// Parent class, contains constants from winuser.h and DllImport function stubs. Will enumerate all raw input devices.
	/// </summary>
	public abstract class RawInput {

		//constants from winuser.h
		public const Int32 RIM_TYPEMOUSE = 0;
		public const Int32 RIDI_DEVICENAME = 0x20000007;
		public const Int32 RID_INPUT = 0x10000003;
		public const Int32 RIDI_DEVICEINFO = 0x2000000b;
		public const Int32 RIDEV_NOLEGACY = 0x00000030;
		public const Int32 RID_HEADER = 0x10000005;
		public const Int32 RI_MOUSE_LEFT_BUTTON_DOWN = 0x0001;  // Left Button changed to down.
		public const Int32 RI_MOUSE_LEFT_BUTTON_UP = 0x0002;  // Left Button changed to up.
		public const Int32 RI_MOUSE_RIGHT_BUTTON_DOWN = 0x0004;  // Right Button changed to down.
		public const Int32 RI_MOUSE_RIGHT_BUTTON_UP = 0x0008;  // Right Button changed to up.
		public const Int32 RI_MOUSE_MIDDLE_BUTTON_DOWN = 0x0010;  // Middle Button changed to down.
		public const Int32 RI_MOUSE_MIDDLE_BUTTON_UP = 0x0020;  // Middle Button changed to up.
		public const Int32 RI_MOUSE_BUTTON_1_DOWN = RI_MOUSE_LEFT_BUTTON_DOWN;
		public const Int32 RI_MOUSE_BUTTON_1_UP = RI_MOUSE_LEFT_BUTTON_UP;
		public const Int32 RI_MOUSE_BUTTON_2_DOWN = RI_MOUSE_RIGHT_BUTTON_DOWN;
		public const Int32 RI_MOUSE_BUTTON_2_UP = RI_MOUSE_RIGHT_BUTTON_UP;
		public const Int32 RI_MOUSE_BUTTON_3_DOWN = RI_MOUSE_MIDDLE_BUTTON_DOWN;
		public const Int32 RI_MOUSE_BUTTON_3_UP = RI_MOUSE_MIDDLE_BUTTON_UP;
		public const Int32 RI_MOUSE_WHEEL = 0x0400;
		public const Int32 WHEEL_DELTA = 120;


		private ArrayList _devices;

		public RawInput() {
			GetRawInputDevices();
		}


		/// <summary>
		/// Returns raw input devices. Standard win32 call in user32.dll.
		/// </summary>
		/// <param name="pRawInputDeviceList"></param>
		/// <param name="puiNumDevices"></param>
		/// <param name="cbSize"></param>
		/// <returns></returns>
		[DllImport("User32.Dll")]
		public static extern Int32 GetRawInputDeviceList(IntPtr pRawInputDeviceList, out Int32 puiNumDevices, Int32 cbSize);

		/// <summary>
		/// Gets information about a raw input device. Used to determine if a device is the windows terminal services (rdp?) mouse.
		/// </summary>
		/// <param name="hDevice"></param>
		/// <param name="uiCommand"></param>
		/// <param name="o"></param>
		/// <param name="pcbSize"></param>
		/// <returns></returns>
		[DllImport("User32.dll",EntryPoint="GetRawInputDeviceInfo",CharSet=CharSet.Unicode,SetLastError=true)]
		public extern static Int32 GetRawInputDeviceInfo([In] IntPtr hDevice, [In] Int32 uiCommand, [In,Out, MarshalAs(UnmanagedType.AsAny)] Object o, [In, Out] ref Int32 pcbSize);

		/// <summary>
		/// Called with IntPtr.Zero to get size of pData for other calls
		/// </summary>
		/// <param name="hDevice"></param>
		/// <param name="uiCommand"></param>
		/// <param name="pMouseInfo"></param>
		/// <param name="pcbSize"></param>
		/// <returns></returns>
		[DllImport("User32.dll",EntryPoint="GetRawInputDeviceInfo",CharSet=CharSet.Unicode,SetLastError=true)]
		public extern static Int32 GetRawInputDeviceInfo([In] IntPtr hDevice, [In] Int32 uiCommand, IntPtr pMouseInfo, [In, Out] ref Int32 pcbSize);

		/// <summary>
		/// Retrieves mouse information -- specifically the number of buttons
		/// </summary>
		/// <param name="hDevice"></param>
		/// <param name="uiCommand"></param>
		/// <param name="devInfo"></param>
		/// <param name="pcbSize"></param>
		/// <returns></returns>
		[DllImport("User32.dll",EntryPoint="GetRawInputDeviceInfo",CharSet=CharSet.Unicode,SetLastError=true)]
		public extern static Int32 GetRawInputDeviceInfo([In] IntPtr hDevice, [In] Int32 uiCommand, [In,Out] ref RID_DEVICE_INFO devInfo, [In, Out] ref Int32 pcbSize);

		/// <summary>
		/// Register for WM_INPUT messages
		/// </summary>
		/// <param name="pcRawInputDevices"></param>
		/// <param name="uiNumDevices"></param>
		/// <param name="cbSize"></param>
		/// <returns></returns>
		[DllImport("User32.dll",EntryPoint="RegisterRawInputDevices",CharSet=CharSet.Unicode,SetLastError=true)]
		public extern static uint RegisterRawInputDevices([In] IntPtr pcRawInputDevices, [In] uint uiNumDevices, [In] uint cbSize);

		[DllImport("User32.dll",EntryPoint="GetRawInputData",CharSet=CharSet.Unicode,SetLastError=true)]
		public extern static Int32 GetRawInputData([In] Int32 hRawInput, [In] Int32 uiCommand, IntPtr pRawInput, [In, Out] ref Int32 pcbSize, [In] Int32 cbSizeHeader);
		[DllImport("User32.dll",EntryPoint="GetRawInputData",CharSet=CharSet.Unicode,SetLastError=true)]
		public extern static Int32 GetRawInputData([In] Int32 hRawInput, [In] Int32 uiCommand, [In,Out] ref RAWINPUT rawInput, [In, Out] ref Int32 pcbSize, [In] Int32 cbSizeHeader);

		/// <summary>
		/// Enumerates the Raw Input Devices and places their corresponding RawInputDevice structures into an ArrayList.
		/// </summary>
		private void GetRawInputDevices() {
			ArrayList devices = new ArrayList();
			RAWINPUTDEVICELIST rawInputDevice;
			IntPtr pRawInputDeviceList = IntPtr.Zero;
			Int32 numDevices;
			Int32 rCode =  GetRawInputDeviceList(IntPtr.Zero, out numDevices, Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));

			if(numDevices > 0) {
				pRawInputDeviceList = Marshal.AllocHGlobal(numDevices * Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
				if(GetRawInputDeviceList(pRawInputDeviceList, out numDevices, Marshal.SizeOf(typeof(RAWINPUTDEVICELIST))) > 0) {
					Int32 listIndex;
					for(listIndex = 0; listIndex < numDevices; listIndex++) {
						rawInputDevice = (RAWINPUTDEVICELIST)Marshal.PtrToStructure(new IntPtr(pRawInputDeviceList.ToInt32() + (listIndex * Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)))), typeof(RAWINPUTDEVICELIST));
						devices.Add(rawInputDevice);
					}
				}
				Marshal.FreeHGlobal(pRawInputDeviceList);
			}
			_devices = devices;
		}

		public ArrayList Devices {
			get {
				return _devices;
			}
		}
	}
}