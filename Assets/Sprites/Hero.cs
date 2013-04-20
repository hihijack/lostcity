using UnityEngine;
using System.Collections;
using System.Reflection;

public class Hero : IActor
{
	
	public float speed = 10.0f;
	
	public float jumpSpeed = 8.0f;
	
	public float gravity = 20.0f;
	
	public float pushPow = 1f;
	
	private CharacterController cc;
	
	private GameObject g_gobjCurStepOn;
	
	private Vector2 moveDir = Vector2.zero;
	
	GameView gameView;
	
	int axisH = 0;
	int btnA = 0;
	int btnB = 0;
	
	void Start(){
		cc = GetComponent<CharacterController>();
		ani_sprite = GetComponent<tk2dAnimatedSprite>();
		state = new HeroActorState_Idle(this);
		gameView = GameObject.Find("CPU").GetComponent<GameView>();
	}
	
	void Update(){
		axisH = gameView.VCInput_Axis;
		btnA = gameView.VCInput_BtnA;
		btnB = gameView.VCInput_BtnB;
		moveDir.x = 0f;
		this.state.DoUpdata();
	}
	
	void OnGUI(){
		GUI.Label(new Rect(50, 50, 300, 20), state.ToString());
		GUI.Label(new Rect(50, 100, 300, 20), moveDir.ToString());
		GUI.Label(new Rect(50, 150, 300, 20), gameView.VCInput_Axis.ToString());
	}
	
	public override void DoUpdateIdle ()
	{
		if(!cc.isGrounded){
			updataState(new IActorAction(EFSMAction.HERO_ONAIR_DOWN));
		}else{
			
			InteractiveHandle();
		
			if(axisH != 0){
				if(IsHitSomeThing()){
					
				}else{
					updataState(new IActorAction(EFSMAction.HERO_RUN));
				}
			}else if(btnA > 0){
				updataState(new IActorAction(EFSMAction.HERO_ONAIR_UP));
			}
		}
	}
	
	public override void DoUpdateRun ()
	{
		if(cc.isGrounded){
			if(axisH != 0){
				moveDir.x = axisH * speed;
				cc.Move(moveDir * Time.deltaTime);
				if(axisH > 0){
					SetFace(true);
				}else{
					SetFace(false);
				}
			}else{
				updataState(new IActorAction(EFSMAction.HERO_IDLE));
			}
			
			if(btnA > 0){
				updataState(new IActorAction(EFSMAction.HERO_ONAIR_UP));
			}	
		}else{
			updataState(new IActorAction(EFSMAction.HERO_ONAIR_DOWN));
		}
		
	}
	
	public override void OnEnterIdle ()
	{
		ani_sprite.Play("idle");
	}
	
	public override void OnEnterRun ()
	{
		ani_sprite.Play("run");
	}
	
	public override void OnEnterOnAirDown ()
	{
		ani_sprite.Play("jump_down");
	}
	
	public override void DoUpdateOnAirDown ()
	{
		moveDir.y -= gravity * Time.deltaTime;
		if(axisH != 0){
			moveDir.x = axisH * speed;
			if(axisH > 0){
				SetFace(true);
			}else{
				SetFace(false);
			}
		}
		cc.Move(moveDir * Time.deltaTime);
		if(cc.isGrounded){
			updataState(new IActorAction(EFSMAction.HERO_IDLE));
		}
	}
	
	public override void OnEnterOnAirUp ()
	{
		ani_sprite.Play("jump_up");
		float jumpSpeedTemp = jumpSpeed;
		GameObject gobjStepOn = GetSpetOnGameObject();
		ERuneStoneType runeStoneStepOnType = GetRuneStoneType(gobjStepOn);
		if(runeStoneStepOnType == ERuneStoneType.JUMP){
			jumpSpeedTemp *= IConst.RuneStone_Jump_Rate;
		}
		moveDir.y = jumpSpeedTemp;
	} 
	
	public override void DoUpdateOnAirUp ()
	{
		moveDir.y -= gravity * Time.deltaTime;
		if(axisH != 0){
			moveDir.x = axisH * speed;
			if(axisH > 0){
				SetFace(true);
			}else{
				SetFace(false);
			}
		}
		if(moveDir.y > 0){
			cc.Move(moveDir * Time.deltaTime);
		}else{
			updataState(new IActorAction(EFSMAction.HERO_ONAIR_DOWN));
		}
	}
	
