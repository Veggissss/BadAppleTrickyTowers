using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(InitSinglePlayerChallenge1ModeCommand))]
[HarmonyPatch("Execute")]
class AddNewRaceAndNewModesPatch
{
    static AccessTools.FieldRef<InitSinglePlayerChallenge1ModeCommand, List<SelectModel>> singlePlayerWorldsRef =
        AccessTools.FieldRefAccess<InitSinglePlayerChallenge1ModeCommand, List<SelectModel>>("_singlePlayerWorlds");

    static bool Prefix(InitSinglePlayerChallenge1ModeCommand __instance)
    {
        List<SelectModel> challenges = new List<SelectModel>(); //GetAllOldChallenges()

        BadAppleGameModeFactory badAppleGameModeFactory = new BadAppleGameModeFactory();
        badAppleGameModeFactory.brickPickerFactory = new RandomNamedBrickPickerFactory(new string[1] { "BRICK_O" }); //new string[5] { "BRICK_Z", "BRICK_T", "BRICK_S", "BRICK_L", "BRICK_J" }
        badAppleGameModeFactory.wizardInvinceable = true;
        //InitGameTypesAndModesCommand //"RACE", "LBRW", "Race Leaderboard World"
        //badAppleGameModeFactory.floorFactory = new FloorFactory("FLOOR_RACE_ENDLESS", 12.5f);

        challenges.Add(new BadAppleGameModeModel("SR_10", "Single Player", badAppleGameModeFactory));

        // Add new bad apple mode and old modes as well
        singlePlayerWorldsRef(__instance).Add(new WorldModel("SR_10", "LBRW", "BAD_APPLE", challenges));

        return false;
    }

