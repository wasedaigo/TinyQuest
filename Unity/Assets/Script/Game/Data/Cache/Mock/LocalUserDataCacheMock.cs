using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Cache {
	public class LocalUserDataCacheMock : LocalUserDataCache
	{
		public override UserWeapon GetWeaponByID(int id) {
			if (this.weaponDictionary == null) {
				TextAsset txt = (TextAsset)Resources.Load("Data/LocalUser", typeof(TextAsset));
				this.Set(txt.text);
			}
			return base.GetWeaponByID(id);
		}
		
		public override UserZoneProgress GetZoneProgressByID(int id) {
			if (this.zoneProgressDictionary == null) {
				TextAsset txt = (TextAsset)Resources.Load("Data/LocalUser", typeof(TextAsset));
				this.Set(txt.text);
			}
			return base.GetZoneProgressByID(id);
		}
	}
}

