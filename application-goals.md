# Driving Lessons Application - Requirements Document

## Project Overview
A desktop application built with WPF, C#, MVVM pattern, and SQLite for connecting driving students with instructors. The platform enables students to discover and book lessons with qualified teachers while providing teachers with tools to manage their students and track progress.

---

## User Roles

### Student
- Browse and search for driving instructors
- View detailed instructor profiles
- Book lesson appointments (pending teacher approval)
- Track personal learning progress
- Leave reviews and ratings for instructors

### Teacher
- Manage personal profile and pricing
- Accept or decline lesson booking requests
- Track individual student progress and skills
- Manage student roster
- View lesson schedule and upcoming appointments
- Track billing information (mock implementation)

---

## Core Features

### 1. Authentication System
- User registration (Student/Teacher role selection)
- User login
- Password management
- Role-based access control

### 2. Teacher Profile Management

#### Public Profile Information
- Full name
- Star rating (aggregate from student reviews)
- Years of teaching experience
- Price per lesson
- Biography/description
- Car make and model
- Transmission type (Automatic/Manual)
- Profile photo (optional)
- Location/service area
- Languages spoken (optional)

#### Teacher Dashboard
- Edit profile information
- View and manage student roster
- Accept/decline booking requests
- Track student progress
- View upcoming lessons
- Manage availability calendar

### 3. Student Discovery & Search

#### Browse Functionality
- View all available teachers in a list/grid
- Display key information: rating, price, experience, transmission type

#### Search & Filter Options
- **Location/Area**: Filter by city, neighborhood, or service area
- **Transmission Type**: Automatic, Manual, or Both
- **Price Range**: Min/max price slider
- **Minimum Rating**: Filter by star rating threshold
- **Availability**: Teachers with openings this week/month
- **Car Make/Model**: Specific vehicle preference
- **Experience Level**: Years of experience range
- **Languages**: Filter by languages spoken

#### Teacher Detail View
- Complete profile information
- Student reviews and ratings
- Availability calendar
- "Book a Lesson" call-to-action

### 4. Booking System

#### Booking Workflow
1. **Student Initiates**: Student selects date/time from teacher's calendar
2. **Request Pending**: Booking enters pending state
3. **Teacher Review**: Teacher receives notification and reviews request
4. **Teacher Decision**: Accept or decline with optional message
5. **Confirmation**: Student receives notification of acceptance/rejection
6. **Scheduled Lesson**: Confirmed lesson appears on both calendars

#### Booking Management
- View pending requests (count badge/notification)
- View upcoming confirmed lessons
- Cancellation system (with time restrictions, e.g., 24-hour notice)
- Booking history

### 5. Progress Tracking

#### Student Progress Dashboard
- Total lessons completed
- Hours of instruction received
- Skills mastered (checklist view)
- Next scheduled lesson
- Overall progress percentage

#### Skills Checklist (Teacher can mark as completed)
- ☐ Basic vehicle controls
- ☐ Parallel parking
- ☐ Three-point turn
- ☐ Highway merging
- ☐ Lane changing
- ☐ Reverse parking
- ☐ Night driving
- ☐ Adverse weather conditions
- ☐ Defensive driving techniques
- ☐ Mock road test

#### Teacher Progress Management
- View individual student progress
- Mark skills as completed/in-progress
- Add notes per student
- Track lesson history per student

### 6. Review & Rating System

#### Review Submission
- Students can review after completed lessons
- 1-5 star rating (required)
- Written review (optional)
- One review per completed lesson

#### Review Display
- Aggregate star rating on teacher profile
- Display recent reviews with student name and date
- Sort reviews by most recent/highest rated
- Total number of reviews

### 7. Billing Tracking (Mock Implementation)

#### Teacher View
- Track income per student
- View payment status per lesson (Paid/Unpaid/Pending)
- Generate simple financial reports (total earnings, earnings by month)
- Mark lessons as paid

#### Student View
- View lesson costs
- Track total spent
- View payment history

---

## Application Flow

### Student Journey
1. **Sign Up/Login** → Student Dashboard
2. **Browse Teachers** → Apply filters/search
3. **View Teacher Profile** → Read reviews, check availability
4. **Request Booking** → Select date/time
5. **Wait for Approval** → Receive notification
6. **Attend Lesson** → Complete lesson
7. **Track Progress** → View skills dashboard
8. **Leave Review** → Rate and review teacher

### Teacher Journey
1. **Sign Up/Login** → Teacher Dashboard
2. **Complete Profile** → Add bio, pricing, vehicle info
3. **Set Availability** → Update calendar
4. **Receive Booking Requests** → View pending requests
5. **Accept/Decline** → Send confirmation
6. **Conduct Lesson** → Teach student
7. **Update Progress** → Mark completed skills, add notes
8. **Track Billing** → Update payment status

---

## Technical Architecture

### Technology Stack
- **Framework**: WPF (Windows Presentation Foundation)
- **Language**: C# (.NET)
- **Pattern**: MVVM (Model-View-ViewModel)
- **Database**: SQLite
- **UI Controls**: WPF Calendar for date/time selection

### Database Schema (High-Level)

#### Tables
- **Users**: Id, Email, PasswordHash, Role, CreatedAt
- **TeacherProfiles**: UserId, Bio, YearsExperience, PricePerLesson, CarModel, TransmissionType, Location, Languages
- **StudentProfiles**: UserId, CreatedAt
- **Bookings**: Id, StudentId, TeacherId, DateTime, Status (Pending/Accepted/Declined/Completed/Cancelled), CreatedAt
- **Reviews**: Id, BookingId, StudentId, TeacherId, Rating, ReviewText, CreatedAt
- **StudentProgress**: Id, StudentId, TeacherId, SkillId, Status (NotStarted/InProgress/Completed), Notes
- **Skills**: Id, SkillName, Description
- **Billing**: Id, BookingId, Amount, PaymentStatus, PaidDate

---

## Future Enhancements (Optional)
- Email/SMS notifications for booking confirmations
- In-app messaging between students and teachers
- Multi-language support
- Mobile companion app
- Integration with calendar applications (Google Calendar, Outlook)
- Advanced analytics and reporting
- Payment gateway integration
- Instructor certification verification
- Student referral program
- Group lesson booking

---

## Success Criteria
- Students can successfully find and book lessons with teachers
- Teachers can efficiently manage their schedule and students
- Progress tracking provides meaningful insights
- The application handles the complete lesson lifecycle from discovery to completion
- User interface is intuitive and responsive
- Data persistence works reliably with SQLite

---

**Document Version**: 1.0  
**Last Updated**: February 2026  
**Project Type**: Hobby/Portfolio Project
