using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public enum EGameState{
	Running,
	UIInfoing
}

public class GameView : MonoBehaviour
{
	
	
	
	public int VCInput_Axis;
	public int VCInput_Ver_Axis;
	public int VCInput_BtnA;
	public int VCInput_BtnB;
	public Camera main_camera;
	
	public EGameState gameState;
	
	public GameObject gobj_UIInfo_BG;
	public GameObject gobj_UIInfo_Lable;
	private UILabel g_UILabel_UIInfo;
	private bool g_IsUIInfo_End;
	
	void Start ()
	{
		g_UILabel_UIInfo = gobj_UIInfo_Lable.GetComponent<UILabel>();
		gameState = EGameState.Running;
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
				VCInput_Ver_Axis = 1;
			}else if(dpad.Down){
				VCInput_Ver_Axis = -1;
			}else{
				VCInput_Ver_Axis = 0;
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
		
		// keyboard controll
		/// for test.when build， close it
		if(Input.GetKey(KeyCode.LeftArrow)){
			VCInput_Axis = -1;
		}else if(Input.GetKey(KeyCode.RightArrow)){
			VCInput_Axis = 1;
		}else{
			VCInput_Axis = 0;
		}
		
		
		if(Input.GetKey(KeyCode.UpArrow)){
			VCInput_Ver_Axis = 1;
		}else if(Input.GetKey(KeyCode.DownArrow)){
			VCInput_Ver_Axis = -1;
		}else{
			VCInput_Ver_Axis = 0;
		}
		
		if(Input.GetKeyDown(KeyCode.Space)){
			VCInput_BtnA = 1;
		}else{
			VCInput_BtnA = 0;
		}
		
		if(Input.GetKeyDown(KeyCode.E)){
			VCInput_BtnB = 1;
		}else{
			VCInput_BtnB = 0;
		}
		
	}
	
	public void ShowUIInfo(string str){
		if(gobj_UIInfo_BG != null && gobj_UIInfo_Lable != null && IsInGameState(EGameState.Running)){
			g_IsUIInfo_End = false;
			gobj_UIInfo_BG.SetActive(true);
			gobj_UIInfo_Lable.SetActive(true);
			StartCoroutine(CoAnimUIInfoShow(str));	
		}
	}
	
	public void HideUIInfo(){
		if(gobj_UIInfo_BG != null && gobj_UIInfo_Lable != null && IsInGameState(EGameState.UIInfoing) && g_IsUIInfo_End){
			SetGameState(EGameState.Running);
			gobj_UIInfo_BG.SetActive(false);
			gobj_UIInfo_Lable.SetActive(false);
		}
	}
	
	private IEnumerator CoAnimUIInfoShow(string str){
		SetGameState(EGameState.UIInfoing);
		
		string strCur = "";
		int iCountAll = str.Length;
		int iCouCur = 0;
		while(iCouCur <= iCountAll){
			strCur = str.Substring(0, iCouCur);
			g_UILabel_UIInfo.text = strCur;
			
			yield return new WaitForSeconds(IConst.UIInfoShowAnimInterval);
			
			iCouCur ++;
		}
		StartCoroutine(CoUIInfoBlink());
	}
	
	private IEnumerator CoUIInfoBlink(){
		g_IsUIInfo_End = true;
		
		string strA = g_UILabel_UIInfo.text;
		string strB = g_UILabel_UIInfo.text + "|";
		int iIsShow = 1;
		while(IsInGameState(EGameState.UIInfoing)){
			if(iIsShow > 0){
				g_UILabel_UIInfo.text = strB;
			}else{
				g_UILabel_UIInfo.text = strA;
			}
			
			yield return new WaitForSeconds(IConst.UIInfoBlinkInterval);
			
			iIsShow *= -1;
			
		}
	}
	
	public void SetGameState(EGameState state){
		this.gameState = state;
	}
	
	public bool IsInGameState(EGameState state){
		return this.gameState == state;
	}
}