using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using ULMSWinFormsApp.Models;

namespace ULMSWinFormsApp.Forms
{
    public partial class FrmCourseEnrollment : Form
    {
        // Add a backing list for enrollments
        private readonly List<Enrollment> enrolmentList = new List<Enrollment>();

        public FrmCourseEnrollment()
        {
            InitializeComponent();
        }

        // Renamed to match the Designer event subscription (btnEnroll_Click)
        private void btnEnroll_Click(object sender, EventArgs e)
        {
            if (cmbCourse.SelectedItem == null)
            {
                MessageBox.Show("Please select a course.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string course = cmbCourse.SelectedItem.ToString();

            // Parse and validate studentId from the textbox
            if (!int.TryParse(txtEnrollStudentId.Text?.Trim(), out int studentId))
            {
                MessageBox.Show("Please enter a valid numeric Student ID.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                txtEnrollStudentId.Focus();
                return;
            }

            // Check if already enrolled
            if (IsAlreadyEnrolled(studentId, course))
            {
                MessageBox.Show("Student is already enrolled in this course.",
                                "Duplicate Enrolment",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            EnrolStudent(studentId, course);
            MessageBox.Show("Enrolment successful!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        // Helper method to check duplicates
        private bool IsAlreadyEnrolled(int studentId, string course)
        {
            // Check your list or database here
            // Example using a list:
            return enrolmentList.Any(e => e.StudentId == studentId.ToString() && e.CourseName == course);
        }

        // Add the missing EnrolStudent method
        private void EnrolStudent(int studentId, string course)
        {
            var enrollment = new Enrollment
            {
                StudentId = studentId.ToString(),
                StudentName = txtEnrollStudentName?.Text?.Trim() ?? string.Empty,
                CourseName = course,
                Semester = cmbSemester.SelectedItem?.ToString() ?? cmbSemester.Text ?? string.Empty
            };

            enrolmentList.Add(enrollment);

            // Update output textbox if available
            if (txtEnrollmentOutput != null)
            {
                txtEnrollmentOutput.AppendText($"ID: {enrollment.StudentId}, Name: {enrollment.StudentName}, Course: {enrollment.CourseName}, Semester: {enrollment.Semester}{Environment.NewLine}");
            }
        }

        private void btnClearEnrollment_Click(object sender, EventArgs e)
        {
            txtEnrollStudentId.Clear();
            txtEnrollStudentName.Clear();
            cmbCourse.SelectedIndex = -1;
            cmbSemester.SelectedIndex = -1;
            txtEnrollmentOutput.Clear();
            txtEnrollStudentId.Focus();
        }

        private void btnBackEnrollment_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
