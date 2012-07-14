using UnityEngine;
using System.Collections;

public class BaloonMessageBox : MonoBehaviour {
	
	public UIAtlas BaloonAtlas;
	public UIAtlas FontAtlas;
	public int Width;
	public int Height;
	public string Message;
	public bool ArrowFaceRight;
	public int FontSize;
	
	private const int LabelMarginX = 16;
	private const int LabelMarginY = 16;
	// Use this for initialization
	void Start () {
		UISprite baloonSprite = NGUITools.AddSprite(this.gameObject, this.BaloonAtlas, "baloon_box");
		baloonSprite.pivot = UIWidget.Pivot.Bottom;
		baloonSprite.MakePixelPerfect();
		baloonSprite.transform.localScale = new Vector3(this.Width, this.Height, 1);
		baloonSprite.depth = 0;
		
		UISprite arrowSprite = NGUITools.AddSprite(this.gameObject, this.BaloonAtlas, "baloon_arrow_left");
		arrowSprite.MakePixelPerfect();
		
		arrowSprite.pivot = UIWidget.Pivot.Top;
		if (this.ArrowFaceRight) {
			Vector3 scale = arrowSprite.transform.localScale;
			arrowSprite.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);	
			arrowSprite.transform.localPosition = new Vector3(32, 0, 0);
		} else {
			arrowSprite.transform.localPosition = new Vector3(-32, 0, 0);
		}
		arrowSprite.depth = 0;
		
		UILabel label = NGUITools.AddWidget<UILabel>(this.gameObject);
		label.font = this.FontAtlas.GetComponent<UIFont>();
		label.text = this.Message;
		label.color = new Color(0, 0, 0, 1);
		label.transform.localScale = new Vector3(FontSize, FontSize, 1);
		label.lineWidth = this.Width;
		label.pivot = UIWidget.Pivot.Center;
		label.depth = 2;
		
		Vector3 pos = label.transform.localPosition;
		label.transform.localPosition = new Vector3(pos.x, pos.y + this.Height / 2, pos.z - 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
