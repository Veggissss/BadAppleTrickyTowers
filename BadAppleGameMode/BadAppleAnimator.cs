using System;
using UnityEngine;
using UnityEngine.Experimental.Director;

public class BadAppleAnimator : IInjectable, ITowerInjectable, IGamePlayControllerInjectable, IBrickInjectable
{
    private Tower _tower;
    private float _animationTime;
    private Brick _parentBrick;
    private Brick _currentBrick;
    private LocalGamePlayController _gamePlayController;
    private int x = 0;

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
        if (time - _animationTime >= 1f)
        {
            _animationTime = time;
            if (_tower.GetLastBrick() == null)
            {
                return;
            }
            else if (_parentBrick == null)
            {
                _parentBrick = _tower.GetLastBrick();
            }
            _gamePlayController.SpawnBrick();

            // Get the new currentBrick available.
            _gamePlayController.Inject(this);
            // BRICK_T, BRICK_L BRICK_J, BRICK_O, BRICK_I, BRICK_S, BRICK_Z | O is 2x2 solid.
            // Grid size = 35 + 1 + 35
            Debug.Log(_currentBrick.resourceId);
            _tower.AddTowerPart(_currentBrick);
            
            //brick.Remove(true);
            _currentBrick.MakePhysical();
            _currentBrick.paused = true;
            _currentBrick.ignoreInHeightCalculations = true;
            x++;
            _currentBrick.globalPosition = new Vector3(x, 3);
            _currentBrick.outOfScreen = false;
            _currentBrick.gravityScale = 0;
            _currentBrick.mass = 99999f;
            _currentBrick.solid = false;
            // Create a new brick using the same resource as the parent
            //BadBrick newBrick = new BadBrick(_parent_bick.resourceId);
            //newBrick.Init();

            // Position above the parent brick
            //newBrick.globalPosition = new Vector3(
            //    _parent_bick.x,
            //   _parent_bick.y + _parent_bick.height + 0.1f,
            //  0
            //);

            // Add to tower
            //_tower.AddTowerPart(newBrick);
        }
    }
}
