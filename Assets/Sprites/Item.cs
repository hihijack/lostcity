using UnityEngine;
using System.Collections;

public enum EItemType{
	None,
	LandMine
}

public class Item {
	public EItemType itemType;
	public string describe;
	public int num;
	public string iconName;
	public bool canUse;
	
	public Item(EItemType itemType){
		this.itemType = itemType;
		this.num = 1;
		switch (itemType) {
			case EItemType.LandMine:{
				this.describe = IText.Des_Item_Landmine;
				this.iconName = "INV_Misc_Bomb_09";
				this.canUse = true;
			}
				break;
			default:
			break;
		}
	}
}
