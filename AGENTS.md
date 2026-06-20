# Instructions for Codex and AI contributors

## Working style

- Read `GAME_DESIGN.md`, `ARCHITECTURE.md`, and the relevant code before changing behavior.
- Keep each task small, reviewable, and centered on one gameplay outcome.
- Prefer plain C# for rules and state. Use `MonoBehaviour` only for scene lifecycle, input, visuals, and other Unity integration.
- Keep classes focused; do not create a catch-all manager.
- Use clear names, four-space indentation, braces on new lines, and explicit access modifiers.
- Add XML `<summary>` comments to public classes and public methods.
- Publish meaningful transitions through `GameEvents` rather than tightly coupling systems.
- Add or update tests whenever rules change.

## Do not overbuild

- Do not add speculative abstractions, service locators, dependency-injection frameworks, or generic event buses.
- Do not add final art, paid assets, save/load, inventory, procedural maps, networking, or complex AI unless the task explicitly asks for them.
- Do not turn a small feature request into a broad refactor.
- Do not hide game rules in scene objects when a small plain C# class is enough.

## Tests

Use **Window > General > Test Runner** for interactive runs. For Windows batch mode, point `UNITY_EDITOR` at the local Unity executable:

```powershell
& $env:UNITY_EDITOR -batchmode -nographics -projectPath $PWD -runTests -testPlatform EditMode -testResults TestResults.xml -logFile unity-tests.log -quit
```

Do not assume Unity is installed at one exact path. A placeholder is acceptable in documentation; discover the editor path locally before automation.

## Task handoff

After every task:

1. Summarize behavior changed.
2. List files created or changed.
3. State tests run and their result, or explain why they could not run.
4. State assumptions.
5. Recommend the next smallest useful task.
