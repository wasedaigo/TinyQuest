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
		
		public System.Action<SkillEntity[], CompositeData> SkillDraw;
		public System.Action<SkillEntity> SkillUse;
		public System.Action<BattlerEntity, int> UpdateHP;
		
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
		
		private int maxTP;
		public int MaxTP {
			get {return this.maxTP;}
		}
		
		private int regenTP;
		public int RegenTP {
			get {return this.regenTP;}
		}
		
		private int tp;
		public int TP {
			get {return this.tp;}
		}
		
		public void SetHP(int value) {
			this.hp = value;	
			this.UpdateHP(this, this.hp);
		}
		
		public void SetTP(int value) {
			this.tp = value;	
		}
		
		private WeaponEntity[] weapons = new WeaponEntity[WeaponSlotNum];
		
		public BattlerEntity(int hp, int maxHP, int regenTP, int maxTP, int no, GroupType group) {
			this.maxHP = maxHP;
			this.regenTP = regenTP;
			this.maxTP = maxTP;
			this.tp = this.maxTP;
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

		public void UseSkill(int handIndex, BattlerEntity targetEntity) {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.UseSkill(handIndex, this, targetEntity, this.SkillUsed);
		}
		
		private void SkillUsed(int handIndex, int skillId) {
			if (this.SkillUse != null) {
				SkillEntity skillEntity = SkillFactory.Instance.Build(skillId);
				this.SkillUse(skillEntity);
			}
		}
		
		public void DrawSkills(bool initialDraw) {
			
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.DrawSkills(this, initialDraw, this.SkillsDrawn);
		}
		
		public void SkillsDrawn(SkillEntity[] drawnSkills, CompositeData compositeData) {
			if (this.SkillDraw != null) {
				this.SkillDraw(drawnSkills, compositeData);
			}
		}
	}
}
