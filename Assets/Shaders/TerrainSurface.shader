Shader "Custom/TerrainSurface"
{
    Properties
    {

    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        float waterLevel;
        float sandLevel;
        float groundLevel;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            if (IN.worldPos.y <= groundLevel) o.Albedo = float4(0, 1, 0, 1);
            if (IN.worldPos.y <= sandLevel) o.Albedo = float4(1, 1, 0, 1);
            if (IN.worldPos.y <= waterLevel) o.Albedo = float4(0, 0, 1, 1);
        }
        ENDCG 
    }
    FallBack "Diffuse"
}
