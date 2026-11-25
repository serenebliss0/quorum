# Quorum — WPF UI Design Specification

**Figma Source:** [Quorum — WPF UI Design](https://www.figma.com/design/P2iXhyeWO3VTTAM2oZlScS)  
**Figma Pages:** `01 · Foundations & Tokens`, `02 · Wireframes`, `03 · High Fidelity (Light)`, `04 · High Fidelity (Dark)`  
**Reference Direction:** Windows 11 Fluent Design (Microsoft WPF Gallery Preview) — single-window Mica shell, translucent card layers, hairline borders, rounded corners, collapsible left navigation rail with 3 px accent indicator.

---

## 1. Window Architecture & Responsiveness

Quorum runs inside **one single `MainWindow`** (`1280 × 720` default, `WindowStartupLocation="CenterScreen"`). Views swap inside a root `ContentControl` (`LoginView`, `SignUpView`, `StudentView`, `TeacherView`, `AdminView`) without spawning separate top-level windows.

### Responsive Rules (`MinWidth = 800`, `MinHeight = 600`)
- **Expanded (`>= 960 px` width, e.g., `1280 × 720` or `1920 × 1080`):**
  - Left navigation rail is `220 px` wide (icon + label + `Credit Load` summary + `Log Out`).
  - Workspace uses `*` proportional `Grid` columns (`ScrollViewer` wraps content vertically so tall tables never clip).
  - Auth views (`LoginView`, `SignUpView`) center a `440–460 px` `MaxWidth` card inside the full `MainWindow`.
  - Split cards on `TeacherView` and `AdminView` sit side-by-side (`1*` / `1*`).
- **Compact (`< 960 px` width, e.g., `800 × 720` half-screen snap):**
  - Left navigation rail collapses to a `56 px` icon-only rail.
  - Two-column splits (`TeacherView`, `AdminView`) reflow into a single-column vertical stack inside the workspace `ScrollViewer`.
  - `DataGrid` columns use `*` star widths so horizontal resizing never clips cells.

---

## 2. Style Guide & Design Tokens

### 2.1 Color Palette (Light & Dark Modes)

| Token | Light | Dark | Usage |
|---|---|---|---|
| `bg/mica` | `#F3F5F8` | `#1B1E24` | `MainWindow` backdrop and left navigation rail |
| `bg/surface` | `#FFFFFF` (92%–96%) | `#252932` (90%–96%) | Cards, inputs, `DataGrid` rows, workspace canvas |
| `border/hairline` | `#DCE1EA` | `#363C49` | 1 px strokes on cards, inputs, and tables |
| `text/primary` | `#181B22` | `#F2F4F8` | Headings, table cells, primary labels |
| `accent/primary` | `#0067C0` | `#4CC2FF` | Primary buttons, active nav bar, credit total |

Secondary & Derived Tokens:
- **Muted Text (`text/secondary`):** `#5A6374` (Light) / `#9FA8B8` (Dark)
- **Subtle Fill (`bg/subtle`):** `#EAEFF5` (Light) / `#2E333E` (Dark) — `DataGrid` column headers, tab strip track
- **Status Pills (`CourseStatus`):**
  - `Open`: `#E6F1FB` / `#005A9E` (Light) · `#1F364D` / `#60CDFF` (Dark)
  - `Running`: `#E6F4EA` / `#0F7B0F` (Light) · `#1D3826` / `#6CCB5F` (Dark)
  - `Full`: `#FDE7E9` / `#C42B1C` (Light) · `#422125` / `#FF99A4` (Dark)
- **Advisory Warning (`IsBelowMinCredits`):** `#FFF4CE` / `#8A5300` (Light) · `#433519` / `#FCE100` (Dark)

### 2.2 Typography (Single Font)

- **Font Family:** `Segoe UI` (`Segoe UI Variable` on Windows 11; rendered as `Noto Sans` in Figma)
- **Scale:**
  - `Title`: `24 px` Bold (line height `32 px`)
  - `Section`: `16 px` SemiBold (line height `22 px`)
  - `BodyStrong`: `13 px` SemiBold (line height `18 px`)
  - `Body`: `13 px` Regular (line height `18 px`)
  - `Caption`: `11 px` Medium (line height `16 px`)

### 2.3 Spacing & Radii (`8 px` Base)

- **Spacing:** `4 px` (label-to-input), `8 px` (inline gap), `12 px` (form row gap), `16 px` (card padding), `24 px` (page/canvas padding)
- **Corner Radius:** `6 px` (inputs, buttons, nav items), `8 px` (cards, `DataGrid` container), `12 px` (window frame), `999 px` (status pills)

---

## 3. View 1 — `LoginView` + `SignUpView` (inside `MainWindow`)

### 3.1 Wireframe & High-Fidelity Layout

- **Figma Frames (`1280 × 720`):**
  - Wireframe: `02 · Wireframes` → `WF / MainWindow — LoginView`, `WF / MainWindow — SignUpView`
  - High Fidelity: `03 · High Fidelity (Light)` & `04 · High Fidelity (Dark)` → `MainWindow · LoginView (1280x720)`, `MainWindow · SignUpView (1280x720)`
- **Structure:** Centered card (`MaxWidth = 440–460 px`) inside the `1280 × 720` `MainWindow` with a two-tab `TabControl` (`Login` | `Sign Up`). Administrators sign in on `Login` with their provisioned account; `Sign Up` only allows `Student` and `Teacher`.

```text
+----------------------------------------------------------------------+
| Quorum · SignUpView (1280x720)                               _ [] X  |
+----------------------------------------------------------------------+
|                                                                      |
|                  +--------------------------------+                  |
|                  | [ Login ]  [ Sign Up ] (Tabs)  |                  |
|                  | Create Account                 |                  |
|                  | Account Type [ComboBox]        |                  |
|                  | Full Name    [TextBox]         |                  |
|                  | Username     [TextBox]         |                  |
|                  | Password     [PasswordBox]     |                  |
|                  | Matric No.   [TextBox] (Stud)  |                  |
|                  | [ OperationResult.Message ]    |                  |
|                  | [ Create Account ] (Button)    |                  |
|                  +--------------------------------+                  |
|                                                                      |
+----------------------------------------------------------------------+
```

### 3.2 WPF Control List

| Region | WPF Controls |
|---|---|
| Shell & Card | `Grid` (centered `MaxWidth` container inside `MainWindow`), `StackPanel` (form stack) |
| Mode Switcher | `TabControl` (`Login` tab, `Sign Up` tab) |
| `LoginView` | `TextBlock` (labels), `TextBox` (Username), `PasswordBox` (Password), `TextBlock` (error banner), `Button` (`Sign In`) |
| `SignUpView` | `TextBlock` (labels), `ComboBox` (`Student` \| `Teacher`), `TextBox` (Full Name), `TextBox` (Username), `PasswordBox` (Password), `TextBox` (Matric Number — visible only when `Student` is selected), `TextBlock` (status banner), `Button` (`Create Account`) |

### 3.3 Button-to-Logic Mapping

| Button | Logic Method Called | Notes |
|---|---|---|
| `Sign In` | `AuthService.Login(username, password)` | Returns `User?`; `MainWindow` swaps `ContentControl` by `User.Role` (`UserRole.Student`, `UserRole.Teacher`, `UserRole.Administrator`) |
| `Create Account` (`Student`) | `AuthService.RegisterStudent(fullName, username, password, matricNo)` | Returns `OperationResult` |
| `Create Account` (`Teacher`) | `AuthService.RegisterTeacher(fullName, username, password)` | Returns `OperationResult` |

### 3.4 Lists, Grids & `OperationResult.Message` Placement

- **Lists/Grids:** `ComboBox` Account Type lists `Student` and `Teacher` only.
- **Feedback Display:** Inline banner `TextBlock` positioned directly above the primary action button inside the centered card:
  - On `SignUpView`, binds to `OperationResult.Message`.
  - On `LoginView`, displays `"Invalid username or password."` when `AuthService.Login` returns `null`.

---

## 4. View 2 — `StudentView` (inside `MainWindow`)

### 4.1 Wireframe & High-Fidelity Layout

- **Figma Frames (`1280 × 720`):**
  - Wireframe: `02 · Wireframes` → `WF / MainWindow — StudentView`
  - High Fidelity: `03 · High Fidelity (Light)` & `04 · High Fidelity (Dark)` → `MainWindow · StudentView (1280x720)`
- **Structure:** Responsive two-column `Grid`: collapsible left navigation rail (`220 px` expanded / `56 px` compact) + scrollable right workspace with banners and full-width `DataGrid` cards.

```text
+----------------------------------------------------------------------+
| Quorum · StudentView (1280x720)                              _ [] X  |
+------------------+---------------------------------------------------+
| | Browse Courses | Course Registration                               |
|   My Courses     | [ TextBlock: OperationResult.Message ]            |
|                  | [ TextBlock: MinCredits Warning (if true) ]       |
|                  | +-----------------------------------------------+ |
|                  | | Available Courses                 [ Register ] | |
|                  | | DataGrid: Code | Title | Credits | Cap | Status| |
|                  | +-----------------------------------------------+ |
| +--------------+ | +-----------------------------------------------+ |
| | Credit Load  | | | My Courses                            [ Drop ] | |
| | 9 / 24 Cr.   | | | DataGrid: Code | Title | Credits | RegisteredAt| |
| +--------------+ | +-----------------------------------------------+ |
| [ Log Out ]      |                                                   |
+------------------+---------------------------------------------------+
```

### 4.2 WPF Control List

| Region | WPF Controls |
|---|---|
| Shell Layout | `Grid` (2 columns: rail + `*` scrollable workspace), `StackPanel` (rail & card stacks) |
| Navigation & Summary | `TabControl` (`Browse Courses`, `My Courses`), `TextBlock` (credit total, e.g. `9 / 24 Credits`), `Button` (`Log Out`) |
| Feedback Banners | `TextBlock` (`OperationResult.Message` banner), `TextBlock` (advisory warning banner, visible when `IsBelowMinCredits == true`) |
| Browse Courses Card | `TextBlock` (heading), `Button` (`Register`), `DataGrid` (Available Courses) |
| My Courses Card | `TextBlock` (heading), `Button` (`Drop`), `DataGrid` (My Courses) |

### 4.3 Button-to-Logic Mapping

| Button | Logic Method Called | Post-Action Refresh |
|---|---|---|
| `Register` | `RegistrationService.Register(studentId, courseCode)` | Reloads `CourseService.BrowseCourses()`, `CourseService.GetCourseStatus(courseCode)`, `RegistrationService.GetStudentRegistrations(studentId)`, `RegistrationService.IsBelowMinCredits(studentId)` |
| `Drop` | `RegistrationService.Drop(studentId, courseCode)` | Reloads `CourseService.BrowseCourses()`, `CourseService.GetCourseStatus(courseCode)`, `RegistrationService.GetStudentRegistrations(studentId)`, `RegistrationService.IsBelowMinCredits(studentId)` |
| `Log Out` | — (UI session clear) | Clears active `User` in `MainWindow` and switches back to `LoginView` |

### 4.4 Lists, Grids & `OperationResult.Message` Placement

- **Available Courses `DataGrid`:** `Code`, `Title`, `Credits`, `Capacity` (`enrolled / capacity`), `Status` (derived `Open` / `Running` / `Full` via `CourseService.GetCourseStatus(courseCode)`).
- **My Courses `DataGrid`:** `Code`, `Title`, `Credits`, `RegisteredAt` (from `RegistrationService.GetStudentRegistrations(studentId)`).
- **`OperationResult.Message`:** Displayed in the top status banner `TextBlock` directly below the `Course Registration` heading.
- **`IsBelowMinCredits(studentId)` Warning:** Displayed in the amber warning `TextBlock` immediately beneath the status banner whenever `IsBelowMinCredits(studentId)` returns `true`.

---

## 5. View 3 — `TeacherView` (inside `MainWindow`)

### 5.1 Wireframe & High-Fidelity Layout

- **Figma Frames (`1280 × 720`):**
  - Wireframe: `02 · Wireframes` → `WF / MainWindow — TeacherView`
  - High Fidelity: `03 · High Fidelity (Light)` & `04 · High Fidelity (Dark)` → `MainWindow · TeacherView (1280x720)`
- **Structure:** Collapsible left navigation rail + right scrollable workspace with status banner, `My Courses` `DataGrid`, and a responsive two-column split (`Create Course` form on left, `Enrolled Students` `DataGrid` on right; stacks vertically below `960 px`).

```text
+----------------------------------------------------------------------+
| Quorum · TeacherView (1280x720)                              _ [] X  |
+------------------+---------------------------------------------------+
| | My Courses     | Teacher Workspace                                 |
|   Create Course  | [ TextBlock: OperationResult.Message ]            |
|   Enrolments     | +-----------------------------------------------+ |
|                  | | My Courses                                    | |
|                  | | DataGrid: Code | Title | Cap | MinToRun | Stat| |
|                  | +-----------------------------------------------+ |
|                  | +-----------------------+ +---------------------+ |
|                  | | Create Course         | | Enrolled Students   | |
|                  | | Code [TextBox]        | | DataGrid:           | |
|                  | | Title [TextBox]       | | MatricNo | FullName | |
|                  | | Cap/Min [TextBox]     | | | Username          | |
| [ Log Out ]      | | [ Create Course ]     | +---------------------+ |
+------------------+---------------------------------------------------+
```

### 5.2 WPF Control List

| Region | WPF Controls |
|---|---|
| Shell Layout | `Grid` (2-column shell + responsive bottom 2-column split), `StackPanel` (form & rail stacks) |
| Navigation | `TabControl` (`My Courses`, `Create Course`, `Enrolments`), `Button` (`Log Out`) |
| Feedback Banner | `TextBlock` (`OperationResult.Message` banner) |
| My Courses Card | `TextBlock` (heading), `DataGrid` (Teacher's courses) |
| Create Course Form | `TextBlock` (labels), `TextBox` (Code), `TextBox` (Title), `TextBox` (Capacity), `TextBox` (MinToRun), `Button` (`Create Course`) — **no Credits input** |
| Enrolled Students Card | `TextBlock` (selected course heading), `DataGrid` (Enrolled Students) |

### 5.3 Button-to-Logic Mapping

| Trigger / Button | Logic Method Called | Post-Action Refresh |
|---|---|---|
| `Create Course` | `CourseService.CreateCourse(actor, code, title, capacity, minToRun)` | Reloads `CourseService.GetTeacherCourses(actor)` + `CourseService.GetCourseStatus(courseCode)` |
| Select row in `My Courses` | `RegistrationService.GetCourseEnrolments(actor, courseCode)` | Populates Enrolled Students `DataGrid` |
| `Log Out` | — (UI session clear) | Clears active `User` in `MainWindow` and switches back to `LoginView` |

### 5.4 Lists, Grids & `OperationResult.Message` Placement

- **My Courses `DataGrid`:** `Code`, `Title`, `Capacity`, `MinToRun`, `Status` (derived `Open` / `Running` / `Full` via `CourseService.GetCourseStatus(courseCode)`). New courses default to `Credits = 3` in Logic.
- **Enrolled Students `DataGrid`:** `MatricNumber`, `FullName`, `Username` for the selected course (via `RegistrationService.GetCourseEnrolments(actor, courseCode)`).
- **`OperationResult.Message`:** Displayed in the top status banner `TextBlock` directly below the `Teacher Workspace` heading.

---

## 6. View 4 — `AdminView` (inside `MainWindow`)

### 6.1 Wireframe & High-Fidelity Layout

- **Figma Frames (`1280 × 720` + `800 × 720` Compact):**
  - Wireframe: `02 · Wireframes` → `WF / MainWindow — AdminView`, `WF / MainWindow — AdminView (Compact < 960px)`
  - High Fidelity: `03 · High Fidelity (Light)` & `04 · High Fidelity (Dark)` → `MainWindow · AdminView (1280x720)`, `MainWindow · AdminView · Compact Responsive (800x720)`
- **Structure:** Collapsible left navigation rail + right scrollable workspace with status banner, `All Courses` `DataGrid` with inline `Update Credits` (`1–6`), and responsive two-column split (`All Enrolments` on left, `AcademicPolicy` form on right; stacks vertically below `960 px`).

```text
+----------------------------------------------------------------------+
| Quorum · AdminView (1280x720)                                _ [] X  |
+------------------+---------------------------------------------------+
| | All Courses    | Administrator Console                             |
|   All Enrolments | [ TextBlock: OperationResult.Message ]            |
|   Academic Policy| +-----------------------------------------------+ |
|                  | | All Courses                                   | |
|                  | | Selected: SEN201 [TextBox:3] [Update Credits] | |
|                  | | DataGrid: Code|Title|Credits|Cap|Min|Status   | |
|                  | +-----------------------------------------------+ |
|                  | +---------------------+ +-----------------------+ |
|                  | | All Enrolments      | | AcademicPolicy        | |
|                  | | [ComboBox: Course]  | | Min/Max [TextBox]     | |
|                  | | DataGrid:           | | Start/End [DatePicker]| |
| [ Log Out ]      | | Matric|Name|Code    | | [ Save Policy ]       | |
+------------------+---------------------------------------------------+
```

### 6.2 WPF Control List

| Region | WPF Controls |
|---|---|
| Shell Layout | `Grid` (2-column shell + responsive bottom 2-column split), `StackPanel` (form & rail stacks) |
| Navigation | `TabControl` (`All Courses`, `All Enrolments`, `Academic Policy`), `Button` (`Log Out`) |
| Feedback Banner | `TextBlock` (`OperationResult.Message` banner) |
| All Courses + Edit Credits | `TextBlock` (selected course code), `TextBox` (Credits `1–6`), `Button` (`Update Credits`), `DataGrid` (All Courses) |
| All Enrolments Card | `ComboBox` (course filter), `DataGrid` (Enrolled Students) |
| AcademicPolicy Form | `TextBox` (`MinCredits`), `TextBox` (`MaxCredits`), `DatePicker` (`RegistrationStart`), `DatePicker` (`RegistrationEnd`), `Button` (`Save Policy`) |

### 6.3 Button-to-Logic Mapping

| Trigger / Button | Logic Method Called | Post-Action Refresh |
|---|---|---|
| `Update Credits` | `CourseService.UpdateCourseCredits(actor, courseCode, credits)` | Reloads `CourseService.BrowseCourses()` + `CourseService.GetCourseStatus(courseCode)` |
| Select course in `Filter Course` `ComboBox` | `RegistrationService.GetCourseEnrolments(actor, courseCode)` | Updates All Enrolments `DataGrid` |
| `Save Policy` | `PolicyService.SavePolicy(actor, policy)` | Reloads `PolicyService.GetPolicy()` |
| `Log Out` | — (UI session clear) | Clears active `User` in `MainWindow` and switches back to `LoginView` |

### 6.4 Lists, Grids & `OperationResult.Message` Placement

- **All Courses `DataGrid`:** `Code`, `Title`, `Credits`, `Capacity`, `MinToRun`, `Status` (from `CourseService.BrowseCourses()` + `CourseService.GetCourseStatus(courseCode)`).
- **All Enrolments `DataGrid`:** `MatricNumber`, `FullName`, `CourseCode` for the selected course in the `ComboBox` (via `RegistrationService.GetCourseEnrolments(actor, courseCode)`).
- **`OperationResult.Message`:** Displayed in the top status banner `TextBlock` directly below the `Administrator Console` heading for both `UpdateCourseCredits` and `SavePolicy`.
