using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RawMouseDriver;
using RawInputSharp;

public class walking : MonoBehaviour {

	RawMouseDriver.RawMouseDriver mousedriver;
	// Use this for initialization
	void Start () {
		mousedriver = new RawMouseDriver.RawMouseDriver ();
	}

	private float moveY = 0.0f;
	private float moveX = 0.0f;
	private float sensitivityY = 0.001f;
	private float sensitivityX = 0.1f;
	private RawMouse mouse1;
	void Update() {
		mousedriver.GetMouse (0, ref mouse1);
		moveY += mouse1.YDelta * sensitivityY;
		moveX += mouse1.XDelta * sensitivityX;

		if (moveY != 0){
			transform.Translate(Vector3.forward * moveY);
		}
		if (moveX != 0){
			transform.Rotate(Vector3.up,moveX);
		}
		moveY = 0.0f;
		moveX = 0.0f;

	}
	void OnApplicationQuit()
	{
		mousedriver.Dispose ();
	}
}
