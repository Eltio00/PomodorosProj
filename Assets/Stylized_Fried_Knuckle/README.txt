================================================================================
                       STYLIZED FRIED KNUCKLE — ASSET PACK
================================================================================

Thank you for purchasing Stylized Fried Knuckle!
This asset pack contains a collection of 21 stylized, hand-crafted 3D props
designed for creating cozy kitchen, tavern, and food-themed environments
centered around a hearty pork knuckle dinner scene.


--------------------------------------------------------------------------------
  PACKAGE CONTENTS
--------------------------------------------------------------------------------

  21  Unique 3D meshes (.fbx)
  42  Prefabs (21 URP + 21 Built-In)
  22  Materials (11 URP + 11 Built-In, including 1 demo floor per pipeline)
  26  Textures (.png) — AlbedoTransparency, Normal, MetallicSmoothness
   2  Demo scenes (URP + Built-In)


--------------------------------------------------------------------------------
  INCLUDED MODELS
--------------------------------------------------------------------------------

  Main Dish:        PorkKnuckle (x2)

  Vegetables:       BellPepper (x3), Onion (x4), Potato (x2),
                    Garlic (x2), GarlicClove

  Utensils:         Fork, Knife, Board

  Containers:       Tincan, PotatoSack

  Decor:            Candle, Fabric


--------------------------------------------------------------------------------
  SUPPORTED RENDER PIPELINES
--------------------------------------------------------------------------------

  - Built-In Render Pipeline
  - Universal Render Pipeline (URP)

  NOTE: HDRP is not supported.


--------------------------------------------------------------------------------
  REQUIREMENTS
--------------------------------------------------------------------------------

  - Unity 2021 LTS or newer


--------------------------------------------------------------------------------
  INSTALLATION
--------------------------------------------------------------------------------

  1. Open your Unity project.
  2. Go to Assets > Import Package > Custom Package.
  3. Select the Stylized_Pork_Knuckle.unitypackage file.
  4. Click "Import" to add all assets to your project.
  5. Open the demo scene matching your render pipeline:
     - URP:      Assets/Stylized Fried Knuckle/Scenes/Demo_URP.unity
     - Built-In: Assets/Stylized Fried Knuckle/Scenes/Demo_Built-In.unity


--------------------------------------------------------------------------------
  FOLDER STRUCTURE
--------------------------------------------------------------------------------

  Assets/Stylized Fried Knuckle/
  |
  |-- Materials/
  |   |-- Built-In/          Built-In render pipeline materials (11)
  |   |-- URP/               URP materials (11)
  |
  |-- Meshes/                FBX source meshes (21 models)
  |
  |-- Prefabs/
  |   |-- Built-In/          Ready-to-use Built-In prefabs (21)
  |   |-- URP/               Ready-to-use URP prefabs (21)
  |
  |-- Scenes/                Demo scenes for each pipeline
  |
  |-- Textures/              PBR texture sets organized by object
      |-- T_[SetName]/
          |-- *_AlbedoTransparency.png
          |-- *_Normal.png
          |-- *_MetallicSmoothness.png
      |-- T_Pork_Knuckle/
          |-- T_Pork_Knuckle_*.png         (meat material)
          |-- T_Pork_Knuckle_Bone/
              |-- T_Pork_Knuckle_Bone_*.png   (bone material)


--------------------------------------------------------------------------------
  TEXTURES
--------------------------------------------------------------------------------

  Format:    PNG (lossless)
  Workflow:  Metallic/Smoothness (PBR)

  Each texture set includes:
    - AlbedoTransparency  (Base color + alpha)
    - Normal              (Tangent-space normal map, OpenGL format)
    - MetallicSmoothness  (Metallic in R, Smoothness in A)

  Note: Some sets (PropsSetC, PropsSetD) ship with two AlbedoTransparency
  variants (_1 and _2) that share the same Normal and MetallicSmoothness
  maps. Each variant is paired with its own Material, so meshes that share
  a set can display different colors while reusing UVs and packed maps.


--------------------------------------------------------------------------------
  TEXTURE SETS & SHARED ATLASES
--------------------------------------------------------------------------------

  Smaller props share a single texture atlas for better performance.

  T_Fabric ............ SM_Fabric
  T_Pork_Knuckle ...... SM_Pork_Knuckle, SM_Pork_Knuckle_2 (meat slot)
  T_Pork_Knuckle_Bone . SM_Pork_Knuckle_2 (bone slot)
  T_PropsSetA ......... SM_Tincan, SM_Candle
  T_PropsSetB ......... SM_Fork, SM_Knife
  T_PropsSetC ......... SM_Garlic_01, SM_Garlic_02, SM_Garlic_Clove,
                        SM_Onion_01 — SM_Onion_04
                        (two AlbedoTransparency variants for the onion colors)
  T_PropsSetD ......... SM_Potato_01, SM_Potato_02,
                        SM_BellPepper_01 — SM_BellPepper_03
                        (two AlbedoTransparency variants for the pepper colors)
  T_PropsSetI ......... SM_Potato_Sack, SM_Board


--------------------------------------------------------------------------------
  NAMING CONVENTIONS
--------------------------------------------------------------------------------

  Meshes:      SM_[ObjectName].fbx
  Prefabs:     P_[ObjectName].prefab
  Materials:   T_[SetName].mat
  Textures:    T_[SetName]_[MapType].png


--------------------------------------------------------------------------------
  TIPS FOR BEST RESULTS
--------------------------------------------------------------------------------

  - Use the prefabs from the folder matching your project's render pipeline.
  - All prefabs are set to position (0, 0, 0) with scale (1, 1, 1).
  - Meshes use real-world scale (1 Unity unit = 1 meter).
  - Check the demo scene for reference on how to arrange the props.
  - Props sharing a texture atlas are designed to be used together
    in the same scene for optimal draw-call batching.
  - SM_Pork_Knuckle_2 uses two materials (meat + bone) — keep both
    T_Pork_Knuckle and T_Pork_Knuckle_Bone assigned for the correct look.


--------------------------------------------------------------------------------
  SUPPORT
--------------------------------------------------------------------------------

  If you have any questions or issues, feel free to reach out
  through the Fab.com messaging system.


--------------------------------------------------------------------------------
  VERSION HISTORY
--------------------------------------------------------------------------------

  v1.0  -  Initial release


================================================================================
