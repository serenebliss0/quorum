# Quorum

A C# WPF course registration system developed for the SEN201 Software Engineering Group Project, Group 8.

**Deadline:** 4 December 2026  
**Platform:** Windows  
**Process model:** Incremental (three core increments, one optional)
<img width="1920" height="1200" alt="image" src="https://github.com/user-attachments/assets/c4824f21-f15d-4df8-95cd-b0ed16cdd80c" />


## Purpose

Quorum lets students register for courses and lets teachers create courses and view their enrolments. Its defining rule is the course quorum: a course becomes **Running** when its enrolment reaches the configured minimum-to-run value.

## Agreed scope

**Students** can create an account, log in, browse courses, register for a course, view registered courses and drop a course.

**Teachers** can create an account, log in, create courses with capacity and minimum-to-run values, and view students enrolled in their courses.

Core rules:
- A student cannot register for the same course twice.
- A student cannot register when the course is full (enrolled count >= capacity).
- Course statuses: `Open`, `Running`, `Full`.
- `Full` applies when capacity is reached; otherwise `Running` applies when the minimum-to-run threshold is reached; otherwise the status is `Open`.

Stretch features that are not promised in the SRS: teachers viewing other teachers in a course, and Supabase storage.

## Technical direction

- **Language/UI:** C# and WPF.
- **Storage:** local JSON by default.
- **Architecture:** `Quorum.UI` (WPF), `Quorum.Logic` (business rules), `Quorum.Data` (models and data storage).
- **Tests:** `Quorum.Tests`, focused on the Logic layer.
- **Optional extension:** Supabase as another `IDataStore` implementation, only if time allows.

## Incremental delivery

1. **By 24 November:** models, JSON storage, sign-up and login.
2. **By 28 November:** course creation, student registration, duplicate/full/quorum rules.
3. **By 30 November:** teacher enrolment view, validation, polish and unit tests.
4. **Optional:** Supabase, only if the core project is ahead of schedule.

System/acceptance testing: 1 December. Demo video and PDF report: 2–3 December. Submission: 4 December. No new features after 30 November.

## Repository structure

```text
docs/
  QUORUM-CONTEXT.md
  planning/
  requirements/
  design/
  testing/
src/
  Quorum.UI/
  Quorum.Logic/
  Quorum.Data/
tests/
  Quorum.Tests/
```

## Team and evidence

See [the shared project context](docs/QUORUM-CONTEXT.md) and [team roles](docs/planning/team-roles.md). Proposed roles must be confirmed by the group. Record genuine contributions with links to commits, issues, diagrams, tests and report sections. Each member must be able to explain the work they submit.

## Definition of done

The application must compile and run cleanly from a fresh checkout. Core student and teacher workflows must work, registration rules must be tested, JSON data must persist between runs, and the report/demo must reflect the actual implemented system.
