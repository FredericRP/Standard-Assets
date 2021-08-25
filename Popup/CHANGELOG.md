# Popup Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
For 1.2.2 release and previous ones, this is a copy/paste from FredericRP's Standard Assets changelog, filtered for this sub package (that's why some version have no content here).

## Unreleased

### Added
- Ability to use prefab directly from popup descriptor, and custom editor
- GetParameter helper to get an indexed parameter for popups
- Close function from popup to be able to link unity events from a popup gameobject

### Changed
- Do not use GUIDs for assembly references to make it clearer if an assembly is not found

### Fixed
- version in package, preparing next release
- singleton package version
- Popups: Debug messages are enabled only when UNITY_EDITOR and DEBUG are defined

## [1.2.2] - 2021-07-30

### Changed
**Breaking change**: use only GameEvent (Raise, Listen, Delete) methods instead of EventHandler (Trigger, Add, Remove), that was always meant to be that way.

### Fixed
- FIX: Editor platform only for editor assemblies on asset bundle tool, event management and object pool

## [1.2.1] - 2020-07-30

### Added
- NEW: Installation guidelines for both package and submodule.

### Changed
- MOD: popup handler supports both legacy and new input system
- MOD: make package manager compatible

### Fixed
- FIX: changelog meta files
- FIX: correct MIT license for every package
- FIX: readme file with updated Documentation folder

## [1.2.0] - 2019-11-19

### Added
- NEW: Popup Management !

### Changed
- MOD: updated readme files for each plugin
- MOD: updated readme file for full instructions

## [1.1.0] - 2019-10-27

## [1.0.1] - 2019-10-16

### Added
- NEW: github social preview

### Changed
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
