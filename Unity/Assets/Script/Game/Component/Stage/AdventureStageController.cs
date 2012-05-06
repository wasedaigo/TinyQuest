
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
	
	// Use this for initialization
	protected override void Start() {
		base.Start();

		// animationPlayer
		this.player = spawnBattler("fighter", Ally.State.Stand, 20, 0);
		
		this.battlers.Add(this.player);
		
		ActionWheel actionWheel = GameObject.Find("ActionWheel").GetComponent<ActionWheel>();
		actionWheel.onSlotChanged = this.onSlotChanged;
		actionWheel.onSubButtonClicked = this.onSubButtonClicked;
		this.monster = spawnMonster("death_wind", -40, 0);
		
		this.Stage.GetCharacterLayer().AddChild(this.player);
		this.Stage.GetCharacterLayer().AddChild(this.monster);
		
		this.userBattlerEntity = BattlerFactory.Instance.BuildUserBattler();
		this.zoneEntity = ZoneFactory.Instance.Build(1);
		
		for (int i = 0; i < BattlerEntity.WeaponSlotNum; i++) {
			WeaponEntity weaponEntity = this.userBattlerEntity.GetWeapon(i);
			if (weaponEntity != null) {
				Debug.Log(i + " - " + weaponEntity.GetMasterWeapon().name);
			}
		}
	
		for (int i = 0; i < BattlerEntity.WeaponSlotNum; i++) {
			WeaponEntity weapon = this.userBattlerEntity.GetWeapon(i);
			if (weapon != null) {
				actionWheel.SetWeaponAtSlot(i, "UI/" + weapon.GetMasterWeapon().path);
			}
		}
	}

	private void onSlotChanged(int slotNo) {
		this.playNextAnimation(slotNo);
	}

	private void onSubButtonClicked() {
	}
	
	private void onScrollFinished() {
		this.player.stopWalkingAnimation();
	}
	
	static string[] ids = new string[] {
			"Combat/Sword/MultiSlash",
			"Combat/Sword/DoubleSlash",
			"Combat/Sword/Slash",
			"Combat/Sword/Slash",
			"Combat/Sword/Slash",
			"Combat/Sword/Slash",
			"Combat/Sword/Slash",
			"Combat/Sword/Slash"
			//"Battle/Skills/Bow/Shoot",
			//"Battle/Skills/Sword/LeaveDance",
			//"Battle/Skills/Spear/SpearAirraid",
			//"Battle/Skills/Axe/Bash",
			//"Battle/Skills/Common/MagicCasting",
			//"Battle/Skills/Laser/Skill_Laser01",
			//"Battle/Skills/Bow/bow_bomb",
			//"Battle/Skills/Axe/CycloneAxe",
			//"Battle/Skills/Axe/Slash",
			//"Battle/Skills/Axe/ArmorBreaker",
			//"Battle/Skills/Fire/Skill_Flare",
			//"Battle/Skills/Monster/DeadlyBite",
	};
	
	static Rect[] weapons = new Rect[] {
			new Rect(32, 0, 32, 32),
			new Rect(32, 0, 32, 32),
			new Rect(32, 0, 32, 32),
			new Rect(32, 0, 32, 32),
			new Rect(0, 0, 32, 32),
			new Rect(0, 0, 32, 32),
			new Rect(0, 0, 32, 32),
			new Rect(0, 0, 32, 32),
			//"Battle/Skills/Bow/Shoot",
			//"Battle/Skills/Sword/LeaveDance",
			//"Battle/Skills/Spear/SpearAirraid",
			//"Battle/Skills/Axe/Bash",
			//"Battle/Skills/Common/MagicCasting",
			//"Battle/Skills/Laser/Skill_Laser01",
			//"Battle/Skills/Bow/bow_bomb",
			//"Battle/Skills/Axe/CycloneAxe",
			//"Battle/Skills/Axe/Slash",
			//"Battle/Skills/Axe/ArmorBreaker",
			//"Battle/Skills/Fire/Skill_Flare",
			//"Battle/Skills/Monster/DeadlyBite",
	};
	
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
			if (obj.IsDead()) {
				obj.Hide();
				Application.LoadLevel("Home");
			}
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
}