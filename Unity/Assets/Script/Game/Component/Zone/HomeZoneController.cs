
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Core;
using TinyQuest.Model;
using TinyQuest.Object;

using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Factory.Data;
using TinyQuest.Factory.Entity;
public class HomeZoneController : BaseZoneController {

	// Use this for initialization
	protected override void Start() {
		base.Start();
		var battler = this.spawnBattler("fighter", Ally.State.Sit, 20, 0);
		this.Stage.GetCharacterLayer().AddChild(battler);

		WeaponEntity entity = WeaponFactory.Instance.Build(1, 1);
	}
	
	public void OnExploreButtonClicked() {
		Application.LoadLevel("Adventure");	
	}
	
	public void OnStorageButtonClicked() {
		Application.LoadLevel("Storage");	
	}
}