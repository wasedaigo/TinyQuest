using JsonFx.Json;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Entity;
using TinyQuest.Factory.Entity;
using TinyQuest.Model;
using TinyQuest.Object;

public class AdventureStageController : BaseStageController {
	public enum State {
		Combat,
		Moving,
		Next,
		Pause
	};
	
	
	public GameObject[] slots;
	public GameObject CombatPanel;
	public GameObject NextPanel;
	public GameObject MovingPanel;
	
	private Monster monster;
	private List<AdventureObject> battlers = new List<AdventureObject>();
	private Ally player;
	private UITexture[] weaponTextures;

	private BattlerEntity userBattlerEntity;
	private ZoneEntity zoneEntity;
	private ActionWheel actionWheel;
	private State state;
	private bool pressed;
	private bool finishZone;
	private Roga2dBaseInterval interval;
	private const float PlayerY = 20;
	
	// Use this for initialization
	protected override void Start() {
		base.Start();
		
		this.weaponTextures = new UITexture[6];

		// animationPlayer
		this.player = spawnBattler("fighter", Ally.State.Stand, 40, PlayerY);
		this.player.LocalPriority = 0.45f;
		
		this.battlers.Add(this.player);
		
		this.Stage.GetCharacterLayer().AddChild(this.player);

		this.zoneEntity = ZoneFactory.Instance.Build(1);
		this.zoneEntity.PlayerMove += this.onPlayerMoved;
		this.zoneEntity.StepProgress += this.OnStepProgressed;
		this.zoneEntity.CommandExecute += this.OnCommandExecuted;
		this.zoneEntity.GotoNextStep += this.GotoNextStep;
		this.zoneEntity.ClearZone += this.ClearZone;
		
		this.userBattlerEntity = this.zoneEntity.GetPlayerBattler();
		this.userBattlerEntity.WeaponUse += this.WeaponUsed;
		this.userBattlerEntity.UpdateAP  += this.APUpdated;
		
		/*
		this.actionWheel = GameObject.Find("ActionWheel").GetComponent<ActionWheel>();
		this.actionWheel.SetUserBattler(this.userBattlerEntity);
		this.actionWheel.SetState(ActionWheel.State.Combat);*/

		this.SetState(State.Pause);

		for (int i = 0; i < BattlerEntity.WeaponSlotNum; i++) {
			WeaponEntity weapon = this.userBattlerEntity.GetWeapon(i);
			if (weapon != null) {
				this.SetWeaponAtSlot(i, "UI/" + weapon.GetMasterWeapon().path);
			}
		}
		
		if (this.zoneEntity.IsAtStart()) {
			this.interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
				new Roga2dFunc(() => {this.player.startWalkingAnimation();}),
				new Roga2dPositionInterval(this.player, Roga2dUtils.pixelToLocal(new Vector2(80, PlayerY)), Roga2dUtils.pixelToLocal(new Vector2(40, PlayerY)), 1.0f, true, null),
				new Roga2dFunc(() => {this.zoneEntity.StartAdventure();})
			});
			Roga2dIntervalPlayer.GetInstance().Play(this.interval);
		} else {
			this.player.LocalPixelPosition = new Vector2(40, PlayerY);
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

	void Update() {
		base.Update();	
		if (this.interval != null && !this.interval.IsDone()) {
			return;		
		}
		if (!this.AnimationPlayer.HasPlayingAnimations()) {
			if (this.state == State.Combat && this.monster != null && this.monster.IsDead()) {
				this.Stage.GetCharacterLayer().RemoveChild(this.monster);
				this.monster = null;
				this.zoneEntity.NextCommand();
				//Application.LoadLevel("Home");
			}	
		}
			
		if (this.finishZone) {
			Roga2dIntervalPlayer.GetInstance().Clear();
			Application.LoadLevel("Home");
		}
		
		if (this.state == State.Moving) {
			this.zoneEntity.MoveForward();
		}
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
		Roga2dIntervalPlayer.GetInstance().Play(this.interval);
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
				break;
			default:
				Debug.LogError("Undefined type " + command.type + " is passed");
				break;
		}
	}
	
	private void HandleMessageCommand(string text) {
		this.ShowMessage(text);
		this.SetState(State.Next);
	}
	
	private void HandleBattleCommand(int enemyId) {
			this.monster = spawnMonster("death_wind", -20, 0);
			this.Stage.GetCharacterLayer().AddChild(this.monster);
			this.CancelMovement();
			this.SetState(State.Combat);
	}
	
	public void SetWeaponAtSlot(int i, string textureId) {
		if (this.weaponTextures[i] != null) {
			NGUITools.Destroy(this.weaponTextures[i]);	
			this.weaponTextures[i] = null;
		}

		GameObject slot = this.slots[i];
		
		UITexture ut = NGUITools.AddWidget<UITexture>(slot);
		Material material = Roga2dResourceManager.getSharedMaterial(textureId, Roga2dBlendType.Unlit);
        ut.material = material;
		ut.MarkAsChanged();
		ut.MakePixelPerfect();
		ut.transform.localScale = new Vector3(ut.transform.localScale.x * 2, ut.transform.localScale.y * 2, ut.transform.localScale.z);
		ut.transform.localPosition = new Vector3(1, 1, -0.1f);
		ut.transform.localEulerAngles = Vector3.one;
		
		this.weaponTextures[i] = ut;
	}
	
    private void AnimationFinished(Roga2dAnimation animation)
    {
		animation.settings.Origin.Show();
		animation.settings.Destroy();
		animation.settings = null;
    }
	
	private void CommandCalled(Roga2dAnimationSettings settings, string command) 
	{
		string[] commandData = command.Split(':');
		if (commandData[0] == "damage") {
			uint damageValue = 2750;
			// Flash effect
			Roga2dBaseInterval interval = EffectBuilder.GetInstance().BuildDamageInterval(settings.Target);
			Roga2dIntervalPlayer.GetInstance().Play(interval);
			
			// Damage pop
			Roga2dAnimation animation = EffectBuilder.GetInstance().BuildDamagePopAnimation(settings.Target.LocalPixelPosition, damageValue);
			this.AnimationPlayer.Play(settings.Root, null, animation, null);
			
			AdventureObject obj = (AdventureObject)settings.Target;
			obj.ApplyDamage(damageValue);
		}
	}
	
	private void playSkillAnimation(WeaponEntity weapon, SkillEntity skillEntity) {
		if (weapon == null) {
			return;	
		}
		AdventureObject battler = this.battlers[0];
		if (battler.Sprite.IsVisible) {
			battler.Sprite.Hide();

			Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
				{ "Combat/BattlerBase", new Roga2dSwapTextureDef() {TextureID = battler.TextureID, PixelSize = new Vector2(32, 32)}},
				{ "Battle/Skills/Monster_Base", new Roga2dSwapTextureDef() {TextureID = "death_wind", PixelSize = this.monster.PixelSize,  SrcRect = this.monster.SrcRect}},
				{ "Combat/WeaponSwordBase", new Roga2dSwapTextureDef() {TextureID = weapon.GetMasterWeapon().path, PixelSize = new Vector2(32, 32),  SrcRect = new Rect(0, 0, 32, 32)}}
			};

			Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.AnimationPlayer, this.Stage.GetCharacterLayer(), battler, this.monster, CommandCalled);

			Roga2dAnimation animation = Roga2dUtils.LoadAnimation("" + skillEntity.Path, false, 1.0f, 0.0f, settings, options);
			this.AnimationPlayer.Play(battler, null, animation,  AnimationFinished);
		}
	}
	
	private void CancelMovement() {
		this.pressed = false;
		this.player.stopWalkingAnimation();
	}
	
	public void OnNextButtonClick() {
		this.HideMessage();
		this.zoneEntity.NextCommand();
		Debug.Log("NextButtonClick");
	}
	
	public void OnSlot1Click() {
		this.userBattlerEntity.UseWeapon(0);
	}
	
	public void OnSlot2Click() {
		this.userBattlerEntity.UseWeapon(1);
	}
	
	public void OnSlot3Click() {
		this.userBattlerEntity.UseWeapon(2);
	}
	
	public void OnSlot4Click() {
		this.userBattlerEntity.UseWeapon(3);
	}
	
	public void OnSlot5Click() {
		this.userBattlerEntity.UseWeapon(4);
	}
	
	public void OnSlot6Click() {
		this.userBattlerEntity.UseWeapon(5);
	}
	
	private void WeaponUsed(WeaponEntity weaponEntity, SkillEntity skillEntity) {
		this.playSkillAnimation(weaponEntity, skillEntity);
	}
	
	private void APUpdated(int newAP) {
		Debug.Log("AP Updated = " + newAP);
	}
}