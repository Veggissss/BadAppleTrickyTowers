using System;
using System.Collections.Generic;
using UnityEngine;

public class BadAppleGameMode : AbstractSinglePlayerGameMode
{
    public const float WATER_HEIGHT = -8.5f;

    public int targetBrickCount;

    public float targetHeight;

    public float targetTime;

    private DataModelFloat _targetHeightModel;

    private Dictionary<string, AbstractCondition> _towerHeightModels = new Dictionary<string, AbstractCondition>();

    private DataModelFloat _timeLeftModel;

    private SinglePlayerGameModePlayController _gameModePlayController;

    private BadAppleAnimator _badAppleAnimator = new BadAppleAnimator();

    private float _time = 0;


    protected override float _GetWizardXOffset()
    {
        return 0;
    }

    protected override void _AddGameController(AbstractGameController gameController)
    { 
        // No enemy wizard
    }

    protected override void _Init()
    {
        base._Init();
        Shader.SetGlobalFloat("_WaterCutoff", -8.5f);
        Shader.SetGlobalColor("_WaterColor", ColorUtil.FromHex(2768553u));
        Shader.SetGlobalColor("_WaterLineColor", ColorUtil.FromHex(12106473u));
        Shader.EnableKeyword("WATER_ON");
        _timeLeftModel = new DataModelFloat();
        _timeLeftModel.value = 99999f;
        _targetHeightModel = new DataModelFloat();
        _targetHeightModel.value = 999f;
        _brickEffectTypesByState = new Dictionary<string, Type[]>();
    }

    protected override void _InitStateControllers()
    {
        base._InitStateControllers();
        AddStateController("EXPLANATION", new GameModeExplanationController(_explanationId, _showControls, "INTRO", skipIntroduction));
        AddStateController("COUNTDOWN", new RaceGameModeCountDownController(_musicResources));
        AddStateController("INTRO", new SinglePlayerRaceGameModeIntroController(_targetHeightModel, skipIntroduction));
        _gameModePlayController = new SinglePlayerRaceGameModePlayController(_highestTowerModel, _lowestTowerModel, _timeModel, _timeLeftModel, _targetHeightModel);
        _gameModePlayController.countDownComplete += _HandleCountDownCompleted;
        _gameModePlayController.countDownAborted += _HandleCountDownAborted;
        _gameModePlayController.countDownStarted += _HandleCountDownStarted;
        AddStateController("PLAY", _gameModePlayController);
    }

    protected override void _Cleanup()
    {
        base._Cleanup();
        Shader.DisableKeyword("WATER_ON");
        if (_gameModePlayController != null)
        {
            _gameModePlayController.countDownComplete -= _HandleCountDownCompleted;
            _gameModePlayController.countDownAborted -= _HandleCountDownAborted;
            _gameModePlayController.countDownStarted -= _HandleCountDownStarted;
        }
    }

    protected override void _CreateHud(GameModel gameModel, AbstractGameController gameController, Rect viewPort)
    {
        AbstractHUD hud = new SinglePlayerRaceHUD(gameModel, viewPort);
        gameController.SetHud(hud);
    }

    protected override void _FillGameModel(GameModel gameModel, AbstractGameController gameController)
    {
        base._FillGameModel(gameModel, gameController);
        gameModel.AddDataModel("TARGET_HEIGHT", _targetHeightModel);
        //gameModel.AddDataModel("TIME_LEFT", _timeLeftModel);
        //gameModel.AddDataModel("BRICKS_SPAWNED", new BricksSpawnedModel());
        TowerHeightModel dataModel = gameModel.GetDataModel<TowerHeightModel>("TOWER_HEIGHT");
        CompareConditionFloat value = new CompareConditionFloat(dataModel, _targetHeightModel, ComparisonType.GREATER_THAN_OR_EQUAL, ValueDirection.FREE);
        _towerHeightModels.Add(gameController.id, value);
        DataModelFloat dataModelFloat = new DataModelFloat();
        dataModelFloat.value = 0f;
        CompareConditionFloat endCondition = new CompareConditionFloat(_timeLeftModel, dataModelFloat, ComparisonType.LESS_THAN_OR_EQUAL, ValueDirection.FREE);
        _SetEndCondition(gameController, endCondition);
    }

    protected override string _GetIdByCondition(AbstractCondition condition)
    {
        foreach (string key in _towerHeightModels.Keys)
        {
            if (_towerHeightModels[key] == condition)
            {
                return key;
            }
        }

        return base._GetIdByCondition(condition);
    }

    protected override BrickGuide _CreateBrickGuide(GameModel gameModel)
    {
        BrickGuide brickGuide = new BrickGuide(new Color(133f / 255f, 59f / 85f, 1f, 0.75f));
        brickGuide.minBottom = -8.5f;
        return brickGuide;
    }

    protected override void _SetLevelId(string value)
    {
        base._SetLevelId(value);
    }

    protected override void _HandleGameEndConditionSuccess(AbstractCondition condition)
    {
        string id = _GetIdByCondition(condition);
        _OnGameEnd(id);
        if (Settings.debug.showHudAtEnd)
        {
            return;
        }

        for (int i = 0; i < _backgrounds.Length; i++)
        {
            if (_backgrounds[i] is FinishLineSingleBackground)
            {
                (_backgrounds[i] as FinishLineSingleBackground).RemovePaceLine();
            }
        }
    }

    protected override void _HandleCountDownCompleted(AbstractGameController gameControler)
    {
        if (!Settings.debug.showHudAtEnd)
        {
            for (int i = 0; i < _backgrounds.Length; i++)
            {
                if (_backgrounds[i] is FinishLineSingleBackground)
                {
                    (_backgrounds[i] as FinishLineSingleBackground).RemovePaceLine();
                }
            }
        }

        base._HandleCountDownCompleted(gameControler);
    }
    protected override void _Update()
    {
        _time += Time.deltaTime;

        var enemyController = this._gameControllers.Find(x => x is EnemyGameController);
        if (enemyController != null)
        {
            Debug.Log("Removed enemy controller");
            this._gameControllers.Remove(enemyController);
        }
        var singlePlayerController = this._gameControllers.Find(x => x is SinglePlayerLocalGameController);
        if (singlePlayerController != null)
        {
            //singlePlayerController.DisableBrickSpawning();
            singlePlayerController.Inject(_badAppleAnimator);
            _badAppleAnimator.UpdateAnimation(_time);
        }
    }
}
