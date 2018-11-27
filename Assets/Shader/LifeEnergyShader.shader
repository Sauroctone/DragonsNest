Shader "Custom/LifeEnergyShader"
{
    Properties
    {
        _Life("Life", Float) = 1
        _LifeColor("LifeColor", Color) = (1,1,1,1)
        _Stamina("Stamina", Float) = 1
        _StaminaColor("StaminaColor", Color) = (1,1,1,1)
        _MainTex("Main", 2D) = "white"
    }
    
    SubShader
    {
        Tags
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent+0" 
        }

        Pass
        {
            Blend One OneMinusSrcAlpha
            Cull Off 
            Lighting Off 
            ZWrite On
            ZTest LEqual
        
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            uniform sampler2D _MainTex;            
            uniform float _Life;
            uniform float4 _LifeColor;
            uniform float _Stamina;
            uniform float4 _StaminaColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag (v2f IN) : COLOR
            {
                const float lifeDistCheck = (IN.uv.y<0.5+_Life/2 && IN.uv.y>=0.5-_Life/2);
                const float stamDistCheck = (IN.uv.y<0.5+_Stamina/2 && IN.uv.y>=0.5-_Stamina/2);
                const float isRighOrLeft = (IN.uv.x>0.5);
                const float isMask = tex2D(_MainTex, IN.uv.xy) == float4(1,1,1,1);

                const float4 voidColor = float4 (0,0,0,0);

                const float4 actualLife = (lifeDistCheck ? _LifeColor :  voidColor);
                const float4 actualStamina = (stamDistCheck ? _StaminaColor :  voidColor);
                const float4 posStamLie = (isRighOrLeft? actualLife : actualStamina);

                return posStamLie*(isMask) +  voidColor*(1-isMask);
            }
            ENDCG
        }
    }
}
