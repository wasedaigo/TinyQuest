using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;

namespace TinyQuest.Entity {
	public class SkillEntity {
		private MasterSkill masterSkill;
		public MasterSkill MasterSkill {
			get { return this.masterSkill; }
		}

		public SkillEntity(MasterSkill masterSkill) {
			this.masterSkill = masterSkill;
		}
	}
}
