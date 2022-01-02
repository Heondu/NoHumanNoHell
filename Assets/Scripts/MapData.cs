using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    [SerializeField] private Vector2 position;
    [SerializeField] private Vector2 size;

    public Vector2 GetPosition()
    {
        return position;
    }

    public Vector2 GetSize()
    {
        return size;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Vector2 center = (position + size) / 2;
        Gizmos.DrawWireCube(center, size);
    }
}
