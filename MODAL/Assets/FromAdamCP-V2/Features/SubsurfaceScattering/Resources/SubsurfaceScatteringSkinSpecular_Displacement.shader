// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "ADAM2/SubsurfaceScattering/Skin (Specular Setup, Displacement)"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo", 2D) = "white" {}

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        _Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
        _GlossMapScale("Smoothness Factor", Range(0.0, 1.0)) = 1.0
        [Enum(Specular Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel ("Smoothness texture channel", Float) = 0

        _SpecColor("Specular", Color) = (0.2,0.2,0.2)
        _SpecGlossMap("Specular", 2D) = "white" {}

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        _Parallax ("Height Scale", Range (0.005, 0.08)) = 0.02
        _ParallaxMap ("Height Map", 2D) = "black" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}

        _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        _DispTex ("Disp Texture", 2D) = "gray" {}
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3
        _Tess ("Tessellation", Range(1,32)) = 4
        _Phong ("Phong Tess. Strength", Range(0,1)) = 0.5

        _DetailMask("Detail Mask", 2D) = "white" {}

        _DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
        _DetailNormalMapScale("Scale", Float) = 1.0
        _DetailNormalMap("Normal Map", 2D) = "bump" {}
        _DetailSmoothnessMap("Detail Smoothness", 2D) = "white" {}

        [Enum(UV0,0,UV1,1)] _UVSec ("UV Set for secondary textures", Float) = 0
        
        [MaterialToggle] _IsAlembic("Is Alembic", Float) = 0

        _SubsurfaceProfile("Subsurface Profile", Int) = 0
        _SubsurfaceRadius("Subsurface Radius", Range(0.0, 1.0)) = 1.0
        _SubsurfaceRadiusMap("Subsurface Radius Map", 2D) = "white" {}
        _Thickness("Thickness", Range(0.0, 1.0)) = 1.0
        _ThicknessMap("Thickness Map", 2D) = "white" {}

        _SpecularLobeInterpolation("Lobe Interpolation", Range(0.0, 1.0)) = 0.15
        _SecondLobeRoughnessDerivation("2nd Lobe Roughness Derivation", Range(0.0, 2.0)) = 0.65 


        // Blending state
        [HideInInspector] _Mode ("__mode", Float) = 0.0
        [HideInInspector] _SrcBlend ("__src", Float) = 1.0
        [HideInInspector] _DstBlend ("__dst", Float) = 0.0
        [HideInInspector] _ZWrite ("__zw", Float) = 1.0
    }

    CGINCLUDE
        #define SUBSURFACE_MATERIAL_SKIN
        #define UNITY_SETUP_BRDF_INPUT SpecularSetup
    ENDCG

    SubShader
    {

        Tags { "RenderType"="Opaque" "PerformanceChecks"="False" }
        LOD 300       
        
        Pass //TODO: This seems to be drawn twice by the internal renderer.
        {
            Name "SSSParamWrite"

            ZWrite Off
            ZTest LEqual

            CGPROGRAM
            #pragma target 5.0

            #pragma shader_feature _THICKNESSMAP
            #pragma shader_feature _SUBSURFACE_RADIUS_MAP

            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature _SPECGLOSSMAP
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature ___ _DETAIL_MULX2
            #pragma shader_feature _PARALLAXMAP
            
            #pragma multi_compile_prepassfinal
            #pragma multi_compile_instancing
			
            #define SAMPLE_NORMAL_RAW

            #define TESSELATION_DEFERRED
            #pragma vertex   tessvert_standard
            #pragma fragment fragDeferred
			#pragma hull     hs_standard
			#pragma domain   ds_standard

			#include "Tessellation.cginc"
			#include "TessellationStandard.cginc"

            ENDCG
        }
        
        UsePass "Hidden/AlembicMotionVectors/MOTIONVECTORS"

        // ------------------------------------------------------------------
        //  Base forward pass (directional light, emission, lightmaps, ...)
        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }

            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]

            Stencil {
                Ref 1 //SSS
                Comp Always
                Pass Replace
            }
            

            CGPROGRAM
            #pragma target 5.0

            // -------------------------------------

            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature _SPECGLOSSMAP
            #pragma shader_feature ___ _DETAIL_MULX2
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature _ _GLOSSYREFLECTIONS_OFF
            #pragma shader_feature _PARALLAXMAP

            #pragma shader_feature _DETAIL_SMOOTHNESS

            #pragma shader_feature _THICKNESSMAP
            #pragma shader_feature _SUBSURFACE_RADIUS_MAP

            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #define SAMPLE_NORMAL_RAW

            #define TESSELATION_BASE
            #pragma vertex   tessvert_standard
            #pragma fragment fragBase
			#pragma hull     hs_standard
			#pragma domain   ds_standard

			#include "Tessellation.cginc"
			#include "TessellationStandard.cginc"

            ENDCG
        }


        // // ------------------------------------------------------------------
        // //  Additive forward pass (one light per pass)
        Pass
        {
            Name "FORWARD_DELTA"
            Tags { "LightMode" = "ForwardAdd" }

            Blend One One

            Fog { Color (0,0,0,0) } // in additive pass fog should be black
            ZWrite Off
            ZTest LEqual

            CGPROGRAM
            #pragma target 5.0

            // -------------------------------------

            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _SPECGLOSSMAP
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature ___ _DETAIL_MULX2
            #pragma shader_feature _PARALLAXMAP

            #pragma shader_feature _DETAIL_SMOOTHNESS

            #pragma shader_feature _THICKNESSMAP

            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog

            #define SAMPLE_NORMAL_RAW

            #define TESSELATION_ADD
            #pragma vertex   tessvert_standard
            #pragma fragment fragAdd
			#pragma hull     hs_standard
			#pragma domain   ds_standard

            #include "Tessellation.cginc"
			#include "TessellationStandard.cginc"

            ENDCG
        }

        // ------------------------------------------------------------------
        //  Shadow rendering pass
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On ZTest LEqual

            CGPROGRAM
            #pragma target 5.0

            // -------------------------------------

            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _SPECGLOSSMAP
            #pragma shader_feature _PARALLAXMAP
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing

            #pragma vertex   tessvert_shadow
			#pragma fragment fragShadowCaster
			#pragma hull   hs_shadow
			#pragma domain ds_shadow

            #include "Tessellation.cginc"
			#include "TessellationStandardShadow.cginc"

            ENDCG
        }
 
    }

    FallBack Off
    CustomEditor "SubsurfaceScatteringSkin_DisplacementGUI"
}
