using UnityEngine;
public class Roga2dRenderObject {
	public Rect srcRect;
	public Vector2 pixelSize;
	public Vector2 pixelCenter;
	private GameObject gameObject;
	private Texture texture;
	private Mesh mesh;
	private Material material;
	private Roga2dBlendType blendType;
	private float alpha;

	public Roga2dRenderObject(Texture texture, Vector2 pixelSize, Vector2 pixelCenter, Rect srcRect)
	{
		this.gameObject = null;
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
		}
	}

	public void Pop() {
		this.gameObject = new GameObject("RenderObject");
		
		// Setup Renderer
		MeshFilter meshFilter = this.gameObject.AddComponent("MeshFilter") as MeshFilter;
		this.gameObject.AddComponent("MeshRenderer");
		
		float[] uvs;
		
		if (this.texture == null) {
			uvs = new float[4] {0.0f, 0.0f, 1.0f, 1.0f};
		} else {
			uvs = new float[4] {
				this.srcRect.xMin / this.texture.width, 
				1 - this.srcRect.yMax / this.texture.height, 
				this.srcRect.xMax / this.texture.width, 
				1 - this.srcRect.yMin / this.texture.height
			};
		}
		
		meshFilter.mesh = GeneratePlane(this.pixelSize.x / Roga2dConst.BasePixelSize, this.pixelSize.y / Roga2dConst.BasePixelSize, uvs);
		this.SetBlend(Roga2dBlendType.Alpha, 1.0f);		
	}
	
	public GameObject GameObject {
		get {
			return this.gameObject;
		}
	}

	private Mesh GeneratePlane(float sizeX, float sizeY, float[] uvs) {
		if (this.mesh != null) {
			Debug.Log("Mesh is not null!");	
		}
		this.mesh = new Mesh();
		mesh.vertices = new Vector3[4] {
			new Vector3(-sizeX, -sizeY, 0.01f), 
			new Vector3(sizeX, -sizeY, 0.01f), 
			new Vector3(sizeX, sizeY, 0.01f), 
			new Vector3(-sizeX, sizeY, 0.01f)
		};
		mesh.uv = new Vector2[4] {
			new Vector2(uvs[0], uvs[1]), 
			new Vector2 (uvs[0], uvs[3]), 
			new Vector2(uvs[2], uvs[3]), 
			new Vector2 (uvs[2], uvs[1])
		};
		mesh.triangles = new int[6] {0, 1, 2, 0, 2, 3};
		mesh.RecalculateNormals();
		
		return this.mesh;
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

		MeshRenderer renderer = this.gameObject.GetComponent("MeshRenderer") as MeshRenderer;
		Color color;
		switch (blendType) {
		case Roga2dBlendType.Alpha:
			this.material = new Material(Shader.Find("Transparent/VertexLit"));
			color = new Color(1.0f, 1.0f, 1.0f, alpha);
			this.material.SetColor("_Color", color);
			break;
		case Roga2dBlendType.Add:
			renderer.material = new Material(Shader.Find("Custom/AlphaAdditive"));
			color = new Color(1.0f, 1.0f, 1.0f, alpha);
			renderer.material.SetColor("_Color", color);
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