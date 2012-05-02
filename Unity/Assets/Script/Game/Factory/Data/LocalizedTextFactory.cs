using UnityEngine;
using JsonFx.Json;
using TinyQuest.Data;

namespace TinyQuest.Factory.Data {
	public class LocalizedTextFactory {
		
		public static readonly LocalizedTextFactory Instance = new LocalizedTextFactory();
		private LocalizedTextFactory(){}
		private MasterLocalizedText masterData;
		public string Get(string key) {
			if (this.masterData == null) {
				TextAsset txt = (TextAsset)Resources.Load("Data/Master/Localize/en", typeof(TextAsset));
				this.masterData = JsonReader.Deserialize<MasterLocalizedText>(txt.text);
			}
			return this.masterData.data[key];
		}
	}
}

