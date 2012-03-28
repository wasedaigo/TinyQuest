using TinyQuest.Core;
using TinyQuest.Component.Panel;
using TinyQuest.Model;
using UnityEngine;

namespace TinyQuest.Component.Window {
	public enum PanelType {
		Undefined,
		MapNavigation,
		Combat
	}

	public class BottomWindow : BaseComponent {

		private Ally player;
		private PanelType selectedPanelType;
		private BasePanel selectedPanel;
		private MapModel mapModel;

		public BottomWindow(MapModel mapModel)
		{
			this.mapModel = mapModel;
			this.setPanel(PanelType.Combat);
		}
		
		public override void Destroy() {
			this.removeSelectedPanel();	
			base.Destroy();
		}
		
		private void removeSelectedPanel() {
			switch (this.selectedPanelType) {
			case PanelType.Combat:
				CombatControlPanel combatControlPanel = (CombatControlPanel)this.selectedPanel;
				combatControlPanel.WindowMessageInvoked -= this.OnMessage;
				break;
			case PanelType.MapNavigation:
				MapNavigatorPanel mapNavigator = (MapNavigatorPanel)this.selectedPanel;
				this.mapModel.StepMoved -= mapNavigator.OnStepMoved;
				mapNavigator.WindowMessageInvoked -= this.OnMessage;
				break;
			}
			if (this.selectedPanel != null) {
				this.RemoveChild(this.selectedPanel);
				this.selectedPanel = null;
			}
		}
		
		private void setPanel(PanelType panelType) {
			// Don't do anything if it is trying to switch to the same panel
			if (this.selectedPanelType == panelType) {
				return;	
			}
			
			this.removeSelectedPanel();
			
			// Construct new panel component and add it to the scene
			BasePanel panel = null;
			switch (panelType) {
			case PanelType.Combat:
				CombatControlPanel combatControlPanel = new CombatControlPanel();
				combatControlPanel.WindowMessageInvoked += this.OnMessage;
				panel = combatControlPanel;
				break;
			case PanelType.MapNavigation:
				MapNavigatorPanel mapNavigator = new MapNavigatorPanel(this.mapModel);
				this.mapModel.StepMoved += mapNavigator.OnStepMoved;
				mapNavigator.WindowMessageInvoked += this.OnMessage;
				mapNavigator.Init();
				panel = mapNavigator;
				break;
			}
			this.AddChild(panel);
			// Adjugst priority and positions
			panel.LocalPriority = -0.2f;
			panel.LocalPixelPosition = new Vector2(-80, 20);
			this.selectedPanel = panel;
			this.selectedPanelType = panelType;
		}

		public void OnMessage(WindowMessage message) {
			this.SendMessage(message);
		}
		
		public override void ReceiveMessage(WindowMessage message) 
		{
			base.ReceiveMessage(message);
			switch (message.Type) {
			case WindowMessageType.StartCombat:
				this.setPanel(PanelType.Combat);
				break;
			case WindowMessageType.FinishCombat:
				this.setPanel(PanelType.MapNavigation);
				break;	
			}
		}

		public override void OnTouchMoved(Vector2 delta) {
			base.OnTouchMoved(delta);
			if (this.selectedPanel != null) {
				this.selectedPanel.OnTouchMoved(delta);
			}
		}
	}
}