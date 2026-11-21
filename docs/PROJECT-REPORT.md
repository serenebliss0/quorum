# QUORUM
## Course Registration System
**SEN201 Software Engineering Group Project | Group 8**

---

# 1. Introduction and Background of the Study

Course registration is an important process in academic institutions, requiring students to select available courses while ensuring that course enrolment is properly monitored. When registration information is handled inefficiently, it can become difficult to track enrolments, prevent duplicate registrations, identify courses that have reached capacity, and determine whether a course has attracted enough students to run.

Academic course registration can involve multiple platforms, administrative procedures and verification processes. When these activities are distributed across separate systems, students may find it difficult to complete registration efficiently, while academic staff may face additional work monitoring course enrolment and verifying registration records.

Quorum is a proposed desktop-based course registration system inspired by challenges associated with fragmented academic registration workflows, particularly those experienced in the context of university registration processes. It aims to provide a unified environment where students can register for courses, teachers can manage course offerings and monitor enrolment, and administrators can configure academic registration policies.

The system will enforce configurable registration constraints and determine course availability using minimum-enrolment and capacity thresholds. By bringing these activities together within a single application, Quorum aims to simplify the registration process and improve the management of course enrolments.

Quorum will provide three categories of users:

| Functionality | Student | Teacher | Administrator |
|---|---|---|---|
| Create account and log in | ✓ | ✓ | Provisioned |
| Browse courses | ✓ | ✓ | ✓ |
| Register for courses | ✓ | ✗ | ✗ |
| View own registrations | ✓ | ✗ | ✗ |
| Drop registered courses | ✓ | ✗ | ✗ |
| Create courses | ✗ | ✓ | ✓ |
| View course enrolments | ✗ | Own courses | All courses |
| Configure credit limits | ✗ | ✗ | ✓ |
| Configure individual course credits | ✗ | ✗ | ✓ |
| Configure registration periods | ✗ | ✗ | ✓ |
| Configure academic policies | ✗ | ✗ | ✓ |
| Configure system settings | ✗ | ✗ | ✓ |

The Administrator acts as the system's policy authority, having responsibility for configuring institution-wide academic rules, registration periods, course credit values and other system settings.

---

# 2. Statement of the Problem

Course registration in academic institutions can involve several processes distributed across different platforms and administrative channels. Students may be required to interact with multiple portals and complete additional verification procedures before their registration is considered fully processed.

This fragmentation can make the registration process unnecessarily complicated. Students may have difficulty determining whether their registration has been completed successfully, while academic staff may have to consult different platforms or records to monitor course enrolments and verify registration information.

There are also challenges associated with maintaining accurate course enrolment records. Without appropriate validation mechanisms, duplicate registrations may occur, courses may exceed their intended capacity, and it may become difficult to determine whether a course has attracted the minimum number of students required to run.

Furthermore, academic registration requirements may differ between institutions and programmes. Fixed registration constraints can therefore make it difficult to accommodate different credit limits, course credit values and registration periods.

Quorum seeks to address these problems by providing a unified course registration environment with centralised registration records, automated validation and configurable academic policies. It will allow students to manage their course registrations, teachers to manage course offerings and monitor enrolments, and administrators to control the academic rules governing the system.

---

# 3. Aim and Objectives

## 3.1 Aim

The aim of this project is to design and develop a desktop-based course registration system that simplifies course registration, facilitates course enrolment management and provides administrators with configurable control over academic registration policies.

## 3.2 Specific Objectives

The objectives of Quorum are to:

1. Develop an account creation and authentication mechanism for students and teachers.
2. Provide students with an interface for browsing available courses and managing their course registrations.
3. Enable teachers to create courses, specify enrolment capacity and minimum enrolment requirements, and view students registered in their courses.
4. Implement validation mechanisms to prevent duplicate registrations and registration into courses that have reached capacity.
5. Implement a quorum mechanism that determines course status based on minimum enrolment and capacity requirements.
6. Provide administrators with the ability to configure minimum and maximum credit loads, individual course credit values, registration periods and other academic policies.
7. Implement local data persistence to maintain user accounts, course information, registrations and system configurations.
8. Provide a clear and accessible desktop interface through which the different user categories can interact with the system according to their permissions.

