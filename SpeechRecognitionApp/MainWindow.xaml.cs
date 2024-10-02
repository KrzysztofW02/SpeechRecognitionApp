using System;
using System.IO;
using System.Windows;
using Vosk;
using NAudio.Wave;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpeechRecognitionApp
{
    public partial class MainWindow : Window
    {
        private WaveInEvent waveIn;
        private MemoryStream memoryStream;
        private VoskRecognizer recognizer;
        private Model _model;
        private bool isRecording = false;

        public MainWindow()
        {
            InitializeComponent();
            _model = new Model(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\models", "vosk-model-en-us-0.22"));
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            this.Focusable = true;
            this.Focus();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;

            memoryStream = new MemoryStream();
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(16000, 1);
            recognizer = new VoskRecognizer(_model, 16000.0f);
            waveIn.BufferMilliseconds = 50;
            waveIn.DataAvailable += OnDataAvailable;

            isRecording = true;
            waveIn.StartRecording();

            StatusLabel.Content = "Recording started...";

            await Task.Run(() => RecognizeSpeechAsync());
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (isRecording)
            {
                recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                FileName = "Recognized_speech.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, ResultTextBox.Text);
                MessageBox.Show("Text saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopButton.IsEnabled = false;
            StartButton.IsEnabled = true;

            if (isRecording)
            {
                isRecording = false;
                waveIn.StopRecording();
                waveIn.Dispose();

                var finalResult = recognizer.FinalResult();
                dynamic jsonResult = JsonConvert.DeserializeObject(finalResult);

                if (!string.IsNullOrWhiteSpace((string)jsonResult.text))
                {
                    ResultTextBox.Text += $"{jsonResult.text}\n";
                }
                else
                {
                    ResultTextBox.Text += "No speech recognized.\n";
                }
                StatusLabel.Content = "Recording stopped.";
            }
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R && Keyboard.Modifiers == ModifierKeys.Control)
            {
                StartButton_Click(sender, e);
            }

            else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SaveButton_Click(sender, e);
            }

            else if (e.Key == Key.E && Keyboard.Modifiers == ModifierKeys.Control)
            {
                StopButton_Click(sender, e);
            }
        }

        private async Task RecognizeSpeechAsync()
        {
            while (isRecording)
            {
                await Task.Delay(50);
            }
        }
    }
}
