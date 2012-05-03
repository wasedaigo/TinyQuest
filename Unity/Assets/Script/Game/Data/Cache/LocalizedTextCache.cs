using UnityEngine;
using JsonFx.Json;
using TinyQuest.Data;

namespace TinyQuest.Data.Cache {
	public class LocalizedTextCache {
		public enum LocalizeGroupKey{
			System,
			Weapon,
			Skill,
			Zone,
			Count
		};
		public static readonly LocalizedTextCache Instance = new LocalizedTextCache();
		private LocalizedTextCache(){}
		private LocalizedText[] textData = new LocalizedText[(int)LocalizeGroupKey.Count];
		private bool loaded;
		
		public string Get(LocalizeGroupKey groupKey, string key) {
			if (!this.loaded) {
				TextAsset txt = (TextAsset)Resources.Load("Data/Localize/en/" + groupKey.ToString(), typeof(TextAsset));
				this.textData[(int)groupKey] = JsonReader.Deserialize<LocalizedText>(txt.text);
				this.loaded = true;
			}
			return this.textData[(int)groupKey].data[key];
		}
		
		public void Set(LocalizeGroupKey groupKey, LocalizedText data) {
			this.textData[(int)groupKey] = data;
		}
	}
}

