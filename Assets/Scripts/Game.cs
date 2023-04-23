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
    [SerializeField]
    private ControlsWidget _controlsWidgetPrefab;

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
    private InputListener _inputListener;

    private LoadingWidget _loadingWidget;
    private DetailWidget _detailWidget;
    private ControlsWidget _controlsWidget;

    private Tower _selectedTower;
    private List<Tower> _towers;

    private async void Awake()
    {
        _loadingWidget = Instantiate(_loadingWidgetPrefab);
        _loadingWidget.Show();

        _detailWidget = Instantiate(_detailWidgetPrefab);
        _detailWidget.Hide();

        _controlsWidget = Instantiate(_controlsWidgetPrefab);
        _controlsWidget.Initialise(TestSelectedTower, ResetTestedTower);
        _controlsWidget.Hide();

        // Get the necessary references
        _camera = FindObjectOfType<WorldCamera>();
        _table = FindObjectOfType<Table>();

        _inputListener = FindObjectOfType<InputListener>();
        _inputListener.Initialise();
        
        _inputListener.SubscribeDragListener(_camera);

        ClickOnBlockHandler clickOnBlockHandler 
            = new ClickOnBlockHandler(_camera.Camera, _detailWidget);
        _inputListener.SubscribeClickListener(clickOnBlockHandler);

        ClickOnTowerHandler clickOnTowerHandler
            = new ClickOnTowerHandler(_camera, SelectTower);
        _inputListener.SubscribeClickListener(clickOnTowerHandler);

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

        // select middle stack from left to right
        int middle = (anchors.Count - 1 ) / 2;
        _camera.LookAtTarget(anchors[middle].transform);

        MasteryToMaterialConverter.AddMaterials(_glassMaterial,
            _woodMaterial,
            _stoneMaterial);

        _towers = new List<Tower>();
        TowerBuilder towerBuilder = new TowerBuilder(_blockPrefab, _towerPrefab);

        for (int i = 0; i < grades.Count; ++i)
        {
            string grade = grades[i];
            Anchor anchor = anchors[i];
            List<BlockModel> models = StackFactory.GetBlocksPerGrade(grade);
            Tower tower = towerBuilder.Build(grade, anchor, models);
            _towers.Add(tower);
        }

        SelectTower(_towers[middle]);

        _loadingWidget.Hide();
        _controlsWidget.Show();
    }

    private void TestSelectedTower()
    {
        _selectedTower.TestTheTower();
    }

    private void ResetTestedTower()
    {
        _selectedTower.ReBuild();
    }

    private void SelectTower(Tower tower)
    {
        _selectedTower = tower;
    }
}
