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
		public Roga2dAnimation BuildDamagePopAnimation(Vector2 position, int value) {
			Roga2dNode node = new Roga2dNode("Damage");
			List<uint> digits = Utils.getDigits((uint)value);
			position.x -= (10 * digits.Count) / 2;
			node.LocalPixelPosition = position;
			node.LocalPriority = 2.0f;
	
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
						new Roga2dWait(i * 0.05f),
						new Roga2dAlphaInterval(sprite, 1.0f, 1.0f, 0.02f, Roga2dTweenType.Fix),
						new Roga2dPositionInterval(sprite, Roga2dUtils.pixelToLocal(new Vector2(popX, 0)), Roga2dUtils.pixelToLocal(new Vector2(popX, -15)), 0.05f, Roga2dTweenType.Linear, null)
					}
				);
				
				popIntervals.Add(interval);
			}
			
			Roga2dBaseInterval resultInterval = new Roga2dSequence(new List<Roga2dBaseInterval>() {
				new Roga2dParallel(popIntervals),
				new Roga2dWait(0.1f)
			});
	
			return Roga2dAnimation.Build(node, resultInterval);	
		}
		
		// Build interval for red-flash
		public Roga2dBaseInterval BuildDamageInterval(Roga2dNode target) {
			List<Roga2dBaseInterval> flashList = new List<Roga2dBaseInterval>();
			flashList.Add(new Roga2dHueInterval(target, new Roga2dHue(0, 0, 0), new Roga2dHue(100, -100, -100), 0.075f, Roga2dTweenType.Linear));
			flashList.Add(new Roga2dHueInterval(target, new Roga2dHue(100, -100, -100), new Roga2dHue(0, 0, 0), 0.075f, Roga2dTweenType.Linear));
			
			List<Roga2dBaseInterval> shakeList = new List<Roga2dBaseInterval>();
			Vector2 pos = target.LocalPosition;
			Vector2 pixelPos = target.LocalPixelPosition;
			for (int i = 0; i < 6; i++) {
				int dx = (2 * (i % 2) - 1) * 2;
				int dy = 0;
				shakeList.Add(new Roga2dPositionInterval(target, pos, Roga2dUtils.pixelToLocal(new Vector2(pixelPos.x + dx, pixelPos.y + dy)), 0.03f, Roga2dTweenType.Linear, null));
			}
			shakeList.Add(new Roga2dPositionInterval(target, pos, pos, 0.03f, Roga2dTweenType.Linear, null));
			
			return new Roga2dParallel(new List<Roga2dBaseInterval>(){new Roga2dSequence(flashList), new Roga2dSequence(shakeList)});
		}
		
		// Build interval for whilte-flash
		public Roga2dBaseInterval BuildAttackFlashInterval(Roga2dNode target) {
			List<Roga2dBaseInterval> list = new List<Roga2dBaseInterval>();
			list.Add(new Roga2dHueInterval(target, new Roga2dHue(0, 0, 0), new Roga2dHue(255, 255, 255), 0.015f, Roga2dTweenType.Linear));
			list.Add(new Roga2dHueInterval(target, new Roga2dHue(255, 255, 255), new Roga2dHue(0, 0, 0), 0.015f, Roga2dTweenType.Linear));
			list.Add(new Roga2dWait(0.1f));
			list.Add(new Roga2dHueInterval(target, new Roga2dHue(0, 0, 0), new Roga2dHue(255, 255, 255), 0.015f, Roga2dTweenType.Linear));
			list.Add(new Roga2dHueInterval(target, new Roga2dHue(255, 255, 255), new Roga2dHue(0, 0, 0), 0.015f, Roga2dTweenType.Linear));
			list.Add(new Roga2dWait(0.1f));
			return new Roga2dSequence(list);
		}
	}
}