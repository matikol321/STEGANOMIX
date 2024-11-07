﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using STEGANOMIX.Model;
using STEGANOMIX.Services;

namespace STEGANOMIX.ViewModel
{
    public class MethodPolishViewModel : ViewModelBase
    {
        private IMethodService _service;

        private ICommand _openFileDialog1Command;
        private ICommand _openFileDialog2Command;
        private ICommand _encodeMessageCommand;
        private ICommand _decodeMessageCommand;
        private ICommand _downloadEncodedMessageCommand;
        private ICommand _downloadDecodedMessageCommand;

        private string _selectedFilePath1;
        private string _selectedFilePath2;
        private string _userMessage;
        private string _decodedMessage;
        private bool _downloadEncodedEnabled;
        private bool _downloadDecodedEnabled;

        private FileStream? _encodeFS;
        private FileStream? _decodeFS;

        public MethodPolishViewModel()
        {
            _openFileDialog1Command = new RelayCommand(x => OpenFileDialog1());
            _openFileDialog2Command = new RelayCommand(x => OpenFileDialog2());
            _encodeMessageCommand = new RelayCommand(x => EncodeMessage());
            _decodeMessageCommand = new RelayCommand(x => DecodeMessage());

            _selectedFilePath1 = "nie wgrano pliku";
            _selectedFilePath2 = "nie wgrano pliku";
            _downloadEncodedEnabled = false;
            _downloadDecodedEnabled = false;
        }

        private void OpenFileDialog1()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT Files (*.txt)|*.txt|PDF Files (*.pdf)|*.pdf";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                SelectedFilePath1 = filename;
            }
        }

        private void OpenFileDialog2()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT Files (*.txt)|*.txt|PDF Files (*.pdf)|*.pdf";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                SelectedFilePath2 = filename;
            }
        }


        private void EncodeMessage()
        {
            if (string.IsNullOrEmpty(SelectedFilePath1) || SelectedFilePath1.Equals("nie wgrano pliku"))
            {
                MessageBox.Show("Nie wgrano pliku");
                return;
            }
            if (string.IsNullOrEmpty(UserMessage))
            {
                MessageBox.Show("Nie wpisano wiadomości");
                return;
            }
            if (!File.Exists(SelectedFilePath1))
            {
                MessageBox.Show("Nie znaleziono pliku");
                return;
            }

            try
            {
                _encodeFS = new FileStream(SelectedFilePath1, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 0, false);
                if (_encodeFS == null)
                {
                    MessageBox.Show("Nie udało się uzyskać dostępu do pliku");
                    return;
                }

                _service = new LinkingWordsWithTemplateService(_encodeFS);
                var encodedMessage = _service.Encode();

                DownloadEncodedEnabled = true;
            }
            catch (Exception ex)
            {
                DownloadEncodedEnabled = false;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_encodeFS != null)
                {
                    _encodeFS.Close();
                    _encodeFS.Dispose();
                    _encodeFS = null;
                }
            }
        }

        private void DecodeMessage()
        {
            if (string.IsNullOrEmpty(SelectedFilePath2) || SelectedFilePath2.Equals("nie wgrano pliku"))
            {
                MessageBox.Show("Nie wgrano pliku");
                return;
            }
            if (!File.Exists(SelectedFilePath2))
            {
                MessageBox.Show("Nie znaleziono pliku");
                return;
            }

            try
            {
                _decodeFS = new FileStream(SelectedFilePath2, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 0, false);
                if (_decodeFS == null)
                {
                    MessageBox.Show("Nie udało się uzyskać dostępu do pliku");
                    return;
                }

                _service = new LinkingWordsWithTemplateService(_decodeFS);
                var encodedMessage = _service.Encode();

                DownloadDecodedEnabled = true;
            }
            catch (Exception ex)
            {
                DownloadDecodedEnabled = false;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_decodeFS != null)
                {
                    _decodeFS.Close();
                    _decodeFS.Dispose();
                    _decodeFS = null;
                }
            }
        }



        public bool DownloadDecodedEnabled
        {
            get
            {
                return _downloadDecodedEnabled;
            }
            set
            {
                _downloadDecodedEnabled = value;
                OnPropertyChanged(nameof(DownloadDecodedEnabled));
            }
        }
        public bool DownloadEncodedEnabled
        {
            get
            {
                return _downloadEncodedEnabled;
            }
            set
            {
                _downloadEncodedEnabled = value;
                OnPropertyChanged(nameof(DownloadEncodedEnabled));
            }
        }
        public string UserMessage
        {
            get
            {
                return _userMessage;
            }
            set
            {
                _userMessage = value;
                OnPropertyChanged(nameof(UserMessage));
            }
        }
        public string DecodedMessage
        {
            get
            {
                return _decodedMessage;
            }
            set
            {
                _decodedMessage = value;
                OnPropertyChanged(nameof(DecodedMessage));
            }
        }
        public string SelectedFilePath1
        {
            get
            {
                return _selectedFilePath1;
            }
            set
            {
                _selectedFilePath1 = value;
                OnPropertyChanged(nameof(SelectedFilePath1));
            }
        }
        public string SelectedFilePath2
        {
            get
            {
                return _selectedFilePath2;
            }
            set
            {
                _selectedFilePath2 = value;
                OnPropertyChanged(nameof(SelectedFilePath2));
            }
        }


        public ICommand OpenFileDialog1Command { get { return _openFileDialog1Command; } }
        public ICommand OpenFileDialog2Command { get { return _openFileDialog2Command; } }
        public ICommand EncodeMessageCommand { get { return _encodeMessageCommand; } }
        public ICommand DecodeMessageCommand { get { return _decodeMessageCommand; } }
        public ICommand DownloadEncodedMessageCommand { get { return _downloadEncodedMessageCommand; } }
        public ICommand DownloadDecodedMessageCommand { get { return _downloadDecodedMessageCommand; } }

    }
}
