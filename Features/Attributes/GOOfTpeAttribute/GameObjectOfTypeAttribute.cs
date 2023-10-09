using System;
using UnityEngine;

namespace GOOfTpeAttribute
{
    public class GameObjectOfTypeAttribute : PropertyAttribute
    {
        public Type Type { get; }
        public bool AllowSceneObject { get; }

        public GameObjectOfTypeAttribute(Type requiredType, bool allowSceneObject = true)
        {
            Type = requiredType;
            AllowSceneObject = allowSceneObject;
        }
    } 
}
