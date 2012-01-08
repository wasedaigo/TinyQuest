using UnityEngine;
using System.Collections.Generic;

public class Roga2dSourceInterval : Roga2dBaseInterval {
	private Roga2dSprite sprite;
	private Roga2dRoot root;
	private int frameDuration;
	private string lastAnimationId;
	private List<Roga2dAnimationKeyFrame> keyFrames;
	private Roga2dBaseInterval interval;
	protected float duration;
	protected int frameNo;
	private int index;
	private List<Roga2dRenderObject> renderObjects;

	public Roga2dSourceInterval(Roga2dSprite sprite, List<Roga2dAnimationKeyFrame> keyFrames, Roga2dRoot root)
	{
		this.sprite = sprite;
		this.frameDuration = 0;
		this.root = root;
		this.lastAnimationId = "";
		this.keyFrames = keyFrames;
		this.renderObjects = new List<Roga2dRenderObject>(this.keyFrames.Count);
        foreach (Roga2dAnimationKeyFrame keyFrame in this.keyFrames) {
			if (keyFrame.Id != "" && keyFrame.Type == Roga2dAnimationKeyFrameType.Image) {
				Texture texture = Resources.Load("Images/" + keyFrame.Id) as Texture;
				Roga2dRenderObject renderObject = new Roga2dRenderObject(texture, keyFrame.PixelSize, keyFrame.PixelCenter, keyFrame.Rect);
				renderObjects.Add(renderObject);
			} else {
				renderObjects.Add(null);
			}
			this.duration += keyFrame.Duration;  
        }
	}
	
	public override bool IsDone() {
        return this.frameNo >= this.duration;
    }
	
    public override void Reset() {
        this.frameNo = 0;
        this.index = 0;
        this.frameDuration = 0;
        if (this.interval != null) {
           this.interval.Reset();
        }
    }

    public override void Start() {
        this.UpdateKeyframe(0, true);
    }
	
    public override void Finish() {
		this.ClearSetting();
    }
	
	private void ClearSetting() {
		if (this.interval != null) {
			this.sprite.RemoveAllChildren();
			this.interval = null;
		}
		this.sprite.RenderObject = null;
	}
	
	private void EmitAnimation(Roga2dAnimationKeyFrame keyFrame) {
        // Emit the new animation (emitted animation won't be controled by this instance anymore)
        Roga2dAnimation animation = Roga2dUtils.LoadAnimation(keyFrame.Id, false, this.sprite.Alpha, this.sprite.Priority, this.root);

        // Apply emit velocity
        if (keyFrame.MaxEmitSpeed > 0) {
            float speed = keyFrame.MinEmitSpeed + (keyFrame.MaxEmitSpeed - keyFrame.MinEmitSpeed) * Random.value;
            float angle = keyFrame.MinEmitAngle + (keyFrame.MaxEmitAngle - keyFrame.MinEmitAngle) * Random.value;
            float rad = (angle / 180) * Mathf.PI;
			this.sprite.Velocity = new Vector2(speed * Mathf.Cos(rad), speed * Mathf.Sin(rad));
        }
		
		if (this.root.Player == null) {
			Debug.LogError("No AnimationPlayer is defined in root");	
		}
		this.root.Player.Play(this.sprite.GameObject.transform, animation);
	}
	
	private void UpdateKeyframe(int index, bool isStart) {
		
		Roga2dAnimationKeyFrame keyFrame = this.keyFrames[index];
        if (this.lastAnimationId != keyFrame.Id) {
            this.ClearSetting();
			this.lastAnimationId = keyFrame.Id;
        }
		
        this.sprite.LocalPriority = keyFrame.Priority;
        this.sprite.BlendType = keyFrame.BlendType;

		if (keyFrame.Type == Roga2dAnimationKeyFrameType.Image) {
			if (this.renderObjects[index] == null) {
				Debug.LogError("Null RenderObject");	
			}
			this.sprite.RenderObject = this.renderObjects[index];
			
		} else if (keyFrame.Type == Roga2dAnimationKeyFrameType.Animation) {
			
            // Display nested animations
            if (this.interval != null) {
               this.interval.Update();
            } else {
				// Not going to emit anything when updatekeyframe called via Start()
                if (keyFrame.Emitter) {
					if (!isStart) {
						EmitAnimation(keyFrame);
					}
                } else {
                    // No animation node is generaetd yet, let's generate it
                    // If no ID exists, ignore it (Which usually means an empty keyframe)
                    if (keyFrame.Id != "") {
						Roga2dAnimation animation = Roga2dUtils.LoadAnimation(keyFrame.Id, true, 1.0f, 0.5f, this.root);
						this.sprite.AddChild(animation.Node);
						this.interval = animation.Interval;
                        this.interval.Start();
                    }
                }
            }
        }
		

    }
	
    public override void Update() {
        if (this.IsDone()) {
            this.ClearSetting();
        } else {
            this.frameDuration += 1;
            this.frameNo += 1;
        
            this.UpdateKeyframe(this.index, false);
			Roga2dAnimationKeyFrame keyFrame = this.keyFrames[index];
			if (this.frameDuration >= keyFrame.Duration) {
	            this.index += 1;
	            this.frameDuration = 0;
	        }
        }
    }
}