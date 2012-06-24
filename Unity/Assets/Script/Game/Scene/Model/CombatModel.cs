using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;

namespace TinyQuest.Scene.Model {
	public class CombatModel {
		public const int GroupCount = 2;
		public System.Action TurnProgress;
		public System.Action<MasterSkill, CombatUnit, CombatUnit> SkillUse;
		public System.Action<UserUnit, int> UpdateHP;
		public System.Action StartBattle;

		private int groupType;
		private List<CombatUnit> combatUnits;
		private UserUnit targetUnit;
		private int combatUnitCount;
		
		public void Start() {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.StartBattle(this.OnStarted);
		}
		
		private void OnStarted(List<CombatUnit> combatUnits) {
			int count = 0;
			foreach (CombatUnit combatUnit in combatUnits) {
				if (combatUnit.groupType == groupType) {
					count++;
				}
			}
			this.combatUnitCount = count;
			this.combatUnits = combatUnits;
			this.StartBattle();
		}
		
		public List<CombatUnit> GetCombatUnits() {
			return this.combatUnits;
		}

		public int GetCombatUnitCount(int groupType) {
			return this.combatUnitCount;
		}

		public CombatUnit GetCombatUnit(int groupType, int index) {
			CombatUnit result = null;
			foreach (CombatUnit combatUnit in this.combatUnits) {
				if (combatUnit.groupType == groupType && combatUnit.index == index) {
					result = combatUnit;
					break;
				}
			}
			return result;
		}

		public MasterSkill GetMasterSkillById(int id) {
			return CacheFactory.Instance.GetMasterDataCache().GetSkillByID(id);
		}
		
		public void ProgressTurn() {
		}
		
		public void UseSkill(int index) {
			CombatUnit casterUnit = this.GetCombatUnit(0, index);
			CombatUnit targetUnit = this.GetCombatUnit(1, 0);
			
			RequestFactory.Instance.GetLocalUserRequest().UseSkill(casterUnit, targetUnit, this.SkillUsed);
		}

		private void SkillUsed(MasterSkill masterSkill, CombatUnit casterUnit, CombatUnit targetUnit) {
			this.SkillUse(masterSkill, casterUnit, targetUnit);
		}
		
		private void HPUpdated(UserUnit entity, int value) {
			this.UpdateHP(entity, value);
		}
	}
}
