# quorum: shared project context

> Paste this whole file at the start of every AI chat (Claude, ChatGPT, Gemini).
> At the end of every chat, add a short entry to the SESSION LOG at the bottom.

## 1. Snapshot

- **Project:** quorum, a Course Registration System (Software Engineering Group Project, Group 8)
- **Language / UI:** C#, WPF (Windows only)
- **Storage:** local JSON file (default). Supabase is an optional extra, only if time allows.
- **Repo:** https://github.com/serenebliss0/quorum.git
- **Deadline:** December 4, 2026
- **Process model:** Incremental (3 increments, plus an optional 4th)
- **Goal:** keep it simple. The priority is a working, well-documented, compilable app, not extra features.

## 2. Team and roles (proposed, confirm with the group)

| Member | Role |
|---|---|
| AJAYI Oluwasemire (Serenity) | Project Manager + Lead C# Developer (core logic) |
| OKEKE Kenechukwu | Requirements Analyst + System Modeller (SRS, UML diagrams) |
| OKIKE Tochukwu | QA/Tester + UI Developer (WPF views, test plan and test cases) |

Matriculation numbers: TBD (needed for the report).

## 3. Assignment deliverables (single PDF report)

1. Project overview and feasibility (technical, operational, schedule) plus risk summary
2. SRS: functional and non-functional requirements plus short scenarios
3. Diagrams: use case, class, sequence, activity, system architecture
4. Testing log: unit, integration, system, acceptance (inputs, expected, actual, pass/fail)
5. GitHub repo link (well-commented code, commit history showing individual contributions)
6. Demo video link (all core features and edge-case validations)
7. Application screenshots
8. Group members, matric numbers, and roles

Rules: submit one PDF, links must be publicly viewable, all C# must compile and run without errors.

## 4. Scope

**Students** can: create an account, log in, browse courses, register for a course, view their registered courses, drop a course.

**Teachers** can: create an account, log in, create courses (with capacity and minimum to run), view the students enrolled in their courses.

**Business rules:**
- A student cannot register for the same course twice.
- A student cannot register for a course that is full (enrolled >= capacity).
- Quorum: a course is "Running" once enrolled >= minimum to run. Statuses: Open, Running, Full.

**Stretch (do NOT promise in the SRS):** teachers viewing other teachers in a course, Supabase storage.

## 5. Architecture

Three layers, separation of concerns:

- `Quorum.UI` (WPF): windows and views only, no business rules
- `Quorum.Logic`: registration rules, account handling, quorum status
- `Quorum.Data`: models, `IDataStore` interface, `JsonDataStore` implementation
- `Quorum.Tests`: unit tests for the Logic layer

Supabase, if added, is a second `IDataStore` implementation. Nothing else changes.

**Accounts:** username + hashed (salted) password + role (Student or Teacher). No email verification, no reset flow.

**Security:** never commit API keys or secrets. Keep them out of the repo.

## 6. Draft data contract (paste into code chats; update when it changes)

- `User`: Id, FullName, Username, PasswordHash, Salt, Role
- `Student : User`: MatricNumber
- `Course`: Code, Title, Credits, Capacity, MinToRun, TeacherId
- `Registration`: StudentId, CourseCode, RegisteredAt
- `IDataStore`: load and save users, courses, and registrations

## 7. Rules for AI helpers

- Follow this file. Do not change a decision unless told to; log changes in the session log.
- One owner per artifact. Claude: architecture, core C# logic, debugging and review. ChatGPT: report prose, SRS wording, test tables. Gemini: diagrams as Mermaid or PlantUML text.
- Do not rewrite files you do not own. Review and suggest instead.
- Keep code simple, commented, and compiling. Do not invent library methods; say when unsure.
- Do not add features beyond Section 4.

## 8. Plan

| Increment | Content | Target |
|---|---|---|
| 1 | Models, JSON storage, sign-up and login | by Nov 24 |
| 2 | Course creation, student registration, duplicate/full/quorum rules | by Nov 28 |
| 3 | Teacher view, validation, polish, unit tests | by Nov 30 |
| 4 (optional) | Supabase implementation | only if ahead |

Then: system and acceptance testing Dec 1, video and PDF Dec 2-3, submit Dec 4. No new features after Nov 30.

## 9. Current state

- Repo created, name locked, scope and stack decided.
- Next: add this file to `docs/`, everyone makes a first commit, create GitHub Issues from Section 8, draft SRS.

## 10. Session log (append only)

- 2026-11-20: Chose name quorum. Chose WPF, JSON default, Supabase optional behind `IDataStore`. Defined student and teacher scope. Created this file.
