using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Desktop_Line
{
    public partial class Form2 : Form
    {
        public string FilePath;
        public Form1 Main;
        public bool PlayTesting = false;
        public List<PathData> Path = new List<PathData>();
        public List<GameObject> Objects = new List<GameObject>();
        public GameObject CurrentlySelected;
        public float X;
        public float Speed;
        int RecordCounter = 0;
        int tick = 0;
        public int SelectedPath;
        public int BuildMode;
        public int SelectedTypw;
        public bool Grabbing = false;
        public string[] Typenames = {"bomba","kwadrat","speed trigger","windowResize","BgColorTrigger","pathColorTrigger","windowMove","FlipTrigger"};
        public Form2(string lvlName,bool load)
        {
            InitializeComponent();
            FilePath = lvlName;
            if(load == true)
            {
                LoadFile();
            }
            Objects.OrderBy(GameObject.ReturnX);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
        }

        public void Start()
        {
            if (op1updown.Value == 0)
            {
                op1updown.Value = 600;
            }
            if (o2updownm.Value == 0)
            {
                o2updownm.Value = 600;
            }
            if(o3updown.Value == 0)
            {
                o3updown.Value = 0;
            }
        }
        public void UpdateEditor()
        {
            Invalidate();
            label2.Text = Objects.Count+"";
            if (RecordCounter <= 0)
            {
                label1.Text = "";
            }
            else
            {
                label1.Text = RecordCounter + "";
                if (tick % 30 == 0)
                {
                    RecordCounter--;
                    if(RecordCounter == 0)
                    {
                        Main.StartRecording(FilePath + "\\save.lvl");
                    }
                }
            }
            if(Path.Count > 0)
            {
                hScrollBar1.Maximum = (int)Path[Path.Count - 1].X;
            }
            tick++;
            if (Path.Count - 1 >= SelectedPath)
            {
                label4.Text = SelectedPath + "/" + Path.Count + "\n X: " + Path[SelectedPath].X + " Y: " + Path[SelectedPath].Y;
            }
            if(CurrentlySelected is Trigger)
            {
                label9.Visible = true;
                numericUpDown6.Visible = true;
                numericUpDown7.Visible = true;
                numericUpDown8.Visible = true;
                numericUpDown9.Visible = true;
            }
            else
            {
                label9.Visible = false;
                numericUpDown6.Visible = false;
                numericUpDown7.Visible = false;
                numericUpDown8.Visible = false;
                numericUpDown9.Visible = false;
            }
            startposupdown.Maximum = Path.Count - 1;
        }
        public void UpdatePathData(List<PathData> pointData)
        {
            Path = pointData;
            return;
            label2.Text = "X: " + X + " ";
            for (int i = 0; i < Path.Count; i++)
            {
                label2.Text = label2.Text + " x" + Path[i].X + " y" + Path[i].Y + " w" + Path[i].Width;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Save();
            Main.GoBackFromEditor();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Main.GoBackFromEditor();
            Close();
        }
        public void LoadFile()
        {

            StreamReader sr = new StreamReader(FilePath + "\\save.lvl");
            while (true)
            {
                string line;
                string[] a;
                line = sr.ReadLine();
                if (line == null)
                {
                    break;
                }

                a = line.Split(' ');
                if (a[0] == "P")
                {
                    Path.Add(new PathData(float.Parse(a[1]), float.Parse(a[2]), float.Parse(a[3]), float.Parse(a[4]), float.Parse(a[5])));
                }else if (a[0] == "O")
                {
                    if (int.Parse(a[1]) == 1)
                    {
                        Objects.Add(new Bomb(float.Parse(a[2]), float.Parse(a[3])));
                        Objects[Objects.Count - 1].ScaleX = float.Parse(a[4]);
                        Objects[Objects.Count - 1].ScaleY = float.Parse(a[5]);
                    }
                    else if (int.Parse(a[1]) == 2)
                    {
                        Objects.Add(new Rectangle1(float.Parse(a[2]), float.Parse(a[3])));
                        Objects[Objects.Count - 1].ScaleX = float.Parse(a[4]);
                        Objects[Objects.Count - 1].ScaleY = float.Parse(a[5]);
                    }
                }
                else if (a[0] == "T")
                {
                    Objects.Add(new Trigger(float.Parse(a[2]), float.Parse(a[3])));
                    ((Trigger)Objects[Objects.Count - 1]).Type = int.Parse(a[1]);
                    ((Trigger)Objects[Objects.Count - 1]).Param1 = int.Parse(a[4]);
                    ((Trigger)Objects[Objects.Count - 1]).Param2 = int.Parse(a[5]);
                    ((Trigger)Objects[Objects.Count - 1]).Param3 = int.Parse(a[6]);
                    ((Trigger)Objects[Objects.Count - 1]).Param4 = int.Parse(a[7]);

                }else if (a[0] == "#")
                {
                    op1updown.Value = decimal.Parse(a[1]);
                    o2updownm.Value = decimal.Parse(a[2]);
                    o3updown.Value = decimal.Parse(a[3]);
                    o4.Value = decimal.Parse(a[4]);
                    o5.Value = decimal.Parse(a[5]);
                }


            }
            sr.Close();
        }
        public void Save()
        {
            Directory.CreateDirectory(FilePath);
            StreamWriter sw = new StreamWriter(FilePath + "\\save.lvl");
            sw.WriteLine("# " + op1updown.Value +" " + o2updownm.Value + " " + o3updown.Value + " " + o4.Value + " " + o5.Value);
            for (int i = 0;i < Path.Count;i++)
            {
                sw.WriteLine("P " + Path[i].X +" " + Path[i].Y +" " + Path[i].Width + " "+ Path[i].OffsetX + " " + Path[i].OffsetY);
            }
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i] is Trigger)
                {
                    sw.WriteLine("T " + Objects[i].Type +" "+ Objects[i].X + " " + Objects[i].Y + " " + ((Trigger)Objects[i]).Param1+ " " + ((Trigger)Objects[i]).Param2 + " " + ((Trigger)Objects[i]).Param3 + " " + ((Trigger)Objects[i]).Param4);
                }
                else
                {
                    sw.WriteLine("O " + Objects[i].Type+" " + Objects[i].X + " " + Objects[i].Y + " " + Objects[i].ScaleX + " " + Objects[i].ScaleY);
                }

            }
            sw.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(PlayTesting == false)
            {
                if(Main.GameController == null)
                {
                    Save();
                    GameController.StartPos = (int)startposupdown.Value;
                    GameController.ignoreDmg = checkBox1.Checked;
                    Main.PlayTest(FilePath + "\\save.lvl");
                }
                button4.Text = "Stop";
                PlayTesting = true;
            }
            else
            {
                if (Main.GameController != null)
                {
                    Main.ForceKill();
                }

                button4.Text = "Playtest";
                PlayTesting = false;
            }

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Editor = null;
        }
   
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            X = hScrollBar1.Value;
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {

            if (Path.Count > 1)
            {

                for (int i = 0; i < Path.Count - 1; i++)
                {
                    if ((((Path[i].X + Path[i].OffsetX) - X < 0) && (Path[i+1].X + Path[i+1].OffsetX) - X < 0) || (((Path[i].X + Path[i].OffsetX) - X > Width) && (Path[i + 1].X + Path[i + 1].OffsetX) - X > Width))
                    {
                        continue;
                    }
                    PointF[] pointTMP = new PointF[4];
                    pointTMP[0] = new PointF((Path[i].X + Path[i].OffsetX) - X, (Path[i].Y+ Path[i].OffsetY) - Path[i].Width / 2);
                    pointTMP[1] = new PointF((Path[i].X + Path[i].OffsetX) - X, (Path[i].Y + Path[i].OffsetY) + Path[i].Width / 2);
                    pointTMP[2] = new PointF((Path[i+1].X + Path[i+1].OffsetX) - X, (Path[i+1].Y + Path[i+1].OffsetY) + Path[i+1].Width / 2);
                    pointTMP[3] = new PointF((Path[i + 1].X + Path[i + 1].OffsetX) - X, (Path[i + 1].Y + Path[i + 1].OffsetY) - Path[i+1].Width / 2);

                    e.Graphics.FillPolygon(Brushes.White, pointTMP);

                }

            }
            foreach (var item in Objects)
            {
                item.Render(e, X);
            }
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Save();
            RecordCounter = 3;
            tick = 1;
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            label3.Text = "x: " + (e.X + X) + "  y:" + e.Y;
            if(Grabbing == true)
            {
                CurrentlySelected.X = e.X + X;
                CurrentlySelected.Y = e.Y;
            }
        }

        private void sciezka_Click(object sender, EventArgs e)
        {
            sciezkabox.Visible = true;
            //triggerbox.Visible = false;
            obiektbox.Visible = false;
            BuildMode = 0;
        }

        private void obiekty_Click(object sender, EventArgs e)
        {
            sciezkabox.Visible = false;
           // triggerbox.Visible = false;
            obiektbox.Visible = true;
            BuildMode = 1;
        }

        private void triggery_Click(object sender, EventArgs e)
        {
            sciezkabox.Visible = false;
            //triggerbox.Visible = true;
            obiektbox.Visible = false;
            BuildMode = 2;
        }

        private void arr1_Click(object sender, EventArgs e)
        {
            SelectedPath--;
            if (SelectedPath < 1)
            {
                SelectedPath = 0;
            }
            arrload();
        }

        private void arr2_Click(object sender, EventArgs e)
        {
            SelectedPath++;
            if (SelectedPath > Path.Count)
            {
                SelectedPath = Path.Count - 1;
            }
            arrload();

        }
        void arrload()
        {
            if (Path.Count - 1 >= SelectedPath)
            {
                numericUpDown1.Value = (decimal)Path[SelectedPath].Width;
                numericUpDown2.Value = (decimal)Path[SelectedPath].OffsetX;
                numericUpDown3.Value = (decimal)Path[SelectedPath].OffsetY;
                X = Path[SelectedPath].X + Path[SelectedPath].OffsetX - Width / 2;
            }

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (Path.Count - 1 >= SelectedPath)
            {
                Path[SelectedPath].Width = (float)numericUpDown1.Value;
            }

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if(Path.Count - 1 >= SelectedPath)
            {
                Path[SelectedPath].OffsetX = (float)numericUpDown2.Value;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (Path.Count - 1 >= SelectedPath)
            {
                Path[SelectedPath].OffsetY = (float)numericUpDown3.Value;
            }
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (BuildMode == 1)
            {
                bool a = false;
                for (int i = 0; i < Objects.Count; i++)
                {
                    if (Math.Abs(e.X + X - Objects[i].X) < Objects[i].ScaleX && Math.Abs(e.Y - Objects[i].Y) < Objects[i].ScaleY)
                    {
                        CurrentlySelected = Objects[i];
                        Grabbing = true;
                        label7.Text = Objects[i].X + " " + Objects[i].Y;
                        numericUpDown4.Value = (decimal)CurrentlySelected.ScaleX;
                        numericUpDown5.Value = (decimal)CurrentlySelected.ScaleY;
                        if(CurrentlySelected is Trigger)
                        {
                            numericUpDown6.Value = ((Trigger)CurrentlySelected).Param1;
                            numericUpDown7.Value = ((Trigger)CurrentlySelected).Param2;
                            numericUpDown8.Value = ((Trigger)CurrentlySelected).Param3;
                            numericUpDown9.Value = ((Trigger)CurrentlySelected).Param4;
                            switch (((Trigger)CurrentlySelected).Type)
                            {
                                default:
                                    break;
                                case 0:
                                    label9.Text = "speed Trigger: \n predkosc(dom 10): ";
                                    break;
                                case 1:
                                    label9.Text = "Window Size: \n Szerokosc:\n\nWysokosc:\n\nStep: ";
                                    break;
                                case 2:
                                    label9.Text = "kolor przeszkod: \n r:\n\ng:\n\nb: \n\nstep: ";
                                    break;
                                case 3:
                                    label9.Text = "kolor sciezki: \n r:\n\ng:\n\nb: \n\nstep: ";
                                    break;
                                case 4:
                                    label9.Text = "pozycja okna: \n X:\n\nY:\n\nstep:";
                                    break;
                                case 5:
                                    label9.Text = "Flip";
                                    break;
                            }
                        }
                        
 
                        a = true;
                        return;
                    }

                }
                if(a == false)
                {
                    if (SelectedTypw == 0)
                    {
                        Objects.Add(new Bomb((e.X + X), e.Y));
                    }
                    else if (SelectedTypw == 1)
                    {
                        Objects.Add(new Rectangle1(e.X + X, e.Y));
                    }
                    else if (SelectedTypw == 2)
                    {
                        Objects.Add(new Trigger(e.X + X, e.Y));
                    }
                    else if (SelectedTypw == 3)
                    {
                        Objects.Add(new Trigger(e.X + X, e.Y));
                        Objects[Objects.Count - 1].Type = 1;
                    }
                    else if (SelectedTypw == 4)
                    {
                        Objects.Add(new Trigger(e.X + X, e.Y));
                        Objects[Objects.Count - 1].Type = 2;
                    }
                    else if (SelectedTypw == 5)
                    {
                        Objects.Add(new Trigger(e.X + X, e.Y));
                        Objects[Objects.Count - 1].Type = 3;
                    }
                    else if (SelectedTypw == 6)
                    {
                        Objects.Add(new Trigger(e.X + X, e.Y));
                        Objects[Objects.Count - 1].Type = 4;
                    }
                    else if (SelectedTypw == 7)
                    {
                        Objects.Add(new Trigger(e.X + X, e.Y));
                        Objects[Objects.Count - 1].Type = 5;
                    }
                    CurrentlySelected = Objects[Objects.Count - 1];
                }

            }
            

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            
        }
        public void Apply()
        {
            if (CurrentlySelected != null)
            {
                CurrentlySelected.Scale = (float)numericUpDown4.Value;
                CurrentlySelected.ScaleX = (float)numericUpDown4.Value;
                CurrentlySelected.ScaleY = (float)numericUpDown5.Value;
                if(CurrentlySelected is Trigger)
                {
                    ((Trigger)CurrentlySelected).Param1 = (int)numericUpDown6.Value;
                    ((Trigger)CurrentlySelected).Param2 = (int)numericUpDown7.Value;
                    ((Trigger)CurrentlySelected).Param3 = (int)numericUpDown8.Value;
                    ((Trigger)CurrentlySelected).Param4 = (int)numericUpDown9.Value;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void arr3_Click(object sender, EventArgs e)
        {
            SelectedTypw--;
            if (SelectedTypw < 1)
            {
                SelectedTypw = 0;
            }
            label6.Text = Typenames[SelectedTypw];
        }

        private void arr4_Click(object sender, EventArgs e)
        {
            SelectedTypw++;
            if (SelectedTypw >= Typenames.Length)
            {
                SelectedTypw = Typenames.Length - 1;
            }
            label6.Text = Typenames[SelectedTypw];
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            Grabbing = false;
        }

        private void del_Click(object sender, EventArgs e)
        {
            if(CurrentlySelected != null)
            {
                Objects.Remove(CurrentlySelected);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(FilePath))
            {
                System.Diagnostics.Process.Start(FilePath);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Path.Insert(SelectedPath + 1,new PathData(Path[SelectedPath].X + 50, Path[SelectedPath].Y + 50,200,0,0));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Path.RemoveAt(SelectedPath);
        }

        private void opocjebut_Click(object sender, EventArgs e)
        {
            if (groupBox1.Visible == false)
            {
                groupBox1.Visible = true;
            }
            else{
                groupBox1.Visible=false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
