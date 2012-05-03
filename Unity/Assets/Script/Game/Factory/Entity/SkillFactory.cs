using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Data.Cache;

namespace TinyQuest.Factory.Entity {
	public class SkillFactory {
		
		public static readonly SkillFactory Instance = new SkillFactory();
		private SkillFactory(){}
		
		public SkillEntity Build(int id) {
			MasterSkill masterSkill = MasterDataCache<MasterSkill>.Instance.Get(id);
			
			SkillEntity skillEntity = new SkillEntity(masterSkill);
			return skillEntity;
		}
	}
}