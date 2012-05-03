namespace TinyQuest.Data{
	public class UserSkill: BaseMasterData {
		public int skillId;
		public int rate;
	}

	public class UserWeapon: BaseMasterData {
		public int weaponId;
		public int exp;
	}

	public class UserZone: BaseMasterData {
		public ZoneEvent[] events;
	}

	public class UserZoneProgress: BaseMasterData {
      	public int progressStep;
		public int clearCount;
	}
}