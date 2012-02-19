namespace TinyQuest.Core {
	public enum PanelWindowMessageType {
		FloorSymbolTouched,
		CombatCardTouched
	}
	public class PanelWindowMessage {
		public PanelWindowMessageType Type;
		public object Data;
		
		public PanelWindowMessage(PanelWindowMessageType type, object data) {
			this.Type = type;
			this.Data = data;
		}
	}
}
