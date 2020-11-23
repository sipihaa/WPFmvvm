using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPFmvvm.Infrastructure.Commands;
using WPFmvvm.Models;
using WPFmvvm.Models.Decant;
using WPFmvvm.ViewModels.Base;

namespace WPFmvvm.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        /*------------------------------------------------------------------------*/

        public ObservableCollection<Group> Groups { get; }

        public object[] CompositeCollection { get; }

        #region Выбранный элемент массива

        private object _SelectedCompositeValue;

        public object SelectedCompositeValue { get => _SelectedCompositeValue; set => Set(ref _SelectedCompositeValue, value); }

        #endregion

        #region Выбранная группа

        private Group _SelectedGroup;

        public Group SelectedGroup
        {
            get => _SelectedGroup;
            set => Set(ref _SelectedGroup, value);
        }

        #endregion

        #region Номер выбранной вкладки

        private int _SelectedPageIndex;

        public int SelectedPageIndex { get => _SelectedPageIndex; set => Set(ref _SelectedPageIndex, value); }

        #endregion

        #region График

        private IEnumerable<DataPoint> _DataPoints;

        public IEnumerable<DataPoint> DataPoints { get => _DataPoints; set => Set(ref _DataPoints, value); }

        #endregion

        #region Заголовок окна

        private string _Title = "Лучший заголовок";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region ProgressBar

        private double _ProgressBar;

        public double ProgressBar
        {
            get => _ProgressBar;
            set => Set(ref _ProgressBar, value);
        }

        #endregion

        /*------------------------------------------------------------------------*/

        #region Команды

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        public void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region ChangeTabIndexCommand

        public ICommand ChangeTabIndexCommand { get; }

        private bool CanChangeTabIndexCommand(object p) => _SelectedPageIndex >= 0 || _SelectedPageIndex <= 4;

        private void OnChangeTabIndexCommand(object p)
        {
            if (p is null) return;
            SelectedPageIndex += Convert.ToInt32(p);
            if (SelectedPageIndex == -1) SelectedPageIndex = 0;
            if (SelectedPageIndex == 5) SelectedPageIndex = 4;
        }

        #endregion

        #region CreateGroupCommand

        public ICommand CreateGroupCommand { get; }

        private bool CanCreateGroupCommandExecute(object p) => true;

        public void OnCreateGroupCommandExecuted(object p)
        {
            var group_max_index = Groups.Count + 1;
            var new_group = new Group
            {
                Name = $"Группа {group_max_index}",
                Students = new ObservableCollection<Student>()
            };

            Groups.Add(new_group);
            SelectedGroup = new_group;
        }

        #endregion

        #region DeleteGroupCommand

        public ICommand DeleteGroupCommand { get; }

        private bool CanDeleteGroupCommandExecute(object p) => true;

        public void OnDeleteGroupCommandExecuted(object p)
        {
            if (!(p is Group group)) return;
            var group_index = Groups.IndexOf(group);
            Groups.Remove(group);
            if (group_index <= Groups.Count && group_index > 0)
                SelectedGroup = Groups[group_index - 1];
            else
                if(Groups.Count != 0) SelectedGroup = Groups[0];
        }

        #endregion

        #endregion

        /*------------------------------------------------------------------------*/

        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            ChangeTabIndexCommand = new LambdaCommand(OnChangeTabIndexCommand, CanChangeTabIndexCommand);
            CreateGroupCommand = new LambdaCommand(OnCreateGroupCommandExecuted, CanCreateGroupCommandExecute);
            DeleteGroupCommand = new LambdaCommand(OnDeleteGroupCommandExecuted, CanDeleteGroupCommandExecute);

            #endregion


            var data_points = new List<DataPoint>((int)(360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double to_rad = Math.PI / 180;
                var y = Math.Sin(x * to_rad);

                data_points.Add(new DataPoint { XValue = x, YValue = y });
            }

            DataPoints = data_points;

            var students = Enumerable.Range(1, 10).Select(i => new Student
            {
                Name = $"Name {i}",
                Surname = $"Surname {i}",
                Patronymic = $"Patronymic {i}",
                Birthday = DateTime.Now,
                Rating = 0,
                Description = $"Ученик {i}"
            });

            var groups = Enumerable.Range(1, 20).Select(i => new Group
            {
                Name = $"Группа {i}",
                Students = new ObservableCollection<Student>(students),
                Description = $"Группа {i}"
            });

            Groups = new ObservableCollection<Group>(groups);


            // ReSharper disable once UseObjectOrCollectionInitializer
            var data_list = new List<object>();

            data_list.Add("Hello world!");
            data_list.Add(42);
            var group = Groups[1];
            data_list.Add(group);
            data_list.Add(group.Students[0]);

            CompositeCollection = data_list.ToArray();
        }
    }
}
