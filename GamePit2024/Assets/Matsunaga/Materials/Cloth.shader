Shader "Custom/Cloth"
{
	//Copyright(c) 2019 @gam0022
	//	(Sho Hosoda)
	//	https ://qiita.com/gam0022/items/6fad2d8df583c70d417f
	//	Released under the MIT license
	//	https ://github.com/YukinobuKurata/YouTubeMagicBuyButton/blob/master/MIT-LICENSE.txt
	//プロパティーブロック。ビルトインと同様に外部から操作可能な設定値を定義できる。
	Properties
	{
		// メインテクスチャ
		_MainTex("Texture", 2D) = "white" {}

		[Header(Wave 1)]
		_WaveAmplitude1("Amplitude", Range(0, 1)) = 0.025// 振幅
		_WaveSpeed1("Speed", Range(-30, 30)) = 9.8// 速度
		_WaveFreq1("Freq", Range(0, 30)) = 8.5// 周波数

		[Header(Wave 2)]
		_WaveAmplitude2("Amplitude", Range(0, 1)) = 0.0045
		_WaveSpeed2("Speed", Range(-30, 30)) = 15.4
		_WaveFreq2("Freq", Range(0, 30)) = 25.3

		// 落下機能
		[Header(Drop)]
		[Toggle] _DROP_Y("Enable Drop Y", Float) = 0
		_DropY("Drop Y", Range(0  , 1)) = 1

		[Header(Lighting)]
		_LightDirection("Light Direction", Vector) = (1.0, 3.0, 2.0, 0.0)
		_ShadowIntensity("Shadow Intensity", Range(0.0, 1.0)) = 0.5

		// 乗算カラー
		[HDR] _TintColor("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)

		// カリングモード
		[Header(Culling)]
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", Float) = 2// Back
	}

		//サブシェーダーブロック。ここに処理を書いていく。
		SubShader
	{
		//タグ。サブシェーダーブロック、もしくはパスが実行されるタイミングや条件を記述する。
		Tags
		{
			"Queue" = "AlphaTest+51"
			//レンダリングのタイミング(順番)
			"RenderType" = "Opaque"
			//レンダーパイプラインを指定する。なくても動く。動作環境を制限する役割。
			"RenderPipeline" = "UniversalRenderPipeline"
			"LightMode" = "UniversalForward"
			"IgnoreProjector" = "True"
		}
		LOD 100
		Pass
		{
			Lighting Off
			Fog{ Mode Off }

			Cull[_CullMode]
			ZWrite On
			ZTest LEqual
			//HLSL言語を使うという宣言(おまじない)。ビルトインではCg言語だった。
			HLSLPROGRAM
			//vertという名前の関数がvertexシェーダーです　と宣言してGPUに教える。
			#pragma vertex vert
			//fragという名前の関数がfragmentシェーダーです　と宣言してGPUに教える。
			#pragma fragment frag
			#pragma multi_compile __ _DROP_Y_ON
			//Core機能をまとめたhlslを参照可能にする。いろんな便利関数や事前定義された値が利用可能となる。
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);

			float _WaveAmplitude1;
			float _WaveSpeed1;
			float _WaveFreq1;

			float _WaveAmplitude2;
			float _WaveSpeed2;
			float _WaveFreq2;

			float _DropY;

			half4 _LightColor0;
			half _ShadowIntensity;
			half4 _TintColor;

			CBUFFER_START(UnityPerMaterial)
			float4 _MainTex_ST;
			CBUFFER_END

			#define _TIME (_Time.y)

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				half3 normal : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;

				// UVの斜め方向のパラメータを t と定義します
				float t = v.uv.x + v.uv.y;

				// 周波数とスクロール速度から t1 を決定します
				float t1 = _WaveFreq1 * t + _WaveSpeed1 * _TIME;

				// 波の高さ wave1 を計算します
				float wave1 = _WaveAmplitude1 * sin(t1);

				// wave1 を t1 で偏微分した dWave1 を計算します
				float dWave1 = _WaveFreq1 * _WaveAmplitude1 * cos(t1);

				// wave1 と同様にして wave2 を計算します
				float t2 = _WaveFreq2 * t + _WaveSpeed2 * _TIME;
				float wave2 = _WaveAmplitude2 * sin(t2);
				float dWave2 = _WaveFreq2 * _WaveAmplitude2 * cos(t2);

				// 上部を固定するための値を計算します
				float fixTopScale = (1.0f - v.uv.y);

				// 2つの波を合成して、頂点座標に反映します
				float wave = fixTopScale * (wave1 + wave2);
				v.vertex += wave;

				// 波（位置）を偏微分した勾配から、法線を計算します
				float dWave = fixTopScale * (dWave1 + dWave2);
				float3 objNormal = normalize(float3(dWave, dWave, -1.0f));
				o.normal = mul((float3x3)unity_ObjectToWorld, objNormal);

				#if _DROP_Y_ON
				{
					float a = 2.0f * _DropY - 1.0f;// -1～1 に変換
					float topY = 0.5f;// 上部のY座標
					v.vertex.y = lerp(topY, v.vertex.y, pow(saturate(v.uv.y + a), 1.0f - a));
				}
				#endif

				o.vertex = TransformObjectToHClip(v.vertex.xyz);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				col *= _TintColor * _LightColor0;
				// ライト情報を取得
				Light light = GetMainLight();
				// DirectionalLight によってライティングします
				float diffuse = saturate(dot(i.normal, light.direction));

				// 影の強さを _ShadowIntensity で調整します
				// _ShadowIntensity = 0.5 で Half-Lambert と同じ効果が得られます
				float halfLambert = lerp(1.0, diffuse, _ShadowIntensity);
				col.rgb *= halfLambert;

				return col;
			}
			ENDHLSL
		}
	}
}
