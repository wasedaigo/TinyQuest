using UnityEngine;
using System.Collections.Generic;
using JsonFx.Json;

public class Roga2dResourceManager {
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
	
	// Material
	private static Dictionary<string, Material> materialDictionary = new Dictionary<string, Material>();
	public static Material getSharedMaterial(string key) {
		if (!materialDictionary.ContainsKey(key))
		{
			Texture texture = getTexture(key);
			Material material = new Material(Shader.Find("Diffuse"));
			material.mainTexture = texture;
			materialDictionary.Add(key, material);
		}

		return materialDictionary[key];
	}
}