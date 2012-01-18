using UnityEngine;
namespace TinyQuest.Component {
	public class Player : Roga2dNode {
		private Roga2dRenderObject renderObject;
		
		public Player() 
		{
			Roga2dRenderObject renderObject = new Roga2dRenderObject(Roga2dResourceManager.getTexture("Battle/Skills/Battler_Base"), new Vector2(32, 32), new Vector2(0, 0), new Rect(128, 0, 32, 32));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			this.AddChild(sprite);
		}
	}
}