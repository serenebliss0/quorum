# Quorum Test Plan

**Project:** Quorum Course Registration System  
**Course:** SEN201 Software Engineering  
**Group:** 8

## 1. Purpose

This test plan defines the verification activities for the Quorum MVP. It is based on the approved project context and the use-case, sequence, activity and class diagrams in `docs/design`.

The plan covers unit, integration, system and acceptance testing. Test cases are derived from the specified user operations, every alternative branch in the registration, teacher-course and administrator-policy sequence diagrams, registration activity diagram, validation rules and JSON persistence requirements.

**Execution record rule:** The **Actual Result** and **Pass/Fail** cells are intentionally blank. They must be completed only after the corresponding test has actually been executed.

## 2. Scope

Testing covers:

- Student and Teacher account registration and login.
- Course browsing and course creation.
- Student course registration and all rejection branches.
- Registered-course viewing and course dropping.
- Course status derivation.
- Teacher ownership and Administrator authorisation.
- Administrator course-credit editing and AcademicPolicy saving.
- Validation of credit values and policy bounds.
- JSON data persistence.
- Password hashing and password verification.

The plan does not test Supabase, external integrations, email verification, password reset or features outside the approved MVP.

## 3. Test Levels

| Level | Purpose | Main focus |
|---|---|---|
| Unit | Verify individual Logic operations in isolation, using controlled data-store test doubles where appropriate. | Registration validation branches, credit and policy validation, course status rules, password verification and role checks. |
| Integration | Verify collaboration between Logic services and IDataStore/JsonDataStore. | Loading and saving users, courses, registrations and AcademicPolicy; service-to-store flows. |
| System | Verify complete user workflows through the WPF application and local JSON storage. | Student registration, rejected registration feedback, Teacher enrolment viewing and Administrator configuration. |
| Acceptance | Confirm that the implemented MVP supports the agreed user capabilities and business rules. | Student, Teacher and Administrator workflows against the approved context and use-case diagram. |

## 4. Test Environment and Data

- Windows environment compatible with the project target.
- .NET 10 SDK/runtime as required by the solution.
- Quorum solution and Quorum.Tests project.
- Local JSON data store using an isolated test data directory.
- A known test Student, Teacher and provisioned Administrator.
- Test courses with known Credits, Capacity, MinToRun and TeacherId values.
- A known AcademicPolicy with valid and invalid test variants.

Unless a case states otherwise, tests must use isolated test data and restore the baseline data between cases.

## 5. Test Cases

