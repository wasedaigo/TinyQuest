using UnityEngine;
namespace TinyQuest.Component {
	public class Stage : Roga2dNode {
		private Roga2dRenderObject renderObject;
		
		public Stage() 
		{
			Roga2dRenderObject renderObject = new Roga2dRenderObject("bg/001", new Vector2(160, 140), new Vector2(0, 0), new Rect(0, 0, 160, 140));
			Roga2dSprite sprite = new Roga2dSprite("Stage", renderObject);
			this.AddChild(sprite);
		}
	}
}