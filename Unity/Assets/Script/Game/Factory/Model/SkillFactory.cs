using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Model;
using TinyQuest.Data.Cache;

namespace TinyQuest.Factory.Model {
	public class SkillFactory {
		
		public static readonly SkillFactory Instance = new SkillFactory();
		private SkillFactory(){}
		
		public SkillModel Build(int id) {
			MasterSkill masterSkill = CacheFactory.Instance.GetMasterDataCache().GetSkillByID(id);	
			SkillModel skillModel = new SkillModel(masterSkill);
			return skillModel;
		}
	}
}