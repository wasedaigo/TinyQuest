
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Core;
using TinyQuest.Model;
using TinyQuest.Object;

public class BaseStageController : MonoBehaviour {
	
	private Roga2dNode root;
	private Roga2dAnimationPlayer animationPlayer;
	public  BaloonMessageBox baloonMessageBox;
		
	public Roga2dAnimationPlayer AnimationPlayer {
		get {return this.animationPlayer;}	
	}
	
	private Stage stage;
	public Stage Stage {
		get {return stage;}	
	}
	
	protected Ally spawnBattler (string name, Ally.State state, float x, float y) {
		Ally battler = new Ally(name, state);
		battler.Sprite.LocalPriority = 0.1f;
		battler.LocalPixelPosition = new Vector2(x, y);
		
		return battler;
	}
	
	protected Monster spawnMonster (string name, float x, float y) {
		Monster battler = new Monster(name);
		battler.LocalPriority = 0.45f;
		battler.LocalPixelPosition = new Vector2(x, y);
		battler.UpdatePriority();
		return battler;
	}
	
	// Use this for initialization
	protected virtual void Start() {
		Shader.WarmupAllShaders() ;
		this.animationPlayer = new Roga2dAnimationPlayer();
		this.stage = this.GetComponent<Stage>();

		BaloonMessageBox box = (BaloonMessageBox)Instantiate(baloonMessageBox, new Vector3 (0, 0, 0), Quaternion.identity);
		box.transform.parent = this.stage.gameObject.transform;
		box.transform.localScale = new Vector3(0.003f, 0.003f, 1);
		box.transform.localPosition = new Vector3(0.75f, 0, 0);
		box.ArrowFaceRight = true;
		box.Width = 256;
		box.Height = 64;
		box.Message = "Hi, Is this your first time playing this game?";
	}

	protected virtual void Update () {
		this.animationPlayer.Update(Time.deltaTime);
		Roga2dIntervalPlayer.GetInstance().Update();
	}
	
	protected void OnDestroy() {
		Roga2dResourceManager.freeResources();
	}
}