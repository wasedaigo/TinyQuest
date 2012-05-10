using System.Collections.Generic;
namespace TinyQuest.Data{

	public class UserWeapon : IDData{
		public int weaponId;
		public int exp;
		public int slot;
	}
	
	public class UserZone{
		public int zoneId;
		public int lastStepIndex;
		public Dictionary<string, ZoneEvent> events;
	}
	
	public class UserZoneProgress : IDData{
    public int stepIndex;
    public int commandIndex;
		public object commandState;
		public int clearCount;
		public int[] weaponDurabilities;
		public int currentAP;
	}
	
	public class LocalUserData {
		public readonly UserZone zone;
		public readonly UserWeapon[] equipWeapons;
		public readonly UserWeapon[] stockWeapons;
		public readonly UserZoneProgress[] zoneProgresses;
	}
}