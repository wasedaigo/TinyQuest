using JsonFx.Json;
using System;

namespace TinyQuest.Data{
	public enum ZoneCommandType {
		Empty = 1,
		Battle = 2,
		Treasure = 3
	}
	
	public enum ZoneCutSceneType {
		Message = 1,
		Pop = 2,
		Depop = 3
	}
	
	public enum ZoneCutScenePopAnimationType {
		SlideIn,
		JumpIn
	}

	public enum ZoneCutSceneDepopAnimationType {
		SlideOut,
		JumpOut
	}
	
	public class TypeContentData {
		
		public readonly int type;
		public readonly object content;
		public T GetContent<T>() {
			return JsonReader.Deserialize<T>(JsonWriter.Serialize(content));
		}
	}
	
	public class ZoneCommandBase {
		public readonly TypeContentData command;
		public TypeContentData[] cutScenes;
	}
	
	public class ZoneCommandEmpty : ZoneCommandBase {
	}
	
	public class ZoneCommandBattle : ZoneCommandBase {
		public int enemyGroupId;
	}
	
	public class ZoneCommandTreasure : ZoneCommandBase {
		public int treasureId;
	}

	// Cut Scenes
	public class ZoneCutSceneBase {
		public readonly int type;
		public ZoneCutSceneType GetCutSceneType() {
			return (ZoneCutSceneType)type;
		}
	}

	public class ZoneMessageCutScene : ZoneCutSceneBase {
		public int pos;
		public string text;
	}

	public class ZonePopCutScene : ZoneCutSceneBase {
		public int unitId;
		public readonly int animation;
		public ZoneCutScenePopAnimationType GetAnimation() {
			return 	(ZoneCutScenePopAnimationType)animation;
		}
	}
	
	public class ZoneDepopCutScene : ZoneCutSceneBase {
		public readonly int animation;
		public ZoneCutSceneDepopAnimationType GetAnimation() {
			return 	(ZoneCutSceneDepopAnimationType)animation;
		}
	}
	
	public class ZoneEvent {
		public ZoneCommandBase[] commands;
	}
}
