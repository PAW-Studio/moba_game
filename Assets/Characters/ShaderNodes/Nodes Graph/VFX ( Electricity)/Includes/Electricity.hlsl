#include <Packages/com.blendernodesgraph.core/Editor/Includes/Importers.hlsl>

void Electricity_float(float3 _POS, float3 _PVS, float3 _PWS, float3 _NOS, float3 _NVS, float3 _NWS, float3 _NTS, float3 _TWS, float3 _BTWS, float3 _UV, float3 _SP, float3 _VVS, float3 _VWS, float WIn, Texture2D gradient_189538, Texture2D gradient_189586, out float4 Color, out float3 Normal, out float Smoothness, out float4 Emission, out float AmbientOcculusion, out float Metallic, out float4 Specular)
{
	
	float _VoronoiTexture_189530_dis; float4 _VoronoiTexture_189530_col; float3 _VoronoiTexture_189530_pos; float _VoronoiTexture_189530_w; float _VoronoiTexture_189530_rad; voronoi_tex_getValue(_POS, 1.7, 1, 0, 0.5, 2, 0, 3, 0, _VoronoiTexture_189530_dis, _VoronoiTexture_189530_col, _VoronoiTexture_189530_pos, _VoronoiTexture_189530_w, _VoronoiTexture_189530_rad);
	float4 _Mapping_189498 = float4(mapping_point(float4(_POS, 0), float3(0, 0, 0), float3(0, 0, 0), 0.55), 0);
	float _VoronoiTexture_189466_dis; float4 _VoronoiTexture_189466_col; float3 _VoronoiTexture_189466_pos; float _VoronoiTexture_189466_w; float _VoronoiTexture_189466_rad; voronoi_tex_getValue(_Mapping_189498, 1.7, 1, 0, 0.5, 2, 0, 3, 2, _VoronoiTexture_189466_dis, _VoronoiTexture_189466_col, _VoronoiTexture_189466_pos, _VoronoiTexture_189466_w, _VoronoiTexture_189466_rad);
	float _Math_189474 = math_subtract(_VoronoiTexture_189530_dis, _VoronoiTexture_189466_dis, 0);
	float4 _ColorRamp_189538 = color_ramp(gradient_189538, _Math_189474);

	Color = _ColorRamp_189538;
	Normal = float3(0.0, 0.0, 0.0);
	Smoothness = 0.0;
	Emission = _ColorRamp_189538;
	AmbientOcculusion = 0.0;
	Metallic = 0.0;
	Specular = float4(0.0, 0.0, 0.0, 0.0);
}