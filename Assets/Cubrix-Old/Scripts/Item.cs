using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NEw Item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int value;
    public int stack;
    public bool stackable;
    public string description;
    public Sprite icon;
    public Activity activity;
}

public static class ScriptableObjectExtension
{
    public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
    {
        if (scriptableObject == null)
        {
            Debug.Log($"ScriptableObject was null. Returning default {typeof(T)} object.");
            return (T)ScriptableObject.CreateInstance(typeof(T));
        }

        T instance = UnityEngine.Object.Instantiate(scriptableObject);
        instance.name = scriptableObject.name;
        return (T)instance;
    }
}
