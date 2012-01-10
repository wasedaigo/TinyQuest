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
}