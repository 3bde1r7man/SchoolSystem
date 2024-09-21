# School System API

This repository contains the API for a School System, allowing management of courses, students, and teachers.

## Overview

The School System API provides a comprehensive set of endpoints to manage various aspects of a school, including course management, student enrollment, and teacher assignments.

## API Endpoints

| Resource | Method | Endpoint | Description |
|----------|--------|----------|-------------|
| **Courses** |
| | GET | /Courses | Retrieve all courses |
| | POST | /Courses | Add a new course |
| | GET | /Courses/{courseId} | Retrieve a specific course |
| | PUT | /Courses/{courseId} | Update a course |
| | DELETE | /Courses/{courseId} | Delete a course |
| **Students** |
| | GET | /Students | Retrieve all students |
| | POST | /Students | Add a new student |
| | GET | /Students/{studid} | Retrieve a specific student |
| | PUT | /Students/{studid} | Update a student |
| | DELETE | /Students/{studid} | Delete a student |
| | GET | /Students/{studid}/courses | Get courses for a student |
| | GET | /Students/{studid}/available-courses | Get available courses for a student |
| | POST | /Students/{studid}/register-course/{courseId} | Register a student for a course |
| | PUT | /Students/{studid}/update-score/{courseId} | Update a student's score in a course |
| | GET | /Students/{studid}/get-score/{courseId} | Get a student's score in a course |
| **Teachers** |
| | GET | /Teachers | Retrieve all teachers |
| | POST | /Teachers | Add a new teacher |
| | GET | /Teachers/{teacherId} | Retrieve a specific teacher |
| | PUT | /Teachers/{teacherId} | Update a teacher |
| | DELETE | /Teachers/{teacherId} | Delete a teacher |
| | GET | /Teachers/{teacherId}/Courses | Get courses for a teacher |
| | POST | /Teachers/{teacherId}/AssignCourse/{courseId} | Assign a course to a teacher |
| | DELETE | /Teachers/{teacherId}/UnassignCourse/{courseId} | Unassign a course from a teacher |

## Getting Started

To get started with this API, follow these steps:

1. Clone this repository
2. Install the required dependencies
3. Run the application

## Usage

You can use tools like Postman to interact with the API. A Postman collection and environment are provided in this repository for easy testing and integration.
