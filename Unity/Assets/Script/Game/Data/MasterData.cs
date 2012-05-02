using System.Collections.Generic;

namespace TinyQuest.Data{
	public class MasterWeapon {
		public string name;
		public string path;
		public string description;
		public int power;
	}

	public class MasterWeapons {
		public Dictionary<string, MasterWeapon> data;
	}
	
	public class MasterSkill {
		public string name;
		public string path;
	}

	public class MasterSkills {
		public Dictionary<string, MasterSkill> data;
	}

	public class MasterLocalizedText {
		public Dictionary<string, string> data;
	}
}
