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
		protected Dictionary<int, UserCore> userCoreDictionary;
		protected Dictionary<int, UserPuppet> userPuppetDictionary;
		protected Dictionary<int, MonsterInstance> monsterInstanceDictionary;
		protected Dictionary<int, UserGear> userGearDictionary;
		
		public string Serialize() {
			// Need to implement
			return "";	
		}
		
		public void Set(string jsonText) {
			this.localUserData = JsonReader.Deserialize<LocalUserData>(jsonText);
			this.userPuppetDictionary = this.GetAsDictionary<UserPuppet>(this.localUserData.ownPuppets);
			this.monsterInstanceDictionary = this.GetAsDictionary<MonsterInstance>(this.localUserData.monsterInstances);
			this.userCoreDictionary = this.GetAsDictionary<UserCore>(this.localUserData.ownCores);
			this.userGearDictionary = this.GetAsDictionary<UserGear>(this.localUserData.ownGears);
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
			this.localUserData.zone = JsonReader.Deserialize<UserZone>(jsonText);
		}
		
		public virtual void SetCombatProgress(CombatProgress combatProgress) {
			this.localUserData.combatProgress = combatProgress;
		}
			
		public virtual CombatProgress GetCombatProgress() {
			return this.localUserData.combatProgress;
		}
			
		public virtual UserStatus GetUserStatus() {
			return this.localUserData.status;
		}
			
		public virtual UserZone GetUserZone() {
			return this.localUserData.zone;
		}
		
		public virtual UserGear[] GetOwnGears() {
			return this.localUserData.ownGears;
		}
	
		public virtual UserCore[] GetOwnCores() {
			return this.localUserData.ownCores;
		}

		public virtual UserPuppet[] GetOwnPuppets() {
			return this.localUserData.ownPuppets;
		}
		
		public virtual UserPuppet[] GetParty() {
			int[] party = this.localUserData.party;
			UserPuppet[] puppets = new UserPuppet[party.Length];
			for (int i = 0; i < party.Length; i++) {
				puppets[i] = this.GetPuppetByID(party[i]);
			}
			return puppets;
		}
		
		public virtual UserPuppet GetPuppetByID(int id) {
			return this.userPuppetDictionary[id];
		}
		
		public virtual UserCore GetUserCoreByID(int id) {
			return this.userCoreDictionary[id];
		}
		
		public virtual UserGear GetUserGearByID(int id) {
			return this.userGearDictionary[id];
		}
		
		public virtual MonsterInstance GetMonsterInstanceByID(int id) {
			return this.monsterInstanceDictionary[id];
		}
		
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

