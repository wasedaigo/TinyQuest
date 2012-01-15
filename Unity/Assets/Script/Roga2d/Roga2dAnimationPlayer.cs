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
	
	public void Play(Roga2dNode root, Transform spawnTransform, Roga2dAnimation animation) {
		
		if (root != null) {
			root.AddChild(animation.Node);
		}

		if (spawnTransform != null) {
			animation.Node.GameObject.transform.position = spawnTransform.position;
			animation.Node.GameObject.transform.rotation = spawnTransform.rotation;
		}
        this.animations.Add(animation);
	}

	static int counter = 0;
	public void Update() {
		counter += 1;
		//if (counter < 2) {return;}
		counter = 0;
        for (int i = this.animations.Count - 1; i >= 0; i-- ) {
            Roga2dAnimation animation = this.animations[i];
			if (animation.IsStarted) {
	            if (animation.Interval.IsDone()) {
					Object.Destroy(animation.Node.GameObject);
					this.animations.RemoveAt(i);
	            } else {
	            	animation.Interval.Update();
				}
			} else {
				animation.Interval.Start();
				animation.IsStarted = true;
			}
			animation.Node.Update();
        }
	}
}