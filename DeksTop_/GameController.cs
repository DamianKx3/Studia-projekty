using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Media;

namespace Desktop_Line
{
    public class GameController
    {
        int ticks = 0;
        public Form1 Main;
        public string FilePath;
        public List<GameObject> GameObjects = new List<GameObject>();
        public List<PathData> Path = new List<PathData>();
        public float speed = 10;
        public int WinX = 0;
        public int WinY = 0;
        public int TargetWinX = 0;
        public int TargetWinY = 0;
        public int WinSizeX = 0;
        public int TargetWinSizeX = 0;
        public int WinSizeY = 0;
        public int TargetWinSizeY = 0;
        public int WindowScrollSpeed= 10;
        public bool FlipGraphics = false;
        public float X = 0;
        public Player Player1;
        public List<PathData> StartPoints = new List<PathData>();
        GameObject Meta;
        public PathData BeforePoint;
        public PathData AfterPoint;
        int PointStep =0;
        bool Recording;
        float HitBoxTop;
        float HitBoxBottom;
        bool Won = false;
        public SoundPlayer SoundPlayer;
        int[] startvalues = new int[10];
        public static int StartPos = 0;
        public int DeathTicks = 0;
        public bool DeathAnim = false;
        public Color ObstaclesColor = Color.Black;
        public Color PathColor = Color.White;
        public static bool ignoreDmg;
        public GameController(bool Record = false)
        {
            StartPoints.Clear();
            Recording = Record;
            if(Recording == true)
            {
                StartPoints.Add(new PathData(0, 0, 1000,0,0));
                Path.Clear();
            }

        }

