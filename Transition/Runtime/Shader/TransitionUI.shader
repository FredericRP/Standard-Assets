Shader "Transitions/TransitionsUI"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TransitionTex("Transition Texture", 2D) = "white" {}
		_Cutoff("Cutoff", Range(0, 1)) = 0
		_Fade("Fade", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		Cull Off 
		ZWrite Off 
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uvMain : TEXCOORD0;
			};

			struct v2f
			{
				float2 uvMain : TEXCOORD0;
				float2 uvCutoff : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			float4 _MainTex_TexelSize;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uvMain = v.uvMain;
				o.uvCutoff = v.uvMain;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uvCutoff.y = 1 - o.uvCutoff.y;
				#endif
				return o;
			}

			sampler2D _TransitionTex;
			float _Fade;

			sampler2D _MainTex;
			float _Cutoff;

			fixed4 frag(v2f i) : SV_Target
			{
				// 0-1 Cutoff matches the CutOff+/-Fade interval
				_Cutoff = (_Cutoff - _Fade) * (1+ 2 * _Fade);

				fixed4 col = tex2D(_MainTex, i.uvMain);
				fixed4 cutCol = tex2D(_TransitionTex, i.uvCutoff);

				clip(cutCol.a - _Cutoff + _Fade);

				if (cutCol.a > (_Cutoff - _Fade) && cutCol.a < (_Cutoff + _Fade))
					col.a = (cutCol.a - _Cutoff + _Fade) / (2 * _Fade);
				else
					col.a = 1;

				return col;
			}					
			ENDCG
		}
	}
}
