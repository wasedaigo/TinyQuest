using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
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
		
		public System.Action<SkillEntity[], CompositeData> SkillDraw;
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
			/*
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
						this.skillIndexMap.Add(index, SkillFactory.Instance.Build(skill.id, MaxHands/ weapon.GetSkillCount()));
						index++;
					}
				}
			}
			*/
		}

		public void UseSkill(int handIndex) {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.UseSkill(handIndex, this.group, this.no, this.SkillUsed);
		}
		
		private void SkillUsed(int handIndex, int skillId) {
			if (this.SkillUse != null) {
				SkillEntity skillEntity = SkillFactory.Instance.Build(skillId);
				this.SkillUse(skillEntity);
			}
		}
		
		
		public void DrawSkills(bool initialDraw) {
			
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.DrawSkills(this.group, this.no, initialDraw, this.SkillsDrawn);
		}
		
		public void SkillsDrawn(SkillEntity[] drawnSkills, CompositeData compositeData) {
			if (this.SkillDraw != null) {
				this.SkillDraw(drawnSkills, compositeData);
			}
		}
	}
}
