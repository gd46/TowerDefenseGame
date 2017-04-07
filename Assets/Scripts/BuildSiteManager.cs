using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BuildSiteManager : Singleton<BuildSiteManager> {


	public BuildSiteBtn buildSiteBtnPressed{get; set;}
	private SpriteRenderer spriteRenderer;
	private List<Collider2D> BuildColliderList = new List<Collider2D>();

	private List<GameObject> BuildSiteList = new List<GameObject>();

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update () {
		// 0 left mouse press 1 right
		if(Input.GetMouseButtonDown(0)) {
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			placeBuildSite(hit);
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
		buildSiteBtnPressed = null;
	}

	private void placeBuildSite(RaycastHit2D hit) {
		if(!EventSystem.current.IsPointerOverGameObject() && buildSiteBtnPressed != null) {
			if(hit.collider.tag == "Ground") {
				if(buildSiteBtnPressed.Price <= GameManager.Instance.TotalMoney) {
					GameObject buildSite = Instantiate (buildSiteBtnPressed.BuildSiteObject) as GameObject;
					buildSite.transform.position = hit.transform.position;
					buyBuildSite(buildSiteBtnPressed.Price);
					RegisterBuildSite(buildSite);
					GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
					disableDragSprite();
				}
			}
		}
	}

	private void buyBuildSite(int price) {
		GameManager.Instance.subtractMoney(price);
	}

	public void selectedBuildSite (BuildSiteBtn buildSiteSelected) {
		if(buildSiteSelected.Price <= GameManager.Instance.TotalMoney) {
			buildSiteBtnPressed = buildSiteSelected;
			SpriteRenderManager.Instance.enableDragSprite(buildSiteSelected.DragSprite);
		} else {
			disableDragSprite();
		}
	}

	private void disableDragSprite () {
		SpriteRenderManager.Instance.disableDragSprite();
		buildSiteBtnPressed = null;
	}

	// Update is called once per frame
	public void RenameTagBuildSiteFull(Collider2D buildTag) {
		buildTag.tag = "BuildSiteFull";
		BuildColliderList.Add(buildTag);
	}

	public void RegisterBuildSite(GameObject buildSite) {
		BuildSiteList.Add(buildSite);
	}

	public void DestroyAllAddedBuildSites() {
		foreach(GameObject buildSite in BuildSiteList) {
			Destroy(buildSite.gameObject);
		}
		BuildSiteList.Clear();
	}

	public void RenameTagBuildSites() {
		foreach(Collider2D buildTag in BuildColliderList) {
			buildTag.tag = "BuildSite";
		}
		BuildColliderList.Clear();
	}
}
