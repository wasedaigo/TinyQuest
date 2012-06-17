using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;

namespace TinyQuest.Model {
	public class CombatModel {
		
		public System.Action TurnProgress;
		public System.Action<SkillModel> SkillUse;
		public System.Action<BattlerModel, int> UpdateHP;

		private BattlerModel.GroupType groupType;
		private List<BattlerModel>[] battlerGroup;
		private BattlerModel activeBattler;
			
		public CombatModel() {
			this.battlerGroup  = new List<BattlerModel>[(int)BattlerModel.GroupType.Count];
			
			for (int i = 0; i < battlerGroup.Length; i++) {
				battlerGroup[i] = new List<BattlerModel>();
			}
		}

		public void Start() {
			this.activeBattler = this.battlerGroup[(int)BattlerModel.GroupType.Player][0];
		}

		public void SetBattler(BattlerModel battlerModel, BattlerModel.GroupType groupType) {
			this.battlerGroup[(int)groupType].Add(battlerModel);
			battlerModel.SkillUse += this.SkillUsed;
			battlerModel.UpdateHP += this.HPUpdated;
		}
		
		public BattlerModel GetBattler(BattlerModel.GroupType groupType, int index) {
			return this.battlerGroup[(int)groupType][index];
		}
		
		public BattlerModel GetActiveBattler() {
			return this.activeBattler;
		}
		
		public void ProgressTurn() {
		}
		
		private void SkillUsed( SkillModel skillModel) {
			this.SkillUse(skillModel);
		}
		
		private void HPUpdated(BattlerModel entity, int value) {
			this.UpdateHP(entity, value);
		}
	}
}
