using UnityEngine;

public class GamePieceBtn : MonoBehaviour {

	[SerializeField]
	private Sprite dragSprite;
	[SerializeField]
	private int price;

	// Use this for initialization
	public Sprite DragSprite {
		get {
			return dragSprite;
		}
	}

	public int Price {
		get {
			return price;
		}
	}
}
