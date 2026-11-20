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

### Business rules
- A student cannot register for the same course twice.
- A student cannot register for a course when enrolled count is greater than or equal to capacity.
- A course is **Running** once enrolled count is greater than or equal to its minimum-to-run value.
- Course statuses are **Open**, **Running**, and **Full**. Full takes precedence when enrolled count reaches capacity; otherwise Running applies when the minimum-to-run threshold has been met; otherwise the status is Open.

### Stretch features (do not promise these in the SRS)
- Teachers viewing other teachers in a course.
- Supabase storage.

Do not add features beyond this scope without an explicit team decision.

## 5. Architecture

Three layers, with clear separation of concerns:

- **Quorum.UI** (WPF): windows and views only; no business rules.
- **Quorum.Logic**: registration rules, account handling and quorum status.
- **Quorum.Data**: data models, the IDataStore interface and JsonDataStore implementation.
- **Quorum.Tests**: unit tests for the Logic layer.

The test project is separate from the three application layers. Supabase, if implemented, is a second IDataStore implementation. Other layers should not need to change.

### Accounts
- Username, salted password hash and role (Student or Teacher).
- No email verification and no password-reset flow.

### Security
- Never commit API keys or secrets. Keep them out of the repository.

## 6. Draft data contract

Update this contract when the team approves a change.

- **User:** Id, FullName, Username, PasswordHash, Salt, Role.
- **Student : User:** MatricNumber.
- **Course:** Code, Title, Credits, Capacity, MinToRun, TeacherId.
- **Registration:** StudentId, CourseCode, RegisteredAt.
- **IDataStore:** load and save users, courses and registrations.

Keep the model simple and consistent with the three-layer structure. Do not silently add fields or entities.

## 7. Rules for AI helpers

- Follow this file. Do not change a decision unless told to; log approved changes.
- Artifact ownership:
  - **Claude:** architecture, core C# logic, debugging and review.
  - **ChatGPT:** report prose, SRS wording and test tables.
  - **Gemini:** diagrams as Mermaid or PlantUML text.
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

- Repository exists; project name, WPF, C#, local JSON default, optional Supabase, scope and three-layer design are agreed.
- Next steps: add this file to docs, each member makes a genuine first commit, create GitHub Issues from the incremental plan, and draft the SRS.
- Matriculation numbers remain to be collected.
- Do not report an item as completed unless there is evidence.

## 10. Session log (append only)

- **2026-11-20:** Chose the name Quorum. Agreed WPF, local JSON by default, Supabase optional behind IDataStore, student and teacher scope, three-layer architecture, draft data contract and incremental schedule.
- **2026-11-20:** Consolidated shared project context as the governing source of truth. Confirmed that earlier console, administrator and SQLite proposals are superseded.