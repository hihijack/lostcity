using UnityEngine;
using System.Collections;

public class BtnPagHandle : MonoBehaviour {

	// Use this for initialization
	GameView gameView;
	void Start () {
		gameView = GameObject.Find("CPU").GetComponent<GameView>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick(){
		gameView.OnBtnPagClick();
	}
}
