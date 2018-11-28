Shader "Custom/LifeEnergyShader"
{
    Properties
    {
        _Life("Life", Float) = 1
        _LifeColor("LifeColor", Color) = (1,1,1,1)
        _FeedBackColor("FeedbackColor", Color) = (1,1,0,1)
        _LifeFeedbackAmount("LifeFeedbackAmount", Float) = 0
        _LifeBeforeShown("LifeBeforShown", float) = 0.66

        _Stamina("Stamina", Float) = 1
        _StaminaColor("StaminaColor", Color) = (1,1,1,1)
        
        _MainTex("Main", 2D) = "white"
        _DisplayLife ("DisplayLife", Int) = 0
    }
    
    SubShader
    {
        Tags
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off 
            Lighting Off 
            ZWrite Off
            ZTest LEqual
        
            CGPROGRAM
            #pragma target 3.5
            #pragma vertex vert
            #pragma fragment frag

            uniform sampler2D _MainTex;            
            uniform float _Life;
            uniform float4 _LifeColor;
            uniform float4 _FeedBackColor;
            uniform float _Stamina;
            uniform float4 _StaminaColor;
            uniform float _LifeBeforeShown;
            uniform float _LifeFeedbackAmount;
            uniform float _DisplayLife;

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
                const float checkLife = (_Life <= 0.66);
                const float lifeDistCheck = (IN.uv.y<0.5+_Life/2 && IN.uv.y>=0.5-_Life/2 && (checkLife || _DisplayLife));
                const float stamDistCheck = (IN.uv.y<0.5+_Stamina/2 && IN.uv.y>=0.5-_Stamina/2);
                const float checkFeedback = (IN.uv.y>0.5+_Life/2 && IN.uv.y<=0.5+_LifeFeedbackAmount/2) || (IN.uv.y<0.5-_Life/2 && IN.uv.y>=0.5-_LifeFeedbackAmount/2);
                const float isRighOrLeft = (IN.uv.x>0.5);
                const float isMask = tex2D(_MainTex, IN.uv.xy) == float4(1,1,1,1);

                const float4 voidColor = float4 (0,0,0,0);

                float4 dynamicStamina = float4 (_StaminaColor.xyz,1-_Stamina);
                const float checkDynamic = (dynamicStamina.w>=_StaminaColor.w);
                dynamicStamina = (checkDynamic ? _StaminaColor : dynamicStamina);

                const float feedBack = (checkFeedback ? _FeedBackColor :  voidColor);

                const float4 tempLife = (lifeDistCheck ? _LifeColor :  voidColor) + feedBack;
                const float checkLifePercent = (_Life<_LifeBeforeShown);
                
                float4 actualLife = (checkLifePercent || _DisplayLife ? tempLife : voidColor);

                const float4 actualStamina = (stamDistCheck ? dynamicStamina :  voidColor);
                const float4 posStamLie = (isRighOrLeft? actualLife : actualStamina);

                return posStamLie*(isMask) +  voidColor*(1-isMask);
            }
            ENDCG
        }
    }
}
