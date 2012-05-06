using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Data.Cache {
	public class MasterDataCacheMock : MasterDataCache
	{
		public override MasterZone GetLoadedZone() {
			if (this.zone == null) {
				TextAsset txt = (TextAsset)Resources.Load("Data/Zone/" + 1, typeof(TextAsset));
				this.SetZone(txt.text);
			}
			return base.GetLoadedZone();
		}
		
		public override MasterWeapon GetWeaponByID(int id) {
			if (this.masterWeaponDictionary == null) {
				TextAsset txt = (TextAsset)Resources.Load("Data/Master", typeof(TextAsset));
				this.Set(txt.text);
			}
			return base.GetWeaponByID(id);
		}
		
		public override MasterSkill GetSkillByID(int id) {
			if (this.masterSkillDictionary == null) {
				TextAsset txt = (TextAsset)Resources.Load("Data/Master", typeof(TextAsset));
				this.Set(txt.text);
			}
			return base.GetSkillByID(id);
		}
	}
}

