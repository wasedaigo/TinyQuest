using UnityEngine;
using System.Collections;

public class AnimationSpawner : MonoBehaviour {
	public GameObject roga2dRoot;
	private Roga2dRoot root;
	private Roga2dAnimationPlayer player;
	
	void Awake () {
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
		this.player = new Roga2dAnimationPlayer();
		this.root = new Roga2dRoot(this.player);
		
		Roga2dGameObjectState state = Roga2dUtils.stashState(this.root.GameObject);
		this.root.GameObject.transform.parent = roga2dRoot.transform;
		Roga2dUtils.applyState(this.root.GameObject, state);

		{
			Roga2dRenderObject renderObject = new Roga2dRenderObject(Roga2dResourceManager.getTexture("bg/bg0001"), new Vector2(192, 256), new Vector2(0, 0), new Rect(0, 0, 128, 64));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			this.root.AddChild(sprite);
		}
		
		{
			Roga2dRenderObject renderObject = new Roga2dRenderObject(Roga2dResourceManager.getTexture("death_wind"), new Vector2(96, 96), new Vector2(0, 0), new Rect(0, 0, 96, 96));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			sprite.GameObject.transform.position = new Vector3(0.0f, -2.0f, 0.45f);
			this.root.AddChild(sprite);
		
			Roga2dNode target = sprite;
			this.root.Target = target;
			
			Roga2dNode origin = new Roga2dNode("TargetOrigin");
			this.root.AddChild(origin);
			origin.LocalPosition = target.LocalPosition;
			this.root.TargetOrigin = origin;
			;
		}
	}
	
	static string[] ids = new string[] {
			"Battle/Skills/Axe/Bash",
			"Battle/Skills/Sword/LeaveDance",
			"Battle/Skills/Battler/TestSwordUpper",
			"Battle/Skills/Laser/Skill_Laser01",
			"Battle/Skills/Spear/SpearAirraid",
			"Battle/Skills/Bow/Shoot",
			"Battle/Skills/Bow/bow_bomb",
			//"Battle/Skills/Axe/CycloneAxe",
			"Battle/Skills/Axe/Slash",
			"Battle/Skills/Axe/ArmorBreaker",
			"Battle/Skills/Fire/Skill_Flare",
			"Battle/Skills/Common/MagicCasting"
	};
	static int no = 0;
	static bool done = false;
	// Update is called once per frame
	void Update () {
		
		this.player.Update();
		if (!player.HasPlayingAnimations()) {
			//Resources.UnloadUnusedAssets();
			//if(done){return;}
			Roga2dAnimation animation = Roga2dUtils.LoadAnimation(ids[no], false, 1.0f, 0.5f, this.root);
			this.player.Play(this.root, null, animation);
			done = true;
			no +=1;
			if (no >= ids.Length) {
				no = 0;	
			}
		}
	}
}
