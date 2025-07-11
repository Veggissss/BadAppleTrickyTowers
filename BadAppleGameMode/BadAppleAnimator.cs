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
        if (time - _animationTime >= 1)
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
            _tower.AddTowerPart(_currentBrick);

            int i = 0;
            foreach (TowerPart tp in _tower.towerParts)
            {
                i++;
                if (tp is Brick brick)
                {
                    Debug.Log($"Brick by brick by brick: {i}");
                    brick.MakePhysical();
                    brick.Remove(true);
                    brick.globalPosition.Set(0, 0, 0);
                    brick.gravityScale = 0;
                    brick.mass = 99999f;
                }
            }
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
