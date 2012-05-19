using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TinyQuest.Entity;
using TinyQuest.Factory.Entity;
using TinyQuest.Object;
using TinyQuest.Core;
using TinyQuest.Data;

public class CombatController : BaseStageController {
	public System.Action CombatFinish;
	public ZoneSceneData ZoneSceneData;
	
	private Stage stage;
	private Ally monster;
	private List<AdventureObject> battlers = new List<AdventureObject>();
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.stage = this.GetComponent<Stage>();
		
		ZoneSceneData.UserBattlerEntity.WeaponUse += this.WeaponUsed;
		ZoneSceneData.UserBattlerEntity.UpdateAP  += this.APUpdated;
	
		//this.monster = spawnMonster("death_wind", -40, 0);
		this.monster = spawnBattler("fighter", Ally.State.Stand, -40, 0);
		this.monster.LocalScale = new Vector2(-1, 1);
		this.monster.LocalPriority = 0.45f;
		this.stage.GetCharacterLayer().AddChild(this.monster);
	}

	// Update is called once per frame
	protected override void Update () {
		if (!this.AnimationPlayer.HasPlayingAnimations()) {
			if (this.monster != null && this.monster.IsDead()) {
				this.stage.GetCharacterLayer().RemoveChild(this.monster);
				this.monster = null;
				
				if (this.CombatFinish != null) {
					this.CombatFinish();	
				}
			}	
		}
		
		base.Update();
	}
	
	public void SetPlayer(Ally player) {
		this.battlers.Add(player);
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
			this.IntervalPlayer.Play(interval);
			
			// Damage pop
			Roga2dAnimation animation = EffectBuilder.GetInstance().BuildDamagePopAnimation(settings.Target.LocalPixelPosition, damageValue);
			this.AnimationPlayer.Play(this.stage.GetCharacterLayer(), null, animation, null);
			
			AdventureObject obj = (AdventureObject)settings.Target;
			//obj.ApplyDamage(damageValue);
		}
	}
	
	private void playSkillAnimation(AdventureObject caster, AdventureObject target, WeaponEntity weapon, MasterSkill masterSkill) {
		if (weapon == null) {
			return;	
		}
		
		if (caster.Sprite.IsVisible) {
			caster.Sprite.Hide();

			Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
				{ "Combat/BattlerBase", new Roga2dSwapTextureDef() {TextureID = caster.TextureID, PixelSize = new Vector2(32, 32)}},
				{ "Battle/Skills/Monster_Base", new Roga2dSwapTextureDef() {TextureID = "death_wind", PixelSize = target.PixelSize,  SrcRect = target.SrcRect}},
				{ "Combat/WeaponSwordBase", new Roga2dSwapTextureDef() {TextureID = weapon.GetMasterWeapon().GetAnimationImagePath(), PixelSize = new Vector2(32, 32),  SrcRect = new Rect(0, 0, 32, 32)}}
			};

			Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.AnimationPlayer, false, caster, caster, target, CommandCalled);
			Roga2dAnimation animation = Roga2dUtils.LoadAnimation("" + masterSkill.animation, false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Bow/Shoot", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Sword/LeaveDance", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Bow/bow_bomb", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Fire/Skill_Flare", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Laser/Skill_Laser01", false, null, settings, options);
			this.AnimationPlayer.Play(caster,  null, animation,  AnimationFinished);
		}
	}
	
	private void WeaponUsed(WeaponEntity weaponEntity, MasterSkill masterSkill) {
		//this.playSkillAnimation(this.monster, this.battlers[0], weaponEntity, skillEntity);
		this.playSkillAnimation(this.battlers[0], this.monster, weaponEntity, masterSkill);
	}
	
	private void APUpdated(int newAP) {
		//Debug.Log("AP Updated = " + newAP);
	}
	
	public void InvokeCommand(int slotNo) {
		ZoneSceneData.UserBattlerEntity.UseWeapon(slotNo);
	}
}
