using UnityEngine;
using System.Collections;

public enum EViewType{
	A,
	B
}

public class CameraControll : MonoBehaviour {

	public GameObject target;
	public EViewType viewType;
	
	// Use this for initialization
	void Start () {
		viewType = EViewType.A;
	}
	
	// Update is called once per frame
	void Update () {
		float x = target.transform.position.x;
		float y = target.transform.position.y;
		float z = target.transform.position.z;
		float xSelf = transform.position.x;
		float ySelf = transform.position.y;
		float zSelf = transform.position.z;
		if(viewType == EViewType.A){
			transform.position = new Vector3(x, y, zSelf);
		}else if(viewType == EViewType.B){
			transform.position = new Vector3(xSelf, y, z);
		}
//		if(Input.GetKeyDown(KeyCode.R)){
//			if(viewType == EViewType.A){
//				viewType = EViewType.B;
//				transform.RotateAround(target.transform.position, 90);
//			}else{
//				viewType = EViewType.A;
//				transform.RotateAround(target.transform.position, -90);
//			}
//		}
	}
}
