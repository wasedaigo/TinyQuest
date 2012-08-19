using JsonFx.Json;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Data.Skills;
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
	private const int UnitPosY = 30;
	private const int TargetPositionCount = 2;
	public Stage stage;
	private Roga2dBaseInterval interval;
	
	private Roga2dAnimationPlayer animationPlayer;
	private Roga2dIntervalPlayer intervalPlayer;
	

	private Vector2[,] _targetPositions = new Vector2[,]{{new Vector2(36, UnitPosY), new Vector2(-36, UnitPosY)}, {new Vector2(80, UnitPosY), new Vector2(-80, UnitPosY)}};
	private Vector2 GetTargetPositions(int targetPosition, int groupNo) {
		return this._targetPositions[targetPosition, groupNo];
	}
	
	private Vector2[,] _startTargetPositions = new Vector2[,]{
		{new Vector2(12, -16), new Vector2(32, -16), new Vector2(20, 0), new Vector2(40, 0), new Vector2(28, 16), new Vector2(48, 16)}, 
		{new Vector2(-12, -16), new Vector2(-32, -16), new Vector2(-20, 0), new Vector2(-40, 0), new Vector2(-28, 16), new Vector2(-48, 16)}
	};
	private Vector2 GetStartTargetPositions(int groupNo, int index) {
		return this._startTargetPositions[groupNo, index];
	}
	
	private Roga2dNode[,] _spawnNodes = new Roga2dNode[TargetPositionCount, Constant.GroupCount];
	private Roga2dNode GetSpawnNode(int targetPosition, int groupNo) {
		return this._spawnNodes[targetPosition, groupNo];
	}
	
	private Roga2dNode[,] _startTargetNodes = new Roga2dNode[Constant.GroupCount, Constant.UnitCount];
	private Roga2dNode GetStartTargetNode(int groupNo, int index) {
		return this._startTargetNodes[groupNo, index];
	}
	
	private CombatUnit[] activeCombatUnits = new CombatUnit[Constant.GroupCount];
	
	private Actor poppedActor;
	
	private Actor[,] actors = new Actor[Constant.GroupCount, Constant.UnitCount];
	private CombatModel combatModel;
	
	private void Awake() {
		Roga2dNode layer = this.stage.GetCharacterLayer();
		this.animationPlayer = new Roga2dAnimationPlayer();
		this.intervalPlayer = new Roga2dIntervalPlayer();
		
		for (int i = 0; i < TargetPositionCount; i++) {
			for (int j = 0; j < Constant.GroupCount; j++) {
				this._spawnNodes[i, j] = new Roga2dNode("Target" + i + ", " + j);
				this._spawnNodes[i, j].LocalPixelPosition = GetTargetPositions(i, j);
				layer.AddChild(this._spawnNodes[i, j]);
			}
		}
		
		for (int i = 0; i < Constant.GroupCount; i++) {
			for (int j = 0; j < Constant.UnitCount; j++) {
				this._startTargetNodes[i, j] = new Roga2dNode("StartTarget" + i + ", " + j);
				this._startTargetNodes[i, j].LocalPixelPosition = GetStartTargetPositions(i, j);
				layer.AddChild(this._startTargetNodes[i, j]);
			}
		}
	}

	public void SetModel(CombatModel combatModel) {
		this.combatModel = combatModel;
	}
	
	private void PlayBattlerAnimation(Roga2dNode targetNode, Actor casterActor, string animationName, object callbackData, System.Action<Roga2dAnimation> callback) {
		Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
			{ "Combat/BattlerBase", new Roga2dSwapTextureDef() {TextureID = casterActor.TextureID, PixelSize = casterActor.PixelSize}},
			{ "Combat/MonsterBase", new Roga2dSwapTextureDef() {TextureID = casterActor.TextureID, PixelSize = casterActor.PixelSize,  SrcRect = casterActor.SrcRect}},
			{ "Combat/WeaponSwordBase", new Roga2dSwapTextureDef() {TextureID = "Weapon/1", PixelSize = new Vector2(32, 32),  SrcRect = new Rect(0, 0, 32, 32)}}
		};
		
		this.PlayAnimation(targetNode, casterActor, animationName, callbackData, options, callback);
	}
	
	private void PlayAnimation(Roga2dNode targetNode, Actor casterActor, string animationName, object callbackData, Dictionary<string, Roga2dSwapTextureDef> options, System.Action<Roga2dAnimation> callback) {
		Roga2dAnimationSettings settings = new Roga2dAnimationSettings(this.animationPlayer, false, casterActor, casterActor, targetNode, CommandCalled);
		settings.CasterPixelSize = new Vector2(32, 32);
		settings.Data = callbackData;
		Roga2dAnimation animation = Roga2dUtils.LoadAnimation(animationName, false, null, settings, options);
		this.animationPlayer.Play(casterActor,  null, animation, callback);	
	}
	
	private void PlaySkillAnimation(CombatAction combatAction) 
	{
		Actor casterActor = this.actors[combatAction.caster.groupNo, combatAction.caster.index];
		Actor targetActor = this.actors[combatAction.target.groupNo, combatAction.target.index];
		BaseSkill.SkillResult skillResult = combatAction.skillResult;
		
		Dictionary<string, Roga2dSwapTextureDef> options = new Dictionary<string, Roga2dSwapTextureDef>() {
			{ "Combat/TargetBase", new Roga2dSwapTextureDef() {TextureID = targetActor.TextureID, PixelSize = targetActor.PixelSize}},
			{ "Combat/BattlerBase", new Roga2dSwapTextureDef() {TextureID = casterActor.TextureID, PixelSize = casterActor.PixelSize}},
			{ "Combat/MonsterBase", new Roga2dSwapTextureDef() {TextureID = casterActor.TextureID, PixelSize = casterActor.PixelSize,  SrcRect = casterActor.SrcRect}},
			{ "Combat/WeaponSwordBase", new Roga2dSwapTextureDef() {TextureID = "Weapon/1", PixelSize = new Vector2(32, 32),  SrcRect = new Rect(0, 0, 32, 32)}}
		};

		this.PlayAnimation(targetActor, casterActor, skillResult.animation, combatAction, options, this.OnSkillFinished);
	}
	
	private void PlayMoveAnimation(string animationName, int groupNo, TargetPosition targetPosition, Actor actor, System.Action callback) {
		Roga2dNode targetNode = this.GetSpawnNode((int)targetPosition, groupNo);
		this.PlayBattlerAnimation(targetNode, actor, animationName, null, (animation) => {
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
		}
		
		// Process target status
		if (combatAction.targetResult != null) {
			list.Add((next) => { this.ProcessUnitStatus(combatAction.targetResult, next); });
		}
		
		Async.Async.Instance.Waterfall(list,
			() => {
				this.SendMessage("ExecuteNextAction");
			}
		);
    }
	
	private Actor GetActorFromCombatUnit(CombatUnit combatUnit) {
		return this.actors[combatUnit.groupNo, combatUnit.index];
	}
	
	private void CombatAction(CombatAction combatAction) {
		Roga2dBaseInterval interval = null;
		if (combatAction.caster.GetUserUnit().Unit.lookType == UnitLookType.Monster) {
			// Monster should display attack effect first
			interval = new Roga2dSequence(new List<Roga2dBaseInterval> {
				EffectBuilder.GetInstance().BuildAttackFlashInterval(this.GetActorFromCombatUnit(combatAction.caster).Sprite),
				new Roga2dFunc(() => {
					this.SendMessage("HideMessage");	
					this.PlaySkillAnimation(combatAction);
				})
			});
			
		} else {
			List<Roga2dBaseInterval> list = new List<Roga2dBaseInterval>();
			
			// Show shouting sequence for skills
			if (combatAction.skillResult.shout != "") {
				list.Add(
					new Roga2dFunc(() => {
						if (combatAction.skillResult.shout != "") {
							ZoneMessageCutScene cutScene = new ZoneMessageCutScene();
							cutScene.text = combatAction.skillResult.shout;
							cutScene.pos = combatAction.caster.groupNo;
							this.SendMessage("ShowMessage", cutScene);	
						}
					})
				);
				list.Add(
					new Roga2dWait(1.0f)
				);
			}
			
			// Execute action
			list.Add(
				new Roga2dFunc(() => {
					this.SendMessage("HideMessage");	
					this.PlaySkillAnimation(combatAction);
				})
			);
			interval = new Roga2dSequence(list);
		}
		intervalPlayer.Play(interval);
	}
	
	private void ShowDamageEffect(Actor actor, CombatActionResult result) 
	{
		if (actor == null || result == null) {return;}
		int damageValue = result.effect;
		if (damageValue > 0) {
			Roga2dBaseInterval interval = EffectBuilder.GetInstance().BuildDamageInterval(actor.Sprite);
			this.intervalPlayer.Play(interval);
			this.StartCoroutine(this.ShowDamagePose(actor));	
		}
	}
	
	private IEnumerator ShowDamagePose(Actor actor) {
		actor.SetPoseType(Actor.PoseType.Attacked);
		yield return new WaitForSeconds(0.3f);
		actor.ResetPose();
	}

	private void PopDamage(Actor actor, CombatActionResult result) 
	{
		if (actor == null || result == null) {return;}
		
		int damageValue = result.effect;
		Roga2dAnimation animation = EffectBuilder.GetInstance().BuildDamagePopAnimation(actor.LocalPixelPosition, damageValue);
		this.animationPlayer.Play(this.stage.GetCharacterLayer(), null, animation, null);
		this.SendMessage("ChangeActorStatus", result);
	}

	private void CommandCalled(Roga2dAnimationSettings settings, string command) 
	{
		Actor actor = null;
		string[] commandData = command.Split(':');
		switch(commandData[0]) {
			case "target_pop_damage":
				{
				CombatAction combatAction = (CombatAction)settings.Data;
				Actor targetActor = this.GetActorFromCombatUnit(combatAction.target);
				this.PopDamage(targetActor, combatAction.targetResult);
				}

				break;
			case "caster_pop_damage":
				{
				CombatAction combatAction = (CombatAction)settings.Data;
				Actor casterActor = this.GetActorFromCombatUnit(combatAction.caster);
				this.PopDamage(casterActor, combatAction.casterResult);
				}
				break;
			case "target_damage":
				{
				CombatAction combatAction = (CombatAction)settings.Data;
				Actor targetActor = this.GetActorFromCombatUnit(combatAction.target);
				this.ShowDamageEffect(targetActor, combatAction.targetResult);
				}
				break;
			case "caster_damage":
				{
				CombatAction combatAction = (CombatAction)settings.Data;
				Actor casterActor = this.GetActorFromCombatUnit(combatAction.caster);
				this.ShowDamageEffect(casterActor, combatAction.casterResult);
				}
				break;
			case "hide":
				actor = settings.Root as Actor;
				actor.Sprite.Hide();
				break;
			case "show":
				actor = settings.Root as Actor;
				actor.Sprite.Show();
				break;
			case "hide_target":
				actor = settings.Target as Actor;
				actor.Sprite.Hide();
				break;
			case "show_target":
				actor = settings.Target as Actor;
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
		foreach (Actor actor in this.actors) {
			this.stage.GetCharacterLayer().RemoveChild(actor);
		}
	}
	
	private Actor BuildActor(int groupType, UnitLookType lookType, int unitId) {
		Actor actor = null;
		
		Vector2 pos = GetSpawnNode((int)TargetPosition.Out, groupType).LocalPixelPosition;
		switch(lookType) {
			case UnitLookType.Monster:
				actor = this.BuildMonster(unitId.ToString(), pos.x, pos.y);
				break;
			case UnitLookType.Puppet:
				actor = this.BuildPuppet(unitId.ToString(), PuppetActor.PoseType.Attack, pos.x, pos.y);
				break;
		}
		
		if (0 != groupType) {
			actor.LocalScale = new Vector2(-1, 1);
		}
		
		this.stage.GetCharacterLayer().AddChild(actor);
		return actor;
	}
	
	protected void SpawnCombatActor(CombatUnit combatUnit) {
		Actor actor = this.BuildActor(combatUnit.groupNo, combatUnit.GetUserUnit().Unit.lookType, combatUnit.GetUserUnit().Unit.id);
		this.actors[combatUnit.groupNo, combatUnit.index] = actor;
		actor.LocalPriority += combatUnit.index * 0.01f;
	}
	
	private void MoveActor(Actor actor, Roga2dNode targetNode) {
		string animationName = "Combat/Common/Jump";
		this.PlayBattlerAnimation(targetNode, actor, animationName, null, (animation) => {
			actor.LocalPixelPosition = targetNode.LocalPixelPosition;
		});
	}
	
	protected void MoveActorFront(CombatUnit combatUnit) {
		Roga2dNode targetNode = this.GetStartTargetNode(combatUnit.groupNo, combatUnit.index);
		Actor actor = this.actors[combatUnit.groupNo, combatUnit.index];
		this.MoveActor(actor, targetNode);
	}
	
	protected void MoveActorBack(CombatUnit combatUnit) {
		Roga2dNode targetNode = this.GetSpawnNode((int)TargetPosition.Out, combatUnit.groupNo);
		Actor actor = this.actors[combatUnit.groupNo, combatUnit.index];
		actor.SetPoseType(Actor.PoseType.Stand);
		this.MoveActor(actor, targetNode);
	}
	
	private void ProcessUnitStatus(CombatActionResult result, System.Action callback) {
		UserUnit userUnit = result.combatUnit.GetUserUnit();
		Actor actor = this.actors[result.combatUnit.groupNo, result.combatUnit.index];
		actor.SetStatus(result.life, result.maxLife);
		
		if (userUnit.Unit.lookType == UnitLookType.Monster && result.combatUnit.IsDead) {
			Roga2dNode targetNode = null;
			this.PlayBattlerAnimation(targetNode, actor, "Combat/Monster/MonsterDie001", null, (animation) => { callback(); });
		} else {
			callback();
		}
	}
	
	public CombatUnit GetActiveCombatUnit(int groupNo) {
		return this.activeCombatUnits[groupNo];
	}
	
	public Actor GetActiveCombatActor(int groupNo) {
		CombatUnit combatUnit = this.GetActiveCombatUnit(groupNo);
		if (combatUnit == null) {
			return null;
		} else {
			return this.actors[combatUnit.groupNo, combatUnit.index];
		}
	}
	
	public void SelectCombatActor(CombatUnit combatUnit, System.Action callback) {
		int groupNo = combatUnit.groupNo;
		
		CombatUnit activeCombatUnit = this.GetActiveCombatUnit(groupNo);
		if (activeCombatUnit != null && activeCombatUnit.IsDead) {
			this.SwapCombatActor(activeCombatUnit, combatUnit, callback);
		} else {
			// Play move-in animation for each group
			List<System.Action<System.Action>> list = new List<System.Action<System.Action>>();
	
			if (combatUnit == null) { return; }
	
			string animation = "Combat/Common/Jump";
			if (combatUnit.GetUserUnit().Unit.lookType == UnitLookType.Monster) {
				animation = "Combat/Monster/MonsterMove001";
			}

			Actor actor = this.actors[groupNo, combatUnit.index];
			Actor activeActor = this.GetActiveCombatActor(groupNo);
			if (actor != activeActor) {
				
				// Move-out old active unit on the screen
				if (activeActor != null) {
					list.Add(
						(next) => {
							this.SendMessage("CombatUnitMoveOut", activeCombatUnit);
							this.PlayMoveAnimation(animation, groupNo, TargetPosition.Out, activeActor, next); 
						}
					);
				}
				
				// Move-in new unit into the screen
				list.Add(
					(next) => {
						this.SendMessage("CombatUnitMoveIn", combatUnit);
						this.activeCombatUnits[groupNo] = combatUnit;
						this.PlayMoveAnimation(animation, groupNo, TargetPosition.In, actor, next);
					}
				);
			}
	
			Async.Async.Instance.Waterfall(list, () => {
				callback();
			});
		}
	}
	
	private void SwapCombatActor(CombatUnit swappedUnit, CombatUnit swappingUnit, System.Action callback) {
		int groupType = swappingUnit.groupNo;
		Actor swappedActor = this.actors[swappedUnit.groupNo, swappedUnit.index];
		Actor swappingActor = this.actors[swappingUnit.groupNo, swappingUnit.index];
		
		if (swappedActor == swappingActor) {
			callback();
			return;
		}
		
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
					this.SendMessage("CombatUnitMoveOut", swappedUnit);
					this.PlayMoveAnimation(swappedUnitAnimation, groupType, TargetPosition.Out, swappedActor, null);
					this.PlayMoveAnimation(swappingUnitAnimation, groupType, TargetPosition.Out, swappingActor, next);
				}
			);
		}
		
		// Move in new unit
		list.Add(
			(next) => {
				this.SendMessage("CombatUnitMoveIn", swappingUnit);
				this.PlayMoveAnimation(swappingUnitAnimation, groupType, TargetPosition.In, swappingActor, next);
				this.activeCombatUnits[groupType] = swappingUnit;
			}
		);

		Async.Async.Instance.Waterfall(list,
			() => {
				callback();
			}
		);
	}
	
	public void ResetPose(int groupNo) {
		this.GetActiveCombatActor(groupNo).ResetPose();
	}
	
	public void SetPose(int groupNo, PuppetActor.PoseType poseType) {
		this.GetActiveCombatActor(groupNo).SetPoseType(poseType);
	}

	/*
	public void PopActor(MasterUnit masterUnit) {
		int groupType = 1;
		this.poppedActor = this.BuildActor(groupType, masterUnit.lookType, masterUnit.id);	
		
		System.Action callback = () => { 
			this.SendMessage("OnActorPopped");
		};
		
		if (masterUnit.lookType == UnitLookType.Monster) {
			this.PlayMoveAnimation("Combat/Monster/MonsterMove001", groupType, TargetPosition.In, this.poppedActor, callback);
		} else {
			this.PlayMoveAnimation("Combat/Common/Jump", groupType, TargetPosition.In, this.poppedActor, callback);
		}
	}

	public void DepopActor() {
		int groupType = 1;
		System.Action callback = () => { 
			this.poppedActor.Destroy();
			this.poppedActor = null;
			this.SendMessage("OnActorDepopped");
		};
		
		if (this.poppedActor is MonsterActor) {
			this.PlayMoveAnimation("Combat/Monster/MonsterMove001", groupType, TargetPosition.Out, this.poppedActor, callback);
		} else {
			this.PlayMoveAnimation("Combat/Common/Jump", groupType, TargetPosition.Out, this.poppedActor, callback);
		}
		
	}
	*/
	public void ShowBattleWinPose() {
		/*
		Actor actor = this.activeCombatActors[Constant.PlayerGroupType];
		
		if (actor is PuppetActor) {
			PuppetActor puppetActor = actor as PuppetActor;
			this.PlayAnimation(null, puppetActor, "Combat/Common/WinPose", null, (Roga2dAnimation animation) => {
				puppetActor.SetPoseType(PuppetActor.PoseType.Attack);
			});
		}
		*/
	}

	protected void Update() {
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
}