	public void SetFace(bool isRigth){
		if(isRigth && ani_sprite.scale.x < 0){
			ani_sprite.FlipX();
		}else if(!isRigth && ani_sprite.scale.x > 0){
			ani_sprite.FlipX();
		}
	}
	
	public bool IsHitSomeThing(){
		return false;
	}
	
	public GameObject GetCurBGGameObject(){
		GameObject gobjR = null;
		RaycastHit[] hits;
		Vector3 posOri = gameView.main_camera.transform.position;
		Vector3 direction  = Vector3.forward;
		hits = Physics.RaycastAll(posOri, direction, 100.0f);
		if(hits.Length == 2 && hits[1].transform.name.Equals("hero")){
			gobjR = hits[0].transform.gameObject;
		}
		return gobjR;
	}
	
	public GameObject GetSpetOnGameObject(){
		return g_gobjCurStepOn;
	}
	#region Game Mehtods
	public void InteractiveHandle(){
		GameObject gobjInteractive =  GetCurBGGameObject();
		if(gobjInteractive != null){
			string name = gobjInteractive.name;
			if(btnB > 0){
				// call event handle func
				string funcName = "InteractiveEventHandleByGameObjName_" + name;
				MethodInfo  mi = this.GetType().GetMethod(funcName);
				if(mi != null){
					mi.Invoke(this, null);
				}else{
					Debug.LogError("can't find method:" + funcName);
				}
			}
		}
	}
	#endregion
	
	#region InteractiveEventHandle
	public void InteractiveEventHandleByGameObjName_Rune_1(){
		GameObject gobj1 = GameObject.Find("Cube_2");
		GameObject gobj2 = GameObject.Find("Cube_3");
		GameObject pos1 = GameObject.Find("pos_2");
		GameObject pos2 = GameObject.Find("pos_3");
		Vector3 v3Pos1 = pos1.transform.position;
		Vector3 v3Pos2 = pos2.transform.position;
		iTween.MoveTo(gobj1, v3Pos1, 1f);
		iTween.RotateTo(gobj1, Vector3.zero, 1f);
		iTween.MoveTo(gobj2, v3Pos2, 1f);
		iTween.RotateTo(gobj2, Vector3.zero, 1f);
	}
	
	public void InteractiveEventHandleByGameObjName_Rune_2(){
		GameObject gobj4 = GameObject.Find("Cube_4");
		GameObject gobj5 = GameObject.Find("Cube_5");
		GameObject gobj6 = GameObject.Find("Cube_6");
		GameObject gobj7 = GameObject.Find("Cube_7");
		GameObject gobj8 = GameObject.Find("Cube_8");
		GameObject pos4 = GameObject.Find("pos_4");
		GameObject pos5 = GameObject.Find("pos_5");
		GameObject pos6 = GameObject.Find("pos_6");
		GameObject pos7 = GameObject.Find("pos_7");
		GameObject pos8 = GameObject.Find("pos_8");
		iTween.MoveTo(gobj4, pos4.transform.position, 1.5f);
		iTween.RotateTo(gobj4, Vector3.zero, 1f);
		iTween.MoveTo(gobj5, pos5.transform.position, 1.5f);
		iTween.RotateTo(gobj5, Vector3.zero, 1f);
		iTween.MoveTo(gobj6, pos6.transform.position, 1.5f);
		iTween.RotateTo(gobj6, Vector3.zero, 1f);
		iTween.MoveTo(gobj7, pos7.transform.position, 1.5f);
		iTween.RotateTo(gobj7, Vector3.zero, 1f);
		iTween.MoveTo(gobj8, pos8.transform.position, 1.5f);
		iTween.RotateTo(gobj8, Vector3.zero, 1f);
	}
	#endregion
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.moveDirection.y < 0 && Mathf.Abs(hit.moveDirection.x) < 0.3f ){
			g_gobjCurStepOn = hit.gameObject;
		}
	}
	
	ERuneStoneType GetRuneStoneType(GameObject gobj){
		ERuneStoneType r = ERuneStoneType.NONE;
		if(gobj != null){
			string name = gobj.name;
			if(name.Contains("RuneStone")){
				Material material = gobj.renderer.material;
				if(material.name.Contains("jump")){
					r = ERuneStoneType.JUMP;
				}
			}
		}
		return r;
	}
	
}
