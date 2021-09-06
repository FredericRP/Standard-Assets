# Event management Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
For 1.2.2 release and previous ones, this is a copy/paste from FredericRP's Standard Assets changelog, filtered for this sub package (that's why some version have no content here).

## Unreleased

### Changed
- Do not use GUIDs for assembly references to make it clearer if an assembly is not found
- Added virtual to GameEvent methods to allow override

### Fixed
- Do not throw a null pointer when exiting play mode when removing event listener
- Added namespace for value reference children classes

## [1.2.2] - 2021-07-30

### Added
- NEW: supporting two and three arguments for event handling
- NEW: ability to use either a global GameEventHandler or multiple ones

### Changed
- MOD: EventHandler class has been renamed to GameEventHandler to prevent issues when you want to import System
**Breaking change**: use only GameEvent (Raise, Listen, Delete) methods instead of EventHandler (Trigger, Add, Remove), that was always meant to be that way.

### Fixed
- FIX: Editor platform only for editor assemblies on asset bundle tool, event management and object pool

## [1.2.1] - 2020-07-30

### Added
- NEW: Generic, Float and Int reference value (scriptableobject)
- NEW: Installation guidelines for both package and submodule.

### Changed
- MOD: make package manager compatible

### Fixed
- FIX: changelog meta files
- FIX: warning log when using incorrect delegate on event handler
- FIX: Int Float game event inspector did not allow to change value
- FIX: correct MIT license for every package
- FIX: readme file with updated Documentation folder

## [1.2.0] - 2019-11-19

### Changed
- MOD: updated readme files for each plugin
- MOD: updated readme file for full instructions

## [1.1.0] - 2019-10-27

### Added
- NEW: typed game event with int and float parameters

### Fixed
- FIX: remove event listener on disable simple event listener

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
