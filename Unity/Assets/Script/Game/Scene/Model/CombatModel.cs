using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;

namespace TinyQuest.Scene.Model {
	public class CombatModel {
		public enum CombatResult{
			OnGoing,
			Win,
			Lose
		}
		
		public const int GroupCount = 2;
		public System.Action TurnProgress;
		public System.Action<UserUnit, int> UpdateHP;
		public System.Action StartBattle;
		public System.Action FinishBattle;
		public System.Action<int> SelectStandByUnit;
		
		public System.Action<CombatAction> ExecuteAction;
		public System.Action<CombatUnit, CombatUnit> SelectUnit;

		private CombatUnitGroup[] combatUnitGroups;
		private UserUnit targetUnit;
		private int combatUnitCount;
		
		private List<CombatAction> combatActionList;
		private int actionIndex;
		private bool turnFinished;
		private int standByUnit;
		
		public CombatModel(){
			this.turnFinished = true;
		}
		
		public int GetStandByUnit() {
			return standByUnit;
		}
		
		public void SetStandByUnitBySlot(int slotNo) {
			CombatUnitGroup combatUnitGroup = this.GetCombatUnits()[CombatGroupInfo.Instance.GetPlayerGroupType(0)];
			CombatUnit unit = combatUnitGroup.combatUnits[slotNo];
			
			this.SetStandByUnit(unit.userUnit.unit);
		}
		
		private void SetStandByUnit(int unitId) {
			this.standByUnit = unitId;
			this.SelectStandByUnit(unitId);
		}
		
		public CombatUnitGroup[] GetCombatUnits() {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			return data.combatUnitGroups;
		}

		public CombatUnit GetCombatUnit(int groupType, int index) {
			CombatUnitGroup[] combatUnitGroups = this.GetCombatUnits();
			return combatUnitGroups[groupType].combatUnits[index];
		}


		public MasterSkill GetMasterSkillById(int id) {
			return CacheFactory.Instance.GetMasterDataCache().GetSkillByID(id);
		}
		
		public CombatResult GetCombatResult() {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			if (data.combatUnitGroups[0].IsAllDead()){
				return CombatResult.Lose;	
			}
			if (data.combatUnitGroups[1].IsAllDead()){
				return CombatResult.Win;	
			}
			return CombatResult.OnGoing;
		}

		public void ProgressTurn(MonoBehaviour monoBehaviour, int slotIndex) {
			if (this.turnFinished) {
				this.turnFinished = false;
				RequestFactory.Instance.GetLocalUserRequest().ProgressTurn(monoBehaviour, slotIndex, this.TurnProgressed);
			}
		}

		private void TurnProgressed(CombatUnit caster, CombatUnit target, List<CombatAction> combatActionList) {
			this.actionIndex = 0;
			this.combatActionList = combatActionList;
			this.SelectUnit(caster, target);
		}
		
		public void ExecuteNextAction() {
			if (this.combatActionList != null && this.combatActionList.Count > actionIndex) {
				this.ExecuteAction(this.combatActionList[actionIndex]);
				this.actionIndex++;
			} else {
				// Terminate action	
				this.actionIndex = 0;
				this.combatActionList = null;
			}
		}

		public bool IsExecutingAction(){
			return this.actionIndex > 0;
		}

		public void FinishTurn() {
			CombatModel.CombatResult combatResult = this.GetCombatResult();
			switch(combatResult) {
				case CombatModel.CombatResult.Lose:
					RequestFactory.Instance.GetLocalUserRequest().FinishCombat(this.FinishBattle);
				break;
				case CombatModel.CombatResult.Win:
					RequestFactory.Instance.GetLocalUserRequest().FinishCombat(this.FinishBattle);
				break;
				default:
					this.turnFinished = true;
				break;
			}
		}
		
		private void HPUpdated(UserUnit entity, int value) {
			this.UpdateHP(entity, value);
		}
	}
}
