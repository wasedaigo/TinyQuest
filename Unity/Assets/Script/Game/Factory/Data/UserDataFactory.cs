using UnityEngine;
using JsonFx.Json;
using TinyQuest.Data;

namespace TinyQuest.Factory.Data {
	public class UserDataFactory<T> {
		
		public static readonly UserDataFactory<T> Instance = new UserDataFactory<T>();
		private UserDataFactory(){}
		private T userData;

		public T Build() {
			if (this.userData == null) {
				string path = typeof(T).Name;
				TextAsset txt = (TextAsset)Resources.Load("Data/User/" + path, typeof(TextAsset));
				this.userData = JsonReader.Deserialize<T>(txt.text);
			}
			return this.userData;
		}
	}
}

