# Future Plans

This document outlines planned improvements and enhancements for FreakyControls that are scheduled for future development.

## Testing & Quality Assurance

### Comprehensive Unit Test Suite
Create unit tests for all FreakyControls to ensure reliability and maintainability.

**Unit Tests (Cross-Platform):**
- Property getters and setters
- Default values
- Bindable properties
- Event firing and handlers
- Edge cases and boundary conditions

**Platform-Specific Handler & UI Tests:**
- Native dropdown sizing and layout behavior (Android, iOS, macOS)
- Border rendering validation (Android GradientDrawable, Apple CALayer, Windows WinUI)
- Touch/click event propagation on each platform
- Keyboard navigation and accessibility
- Memory cleanup and handler disposal

**Scope:** All controls including but not limited to:
- FreakyAutoCompleteView
- FreakyButton
- FreakyEntry
- FreakyEditor
- FreakyCodeView
- FreakyZoomableView
- FreakySwitch
- FreakyChip
- FreakyJumpList
- FreakyImage
- FreakyRadioButton
- And all other controls in the library

**Priority:** High  
**Status:** Not Started

---

## Code Quality & Safety

### Enable Nullable Reference Types
Enable `<Nullable>enable</Nullable>` in Maui.FreakyControls.csproj to enable nullable reference type checking and eliminate null-reference warnings across the codebase.

**Tasks:**
- Enable nullable in project file
- Resolve null-safety warnings
- Add appropriate null checks and null-coalescing operators
- Update method signatures with proper null annotations (`?`, `!`)

**Priority:** Medium  
**Status:** Not Started

---

## Documentation

### API Documentation
Ensure all public properties and methods have comprehensive XML documentation comments for IntelliSense support across all controls.

**Status:** Partially Complete

---

## Performance & Optimization

*Planned items to be added as they are identified*

---

## Known Limitations

*Items will be tracked here as edge cases and limitations are discovered*

---

**Last Updated:** August 6, 2026
