Shader "Custom/HexGridShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LineColor ("Line Color", Color) = (0,0,0,1)
        _LineWidth ("Line Width", Range(0, 0.1)) = 0.025
        [Toggle(USE_TEXTURE)] _UseTexture("Use Texture", Float) = 0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
        }
        
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature USE_TEXTURE
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _LineColor;
            float _LineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 计算到中心的距离
                float2 center = float2(0.5, 0.5);
                float2 pos = i.uv - center;
                float dist = length(pos);
                
                // 六边形边缘
                float2 hexCoord = abs(pos);
                float hexEdge = max(
                    hexCoord.x * 1.15470054, // 2/√3
                    hexCoord.y + hexCoord.x * 0.5
                );
                
                // 边线效果
                float edge = smoothstep(0.5 - _LineWidth, 0.5, hexEdge);
                
                // 最终颜色
                fixed4 col = i.color;
                #ifdef USE_TEXTURE
                    col *= tex2D(_MainTex, i.uv);
                #endif
                
                return lerp(col, _LineColor, edge);
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}
