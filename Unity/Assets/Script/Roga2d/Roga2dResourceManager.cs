using UnityEngine;
using System.Collections.Generic;
using JsonFx.Json;

public class Roga2dResourceManager {
	
	public static void Init() {
		
	}
	
	// Animation
	private static Dictionary<string, Roga2dAnimationData> animationDictionary = new Dictionary<string, Roga2dAnimationData>();
	public static Roga2dAnimationData getAnimation(string key) {
		if (key == "") {return null;}
		if (!animationDictionary.ContainsKey(key))
		{
			TextAsset txt = (TextAsset)Resources.Load("Animations/" + key, typeof(TextAsset));
			Roga2dAnimationData animationData = JsonReader.Deserialize<Roga2dAnimationData>(txt.text);
			animationDictionary.Add(key, animationData);
		}

		return animationDictionary[key];
	}
	
	// Texture
	private static Dictionary<string, Texture> textureDictionary = new Dictionary<string, Texture>();
	public static Texture getTexture(string key) {
		if (!textureDictionary.ContainsKey(key))
		{
			Texture texture = Resources.Load("Images/" + key) as Texture;
			
			if (texture == null){ 
				return null;
			}
			texture.filterMode = FilterMode.Point;
			textureDictionary.Add(key, texture);
		}

		return textureDictionary[key];
	}
	
	
	// Shader
	private static Dictionary<string, Shader> shaderDictionary = new Dictionary<string, Shader>();
	public static Shader getShader(string key) {
		if (!shaderDictionary.ContainsKey(key))
		{
			Shader shader = Shader.Find(key);
			shaderDictionary.Add(key, shader);
		}

		return shaderDictionary[key];
	}
	
	// Material
	private static Dictionary<string, Material> materialDictionary = new Dictionary<string, Material>();
	public static Material getSharedMaterial(string key, Roga2dBlendType blendType) {
		string hash = key + blendType;
		
		if (!materialDictionary.ContainsKey(hash))
		{
			Material material = null;

			switch (blendType) {
			case Roga2dBlendType.Alpha:
				material = new Material(Roga2dResourceManager.getShader("TintAlphaBlended"));
				//material = new Material(Roga2dResourceManager.getShader("Mobile/Particles/Alpha Blended"));
				break;
			case Roga2dBlendType.Add:
				material = new Material(Roga2dResourceManager.getShader("Custom/AlphaAdditive"));
				//material = new Material(Roga2dResourceManager.getShader("Mobile/Particles/Additive"));
				break;
			default:
				Debug.LogError("Invalid BlendType is passed");
				break;
			}
	
			Texture texture = getTexture(key);
			material.mainTexture = texture;
			materialDictionary.Add(hash, material);
		}

		return materialDictionary[hash];
	}
}