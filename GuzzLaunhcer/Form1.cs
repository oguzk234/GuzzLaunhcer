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
using System.IO.Compression;

namespace GuzzLaunhcer
{
    public partial class GuzzLauncher : Form
    {
        string appDir;

        string appDataDir;
        string configFileDir;
        public static string downloadDir;

        //ARRAY SYSTEM
        public static List<GameBox> gameBoxList = new List<GameBox>();
        public static int imageSize = 182;
        public static int imageSpace = 36;
        public static int imageCount = 0;
        public static Point DefaultPoint = new Point(12, 12);
        //

        //public static List<GameBoxData> gameBoxDataList = new List<GameBoxData>();


        public GuzzLauncher()
        {
            InitializeComponent();
        }

        private async void GuzzLauncher_Load(object sender, EventArgs e)
        {
            appDir = AppDomain.CurrentDomain.BaseDirectory;
            //configFileDir = System.IO.Path.Combine(appDir, "config.txt");

            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "GuzzLauncher"));
            appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "GuzzLauncher");
            configFileDir = Path.Combine(appDataDir, "config.txt"); 

            //downloadDir = File.ReadLines(configFileDir).First(); // FIRST SETUP CHECK DE KONTROL EDILIYOR

            //MessageBox.Show(downloadDir);
            FirstSetupCheck();

            
            //gameBoxDataList = await GetGamesOnline();
            foreach (GameBoxData gameData in await GetGamesOnline())
            {
                GameBox gBox = CreateGameBox(gameData.NName, DefaultPoint, gameData.IImage, gameData.VVersion , gameData.DDownloadLink);
                //gBox.gameButton
            }
            

            //await GetGamesOnline(); //BU BİTMEDEN ALTA GEÇMİYO MÜKEMMEL (sanırım)
            //StartGameBoxes();

        }
        private void FirstSetupCheck()
        {
            if (System.IO.File.Exists(configFileDir))
            {
                downloadDir = System.IO.File.ReadLines(configFileDir).First();
                MessageBox.Show(downloadDir);
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
            CreateGameBox("31", DefaultPoint, Properties.Resources.artworks_000013535097_9rz0uo_t500x500,"V0.1","nul");
            CreateGameBox("32fgdfdgfdgfdgdf", DefaultPoint, Properties.Resources.artworks_000013535097_9rz0uo_t500x500,"V0.25","nul");
            CreateGameBox("33333", DefaultPoint, Properties.Resources.artworks_000013535097_9rz0uo_t500x500, "V0.28","nul");

        }

        /*
        private void CreateGameBox(string name, Point Location, Image image,string version)
        {
            GameBox gameBox = new GameBox(name, Location, image,version);
            //GameBox gameBox = new GameBox("Ahmet Kaya Ball 2", new Point(12, 12), Properties.Resources.artworks_000013535097_9rz0uo_t500x500);

            this.Controls.Add(gameBox.gamePictureBox);  // BU İKİSİ RENDERLANMASINI SAĞLIYOR
            this.Controls.Add(gameBox.gameText);
            gameBox.gameText.BringToFront();
            this.Controls.Add(gameBox.gameButton);

            imageCount++;
            DefaultPoint = new Point(12 + ((imageCount * imageSize) + (imageSpace * imageCount)), 12);
            //sMessageBox.Show(imageCount.ToString());
            gameBoxList.Add(gameBox);
        }
        */
        private GameBox CreateGameBox(string name, Point Location, Image image, string version , string downloadlink)
        {
            GameBox gameBox = new GameBox(name, Location, image, version , downloadlink);
            //GameBox gameBox = new GameBox("Ahmet Kaya Ball 2", new Point(12, 12), Properties.Resources.artworks_000013535097_9rz0uo_t500x500);

            this.Controls.Add(gameBox.gamePictureBox);  // BU İKİSİ RENDERLANMASINI SAĞLIYOR
            this.Controls.Add(gameBox.gameText);
            gameBox.gameText.BringToFront();
            this.Controls.Add(gameBox.gameButton);
            this.Controls.Add(gameBox.progressBar);

            imageCount++;
            DefaultPoint = new Point(12 + ((imageCount * imageSize) + (imageSpace * imageCount)), 12);
            //sMessageBox.Show(imageCount.ToString());
            gameBoxList.Add(gameBox);
            return gameBox;
        }



        public static async Task<List<GameBoxData>> GetGamesOnline()
        {
            string url = "https://raw.githubusercontent.com/oguzk234/GuzzLauncherOnlineDatas/refs/heads/main/GuzzGamesData2.txt";

            List<GameBoxData> gameBoxDatas = new List<GameBoxData>();

            Image image = null;
            string name = null;
            string version = null;
            string downloadLink = null;

            using (HttpClient client = new HttpClient())
            {
                try
                { 
                    //ASENKRON OKUMAĞ
                    string content = await client.GetStringAsync(url);
                    //MessageBox.Show(content);   //IMPO

                    string[] games = content.Split('>');
                    //MessageBox.Show("OYUN SAYISI = "+games.Length.ToString());   //IMPO
                    foreach(string game in games)
                    {
                        string[] parts = game.Split('<');

                        name = parts[0];
                        version = parts[1];
                        downloadLink = parts[2];
                        byte[] imageBytes = await client.GetByteArrayAsync(parts[3]);
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            image = Image.FromStream(ms);
                        }
                        gameBoxDatas.Add(new GameBoxData(image, name, version, downloadLink));
                    }
                    //string[] parts = content.Split('<');


                    //MessageBox.Show(content + " = Content");  //Foreach ile hepsini doğrulayabilirsin

                }
                catch (Exception ex)
                {
                    MessageBox.Show("GetGamesOnline HATASI " + ex.Message);
                }
                finally
                {

                }
            }

            //return new GameBoxData(image, name, version, downloadLink);
            //gameBoxDatas.Add(new GameBoxData(image, name, version, downloadLink));
            return gameBoxDatas;

        }

        public static async Task DownloadGame(string gameFolderName, string url = "https://github.com/oguzk234/AhmetKayaBall_Beta_V0.23")
        { //GAME FOLDER NAME İLE GAME NAME AYNI OLMALI
            MessageBox.Show("indirilen URL = "+url);
            string gameRarFile = Path.Combine(downloadDir, gameFolderName + ".zip");

            GameBox downloadedGame = FindGameFromName(gameFolderName);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    downloadedGame.isDownloading = true;

                    var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
                    response.EnsureSuccessStatusCode();

                    long totalBytes = (int)response.Content.Headers.ContentLength;


                    using (var responseStream = await client.GetStreamAsync(url))
                    using (var fileStream = new FileStream(gameRarFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        byte[] buffer = new byte[8192]; // 8 KB'lik veri parçaları indirilecek
                        long totalRead = 0;
                        int bytesRead;

                        while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, bytesRead);
                            totalRead += bytesRead;

                            // İlerleme çubuğunu güncelle
                            int progressPercentage = (int)((totalRead * 100) / totalBytes);
                            downloadedGame.progressBar.Value = progressPercentage;

                        }

                    }


                    /* OLD
                    using (FileStream fs = new FileStream(gameRarFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                    */


                    MessageBox.Show("RAR INDIRME OLUMLU");
                    MessageBox.Show(gameRarFile + "  " + downloadDir);

                    ZipFile.ExtractToDirectory(gameRarFile,downloadDir);
                    File.Delete(gameRarFile);

                    downloadedGame.isDownloading = false;
                    downloadedGame.CheckGameStatus();
                    downloadedGame.CheckButtonStatus();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"INDIRMEDE Hata oluştu: {ex.Message}");
                    downloadedGame.isDownloading = false;
                }
                finally
                {
                    downloadedGame.isDownloading = false;
                }
            }
        }




        public static GameBox FindGameFromName(string gameName)
        {
            foreach (GameBox gBox in gameBoxList)
            {
                if (gameName == gBox.gameName)
                {
                    return gBox;
                }
            }
            return null;
        }



    }
}




