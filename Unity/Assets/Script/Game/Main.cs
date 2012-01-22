using UnityEngine;
using System.Collections;
using TinyQuest.Component;

public class Main : MonoBehaviour {
	public GameObject roga2dRoot;
	private Roga2dRoot root;
	private Roga2dAnimationPlayer animationPlayer;
	private Player player;
	
	// Use this for initialization
	void Start () {
	
		this.animationPlayer = new Roga2dAnimationPlayer();
		this.root = new Roga2dRoot(this.animationPlayer);
		this.root.LocalScale = new Vector2(2.0f, 2.0f);

		Roga2dGameObjectState state = Roga2dUtils.stashState(this.root.GameObject);
		this.root.GameObject.transform.parent = roga2dRoot.transform;
		Roga2dUtils.applyState(this.root.GameObject, state);
		
		this.player = new Player("lilia");
		this.player.LocalPriority = 0.1f;
		this.root.AddChild(player);
		
		Stage stage = new Stage();
		stage.LocalPriority = 0.0f;
		
		this.root.AddChild(stage);
	}
	
	// Update is called once per frame
	private static bool isPressed = false;
	void Update () {
		this.player.Update();
		this.root.Update();
		if (Input.GetMouseButtonUp(0)) {
			isPressed = false;
		}
		if (Input.GetMouseButtonDown(0)) {
			isPressed = true;
		}
		if (isPressed) {
			Vector2 position = this.player.LocalPixelPosition;
			this.player.LocalPixelPosition = new Vector2(position.x - 1, position.y);
		}
	}
}
