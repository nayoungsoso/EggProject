#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System; //This allows the IComparable Interface

[CustomEditor(typeof(OrderSprites))]
public class OrderSpritesEditor : Editor {

	public class cList_Z_Position : IComparable<cList_Z_Position>
	{
		public GameObject obj;
		public int posZ;

		public cList_Z_Position (GameObject newObj ,int newposZ)
		{
			obj = newObj;
			posZ = newposZ;
		}

		//This method is required by the IComparable
		//interface. 
		public int CompareTo(cList_Z_Position other)
		{
			if(other == null)
			{
				return 1;
			}

			//Return the difference in power.
			return posZ - other.posZ;
		}
	}

	public bool 			SeeInspector = false;						// use to draw default Inspector
	SerializedProperty 		sortingOrderStart;							// Access property from MainMenu.cs script
	SerializedProperty 		b_increase;

	private Texture2D MakeTex(int width, int height, Color col) {		// use to change the GUIStyle
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i) {
			pix[i] = col;
		}
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}

	void OnEnable () {
		// Setup the SerializedProperties.
		sortingOrderStart = serializedObject.FindProperty ("sortingOrderStart");
		b_increase = serializedObject.FindProperty ("b_increase");
	}

	public override void OnInspectorGUI()
	{
		SeeInspector = EditorGUILayout.Foldout(SeeInspector,"Inspector");

		if(SeeInspector)							// If true Default Inspector is drawn on screen
			DrawDefaultInspector();

		OrderSprites myScript = (OrderSprites)target; 

		serializedObject.Update ();
		GUIStyle style = new GUIStyle(GUI.skin.box);

		GUILayout.Label("");

		style.normal.background = MakeTex(2, 2, new Color(1,.2f,0,.3f));

		List<cList_Z_Position>	List_Z_Position = new List<cList_Z_Position>();

		style.normal.background = MakeTex(2, 2, new Color(1,1,0,.5f));
		EditorGUILayout.BeginVertical(style);
		EditorGUILayout.HelpBox("1 - Choose a unique ''Order in Layer'' for each Sprite" +
			"\n (Order is choosed depending Transform Z position)",MessageType.Info);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("First Layer : ", GUILayout.Width(100));
		EditorGUILayout.PropertyField(sortingOrderStart, new GUIContent (""), GUILayout.Width(40));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Reverse Order : ", GUILayout.Width(100));
		EditorGUILayout.PropertyField(b_increase, new GUIContent (""));
		EditorGUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties ();

		if(GUILayout.Button("Order"))
		{

			foreach (Renderer renderer in myScript.gameObject.GetComponentsInChildren<Renderer>()){
				List_Z_Position.Add(new cList_Z_Position(renderer.gameObject,Mathf.RoundToInt(renderer.gameObject.transform.position.z*1000)));
			}

			List_Z_Position.Sort();
			if(b_increase.boolValue)List_Z_Position.Reverse();


			int renderOrderOffset = sortingOrderStart.intValue;
			int cmpt	= 0;
			for(int i = 0;i<List_Z_Position.Count;i++){
				cmpt++;
				SerializedObject serializedObjectRenderer = new UnityEditor.SerializedObject(List_Z_Position[i].obj.GetComponent<Renderer>());
				serializedObjectRenderer.Update ();
				SerializedProperty tmpSer = serializedObjectRenderer.FindProperty("m_SortingOrder");
				tmpSer.intValue = renderOrderOffset;
				serializedObjectRenderer.ApplyModifiedProperties ();
				renderOrderOffset++;
			}

			Debug.Log("Info : Order Done : " + cmpt + " file(s)");
		}
		EditorGUILayout.EndVertical();
	}
}
#endif