#include <Packages/com.blendernodesgraph.core/Editor/Includes/Importers.hlsl>

void Nerves_float(float3 _POS, float3 _PVS, float3 _PWS, float3 _NOS, float3 _NVS, float3 _NWS, float3 _NTS, float3 _TWS, float3 _BTWS, float3 _UV, float3 _SP, float3 _VVS, float3 _VWS, float name, Texture2D gradient_11198, out float4 Color, out float3 Normal, out float Smoothness, out float4 Emission, out float AmbientOcculusion, out float Metallic, out float4 Specular)
{
	
	float4 _Mapping_11204 = float4(mapping_point(float4(_POS, 0), float3(0, 0, 0), float3(0, 0, 0), float3(1, 1, 1)), 0);
	float _VoronoiTexture_11196_dis; float4 _VoronoiTexture_11196_col; float3 _VoronoiTexture_11196_pos; float _VoronoiTexture_11196_w; float _VoronoiTexture_11196_rad; voronoi_tex_getValue(_Mapping_11204, name, 2, 0.2, 0.7583333, 1, 0, 3, 0, _VoronoiTexture_11196_dis, _VoronoiTexture_11196_col, _VoronoiTexture_11196_pos, _VoronoiTexture_11196_w, _VoronoiTexture_11196_rad);
	float4 _ColorRamp_11198 = color_ramp(gradient_11198, _VoronoiTexture_11196_dis);

	Color = _ColorRamp_11198;
	Normal = float3(0.0, 0.0, 0.0);
	Smoothness = 0.0;
	Emission = _ColorRamp_11198;
	AmbientOcculusion = 0.0;
	Metallic = 0.0;
	Specular = float4(0.0, 0.0, 0.0, 0.0);
}