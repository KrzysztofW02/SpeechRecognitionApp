# Speech Recognition App

## Overview
This project is a simple speech recognition application built using C# and WPF. The application allows users to record audio, recognize speech using the Vosk library or load audio file, and save the recognized text to a file.

## Prerequisites
- .NET Framework
- Vosk API
- NAudio
- Newtonsoft.Json

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/SpeechRecognitionApp.git
2. Navigate to the project directory:
   ```bash
   cd SpeechRecognitionApp
3. Download the Vosk model and place it in the `models` directory. You can download models from [here](https://alphacephei.com/vosk/models).

4. Open the solution in Visual Studio and restore the NuGet packages.
## Usage
1. Run the application.
2. Click **START RECORDING** to begin recording your speech.
3. Click **STOP RECORDING** to end the recording.
4. Click **SAVE TO FILE** to save the recognized speech as a `.txt` file.
5. To load an audio file, click **LOAD AUDIO** and select a `.wav` file.

## Keyboard Shortcuts
- **Ctrl + R**: Start recording
- **Ctrl + S**: Save recognized text
- **Ctrl + E**: Stop recording
