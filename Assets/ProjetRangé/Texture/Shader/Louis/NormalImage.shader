Shader "Custom/ImgShader"
{
    Properties
    {
        _TextColor("TextColor",COLOR) = (0,0,0,1)
        _MainText("Main Text", 2D) = "white"
    }
    
    SubShader
    {
        Tags
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Overlay" 
            "IgnoreProjector"="True"

        }

        Pass
        {
            Blend One OneMinusSrcAlpha
            Cull Off 
            Lighting Off 
            ZWrite Off
            ZTest Always
            Fog {Mode Off}
        
        
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            uniform sampler2D _MainText;
            uniform float4 _TextColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;

            };

            struct v2f
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;

            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            
            float4 frag (v2f IN) : COLOR
            {
                const float4 img = tex2D (_MainText, IN.uv.xy);
                const float isMask = tex2D(_MainText, IN.uv.xy) == float4(1,1,1,1);


                return isMask* _TextColor;
            }
            ENDCG
        }
    }
}
