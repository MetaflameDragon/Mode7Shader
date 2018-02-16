using UnityEngine;
using UnityEditor;
using Colour = UnityEngine.Color;

[CustomEditor(typeof(Mode7Controller))]
public class Mode7ControllerEditor : Editor
{
	private bool isAnimating = false;
	private Mode7Config startConfig;
	private Mode7Config targetConfig;

	private Colour col1 = new Colour(0.5f, 0.5f, 0.5f);
	private Colour col2 = new Colour(0.7f, 0.7f, 0.7f);
	private Colour colSelected = new Colour(0.3f, 0.3f, 0.3f);

	private int selectedIndex = 0;

	public override void OnInspectorGUI()
	{
		Mode7Controller controller = (Mode7Controller)target;

		// Use this if the material/shader doesn't get updated after clicking a transition button
		if (GUILayout.Button("Reload Material"))
		{
			controller.ReloadMaterial();
		}

		col1 = EditorGUILayout.ColorField("Odd row colour", col1);
		col2 = EditorGUILayout.ColorField("Even row colour", col2);
		colSelected = EditorGUILayout.ColorField("Selected row colour", colSelected);


		controller.animationCurve = EditorGUILayout.CurveField("Animation Curve", controller.animationCurve, Colour.green, new Rect(0, 0, 1, 1), GUILayout.Height(100));

		controller.animationTime = Mathf.Max(EditorGUILayout.DelayedFloatField("Animation time", controller.animationTime), 0);
		
		float buttonWidth = 25;
		float fieldWidth = 40;
		float configHeight = 10;
		float spacing = 5;

		GUIStyle numberFieldStyle = new GUIStyle(EditorStyles.numberField)
		{
			alignment = TextAnchor.MiddleCenter
		};
		
		GUIStyle richTextButtonStyle = new GUIStyle(GUI.skin.button)
		{
			richText = true
		};

		GUIStyle middleAlignedText = new GUIStyle(EditorStyles.label)
		{
			alignment = TextAnchor.MiddleCenter,
			richText = true
		};

		Rect r = GUILayoutUtility.GetRect(buttonWidth, configHeight + spacing);
		string[] labels = new string[] { "H", "V", "X<size=7>0</size>", "Y<size=7>0</size>", "A", "B", "C", "D" };

		for (int i = 0; i < labels.Length; i++)
		{
			Rect labelRect = new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * i, r.y, fieldWidth, r.height);
			EditorGUI.DrawRect(labelRect, Colour.grey);
			EditorGUI.LabelField(labelRect, labels[i], middleAlignedText);
		}

		if (GUI.Button(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 8, r.y, 30, r.height), "+", richTextButtonStyle))
		{
			controller.configs.Insert(0, new Mode7Config());
			selectedIndex++;
		}

		for (int i = 0; i < controller.configs.Count; i++)
		{
			r = GUILayoutUtility.GetRect(buttonWidth, configHeight + spacing);

			EditorGUI.DrawRect(new Rect(r.x, r.y, r.width, r.height), i == selectedIndex ? colSelected : (((i & 1) > 0) ? col1 : col2));

			string buttonText;

			if (i != selectedIndex)
			{
				buttonText = i.ToString();
			}
			else
			{
				buttonText = "<color=" + (isAnimating ? "red" : "black") + "><b>" + i.ToString() + "</b></color>";
			}

			if (GUI.Button(new Rect(r.x, r.y, buttonWidth, r.height), buttonText, richTextButtonStyle))
			{
				selectedIndex = i;
				//Debug.LogFormat("Animation start at {0}", Time.time);
				isAnimating = true;
				controller.animationStartTime = Time.time;
				startConfig = controller.GetConfig();
				targetConfig = controller.configs[i];
			}

			#region Drawing Mode7Config fields and updating the config
			Mode7Config config = controller.configs[i];
			config.h = EditorGUI.FloatField(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 0, r.y, fieldWidth, r.height), config.h, numberFieldStyle);
			config.v = EditorGUI.FloatField(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 1, r.y, fieldWidth, r.height), config.v, numberFieldStyle);
			config.x0 = EditorGUI.FloatField(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 2, r.y, fieldWidth, r.height), config.x0, numberFieldStyle);
			config.y0 = EditorGUI.FloatField(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 3, r.y, fieldWidth, r.height), config.y0, numberFieldStyle);
			config.a = EditorGUI.FloatField(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 4, r.y, fieldWidth, r.height), config.a, numberFieldStyle);
			config.b = EditorGUI.FloatField(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 5, r.y, fieldWidth, r.height), config.b, numberFieldStyle);
			config.c = EditorGUI.FloatField(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 6, r.y, fieldWidth, r.height), config.c, numberFieldStyle);
			config.d = EditorGUI.FloatField(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 7, r.y, fieldWidth, r.height), config.d, numberFieldStyle);
			if (config != controller.configs[i])
			{
				//Debug.Log("Value updated");
				controller.configs[i] = config;
				if (i == selectedIndex)
				{
					UpdateConfig(controller);
				}
			}
			#endregion

			if (GUI.Button(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 8, r.y, 30, r.height), "+", richTextButtonStyle))
			{
				controller.configs.Insert(i + 1, new Mode7Config());
				if (selectedIndex > i)
				{
					selectedIndex++;
				}
			}

			if (GUI.Button(new Rect(buttonWidth + 20 + spacing + (spacing + fieldWidth) * 8 + 30 + spacing, r.y, 30, r.height), "-", richTextButtonStyle))
			{
				controller.configs.RemoveAt(i);
				if (selectedIndex == i)
				{
					selectedIndex = Mathf.Max(selectedIndex - 1, 0);
				}
				UpdateConfig(controller);
			}
		}

		if (isAnimating)
		{
			if (controller.animationStartTime + controller.animationTime < Time.time)
			{
				//Debug.Log("Animation end");
				isAnimating = false;
			}
			else
			{
				Mode7Config config = controller.InterpolateFromTo(startConfig, targetConfig, (Time.time - controller.animationStartTime) / controller.animationTime);
				controller.SetConfig(config);
			}
			Repaint();
		}
	}

	public void UpdateConfig(Mode7Controller controller)
	{
		if (selectedIndex >= 0 && selectedIndex < controller.configs.Count - 1)
		{
			controller.SetConfig(controller.configs[selectedIndex]);
		}
	}
}