﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TransparentCutoutSoftEdgeBlendedUnlit" { 
Properties {
    _Color ("Main Color", Color) = (1, 1, 1, 1)
    _Cutoff ("Base Alpha cutoff", Range (0,.9)) = .9
    _Blend ("Blend", Range (0, 1) ) = 0.0 
    _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" { } 
        _Texture2 ("Texture 2", 2D) = ""
}

SubShader {
    Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
    Lighting off
    
    // Render both front and back facing polygons.
    Cull Off
    
    // first pass:
    //   render any pixels that are more than [_Cutoff] opaque
    Pass {  
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _Texture2;
            float4 _MainTex_ST;
            float4 _Texture2_ST;
            float _Cutoff;
            float _Blend;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
            
            float4 _Color;
            half4 frag (v2f i) : COLOR
            {
                half4 col = lerp(tex2D(_MainTex, i.texcoord),tex2D(_Texture2, i.texcoord), _Blend);
                clip(col.a - _Cutoff);
                return col;
            }
        ENDCG
    }

    // Second pass:
    //   render the semitransparent details.
    Pass {
        Tags { "RequireOption" = "SoftVegetation" }
        
        // Dont write to the depth buffer
        ZWrite off
        
        // Set up alpha blending
        Blend SrcAlpha OneMinusSrcAlpha
        
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _Texture2;
            float4 _MainTex_ST;
            float4 _Texture2_ST;
            float _Cutoff;
            float _Blend;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
            
            float4 _Color;
            half4 frag (v2f i) : COLOR
            {
                half4 col = lerp(tex2D(_MainTex, i.texcoord),tex2D(_Texture2, i.texcoord), _Blend);
                clip(-(col.a - _Cutoff));
                return col;
            }
        ENDCG
    }
}

SubShader {
    Tags { "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
    Lighting off
    
    // Render both front and back facing polygons.
    Cull Off
    
    // first pass:
    //   render any pixels that are more than [_Cutoff] opaque
    Pass {  
        AlphaTest Greater [_Cutoff]
        SetTexture [_MainTex] {
            constantColor [_Color]
            combine texture * constant, texture * constant 
        }
    }

    // Second pass:
    //   render the semitransparent details.
    Pass {
        Tags { "RequireOption" = "SoftVegetation" }
        
        // Dont write to the depth buffer
        ZWrite off
        
        // Only render pixels less or equal to the value
        AlphaTest LEqual [_Cutoff]
        
        // Set up alpha blending
        Blend SrcAlpha OneMinusSrcAlpha
        
        SetTexture [_MainTex] {
            constantColor [_Color]
            Combine texture * constant, texture * constant 
        }
    }
}

}