# mob-mentality

`mob-mentality` is a Unity 6 2D roguelike / wave-survival prototype. The player is a small mob army pursuing a single rival wizard. Surviving each wave presents three army upgrade cards, while the wizard automatically grows stronger after every choice.

This first vertical slice uses generated square sprites and deliberately simple nearest-target combat. There are no external art assets, paid packages, networking, saves, audio, or procedural levels.

## Open the project

1. Install Unity Hub and a Unity 6 editor. The project targets the Unity 6 / `6000.0` line; allowing a newer Unity 6 LTS editor to upgrade it is fine.
2. In Unity Hub, choose **Add > Add project from disk**.
3. Select this repository's root folder (the folder containing `Assets`, `Packages`, and `ProjectSettings`).
4. Wait for package restore and script compilation to finish.

## Play

Open `Assets/_MobMentality/Scenes/Main.unity` and press **Play**. Click **Start Wave** or press Space. The blue player army automatically pursues the single purple wizard while he launches ranged spells at them. When the wizard is defeated, select one of the three upgrade cards. The mob and wizard strengths update, and the next wave button becomes available.

The **Debug: Complete** button (or C key) skips the remaining combat so the reward loop is easy to inspect.

## Run tests

In the editor, open **Window > General > Test Runner**, choose **EditMode**, and click **Run All**.

For Windows batch mode, set `UNITY_EDITOR` to the editor executable installed on your machine, then run:

```powershell
& $env:UNITY_EDITOR -batchmode -nographics -projectPath $PWD -runTests -testPlatform EditMode -testResults TestResults.xml -logFile unity-tests.log -quit
```

Example placeholder path (replace the version and location):

```powershell
$env:UNITY_EDITOR = "C:\Path\To\Unity\Hub\Editor\6000.0.xf1\Editor\Unity.exe"
```

Unity writes NUnit-format results to `TestResults.xml` and its execution log to `unity-tests.log`.

## Current scope

- A single progressively stronger rival wizard and automatic combat
- Wave completion when the wizard is defeated
- Three distinct reward choices
- Mob stat upgrades and automatic boss growth
- HUD for wave, phase, mob strength, and boss strength
- Plain C# progression models with EditMode tests

See [GAME_DESIGN.md](GAME_DESIGN.md), [ARCHITECTURE.md](ARCHITECTURE.md), and [PLAN.md](PLAN.md) for the intended direction.
