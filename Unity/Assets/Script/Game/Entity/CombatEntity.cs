using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;

namespace TinyQuest.Entity {
	public class CombatEntity {
		
		public System.Action TurnProgress;
		public System.Action<SkillEntity[], CompositeData> SkillDraw;
		public System.Action<SkillEntity> SkillUse;
		public System.Action<BattlerEntity, int> UpdateHP;

		private BattlerEntity.GroupType groupType;
		private List<BattlerEntity>[] battlerGroup;
		private BattlerEntity activeBattler;
			
		public CombatEntity() {
			this.battlerGroup  = new List<BattlerEntity>[(int)BattlerEntity.GroupType.Count];
			
			for (int i = 0; i < battlerGroup.Length; i++) {
				battlerGroup[i] = new List<BattlerEntity>();
			}
		}

		public void Start() {
			this.activeBattler = this.battlerGroup[(int)BattlerEntity.GroupType.Player][0];
			this.activeBattler.DrawSkills(true);
		}

		public void SetBattler(BattlerEntity battlerEntity, BattlerEntity.GroupType groupType) {
			this.battlerGroup[(int)groupType].Add(battlerEntity);
			battlerEntity.SkillUse += this.SkillUsed;
			battlerEntity.SkillDraw += this.SkillDrawn;
			battlerEntity.UpdateHP += this.HPUpdated;
		}
		
		public BattlerEntity GetBattler(BattlerEntity.GroupType groupType, int index) {
			return this.battlerGroup[(int)groupType][index];
		}
		
		public BattlerEntity GetActiveBattler() {
			return this.activeBattler;
		}
		
		public void ProgressTurn() {
			// Switch player & monster each turn
			if (this.activeBattler == null || this.activeBattler.Group == BattlerEntity.GroupType.Enemy) {
				this.activeBattler = this.battlerGroup[(int)BattlerEntity.GroupType.Player][0];	
			} else {
				this.activeBattler = this.battlerGroup[(int)BattlerEntity.GroupType.Player][0];
			}
			
			this.activeBattler.DrawSkills(false);
		}
		
		private void SkillDrawn(SkillEntity[] skillEntities, CompositeData compositeData) {
			this.SkillDraw(skillEntities, compositeData);
		}
		
		private void SkillUsed( SkillEntity skillEntity) {
			this.SkillUse(skillEntity);
		}
		
		private void HPUpdated(BattlerEntity entity, int value) {
			this.UpdateHP(entity, value);
		}
	}
}
