//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;
//
//public class GameView : MonoBehaviour
//{
//	
//	public bool isRunning = true;
//	public static Queue gameStateQueue = new Queue ();
//	
//	public GameObject gObjHero;
//	
//	// level data
//	
//	// enermy
//	Dictionary<int, IActor> g_DicActors;
//	List<int> listGCActorIdToDelete;
//	int g_EnermyId = 0;
//	Rect g_RectLeftCE;
//	Rect g_RectRightCE;
//	Rect g_RectUpCE;
//	Rect g_RectDownCE;
//	
//	// bg
//	public GameObject g_gObjFg1;
//	public GameObject g_gObjFg2;
//	public GameObject g_gObjBg1;
//	public GameObject g_gObjBg2;
//	float fgSpeed = 2f;
//	
//	System.Random random = new System.Random();
//	
//	// mainhead
//	GameObject g_GobjMainHead;
//	
//	// camera
//	public Camera camera_bg;
//	public Camera camera_actor;
//	public Camera camera_ui;
//	
//	public Hero hero = null;
//	
//	// cache
//	// Use this for initialization
//	
//	
//	public tk2dTextMesh tmHeroState;
//	
//	void Start ()
//	{
//		
//		
//		StartCoroutine(InitGame());
//		
//		// init hero
//		gObjHero = Tools.LoadResourcesGameObject(IPath.Path_Hero);
//		gObjHero.transform.position = new Vector3(100f, 200f, 0f);
//		gObjHero.AddComponent<Hero>();
//		hero = gObjHero.GetComponent<Hero>();
//		
//		tmHeroState = GameObject.Find("heroState").GetComponent<tk2dTextMesh>();
//		
//		//bg
////		g_gObjFg1 = GameObject.Find("fg1");
////		g_gObjFg2 = GameObject.Find("fg2");
//		
////		iTween.MoveTo(g_gObjFg1, iTween.Hash("position", new Vector3(-5.12f, -0.28f, -0.06f), "speed", fgSpeed, "easetype", "linear","oncomplete", "OnFgMoveEnd", "oncompletetarget", gameObject, "oncompleteparams", g_gObjFg1));
////		iTween.MoveTo(g_gObjFg2, iTween.Hash("position", new Vector3(-5.12f, -0.28f, -0.06f), "speed", fgSpeed, "easetype", "linear","oncomplete", "OnFgMoveEnd", "oncompletetarget", gameObject, "oncompleteparams", g_gObjFg2));
//		
//		// read json data of level ini
//		InitLevelData();
//		
////		InitMainHead();
//		curlevelId = 0;
//		
//		// init CE rect
//		g_RectRightCE = new Rect(camera_bg.pixelWidth + 10f , 10, 30f, camera_bg.pixelHeight - 10);
//		g_RectLeftCE = new Rect(-60f, 10, 30f, camera_bg.pixelHeight - 10);
//		g_RectUpCE = new Rect(camera_bg.pixelWidth / 2, -10f, camera_bg.pixelWidth / 2, 10f);
//		g_RectDownCE = new Rect(camera_bg.pixelWidth / 2, camera_bg.pixelHeight, camera_bg.pixelWidth / 2, 10);
//		
//		g_DicActors = new Dictionary<int, IActor>(40);
//		listGCActorIdToDelete = new List<int>(40);
//		StartCoroutine(CoEnermyCreate());
//	}
//	
//	IEnumerator InitGame(){
//		StartCoroutine (coGameState ());
//		yield return 0;
//	}
//	
//	// Update is called once per frame
//	void Update ()
//	{
//		//test ###
//		if (Input.GetKeyDown(KeyCode.A)) {
//		}
//		if (Input.GetKeyDown(KeyCode.S)) {
//		}
//		if (Input.GetKeyDown(KeyCode.D)) {
//		}
//		if (Input.GetKeyDown(KeyCode.F)) {
//		}
//		
//		tmHeroState.text = hero.state.GetType().FullName;
//		tmHeroState.Commit();
//		
//		if(Input.GetKeyDown(KeyCode.A)){
//			Debug.LogError("size:" + hero.gameObject.renderer.bounds.size);
//			Debug.LogError("GetWorldPos" + hero.GetWorldPos());//#######
//		}
//		Vector3 pos = camera_bg.ScreenToWorldPoint(Input.mousePosition);
//	}
//	
//	protected IEnumerator coGameState ()
//	{
//		while (isRunning) {
//			if (gameStateQueue.Count > 0) {
//				GameStateObj mObj = (GameStateObj)gameStateQueue.Dequeue ();
//				Switch (mObj);
//			}
//			yield return 0;
//		}
//	}
//	#region Switch
//	protected virtual void  Switch (GameStateObj obj)
//	{
//		
//		int gameState = obj.mGameState;
//		JsonData data = obj.mData;
//		
//		switch (gameState) {
//			default:
//				break;
//		}
//	}
//	#endregion
//	
//	 #region General_Method
//	IEnumerator CoGeneral_TimeText_Updata (tk2dTextMesh tmTimeValue, int time) {
//		if(tmTimeValue != null){
//			do {
//				if(tmTimeValue == null){
//					break;
//				}else{
//					// set text
//					tmTimeValue.text = Tools.FormatTime(time);
//					tmTimeValue.Commit();
//				}
//				yield return new WaitForSeconds(1f);
//				time --;
//			} while (time > 0);
//		}
//	}
////	private IEnumerator CoStartCameraShake(){
////		Vector3 posOri = camera.transform.position;
////		camera.transform.position = posOri + new Vector3(0.1f, 0.1f, 0);
////		yield return 0;
////		camera.transform.position = posOri;
////	} 
//	#endregion
//	
//	#region game method
//	public Rect GetRectById(int id){
//		if(id == IConst.RectId_UP){
//			return g_RectUpCE;
//		}else if(id == IConst.RectId_Down){
//			return g_RectDownCE;
//		}else if(id == IConst.RectId_Left){
//			return g_RectLeftCE;
//		}else if(id == IConst.RectId_Right){
//			return g_RectRightCE;
//		}else{
//			return g_RectRightCE;
//		}
//	}
//	
//	private void InitLevelData(){
//		string strJson = (Resources.Load("GameData/level") as TextAsset).text;
//		JsonData jdLevels = JsonMapper.ToObject(strJson);
//		g_ArrLevels = new Level[jdLevels.Count];
//		for (int i = 0; i < jdLevels.Count; i++) {
//			JsonData jdLevel = jdLevels[i];
//			Level level = new Level();
//			level.levelid = (int)jdLevel["levelid"];
//			level.levelname = (string)jdLevel["levelname"];
//			// set ce group queue
//			level.groupqueue = new Queue();
//			JsonData jdGroupQueue = jdLevel["groupqueue"];
//			for (int groupindex = 0; groupindex < jdGroupQueue.Count; groupindex++) {
//				CEGroup cegroup = new CEGroup();
//				cegroup.groupid = (int)jdGroupQueue[groupindex]["groupid"];
//				cegroup.nextgrouptime = (double)jdGroupQueue[groupindex]["nextgrouptime"];
//				cegroup.cequeue = new Queue();
//				// set ce queue
//				JsonData jdCequeue = jdGroupQueue[groupindex]["cequeue"];
//				for (int ceIndex = 0; ceIndex < jdCequeue.Count; ceIndex++) {
//						CENode cenode = JsonMapper.ToObject<CENode>(jdCequeue[ceIndex].ToJson()); 
//						cegroup.cequeue.Enqueue(cenode);
//				}
//				level.groupqueue.Enqueue(cegroup);
//			}
//			g_ArrLevels[i] = level;
//		}
//	}
//	
//	private void OnFgMoveEnd(GameObject gobj){
//		gobj.transform.position = new Vector3(1.67f, -0.27f, -0.06f);
//		iTween.MoveTo(gobj, iTween.Hash("position", new Vector3(-5.12f, -0.28f, -0.06f), "speed", fgSpeed, "easetype", "linear","oncomplete", "OnFgMoveEnd", "oncompletetarget", gameObject, "oncompleteparams", gobj));
//	}
//	#endregion
//	
//	#region Enermy Manager
//	private IEnumerator CoEnermyCreate(){
//		while(isRunning){
//			Level level = g_ArrLevels[curlevelId];
//			if(level.groupqueue.Count > 0){
//				// create group enermy
//				CEGroup cegroup = (CEGroup)level.groupqueue.Dequeue();
//				while(cegroup.cequeue.Count > 0){
//					CENode cenode = (CENode)cegroup.cequeue.Dequeue();
//					for (int i = 0; i < cenode.repeat; i++) {
//						// cerete enermy
//						Vector3 pos;
//						if(cenode.rectid > 0){
//							Rect rect = GetRectById(cenode.rectid);
//							pos = Tools.GetRandmonPosInRect(rect, camera_bg);
//						}else{
//							pos = new Vector3((float)cenode.x, (float)cenode.y, 0);
//						}
//						CreateEnermyById(cenode.enermyid, pos);
//						// wait interval
//						yield return new WaitForSeconds((float)cenode.interval);
//					}
//					// wait next node time
//					yield return new WaitForSeconds((float)cenode.nextnodetime);
//				}
//			}else{
//				Debug.Log("enermy create end");
//				break;
//			}
//		}
//	}
//	
//	private void CreateEnermyById(int id, Vector3 v3pos){
//		string gobjPath = "";
//		if (id == 1) { // bat
//			gobjPath = IPath.Path_Enermy + "airplane";
//		}else if(id == 2){
//			gobjPath = IPath.Path_Enermy + "guided";
//		}else if(id == 3){
//			gobjPath = IPath.Path_Enermy + "bred";
//		}else if(id == 4){
//			gobjPath = IPath.Path_Enermy + "twop";
//		}
//		
//		// ui init
//		GameObject gobjEnermy = Tools.LoadResourcesGameObject(gobjPath);
//		gobjEnermy.transform.position = v3pos;
//		if (id == 1) { // bat
//		}else if(id == 2){
//		}else if(id == 3){
//			gobjEnermy.AddComponent<Com_Bred>();
//		}else if(id == 4){
//			gobjEnermy.AddComponent<Com_Twop>();
//		}
//	}
//	
//	private void DoEnermyDie(Enermy enermy){
//		
//	}
//	#endregion
//	
//	#region Effect Manager
//	public GameObject CreateEffAtPos(string aniName, Vector2 v2Pos){
//		GameObject effect = Tools.LoadResourcesGameObject(IPath.Path_AniEffect);
//		effect.transform.position = new Vector3(v2Pos.x, v2Pos.y, -0.1f);
//		if(effect != null){
//			tk2dAnimatedSprite ani = effect.GetComponent<tk2dAnimatedSprite>();
//			ani.Play(aniName);
//			ani.animationCompleteDelegate = OnEffectAniComplete;
//		}
//		return effect;
//	}
//	
//	public void OnEffectAniComplete(tk2dAnimatedSprite sprite, int clipId){
//		DestroyObject(sprite.gameObject); 
//	}
//	#endregion
//	
//	#region battle
//	public void DoHeroUnderFire(){
//		
//	}
//	
////	public void HeroChangeHp(int ivar){
////		// data
////		g_Hero.hpCur += ivar;
////		if(g_Hero.hpCur > g_Hero.hpMax){
////			g_Hero.hpCur = g_Hero.hpMax;
////		}
////		if(g_Hero.hpCur > 0){
////			// ui
////			for (int i = 0; i < g_Hero.hpMax; i++) {
////				GameObject gobjHp = Tools.GetTransformInChildByPath(g_GobjMainHead, "hp" + i).gameObject;
////				if(i >= g_Hero.hpCur){
////					Tools.SetGameObjectSprite(gobjHp, "hp_none");
////				}else{
////					Tools.SetGameObjectSprite(gobjHp, "hp_full");
////				}
////			}
////		}else{
////			// do hero die
////		}
////	}
//	
//	// by 1
////	public void HeroAddLife(bool isAdd){
////		// data
////		if(isAdd){
////			g_Hero.lifes ++;
////		}else{
////			g_Hero.lifes --;
////		}
////		
////		if(g_Hero.lifes > 0){
////			// ui
////			if(isAdd){
////				GameObject gobjHp = Tools.LoadResourcesGameObject(IPath.Path_MainHead + "hp");
////				gobjHp.transform.position = new Vector3(-1.1f + ((g_Hero.lifes - 1) * 0.1f), 0.85f, 0f);
////				gobjHp.transform.parent = g_GobjMainHead.transform;
////				gobjHp.name = "hp" + (g_Hero.lifes - 1);	
////			}else{
////				GameObject gobjHp = Tools.GetTransformInChildByPath(g_GobjMainHead, "hp" + g_Hero.lifes).gameObject;
////				if(gobjHp != null){
////					DestroyObject(gobjHp);
////				}
////			}
////			
////		}else{
////			// do game over
////			Debug.Log("die");
////		}
////		
////	}
//	#endregion
//	
//	#region Actor Manage
//	public void AddActor(IActor actor){
//		actor.id = ++ g_EnermyId;
//		g_DicActors.Add(actor.id, actor);
//	}
//	#endregion
//	
//	public void PlaySound(string clipname, bool isLoop){
//		AudioClip audioClip = Resources.Load("Sounds/" + clipname) as AudioClip;
//		if(audioClip != null){
//			AudioSource soundPlayer = GetComponent<AudioSource>();
//			soundPlayer.clip = audioClip;
//			soundPlayer.loop = isLoop;
//			soundPlayer.Play();
//		}
//	}
//}
