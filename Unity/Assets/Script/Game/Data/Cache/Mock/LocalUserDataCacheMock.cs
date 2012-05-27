using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Cache {
	public class LocalUserDataCacheMock : LocalUserDataCache
	{
		private void LoadLocalUserMock() {
			TextAsset txt = (TextAsset)Resources.Load("Data/LocalUserMock", typeof(TextAsset));
			this.Set(txt.text);	
		}
		
		public override UserWeapon[] GetEquipWeapons() {
			if (this.equipWeapons == null) {
				this.LoadLocalUserMock();
			}
			return base.GetEquipWeapons();
		}

		public override UserWeapon[] GetStockWeapons() {
			if (this.stockWeapons == null) {
				this.LoadLocalUserMock();
			}
			return base.GetStockWeapons();
		}
		
		public override CombatProgress GetCombatProgress() {
			if (this.combatProgress == null) {
				this.LoadLocalUserMock();
			}
			return base.GetCombatProgress();
		}
		
		public override UserStatus GetUserStatus() {
			if (this.userStatus == null) {
				this.LoadLocalUserMock();
			}
			return base.GetUserStatus();
		}
		
		public override UserZone GetUserZone() {
			if (this.userZone == null) {
				this.LoadLocalUserMock();
			}
			return base.GetUserZone();
		}
	}
}

