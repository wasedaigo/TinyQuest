using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Entity {
	public class Monster : BaseObject {
		public Monster(string name) 
		: base("Monsters/" + name, new Vector2(96, 96), new Vector2(0, 0), new Rect(0, 0, 96, 96))
		{
		}
	}
}