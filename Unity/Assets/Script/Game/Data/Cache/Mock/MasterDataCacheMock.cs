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
		
		public override MasterGear GetGearByID(int id) {
			if (this.masterGearDictionary == null) {
				this.LoadMaster();
			}
			return base.GetGearByID(id);
		}

		public override MasterSkill GetSkillByID(int id) {
			if (this.masterSkillDictionary == null) {
				this.LoadMaster();
			}
			return base.GetSkillByID(id);
		}
		
		public virtual MasterCore GetCoreByID(int id) {
			if (this.masterCoreDictionary == null) {
				this.LoadMaster();
			}
			return base.GetCoreByID(id);
		}
		
		public override MasterMonster GetMonsterByID(int id) {
			if (this.masterMonsterDictionary == null) {
				this.LoadMaster();
			}
			return base.GetMonsterByID(id);
		}
	}
}