
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Core;
using TinyQuest.Entity;
using TinyQuest.Factory.Entity;
using TinyQuest.Model;
using TinyQuest.Object;

public class AdventureStageController : BaseStageController {
	public enum State {
		Combat,
		Progress
	};
	
	
	public GameObject[] slots;
	public GameObject CombatPanel;
	public GameObject ProgressPanel;
	
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

		
		this.userBattlerEntity = BattlerFactory.Instance.BuildUserBattler();

		this.userBattlerEntity.WeaponUse += this.WeaponUsed;
		this.userBattlerEntity.UpdateAP  += this.APUpdated;

		this.zoneEntity = ZoneFactory.Instance.Build(1);
		this.zoneEntity.PlayerMove += this.onPlayerMoved;
		this.zoneEntity.StepProgress += this.OnStepProgressed;
		/*
		this.actionWheel = GameObject.Find("ActionWheel").GetComponent<ActionWheel>();
		this.actionWheel.SetUserBattler(this.userBattlerEntity);
		this.actionWheel.SetState(ActionWheel.State.Combat);*/
		
		this.SetState(State.Progress);
		
		for (int i = 0; i < BattlerEntity.WeaponSlotNum; i++) {
			WeaponEntity weapon = this.userBattlerEntity.GetWeapon(i);
			if (weapon != null) {
				this.SetWeaponAtSlot(i, "UI/" + weapon.GetMasterWeapon().path);
			}
		}

		this.interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
			new Roga2dPositionInterval(this.player, Roga2dUtils.pixelToLocal(new Vector2(80, PlayerY)), Roga2dUtils.pixelToLocal(new Vector2(40, PlayerY)), 1.0f, true, null)
		});
			
			
		Roga2dIntervalPlayer.GetInstance().Play(this.interval);
	}
	
	private void SetState(State state) {
		this.state = state;
		this.CombatPanel.SetActiveRecursively(false);
		//this.ProgressPanel.SetActiveRecursively(false);
		switch (this.state) {
			case State.Combat:
				this.CombatPanel.SetActiveRecursively(true);
				this.player.stopWalkingAnimation();
				break;
			case State.Progress:
				//this.ProgressPanel.SetActiveRecursively(true);
				this.player.startWalkingAnimation();
				break;
		}	
	}
	
	void Update() {
		base.Update();	
		if (this.interval != null && !this.interval.IsDone()) {
			return;		
		}
		if (!this.AnimationPlayer.HasPlayingAnimations()) {
			if (this.monster != null && this.monster.IsDead()) {
				this.Stage.GetCharacterLayer().RemoveChild(this.monster);
				this.monster = null;
				this.SetState(State.Progress);
				//Application.LoadLevel("Home");
			}	
		}
			
		if (this.finishZone) {
			Roga2dIntervalPlayer.GetInstance().Clear();
			Application.LoadLevel("Home");
		}
		
		this.OnActionButtonPressing();
	}
	
	private void onPlayerMoved(float distance) {
		this.Stage.Scroll(distance);
	}
	
	private void OnStepProgressed(int step) {
		if (step == 14) {
			this.interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
				new Roga2dFunc(() => {this.player.startWalkingAnimation();}),
				new Roga2dPositionInterval(this.player, Roga2dUtils.pixelToLocal(new Vector2(40, PlayerY)), Roga2dUtils.pixelToLocal(new Vector2(-100, PlayerY)), 3.0f, true, null),
				new Roga2dFunc(() => {this.finishZone = true;}),
			});
			Roga2dIntervalPlayer.GetInstance().Play(this.interval);

		} else {
			Roga2dIntervalPlayer.GetInstance().Play(this.interval);
			this.monster = spawnMonster("death_wind", -20, 0);
			this.Stage.GetCharacterLayer().AddChild(this.monster);
			this.CancelMovement();
			this.SetState(State.Combat);
		}
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
	
	public void OnActionButtonClick() {
		switch (this.state) {
			case State.Combat:
				break;
			case State.Progress:
				break;
		}
	}
	
	private void OnActionButtonPressing() {
		if (this.player.IsWalking()) {
			switch (this.state) {
				case State.Combat:
					break;
				case State.Progress:
					this.zoneEntity.MoveForward();
					break;
			}	
		}
	}
	
	public void OnActionButtonPress() {
		this.pressed = true;
		if (this.state == State.Progress) {
			this.player.startWalkingAnimation();
		}
	}

	
	public void OnActionButtonRelease() {
		this.pressed = false;
		
		if (this.state == State.Progress) {
			this.player.stopWalkingAnimation();
		}
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