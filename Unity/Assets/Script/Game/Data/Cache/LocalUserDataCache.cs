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
		protected Dictionary<int, UserUnit> userUnitDictionary;
		//protected Dictionary<int, UnitInstance> unitInstanceDictionary;
		
		public string Serialize() {
			// Need to implement
			return "";	
		}
		
		public void Set(string jsonText) {
			this.localUserData = JsonReader.Deserialize<LocalUserData>(jsonText);
			//this.userUnitDictionary = this.GetAsDictionary<UserUnit>(this.localUserData.OwnUnits);
			//this.unitInstanceDictionary = this.GetAsDictionary<UnitInstance>(this.localUserData.unitInstances);
		}
		
		private Dictionary<int, T> GetAsDictionary<T>(T[] data) 
			where T : IDData
		{
			Dictionary<int, T> dictionary = new Dictionary<int, T>();
			for (int i = 0; i < data.Length; i++) {
				dictionary.Add(data[i].id, data[i]);
			}
			return dictionary;
		}
		
		public void SetZone(string jsonText) {
			this.localUserData.Zone = JsonReader.Deserialize<UserZone>(jsonText);
		}
		
		public virtual void SetCombatProgress(CombatProgress combatProgress) {
			this.localUserData.CombatProgress = combatProgress;
		}
			
		public virtual CombatProgress GetCombatProgress() {
			return this.localUserData.CombatProgress;
		}
			
		public virtual UserStatus GetUserStatus() {
			return this.localUserData.Status;
		}
			
		public virtual UserZone GetUserZone() {
			return this.localUserData.Zone;
		}

		public virtual UserUnit[] GetOwnUnits() {
			return this.localUserData.OwnUnits;
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
		
		public virtual UserUnit GetUnitByID(int id) {
			return this.userUnitDictionary[id];
		}
		
		public virtual UserUnit GetUserUnitByID(int id) {
			return this.userUnitDictionary[id];
		}
		/*
		public virtual UnitInstance GetUnitInstanceByID(int id) {
			return this.unitInstanceDictionary[id];
		}*/
		
		public void Commit() {
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

