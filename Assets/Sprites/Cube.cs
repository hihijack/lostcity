using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {
	public bool canDestory;
	
	public void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("damage")){
			if(canDestory){
				DestroyObject(gameObject);
			}
		}
	}
}
