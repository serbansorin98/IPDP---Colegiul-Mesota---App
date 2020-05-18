using Colegiul_National_Dr.Ioan_Mesota.Helper;
using Colegiul_National_Dr.Ioan_Mesota.Model;
using Colegiul_National_Dr.Ioan_Mesota.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Colegiul_National_Dr.Ioan_Mesota.ViewModel
{
    public class MainViewModel: INotifyPropertyChanged
    {
        #region Fields

        private ICommand _display;

        private Visibility _isVisible = Visibility.Visible;

        #endregion

        #region Constructors

        public MainViewModel() {
            LoadContent();
        }

        #endregion

        #region Properties

        public List<Graduation> Graduations { get; set; }

        public Visibility IsVisible {
            get { return _isVisible; }
            set {
                _isVisible=value;
                OnPropertyChanged("IsVisible");
            }
        }


        public ICommand Display {
            get {
                if(_display==null) {
                    _display=new RelayCommand(param => ShowGraduationDialog(param),param => true);
                }
                return _display;
            }
        }

        #endregion

        #region Methods

        private void ShowGraduationDialog(object selectedGraduation) {
            IsVisible=Visibility.Hidden;
            Graduation graduation = selectedGraduation as Graduation;
            GraduationWindow graduationWindow = new GraduationWindow();
            graduationWindow.Closed+=delegate { IsVisible=Visibility.Visible; };
            GraduationViewModel graduationViewModel = new GraduationViewModel(graduation,Graduations);
            graduationWindow.DataContext=graduationViewModel;
            graduationWindow.ShowDialog();
        }

        /// <summary>
        /// Loads the content
        /// </summary>
        private void LoadContent() {
            Graduations=new List<Graduation>();
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,@"..\..\Data"));
            string[] graduationFiles = Directory.GetFiles(path);
            for(int i = 0;i<graduationFiles.Length;i++) {
                using(StreamReader reader = new StreamReader(graduationFiles[i])) {
                    string line = reader.ReadLine();
                    List<Student> students = new List<Student>();
                    while(!reader.EndOfStream) {
                        line=reader.ReadLine();
                        string[] values = line.Split(',');
                        Student student = new Student() {
                            Name=values[0],
                            Surname=values[1],
                            Marks=(decimal.Parse(values[2]))/100
                        };
                        students.Add(student);
                    }
                    students=students.OrderBy(param => param.Name).ThenBy(param => param.Surname).ToList();
                    Graduations.Add(new Graduation() {
                        Students=students,
                        Title=Path.GetFileNameWithoutExtension(graduationFiles[i])
                    });
                }
            }
        }

        #endregion

        #region NotifyChanges

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}