// Simplified VertexLit Blended Particle shader. Differences from regular VertexLit Blended Particle one:
// - no AlphaTest
// - no ColorMask

Shader "TintAlphaBlended" {
Properties {
	_EmisColor ("Emissive Color", Color) = (.2,.2,.2,0)
	_MainTex ("Particle Texture", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off ZWrite Off Fog { Color (0,0,0,0) }
	Lighting Off
	SubShader {
		Pass {
			SetTexture [_MainTex] {
			  constantColor [_EmisColor]
				combine texture +- constant, texture * constant
			}
		}
	}
}
}