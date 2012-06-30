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
		
		public virtual void ProgressStep(System.Action callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			userZone.stepIndex += 1;
			userZone.commandIndex = 0;
			
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
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
				switch((ZoneCommand.Type)command.type) {
				case ZoneCommand.Type.Battle:
					/*
					CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
					if (combatProgress == null) {
						CombatUnit playerBattlerData = new CombatUnit(1, 0, 0, 100, new int[]{});
						CombatUnit enemyBattlerData = new CombatUnit(2, 1, 0, 100, new int[]{});
						combatProgress = new CombatProgress(1, new CombatUnit[][]{
							new CombatUnit[]{playerBattlerData},
							new CombatUnit[]{enemyBattlerData}
						});
						CacheFactory.Instance.GetLocalUserDataCache().SetCombatProgress(combatProgress);
					}
					*/
					break;
				}
			}
			
			callback(postCommandState, command, zoneCommandState);
		}
		
		
		public virtual void StartBattle(System.Action<List<CombatUnit>[]> callback) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			CombatProgress combatProgress = data.combatProgress;
			if (combatProgress == null) {
				List<CombatUnit>[] combatUnitGroups = new List<CombatUnit>[2];
				for (int i = 0; i < combatUnitGroups.Length; i++) {
					combatUnitGroups[i] = new List<CombatUnit>();
				}

				for (int i = 0; i < data.ownUnits.Count; i++) {
					UserUnit playerUnit = data.ownUnits[i];
					combatUnitGroups[0].Add(new CombatUnit(playerUnit.id, 0, i)); // Player	
				}
				
				UserUnit enemyUnit = data.zoneUnits[0];
				combatUnitGroups[1].Add(new CombatUnit(enemyUnit.id, 1, 0)); // Enemy
				
				data.combatProgress = new CombatProgress(combatUnitGroups);
			}

			CacheFactory.Instance.GetLocalUserDataCache().Commit();			
			
			callback(data.combatProgress.combatUnitGroups);
		}
		
		public virtual void ProgressCommand(System.Action<ZoneModel.PostCommandState, ZoneCommand, object> callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			userZone.commandIndex += 1;

			CacheFactory.Instance.GetLocalUserDataCache().Commit();			
			
			this.GetExecutingCommand(callback);
		}
		
		private struct CombatProcessBlock {
			public UserUnit caster;
			public UserUnit target;
			
			public CombatProcessBlock(UserUnit caster, UserUnit target) {
				this.caster = caster;
				this.target = target;
			}
			
			public CombatAction Execute() {
				if (this.caster.IsDead || this.target.IsDead) { return null; }
				MasterSkill skill = CacheFactory.Instance.GetMasterDataCache().GetSkillByID(this.caster.Unit.normalAttack);
				
				int effect = this.caster.Power * skill.multiplier / 100;
				this.target.hp -= effect;
				if (this.target.hp < 0) {
					this.target.hp = 0;	
				}
				
				return new CombatAction(this.caster, this.target, skill, effect);
			}
		}
	
		public virtual void ProgressTurn(int playerIndex, System.Action<CombatUnit, CombatUnit, List<CombatAction>> callback) {
			int enemyIndex = 0;
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
	
			CombatUnit playerUnit = combatProgress.combatUnitGroups[0][playerIndex];
			CombatUnit enemyUnit = combatProgress.combatUnitGroups[1][enemyIndex];
			
			// Add actions
			List<CombatAction> combatActions = new List<CombatAction>();
			if (combatProgress.activeUnitIndexes == null) {
				// The first turn, there is no action
				combatProgress.activeUnitIndexes = new int[Constant.GroupTypeCount]{playerIndex, enemyIndex};
			} else {
				combatProgress.activeUnitIndexes[0] = playerIndex;
				combatProgress.activeUnitIndexes[1] = enemyIndex;

				
				CombatProcessBlock[] blocks = new CombatProcessBlock[Constant.GroupTypeCount];
				if (playerUnit.GetUserUnit().Speed >= enemyUnit.GetUserUnit().Speed) {
					blocks[0] = new CombatProcessBlock(playerUnit.GetUserUnit(), enemyUnit.GetUserUnit());
					blocks[1] = new CombatProcessBlock(enemyUnit.GetUserUnit(), playerUnit.GetUserUnit());
				} else {
					blocks[0] = new CombatProcessBlock(enemyUnit.GetUserUnit(), playerUnit.GetUserUnit());
					blocks[1] = new CombatProcessBlock(playerUnit.GetUserUnit(), enemyUnit.GetUserUnit());
				}

				for (int i = 0; i < blocks.Length; i++) {
					CombatAction action = blocks[i].Execute();
					if (action != null) {
						combatActions.Add(action);
					}
				}
			}

			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback(playerUnit, enemyUnit, combatActions);
		}
	}
}
