using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BuildSiteManager : Singleton<BuildSiteManager> {


	public BuildSiteBtn buildSiteBtnPressed{get; set;}
	private SpriteRenderer spriteRenderer;
	private List<Collider2D> BuildList = new List<Collider2D>();

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
	}

	private void placeBuildSite(RaycastHit2D hit) {
		if(!EventSystem.current.IsPointerOverGameObject() && buildSiteBtnPressed != null) {
			if(hit.collider.tag == "Ground") {
				if(buildSiteBtnPressed.Price <= GameManager.Instance.TotalMoney) {
					GameObject buildSite = Instantiate (buildSiteBtnPressed.BuildSiteObject) as GameObject;
					buildSite.transform.position = hit.transform.position;
					buyBuildSite(buildSiteBtnPressed.Price);
					GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
					RegisterBuildSite(hit.collider);
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
	public void RegisterBuildSite(Collider2D buildTag) {
		buildTag.tag = "BuildSiteFull";
		BuildList.Add(buildTag);
	}

	public void RenameTagBuildSites() {
		foreach(Collider2D buildTag in BuildList) {
			buildTag.tag = "BuildSite";
		}
		BuildList.Clear();
	}
}
