using UnityEngine;
using System.Collections;
using LitJson;

public class AnimationSpawner : MonoBehaviour {
	public Transform planePrefab;
	public Roga2dAnimationPlayer player;
	public GameObject target;
	Roga2dRoot root ;
	
	void Awake () {
		Application.targetFrameRate = 10;
	}
	
	// Use this for initialization
	void Start () {
		
		/*
    Texture texture = Resources.Load("chara") as Texture;
		
	
	Roga2dRenderObject renderObject = new Roga2dRenderObject(texture, new Vector2(64, 64), new Vector2(32, 32), new Rect(0, 0, 1, 1));
	Roga2dSprite sprite = new Roga2dSprite(renderObject);
	sprite.LocalAlpha = 0.5f;
	sprite.Update();
		
	renderObject = new Roga2dRenderObject(texture, new Vector2(128, 128), new Vector2(128, 128), new Rect(0, 0, 1, 1));
	new Roga2dSprite(renderObject);
		
	renderObject = new Roga2dRenderObject(texture, new Vector2(64, 64), new Vector2(0, 0), new Rect(0, 0, 1, 1));
	sprite.RenderObject = renderObject;
	sprite.Update();*/
		this.root = new Roga2dRoot(player);
		this.root.Target = new Roga2dNode(this.target);
		
	}

	static bool done = false;
	// Update is called once per frame
	void Update () {
		if (!player.HasPlayingAnimations()) {
			//Resources.UnloadUnusedAssets();
			//if(done){return;}
			//string id = "Battle/Skills/Axe/Bash";
			//string id = "Battle/Skills/Effect/Stab01";
			//string id = "Battle/Skills/Sword/LeaveDance";
			//string id = "Battle/Skills/Effect/PlaneCutTest";
			//string id = "Battle/Skills/Effect/PlaneCutBottom";
			//string id = "Battle/Skills/Battler/SwordDash";
			//string id = "Battle/Skills/Battler/SwordUpper";
			string id = "Battle/Skills/Laser/Skill_Laser01";
			//string id = "Battle/Skills/Spear/SpearAirraid";
			//string id = "Battle/Skills/Bow/Shoot";
			//string id = "Battle/Skills/Axe/CycloneAxe";
			//string id = "Battle/Skills/Axe/SpinAxeY";
			//string id = "Battle/Skills/Axe/Slash";
			//string id = "Battle/Skills/Axe/ArmorBreaker";
			//string id = "Battle/Skills/Battler/Jump";
			//string id = "Battle/Skills/Battler/Back";
			//string id = "Battle/Skills/Battler/RollBack";
			//string id = "Battle/Skills/Fire/Skill_Flare";
			//string id = "Battle/Skills/Fire/FlareEffect";
			//string id = "Battle/Skills/Fire/FlareParticle";
			//string id = "Battle/Skills/Common/MagicCasting";
			Roga2dAnimation animation = Roga2dUtils.LoadAnimation(id, false, 1.0f, 0.5f, this.root);
			player.Play(player.transform, animation);
			done = true;
		}
	}
}
