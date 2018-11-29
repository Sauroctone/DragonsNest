Shader "Custom/UIEggShader"
{
    Properties
    {
        _FillAmount("FillAmount", Float) = 1
        _ColorBase("Color Base", COLOR) = (0,0,0,1)
        _ColorTextFL("Color Full Amount", COLOR) = (1,1,1,1)
        _ColorTextLL("Color Low Amount", COLOR) = (1,1,1,1)
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
            uniform float4 _ColorTextFL;
            uniform float4 _ColorTextLL;
            uniform float4 _ColorBase;

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
                const float4 voidColor = float4 (0,0,0,0);
                
                const float4 gradtext = tex2D (_GradientTex,IN.uv.xy);
                
                const float isMask = tex2D(_MainTexture, IN.uv.xy) == float4(1,1,1,1);
                
                const float4 color = lerp(_ColorTextLL,_ColorTextFL,_FillAmount);
                
                const float checkGradient = (gradtext.x <= _FillAmount);
                
                float4 retour = (checkGradient? color : _ColorBase);
    
                return (isMask)*retour; 
            }
            ENDCG
        }
    }
}
