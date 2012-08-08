using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;

namespace TinyQuest.Scene.Model {

	public class CombatModel {
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
					targetResult.swapUnit = RequestFactory.Instance.GetLocalUserRequest().GetFirstAliveUnit(this.target.groupType);
				}
				
				LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
				data.combatProgress = new CombatProgress();
				
				return new CombatAction(this.caster, this.target, skill, null, targetResult);
			}
		}

		public enum CombatResult{
			OnGoing,
			Win,
			Lose
		}
		
		public const int GroupCount = 2;
		public System.Action TurnProgress;
		public System.Action<UserUnit, int> UpdateHP;
		public System.Action FinishBattle;
		public System.Action SelectStandbyUnit;
		public System.Action UpdateStatus;
		public System.Action StartTurn;
		
		public System.Action<CombatAction> ExecuteAction;

		private CombatUnitGroup[] combatUnitGroups;
		private UserUnit targetUnit;
		private int combatUnitCount;
		
		private List<CombatAction> combatActionList;
		private int actionIndex;
		private bool turnFinished;
		private int standbyUnitIndex;
		private int turn;
		
		public CombatModel(){
			this.turnFinished = true;
			this.turn = 1;
		}
		
		public CombatUnit GetFightingUnit(int groupNo) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			int fightingUnitIndex = data.fightingUnitIndexes[groupNo];
			CombatUnitGroup combatUnitGroup = data.combatUnitGroups[groupNo];
			CombatUnit unit = combatUnitGroup.combatUnits[fightingUnitIndex];
			
			return unit;
		}
		
		public CombatUnit GetStandbyUnit() {
			CombatUnitGroup combatUnitGroup = this.GetCombatUnits()[CombatGroupInfo.Instance.GetPlayerGroupType(0)];
			
			if (this.standbyUnitIndex < 0) {
				return null;
			} else {
				return combatUnitGroup.combatUnits[this.standbyUnitIndex];
			}
		}
		
		public void SetStandbyUnitByIndex(int index) {
			CombatUnitGroup combatUnitGroup = this.GetCombatUnits()[CombatGroupInfo.Instance.GetPlayerGroupType(0)];
			CombatUnit unit = combatUnitGroup.combatUnits[index];
			
			this.standbyUnitIndex = index;
			this.SelectStandbyUnit();
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
		
		private bool IsPlayerFirst(CombatUnit playerUnit, CombatUnit enemyUnit) {
			if (playerUnit.GetUserUnit().Speed > enemyUnit.GetUserUnit().Speed) {
				return true;
			}
			
			if (playerUnit.GetUserUnit().Speed == enemyUnit.GetUserUnit().Speed) {
				return playerUnit.groupType > enemyUnit.groupType;
			}

			return false;
		}
		
		public virtual void ProcessActions() {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			
			int playerGroupNo = CombatGroupInfo.Instance.GetPlayerGroupType(0);
			int opponentGroupNo = CombatGroupInfo.Instance.GetPlayerGroupType(1);
			CombatUnit playerUnit = data.combatUnitGroups[CombatGroupInfo.Instance.GetPlayerGroupType(playerGroupNo)].combatUnits[data.fightingUnitIndexes[playerGroupNo]];
			CombatUnit enemyUnit = data.combatUnitGroups[CombatGroupInfo.Instance.GetPlayerGroupType(opponentGroupNo)].combatUnits[data.fightingUnitIndexes[opponentGroupNo]];

			// Add actions
			List<CombatAction> combatActions = new List<CombatAction>();
			
			CombatProcessBlock[] blocks = new CombatProcessBlock[CombatGroupInfo.Instance.GetGroupCount()];
			if (this.IsPlayerFirst(playerUnit, enemyUnit)) {
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
			
			if (playerUnit.index == standbyUnitIndex && playerUnit.IsDead) {
				CombatUnit nextUnit = RequestFactory.Instance.GetLocalUserRequest().GetFirstAliveUnit(playerGroupNo);
				if (nextUnit == null) {
					standbyUnitIndex = -1;
				} else {
					standbyUnitIndex = nextUnit.index;
				}
			}
			
			data.combatProgress.turnCount += 1;
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			
			turn++;
			TurnProgressed(playerUnit, enemyUnit, combatActions);			
		}
		
		public void StartBattle(MonoBehaviour monoBehavior) {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.StartBattle(monoBehavior,
				() => {
					this.UpdateStatus();
					CombatUnit fightingUnit = this.GetFightingUnit(CombatGroupInfo.Instance.GetPlayerGroupType(0));
					this.SetStandbyUnitByIndex(fightingUnit.index);
				
					this.ProcessActions();
				}
			);
		}

		public void LoadNextTurn(MonoBehaviour monoBehaviour) {
			RequestFactory.Instance.GetLocalUserRequest().ProgressTurn(monoBehaviour, this.standbyUnitIndex, this.turn);
		}
		
		public void NextTurn(MonoBehaviour monoBehaviour) {
			monoBehaviour.StartCoroutine(this.ProcessNextTurn(monoBehaviour, this.ProcessActions));
		}
		
		private IEnumerator ProcessNextTurn(MonoBehaviour monoBehaviour, System.Action callback) {
			while(RequestFactory.Instance.GetLocalUserRequest().IsRequesting) {
				yield return new WaitForSeconds(0.1f);
			}
			
			callback();
		}

		private void TurnProgressed(CombatUnit caster, CombatUnit target, List<CombatAction> combatActionList) {
			this.actionIndex = 0;
			this.combatActionList = combatActionList;
			this.StartTurn();
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
