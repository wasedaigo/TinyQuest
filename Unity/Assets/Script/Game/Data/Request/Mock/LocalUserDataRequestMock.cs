using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using Async;

namespace TinyQuest.Data.Request {
	public class LocalUserDataRequestMock : LocalUserDataRequest
	{
		public override void StartBattle(MonoBehaviour monoBehaviour, System.Action callback) {
			TextAsset txt = (TextAsset)Resources.Load("Data/CombatMock", typeof(TextAsset));
			Debug.Log("WWW Ok!: " + txt.text);
			CacheFactory.Instance.GetLocalUserDataCache().SetData(txt.text);	
			LocalUserDataCache cache = CacheFactory.Instance.GetLocalUserDataCache();
			LocalUserData data = cache.Data;
			callback();
		}
	}
}
