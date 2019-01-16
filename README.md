# TimeSheet
#### A class Library to manage the employees and their working hours and calculate their weekly wages. 


Application allows the user to 

* Add an employee and their hourly rate
* Enter a Record of timesheet for any day for respective user. (Week number details are also recorded)
* Retrieve the employees total hours and cost for the week

For persisting data, details are saved into XML files.

## Pre-Requisites

1. Programming Language: C#
2. Target Frameowrk :.NET Core 2.1
3. NUnit and NUnit3 Test Adapter installed and configured on the selected IDE. 
4. IDE : Visual Studio

## Assumptions:

1. Employee details can only be added but can't be updated ( i.e. Hourly rate once entered can't be changed in a workflow)
2. Start Time and End Time can only be enetred in "dd/MM/yyyy HH:mm:ss" format.
3. Timesheet entry once entered for user for a particular day, cannot be updated. ( Edit timesheet workflow is not handled right now as the problem statement doesn't specify the same.) 
4. Entries for a particular date and for aparticular employee cannot be entered twice.
5. Constraint metioned in the problem statement "Hourly rate cannot exceed $50 an hour" is considered for base rate , after applying the penalty i.e. for Saturday and Sunday , hourly rate can go over $50.
6. Wages can be calculated for week only.


## Running the tests 
1. Unzip the project.
2. Open the solution file in IDE.
3. Install NUnit and NUnit3 Test Adapter from Nuget Packages.
4. Run the tests.


## Author
Sagar Kathuria
