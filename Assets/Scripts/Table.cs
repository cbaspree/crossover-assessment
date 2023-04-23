using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private const float _tableWidth = 5.0f;

    [SerializeField]
    private Transform _plane;
    [SerializeField]
    private Transform _anchorsRoot;
    [SerializeField]
    private Anchor _anchorPrefab;

    private List<Anchor> _anchors;

    public List<Anchor> Anchors { get => _anchors; }

    public void Initialise(int towerCount)
    {
        Scale(towerCount);
        PositionAnchorRoot(towerCount);
        GenerateAnchors(towerCount);
    }

    private void Awake()
    {
        _anchors = new List<Anchor>();
    }

    private void Scale(int factor)
    {
        Vector3 localScale = _plane.localScale;
        _plane.localScale = new Vector3(localScale.x * factor,
            localScale.y,
            localScale.z);
    }

    private void PositionAnchorRoot(int factor)
    {
        float tableHalfWidth = _tableWidth / 2.0f;

        _anchorsRoot.localPosition = new Vector3(
                _anchorsRoot.localPosition.x - (tableHalfWidth * (factor - 1)),
                _anchorsRoot.localPosition.y,
                _anchorsRoot.localPosition.z);
    }

    private void GenerateAnchors(int factor)
    {
        for (int i = 0; i < factor; ++i)
        {
            Anchor anchor = Instantiate(_anchorPrefab, _anchorsRoot, false);
            Vector3 localPosition = anchor.transform.localPosition;

            float anchorWidth = anchor.transform.localScale.x;
            int blockNumberBetweenAnchors = 5;

            anchor.transform.localPosition = new Vector3(anchorWidth * blockNumberBetweenAnchors * i,
                localPosition.y,
                localPosition.z);

            anchor.Initialise();

            _anchors.Add(anchor);
        }
    }
}
