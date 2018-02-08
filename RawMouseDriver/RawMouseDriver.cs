using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using RawInputSharp;
using System.Threading;

namespace RawMouseDriver
{
	/// <summary>
	/// Summary description for RawMouseDriver.
	/// </summary>
    public class RawMouseDriver : IDisposable
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private static RawMouseInput _rmInput = null; // needa to be static to be able to call from wndproc
        private PreMessageFilter _filter;
        private Thread _driverThread;
        private DriverWindow _driverWindow;
        private bool disposed = false;
        protected const int WM_INPUT = 0x00FF;

        public RawMouseDriver()
		{
            _driverThread= new Thread(new ThreadStart(RunDriverThread));
            _driverThread.Start();
		}

        private void RunDriverThread()
        {
			//create and init
			_rmInput = new RawMouseInput();
            if (_rmInput == null)
            {
                MessageBox.Show("ERROR: could not create rawinput class");
            }

            _driverWindow = new DriverWindow(_rmInput);
            if (_driverWindow == null)
            {
                MessageBox.Show("ERROR: could not create driverwindow class");//error
            }

            _rmInput.RegisterForWM_INPUT(_driverWindow.Handle);
             _filter = new PreMessageFilter();
             if (_filter == null)
             {
                 MessageBox.Show("ERROR: could not add premessage filter");//error
             }

            Application.AddMessageFilter(_filter);
            Application.Run(_driverWindow);
            Application.RemoveMessageFilter(_filter);

            if (_driverWindow.IsDisposed == false)
            {
                _driverWindow.Dispose();
            }
            _driverWindow = null;
            _rmInput = null;
        }


       private class PreMessageFilter : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM_INPUT && _rmInput != null)// Allow any non WM_INPUT message to pass through
                {
                    if (m != null)
                    {
                        _rmInput.UpdateRawMouse(m.LParam);
                    }
                }
                return false;
            }
        }

        public int GetMouse(int index, ref RawMouse mouse)
        {
            if (index >= 0 && index < _rmInput.Mice.Count)
            {
                mouse = (RawMouse)_rmInput.Mice[index];
                return 0;
            }
            return -1;
        }

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (_driverWindow != null)
                {
                    _driverWindow.Invoke((MethodInvoker)delegate() { _driverWindow.Close(); });//close the dialog
                }
                disposed = true;
            }
        }

        ~RawMouseDriver()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

	}
}
