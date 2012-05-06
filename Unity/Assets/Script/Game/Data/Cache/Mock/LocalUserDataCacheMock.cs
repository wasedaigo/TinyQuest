using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Cache {
	public class LocalUserDataCacheMock : LocalUserDataCache
	{
		public override UserWeapon[] GetEquipWeapons() {
			if (this.equipWeapons == null) {
				TextAsset txt = (TextAsset)Resources.Load("Data/LocalUserMock", typeof(TextAsset));
				this.Set(txt.text);
			}
			return base.GetEquipWeapons();
		}

		public override UserWeapon[] GetStockWeapons() {
			if (this.stockWeapons == null) {
				TextAsset txt = (TextAsset)Resources.Load("Data/LocalUserMock", typeof(TextAsset));
				this.Set(txt.text);
			}
			return base.GetStockWeapons();
		}
		
		public override UserZone GetUserZone() {
			return base.GetUserZone();
		}
		
		public override UserZoneProgress GetZoneProgressByID(int id) {
			if (this.zoneProgressDictionary == null) {
				TextAsset txt = (TextAsset)Resources.Load("Data/LocalUserMock", typeof(TextAsset));
				this.Set(txt.text);
			}
			return base.GetZoneProgressByID(id);
		}
	}
}

