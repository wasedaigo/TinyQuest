using System.Collections.Generic;

namespace TinyQuest.Data{
	public class UserSkill {
		public int skillId;
		public int rate;
	}
	
	public class UserWeapon {
		public int id;
		public int weaponId;
		public UserSkill[] skills;
		public int exp;
	}

	public class UserWeapons {
		public UserWeapon[] data;
	}
}