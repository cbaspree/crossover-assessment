using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private string _url = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

    [Header("GUI")]

    [SerializeField]
    private LoadingWidget _loadingWidgetPrefab;
    [SerializeField]
    private DetailWidget _detailWidgetPrefab;

    [Header("Materials")]

    [SerializeField]
    private Material _glassMaterial;
    [SerializeField]
    private Material _woodMaterial;
    [SerializeField]
    private Material _stoneMaterial;

    [Header("World")]
        
    [SerializeField]
    private Block _blockPrefab;
    [SerializeField]
    private Tower _towerPrefab;

    private WorldCamera _camera;
    private Table _table;
    private InputListener _inputCollector;

    private LoadingWidget _loadingWidget;
    private DetailWidget _detailWidget;

    private async void Awake()
    {
        _loadingWidget = Instantiate(_loadingWidgetPrefab);
        _loadingWidget.Show();

        _detailWidget = Instantiate(_detailWidgetPrefab);
        _detailWidget.Hide();

        // Get the necessary references
        _camera = FindObjectOfType<WorldCamera>();
        _table = FindObjectOfType<Table>();

        _inputCollector = FindAnyObjectByType<InputListener>();
        _inputCollector.Initialise(_camera.Camera);
        
        _inputCollector.SubscribeDragListener(_camera);

        ClickOnBlockHandler clickOnBlockHandler 
            = new ClickOnBlockHandler(_camera.Camera, _detailWidget);
        _inputCollector.SubscribeClickListener(clickOnBlockHandler);

        ClickOnTowerHandler clickOnTowerHandler
            = new ClickOnTowerHandler(_camera);
        _inputCollector.SubscribeClickListener(clickOnTowerHandler);

        RequestResult result = await HttpRequester.Get(_url);

        // create the blocks with all the information
        string content = result.Content;
        BlockModel[] blockModels = BlockFactory.Create(content);

        // create the stacks per grade
        StackFactory.CategorizeBlocksPerGrade(blockModels);
        List<string> grades = StackFactory.Grades;

        // adapt table to hold all the stacks
        _table.Initialise(grades.Count);
        List<Anchor> anchors = _table.Anchors;

        int middle = (anchors.Count - 1 ) / 2;
        _camera.LookAtTarget(anchors[middle].transform);

        MasteryToMaterialConverter.AddMaterials(_glassMaterial,
            _woodMaterial,
            _stoneMaterial);

        TowerBuilder towerBuilder = new TowerBuilder(_blockPrefab, _towerPrefab);
        
        for (int i = 0; i < grades.Count; ++i)
        {
            string grade = grades[i];
            Anchor anchor = anchors[i];
            List<BlockModel> models = StackFactory.GetBlocksPerGrade(grade);
            Tower tower = towerBuilder.Build(grade, anchor, models);
        }

        _loadingWidget.Hide();
    }
}
