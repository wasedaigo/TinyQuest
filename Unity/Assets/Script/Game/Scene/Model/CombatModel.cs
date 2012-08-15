using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;
using TinyQuest.Data.Skills;

namespace TinyQuest.Scene.Model {

	public class CombatModel {
		private struct CombatProcessBlock {
			public CombatUnit caster;
			public CombatUnit target;
			
			public CombatProcessBlock(CombatUnit caster, CombatUnit target) {
				this.caster = caster;
				this.target = target;
			}
			
			private BaseSkill ChooseSkill(int rand){
				BaseSkill skill = null;
				for (int i = 0; i < this.caster.GetUserUnit().Unit.skills.Length; i++) {
					skill = SkillFactory.Instance.GetSkill(this.caster.GetUserUnit().Unit.skills[i]);
					if (rand <= skill.GetChance()) {
						break;
					}
					rand -= skill.GetChance();
					skill = null;
				}
				
				return skill;
			}
			
			public CombatAction Execute(int rand) {
				if (this.caster.IsDead || this.target.IsDead) { return null; }

				BaseSkill skill = this.ChooseSkill(rand);
				if (skill == null) {
					skill = SkillFactory.Instance.GetNormalAttack(this.caster.GetUserUnit().Unit.normalAttack);
				}
				
				BaseSkill.SkillResult result = skill.Calculate(this.caster);

				int effect = result.damage;
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

				return new CombatAction(this.caster, this.target, result, null, targetResult);
			}
		}
		
		
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
		
		public System.Action<CombatAction> ExecuteAction;

		private CombatUnitGroup[] combatUnitGroups;
		private UserUnit targetUnit;
		private int combatUnitCount;
		
		private List<CombatAction> combatActionList;
		private int actionIndex;
		private int turn;
		
		public CombatModel(){
			this.turn = 1;
		}
		
		public bool IsPlayerTurn() {
			// currentTurnGroupNo will be updated when a request is sent
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			return data.currentTurnGroupNo == 1;
		}
		
		public CombatUnit GetFirstAliveUnit(int groupNo) {
			return RequestFactory.Instance.GetLocalUserRequest().GetFirstAliveUnit(groupNo);
		}
		
		public CombatUnit GetFightingUnit(int groupNo) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			CombatUnitGroup combatUnitGroup = data.combatUnitGroups[groupNo];
			int fightingUnitIndex = combatUnitGroup.fightingUnitIndex;
			if (fightingUnitIndex < 0) {
				return null;
			}
			CombatUnit unit = combatUnitGroup.combatUnits[fightingUnitIndex];
			
			return unit;
		}
		
		private void SetFightingUnitIndex(int groupNo, int index) {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;
			data.combatUnitGroups[0].fightingUnitIndex = index;
		}
		
		public CombatUnit GetPlayerFightingUnit() {
			return this.GetFightingUnit(0);
		}
		
		public void SetPlayerFightingUnitIndex(int index) {
			this.SetFightingUnitIndex(0, index);
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
		
		public void ProcessActions() {
			LocalUserData data = CacheFactory.Instance.GetLocalUserDataCache().Data;

			CombatUnitGroup playerCombatUnitGroup = data.combatUnitGroups[0];
			CombatUnitGroup enemyCombatUnitGroup = data.combatUnitGroups[1];
			CombatUnit playerUnit = playerCombatUnitGroup.combatUnits[playerCombatUnitGroup.fightingUnitIndex];
			CombatUnit enemyUnit = enemyCombatUnitGroup.combatUnits[enemyCombatUnitGroup.fightingUnitIndex];
			
			// Add actions
			List<CombatAction> combatActions = new List<CombatAction>();
			
			List<CombatProcessBlock> blocks = new List<CombatProcessBlock>();
			if (data.currentTurnGroupNo == 0) {
				blocks.Add(new CombatProcessBlock(playerUnit, enemyUnit));
			} else {
				blocks.Add(new CombatProcessBlock(enemyUnit, playerUnit));
			}

			for (int i = 0; i < blocks.Count; i++) {
				CombatAction action = blocks[i].Execute(data.skillRands[i]);
				if (action != null) {
					combatActions.Add(action);
				}
			}
			
			data.turnCount += 1;
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			
			turn++;
			this.actionIndex = 0;
			this.combatActionList = combatActions;	
		}

		public void StartBattle(MonoBehaviour monoBehavior) {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.StartBattle(monoBehavior, this.BattleStarted);
		}

		public void SendTurnInput(MonoBehaviour monoBehaviour, System.Action callback) {
			RequestFactory.Instance.GetLocalUserRequest().SendTurnInput(monoBehaviour, this.GetPlayerFightingUnit().index, this.turn, () => {
				callback();
			});
		}

		public void ReceiveTurnInput(MonoBehaviour monoBehaviour, System.Action callback) {
			RequestFactory.Instance.GetLocalUserRequest().ReceiveTurnInput(monoBehaviour, this.turn, () => {
				callback();
			});
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
