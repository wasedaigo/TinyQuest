using UnityEngine;
using System.Collections.Generic;
using JsonFx.Json;
using TinyQuest.Data;

namespace TinyQuest.Data.Cache {
	public class LocalizedTextCache {
		protected Dictionary<string, LocalizedText> textData;
		
		public virtual string Get(string groupKey, string key, string subKey) {
			return this.textData[groupKey].data[key][subKey];
		}

		public void Set(string text) {
			this.textData = JsonReader.Deserialize<Dictionary<string, LocalizedText>>(text);
		}
		
		public void Set(string groupKey, Dictionary<string, Dictionary<string, string>> data) {
			this.textData[groupKey].data = data;
		}
	}
}
