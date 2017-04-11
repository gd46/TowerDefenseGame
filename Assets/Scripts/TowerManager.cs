using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerManager : Singleton<TowerManager> {

	public TowerBtn towerBtnPressed{get; set;}
	private SpriteRenderer spriteRenderer;
	private List<Tower> TowerList = new List<Tower>();
	private Collider2D buildTile;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		buildTile = GetComponent<Collider2D>();
	}

	// Update is called once per frame
	void Update () {
		// 0 left mouse press 1 right
		if(Input.GetMouseButtonDown(0)) {
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			placeTower(hit);
		}
		if(spriteRenderer.enabled) {
			SpriteRenderManager.Instance.followMouse();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) {
			handleEscape();
		}
	}

	private void handleEscape() {
		SpriteRenderManager.Instance.disableDragSprite();
		towerBtnPressed = null;
	}

	private void RegisterTower(Tower tower) {
		TowerList.Add(tower);
	}

	public void DestroyAllTowers() {
		foreach(Tower tower in TowerList) {
			 Destroy(tower.gameObject);
		}
		TowerList.Clear();
	}

	private void placeTower(RaycastHit2D hit) {
		if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null) {
			if(hit.collider.tag == "BuildSite") {
				if(towerBtnPressed.Price <= GameManager.Instance.TotalMoney) {
					//Needed to explictly cast using (Tower) or us as Tower
					Tower newTower = Instantiate (towerBtnPressed.TowerObject) as Tower;
					newTower.transform.position = hit.transform.position;
					buyTower(towerBtnPressed.Price);
					GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
					RegisterTower(newTower);
					disableDragSprite();
					// Rename tag to prevent multiple towers being built on one build site
					buildTile = hit.collider;
					BuildSiteManager.Instance.RenameTagBuildSiteFull(buildTile);
				}
			}
		}
	}

	private void buyTower(int price) {
		GameManager.Instance.subtractMoney(price);
	}

	public void selectedTower (TowerBtn towerSelected) {
		if(towerSelected.Price <= GameManager.Instance.TotalMoney) {
			towerBtnPressed = towerSelected;
			SpriteRenderManager.Instance.enableDragSprite(towerBtnPressed.DragSprite);
		} else {
			disableDragSprite();
		}
	}

	private void disableDragSprite() {
		SpriteRenderManager.Instance.disableDragSprite();
		towerBtnPressed = null;
	}
}
