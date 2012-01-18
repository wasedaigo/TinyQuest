using UnityEngine;
namespace TinyQuest.Component {
	public class Stage : Roga2dNode {
		private Roga2dRenderObject renderObject;
		
		public Stage() 
		{
			Roga2dRenderObject renderObject = new Roga2dRenderObject(Roga2dResourceManager.getTexture("bg/bg0001"), new Vector2(240, 160), new Vector2(0, 0), new Rect(0, 0, 128, 64));
	
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			this.AddChild(sprite);
		}
	}
}