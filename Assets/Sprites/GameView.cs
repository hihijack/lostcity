using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class GameView : MonoBehaviour
{
	
	
	
	public int VCInput_Axis;
	public int VCInput_BtnA;
	public int VCInput_BtnB;
	
	void Start ()
	{
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		// try and get the dpad
		VCDPadBase dpad = VCDPadBase.GetInstance("dpad");
		// if we got one, perform movement
		if (dpad)
		{
			if (dpad.Left){
				VCInput_Axis = -1;
			}else if (dpad.Right){
				VCInput_Axis = 1;
			}else{
				VCInput_Axis = 0;
			}
			
			if (dpad.Up){
				
			}
			if (dpad.Down){
				
			}
		}
		
		VCButtonBase abtn = VCButtonBase.GetInstance("BtnA");
		if (abtn != null && abtn.HoldTime > 0)
		{
			// press a btn
			VCInput_BtnA = 1;
		}else{
			VCInput_BtnA = 0;
		}
		
		VCButtonBase bbtn = VCButtonBase.GetInstance("BtnB");
		if(bbtn != null && bbtn.HoldTime > 0)
		{
			// press b btn
			VCInput_BtnB = 1;
		}else{
			VCInput_BtnB = 0;
		}
	}
}