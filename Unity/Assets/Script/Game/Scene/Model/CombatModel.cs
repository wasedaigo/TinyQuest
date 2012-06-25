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
		public System.Action<UserUnit, int> UpdateHP;
		public System.Action StartBattle;
		public System.Action<CombatAction> ExecuteAction;

		private List<CombatUnit>[] combatUnits;
		private UserUnit targetUnit;
		private int combatUnitCount;
		
		private List<CombatAction> combatActionList;
		private int actionIndex;
		
		public void Start() {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.StartBattle(this.OnStarted);
		}
		
		private void OnStarted(List<CombatUnit>[] combatUnits) {
			this.combatUnits = combatUnits;
			this.StartBattle();
		}
		
		public List<CombatUnit>[] GetCombatUnits() {
			return this.combatUnits;
		}

		public CombatUnit GetCombatUnit(int groupType, int index) {
			return this.combatUnits[groupType][index];
		}

		public MasterSkill GetMasterSkillById(int id) {
			return CacheFactory.Instance.GetMasterDataCache().GetSkillByID(id);
		}
		
		public void ProgressTurn(int slotIndex) {
			if (!IsExecutingAction()) {
				RequestFactory.Instance.GetLocalUserRequest().ProgressTurn(slotIndex, this.TurnProgressed);
			}
		}

		private void TurnProgressed(List<CombatAction> combatActionList) {
			this.actionIndex = 0;
			this.combatActionList = combatActionList;
			this.ExecuteNextAction();
		}
		
		public void ExecuteNextAction() {
			if (this.combatActionList.Count > actionIndex) {
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

		
		private void HPUpdated(UserUnit entity, int value) {
			this.UpdateHP(entity, value);
		}
	}
}
