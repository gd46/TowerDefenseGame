using UnityEngine;

public class BuildSiteBtn : GamePieceBtn {

	[SerializeField]
	private GameObject buildSiteObject;
	
	public GameObject BuildSiteObject {
		get {
			return buildSiteObject;
		}
	}
}