---

# 4. Scope and Limitations

## 4.1 Project Scope

Quorum will be developed as a Windows desktop application using C# and Windows Presentation Foundation (WPF). The application will use local JSON-based storage as its primary persistence mechanism.

The system will support three user categories:

**Students**
- Create accounts and log in.
- Browse available courses.
- Register for courses.
- View their registered courses.
- Drop registered courses.

**Teachers**
- Create accounts and log in.
- Create courses.
- Specify course credit values, enrolment capacity and minimum enrolment requirements where permitted by the system's configuration.
- View students enrolled in courses they have created.

**Administrators**
- Access the system through provisioned administrator accounts.
- Browse available courses and view enrolments across all courses.
- Configure minimum and maximum credit loads.
- Configure individual course credit values.
- Configure registration opening and closing periods.
- Configure academic policies and registration constraints.
- Manage general system settings.

The system will also implement registration validation, duplicate-registration prevention, course capacity enforcement and automatic course status determination.

Course statuses will include:

- **Open:** The course has not yet reached its minimum enrolment requirement.
- **Running:** The course has reached its minimum enrolment requirement but has not reached capacity.
- **Full:** The course has reached its maximum enrolment capacity.

The Full status takes precedence when the course reaches capacity.

## 4.2 Project Limitations

The project will be limited to the functionality required for the course registration system.

The following are outside the scope of the initial implementation:

- Student grading and examination management.
- School fee management and payment processing.
- Attendance management.
- General student information management beyond the information required for registration.
- Integration with existing university portals or databases.
- Public API access for third-party developers.
- Webhook and WebSocket integrations.
- Cloud-based or multi-institution deployment.
- Advanced institutional approval and verification workflows.

The system will initially operate using local JSON storage. Supabase may be considered as an optional extension if sufficient development time remains, but it is not a core project requirement.

Quorum is an academic prototype inspired by university registration challenges. It is not intended to replace a complete university management system or serve as an official university registration platform.

---

# 5. Feasibility Study

The feasibility study evaluates whether the proposed system can be developed and operated successfully within the available technical, operational and time constraints.

## 5.1 Technical Feasibility

Quorum is technically feasible because its proposed technologies and development tools are suitable for building a Windows desktop application.

The system will be developed using:

- **C#:** The primary programming language used for application logic and system functionality.
- **Windows Presentation Foundation (WPF):** The framework used to develop the desktop user interface.
- **JSON:** The initial data storage format for maintaining application records and configurations.
- **.NET:** The development platform used to build and run the application.
- **Git and GitHub:** Tools for source-code management, collaboration and contribution tracking.
- **Automated testing tools:** Used to verify the correctness of the application's business logic.

The proposed architecture separates the user interface, application logic and data operations into distinct components. This separation will make the application easier to maintain and allow its core functionality to be tested independently of the user interface.

The team has access to the required development environment and has relevant programming knowledge to implement the proposed functionality.

Therefore, the project is considered technically feasible.

## 5.2 Operational Feasibility

Quorum is operationally feasible because its intended workflows correspond to familiar activities within academic institutions.

Students will interact with a straightforward interface to browse and register for courses. Teachers will use the system to create courses and monitor their enrolments, while administrators will configure the academic rules and system settings governing registration.

The centralisation of these activities within one application is intended to reduce the need for users to consult multiple independent registration interfaces when performing the functions supported by Quorum.

The system will also provide validation feedback when a registration attempt violates an applicable rule, helping users understand why an operation cannot be completed.

Since the application is designed around three clearly defined user categories, access to functionality can be organised according to each user's responsibilities.

Therefore, the proposed system is considered operationally feasible as an academic prototype.

## 5.3 Schedule Feasibility

The project has a submission deadline of 4 December 2026. Development activities will be organised into three incremental stages, allowing the team to implement and verify functionality progressively.

The schedule prioritises essential functionality and reserves the final days for testing, documentation and demonstration preparation.

The optional fourth increment, involving Supabase storage, will only be considered if the required functionality has been completed ahead of schedule.

