using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Net.Http;

namespace GuzzLaunhcer
{
    public partial class GuzzLauncher : Form
    {
        string appDir;
        string configFileDir;
        string downloadDir;

        //ARRAY SYSTEM
        public static List<GameBox> gameBoxList = new List<GameBox>();
        public static int imageSize = 182;
        public static int imageSpace = 36;
        public static int imageCount = 0;
        public static Point DefaultPoint = new Point(12, 12);
        //




        public GuzzLauncher()
        {
            InitializeComponent();
        }

        private void GuzzLauncher_Load(object sender, EventArgs e)
        {
            appDir = AppDomain.CurrentDomain.BaseDirectory;
            configFileDir = System.IO.Path.Combine(appDir, "config.txt");


            //FirstSetupCheck();
            StartGameBoxes();

        }
        private void FirstSetupCheck()
        {
            if (System.IO.File.Exists(configFileDir))
            {
                downloadDir = System.IO.File.ReadLines(configFileDir).First();
                //MessageBox.Show(downloadDir);
            }
            else
            {
                MessageBox.Show("Please select a download path");
                button1_Click(new object(), new EventArgs());
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog() { Description = "Select Download Path", RootFolder = Environment.SpecialFolder.Programs };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                downloadDir = folderBrowserDialog.SelectedPath;

                System.IO.File.WriteAllText(configFileDir, downloadDir);
                //MessageBox.Show("Seçilen Klasör: " + folderBrowserDialog.SelectedPath + " ConfigFile dir =  " + configFileDir);
                Clipboard.SetText(configFileDir);


                //Process.Start(new ProcessStartInfo(configFileDir) { UseShellExecute = true });
            }
        }

        /*
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        */

        private void StartGameBoxes()
        {
            CreateGameBox("31", DefaultPoint, Properties.Resources.artworks_000013535097_9rz0uo_t500x500);
            CreateGameBox("32fgdfdgfdgfdgdf", DefaultPoint, Properties.Resources.artworks_000013535097_9rz0uo_t500x500);

        }

        private void CreateGameBox(string name, Point Location, Image image)
        {
            GameBox gameBox = new GameBox(name, Location, image);
            //GameBox gameBox = new GameBox("Ahmet Kaya Ball 2", new Point(12, 12), Properties.Resources.artworks_000013535097_9rz0uo_t500x500);

            this.Controls.Add(gameBox.gamePictureBox);  // BU İKİSİ RENDERLANMASINI SAĞLIYOR
            this.Controls.Add(gameBox.gameText);
            gameBox.gameText.BringToFront();

            imageCount++;
            DefaultPoint = new Point(12 + ((imageCount * imageSize) + (imageSpace * imageCount)), 12);
            //sMessageBox.Show(imageCount.ToString());
            gameBoxList.Add(gameBox);
        }


        public static async Task DownloadGame(string downloadPath,string gameFolderName, string url = "https://github.com/oguzk234/AhmetKayaBall_Beta_V0.23")
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    using (FileStream fs = new FileStream(Path.Combine(downloadPath,gameFolderName), FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fs);
                    }

                    MessageBox.Show("INDIRME OLUMLU");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"INDIRMEDE Hata oluştu: {ex.Message}");
                }
            }
        }


    }
}


public class GameBox
{

    public string gameName;
    public Point gameLocation;
    public Image gameImage;

    public PictureBox gamePictureBox;// KENDI OLUSTURULACAK
    public Label gameText;// KENDI OLUSTURULACAK
    public Button gameButton;
    public enum GameStatus { NotDownloaded,UpdateNeeded,ReadyToPlay }
    public GameStatus gameStatus;

    public GameBox(string GameName, Point GameLocation, Image GameImage)
    {
        gameName = GameName; gameLocation = GameLocation; gameImage = GameImage;

        gameText = new Label();
        gamePictureBox = new PictureBox();
        gameButton = new Button();


        #region PictureBoxValues
        this.gamePictureBox.Image = this.gameImage;
        this.gamePictureBox.Location = this.gameLocation;

        //DEFAULT VALUES
        this.gamePictureBox.Size = new Size(global::GuzzLaunhcer.GuzzLauncher.imageSize, global::GuzzLaunhcer.GuzzLauncher.imageSize);
        this.gamePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.gamePictureBox.TabIndex = 1;
        this.gamePictureBox.TabStop = false;
        this.gamePictureBox.Name = this.gameName + "PictureBox";
        //DEFAULT VALUES

        //gameBox.gameImage.Click += new System.EventHandler(this.pictureBox1_Click);
        #endregion



        #region LabelValues
        //this.gameText.Location = new Point(gameLocation.X + GuzzLaunhcer.GuzzLauncher.imageSize/2, gameLocation.Y + GuzzLaunhcer.GuzzLauncher.imageSize+6);
        this.gameText.Location = new Point(gameLocation.X , gameLocation.Y + GuzzLaunhcer.GuzzLauncher.imageSize + 6);
        this.gameText.Size = new Size(GuzzLaunhcer.GuzzLauncher.imageSize, 25);
        this.gameText.Text = gameName;

        //DEFAULT VALUES
        this.gameText.AutoSize = false;
        this.gameText.TabIndex = 5;
        this.gameText.Name = this.gameName + "Label";
        this.gameText.TextAlign = ContentAlignment.MiddleCenter;
        this.gameText.BorderStyle = BorderStyle.Fixed3D;
        //DEFAULT VALUES
        #endregion



        #region ButtonValues
        this.gameButton.Location = new Point(gameLocation.X, gameLocation.Y + GuzzLaunhcer.GuzzLauncher.imageSize + 14);
        this.gameButton.Location
        #endregion
    }

    public void CheckGameStatus()
    {

    }
}







//INITILIZE EXAMPLED DEF
/*
private void InitializeGameBox(GameBox gameBox)
{
    gameBox.gameImage.Image = global::GuzzLaunhcer.Properties.Resources.artworks_000013535097_9rz0uo_t500x500;
    gameBox.gameImage.Location = new System.Drawing.Point(12, 12);
    gameBox.gameImage.Name = "pictureBox1";
    gameBox.gameImage.Size = new System.Drawing.Size(128, 128);
    gameBox.gameImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
    gameBox.gameImage.TabIndex = 1;
    gameBox.gameImage.TabStop = false;
    gameBox.gameImage.Click += new System.EventHandler(this.pictureBox1_Click);





}
*/