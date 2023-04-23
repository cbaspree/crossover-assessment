using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _label;

    private string _name;

    public void Initialise(string name, Anchor anchor)
    {
        transform.position = new Vector3(
               anchor.Center.x,
               anchor.Center.y,
               anchor.Center.z);

        _name = name;
        _label.text = _name;
    }
}
