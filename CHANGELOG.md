# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project
adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [v0.0.9](https://github.com/chark/scriptable-scenes/compare/v0.0.8...v0.0.9) - 2023-10-04

### Added

- Odin Inspector support.
- Samples.
- Documentation.

### Changed

- Migrated to Unity 2022.
- Changed Editor scripts to use UI Toolkit instead of IMGUI.

## Removed

- Missing scene (in build settings) rendering. Will be added later as this consumed too much time to migrate to UI Toolkit.

## [v0.0.8](https://github.com/chark/scriptable-scenes/compare/v0.0.7...v0.0.8) - 2022-10-23

### Changed

- Fixed Scriptable Scene assets not updating when files are moved, deleted or created.
- Updated tests to operate on scene path rather than build index.

## [v0.0.7](https://github.com/chark/scriptable-scenes/compare/v0.0.6...v0.0.7) - 2022-10-22

### Added

- More properties to `BaseScriptableScene` and `ScriptableScene`.

### Changed

- Scenes not added to build settings can now be Opened, Loaded and Activated in Editor. In Player, a warning will be printed for such scenes.
- Removed `BuildIndex` from `BaseScriptableScene` (want to hide it).

## [v0.0.6](https://github.com/chark/scriptable-scenes/compare/v0.0.5...v0.0.6) - 2022-10-17

### Added

- Status icon which shown in Scene Manager indicating if a collection is added to Build Settings.
- Warnings on `ScriptableSceneCollection` and `ScriptableScene` assets which will be if a scene is not added to Build Settings.
- Button in `ScriptableScene` editor to add scene to Build Settings.

## [v0.0.5](https://github.com/chark/scriptable-scenes/compare/v0.0.4...v0.0.5) - 2022-10-16

### Added

- Button tooltips.

### Changed

- Changed global control buttons to use icons in Scriptable Scene Manager.
- Moved collection controls next to title in Scriptable Scene Manager.

## [v0.0.4](https://github.com/chark/scriptable-scenes/compare/v0.0.3...v0.0.4) - 2022-10-15

### Added

- Added more initial scene load options to `ScriptableSceneController`.
- Added more guards when loading invalid scenes.

### Fixed

- `ScriptableSceneController` not cleaning up `IsLoading` and `loadingCollection` on exception.

## [v0.0.3](https://github.com/chark/scriptable-scenes/compare/v0.0.2...v0.0.3) - 2022-09-28

### Fixed

- Fixed transition events not being invoked.

## [v0.0.2](https://github.com/chark/scriptable-scenes/compare/v0.0.1...v0.0.2) - 2022-09-28

Minor UX updates and bug fixes.

### Added

- More logging which shows that a scene is being currently loaded.
- Methods to access in `loadingCollection` and `loadedCollection` in `ScriptableSceneController`.
- Exposed `IsLoading` property in `ScriptableSceneController`.
- Events which are fired when transitions are entered and exited.

### Changed

- Updated Scene Manager Window to support reordering and to provide more info.
- Improved component UX via `AddComponentMenu`.

### Fixed

- Incorrect collection progress being reported when a collection is loading.

## [v0.0.1](https://github.com/chark/scriptable-scenes/compare/v0.0.1) - 2022-05-21

Initial preview version.

### Added

- `ScriptableScene` - wrapper `ScriptableObject` for `SceneAsset`, which allows referencing scenes without needing to hard-code scene name, path or build index. Click on _Assets > Create > CHARK > Scriptable Scenes > Scriptable Scene_ to create.
- `ScriptableSceneCollection` - container for `ScriptableScene` and is useful to load a set of scenes at once (`SetupScene`, `UIScene`, `GameplayScene`, etc). Click on _Assets > Create > CHARK > Scriptable Scenes > Scriptable Scene Collection_ to create.
- `ScriptableSceneTransition` - `ScriptableObject` that can be used to inject scene transitions.
- `FadeScriptableSceneTransition` - built-in transition which simply fades a canvas in and out (via `FadeCanvas`) during scene loading.
- `FadeCanvas` - built-in component which takes care of actually fading the canvas and subscribing to a `ScriptableSceneTransition`.
- `ScriptableSceneManagerWindow` - Editor Window which can be used to quickly open a set of scene in Edit and also Play mode. Click on _Window > CHARK > Scriptable Scenes > Scriptable Scene Manager_ to open.
