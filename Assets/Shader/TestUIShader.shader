Shader "Custom/MinimalShader"
{
    Properties
    {
        _FillAmount("FillAmount", Float) = 1
        _GradientTex("GradientTex", 2D) = "white"
        _MainTexture("MainTexture", 2D) = "white"

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

            uniform float _FillAmount;
            uniform sampler2D _GradientTex;  
            uniform sampler2D _MainTexture;  

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
                o.uv = v.texcoord0;
                return o;
            }
            
            float4 frag (v2f IN) : COLOR
            {
                const float4 voidColor = (0,0,0,0);
                const float4 gradtext = tex2D (_GradientTex,IN.uv.xy);
                const float isMask = tex2D(_MainTexture, IN.uv.xy) == float4(0,0,0,0);

                const float checkGradient = (gradtext.x <= _FillAmount);
                float time = abs(sin(_Time.y));
                float4 test = float4(IN.uv.xy,time, 1);
                float4 retour = (checkGradient? test : voidColor);
                return (1-isMask)*retour; 
            }
            ENDCG
        }
    }
}