| ID | Level | Requirement | Input | Expected Result | Actual Result | Pass/Fail |
|---|---|---|---|---|---|---|
| TC-UNIT-001 | Unit | FR-05, FR-08, FR-09 | Registration attempt while current time is before RegistrationStart or after RegistrationEnd. | OperationResult is unsuccessful; registration-period-closed message is returned; no registration is added. | | |
| TC-UNIT-002 | Unit | FR-06 | Open registration period; Student already has a registration for the selected course; submit the same course again. | OperationResult is unsuccessful; duplicate message is returned; no additional registration is created. | | |
| TC-UNIT-003 | Unit | FR-07 | Open period; no duplicate; course enrolment count equals Capacity. | OperationResult is unsuccessful; course-full message is returned; no registration is added. | | |
| TC-UNIT-004 | Unit | FR-07 | Open period; no duplicate; course enrolment count is greater than Capacity. | OperationResult is unsuccessful; course-full message is returned; no registration is added. | | |
| TC-UNIT-005 | Unit | FR-09 | Open period; no duplicate; seat available; current Student credits + selected course Credits is greater than MaxCredits. | OperationResult is unsuccessful; MaxCredits-exceeded message is returned; no registration is added. | | |
| TC-UNIT-006 | Unit | FR-05, FR-10 | Open period; no duplicate; seat available; resulting credits equal MaxCredits. | Registration succeeds and is added once; no MaxCredits rejection occurs. | | |
| TC-UNIT-007 | Unit | FR-05, FR-10 | Open period; no duplicate; seat available; resulting credits are below MaxCredits but below MinCredits. | Registration succeeds; IsBelowMinCredits is true; minimum-credit warning is advisory and does not block registration. | | |
| TC-UNIT-008 | Unit | FR-05 | Open period; no duplicate; seat available; resulting credits are within policy bounds. | A Registration containing the StudentId, CourseCode and current RegisteredAt is created; OperationResult is successful. | | |
| TC-UNIT-009 | Unit | FR-20 | Enrolment count is below MinToRun and below Capacity. | GetCourseStatus returns Open. | | |
| TC-UNIT-010 | Unit | FR-20 | Enrolment count is equal to MinToRun and below Capacity. | GetCourseStatus returns Running. | | |
| TC-UNIT-011 | Unit | FR-20 | Enrolment count is equal to Capacity. | GetCourseStatus returns Full, taking precedence over Running. | | |
| TC-UNIT-012 | Unit | FR-20 | Enrolment count is at or above Capacity and also at or above MinToRun. | GetCourseStatus returns Full. | | |
| TC-UNIT-013 | Unit | FR-13, FR-14 | Teacher actor; unique code; non-empty title; valid Capacity and MinToRun. | Course creation succeeds; TeacherId equals actor.Id; Credits defaults to 3. | | |
| TC-UNIT-014 | Unit | FR-13 | Administrator actor passed to CreateCourse. | OperationResult is unsuccessful because CreateCourse is Teacher-only. | | |
| TC-UNIT-015 | Unit | FR-13 | Teacher actor; duplicate course code. | OperationResult is unsuccessful; no duplicate course is saved. | | |
| TC-UNIT-016 | Unit | FR-13 | Teacher actor; invalid course details, including missing code/title or invalid capacity/MinToRun relationship. | OperationResult is unsuccessful; no course is saved. | | |
| TC-UNIT-017 | Unit | FR-17 | Administrator actor; existing course; credits = 1. | Credit update succeeds and the course credit value becomes 1. | | |
| TC-UNIT-018 | Unit | FR-17 | Administrator actor; existing course; credits = 6. | Credit update succeeds and the course credit value becomes 6. | | |
| TC-UNIT-019 | Unit | FR-17 | Administrator actor; existing course; credits = 0. | OperationResult is unsuccessful; course credits remain unchanged. | | |
| TC-UNIT-020 | Unit | FR-17 | Administrator actor; existing course; credits = 7. | OperationResult is unsuccessful; course credits remain unchanged. | | |
| TC-UNIT-021 | Unit | FR-17 | Administrator actor; existing course; credits supplied as a fractional number. | OperationResult is unsuccessful; fractional credit value is not accepted; course remains unchanged. | | |
| TC-UNIT-022 | Unit | FR-17 | Administrator actor; unknown course code; credits = 3. | OperationResult is unsuccessful because the course does not exist; no course is created or modified. | | |
| TC-UNIT-023 | Unit | FR-17 | Teacher actor attempts UpdateCourseCredits for an existing course with credits = 4. | OperationResult is unsuccessful because the actor is not an Administrator; course credits remain unchanged. | | |
| TC-UNIT-024 | Unit | FR-19 | Administrator actor; MinCredits = 0; MaxCredits = 0; valid Start and End with End after Start. | Policy validation succeeds. | | |
| TC-UNIT-025 | Unit | FR-19 | Administrator actor; MinCredits = -1; MaxCredits = 10; valid date order. | Policy save is rejected because MinCredits is negative. | | |
| TC-UNIT-026 | Unit | FR-19 | Administrator actor; MinCredits = 10; MaxCredits = 9; valid date order. | Policy save is rejected because MaxCredits is less than MinCredits. | | |
| TC-UNIT-027 | Unit | FR-19 | Administrator actor; MinCredits = 5; MaxCredits = 10; RegistrationEnd equals RegistrationStart. | Policy save is rejected because RegistrationEnd is not after RegistrationStart. | | |
| TC-UNIT-028 | Unit | FR-19 | Administrator actor; MinCredits = 5; MaxCredits = 10; RegistrationEnd is before RegistrationStart. | Policy save is rejected because RegistrationEnd is not after RegistrationStart. | | |
| TC-UNIT-029 | Unit | FR-19 | Teacher or Student actor submits an otherwise valid AcademicPolicy to SavePolicy. | OperationResult is unsuccessful because the actor is not an Administrator; policy is not changed. | | |
| TC-UNIT-030 | Unit | FR-15, FR-18 | Teacher actor requests enrolments for a course whose TeacherId equals actor.Id. | Service returns the Students registered in that course. | | |
| TC-UNIT-031 | Unit | FR-15, FR-18 | Teacher actor requests enrolments for a course owned by another Teacher. | Service returns an empty student list; no other Teacher's enrolment data is disclosed. | | |
| TC-UNIT-032 | Unit | FR-15, FR-18 | Administrator actor requests enrolments for an existing course. | Service returns the Students registered in that course. | | |
| TC-UNIT-033 | Unit | FR-15 | Teacher or Student actor who does not own the course requests its enrolments. | Service returns an empty student list. | | |
| TC-UNIT-034 | Unit | FR-15, FR-18 | Teacher or Administrator requests enrolments for a course code that does not exist. | Service returns an empty student list. | | |
| TC-UNIT-035 | Unit | FR-01, FR-02 | Register a Student or Teacher using a username already present in the user store. | Account creation fails with an appropriate result; existing user data is not overwritten and no duplicate username is saved. | | |
| TC-UNIT-036 | Unit | FR-03 | Login using a known username and an incorrect password. | Authentication fails; no authenticated User is returned. | | |
| TC-UNIT-037 | Unit | FR-03 | Login using a known username and its correct password. | Authentication succeeds and returns the matching User with the correct role. | | |
| TC-UNIT-038 | Unit | FR-03, NFR-SEC-01, NFR-SEC-02 | Register a user with a password; inspect the created user record. | PasswordHash and Salt are stored; plaintext password is not stored. | | |
| TC-INT-001 | Integration | FR-01, FR-02, NFR-REL-01 | Register Student and Teacher accounts through AuthService using JsonDataStore; reload users. | Both user records are present after reload with their correct roles and stored password hash/salt fields. | | |
| TC-INT-002 | Integration | FR-13, FR-14, NFR-REL-01 | Create a course through CourseService using JsonDataStore; reload courses. | Course persists with supplied details, acting TeacherId and default Credits = 3. | | |
| TC-INT-003 | Integration | FR-05, FR-06, NFR-REL-01 | Successfully register a Student using RegistrationService and JsonDataStore; reload registrations. | The new registration is present exactly once with the expected StudentId, CourseCode and RegisteredAt. | | |
| TC-INT-004 | Integration | FR-19, NFR-REL-01 | Save a valid AcademicPolicy through PolicyService using JsonDataStore; reload policy. | Saved MinCredits, MaxCredits, RegistrationStart and RegistrationEnd are restored unchanged. | | |
| TC-INT-005 | Integration | FR-17, NFR-REL-01 | Update an existing course's credits through CourseService using JsonDataStore; reload courses. | Updated credit value persists for the correct course. | | |
| TC-INT-006 | Integration | FR-11, FR-15, FR-18 | Persist users, courses and registrations; invoke student-registration and course-enrolment retrieval operations. | Services load the persisted records and return the correct registrations or enrolled Students. | | |
| TC-INT-007 | Integration | NFR-REL-02 | Attempt a duplicate, closed-period, full-course or MaxCredits-rejected registration using JsonDataStore. Reload registrations after each attempt. | Rejected operation creates no registration record; persisted registration data is unchanged. | | |
| TC-INT-008 | Integration | FR-17, FR-19 | Save valid course-credit and policy changes; reload data and evaluate a later registration using the updated values. | Updated settings persist and are used for future registration validation; existing registration records are not re-checked or rewritten. | | |
| TC-SYS-001 | System | FR-01, FR-03 | Through WPF, create a Student account and log in with the same correct credentials. | Account is created, login succeeds and Student functionality is available. | | |
| TC-SYS-002 | System | FR-02, FR-03 | Through WPF, create a Teacher account and log in with the same correct credentials. | Account is created, login succeeds and Teacher functionality is available. | | |
| TC-SYS-003 | System | FR-03 | Log in with an incorrect password. | Login is rejected and protected functionality is not opened. | | |
| TC-SYS-004 | System | FR-04 | Log in as each role and browse courses. | Course browsing is available to Student, Teacher and Administrator. | | |
| TC-SYS-005 | System | FR-05, FR-10, FR-20 | Student registers for a valid course while registration is open, a seat is available and MaxCredits is not exceeded; set resulting credits below MinCredits. | Registration is saved; confirmation is shown; a MinCredits advisory warning appears; derived course status is displayed. | | |
| TC-SYS-006 | System | FR-08 | Student attempts registration while the configured registration period is closed. | Registration is blocked and a period-closed message is shown. | | |
| TC-SYS-007 | System | FR-06 | Student attempts to register for a course already in their registrations. | Registration is blocked and a duplicate message is shown. | | |
| TC-SYS-008 | System | FR-07 | Student attempts registration when course enrolment equals Capacity. | Registration is blocked and a course-full message is shown. | | |
| TC-SYS-009 | System | FR-09 | Student attempts registration that would make total credits exceed MaxCredits. | Registration is blocked and a MaxCredits-exceeded message is shown. | | |
| TC-SYS-010 | System | FR-11, FR-12 | Student views registered courses, then drops one registered course. | Only the Student's registrations are shown; the selected course is dropped and the displayed registration list is updated. | | |
| TC-SYS-011 | System | FR-13, FR-14 | Teacher creates a course with unique code, title, capacity and MinToRun. | Course is saved with the Teacher as owner and Credits = 3; updated course list and status are displayed. | | |
| TC-SYS-012 | System | FR-13 | Attempt course creation through the Teacher course-creation operation using an Administrator actor. | Operation is rejected as an unauthorised actor; no course is saved. | | |
| TC-SYS-013 | System | FR-15 | Teacher selects a course they created and opens enrolments. | Enrolled Students for that course are displayed. | | |
| TC-SYS-014 | System | FR-15 | Teacher attempts to view enrolments for another Teacher's course. | No student list is disclosed; “No students to display” is shown. | | |
| TC-SYS-015 | System | FR-16, FR-17 | Administrator browses courses and changes the credit value of an existing course to a valid whole number from 1 to 6. | Course list refreshes and displays the updated credit value. | | |
| TC-SYS-016 | System | FR-17 | Non-Administrator attempts to change course credits. | Operation is rejected and course credit value remains unchanged. | | |
| TC-SYS-017 | System | FR-18 | Administrator views enrolments for courses created by different Teachers. | Enrolment information across all courses is available. | | |
| TC-SYS-018 | System | FR-19 | Administrator saves a valid AcademicPolicy. | Policy is saved and a confirmation is displayed. | | |
| TC-SYS-019 | System | FR-19 | Administrator attempts to save a policy with invalid credit bounds or date order. | Policy is rejected, an error is displayed and the previous saved policy remains unchanged. | | |
| TC-SYS-020 | System | FR-20 | Change course enrolment through registration and dropping so counts cross MinToRun and Capacity boundaries. | Displayed status is recalculated dynamically using Full precedence, then Running, then Open. | | |
| TC-SYS-021 | System | NFR-REL-01 | Create or update a user, course, registration and policy; close and restart the application. | All successfully saved data remains available after restart. | | |
| TC-ACC-001 | Acceptance | FR-01, FR-03, FR-04, FR-05, FR-11, FR-12 | Student completes account creation, login, course browsing, valid registration, registration viewing and course dropping. | All Student capabilities defined in the context are available and operate according to the business rules. | | |
| TC-ACC-002 | Acceptance | FR-02, FR-03, FR-13, FR-14, FR-15 | Teacher completes account creation, login, course creation and viewing enrolments in an owned course. | All Teacher capabilities defined in the context are available; new course ownership and default credits are correct. | | |
| TC-ACC-003 | Acceptance | FR-03, FR-04, FR-16, FR-17, FR-18, FR-19 | Provisioned Administrator logs in, browses courses, edits course credits, views enrolments across courses and saves AcademicPolicy. | All Administrator capabilities defined in the context are available; only Administrator-authorised changes succeed. | | |
| TC-ACC-004 | Acceptance | FR-05 to FR-10, FR-20 | Execute the registration success path and each registration rejection branch, then verify the MinCredits advisory and course status. | All registration outcomes match the sequence/activity diagrams and approved business rules; rejected attempts do not persist. | | |

## 6. Execution and Reporting

1. Execute unit tests before integration tests.
2. Execute integration tests against an isolated JSON data directory.
3. Execute system tests through the WPF application.
4. Execute acceptance tests against the Student, Teacher and Administrator capabilities in the shared context.
5. Record the actual observed result in **Actual Result** only after execution.
6. Mark **Pass** only when the observed result matches the expected result; otherwise mark **Fail** and record the defect separately.
7. Do not pre-populate test outcomes or infer success from code inspection.

## 7. Traceability Sources

- `docs/QUORUM-CONTEXT.md` — Section 4 scope, business rules, architecture, data contract and approved validation decisions.
- `docs/design/uc-use-case.puml` — use-case actors and use cases.
- `docs/design/seq-student-registration.puml` — registration branches and advisory MinCredits behaviour.
- `docs/design/act-registration-validation.puml` — registration activity branches.
- `docs/design/seq-teacher-course.puml` — Teacher course creation defaults and enrolment authorisation.
- `docs/design/seq-admin-credits-policy.puml` — Administrator authorisation, credit range and AcademicPolicy validation.
- `docs/design/class-a-data-model.puml` — data fields and IDataStore contract.
- `docs/design/class-b-services.puml` — Logic service contracts and actor-based authorisation.
