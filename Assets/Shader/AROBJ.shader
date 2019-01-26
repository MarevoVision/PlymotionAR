Shader "Trofymchuk/AROBJ" {
	Properties {
		_Color ("Color", Color) = (0,0,0,0)
        _Emission ("Emmisive Color", Color) = (0.6,0.6,0.6,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
    Pass {
            Material {
                Diffuse [_Color]
                Ambient [_Color]
                Emission [_Emission]
            }
            Lighting On
            SetTexture [_MainTex] {
                Combine texture * primary DOUBLE, texture * primary
            }
        }
	}
	FallBack "Diffuse"
}
