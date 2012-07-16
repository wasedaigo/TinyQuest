using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;

public class ZoneCutSceneController : MonoBehaviour {
	
	private TypeContentData[] cutScenes;
	private int currentCutsceneIndex;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void StartCutScene(TypeContentData[] cutScenes) {
		this.cutScenes = cutScenes;
		this.currentCutsceneIndex = 0;
		this.PlayNextCutScene();
	}
	
	private void PlayNextCutScene() {
		if (this.currentCutsceneIndex >= this.cutScenes.Length) {
			this.SendMessage("OnCutSceneFinished");
		} else {
			
			TypeContentData typeContentData = this.cutScenes[this.currentCutsceneIndex];
			switch((ZoneCutSceneType)typeContentData.type) {
			case ZoneCutSceneType.Message:
				this.SendMessage("ShowPanel", ZoneSceneManager.ZonePanelType.Next);
				ZoneMessageCutScene messageCutScene = typeContentData.GetContent<ZoneMessageCutScene>();
				this.SendMessage("ShowMessage", messageCutScene);
				break;
			case ZoneCutSceneType.Pop:
				this.SendMessage("ShowPanel", ZoneSceneManager.ZonePanelType.None);
				ZonePopCutScene popCutScene = typeContentData.GetContent<ZonePopCutScene>();
				MasterUnit unit = CacheFactory.Instance.GetMasterDataCache().GetUnitByID(popCutScene.unitId);
				this.SendMessage("PopActor", unit);
				break;
			case ZoneCutSceneType.Depop:
				this.SendMessage("ShowPanel", ZoneSceneManager.ZonePanelType.None);
				this.SendMessage("DepopActor");
				break;
			}
			this.currentCutsceneIndex++;
		}
	}
	
	private void OnActorPopped() {
		PlayNextCutScene();
	}
	
	private void OnActorDepopped() {
		PlayNextCutScene();
	}

	public void OnNextClicked() {
		this.SendMessage("HideMessage");
		PlayNextCutScene();
	}
}