Given the limited project duration, the team will maintain a controlled project scope and avoid introducing new features after the implementation freeze on 30 November 2026.

The project is considered schedule-feasible provided that the team follows the agreed development schedule and completes the core implementation within the allocated time.

---

# 6. Development Methodology

## 6.1 Selected Development Model

Quorum will adopt the **Incremental Software Development Model**.

The Incremental Model involves developing a software system through a series of smaller, functional releases known as increments. Each increment introduces additional functionality while building upon the work completed in previous stages.

This model was selected because the functionality of Quorum can be divided into distinct development stages. It also allows the team to test individual features progressively rather than waiting until the entire application has been implemented.

The incremental approach is particularly suitable for the project because of its limited development period and the need to deliver a working application before the submission deadline.

## 6.2 Justification for the Selected Model

The Incremental Model is appropriate for Quorum for the following reasons:

1. **Modular development:** The system's functionality can be divided into manageable increments.
2. **Early delivery:** Core features can become operational before the entire application is completed.
3. **Progressive testing:** Each increment can be tested before additional functionality is introduced.
4. **Manageable development:** The team can focus on a smaller set of requirements at each stage.
5. **Reduced integration risk:** Regular integration allows defects and compatibility issues to be identified earlier.

## 6.3 Development Increments

The project will be divided into three primary increments and one optional extension.

| Increment | Features | Target Completion |
|---|---|---|
| Increment 1 | Data models, JSON storage, account creation and login | 24 November 2026 |
| Increment 2 | Course creation, student registration, duplicate prevention, capacity enforcement and quorum rules | 28 November 2026 |
| Increment 3 | Teacher enrolment views, validation, interface refinement and unit testing | 30 November 2026 |
| Increment 4 (Optional) | Supabase storage implementation | Only if time permits |

Following the three primary increments, the team will conduct system and acceptance testing, prepare the demonstration video, compile the final project report and verify the application before submission.

---

# 7. Risk Assessment and Project Schedule

## 7.1 Risk Assessment

Risk assessment involves identifying potential problems that may affect the development, operation or completion of the project and establishing measures to reduce their impact.

The following risks have been identified:

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Scope expansion | High | High | Maintain the agreed project scope and defer optional features. |
| Delays in implementation | Medium | High | Follow the incremental schedule and prioritise essential functionality. |
| Data loss or JSON corruption | Medium | High | Validate data before saving and test storage and recovery behaviour. |
| Registration logic errors | Medium | High | Implement automated tests for duplicate registration, capacity and credit validation. |
| Integration difficulties | Medium | Medium | Establish clear interfaces between components and integrate frequently. |
| Uneven team contributions | Medium | High | Assign responsibilities and track genuine contributions through GitHub. |
| Incomplete documentation | Medium | High | Prepare report sections and diagrams alongside development activities. |
| Insufficient testing time | Medium | High | Complete core functionality before the feature freeze and reserve dedicated testing time. |

## 7.2 Project Schedule

The project will follow the incremental development approach, with additional time allocated for testing, documentation and final submission preparation.

| Date | Activity | Expected Output |
|---|---|---|
| 20 November 2026 | Project planning and scope definition | Project plan and agreed requirements baseline |
| 21 November 2026 | Feasibility study and requirements analysis | Feasibility study and SRS draft |
| 22 November 2026 | Requirements refinement and use-case modelling | Validated requirements and use-case diagram |
| 23 November 2026 | System analysis and design | UML diagrams and architecture design |
| 24 November 2026 | Increment 1 completion | Models, JSON storage, sign-up and login |
| 25–28 November 2026 | Increment 2 implementation | Course management and registration functionality |
| 29–30 November 2026 | Increment 3 implementation and feature freeze | Teacher views, validation and unit tests |
| 1 December 2026 | System and acceptance testing | Testing log and resolved defects |
| 2 December 2026 | Demonstration preparation and report compilation | Screenshots, demonstration recording and report draft |
| 3 December 2026 | Final review and verification | Completed report and verified application |
| 4 December 2026 | Project submission | Final project submission |

The schedule will be monitored throughout development to ensure that delays are identified early and that the team maintains sufficient time for testing and documentation.

