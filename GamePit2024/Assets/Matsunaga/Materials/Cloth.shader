Shader "Custom/Cloth"
{
	//Copyright(c) 2019 @gam0022
	//	(Sho Hosoda)
	//	https ://qiita.com/gam0022/items/6fad2d8df583c70d417f
	//	Released under the MIT license
	//	https ://github.com/YukinobuKurata/YouTubeMagicBuyButton/blob/master/MIT-LICENSE.txt
	//�v���p�e�B�[�u���b�N�B�r���g�C���Ɠ��l�ɊO�����瑀��\�Ȑݒ�l���`�ł���B
	Properties
	{
		// ���C���e�N�X�`��
		_MainTex("Texture", 2D) = "white" {}

		[Header(Wave 1)]
		_WaveAmplitude1("Amplitude", Range(0, 1)) = 0.025// �U��
		_WaveSpeed1("Speed", Range(-30, 30)) = 9.8// ���x
		_WaveFreq1("Freq", Range(0, 30)) = 8.5// ���g��

		[Header(Wave 2)]
		_WaveAmplitude2("Amplitude", Range(0, 1)) = 0.0045
		_WaveSpeed2("Speed", Range(-30, 30)) = 15.4
		_WaveFreq2("Freq", Range(0, 30)) = 25.3

		// �����@�\
		[Header(Drop)]
		[Toggle] _DROP_Y("Enable Drop Y", Float) = 0
		_DropY("Drop Y", Range(0  , 1)) = 1

		[Header(Lighting)]
		_LightDirection("Light Direction", Vector) = (1.0, 3.0, 2.0, 0.0)
		_ShadowIntensity("Shadow Intensity", Range(0.0, 1.0)) = 0.5

		// ��Z�J���[
		[HDR] _TintColor("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)

		// �J�����O���[�h
		[Header(Culling)]
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", Float) = 2// Back
	}

		//�T�u�V�F�[�_�[�u���b�N�B�����ɏ����������Ă����B
		SubShader
	{
		//�^�O�B�T�u�V�F�[�_�[�u���b�N�A�������̓p�X�����s�����^�C�~���O��������L�q����B
		Tags
		{
			"Queue" = "AlphaTest+51"
			//�����_�����O�̃^�C�~���O(����)
			"RenderType" = "Opaque"
			//�����_�[�p�C�v���C�����w�肷��B�Ȃ��Ă������B������𐧌���������B
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
			//HLSL������g���Ƃ����錾(���܂��Ȃ�)�B�r���g�C���ł�Cg���ꂾ�����B
			HLSLPROGRAM
			//vert�Ƃ������O�̊֐���vertex�V�F�[�_�[�ł��@�Ɛ錾����GPU�ɋ�����B
			#pragma vertex vert
			//frag�Ƃ������O�̊֐���fragment�V�F�[�_�[�ł��@�Ɛ錾����GPU�ɋ�����B
			#pragma fragment frag
			#pragma multi_compile __ _DROP_Y_ON
			//Core�@�\���܂Ƃ߂�hlsl���Q�Ɖ\�ɂ���B�����ȕ֗��֐��⎖�O��`���ꂽ�l�����p�\�ƂȂ�B
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

				// UV�̎΂ߕ����̃p�����[�^�� t �ƒ�`���܂�
				float t = v.uv.x + v.uv.y;

				// ���g���ƃX�N���[�����x���� t1 �����肵�܂�
				float t1 = _WaveFreq1 * t + _WaveSpeed1 * _TIME;

				// �g�̍��� wave1 ���v�Z���܂�
				float wave1 = _WaveAmplitude1 * sin(t1);

				// wave1 �� t1 �ŕΔ������� dWave1 ���v�Z���܂�
				float dWave1 = _WaveFreq1 * _WaveAmplitude1 * cos(t1);

				// wave1 �Ɠ��l�ɂ��� wave2 ���v�Z���܂�
				float t2 = _WaveFreq2 * t + _WaveSpeed2 * _TIME;
				float wave2 = _WaveAmplitude2 * sin(t2);
				float dWave2 = _WaveFreq2 * _WaveAmplitude2 * cos(t2);

				// �㕔���Œ肷�邽�߂̒l���v�Z���܂�
				float fixTopScale = (1.0f - v.uv.y);

				// 2�̔g���������āA���_���W�ɔ��f���܂�
				float wave = fixTopScale * (wave1 + wave2);
				v.vertex += wave;

				// �g�i�ʒu�j��Δ����������z����A�@�����v�Z���܂�
				float dWave = fixTopScale * (dWave1 + dWave2);
				float3 objNormal = normalize(float3(dWave, dWave, -1.0f));
				o.normal = mul((float3x3)unity_ObjectToWorld, objNormal);

				#if _DROP_Y_ON
				{
					float a = 2.0f * _DropY - 1.0f;// -1�`1 �ɕϊ�
					float topY = 0.5f;// �㕔��Y���W
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
				// ���C�g�����擾
				Light light = GetMainLight();
				// DirectionalLight �ɂ���ă��C�e�B���O���܂�
				float diffuse = saturate(dot(i.normal, light.direction));

				// �e�̋����� _ShadowIntensity �Œ������܂�
				// _ShadowIntensity = 0.5 �� Half-Lambert �Ɠ������ʂ������܂�
				float halfLambert = lerp(1.0, diffuse, _ShadowIntensity);
				col.rgb *= halfLambert;

				return col;
			}
			ENDHLSL
		}
	}
}
