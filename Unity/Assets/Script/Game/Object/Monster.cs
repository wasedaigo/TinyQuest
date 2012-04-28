using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Object {
	public class Monster : AdventureObject {
		public Monster(string name) 
		: base("Monsters/" + name, new Vector2(64, 64), new Vector2(0, 0), new Rect(0, 0, 64, 64))
		{
		}
	}
}