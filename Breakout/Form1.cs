using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout
{
    public partial class Form1 : Form
    {
        //Global Variables
        bool goRight;
        bool goLeft;
        int speed = 10;

        int ballx = 5;
        int bally = 5;

        int score = 0;

        private Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();

            //Logic for the Controls
            foreach (Control x in this.Controls)
            {   
                //Logic for the colors of the blocks
                if (x is PictureBox && x.Tag == "block")
                {
                    Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    x.BackColor = randomColor;
                }
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            //If you press the left key and you're inside the panel, go left! This keeps the player in the form.
            if (e.KeyCode == Keys.Left && player.Left > 0)
            {
                goLeft = true;
            }

            //If you press the Right key and you're panel is less than the panel width, go right! This keeps the player in the form.
            if (e.KeyCode == Keys.Right && player.Right < 920)
            {
                goRight = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            //If the Left Key is not pressed, then its set to false.
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }

            //If the Right Key is not pressed, then its set to false.
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
        }

        //This will move the ball as soon as the Timer is ran.
        //The label is also now set up to take and display the score
        private void timer1_Tick(object sender, EventArgs e)
        {
            ball.Left += ballx;
            ball.Top += bally;

            label1.Text = "Score: " + score;

            //Here we check if goLeft and goRight is set to true, then we allow it to move in those directions.
            if (goLeft) { player.Left -= speed; }
            if (goRight) { player.Left += speed; }

            //Here we make sure the ball does not go off the screen. If the width is less than 1 or too close to the other side of the form, We set them to disable.
            if (player.Left <1)
            {
                goLeft = false;
            }
            else if (player.Left + player.Width > 920)
            {
                goRight = false;
            }

            //This checks the location of the ball compared to the game, if its too close it will bounce the ball to the other side. 
            if (ball.Left + ball.Width > ClientSize.Width || ball.Left < 0)
            {
                ballx = -ballx;
            }

            //This checks the top position and keeps the ball in game by reversing the bounce
            if (ball.Top < 0 || ball.Bounds.IntersectsWith(player.Bounds))
            {
                bally = -bally;
            }

            //This ends the game once the player has left the screen
            if (ball.Top + ball.Height > ClientSize.Height)
            {
                gameOver();
            }

            //This removes the blocks that have been hit and also increments the score
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "block")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        bally = -bally;
                        score++;
                    }
                }
            }

            //If you hit all 35 boxes, then show the Winners message.
            if (score >34)
            {
                gameOver();
                MessageBox.Show("You Win");
            }
        }

        //This function will stop the timer when the game ends.
        private void gameOver()
        {
            timer1.Stop();
        }
    }
}
