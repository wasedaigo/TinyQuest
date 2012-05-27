using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Request;
using TinyQuest.Factory.Entity;

namespace TinyQuest.Entity {
	public class BattlerEntity {
		
		public enum GroupType {
			Player = 0,
			Enemy = 1,
			Count
		};

		public const int MaxHands = 3;
		public const int WeaponSlotNum = 6;
		public const int MaxAP = 6;
		public const int HealAP = 3;
		
		public System.Action<SkillEntity[]> SkillDraw;
		public System.Action<SkillEntity> SkillUse;
		
		private int no;
		public int No {
			get {return no;}
		}
		
		private GroupType group;
		public GroupType Group {
			get {return group;}
		}
		
		private int maxHP;
		public int MaxHP {
			get {return this.maxHP;}
		}

		private int hp;
		public int HP {
			get {return this.hp;}
		}

		private Dictionary<int, SkillEntity> skillIndexMap = new Dictionary<int, SkillEntity>();		
		private WeaponEntity[] weapons = new WeaponEntity[WeaponSlotNum];

		public BattlerEntity(int hp, int maxHP, int no, GroupType group) {
			this.maxHP = maxHP;
			this.hp = maxHP;
			this.no = no;
			this.group = group;
		}

		public WeaponEntity GetWeapon(int slotIndex) {
			return this.weapons[slotIndex];
		}
		
		public void SetWeapons(WeaponEntity[] weapons) {
			if (this.skillIndexMap.Count > 0) { 
				Debug.LogError("Weapons are already set");
				return;
			}

			this.weapons = weapons;
			int index = 0;
			for (int i = 0; i < weapons.Length; i++) {
				WeaponEntity weapon = weapons[i];
				MasterSkill[] skills = weapon.GetSkills();
				for (int j = 0; j < skills.Length; j++) {
					MasterSkill skill = skills[j];
					if (skill != null) {
						this.skillIndexMap.Add(index, SkillFactory.Instance.Build(skill.id, weapon, MaxHands/ weapon.GetSkillCount()));
						index++;
					}
				}
			}
		}

		public void UseSkill(int slotIndex) {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.UseSkill(slotIndex, this.group, this.no, this.SkillUsed);
		}
		
		private void SkillUsed(int skillIndex) {
			if (this.SkillUse != null) {
				SkillEntity skillEntity = this.skillIndexMap[skillIndex];
				this.SkillUse(skillEntity);
			}
		}

		public void DrawSkills() {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			
			List<int> allSkillIndexList = new List<int>();
			for (int i = 0; i < skillIndexMap.Count; i++) {
				allSkillIndexList.Add(i);
			}
			
			req.DrawSkills(this.group, this.no, allSkillIndexList, this.SkillsDrawn);
		}
		
		public void SkillsDrawn(int[] drawnSkillIndexes) {
			SkillEntity[] skills = new SkillEntity[MaxHands];
			for (int i = 0; i < drawnSkillIndexes.Length; i++) {
				int skillIndex = drawnSkillIndexes[i];
				if (skillIndex >= 0) {
					skills[i] = this.skillIndexMap[skillIndex];
				}
			}

			if (this.SkillDraw != null) {
				this.SkillDraw(skills);
			}
		}
	}
}
