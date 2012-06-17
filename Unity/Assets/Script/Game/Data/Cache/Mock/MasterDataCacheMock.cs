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
		
		private void LoadMaster() {
			TextAsset txt = (TextAsset)Resources.Load("Data/Master", typeof(TextAsset));
			this.Set(txt.text);	
		}

		public override MasterSkill GetSkillByID(int id) {
			if (this.masterSkillDictionary == null) {
				this.LoadMaster();
			}
			return base.GetSkillByID(id);
		}
		
		public override MasterUnit GetUnitByID(int id) {
			if (this.masterUnitDictionary == null) {
				this.LoadMaster();
			}
			return base.GetUnitByID(id);
		}
		
	}
}