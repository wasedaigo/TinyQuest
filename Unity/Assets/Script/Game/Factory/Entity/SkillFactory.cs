using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Data.Cache;

namespace TinyQuest.Factory.Entity {
	public class SkillFactory {
		
		public static readonly SkillFactory Instance = new SkillFactory();
		private SkillFactory(){}
		
		public SkillEntity Build(int id, WeaponEntity ownerWeapon, float weight) {
		
			MasterSkill masterSkill = CacheFactory.Instance.GetMasterDataCache().GetSkillByID(id);	
			SkillEntity skillEntity = new SkillEntity(ownerWeapon, masterSkill, weight);
			return skillEntity;
		}
	}
}