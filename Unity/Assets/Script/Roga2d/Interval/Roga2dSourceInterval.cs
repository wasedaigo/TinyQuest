using UnityEngine;
using System.Collections.Generic;

class Roga2dRenderObjectDesc {
	public Roga2dRenderObject RenderObject;
	public Rect SrcRect;
	public Vector2 PixelSize;
};

public class Roga2dSourceInterval : Roga2dBaseInterval {
	private Roga2dSprite sprite;
	private Roga2dAnimationSettings settings;
	private string lastAnimationId;
	private List<Roga2dAnimationKeyFrame> keyFrames;
	private Roga2dBaseInterval interval;
	private float frameDuration;
	protected float duration;
	protected float elapsed;
	protected int frameNo;
	private int lastUpdateIndex;
	private int lastUpdateFrameNo;
	private int index;
	private List<Roga2dRenderObjectDesc> renderObjectDescs;
	private Dictionary<string, Roga2dSwapTextureDef> options;

	public Roga2dSourceInterval(Roga2dSprite sprite, List<Roga2dAnimationKeyFrame> keyFrames, Roga2dAnimationSettings settings, Dictionary<string, Roga2dSwapTextureDef> options)
	{
		this.lastUpdateIndex = -1;
		this.lastUpdateFrameNo = -1;
		this.options = options;
		this.sprite = sprite;
		this.frameDuration = 0;
		this.frameNo = 0;
		this.index = 0;
		this.settings = settings;
		this.lastAnimationId = "";
		this.keyFrames = keyFrames;
		this.renderObjectDescs = new List<Roga2dRenderObjectDesc>(this.keyFrames.Count);
		
		string lastKeyFrameId = "";
        foreach (Roga2dAnimationKeyFrame keyFrame in this.keyFrames) {
			if (keyFrame.Id != "" && keyFrame.Type == Roga2dAnimationKeyFrameType.Image) {
				
				Roga2dRenderObject renderObject;
				if (lastKeyFrameId == keyFrame.Id) {
					renderObject = renderObjectDescs[renderObjectDescs.Count - 1].RenderObject;
				} else {
					renderObject = new Roga2dRenderObject(keyFrame.Id, keyFrame.PixelSize, keyFrame.PixelCenter, keyFrame.Rect);
				}
				Roga2dRenderObjectDesc desc = new Roga2dRenderObjectDesc();
				desc.RenderObject = renderObject;
				desc.SrcRect = keyFrame.Rect;
				desc.PixelSize = keyFrame.PixelSize;
				renderObjectDescs.Add(desc);
				lastKeyFrameId = keyFrame.Id;
			} else {
				renderObjectDescs.Add(null);
				lastKeyFrameId = "";
			}
			this.duration += keyFrame.Duration;  
        }
	}
	
	public override sealed float ExcessTime() {
		return 0;
	}
	
	public override bool IsDone() {
        return this.elapsed >= this.duration;
    }
	
    public override void Reset() {
        this.elapsed = 0;
        this.index = 0;
        this.frameDuration = 0;
        if (this.interval != null) {
           this.interval.Reset();
        }
    }

    public override void Start() {
        this.UpdateKeyframe(0);
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
		this.sprite.UpdatePriority();
        // Emit the new animation (emitted animation won't be controled by this instance anymore)
        Roga2dAnimation animation = Roga2dUtils.LoadAnimation(keyFrame.Id, false, this.sprite, this.settings, this.options);
        // Apply emit velocity
        if (keyFrame.MaxEmitSpeed > 0) {
            float speed = keyFrame.MinEmitSpeed + (keyFrame.MaxEmitSpeed - keyFrame.MinEmitSpeed) * Random.value;
            float angle = keyFrame.MinEmitAngle + (keyFrame.MaxEmitAngle - keyFrame.MinEmitAngle) * Random.value;
            float rad = (angle / 180) * Mathf.PI;
			animation.Node.Velocity = Roga2dUtils.pixelToLocal(new Vector2(speed * Mathf.Cos(rad), speed * Mathf.Sin(rad)));
        }
		
		if (this.settings.Player == null) {
			Debug.LogError("No AnimationPlayer is defined in root");	
		}

		this.settings.Player.Play(this.settings.Root, this.sprite.Transform, animation, null);
	}
	