No new features will be introduced after 30 November 2026. Optional functionality will only be considered if all required features have been completed and verified.

---

## 8. SRS

The system will provide the following major capabilities:

- User account creation and authentication for Students and Teachers.
- Administrator authentication through a provisioned administrator account.
- Course creation and course browsing.
- Student course registration, registration viewing, and course dropping.
- Validation of duplicate registrations and course enrolment capacity.
- Determination of course status based on enrolment and minimum enrolment requirements.
- Teacher access to enrolment information for courses they created.
- Administrator access to enrolment information across all courses.
- Administrator configuration of academic policies, registration periods, system settings, and credit-load limits.
- Administrator configuration of individual course credit values.
- Local data persistence using JSON storage.

The application will be developed using C# and Windows Presentation Foundation (WPF). Local JSON storage will serve as the default persistence mechanism. The architecture will allow for a possible alternative storage implementation, such as Supabase, if development time permits.

The following are outside the scope of the current project:

- Examination, grading, and academic result management.
- School fee payment and financial management.
- Attendance tracking.
- General student information management beyond information required for course registration.
- Integration with existing university portals or external institutional systems.
- Public API development, webhook integrations, and WebSocket-based communication.
- Multi-institution deployment and advanced institutional approval workflows.

Quorum is intended to address course registration and enrolment management rather than serve as a complete university management system.

### 1.3 Definitions, Acronyms and Abbreviations

| Term | Definition |
|---|---|
| **Quorum** | The Course Registration System being developed by Group 8. |
| **SRS** | Software Requirements Specification. |
| **Student** | A registered system user who can browse, register for, and manage their course registrations. |
| **Teacher** | A registered system user who can create courses and view enrolments for courses they created. |
| **Administrator** | A provisioned system user with authority to configure academic policies and system settings. |
| **Course** | An academic subject made available for student registration. |
| **Course Registration** | The process through which a student enrols in an available course. |
| **Enrolment Capacity** | The maximum number of students permitted to register for a course. |
| **Minimum to Run (MinToRun)** | The minimum number of student registrations required for a course to be considered running. |
| **Quorum (Threshold)** | The minimum enrolment threshold required for a course to run. |
| **Credit Unit** | A numerical value assigned to a course to represent its academic credit weight. |
| **Credit Load** | The total number of credit units accumulated by a student through registered courses. |
| **Registration Period** | The period during which students are permitted to register for courses. |
| **Add/Drop Deadline** | The configured deadline by which students may add or drop registered courses. |
| **JSON** | JavaScript Object Notation, a lightweight data-interchange format used for local data storage. |
| **WPF** | Windows Presentation Foundation, a framework used to develop the application's graphical user interface. |
| **TBD** | To Be Determined; indicates information that has not yet been finalised. |

# 2. Overall Description

## 2.1 Product Perspective

Quorum is a standalone Windows desktop application developed to support academic course registration and enrolment management.

The system will operate as a self-contained application, with its user interface, application logic and data storage organised into separate components. This separation is intended to improve maintainability, support independent testing of business logic and allow the storage mechanism to be extended in the future.

The proposed system will consist of three primary architectural components:

- **Quorum.UI:** Responsible for the graphical user interface and interaction with users.
- **Quorum.Logic:** Responsible for application operations, registration validation, account handling and enforcement of academic rules.
- **Quorum.Data:** Responsible for data models, storage operations and persistence through the JSON-based data store.

The application will initially store its records locally using JSON files. It will not depend on an existing university portal or external institutional database to perform its core functions.

The architecture may accommodate an alternative data storage implementation in the future, but such an implementation is not required for the initial release.

## 2.2 Product Functions

Quorum will provide the following major functions:

### 2.2.1 User Authentication

- Allow Students and Teachers to create accounts.
- Allow registered Students and Teachers to log in.
- Authenticate users using their account credentials.
- Provide Administrators with access through provisioned administrator accounts.
- Restrict access to system functions according to the authenticated user's role.

### 2.2.2 Course Management

- Allow Teachers to create courses.
- Allow Administrators to create courses.
- Allow users to browse available courses.
- Allow Teachers to specify course enrolment capacity and minimum enrolment requirements.
- Allow Administrators to configure the credit value assigned to individual courses.

