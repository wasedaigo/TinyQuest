using UnityEngine;
using System.Collections.Generic;
using JsonFx.Json;
using TinyQuest.Data;


namespace TinyQuest.Data.Cache {
	public class LocalizedTextCacheMock : LocalizedTextCache {
		public override string Get(string groupKey, string key, string subKey) {
			if (this.textData == null) {
				TextAsset txt = (TextAsset)Resources.Load("Data/Localize/en", typeof(TextAsset));
				base.Set(txt.text);
			}
			return this.textData[groupKey].data[key][subKey];
		}
	}
}

