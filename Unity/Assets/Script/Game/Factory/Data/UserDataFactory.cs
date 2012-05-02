using UnityEngine;
using JsonFx.Json;
using TinyQuest.Data;

namespace TinyQuest.Factory.Data {
	public class UserDataFactory<T> {
		
		public static readonly UserDataFactory<T> Instance = new UserDataFactory<T>();
		private UserDataFactory(){}
		private T masterData;

		public T Build() {
			if (this.masterData == null) {
				string path = typeof(T).Name;
				TextAsset txt = (TextAsset)Resources.Load("Data/User/" + path, typeof(TextAsset));
				this.masterData = JsonReader.Deserialize<T>(txt.text);
			}
			return this.masterData;
		}
	}
}

