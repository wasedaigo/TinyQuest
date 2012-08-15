using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Cache {
	public class LocalUserDataCache
	{
		protected LocalUserData localUserData;
		//protected Dictionary<int, UnitInstance> unitInstanceDictionary;
		
		public LocalUserData Data {
			get {
				return this.localUserData;	
			}
		}
		
		public string Serialize() {
			// Need to implementa
			
			return "";	
		}
		
		public void SetData(string jsonText) {
			this.localUserData = JsonReader.Deserialize<LocalUserData>(jsonText);
		}
		
		private Dictionary<int, T> GetAsDictionary<T>(List<T> data) 
			where T : IDData
		{
			Dictionary<int, T> dictionary = new Dictionary<int, T>();
			for (int i = 0; i < data.Count; i++) {
				dictionary.Add(data[i].id, data[i]);
			}
			return dictionary;
		}
		
		public void SetZone(string jsonText) {
			this.localUserData.zone = JsonReader.Deserialize<UserZone>(jsonText);
		}
			
		public virtual UserStatus GetUserStatus() {
			return this.localUserData.status;
		}
			
		public virtual UserZone GetUserZone() {
			return this.localUserData.zone;
		}
		/*
		public virtual UserUnit[] GetParty() {
			int[] party = this.localUserData.party;
			UserUnit[] units = new UserUnit[party.Length];
			for (int i = 0; i < party.Length; i++) {
				units[i] = this.GetUnitByID(party[i]);
			}
			return units;
		}*/
		
		/*
		public virtual UnitInstance GetUnitInstanceByID(int id) {
			return this.unitInstanceDictionary[id];
		}*/
		
		public void Commit() {
			return;
			string text = JsonWriter.Serialize(this.localUserData);

			// make a path
			string path = Path.Combine(Application.persistentDataPath, "Save/LocalUser.txt");
			Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Save"));
			
			// write some data
			using (StreamWriter writer = new StreamWriter(File.Create(path)))
			{
			    writer.Write(text);
			}
			
			Debug.Log("LocalUser.txt Committed at " + path);
		}
	}
}