public class GameBox
{

    public string version;
    public string gameName;
    public Point gameLocation;
    public Image gameImage;


    public PictureBox gamePictureBox;// KENDI OLUSTURULACAK
    public Label gameText;// KENDI OLUSTURULACAK
    public Button gameButton;
    public ProgressBar progressBar;

    public enum GameStatus { NotDownloaded,UpdateNeeded,ReadyToPlay }
    public GameStatus gameStatus = GameStatus.NotDownloaded;

    public string DownloadLink;

    public bool isDownloading = false;

    public GameBox(string GameName, Point GameLocation, Image GameImage , string Version , string downloadlink)
    {
        version = Version;
        gameName = GameName; gameLocation = GameLocation; gameImage = GameImage;

        gameText = new Label();
        gamePictureBox = new PictureBox();
        gameButton = new Button();
        DownloadLink = downloadlink;
        progressBar = new ProgressBar();




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
        this.gameText.Size = new Size(GuzzLaunhcer.GuzzLauncher.imageSize, 35);
        this.gameText.Text = gameName;

        //DEFAULT VALUES
        this.gameText.AutoSize = false;
        this.gameText.TabIndex = 5;
        this.gameText.Name = this.gameName + "Label";
        this.gameText.TextAlign = ContentAlignment.MiddleCenter;
        this.gameText.BorderStyle = BorderStyle.Fixed3D;
        //DEFAULT VALUES
        #endregion

        CheckGameStatus();

        #region ButtonValues
        this.gameButton.Location = new Point(gameLocation.X, gameLocation.Y + GuzzLaunhcer.GuzzLauncher.imageSize + 44);
        this.gameButton.Size = new Size(GuzzLaunhcer.GuzzLauncher.imageSize, 32);

        CheckButtonStatus();

        //DEFAULT VALUES
        this.gameButton.Name = this.gameName + "Button";
        this.gameButton.TabIndex = 0;
        this.gameButton.UseVisualStyleBackColor = true;
        this.gameButton.Click += new System.EventHandler(this.OnButtonClick);
        //DEFAULT VALUES
        #endregion





        #region ProgressBarValues
        this.progressBar.Location = new Point(gameButton.Location.X, gameButton.Location.Y + 34);
        this.progressBar.Size = new Size(GuzzLaunhcer.GuzzLauncher.imageSize, 18);
        #endregion


    }

