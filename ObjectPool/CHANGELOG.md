# Object Pool Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
For 1.2.2 release and previous ones, this is a copy/paste from FredericRP's Standard Assets changelog, filtered for this sub package (that's why some version have no content here).

## Unreleased

### Fixed
- Prevent exception if using GetObjectPool method when there is no existing pool

### Changed
- Do not use GUIDs for assembly references to make it clearer if an assembly is not found

## [1.2.2] - 2021-07-30

### Changed
**Breaking change**: use only GameEvent (Raise, Listen, Delete) methods instead of EventHandler (Trigger, Add, Remove), that was always meant to be that way.

### Fixed
- FIX: Editor platform only for editor assemblies on asset bundle tool, event management and object pool

## [1.2.1] - 2020-07-30

### Added
- NEW: Installation guidelines for both package and submodule.

### Changed
- MOD: make package manager compatible

### Fixed
- FIX: changelog meta files
- FIX: correct MIT license for every package
- FIX: readme file with updated Documentation folder

## [1.2.0] - 2019-11-19

### Changed
- MOD: updated readme files for each plugin
- MOD: updated readme file for full instructions

### Fixed
- FIX: find prefab by using exact id instead of prefix
- FIX: set default parent at instantiating to prevent bad Canvas or UI behavior
- FIX: object pool id editable from inspector

## [1.1.0] - 2019-10-27

### Fixed
- FIX: object pool ID was not accessible and changeable from inspector
- FIX: object pool editor update for unity to handle correctly the setDirty

### Changed
- MOD: max and buffer int fields are wider
- MOD: prepare handling of heavy awake on objectPool

## [1.0.1] - 2019-10-16

### Added
- NEW: github social preview

### Changed
- MOD: pictures to illustrate transition and object pool
- MOD: all previous readme.txt changed to markdown format

### Removed
- DEL: removed first package export

## [1.0.0] - 2019-08-08

### Added
- first public release: a generic event handler to trigger and subscribe game events in your game in a minute.

### Changed

### Deprecated

### Removed

### Fixed

### Security
