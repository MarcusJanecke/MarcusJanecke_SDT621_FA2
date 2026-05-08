using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ULMSWinFormsApp.Models;

namespace ULMSWinFormsApp.Forms
{
    public partial class FrmReports : Form
    {
        // Temporary in-memory sample data used by the report form. In a real app
        // this should be replaced with a proper data repository or service.
        private readonly List<Student> _sampleStudents = new List<Student>
        {
            new Student { StudentId = "1", FullName = "Alice Smith", Email = "alice@example.com", Age = 20, Programme = "Software Development" },
            new Student { StudentId = "2", FullName = "Bob Jones", Email = "bob@example.com", Age = 22, Programme = "Data Science" },
            new Student { StudentId = "3", FullName = "Carol White", Email = "carol@example.com", Age = 21, Programme = "Cloud Computing" }
        };

        public FrmReports()
        {
            InitializeComponent();
        }

        private void btnBackReport_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClearReport_Click(object sender, EventArgs e)
        {
            cmbReportType.SelectedIndex = -1;
            txtReportStudentId.Clear();
            txtReportOutput.Clear();
            dataGridView1.DataSource = null;
            lblStatus.Text = string.Empty;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            txtReportOutput.Clear();
            dataGridView1.DataSource = null;
            lblStatus.Text = string.Empty;

            if (cmbReportType.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a report type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Optional Student ID filter - validate if present
            string filterText = txtReportStudentId.Text?.Trim();
            int filterId = -1;
            bool filterById = false;

            if (!string.IsNullOrEmpty(filterText))
            {
                if (!int.TryParse(filterText, out filterId))
                {
                    MessageBox.Show("Student ID filter must be a valid integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtReportStudentId.Focus();
                    return;
                }

                filterById = true;
            }

            string selected = cmbReportType.SelectedItem.ToString();

            switch (selected)
            {
                case "Student Summary Report":
                    GenerateStudentSummaryReport(filterById ? filterId.ToString() : null);
                    break;

                case "Marks Report":
                    GenerateMarksReport(filterById ? filterId.ToString() : null);
                    break;

                case "Enrollment Report":
                    GenerateEnrollmentReport(filterById ? filterId.ToString() : null);
                    break;

                default:
                    MessageBox.Show("Unknown report type selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void GenerateStudentSummaryReport(string studentIdFilter)
        {
            var rows = _sampleStudents.AsEnumerable();

            if (!string.IsNullOrEmpty(studentIdFilter))
            {
                rows = rows.Where(s => s.StudentId == studentIdFilter);
            }

            if (!rows.Any())
            {
                lblStatus.Text = "No students found for the selected filter.";
                return;
            }

            // Show in DataGridView
            dataGridView1.DataSource = rows.Select(s => new
            {
                s.StudentId,
                s.FullName,
                s.Email,
                s.Age,
                s.Programme
            }).ToList();

            lblStatus.Text = "Student summary generated.";
        }

        private void GenerateMarksReport(string studentIdFilter)
        {
            // There is no central marks store in this toy project. Create sample marks
            var sampleMarks = new List<MarkRecord>
            {
                new MarkRecord { StudentId = "1", StudentName = "Alice Smith", Subject1 = 78, Subject2 = 84, Subject3 = 69, Average = (78+84+69)/3.0, ResultStatus = "PASS" },
                new MarkRecord { StudentId = "2", StudentName = "Bob Jones", Subject1 = 45, Subject2 = 51, Subject3 = 38, Average = (45+51+38)/3.0, ResultStatus = "FAIL" },
            };

            var rows = sampleMarks.AsEnumerable();

            if (!string.IsNullOrEmpty(studentIdFilter))
            {
                rows = rows.Where(m => m.StudentId == studentIdFilter);
            }

            if (!rows.Any())
            {
                lblStatus.Text = "No marks found for the selected filter.";
                return;
            }

            dataGridView1.DataSource = rows.Select(m => new
            {
                m.StudentId,
                m.StudentName,
                m.Subject1,
                m.Subject2,
                m.Subject3,
                Average = m.Average.ToString("F2"),
                m.ResultStatus
            }).ToList();

            lblStatus.Text = "Marks report generated.";
        }

        private void GenerateEnrollmentReport(string studentIdFilter)
        {
            // No real enrollment store; stubbed sample
            var sampleEnrollments = new List<Models.Enrollment>
            {
                new Models.Enrollment { StudentId = "1", CourseId = "CS101", CourseName = "Intro to Programming" },
                new Models.Enrollment { StudentId = "2", CourseId = "DS201", CourseName = "Data Analysis" }
            };

            var rows = sampleEnrollments.AsEnumerable();

            if (!string.IsNullOrEmpty(studentIdFilter))
            {
                rows = rows.Where(e => e.StudentId == studentIdFilter);
            }

            if (!rows.Any())
            {
                lblStatus.Text = "No enrollments found for the selected filter.";
                return;
            }

            dataGridView1.DataSource = rows.Select(e => new
            {
                e.StudentId,
                e.CourseId,
                e.CourseName
            }).ToList();

            lblStatus.Text = "Enrollment report generated.";
        }
    }
}
