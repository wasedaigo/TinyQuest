using JsonFx.Json;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Entity;
using TinyQuest.Factory.Entity;
using TinyQuest.Object;

public class ZoneStageController : BaseStageController {
	public enum State {
		Combat,
		Moving,
		Next,
		Pause
	};
	private const float PlayerY = 10;

	public GameObject CombatPanel;
	public GameObject NextPanel;
	public GameObject MovingPanel;
	public CombatPanelController CombatPanelController;

	private Ally player;
	private State state;
	private bool finishZone;
	private Roga2dBaseInterval interval;
	
	private ZoneEntity zoneEntity;

	// Use this for initialization
	protected override void Start() {
		base.Start();

		// animationPlayer
		this.player = spawnBattler("fighter", Ally.State.Stand, 40, PlayerY);
		this.player.LocalPriority = 0.45f;
		
		this.Stage.GetCharacterLayer().AddChild(this.player);

		this.zoneEntity = ZoneFactory.Instance.Build();
		this.zoneEntity.PlayerMove += this.onPlayerMoved;
		this.zoneEntity.StepProgress += this.OnStepProgressed;
		this.zoneEntity.CommandExecute += this.OnCommandExecuted;
		this.zoneEntity.GotoNextStep += this.GotoNextStep;
		this.zoneEntity.ClearZone += this.ClearZone;

		this.SetState(State.Pause);

		if (this.zoneEntity.IsAtStart()) {
			this.interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
				new Roga2dFunc(() => {this.player.startWalkingAnimation();}),
				new Roga2dPositionInterval(this.player, Roga2dUtils.pixelToLocal(new Vector2(80, PlayerY)), Roga2dUtils.pixelToLocal(new Vector2(40, PlayerY)), 1.0f, true, null),
				new Roga2dFunc(() => {this.zoneEntity.StartAdventure();})
			});
			this.IntervalPlayer.Play(this.interval);
		} else {
			this.player.LocalPixelPosition = new Vector2(20, PlayerY);
			this.zoneEntity.StartAdventure();
		}
	}
	
	private void SetState(State state) {
		this.state = state;
		this.CombatPanel.SetActiveRecursively(false);
		this.NextPanel.SetActiveRecursively(false);
		this.MovingPanel.SetActiveRecursively(false);
		switch (this.state) {
			case State.Combat:
				this.CombatPanel.SetActiveRecursively(true);
				this.player.stopWalkingAnimation();
				break;
			case State.Next:
				this.NextPanel.SetActiveRecursively(true);
				this.player.stopWalkingAnimation();
				break;
			case State.Moving:
				this.MovingPanel.SetActiveRecursively(true);
				this.player.startWalkingAnimation();
				break;
			case State.Pause:
				this.player.stopWalkingAnimation();
				break;
		}	
	}

	protected override void Update() {
		base.Update();
		
		if (this.interval != null && !this.interval.IsDone()) {
			return;		
		}

		if (this.finishZone) {
			this.IntervalPlayer.Clear();
			Application.LoadLevel("Home");
		}
		
		if (this.state == State.Moving) {
			this.zoneEntity.MoveForward();
		}
	}
	
	protected void OnDestroy() {
		Roga2dResourceManager.freeResources();	
	}
	
	private void onPlayerMoved(float distance) {
		this.Stage.Scroll(distance);
	}
	
	private void OnStepProgressed(int stepIndex, bool hasEvent) {
	}
	
	private void GotoNextStep() {
		this.SetState(State.Moving);
	}
	
	private void ClearZone() {
		this.interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
			new Roga2dFunc(() => {this.player.startWalkingAnimation();}),
			new Roga2dPositionInterval(this.player, Roga2dUtils.pixelToLocal(new Vector2(40, PlayerY)), Roga2dUtils.pixelToLocal(new Vector2(-100, PlayerY)), 2.0f, true, null),
			new Roga2dFunc(() => {this.finishZone = true;})
		});
		this.IntervalPlayer.Play(this.interval);
	}

	private void OnCommandExecuted(ZoneCommand command, object zoneCommandState) {
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
	}
	
	private void HandleMessageCommand(string text) {
		MessageBoxController controller = this.gameObject.GetComponent<MessageBoxController>();
		controller.baloonMessageBox = this.baloonMessageBox;
		controller.MessageFinish = this.CommandFinished;
		controller.ShowText(text);

		this.SetState(State.Next);
	}
	
	private void HandleBattleCommand(int enemyID) {
		CombatController controller = this.gameObject.GetComponent<CombatController>();
		CombatEntity combatEntity = CombatFactory.Instance.Build(enemyID, this.zoneEntity.GetPlayerBattler());
		controller.SetCombatEntity(combatEntity);
		controller.SetPlayer(this.player);
		controller.CombatFinish = this.CommandFinished;
		controller.StartBattle();
		
		this.SetState(State.Combat);
	}
	
	private void HandleTreasureCommand(int treasureID) {
		// TODO
	}
	
	private void CommandFinished() {
		this.zoneEntity.NextCommand();
	}
}