using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.IO;
using System.Media;
using System.Diagnostics;

namespace Anna
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer s = new SpeechSynthesizer();
        SpeechRecognitionEngine sr = new SpeechRecognitionEngine();
        PromptBuilder pb = new PromptBuilder();


        public bool wake;

        public bool paused;

        public Form1()
        {
            InitializeComponent();
            Say("welcome");
        }

        private void btnSpeak_Click(object sender, EventArgs e)
        {
            s.SelectVoiceByHints(VoiceGender.Female);
            Choices list = new Choices();
            list.Add(File.ReadAllLines("commands.txt"));

            Grammar gm = new Grammar(new GrammarBuilder(list));

            try
            {
                sr.RequestRecognizerUpdate();
                sr.LoadGrammar(gm);
                sr.SpeechRecognized += Sr_SpeechRecognized;
                sr.SetInputToDefaultAudioDevice();
                sr.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                richTextBox1.Text += "Again";
                return;
            }

            pb.ClearContent();
            pb.AppendText(richTextBox1.Text);
            s.Speak(pb);
        }

        public void Say(string phrase)
        {
            s.SpeakAsync(phrase);
            wake = false;   
        }

        private void Sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speechSaid = e.Result.Text;

            if (speechSaid == "hey anna")
            {
                SoundPlayer player = new SoundPlayer(@"C:\Users\dani2\source\repos\Anna\Anna\bin\Debug\listening.wav");
                player.Play();
                wake = true;
            }

            if (wake == true)
            {
                paused = false;

                switch (speechSaid)
                {
                    case ("hello"):
                        Say("hi");
                        break;

                    case ("how are you doing"):
                        Say("good, how about you");
                        break;

                    case ("good"):
                        Say("i am glad to hear it");
                        break;

                    case ("open google"):
                        Say("opening google");
                        Process.Start("https://www.google.com");
                        break;

                    case ("exit"):
                        s.Speak("closing program, have a good day, sir");
                        SendKeys.Send("%{f4}");
                        break;

                    case ("open youtube"):
                        Say("opening youtube");
                        Process.Start("https://www.youtube.com");
                        break;

                    case ("close tab"):
                        Say("closing tab");
                        SendKeys.Send("^W");
                        break;

                    case ("new tab"):
                        Say("opening new tab");
                        SendKeys.Send("^t");
                        break;


                    //4*\t + enter
                    case ("searching"):
                        Say("searching in youtube");
                        SendKeys.Send("\t");
                        SendKeys.Send("\t");
                        SendKeys.Send("\t");
                        SendKeys.Send("\t");
                        SendKeys.Send("\t");
                        SendKeys.Send("\t");
                        SendKeys.Send("{ENTER}");
                        break;


                    #region play/pause
                    //case ("pause"):
                    //    IsPaused();
                    //    break;

                    //case ("play"):
                    //    IsPlayed();
                    //    break;
                    #endregion

                    case ("motherfucker"):
                        Say("i am sorry, sir");
                        break;


                    case ("open notepad"):
                        Say("opening notepad");
                        Process.Start("notepad.exe");
                        break;
                }
            }
        }

        private void IsPlayed()
        {
            if (paused == true)
            {
                paused = false;
                Say("playing video");
                SendKeys.Send("k");
            }
            else
            {
                Say("it's on");
            }
        }

        private void IsPaused()
        {
            if (paused == false)
            {
                paused = true;
                Say("pausing video");
                SendKeys.Send("k");
            }
            else
            {
                Say("it is already paused");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
