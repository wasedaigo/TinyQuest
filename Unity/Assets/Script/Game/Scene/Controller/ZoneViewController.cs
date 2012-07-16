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
	private Dictionary<CombatUnit, Actor> actors = new Dictionary<CombatUnit, Actor>();
	
	private Vector2[,] TargetPositions = new Vector2[,]{{new Vector2(36, 10), new Vector2(-36, 10)}, {new Vector2(80, 10), new Vector2(-80, 10)}};
	private Roga2dNode[,] targetNodes = new Roga2dNode[TargetPositionCount, Constant.GroupTypeCount];
	private Actor[] activeCombatActors = new Actor[Constant.GroupTypeCount];
	private Actor poppedActor;
	
	private void Awake() {
		this.animationPlayer = new Roga2dAnimationPlayer();
		this.intervalPlayer = new Roga2dIntervalPlayer();
		
		for (int i = 0; i < TargetPositionCount; i++) {
			for (int j = 0; j < Constant.GroupTypeCount; j++) {
				this.targetNodes[i, j] = new Roga2dNode();
				this.targetNodes[i, j].LocalPixelPosition = TargetPositions[i, j];
			}
		}

		this.stage.transform.parent = this.gameObject.transform;
		for (int i = 0; i < TargetPositionCount; i++) {
			for (int j = 0; j < Constant.GroupTypeCount; j++) {
				Roga2dNode layer = this.stage.GetCharacterLayer();
				layer.AddChild(this.targetNodes[i, j]);
			}
		}
	}

	private Actor GetPlayer() {
		return this.activeCombatActors[Constant.PlayerGroupType];
	}
	
	private void PlayAnimation(Roga2dNode targetNode, Actor casterActor, string animationName, object callbackData, System.Action<Roga2dAnimation> callback) {
		Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
			{ "Combat/BattlerBase", new Roga2dSwapTextureDef() {TextureID = casterActor.TextureID, PixelSize = casterActor.PixelSize}},
			{ "Combat/MonsterBase", new Roga2dSwapTextureDef() {TextureID = casterActor.TextureID, PixelSize = casterActor.PixelSize,  SrcRect = casterActor.SrcRect}},
			{ "Combat/WeaponSwordBase", new Roga2dSwapTextureDef() {TextureID = "Weapon/1", PixelSize = new Vector2(32, 32),  SrcRect = new Rect(0, 0, 32, 32)}}
		};
		
		Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.animationPlayer, false, casterActor, casterActor, targetNode, CommandCalled);
		settings.CasterPixelSize = new Vector2(32, 32);
		settings.Data = callbackData;
		Roga2dAnimation animation = Roga2dUtils.LoadAnimation(animationName, false, null, settings, options);
		this.animationPlayer.Play(casterActor,  null, animation, callback);	
	}
	
	private void PlaySkillAnimation(CombatAction combatAction) 
	{
		Actor casterActor = this.actors[combatAction.caster];
		Actor targetActor = this.actors[combatAction.target];
		MasterSkill masterSkill = combatAction.skill;
		
		this.PlayAnimation(targetActor, casterActor, masterSkill.animation, combatAction, this.OnSkillFinished);
	}
	
	private void PlayMoveAnimation(string animationName, int groupNo, TargetPosition targetPosition, Actor actor, System.Action callback) {
		Roga2dNode targetNode = this.targetNodes[(int)targetPosition, groupNo];
		this.PlayAnimation(targetNode, actor, animationName, null, (animation) => {
			actor.LocalPixelPosition = targetNode.LocalPixelPosition;
			if (callback != null) {
				callback();	
			}
		});
	}
	
    private void OnSkillFinished(Roga2dAnimation animation)
    {
		CombatAction combatAction = (CombatAction)animation.settings.Data;
		List<System.Action<System.Action>> list = new List<System.Action<System.Action>>();
		
		// Process caster status
		if (combatAction.casterResult != null) {
			list.Add((next) => { this.ProcessUnitStatus(combatAction.casterResult, next); });
			if (combatAction.casterResult.swapUnit != null) {
				list.Add((next) => { this.SwapActor(combatAction.casterResult.combatUnit, combatAction.casterResult.swapUnit, next); });
			}
		}
		
		// Process target status
		if (combatAction.targetResult != null) {
			list.Add((next) => { this.ProcessUnitStatus(combatAction.targetResult, next); });
			if (combatAction.targetResult.swapUnit != null) {
				list.Add((next) => { this.SwapActor(combatAction.targetResult.combatUnit, combatAction.targetResult.swapUnit, next); });
			}
		}
		
		Async.Async.Instance.Waterfall(list,
			() => {
				this.SendMessage("ExecuteNextAction");
			}
		);
    }
	
	private void CombatAction(CombatAction combatAction) {
		if (combatAction.caster.GetUserUnit().Unit.lookType == UnitLookType.Monster) {
			// Monster should display attack effect first
			Roga2dBaseInterval interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
				EffectBuilder.GetInstance().BuildAttackFlashInterval(this.actors[combatAction.caster].Sprite),
				new Roga2dFunc(() => {
					this.PlaySkillAnimation(combatAction);
				})
			});
			intervalPlayer.Play(interval);
		} else {
			this.PlaySkillAnimation(combatAction);
		}
	}
	
	private void ShowEffect(Actor actor, CombatActionResult result) 
	{
		if (actor == null || result == null) {return;}

		int damageValue = result.effect;

		Roga2dBaseInterval interval = EffectBuilder.GetInstance().BuildDamageInterval(actor.Sprite);
		this.intervalPlayer.Play(interval);
		
		// Damage pop
		Roga2dAnimation animation = EffectBuilder.GetInstance().BuildDamagePopAnimation(actor.LocalPixelPosition, damageValue);
		this.animationPlayer.Play(this.stage.GetCharacterLayer(), null, animation, null);
		
		this.SendMessage("ChangeActorStatus", result);
	}

	private void CommandCalled(Roga2dAnimationSettings settings, string command) 
	{
		Actor actor = null;
		string[] commandData = command.Split(':');
		switch(commandData[0]) {
			case "damage":
				CombatAction combatAction = (CombatAction)settings.Data;
			
				Actor casterActor = this.actors[combatAction.caster];
				this.ShowEffect(casterActor, combatAction.casterResult);

				Actor targetActor = this.actors[combatAction.target];
				this.ShowEffect(targetActor, combatAction.targetResult);

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

	protected Actor BuildPuppet (string name, PuppetActor.PoseType poseType, float x, float y) {
		PuppetActor actor = new PuppetActor(name, poseType);
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

	protected void OnFinishBattle() {
		List<CombatUnit> removeList = new List<CombatUnit>();
		foreach (KeyValuePair<CombatUnit, Actor> pair in this.actors) {
			CombatUnit combatUnit = pair.Key;
			if (combatUnit.groupType == Constant.EnemyGroupType) {
				removeList.Add(combatUnit);
			}
		}
		
		// Remove non-needed actors from the scene
		foreach (CombatUnit key in removeList) {
			this.stage.GetCharacterLayer().RemoveChild(this.actors[key]);
			this.actors.Remove(key);
		}
		this.activeCombatActors[Constant.EnemyGroupType] = null;
	}
	
	private Actor BuildActor(int groupType, UnitLookType lookType, int unitId) {
		Actor actor = null;

		switch(lookType) {
			case UnitLookType.Monster:
				actor = this.BuildMonster(unitId.ToString(), TargetPositions[(int)TargetPosition.Out, groupType][0], TargetPositions[(int)TargetPosition.Out, groupType][Constant.EnemyGroupType]);
				break;
			case UnitLookType.Puppet:
				actor = this.BuildPuppet(unitId.ToString(), PuppetActor.PoseType.Stand, TargetPositions[(int)TargetPosition.Out, groupType][0], TargetPositions[(int)TargetPosition.Out, groupType][Constant.EnemyGroupType]);
				break;
		}
		
		if (groupType == 1) {
			actor.LocalScale = new Vector2(-1, 1);
		}
		
		this.stage.GetCharacterLayer().AddChild(actor);
		
		return actor;
	}
	
	protected void SpawnCombatActor(CombatUnit combatUnit) {
		if (this.actors.ContainsKey(combatUnit)) {return;};

		Actor actor = this.BuildActor(combatUnit.groupType, combatUnit.GetUserUnit().Unit.lookType, combatUnit.GetUserUnit().Unit.id);

		actor.Sprite.Hide();
		this.actors[combatUnit] = actor;
	}

	protected void ShowCombatActors(CombatUnit[] combatUnits) {
		for (int i = 0; i < Constant.GroupTypeCount; i++) {
			if (combatUnits[i] == null) { continue; }
			Actor actor = this.actors[combatUnits[i]];
			this.activeCombatActors[i] = actor;
			actor.Show();
			Roga2dNode targetNode = this.targetNodes[(int)TargetPosition.In, i];
			actor.LocalPixelPosition = targetNode.LocalPixelPosition;
		}
	}

	private void ProcessUnitStatus(CombatActionResult result, System.Action callback) {
		UserUnit userUnit = result.combatUnit.GetUserUnit();
		
		Actor actor = this.actors[result.combatUnit];
		actor.SetStatus(result.life, result.maxLife);
		
		if (userUnit.Unit.lookType == UnitLookType.Monster && result.combatUnit.IsDead) {
			Roga2dNode targetNode = null;
			this.PlayAnimation(targetNode, actor, "Combat/Monster/MonsterDie001", null, (animation) => { callback(); });
		} else {
			callback();
		}
	}

	protected void SelectCombatActors(CombatUnit[] combatUnits) {
		// Play move-in animation for each group
		List<System.Action<System.Action>> list = new List<System.Action<System.Action>>();
		for (int i = 0; i < Constant.GroupTypeCount; i++) {
			if (combatUnits[i] == null) { continue; }
			int t = i;
			string animation = "Combat/Common/Jump";
			if (combatUnits[t].GetUserUnit().Unit.lookType == UnitLookType.Monster) {
				animation = "Combat/Monster/MonsterMove001";
			}

			Actor actor = this.actors[combatUnits[t]];
			if (actor != this.activeCombatActors[t]) {
				
				// Move-out old active unit on the screen
				if (this.activeCombatActors[t] != null) {
					list.Add(
						(next) => { this.PlayMoveAnimation(animation, t, TargetPosition.Out, this.activeCombatActors[t], next); }
					);
				}
				
				// Move-in new unit into the screen
				list.Add(
					(next) => {
						this.activeCombatActors[t] = actor;
						this.PlayMoveAnimation(animation, t, TargetPosition.In, actor, next);
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

	protected void SwapActor(CombatUnit swappedUnit, CombatUnit swappingUnit, System.Action callback) {
		int groupType = swappingUnit.groupType;
		Actor swappedActor = this.actors[swappedUnit];
		Actor swappingActor = this.actors[swappingUnit];
		
		string swappingUnitAnimation = "Combat/Common/Jump";
		if (swappingUnit.GetUserUnit().Unit.lookType == UnitLookType.Monster) {
			swappingUnitAnimation = "Combat/Monster/MonsterMove001";
		}

		List<System.Action<System.Action>> list = new List<System.Action<System.Action>>();
		
		// Play rescue animation if swappedUnit is Puppet
		if (swappedUnit.GetUserUnit().Unit.lookType == UnitLookType.Puppet) {
			string swappedUnitAnimation = "Combat/Common/Jump";
			if (swappedUnit.IsDead) {
				swappedUnitAnimation = "Combat/Common/DeadJump";
			}
			
			// Swapping Unit jump-in for rescue
			list.Add(
					(next) => {
						this.PlayMoveAnimation(swappingUnitAnimation, groupType, TargetPosition.In, swappingActor, next);
					}
			);
			
			// Swapping Unit move out dead unit from the screen
			list.Add(
				(next) => {
					this.PlayMoveAnimation(swappedUnitAnimation, groupType, TargetPosition.Out, swappedActor, null);
					this.PlayMoveAnimation(swappingUnitAnimation, groupType, TargetPosition.Out, swappingActor, next);
				}
			);
		}
		
		// Move in new unit
		list.Add(
			(next) => {
				this.PlayMoveAnimation(swappingUnitAnimation, groupType, TargetPosition.In, swappingActor, next);
				this.activeCombatActors[groupType] = swappingActor;
			}
		);

		Async.Async.Instance.Waterfall(list,
			() => {
				callback();
			}
		);
	}

	public void PopActor(MasterUnit masterUnit) {
		int groupType = 1;
		this.poppedActor = this.BuildActor(groupType, masterUnit.lookType, masterUnit.id);	
		
		System.Action callback = () => { 
			this.SendMessage("OnActorPopped");
		};
		
		this.PlayMoveAnimation("Combat/Monster/MonsterMove001", groupType, TargetPosition.In, this.poppedActor, callback);
	}

	public void DepopActor() {
		int groupType = 1;
		System.Action callback = () => { 
			this.poppedActor.Destroy();
			this.poppedActor = null;
			this.SendMessage("OnActorDepopped");
		};
		
		this.PlayMoveAnimation("Combat/Monster/MonsterMove001", groupType, TargetPosition.Out, this.poppedActor, callback);
	}
	
	public void PlayerMoveIn(bool isStart) {
		if (isStart) {
			Roga2dBaseInterval interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
				new Roga2dFunc(() => {this.GetPlayer().startWalkingAnimation();}),
				new Roga2dPositionInterval(
					this.GetPlayer(), 
					this.targetNodes[(int)TargetPosition.Out, Constant.PlayerGroupType].LocalPosition, 
					this.targetNodes[(int)TargetPosition.In, Constant.PlayerGroupType].LocalPosition, 1.0f, Roga2dTweenType.Linear, null),
				new Roga2dFunc(() => {
					this.SendMessage("OnPlayerMovedIn");
				})
			});
			this.intervalPlayer.Play(interval);
		} else {
			this.GetPlayer().LocalPosition = this.targetNodes[(int)TargetPosition.In, Constant.PlayerGroupType].LocalPosition;
			this.SendMessage("OnPlayerMovedIn");	
		}
	}
	
	public void PlayerMoveOut() {
		Roga2dBaseInterval interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
			new Roga2dFunc(() => {this.GetPlayer().startWalkingAnimation();}),
			new Roga2dPositionInterval(
				this.GetPlayer(),
				this.targetNodes[(int)TargetPosition.In, Constant.PlayerGroupType].LocalPosition,
				this.targetNodes[(int)TargetPosition.Out, Constant.EnemyGroupType].LocalPosition, 2.0f, Roga2dTweenType.Linear, null),
			new Roga2dFunc(() => {
				this.SendMessage("OnPlayerMovedOut");
			})
		});
		this.intervalPlayer.Play(interval);
	}

	protected void Update() {
		this.animationPlayer.Update(Time.deltaTime);
		this.intervalPlayer.Update();
		this.stage.UpdateView();
	}

	protected void OnDestroy() {
		Roga2dResourceManager.freeResources();	
	}
	
	protected void StartWalkAnimation(CombatUnit combatUnit) {
		this.actors[combatUnit].startWalkingAnimation();
	}
	
	protected void StopWalkAnimation(CombatUnit combatUnit) {
		this.actors[combatUnit].stopWalkingAnimation();
	}
	
	protected void OnPlayerMoved(float distance) {
		this.stage.Scroll(distance);
	}
}