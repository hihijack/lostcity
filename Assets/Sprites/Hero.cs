using UnityEngine;
using System.Collections;

//public enum HeroState{
//	Idle,
//	Run,
//	JumpUp,
//	JumpDown
//}

public class Hero : IActor
{
	
	public float speed = 10.0f;
	
	public float jumpSpeed = 8.0f;
	
	public float gravity = 20.0f;
	
	public float pushPow = 1f;
	
	private CharacterController cc;
	
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
		
		this.state.DoUpdata();
	}
	
//	public void OnHeroMoveEnd(){
//		updataState(new IActorAction(EFSMAction.HERO_IDLE));
//	}
//	
//	public override void OnEnterUnAttack ()
//	{
//		ActionHeroUnAttack actionUnAttack = (ActionHeroUnAttack)this.action;
//		Enermy attacker = actionUnAttack.attacker;
//		if(ani_sprite.scale.x * attacker.ani_sprite.scale.x > 0){
//			ani_sprite.Play("hurt_back");
//		}else{
//			ani_sprite.Play("hurt");
//		}
//		ani_sprite.animationCompleteDelegate = OnAniEnd;
//	}
//	
//	public void OnAniEnd(tk2dAnimatedSprite aniSprite, int clipid){
//		updataState(new IActorAction(EFSMAction.HERO_IDLE));
//		ani_sprite.animationCompleteDelegate = null;
//	}
//	
//	
//	
	public override void DoUpdateIdle ()
	{
		if(!cc.isGrounded){
			updataState(new IActorAction(EFSMAction.HERO_ONAIR_DOWN));
		}else{
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
		moveDir.y = jumpSpeed;
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
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic)
			return;

		if (hit.moveDirection.y < -0.3F)
			return;

		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, 0);
		Vector3 v = pushDir * pushPow;
		body.velocity = pushDir * pushPow;
	}
	
	
	//	
//	public override void OnEnterHeroFlash ()
//	{
//		Vector2 pos = this.GetWorldPos();
//		ActionHeroFlash flashAction = (ActionHeroFlash)this.action;
//		if(this.ani_sprite.scale.x *(flashAction.x - pos.x) < 0){
//			ani_sprite.MyFlipX();
//		}
//		GameObject gobjEff = gameView.CreateEffAtPos("flash", pos);
////		Vector2 target = new Vector2(flashAction.x - pos.x, flashAction.y - pos.y);
////		float angle = Vector2.Angle(Vector2.right, target);
////		gobjEff.transform.eulerAngles = new Vector3(1f, 1f, angle);
//		
//		ani_sprite.Play("flash");
//	}
//	
//	public override void DoUpdateHeroFlash ()
//	{
//		GameObject[] gobjEnermys = GameObject.FindGameObjectsWithTag("enermy");
//		foreach(GameObject gobjEnermy in gobjEnermys){
//			if(Tools.IsCollide(gameObject, gobjEnermy)){
//				Enermy enermy = gobjEnermy.GetComponent<Enermy>();
//				Vector2 posCur = GetWorldPos();
//				ActionHeroFlash actionF = (ActionHeroFlash)action;
//				Vector2 posStart = new Vector2(actionF.xStart, actionF.yStart);
//				Vector2 v2 = posCur - posStart;
//				float distanceFlash = v2.magnitude;
////				Debug.LogError("distanceFlash" + distanceFlash);//#####
//				if(distanceFlash > 120){
//					IActorAction actionnew = new ActonHeroFlashAttack(enermy);
//					updataState(actionnew);
//				}
//			}
//		}
//	}
//	
//	public override void OnEnterHeroFlashAttack ()
//	{
//		// stop flash
//		iTween.Stop(gameObject);
//		
//		ani_sprite.Play("flash_attack");
//		ani_sprite.animationCompleteDelegate = OnAniEnd;
//		
//		gameView.CreateEffAtPos("hit", GetWorldPos());
//		gameView.PlaySound("hit1", false);
//		
//		ActonHeroFlashAttack action = (ActonHeroFlashAttack)this.action;
//		action.attackeder.updataState(new ActionUnAttack_By_Flash(this));
//	}
}
