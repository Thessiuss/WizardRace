Shader "Mobile/Surface/ColorAlphaMetallicSmooth"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Transparency ("Transparency", Range(0.0, 1.0)) = 0.45
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

		ZWrite off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		half _Smoothness;
		half _Metallic;
		fixed4 _Color;
		float _Transparency;

		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			c.a = _Transparency;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = c.a;
		}
		ENDCG      
    }
    FallBack "Diffuse"
}
