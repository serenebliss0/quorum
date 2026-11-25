# Quorum — WPF UI Design Specification

**Figma Source:** [Quorum — WPF UI Design](https://www.figma.com/design/P2iXhyeWO3VTTAM2oZlScS)  
**Figma Pages:** `01 · Foundations & Tokens`, `02 · Wireframes`, `03 · High Fidelity (Light)`, `04 · High Fidelity (Dark)`  
**Reference Direction:** Windows 11 Fluent Design (Microsoft WPF Gallery Preview) — Mica window backdrop, translucent card layers, hairline borders, rounded corners, left navigation rail with 3 px accent indicator.

---

## 1. Style Guide & Design Tokens

### 1.1 Color Palette (Light & Dark Modes)

| Token | Light | Dark | Usage |
|---|---|---|---|
| `bg/mica` | `#F3F5F8` | `#1B1E24` | Window backdrop and left navigation rail |
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

### 1.2 Typography (Single Font)

- **Font Family:** `Segoe UI` (`Segoe UI Variable` on Windows 11; rendered as `Noto Sans` in Figma)
- **Scale:**
  - `Title`: `24 px` Bold (line height `32 px`)
  - `Section`: `16 px` SemiBold (line height `22 px`)
  - `BodyStrong`: `13 px` SemiBold (line height `18 px`)
  - `Body`: `13 px` Regular (line height `18 px`)
  - `Caption`: `11 px` Medium (line height `16 px`)

### 1.3 Spacing & Radii (`8 px` Base)

- **Spacing:** `4 px` (label-to-input), `8 px` (inline gap), `12 px` (form row gap), `16 px` (card padding), `24 px` (page/canvas padding)
- **Corner Radius:** `6 px` (inputs, buttons, nav items), `8 px` (cards, `DataGrid` container), `12 px` (window frame), `999 px` (status pills)

---

## 2. Screen 1 — Login + Sign Up (`AuthWindow`)

### 2.1 Wireframe & High-Fidelity Layout

- **Figma Frames:**
  - Wireframe: `02 · Wireframes` → `WF / 1A · Login (AuthWindow)`, `WF / 1B · Sign Up (AuthWindow)`
  - High Fidelity: `03 · High Fidelity (Light)` & `04 · High Fidelity (Dark)` → `1A · Login`, `1B · Sign Up`
- **Structure:** Centered single-card dialog (`480 × 480` Login, `480 × 670` Sign Up) over Mica backdrop with a two-tab `TabControl` (`Login` | `Sign Up`). Administrators use the `Login` tab with their provisioned account; `Sign Up` only offers `Student` and `Teacher`.

```text
+------------------------------------------+
| Quorum — AuthWindow              _ [] X  |
+------------------------------------------+
|  +------------------------------------+  |
|  | [ Login ]  [ Sign Up ]  (TabControl)| |
|  |                                    |  |
|  | Create Account                     |  |
|  | Account Type   [ComboBox: Student] |  |
|  | Full Name      [TextBox]           |  |
|  | Username       [TextBox]           |  |
|  | Password       [PasswordBox]       |  |
|  | Matric Number  [TextBox] (Student) |  |
|  | [ OperationResult.Message Banner ] |  |
|  | [ Create Account ] (Button)        |  |
|  +------------------------------------+  |
+------------------------------------------+
```

### 2.2 WPF Control List

| Region | WPF Controls |
|---|---|
| Root & Card | `Grid` (window root), `StackPanel` (centered form container) |
| Mode Switcher | `TabControl` (`Login` tab, `Sign Up` tab) |
| Login Tab | `TextBlock` (labels), `TextBox` (Username), `PasswordBox` (Password), `TextBlock` (error banner), `Button` (`Sign In`) |
| Sign Up Tab | `TextBlock` (labels), `ComboBox` (`Student` \| `Teacher`), `TextBox` (Full Name), `TextBox` (Username), `PasswordBox` (Password), `TextBox` (Matric Number — visible only when `Student` is selected), `TextBlock` (status banner), `Button` (`Create Account`) |

### 2.3 Button-to-Logic Mapping

| Button | Logic Method Called | Notes |
|---|---|---|
| `Sign In` | `AuthService.Login(username, password)` | Returns `User?`; routes by `User.Role` (`Student`, `Teacher`, `Administrator`) |
| `Create Account` (`Student`) | `AuthService.RegisterStudent(fullName, username, password, matricNo)` | Returns `OperationResult` |
| `Create Account` (`Teacher`) | `AuthService.RegisterTeacher(fullName, username, password)` | Returns `OperationResult` |

### 2.4 Lists, Grids & `OperationResult.Message` Placement

- **Lists/Grids:** `ComboBox` Account Type lists `Student` and `Teacher` only.
- **Feedback Display:** Inline banner `TextBlock` positioned directly above the primary action button inside the form card:
  - On `Sign Up`, binds to `OperationResult.Message` (success or validation failure).
  - On `Login`, displays `"Invalid username or password."` when `AuthService.Login` returns `null`.

---

## 3. Screen 2 — Student Window (`StudentWindow`)

### 3.1 Wireframe & High-Fidelity Layout

- **Figma Frames:**
  - Wireframe: `02 · Wireframes` → `WF / 2 · StudentWindow`
  - High Fidelity: `03 · High Fidelity (Light)` & `04 · High Fidelity (Dark)` → `2 · StudentWindow`
- **Structure (`960 × 740`):** Two-column `Grid`: `210 px` left navigation rail (`Browse Courses`, `My Courses`, credit total summary card, `Log Out` button) + rounded right workspace canvas containing status banners and course `DataGrid` cards.

```text
+--------------------+---------------------------------------------+
| Quorum — StudentWindow                                   _ [] X  |
+--------------------+---------------------------------------------+
| | Browse Courses   | Course Registration                         |
|   My Courses       | [ TextBlock: OperationResult.Message ]      |
|                    | [ TextBlock: MinCredits Warning (if true) ] |
|                    | +-----------------------------------------+ |
|                    | | Available Courses           [ Register ]  | |
|                    | | DataGrid: Code|Title|Credits|Cap|Status   | |
|                    | +-----------------------------------------+ |
| +----------------+ | +-----------------------------------------+ |
| | Credit Load    | | | My Courses                      [ Drop ]  | |
| | 9 / 24 Credits | | | DataGrid: Code|Title|Credits|RegisteredAt | |
| +----------------+ | +-----------------------------------------+ |
| [ Log Out ]        |                                             |
+--------------------+---------------------------------------------+
```

### 3.2 WPF Control List

| Region | WPF Controls |
|---|---|
| Shell Layout | `Grid` (2 columns: `210 px` rail + `*` workspace), `StackPanel` (rail & card stacks) |
| Navigation & Summary | `TabControl` (`Browse Courses`, `My Courses`), `TextBlock` (credit total, e.g. `9 / 24 Credits`), `Button` (`Log Out`) |
| Feedback Banners | `TextBlock` (`OperationResult.Message` banner), `TextBlock` (advisory warning banner, visible when `IsBelowMinCredits == true`) |
| Browse Courses Card | `TextBlock` (heading), `Button` (`Register`), `DataGrid` (Available Courses) |
| My Courses Card | `TextBlock` (heading), `Button` (`Drop`), `DataGrid` (My Courses) |

### 3.3 Button-to-Logic Mapping

| Button | Logic Method Called | Post-Action Refresh |
|---|---|---|
| `Register` | `RegistrationService.Register(studentId, courseCode)` | Reloads `CourseService.BrowseCourses()`, `CourseService.GetCourseStatus(courseCode)`, `RegistrationService.GetStudentRegistrations(studentId)`, `RegistrationService.IsBelowMinCredits(studentId)` |
| `Drop` | `RegistrationService.Drop(studentId, courseCode)` | Reloads `CourseService.BrowseCourses()`, `CourseService.GetCourseStatus(courseCode)`, `RegistrationService.GetStudentRegistrations(studentId)`, `RegistrationService.IsBelowMinCredits(studentId)` |
| `Log Out` | — (UI session clear) | Closes `StudentWindow`, returns to `AuthWindow` |

### 3.4 Lists, Grids & `OperationResult.Message` Placement

- **Available Courses `DataGrid`:** `Code`, `Title`, `Credits`, `Capacity` (`enrolled / capacity`), `Status` (derived `Open` / `Running` / `Full` via `CourseService.GetCourseStatus(courseCode)`).
- **My Courses `DataGrid`:** `Code`, `Title`, `Credits`, `RegisteredAt` (from `RegistrationService.GetStudentRegistrations(studentId)`).
- **`OperationResult.Message`:** Displayed in the top status banner `TextBlock` directly below the `Course Registration` page title.
- **`IsBelowMinCredits(studentId)` Warning:** Displayed in a dedicated amber warning `TextBlock` immediately beneath the status banner whenever `IsBelowMinCredits(studentId)` returns `true`.

---

## 4. Screen 3 — Teacher Window (`TeacherWindow`)

### 4.1 Wireframe & High-Fidelity Layout

- **Figma Frames:**
  - Wireframe: `02 · Wireframes` → `WF / 3 · TeacherWindow`
  - High Fidelity: `03 · High Fidelity (Light)` & `04 · High Fidelity (Dark)` → `3 · TeacherWindow`
- **Structure (`960 × 660`):** `210 px` left navigation rail (`My Courses`, `Create Course`, `Enrolments`, `Log Out`) + right workspace with status banner, `My Courses` `DataGrid`, and a two-column bottom split (`Create Course` form on left, `Enrolled Students` `DataGrid` on right).

```text
+--------------------+---------------------------------------------+
| Quorum — TeacherWindow                                   _ [] X  |
+--------------------+---------------------------------------------+
| | My Courses       | Teacher Workspace                           |
|   Create Course    | [ TextBlock: OperationResult.Message ]      |
|   Enrolments       | +-----------------------------------------+ |
|                    | | My Courses                              | |
|                    | | DataGrid: Code|Title|Cap|MinToRun|Status  | |
|                    | +-----------------------------------------+ |
|                    | +--------------------+ +------------------+ |
|                    | | Create Course      | | Enrolled Students| |
|                    | | Code [TextBox]     | | DataGrid:        | |
|                    | | Title [TextBox]    | | MatricNo|FullName| |
|                    | | Cap/Min [TextBox]  | | |Username        | |
| [ Log Out ]        | | [ Create Course ]  | +------------------+ |
+--------------------+---------------------------------------------+
```

### 4.2 WPF Control List

| Region | WPF Controls |
|---|---|
| Shell Layout | `Grid` (2 columns + bottom 2-column split), `StackPanel` (form & rail stacks) |
| Navigation | `TabControl` (`My Courses`, `Create Course`, `Enrolments`), `Button` (`Log Out`) |
| Feedback Banner | `TextBlock` (`OperationResult.Message` banner) |
| My Courses Card | `TextBlock` (heading), `DataGrid` (Teacher's courses) |
| Create Course Form | `TextBlock` (labels), `TextBox` (Code), `TextBox` (Title), `TextBox` (Capacity), `TextBox` (MinToRun), `Button` (`Create Course`) — **no Credits input** |
| Enrolled Students Card | `TextBlock` (selected course heading), `DataGrid` (Enrolled Students) |

### 4.3 Button-to-Logic Mapping

| Trigger / Button | Logic Method Called | Post-Action Refresh |
|---|---|---|
| `Create Course` | `CourseService.CreateCourse(actor, code, title, capacity, minToRun)` | Reloads `CourseService.GetTeacherCourses(actor)` + `CourseService.GetCourseStatus(courseCode)` |
| Select row in `My Courses` | `RegistrationService.GetCourseEnrolments(actor, courseCode)` | Populates Enrolled Students `DataGrid` |
| `Log Out` | — (UI session clear) | Closes `TeacherWindow`, returns to `AuthWindow` |

### 4.4 Lists, Grids & `OperationResult.Message` Placement

- **My Courses `DataGrid`:** `Code`, `Title`, `Capacity`, `MinToRun`, `Status` (derived `Open` / `Running` / `Full` from `CourseService.GetCourseStatus(courseCode)`). New courses default to `Credits = 3` in Logic.
- **Enrolled Students `DataGrid`:** `MatricNumber`, `FullName`, `Username` for the selected course (from `RegistrationService.GetCourseEnrolments(actor, courseCode)`).
- **`OperationResult.Message`:** Displayed in the top status banner `TextBlock` directly below the `Teacher Workspace` title.

---

## 5. Screen 4 — Admin Window (`AdminWindow`)

### 5.1 Wireframe & High-Fidelity Layout

- **Figma Frames:**
  - Wireframe: `02 · Wireframes` → `WF / 4 · AdminWindow`
  - High Fidelity: `03 · High Fidelity (Light)` & `04 · High Fidelity (Dark)` → `4 · AdminWindow`
- **Structure (`960 × 720`):** `210 px` left navigation rail (`All Courses`, `All Enrolments`, `Academic Policy`, `Log Out`) + right workspace with status banner, `All Courses` `DataGrid` with inline `Update Credits` bar (`1–6`), and bottom split (`All Enrolments` on left, `AcademicPolicy` form on right).

```text
+--------------------+---------------------------------------------+
| Quorum — AdminWindow                                     _ [] X  |
+--------------------+---------------------------------------------+
| | All Courses      | Administrator Console                       |
|   All Enrolments   | [ TextBlock: OperationResult.Message ]      |
|   Academic Policy  | +-----------------------------------------+ |
|                    | | All Courses                             | |
|                    | | Selected: SEN201 [TextBox:3] [Update Cr]| |
|                    | | DataGrid: Code|Title|Credits|Cap|Min|St | |
|                    | +-----------------------------------------+ |
|                    | +-------------------+ +-------------------+ |
|                    | | All Enrolments    | | AcademicPolicy    | |
|                    | | [ComboBox: Course]| | Min/Max [TextBox] | |
|                    | | DataGrid:         | | Start/End [Date]  | |
| [ Log Out ]        | | Matric|Name|Code  | | [ Save Policy ]   | |
+--------------------+---------------------------------------------+
```

### 5.2 WPF Control List

| Region | WPF Controls |
|---|---|
| Shell Layout | `Grid` (2 columns + bottom 2-column split), `StackPanel` (form & rail stacks) |
| Navigation | `TabControl` (`All Courses`, `All Enrolments`, `Academic Policy`), `Button` (`Log Out`) |
| Feedback Banner | `TextBlock` (`OperationResult.Message` banner) |
| All Courses + Edit Credits | `TextBlock` (selected course code), `TextBox` (Credits `1–6`), `Button` (`Update Credits`), `DataGrid` (All Courses) |
| All Enrolments Card | `ComboBox` (course filter), `DataGrid` (Enrolled Students) |
| AcademicPolicy Form | `TextBox` (`MinCredits`), `TextBox` (`MaxCredits`), `DatePicker` (`RegistrationStart`), `DatePicker` (`RegistrationEnd`), `Button` (`Save Policy`) |

### 5.3 Button-to-Logic Mapping

| Trigger / Button | Logic Method Called | Post-Action Refresh |
|---|---|---|
| `Update Credits` | `CourseService.UpdateCourseCredits(actor, courseCode, credits)` | Reloads `CourseService.BrowseCourses()` + `CourseService.GetCourseStatus(courseCode)` |
| Select course in `Filter Course` `ComboBox` | `RegistrationService.GetCourseEnrolments(actor, courseCode)` | Updates All Enrolments `DataGrid` |
| `Save Policy` | `PolicyService.SavePolicy(actor, policy)` | Reloads `PolicyService.GetPolicy()` |
| `Log Out` | — (UI session clear) | Closes `AdminWindow`, returns to `AuthWindow` |

### 5.4 Lists, Grids & `OperationResult.Message` Placement

- **All Courses `DataGrid`:** `Code`, `Title`, `Credits`, `Capacity`, `MinToRun`, `Status` (from `CourseService.BrowseCourses()` + `CourseService.GetCourseStatus(courseCode)`).
- **All Enrolments `DataGrid`:** `MatricNumber`, `FullName`, `CourseCode` for the selected course in the `ComboBox` (via `RegistrationService.GetCourseEnrolments(actor, courseCode)`).
- **`OperationResult.Message`:** Displayed in the top status banner `TextBlock` directly below the `Administrator Console` title for both `UpdateCourseCredits` and `SavePolicy`.
