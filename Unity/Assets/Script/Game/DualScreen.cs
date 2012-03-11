using UnityEngine;
using System.Collections;
using TinyQuest.Cache;
using TinyQuest.Component;
using TinyQuest.Component.Window;
using TinyQuest.Model;

public class DualScreen : Roga2dNode{
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
		
		AdventureWindow adventureWindow = new AdventureWindow(mapModel);
		topWindow.AddChild(adventureWindow);
		
		ControlWindow controlWindow = new ControlWindow(mapModel);
		bottomWindow.AddChild(controlWindow);
		
		// Connect PreviewWindow and ControlWindow
		controlWindow.MessageEvent += controlWindow.ReceiveMessage;
		controlWindow.MessageEvent += adventureWindow.ReceiveMessage;

		adventureWindow.MessageEvent += controlWindow.ReceiveMessage;
		adventureWindow.MessageEvent += adventureWindow.ReceiveMessage;

		// BG
		Roga2dSprite mainWindow = new Roga2dSprite("UI/frame", new Vector2(160, 240), new Vector2(0, 0), new Rect(0, 0, 160, 240));
		frame.AddChild(mainWindow);
	}
}
