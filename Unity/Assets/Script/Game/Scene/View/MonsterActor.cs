using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Object {
	public class MonsterActor : Actor {
		public MonsterActor(string name)
		{
			string textureId = "Monsters/" + name;
			Texture texture = Roga2dResourceManager.getTexture(textureId);
			
			this.SetSprite(textureId, new Vector2(texture.width, texture.height), new Vector2(0, 0), new Rect(0, 0, texture.width, texture.height));
		}
	}
}