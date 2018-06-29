using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {

    private static GameObject canvas;
    private static FloatingText textPrefab;
	
	public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        textPrefab = Resources.Load<FloatingText>("Prefabs/UI/FloatingTextBox");
	}

    public static void CreatePopupText(string text, Transform sourcePosition)
    {
        FloatingText instance = Instantiate(textPrefab);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(sourcePosition.position);
       
        instance.SetText(text);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
    }
	
	
}
