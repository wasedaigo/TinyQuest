using UnityEngine;

public class Roga2dConst {
	public static float BasePixelSize = 64.0f;
};

public enum Roga2dBlendType {
	Alpha,
	Add
};

public enum Roga2dFacingType {
	None,
	FaceToDir,
	FaceToMov
};

public enum Roga2dAnimationKeyFrameType {
	None,
	Image,
	Animation
};

public enum Roga2dPositionType {
	None,
	RelativeToTarget,
	RelativeToTargetOrigin
};

public struct Roga2dHue {
	public int r;
	public int g;
	public int b;
	
	public Roga2dHue(int r, int g, int b) {
		this.r = r;
		this.g = g;
		this.b = b;
	}
	
	public void SetRGB(int r, int g, int b) {
		this.r = r;
		this.g = g;
		this.b = b;
	}
	
	public override bool Equals(object obj) 
	{
	  return obj is Roga2dHue && this == (Roga2dHue)obj;
	}
	
	public override int GetHashCode() 
	{
	  return this.r ^ this.g ^ this.b;
	}

	public static bool operator ==(Roga2dHue a, Roga2dHue b)
	{
	    return a.r == b.r && a.g == b.g && a.b == b.b;
	}
	
	public static bool operator !=(Roga2dHue a, Roga2dHue b)
	{
	    return !(a == b);
	}
	
	public override string ToString() 
	{
		return "[" + this.r + ", " + this.g + ", " + this.b + "]";
	}
};

public class Roga2dPositionIntervalOption {
	public Roga2dPositionType StartPositionType;
	public Vector2 StartPositionAnchor;
	public Roga2dPositionType EndPositionType;
	public Vector2 EndPositionAnchor;
	public Roga2dNode Target;
	public Roga2dNode TargetOrigin;
	
	public static Roga2dPositionIntervalOption Build() {
		Roga2dPositionIntervalOption option = new Roga2dPositionIntervalOption();
		option.StartPositionType = Roga2dPositionType.None;
		option.StartPositionAnchor = new Vector2(0.0f, 0.0f);
		option.EndPositionType = Roga2dPositionType.None;
		option.EndPositionAnchor = new Vector2(0.0f, 0.0f);
		option.Target = null;
		option.TargetOrigin = null;
		return option;
	}
}

public class Roga2dRotationIntervalOption {
	public Roga2dFacingType FacingType;
	public Roga2dNode Target;
	
	public static Roga2dRotationIntervalOption Build() {
		Roga2dRotationIntervalOption option = new Roga2dRotationIntervalOption();
		option.FacingType = Roga2dFacingType.None;
		option.Target = null;
		
		return option;
	}
}

public class Roga2dRotationIntervalDataStore {
	public bool ignore;
	public float lastAlpha;
	public Vector2 lastAbsPosition;
}

public class Roga2dAnimationKeyFrame {
	public string Id;
	public int FrameNo;
	public int Duration;
	public Rect Rect;
	public Vector2 PixelCenter;
	public Vector2 PixelSize;
	public bool Emitter;
	public float MaxEmitSpeed;
	public float MinEmitSpeed;
	public float MaxEmitAngle;
	public float MinEmitAngle;
	public Roga2dBlendType BlendType;
	public float Priority;
	public Roga2dAnimationKeyFrameType Type;
	
	public static Roga2dAnimationKeyFrame Build() {
		Roga2dAnimationKeyFrame keyFrame = new Roga2dAnimationKeyFrame();
		keyFrame.Id = "";
		keyFrame.FrameNo = 0;
		keyFrame.Duration = 0;
		keyFrame.Rect = new Rect(0, 0, 1, 1);
		keyFrame.PixelCenter = new Vector2(0, 0);
		keyFrame.PixelSize = new Vector2(1, 1);
		keyFrame.Emitter = false;
		keyFrame.MaxEmitSpeed = 0.0f;
		keyFrame.MinEmitSpeed = 0.0f;
		keyFrame.MaxEmitAngle = 0.0f;
		keyFrame.MinEmitAngle = 0.0f;
		keyFrame.BlendType = Roga2dBlendType.Alpha;
		keyFrame.Priority = 0.5f;
		keyFrame.Type = Roga2dAnimationKeyFrameType.Image;
		return keyFrame;
	}
}

public class Roga2dGameObjectState {
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;
}
