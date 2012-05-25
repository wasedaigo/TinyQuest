using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Entity {
	public class CombatEntity {
		
		public System.Action TurnProgress;
		public System.Action<SkillEntity[]> SkillDraw;
		public System.Action<WeaponEntity, SkillEntity> SkillUse;
		
		private int turnNo;
		private BattlerEntity.GroupType groupType;
		private List<BattlerEntity>[] battlerGroup;
		private BattlerEntity activeBattler;
			
		public CombatEntity() {
			this.turnNo = 0;
			this.battlerGroup  = new List<BattlerEntity>[(int)BattlerEntity.GroupType.Count];
			
			for (int i = 0; i < battlerGroup.Length; i++) {
				battlerGroup[i] = new List<BattlerEntity>();
			}
		}
		
		public void SetBattler(BattlerEntity battlerEntity, BattlerEntity.GroupType groupType) {
			this.battlerGroup[(int)groupType].Add(battlerEntity);
			battlerEntity.SkillUse += this.SkillUsed;
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
			
			this.activeBattler.SkillDraw += this.SkillDrawn;
			this.activeBattler.DrawSkills();

			if (this.TurnProgress != null) {
				this.TurnProgress();	
			}
		}
		
		private void SkillDrawn(SkillEntity[] skillEntities) {
			this.SkillDraw(skillEntities);
		}
		
		private void SkillUsed(WeaponEntity weaponEntity, SkillEntity skillEntity) {
			if (this.SkillUse != null) {
				this.SkillUse(weaponEntity, skillEntity);
			}
		}
	}
}
