// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}


	SubShader
	{

			 Tags { 
        "Queue"="Geometry+5"
        "RenderType"="Opaque"
    }
	
		
			
			Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			ZTest Greater
			Offset -0.1,-0.1
			ZWrite Off
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 viewDir : TEXCOORD;
				float3 normal : NORMAL;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(UnityWorldSpaceViewDir(o.pos));
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//Note you have to normalize these again since they are being interpolated between vertices
				float rim = 1 - dot(normalize(i.normal), normalize(i.viewDir));
				return lerp(half4(1, 0, 0, 1), half4(0, 1, 0, 1), rim);
			}
				ENDCG
		}

			Pass
			{
				 Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag

			// #pragma vertex SpriteVert
			// #pragma fragment SpriteFrag
			//#pragma target 2.0
			//#pragma multi_compile_instancing
			//#pragma multi_compile _ PIXELSNAP_ON
			//#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"

			sampler2D _MainTex;

		fixed4 frag(v2f_img i) : SV_Target
		{
			float4 color = tex2D(_MainTex, i.uv);
			return color;
		}
		ENDCG
			}

	}
}