using UnityEngine;

using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Object;

public class SkillButtonView : MonoBehaviour {
	public UILabel nameLabel;
	public UILabel lifeLabel;
	public GameObject faceIcon;
	public GameObject infoPanel;
	public UISprite background;
	
	private Roga2dNode faceIconNode;
	private bool initialized = false;
	private UIImageButton button;
	
	public void Start() {
		this.button 	= this.gameObject.GetComponent<UIImageButton>();
	}

	public void SetFaceIcon(int puppetId) {
		
		if (this.faceIconNode != null) {
			this.faceIconNode.Destroy();
			this.faceIconNode = null;
		}
		
		FaceIcon actor = new FaceIcon(puppetId);
		actor.Transform.parent = faceIcon.transform;
		actor.Transform.parent = this.gameObject.transform;
		actor.Transform.localPosition = new Vector3(-15, 1.5f, -0.1f);
		actor.Transform.localEulerAngles = new Vector3(0, 0, 180);
		actor.Transform.localScale = new Vector3(-24, 24, 0);
		
		Utils.SetLayerRecursively(actor.Transform, 5);
		this.faceIconNode = actor;
			
		/*
		UITexture ut = NGUITools.AddWidget<UITexture>(faceIcon);
		Material material = Roga2dResourceManager.getSharedMaterial(textureId, Roga2dBlendType.Unlit);
	    ut.material = material;
		ut.MarkAsChanged();
		ut.MakePixelPerfect();
		ut.transform.localScale = new Vector3(1, 1, 1);
		ut.transform.localPosition = Vector3.zero;
		this.faceIconTexture = ut;
		*/
	}

	public void SetName(string name) {
		nameLabel.text = name;
	}
	
	public void SetLife(int life, int maxLife) {
		float ratio = life / (float)maxLife;
		ActorHealthState state = Utils.GetHealthState(ratio);
		switch (state) {
		case ActorHealthState.Full:
			lifeLabel.color = Color.blue;
			break;
		case ActorHealthState.Ok:
			lifeLabel.color = Color.white;
			break;
		case ActorHealthState.Dying:
			lifeLabel.color = Color.yellow;
			break;
		case ActorHealthState.Dead:
			lifeLabel.color = Color.red;
			break;
		}
		
		
		if (life <= 0) {
			this.button.normalSprite = "papet_btn_dead";
			this.background.spriteName = "papet_btn_dead";
			this.infoPanel.SetActiveRecursively(false);
			collider.enabled = false;
		} else {
			
			if (this.button != null) {
				this.button.normalSprite = "papet_btn";
				this.background.spriteName = "papet_btn";
			}
			this.infoPanel.SetActiveRecursively(true);
			collider.enabled = true;
		}
		lifeLabel.text = life.ToString();
	}
	
	public void UpdateStatus(CombatUnit combatUnit) {
		this.SetLife(combatUnit.hp, combatUnit.GetUserUnit().MaxHP);
		if (!initialized) {
			this.SetFaceIcon(combatUnit.GetUserUnit().Unit.id);
			this.initialized = true;
		}
	}
}