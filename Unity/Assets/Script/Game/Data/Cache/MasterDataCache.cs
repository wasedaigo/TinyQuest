using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Cache {
	public class MasterDataCache<T> : IAsync
		where T : BaseMasterData
	{
		public static readonly MasterDataCache<T> Instance = new MasterDataCache<T>();
		private MasterDataCache(){}
		private Dictionary<int, T> masterData = new Dictionary<int, T>();
		private bool loaded;
		
		public T Get(int id) {
			if (!this.loaded) {
				Debug.LogError("Cache is not loaded");
			}
			return this.masterData[id];
		}
		
		public void Load(System.Action callback) {
			string path = typeof(T).Name;
			TextAsset txt = (TextAsset)Resources.Load("Data/Master/" + path, typeof(TextAsset));
			MasterDataCollection<T> data = JsonReader.Deserialize<MasterDataCollection<T>>(txt.text);
			for (int i = 0; i < data.data.Length; i++) {
				this.masterData.Add(data.data[i].id, data.data[i]);
			}
			this.loaded = true;
			callback();
		}

		public void Load(int id) {
			this.masterData.Clear();
			string path = typeof(T).Name;
			TextAsset txt = (TextAsset)Resources.Load("Data/Master/" + path + "/" + id, typeof(TextAsset));
			MasterDataCollection<T> data = JsonReader.Deserialize<MasterDataCollection<T>>(txt.text);
			for (int i = 0; i < data.data.Length; i++) {
				if (data.data[i].id == id) {
					this.masterData.Add(data.data[i].id, data.data[i]);
					break;	
				}
			}
			if (this.masterData.Count == 0) {
				Debug.LogError("<" + path + "> No data loaded for " + id);	
			}
			this.loaded = true;
		}
	}
}

