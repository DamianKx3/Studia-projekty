using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
namespace Desktop_Line
{
    public partial class Form1 : Form
    {
        public int X = 0;
        public int Y = 0;
        public int SizeX = 0;
        public int SizeY = 0;
        public bool FlipGraphics = false;
        int Scene = 0;
        int MenuCard = 0;
        int ticks = 0;
        int sin = 0;
        public Button PlayButton;
        public Button EditorButton;
        public Button CNButton;
        public Button CustomizeButton;
        public GameController GameController;
        public Form2 Editor;
        public bool Recording = false;
        public List<PathData> ReturnRecording = new List<PathData>();
        public Label debug;
        public Form3 lvlSelector;
        public customize customize;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            timer1.Interval = 16;
            StartMenu();
            Directory.CreateDirectory(Application.StartupPath + "\\Levels");
            Directory.CreateDirectory(Application.StartupPath + "\\PreMade");
            DoubleBuffered = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ticks++;
            if (ticks % 5 == 0)
            {
                sin++;
            }
            
            Location = new Point(Screen.PrimaryScreen.Bounds.Width / 10 + X, Screen.PrimaryScreen.Bounds.Height / 10 + Y);
            Size = new Size(SizeX, SizeY);
            if (Scene == 0)
            {
                Y = (int)(Math.Sin(sin) * 2);
                if (MenuCard == 0)
                {
                    if ((ticks / 20) % 2 == 0)
                    {
                        MenuTitle.Text = "Desk Top_";
                    }
                    else
                    {
                        MenuTitle.Text = "Desk Top";
                    }
                }
            }
            if (Scene == 1)
            {
                if(GameController != null)
                {
                    GameController.Update();
                }

            }
            if (Editor != null)
            {
                if(Recording == true)
                {
                    Editor.UpdatePathData(ReturnRecording.ToList<PathData>());
                }
                Editor.UpdateEditor();
            }
            if(GameController == null)
            {
                FlipGraphics = false;
            }
            if(customize != null)
            {
                customize.Update1();
            }
            Invalidate();

        }
        public void StartMenu()
        {
            GameController.StartPos = 0;
            GameController.ignoreDmg = false;
            Size buttonSize = new Size(150, 30);
            SizeX = 600;
            SizeY = 600;
            Size = new Size(SizeX, SizeY);
            MenuTitle.Visible = true;
            label1.Visible = true;
            BackColor = Color.White;
            PlayButton = new Button();
            PlayButton.Text = "Graj";
            PlayButton.Location = new Point(Width / 2 - 100, Height / 3);
            Controls.Add(PlayButton);
            PlayButton.Click += StartClick;
            PlayButton.Size = buttonSize;

            //CustomizeButton = new Button();
            //CustomizeButton.Text = "Kreator strzalki";
            //CustomizeButton.Location = new Point(Width / 2 - 100, Height / 3 + 50);
            //Controls.Add(CustomizeButton);
            ////CustomizeButton.Click += Customize1;
            //CustomizeButton.Size = buttonsize;

            EditorButton = new Button();
            EditorButton.Text = "Edytor- wczytaj";
            EditorButton.Location = new Point(Width / 2 - 100, Height / 3 + 150);
            Controls.Add(EditorButton);
            EditorButton.Click += EditorClick;
            EditorButton.Size = buttonSize;

            CNButton = new Button();
            CNButton.Text = "Edytor- stworz nowy";
            CNButton.Location = new Point(Width / 2 - 100, Height / 3 + 200);
            Controls.Add(CNButton);
            CNButton.Click += CreateNew;
            Recording = false;
            ReturnRecording.Clear();
            CNButton.Size = buttonSize;
        }
        void StartClick(object sender, EventArgs e)
        {
            if(lvlSelector == null)
            { 
                lvlSelector = new Form3();
                lvlSelector.Main = this;

                lvlSelector.Show();
                lvlSelector.Location = new Point(800, 100);
            }
        }
        public void Closesel()
        {
            lvlSelector = null;
        }
        void EditorClick(object sender, EventArgs e)
        {
            if (Editor == null)
            {
                FolderBrowserDialog ofd = new  FolderBrowserDialog();
                string name = "";
                ofd.SelectedPath = Application.StartupPath + "\\levels\\";
                ofd.ShowDialog();
                name = ofd.SelectedPath;

                Editor = new Form2(name,true);
                Editor.Show();
                Editor.Main = this;
                Editor.Start();
                Scene = 0;
            }


        }
        
        void CreateNew(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }

        void Customize1(object sender, EventArgs e)
        {
            if(customize == null)
            {
                customize = new customize(this);
                customize.Show();
            }

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (GameController != null)
            {
                GameController.KeyDown(e);
            }

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if(FlipGraphics == false)
            {
                e.Graphics.ScaleTransform(1, 1);
                e.Graphics.TranslateTransform(0, 0);
            }
            else
            {
                e.Graphics.ScaleTransform(-1, 1);
                e.Graphics.TranslateTransform(-Width, 0);
            }
            if (GameController != null)
            {
                GameController.Paint(e);
            }

        }
        public void GoBackFromEditor()
        {
            Scene = 0;
            Editor = null;
            StartMenu();
        }
        public void PlayTest(string path)
        {
            Controls.Remove(EditorButton);
            EditorButton.Dispose();
            Controls.Remove(PlayButton);
            PlayButton.Dispose();
            Controls.Remove(CNButton);
            CNButton.Dispose();
            groupBox1.Visible=false;
            MenuTitle.Visible = false;
            label1.Visible = false;
            GameController = new GameController(Recording);
            GameController.FilePath = path;
            GameController.Main = this;
            GameController.Start();
            Scene = 1;
        }
        public void StartRecording(string path)
        {
            Recording = true;
            PlayTest(path);
        }
        public void StopRecording()
        {

        }
        public void ForceKill()
        {
            if(GameController.SoundPlayer != null)
            {
                GameController.SoundPlayer.Stop();
            }

            GameController = null;
            Scene = 0;
            StartMenu();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Editor == null)
            {
                Editor = new Form2(Application.StartupPath + "\\levels\\" + textBox1.Text,false);
                Editor.Show();
                Editor.Main = this;
                Editor.Start();
                Scene = 0;
            }
        }

        private void close1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }
    }
}
