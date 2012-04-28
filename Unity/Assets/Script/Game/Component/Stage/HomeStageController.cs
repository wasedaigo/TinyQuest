
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Core;
using TinyQuest.Model;
using TinyQuest.Object;

public class HomeStageController : BaseStageController {

	// Use this for initialization
	protected override void Start() {
		base.Start();
		var battler = this.spawnBattler("fighter", 20, 0);
		this.Stage.GetCharacterLayer().AddChild(battler);
	}
	
	public void OnActionButtonClick() {
		Application.LoadLevel("Adventure");	
	}
}