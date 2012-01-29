using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Component {
	public class Monster : Entity {
		public Monster(string name) 
		: base("Monsters/" + name, new Vector2(96, 96), new Vector2(0, 0), new Rect(0, 0, 96, 96))
		{
		}
	}
}