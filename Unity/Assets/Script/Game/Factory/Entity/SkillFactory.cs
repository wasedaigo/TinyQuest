using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Factory.Data;

namespace TinyQuest.Factory.Entity {
	public class SkillFactory {
		
		public static readonly SkillFactory Instance = new SkillFactory();
		private SkillFactory(){}
		
		public SkillEntity Build(int id) {
			MasterSkill masterSkill = MasterDataFactory<MasterSkill>.Instance.Get(id);
			
			SkillEntity skillEntity = new SkillEntity(masterSkill);
			return skillEntity;
		}
	}
}