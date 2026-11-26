# Quorum: Shared Project Context

> Source of truth for AI-assisted work on Quorum. Follow this file; do not change decisions unless the team explicitly approves the change. Append a short entry to the session log after each planning session.

## 1. Snapshot

- **Project:** Quorum, Course Registration System, SEN201 Software Engineering Group Project, Group 8.
- **Language and UI:** C# with WPF, Windows only.
- **Storage:** Local JSON file by default. Supabase is optional only if time allows.
- **Repository:** https://github.com/serenebliss0/quorum
- **Deadline:** 4 December 2026.
- **Process model:** Incremental, 3 increments plus an optional fourth.
- **Goal:** Keep the system simple. Prioritise a working, well-documented, compilable application over extra features.

## 2. Team and proposed roles

Confirm roles with the group; these are proposals, not evidence of completed contributions.

| Member | Proposed role |
|---|---|
| AJAYI Oluwasemire (Serenity) | Project Manager and Lead C# Developer (core logic) |
| OKEKE Kenechukwu | Requirements Analyst and System Modeller (SRS, UML diagrams) |
| OKIKE Tochukwu | QA/Tester and UI Developer (WPF views, test plan and test cases) |

Matriculation numbers are TBD and required for the report.

## 3. Assignment deliverables

Submit one PDF report containing:
1. Project overview and technical, operational and schedule feasibility, plus risk summary.
2. SRS: functional and non-functional requirements and short scenarios.
3. Use-case, class, sequence, activity and system-architecture diagrams.
4. Testing log covering unit, integration, system and acceptance tests, including inputs, expected results, actual results and pass/fail.
5. Public GitHub repository link, well-commented code and commit history showing genuine individual contributions.
6. Publicly viewable demo video link showing core features and edge-case validation.
7. Application screenshots.
8. Group member names, matriculation numbers and roles.

All C# must compile and run without errors. Links must be publicly viewable.

## 4. Scope

### Students can
- Create an account and log in.
- Browse courses.
- Register for a course.
- View their registered courses.
- Drop a course.

### Teachers can
- Create an account and log in.
- Create courses, including capacity and minimum-to-run values.
- View students enrolled in their courses.

### Administrators can
- Log in (provisioned account).
- Browse all courses and create courses.
- View students enrolled across all courses.
- Configure minimum and maximum credit limits, individual course credit values, registration periods, academic policies and system settings.

### Business rules
- A student cannot register for the same course twice.
- A student cannot register for a course when enrolled count is greater than or equal to capacity.
- Registrations must respect the configured credit limits and active registration period.
- A course is **Running** once enrolled count is greater than or equal to its minimum-to-run value.
- Course statuses are **Open**, **Running**, and **Full**. Full takes precedence when enrolled count reaches capacity; otherwise Running applies when the minimum-to-run threshold has been met; otherwise the status is Open.

### Stretch / future features (do not promise these in the SRS)
- Teachers viewing other teachers in a course.
- Supabase storage.
- External APIs and event-driven integrations (future possibilities outside the current MVP).

Do not add features beyond this scope without an explicit team decision.

## 5. Architecture

Three layers, with clear separation of concerns:

- **Quorum.UI** (WPF): windows and views only; no business rules.
- **Quorum.Logic**: registration rules, account handling, quorum status and academic policy enforcement.
- **Quorum.Data**: data models, the IDataStore interface and JsonDataStore implementation.
- **Quorum.Tests**: unit tests for the Logic layer.

The test project is separate from the three application layers. Supabase, if implemented, is a second IDataStore implementation. Other layers should not need to change.

### Accounts
- Username, salted password hash and role (Student, Teacher or Administrator; Administrator accounts are provisioned).
- No email verification and no password-reset flow.

### Security
- Never commit API keys or secrets. Keep them out of the repository.

## 6. Draft data contract

Update this contract when the team approves a change.

