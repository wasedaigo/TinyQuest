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
		if (this.zoneModel.IsAtGoal()) {
			this.SendMessage("ShowPanel", ZoneSceneManager.ZonePanelType.Clear);
		} else {
			this.SendMessage("PlayerMoveIn", this.zoneModel.IsAtStart());
		}
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
		this.SendMessage("ShowPanel", ZoneSceneManager.ZonePanelType.Clear);
	}
	
	protected void _OnPlayerMoved(float distance) {
		this.SendMessage("OnPlayerMoved", distance);	
	}
	
	protected void Update() {
		
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

	private void OnCommandExecuted(ZoneCommandBase command) {
		this.SetState(ZoneState.Pause);
		
		this.SendMessage("StartCutScene", command.cutScenes);
		switch ((ZoneCommandType)command.command.type) {
			case ZoneCommandType.Empty:
				this.SendMessage("PlayNextCutScene");
				break;

			case ZoneCommandType.Battle:
				ZoneCommandBattle battleCommand = command.command.GetContent<ZoneCommandBattle>();
				this.SendMessage("StartBattle", battleCommand.enemyGroupId);
				this.SetState(ZoneState.Combat);
				break;

			case ZoneCommandType.Treasure:
				this.SendMessage("PlayNextCutScene");
				break;

			default:
				Debug.LogError("Undefined type " + command.command.type + " is passed");
				break;
		}
	}

	public void NextCommand() {
		if (this.zoneModel.IsCommandExecuting()) {
			this.zoneModel.NextCommand();
		}
	}

	public void OnProgressClicked() {
		if (!this.zoneModel.IsCommandExecuting()) {
			this.GotoNextStep();
		}
	}
	
	private void OnClearResultOkClicked() {
		Application.LoadLevel("Home");
	}
}