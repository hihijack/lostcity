using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public enum EGameState{
	Running,
	UIInfoing,
	UIPaging
}

public class GameView : MonoBehaviour
{
	public int VCInput_Axis;
	public int VCInput_Ver_Axis;
	public int VCInput_BtnA;
	public int VCInput_BtnB;
	private float g_TimeLast_TouchDir = 0; 
	public Camera main_camera;
	
	public EGameState gameState;
	
	public GameObject gobj_UI_Panel;
	public GameObject gobj_UIInfo_BG;
	public GameObject gobj_UIInfo_Lable;
	private GameObject gobj_UI_Items;
	private UILabel g_UILabel_UIInfo;
	private bool g_IsUIInfo_End;
	
	private int g_Cur_UIItems_SelectedIndex = 0;
	
	public Dictionary<EItemType, Item> dicItems = new Dictionary<EItemType, Item>(40);
	
	public EItemType curChooseItemType = EItemType.None;
	
	void Start ()
	{
		g_UILabel_UIInfo = gobj_UIInfo_Lable.GetComponent<UILabel>();
		gameState = EGameState.Running;
		gobj_UI_Items = Tools.GetGameObjectInChildByPathSimple(gobj_UI_Panel, "UI_Items");
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
		#if UNITY_EDITOR||UNITY_STANDALONE_WIN
//		if(Input.GetKey(KeyCode.LeftArrow)){
//			VCInput_Axis = -1;
//		}else if(Input.GetKey(KeyCode.RightArrow)){
//			VCInput_Axis = 1;
//		}else{
//			VCInput_Axis = 0;
//		}
//		
//		if(Input.GetKey(KeyCode.UpArrow)){
//			VCInput_Ver_Axis = 1;
//		}else if(Input.GetKeyDown(KeyCode.DownArrow)){
//			VCInput_Ver_Axis = -1;
//		}else{
//			VCInput_Ver_Axis = 0;
//		}
//		
//		if(Input.GetKeyDown(KeyCode.Space)){
//			VCInput_BtnA = 1;
//		}else{
//			VCInput_BtnA = 0;
//		}
//		
//		if(Input.GetKeyDown(KeyCode.E)){
//			VCInput_BtnB = 1;
//		}else{
//			VCInput_BtnB = 0;
//		}
		#endif
		
		// btn handle
		if(IsInGameState(EGameState.UIPaging)){
			UIPagControll();
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
	
	public void TogglePagOpenClose(){
		if(IsInGameState(EGameState.Running)){
			ShowPag();	
		}else if(IsInGameState(EGameState.UIPaging)){
			HidePag();
		}
	}
	
	public void ShowPag(){
		SetGameState(EGameState.UIPaging);
		gobj_UI_Items.SetActive(true);
		// show ites list
		GameObject gobjGrid = Tools.GetGameObjectInChildByPathSimple(gobj_UI_Items, "ListView/Grid");
		UIGrid grid = gobjGrid.GetComponent<UIGrid>();
		foreach(Item item in dicItems.Values){
			string path = IPath.Path_Items + "ItemIcon";
			GameObject gobjItem = Tools.LoadResourcesGameObject(path);
			gobjItem.transform.parent = gobjGrid.transform;
			gobjItem.transform.localPosition = Vector3.zero;
			gobjItem.transform.localScale = new Vector3(1f, 1f, 1f);
			
			ItemIcon itemIcon = gobjItem.GetComponent<ItemIcon>();
			itemIcon.item = item;
			
			// set txu
			UISlicedSprite ss = Tools.GetComponentInChildByPath<UISlicedSprite>(gobjItem, "Button/Background");
			ss.spriteName = item.iconName;
			Debug.LogError(item.iconName);//######
			// set num
			UILabel txtNum = Tools.GetComponentInChildByPath<UILabel>(gobjItem, "num");
			txtNum.text = item.num.ToString();
		}
		grid.repositionNow = true;
		
		g_Cur_UIItems_SelectedIndex = 0;
		SelectItemByIndex(g_Cur_UIItems_SelectedIndex);
	}
	
	public void HidePag(){
		SetGameState(EGameState.Running);
		GameObject gobjGrid = Tools.GetGameObjectInChildByPathSimple(gobj_UI_Items, "ListView/Grid");
		int childCount = gobjGrid.transform.childCount;
		for (int i = 0; i < childCount; i++) {
			GameObject gobjItem = gobjGrid.transform.GetChild(i).gameObject;
			DestroyObject(gobjItem);
		}
		gobj_UI_Items.SetActive(false);
	}
	
	public void OnBtnPagClick(){
		TogglePagOpenClose();
	}
	
	void UIPagControll(){
//		UICamera.selectedObject =
		if(!IsTouchDirInIntervalTime(0.06f)){
			g_TimeLast_TouchDir = Time.time;
			
			GameObject gobjGrid = Tools.GetGameObjectInChildByPathSimple(gobj_UI_Items, "ListView/Grid");
			int itemsCount = gobjGrid.transform.GetChildCount();
			if(itemsCount > 0){
				if(VCInput_Axis > 0){
					if(g_Cur_UIItems_SelectedIndex < (itemsCount - 1)){
						g_Cur_UIItems_SelectedIndex ++;
						SelectItemByIndex(g_Cur_UIItems_SelectedIndex);
					}
				}else if(VCInput_Axis < 0){
					if(g_Cur_UIItems_SelectedIndex > 0){
						g_Cur_UIItems_SelectedIndex --;
						SelectItemByIndex(g_Cur_UIItems_SelectedIndex);
					}
				}else if(VCInput_Ver_Axis > 0){
					if(g_Cur_UIItems_SelectedIndex > 5){
						g_Cur_UIItems_SelectedIndex -= 6;
						SelectItemByIndex(g_Cur_UIItems_SelectedIndex);
					}
				}else if(VCInput_Ver_Axis < 0){
					if(g_Cur_UIItems_SelectedIndex + 6 < itemsCount){
						g_Cur_UIItems_SelectedIndex += 6;
						SelectItemByIndex(g_Cur_UIItems_SelectedIndex);
					}
				}else if(VCInput_BtnA > 0){
					GameObject gobjItem = gobjGrid.transform.GetChild(g_Cur_UIItems_SelectedIndex).gameObject;
					ItemIcon itemIcom = gobjItem.GetComponent<ItemIcon>();
					Item item = itemIcom.item;
					if(item.canUse){
						curChooseItemType = item.itemType;
						
						// show txu
						UISlicedSprite ss = Tools.GetComponentInChildByPath<UISlicedSprite>(gobj_UI_Panel,"BtnPag/Background");
						ss.spriteName = item.iconName;
						// show num
						GameObject gobjNum = Tools.GetGameObjectInChildByPathSimple(gobj_UI_Panel, "BtnPag/num");
						gobjNum.SetActive(true);
						UILabel txtNum = gobjNum.GetComponent<UILabel>();
						txtNum.text = item.num.ToString();
						
					}
				}
			}
			if(VCInput_BtnB > 0){
				TogglePagOpenClose();
			}
		}
	}
	
	void SelectItemByIndex(int index){
		GameObject gobjGrid = Tools.GetGameObjectInChildByPathSimple(gobj_UI_Items, "ListView/Grid");
		if(gobjGrid.transform.childCount > 0){
			GameObject gobjItem = gobjGrid.transform.GetChild(index).gameObject;
			UICamera.selectedObject = Tools.GetGameObjectInChildByPathSimple(gobjItem, "Button");
			// show desc
			Item item = gobjItem.GetComponent<ItemIcon>().item;
			UILabel desc = Tools.GetComponentInChildByPath<UILabel>(gobj_UI_Items, "txt_info");
			desc.text = item.describe;
		}
	}
	
	public bool IsTouchDirInIntervalTime(float interval_time){
		bool isInIntervalTime = true;
		float curIntervalTime = Time.time - g_TimeLast_TouchDir;
		if(curIntervalTime > interval_time){
			isInIntervalTime = false;
		}
		return isInIntervalTime;
	}
	
	public void PlayerHitFloatItem(FloatItem floatItem){
		EItemType itemType = floatItem.itemType;
		Item item = null;
		if(dicItems.ContainsKey(itemType)){
			item = dicItems[itemType];
			item.num ++;
		}else{
			item = new Item(floatItem.itemType);
			dicItems.Add(itemType, item);
		}
		
		DestroyObject(floatItem.gameObject);
	}
	
	public bool HasItem(EItemType itemType){
		bool has = false;
		if(dicItems.ContainsKey(itemType)){
			Item item = dicItems[itemType];
			if(item.num > 0){
				has = true;
			}
		}
		return has;
	}
}