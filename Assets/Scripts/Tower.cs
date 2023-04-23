using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private const int _blocksPerFloor = 3;
    private const float _blockHeight = 0.5f;

    [SerializeField]
    private TextMeshPro _label;

    private Block _blockPrefab;
    private List<Block> _blocks;
    private Anchor _anchor;
    private List<BlockModel> _models;

    public void Initialise(string name, Anchor anchor, Block blockPrefab, List<BlockModel> models)
    {
        _blocks = new List<Block>();

        transform.position = new Vector3(
               anchor.Center.x,
               anchor.Center.y,
               anchor.Center.z);

        _label.text = name;
        _anchor = anchor;
        _blockPrefab = blockPrefab;
        _models = models;
    }

    public void CreateBlocks()
    {
        int blockCount = 0;
        int floor = 0;
        for (int i = 0; i < _models.Count; ++i)
        {
            Block block = Instantiate(_blockPrefab, transform, false);
            BlockModel model = _models[i];
            block.Initialise(this,
                model,
                MasteryToMaterialConverter.Convert(model.Mastery));

            if (floor % 2 == 0)
            {
                block.transform.position = new Vector3(
                _anchor.WidthwisePositions[blockCount].x,
                _anchor.WidthwisePositions[blockCount].y + floor * _blockHeight,
                _anchor.WidthwisePositions[blockCount].z);
            }
            else
            {
                block.transform.position = new Vector3(
                _anchor.LengthwisePositions[blockCount].x,
                _anchor.LengthwisePositions[blockCount].y + floor * _blockHeight,
                _anchor.LengthwisePositions[blockCount].z);

                block.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }


            ++blockCount;
            if (blockCount == _blocksPerFloor)
            {
                ++floor;
                blockCount = 0;
            }

            _blocks.Add(block);
        }
    }

    public void TestTheTower()
    {
        for (int i = 0; i < _blocks.Count; ++i)
        {
            Block block = _blocks[i];
            BlockModel model = block.Model;
            if (model.Mastery == 0)
            {
                block.gameObject.SetActive(false);
                continue;
            }

            block.ApplyPhysics(true);
        }
    }

    public void ReBuild()
    {
        DestroyBlocks();
        CreateBlocks();
    }

    private void DestroyBlocks()
    {
        for (int i = _blocks.Count - 1; i >= 0; --i)
        {
            Block block = _blocks[i];
            _blocks.RemoveAt(i);
            Destroy(block.gameObject);
        }
    }
}