### 2.2.3 Student Registration

- Allow Students to register for available courses.
- Prevent Students from registering for the same course more than once.
- Prevent registration when a course has reached its enrolment capacity.
- Enforce the configured academic credit-load limits.
- Restrict registration to the permitted registration period.
- Allow Students to view their registered courses.
- Allow Students to drop registered courses within the permitted period.

### 2.2.4 Enrolment Monitoring

- Allow Teachers to view the students enrolled in courses they created.
- Allow Administrators to view enrolments across all courses.
- Determine course status based on the number of registered students, the minimum enrolment requirement and the course capacity.

### 2.2.5 Academic Policy Configuration

- Allow Administrators to configure the minimum and maximum permitted student credit loads.
- Allow Administrators to configure individual course credit values.
- Allow Administrators to configure registration opening and closing periods.
- Allow Administrators to configure academic policies and registration constraints.
- Allow Administrators to configure general system settings.

## 2.3 User Classes and Characteristics

Quorum will support three categories of users. Each category will have access to functionality appropriate to its responsibilities.

### 2.3.1 Student

Students are the primary users of the course registration functionality.

Students will be able to:

- Create an account and log in.
- Browse available courses.
- Register for courses that satisfy the system's registration rules.
- View their registered courses.
- Drop registered courses within the permitted period.

Students will not have permission to create courses, view other students' registration records or modify academic policies.

### 2.3.2 Teacher

Teachers are responsible for creating and managing the courses they offer.

Teachers will be able to:

- Create an account and log in.
- Browse available courses.
- Create courses.
- Specify course enrolment capacity and minimum enrolment requirements.
- View the students enrolled in courses they created.

Teachers will not have permission to register students for courses, manage student registrations or configure institution-wide academic policies.

### 2.3.3 Administrator

Administrators are responsible for managing the academic policies and general configuration of the system.

Administrator accounts will be provisioned rather than created through the public account registration process.

Administrators will be able to:

- Log in using a provisioned administrator account.
- Browse available courses.
- Create courses.
- View enrolments across all courses.
- Configure global minimum and maximum credit-load limits.
- Configure individual course credit values.
- Configure registration periods and applicable deadlines.
- Configure academic policies and registration constraints.
- Manage general system settings.

Administrators will have the highest level of configuration authority within the application.

## 2.4 Operating Environment

Quorum will operate within the following environment:

| Component | Requirement |
|---|---|
| Operating System | Windows |
| Programming Language | C# |
| User Interface Framework | Windows Presentation Foundation (WPF) |
| Development Platform | .NET |
| Data Storage | Local JSON files |
| Source Control | Git and GitHub |
| Application Type | Desktop application |

The application is intended to run on compatible Windows systems with the required .NET runtime or deployment dependencies installed.

| Prefix | Meaning | First IDs |
|---|---|---|
| **FR** | Functional Requirements | Assigned in Section 3 |
| **NFR** | Non-Functional Requirements | Assigned in Section 4 |
| **IR** | Interface Requirements | Assigned in Section 5 |
| **BR** | Business Rules | Assigned in Section 6 |
| **CR** | Constraints | CR-TECH-001 onwards |
| **AR** | Assumptions | AR-USER-001 onwards |

## 2.5 Design and Implementation Constraints

The development of Quorum will be subject to the following constraints:

**CR-TECH-001: Platform Constraint**

The initial application shall target Windows desktop environments and shall be developed using Windows Presentation Foundation (WPF).

**CR-TECH-002: Programming Language Constraint**

C# shall be used as the primary programming language, with .NET providing the application runtime.

**CR-DATA-001: Storage Constraint**

Local JSON files shall serve as the primary data persistence mechanism for the initial implementation.

**CR-PROJ-001: Time Constraint**

Development shall be completed within the agreed project schedule, with a submission deadline of 4 December 2026.

**CR-SCOPE-001: Scope Constraint**

Implementation shall be limited to the approved course registration and enrolment management functionality.

**CR-SEC-001: Access Control Constraint**

System operations shall be restricted according to the permissions assigned to Students, Teachers and Administrators.

