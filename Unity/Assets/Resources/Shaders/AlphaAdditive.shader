Shader "Custom/AlphaAdditive" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Main Texture", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	Blend SrcAlpha One
		
	Pass {
		Material {
			Diffuse [_Color]
			Ambient [_Color]
		}
		Lighting On
		SetTexture [_MainTex] {
			Combine texture * primary
		} 
	}	
}
}