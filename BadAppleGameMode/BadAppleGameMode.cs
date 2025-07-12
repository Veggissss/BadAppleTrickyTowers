using System;
using System.Collections.Generic;
using UnityEngine;

public class BadAppleGameMode : AbstractEndlessGameMode
{
    private DataModelFloat _pacerSpeedModel;
    private DataModelFloat _pacerHeightModel;
    private DataModelFloat _targetHeightModel;
    private TowerHeightModel _towerHeightModel;

    private EndlessRaceGameModePlayController _gameModePlayController;

    private readonly BadAppleAnimator _badAppleAnimator = new BadAppleAnimator();
    private float _time = 0;

    protected override void _Init()
    {
        base._Init();
        _pacerSpeedModel = new DataModelFloat();
        _pacerSpeedModel.value = 10000000f;
        _pacerHeightModel = new DataModelFloat();
        _pacerHeightModel.value = -500000f;
        _targetHeightModel = new DataModelFloat();
        _targetHeightModel.value = 999900f;

        _brickEffectTypesByState = new Dictionary<string, Type[]>();
        List<Type> list = new List<Type>();
        list.Add(typeof(BrickClipper));
        list.Add(typeof(BrickSplash));
        _brickEffectTypesByState.Add("ROOF", list.ToArray());
        _brickEffectTypesByState.Add("GAME", list.ToArray());
    }

    protected override void _InitStateControllers()
    {
        base._InitStateControllers();
        AddStateController("INTRO", new EndlessRaceGameModeIntroController());
        AddStateController("COUNTDOWN", new RaceGameModeCountDownController(_musicResources));
        _gameModePlayController = new EndlessRaceGameModePlayController(_timeModel, _highestTowerModel, _lowestTowerModel);
        _gameModePlayController.countDownComplete += _HandleCountDownCompleted;
        AddStateController("PLAY", _gameModePlayController);
    }

    protected override void _Cleanup()
    {
        base._Cleanup();
        if (_gameModePlayController != null)
        {
            _gameModePlayController.countDownComplete -= _HandleCountDownCompleted;
        }
    }

    protected override void _SetCustomGameStateControllers(AbstractGameController gameController)
    {
        return;
        Dictionary<string, AbstractStateController> dictionary = new Dictionary<string, AbstractStateController>();
        dictionary.Add("ROOF", new LocalRoofController("ROOF_01", hideHudOnFinish: false));
        dictionary.Add("BASK", new SinglePlayerLeaderboardBaskController(hideHUDOnFinish: false, playWinStinger: false, "SCORE_HEIGHT"));
        gameController.customStateControllers = dictionary;
    }

    protected override TowerPartRemoveChecker _CreateTowerPartRemoveChecker(GameModel gameModel)
    {
        return new TowerPartRemoveCheckerSurvival(0f);
    }

    protected override void _CreateHud(GameModel gameModel, AbstractGameController gameController, Rect viewPort)
    {
        return;
    }

    protected override void _FillGameModel(GameModel gameModel, AbstractGameController gameController)
    {
        base._FillGameModel(gameModel, gameController);
        DataModelInt dataModelInt = new DataModelInt();
        dataModelInt.value = (int)_highscore;
        gameModel.AddDataModel("HIGHSCORE", dataModelInt);
        gameModel.AddDataModel("SCORE_HEIGHT", new DataModelInt());
        gameModel.AddDataModel("PACER_SPEED", _pacerSpeedModel);
        gameModel.AddDataModel("PACER_HEIGHT", _pacerHeightModel);
        gameModel.AddDataModel("TARGET_HEIGHT", _targetHeightModel);
        if (gameModel.ContainsDataModel("TOWER_HEIGHT"))
        {
            _towerHeightModel = gameModel.GetDataModel<TowerHeightModel>("TOWER_HEIGHT");
            _towerHeightModel.value = 0;
        }
        else
        {
            Debug.LogError("Could not find datamodel TOWER_HEIGHT!");
        }
    }

    protected override string _GetIdByCondition(AbstractCondition condition)
    {
        foreach (string key in _gameEndConditions.Keys)
        {
            if (_gameEndConditions[key] == condition)
            {
                return key;
            }
        }

        return null;
    }

    protected override BrickGuide _CreateBrickGuide(GameModel gameModel)
    {
        BrickGuide brickGuide = new BrickGuide(new Color(1f, 83f / 85f, 81f / 85f, 0.5f));
        brickGuide.minBottom = -8.5f;
        return brickGuide;
    }


    protected override void _Update()
    {
        _time += Time.deltaTime;

        // Set camera to be at the bottom
        _towerHeightModel.value = 0;

        var enemyController = this._gameControllers.Find(x => x is EnemyGameController);
        if (enemyController != null)
        {
            Debug.Log("Removed enemy controller");
            this._gameControllers.Remove(enemyController);
        }
        var singlePlayerController = this._gameControllers.Find(x => x is SinglePlayerLocalGameController);
        if (singlePlayerController != null)
        {
            singlePlayerController.Inject(_badAppleAnimator);
            _badAppleAnimator.UpdateAnimation(_time);
        }
    }
}