        public void Start()
        {
            HitBoxTop = 0;
            HitBoxBottom = Main.Height;
            Player1 = new Player(0, 0);
            Player1.Controller = this;
            GameObjects.Add(Player1);
            WinSizeX = Main.Width;
            WinSizeY = Main.Height;
            LoadGame();
            
            if(Path.Count > 1 && Recording == false)
            {
                Meta = new Meta1(Path[Path.Count - 1].X, 0);
                GameObjects.Add(Meta);

            }
            Respawn();



        }
        public void Respawn()
        {
            string folder = System.IO.Path.GetDirectoryName(FilePath);
            string[] files = Directory.GetFiles(folder, "*.wav");
            if (files.Length > 0)
            {
                SoundPlayer = new SoundPlayer(files[0]);
                SoundPlayer.Play();
            }
            foreach(GameObject obj in GameObjects)
            {
                if (obj is Trigger)
                {
                    ((Trigger)obj).Triggered = false;
                }
            }


            Player1.Gravity = true;
            Player1.trail.Clear();
            FlipGraphics = false;
            TargetWinSizeX = startvalues[0];
            TargetWinSizeY = startvalues[1];
            speed = startvalues[2];
            WinX = startvalues[3];
            WinY = startvalues[4];
            Player1.X = 0;
            if(Path.Count > 0)
            {
                if(StartPos < 0)
                {
                    StartPos = 0;
                }
                Player1.Y = Path[StartPos].Y;
                X = (Path[StartPos].X + Path[StartPos].OffsetX) - 100;

            }
            else
            {
                Player1.Y = 0;
                X = 0;
            }


            ObstaclesColor = Color.Black;
            PathColor = Color.White;
            if(StartPos % 2 == 1)
            {
                Player1.Gravity = false;
            }

            ticks = 0;
            PointStep = 0;
            HitBoxTop = 0;
            HitBoxBottom = Main.Height;
            if (Path.Count > 1)
            {
                BeforePoint = Path[0];
                AfterPoint = Path[1];
            }
        }
        public void Update()
        {
            if(Player1.X < 200)
            {
                if(Math.Abs(Player1.X - 200) > 20)
                {
                    Player1.X = Player1.X + 20;
                }
                else
                {
                    Player1.X = 200;
                }

            }
            Main.X = WinX; 
            Main.Y = WinY;
            Main.SizeX = WinSizeX;
            Main.SizeY = WinSizeY;
            Player1.LowLimit = 0;
            Player1.HeighLimit = Main.Height - 50;
            Main.BackColor = ObstaclesColor;
            Main.FlipGraphics = FlipGraphics;
            foreach (var item in GameObjects)
            {
                item.Update();
                item.speed = speed;
            }
            X = X + speed;
            ticks++;
            if (Path.Count > 1)
            {
                if (X + Player1.X > AfterPoint.X)
                {
                    if (PointStep < Path.Count - 2)
                    {
                        PointStep++;
                        BeforePoint = Path[PointStep];
                        AfterPoint = Path[PointStep+1];
                    }
                    else
                    {
                        if(Recording == false)
                        {
                            Win();
                        }

                    }

                }
                if (Won == true && Recording == false)
                {
                    Player1.angle = 0;
                    Player1.X = Player1.X + 50;
                    if (Player1.X > Main.Width)
                    {
                        Won = false;
                        Main.ForceKill();
                    }
                }
                else
                {
                    PointF PlayerPos = new PointF(X + Player1.X,Player1.Y);

                    PointF BeforeUp = new PointF(BeforePoint.X + BeforePoint.OffsetX, BeforePoint.Y + BeforePoint.OffsetY + BeforePoint.Width / 2);
                    PointF BeforeDown = new PointF(BeforePoint.X + BeforePoint.OffsetX, BeforePoint.Y + BeforePoint.OffsetY - BeforePoint.Width / 2);

                    PointF AfterUp = new PointF(AfterPoint.X + AfterPoint.OffsetX, AfterPoint.Y + AfterPoint.OffsetY + AfterPoint.Width / 2);
                    PointF AfterDown = new PointF(AfterPoint.X + AfterPoint.OffsetX, AfterPoint.Y + AfterPoint.OffsetY - AfterPoint.Width / 2);

                    HitBoxBottom =  GetYHitbox(BeforeUp, AfterUp,PlayerPos);
                    HitBoxTop = GetYHitbox(BeforeDown, AfterDown,PlayerPos);
                    foreach (var item in GameObjects)
                    {
                        if (item.Hitbox(PlayerPos) == true && ignoreDmg == false)
                        {
                            Die();
                        }
                    }






                    if ((Player1.Y < HitBoxTop || Player1.Y > HitBoxBottom )&& Recording == false && ignoreDmg == false)
                    {
                        Die();

                    }
                }



            }
            if(TargetWinSizeX > WinSizeX)
            {
                if(Math.Abs(TargetWinSizeX - WinSizeX) > WindowScrollSpeed)
                {
                    WinSizeX = WinSizeX + WindowScrollSpeed;
                }
                else
                {
                    WinSizeX = TargetWinSizeX;
                }

            }
            if (TargetWinSizeX < WinSizeX)
            {
                if (Math.Abs(TargetWinSizeX - WinSizeX) > WindowScrollSpeed)
                {
                    WinSizeX = WinSizeX - WindowScrollSpeed;
                }
                else
                {
                    WinSizeX = TargetWinSizeX;
                }

            }
            if (TargetWinSizeY > WinSizeY)
            {
                if (Math.Abs(TargetWinSizeY - WinSizeY) > WindowScrollSpeed)
                {
                    WinSizeY = WinSizeY + WindowScrollSpeed;
                }
                else
                {
                    WinSizeX = TargetWinSizeX;
                }

            }
            if (TargetWinSizeY < WinSizeY)
            {
                if (Math.Abs(TargetWinSizeX - WinSizeY) > WindowScrollSpeed)
                {
                    WinSizeY = WinSizeY - WindowScrollSpeed;
                }
                else
                {
                    WinSizeY = TargetWinSizeY;
                }

            }



            if (TargetWinX > WinX)
            {
                if (Math.Abs(TargetWinX - WinX) > WindowScrollSpeed)
                {
                    WinX = WinX + WindowScrollSpeed;
                }
                else
                {
                    WinX = TargetWinX;
                }

            }
            if (TargetWinX < WinX)
            {
                if (Math.Abs(TargetWinX - WinX) > WindowScrollSpeed)
                {
                    WinX = WinX - WindowScrollSpeed;
                }
                else
                {
                    WinX = TargetWinX;
                }

            }
            if (TargetWinY > WinY)
            {
                if (Math.Abs(TargetWinY - WinY) > WindowScrollSpeed)
                {
                    WinY = WinY + WindowScrollSpeed;
                }
                else
                {
                    WinX = TargetWinX;
                }

            }
            if (TargetWinY < WinY)
            {
                if (Math.Abs(TargetWinX - WinY) > WindowScrollSpeed)
                {
                    WinY = WinY - WindowScrollSpeed;
                }
                else
                {
                    WinY = TargetWinY;
                }

            }
            if(Recording == false)
            {
                if (DeathAnim == true)
                {
                    DeathTicks++;
                    speed = 0;
                    if (DeathTicks > 60)
                    {
                        DeathTicks = 0;
                        DeathAnim = false;
                        Respawn();
                    }
                    Player1.Dead = true;
                }
                else
                {
                    Player1.Dead = false;
                }
            }


        }
        public float GetYHitbox(PointF p1, PointF p2, PointF player)
        {
            if (Math.Abs(p2.X - p1.X) < 0.0001f)
            {
                return p1.Y;
            }

            return p1.Y + (player.X - p1.X) / (p2.X - p1.X) * (p2.Y - p1.Y);
        }

