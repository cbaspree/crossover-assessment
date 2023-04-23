using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputListener : MonoBehaviour
{
    private enum InputPhase
    {
        TouchDown = 1,
        DragStarted,
        Dragging,
        DragEnded,
        ClickUp
    }

    [System.Flags]
    private enum Gesture
    {
        None = (1 << 0),
        Touch = (1 << 1),
        Drag = (1 << 2),
    }

    private const int _mouseLeftButton = 0;
    private const int _mouseRightButton = 1;

    private struct UserInput
    {
        public int Id;
        public Vector3 Position;
        public Vector3 DeltaPosition;
        public InputPhase Phase;
    }

    [SerializeField]
    private EventSystem _eventSystem;

    [Header("Configurations")]

    [SerializeField]
    private float _clickAndTapMaximumTimeMS = 200.0f;
    [SerializeField]
    private float _dragThreshold = 0.9f;

    private float _lastClickTime = 0f;
    private UserInput _activeInput;
    private Gesture _activeGesture;

    private List<IClickHandler> _clickHandlers;
    private List<IDragHandler> _dragHandlers;

    public void Initialise(Camera activeCamera)
    {
        _activeInput = new UserInput();
        _activeGesture = Gesture.None;
        _clickHandlers = new List<IClickHandler>();
        _dragHandlers = new List<IDragHandler>();
    }

    public void SubscribeClickListener(IClickHandler handler)
    {
        _clickHandlers.Add(handler);
    }

    public void SubscribeDragListener(IDragHandler handler)
    {
        _dragHandlers.Add(handler);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(_mouseLeftButton))
        {
            if (_eventSystem.IsPointerOverGameObject())
            {
                return;
            }

            _activeInput.Id = _mouseLeftButton;
            _activeInput.Position = Input.mousePosition;
            _activeInput.Phase = InputPhase.ClickUp;

            if (Time.time - _lastClickTime <= _clickAndTapMaximumTimeMS * 0.001f)
            {
                NotifyClickHandlers();
            }
        }
        else if (Input.GetMouseButtonUp(_mouseRightButton))
        {
            if (_eventSystem.IsPointerOverGameObject())
            {
                return;
            }

            _activeInput.Id = _mouseRightButton;
            _activeInput.Position = Input.mousePosition;
            _activeInput.Phase = InputPhase.ClickUp;

            if (Time.time - _lastClickTime <= _clickAndTapMaximumTimeMS * 0.001f)
            {
                NotifyClickHandlers();
            }
        }
        else if (Input.GetMouseButtonDown(_mouseLeftButton) || Input.GetMouseButtonDown(_mouseRightButton))
        {
            if (_eventSystem.IsPointerOverGameObject())
            {
                return;
            }

            _activeInput.Id = Input.GetMouseButton(_mouseLeftButton) ? _mouseLeftButton : _mouseRightButton;
            _activeInput.Position = Input.mousePosition;
            _activeInput.Phase = InputPhase.TouchDown;

            _lastClickTime = Time.time;
        }
        else if (Input.GetMouseButton(_mouseLeftButton) || Input.GetMouseButton(_mouseRightButton))
        {
            if (_eventSystem.IsPointerOverGameObject())
            {
                return;
            }

            if (_activeInput.Id == _mouseLeftButton || _activeInput.Id == _mouseRightButton)
            {
                _activeInput.DeltaPosition = Input.mousePosition - _activeInput.Position;
                _activeInput.Position = Input.mousePosition;

                switch (_activeInput.Phase)
                {
                    case InputPhase.TouchDown:
                        if (_activeInput.DeltaPosition.sqrMagnitude > _dragThreshold * _dragThreshold)
                        {
                            _activeInput.Phase = InputPhase.DragStarted;
                            NotifyDragHandlers();
                        }
                        break;
                    case InputPhase.DragStarted:
                        _activeInput.Phase = InputPhase.Dragging;
                        NotifyDragHandlers();
                        break;
                    case InputPhase.Dragging:
                        NotifyDragHandlers();
                        break;
                    default:
                        break;
                }
            }
        }
        else if (Input.GetMouseButtonUp(_mouseLeftButton) || Input.GetMouseButtonUp(_mouseRightButton))
        {
            if (_eventSystem.IsPointerOverGameObject())
            {
                return;
            }

            if ((_activeInput.Id == _mouseLeftButton || _activeInput.Id == _mouseRightButton)
                && _activeInput.Phase == InputPhase.Dragging)
            {
                _activeInput.Phase = InputPhase.DragEnded;
                _activeInput.DeltaPosition = Input.mousePosition - _activeInput.Position;
                _activeInput.Position = Input.mousePosition;
                NotifyDragHandlers();
            }
        }
    }

    private void NotifyClickHandlers()
    {
        for (int i = 0; i < _clickHandlers.Count; ++i)
        {
            _clickHandlers[i].Click(_activeInput.Position, _activeInput.Id);
        }
    }

    private void NotifyDragHandlers()
    {
        for (int i = 0; i < _dragHandlers.Count; ++i)
        {
            _dragHandlers[i].Dragging(_activeInput.Position, _activeInput.DeltaPosition, _activeInput.Id);
        }
    }
}
