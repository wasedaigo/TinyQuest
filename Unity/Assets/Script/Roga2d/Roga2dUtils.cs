using UnityEngine;
using System.Collections.Generic;
using System;

public class Roga2dSourceIntervalData {
	public string id;
	public int frameNo;
	public int duration;
	public float[] rect;
	public int[] center;
	public bool emitter;
	public float maxEmitSpeed;
	public float minEmitSpeed;
	public float maxEmitAngle;
	public float minEmitAngle;
	public float priority;
	public Roga2dBlendType blendType;
	public Roga2dAnimationKeyFrameType type;
}

public class Roga2dPositionIntervalData {
	public bool tween;
	public int frameNo;
	public int duration;
	public float[] startValue;
	public float[] endValue;
	public bool wait;
	public Roga2dPositionType startPositionType;
	public Roga2dPositionType endPositionType;
	public float[] startPositionAnchor;
	public float[] endPositionAnchor;
}

public class Roga2dRotationIntervalData {
	public bool tween;
	public int frameNo;
	public int duration;
	public float startValue;
	public float endValue;
	public bool wait;
	public Roga2dFacingType facingOption;
}

public class Roga2dScaleIntervalData {
	public bool tween;
	public int frameNo;
	public int duration;
	public float[] startValue;
	public float[] endValue;
	public bool wait;
}

public class Roga2dAlphaIntervalData {
	public bool tween;
	public int frameNo;
	public int duration;
	public float startValue;
	public float endValue;
	public bool wait;
}

public class Roga2dIntervalData {
	public Roga2dPositionIntervalData[] position;
	public Roga2dRotationIntervalData[] rotation;
	public Roga2dScaleIntervalData[] scale;
	public Roga2dAlphaIntervalData[] alpha;
	public Roga2dSourceIntervalData[] source;
}

public class Roga2dAnimationDependencyData {
	public string[] animations;
	public string[] images;
}

public class Roga2dAnimationData {
	public Roga2dAnimationDependencyData dependencies;
	public Roga2dIntervalData[] timelines;
}

public class Roga2dUtils {
	// Sometimes there are sprites at the exact same z-position
	// This causes weird rendering effect depending on devices, so let's avoid that by little hack here
	private static float priorityAddition;
	private static void incrementPriorityAddition() {
		priorityAddition += 0.0001f;
		if (priorityAddition > 0.01f) {
			priorityAddition = 0.0f;	
		}
	}
	
	public static float getPriorityAddition() {
		incrementPriorityAddition();
		return priorityAddition;
	}
	
	public static float Completement(float start, float end, float proportion) {
		return start + (end - start) * proportion;
	}
	
	public static Vector2 Completement(Vector2 start, Vector2 end, float proportion) {
		Vector2 result;
		result.x = start.x + (end.x - start.x) * proportion;
		result.y = start.y + (end.y - start.y) * proportion;
		return result;
	}

