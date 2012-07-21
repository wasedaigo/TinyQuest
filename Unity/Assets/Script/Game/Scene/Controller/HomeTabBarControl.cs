using UnityEngine;
using System.Collections;

public class HomeTabBarControl : MonoBehaviour {
	
	public UILabel Title;
	public GameObject[] TabPages;
	public UIImageButton[] TabButtons;
	public string[] TabNames;
	
	// Use this for initialization
	void Start () {
		TabClick(0);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void DeactivateAllPages() {
		for (int i = 0; i < TabPages.Length; i++) {
			TabPages[i].SetActiveRecursively(false);
			TabButtons[i].enabled = true;
			TabButtons[i].gameObject.collider.enabled = true;
			TabButtons[i].normalSprite = "blank";
			UISprite highlight = TabButtons[i].gameObject.GetComponentInChildren<UISprite>();
			highlight.spriteName = "blank";
		}
	}
	
	void TabClick(int no) {
		this.DeactivateAllPages();
		TabPages[no].SetActiveRecursively(true);
		Title.text = TabNames[no];
		TabButtons[no].enabled = false;
		TabButtons[no].gameObject.collider.enabled = false;
		
			TabButtons[no].normalSprite = "tab_overlay";
		UISprite highlight = TabButtons[no].gameObject.GetComponentInChildren<UISprite>();
		highlight.spriteName = "tab_overlay";
	}
	
	void EpisodeClick() {
		TabClick(0);
	}
	
	void PuppetClick() {
		TabClick(1);
	}
	
	void BandClick() {
		TabClick(2);
	}
	
	void MoreClick() {
		TabClick(3);
	}
}
