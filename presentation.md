# Contract Monthly Claim System (CMCS)

---

## Slide 2: Agenda

* System Overview
* Key Features
* System Architecture
* Live Demonstration
* Future Enhancements
* Q&A

---

## Slide 3: System Overview

* **What is CMCS?** A web-based application to automate and streamline the monthly claim process for Independent Contractor (IC) lecturers.
* **Problem:** The manual claim process is time-consuming, prone to errors, and lacks transparency.
* **Solution:** CMCS provides a centralized platform for claim submission, tracking, and approval, improving efficiency and accuracy.

---

## Slide 4: Key Features

* **Role-Based Access:** Secure login for Lecturers, Programme Coordinators, and Academic Managers.
* **Claim Submission:** Easy-to-use form for lecturers to submit monthly claims with supporting documents.
* **Multi-Level Approval Workflow:** Claims are routed automatically for a two-step approval process.
* **Dashboard:** At-a-glance view of pending claims and system statistics for managers.
* **Claim Tracking:** Lecturers can track the status of their submitted claims.

---

## Slide 5: System Architecture

* **Technology Stack:** ASP.NET Core MVC, Entity Framework Core, SQL Server.
* **Design Pattern:** Model-View-Controller (MVC) for separation of concerns.
* **Database:** Code-First approach with Entity Framework Core for database design and migrations.
* **Frontend:** Responsive design using Bootstrap for a consistent experience across devices.

---

## Slide 6: Live Demonstration - Step 1: Lecturer Submits Claim

* Show the lecturer login.
* Navigate to the 'Submit Claim' page.
* Fill out the claim form (hours, rate).
* Attach a supporting document.
* Submit the claim and view the confirmation.
* Show the 'My Claims' page with the new claim's status as 'Submitted'.

---

## Slide 7: Live Demonstration - Step 2: Programme Coordinator Approval

* Log in as a Programme Coordinator.
* Show the dashboard with the count of pending claims.
* Navigate to the 'Approve Claims' page.
* Show the list of submitted claims.
* Click 'Approve' on the new claim.
* Show that the claim is now gone from this list.

---

## Slide 8: Live Demonstration - Step 3: Academic Manager Approval

* Log in as an Academic Manager.
* Show the dashboard with the count of pending claims.
* Navigate to the 'Approve Claims' page.
* Show the claim with the status 'ApprovedByProgrammeCoordinator'.
* Click 'Approve' for final approval.
* Show the claim is now gone from this list.

---

## Slide 9: Live Demonstration - Step 4: Final Status

* Log back in as the lecturer.
* Go to the 'My Claims' page.
* Show the claim status is now 'ApprovedByAcademicManager'.

---

## Slide 10: Future Enhancements

* **Email Notifications:** Automated emails for claim status changes.
* **Reporting:** Advanced reporting and analytics for managers.
* **Rejection Reasons:** Adding a field for managers to provide a reason for rejecting a claim.
* **Integration:** Potential for integration with HR and payroll systems.

---

## Slide 11: Q&A

* Open the floor for questions.
