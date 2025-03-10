using BattleGameTester.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleGameTester.Core
{
    public enum EPlayer
    {
        P1, P2
    }
    public enum GridState
    {
        CellPick,
        TargetPick,
        MovePick,
        Locked
    }
    public class CellInfo
    {
        public EPlayer player { get; set; }
        public bool isEmpty { get; set; }
        public CellInfo(EPlayer player, bool isEmpty)
        {
            this.player = player;
            this.isEmpty = isEmpty;
        }
    }
    public class GridSystem : MonoBehaviour, IGridSystem
    {
        public event Action<GridState> GridStateChanged;
        public GridState State
        {
            get => _state;
            set
            {
                _state = value;
                _inputCell = activeCell;
                GridStateChanged?.Invoke(value);
            }
        }
        private GridState _state;
        private static List<GridState> PickStates = new List<GridState> { GridState.CellPick, GridState.TargetPick, GridState.MovePick };

        //Actions to CellIndicator
        private event Action<Vector3> CursorMovedToNewCell;
        private event Action CursorEnterUnavailableCell;
        private event Action CursorEnterAvailableCell;
        private event Action CursorEnterGrid;
        private event Action CursorExitGrid;
        private event Action CellOpened;

        public event Action<Vector2Int> CursorEnterNotEmptyCell;
        public event Action CursorEnterEmptyCell;

        //Actions to CellMenu
        public event Action<Vector3> EmptyCellOpened;
        public event Action<Vector3> SquadCellOpened;
        public event Action CellClosed;
        public event Action<Vector2Int> CellPicked;

        public EPlayer ActivePlayer { get => _activePlayer; set { _activePlayer = value; ChangeGridColors(); } }
        private EPlayer _activePlayer;
        public Vector2Int activeCell { get; private set; }
        private Vector2Int _inputCell;

        public Camera ActiveCamera
        {
            get => _activeCamera;
            set => _activeCamera = value;
        }
        private Camera _activeCamera;
        private Vector3 _lastPosition;

        [SerializeField]
        private Grid grid;

        private Dictionary<Vector2Int, CellInfo> GridCells;

        [SerializeField]
        private LayerMask placementLayermask;
        //VisualEffects
        [SerializeField]
        private GameObject cellIndicatorPrefab;
        [SerializeField]
        private Material GridLMat;
        [SerializeField]
        private Material GridRMat;
        [SerializeField]
        private Color Available;
        [SerializeField]
        private Color Blocked;

        private IResourceManager resourceManager;
        private IAudioManager audioManager;
        private IUserInput userInput;
        private IGameHUD gameHUD;

        private ICellIndicator cellIndicator;

        public Vector3Int lastDetectedCell;

        private void Awake()
        {
            resourceManager = CompositionRoot.GetResourceManager();
            audioManager = CompositionRoot.GetAudioManager();
            userInput = CompositionRoot.GetUserInput();
            gameHUD = CompositionRoot.GetGameHUD();

            var cameraManager = CompositionRoot.GetCameraManager();
            ActiveCamera = cameraManager.CameraComponent;
            cameraManager.CameraChanged += OnCameraChanged;

            if (cellIndicatorPrefab == null) throw new Exception("No CellIndicatorPrefab in Grid System.");
            cellIndicator = resourceManager.CreatePrefabInstance<ICellIndicator>(cellIndicatorPrefab, this.transform);
        }
        private void Start()
        {
            ActivePlayer = EPlayer.P1;
            State = GridState.CellPick;
            BaseView.Showed += OnView_Showed;
            BaseView.Hided += OnView_Hided;

            GridCells = CreateCells();

            //GridSystem -> CellIndicator
            CursorMovedToNewCell += cellIndicator.MoveTo;
            CursorEnterUnavailableCell += cellIndicator.MakeUnavailable;
            CursorEnterAvailableCell += cellIndicator.MakeAvailable;
            CursorEnterGrid += cellIndicator.Show;
            CursorExitGrid += cellIndicator.Hide;
            CellOpened += cellIndicator.OnCellOpened;
            CellClosed += cellIndicator.OnCellClosed;

            //GridSystem -> GridSystem
            CursorEnterGrid += OnCursorEnterGrid; //!Don't add EXIT Action
            GridStateChanged += OnGridStateChanged;
        }



        private void Update()
        {
            if (PickStates.Contains(State))
            {
                MoveCellIndicator();
            }
        }
        public void CloseCell()
        {
            userInput.Escaped -= CloseCell;
            if (State == GridState.Locked) { audioManager.PlayEffect(EAudio.Click_Close); }
            Debug.Log("CloseCell_State = " + State);
            CellClosed?.Invoke();
            var mousePos = _activeCamera.WorldToScreenPoint(grid.CellToWorld(lastDetectedCell) + new Vector3(0.5f, 0.5f));
            Mouse.current.WarpCursorPosition(new Vector2(mousePos.x, mousePos.y));
            State = GridState.CellPick;
            userInput.Unlock();
            gameHUD.Buttons.Unlock();
        }
        public void SetCellEmptyState(Vector2Int cell, bool isEmpty)
        {
            GridCells[cell].isEmpty = isEmpty;
        }
        public void MakeActiveCellNotEmpty()
        {
            if (GridCells[activeCell].isEmpty != false) SetCellEmptyState(activeCell, false);
        }
        public void MakeActiveCellEmpty()
        {
            if (GridCells[activeCell].isEmpty == false) SetCellEmptyState(activeCell, true);
        }
        public Vector2Int GetActiveCell()
        {
            return activeCell;
        }
        public Vector3 GetCellWorldPosition(Vector2Int cell)
        {
            return grid.CellToWorld(new Vector3Int(cell.x, 0, cell.y)) + new Vector3(0.5f, 0.02f, 0.5f);
        }
        public Vector3 GetActiveCellWorldPosition()
        {
            return GetCellWorldPosition(activeCell);
        }


        #region GridOperations
        private void OpenCell()
        {
            if (State == GridState.Locked) return;
            userInput.Lock();
            gameHUD.Buttons.Lock();
            var pickedCell = new Vector2Int(lastDetectedCell.x, lastDetectedCell.z);

            switch (State)
            {
                case GridState.CellPick:
                    if (isPlayerCell(pickedCell))
                    {
                        CellOpened?.Invoke();
                        State = GridState.Locked;
                        audioManager.PlayEffect(EAudio.Click_Open);
                        activeCell = pickedCell;
                        if (isCellEmpty(pickedCell)) EmptyCellOpened?.Invoke(grid.CellToWorld(lastDetectedCell));
                        else SquadCellOpened?.Invoke(grid.CellToWorld(lastDetectedCell));
                    }
                    else
                    {
                        audioManager.PlayEffect(EAudio.Click_Error);
                        State = GridState.CellPick;
                        CursorEnterUnavailableCell?.Invoke();
                    }
                    break;

                case GridState.MovePick:
                    if (isPlayerCell(pickedCell) && isCellEmpty(pickedCell))
                    {
                        CellPicked?.Invoke(pickedCell);
                        GridCells[_inputCell].isEmpty = true;
                        GridCells[pickedCell].isEmpty = false;
                        audioManager.PlayEffect(EAudio.Squad_Moved);
                        CellClosed?.Invoke();
                        State = GridState.CellPick;
                    }
                    else audioManager.PlayEffect(EAudio.Click_Error);
                    break;
                case GridState.TargetPick:
                    if (!isPlayerCell(pickedCell) && !isCellEmpty(pickedCell))
                    {
                        CellPicked?.Invoke(pickedCell);
                        CellClosed?.Invoke();
                    }
                    else audioManager.PlayEffect(EAudio.Click_Error);
                    break;

                default:
                    break;
            }
            userInput.Escaped += CloseCell;
        }

        //Кол-во ячеек на поле (должно быть одинаковым с двух сторон)
        private Dictionary<Vector2Int, CellInfo> CreateCells()
        {
            var cells = new Dictionary<Vector2Int, CellInfo>();
            var grids = grid.GetComponentsInChildren<Transform>()[1..3];
            int xSize = 0;
            int zSize = Mathf.FloorToInt(grids[0].localScale.z * 10f);
            foreach (var grid in grids)
            {
                xSize += Mathf.FloorToInt(grid.localScale.x * 10f);
                //Debug.Log($"grid.localScale.x = {grid.localScale.x} | grid.localScale.z = {grid.localScale.z}");
            }

            var tempX = -Mathf.FloorToInt(xSize / 2);
            var tempZ = -Mathf.FloorToInt(zSize / 2);

            for (int i = tempX; i < Mathf.Abs(tempX); i++)
            {
                for (int j = tempZ; j < Mathf.Abs(tempZ); j++)
                {
                    if (i < 0)
                    {
                        cells.Add(new Vector2Int(i, j), new CellInfo(EPlayer.P1, true));
                    }
                    else
                    {
                        cells.Add(new Vector2Int(i, j), new CellInfo(EPlayer.P2, true));
                    }
                }
            }
            if (cells.Count > 0)
            {
                Debug.Log($"GridCells Amount: {cells.Count}");
                return cells;
            }
            else { throw new Exception("Dictionary Cells has no values!"); }
        }
        private void ChangeGridColors()
        {
            switch (State, ActivePlayer)
            {
                case (GridState.CellPick, EPlayer.P1) or (GridState.TargetPick, EPlayer.P2):
                    GridLMat.color = Available;
                    GridRMat.color = Blocked;
                    break;
                case (GridState.CellPick, EPlayer.P2) or (GridState.TargetPick, EPlayer.P1):
                    GridLMat.color = Blocked;
                    GridRMat.color = Available;
                    break;
                case (GridState.MovePick, EPlayer.P1) or (GridState.MovePick, EPlayer.P2):
                    break;
                case (GridState.Locked, EPlayer.P1) or (GridState.Locked, EPlayer.P2):
                    break;
                default:
                    Debug.Log("Unknown ChangeGridColors() state");
                    break;
            }

        }
        #endregion
        private void OnCameraChanged(Camera cam)
        {
            ActiveCamera = cam;
        }
        private void OnGridStateChanged(GridState state)
        {
            switch (state)
            {
                case GridState.CellPick:
                    cellIndicator.State = CellIndicatorState.Frame;
                    userInput.Unlock();
                    gameHUD.Buttons.Unlock();
                    break;
                case GridState.MovePick:
                    cellIndicator.State = CellIndicatorState.Move;
                    userInput.Lock();
                    gameHUD.Buttons.Lock();
                    break;
                case GridState.TargetPick:
                    cellIndicator.State = CellIndicatorState.Target;
                    userInput.Lock();
                    gameHUD.Buttons.Lock();
                    break;
                case GridState.Locked:
                    userInput.Lock();
                    gameHUD.Buttons.Lock();
                    break;

            }
            ChangeGridColors();
            CellPicked = null;
        }
        private void OnCursorEnterGrid()
        {
            CursorEnterGrid -= OnCursorEnterGrid;
            CursorExitGrid += OnCursorExitGrid;
            //Debug.Log("Mouse Entered Grid");
            userInput.Clicked += OpenCell;
        }
        private void OnCursorExitGrid()
        {
            CursorExitGrid -= OnCursorExitGrid;
            CursorEnterGrid += OnCursorEnterGrid;
            //Debug.Log("Mouse Exit Grid");
            userInput.Clicked -= OpenCell;
        }

        #region CellIndicator & Mouse
        private void MoveCellIndicator()
        {
            Vector3 mousePosition = GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            if (lastDetectedCell != gridPosition)
            {
                lastDetectedCell = gridPosition;
                //Debug.Log($"GP: {gridPosition} || MP: {mousePosition}");

                CursorMovedToNewCell?.Invoke(grid.CellToWorld(gridPosition));

                var cell = new Vector2Int(lastDetectedCell.x, lastDetectedCell.z);
                switch (State)
                {
                    case GridState.CellPick:
                        if (isPlayerCell(cell)) CursorEnterAvailableCell?.Invoke();
                        else CursorEnterUnavailableCell?.Invoke();
                        if (!isCellEmpty(cell)) CursorEnterNotEmptyCell?.Invoke(cell);
                        else CursorEnterEmptyCell?.Invoke();
                        break;
                    case GridState.MovePick:
                        if (isPlayerCell(cell) && isCellEmpty(cell)) { CursorEnterAvailableCell?.Invoke(); }
                        else CursorEnterUnavailableCell?.Invoke();
                        break;
                    case GridState.TargetPick:
                        if (!isPlayerCell(cell) && !isCellEmpty(cell)) { CursorEnterAvailableCell?.Invoke(); CursorEnterNotEmptyCell?.Invoke(cell); }
                        else CursorEnterUnavailableCell?.Invoke();
                        if (isCellEmpty(cell)) CursorEnterEmptyCell?.Invoke();
                        break;
                    default:
                        break;
                }
                
            }
        }
        private Vector3 GetSelectedMapPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = _activeCamera.nearClipPlane;
            Ray ray = _activeCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, placementLayermask))
            {
                _lastPosition = hit.point;
                CursorEnterGrid?.Invoke();
            }
            else
            {
                CursorExitGrid?.Invoke();
            }
            return _lastPosition;
        }
        #endregion

        private bool isPlayerCell(Vector2Int cell)
        {
            var cellInfo = GridCells[cell];
            if (cellInfo.player == ActivePlayer) { return true; }
            else { return false; }
        }
        private bool isCellEmpty(Vector2Int cell)
        {
            var cellInfo = GridCells[cell];
            if (cellInfo.isEmpty) return true;
            else return false;
        }


        private void OnView_Showed()
        {
            State = GridState.Locked;
            Debug.Log("> SHOW");
        }
        private void OnView_Hided()
        {
            State = GridState.CellPick;
            Debug.Log("< HIDE");
        }
    }
}