    public static Roga2dAnimation LoadAnimation(string id, bool isSubAnimation, float baseAlpha, float basePriority, Roga2dRoot root, Dictionary<string, string> options) {
		Roga2dAnimationData animationData = Roga2dResourceManager.getAnimation(id);
		
		List<Roga2dBaseInterval> parallels = new List<Roga2dBaseInterval>();
        Roga2dNode node = new Roga2dNode(new GameObject(id));
        node.LocalAlpha = baseAlpha;
        node.LocalPriority = basePriority;
        
       foreach (Roga2dIntervalData timeline in animationData.timelines) {
            
            Roga2dSprite sprite = new Roga2dSprite(null);
            List<Roga2dBaseInterval> sequences = new List<Roga2dBaseInterval>();
		
			// Add alpha interval
			{
				List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
				foreach(Roga2dAlphaIntervalData alphaIntervalData in timeline.alpha) {
					float start = alphaIntervalData.startValue;
					float end = alphaIntervalData.endValue;
					Roga2dAlphaInterval interval = new Roga2dAlphaInterval(sprite, start, end, alphaIntervalData.duration, alphaIntervalData.tween);
					intervals.Add(interval);
				}
				if(intervals.Count > 0) {
					sequences.Add(new Roga2dSequence(intervals));
				}
			}
			
			// Add scale interval
			{
				List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
				foreach(Roga2dScaleIntervalData scaleIntervalData in timeline.scale) {
					Vector2 start = FixCoordinate(new Vector2(scaleIntervalData.startValue[0], scaleIntervalData.startValue[1]));
					Vector2 end = FixCoordinate(new Vector2(scaleIntervalData.endValue[0], scaleIntervalData.endValue[1]));
					intervals.Add(new Roga2dScaleInterval(sprite, start, end, scaleIntervalData.duration, scaleIntervalData.tween));
				}
				if(intervals.Count > 0) {
					sequences.Add(new Roga2dSequence(intervals));
				}
			}

			// Add position interval
			{
				List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
				foreach(Roga2dPositionIntervalData positionIntervalData in timeline.position) {
					Vector2 start = Roga2dUtils.pixelToLocal(new Vector2(positionIntervalData.startValue[0], positionIntervalData.startValue[1]));
					Vector2 end = Roga2dUtils.pixelToLocal(new Vector2(positionIntervalData.endValue[0], positionIntervalData.endValue[1]));
					Roga2dPositionIntervalOption option = Roga2dPositionIntervalOption.Build();
	                option.StartPositionAnchor = Roga2dUtils.pixelToLocal(new Vector2(positionIntervalData.startPositionAnchor[0], positionIntervalData.startPositionAnchor[1]));
					option.EndPositionAnchor = Roga2dUtils.pixelToLocal(new Vector2(positionIntervalData.endPositionAnchor[0], positionIntervalData.endPositionAnchor[1]));
	                option.StartPositionType = positionIntervalData.startPositionType;
					option.EndPositionType = positionIntervalData.endPositionType;
	                option.Target = (root!=null) ? root.Target : null;
					option.TargetOrigin = (root!=null) ? root.TargetOrigin : null;

					intervals.Add(new Roga2dPositionInterval(sprite, start, end, positionIntervalData.duration, positionIntervalData.tween, option));
				}
				if(intervals.Count > 0) {
					sequences.Add(new Roga2dSequence(intervals));
				}
			}
			
			// Add rotation interval
			{
				List<Roga2dBaseInterval> intervals = new List<Roga2dBaseInterval>();
				foreach(Roga2dRotationIntervalData rotationIntervalData in timeline.rotation) {
					
					float start = rotationIntervalData.startValue;
					float end = rotationIntervalData.endValue;
					Roga2dRotationIntervalOption option = Roga2dRotationIntervalOption.Build();
	                option.FacingType = rotationIntervalData.facingOption;
	                option.Target = (root!=null) ? root.Target : null;
					intervals.Add(new Roga2dRotationInterval(sprite, start, end, rotationIntervalData.duration, rotationIntervalData.tween, option));
				}
				if(intervals.Count > 0) {
					sequences.Add(new Roga2dSequence(intervals));
				}
			}

			// Add source interval
			List<Roga2dAnimationKeyFrame> keyFrames = new List<Roga2dAnimationKeyFrame>();
			foreach(Roga2dSourceIntervalData sourceIntervalData in timeline.source) {
				Roga2dAnimationKeyFrame keyFrame = Roga2dAnimationKeyFrame.Build();
				keyFrame.Type = sourceIntervalData.type;
				keyFrame.FrameNo = sourceIntervalData.frameNo;
				if (keyFrame.Type == Roga2dAnimationKeyFrameType.Image) {
					keyFrame.Id = swapTextureId(sourceIntervalData.id, options);
				} else {
					keyFrame.Id = sourceIntervalData.id;
				}
				keyFrame.Duration = sourceIntervalData.duration;
				keyFrame.Priority = sourceIntervalData.priority;
				keyFrame.BlendType = sourceIntervalData.blendType;
				if (keyFrame.Type == Roga2dAnimationKeyFrameType.Image) {
					keyFrame.Rect = new Rect(sourceIntervalData.rect[0], sourceIntervalData.rect[1], sourceIntervalData.rect[2], sourceIntervalData.rect[3]);
					keyFrame.PixelCenter = new Vector2(sourceIntervalData.center[0], sourceIntervalData.center[1]);
					keyFrame.PixelSize = new Vector2(sourceIntervalData.rect[2], sourceIntervalData.rect[3]);
				} else {				
					keyFrame.Emitter = sourceIntervalData.emitter;
					keyFrame.MaxEmitAngle = sourceIntervalData.maxEmitAngle;
					keyFrame.MinEmitAngle = sourceIntervalData.minEmitAngle;
					keyFrame.MaxEmitSpeed = sourceIntervalData.maxEmitSpeed;
					keyFrame.MinEmitSpeed = sourceIntervalData.minEmitSpeed;
				}
				keyFrames.Add(keyFrame);
			}
			if (keyFrames.Count > 0) {
				sequences.Add(new Roga2dSourceInterval(sprite, keyFrames, root, options));
			}

			parallels.Add(new Roga2dParallel(sequences));
            node.AddChild(sprite);
        }
		
		{
			Roga2dParallel parallel = new Roga2dParallel(parallels);
	        if (isSubAnimation) {
				 return Roga2dAnimation.Build(node, new Roga2dLoop(parallel, 0));
	        } else {
				 return Roga2dAnimation.Build(node, parallel);
	        }
		}
    }
	
