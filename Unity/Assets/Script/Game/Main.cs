using UnityEngine;
using System.Collections;
using TinyQuest.Component;
using TinyQuest.Model;

public class Main : MonoBehaviour {
	public Camera MainScreenCamera;
	public GameObject MainScreen;
	public UIAtlas atlas;
	private Roga2dNode mainScreen;

	void Awake () {
		Application.targetFrameRate = 60;
	}
	
	// Use this for initialization
	void Start() {
		this.mainScreen = new MainScreen();
		this.mainScreen.LocalScale = new Vector2(1.25f, 1.25f);
		Roga2dGameObjectState state = Roga2dUtils.stashState(mainScreen.Transform);
		this.mainScreen.Transform.parent = this.MainScreen.transform;
		Roga2dUtils.applyState(this.mainScreen.Transform, state);
	}
	
	void Update(){
		this.mainScreen.Update();
	}
}
