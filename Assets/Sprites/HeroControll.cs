using UnityEngine;
using System.Collections;

public enum State{
	
}

public class HeroControll : MonoBehaviour {
	
	public float speed = 3.0f;
	
	public float jumpSpeed = 8.0f;
	
	public float gravity = 20.0f;
	
	private CharacterController cc;
	
	private Vector2 moveDir = Vector2.zero;
	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		float axisH = Input.GetAxis("Horizontal");
		float axisV = 0f;
		
		moveDir.x = axisH * speed;
		
		if(cc.isGrounded){
			if(Input.GetAxis("Jump") > 0){
				moveDir.y = jumpSpeed;
			}
		}else{
			moveDir.y -= gravity * Time.deltaTime;
		}
		
		cc.Move(moveDir * Time.deltaTime);
		//
		//
	}
}
