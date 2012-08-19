using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;
using TinyQuest.Data.Skills;

namespace TinyQuest.Scene.Model {

	public class CombatModel {
		
		
		public enum CombatResult{
			OnGoing,
			Win,
			Lose
		}
		
		public System.Action TurnProgress;
		public System.Action<UserUnit, int> UpdateHP;
		public System.Action BattleStarted;
		public System.Action FinishBattle;
		public System.Action UpdateStatus;
		public System.Action<CombatUnit, CombatUnit> CombatReady;
		
		public System.Action<CombatAction> ExecuteAction;

		private UserUnit targetUnit;
		private int combatUnitCount;
		
		private List<CombatAction> combatActionList;
		private int actionIndex;
		private int standByUnitIndex;
		private bool forceSwap;
		
		public CombatModel(){
		}
		
		public LocalUserData GetData() {
			return  CacheFactory.Instance.GetLocalUserDataCache().Data;
		}
		
		public void ForceSwap() {
			this.forceSwap = true;
		}
		
		public int GetCurrentTurn() {
			return GetData().turnCount;
		}
		
		public CombatUnit GetCombatUnit(int groupNo, int index) {
			if (index < 0) {
				return null;
			}
			CombatUnitGroup combatUnitGroup = GetData().combatUnitGroups[groupNo];
			return combatUnitGroup.combatUnits[index];
		}
		
		public int GetStandByUnitIndex(int groupNo) {
			CombatUnitGroup combatUnitGroup = GetData().combatUnitGroups[groupNo];
			return combatUnitGroup.standByUnitIndex;
		}
		
		public CombatUnit GetFightingUnit(int groupNo) {
			CombatUnitGroup combatUnitGroup = GetData().combatUnitGroups[groupNo];
			return GetCombatUnit(groupNo, combatUnitGroup.fightingUnitIndex);
		}
		

		public CombatUnitGroup[] GetCombatUnits() {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			return data.combatUnitGroups;
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

		public void StartBattle(MonoBehaviour monoBehavior) {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.StartBattle(monoBehavior, this.BattleStarted);
		}

		public void SendTurnInput(MonoBehaviour monoBehaviour, int standByUnitIndex) {
			RequestFactory.Instance.GetLocalUserRequest().SendTurnInput(monoBehaviour, standByUnitIndex, this.forceSwap, this.HandleSendTurnInputResponse);
			this.forceSwap = false;
		}
		
		public void HandleSendTurnInputResponse(LocalUserDataRequest.ActionResult actionResult) {
			this.actionIndex = 0;
			this.combatActionList = actionResult.combatActions;
			this.CombatReady(actionResult.activePlayerUnit, actionResult.activeOpponentUnit);
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

		public bool FinishTurn() {
			bool battleFinished = false;
			CombatModel.CombatResult combatResult = this.GetCombatResult();
			switch(combatResult) {
				case CombatModel.CombatResult.Lose:
					RequestFactory.Instance.GetLocalUserRequest().FinishCombat(this.FinishBattle);
					battleFinished = true;
				break;
				case CombatModel.CombatResult.Win:
					RequestFactory.Instance.GetLocalUserRequest().FinishCombat(this.FinishBattle);
					battleFinished = true;
				break;
				default:
				break;
			}
			
			return battleFinished;
		}
		
		private void HPUpdated(UserUnit entity, int value) {
			this.UpdateHP(entity, value);
		}
	}
}
