using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyQuest.Core {
	public class EffectBuilder {
		private static EffectBuilder instance;
		
		public static EffectBuilder GetInstance()
		{
			if (instance == null) {
				instance = new EffectBuilder();
			}
			return instance;
		}
		
		// Build animation for damage pop
		public Roga2dAnimation BuildDamagePopAnimation(Vector2 position, uint value) {
			Roga2dNode node = new Roga2dNode("Damage");
			List<uint> digits = Utils.getDigits(value);
			position.x -= (10 * digits.Count) / 2;
			node.LocalPixelPosition = position;
			node.LocalPriority = 1.0f;
	
			List<Roga2dBaseInterval> popIntervals = new List<Roga2dBaseInterval>();
			for (int i = 0; i < digits.Count; i++) {
				uint no = digits[i];
				// Get Font Image
				Roga2dSprite sprite = new Roga2dSprite("Font/number_font", new Vector2(7, 10), new Vector2(0, 0), new Rect(no * 7, 0, 7, 10));
				// X-coordinate each digit pop
				int popX = i * 10;
				// Init state
				sprite.LocalAlpha = 0.0f;
				sprite.LocalPosition = Roga2dUtils.pixelToLocal(new Vector2(popX, 0));
				// Add to parent
				node.AddChild(sprite);
	
				Roga2dBaseInterval interval = new Roga2dSequence(
					new List<Roga2dBaseInterval>() {
						new Roga2dWait(i * 2),
						new Roga2dAlphaInterval(sprite, 1.0f, 1.0f, 1, false),
						new Roga2dPositionInterval(sprite, Roga2dUtils.pixelToLocal(new Vector2(popX, 0)), Roga2dUtils.pixelToLocal(new Vector2(popX, -50)), 7, true, null)
						// TODO: destroy damage effects
					}
				);
				
				popIntervals.Add(interval);
			}
			
			Roga2dBaseInterval resultInterval = new Roga2dSequence(new List<Roga2dBaseInterval>() {
				new Roga2dParallel(popIntervals),
				new Roga2dWait(5)
			});
	
			return Roga2dAnimation.Build(node, resultInterval);	
		}
		
		// Build interval for red-flash
		public Roga2dBaseInterval BuildDamageInterval(Roga2dNode target) {
			List<Roga2dBaseInterval> list = new List<Roga2dBaseInterval>();
			list.Add(new Roga2dHueInterval(target, new Roga2dHue(0, 0, 0), new Roga2dHue(100, -100, -100), 5, true));
			list.Add(new Roga2dHueInterval(target, new Roga2dHue(100, -100, -100), new Roga2dHue(0, 0, 0), 5, true));
			return new Roga2dSequence(list);
		}
	}
}