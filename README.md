# Quorum

A C# course registration management system developed for the SEN201 Software Engineering group project.

> **Project status:** Planning  
> **Submission deadline:** 4 December 2026  
> **Group:** 8

## Purpose

Quorum demonstrates a structured course-registration workflow: students browse an available course catalogue, select courses for an academic term, validate selections against registration rules, and review their registered courses.

The project is an academic prototype, not a production university information system.

## Proposed MVP

- Maintain student records (create, view, update).
- Maintain a course catalogue with course code, title, credit units, semester and capacity.
- Register a student for one or more courses in a defined semester.
- Prevent duplicate registration for the same course and semester.
- Validate course selection against configurable rules (for example, maximum credit load and prerequisites, where supplied).
- View and remove a student's registered courses.
- Persist data between application runs.
- Provide clear validation messages and an operator-friendly interface.

**Policy note:** Registration rules such as credit limits, prerequisites, level restrictions and add/drop deadlines must be confirmed by the group/lecturer or explicitly labelled as prototype assumptions. Do not present invented rules as official PAU policy.

## Proposed technical direction

- **Language:** C#
- **Application:** Console application for the first deliverable, unless the lecturer has mandated a graphical interface.
- **Architecture:** Layered design separating Presentation, Application/Business Logic, Domain and Persistence.
- **Persistence:** JSON files for a simple, inspectable MVP; keep persistence behind an interface so it can be replaced.
- **Testing:** Automated unit tests for domain and validation logic, plus integration and end-to-end scenario tests.
- **Version control:** Git and GitHub; feature branches and pull requests where practical.

These are proposals pending confirmation against the lecturer's expectations and the team's available environment.

## Engineering lifecycle

1. Planning and feasibility
2. Requirements engineering and SRS
3. System modelling (use case, class, sequence and activity diagrams; data flow where useful)
4. Architecture and detailed design
5. Implementation
6. Verification and validation (unit, integration, system and acceptance)
7. Report, screenshots and demonstration

## Repository map

```text
docs/
  planning/
  requirements/
  design/
  testing/
  report/
src/
  Quorum.Domain/
  Quorum.Application/
  Quorum.Infrastructure/
  Quorum.Console/
tests/
  Quorum.UnitTests/
  Quorum.IntegrationTests/
```

The structure may be adjusted once the framework and UI are confirmed.

## Team and contribution record

See `docs/planning/team-roles.md`. Each member should make and explain genuine contributions. AI tools may assist with drafting, implementation and review, but the team remains responsible for understanding, validating and defending all submitted work.

## Definition of done

A feature is done when its requirement and expected behaviour are documented, implementation follows the agreed design, relevant tests pass, invalid input and failure cases are handled, changes are committed meaningfully, and the team can explain and demonstrate it.

## Current status

The repository has been created. Scope, policy assumptions, application type and team roles are being confirmed before implementation begins.
