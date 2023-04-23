using UnityEngine;

public class Block : MonoBehaviour, IClickable
{
    private const string _emissionColorName = "_EmissionColor";

    [SerializeField]
    private MeshRenderer _renderer;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private Color _highlightColor;
    [SerializeField]
    private Color _normalColor;

    private Tower _parent;
    private BlockModel _model;

    public Tower Parent { get => _parent; }
    public BlockModel Model { get => _model; }

    public void Initialise(Tower parent, BlockModel model, Material material)
    {
        _parent = parent;
        _renderer.material = material;
        _model = model;
    }

    public void Clicked()
    {
        Highlight(true);
    }

    public void Highlight(bool highlight)
    {
        if (highlight)
        {
            _renderer.material.SetColor(_emissionColorName, _highlightColor);
        }
        else
        {
            _renderer.material.SetColor(_emissionColorName, _normalColor);
        }
    }

    public void ApplyPhysics(bool apply)
    {
        _rigidbody.isKinematic = !apply;
    }

    private void Awake()
    {
        ApplyPhysics(false);
    }
}
