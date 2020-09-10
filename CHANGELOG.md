# Standard Assets Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

### Added
- NEW: supporting two and three arguments for event handling

### Changed
- MOD: Transition namespace has been renamed to ScreenTransitions to prevent namespace conflicting with Transition class
- MOD: EventHandler class has been renamed to GameEventHandler to prevent issues when you want to import System
**Breaking change**: use only GameEvent (Raise, Listen, Delete) methods instead of EventHandler (Trigger, Add, Remove), that was always meant to be that way.
- MOD: to make it fully understandable, the bucket demo displays the full list of randomized numbers

### Fixed
- FIX: Editor platform only for editor assemblies on asset bundle tool, event management and object pool
- FIX: transition animations for 16/9 ratio

## [1.2.1] - 2020-07-30

### Added
- NEW: Generic, Float and Int reference value (scriptableobject)
- NEW: Installation guidelines for both package and submodule.

### Changed
- MOD: popup handler supports both legacy and new input system
- MOD: transition supports both legacy and new input system
- MOD: make package manager compatible
- MOD: transition is no more a singleton to allow multiple instances. Get the right one from its name.

### Fixed
- FIX: changelog meta files
- FIX: warning log when using incorrect delegate on event handler
- FIX: Int Float game event inspector did not allow to change value
- FIX: correct MIT license for every package
- FIX: readme file with updated Documentation folder

## [1.2.0] - 2019-11-19

### Added
- NEW: demo scene for bucket usage
- NEW: Popup Management !

### Changed
- MOD: asset bundle editor window made smaller and with a description
- MOD: english tips instead of french ones
- MOD: updated readme files for each plugin
- MOD: updated readme file for full instructions

### Fixed
- FIX: transition animator call when exiting play mode caused a null ref exception
- FIX: find prefab by using exact id instead of prefix
- FIX: set default parent at instantiating to prevent bad Canvas or UI behavior
- FIX: object pool id editable from inspector

## [1.1.0] - 2019-10-27

### Added
- NEW: typed game event with int and float parameters
- NEW: asset bundle tools (export and local load)
- NEW: tips loader (from asset bundle, and display in canvas)

### Fixed
- FIX: remove event listener on disable simple event listener
- FIX: object pool ID was not accessible and changeable from inspector
- FIX: object pool editor update for unity to handle correctly the setDirty

### Changed
- MOD: max and buffer int fields are wider
- MOD: TipsLoader is now  singleton for multi scene handling without S.O
- MOD: prepare handling of heavy awake on objectPool

## [1.0.1] - 2019-10-16

### Added
- NEW: github social preview
- NEW: transition demo gif
- NEW: bucket generator
- NEW: readme for bucket

### Changed
- MOD: pictures to illustrate transition and object pool
- MOD: all previous readme.txt changed to markdown format

### Fixed
- FIX: bucket class instead of bucket generator

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
