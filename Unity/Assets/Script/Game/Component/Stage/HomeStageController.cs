
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Object;

public class HomeStageController : BaseStageController {
	
	void Awake () {
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	protected override void Start() {
		base.Start();
		var battler = this.spawnBattler("fighter", Ally.State.Sit, 20, 0);
		this.Stage.GetCharacterLayer().AddChild(battler);
	}

	public void OnExploreButtonClicked() {
		Application.LoadLevel("ZoneLoading");	
	}
	
	public void OnStorageButtonClicked() {
		Application.LoadLevel("Storage");	
	}
}