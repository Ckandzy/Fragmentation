using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(TakeDamager))]
public class TakeDamagerEditor : Editor
{
    static BoxBoundsHandle s_BoxBoundsHandle = new BoxBoundsHandle();
    static Color s_EnabledColor = Color.green + Color.grey;
    
    SerializedProperty m_HitPointProp;
    SerializedProperty m_RefreshDamageTimeProp;
    SerializedProperty m_OnHitMissingProp;
    SerializedProperty m_StatusProp;
    SerializedProperty m_DamageBuffIDs;
    SerializedProperty m_DamageNum;
    SerializedProperty m_OffsetProp;
    SerializedProperty m_SizeProp;
    SerializedProperty m_OffsetBasedOnSpriteFacingProp;
    SerializedProperty m_SpriteRendererProp;
    SerializedProperty m_CanHitTriggersProp;
    SerializedProperty m_ForceRespawnProp;
    SerializedProperty m_IgnoreInvincibilityProp;
    SerializedProperty m_HittableLayersProp;
    SerializedProperty m_OnDamageableHitProp;
    SerializedProperty m_OnNonDamageableHitProp;

    void OnEnable()
    {
        //RequiresConstantRepaint();
        // m_DamageBuffIDs = serializedObject.FindProperty("BuffIds");
        // m_DamageNum = serializedObject.FindProperty("status");
        m_RefreshDamageTimeProp = serializedObject.FindProperty("RefreshDamageTime");
        m_HitPointProp = serializedObject.FindProperty("hitPoint");
        m_StatusProp = serializedObject.FindProperty("status");
        m_OffsetProp = serializedObject.FindProperty("offset");
        m_SizeProp = serializedObject.FindProperty("size");
        m_OffsetBasedOnSpriteFacingProp = serializedObject.FindProperty("offsetBasedOnSpriteFacing");
        m_SpriteRendererProp = serializedObject.FindProperty("spriteRenderer");
        m_CanHitTriggersProp = serializedObject.FindProperty("canHitTriggers");
        m_ForceRespawnProp = serializedObject.FindProperty("forceRespawn");
        m_IgnoreInvincibilityProp = serializedObject.FindProperty("ignoreInvincibility");
        m_HittableLayersProp = serializedObject.FindProperty("hittableLayers");
        m_OnDamageableHitProp = serializedObject.FindProperty("OnDamageableHit");
        m_OnNonDamageableHitProp = serializedObject.FindProperty("OnNonDamageableHit");
        m_OnHitMissingProp = serializedObject.FindProperty("OnHitMissing");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //EditorGUILayout.PropertyField(m_DamageNum);
        //EditorGUILayout.PropertyField(m_DamageBuffIDs, true);
        EditorGUILayout.PropertyField(m_HitPointProp);
        EditorGUILayout.PropertyField(m_RefreshDamageTimeProp);
        EditorGUILayout.PropertyField(m_StatusProp);
        EditorGUILayout.PropertyField(m_OffsetProp);
        EditorGUILayout.PropertyField(m_SizeProp);
        EditorGUILayout.PropertyField(m_OffsetBasedOnSpriteFacingProp);
        if (m_OffsetBasedOnSpriteFacingProp.boolValue)
            EditorGUILayout.PropertyField(m_SpriteRendererProp);
        EditorGUILayout.PropertyField(m_CanHitTriggersProp);
        EditorGUILayout.PropertyField(m_ForceRespawnProp);
        EditorGUILayout.PropertyField(m_IgnoreInvincibilityProp);
        EditorGUILayout.PropertyField(m_HittableLayersProp);
        EditorGUILayout.PropertyField(m_OnDamageableHitProp);
        EditorGUILayout.PropertyField(m_OnNonDamageableHitProp);
        EditorGUILayout.PropertyField(m_OnHitMissingProp);
        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        TakeDamager damager = (TakeDamager)target;
        
        if (!damager.enabled)
            return;

        Matrix4x4 handleMatrix = damager.transform.localToWorldMatrix;
        ////Hotkang 2018.7.24 疑问: 用处何在？ 注释掉后暂未发现区别
        ////Debug.Log(handleMatrix);
        handleMatrix.SetRow(0, Vector4.Scale(handleMatrix.GetRow(0), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(1, Vector4.Scale(handleMatrix.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(2, new Vector4(0f, 0f, 1f, damager.transform.position.z));
        ////Debug.Log(handleMatrix);
        using (new Handles.DrawingScope(handleMatrix))
        {
            s_BoxBoundsHandle.center = damager.offset;
            s_BoxBoundsHandle.size = damager.size;

            s_BoxBoundsHandle.SetColor(s_EnabledColor);
            EditorGUI.BeginChangeCheck();
            s_BoxBoundsHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(damager, "Modify Damager");

                damager.size = s_BoxBoundsHandle.size;
                damager.offset = s_BoxBoundsHandle.center;
            }
        }
        // 画点
        //Handles.DotHandleCap(0, damager.hitPoint+ new Vector2(10, 0), Quaternion.identity, 2, EventType.DragUpdated);
        //Handles.DotHandleCap(0, damager.hitPoint + new Vector2(0, 5), Quaternion.identity, 2, EventType.DragUpdated);
        //Debug.Log("drow nice");
    }
}
