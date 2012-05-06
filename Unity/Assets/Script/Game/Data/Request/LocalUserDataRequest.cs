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
			userZoneProgress.progressStep += 1;
			CacheFactory.Instance.GetLocalUserDataCache().Commit();
			callback();
		}
	}
}
