# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-11-05

This version represents the first "full" release of PEAKEmoteLib. It integrates the new vanilla changes made to the emote wheel system introduced in the game's Roots update.

### Added

- Documentation comments to source code

### Changed

- Custom emotes now appear in pages on the vanilla emote wheel's pagination system, replacing a bespoke pagination implementation.

### Fixed

- Playing a custom emote no longer permanently overrides the (previously unused, but now used as the "shimmy" vanilla emote) `A_Scout_Emote_Dance2` state with the custom animation data.

## [0.2.0] - 2025-10-26

### Added

- A new `RegisterEmote` overload to `BaseUnityPluginExtensions` to allow registering an emote directly from an `Emote` object. 

## [0.1.0] - 2025-10-26

### Added

- Everything (all initial functionality)