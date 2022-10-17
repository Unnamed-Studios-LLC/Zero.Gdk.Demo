Shader "Custom/Sprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ZMin("Z Min", Float) = 1
        _ZScale("Z Scale", Float) = 1
        _ZOffset("Z Offset", Float) = 0
        _FloorColor("Floor Color", Color) = (1, 1, 1, 1)
        _EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 tint : COLOR0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float4 tint : COLOR0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ZMin;
            float _ZScale;
            float _ZOffset;
            float4 _FloorColor;
            float4 _EdgeColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.tint = v.tint;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float blend = clamp(i.worldPos.z * _ZScale, 0, 1);
                clip(-blend + 0.8);
                blend = blend > 0.5 ? 1 : 0;

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - 0.001);
                col = lerp(col, _EdgeColor, blend);
                col *= i.tint;
                col.rgb *= col.a;
                return col;
            }
            ENDCG
        }
    }
}
