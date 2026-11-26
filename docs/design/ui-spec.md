# Quorum — WPF UI Design Specification

**Figma Source:** [Quorum — WPF UI Design](https://www.figma.com/design/P2iXhyeWO3VTTAM2oZlScS)  
**Figma Pages:** `01 · Foundations & Tokens`, `02 · Wireframes`, `03 · High Fidelity (Light)`, `04 · High Fidelity (Dark)`  
**Reference Direction:** Windows 11 Fluent Design (Microsoft WPF Gallery Preview) — single-window Mica shell, automatic Windows system theme detection (`AppsUseLightTheme`), translucent card layers, hairline borders, rounded corners, and a collapsible left navigation rail (`≡ Menu` toggle + `< 960 px` auto-collapse).

---

## 1. Window Architecture, Theme & Responsiveness

Quorum runs inside **one single `MainWindow`** (`1280 × 720` default, `WindowStartupLocation="CenterScreen"`). Views swap inside a root `ContentControl`:
1. `SplashView` (brief `1.3 s` launch splash with placeholder geometric Quorum logo + progress bar)
2. `AdminSetupView` (first-run onboarding when no Administrator exists yet: configure School/University Name + primary Administrator account)
3. `AuthView` (two-column split `Login` / `Sign Up` once an Administrator exists)
4. `StudentView`, `TeacherView`, `AdminView` (role workspaces)

### System Theme & Tooltips
- **System Theme Default:** `ThemeManager.ApplySystemTheme()` reads `HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize\AppsUseLightTheme` on startup and allows manual toggling in the title bar.
- **Hover Tooltips:** Key fields (`MinToRun`, `Capacity`, `Credits`, `MinCredits`, `MaxCredits`, `RegistrationStart`, `RegistrationEnd`), `Status` pills (`Open`, `Running`, `Full`), and navigation/action buttons show Fluent-styled `ToolTip`s after `800 ms` (`ToolTipService.InitialShowDelay="800"`).

### Responsive Rules (`MinWidth = 800`, `MinHeight = 600`)
- **Expanded (`>= 960 px` width):**
  - Left navigation rail defaults to `220 px` and can be manually collapsed/expanded via the `≡ Menu` button (`56 px` ↔ `220 px`).
  - `AdminSetupView` and `AuthView` render a two-column split (`1.05*` Hero Brand/Overview panel + `1*` Form card).
  - Split cards on `TeacherView` and `AdminView` sit side-by-side (`1*` / `1*`).
- **Compact (`< 960 px` width, e.g., `800 × 720`):**
  - Left navigation rail auto-collapses to `56 px` icon-only mode.
  - `AdminSetupView` and `AuthView` hide the left hero panel so the form fills the view cleanly.
  - Two-column splits (`TeacherView`, `AdminView`) reflow into a single-column vertical stack inside the workspace `ScrollViewer`.

---

## 2. Style Guide & Design Tokens

### 2.1 Color Palette (Light & Dark Modes)

| Token | Light | Dark | Usage |
|---|---|---|---|
| `bg/mica` | `#F3F5F8` | `#1B1E24` | `MainWindow` backdrop and left navigation rail |
| `bg/surface` | `#FFFFFF` (90%–96%) | `#252932` (90%–96%) | Cards, inputs, `DataGrid` rows, workspace canvas |
| `border/hairline` | `#DCE1EA` | `#363C49` | 1 px strokes on cards, inputs, tables, tooltips |
| `text/primary` | `#181B22` | `#F2F4F8` | Headings, table cells, primary labels |
| `accent/primary` | `#0067C0` | `#4CC2FF` | Primary buttons, active nav bar, credit total, logo badge |

Secondary & Derived Tokens:
- **Muted Text (`text/secondary`):** `#5A6374` (Light) / `#9FA8B8` (Dark)
- **Subtle Fill (`bg/subtle`):** `#EAEFF5` (Light) / `#2E333E` (Dark) — `DataGrid` column headers, tab track
- **Accent Subtle (`accent/subtle`):** `#E6F1FB` (Light) / `#1F364D` (Dark) — Hero panel, InfoBar, selected row
- **Status Pills (`CourseStatus`):**
  - `Open`: `#E6F1FB` / `#005A9E` (Light) · `#1F364D` / `#60CDFF` (Dark)
  - `Running`: `#E6F4EA` / `#0F7B0F` (Light) · `#1D3826` / `#6CCB5F` (Dark)
  - `Full`: `#FDE7E9` / `#C42B1C` (Light) · `#422125` / `#FF99A4` (Dark)
- **Advisory Warning (`IsBelowMinCredits`):** `#FFF4CE` / `#8A5300` (Light) · `#433519` / `#FCE100` (Dark)

### 2.2 Typography & Spacing

- **Font Family:** `Segoe UI Variable, Segoe UI` (`Noto Sans` in Figma)
  - `Title`: `24 px` Bold · `Section`: `16 px` SemiBold · `BodyStrong`: `13 px` SemiBold · `Body`: `13 px` Regular · `Caption`: `11 px` Medium
- **Spacing (`8 px` base):** `4 px`, `8 px`, `12 px`, `16 px`, `24 px`
- **Corner Radius:** `6 px` (inputs, buttons, tooltips, nav items), `8 px` (cards, tab track), `12 px` (window & hero panels), `999 px` (status pills)

---

## 3. Views Inside `MainWindow`

### 3.0 `SplashView` (`1280 × 720`)
- Centered placeholder geometric Quorum emblem (`76 × 76` rounded badge), `"Quorum"` title, `"Course Registration System"`, and indeterminate `ProgressBar`. Transitions after `1.3 s` to `AdminSetupView` (if `!HasAdminAccount()`) or `AuthView`.

### 3.1 `AdminSetupView` (`1280 × 720`, First-Run Onboarding)
- **Layout:** Two-column split (`1.05*` 3-step onboarding hero + `1*` setup form card).
- **Controls:** `TextBox` (`School / University Name`, `Admin Full Name`, `Admin Username`), `PasswordBox` (`Admin Password`), `TextBlock` (`OperationResult.Message` banner), `Button` (`Complete Setup`, `Go to Login`).
- **Action:** Calls `SetupInstitutionAndAdmin(...)`, persists Administrator (`UserRole.Administrator`) via `PasswordHasher` + `IDataStore` and `InstitutionName`, then signs straight into `AdminView`.

### 3.2 `AuthView` — `Login` & `Sign Up` (`1280 × 720`)
- **Layout:** Two-column split (`1.05*` Institution Hero + Quorum overview cards + `Administrator Login` / `Institution & Admin Setup` shortcut button; `1*` Auth form card with unclipped custom `TabControl` pill track).
- **Controls:**
  - `Login` tab: `ComboBox` (`Sign in as`: `Student` | `Teacher` | `Administrator`), `TextBox` (`Username`), `PasswordBox` (`Password`), `TextBlock` (role/error banner), `Button` (`Sign In`).
  - `Sign Up` tab: `ComboBox` (`Account Type`: `Student` | `Teacher`), `TextBox` (`Full Name`, `Username`, `Matric Number` when `Student`), `PasswordBox` (`Password`), `TextBlock` (`OperationResult.Message`), `Button` (`Create Account`).
- **Logic Calls:** `AuthService.Login(username, password)` (verifying selected role matches `user.Role`), `AuthService.RegisterStudent(...)`, `AuthService.RegisterTeacher(...)`.

### 3.3 `StudentView` (`1280 × 720` + `< 960 px` Compact)
- **Layout:** `≡ Menu` collapsible left rail (`220 px` / `56 px`, `Browse Courses`, `My Courses`, `Credit Load`, `Log Out`) + scrollable workspace with `OperationResult.Message` banner, amber `IsBelowMinCredits` warning banner, `Available Courses` `DataGrid` (`Code`, `Title`, `Credits`, `Capacity`, `Status` pill) + `Register` button, and `My Courses` `DataGrid` (`Code`, `Title`, `Credits`, `RegisteredAt`) + `Drop` button.
- **Logic Calls:** `CourseService.BrowseCourses()`, `CourseService.GetCourseStatus(courseCode)`, `RegistrationService.Register(studentId, courseCode)`, `RegistrationService.Drop(studentId, courseCode)`, `RegistrationService.GetStudentRegistrations(studentId)`, `RegistrationService.IsBelowMinCredits(studentId)`.

### 3.4 `TeacherView` (`1280 × 720` + `< 960 px` Compact)
- **Layout:** `≡ Menu` collapsible left rail (`My Courses`, `Create Course`, `Enrolments`, `Log Out`) + `OperationResult.Message` banner + `My Courses` `DataGrid` (`Code`, `Title`, `Capacity`, `MinToRun`, `Status` pill) + responsive 2-column split (`Create Course` form with `Code`, `Title`, `Capacity`, `MinToRun` — NO credits field — and `Enrolled Students` `DataGrid`: `MatricNumber`, `FullName`, `Username`).
- **Logic Calls:** `CourseService.GetTeacherCourses(actor)`, `CourseService.GetCourseStatus(courseCode)`, `CourseService.CreateCourse(actor, code, title, capacity, minToRun)`, `RegistrationService.GetCourseEnrolments(actor, courseCode)`.

### 3.5 `AdminView` (`1280 × 720` + `< 960 px` Compact)
- **Layout:** `≡ Menu` collapsible left rail (`All Courses`, `All Enrolments`, `Teachers`, `Settings & Policy`, `Log Out`) + `OperationResult.Message` banner + `All Courses` `DataGrid` (`Code`, `Title`, `Credits`, `Capacity`, `MinToRun`, `Status` pill) with inline `Credits (1–6)` `TextBox` + `Update Credits` button + responsive splits for:
  - `All Enrolments` (`ComboBox` course filter + `MatricNumber`, `FullName`, `CourseCode` `DataGrid`)
  - `Institution & AcademicPolicy` (`School / University Name` + `Save School`, `MinCredits`, `MaxCredits`, `RegistrationStart` `DatePicker`, `RegistrationEnd` `DatePicker` + `Save Policy`)
  - `Teachers` (`Full Name`, `Username`, `Password` + `Add Teacher` button, alongside `Registered Teachers` `DataGrid`: `FullName`, `Username`)
- **Logic Calls:** `CourseService.BrowseCourses()`, `CourseService.UpdateCourseCredits(actor, courseCode, credits)`, `RegistrationService.GetCourseEnrolments(actor, courseCode)`, `PolicyService.GetPolicy()`, `PolicyService.SavePolicy(actor, policy)`, `AuthService.RegisterTeacher(...)`, plus institution/admin onboarding methods via `IQuorumGateway`.
