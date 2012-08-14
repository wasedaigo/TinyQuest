using TinyQuest.Data;

public class Constant {
	public static int UnitCount = 5;
	public static int SkillSlotCount = 2;
}

public class CombatGroupInfo {
	public static readonly CombatGroupInfo Instance = new CombatGroupInfo();
	
	private int playerGroupType;
	public void SetPlayerGroupType(int playerGroupType) {
		this.playerGroupType = playerGroupType;	
	}
	public int GetPlayerGroupType(int i) {
		if (i == 0) {
			return this.playerGroupType;
		} else {
			return 1 - this.playerGroupType;
		}
	}

	public int GetGroupCount() {
		return 2;	
	}
	
}

public enum ActorHealthState {
	Full,
	Ok,
	Dying,
	Dead
}