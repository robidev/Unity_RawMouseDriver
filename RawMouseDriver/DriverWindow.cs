using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RawInputSharp;

namespace RawMouseDriver
{
    public partial class DriverWindow : Form
    {
        private RawMouseInput _rawinput;
        private bool onetime = true;

        public DriverWindow(RawMouseInput input)
        {
            InitializeComponent();
            _rawinput = input;
        }

		protected const int WM_INPUT = 0x00FF;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_INPUT:
                    UpdateControls();
                    break;
            }
            base.WndProc(ref m);
        }


		protected void UpdateControls() {
            if (onetime == true)
            {
                onetime = false;
            }
            if (listBox1.SelectedIndex >= 0 && listBox1.SelectedIndex < _rawinput.Mice.Count)
            {
                label1.Text = "X:" + ((RawMouse)_rawinput.Mice[listBox1.SelectedIndex]).X + " Y:" + ((RawMouse)_rawinput.Mice[listBox1.SelectedIndex]).Y;
            }

		}

        private void DriverWindow_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (RawMouse mouse in _rawinput.Mice)
            {
                listBox1.Items.Add(mouse.Name.ToString());
            }
            listBox1.SelectedIndex = 0;
        }
    }
}
