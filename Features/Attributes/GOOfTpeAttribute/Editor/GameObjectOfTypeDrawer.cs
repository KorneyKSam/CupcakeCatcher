using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOOfTpeAttribute
{
	[CustomPropertyDrawer(typeof(GameObjectOfTypeAttribute))]
	public class GameObjectOfTypeDrawer : PropertyDrawer
	{
		private const string ErrorIsNotField = "GameObjectTypeAttribute works only with GameObjects references";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			bool isFieldGameObject = IsFieldGameObject();
			if (!isFieldGameObject)
			{
				DrawError(position, ErrorIsNotField);
				return;
			}

			var gameObjectOfTypeAttribute = attribute as GameObjectOfTypeAttribute;
			var requiredType = gameObjectOfTypeAttribute.Type;

			CheckDragAndDrops(position, requiredType);
			CheckValues(property, requiredType);
			DrawObjectField(property, position, label, gameObjectOfTypeAttribute.AllowSceneObject);
        }

		public bool IsFieldGameObject()
		{
            return fieldInfo.FieldType == typeof(GameObject) || typeof(IEnumerable<GameObject>).IsAssignableFrom(fieldInfo.FieldType);
        }

        private void CheckDragAndDrops(Rect position, Type requiredType)
        {
			if (position.Contains(Event.current.mousePosition))
			{
				var draggerObjectCount = DragAndDrop.objectReferences.Length;
				for (int i = 0; i < draggerObjectCount; i++)
				{
					if (!IsValidObject(DragAndDrop.objectReferences[i], requiredType))
					{
						DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
						break;
					}
				}
			}
        }

		private void CheckValues(SerializedProperty property, Type requiredType)
		{
			if (property.objectReferenceValue != null)
			{
				if (!IsValidObject(property.objectReferenceValue, requiredType))
				{
					property.objectReferenceValue = null;
				}
			}
		}

		private bool IsValidObject(Object @object, Type requiredType)
		{
			bool result = false;

			var go = (@object as GameObject);

			if (go != null)
			{
				result = go.GetComponent(requiredType) != null;
			}

			return result;
		}

		private void DrawObjectField(SerializedProperty property, Rect position, GUIContent label, bool allowSceneObjects)
		{
			property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(GameObject), allowSceneObjects);
		}

		private void DrawError(Rect position, string message)
		{
			EditorGUI.HelpBox(position, message, MessageType.Error);
		}
	} 
}
