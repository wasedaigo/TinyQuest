using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Cache {
	public class LocalUserDataCache<T> : IAsync
		where T : BaseMasterData
	{
		public static readonly LocalUserDataCache<T> Instance = new LocalUserDataCache<T>();
		private LocalUserDataCache(){}

		private Dictionary<int, T> userData = new Dictionary<int, T>();
		private bool loaded;
		
		public T Get(int id) {
			if (!this.loaded) {
				if (!this.loaded) {
					Debug.LogError("Cache is not loaded");
				}
			}
			return this.userData[id];
		}
		
		public void Load(System.Action callback) {
			string path = typeof(T).Name;
			TextAsset txt = (TextAsset)Resources.Load("Data/User/Local/" + path, typeof(TextAsset));
			MasterDataCollection<T> data = JsonReader.Deserialize<MasterDataCollection<T>>(txt.text);
			for (int i = 0; i < data.data.Length; i++) {
				this.userData.Add(data.data[i].id, data.data[i]);
			}
			this.loaded = true;
			callback();
		}

		public void Set(int id, T data) {
			this.userData.Add(data.id, data);
		}

		public void Save(int id,  System.Action callback) {
			string path = typeof(T).Name;
			
			string text = JsonWriter.Serialize(this.userData);
			StreamWriter writer = new StreamWriter("Resources/Data/User/Local/" + path + ".txt");
  			writer.WriteLine(text);
			writer.Close();
			callback();
		}
	}
}