	public static string swapTextureId(string baseFileName, Dictionary<string, string> conversionMap){

		if (conversionMap.ContainsKey(baseFileName)) {
			return conversionMap[baseFileName];
		}
		
		return baseFileName;
	}

	public static Vector2 pixelToLocal(Vector2 pixelCoordinate){
		return new Vector2(-pixelCoordinate.y / 32.0f, pixelCoordinate.x / 32.0f);
	}
	
	public static Vector2 localToPixel(Vector2 localCoordinate){
		return new Vector2(localCoordinate.y * 32.0f, -localCoordinate.x * 32.0f);
	}
	
	public static Vector2 FixCoordinate(Vector2 coordinate){
		return new Vector2(coordinate.y, coordinate.x);
	}
	
	public static float RoundPrecision(float value){
		return Mathf.Round(value * 10000) / 10000;
	}
	
	public static Roga2dGameObjectState stashState(Transform transform) {
		
		Roga2dGameObjectState state = new Roga2dGameObjectState();
		// Unity3D nodes break local coordinate of child node when it is added
		// Thus, it is needed to set the parent node (0,0,0) first
		// Gotta save current state of the node before adding a child
		state.position = transform.localPosition;
		state.rotation = transform.localRotation;
		state.scale = transform.localScale;
		transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		transform.localRotation = new Quaternion(0, 0, 0, 0);
		transform.localPosition = new Vector3(0, 0, 0);
		
		return state;
	}
	
	// Recover old state after child node is already added
	public static void applyState(Transform transform, Roga2dGameObjectState state) {
		transform.localPosition = state.position;
		transform.localRotation = state.rotation;
		transform.localScale = state.scale;	
	}
}

public abstract class Roga2dBaseMathProvider<T> {
	public abstract T Completement(T start, T end, float proportion);
}

public class Roga2dFloatMathProvider : Roga2dBaseMathProvider<float> {
	public override float Completement(float start, float end, float proportion) {
		return start + (end - start) * proportion;
	}
}

public class Roga2dVector2MathProvider : Roga2dBaseMathProvider<Vector2> {
	public override Vector2 Completement(Vector2 start, Vector2 end, float proportion) {
		Vector2 result;
		result.x = start.x + (end.x - start.x) * proportion;
		result.y = start.y + (end.y - start.y) * proportion;
		return result;
	}
}

public class Roga2dHueMathProvider : Roga2dBaseMathProvider<Roga2dHue> {
	public override Roga2dHue Completement(Roga2dHue start, Roga2dHue end, float proportion) {
		int r = start.r + (int)((end.r - start.r) * proportion);
		int g = start.g + (int)((end.g - start.g) * proportion);
		int b = start.b + (int)((end.b - start.b) * proportion);
		return new Roga2dHue(r, g, b);
	}
}

public static class Roga2dUtils<T> {
        static Roga2dBaseMathProvider<T> _math;
        static Roga2dUtils()
        {
            if (typeof(T) == typeof(float))
                _math = new Roga2dFloatMathProvider() as Roga2dBaseMathProvider<T>;
            else if (typeof(T) == typeof(Vector2))
                _math = new Roga2dVector2MathProvider() as Roga2dBaseMathProvider<T>;
            else if (typeof(T) == typeof(Roga2dHue))
                _math = new Roga2dHueMathProvider() as Roga2dBaseMathProvider<T>;
        }
	
    	public static T Completement(T start, T end, float proportion)
        {
            return _math.Completement(start, end, proportion);
        }
}