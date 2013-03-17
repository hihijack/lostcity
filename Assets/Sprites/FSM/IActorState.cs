using UnityEngine;
using System.Collections;

public class IActorState{
    public IActor actor;
    public float time;
    public IActorState() {}
    public virtual IActorState toNextState(EFSMAction action) { return null; }
    public virtual void OnEnter() { }
    public virtual void DoUpdata() { }
}



//public class ActorState_Move : IActorState {
//    public ActorState_Move(IActor actor)
//    {
//        this.actor = actor;
//    }
//
//    public override IActorState toNextState(EFSMAction action)
//    {
//        IActorState result = null;
//        if(action == EFSMAction.ATTACK_PLAYER){
//            result = new ActorState_AttackPlayer(actor);
//        }else if(action == EFSMAction.UNATTACK){
//			result = new ActorState_UnAttack(actor);
//		}else if(action == EFSMAction.UNATTACK_BY_FLASH){
//			result = new ActorState_UnAttack_By_Flash(actor);
//		}
//        return result;
//    }
//
//    public override void OnEnter()
//    {
//        
//    }
//
//    public override void DoUpdata()
//    {
//        actor.DoUpdateMove();
//    }
//}
//
//public class ActorState_AttackPlayer : IActorState{
//    public ActorState_AttackPlayer(IActor actor)
//    {
//        this.actor = actor;
//    }
//
//    public override IActorState toNextState(EFSMAction action)
//    {
//       IActorState result = null;
//        if(action == EFSMAction.IDLE){
//            result = new ActorState_Idle(actor);
//        }
//        return result;
//    }
//
//    public override void OnEnter()
//    {
//       actor.OnEnterAttack();
//    }
//
//    public override void DoUpdata()
//    {
////        time += Time.deltaTime;
////        enermy.DoUpdateAttack();
//    }
//}
//
//public class ActorState_Idle : IActorState {
//    public ActorState_Idle(IActor actor)
//    {
//        this.actor = actor;
//    }
//
//    public override IActorState toNextState(EFSMAction action)
//    {
//        IActorState result = null;
//        if(action == EFSMAction.MOVE){
////            result = new ActorState_Move(actor);
//        }
//        return result;
//    }
//
//    public override void DoUpdata()
//    {
//        actor.DoUpdateIdle();
//    }
//	
//	public override void OnEnter ()
//	{
//		actor.OnEnterIdle();
//	}
//}
//
//public class HeroActorState_Flash : IActorState{
//	public HeroActorState_Flash(IActor actor){
//		this.actor = actor;
//	}
//	
//	public override IActorState toNextState (EFSMAction action)
//	{
//		IActorState result = null;
//		if(action == EFSMAction.HERO_IDLE){
//			result = new HeroActorState_Idle(actor);
//		}else if(action == EFSMAction.HERO_FLASH_ATTACK){
//			result = new HeroActorState_Flash_Attack(actor);
//		}
//		return result;
//	}
//	
//	public override void OnEnter ()
//	{
//		actor.OnEnterHeroFlash();
//	}
//	
//	public override void DoUpdata ()
//	{
//		actor.DoUpdateHeroFlash();
//	}
//}
//
public class HeroActorState_Idle : IActorState{
	public HeroActorState_Idle(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EFSMAction action)
	{
		IActorState result = null;
		if(action == EFSMAction.HERO_RUN){
			result = new HeroActorState_Run(actor);
		}else if(action == EFSMAction.HERO_ONAIR_DOWN){
			result = new HeroActorState_OnAir_Down(actor);
		}
		return result;
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdateIdle();
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterIdle();
	}
}


public class HeroActorState_Run : IActorState{
	public HeroActorState_Run(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EFSMAction action)
	{
		IActorState result = null;
		if(action == EFSMAction.HERO_IDLE){
			result = new HeroActorState_Idle(actor);
		}
		return result;
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdateRun();
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterRun();
	}
}

public class HeroActorState_OnAir_Down : IActorState{
	public HeroActorState_OnAir_Down(IActor actor){
		this.actor = actor;
	}
	
	public override IActorState toNextState (EFSMAction action)
	{
		IActorState result = null;
		return result;
	}
	
	public override void DoUpdata ()
	{
		actor.DoUpdateOnAirDown();
	}
	
	public override void OnEnter ()
	{
		actor.OnEnterOnAirDown();
	}
}
//
//public class HeroActorState_UnAttack : IActorState{
//	public HeroActorState_UnAttack(IActor actor){
//		this.actor = actor;
//	}
//	
//	public override IActorState toNextState (EFSMAction action)
//	{
//		IActorState result = null;
//		if(action == EFSMAction.HERO_IDLE){
//			result = new HeroActorState_Idle(actor);
//		}
//		return result;
//	}
//	
//	public override void OnEnter ()
//	{
//		actor.OnEnterUnAttack();
//	}
//}
//
//public class HeroActorState_Flash_Attack : IActorState{
//	public HeroActorState_Flash_Attack(IActor actor){
//		this.actor = actor;
//	}
//	
//	public override IActorState toNextState (EFSMAction action)
//	{
//		IActorState result = null;
//		if(action == EFSMAction.HERO_IDLE){
//			result = new HeroActorState_Idle(actor);
//		}
//		return result;
//	}
//	
//	public override void OnEnter ()
//	{
//		actor.OnEnterHeroFlashAttack();
//	}
//}
//
//public class ActorState_UnAttack : IActorState{
//	public ActorState_UnAttack(IActor actor){
//		this.actor = actor;
//	}
//	
//	public override IActorState toNextState (EFSMAction action)
//	{
//		IActorState result = null;
//		if(action == EFSMAction.IDLE){
//			result = new ActorState_Idle(actor);
//		}
//		return result;
//	}
//	
//	public override void OnEnter ()
//	{
//		actor.OnEnterUnAttack();
//	}
//}
//
//public class ActorState_UnAttack_By_Flash : IActorState{
//	public ActorState_UnAttack_By_Flash(IActor actor){
//		this.actor = actor;
//	}
//	
//	public override IActorState toNextState (EFSMAction action)
//	{
//		IActorState result = null;
//		if(action == EFSMAction.IDLE){
//			result = new ActorState_Idle(actor);
//		}
//		return result;
//	}
//	
//	public override void OnEnter ()
//	{
//		actor.OnEnterUnAttack_By_Flash();
//	}
//}
