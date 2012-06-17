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

		public override UserUnit[] GetOwnUnits() {
			if (this.localUserData == null) {
				this.LoadLocalUserMock();
			}
			return base.GetOwnUnits();
		}
		/*
		public override UserUnit[] GetParty() {
			if (this.localUserData == null) {
				this.LoadLocalUserMock();
			}
			return base.GetParty();	
		}*/
	
		public override CombatProgress GetCombatProgress() {
			if (this.localUserData == null) {
				this.LoadLocalUserMock();
			}
			return base.GetCombatProgress();
		}
		
		public override UserStatus GetUserStatus() {
			if (this.localUserData == null) {
				this.LoadLocalUserMock();
			}
			return base.GetUserStatus();
		}
		
		public override UserZone GetUserZone() {
			if (this.localUserData == null) {
				this.LoadLocalUserMock();
			}
			return base.GetUserZone();
		}
	}
}

