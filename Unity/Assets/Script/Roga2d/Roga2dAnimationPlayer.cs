using UnityEngine;
using System.Collections.Generic;

public class Roga2dAnimationPlayer : MonoBehaviour {
	private List<Roga2dAnimation> animations;
	
	public void Start()
	{
		this.animations = new List<Roga2dAnimation>();
	}
	
	public bool HasPlayingAnimations() {
		return animations.Count > 0;
	}
	
	public void Play(Transform rootTransform, Roga2dAnimation animation) {
		GameObject go = new GameObject("Root");
		animation.Root = go;
		
		Roga2dGameObjectState state = Roga2dUtils.stashState(animation.Node.GameObject);
		animation.Node.GameObject.transform.parent = go.transform;
		Roga2dUtils.applyState(animation.Node.GameObject, state);

		go.transform.position = rootTransform.position;
		go.transform.rotation = rootTransform.rotation;

        this.animations.Add(animation);
		Debug.Log("Add");
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
					Destroy(animation.Node.GameObject);
					Destroy(animation.Root);
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