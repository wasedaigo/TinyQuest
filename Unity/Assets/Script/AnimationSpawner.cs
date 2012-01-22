using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Component;

public class AnimationSpawner : MonoBehaviour {
	public GameObject roga2dRoot;
	private Roga2dRoot root;
	private Roga2dAnimationPlayer player;

	private List<Player> battlers = new List<Player>();
	
	void Awake () {
		Application.targetFrameRate = 60;
	}
	
	Player spawnBattler (string name, float x, float y) {
		Player battler = new Player(name);
		battler.Sprite.LocalPriority = 0.1f;
		battler.LocalPixelPosition = new Vector2(x, y);
		this.battlers.Add(battler);
		return battler;
	}
	
	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		this.player = new Roga2dAnimationPlayer();
		this.root = new Roga2dRoot(this.player);
		this.root.LocalScale = new Vector2(2.0f, 2.0f);
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.root.GameObject);
		this.root.GameObject.transform.parent = roga2dRoot.transform;
		Roga2dUtils.applyState(this.root.GameObject, state);

		// Player
		
		this.root.AddChild(spawnBattler("pierre", 40, -20));
		this.root.AddChild(spawnBattler("amon", 40, 20));
		this.root.AddChild(spawnBattler("draco", 80, -40));
		this.root.AddChild(spawnBattler("lilia", 80, 0));
		this.root.AddChild(spawnBattler("hose", 80, 40));
		
		// Stage
		Stage stage = new Stage();
		stage.LocalPriority = 0.0f;
		this.root.AddChild(stage);
		
		{
			Roga2dRenderObject renderObject = new Roga2dRenderObject(Roga2dResourceManager.getTexture("death_wind"), new Vector2(96, 96), new Vector2(0, 0), new Rect(0, 0, 96, 96));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			sprite.GameObject.transform.position = new Vector3(0.0f, -2.0f, 0.45f);
			this.root.AddChild(sprite);
		
			Roga2dNode target = sprite;
			this.root.Target = target;
		}

	}
	
	static string[] ids = new string[] {
			"Battle/Skills/Sword/LeaveDance",
			"Battle/Skills/Spear/SpearAirraid",
			"Battle/Skills/Axe/Bash",
			
			"Battle/Skills/Laser/Skill_Laser01",
			"Battle/Skills/Bow/Shoot",
			"Battle/Skills/Bow/bow_bomb",
			"Battle/Skills/Axe/CycloneAxe",
			"Battle/Skills/Axe/Slash",
			"Battle/Skills/Axe/ArmorBreaker",
			"Battle/Skills/Fire/Skill_Flare",
			"Battle/Skills/Common/MagicCasting"
	};
	
	static int no = 0;
    void AnimationFinished(Roga2dAnimation animation)
    {
		foreach (Player battler in this.battlers) {
			battler.Sprite.Show();
		}
    }

	void Update () {
		this.player.Update();
		this.root.Update();
		if (Input.GetMouseButtonDown(0)) {
			if (!player.HasPlayingAnimations()) {
				Player battler = this.battlers[no % 5];
				battler.Sprite.Hide();
				Dictionary<string, string> options = new Dictionary<string, string>();
				options["Battle/Skills/Battler_Base"] = battler.TextureID;
				Roga2dAnimation animation = Roga2dUtils.LoadAnimation(ids[no], false, 1.0f, 0.5f, this.root, options);
				this.player.Play(battler, null, animation,  AnimationFinished);
				no +=1;
				if (no >= ids.Length) {
					no = 0;	
				}
			}
		}
	}
}
