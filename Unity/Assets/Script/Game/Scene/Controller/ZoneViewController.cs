using JsonFx.Json;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Scene;
using TinyQuest.Scene.Model;
using TinyQuest.Object;
using Async;

public class ZoneViewController : MonoBehaviour {
	private enum TargetPosition {
		In,
		Out
	}
	private const int TargetPositionCount = 2;
		
	public Stage stage;
	private Roga2dBaseInterval interval;

	private Roga2dAnimationPlayer animationPlayer;
	private Roga2dIntervalPlayer intervalPlayer;
	private Dictionary<UserUnit, Actor> actors = new Dictionary<UserUnit, Actor>();
	
	private Vector2[,] TargetPositions = new Vector2[,]{{new Vector2(36, 0), new Vector2(-36, 0)}, {new Vector2(80, 0), new Vector2(-80, 0)}};
	private Roga2dNode[,] targetNodes = new Roga2dNode[TargetPositionCount, Constant.GroupTypeCount];
	private Actor[] activeActors = new Actor[Constant.GroupTypeCount];
	
	private void PlayAnimation(int groupNo, TargetPosition targetPosition, Actor casterActor, string animationName, System.Action<Roga2dAnimation> callback) {
		Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
			{ "Combat/BattlerBase", new Roga2dSwapTextureDef() {TextureID = casterActor.TextureID, PixelSize = new Vector2(32, 32)}},
			{ "Battle/Skills/Monster_Base", new Roga2dSwapTextureDef() {TextureID = "death_wind", PixelSize = casterActor.PixelSize,  SrcRect = casterActor.SrcRect}}
		};
		
		Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.animationPlayer, false, casterActor, casterActor, this.targetNodes[(int)targetPosition, groupNo], CommandCalled);
		Roga2dAnimation animation = Roga2dUtils.LoadAnimation(animationName, false, null, settings, options);
		this.animationPlayer.Play(casterActor,  null, animation, callback);	
	}
	
    private void OnSkillAnimationFinished(Roga2dAnimation animation)
    {
		
		this.SendMessage("ExecuteNextAction");
    }
	
	private void CombatAction(SkillAnimationParams skillAnimationParams) {
		if (skillAnimationParams.Caster.Unit.lookType == UnitLookType.Monster) {
			// Monster should display attack effect first
			Roga2dBaseInterval interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
				EffectBuilder.GetInstance().BuildAttackFlashInterval(this.actors[skillAnimationParams.Caster].Sprite),
				new Roga2dFunc(() => {
					this.PlaySkillAnimation(skillAnimationParams);
				})
			});
			intervalPlayer.Play(interval);
		} else {
			this.PlaySkillAnimation(skillAnimationParams);
		}
	}
	
	private void PlaySkillAnimation(SkillAnimationParams skillAnimationParams) 
	{
		Actor casterActor = this.actors[skillAnimationParams.Caster];
		Actor targetActor = this.actors[skillAnimationParams.Target];
		MasterSkill masterSkill = skillAnimationParams.MasterSkill;

		Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
			{ "Combat/BattlerBase", new Roga2dSwapTextureDef() {TextureID = casterActor.TextureID, PixelSize = new Vector2(32, 32)}},
			{ "Battle/Skills/Monster_Base", new Roga2dSwapTextureDef() {TextureID = "death_wind", PixelSize = casterActor.PixelSize,  SrcRect = casterActor.SrcRect}},
			{ "Combat/WeaponSwordBase", new Roga2dSwapTextureDef() {TextureID = "Weapon/1", PixelSize = new Vector2(32, 32),  SrcRect = new Rect(0, 0, 32, 32)}}
		};
		Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.animationPlayer, false, casterActor, casterActor, targetActor, CommandCalled);
		Roga2dAnimation animation = Roga2dUtils.LoadAnimation("" + masterSkill.animation, false, null, settings, options);
		this.animationPlayer.Play(casterActor,  null, animation,  this.OnSkillAnimationFinished);
	}
	
	private void CommandCalled(Roga2dAnimationSettings settings, string command) 
	{
		Actor actor = null;
		string[] commandData = command.Split(':');
		switch(commandData[0]) {
			case "damage":
				uint damageValue = 2750;
				// Flash effect
				actor = settings.Target as Actor;
				Roga2dBaseInterval interval = EffectBuilder.GetInstance().BuildDamageInterval(actor.Sprite);
				this.intervalPlayer.Play(interval);
				
				// Damage pop
				Roga2dAnimation animation = EffectBuilder.GetInstance().BuildDamagePopAnimation(settings.Target.LocalPixelPosition, damageValue);
				this.animationPlayer.Play(this.stage.GetCharacterLayer(), null, animation, null);
				break;
			case "hide":
				actor = settings.Root as Actor;
				actor.Sprite.Hide();
				break;
			case "show":
				actor = settings.Root as Actor;
				actor.Sprite.Show();
				break;
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

	protected void SpawnActor(CombatUnit combatUnit) {
		Actor actor = null;
		int groupType = combatUnit.groupType;
		UserUnit userUnit = combatUnit.GetUserUnit();
		switch(userUnit.Unit.lookType) {
			case UnitLookType.Monster:
				actor = this.BuildMonster("goblin", TargetPositions[(int)TargetPosition.Out, groupType][0], TargetPositions[(int)TargetPosition.Out, groupType][1]);
				break;
			case UnitLookType.Puppet:
				actor = this.BuildPuppet(userUnit.Unit.id.ToString(), PuppetActor.State.Stand, TargetPositions[(int)TargetPosition.Out, groupType][0], TargetPositions[(int)TargetPosition.Out, groupType][1]);
				actor.LocalPriority += 0.001f * userUnit.id;
				break;
		}
		
		if (groupType == 1) {
			actor.LocalScale = new Vector2(-1, 1);
		}
		
		actor.Sprite.Hide();
		this.actors[userUnit] = actor;
		this.stage.GetCharacterLayer().AddChild(actor);
	}
	
	private void PlayJumpAnimation(int groupNo, TargetPosition targetPosition, Actor actor, System.Action callback) {
		this.PlayAnimation(groupNo, targetPosition, actor, "Combat/Common/Jump", (animation) => {
			actor.LocalPixelPosition = this.targetNodes[(int)targetPosition, groupNo].LocalPixelPosition;
			callback();	
		});
	}
	
	protected void SelectActor(CombatUnit[] combatUnits) {
		List<System.Action<System.Action>> list = new List<System.Action<System.Action>>();
		for (int i = 0; i < Constant.GroupTypeCount; i++) {
			int t = i;
			Actor actor = this.actors[combatUnits[t].GetUserUnit()];
			if (actor != this.activeActors[t]) {
				if (this.activeActors[t] != null) {
					list.Add(
						(next) => {
							this.PlayJumpAnimation(t, TargetPosition.Out, this.activeActors[t], next);
						}
				);
				}
				list.Add(
					(next) => {
						this.activeActors[t] = actor;
						this.PlayJumpAnimation(t, TargetPosition.In, actor, next);
					}
				);
			}
		}

		Async.Async.Instance.Parallel(list,
			() => {
				this.SendMessage("ExecuteNextAction");
			}
		);
	}

	// Use this for initialization
	protected void Start() {
		this.animationPlayer = new Roga2dAnimationPlayer();
		this.intervalPlayer = new Roga2dIntervalPlayer();
		
		this.stage.transform.parent = this.gameObject.transform;
		
		for (int i = 0; i < TargetPositionCount; i++) {
			for (int j = 0; j < Constant.GroupTypeCount; j++) {
				this.targetNodes[i, j] = new Roga2dNode();
				this.targetNodes[i, j].LocalPixelPosition = TargetPositions[i, j];
				this.stage.GetCharacterLayer().AddChild(this.targetNodes[i, j]);
			}
		}
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
		/*foreach (KeyValuePair<UserUnit, Actor> pair in this.actors) {
			pair.Value.Update();
		}*/
		
		
		this.animationPlayer.Update(Time.deltaTime);
		this.intervalPlayer.Update();
		this.stage.UpdateView();
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