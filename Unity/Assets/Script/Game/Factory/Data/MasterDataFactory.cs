using UnityEngine;
using JsonFx.Json;
using TinyQuest.Data;

namespace TinyQuest.Factory.Data {
	public class MasterDataFactory<T> {
		
		public static readonly MasterDataFactory<T> Instance = new MasterDataFactory<T>();
		private MasterDataFactory(){}
		private T masterData;
		
		public T Get() {
			if (this.masterData == null) {
				string path = typeof(T).Name;
				TextAsset txt = (TextAsset)Resources.Load("Data/Master/" + path, typeof(TextAsset));
				this.masterData = JsonReader.Deserialize<T>(txt.text);
			}
			return this.masterData;
		}
	}
}

