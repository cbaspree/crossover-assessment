using UnityEngine;

public interface IDragHandler
{
    void Dragging(Vector2 currentPosition, Vector2 deltaPosition, int mouseButton);
}
