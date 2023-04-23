using TMPro;
using UnityEngine;

public class LoadingWidget : MonoBehaviour, IWidget
{
    [SerializeField]
    private Transform _canvas;
    [SerializeField]
    private TextMeshProUGUI _text;
    
    public void Show()
    {
        _canvas.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _canvas.gameObject.SetActive(false);
    }
}
