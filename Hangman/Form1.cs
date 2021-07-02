using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Hangman
{
    public partial class Hangman : Form
    {
        string word1;
        int misses = 0;
        int wordlength = 0;
        int counter = 0;
        string usedLetters = "";
        int seconds = 0;
        int milliseconds = 0;
        List<Label> WordLettersLabels = new List<Label>();
        List<Label> HighscoreLabels = new List<Label>();
        Stopwatch stopwatch = new Stopwatch();

        public Hangman()
        {
            InitializeComponent();
            CreateHighScoreLabels();
        }

        private void CreateLetterLabels(int wordLength)
        {

            int horizontal = 20;
            int vertical = 310;
            for (int i = 0; i < wordLength; i++)
            {
                System.Windows.Forms.Label newLetterLabel = new System.Windows.Forms.Label
                {
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 30),
                    Name = "label1",
                    AutoSize = true,
                    Text = @"_",
                    Location = new System.Drawing.Point(horizontal, vertical)
                };
                horizontal += newLetterLabel.Width / 2;
                Controls.Add(newLetterLabel);
                WordLettersLabels.Add(newLetterLabel);
            }
            for (int i = 0; i < wordLength; i++)
            {
                if (word1[i] == ' ')
                {
                    WordLettersLabels[i].Text = " ";
                }
            } 
        }
        private void CreateHighScoreLabels()
        {

            int horizontal = 13;
            int vertical = 151;
            for (int i = 0; i < 10; i++)
            {
                System.Windows.Forms.Label newLetterLabel = new System.Windows.Forms.Label
                {
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 12),
                    Name = "label10",
                    AutoSize = true,
                    Text = @"_",
                    Location = new System.Drawing.Point(horizontal, vertical),
                    Visible = false
                };
                vertical += 26;
                Controls.Add(newLetterLabel);
                HighscoreLabels.Add(newLetterLabel);
            }
        }
        private void get_word()
        {
            string fileName = @"countries_and_capitals.txt";
            string[] lines = File.ReadAllLines(fileName);
            int ile_slow = lines.Length;
            Random gen = new Random();
            int indeks_slowa = gen.Next(0, ile_slow);
            string getLine = lines[indeks_slowa];
            string[] line_spaced = getLine.Split('|');
            label2.Text = $"Hint: The capital of {line_spaced[0]}";
            word1 = line_spaced[1];
            word1 = word1.ToLower();
            word1 = word1.TrimStart();
            wordlength = word1.Length;
            CreateLetterLabels(wordlength);
        }
        private void button1_Click(object sender, EventArgs e) //new game button
        {

            var lines2 = System.IO.File.ReadLines("highscores.txt");
            var linesOrderedByTime = lines2.OrderBy((p) => p.Split('|')[2]).ToList();
            File.WriteAllText(@"highscores.txt", String.Empty);


            for (int i = 0; i < 10; i++)

            {
                using (StreamWriter writer = new StreamWriter(@"highscores.txt", true))
                {
                    writer.WriteLine(linesOrderedByTime[i]);
                }
            }

            for (int l = 0; l < 10; l++)
            {
                HighscoreLabels[l].Visible = false;
            }

            misses = 0;
            counter = 0;
            wordlength = 0;
            seconds = 0;
            milliseconds = 0;
            usedLetters = "";
            WordLettersLabels.Clear();
            label1.Visible = true;
            pictureBox1.Visible = true;
            label3.Visible = true;
            button5.Visible = true;
            button6.Visible = true;
            button1.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            textBox3.Visible = false;
            button2.Visible = false;
            misses = 0;
            pictureBox1.Image = HangmanGame.Properties.Resources.hang0;
            label1.Text = $"not in word: {usedLetters}";
            usedLetters = "";
            stopwatch.Restart();
            stopwatch.Start();
            get_word();
           
        }
        private void button2_Click(object sender, EventArgs e) // highscore add button
        {
            DateTime localDate = DateTime.Now;
            using (StreamWriter writer = new StreamWriter(@"highscores.txt", true))
            {
                writer.WriteLine($"{textBox3.Text} | {localDate} | {seconds}:{milliseconds} | {counter} | {word1} ");
            }
            label9.Visible = false;
            textBox3.Visible = false;
            button2.Visible = false;
            label10.Visible = true;
        }
        private void button3_Click(object sender, EventArgs e) // letter guess
        {
            string letter = textBox1.Text;
            letter = letter.ToLower();
            bool is_guessed = false;
            for (int i = 0; i < wordlength; i++)
            {
                if (Convert.ToString(word1[i]) == letter)
                {
                    is_guessed = true;
                    WordLettersLabels[i].Text = Convert.ToString(word1[i]);
                }
            }
            if (is_guessed == false)
            {
                if (!usedLetters.Contains(letter))
                {
                usedLetters += $"{letter},";
                misses = misses + 1;
                lifeLose();
                label1.Text = $"not in word: {usedLetters}";
                }
            }
            if (misses < 6)
            {
                user_ask();
            }
            counter += 1;
            win();


        }
        private void button4_Click(object sender, EventArgs e) //word guess
        {
            string word = textBox2.Text;
            word = word.ToLower();
            if (word == word1)
            {
                for (int i = 0; i < wordlength; i++)
                {
                    WordLettersLabels[i].Text = Convert.ToString(word1[i]);
                }
            }
            else
            {
                misses = misses + 2;
                lifeLose();
            }
            if (misses < 6)
            {
               user_ask();
            }
            win();
            
        }

        private void button5_Click(object sender, EventArgs e) // letter to guess
        {
            button3.Visible = true;
            textBox1.Visible = true;
            button5.Visible = false;
            button6.Visible = false;

        }
        private void button6_Click(object sender, EventArgs e) // word to guess
        {
            button5.Visible = false;
            button6.Visible = false;
            button4.Visible = true;
            textBox2.Visible = true;
        }

        private void win()
        {
            var lines2 = System.IO.File.ReadAllLines("highscores.txt");
            int x = 0;
            for (int i = 0; i < wordlength; i++)
            {
                if (WordLettersLabels[i].Text == Convert.ToString(word1[i]))
                {
                    x += 1;
                    if (x == wordlength)
                    {
                        button1.Location = new Point(632, 287);
                        label6.Visible = true;
                        label1.Visible = false;
                        label2.Visible = false;
                        pictureBox1.Visible = false;
                        label3.Visible = false;
                        button3.Visible = false;
                        button4.Visible = false;
                        button5.Visible = false;
                        button6.Visible = false;
                        button1.Visible = true;
                        label9.Visible = true;
                        textBox3.Visible = true;
                        button2.Visible = true;
                        textBox1.Visible = false;
                        textBox2.Visible = false;
                        label11.Visible = true;
                        TimeSpan ts = stopwatch.Elapsed;
                        stopwatch.Stop();
                        seconds = ts.Seconds;
                        milliseconds = ts.Milliseconds;

                        for (int a = 0; a < wordlength; a++)
                        {
                            WordLettersLabels[a].Visible = false;
                        }
                        for (int l = 0; l < 10; l++)
                        {
                            HighscoreLabels[l].Text = $"{l+1}. {Convert.ToString(lines2[l])}";
                            HighscoreLabels[l].Visible = true;
                        }
                        label7.Text = $"You guessed the capital afer {counter} letters. It took you {seconds}:{milliseconds} seconds";
                        label7.Visible = true;
                        label8.Text = $"The capital was: {word1}";
                        label8.Visible = true;
                    }
                }
            }
        }

        private void lose()
        {
            var lines2 = System.IO.File.ReadAllLines("highscores.txt");
            for (int i = 0; i < wordlength; i++)
            {
                WordLettersLabels[i].Visible = false;
            }
            for (int l = 0; l < 10; l++)
            {
                HighscoreLabels[l].Text = $"{l + 1}. {Convert.ToString(lines2[l])}";
                HighscoreLabels[l].Visible = true;
            }
            button1.Location = new Point(632, 287);
            label1.Visible = false;
            label2.Visible = false;
            pictureBox1.Visible = false;
            label3.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            label5.Visible = true;
            button1.Visible = true;
            textBox1.Visible = false;
            textBox2.Visible = false;
            label8.Text = $"The capital was: {word1}";
            label8.Visible = true;
            label11.Visible = true;
        }

        private void lifeLose()
        {
            if (misses == 0) { pictureBox1.Image = HangmanGame.Properties.Resources.hang0; }
            if (misses == 1) { pictureBox1.Image = HangmanGame.Properties.Resources.hang1; }
            if (misses == 2) { pictureBox1.Image = HangmanGame.Properties.Resources.hang2; }
            if (misses == 3) { pictureBox1.Image = HangmanGame.Properties.Resources.hang3; }
            if (misses == 4) { pictureBox1.Image = HangmanGame.Properties.Resources.hang4; }
            if (misses == 5) { pictureBox1.Image = HangmanGame.Properties.Resources.hang5; label2.Visible = true; }
            if (misses >5) {  pictureBox1.Image = HangmanGame.Properties.Resources.hangLose; lose(); }
        }
        private void user_ask()
        {
            button3.Visible = false;
            textBox1.Visible = false;
            button5.Visible = true;
            button6.Visible = true;
            button4.Visible = false;
            textBox2.Visible = false;
        }

    }
}
