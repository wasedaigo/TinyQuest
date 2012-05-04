namespace TinyQuest.Data{

	public class UserWeapon : IDData{
		public int weaponId;
		public int exp;
	}

	public class UserZone : IDData{
		public ZoneEvent[] events;
	}

	public class UserZoneProgress : IDData {
      	public int progressStep;
		public int clearCount;
	}
	
	public class LocalUserData {
		public UserZone zone;
		public UserWeapon[] weapons;
		public UserZoneProgress[] zoneProgresses;
	}
}