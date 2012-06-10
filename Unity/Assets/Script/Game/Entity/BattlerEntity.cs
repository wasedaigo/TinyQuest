using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Entity;
using TinyQuest.Data.Request;
using TinyQuest.Factory.Entity;

namespace TinyQuest.Entity {
	public class BattlerEntity {
		
		public enum GroupType {
			Player = 0,
			Enemy = 1,
			Count
		};

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
		
		public void SetHP(int value) {
			this.hp = value;	
			this.UpdateHP(this, this.hp);
		}
		
		private CoreEntity core;

		
		public BattlerEntity(CoreEntity core, int hp, int maxHP,  int no, GroupType group) {
			this.core = core;
			this.maxHP = maxHP;
			this.hp = maxHP;
			this.no = no;
			this.group = group;
		}

		public CoreEntity GetCore() {
			return this.core;
		}

		public void UseSkill(int handIndex, BattlerEntity targetEntity) {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.UseSkill(this, targetEntity, this.SkillUsed);
		}
		
		private void SkillUsed(int skillId) {
			if (this.SkillUse != null) {
				SkillEntity skillEntity = SkillFactory.Instance.Build(skillId);
				this.SkillUse(skillEntity);
			}
		}
	}
}
