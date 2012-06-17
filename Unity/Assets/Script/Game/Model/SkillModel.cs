using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;

namespace TinyQuest.Model {
	public class SkillModel {
		private MasterSkill masterSkill;
		public MasterSkill MasterSkill {
			get { return this.masterSkill; }
		}

		public SkillModel(MasterSkill masterSkill) {
			this.masterSkill = masterSkill;
		}
	}
}
