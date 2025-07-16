using BadAppleTrickyTowersMod.TetrisPlayer;
using System;
using System.IO;
using UnityEngine;

public class BadAppleAnimator : IInjectable, ITowerInjectable, IGamePlayControllerInjectable, IBrickInjectable, IZoomableCameraInjectable
{
    private Tower _tower;
    private ZoomableCamera _zoomableCamera;

    private Brick _currentBrick;
    private LocalGamePlayController _gamePlayController;

    private readonly string _frameFolderPath;
    private bool[,] _currentFrame;
    private int _currentFrameCount;

    private readonly TetrominoFiller _tetrominoFiller;
    private bool _finishedBrickSpawning;
    private bool _hasFinished;

    private Brick _firstPlacedBrick;

    public BadAppleAnimator()
    {
        _frameFolderPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "BepInEx/plugins/BadAppleMod/frames/");
        _tetrominoFiller = new TetrominoFiller();

        _currentFrameCount = 0;
        _finishedBrickSpawning = false;
        _hasFinished = false;
        _firstPlacedBrick = null;

        LoadNextFrame();
    }

    private void LoadNextFrame()
    {
        _currentFrameCount++;
        _currentFrame = FrameExtractor.LoadFrameFromFolder(_frameFolderPath, _currentFrameCount);
        if (_currentFrame == null)
        {
            _currentFrameCount--;
            _hasFinished = true;
            return;
        }
        _tetrominoFiller.SetFrame(_currentFrame); 

        Debug.Log($"Loading next frame {_currentFrameCount}");
    }

    private void ClearBricks()
    {
        if (_tower == null)
        {
            return;
        }
        foreach (TowerPart tp in _tower.towerParts)
        {
            if (tp is Brick brick)
            {
                brick.Remove(false);
            }
        }
    }

    public void SetBrick(Brick brick)
    {
        _currentBrick = brick;
    }

    public void SetGamePlayController(AbstractGamePlayController gamePlayController)
    {
        if (gamePlayController is LocalGamePlayController)
        {
            _gamePlayController = (LocalGamePlayController) gamePlayController;
        }
        else
        {
            throw new Exception("Not a LocalGamePlay controller.");
        }
    }

    public void SetTower(Tower tower)
    {
        _tower = tower;
    }

    public void UpdateAnimation(float time)
    {
        if (_tower == null)
        {
            return;
        }
        if (_hasFinished)
        {
            ClearBricks();
            return;
        }

        if (_tower.GetLastBrick() == null)
        {
            return;
        }
        else if (_firstPlacedBrick == null)
        {
            _firstPlacedBrick = _tower.GetLastBrick();
            _firstPlacedBrick.gameObject.SetActive(false);
        }

        // Zoom to fit the frame bricks
        if (_zoomableCamera != null)
        {
            _zoomableCamera.SetVerticalBounds(86f,-8f);
        }
    
        // Spawn grid with bricks
        if (!_finishedBrickSpawning)
        {
            FillGrid();
        }
        else
        {
            // Disable bricks for next frame
            _gamePlayController.DisableBrickSpawning();

            // See what placedTetrominos should be visible
            LoadNextFrame();
            _tetrominoFiller.ApplyFrame(_currentFrame);

            foreach (PlacedTetromino placed in _tetrominoFiller.PlacedBricks)
            {
                UpdatePlacedBrickPosition(placed);
            }
        }
    }

    private void FillGrid()
    {
        if (_gamePlayController == null)
        {
            return;
        }
        _gamePlayController.SpawnBrick();
        _tower.StopPhysics();

        // Get the new currentBrick available.
        _gamePlayController.Inject(this);
        _tower.AddTowerPart(_currentBrick);

        // Set brick to be non-interactable
        _currentBrick.paused = true;
        _currentBrick.solid = false;
        _currentBrick.gravityScale = 0;
        _currentBrick.ignoreInHeightCalculations = true;
        _currentBrick.outOfScreen = false;

        // One brick placed at a time
        Tetromino brick = TetrominoFactory.Create(_currentBrick.resourceId);
        PlacedTetromino placed = _tetrominoFiller.FillInitialFrame(brick);
        if (placed != null)
        {
            // Bind placed to real brick instance
            placed.brickInstance = _currentBrick;
            UpdatePlacedBrickPosition(placed);
        }
        else
        {
            _currentBrick.gameObject.SetActive(false);
            _finishedBrickSpawning = true;
        }
    }

    private void UpdatePlacedBrickPosition(PlacedTetromino placed)
    {
        placed.brickInstance.rotation = placed.Rotation;
        if (placed.Visible)
        {
            placed.brickInstance.gameObject.SetActive(true);
            placed.brickInstance.globalPosition = new Vector3(placed.Position.x - 60, placed.Position.y - 5);
        }
        else
        {
            placed.brickInstance.gameObject.SetActive(false);
        }
    }


    public void SetZoomableCamera(ZoomableCamera zoomableCamera)
    {
        _zoomableCamera = zoomableCamera;
    }
}
