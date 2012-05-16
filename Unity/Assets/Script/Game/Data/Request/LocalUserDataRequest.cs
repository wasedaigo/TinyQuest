using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Data.Cache;

namespace TinyQuest.Data.Request {
	public class LocalUserDataRequest
	{
		public virtual void Get(System.Action<string> callback) {
		}
		
		public virtual void ProgressStep(System.Action callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			userZone.stepIndex += 1;
			userZone.commandIndex = 0;
			
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}
		
		public virtual void ProgressCommand(System.Action callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			userZone.commandIndex += 1;
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}
		
		public virtual void UseWeapon(int slot, System.Action<bool, int> callback) {
			UserWeapon[] userWeapons = CacheFactory.Instance.GetLocalUserDataCache().GetEquipWeapons();
			MasterWeapon masterWeapon = CacheFactory.Instance.GetMasterDataCache().GetWeaponByID(userWeapons[slot].weaponId);
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			
			MasterSkill masterSkill = CacheFactory.Instance.GetMasterDataCache().GetSkillByID(masterWeapon.skill1);
			userZone.currentTP -= masterSkill.tp;
			
			bool isBroken = true;
			if (userZone.weaponDurabilities[slot] > 0) {
				isBroken = false;
				userZone.weaponDurabilities[slot] -= 1;
			}
			
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			
			callback(isBroken, userZone.currentTP);
		}

		public virtual void ProcessCombat(BattlerEntity caster, BattlerEntity target, System.Action callback) {

			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			if (combatProgress != null) {
				CombatBattler playerBattlerData = new CombatBattler((int)BattlerEntity.NoType.Player, (int)BattlerEntity.GroupType.Player, 100, new int[]{});
				CombatBattler enemyBattlerData = new CombatBattler((int)BattlerEntity.NoType.Enemy, (int)BattlerEntity.GroupType.Enemy, 100, new int[]{});
				combatProgress = new CombatProgress(1, new CombatBattler[]{playerBattlerData, enemyBattlerData});
				CacheFactory.Instance.GetLocalUserDataCache().SetCombatProgress(combatProgress);
			}
			
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}
	}
}
