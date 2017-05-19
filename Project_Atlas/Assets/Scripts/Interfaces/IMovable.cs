using UnityEngine;
using System.Collections;

public interface IMovable
{
    IEnumerator Move(Vector3 position, Vector3 direction);
    IEnumerator Rotate(Quaternion rotation);
}
