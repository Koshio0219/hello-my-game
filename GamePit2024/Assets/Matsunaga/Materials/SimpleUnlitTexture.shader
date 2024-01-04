Shader "Custom/SimpleUnlitTextrure"
{
	//�v���p�e�B�[�u���b�N�B�r���g�C���Ɠ��l�ɊO�����瑀��\�Ȑݒ�l���`�ł���B
	Properties
	{
		//�����ɏ��������̂�Inspector�ɕ\�������B
		_Color("MainColor",Color) = (0,0,0,0)
		_MainTex("Texture", 2D) = "white" {}
	}

		//�T�u�V�F�[�_�[�u���b�N�B�����ɏ����������Ă����B
		SubShader
	{
		//�^�O�B�T�u�V�F�[�_�[�u���b�N�A�������̓p�X�����s�����^�C�~���O��������L�q����B
		Tags
		{
		//�����_�����O�̃^�C�~���O(����)
		"RenderType" = "Opaque"
		//�����_�[�p�C�v���C�����w�肷��B�Ȃ��Ă������B������𐧌���������B
		"RenderPipeline" = "UniversalRenderPipeline"
		"Queue" = "AlphaTest+51"
	}

	Pass
	{
		//HLSL������g���Ƃ����錾(���܂��Ȃ�)�B�r���g�C���ł�Cg���ꂾ�����B
		HLSLPROGRAM
		//vert�Ƃ������O�̊֐���vertex�V�F�[�_�[�ł��@�Ɛ錾����GPU�ɋ�����B
		#pragma vertex vert
		//frag�Ƃ������O�̊֐���fragment�V�F�[�_�[�ł��@�Ɛ錾����GPU�ɋ�����B
		#pragma fragment frag

		//Core�@�\���܂Ƃ߂�hlsl���Q�Ɖ\�ɂ���B�����ȕ֗��֐��⎖�O��`���ꂽ�l�����p�\�ƂȂ�B
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		//���_�V�F�[�_�[�ɓn���\���́B���O�͎����Œ�`�\�B
		struct appdata
		{
		//�I�u�W�F�N�g��Ԃɂ����钸�_���W���󂯎�邽�߂̕ϐ�
		float4 position : POSITION;

		//UV���W���󂯎�邽�߂̕ϐ�
		float2 uv : TEXCOORD0;
	};

	//�t���O�����g�V�F�[�_�[�ɓn���\���́B���O�͎����Œ�`�\�B
	struct v2f
	{
		//���_���W���󂯎�邽�߂̕ϐ��B
		float4 vertex : SV_POSITION;

		//UV���W���󂯎�邽�߂̕ϐ��B
		float2 uv : TEXCOORD0;
	};

	//�ϐ��̐錾�BProperties�Œ�`�������O�ƈ�v������B
	float4 _Color;

	//�e�X�N�`���[�T���v���p�̕ϐ��B���܂��Ȃ��B
	TEXTURE2D(_MainTex);
	SAMPLER(sampler_MainTex);

	//SRP Batcher�ւ̑Ή��BTexture�͏����Ȃ��Ă�����ɂ���Ă����B
	//_MainTex_ST��Texture���v���p�e�B�[�ɐݒ肵���ۂɎ����Œ�`�����I�t�Z�b�g��^�C�����O�p�̒l�B
	CBUFFER_START(UnityPerMaterial)
	float4 _MainTex_ST;
	CBUFFER_END

		//���_�V�F�[�_�[�B�����ɂ͎��O��`�����\���̂��n���Ă���B
		v2f vert(appdata v)
		{
		//��قǐ錾�����\���̂̃I�u�W�F�N�g�����B
		v2f o;

		//"3D�̐��E�ł̍��W��2D(�X�N���[��)�ɂ����Ă͂��̈ʒu�ɂȂ�܂���"�@�Ƃ����ϊ����֐����g���čs���Ă���B
		o.vertex = TransformObjectToHClip(v.position.xyz);

		//UV�󂯎��BTRANSFORM_TEX�ŃI�t�Z�b�g��^�C�����O�̏�����K�p�B
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);

		//�ϊ����ʂ�Ԃ��B�t���O�����g�V�F�[�_�[�֓n��B
		return o;
	}

	//�t���O�����g�V�F�[�_�[�B�����ɂ͒��_�V�F�[�_�[�ŏ������ꂽ�\���̂��n���Ă���B          
	float4 frag(v2f i) : SV_Target
	{
		//�e�X�N�`���[�̃T���v�����O�B
		float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

		//�e�N�X�`���[�̃T���v�����O���ʂ�Color�v���p�e�B�[�̒l���������킹��B
		return col * _Color;
	}
	ENDHLSL
}
	}
}