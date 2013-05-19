using UnityEngine;
using System.Collections;

public class Enermy : IActor
{
	public string name;
	public int hp;
	public bool isDead;
	public float moveSpeed;
	public GameView gameView;
	public Vector3 moveDir;
	
	public void Start(){
		gameView = GameObject.Find("CPU").GetComponent<GameView>();
		this.isEnermy = true;
		this.isDead = false;
	}
	
//	public virtual void DoEnermyMoveTargetPlayer(GameObject gobjHero){
//		Vector2 posHero = gobjHero.transform.position;
//		Vector2 posEnermy = gameObject.transform.position;
//		Vector2 v2 = Vector2.ClampMagnitude(posHero - posEnermy, speed *  Time.deltaTime);
//		Vector3 v3 = v2;
//		gameObject.transform.position +=  v3;
//			
//		// set facing
//		if(v2.x * ani_sprite.scale.x < 0){
//			ani_sprite.FlipX();
//		}
//	}
}

