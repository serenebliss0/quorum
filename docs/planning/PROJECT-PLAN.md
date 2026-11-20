# Quorum Project Plan

**Course:** SEN201  
**Project:** Group 8 — Course Registration System  
**Deadline:** 4 December 2026  
**Planning baseline date:** 20 November 2026

## 1. Problem and purpose

Quorum is a small course-registration management application. It demonstrates how a student can select courses from a catalogue, submit a registration, receive validation feedback and review or amend the resulting registration.

The system is an academic prototype. It is not intended to replace a university's actual student information system.

## 2. Scope

### In scope for the MVP
- Student records.
- Course catalogue.
- A registration associated with a student and academic term.
- Adding and removing courses from a registration.
- Duplicate-registration prevention.
- Validation of configurable course-selection rules.
- Calculation and display of total registered credit units.
- Persistent local storage.
- Clear success, validation and error feedback.

### Out of scope unless time and requirements permit
- Authentication infrastructure and password recovery.
- Payment processing.
- Live integration with a university database or portal.
- Timetable clash detection unless timetable data is available.
- Lecturer approval workflows.
- Multi-campus administration.
- Production deployment, cloud hosting and concurrent multi-user access.

## 3. Stakeholders and actors

- **Student:** browses the catalogue, registers for courses, reviews and amends their selection.
- **Registration administrator/operator:** maintains student/course records and may inspect registrations in the prototype.
- **Project team:** analyses, designs, implements and tests the application.
- **Lecturer/assessor:** evaluates lifecycle evidence and the working application.

Actor permissions are provisional and must be agreed in the SRS.

## 4. Proposed functional requirements

| ID | Requirement |
|---|---|
| FR-01 | The system shall create and display student records. |
| FR-02 | The system shall create, view and update course records. |
| FR-03 | The system shall display courses available for a selected term. |
| FR-04 | The system shall create a registration for a student and term. |
| FR-05 | The system shall add a course to a registration when validation succeeds. |
| FR-06 | The system shall prevent the same course from being registered twice by the same student in the same term. |
| FR-07 | The system shall calculate and display the registration's total credit units. |
| FR-08 | The system shall allow a registered course to be removed, subject to configured rules. |
| FR-09 | The system shall display a student's registered courses. |
| FR-10 | The system shall preserve records after the application is closed and reopened. |
| FR-11 | The system shall explain why a registration action is rejected. |

## 5. Proposed non-functional requirements

| ID | Requirement |
|---|---|
| NFR-01 Usability | Menus, prompts and validation messages shall be clear and consistent. |
| NFR-02 Reliability | Invalid input shall not corrupt or partially overwrite stored records. |
| NFR-03 Maintainability | Presentation, business rules, domain entities and persistence shall be separated. |
| NFR-04 Testability | Core registration rules shall be testable independently of the user interface. |
| NFR-05 Data integrity | Student, course and registration identifiers shall be validated before records are saved. |
| NFR-06 Portability | The application shall run on the agreed .NET runtime without machine-specific absolute paths. |

## 6. Business rules and assumptions to validate

These are **not confirmed institutional policies**. They must be agreed or replaced with verified values:
- Whether registrations are limited by academic level, programme or department.
- Minimum and maximum credit units.
- Whether prerequisites are enforced, and where prerequisite data comes from.
- Whether students may register for courses outside their level/programme.
- Whether course capacity is enforced.
- Whether add/drop deadlines or approval states are modelled.
- Whether course codes are unique globally or within a term.

For a working prototype, use a small documented sample dataset and configurable validation values. Never label assumptions as official university rules.

## 7. Feasibility

### Technical
Feasible as a C# local prototype using the .NET SDK, a local JSON data store and automated tests. No external university APIs are required.

### Operational
The workflow is familiar and can be demonstrated with sample students and courses. An operator can maintain the catalogue and records.

### Schedule
The time available is short (20 November to 4 December). Freeze the MVP early, implement essential workflows, and reserve the final days for integration, testing, report completion and demonstration.

## 8. Proposed architecture

Use a layered architecture:

- **Presentation:** console menus and input/output.
- **Application:** use cases such as RegisterForCourse, DropCourse and ViewRegistration.
- **Domain:** Student, Course, Registration and registration rules.
- **Infrastructure:** JSON repositories and file persistence.

Dependencies should point inward. The domain layer should not depend on console input or JSON file details. Persistence should be accessed through repository interfaces.

## 9. Proposed data model

- **Student:** StudentId, FullName, Programme, Level.
- **Course:** CourseCode, Title, CreditUnits, Term, optional Capacity, optional PrerequisiteCodes.
- **Registration:** RegistrationId, StudentId, Term, Status, RegisteredCourseCodes.
- **RegistrationItem:** RegistrationId, CourseCode, DateAdded.

Use stable identifiers and validate references before saving. The final model may change after the SRS and class diagram are reviewed.

## 10. Risks

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Scope expands beyond available time | High | High | Freeze MVP; defer optional features. |
| Team members contribute unevenly | High | High | Assign named, reviewable deliverables and track contributions. |
| Requirements or institutional rules are unclear | Medium | High | Record assumptions and obtain lecturer/team confirmation. |
| Integration conflicts | Medium | Medium | Agree interfaces early; use small branches and frequent integration. |
| Data persistence defects | Medium | High | Test save/load and malformed/missing files. |
| Diagrams and report are left until the end | High | High | Create lifecycle evidence alongside each phase. |
| Team cannot explain AI-assisted code | Medium | High | Require a walkthrough and review for every submitted module. |

## 11. Delivery schedule

| Date | Target |
|---|---|
| 20 Nov | Confirm scope, assumptions, team roles, workflow and toolchain. |
| 21 Nov | Feasibility, project plan, stakeholder/actor analysis and SRS draft. |
| 22 Nov | Validate requirements; use-case and activity diagrams. |
| 23 Nov | Domain model, class diagram, sequence diagrams and architecture. |
| 24 Nov | Solution scaffold, domain entities, repository interfaces and sample data. |
| 25 Nov | Course catalogue and student-record workflows. |
| 26 Nov | Registration workflow and duplicate/credit validation. |
| 27 Nov | Drop/view workflows and persistence. |
| 28 Nov | Unit tests and integration tests; fix defects. |
| 29 Nov | System/acceptance scenarios; usability polish. |
| 30 Nov | Feature freeze; full regression and report consolidation. |
| 1 Dec | Screenshots, demonstration script and video recording. |
| 2 Dec | Final report review, traceability and clean-build verification. |
| 3 Dec | Buffer day; final fixes and submission checks. |
| 4 Dec | Submission deadline. |

## 12. Definition of done

The project is ready when:
- each in-scope requirement maps to an implemented feature and test;
- the application builds and runs from a clean checkout;
- valid and invalid registration scenarios have been demonstrated;
- unit, integration, system and acceptance evidence is recorded;
- required UML and architecture diagrams are included;
- the report documents the actual process and team contributions;
- screenshots and a working demonstration link are included; and
- every team member can explain their contribution and the overall workflow.

## 13. Decisions still required

1. Confirm whether the lecturer requires Console, WinForms, WPF or ASP.NET.
2. Confirm whether the system should model only student-side registration or also administrator catalogue management.
3. Confirm authoritative registration rules/data source, or approve clearly labelled prototype assumptions.
4. Confirm team member names, GitHub usernames and responsibilities.
5. Confirm whether any additional submission requirements were given verbally.
