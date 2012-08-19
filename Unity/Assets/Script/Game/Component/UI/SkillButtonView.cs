using UnityEngine;

using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Object;

public class SkillButtonView : MonoBehaviour {
	public UILabel nameLabel;
	public GameObject lifeBarImage;
	public GameObject faceIcon;
	public GameObject infoPanel;
	public UISprite background;
	
	private Roga2dNode faceIconNode;
	private bool initialized = false;
	private UIImageButton button;
	private int life;
	
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
		actor.Transform.localPosition = new Vector3(-15, 1.5f, -0.1f);
		actor.Transform.localEulerAngles = new Vector3(0, 0, 180);
		actor.Transform.localScale = new Vector3(-24, 24, 0);
		
		Utils.SetLayerRecursively(actor.Transform, 5);
		this.faceIconNode = actor;
	}

	public void SetName(string name) {
		nameLabel.text = name;
	}
	
	public void SetTouchEnabled(bool enabled) {
		if (IsDead()) {return;}
		collider.enabled = enabled;
	}
	
	private bool IsDead() {
		return this.life <= 0;	
	}
	
	public void SetLife(int life, int maxLife) {
		this.life = life;
		lifeBarImage.transform.localScale = new Vector3(life / (float)maxLife, 1, 1);
	}
	
	public void UpdateStatus(CombatUnit combatUnit) {
		this.SetLife(combatUnit.hp, combatUnit.GetUserUnit().MaxHP);
		if (!initialized) {
			this.SetFaceIcon(combatUnit.GetUserUnit().Unit.id);
			this.initialized = true;
		}
	}
}