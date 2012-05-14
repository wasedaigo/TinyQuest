using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TinyQuest.Entity;
using TinyQuest.Factory.Entity;
using TinyQuest.Object;
using TinyQuest.Core;

public class CombatController : BaseStageController {
	public System.Action CombatFinish;
	
	private BattlerEntity userBattlerEntity;
	private ZoneEntity zoneEntity;
	private UITexture[] weaponTextures;
	private Stage stage;
	private Ally monster;
	private List<AdventureObject> battlers = new List<AdventureObject>();
	private GameObject[] slots;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.stage = this.GetComponent<Stage>();
		this.weaponTextures = new UITexture[6];
		
		this.zoneEntity = ZoneFactory.Instance.Build();
		this.userBattlerEntity = this.zoneEntity.GetPlayerBattler();
		this.userBattlerEntity.WeaponUse += this.WeaponUsed;
		this.userBattlerEntity.UpdateAP  += this.APUpdated;
		
		for (int i = 0; i < BattlerEntity.WeaponSlotNum; i++) {
			WeaponEntity weapon = this.userBattlerEntity.GetWeapon(i);
			if (weapon != null) {
				this.SetWeaponAtSlot(i, "UI/" + weapon.GetMasterWeapon().path);
			}
		}
	
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
	
	public void SetSlots(GameObject[] slots) {
		this.slots = slots;
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
			this.IntervalPlayer.Play(interval);
			
			// Damage pop
			Roga2dAnimation animation = EffectBuilder.GetInstance().BuildDamagePopAnimation(settings.Target.LocalPixelPosition, damageValue);
			this.AnimationPlayer.Play(this.stage.GetCharacterLayer(), null, animation, null);
			
			AdventureObject obj = (AdventureObject)settings.Target;
			//obj.ApplyDamage(damageValue);
		}
	}
	
	private void playSkillAnimation(AdventureObject caster, AdventureObject target, WeaponEntity weapon, SkillEntity skillEntity) {
		if (weapon == null) {
			return;	
		}
		
		if (caster.Sprite.IsVisible) {
			caster.Sprite.Hide();

			Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
				{ "Combat/BattlerBase", new Roga2dSwapTextureDef() {TextureID = caster.TextureID, PixelSize = new Vector2(32, 32)}},
				{ "Battle/Skills/Monster_Base", new Roga2dSwapTextureDef() {TextureID = "death_wind", PixelSize = target.PixelSize,  SrcRect = target.SrcRect}},
				{ "Combat/WeaponSwordBase", new Roga2dSwapTextureDef() {TextureID = weapon.GetMasterWeapon().path, PixelSize = new Vector2(32, 32),  SrcRect = new Rect(0, 0, 32, 32)}}
			};

			Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.AnimationPlayer, false, caster, caster, target, CommandCalled);
			Roga2dAnimation animation = Roga2dUtils.LoadAnimation("" + skillEntity.Path, false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Bow/Shoot", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Sword/LeaveDance", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Bow/bow_bomb", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Fire/Skill_Flare", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Laser/Skill_Laser01", false, null, settings, options);
			this.AnimationPlayer.Play(caster,  null, animation,  AnimationFinished);
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
		//this.playSkillAnimation(this.monster, this.battlers[0], weaponEntity, skillEntity);
		this.playSkillAnimation(this.battlers[0], this.monster, weaponEntity, skillEntity);
	}
	
	private void APUpdated(int newAP) {
		//Debug.Log("AP Updated = " + newAP);
	}
}
