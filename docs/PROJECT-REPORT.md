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

**End of Project Initiation and Planning Draft**