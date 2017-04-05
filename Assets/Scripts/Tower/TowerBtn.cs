using UnityEngine;

public class TowerBtn : GamePieceBtn {

	[SerializeField]
	private Tower towerObject;
	
	public Tower TowerObject {
		get {
			return towerObject;
		}
	}
}
