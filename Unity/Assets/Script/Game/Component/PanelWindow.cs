using TinyQuest.Core;
using TinyQuest.Component.Panel;
using TinyQuest.Model;
using UnityEngine;

namespace TinyQuest.Component {
	public enum PanelType {
		Undefined,
		MapNavigation,
		Combat
	}
	
	public class PanelWindow : Roga2dNode {
		public event WindowMessageEvent MessageEvent;
		private Ally player;
		private bool isPressed;
		private Vector2 lastTouchedPosition;
		private Collider pressedCollider;
		private PanelType selectedPanelType;
		private BasePanel selectedPanel;
		private MapModel mapModel;

		public PanelWindow(MapModel mapModel)
		{
			this.mapModel = mapModel;
			this.setPanel(PanelType.MapNavigation);
		}
		
		public override void Destroy() {
			this.removeSelectedPanel();	
			base.Destroy();
		}
		
		private void removeSelectedPanel() {
			switch (this.selectedPanelType) {
			case PanelType.Combat:
				CombatControlPanel combatControlPanel = (CombatControlPanel)this.selectedPanel;
				combatControlPanel.CardSelected -= this.onCardSelected;
				break;
			case PanelType.MapNavigation:
				MapNavigatorPanel mapNavigator = (MapNavigatorPanel)this.selectedPanel;
				this.mapModel.StepMoved -= mapNavigator.OnStepMoved;
				mapNavigator.MessageSent -= this.OnMessage;
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
				combatControlPanel.CardSelected += this.onCardSelected;
				panel = combatControlPanel;
				break;
			case PanelType.MapNavigation:
				MapNavigatorPanel mapNavigator = new MapNavigatorPanel(this.mapModel);
				this.mapModel.StepMoved += mapNavigator.OnStepMoved;
				mapNavigator.MessageSent += this.OnMessage;
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
			MessageEvent(message);
		}
		
		private void onCardSelected(int cardIndex) {
			WindowMessage message = new WindowMessage(WindowMessageType.CombatCardTouched, cardIndex);
			MessageEvent(message);
		}
		
		public void ReceiveMessage(WindowMessage message) 
		{
			switch (message.Type) {
			case WindowMessageType.StartCombat:
				this.setPanel(PanelType.Combat);
				break;
			case WindowMessageType.FinishCombat:
				this.setPanel(PanelType.MapNavigation);
				break;	
			}
		}

		public override void Update ()
		{
			base.Update ();
			
			// Construct a ray from the current mouse coordinates
			if (Input.GetMouseButtonDown(0)) {
				this.isPressed = true;
				this.lastTouchedPosition = Input.mousePosition;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hitInfo = new RaycastHit();
				if (Physics.Raycast(ray, out hitInfo)) {
					this.pressedCollider = hitInfo.collider;
					
					if (this.pressedCollider != null) {
						object obj = hitInfo.point;
						this.pressedCollider.SendMessage("ReceiveTouchDown", obj);
					}
				}
			}
			
			if (this.isPressed) {
				if (this.selectedPanel != null) {
					float actualLogicalRatio = Config.LogicalWidth / (float)Screen.width;
					this.selectedPanel.OnTouchMoved(
						new Vector2(
							Input.mousePosition.x - this.lastTouchedPosition.x,
							-Input.mousePosition.y + this.lastTouchedPosition.y
						) * actualLogicalRatio
					);
					this.lastTouchedPosition = Input.mousePosition;
				}
				//
			}
			
			if (Input.GetMouseButtonUp(0)) {	
				this.isPressed = false;
				if (this.pressedCollider != null) {
					this.pressedCollider.SendMessage("ReceiveTouchUp");
					this.pressedCollider = null;
				}
			}
		}
	}
}