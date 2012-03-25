using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Component {
	public class Stage : Roga2dNode {
		public delegate void StageScrollFinishEventHandler();
		public event StageScrollFinishEventHandler ScrollFinished;
		
		private Roga2dNode root;
		private List<Roga2dSprite> sprites;
		private Roga2dRenderObject renderObject;
		
		public Stage() 
		{
			this.root = new Roga2dNode();
			this.AddChild(this.root);
			this.sprites = new List<Roga2dSprite>();
			
			this.setupStage();
		}
		
		private void setupStage() {
			for (int i = 0; i < 3; i++) {
				Roga2dSprite sprite = new Roga2dSprite("bg/001", new Vector2(160, 140), new Vector2(0, 0), new Rect(0, 0, 160, 140));
				this.root.AddChild(sprite);	
				sprite.LocalPixelPosition = new Vector2(-i * 160, 0);
				this.sprites.Add(sprite);
			}
		}
		
		public void Scroll(float duration) {
			Roga2dPositionInterval posInterval = new Roga2dPositionInterval(this, new Vector2(0, 0), Roga2dUtils.pixelToLocal(new Vector2(320, 0)), Roga2dUtils.TimeToFrame(duration), true, null);
			Roga2dFunc func = new Roga2dFunc(this.onScrolled);
			
			Roga2dBaseInterval interval = new Roga2dSequence(new List<Roga2dBaseInterval> {posInterval, func});
			Roga2dIntervalPlayer.GetInstance().Play(interval);
		}
		
		private void onScrolled() {
			if (this.ScrollFinished != null) {
				this.ScrollFinished();	
			}
			//this.LocalPixelPosition = new Vector2(0, 0);
		}
	}
}