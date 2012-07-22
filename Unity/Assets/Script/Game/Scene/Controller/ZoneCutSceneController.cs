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
	}

	private void PlayNextCutScene() {
		this.SendMessage("HideMessage");
		if (this.currentCutsceneIndex >= this.cutScenes.Length) {
			this.cutScenes = null;
			this.SendMessage("OnCutSceneFinished");
		} else {			
			TypeContentData typeContentData = this.cutScenes[this.currentCutsceneIndex];
			this.currentCutsceneIndex++;
			switch((ZoneCutSceneType)typeContentData.type) {
			case ZoneCutSceneType.Message:
				ZoneMessageCutScene messageCutScene = typeContentData.GetContent<ZoneMessageCutScene>();
				this.SendMessage("ShowMessage", messageCutScene);
				break;
			case ZoneCutSceneType.Pop:
				ZonePopCutScene popCutScene = typeContentData.GetContent<ZonePopCutScene>();
				MasterUnit unit = CacheFactory.Instance.GetMasterDataCache().GetUnitByID(popCutScene.unitId);
				this.SendMessage("PopActor", unit);
				break;
			case ZoneCutSceneType.Depop:
				this.SendMessage("DepopActor");
				break;
			}
		}
	}

	private void OnCombatActorSelected() {
		// Play next cutscene after enemy moved in
		if (this.cutScenes != null) {
			this.PlayNextCutScene();
		}
	}
	
	private void OnActorPopped() {
		PlayNextCutScene();
	}
	
	private void OnActorDepopped() {
		PlayNextCutScene();
	}

	public void OnNextClicked() {
		PlayNextCutScene();
	}
}