        public void KeyDown(KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {

                Main.ForceKill();
                
            }
            if (e.KeyCode == Keys.Space)
            {
                StartPoints.Add(new PathData(Player1.X + X, Player1.Y,200,0,0));
                Main.ReturnRecording = StartPoints;
            }
            foreach (var item in GameObjects)
            {
                item.OnKeyPressed(e.KeyCode);
            }
        }
        public void TouchedCeil()
        {
            StartPoints.Add(new PathData(Player1.X + X, Player1.Y, 200,0,0));
        }
        public void Die()
        {

            DeathAnim = true;



        }
       
        public void Win()
        {
            speed = 0;
            Won = true;
        }
        public void Paint(PaintEventArgs e)
        {
            if (Path.Count > 1)
            {

                for (int i = 0; i < Path.Count - 1; i++)
                {
                    if (((Path[i].X - X < 0) && Path[i + 1].X - X < 0) || ((Path[i].X - X > Main.Width) && Path[i + 1].X - X > Main.Width))
                    {
                        continue;
                    }
                    PointF[] pointTMP = new PointF[4];
                    pointTMP[0] = new PointF((Path[i].X + Path[i].OffsetX) - X, (Path[i].Y + Path[i].OffsetY) - Path[i].Width / 2);
                    pointTMP[1] = new PointF((Path[i].X + Path[i].OffsetX) - X, (Path[i].Y + Path[i].OffsetY) + Path[i].Width / 2);
                    pointTMP[2] = new PointF((Path[i + 1].X + Path[i + 1].OffsetX) - X, (Path[i + 1].Y + Path[i + 1].OffsetY) + Path[i + 1].Width / 2);
                    pointTMP[3] = new PointF((Path[i + 1].X + Path[i + 1].OffsetX) - X, (Path[i + 1].Y + Path[i + 1].OffsetY) - Path[i + 1].Width / 2);
                    SolidBrush b = new SolidBrush(PathColor);
                    e.Graphics.FillPolygon(b, pointTMP);
                    b.Dispose();

                }



            }
            foreach (var item in GameObjects)
            {
                item.Color = ObstaclesColor;
                item.Render(e,X);

            }
            if(Won == true && Recording == false)
            {
                Player1.Won = true;
                Player1.Render(e, X);
            }

            
        }
        public void LoadGame()
        {
            StreamReader sr = new StreamReader(FilePath);
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
                }
                else if (a[0] == "O")
                {
                    if (int.Parse(a[1]) == 1)
                    {
                        GameObjects.Add(new Bomb(float.Parse(a[2]), float.Parse(a[3])));
                        GameObjects[GameObjects.Count - 1].ScaleX = float.Parse(a[4]);
                        GameObjects[GameObjects.Count - 1].ScaleY = float.Parse(a[5]);
                    }
                    else if (int.Parse(a[1]) == 2)
                    {
                        GameObjects.Add(new Rectangle1(float.Parse(a[2]), float.Parse(a[3])));
                        GameObjects[GameObjects.Count - 1].ScaleX = float.Parse(a[4]);
                        GameObjects[GameObjects.Count - 1].ScaleY = float.Parse(a[5]);
                    }
                }
                else if (a[0] == "T")
                {
                    GameObjects.Add(new Trigger(float.Parse(a[2]), float.Parse(a[3])));
                    ((Trigger)GameObjects[GameObjects.Count - 1]).Type = int.Parse(a[1]);
                    ((Trigger)GameObjects[GameObjects.Count - 1]).Param1 = int.Parse(a[4]);
                    ((Trigger)GameObjects[GameObjects.Count - 1]).Param2 = int.Parse(a[5]);
                    ((Trigger)GameObjects[GameObjects.Count - 1]).Param3 = int.Parse(a[6]);
                    ((Trigger)GameObjects[GameObjects.Count - 1]).Param4 = int.Parse(a[7]);
                    ((Trigger)GameObjects[GameObjects.Count - 1]).Controller = this;

                }
                else if (a[0] == "#")
                {
                    startvalues[0] = int.Parse(a[1]);
                    startvalues[1] = int.Parse(a[2]);
                    startvalues[2] = int.Parse(a[3]);
                    startvalues[3] = int.Parse(a[4]);
                    startvalues[4] = int.Parse(a[5]);
                }


            }
            sr.Close();
        }

    }
}
