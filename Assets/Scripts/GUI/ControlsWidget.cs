using UnityEngine;
using UnityEngine.UI;

public class ControlsWidget : MonoBehaviour, IWidget
{
    [SerializeField]
    private Transform _canvas;
    [SerializeField]
    private Button _testStackButton;
    [SerializeField]
    private Button _resetStackButton;

    private System.Action _testStackButtonClickedEventHandler;
    private System.Action _restStackButtonClickedEventHandler;

    public void Initialise(System.Action testStackButtonClickedEventHandler,
        System.Action restStackButtonClickedEventHandler)
    {
        _testStackButtonClickedEventHandler = testStackButtonClickedEventHandler;
        _restStackButtonClickedEventHandler = restStackButtonClickedEventHandler;
    }

    public void Show()
    {
        _canvas.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _canvas.gameObject.SetActive(false);
    }

    private void Awake()
    {
        _testStackButton.onClick.AddListener(TestStackButtonClicked);
        _resetStackButton.onClick.AddListener(ResetStackButtonClicked);
    }

    private void TestStackButtonClicked()
    {
        _testStackButtonClickedEventHandler?.Invoke();
    }

    private void ResetStackButtonClicked()
    {
        _restStackButtonClickedEventHandler?.Invoke();
    }
}
