using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Model;
using TinyQuest.Data.Request;
using TinyQuest.Factory.Model;

namespace TinyQuest.Model {
	public class BattlerModel {
		
		public enum GroupType {
			Player = 0,
			Enemy = 1,
			Count
		};

		public System.Action<SkillModel> SkillUse;
		public System.Action<BattlerModel, int> UpdateHP;
		
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
		
		private MasterUnit unit;

		
		public BattlerModel(MasterUnit unit, int hp, int no, GroupType group) {
			this.unit = unit;
			this.maxHP = unit.hpTable.GetValue(1);
			this.hp = unit.hpTable.GetValue(1);
			this.no = no;
			this.group = group;
		}

		public void UseSkill(int handIndex, BattlerModel targetModel) {
			LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
			req.UseSkill(this, targetModel, this.SkillUsed);
		}
		
		private void SkillUsed(int skillId) {
			if (this.SkillUse != null) {
				SkillModel skillModel = SkillFactory.Instance.Build(skillId);
				this.SkillUse(skillModel);
			}
		}
	}
}