- **User:** Id, FullName, Username, PasswordHash, Salt, Role.
- **Student : User:** MatricNumber.
- **Course:** Code, Title, Credits, Capacity, MinToRun, TeacherId.
- **Registration:** StudentId, CourseCode, RegisteredAt.
- **AcademicPolicy:** MinCredits, MaxCredits, RegistrationStart, RegistrationEnd, PolicySettings (draft; final fields to be confirmed in code).
- **IDataStore:** load and save users, courses, registrations and academic policies/settings.

Keep the model simple and consistent with the three-layer structure. Do not silently add fields or entities.

## 7. Rules for AI helpers

- Follow this file. Do not change a decision unless told to; log approved changes.
- Artifact ownership:
  - **Claude:** architecture, core C# logic orchestration, debugging and review (ChatGPT may step in on code/logic when Claude hits usage limits).
  - **ChatGPT:** report prose, SRS wording, test tables, implementing Gemini's UI designs in C# (WPF), and backup C# code implementation.
  - **Gemini:** diagrams as Mermaid or PlantUML text, and UI design.
- Do not rewrite files owned by another helper. Review and suggest instead.
- Keep code simple, commented and compiling.
- Do not invent library methods; say when unsure.
- Do not add features beyond Section 4.

## 8. Incremental plan

| Increment | Content | Target |
|---|---|---|
| 1 | Models, JSON storage, sign-up and login | By 24 November |
| 2 | Course creation, student registration, duplicate/full/quorum rules | By 28 November |
| 3 | Teacher view, validation, polish and unit tests | By 30 November |
| 4 (optional) | Supabase implementation | Only if ahead |

Then complete system and acceptance testing on 1 December, video and PDF on 2–3 December, and submit on 4 December. No new features after 30 November.

## 9. Current state

