using UnityEngine;

/*
 * 	[Optimization idea]
 *  1. Make object pool for GameObject / Mesh / Material, reuse them instead of Destoryoing them
 *  2. Use shared material. (Trade-off with adjustable transparency)
 *  3. Try not to use different render-object for each different keyframe if it shares texture
 */
public class Roga2dRenderObject {
	public Rect srcRect;
	public Vector2 pixelSize;
	public Vector2 pixelCenter;
	private GameObject gameObject;
	private Transform transform;
	private MeshRenderer renderer;
	private Texture texture;
	private Mesh mesh;
	private Material material;
	private Roga2dBlendType blendType;
	private float alpha;

	public Roga2dRenderObject(Texture texture, Vector2 pixelSize, Vector2 pixelCenter, Rect srcRect)
	{
		this.gameObject = null;
		this.transform = null;
		this.pixelSize = pixelSize;
		this.pixelCenter = pixelCenter;
		this.srcRect = srcRect;
		this.texture = texture;
		this.alpha = -0.1f; // In order to update its materal at SetBlend at first time
	}

	public void Destroy() {
		if (this.gameObject != null) {
			Object.Destroy(this.gameObject);
			Object.Destroy(this.mesh);
			Object.Destroy(this.material);
			this.gameObject = null;
			this.transform = null;
			this.mesh = null;
			this.material = null;
		}
	}

	public void Pop() {
		this.gameObject = new GameObject("RenderObject");
		this.transform = this.gameObject.transform;
		
		if (this.texture != null) {
			MeshFilter meshFilter = this.gameObject.AddComponent("MeshFilter") as MeshFilter;
			this.renderer = this.gameObject.AddComponent("MeshRenderer") as MeshRenderer;
			
			this.alpha = 0.0f;
			this.mesh = GeneratePlane(1, 1);
			meshFilter.mesh = this.mesh;
			this.SetSize(this.pixelSize);
			this.SetSrcRect(this.srcRect);
			this.SetBlend(Roga2dBlendType.Alpha, 1.0f);	
		}
	}
	
	public GameObject GameObject {
		get {
			return this.gameObject;
		}
	}
	
	public Transform Transform {
		get {
			return this.transform;
		}
	}
	
	public MeshRenderer Renderer {
		get {
			return this.renderer;
		}
	}
	
	private Mesh GeneratePlane(float sizeX, float sizeY) {
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[4] {
			new Vector3(-1.0f, -1.0f, 0.01f), 
			new Vector3(1.0f, -1.0f, 0.01f), 
			new Vector3(1.0f, 1.0f, 0.01f), 
			new Vector3(-1.0f, 1.0f, 0.01f)
		};
		mesh.triangles = new int[6] {0, 1, 2, 0, 2, 3};
		mesh.RecalculateNormals();
		
		return mesh;
	}

	public void SetSize(Vector2 pixelSize) {
		pixelSize = Roga2dUtils.FixCoordinate(pixelSize);
		this.transform.localScale = new Vector3(pixelSize.x / Roga2dConst.BasePixelSize, pixelSize.y / Roga2dConst.BasePixelSize, 0.1f);
	}

	public void SetSrcRect(Rect srcRect) {
		this.srcRect = srcRect;
		if (this.texture != null) {
			float uv1 = this.srcRect.xMin / this.texture.width;
			float uv2 = 1 - this.srcRect.yMax / this.texture.height;
			float uv3 = this.srcRect.xMax / this.texture.width;
			float uv4 = 1 - this.srcRect.yMin / this.texture.height;

			this.mesh.uv = new Vector2[4] {
				new Vector2(uv1, uv2), 
				new Vector2(uv1, uv4), 
				new Vector2(uv3, uv4), 
				new Vector2(uv3, uv2)
			};
		}
	}
	
	public void SetBlend(Roga2dBlendType blendType, float alpha) {
		
		// If nothing has changed, don't do anything
		if (this.blendType == blendType && this.alpha == alpha) {
			return;	
		}
		this.blendType = blendType;
		this.alpha = alpha;
		
		if (this.material != null) {
			Object.Destroy(this.material);
		}

		this.renderer = this.gameObject.GetComponent("MeshRenderer") as MeshRenderer;
		Color color;
		switch (blendType) {
		case Roga2dBlendType.Alpha:
			this.material = new Material(Shader.Find("Transparent/VertexLit"));
			color = new Color(1.0f, 1.0f, 1.0f, alpha);
			this.material.SetColor("_Color", color);
			break;
		case Roga2dBlendType.Add:
			this.material = new Material(Shader.Find("Custom/AlphaAdditive"));
			color = new Color(1.0f, 1.0f, 1.0f, alpha);
			this.material.SetColor("_Color", color);
			break;
		default:
			Debug.LogError("Invalid BlendType is passed");
			break;
		}
		
		// Setup Texture
		if (this.texture != null) {
			this.material.mainTexture = this.texture;
		}

		renderer.material = this.material;
	}
	
	public Vector2 PixelSize {
		get {
			return this.pixelSize;	
		}
	}
	
	public Vector2 PixelCenter {
		get {
			return this.pixelCenter;	
		}
	}
	
	public Vector2 Anchor {
		get {
			float centerX = (this.pixelCenter.x * 2.0f) / Roga2dConst.BasePixelSize;
			float centerY = (this.pixelCenter.y * 2.0f) / Roga2dConst.BasePixelSize;
			return new Vector2(centerX, centerY);
		}
	}

	public Rect SrcRect {
		get {
			return this.srcRect;	
		}
	}
}