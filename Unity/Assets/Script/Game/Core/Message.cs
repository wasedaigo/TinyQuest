namespace TinyQuest.Core {
	public enum WindowMessageType {
		StartCombat,
		FinishCombat,
		FloorSymbolTouched,
		CombatCardTouched
	}
	public class PanelWindowMessage {
		public WindowMessageType Type;
		public object Data;
		
		public PanelWindowMessage(WindowMessageType type, object data) {
			this.Type = type;
			this.Data = data;
		}
	}
	
	public delegate void WindowMessageEvent(PanelWindowMessage message);
}
