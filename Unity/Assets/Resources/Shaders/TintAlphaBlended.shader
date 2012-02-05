// Simplified VertexLit Blended Particle shader. Differences from regular VertexLit Blended Particle one:
// - no AlphaTest
// - no ColorMask

Shader "Custom/TintAlphaBlended" {
Properties {
	_EmisColor ("Emissive Color", Color) = (.5,.5,.5, 1.0)
	_MainTex ("Particle Texture", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

	Cull Off ZWrite Off Fog { Color (0,0,0,0) }
	Blend SrcAlpha OneMinusSrcAlpha 

	Pass {
		Tags { "LightMode" = "Always" }
		Material {
			Diffuse [_EmisColor]
			Emission [_EmisColor]	
		}
		Lighting On
		SetTexture [_MainTex] {
			Combine texture * primary DOUBLE, texture * primary
		} 
	}	
}
}