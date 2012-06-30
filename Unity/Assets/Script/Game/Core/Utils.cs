using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyQuest.Core {
	public class Utils {	
		public static List<uint> getDigits(uint value) {
			
			List<uint> result = new List<uint>();
			uint q;
			uint r;
			uint temp = value;
			while (true) {
				q = (uint)Mathf.FloorToInt(temp / 10);
				r = temp - q * 10;
				result.Insert(0, r);
				if (q == 0) {
					break;	
				}
				temp = (temp - r) / 10;
			}
			
			return result;	
		}
		
		public static ActorHealthState GetHealthState(float ratio) {
			if (ratio == 1) {
				return ActorHealthState.Full;
			}
			
			if (ratio == 0) {
				return ActorHealthState.Dead;
			} 
			
			if (ratio <= 0.25f) {
				return ActorHealthState.Dying;
			}
			
			return ActorHealthState.Ok;	
		}
	}
}
