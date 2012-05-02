using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Factory.Data;

namespace TinyQuest.Factory.Entity {
	public class SkillFactory {
		
		public static readonly SkillFactory Instance = new SkillFactory();
		private SkillFactory(){}
		
		public SkillEntity Build(int id) {
			MasterSkills masterSkills = MasterDataFactory<MasterSkills>.Instance.Get();
			MasterSkill masterSkill = masterSkills.data[id.ToString()];
			
			SkillEntity skillEntity = new SkillEntity(masterSkill);
			return skillEntity;
		}
	}
}