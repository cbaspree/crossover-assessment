using TMPro;
using UnityEngine;

public class DetailWidget : MonoBehaviour, IWidget
{
    [SerializeField]
    private Transform _canvas;
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _gradeLevel;
    [SerializeField]
    private TextMeshProUGUI _domain;
    [SerializeField]
    private TextMeshProUGUI _cluster;
    [SerializeField]
    private TextMeshProUGUI _standardId;
    [SerializeField]
    private TextMeshProUGUI _standardDescription;

    public void Show()
    {
        _canvas.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _canvas.gameObject.SetActive(false);
    }

    public void Populate(string title,
        string gradeLevel,
        string domain,
        string cluster,
        string standardId,
        string standardDescription)
    {
        _title.text = title;
        _gradeLevel.text = $"{gradeLevel}: {domain}";
        _cluster.text = cluster;
        _standardId.text = $"{standardId}: {standardDescription}";
    }
}
