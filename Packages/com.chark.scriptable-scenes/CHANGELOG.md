﻿# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.0.2] - 2022-09-28
Minor UX updates and bug fixes.

### Added
- More logging which shows that a scene is being currently loaded.
- Methods to access in `loadingCollection` and `loadedCollection` in `ScriptableSceneController`.
- Exposed `IsLoading` property in `ScriptableSceneController`.
- Events in `BaseScriptableSceneTransition`.

### Changed
- Updated Scene Manager Window to support reordering and to provide more info.
- Improved component UX via `AddComponentMenu`.

### Fixed
- Incorrect collection progress being reported when a collection is loading.

## [0.0.1] - 2022-05-21
Initial preview version.

### Added
- `ScriptableScene` - wrapper `ScriptableObject` for `SceneAsset`, which allows referencing scenes without needing to hard-code scene name, path or build index. Click on _Assets > Create > CHARK > Scriptable Scenes > Scriptable Scene_ to create.
- `ScriptableSceneCollection` - container for `ScriptableScene` and is useful to load a set of scenes at once (`SetupScene`, `UIScene`, `GameplayScene`, etc). Click on _Assets > Create > CHARK > Scriptable Scenes > Scriptable Scene Collection_ to create.
- `ScriptableSceneTransition` - `ScriptableObject` that can be used to inject scene transitions.
- `FadeScriptableSceneTransition` - built-in transition which simply fades a canvas in and out (via `FadeCanvas`) during scene loading.
- `FadeCanvas` - built-in component which takes care of actually fading the canvas and subscribing to a `ScriptableSceneTransition`.
- `ScriptableSceneManagerWindow` - Editor Window which can be used to quickly open a set of scene in Edit and also Play mode. Click on _Window > CHARK > Scriptable Scenes > Scriptable Scene Manager_ to open.