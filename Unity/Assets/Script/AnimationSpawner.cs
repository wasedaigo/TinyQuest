using UnityEngine;
using System.Collections;

public class AnimationSpawner : MonoBehaviour {
	public GameObject roga2dRoot;
	private Roga2dRoot root;
	private Roga2dAnimationPlayer player;
	private Roga2dSprite battler;
	
	void Awake () {
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
		this.player = new Roga2dAnimationPlayer();
		this.root = new Roga2dRoot(this.player);
		this.root.LocalScale = new Vector2(2.0f, 2.0f);
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.root.GameObject);
		this.root.GameObject.transform.parent = roga2dRoot.transform;
		Roga2dUtils.applyState(this.root.GameObject, state);
		{
			Roga2dRenderObject renderObject = new Roga2dRenderObject(Roga2dResourceManager.getTexture("bg/bg0001"), new Vector2(240, 160), new Vector2(0, 0), new Rect(0, 0, 128, 64));

			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			sprite.LocalPriority = 0.05f;
			this.root.AddChild(sprite);
		}
		
		{
			Roga2dRenderObject renderObject = new Roga2dRenderObject(Roga2dResourceManager.getTexture("Battle/Skills/Battler_Base"), new Vector2(32, 32), new Vector2(0, 0), new Rect(128, 0, 32, 32));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			sprite.LocalPriority = 0.1f;
			sprite.Update();
			this.root.AddChild(sprite);
			this.battler = sprite;
		}
		
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
		this.battler.Show();
    }

	void Update () {
		this.player.Update();
		this.root.Update();
		if (Input.GetMouseButtonDown(0)) {
			this.battler.Hide();
			if (!player.HasPlayingAnimations()) {
				Roga2dAnimation animation = Roga2dUtils.LoadAnimation(ids[no], false, 1.0f, 0.5f, this.root);
				this.player.Play(this.root, null, animation,  AnimationFinished);
				no +=1;
				if (no >= ids.Length) {
					no = 0;	
				}
			}
		}
	}
}
