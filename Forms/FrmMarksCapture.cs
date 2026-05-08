using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ULMSWinFormsApp.Models;

namespace ULMSWinFormsApp.Forms
{
    public partial class FrmMarksCapture : Form
    {
        // In-memory storage for saved marks
        private readonly List<MarkRecord> _savedMarks = new List<MarkRecord>();

        public FrmMarksCapture()
        {
            InitializeComponent();
        }

        private void btnCalculateResults_Click(object sender, EventArgs e)
        {
            // Validate Student ID is an integer
            if (!int.TryParse(txtMarkStudentId.Text?.Trim(), out int studentId))
            {
                MessageBox.Show("Please enter a valid numeric Student ID.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                txtMarkStudentId.Focus();
                return;
            }

            // Validate Student Name is provided
            if (string.IsNullOrWhiteSpace(txtMarkStudentName.Text))
            {
                MessageBox.Show("Please enter the student name.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                txtMarkStudentName.Focus();
                return;
            }

            // Validate subjects are integers
            if (!int.TryParse(txtSubject1.Text?.Trim(), out int s1) ||
                !int.TryParse(txtSubject2.Text?.Trim(), out int s2) ||
                !int.TryParse(txtSubject3.Text?.Trim(), out int s3))
            {
                MessageBox.Show("All subject marks must be whole numbers.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Range checks for marks (0-100)
            if (s1 < 0 || s1 > 100 || s2 < 0 || s2 > 100 || s3 < 0 || s3 > 100)
            {
                MessageBox.Show("Marks must be between 0 and 100.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Build record using validated values
            MarkRecord record = new MarkRecord();
            record.StudentId = studentId.ToString();
            record.StudentName = txtMarkStudentName.Text.Trim();
            record.Subject1 = s1;
            record.Subject2 = s2;
            record.Subject3 = s3;

            // Correct average calculation (use double division)
            record.Average = (record.Subject1 + record.Subject2 + record.Subject3) / 3.0;
            record.ResultStatus = record.Average >= 50 ? "PASS" : "FAIL";

            txtMarksOutput.Text =
                "Marks processed successfully!" + Environment.NewLine +
                "Student ID: " + record.StudentId + Environment.NewLine +
                "Student Name: " + record.StudentName + Environment.NewLine +
                "Subject 1: " + record.Subject1 + Environment.NewLine +
                "Subject 2: " + record.Subject2 + Environment.NewLine +
                "Subject 3: " + record.Subject3 + Environment.NewLine +
                "Average: " + record.Average + Environment.NewLine +
                "Final Result: " + record.ResultStatus;
        }

        private void btnSaveMark_Click(object sender, EventArgs e)
        {
            // Validate Student ID
            if (!int.TryParse(txtMarkStudentId.Text?.Trim(), out int studentId))
            {
                MessageBox.Show("Please enter a valid numeric Student ID.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                txtMarkStudentId.Focus();
                return;
            }

            // Validate Student Name
            if (string.IsNullOrWhiteSpace(txtMarkStudentName.Text))
            {
                MessageBox.Show("Please enter the student name.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                txtMarkStudentName.Focus();
                return;
            }

            // Check for empty field
            if (string.IsNullOrWhiteSpace(txtSubject1.Text))
            {
                MessageBox.Show("Please enter a mark.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Try parse to handle non-numeric input
            if (!int.TryParse(txtSubject1.Text.Trim(), out int mark))
            {
                MessageBox.Show("Mark must be a numeric value.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Range validation
            if (mark < 0 || mark > 100)
            {
                MessageBox.Show("Mark must be between 0 and 100.",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // All good — save the mark using the dedicated save method
            SaveMark(mark);
            MessageBox.Show("Mark saved successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        // Implement the missing SaveMark method
        private void SaveMark(int mark)
        {
            var record = new MarkRecord();
            // Validate and assign Student ID and Name (assume prior validation in caller)
            if (int.TryParse(txtMarkStudentId.Text?.Trim(), out int studentId))
            {
                record.StudentId = studentId.ToString();
            }
            else
            {
                record.StudentId = txtMarkStudentId.Text?.Trim() ?? string.Empty;
            }

            record.StudentName = txtMarkStudentName.Text?.Trim() ?? string.Empty;
            record.Subject1 = mark;

            // Try parse other subjects as integers; default to 0 if invalid
            if (int.TryParse(txtSubject2.Text?.Trim(), out int s2))
                record.Subject2 = s2;
            else
                record.Subject2 = 0;

            if (int.TryParse(txtSubject3.Text?.Trim(), out int s3))
                record.Subject3 = s3;
            else
                record.Subject3 = 0;

            // Proper average calculation
            record.Average = (record.Subject1 + record.Subject2 + record.Subject3) / 3.0;
            record.ResultStatus = record.Average >= 50 ? "PASS" : "FAIL";

            _savedMarks.Add(record);
        }

        private void btnClearMarks_Click(object sender, EventArgs e)
        {
            txtMarkStudentId.Clear();
            txtMarkStudentName.Clear();
            txtSubject1.Clear();
            txtSubject2.Clear();
            txtSubject3.Clear();
            txtMarksOutput.Clear();
            txtMarkStudentId.Focus();
        }

        private void btnBackMarks_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
