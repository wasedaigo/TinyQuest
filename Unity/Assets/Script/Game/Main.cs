using UnityEngine;
using System.Collections;
using TinyQuest.Scene;
using TinyQuest.Component;

public class Main : MonoBehaviour {
	public GameObject MainWindow;
	public GameObject SubWindow;
	private DungeonScene mainWindow;
	private AdventureWindow subWindow;

	// Use this for initialization
	void Start () {
		this.mainWindow = new DungeonScene();
		this.mainWindow.LocalScale = new Vector2(2.0f, 2.0f);
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.mainWindow.Transform);
		this.mainWindow.Transform.parent = MainWindow.transform;
		Roga2dUtils.applyState(this.mainWindow.Transform, state);
		
		this.subWindow = new AdventureWindow();
		state = Roga2dUtils.stashState(this.subWindow.Transform);
		this.subWindow.Transform.parent = SubWindow.transform;
		Roga2dUtils.applyState(this.subWindow.Transform, state);
		
		
		this.mainWindow.SymbolTouched += this.subWindow.SymbolTouched;
	}

	// Update is called once per frame
	void Update () {
		this.mainWindow.Update();
		this.subWindow.Update();
	}
}
