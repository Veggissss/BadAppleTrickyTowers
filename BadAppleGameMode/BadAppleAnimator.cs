using BadAppleTrickyTowersMod.TetrisPlayer;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Director;

public class BadAppleAnimator : IInjectable, ITowerInjectable, IGamePlayControllerInjectable, IBrickInjectable
{
    private Tower _tower;

    private float _animationTime;
    private Brick _parentBrick;
    private Brick _currentBrick;
    private LocalGamePlayController _gamePlayController;
    private bool[,] _currentFrame;
    private int _currentFrameCount;
    private string _frameFolderPath;
    private TetrominoFiller _tetrominoFiller;
    private bool _loadNextFrame;

    public BadAppleAnimator()
    {
        _frameFolderPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "BepInEx/plugins/BadAppleMod/frames/");
        _currentFrameCount = 0;
        LoadNextFrame();
        _loadNextFrame = false;
    }

    private void LoadNextFrame()
    {
        ClearBricks();
        _currentFrameCount++;
        _currentFrame = FrameExtractor.LoadFramesFromFolder(_frameFolderPath, _currentFrameCount);
        _tetrominoFiller = new TetrominoFiller(_currentFrame);
        Debug.Log($"Loading next frame {_currentFrameCount}");
        //_tetrominoFiller.GetFrameGrid().PrintToConsole();
    }

    private void ClearBricks()
    {
        if (_tower == null)
        {
            return;
        }
        foreach (TowerPart tp in _tower.towerParts)
        {
            //_tower.RemoveTowerPartInNextUpdate(tp, false);
            if (tp is Brick brick)
            {
                brick.Remove(false);
                brick.Die();
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
        // Wait between each finish frame so when sped up it is smoother.
        if (_loadNextFrame)
        {
            if (time - _animationTime >= 5)
            {
                _loadNextFrame = false;
                LoadNextFrame();
            }
            else
            {
                // Wait for frame hold
                return;
            }
        }

        // Wait for first block placement.
        if (_tower.GetLastBrick() == null)
        {
            return;
        }
        else if (_parentBrick == null)
        {
            _parentBrick = _tower.GetLastBrick();
        }
        _gamePlayController.SpawnBrick();
        _tower.StopPhysics();

        // Get the new currentBrick available.
        _gamePlayController.Inject(this);
        _tower.AddTowerPart(_currentBrick);
            
        // Set brick properties
        _currentBrick.paused = true;
        _currentBrick.ignoreInHeightCalculations = true;
        _currentBrick.outOfScreen = false;
        _currentBrick.solid = false;
        _currentBrick.gravityScale = 0;

        // One brick at a time (e.g. you get this from a queue)
        Tetromino brick = TetrominoFactory.Create(_currentBrick.resourceId);
        PlacedTetromino placed = _tetrominoFiller.TryPlaceBrick(brick);
        if (placed != null)
        {
            //Debug.Log($"Placed brick {_currentBrick.resourceId} @ {placed.Position}");
            _currentBrick.globalPosition = new Vector3(placed.Position.x-20, placed.Position.y-5);
        }
        else
        {
            //Debug.Log($"Brick {brick.ResourceId} could not be placed.");
            _animationTime = time;
            _loadNextFrame = true;
        }
        
    }
}
