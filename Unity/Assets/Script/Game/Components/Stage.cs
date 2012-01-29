using UnityEngine;
namespace TinyQuest.Component {
	public class Stage : Roga2dNode {
		private Roga2dRenderObject renderObject;
		
		public Stage() 
		{
			Roga2dRenderObject renderObject = new Roga2dRenderObject(Roga2dResourceManager.getTexture("bg/bg_test"), new Vector2(320, 640), new Vector2(0, 80), new Rect(0, 0, 320, 640));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			this.AddChild(sprite);
		}
	}
}