**CR-AUTH-001: Administrator Account Constraint**

Administrator accounts shall be provisioned and shall not be created through the public account registration process.

**CR-INT-001: Integration Constraint**

The initial application shall operate independently and shall not require integration with external university systems.

## 2.6 Assumptions and Dependencies

The development and operation of Quorum are based on the following assumptions:

**AR-USER-001: User Credentials**

Students and Teachers will have valid account credentials before accessing their respective system functions.

**AR-ADMIN-001: Administrator Provisioning**

Administrator accounts will be provisioned before administrators access the application.

**AR-COURSE-001: Course Information**

Teachers will provide accurate course information, including course capacity and minimum enrolment requirements.

**AR-POLICY-001: Academic Policy Configuration**

Administrators will configure the academic policies and registration constraints that govern student registration.

**AR-COURSE-002: Course Credit Configuration**

Individual course credit values will be configured by Administrators.

**AR-POLICY-002: Registration Validation**

The system will use the configured registration periods and credit-load limits when validating student registration operations.

**AR-DATA-001: Local Data Availability**

The application will have access to its local data storage location when performing operations that require reading or saving records.

**AR-ENV-001: Operating Environment**

Users will operate the application within a compatible Windows environment.

The system's initial operation will depend on the availability of its local JSON data files and the correct configuration of the application environment.

# 3. Functional Requirements

This section defines the functional requirements of the Quorum Course Registration System. These requirements describe the operations the system shall perform and the functionality it shall provide to Students, Teachers and Administrators.

Each requirement is assigned a unique identifier to support traceability during system design, implementation and testing.

The priority of each requirement is classified as follows:

- **Must:** Essential functionality required for the initial system release.
- **Should:** Desirable functionality that may be implemented if development time permits.

## 3.1 User Authentication and Account Management

### FR-AUTH-001: Student Account Creation

**Description:** The system shall allow individuals to create Student accounts by providing the required registration information.

**Actor:** Student

**Priority:** Must

**Acceptance Criteria:**
- The system accepts valid account information.
- The system rejects incomplete or invalid registration information.
- The system prevents the creation of accounts using an existing username.
- A successfully created account is saved to the system's data store.
- The account is assigned the Student role.

### FR-AUTH-002: Teacher Account Creation

**Description:** The system shall allow individuals to create Teacher accounts by providing the required registration information.

**Actor:** Teacher

**Priority:** Must

**Acceptance Criteria:**
- The system accepts valid account information.
- The system rejects incomplete or invalid registration information.
- The system prevents the creation of accounts using an existing username.
- A successfully created account is saved to the system's data store.
- The account is assigned the Teacher role.

### FR-AUTH-003: User Authentication

**Description:** The system shall authenticate registered Students, Teachers and provisioned Administrators using their account credentials.

**Actor:** Student, Teacher, Administrator

**Priority:** Must

**Acceptance Criteria:**
- Users with valid credentials are granted access.
- Users with invalid credentials are denied access.
- The authenticated user's role is identified by the system.
- Unauthenticated users cannot access protected system functionality.

### FR-AUTH-004: Role-Based Access Control

**Description:** The system shall restrict access to application functionality according to the authenticated user's assigned role.

**Actor:** Student, Teacher, Administrator

**Priority:** Must

**Acceptance Criteria:**
- Students can access only Student-authorised functionality.
- Teachers can access only Teacher-authorised functionality.
- Administrators can access Administrator-authorised functionality.
- Users cannot perform operations reserved for other roles.

## 3.2 Course Management and Browsing

### FR-COURSE-001: Course Creation

**Description:** The system shall allow authenticated Teachers and Administrators to create new courses by providing the required course information.

**Actor:** Teacher, Administrator

**Priority:** Must

**Acceptance Criteria:**
- Authenticated Teachers and Administrators can access the course creation function.
- Required course information must be provided.
- The system rejects incomplete or invalid course information.
- The system prevents duplicate course codes.
- Successfully created courses are saved to the data store.

### FR-COURSE-002: Course Capacity Configuration

**Description:** The system shall allow Teachers to specify the maximum enrolment capacity of courses they create.

