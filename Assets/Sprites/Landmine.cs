using UnityEngine;
using System.Collections;

public class Landmine : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.CompareTag("enermy")){
			StartCoroutine(CoExplode());
		}
	}
	
	IEnumerator CoExplode(){
		// show damage
		GameObject gobjDamage = Tools.GetGameObjectInChildByPathSimple(gameObject, "damage");
		gobjDamage.SetActive(true);
		yield return 1;
		// kill self
		DestroyObject(gameObject);
	}
}
