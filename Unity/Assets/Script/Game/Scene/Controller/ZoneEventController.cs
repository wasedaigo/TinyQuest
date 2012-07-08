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

	private Roga2dAnimationPlayer animationPlayer;
	private Roga2dIntervalPlayer intervalPlayer;

	// Use this for initialization
	protected void Start() {
		Shader.WarmupAllShaders() ;
		this.animationPlayer = new Roga2dAnimationPlayer();
		this.intervalPlayer = new Roga2dIntervalPlayer();

		this.SetState(ZoneState.Pause);
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
			//this.SendMessage("OnStateChanged", ZoneState.Pause);	
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
			this.intervalPlayer.Clear();
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
		/*
		this.interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
			new Roga2dFunc(() => {this.player.startWalkingAnimation();}),
			new Roga2dPositionInterval(this.player, Roga2dUtils.pixelToLocal(new Vector2(40, PlayerY)), Roga2dUtils.pixelToLocal(new Vector2(-100, PlayerY)), 2.0f, true, null),
			new Roga2dFunc(() => {this.finishZone = true;})
		});
		this.IntervalPlayer.Play(this.interval);
		*/
	}

	private void OnCommandExecuted(ZoneCommand command, object zoneCommandState) {
		/*
		switch (command.type) {
			case (int)ZoneCommand.Type.Battle:
				ZoneCommandBattle battleCommand = JsonReader.Deserialize<ZoneCommandBattle>(JsonWriter.Serialize(command.content));
				this.HandleBattleCommand(battleCommand.enemyID);
				break;
			case (int)ZoneCommand.Type.Message:
				ZoneCommandMessage messageCommand = JsonReader.Deserialize<ZoneCommandMessage>(JsonWriter.Serialize(command.content));
				this.HandleMessageCommand(messageCommand.text);
				break;
			case (int)ZoneCommand.Type.Treasure:
				ZoneCommandTreasure treasureCommand = JsonReader.Deserialize<ZoneCommandTreasure>(JsonWriter.Serialize(command.content));
				this.HandleTreasureCommand(treasureCommand.treasureID);
				break;
			default:
				Debug.LogError("Undefined type " + command.type + " is passed");
				break;
		}
		*/
	}
	
	private void HandleMessageCommand(string text) {
		/*
		MessageBoxController controller = this.gameObject.GetComponent<MessageBoxController>();
		controller.baloonMessageBox = this.baloonMessageBox;
		controller.MessageFinish = this.CommandFinished;
		controller.ShowText(text);

		this.SetState(State.Next);
		?/
	}
	
	private void HandleBattleCommand(int enemyID) {
	/*
		CombatController controller = this.gameObject.GetComponent<CombatController>();
		CombatModel combatModel = CombatFactory.Instance.Build(enemyID, this.zoneModel.GetPlayerBattler());
		controller.SetCombatModel(combatModel);
		controller.SetPlayer(this.player);
		controller.CombatFinish = this.CommandFinished;
		controller.StartBattle();
		
		this.SetState(State.Combat);
		*/
	}
	
	private void HandleTreasureCommand(int treasureID) {
		// TODO
	}
	
	private void CommandFinished() {
		this.zoneModel.NextCommand();
	}
	
	public void OnProgressClicked() {
		Debug.Log("OnProgressClicked");	
	}
}