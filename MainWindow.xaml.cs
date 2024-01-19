using DataImportToDatabase.Models;
using DataImportToDatabase.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataImportToDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtPath.Text))
            {
                importDataFromCSVFile(txtPath.Text);
            }
            MessageBox.Show("Don't have choose file");
        }

        private void importDataFromCSVFile(string filePath)
        {
            bool skipHeader = false;
            List<int> listSubjectId = null;
            SchoolYear selectedYear = findSchoolYear((cboYear.SelectedItem as ComboBoxItem)?.Content.ToString());
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using(StreamReader reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        
                        string[] line = reader.ReadLine()!.Split(',');
                        if (skipHeader)
                        {
                            int year = getIntegerOrDefault(line[6]);
                            if (selectedYear == null)
                            {
                                MessageBox.Show("Already import");
                                break;
                            }
                            if (year == selectedYear.ExamYear)
                            {
                                int studentId = importStudentByYear(selectedYear.Id, line);
                                importScore(line, studentId, listSubjectId);
                            }
                            else
                            {
                                MessageBox.Show("Error");
                                break;
                            }
                        }
                        else
                        {
                            skipHeader = true;
                            listSubjectId = importSubjectInHeader(line);
                        }
                    }
                }
            }
            MessageBox.Show("Import done !!");
        }

        private SchoolYear findSchoolYear(string? input)
        {
            using (var _context = new MyDbContext())
            {
                SchoolYear schoolYear = _context.schoolYear.FirstOrDefault(x => x.ExamYear == getIntegerOrDefault(input))!;
                Student student = _context.students.FirstOrDefault(x => x.SchoolYearId == schoolYear.Id);
                if(student == null)
                {
                    return schoolYear;
                }
                return null;
            }
        }
        private int getIntegerOrDefault(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;
            return int.Parse(input);
        }
        private int importStudentByYear(int yearId, string[] line)
        {
            using(var _context = new MyDbContext())
            {
                var _student = new Student(line[0], yearId, true);
                _context.students.Add(_student);
                _context.SaveChanges();
                return _student.Id;
            }
        }

        private void importScore(string[] line, int studentId, List<int> listSubjectId)
        {
            foreach (var subjectId in listSubjectId)
            {
                using(var _context = new MyDbContext())
                {
                    int i = 1;
                    if (i == 6) continue;
                    Score score = new Score(studentId, subjectId, getDoubleOrDefault(line[i]));
                    i++;
                    _context.scores.Add(score);
                    _context.SaveChanges();
                }
            }
        }
        private List<int> importSubjectInHeader(string[] line)
        {
            List<int> result = new List<int>();
            using (var _context = new MyDbContext())
            {
                if (!_context.subjects.Any())
                {
                    for (int i = 1; i < line.Length; i++)
                    {
                        if (i == 6) continue;

                        Subject _subject = new Subject(line[i], line[i]);
                        _context.subjects.Add(_subject);
                        _context.SaveChanges();
                        result.Add(_subject.Id);
                    }
                }
                else
                {
                    result = _context.subjects.Select(x => x.Id).ToList();
                }
            }
            return result;
        }

        private double getDoubleOrDefault(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;
            return double.Parse(input);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var _context = new MyDbContext())
                {
                    string? selectedYear = (cboYear.SelectedItem as ComboBoxItem)?.Content.ToString();
                    _context.schoolYear.FirstOrDefault(sy => sy.ExamYear == getIntegerOrDefault(selectedYear));
                    _context.Database.ExecuteSqlRaw("DELETE FROM dbo.Score");
                    _context.Database.ExecuteSqlRaw("DELETE FROM dbo.Student");
                }
                MessageBox.Show("Clear Done");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBrowser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    Filter = "CSV Files (*.csv)|*.csv|Excel Files (*.xls)|*.xls|All Files (*.*)|*.*"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    txtPath.Text = openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}