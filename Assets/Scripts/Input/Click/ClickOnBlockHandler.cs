using UnityEngine;

public class ClickOnBlockHandler : IClickHandler
{
    private const string _clickableLayerName = "Clickable";
    private const int _mouseRightButton = 1;

    private Camera _camera;
    private DetailWidget _widget;
    private Block _selectedBlock;

    public ClickOnBlockHandler(Camera camera, DetailWidget widget)
    {
        _camera = camera;
        _widget = widget;
    }

    public void Click(Vector2 mousePosition, int mouseButton)
    {
        if (mouseButton == _mouseRightButton)
        {
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            int layer = 1 << LayerMask.NameToLayer(_clickableLayerName);
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                DeselectBlock();
                return;
            }

            IClickable clickable = hit.collider.gameObject.GetComponentInParent<IClickable>();

            if (clickable == null)
            {
                Debug.Log("Raycast hit a gameobject without the clickable component");
                DeselectBlock();
                return;
            }

            clickable.Clicked();

            Block block = clickable as Block;
            if (block == null)
            {
                Debug.Log("Raycast hit a gameobject that is not a block");
                DeselectBlock();
                return;
            }

            if (block != _selectedBlock)
            {
                _selectedBlock?.Highlight(false);
                _selectedBlock = block;
            }
            
            _widget.Populate("Details",
                _selectedBlock.Model.Grade,
                _selectedBlock.Model.Domain,
                _selectedBlock.Model.Cluster,
                _selectedBlock.Model.Standardid,
                _selectedBlock.Model.StandardDescription);
            
            _widget.Show();
        }
        else
        {
            DeselectBlock();
        }
    }

    private void DeselectBlock()
    {
        _selectedBlock?.Highlight(false);
        _widget.Hide();
    }
}
