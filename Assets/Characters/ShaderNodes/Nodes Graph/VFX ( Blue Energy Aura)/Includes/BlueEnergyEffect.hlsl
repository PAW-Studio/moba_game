#include <Packages/com.blendernodesgraph.core/Editor/Includes/Importers.hlsl>

void BlueEnergyEffect_float(float3 _POS, float3 _PVS, float3 _PWS, float3 _NOS, float3 _NVS, float3 _NWS, float3 _NTS, float3 _TWS, float3 _BTWS, float3 _UV, float3 _SP, float3 _VVS, float3 _VWS, float name, Texture2D gradient_55008, out float4 Color, out float3 Normal, out float Smoothness, out float4 Emission, out float AmbientOcculusion, out float Metallic, out float4 Specular)
{
	
	float _Math_54928 = math_divide(1, 1.5, 0);
	float4 _CombineXYZ_54920 = float4(combine_xyz(1, 1, _Math_54928), 0);
	float4 _Mapping_54912 = float4(mapping_point(float4(_POS, 0), float3(0, 0, 0), float3(0, 0, 0), _CombineXYZ_54920), 0);
	float _GradientTexture_54904_fac; float4 _GradientTexture_54904_col; node_tex_gradient(_Mapping_54912, 0, _GradientTexture_54904_fac, _GradientTexture_54904_col);
	float _Math_54944 = math_power(_GradientTexture_54904_col, 4, 0);
	float _Math_54976 = math_multiply(name, 5, 0);
	float _Math_54992 = math_divide(0.29, 8, 0);
	float4 _CombineXYZ_54984 = float4(combine_xyz(0.29, 0.29, _Math_54992), 0);
	float4 _Mapping_54952 = float4(mapping_point(float4(_POS, 0), float3(0, 0, -0.6375), float3(0, 0, 0), _CombineXYZ_54984), 0);
	float _SimpleNoiseTexture_55040_fac; float4 _SimpleNoiseTexture_55040_col; node_simple_noise_texture_full(_Mapping_54952, _Math_54976, 6, 2.95, 0.7116668, 2, 2, _SimpleNoiseTexture_55040_fac, _SimpleNoiseTexture_55040_col);
	float4 _ColorRamp_55008 = color_ramp(gradient_55008, _SimpleNoiseTexture_55040_fac);
	float _Math_55016 = math_power(_ColorRamp_55008, 1.2, 0);
	float _Math_54896 = math_multiply(_Math_54944, _Math_55016, 0);

	Color = _Math_54896;
	Normal = float3(0.0, 0.0, 0.0);
	Smoothness = 0.0;
	Emission = _Math_54896;
	AmbientOcculusion = 0.0;
	Metallic = 0.0;
	Specular = float4(0.0, 0.0, 0.0, 0.0);
}