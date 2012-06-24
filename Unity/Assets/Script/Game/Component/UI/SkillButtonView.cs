using UnityEngine;

using TinyQuest.Data;

public class SkillButtonView : MonoBehaviour {
	public UILabel nameLabel;
	public UILabel lifeLabel;
	public GameObject faceIcon;
		
	private UITexture faceIconTexture;
	private bool initialized = false;
	
	public void SetFaceIcon(int puppetId) {
		string textureId = "UI/Icon/puppet/" + puppetId.ToString();
		
		if (this.faceIconTexture != null) {
			NGUITools.Destroy(this.faceIconTexture);
		}

		UITexture ut = NGUITools.AddWidget<UITexture>(faceIcon);
		Material material = Roga2dResourceManager.getSharedMaterial(textureId, Roga2dBlendType.Unlit);
	    ut.material = material;
		ut.MarkAsChanged();
		ut.MakePixelPerfect();
		ut.transform.localScale = new Vector3(1, 1, 1);
		ut.transform.localPosition = Vector3.zero;
		this.faceIconTexture = ut;
	}
	
	public void SetName(string name) {
		nameLabel.text = name;
	}
	
	public void SetLife(int life) {
		lifeLabel.text = life.ToString();
	}
	
	public void UpdateStatus(CombatUnit combatUnit) {
		this.SetLife(combatUnit.hp);
		if (!initialized) {
			this.SetFaceIcon(combatUnit.GetUserUnit().Unit.id);
			this.SetName("TEST" + combatUnit.GetUserUnit().Unit.id.ToString());
			this.initialized = true;
		}
	}
}