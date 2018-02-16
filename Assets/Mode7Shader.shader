Shader "Custom/Mode7Shader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

	_TextureSizeX("Texture Size X", Float) = 512
		_TextureSizeY("Texture Size Y", Float) = 512

		_H("H", Float) = 0
		_V("V", Float) = 0
		_X0("Pivot X", Float) = 0
		_Y0("Pivot Y", Float) = 0
		_A("A", Float) = 1
		_B("B", Float) = 0
		_C("C", Float) = 0
		_D("D", Float) = 1

		_BorderCol("Border Colour", Color) = (1, 1, 1, 1)

		_BorderMode("Border Mode", Int) = 0
	}

		SubShader
	{
		Tags
	{
		"PreviewType" = "Plane"
		"Queue" = "Transparent"
	}
		Pass
	{
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

	sampler2D _MainTex;
	float4 _MainTex_TexelSize;

	float _H;
	float _V;
	float _X0;
	float _Y0;
	float _A;
	float _B;
	float _C;
	float _D;

	float _TextureSizeX;
	float _TextureSizeY;

	float4 _BorderCol;
	int _BorderMode;

	float4 frag(v2f i) : SV_Target
	{
		float x = i.uv.x * 0.25;
	float y = (i.uv.y - 1) * -1 * 7.0 / 32;

	float xi = x + (_H - _X0) / 4;
	float yi = y + (_V - _Y0) / 4;

	float2 pixel = float2(
		_A * xi + _B * yi + _X0 / 4,
		_C * xi + _D * yi + _Y0 / 4);

	pixel = float2(pixel.x, (pixel.y - 1) * -1);

	int value = (1 - ((int)min(floor(abs(pixel.x - 0.5) * 2), 1) | (int)min(floor(abs(pixel.y - 0.5) * 2), 1))) | (int)(max(1 - abs(_BorderMode), 0));

	float4 colour = tex2D(_MainTex, pixel) * float4(value, value, value, value) + _BorderCol * float4(1 - value, 1 - value, 1 - value, 1 - value);

	return colour;
	}
		ENDCG
	}
	}
}