    // Keep the original single player challenge modes
    private static List<SelectModel> GetAllOldChallenges()
    {
        SinglePlayerRaceGameModeFactory singlePlayerRaceGameModeFactory = new SinglePlayerRaceGameModeFactory(14f, 60f);
        singlePlayerRaceGameModeFactory.curseSpawnerFactory = new ScriptedCurseSpawnerFactory(new AbstractCurseSpawnStruct[1]
        {
            new HeightCurseSpawnStruct(11f, "LARGE")
        });
        singlePlayerRaceGameModeFactory.brickPickerFactory = new RandomNamedBrickPickerFactory();
        singlePlayerRaceGameModeFactory.sceneryFactory = new SceneryFactory();
        singlePlayerRaceGameModeFactory.sceneryFactory.RegisterScenery("NONE", new Scenery[1]
        {
            new TowerScenery("FLOOR_RACE", "ENEMY_TOWER_LEVEL_01")
        });
        SinglePlayerSurvivalGameModeFactory singlePlayerSurvivalGameModeFactory = new SinglePlayerSurvivalGameModeFactory(15, 3);
        singlePlayerSurvivalGameModeFactory.curseSpawnerFactory = new ScriptedCurseSpawnerFactory(new AbstractCurseSpawnStruct[3]
        {
            new BricksCurseSpawnStruct(5, "LARGE"),
            new BricksCurseSpawnStruct(10, "LARGE"),
            new BricksCurseSpawnStruct(15, "LARGE")
        });
        singlePlayerSurvivalGameModeFactory.brickPickerFactory = new RandomNamedBrickPickerFactory(null, -1, 15);
        singlePlayerSurvivalGameModeFactory.dropSpeedControllerFactory = new DropSpeedControllerFactory(3f);
        singlePlayerSurvivalGameModeFactory.floorFactory = new FloorFactory("FLOOR_LWS", 12.5f);
        SinglePlayerPuzzleGameModeFactory singlePlayerPuzzleGameModeFactory = new SinglePlayerPuzzleGameModeFactory(9, 2.2f, 1);
        singlePlayerPuzzleGameModeFactory.brickPickerFactory = new OrderedNamedBrickPickerFactory(new string[9] { "BRICK_O", "BRICK_Z", "BRICK_S", "BRICK_T", "BRICK_T", "BRICK_O", "BRICK_T", "BRICK_T", "BRICK_I" }, 9);
        singlePlayerPuzzleGameModeFactory.floorFactory = new FloorFactory("FLOOR_PUZZLE_BASIC", 15.5f);
        SinglePlayerPuzzleGameModeFactory singlePlayerPuzzleGameModeFactory2 = new SinglePlayerPuzzleGameModeFactory(6, 5.2f, 1);
        singlePlayerPuzzleGameModeFactory2.brickPickerFactory = new OrderedNamedBrickPickerFactory(new string[6] { "BRICK_J", "BRICK_I", "BRICK_S", "BRICK_Z", "BRICK_L", "BRICK_J" }, 6);
        singlePlayerPuzzleGameModeFactory2.floorFactory = new FloorFactory("FLOOR_OVERHANG_TUTORIAL", 12f);
        SinglePlayerRaceGameModeFactory singlePlayerRaceGameModeFactory2 = new SinglePlayerRaceGameModeFactory(16f, 60f);
        singlePlayerRaceGameModeFactory2.curseSpawnerFactory = new ScriptedCurseSpawnerFactory(new AbstractCurseSpawnStruct[3]
        {
        new HeightCurseSpawnStruct(4f, "GRASSY"),
        new HeightCurseSpawnStruct(8f, "LARGE"),
        new HeightCurseSpawnStruct(11f, "GRASSY")
        });
        singlePlayerRaceGameModeFactory2.brickPickerFactory = new RandomNamedBrickPickerFactory();
        singlePlayerRaceGameModeFactory2.sceneryFactory = new SceneryFactory();
        singlePlayerRaceGameModeFactory2.sceneryFactory.RegisterScenery("NONE", new Scenery[1]
        {
        new TowerScenery("FLOOR_RACE", "ENEMY_TOWER_LEVEL_02")
        });
        SinglePlayerSurvivalGameModeFactory singlePlayerSurvivalGameModeFactory2 = new SinglePlayerSurvivalGameModeFactory(20, 3);
        singlePlayerSurvivalGameModeFactory2.curseSpawnerFactory = new ScriptedCurseSpawnerFactory(new AbstractCurseSpawnStruct[4]
        {
        new BricksCurseSpawnStruct(5, "MYSTERY"),
        new BricksCurseSpawnStruct(10, "MYSTERY"),
        new BricksCurseSpawnStruct(13, "MYSTERY"),
        new BricksCurseSpawnStruct(20, "LARGE_MYSTERY")
        });
        singlePlayerSurvivalGameModeFactory2.brickPickerFactory = new RandomNamedBrickPickerFactory(null, -1, 20);
        singlePlayerSurvivalGameModeFactory2.spellPickerFactory = new OrderedSpellPickerFactory(new string[1] { "PETRIFY" });
        singlePlayerSurvivalGameModeFactory2.spellContainerSpawnerFactory = new SpellContainerSpawnerFactory(12f, 1);
        singlePlayerSurvivalGameModeFactory2.dropSpeedControllerFactory = new DropSpeedControllerFactory(3f);
        singlePlayerSurvivalGameModeFactory2.floorFactory = new FloorFactory("FLOOR_LWS_PETRIFIED_BRICK", 12.5f);
        SinglePlayerPuzzleGameModeFactory singlePlayerPuzzleGameModeFactory3 = new SinglePlayerPuzzleGameModeFactory(7, 1.2f, 1);
        singlePlayerPuzzleGameModeFactory3.brickPickerFactory = new OrderedNamedBrickPickerFactory(new string[7] { "BRICK_O", "BRICK_Z", "BRICK_S", "BRICK_L", "BRICK_S", "BRICK_J", "BRICK_Z" }, 7);
        singlePlayerPuzzleGameModeFactory3.floorFactory = new FloorFactory("FLOOR_PUZZLE_LEVEL01", 15f);
        SinglePlayerRaceGameModeFactory singlePlayerRaceGameModeFactory3 = new SinglePlayerRaceGameModeFactory(22f, 70f);
        singlePlayerRaceGameModeFactory3.curseSpawnerFactory = new ScriptedCurseSpawnerFactory(new AbstractCurseSpawnStruct[4]
        {
        new HeightCurseSpawnStruct(5f, "GRASSY"),
        new HeightCurseSpawnStruct(9f, "LARGE"),
        new HeightCurseSpawnStruct(10f, "GRASSY"),
        new HeightCurseSpawnStruct(15f, "SLOW")
        });
        singlePlayerRaceGameModeFactory3.brickPickerFactory = new RandomNamedBrickPickerFactory();
        singlePlayerRaceGameModeFactory3.spellPickerFactory = new OrderedSpellPickerFactory(new string[1] { "PETRIFY" });
        singlePlayerRaceGameModeFactory3.spellContainerSpawnerFactory = new SpellContainerSpawnerFactory(9f, 1);
        singlePlayerRaceGameModeFactory3.sceneryFactory = new SceneryFactory();
        singlePlayerRaceGameModeFactory3.sceneryFactory.RegisterScenery("NONE", new Scenery[1]
        {
        new TowerScenery("FLOOR_RACE", "ENEMY_TOWER_LEVEL_03")
        });
        SinglePlayerPuzzleGameModeFactory singlePlayerPuzzleGameModeFactory4 = new SinglePlayerPuzzleGameModeFactory(6, 2.2f, 1);
        singlePlayerPuzzleGameModeFactory4.brickPickerFactory = new OrderedNamedBrickPickerFactory(new string[6] { "BRICK_T", "BRICK_J", "BRICK_L", "BRICK_I", "BRICK_L", "BRICK_O" }, 6);
        singlePlayerPuzzleGameModeFactory4.floorFactory = new FloorFactory("FLOOR_PUZZLE_LONG_I", 14f);
        SinglePlayerSurvivalGameModeFactory singlePlayerSurvivalGameModeFactory3 = new SinglePlayerSurvivalGameModeFactory(26, 3);
        singlePlayerSurvivalGameModeFactory3.curseSpawnerFactory = new ScriptedCurseSpawnerFactory(new AbstractCurseSpawnStruct[7]
        {
            new BricksCurseSpawnStruct(1, "MIST"),
            new BricksCurseSpawnStruct(11, "MIST"),
            new BricksCurseSpawnStruct(19, "MIST"),
            new BricksCurseSpawnStruct(23, "MIST"),
            new BricksCurseSpawnStruct(24, "MYSTERY"),
            new BricksCurseSpawnStruct(25, "MYSTERY"),
            new BricksCurseSpawnStruct(26, "MYSTERY")
        });
        singlePlayerSurvivalGameModeFactory3.brickPickerFactory = new RandomNamedBrickPickerFactory(null, -1, 26);
        singlePlayerSurvivalGameModeFactory3.spellPickerFactory = new InstantSpellPickerFactory(new string[1] { "PETRIFY" });
        singlePlayerSurvivalGameModeFactory3.spellContainerSpawnerFactory = new SpellContainerSpawnerFactory(9f, 1);
        singlePlayerSurvivalGameModeFactory3.dropSpeedControllerFactory = new DropSpeedControllerFactory(3f);
        singlePlayerSurvivalGameModeFactory3.floorFactory = new FloorFactory("FLOOR_LWS", 12.5f);
        return new List<SelectModel>
        {
            new SinglePlayerRaceGameModeModel("SR_10", "Challenge 01", singlePlayerRaceGameModeFactory, "SINGLE_PLAYER_RACE_01"),
            new SinglePlayerSurvivalGameModeModel("SS_17", "Challenge 02", singlePlayerSurvivalGameModeFactory, "SINGLE_PLAYER_SURVIVAL_01"),
            new SinglePlayerPuzzleGameModeModel("SP_20", "Challenge 03", singlePlayerPuzzleGameModeFactory, "SINGLE_PLAYER_PUZZLE_01"),
            new SinglePlayerPuzzleGameModeModel("SP_16", "Challenge 04", singlePlayerPuzzleGameModeFactory2),
            new SinglePlayerRaceGameModeModel("SR_11", "Challenge 05", singlePlayerRaceGameModeFactory2),
            new SinglePlayerSurvivalGameModeModel("SS_10", "Challenge 06", singlePlayerSurvivalGameModeFactory2),
            new SinglePlayerPuzzleGameModeModel("SP_15", "Challenge 07", singlePlayerPuzzleGameModeFactory3),
            new SinglePlayerRaceGameModeModel("SR_18", "Challenge 08", singlePlayerRaceGameModeFactory3),
            new SinglePlayerPuzzleGameModeModel("SP_10", "Challenge 09", singlePlayerPuzzleGameModeFactory4),
            new SinglePlayerSurvivalGameModeModel("SS_11", "Challenge 10", singlePlayerSurvivalGameModeFactory3)
        };
    }
}
