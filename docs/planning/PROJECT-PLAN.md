# Quorum Project Plan

**Course:** SEN201  
**Group:** 8  
**Deadline:** 4 December 2026  
**Baseline:** 20 November 2026  
**Governing specification:** [Shared project context](../QUORUM-CONTEXT.md)

## 1. Purpose

Quorum is a Windows desktop course registration prototype built with C# and WPF. Students can register for courses. Teachers can create courses and inspect the students enrolled in their courses. The quorum rule determines whether each course is Open, Running or Full.

## 2. Scope

### Student workflows
- Create an account and log in.
- Browse courses.
- Register for a course.
- View registered courses.
- Drop a course.

### Teacher workflows
- Create an account and log in.
- Create a course with capacity and minimum-to-run values.
- View the students enrolled in courses created by that teacher.

### Explicitly excluded from the promised SRS scope
- Teachers viewing other teachers in a course.
- Supabase storage (optional stretch only).
- Email verification and password reset.
- Payment, timetable management, university portal integration and approval workflows.

Do not add extra features without an explicit team decision.

## 3. Business rules

1. A student cannot register for the same course twice.
2. Registration is rejected when enrolled count is greater than or equal to course capacity.
3. A course is **Full** when enrolled count is greater than or equal to capacity.
4. Otherwise, a course is **Running** when enrolled count is greater than or equal to MinToRun.
5. Otherwise, the course is **Open**.

The order matters: Full takes precedence over Running when both thresholds are reached.

## 4. Architecture

The agreed design uses three layers:

- **Quorum.UI:** WPF windows/views only. It must not contain business rules.
- **Quorum.Logic:** account handling, registration validation and course quorum status.
- **Quorum.Data:** data models, IDataStore contract and JsonDataStore implementation.
- **Quorum.Tests:** automated unit tests for the Logic layer.

The test project is separate from the three application layers. The optional Supabase implementation would implement the same IDataStore contract; the other layers should not need to change.

## 5. Draft data contract

| Type | Fields |
|---|---|
| User | Id, FullName, Username, PasswordHash, Salt, Role |
| Student : User | MatricNumber |
| Course | Code, Title, Credits, Capacity, MinToRun, TeacherId |
| Registration | StudentId, CourseCode, RegisteredAt |

IDataStore loads and saves users, courses and registrations. Treat this as the current draft contract; changes require team approval and an update to the shared context.

## 6. Accounts and security

- Accounts use username, salted password hash and role (Student or Teacher).
- No email verification or password-reset flow is in scope.
- Never commit API keys, credentials or other secrets.
- Passwords must not be stored as plaintext. Use an established .NET password-hashing approach rather than inventing a cryptographic scheme.

## 7. Feasibility

### Technical
The project is feasible as a local Windows WPF application using C#, JSON persistence and a test project. The UI is separated from business rules, allowing the Logic layer to be tested independently.

### Operational
The student and teacher workflows are small and demonstrable using sample accounts and course records. The prototype does not claim integration with PAU's live student information systems.

### Schedule
The delivery window is short. Prioritise the three increments, integrate frequently, and freeze features after 30 November. Supabase is strictly optional.

## 8. Risks

| Risk | Impact | Mitigation |
|---|---|---|
| Scope creep | High | Follow the shared context; freeze features on 30 November. |
| JSON data loss or corruption | High | Validate before saving; test load/save and missing or malformed files. |
| Incorrect duplicate/full/quorum logic | High | Put rules in Logic and cover boundary cases with unit tests. |
| UI contains business rules | Medium | Keep WPF views thin and route actions through Logic. |
| Uneven team contributions | High | Use named owners, GitHub issues and genuine member commits. |
| Late integration | High | Agree interfaces early and integrate each increment. |
| Secrets accidentally committed | High | Keep credentials out of source and review commits. |
| Report/video delayed | High | Capture screenshots and testing evidence as features stabilise. |

## 9. Incremental schedule

| Date | Target |
|---|---|
| By 24 Nov | Increment 1: models, JSON storage, sign-up and login. |
| By 28 Nov | Increment 2: course creation, student registration, duplicate/full/quorum rules. |
| By 30 Nov | Increment 3: teacher enrolment view, validation, polish and unit tests. |
| 30 Nov | Feature freeze. No new features after this date. |
| 1 Dec | System and acceptance testing; record actual outcomes. |
| 2–3 Dec | Demo video, screenshots, final PDF report and clean-build checks. |
| 4 Dec | Submission deadline. |

Increment 4 is optional Supabase storage and must not jeopardise the core app.

## 10. Definition of done

- The application builds and runs from a clean checkout on Windows.
- Students can complete the agreed account and registration workflows.
- Teachers can create courses and view their own course enrolments.
- Duplicate and full-course registration attempts are rejected.
- Course status transitions correctly across Open, Running and Full boundaries.
- JSON data survives application restarts.
- Logic unit tests pass and actual test outcomes are recorded.
- Required diagrams, report, screenshots and demo link are included.
- Contributions are genuine and traceable.