    public async void OnButtonClick(object sender, EventArgs e)
    {
        //MessageBox.Show(sender.ToString() + " /// " + this.gameName);
        string GamePath = Path.Combine(GuzzLaunhcer.GuzzLauncher.downloadDir, this.gameName);

        switch (gameStatus)
        {
            case GameStatus.NotDownloaded:

                //MessageBox.Show("Indirme Basladi = " + this.DownloadLink);
                if(isDownloading == false)
                {
                    await GuzzLaunhcer.GuzzLauncher.DownloadGame(this.gameName, DownloadLink);

                    this.CheckGameStatus();
                    this.CheckButtonStatus();

                    isDownloading = true;
                }
                else
                {
                    MessageBox.Show("Already Downloading " + this.gameName);
                }


                //MessageBox.Show("Indirme Tamamlandi");
                break;


            case GameStatus.ReadyToPlay:

                try
                {
                    // Yeni bir işlem (process) başlat
                    Process process = new Process();
                    process.StartInfo.FileName = Path.Combine(GuzzLaunhcer.GuzzLauncher.downloadDir, gameName, gameName + ".exe");


                    process.Start();

                    //process.WaitForExit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                }


                break;


            case GameStatus.UpdateNeeded:

                if (isDownloading == false)
                {
                    if (Directory.Exists(GamePath))
                    {
                        Directory.Delete(GamePath, true);
                        await GuzzLaunhcer.GuzzLauncher.DownloadGame(this.gameName, DownloadLink);

                        this.CheckGameStatus();
                        this.CheckButtonStatus();

                        isDownloading = true;
                    }

                }

                await GuzzLaunhcer.GuzzLauncher.DownloadGame(this.gameName, DownloadLink);


                break;
        }
    }

    public void CheckButtonStatus()
    {
        switch (gameStatus)
        {
            case GameStatus.NotDownloaded:
                this.gameButton.Text = "Download";
                this.gameButton.ForeColor = Color.Red;
                this.progressBar.Value = 0;
                break;
            case GameStatus.ReadyToPlay:
                this.gameButton.Text = "Play";
                this.gameButton.ForeColor = Color.Green;
                this.progressBar.Value = 100;
                break;
            case GameStatus.UpdateNeeded:
                this.gameButton.Text = "Update";
                this.gameButton.ForeColor = Color.Orange;
                this.progressBar.Value = 50;
                break;
        }
    }

    public void CheckGameStatus()
    {
        string GamePath = Path.Combine(GuzzLaunhcer.GuzzLauncher.downloadDir, this.gameName);
        //string GameExePath = Path.Combine(GamePath, gameName + ".exe");
        //MessageBox.Show(GamePath);  //IMPO
        string GameConfigPath = Path.Combine(GamePath, "GuzzLauncherConfig.txt");
        //MessageBox.Show(GameConfigPath);
        if (Directory.Exists(GamePath) == false)
        {
            //MessageBox.Show(this.gameName + " OYUNU KLASÖRÜ BULUNAMADI");  //BU DA IMPO
            //MessageBox.Show(this.gameName + " GameDirectorysi OLUŞTURULDU");
            //Directory.CreateDirectory(GamePath);
            //File.WriteAllText(GameConfigPath,version);

            //File.WriteAllText("C:\\GuzzLauncher\\31\\zort.txt","V0.2");
            //File.AppendAllText(GameConfigPath, "V0.1");

            this.gameStatus = GameStatus.NotDownloaded;
        }
        else if(File.Exists(GameConfigPath))
        {
            //MessageBox.Show(this.gameName + " GameDirectorysi BULUNDU");   //BURALAR IMPO

            string[] GameConfigLines = File.ReadAllLines(GameConfigPath);
            if(GameConfigLines[0] != version)
            {
                MessageBox.Show(gameName + " adlı oyunda sürüm uyuşmazlığı, Yeni = " + version + " Eski = " + GameConfigLines[0]);
                this.gameStatus = GameStatus.UpdateNeeded;

            }
            else if (GameConfigLines[0] == version)
            {
                //MessageBox.Show("SÜRÜM GÜNCEL");
                this.gameStatus = GameStatus.ReadyToPlay;
            }
            else
            {
                MessageBox.Show("Bİ BOKLAR VAR");
            }

        }
        else
        { 
            MessageBox.Show("ANA KLASÖR VAR AMA CONFIGI YOK O YUZDEN CONFIG OLUSTURULUYOR + " + gameName);
            File.WriteAllText(GameConfigPath, version);
            this.CheckGameStatus();
        }
    }
}


public struct GameBoxData
{
    public GameBoxData(Image iimage,string nname,string vversion,string ddownloadLink)
    {
        IImage = iimage;
        NName = nname;
        VVersion = vversion;
        DDownloadLink = ddownloadLink;
    }

    public Image IImage;
    public string NName;
    public string VVersion;
    public string DDownloadLink;
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









/*
using (HttpClient client = new HttpClient())
{
    try
    {
        downloadedGame.isDownloading = true;

        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var contentLength = response.Content.Headers.ContentLength;




        using (var stream = await response.Content.ReadAsStreamAsync())
        {
            using (FileStream fs = new FileStream(gameRarFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //await response.Content.CopyToAsync(fs);
                byte[] buffer = new byte[8192];
                long totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fs.WriteAsync(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;

                    if (contentLength.HasValue)
                    {
                        double progressPercentage = (double)totalBytesRead / contentLength.Value * 100;

                        downloadedGame.progressBar.Value = (int)progressPercentage;
                        //MessageBox.Show("indirme");

                    }

                }

            }


        }
*/