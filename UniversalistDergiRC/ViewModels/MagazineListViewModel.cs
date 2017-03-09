﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UniversalistDergiRC.Core;
using UniversalistDergiRC.DataAccess;
using UniversalistDergiRC.Model;
using UniversalistDergiRC.Repositories;
using Xamarin.Forms;

namespace UniversalistDergiRC.ViewModels
{
    public class MagazineListViewModel : BaseModel
    {
        private bool _isRefreshing;
        private ObservableCollection<MagazineSummaryModel> _magazineIssueList;
        private NavigationController _navigationController;
        private ICommand _openReadingPageCommand;
        private ICommand _refreshCommand;
        private MagazineSummaryModel selectedMagazine;

        public MagazineListViewModel(NavigationController controller)
        {
            _navigationController = controller;
            refreshMagazineIssueList(true);
        }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }

            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MagazineSummaryModel> MagazineIssueList
        {
            get
            {
                _magazineIssueList = _magazineIssueList ?? new ObservableCollection<MagazineSummaryModel>();
                return _magazineIssueList;
            }
            set
            {
                if (_magazineIssueList != value)
                {
                    _magazineIssueList = value;
                    OnPropertyChanged(() => MagazineIssueList);
                }
            }
        }

        public ICommand OpenReadingPageCommand
        {
            get
            {
                _openReadingPageCommand = _openReadingPageCommand ?? new Command(openSelectedMagazine);
                return _openReadingPageCommand;
            }
            set
            {
                if (_openReadingPageCommand != value)
                {
                    _openReadingPageCommand = value;
                    OnPropertyChanged(() => OpenReadingPageCommand);
                }
            }
        }

        private ICommand _openMasterPageCommand;
            public ICommand OpenMasterPageCommand
        {
            get
            {
                _openMasterPageCommand = _openMasterPageCommand ?? new Command(openMasterPage);
                return _openMasterPageCommand;
            }
            set
            {
                if (_openMasterPageCommand != value)
                {
                    _openMasterPageCommand = value;
                    OnPropertyChanged(() => OpenMasterPageCommand);
                }
            }
        }

        private void openMasterPage()
        {
            _navigationController.OpenMasterPage();
        }

        public ICommand RefreshCommand
        {
            get
            {
                _refreshCommand = _refreshCommand ?? new Command(refreshMagazineIssueListCommand);
                return _refreshCommand;
            }
            set
            {
                if (_refreshCommand != value)
                {
                    _refreshCommand = value;
                    OnPropertyChanged(() => RefreshCommand);
                }
            }
        }

        public MagazineSummaryModel SelectedMagazine
        {
            get
            {
                return selectedMagazine;
            }
            set
            {
                if (selectedMagazine != value)
                {
                    selectedMagazine = value;
                    openSelectedMagazine(selectedMagazine);
                    OnPropertyChanged(() => SelectedMagazine);
                }
            }
        }

        private void openSelectedMagazine(object obj)
        {
            MagazineSummaryModel selectedMagazine = obj as MagazineSummaryModel;
            if (SelectedMagazine == null)
                return;

            _navigationController.OpenReadingPage(selectedMagazine.Issue, Constants.FIRST_PAGE_NUMBER);

            SelectedMagazine = null;
        }

        private void refreshMagazineIssueList(bool tryLocal)
        {
            IsRefreshing = true;
            try
            {
                MagazineIssueList = DataAccessManager.GetMagazineIssues(tryLocal);
            }
            catch
            {
                MessagingCenter.Send(this, Constants.CONNECTION_ERROR_MESSAGEKEY);
            }
            IsRefreshing = false;
        }

        private void refreshMagazineIssueListCommand(object obj)
        {
            refreshMagazineIssueList(false);
        }
    }
}
