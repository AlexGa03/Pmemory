using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Pmemory
{
    
    //Class
    public partial class Form1 : Form
    {
 
        //Properties
        Image[] image;
        Button[] bt;
        Random random= new Random();
        List<int> randomList;
        List<String> CorrectButton;
        //-------------------
        int buttonNumber = 0;//0, 1(button1) 2(button2)
        Button BT1, BT2;
        String BT1Name;
        String BT2Name;
        int index1, index2;
        int Nmax = 16;
        SoundPlayer player;
        int seconds;
        int minutes;
        string time;
        string secondsStr;
        string minutesStr;
        int guessed;
        int errors;
        bool buttonWrong;
        int points;
       
        //Builder
        public Form1()
        {
            InitializeComponent();
            initConfig();
            System.Diagnostics.Debug.WriteLine("This is a log");
        }
        private void initConfig() 
        {
            timer2.Enabled = false;
            timer1.Enabled = false;
            timer3.Enabled = true;
            timer4.Enabled = true;
            seconds = 0;
            minutes = 0;
            guessed = 0;
            errors= 0;
            points = 0;
            CorrectButton = new List<String>();
            generate_randomNumberNoDuplicate(0, Nmax);
            string name = "";
            image = new Image[]
            { 
                Properties.Resources._01A,
                Properties.Resources._02A,
                Properties.Resources._03A,
                Properties.Resources._04A,
                Properties.Resources._05A,
                Properties.Resources._06A,
                Properties.Resources._07A,
                Properties.Resources._08A,
                Properties.Resources._01B,
                Properties.Resources._02B,
                Properties.Resources._03B,
                Properties.Resources._04B,
                Properties.Resources._05B,
                Properties.Resources._06B,
                Properties.Resources._07B,
                Properties.Resources._08B



            };
            bt = new Button[] { 
            button1,button2,button3,button4,button5,button6,button7,button8,
            button9,button10,button11,button12,button13,button14,button15,button16
            };
            for(int  i=0;i<Nmax; i++)
            {
                name = "";
                if (i < 10) name = "0";
                name += i;
                if (i < Nmax / 2) name += "A";
                else name += "B";
                bt[i].Name = name;
                bt[i].Image = Properties.Resources._00;
                resizeImg(bt[i]);
                
            }

            player = new SoundPlayer();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            
            
                BT1.Image = Properties.Resources._00;
                resizeImg(BT1);
                BT2.Image = Properties.Resources._00;
                resizeImg(BT2);
                timer1.Enabled = false;
                sound("2");
                bt = new Button[] {
                button1,button2,button3,button4,button5,button6,button7,button8,
                button9,button10,button11,button12,button13,button14,button15,button16
                };
                    for (int i = 0; i < Nmax; i++)
                    {
                        bt[i].Enabled = true;



                    }

            label8.Text = "Score:" + points.ToString();



        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            sound("1");
            bt = new Button[] {
            button1,button2,button3,button4,button5,button6,button7,button8,
            button9,button10,button11,button12,button13,button14,button15,button16
            };
            for (int i = 0; i < Nmax; i++)
            {
                 bt[i].Enabled = true;



            }
            label8.Text = "Score:" + points.ToString();
        }
        
        private void timer3_Tick(object sender, EventArgs e)
        {
            
            label3.Text = DateTime.Now.ToString("dddd , MMM dd yyyy,hh:mm:ss");
        }
        
        private void timer4_Tick(object sender, EventArgs e)
        {
            
            
            //Timer
            seconds++;
            if(seconds == 60)
            {
                minutes++;
                seconds = 0;
            }
            if(seconds < 10) secondsStr = "0"+seconds.ToString();   
            else secondsStr = seconds.ToString();
            if (minutes < 10) minutesStr = "0" + minutes.ToString();
            else minutesStr = minutes.ToString();
            time = "Time: "+ minutesStr + ":" + secondsStr;
            label2.Text = time;
            label4.Text = "Hits:" + guessed;
            label5.Text = "Fails:" + errors;
            if (guessed == Nmax / 2)
            {
                label2.ForeColor = Color.Green;
                label4.ForeColor = Color.Green; 
                label5.ForeColor = Color.Red;
                label7.Text = "Winner";
                panel1.BackColor= Color.Black;
                timer4.Enabled = false;
            }
        }


        //Methods
        private void ButtonClick(object sender, EventArgs e)
        {
            buttonWrong = false;
            
            Button button = (Button)sender;
            string name = button.Name;
            name = name.Substring(0,name.Length - 1);
            int nImage = randomList[Convert.ToInt32(name)];
            button.Image = image[nImage];
            //Resize image
            resizeImg(button);
            //----------------------------------------------
            
            //check if the button has already been guessed
            for (int i = 0; i < CorrectButton.Count() && !buttonWrong; i++)
            {
                if (name == CorrectButton[i])
                {
                    buttonWrong = true;
                    
                    
                }
            }
            if (buttonWrong) sound("4");
            else sound("0");


            if (guessed < Nmax/2 )
            {
                buttonNumber++;
                if (buttonNumber == 1)
                {
                    if(buttonWrong)
                    {
                        buttonNumber--;
                    }
                    else
                    {
                        BT1 = button;
                        BT1Name = name;
                        index1 = nImage;
                    }
                    
                }
                else if (buttonNumber == 2)
                {

                    index2 = nImage;
                    if (index2 == index1 || buttonWrong) buttonNumber--;
                    else
                    {
                        BT2 = button;
                        BT2Name = name;
                        bt = new Button[] {
                            button1,button2,button3,button4,button5,button6,button7,button8,
                            button9,button10,button11,button12,button13,button14,button15,button16
                        };
                        for (int i = 0; i < Nmax; i++)
                        {
                            bt[i].Enabled = false;
                        }

                       
                        if (index1 % (Nmax / 2) != index2 % (Nmax / 2))
                        {
                            timer1.Enabled = true;
                            errors++;
                            if(points > 0) points -= 100;

                        }
                        else
                        {
                            CorrectButton.Add(BT1Name);
                            CorrectButton.Add(BT2Name);
                            timer2.Enabled = true;
                            guessed++;
                            points += 500;
                        }
                        buttonNumber = 0;
                    }
                }
            }
            

        }
        private void resizeImg(Button frame)
        {
            Bitmap resized = new Bitmap(frame.Image, new Size(40, 40));
            frame.Image = resized;
            frame.ImageAlign = ContentAlignment.MiddleCenter;
        }

        

        private void generate_randomNumberNoDuplicate(int Nmin, int Nmax)
        {
            randomList = new List<int>();



            List<int> ordedList = Enumerable.Range(Nmin, Nmax).ToList();
            Nmin = 0;
            for (int i = 0; i < Nmax; i++)
            {
                int index = random.Next(0, ordedList.Count);
                randomList.Add(ordedList[index]);
                ordedList.RemoveAt(index);
            }





        
        }
        private void sound(string s)
        {
            player.SoundLocation = @"sound\" +s+".wav";
            player.Play();
        }
    }//CLASS END
}//NSPACE END
