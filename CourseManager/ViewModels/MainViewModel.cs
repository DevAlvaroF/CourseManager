using Caliburn.Micro;
using CourseManager.Models;
using CourseManager.Repository;
using CourseManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
namespace CourseManager.ViewModels
{
    internal class MainViewModel : Screen
    {
        private string ConnectionString { get; } = @"Data Source=localhost;Initial Catalog=CourseReport;Integrated Security=True";
        public string AppStatus { get; set; }

        #region Bindable Collections
        // ---------------------------------------------------------------------
        //  These names need to match the ones used in the MainView.xaml for binding e.g. ->    Text="{Binding Path=AppStatus}
        // ---------------------------------------------------------------------

        public BindableCollection<StudentModel> Students { get; set; } = new BindableCollection<StudentModel>();
        public BindableCollection<CourseModel> Courses { get; set; } = new BindableCollection<CourseModel>();
        public BindableCollection<EnrollmentModel> Enrollments { get; set; } = new BindableCollection<EnrollmentModel>();
        #endregion


        #region Selected Item Properties


        private EnrollmentModel _selectedEnrollment;
        public EnrollmentModel SelectedEnrollment
        {
            get
            {
                return _selectedEnrollment;
            }
            set
            {
                _selectedEnrollment = value;
                NotifyOfPropertyChange(() => SelectedEnrollment);
                NotifyOfPropertyChange(() => SelectedEnrollmentCourse);
                NotifyOfPropertyChange(() => SelectedEnrollmentStudent);
            }
        }



        public CourseModel SelectedEnrollmentCourse
        {
            get
            {
                try
                {
                    Dictionary<int, CourseModel> courseDictionary = Courses.ToDictionary(c => c.CourseId);
                    if (SelectedEnrollment != null && courseDictionary.ContainsKey(SelectedEnrollment.CourseId))
                    {
                        return courseDictionary[SelectedEnrollment.CourseId];
                    }
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }

                return null;
            }
            set
            {
                try
                {
                    var selectedEnrollmentcourse = value;
                    SelectedEnrollment.CourseId = selectedEnrollmentcourse.CourseId;
                    NotifyOfPropertyChange(() => SelectedEnrollment);

                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }
            }
        }


        public StudentModel SelectedEnrollmentStudent
        {
            get
            {
                try
                {
                    Dictionary<int, StudentModel> studentDictionary = Students.ToDictionary(c => c.StudentId);
                    if (SelectedEnrollment != null && studentDictionary.ContainsKey(SelectedEnrollment.StudentId))
                    {
                        return studentDictionary[SelectedEnrollment.StudentId];
                    }
                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }

                return null;
            }
            set
            {
                try
                {
                    var selectedEnrollmentStudent = value;
                    SelectedEnrollment.StudentId = selectedEnrollmentStudent.StudentId;
                    NotifyOfPropertyChange(() => SelectedEnrollment);

                }
                catch (Exception ex)
                {
                    UpdateAppStatus(ex.Message);
                }
            }
        }

        #endregion


        //private EnrollmentCommand _enrollmentCommand;
        public MainViewModel()
        {
            SelectedEnrollment = new EnrollmentModel();

            try
            {
                //_enrollmentCommand = new EnrollmentCommand(ConnectionString);

                // Get Student List
                StudentCommand studentCmd = new StudentCommand(ConnectionString);
                Students.AddRange(studentCmd.GetList()); // Add to bindiable list to MainView

                // Get Courses List
                CourseCommand courseCmd = new CourseCommand(ConnectionString);
                Courses.AddRange(courseCmd.GetList()); // Add to bindiable list to MainView

                // Get Enrollments List
                EnrollmentCommand enrollmentCmd = new EnrollmentCommand(ConnectionString);
                Enrollments.AddRange(enrollmentCmd.GetList()); // Add to bindiable list to MainView

            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message);
            }
        }

        private void UpdateAppStatus(string message)
        {
            AppStatus = message;
            NotifyOfPropertyChange(() => AppStatus);
        }

        public void CreateNewEnrollment()
        {
            try
            {
                SelectedEnrollment = new EnrollmentModel();

                AppStatus = "New Enrollment Created";
                NotifyOfPropertyChange(() => AppStatus);

                UpdateAppStatus("New Enrollment Created!");
            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message);
            }

        }

        public void sendReport()
        {
            EmailView emailView = new EmailView();
            emailView.Show();
        }

        public void SaveEnrollment()
        {
            try
            {
                Dictionary<int, EnrollmentModel> enrollmentsDic = Enrollments.ToDictionary(c => c.EnrollmentId);

                if (SelectedEnrollment != null)
                {
                    EnrollmentCommand enrollmentCmd = new EnrollmentCommand(ConnectionString);
                    enrollmentCmd.Upsert(SelectedEnrollment);

                    Enrollments.Clear();
                    Enrollments.AddRange(enrollmentCmd.GetList());

                    UpdateAppStatus("Enrollment Saved!");
                }
            }
            catch (Exception ex)
            {
                UpdateAppStatus(ex.Message);
            }

        }

    }
}
