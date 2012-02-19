namespace TinyQuest.Core {
	public enum WindowMessageType {
		StartCombat,
		FinishCombat,
		FloorSymbolTouched,
		CombatCardTouched
	}
	public class WindowMessage {
		public WindowMessageType Type;
		public object Data;
		
		public WindowMessage(WindowMessageType type, object data) {
			this.Type = type;
			this.Data = data;
		}
	}
	
	public delegate void WindowMessageEvent(WindowMessage message);
}
