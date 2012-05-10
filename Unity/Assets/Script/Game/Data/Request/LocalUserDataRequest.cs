using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using TinyQuest.Data.Cache;

namespace TinyQuest.Data.Request {
	public class LocalUserDataRequest
	{
		public virtual void Get(System.Action<string> callback) {
		}
		
		public virtual void ProgressStep(System.Action callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			UserZoneProgress userZoneProgress = CacheFactory.Instance.GetLocalUserDataCache().GetZoneProgressByID(userZone.zoneId);
			userZoneProgress.stepIndex += 1;
			userZoneProgress.commandIndex = 0;
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}
		
		public virtual void ProgressCommand(System.Action callback) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			UserZoneProgress userZoneProgress = CacheFactory.Instance.GetLocalUserDataCache().GetZoneProgressByID(userZone.zoneId);
			userZoneProgress.commandIndex += 1;
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}
		
		public virtual void UseWeapon(int slot, System.Action<bool, int> callback) {
			UserWeapon[] userWeapons = CacheFactory.Instance.GetLocalUserDataCache().GetEquipWeapons();
			MasterWeapon masterWeapon = CacheFactory.Instance.GetMasterDataCache().GetWeaponByID(userWeapons[slot].weaponId);
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			UserZoneProgress userZoneProgress = CacheFactory.Instance.GetLocalUserDataCache().GetZoneProgressByID(userZone.zoneId);
			userZoneProgress.currentAP -= masterWeapon.ap;
			
			bool isBroken = true;
			if (userZoneProgress.weaponDurabilities[slot] > 0) {
				isBroken = false;
				userZoneProgress.weaponDurabilities[slot] -= 1;
			}
			
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			
			callback(isBroken, userZoneProgress.currentAP);
		}
		
	}
}
