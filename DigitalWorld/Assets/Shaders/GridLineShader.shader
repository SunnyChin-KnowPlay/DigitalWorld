Shader "Grid/GridLineShader" {
    Properties{
        _Color("Color", Color) = (1, 1, 1, 1)
        _LineWidth("Line Width", Range(0.1, 10)) = 1
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            float4 _Color;
            float _LineWidth;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutput o) {
                // Calculate line width based on uv
                float lineWidth = step(IN.uv_MainTex.x, _LineWidth) * step(IN.uv_MainTex.y, _LineWidth);

                // Set color and transparency
                o.Albedo = _Color.rgb;
                o.Alpha = _Color.a * lineWidth;
            }
            ENDCG
    }
        FallBack "Diffuse"
}
