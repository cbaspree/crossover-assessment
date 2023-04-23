using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder
{
    private Block _blockPrefab;
    private Tower _towerPrefab;

    public TowerBuilder(Block blockPrefab, Tower towerPrefab)
    {
        _blockPrefab = blockPrefab;
        _towerPrefab = towerPrefab;
    }

    public Tower Build(string grade, Anchor anchor, List<BlockModel> models)
    {
        Tower tower = Object.Instantiate(_towerPrefab);
        tower.Initialise(grade, anchor, _blockPrefab, models);
        tower.CreateBlocks();

        return tower;
    }
}
