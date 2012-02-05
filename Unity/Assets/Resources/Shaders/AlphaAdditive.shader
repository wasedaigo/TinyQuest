Shader "Custom/AlphaAdditive" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Main Texture", 2D) = "white" {}
}

SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	Pass {
		Material {
			Diffuse [_Color]
			Ambient [_Color]
		}
		Lighting On
		SetTexture [_MainTex] {
			Combine texture * primary DOUBLE
		} 
	}	
}
}