**Actor:** Teacher

**Priority:** Must

**Acceptance Criteria:**
- The Teacher can specify a course capacity during course creation.
- The specified capacity must be a valid positive value.
- The system stores the configured capacity with the course information.
- The capacity is used when validating student registration attempts.

### FR-COURSE-003: Minimum Enrolment Configuration

**Description:** The system shall allow Teachers to specify the minimum number of student registrations required for a course to run.

**Actor:** Teacher

**Priority:** Must

**Acceptance Criteria:**
- The Teacher can specify a minimum enrolment value during course creation.
- The minimum enrolment value must be valid.
- The configured minimum must not exceed the course capacity.
- The system stores the configured value with the course information.

### FR-COURSE-004: Course Browsing

**Description:** The system shall allow authenticated Students, Teachers and Administrators to browse available courses and view their relevant information.

**Actor:** Student, Teacher, Administrator

**Priority:** Must

**Acceptance Criteria:**
- All three user roles can access the course browsing function.
- The system displays available course information.
- Course information includes its code, title, credit value, enrolment capacity and current enrolment status.
- The system displays the course's current enrolment information.

## 3.3 Student Course Registration

### FR-REG-001: Course Registration

**Description:** The system shall allow authenticated Students to register for available courses, provided that all applicable registration requirements are satisfied.

**Actor:** Student

**Priority:** Must

**Acceptance Criteria:**
- Only authenticated Students can register for courses.
- The selected course must exist in the system.
- The registration must satisfy the applicable registration rules.
- A successful registration is saved to the data store.
- The Student receives confirmation of a successful registration.

### FR-REG-002: Duplicate Registration Prevention

**Description:** The system shall prevent a Student from registering for a course in which they are already registered.

**Actor:** Student

**Priority:** Must

**Acceptance Criteria:**
- The system checks the Student's existing registrations before processing a new registration.
- A duplicate registration attempt is rejected.
- The system does not create an additional registration record.
- The Student receives an appropriate notification.

### FR-REG-003: Course Capacity Enforcement

**Description:** The system shall prevent registration when the number of students registered for a course has reached its configured enrolment capacity.

**Actor:** Student

**Priority:** Must

**Acceptance Criteria:**
- The system checks the current enrolment count before accepting a registration.
- Registration is rejected when the enrolment count is equal to or greater than the configured capacity.
- The system does not exceed the configured course capacity.
- The Student receives an appropriate notification when registration is rejected.

### FR-REG-004: Credit Load Validation

**Description:** The system shall validate a Student's registered credit load against the minimum and maximum credit-load limits configured by the Administrator.

**Actor:** Student, Administrator

**Priority:** Must

**Acceptance Criteria:**
- The system calculates the total credit value of a Student's registered courses.
- The system retrieves the configured minimum and maximum credit-load limits.
- The system uses these limits when validating the Student's registration load.
- Registration operations that violate applicable credit-load restrictions are rejected.

### FR-REG-005: Registration Period Enforcement

**Description:** The system shall permit Students to register for courses only within the registration period configured by the Administrator.

**Actor:** Student, Administrator

**Priority:** Must

**Acceptance Criteria:**
- The system retrieves the configured registration opening and closing dates.
- Registration is permitted within the configured period.
- Registration attempts outside the permitted period are rejected.
- The Student receives an appropriate notification when registration is unavailable.

### FR-REG-006: View Registered Courses

**Description:** The system shall allow Students to view the courses for which they are currently registered.

**Actor:** Student

**Priority:** Must

**Acceptance Criteria:**
- An authenticated Student can access their registration records.
- The system displays only the registrations belonging to the authenticated Student.
- The displayed information includes relevant course details.
- The displayed records reflect successful registration and dropping operations.

### FR-REG-007: Drop Registered Courses

**Description:** The system shall allow Students to drop courses in which they are registered, subject to the configured add/drop deadline and applicable academic policies.

**Actor:** Student, Administrator

**Priority:** Must

**Acceptance Criteria:**
- A Student can select a course from their existing registrations.
- The system verifies that the Student is registered for the selected course.
- The system verifies that the applicable dropping period has not expired.
- A successful drop removes the active registration.
- The Student's current credit load and course enrolment count are updated accordingly.

