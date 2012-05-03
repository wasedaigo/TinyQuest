using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Data.Cache {
	public class MasterDataCache<T>
		where T : BaseMasterData
	{
		public static readonly MasterDataCache<T> Instance = new MasterDataCache<T>();
		private MasterDataCache(){}
		private Dictionary<int, T> masterData = new Dictionary<int, T>();
		
		public T Get(int id) {
			return this.masterData[id];
		}
		
		public void Set(string jsonText) {
			MasterDataCollection<T> data = JsonReader.Deserialize<MasterDataCollection<T>>(jsonText);
			this.masterData.Clear();
			for (int i = 0; i < data.data.Length; i++) {
				this.masterData.Add(data.data[i].id, data.data[i]);
			}
		}

	}
}

