using UnityEngine;
using System.Collections.Generic;
public class Roga2dAnimationPlayer {
	private List<Roga2dAnimation> animations;
	public Roga2dAnimationPlayer()
	{
		this.animations = new List<Roga2dAnimation>();
	}
	
	public bool HasPlayingAnimations() {
		return animations.Count > 0;
	}
	
	public void Play(Roga2dNode root, Transform spawnTransform, Roga2dAnimation animation, Roga2dAnimationFinishCallback finishCallback) {
		if (root != null) {
			root.AddChild(animation.Node);
		}
		if (spawnTransform != null) {
			animation.Node.Transform.position = spawnTransform.position;
			animation.Node.Transform.rotation = spawnTransform.rotation;
			//animation.Node.Transform.localScale = spawnTransform.localScale;
		}
		animation.finishCallback = finishCallback;
        this.animations.Add(animation);
	}

	public void Update(float delta) {
		//delta = Roga2dUtils.limitAnimationDelta(delta);
        for (int i = this.animations.Count - 1; i >= 0; i-- ) {
            Roga2dAnimation animation = this.animations[i];
			if (animation.IsStarted) {
				animation.Interval.Update(delta);
	            if (animation.Interval.IsDone()) {
					animation.Node.Destroy();
					if (animation.Node.Parent != null) {
						animation.Node.Parent.RemoveChild(animation.Node);
					}
					this.animations.RemoveAt(i);
					if (animation.finishCallback != null) {
						animation.finishCallback(animation);
					}
	            }
			} else {
				animation.Interval.Start();
				animation.IsStarted = true;
			}
			animation.Node.Update();
        }
	}
}