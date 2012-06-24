using JsonFx.Json;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Scene;
using TinyQuest.Scene.Model;
using TinyQuest.Object;

public class ZoneViewController : MonoBehaviour {
	
	public Stage stage;
	private Roga2dBaseInterval interval;

	private Roga2dAnimationPlayer animationPlayer;
	private Roga2dIntervalPlayer intervalPlayer;
	private Dictionary<UserUnit, Actor> actors = new Dictionary<UserUnit, Actor>(); 
	
    private void OnAnimationFinished(Roga2dAnimation animation)
    {
		animation.settings.Origin.Show();
		animation.settings.Destroy();
		animation.settings = null;
		
		this.SendMessage("AnimationFinished");
		
    }
	
	private void PlaySkillAnimation(SkillAnimationParams skillAnimationParams) 
	{
		Actor casterActor = this.actors[skillAnimationParams.Caster];
		Actor targetActor = this.actors[skillAnimationParams.Target];
		MasterSkill masterSkill = skillAnimationParams.MasterSkill;
		if (casterActor.Sprite.IsVisible) {
			casterActor.Sprite.Hide();

			Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
				{ "Combat/BattlerBase", new Roga2dSwapTextureDef() {TextureID = casterActor.TextureID, PixelSize = new Vector2(32, 32)}},
				{ "Battle/Skills/Monster_Base", new Roga2dSwapTextureDef() {TextureID = "death_wind", PixelSize = targetActor.PixelSize,  SrcRect = targetActor.SrcRect}},
				{ "Combat/WeaponSwordBase", new Roga2dSwapTextureDef() {TextureID = "Weapon/1", PixelSize = new Vector2(32, 32),  SrcRect = new Rect(0, 0, 32, 32)}}
			};

			Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.animationPlayer, false, casterActor, casterActor, targetActor, CommandCalled);
			Roga2dAnimation animation = Roga2dUtils.LoadAnimation("" + masterSkill.animation, false, null, settings, options);
			this.animationPlayer.Play(casterActor,  null, animation,  this.OnAnimationFinished);
		}
	}

	private void CommandCalled(Roga2dAnimationSettings settings, string command) 
	{
		string[] commandData = command.Split(':');
		if (commandData[0] == "damage") {
			uint damageValue = 2750;
			// Flash effect
			Roga2dBaseInterval interval = EffectBuilder.GetInstance().BuildDamageInterval(settings.Target);
			this.intervalPlayer.Play(interval);
			
			// Damage pop
			Roga2dAnimation animation = EffectBuilder.GetInstance().BuildDamagePopAnimation(settings.Target.LocalPixelPosition, damageValue);
			this.animationPlayer.Play(this.stage.GetCharacterLayer(), null, animation, null);
			
			//AdventureObject obj = (AdventureObject)settings.Target;
			//obj.ApplyDamage(damageValue);
		}
	}

	protected Actor BuildPuppet (string name, PuppetActor.State state, float x, float y) {
		PuppetActor actor = new PuppetActor(name, state);
		actor.LocalPriority = 0.45f;
		actor.LocalPixelPosition = new Vector2(x, y);
		actor.UpdatePriority();
		return actor;
	}

	protected Actor BuildMonster (string name, float x, float y) {
		MonsterActor actor = new MonsterActor(name);
		actor.LocalPriority = 0.45f;
		actor.LocalPixelPosition = new Vector2(x, y);
		actor.UpdatePriority();
		
		return actor;
	}

	protected void SpawnActor(UserUnit userUnit) {
		Actor actor = null;
		switch(userUnit.Unit.lookType) {
			case UnitLookType.Puppet:
				actor = this.BuildMonster("goblin", -30, 0);
				break;
			case UnitLookType.Monster:
				actor = this.BuildPuppet(userUnit.Unit.id.ToString(), PuppetActor.State.Stand, 10 + 32 * ((userUnit.id - 1) % 2), -10 + 16 * ((userUnit.id - 1) / 2));
				actor.LocalPriority += 0.001f * userUnit.id;
				break;
		}
		
		this.actors[userUnit] = actor;
		this.stage.GetCharacterLayer().AddChild(actor);
	}

	// Use this for initialization
	protected void Start() {
		this.animationPlayer = new Roga2dAnimationPlayer();
		this.intervalPlayer = new Roga2dIntervalPlayer();
		
		this.stage.transform.parent = this.gameObject.transform;
	}

	public void PlayerMoveIn(bool immediate) {
		/*
		if (immediate) {
			this.player.LocalPixelPosition = new Vector2(20, PlayerY);
			this.SendMessage("OnPlayerMovedIn");
		} else {
			this.interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
				new Roga2dFunc(() => {this.player.startWalkingAnimation();}),
				new Roga2dPositionInterval(this.player, Roga2dUtils.pixelToLocal(new Vector2(80, PlayerY)), Roga2dUtils.pixelToLocal(new Vector2(40, PlayerY)), 1.0f, true, null),
				new Roga2dFunc(() => {
					this.SendMessage("OnPlayerMovedIn");
				})
			});
			this.IntervalPlayer.Play(this.interval);
		}
		*/
	}
	
	public void PlayerMoveOut() {
		/*
		this.interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
			new Roga2dFunc(() => {this.player.startWalkingAnimation();}),
			new Roga2dPositionInterval(this.player, Roga2dUtils.pixelToLocal(new Vector2(40, PlayerY)), Roga2dUtils.pixelToLocal(new Vector2(-100, PlayerY)), 2.0f, true, null),
			new Roga2dFunc(() => {
				this.SendMessage("OnPlayerMovedOut");
			})
		});
		this.IntervalPlayer.Play(this.interval);
		*/
	}

	protected void Update() {
		this.stage.UpdateView();
		
		this.animationPlayer.Update(Time.deltaTime);
		this.intervalPlayer.Update();
	}

	protected void OnDestroy() {
		Roga2dResourceManager.freeResources();	
	}
	
	protected void OnPlayerMoved(float distance) {
		this.stage.Scroll(distance);
	}

	public void OnStateChanged(ZoneState state) {
		/*
		switch (state) {
			case ZoneState.Combat:
				this.player.stopWalkingAnimation();
				break;
			case ZoneState.Next:
				this.player.stopWalkingAnimation();
				break;
			case ZoneState.Moving:
				this.player.startWalkingAnimation();
				break;
			case ZoneState.Pause:
				this.player.stopWalkingAnimation();
				break;
		}
		*/
	}
}