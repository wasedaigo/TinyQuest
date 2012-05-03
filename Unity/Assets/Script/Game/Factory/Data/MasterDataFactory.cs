using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Factory.Data {
	public class MasterDataFactory<T> 
		where T : BaseMasterData
	{
		public static readonly MasterDataFactory<T> Instance = new MasterDataFactory<T>();
		
		public delegate void LoadData();
		public LoadData OnDataLoaded;
		
		private MasterDataFactory(){}
		private Dictionary<int, T> masterData = new Dictionary<int, T>();
		private bool loaded;
		
		public T Get(int id) {
			if (!this.loaded) {
				string path = typeof(T).Name;
				TextAsset txt = (TextAsset)Resources.Load("Data/Master/" + path, typeof(TextAsset));
				MasterDataCollection<T> data = JsonReader.Deserialize<MasterDataCollection<T>>(txt.text);
				for (int i = 0; i < data.data.Length; i++) {
					this.masterData.Add(data.data[i].id, data.data[i]);
				}
				this.loaded = true;
			}
			return this.masterData[id];
		}

		public void Load(int id, LoadData callback) {
			string path = typeof(T).Name;
			TextAsset txt = (TextAsset)Resources.Load("Data/Master/" + path + "/" + id, typeof(TextAsset));
			T data = JsonReader.Deserialize<T>(txt.text);
			this.masterData.Add(data.id, data);
			
			callback();
		}
	}
}

