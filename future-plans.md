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
- Border rendering validation (Android GradientDrawable, Apple CALayer)
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
- Resolve null-safety warnings by adding null checks and null-coalescing operators
- Update method signatures with proper null annotations (`?` for nullable, `null!` only when non-null invariant is proven)
- Avoid using `!` as a blanket null-suppression tool; validate non-null contracts first

**Priority:** Medium  
**Status:** Not Started

---

## Documentation

### API Documentation
Ensure all public properties and methods have comprehensive XML documentation comments for IntelliSense support across all controls.

**Tasks:**
- Add XML doc comments to all public members
- Enable compiler warnings for missing documentation (CS1591)
- Enforce CI checks to fail build when public members lack documentation
- Remove documentation warning suppressions

**Status:** Partially Complete

---

## Samples & UI/UX

### Modern Sample App Redesign
Rebuild the Samples application with a modern, polished UI that properly showcases FreakyControls. The current samples are functional but lack visual appeal and don't demonstrate the full potential of the control library.

**Goals:**
- **Modern Design System** — Implement a consistent, visually appealing design language with proper spacing, typography, and color palettes
- **Control Showcase** — Create dedicated showcase screens for each control demonstrating features and use cases (not just basic examples)
- **Dark Mode Support** — Full dark/light theme support with smooth transitions
- **Navigation Patterns** — Implement modern navigation patterns (bottom tabs, side navigation) instead of simple page stacks
- **Interactive Demos** — Add interactive examples where users can adjust property values and see live results
- **Responsive Layouts** — Ensure samples work beautifully on phones, tablets, and desktop platforms
- **Documentation & Guidance** — Include inline help, tooltips, and code snippets for each control demonstration

**Scope:**
- Redesign Samples app shell and navigation with modern design
- Create beautiful showcase screens for FreakyAutoCompleteView, FreakyButton, FreakyEntry, FreakySwitch, and other controls
- Add interactive property editors for live customization
- Implement proper theme support (dark/light)
- Create reusable UI components (cards, section headers, code snippets)
- **Maintain basic samples folder** — Keep simple, straightforward control examples for developers who prefer minimal UI and just want to understand the API

**Priority:** Medium  
**Status:** Not Started

---

## Performance & Optimization

*Planned items to be added as they are identified*

---

## Known Limitations

### FreakyAutoCompleteView — Windows (WinUI 3)
- **DropDownWidth/Height** — AutoSuggestBox dropdown sizing is not exposed via public APIs
- **DropDownBorderColor/Width/CornerRadius** — Dropdown border styling is not supported; the internal popup is not directly accessible for customization

---

**Last Updated:** August 6, 2026
