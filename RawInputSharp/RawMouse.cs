using System;

namespace RawInputSharp
{
	/// <summary>
	/// Maintains state of a given mouse.
	/// </summary>
	public class RawMouse {
		private IntPtr _handle;
		private int _x;
		private int _y;
		private int _z;
		private bool[] _buttons;
		private string _name;

		public RawMouse(IntPtr handle, int numButtons, string name) {
			_handle = handle;
			_buttons = new bool[numButtons];
			_name = name;
		}

		public int X {
			get {
				return _x;
			}

			set {
				_x = value;
			}
		}

		public int Y {
			get {
				return _y;
			}

			set {
				_y = value;
			}
		}

		public int Z {
			get {
				return _z;
			}

			set {
				_z = value;
			}
		}

		public int YDelta {
			get {
				int y = _y;
				_y = 0;
				return y;
			}
		}

		public int XDelta {
			get {
				int x = _x;
				_x = 0;
				return x;
			}
		}

		public int ZDelta {
			get {
				int z = _z;
				_z = 0;
				return z;
			}
		}

		public IntPtr Handle {
			get {
				return _handle;
			}
		}

		public bool[] Buttons {
			get {
				return _buttons;
			}
		}

		public string Name {
			get {
				return _name;
			}
		}



	}
			
}
