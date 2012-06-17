using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TinyQuest.Model;
using TinyQuest.Factory.Model;
using TinyQuest.Object;
using TinyQuest.Core;
using TinyQuest.Data;

public class CombatController : BaseStageController {
	public System.Action CombatFinish;
	public GameObject BattlerStatus;
	
	public UILabel AllyHP;
	public UILabel AllyTP;
	public UILabel EnemyHP;
	public UILabel EnemyTP;
	
	private Monster monster;
	private List<AdventureObject> battlers = new List<AdventureObject>();
	private CombatModel combatModel;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		
	}

	// Update is called once per frame
	protected override void Update () {
		if (!this.AnimationPlayer.HasPlayingAnimations()) {
			if (this.monster != null && this.monster.IsDead()) {
				this.Stage.GetCharacterLayer().RemoveChild(this.monster);
				this.monster = null;
				
				if (this.CombatFinish != null) {
					this.CombatFinish();	
				}
			}	
		}
		
		base.Update();
	}
	
	public void SetCombatModel(CombatModel combatModel) {
		this.combatModel = combatModel;
		this.combatModel.SkillUse += this.SkillUsed;
		this.combatModel.UpdateHP += this.HPUpdated;
	}
	
	public void SetPlayer(Ally player) {
		this.battlers.Add(player);
	}
	
	public void StartBattle() {
		this.monster = spawnMonster("goblin", -30, -5);
		//this.monster = spawnBattler("fighter", Ally.State.Stand, -40, 0);
		this.monster.LocalScale = new Vector2(1, 1);
		this.monster.LocalPriority = 0.45f;
		this.Stage.GetCharacterLayer().AddChild(this.monster);

		BattlerStatus.SetActiveRecursively(true);
		this.UpdateStatus();
		this.combatModel.Start();
	}
	
	private void UpdateStatus() {
		BattlerModel ally = this.combatModel.GetBattler(BattlerModel.GroupType.Player, 0);
		BattlerModel enemy = this.combatModel.GetBattler(BattlerModel.GroupType.Enemy, 0);
		this.AllyHP.text = ally.HP.ToString();
		this.EnemyHP.text = enemy.HP.ToString();
	}
	
    private void AnimationFinished(Roga2dAnimation animation)
    {
		animation.settings.Origin.Show();
		animation.settings.Destroy();
		animation.settings = null;
		
		this.combatModel.ProgressTurn();
		this.UpdateStatus();
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
			this.AnimationPlayer.Play(this.Stage.GetCharacterLayer(), null, animation, null);
			
			//AdventureObject obj = (AdventureObject)settings.Target;
			//obj.ApplyDamage(damageValue);
		}
	}
	
	private void playSkillAnimation(AdventureObject caster, AdventureObject target, SkillModel skillModel) {
		if (skillModel == null) {
			return;	
		}
		
		if (caster.Sprite.IsVisible) {
			caster.Sprite.Hide();

			Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
				{ "Combat/BattlerBase", new Roga2dSwapTextureDef() {TextureID = caster.TextureID, PixelSize = new Vector2(32, 32)}},
				{ "Battle/Skills/Monster_Base", new Roga2dSwapTextureDef() {TextureID = "death_wind", PixelSize = target.PixelSize,  SrcRect = target.SrcRect}}//,
//				{ "Combat/WeaponSwordBase", new Roga2dSwapTextureDef() {TextureID = skillModel.OwnerWeapon.GetMasterWeapon().GetAnimationImagePath(), PixelSize = new Vector2(32, 32),  SrcRect = new Rect(0, 0, 32, 32)}}
			};

			Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.AnimationPlayer, false, caster, caster, target, CommandCalled);
			Roga2dAnimation animation = Roga2dUtils.LoadAnimation("" + skillModel.MasterSkill.animation, false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Bow/Shoot", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Sword/LeaveDance", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Bow/bow_bomb", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Fire/Skill_Flare", false, null, settings, options);
			//Roga2dAnimation animation = Roga2dUtils.LoadAnimation("Battle/Skills/Laser/Skill_Laser01", false, null, settings, options);
			this.AnimationPlayer.Play(caster,  null, animation,  AnimationFinished);
		}
	}
	
	private void SkillUsed(SkillModel skillModel) {
		this.playSkillAnimation(this.battlers[0], this.monster, skillModel);
	}

	private void HPUpdated(BattlerModel entity, int value) {

	}

	public void InvokeCommand(int slotNo) {
		this.combatModel.GetActiveBattler().UseSkill(slotNo, this.combatModel.GetBattler(BattlerModel.GroupType.Enemy, 0));
	}
	
}
