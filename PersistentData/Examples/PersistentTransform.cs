using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Persistent Debug Cube", menuName = "Persistent/Debug Cube")]
public class PersistentTransform : Persistent.Data<PersistentTransform.TransformStruct>
{
    public struct TransformStruct
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    public bool Save(Transform transform)
    {
        data.position = transform.localPosition;
        data.rotation = transform.localRotation;
        data.scale = transform.localScale;
        return true;
    }

    public bool Load(Transform transform)
    {
        transform.localPosition = data.position;
        transform.localRotation = data.rotation;
        transform.localScale = data.scale;
        return true;
    }
}
