using System.Reflection;
using UnityEngine;

public class ClickOnTowerHandler : IClickHandler
{
    private const string _clickableLayerName = "Clickable";
    private const int _mouseLeftButton = 0;

    private WorldCamera _worldCamera;
    private Tower _selectedTower;
    System.Action<Tower> _towerClickedCallback;

    public ClickOnTowerHandler(WorldCamera worldCamera, System.Action<Tower> towerClickedCallback)
    {
        _worldCamera = worldCamera;
        _towerClickedCallback = towerClickedCallback;
    }

    public void Click(Vector2 mousePosition, int mouseButton)
    {
        if (mouseButton == _mouseLeftButton)
        {
            Ray ray = _worldCamera.Camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            int layer = 1 << LayerMask.NameToLayer(_clickableLayerName);
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                return;
            }

            IClickable clickable = hit.collider.gameObject.GetComponentInParent<IClickable>();

            if (clickable == null)
            {
                Debug.Log("Raycast hit a gameobject without the clickable component");
                return;
            }

            Block block = clickable as Block;
            if (block == null)
            {
                Debug.Log("Raycast hit a gameobject that is not a block");
                return;
            }

            _selectedTower = block.Parent;
            _worldCamera.ResetCameraView();
            _worldCamera.LookAtTarget(_selectedTower.transform);

            _towerClickedCallback?.Invoke(_selectedTower);
        }
    }
}