## 3.4 Course Enrolment Monitoring

### FR-ENR-001: Teacher Enrolment Viewing

**Description:** The system shall allow Teachers to view the Students registered in courses they created.

**Actor:** Teacher

**Priority:** Must

**Acceptance Criteria:**
- An authenticated Teacher can view enrolment information for their courses.
- The system displays the Students registered in each selected course.
- Teachers cannot access enrolment records belonging exclusively to courses created by other Teachers.

### FR-ENR-002: Administrator Enrolment Viewing

**Description:** The system shall allow Administrators to view enrolment information across all courses in the system.

**Actor:** Administrator

**Priority:** Must

**Acceptance Criteria:**
- An authenticated Administrator can access enrolment information for all courses.
- The system displays the Students registered in the selected courses.
- The Administrator can view the current enrolment count for each course.

### FR-ENR-003: Automatic Course Status Determination

**Description:** The system shall automatically determine the status of each course using its current enrolment count, minimum enrolment requirement and maximum enrolment capacity.

**Actor:** System

**Priority:** Must

**Acceptance Criteria:**
- A course is assigned **Open** status when its enrolment count is below its minimum enrolment requirement.
- A course is assigned **Running** status when its enrolment count has reached its minimum enrolment requirement but remains below capacity.
- A course is assigned **Full** status when its enrolment count reaches or exceeds capacity.
- Full status takes precedence over the other statuses.
- Course status is updated when the enrolment count changes.

## 3.5 Academic Policy and System Configuration

### FR-ADM-001: Credit Load Configuration

**Description:** The system shall allow Administrators to configure the minimum and maximum credit-load limits applicable to Students.

**Actor:** Administrator

**Priority:** Must

**Acceptance Criteria:**
- An Administrator can view the current credit-load limits.
- An Administrator can modify the minimum and maximum values.
- The system validates the configured values.
- The system rejects configurations where the minimum credit load exceeds the maximum credit load.
- Updated values are saved and used during registration validation.

### FR-ADM-002: Individual Course Credit Configuration

**Description:** The system shall allow Administrators to configure the credit value assigned to individual courses.

**Actor:** Administrator

**Priority:** Must

**Acceptance Criteria:**
- An Administrator can select an existing course.
- The Administrator can assign or modify its credit value.
- The configured credit value must be valid.
- Updated credit values are saved and used when calculating Student credit loads.

### FR-ADM-003: Registration Period Configuration

**Description:** The system shall allow Administrators to configure the opening and closing dates of the course registration period.

**Actor:** Administrator

**Priority:** Must

**Acceptance Criteria:**
- An Administrator can configure the registration opening date.
- An Administrator can configure the registration closing date.
- The system validates the configured dates.
- The opening date must not occur after the closing date.
- Updated registration periods are applied when validating Student registration attempts.

### FR-ADM-004: Add/Drop Deadline Configuration

**Description:** The system shall allow Administrators to configure the deadline by which Students may drop registered courses.

**Actor:** Administrator

**Priority:** Must

**Acceptance Criteria:**
- An Administrator can configure the add/drop deadline.
- The system validates the configured deadline.
- The configured deadline is used when processing Student course-dropping requests.
- Dropping requests submitted after the permitted deadline are rejected.

### FR-ADM-005: Academic Policy Configuration

**Description:** The system shall allow Administrators to configure the academic policies and registration constraints supported by Quorum.

**Actor:** Administrator

**Priority:** Must

**Acceptance Criteria:**
- An Administrator can access the academic policy configuration function.
- The system allows modification of supported academic policy settings.
- The system validates policy configurations before saving them.
- Updated policies are applied to relevant system operations.

### FR-ADM-006: General System Configuration

**Description:** The system shall allow Administrators to configure supported general system settings.

**Actor:** Administrator

**Priority:** Must

**Acceptance Criteria:**
- An Administrator can access the system configuration function.
- Supported settings can be viewed and modified.
- Invalid configurations are rejected.
- Valid changes are saved and applied by the system.

## 9. UML Diagrams

![alt text](image.png)
