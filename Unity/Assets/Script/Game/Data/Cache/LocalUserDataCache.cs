using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Cache {
	public class LocalUserDataCache<T>
		where T : BaseMasterData
	{
		public static readonly LocalUserDataCache<T> Instance = new LocalUserDataCache<T>();
		private LocalUserDataCache(){}

		private Dictionary<int, T> userData = new Dictionary<int, T>();
		
		public T Get(int id) {
			return this.userData[id];
		}
		
		public void Set(MasterDataCollection<T> data) {
			this.userData.Clear();
			for (int i = 0; i < data.data.Length; i++) {
				this.userData.Add(data.data[i].id, data.data[i]);
			}
		}
	}
}

