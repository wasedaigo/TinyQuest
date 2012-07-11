using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using TinyQuest.Scene.Model;
using TinyQuest.Data.Cache;

namespace TinyQuest.Data.Request {
	public class LocalUserDataRequest
	{
		const int MaxHandCount = 3;
		public virtual void Get(System.Action<string> callback) {
		}
		
		public virtual void ProgressStep(System.Action<ZoneModel.PostCommandState, ZoneCommand, object> callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			userZone.stepIndex += 1;
			userZone.commandIndex = 0;
			
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			this.GetExecutingCommand(callback);
		}
		
		public virtual void GetExecutingCommand(System.Action<ZoneModel.PostCommandState, ZoneCommand, object> callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			int stepIndex = userZone.stepIndex;
			int commandIndex = userZone.commandIndex;
			
			ZoneModel.PostCommandState postCommandState = ZoneModel.PostCommandState.None;
			ZoneCommand command = null;
			object zoneCommandState = null;
			
			bool allCommandFinished = true;
			string key = stepIndex.ToString();
			if (userZone.events.ContainsKey(key)) {
				ZoneEvent zoneEvent = userZone.events[stepIndex.ToString()];
				ZoneCommand[] commands = zoneEvent.commands;
				if (commandIndex < commands.Length) {
					command = commands[commandIndex];
					zoneCommandState = userZone.commandState;
					allCommandFinished = false;
				}
			}
			
			if (allCommandFinished) {
				if (userZone.stepIndex >= userZone.lastStepIndex) {
					postCommandState = ZoneModel.PostCommandState.ClearZone;
				} else {
					postCommandState = ZoneModel.PostCommandState.NextStep;
				}
			} else {
				postCommandState = ZoneModel.PostCommandState.None;
			}
			
			callback(postCommandState, command, zoneCommandState);
		}
		
		public virtual void LoadZone(System.Action<CombatUnitGroup[]> callback) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;

			data.combatUnitGroups = new CombatUnitGroup[Constant.GroupTypeCount];
			for (int i = 0; i < Constant.GroupTypeCount; i++) {
				data.combatUnitGroups[i] = new CombatUnitGroup();
			}

			for (int i = 0; i < data.ownUnits.Count; i++) {
				UserUnit playerUnit = data.ownUnits[i];
				data.combatUnitGroups[0].combatUnits.Add(new CombatUnit(playerUnit.id, 0, i)); // Player	
			}
			
			for (int i = 0; i < data.zoneEnemies.Count; i++) {
				UserUnit enemyUnit = data.zoneEnemies[i];
				data.combatUnitGroups[1].combatUnits.Add(new CombatUnit(enemyUnit.id, 1, i)); // Enemy	
			}
			
			data.combatProgress = new CombatProgress();
			CacheFactory.Instance.GetLocalUserDataCache().Commit();			
			
			callback(data.combatUnitGroups);
		}

		public virtual void ProgressCommand(System.Action<ZoneModel.PostCommandState, ZoneCommand, object> callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			userZone.commandIndex += 1;

			CacheFactory.Instance.GetLocalUserDataCache().Commit();			
			
			this.GetExecutingCommand(callback);
		}

		public static CombatUnit GetFirstAliveUnit(int groupType) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			List<CombatUnit> combatUnitList = data.combatUnitGroups[groupType].combatUnits;
			foreach (CombatUnit combatUnit in combatUnitList) {
				if (combatUnit.GetUserUnit().hp > 0) {
					return combatUnit;
				}
			}
			
			return null;
		}
		
		private struct CombatProcessBlock {
			public CombatUnit caster;
			public CombatUnit target;
			
			public CombatProcessBlock(CombatUnit caster, CombatUnit target) {
				this.caster = caster;
				this.target = target;
			}
			
			public CombatAction Execute() {
				if (this.caster.GetUserUnit().IsDead || this.target.GetUserUnit().IsDead) { return null; }
				MasterSkill skill = CacheFactory.Instance.GetMasterDataCache().GetSkillByID(this.caster.GetUserUnit().Unit.normalAttack);
				
				int effect = this.caster.GetUserUnit().Power * skill.multiplier / 100;
				this.target.GetUserUnit().hp -= effect;
				if (this.target.GetUserUnit().hp < 0) {
					this.target.GetUserUnit().hp = 0;	
				}
				
				CombatActionResult targetResult = new CombatActionResult();
				targetResult.life = this.target.GetUserUnit().hp;
				targetResult.maxLife = this.target.GetUserUnit().MaxHP;
				targetResult.effect = effect;
				targetResult.combatUnit = this.target;
				
				if (targetResult.life == 0) {
					targetResult.swapUnit = LocalUserDataRequest.GetFirstAliveUnit(this.target.groupType);
				}
				
				return new CombatAction(this.caster, this.target, skill, null, targetResult);
			}
		}

		public virtual void ProgressTurn(int playerIndex, System.Action<CombatUnit, CombatUnit, List<CombatAction>> callback) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;

			CombatUnit playerUnit = data.combatUnitGroups[0].combatUnits[playerIndex];
			CombatUnit enemyUnit = GetFirstAliveUnit(1);
			int enemyIndex = enemyUnit.index;

			// Add actions
			List<CombatAction> combatActions = new List<CombatAction>();
			data.combatUnitGroups[0].activeIndex = playerIndex;
			data.combatUnitGroups[1].activeIndex = enemyIndex;
			
			CombatProcessBlock[] blocks = new CombatProcessBlock[Constant.GroupTypeCount];
			if (playerUnit.GetUserUnit().Speed >= enemyUnit.GetUserUnit().Speed) {
				blocks[0] = new CombatProcessBlock(playerUnit, enemyUnit);
				blocks[1] = new CombatProcessBlock(enemyUnit, playerUnit);
			} else {
				blocks[0] = new CombatProcessBlock(enemyUnit, playerUnit);
				blocks[1] = new CombatProcessBlock(playerUnit, enemyUnit);
			}

			for (int i = 0; i < blocks.Length; i++) {
				CombatAction action = blocks[i].Execute();
				if (action != null) {
					combatActions.Add(action);
				}
			}
			
			data.combatProgress.turnCount += 1;
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback(playerUnit, enemyUnit, combatActions);
		}
	}
}
