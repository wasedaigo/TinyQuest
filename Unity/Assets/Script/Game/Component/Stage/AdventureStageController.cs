
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Core;
using TinyQuest.Entity;
using TinyQuest.Factory.Entity;
using TinyQuest.Model;
using TinyQuest.Object;

public class AdventureStageController : BaseStageController {
	
	private Monster monster;
	private List<AdventureObject> battlers = new List<AdventureObject>();
	private Ally player;

	private BattlerEntity userBattlerEntity;
	private ZoneEntity zoneEntity;
	private ActionWheel actionWheel;
	
	// Use this for initialization
	protected override void Start() {
		base.Start();

		// animationPlayer
		this.player = spawnBattler("fighter", Ally.State.Stand, 20, 0);
		
		this.battlers.Add(this.player);
		
		this.monster = spawnMonster("death_wind", -40, 0);
		
		this.Stage.GetCharacterLayer().AddChild(this.player);
		this.Stage.GetCharacterLayer().AddChild(this.monster);
		
		this.userBattlerEntity = BattlerFactory.Instance.BuildUserBattler();
		this.zoneEntity = ZoneFactory.Instance.Build(1);
		
		this.actionWheel = GameObject.Find("ActionWheel").GetComponent<ActionWheel>();
		this.actionWheel.SetUserBattler(this.userBattlerEntity);
		this.actionWheel.SetState(ActionWheel.State.Combat);
	}
	
	void Update() {
		base.Update();	
		if (!this.AnimationPlayer.HasPlayingAnimations()) {
			if (this.monster != null && this.monster.IsDead()) {
				this.Stage.GetCharacterLayer().RemoveChild(this.monster);
				this.monster = null;
				
			}	
		}
	}
	
	private void onScrollFinished() {
		this.player.stopWalkingAnimation();
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
	
	private void playNextAnimation(int no) {
		WeaponEntity weapon = this.userBattlerEntity.GetWeapon(no);
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
			
			SkillEntity skillEntity = SkillFactory.Instance.Build(weapon.GetMasterWeapon().skills[0]);
			
			Debug.Log(skillEntity.Path);
			Roga2dAnimation animation = Roga2dUtils.LoadAnimation("" + skillEntity.Path, false, 1.0f, 0.5f, settings, options);
			this.AnimationPlayer.Play(battler, null, animation,  AnimationFinished);
		}
	}
	
	public void OnActionButtonClick() {
		this.playNextAnimation(this.actionWheel.getSlotAt(0));
	}
}