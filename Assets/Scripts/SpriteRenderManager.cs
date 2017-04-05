using UnityEngine;

public class SpriteRenderManager : Singleton<SpriteRenderManager> {

	private SpriteRenderer spriteRenderer;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void followMouse () {
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector2(transform.position.x, transform.position.y);
	}

	public void enableDragSprite (Sprite sprite) {
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = sprite;
	}

	public void disableDragSprite () {
		spriteRenderer.enabled = false;
	}
}
