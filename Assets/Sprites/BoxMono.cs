using UnityEngine;
using System.Collections;

public class BoxMono : Enermy {

//	public 
	// Use this for initialization
	void Start () {
		
		base.Start();
		
		this.actor_type = EActorType.BoxMono;
		this.hp = 1;
		this.moveSpeed = 1;
		this.moveDir = new Vector3(-1f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(!isDead){
			// move left
			gameObject.transform.Translate(this.moveSpeed * this.moveDir * Time.deltaTime);
		} 
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
		// if hit palyer, kill player
		// if hit other actor or collider, move reverse
		Debug.LogError(hit.gameObject.name);
		Debug.LogError(hit.moveDirection);//########
		if(Mathf.Abs(hit.moveDirection.y) <= 0.3){
			IActor actorHit = hit.gameObject.GetComponent<Hero>();
			if(actorHit != null){
				if(!actorHit.isEnermy){
					// kill player
					Debug.LogError("0000");
				}else{
					Debug.LogError("111");
					this.moveDir *= -1;
					
				}
			}else{
				Debug.LogError("222");
				this.moveDir *= -1;
			}
		}
	}
	void OnCollisionEnter(Collision collision) {
		Debug.LogError("OnCollisionEnter");
		Debug.LogError(collision.gameObject.name);
		Debug.LogError(collision.relativeVelocity);
		Debug.LogError(collision.collider);
		// if is head hited, killed
		// else if hit player , kill player
		// else if hit wall or other acotr, change direction
		bool isHeadHited = false;
		bool isHitPlayerWithoutHead = false;
		bool isHitOther = false;
		foreach (ContactPoint contact in collision.contacts) {
			string thisCollider = contact.thisCollider.name;
			string otherCollider = contact.otherCollider.name;
			print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
			Debug.DrawRay(contact.point, contact.normal, Color.red);
			if(thisCollider.Equals("head")){
				isHeadHited = true;
				isHitOther = false;
				break;
			}else if(otherCollider.Equals("hero")){
				isHitPlayerWithoutHead = true;
				isHitOther = false;
				break;
			}else if(thisCollider.Equals("left") || thisCollider.Equals("right")){
				isHitOther = true;
			}
			
		}
		
		if(isHeadHited){
			// killed
			DestroyObject(gameObject);
		}else if(isHitPlayerWithoutHead){
			// kill player
			Debug.LogError("kill player");
		}else if(isHitOther){
			this.moveDir *= -1;
		}
		

	}
	
	
}
