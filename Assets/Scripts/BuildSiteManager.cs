using UnityEngine;
using System.Collections.Generic;

public class BuildSiteManager : Singleton<BuildSiteManager> {

	private List<Collider2D> BuildList = new List<Collider2D>();
	
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
