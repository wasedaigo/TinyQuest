using UnityEngine;
using System.Collections;
using TinyQuest.Cache;
using TinyQuest;
using TinyQuest.Component;
using TinyQuest.Component.Window;
using TinyQuest.Model;

public class DualScreen : Roga2dNode{
	private bool isPressed;
	private Vector2 lastTouchedPosition;
	private Collider pressedCollider;
	private BaseComponent topPanel;
	private BaseComponent bottomPanel;

	// Use this for initialization
	public DualScreen() {
		Roga2dNode topWindow = new Roga2dNode();
		topWindow.LocalPriority = 20.0f;
		topWindow.LocalPosition = new Vector2(0, -1.5f);
		this.AddChild(topWindow);
		
		Roga2dNode bottomWindow = new Roga2dNode();
		topWindow.LocalPriority = 10.0f;
		this.AddChild(bottomWindow);
		
		Roga2dNode frame = new Roga2dNode();
		frame.LocalPriority = 30.0f;
		this.AddChild(frame);
		
		MapModel mapModel = MapCache.GetInstance().GetModel();
		
		this.topPanel = new AdventureWindow(mapModel);
		topWindow.AddChild(this.topPanel);
		
		this.bottomPanel = new ControlWindow(mapModel);
		bottomWindow.AddChild(this.bottomPanel);
		
		// Set up Frame
		Roga2dSprite mainWindow = new Roga2dSprite("UI/frame", new Vector2(160, 240), new Vector2(0, 0), new Rect(0, 0, 160, 240));
		frame.AddChild(mainWindow);

		// Connect PreviewWindow and ControlWindow
		this.bottomPanel.MessageEvent += this.bottomPanel.ReceiveMessage;
		this.bottomPanel.MessageEvent += this.topPanel.ReceiveMessage;

		this.topPanel.MessageEvent += this.bottomPanel.ReceiveMessage;
		this.topPanel.MessageEvent += this.topPanel.ReceiveMessage;
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
			float actualLogicalRatio = Config.LogicalWidth / (float)Screen.width;
			Vector2 delta = new Vector2(
					Input.mousePosition.x - this.lastTouchedPosition.x,
					-Input.mousePosition.y + this.lastTouchedPosition.y
			) * actualLogicalRatio;
			
			this.bottomPanel.OnTouchMoved(delta);
			this.topPanel.OnTouchMoved(delta);
			this.lastTouchedPosition = Input.mousePosition;
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
