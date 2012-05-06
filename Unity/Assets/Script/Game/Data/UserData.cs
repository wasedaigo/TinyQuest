namespace TinyQuest.Data{

	public class UserWeapon : IDData{
		public int weaponId;
		public int exp;
		public int slot;
	}
	
	public class UserZone{
		public int zoneId;
		public ZoneEvent[] events;
	}
	
	public class UserZoneProgress : IDData{
      	public int progressStep;
		public int clearCount;
	}
	
	public class LocalUserData {
		public readonly UserZone zone;
		public readonly UserWeapon[] equipWeapons;
		public readonly UserWeapon[] stockWeapons;
		public readonly UserZoneProgress[] zoneProgresses;
	}
}