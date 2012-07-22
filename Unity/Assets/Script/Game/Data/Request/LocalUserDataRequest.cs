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
		public virtual void StartBattle(System.Action<CombatUnitGroup[]> callback) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;

			data.combatUnitGroups = new CombatUnitGroup[Constant.GroupTypeCount];
			for (int i = 0; i < Constant.GroupTypeCount; i++) {
				data.combatUnitGroups[i] = new CombatUnitGroup();
			}

			for (int i = 0; i < data.ownUnits.Count; i++) {
				UserUnit playerUnit = data.ownUnits[i];
				data.combatUnitGroups[0].combatUnits.Add(new CombatUnit(playerUnit, 0, i)); // Player	
				data.combatUnitGroups[1].combatUnits.Add(new CombatUnit(playerUnit, 1, i)); // Enemy	
			}


			CacheFactory.Instance.GetLocalUserDataCache().Commit();			
			
			callback(data.combatUnitGroups);
		}

		public static CombatUnit GetFirstAliveUnit(int groupType) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			List<CombatUnit> combatUnitList = data.combatUnitGroups[groupType].combatUnits;
			foreach (CombatUnit combatUnit in combatUnitList) {
				if (combatUnit.hp > 0) {
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
				if (this.caster.IsDead || this.target.IsDead) { return null; }
				MasterSkill skill = CacheFactory.Instance.GetMasterDataCache().GetSkillByID(this.caster.GetUserUnit().Unit.normalAttack);
				
				int effect = this.caster.GetUserUnit().Power * skill.multiplier / 100;
				this.target.hp -= effect;
				if (this.target.hp < 0) {
					this.target.hp = 0;	
				}
				
				CombatActionResult targetResult = new CombatActionResult();
				targetResult.life = this.target.hp;
				targetResult.maxLife = this.target.GetUserUnit().MaxHP;
				targetResult.effect = effect;
				targetResult.combatUnit = this.target;
				
				if (targetResult.life == 0) {
					targetResult.swapUnit = LocalUserDataRequest.GetFirstAliveUnit(this.target.groupType);
				}
				
				LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
				data.combatProgress = new CombatProgress();
				
				return new CombatAction(this.caster, this.target, skill, null, targetResult);
			}
		}

		public virtual void ProgressTurn(int playerIndex, System.Action<CombatUnit, CombatUnit, List<CombatAction>> callback) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;

			CombatUnit playerUnit = data.combatUnitGroups[Constant.PlayerGroupType].combatUnits[playerIndex];
			CombatUnit enemyUnit = GetFirstAliveUnit(1);
			int enemyIndex = enemyUnit.index;

			// Add actions
			List<CombatAction> combatActions = new List<CombatAction>();
			data.combatUnitGroups[Constant.PlayerGroupType].activeIndex = playerIndex;
			data.combatUnitGroups[Constant.EnemyGroupType].activeIndex = enemyIndex;
			
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
		
		public virtual void FinishCombat(System.Action callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			userZone.commandIndex += 1;
			
			callback();
		}
	}
}
