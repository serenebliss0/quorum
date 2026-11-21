# Quorum Software Requirements Specification

**Project:** Quorum Course Registration System  
**Course:** SEN201 Software Engineering  
**Group:** 8  
**Repository:** https://github.com/serenebliss0/quorum

## 1. Introduction

### 1.1 Purpose

This document specifies the functional and non-functional requirements of Quorum. It provides a basis for implementation, verification and acceptance testing.

### 1.2 Scope

Quorum is a Windows desktop course registration system. Its MVP supports Students, Teachers and provisioned Administrators. It supports account creation and login for Students and Teachers, course browsing, course registration and dropping, course creation, enrolment viewing, course-credit editing and academic-policy configuration.

The application uses local JSON storage by default. Supabase and external integrations are not part of the MVP.

### 1.3 User Classes

- **Student:** Creates an account, logs in, browses courses, registers for courses, views registrations and drops courses.
- **Teacher:** Creates an account, logs in, creates courses with capacity and minimum-to-run values, and views enrolments for their own courses.
- **Administrator:** Uses a provisioned account, logs in, browses courses, edits course credit values, views enrolments across all courses and configures academic policy.

## 2. Functional Requirements

The following requirements are derived from Section 4 of the shared context and the accepted use-case and sequence diagrams. “Must” indicates MVP functionality.

| ID | Actor | Functional requirement | Matching use case | Priority |
|---|---|---|---|---|
| FR-01 | Student | The system shall allow a Student to create an account. | Create Account | Must |
| FR-02 | Teacher | The system shall allow a Teacher to create an account. | Create Account | Must |
| FR-03 | Student, Teacher, Administrator | The system shall allow a registered Student or Teacher, or a provisioned Administrator, to log in. | Log In | Must |
| FR-04 | Student, Teacher, Administrator | The system shall allow authenticated users to browse courses. | Browse Courses | Must |
| FR-05 | Student | The system shall allow a Student to register for a course when registration is open, the Student is not already registered for the course, the course has capacity, and adding the course does not exceed MaxCredits. | Register for Course | Must |
| FR-06 | Student | The system shall prevent a Student from registering for the same course more than once. | Register for Course | Must |
| FR-07 | Student | The system shall prevent registration when the course enrolment count is greater than or equal to its capacity. | Register for Course | Must |
| FR-08 | Student | The system shall prevent registration when the registration period is not open. | Register for Course | Must |
| FR-09 | Student | The system shall prevent registration when the Student's current registered credits plus the selected course credits exceed AcademicPolicy.MaxCredits. | Register for Course | Must |
| FR-10 | Student | Following a successful registration, the system shall indicate when the Student's total registered credits are below AcademicPolicy.MinCredits. This minimum-credit condition is advisory and shall not block registration. | Register for Course | Must |
| FR-11 | Student | The system shall allow a Student to view their registered courses. | View Registered Courses | Must |
| FR-12 | Student | The system shall allow a Student to drop a course. | Drop Course | Must |
| FR-13 | Teacher | The system shall allow a Teacher to create a course by supplying its code, title, capacity and minimum-to-run value. | Create Course | Must |
| FR-14 | Teacher | The system shall assign the creating Teacher's user ID as the course's TeacherId and set a newly created course's credit value to 3. | Create Course | Must |
| FR-15 | Teacher | The system shall allow a Teacher to view the students enrolled in courses created by that Teacher. | View Own Course Enrolments | Must |
| FR-16 | Administrator | The system shall allow an Administrator to browse courses. | Browse Courses | Must |
| FR-17 | Administrator | The system shall allow an Administrator to edit the credit value of an existing course. | Edit Course Credits | Must |
| FR-18 | Administrator | The system shall allow an Administrator to view students enrolled across all courses. | View All Course Enrolments | Must |
| FR-19 | Administrator | The system shall allow an Administrator to configure the minimum and maximum credit limits and registration start and end dates in AcademicPolicy. | Configure Academic Policy | Must |
| FR-20 | System | The system shall determine course status from current enrolments: Full when enrolment is greater than or equal to Capacity; otherwise Running when enrolment is greater than or equal to MinToRun; otherwise Open. | Browse Courses / Register for Course | Must |

### 2.1 Functional Rules and Validation

- A Student cannot register for the same course twice.
- A Student cannot register when enrolled count is greater than or equal to course Capacity.
- Registration is blocked when the registration period is closed.
- Registration is blocked when adding the course would cause the Student's total credits to exceed MaxCredits.
- MinCredits is advisory after registration. If the Student's total registered credits are below MinCredits, the interface shows a warning; it does not reject the registration.
- Course credit values must be whole numbers from 1 through 6, inclusive. The course must exist before its credit value can be changed.
- A new course starts with 3 credits. Only an Administrator may change course credits.
- AcademicPolicy is valid only when MinCredits is at least 0, MaxCredits is greater than or equal to MinCredits, and RegistrationEnd is later than RegistrationStart.
- Updates to course credits and AcademicPolicy apply to future registrations only. Existing registrations are not re-checked.
- Course status precedence is Full, then Running, then Open.

## 3. Non-Functional Requirements

The statements below are measurable MVP quality requirements. They do not introduce new user-facing features.