- **On `main` (`132f9b6`):**
  - Docs & design: UML/Architecture diagrams (`.puml` + `.png`) in `docs/design/`, `docs/requirements/SRS.md`, `docs/testing/test-plan.md`, `docs/design/ui-spec.md`, and Figma design (`https://www.figma.com/design/P2iXhyeWO3VTTAM2oZlScS`).
  - Increment 1 (`Quorum.Data`, `Quorum.Logic`, `Quorum.Tests`): Models (`User`, `Student`, `Course`, `Registration`, `AcademicPolicy`, `UserRole`), `IDataStore`, `JsonDataStore` (`users.json`, `courses.json`, `registrations.json`, `policy.json` in `%LocalAppData%\Quorum`), `OperationResult`, `PasswordHasher`, `AuthService`, and 11 passing xUnit tests (`tests/Quorum.Tests/UnitTest1.cs` + `InMemoryDataStore`).
  - WPF UI (`Quorum.UI`, merged via PR #1): Single-window Fluent shell (`MainWindow`, `1280x720`, `MinWidth 800`, `MinHeight 600`), Light/Dark theme switching, `< 960 px` responsive layout (`AuthView`, `StudentView`, `TeacherView`, `AdminView`), `AuthView` live-wired to `AuthService`, and `IQuorumGateway` holding preview stubs for Increment 2 services.
  - CI/CD: `.github/workflows/ci.yml` (`.NET 10` Release build & test on `windows-latest`) and `.github/workflows/release.yml` (`v*` tag self-contained `win-x64` publish + GitHub Release).
- **Remaining:** Implement Increment 2 (`CourseService`, `RegistrationService`, `PolicyService`, `CourseStatus`) in `Quorum.Logic`, wire `QuorumGateway` to live Logic services, remove template `Class1.cs` files, expand tests, collect matriculation numbers, and record demo video + final PDF report.
- Do not report an item as completed unless there is evidence.

## 10. Session log (append only)

- **2026-11-20:** Chose the name Quorum. Agreed WPF, local JSON by default, Supabase optional behind IDataStore, student and teacher scope, three-layer architecture, draft data contract and incremental schedule.
- **2026-11-20:** Consolidated shared project context as the governing source of truth. Confirmed that earlier console, administrator and SQLite proposals are superseded.
- **2026-11-20:** Updated AI helper roles: Gemini also owns UI design; ChatGPT also implements UI designs in C# (WPF) and serves as backup C# coder when Claude hits limits; Claude remains lead for logic orchestration, architecture, debugging, and review.
- **2026-11-20**: Expanded Quorum's user model to three roles:
  Student, Teacher and Administrator. Administrators have
  full configuration authority over academic policies,
  credit limits, individual course credit values,
  registration periods and system settings. Confirmed
  external APIs and event-driven integrations as future
  possibilities outside the current MVP.
- **2026-11-20:** Synced Sections 4, 5, and 6 of `QUORUM-CONTEXT.md` with the Administrator role, academic policy/credit-limit rules, and `PROJECT-REPORT.md`.
- **2026-11-21:** Scaffolded and pushed the solution (.NET 10, WPF UI, xUnit tests, UI -> Logic -> Data references). WPF confirmed over Avalonia and WinForms. Removed duplicate docs/CONTEXT.md; this file is the single source of truth.
- **2026-11-21:** Administrator role retained in the MVP. AcademicPolicy is limited to MinCredits, MaxCredits, RegistrationStart and RegistrationEnd. Only administrators can change course credits; teachers cannot set or edit them, and new courses start with a default of 3 credits. Admins browse courses, edit credit values, view enrolments across all courses and configure the AcademicPolicy. Registration is blocked outside the registration period or when MaxCredits would be exceeded; MinCredits is advisory (shown as a warning, not blocking). Increment 2 covers the credit and period rules; Increment 3 covers the admin screens.
- **2026-11-21:** Logic-layer contract locked: methods that can fail by a business rule return OperationResult (IsSuccess, Message). Admin-only and teacher-only operations take an acting User and are enforced in the Logic layer (CreateCourse: Teacher; UpdateCourseCredits and SavePolicy: Administrator; GetCourseEnrolments: the course's teacher or an Administrator). New courses default to 3 credits.
- **2026-11-21:** Validation rules: credits must be a whole number from 1 to 6 and the course must exist; AcademicPolicy requires MinCredits >= 0, MaxCredits >= MinCredits, and RegistrationEnd after RegistrationStart. Policy and credit changes apply to future registrations only; existing registrations are not re-checked.
- **2026-11-21:** All UML diagrams accepted and stored in docs/design (use case, class A and B, three sequence diagrams, activity, architecture). Class diagram B method names are the locked Logic-layer contract. Next: Increment 1 code.

- **2026-11-21:** Drafted `docs/requirements/SRS.md` and `docs/testing/test-plan.md` from Section 4 of this context and the accepted PlantUML diagrams in `docs/design`. Recorded the functional scope, service validation branches, credits 1–6, policy bounds, actor authorisation, duplicate-username and wrong-password cases, password hashing and JSON persistence. Test-plan Actual Result and Pass/Fail columns are intentionally blank pending real execution.
- **2026-11-25:** Implemented Increment 1 (`UserRole`, models, `IDataStore`, `JsonDataStore`, `OperationResult`, `PasswordHasher`, `AuthService`, and 11 unit tests). Updated Figma UI and `docs/design/ui-spec.md` to a single `MainWindow` (`1280x720`, `< 960 px` responsive compact mode) with swappable views and Light/Dark Fluent tokens. Built `Quorum.UI` on `feat/wpf-ui` (`AuthView` live-wired; `IQuorumGateway` stubs for Increment 2).
- **2026-11-26:** Added `.github/workflows/ci.yml` (build & test on `windows-latest`) and `.github/workflows/release.yml` (tag `v*` self-contained `win-x64` GitHub Release). Merged `feat/wpf-ui` into `main` via PR #1 and updated `QUORUM-CONTEXT.md`.
