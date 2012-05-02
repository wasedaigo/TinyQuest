using System.Collections.Generic;

namespace TinyQuest.Data{
	public class UserSkill {
		public int skillId;
		public int rate;
	}
	
	public class UserWeapon {
		public UserSkill[] skills;
	}

	public class UserWeapons {
		public Dictionary<string, UserWeapon> data;
	}
}