using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace TicTacToeGame
{

public partial class Form1 : Form
    {
        Button[,] buttons;
        public Form1()
        {
            InitializeComponent();
                buttons = new Button[3, 3]{
                {button1 , button2 , button3},
                {button4 , button5 , button6},
                {button7 , button8 , button9}
            };
        }

        bool XwonOrTie;
        int PlayCounter = 0, clicks = 0;
        private async void button1_Click(object sender, EventArgs e) //the click event handler.
        {
            clicks++;
            Button button = (Button)sender;
            if (button.Text != "" || clicks == 2) return; //preventing player to click two times in a row, at same or different places.
            PlayCounter++;
            PlaySound("boing_spring.wav");
            button.Text = "X";
            XwonOrTie = true;
            CheckWinner();
            if (XwonOrTie) return;
            await Task.Delay(380);
            PlayIntellgent();
            clicks = 0;
            CheckWinner();
        }

        bool BlockOrWonSuccess = true;
        private void PlayIntellgent() //The computer turn.
        {
            PlaySound("boing_poing.wav");
            BlockOrWonSuccess = true;

            BlockOrWin("O");            //try to win
            if (BlockOrWonSuccess)      //if succeeded, return
                return;
            else                     //if not, then try to block
            {
                BlockOrWonSuccess = true;
                BlockOrWin("X");
                if (BlockOrWonSuccess) //if succeeded, return
                    return;
            }

            //couldn't win or block then play random.
            int row, col;
            Random random = new Random(); 
            do
            {
                row = random.Next(0, 3);
                col = random.Next(0, 3);
            }while ((buttons[row, col].Text == "X" || buttons[row, col].Text == "O") && PlayCounter < 5);

            if (PlayCounter < 5)
                buttons[row, col].Text = "O";
        }

        private void BlockOrWin(string state)//if X sent, try to block, if O, try to win.
        {
            int xCounter = 0;
            Button empty = null;
            for (int i = 0; i < 3; i++)
            {
                xCounter = 0; empty = null;
                for (int y = 0; y < 3; y++)
                {
                    if (buttons[i, y].Text == state) xCounter++;
                    else if (buttons[i, y].Text == "") empty = buttons[i, y];
                }
                if (xCounter == 2 && empty != null)
                {
                    empty.Text = "O";
                    return;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                xCounter = 0; empty = null;
                for (int y = 0; y < 3; y++)
                {
                    if (buttons[y, i].Text == state) xCounter++;
                    else if (buttons[y, i].Text == "") empty = buttons[y, i];
                }
                if (xCounter == 2 && empty != null)
                {
                    empty.Text = "O";
                    return;
                }
            }

            xCounter = 0;
            empty = null;

            if (buttons[0, 0].Text == state) xCounter++;
            else if (buttons[0, 0].Text == "") empty = buttons[0, 0];
            if (buttons[1, 1].Text == state) xCounter++;
            else if (buttons[1, 1].Text == "") empty = buttons[1, 1];
            if (buttons[2, 2].Text == state) xCounter++;
            else if (buttons[2, 2].Text == "") empty = buttons[2, 2];

            if (xCounter == 2 && empty != null)
            {
                empty.Text = "O";
                return;
            }

            xCounter = 0;
            empty = null;

            if (buttons[0, 2].Text == state) xCounter++;
            else if (buttons[0, 2].Text == "") empty = buttons[0, 2];
            if (buttons[1, 1].Text == state) xCounter++;
            else if (buttons[1, 1].Text == "") empty = buttons[1, 1];
            if (buttons[2, 0].Text == state) xCounter++;
            else if (buttons[2, 0].Text == "") empty = buttons[2, 0];

            if (xCounter == 2 && empty != null)
            {
                empty.Text = "O";
                return;
            }

            BlockOrWonSuccess = false;
        }

        private void CheckWinner() //checking for a winner.
        {
            for (int i = 0; i < 3; i++) //Checking vertical blocks to see if someone wins.
            {
                if (buttons[i, 0].Text != "")
                {
                    if (buttons[i, 0].Text.Equals(buttons[i, 1].Text) && buttons[i, 1].Text.Equals(buttons[i, 2].Text))
                    {
                        Paint_Green(buttons[i, 0], buttons[i, 1], buttons[i, 2]);
                        Set_Score(buttons[i, 0].Text);
                        ResetButtons();
                        return;
                    }
                }
            }

            for (int i = 0; i < 3; i++) //Checking horizontal blocks to see if someone wins.
            {
                if (buttons[0, i].Text != "")
                {
                    if (buttons[0, i].Text.Equals(buttons[1, i].Text) && buttons[1, i].Text.Equals(buttons[2, i].Text))
                    {
                        Paint_Green(buttons[0, i], buttons[1, i], buttons[2, i]);
                        Set_Score(buttons[0, i].Text);
                        ResetButtons();
                        return;
                    }
                }
            }

            if (buttons[1, 1].Text != "") //Checking at diagonals to see if someone wins.
            {
                if (buttons[0, 0].Text.Equals(buttons[1, 1].Text) && buttons[1, 1].Text.Equals(buttons[2, 2].Text))
                {
                    Paint_Green(buttons[0, 0], buttons[1, 1], buttons[2, 2]);
                    Set_Score(buttons[0, 0].Text);
                    ResetButtons();
                    return;
                }

                if (buttons[0, 2].Text.Equals(buttons[1, 1].Text) && buttons[1, 1].Text.Equals(buttons[2, 0].Text))
                {
                    Paint_Green(buttons[0, 2], buttons[1, 1], buttons[2, 0]);
                    Set_Score(buttons[0, 2].Text);
                    ResetButtons();
                    return;
                }
            }

            if (PlayCounter == 5)
            {
                Set_Score("t");
                ResetButtons();
                return;
            }

            XwonOrTie = false;
        }

        private void Paint_Green(Button block1, Button block2, Button block3) //painting winning blocks green.
        {
            block1.ForeColor = System.Drawing.Color.MediumSeaGreen;
            block2.ForeColor = System.Drawing.Color.MediumSeaGreen;
            block3.ForeColor = System.Drawing.Color.MediumSeaGreen;
        }

        private void ResetButtons() //reseting everything in case of win or tie.
        {
            clicks = 0;
            PlayCounter = 0;
            foreach (Button block in buttons)
            {
                block.ForeColor = System.Drawing.Color.Black;
                block.Text = "";
            }
        }

        int score_o = 0, score_x = 0, tie = 0;
        private void Set_Score(string winner) //setting the score in case of win or tie.
        {
            PlaySound("blip.wav");
       
            if (winner == "O")
            {
                score_o++;
                label6.Text = score_o.ToString();
                MessageBox.Show("The Computer Won!");
            }
            else if (winner == "X")
            {
                score_x++;
                label4.Text = score_x.ToString();
                MessageBox.Show("You won!");
            }
            else
            {
                tie++;
                label5.Text = tie.ToString();
                MessageBox.Show("Tie!");
            }
        }

        private void PlaySound(string path) //playing a .wav file at the given path. 
        {
            SoundPlayer simpleSound = new SoundPlayer(path);
            simpleSound.Play();
        }

    }
}
