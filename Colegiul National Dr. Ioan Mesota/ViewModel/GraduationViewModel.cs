using Colegiul_National_Dr.Ioan_Mesota.Helper;
using Colegiul_National_Dr.Ioan_Mesota.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Colegiul_National_Dr.Ioan_Mesota.ViewModel
{
    public class GraduationViewModel: INotifyPropertyChanged
    {
        #region Commands 

        private ICommand _moveNextCommand;
        public ICommand MoveNextCommand {
            get {
                if(_moveNextCommand==null) {
                    _moveNextCommand=new RelayCommand(m => MoveNext(),m => CanMoveNext());
                }
                return _moveNextCommand;
            }
        }

        private ICommand _moveBackCommand;
        public ICommand MoveBackCommand {
            get {
                if(_moveBackCommand==null) {
                    _moveBackCommand=new RelayCommand(m => MoveBack(),m => CanMoveBack());
                }
                return _moveBackCommand;
            }
        }


        private ICommand _closeCommand;
        public ICommand CloseCommand {
            get {
                if(_closeCommand==null) {
                    _closeCommand=new RelayCommand(p => Close(),p => true);
                }
                return _closeCommand;
            }
        }

        #endregion

        #region Fields

        private bool? _dialogResult;
        private List<Graduation> _graduations;
        private Graduation _graduation;
        private string _fullTitle;
        private string _headOfPromotionDetails;

        #endregion

        #region Properties 

        public bool? DialogResult {
            get { return _dialogResult; }
            private set {
                _dialogResult=value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(DialogResult)));
            }
        }


        public Graduation Graduation {
            get { return _graduation; }
            set {
                _graduation=value;
                OnPropertyChanged("Graduation");
            }
        }

        public string FullTitle {
            get { return _fullTitle; }
            set {
                _fullTitle=value;
                OnPropertyChanged("FullTitle");
            }
        }

        public Student HeadOfPromotion { get; set; }

        public string HeadOfPromotionDetails {
            get { return _headOfPromotionDetails; }
            set {
                _headOfPromotionDetails=value;
                OnPropertyChanged("HeadOfPromotionDetails");
            }
        }

        #endregion

        #region Constructors 

        public GraduationViewModel(Graduation graduation,List<Graduation> graduations) {
            _graduations=graduations;
            Graduation=graduation;
            ProcessGraduation();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Moves to the next graduation
        /// </summary>
        private void MoveNext() {
            int position = _graduations.IndexOf(Graduation);
            Graduation=_graduations.ElementAt(position+1);
            ProcessGraduation();
        }

        /// <summary>
        /// Determines whether the next graduation can be reached.
        /// </summary>
        /// <returns></returns>
        private bool CanMoveNext() {
            return !(_graduations.IndexOf(Graduation)==_graduations.Count-1);
        }

        /// <summary>
        /// Moves to the previous graduation.
        /// </summary>
        private void MoveBack() {
            int position = _graduations.IndexOf(Graduation);
            Graduation=_graduations.ElementAt(position-1);
            ProcessGraduation();
        }

        /// <summary>
        /// Determines whether the previous graduation can be reached.
        /// </summary>
        /// <returns></returns>
        public bool CanMoveBack() {
            return _graduations.IndexOf(Graduation)!=0;
        }

        /// <summary>
        /// Closes the graduation window.
        /// </summary>
        public void Close() {
            DialogResult=true;
        }

        /// <summary>
        /// Updates UI information regarding the current graduation.
        /// </summary>
        private void ProcessGraduation() {
            FullTitle=string.Concat("Promotia ",Graduation.Title);
            HeadOfPromotion=Graduation.Students.Aggregate((first,second) => first.Marks>second.Marks ? first : second);
            HeadOfPromotionDetails=string.Concat("Sef de promotie: ",HeadOfPromotion.Name," ",HeadOfPromotion.Surname," cu media ",HeadOfPromotion.Marks);
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
