using System.Collections.Generic;
namespace TinyQuest.Data{
	
	public class UserStatus {
		public int maxHP;	
	}
	
	public class UserWeapon : IDData{
		public int weaponId;
		public int exp;
		public int slot;
	}
	
	public class UserZone{
		public int zoneId;
		public int lastStepIndex;
		public Dictionary<string, ZoneEvent> events;

		public int playerHP;
    	public int stepIndex;
    	public int commandIndex;
		public object commandState;
		public int[] weaponDurabilities;
		public int currentAP;
	}

	public class LocalUserData {
		public readonly UserStatus status;
		public readonly UserZone zone;
		public readonly UserWeapon[] equipWeapons;
		public readonly UserWeapon[] stockWeapons;
	}
}