| ID | Quality attribute | Non-functional requirement | Verification |
|---|---|---|---|
| NFR-USA-01 | Usability | For each supported workflow, the application shall display a success or failure outcome after the user submits an operation. | System test each workflow and inspect the resulting feedback. |
| NFR-USA-02 | Usability | For rejected registration, course creation, credit update or policy-save operations, the application shall display a message identifying the reason for rejection. | Exercise each rejection path and inspect the message. |
| NFR-PER-01 | Performance | With a local JSON dataset containing up to 1,000 users, 1,000 courses and 10,000 registrations, course browsing and registration validation shall complete within 2 seconds per operation on the project test machine. | Execute 10 repetitions per operation and record elapsed time; each run must be at most 2 seconds. |
| NFR-REL-01 | Reliability | After a successful save operation, the saved record shall remain available after the application is closed and restarted. | Save, restart and reload the affected data; verify the saved values. |
| NFR-REL-02 | Reliability | A business-rule rejection shall not create or modify a registration, course, course-credit value or academic policy. | Compare persisted data before and after rejected operations. |
| NFR-DAT-01 | Data validation | Course credit values shall be accepted only as whole numbers in the inclusive range 1–6, and only for an existing course. | Test boundary values 1 and 6, out-of-range values 0 and 7, fractional values and an unknown course code. |
| NFR-DAT-02 | Data validation | AcademicPolicy shall be saved only when MinCredits >= 0, MaxCredits >= MinCredits and RegistrationEnd > RegistrationStart. | Test each boundary and invalid relationship independently. |
| NFR-SEC-01 | Password security | The system shall store passwords only as salted password hashes; plaintext passwords shall not be written to user records or JSON storage. | Inspect persisted user data and verify that password values are not plaintext. |
| NFR-SEC-02 | Password security | Password verification shall use the stored salt and password hash; an incorrect password shall not authenticate a user. | Test login with the correct password and with an incorrect password. |
| NFR-MNT-01 | Maintainability | The application shall preserve the three-layer separation: WPF views in Quorum.UI, business operations in Quorum.Logic, and models/data-store operations in Quorum.Data. | Inspect project references and verify that UI contains no business-rule implementation or direct data-store dependency. |
| NFR-MNT-02 | Maintainability | Business-rule validation shall be testable independently of WPF views through the Quorum.Tests project. | Run unit tests against Logic services without launching the WPF application. |

## 4. Scenario Descriptions

### SC-01: Student Registers for a Course

**Primary actor:** Student

**Preconditions:** The Student is authenticated; the registration period is open; the selected course exists, has an available seat, is not already registered by the Student, and adding its credits will not exceed MaxCredits.

**Main flow:**
1. The Student selects a course and requests registration.
2. The system checks the registration period.
3. The system checks for an existing registration.
4. The system checks course capacity.
5. The system checks the Student's resulting credit total against MaxCredits.
6. The system creates and saves the registration.
7. The system derives the course status.
8. The system confirms registration and displays a MinCredits advisory warning when the Student's total is below MinCredits.

**Postcondition:** The registration is persisted.

### SC-02: Registration Is Blocked

**Primary actor:** Student

**Preconditions:** The Student is authenticated and attempts to register for a course.

**Alternative outcomes:** The system rejects the attempt when registration is closed, the Student is already registered for the course, the course is full, or adding the course would exceed MaxCredits. The system displays the corresponding reason and does not save a new registration.

**Postcondition:** No new registration is persisted.

### SC-03: Teacher Views Course Enrolments

**Primary actor:** Teacher

**Preconditions:** The Teacher is authenticated and has created the selected course.

**Main flow:**
1. The Teacher selects one of their courses.
2. The system verifies that the course exists and that the acting Teacher owns it.
3. The system loads registrations and users.
4. The system filters registrations for the selected course and matches them to Student users.
5. The system displays the enrolled Students.

**Alternative outcome:** If the course does not exist or the actor is neither its Teacher nor an Administrator, the service returns an empty student list and the interface displays “No students to display”.

**Postcondition:** No data is changed.

### SC-04: Administrator Sets Academic Policy

**Primary actor:** Administrator

**Preconditions:** The Administrator is authenticated.

**Main flow:**
1. The Administrator submits MinCredits, MaxCredits, RegistrationStart and RegistrationEnd.
2. The system verifies that the actor has the Administrator role.
3. The system validates the policy values.
4. The system saves the valid AcademicPolicy.
5. The system confirms that the policy was saved.

**Alternative outcome:** The system rejects the policy when the actor is not an Administrator, MinCredits is negative, MaxCredits is less than MinCredits, or RegistrationEnd is not later than RegistrationStart. It displays an error and does not save the policy.

**Postcondition:** A valid policy is persisted and applies to future registrations only.

## 5. Source Documents

- `docs/QUORUM-CONTEXT.md` — project scope, business rules, architecture, data contract and approved decisions.
- `docs/design/uc-use-case.puml` — actor/use-case mapping.
- `docs/design/seq-student-registration.puml` — registration flow and alternative branches.
- `docs/design/seq-teacher-course.puml` — course creation and teacher enrolment authorisation.
- `docs/design/seq-admin-credits-policy.puml` — administrator credit and policy validation.
- `docs/design/act-registration-validation.puml` — registration validation workflow.
- `docs/design/class-a-data-model.puml` and `docs/design/class-b-services.puml` — data and Logic-layer contracts.