	private void UpdateKeyframe(int index) {
		
		Roga2dAnimationKeyFrame keyFrame = this.keyFrames[index];
        if (this.lastAnimationId != keyFrame.Id || keyFrame.Emitter) {
            this.ClearSetting();
			this.lastAnimationId = keyFrame.Id;
        }
		
        this.sprite.LocalPriority = keyFrame.Priority + Roga2dUtils.getPriorityAddition();
        this.sprite.BlendType = keyFrame.BlendType;
		
		
		if (keyFrame.Type == Roga2dAnimationKeyFrameType.Image) {
			// Update RenderObject
			if (this.renderObjectDescs[index] == null) {
				Debug.LogError("Null RenderObject");	
			}
			Roga2dRenderObjectDesc desc = this.renderObjectDescs[index];
			
			if (desc.RenderObject == this.sprite.RenderObject) {
				// Reuse old RenderObject and only changes its UV Map
				this.sprite.RenderObject.SetSrcRect(desc.SrcRect);
				this.sprite.RenderObject.SetSize(desc.PixelSize);
			} else {
				// Assign a new RenderObject
				this.sprite.RenderObject = desc.RenderObject;
			}
		}
    }
	
	private void UpdateSubAnimation(int index, float delta) {
		if (this.interval != null) {
			this.interval.Update(delta);
			return;
		}
		Roga2dAnimationKeyFrame keyFrame = this.keyFrames[index];
		if (keyFrame.Type != Roga2dAnimationKeyFrameType.Animation) { return; }
        if (keyFrame.Emitter) { return; }
		if (keyFrame.Id == "") { return; }

		// No animation node is generaetd yet, let's generate it
		// If no ID exists, ignore it (Which usually means an empty keyframe)
		Roga2dAnimation animation = Roga2dUtils.LoadAnimation(keyFrame.Id, true, null, this.settings, this.options);
		this.sprite.AddChild(animation.Node);
		this.interval = animation.Interval;
		this.interval.Start();
	}

	private void UpdateEmitAnimation(int index) {
		if (this.interval != null) { return; }

		Roga2dAnimationKeyFrame keyFrame = this.keyFrames[index];
		if (keyFrame.Type != Roga2dAnimationKeyFrameType.Animation) { return; }
        if (!keyFrame.Emitter) { return; }
		
		EmitAnimation(keyFrame);
	}
	
	private void checkEmitUpdate(int index, int frameNo) {
		if (this.lastUpdateIndex != index || this.lastUpdateFrameNo != frameNo) {
			this.UpdateEmitAnimation(index);
		}
		this.lastUpdateIndex = index;
		this.lastUpdateFrameNo = frameNo;
	}
	
    public override void Update(float delta) {
        if (this.IsDone()) {
            this.ClearSetting();
        } else {
			while (delta > 0) {
				if (this.frameDuration == 0) {
					this.UpdateKeyframe(index);
					this.checkEmitUpdate(this.index, this.frameNo);
				}

				this.frameNo = Mathf.FloorToInt(this.elapsed * Roga2dConst.AnimationFPS);
				float targetFrameNo =  this.frameNo + 1;
				float targetElapsedTime = targetFrameNo / Roga2dConst.AnimationFPS;
				float targetDelta =  targetElapsedTime - this.elapsed;
				if (targetDelta == 0) {
					targetDelta	= 1 / Roga2dConst.AnimationFPS;
				}
				
				if (targetDelta > delta) {
					targetDelta = delta;
				}
				this.frameDuration += targetDelta;
	            this.elapsed += targetDelta;
				
				this.UpdateSubAnimation(this.index, targetDelta);
				
				int updateFrameNo = Mathf.FloorToInt(this.elapsed * Roga2dConst.AnimationFPS);
				bool frameChanged = this.frameNo != updateFrameNo;
				this.frameNo = updateFrameNo;
				
				Roga2dAnimationKeyFrame keyFrame = this.keyFrames[index];
				if (this.frameDuration >= keyFrame.Duration) {
		            this.index += 1;

		            this.frameDuration -= keyFrame.Duration;
					
					// Force quit if all keyframes were executed
					if (this.index >= this.keyFrames.Count) {
						this.elapsed = this.duration;
						break;
					}
					
					this.UpdateKeyframe(index);
					this.checkEmitUpdate(this.index, this.frameNo);
		        } else {			
					if (frameChanged && this.frameDuration > Roga2dConst.AnimationFrameTime) {
						this.checkEmitUpdate(this.index, this.frameNo);	
					}
				}
				
				delta -= targetDelta;
			}
        }
    }
}