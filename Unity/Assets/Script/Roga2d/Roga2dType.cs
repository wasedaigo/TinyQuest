using UnityEngine;

public class Roga2dConst {
	public static float BasePixelSize = 32.0f;
	public static float AnimationFPS = 30.0f;
	public static float AnimationFrameTime = 1.0f / AnimationFPS;
};

public enum Roga2dBlendType {
	Alpha,
	Add,
	Unlit
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
	
	public bool IsZero() {
		return this.r == 0 && this.g == 0 && this.b == 0;
	}

	public override bool Equals(object obj) 
	{
	  return obj is Roga2dHue && this == (Roga2dHue)obj;
	}
	
	public override int GetHashCode() 
	{
	  return this.r ^ this.g ^ this.b;
	}
	
	public static Roga2dHue operator +(Roga2dHue a, Roga2dHue b)
	{
		int newR = a.r + b.r;
		int newG = a.g + b.g;
		int newB = a.b + b.b;
		newR = Mathf.Min(255, Mathf.Max(-255, newR));
		newG = Mathf.Min(255, Mathf.Max(-255, newG));
		newB = Mathf.Min(255, Mathf.Max(-255, newB));
	    return new Roga2dHue(newR, newG, newB);
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
	public Roga2dRotationIntervalDataStore DataStore;
	
	public static Roga2dRotationIntervalOption Build() {
		Roga2dRotationIntervalOption option = new Roga2dRotationIntervalOption();
		option.FacingType = Roga2dFacingType.None;
		option.Target = null;
		option.DataStore = null;

		return option;
	}
}

public class Roga2dRotationIntervalDataStore {
	public bool initialized;
	public float lastAlpha;
	public Vector2 lastAbsPosition;
	public float lastRotation;
}

public class Roga2dAnimationKeyFrame {
	public string Id;
	public int FrameNo;
	public float Duration;
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
}

public class Roga2dGameObjectState {
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;
}

public class Roga2dSwapTextureDef {
	private string textureID;
	private Vector2 pixelSize;
	private Vector2 pixelCenter;
	private Rect srcRect;
	private bool swapTextureID;
	private bool swapPixelSize;
	private bool swapPixelCenter;
	private bool swapSrcRect;
	
	public string TextureID {
		get {
			return this.textureID;	
		}
		set {
			this.textureID = value;
			this.swapTextureID = true;
		}
	}
	
	public Vector2 PixelSize {
		get {
			return this.pixelSize;	
		}
		set {
			this.pixelSize = value;
			this.swapPixelSize = true;
		}
	}
	
	public Vector2 PixelCenter {
		get {
			return this.pixelCenter;	
		}
		set {
			this.pixelCenter = value;
			this.swapPixelCenter = true;
		}
	}
	
	public Rect SrcRect {
		get {
			return this.srcRect;	
		}
		set {
			this.srcRect = value;
			this.swapSrcRect = true;
		}
	}

	public bool SwapTextureID {
		get {
			return this.swapTextureID;	
		}
	}

	public bool SwapPixelSize {
		get {
			return this.swapPixelSize;	
		}
	}
	
	public bool SwapPixelCenter {
		get {
			return this.swapPixelCenter;	
		}
	}
	
	public bool SwapSrcRect {
		get {
			return this.swapSrcRect;	
		}
	}
}
