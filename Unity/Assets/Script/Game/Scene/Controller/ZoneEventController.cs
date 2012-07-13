using JsonFx.Json;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Scene.Model;
using TinyQuest.Object;
using TinyQuest.Scene;

public class ZoneEventController : MonoBehaviour {

	private const float PlayerY = 10;

	private ZoneState state;
	private bool finishZone;
	private Roga2dBaseInterval interval;
	
	private ZoneModel zoneModel;
	
	public void ResumeAdventure() {
		this.SendMessage("PlayerMoveIn", this.zoneModel.IsAtStart());
	}
	
	public void SetModels(ZoneModel zoneModel) {
		this.zoneModel = zoneModel;
		this.zoneModel.PlayerMove += this._OnPlayerMoved;
		this.zoneModel.CommandExecute += this.OnCommandExecuted;
		this.zoneModel.GotoNextStep += this.GotoNextStep;
		this.zoneModel.ClearZone += this.ClearZone;
	}

	private void SetState(ZoneState state) {
		if (this.state != state) {
			this.state = state;	
			
			CombatUnit walkingUnit = this.zoneModel.GetWalkingUnit();
			if (this.state == ZoneState.Moving) {
				this.SendMessage("StartWalkAnimation", walkingUnit);
			} else {
				this.SendMessage("StopWalkAnimation", walkingUnit);
			}
		}
	}
	
	protected void OnPlayerMovedIn() {
		this.zoneModel.StartAdventure();
	}
	
	protected void OnPlayerMovedOut() {
		Application.LoadLevel("Home");
	}
	
	protected void _OnPlayerMoved(float distance) {
		this.SendMessage("OnPlayerMoved", distance);	
	}
	
	protected void Update() {

		if (this.finishZone) {
			Application.LoadLevel("Home");
		}
		
		if (this.state == ZoneState.Moving) {
			this.zoneModel.MoveForward();
		}
	}
	
	protected void OnDestroy() {
		Roga2dResourceManager.freeResources();	
	}
	
	private void GotoNextStep() {
		this.SetState(ZoneState.Moving);
	}
	
	private void ClearZone() {
		this.SendMessage("PlayerMoveOut");
	}

	private void OnCommandExecuted(ZoneCommand command, object zoneCommandState) {
		CombatUnit walkingUnit = this.zoneModel.GetWalkingUnit();	
		this.SetState(ZoneState.Pause);
		switch (command.type) {
			case (int)ZoneCommand.Type.Battle:
				ZoneCommandBattle battleCommand = JsonReader.Deserialize<ZoneCommandBattle>(JsonWriter.Serialize(command.content));
				this.HandleBattleCommand(battleCommand.enemyGroupId);
				break;
			case (int)ZoneCommand.Type.Message:
				ZoneCommandMessage messageCommand = JsonReader.Deserialize<ZoneCommandMessage>(JsonWriter.Serialize(command.content));
				this.HandleMessageCommand(messageCommand.text);
				break;
			case (int)ZoneCommand.Type.Treasure:
				ZoneCommandTreasure treasureCommand = JsonReader.Deserialize<ZoneCommandTreasure>(JsonWriter.Serialize(command.content));
				this.HandleTreasureCommand(treasureCommand.treasureId);
				break;
			default:
				Debug.LogError("Undefined type " + command.type + " is passed");
				break;
		}
	}
	
	private void HandleMessageCommand(string text) {
		this.SendMessage("ShowMessage", text);
		this.SetState(ZoneState.Next);
	}
	
	private void HandleBattleCommand(int enemyGroupId) {

		/*
		CombatController controller = this.gameObject.GetComponent<CombatController>();
		CombatModel combatModel = CombatFactory.Instance.Build(enemyID, this.zoneModel.GetPlayerBattler());
		controller.SetCombatModel(combatModel);
		controller.SetPlayer(this.player);
		controller.CombatFinish = this.CommandFinished;
		controller.StartBattle();
		*/
		this.SendMessage("StartBattle", enemyGroupId);
		this.SetState(ZoneState.Combat);
	}
	
	private void HandleTreasureCommand(int treasureID) {
		// TODO
	}
	
	public void OnProgressClicked() {
		this.SendMessage("HideMessage");
		
		if (this.zoneModel.IsCommandExecuting()) {
			this.zoneModel.NextCommand();
		} else {
			this.GotoNextStep();
		}
	}
}