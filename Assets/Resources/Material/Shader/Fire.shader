Shader "Custom/Fire"
{
    Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Tex_Noise("Albedo2 (RGB)", 2D) = "white" {}

		_Noise_Power("_Noise_Power", Range(0, 1)) = 0.1
		_Noise_Speed("_Noise_Speed", Range(0, 10)) = 1.0

		_Emission_Power("_Emission_Power", Range(0, 10)) = 2.0
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent"  "Queue" = "Transparent"}
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard alpha:fade

			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _Tex_Noise;

			float _Noise_Power;
			float _Noise_Speed;
			float _Emission_Power;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_Tex_Noise;
			};


			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				fixed4 d = tex2D(_Tex_Noise, IN.uv_Tex_Noise + float2(0.0f, 1 - (_Time.y * _Noise_Speed)));
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex + d.r * _Noise_Power);

				o.Emission = c.rgb * _Emission_Power;

				o.Alpha = c.a * d.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
