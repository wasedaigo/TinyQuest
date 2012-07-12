using TinyQuest.Data;

public class Constant {
	public const int PlayerGroupType = 0;
	public const int EnemyGroupType = 1;
	public const int GroupTypeCount = 2;
}

public enum ActorHealthState {
	Full,
	Ok,
	Dying,
	Dead
}