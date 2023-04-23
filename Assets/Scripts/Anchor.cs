using UnityEngine;

public class Anchor : MonoBehaviour
{
    private const int _blockWidth = 1;
    private const float _gapBetweenBlocks = 0.1f;

    private Vector3 _center;
    private Vector3 _left;
    private Vector3 _right;
    private Vector3 _front;
    private Vector3 _back;

    private Vector3[] _widthwisePositions;
    private Vector3[] _lengthwisePositions;

    public Vector3[] WidthwisePositions { get => _widthwisePositions; }
    public Vector3[] LengthwisePositions { get => _lengthwisePositions; }
    public Vector3 Center { get => _center; }

    public void Initialise()
    {
        _center = transform.position;
        _left = new Vector3(_center.x - (_blockWidth + _gapBetweenBlocks), _center.y, _center.z);
        _right = new Vector3(_center.x + (_blockWidth + _gapBetweenBlocks), _center.y, _center.z);
        
        _widthwisePositions = new Vector3[] { _left, _center, _right };

        _front = new Vector3(_center.x, _center.y, _center.z + (_blockWidth + _gapBetweenBlocks));
        _back = new Vector3(_center.x, _center.y, _center.z - (_blockWidth + _gapBetweenBlocks));

        _lengthwisePositions = new Vector3[] { _front, _center, _back };
    }
}
