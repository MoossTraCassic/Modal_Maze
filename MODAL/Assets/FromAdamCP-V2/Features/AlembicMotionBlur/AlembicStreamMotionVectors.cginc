#include "UnityCG.cginc"

		#define VEL_SCALE	(1)

        // Object rendering things

#if defined(USING_STEREO_MATRICES)
        float4x4 _StereoNonJitteredVP[2];
        float4x4 _StereoPreviousVP[2];
#else
        float4x4 _NonJitteredVP;
        float4x4 _PreviousVP;
#endif
        float4x4 _PreviousM;
        bool _HasLastPositionData;
        bool _ForceNoMotion;
        float _MotionVectorDepthBias;

        struct MotionVectorData
        {
            float4 transferPos : TEXCOORD0;
            float4 transferPosOld : TEXCOORD1;
            float4 pos : SV_POSITION;

#if defined(IS_ALEMBIC)
            float4 transferStreamPosOld : TEXCOORD2;
#endif

            UNITY_VERTEX_OUTPUT_STEREO
        };

        struct MotionVertexInput
        {
            float4 vertex : POSITION;
            float3 oldPos : NORMAL;

#if defined(IS_ALEMBIC)
            float2 velocityXY : TEXCOORD2;
            float2 velocityZ  : TEXCOORD3;
#endif

            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        MotionVectorData VertMotionVectors(MotionVertexInput v)
        {
            MotionVectorData o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
            o.pos = UnityObjectToClipPos(v.vertex);

            // this works around an issue with dynamic batching
            // potentially remove in 5.4 when we use instancing
#if defined(UNITY_REVERSED_Z)
            o.pos.z -= _MotionVectorDepthBias * o.pos.w;
#else
            o.pos.z += _MotionVectorDepthBias * o.pos.w;
#endif

#if defined(USING_STEREO_MATRICES)
            o.transferPos = mul(_StereoNonJitteredVP[unity_StereoEyeIndex], mul(unity_ObjectToWorld, v.vertex));
            o.transferPosOld = mul(_StereoPreviousVP[unity_StereoEyeIndex], mul(_PreviousM, _HasLastPositionData ? float4(v.oldPos, 1) : v.vertex));
#else
            o.transferPos = mul(_NonJitteredVP, mul(unity_ObjectToWorld, v.vertex));
            o.transferPosOld = mul(_PreviousVP, mul(_PreviousM, _HasLastPositionData ? float4(v.oldPos, 1) : v.vertex));

            #if defined(IS_ALEMBIC)
            //Recontruct the previous stream position.
            float3 streamVelocity = float3(v.velocityXY, v.velocityZ.x);
            float3 streamPosOld = v.vertex + (streamVelocity * -unity_DeltaTime.x * VEL_SCALE);
            o.transferStreamPosOld = mul(_PreviousVP, mul(_PreviousM, float4(streamPosOld, 1)));
            #endif            
#endif

            return o;
        }

        half4 FragMotionVectors(MotionVectorData i) : SV_Target
        {
            float3 hPos = (i.transferPos.xyz / i.transferPos.w);
            float3 hPosOld = (i.transferPosOld.xyz / i.transferPosOld.w);

            #if defined(IS_ALEMBIC)
            float3 hStreamPosOld = (i.transferStreamPosOld.xyz / i.transferStreamPosOld.w);
            #endif

            // V is the viewport position at this pixel in the range 0 to 1.
            float2 vPos = (hPos.xy + 1.0f) / 2.0f;
            float2 vPosOld = (hPosOld.xy + 1.0f) / 2.0f;
            
            #if defined(IS_ALEMBIC)
            float2 vStreamPosOld = (hStreamPosOld.xy + 1.0f) / 2.0f;
            #endif

#if UNITY_UV_STARTS_AT_TOP
            vPos.y = 1.0 - vPos.y;
            vPosOld.y = 1.0 - vPosOld.y;

            #if defined(IS_ALEMBIC)
            vStreamPosOld.y = 1.0 - vStreamPosOld.y;
            #endif
#endif

            half2 uvDiffAlembic;
            #if defined(IS_ALEMBIC)
            uvDiffAlembic = vPos - vStreamPosOld;
            #else
            uvDiffAlembic = half2(0, 0);
            #endif

            half2 uvDiff = (vPos - vPosOld) + uvDiffAlembic;
            return lerp(half4(uvDiff, 0, 1), 0, (half)_ForceNoMotion);
        }