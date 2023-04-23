using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder
{
    private const int _blocksPerFloor = 3;
    private const float _blockHeight = 0.5f;
    
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
        tower.Initialise(grade, anchor);

        int blockCount = 0;
        int floor = 0;
        for (int i = 0; i < models.Count; i += 3)
        {
            Block block = Object.Instantiate(_blockPrefab, tower.transform, false);
            BlockModel model = models[i];
            block.Initialise(tower, 
                model, 
                MasteryToMaterialConverter.Convert(model.Mastery));

            if (floor % 2 == 0)
            {
                block.transform.position = new Vector3(
                anchor.WidthwisePositions[blockCount].x,
                anchor.WidthwisePositions[blockCount].y + floor * _blockHeight,
                anchor.WidthwisePositions[blockCount].z);
            }
            else
            {
                block.transform.position = new Vector3(
                anchor.LengthwisePositions[blockCount].x,
                anchor.LengthwisePositions[blockCount].y + floor * _blockHeight,
                anchor.LengthwisePositions[blockCount].z);

                block.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }
            

            ++blockCount;
            if (blockCount == _blocksPerFloor)
            {
                ++floor;
                blockCount = 0;
            }
        }

        return null;
    }
}
