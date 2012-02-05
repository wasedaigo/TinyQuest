using UnityEngine;
public class Roga2dTiledSprite : Roga2dNode {
	
	private int countX;
	private int countY;
	private float textureWidth;
	private float textureHeight;
	private float gridWidth;
	private float gridHeight;
	public Roga2dTiledSprite(string textureID, int countX, int countY) 
	: base("TileMap")
	{
		this.countX = countX;
		this.countY = countY;
		MeshFilter meshFilter = this.GameObject.AddComponent("MeshFilter") as MeshFilter;
		MeshRenderer renderer = this.GameObject.AddComponent("MeshRenderer") as MeshRenderer;

		Texture texture = Roga2dResourceManager.getTexture(textureID);
		this.textureWidth = texture.width;
		this.textureHeight = texture.height;
		renderer.sharedMaterial =  Roga2dResourceManager.getSharedMaterial(textureID, Roga2dBlendType.Alpha);
		this.SetGridSize(16.0f, 16.0f);
		meshFilter.mesh = GenerateGrid(countX, countY);
	}
	
	private void SetGridSize(float width, float height) {
		this.gridWidth = width;
		this.gridHeight = height;
		this.LocalScale = new Vector3(this.countX * width / Roga2dConst.BasePixelSize, this.countY * height / Roga2dConst.BasePixelSize, 0.1f);
	}

	private Mesh GenerateGrid(int countX, int countY) {

		int gridCount = countX * countY;
		Vector3[] vertices = new Vector3[gridCount * 4];
		Vector2[] uvs = new Vector2[gridCount * 4];
		int[] triangles = new int[gridCount * 6];
	
		float vw = 1.0f / countX;
		float vh = 1.0f / countY;
		float ux = 1 / this.textureWidth;
		float uy = 1 / this.textureHeight;
		float uw = (this.gridWidth - 1) / this.textureWidth;
		float uh = (this.gridHeight - 1) / this.textureHeight;
		float[] uv = new float[4]{ux, 1.0f - uy, ux + uw, 1.0f - uy - uh};

		// Setup Mesh data
		int i = 0;
		for (int x = 0; x < countX; x++) {
			for (int y = 0; y < countY; y++) {
				int t1 = i * 4;
				
				// Vertex
				float vx = vw * x;
				float vy = vh * y;
				vertices[t1] = new Vector3(vx , vy, 0.01f);
				vertices[t1 + 1] = new Vector3(vx, vy + vh, 0.01f);
				vertices[t1 + 2] = new Vector3(vx + vw, vy, 0.01f);
				vertices[t1 + 3] = new Vector3(vx + vw, vy + vh, 0.01f);
		
				// UVs
				uvs[t1] = new Vector2(uv[0], uv[1]);
				uvs[t1 + 1] = new Vector2(uv[0], uv[3]);
				uvs[t1 + 2] = new Vector2(uv[2], uv[1]);
				uvs[t1 + 3] = new Vector2(uv[2], uv[3]);
				
				// Index
				int t2 = i * 6;
				triangles[t2] = t1;
				triangles[t2 + 1] = t1 + 1;
				triangles[t2 + 2] = t1 + 2;
				triangles[t2 + 3] = t1 + 2;
				triangles[t2 + 4] = t1 + 1;
				triangles[t2 + 5] = t1 + 3;
				i++;
			}
		}
		
		// Initialize Mesh
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		return mesh;
	}
}