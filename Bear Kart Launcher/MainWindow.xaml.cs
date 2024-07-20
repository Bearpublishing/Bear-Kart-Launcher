using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;

namespace Bear_Kart_Launcher
{
    enum k3bLauncherStatus
    {
        ready,
        failed,
        downloadingGame,
        downloadingUpdate
    }

    enum bk2LauncherStatus
    {
        ready,
        failed,
        downloadingGame,
        downloadingUpdate
    }

    enum bk1LauncherStatus
    {
        ready,
        failed,
        downloadingGame,
        downloadingUpdate
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string k3brootPath;
        private string k3bversionFile;
        private string k3bgameZip;
        private string k3bgameExe;
        private string bk2rootPath;
        private string bk2versionFile;
        private string bk2gameZip;
        private string bk2gameExe;
        private string bk1rootPath;
        private string bk1versionFile;
        private string bk1gameZip;
        private string bk1gameExe;
        private string k3bVersiontxt;
        private string k3bBuildzip;
        private string bk2Versiontxt;
        private string bk2Buildzip;
        private string bk1Versiontxt;
        private string bk1Buildzip;

        private k3bLauncherStatus _k3bstatus;

        internal k3bLauncherStatus k3bStatus
        {
            get => _k3bstatus;
            set
            {
                _k3bstatus = value;
                switch (_k3bstatus)
                {
                    case k3bLauncherStatus.ready:
                        PlayButton.Content = "Play";
                        break;
                    case k3bLauncherStatus.failed:
                        PlayButton.Content = "Failed :)";
                        break;
                    case k3bLauncherStatus.downloadingGame:
                        PlayButton.Content = "Downloading Game...";
                        break;
                    case k3bLauncherStatus.downloadingUpdate:
                        PlayButton.Content = "Downloading Update...";
                        break;
                    default:
                        break;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            k3brootPath = Directory.GetCurrentDirectory();
            k3bversionFile = Path.Combine(k3brootPath, "k3bVersion.txt");
            k3bgameZip = Path.Combine(k3brootPath, "k3bBuild.zip");
            k3bgameExe = Path.Combine(k3brootPath, "k3b", "Kart 3 Bear Jungle the Fun in.exe");
            bk2rootPath = Directory.GetCurrentDirectory();
            bk2versionFile = Path.Combine(bk2rootPath, "bk2Version.txt");
            bk2gameZip = Path.Combine(bk2rootPath, "bk2Build.zip");
            bk2gameExe = Path.Combine(bk2rootPath, "bk2", "Bear Kart 2 - Fun in the Jungle.exe");
            bk1rootPath = Directory.GetCurrentDirectory();
            bk1versionFile = Path.Combine(bk2rootPath, "bk1Version.txt");
            bk1gameZip = Path.Combine(bk2rootPath, "bk1Build.zip");
            bk1gameExe = Path.Combine(bk2rootPath, "bk1", "Bear Kart.exe");            

            if (File.Exists(Path.Combine(k3brootPath, "repos.conf")))
            {
                // do nothing
            }
            else
            {
                using (File.Create(Path.Combine(k3brootPath, "repos.conf"))) {}                
                StreamWriter sr = new StreamWriter(Path.Combine(k3brootPath, "repos.conf"));
                sr.WriteLine("https://www.dropbox.com/scl/fi/3yap1yoc0k5un0yoysv7a/k3bVersion.txt?rlkey=gce6onfq4773pem0sy2x10i8h&st=p65olgxa&dl=1");
                sr.WriteLine("https://www.dropbox.com/scl/fi/w9n8ltx4d11nbtiqr6q7u/k3bBuild.zip?rlkey=3nvv3vad8jhtjpeyszs9jo74b&st=03o4wv05&dl=1");
                sr.WriteLine("https://www.dropbox.com/scl/fi/53uwa94bau87e8cbachwt/bk2Version.txt?rlkey=yfdj4k3u1pc7wifblx5ijic4y&st=73dwsukt&dl=1");
                sr.WriteLine("https://www.dropbox.com/scl/fi/5y1co93idix2yrvunn9s9/bk2Build.zip?rlkey=2up0ysq4rsjjcv4mzvkumthc8&st=70vz75ej&dl=1");
                sr.WriteLine("https://www.dropbox.com/scl/fi/z12pd6pqbi31e5jhogpyt/bk1Version.txt?rlkey=wnk2mpbbrkrmuy9wbobweb688&st=qoxr0opv&dl=1");
                sr.WriteLine("https://www.dropbox.com/scl/fi/x32nm30gzc9zgce0kklsc/bk1Build.zip?rlkey=w48vnio9ghtojdgg8xtqbguti&st=skilv14d&dl=1");
                sr.Close();
            }
            StreamReader sw = new StreamReader(Path.Combine(k3brootPath, "repos.conf"));
            k3bVersiontxt = sw.ReadLine();
            k3bBuildzip = sw.ReadLine();
            bk2Versiontxt = sw.ReadLine();
            bk2Buildzip = sw.ReadLine();
            bk1Versiontxt = sw.ReadLine();
            bk1Buildzip = sw.ReadLine();
            sw.Close();
        }

        private void k3bCheckForUpdates()
        {
            if (File.Exists(k3bversionFile))
            {
                k3bVersion localVersion = new k3bVersion(File.ReadAllText(k3bversionFile));
                VersionText.Text = localVersion.ToString();

                try
                {
                    WebClient webClient = new WebClient();
                    k3bVersion onlineVersion = new k3bVersion(webClient.DownloadString(k3bVersiontxt));

                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        k3bInstallGameFiles(true, onlineVersion);
                    }
                    else
                    {
                        k3bStatus = k3bLauncherStatus.ready;
                    }
                }
                catch (Exception ex)
                {
                    k3bStatus = k3bLauncherStatus.failed;
                    MessageBox.Show($"Error checking for game updates: {ex}");
                }
            }
            else
            {
                k3bInstallGameFiles(false, k3bVersion.zero);
            }
        }

        private void k3bInstallGameFiles(bool _isUpdate, k3bVersion _onlineVersion)
        {
            try
            {
                WebClient webClient = new WebClient();
                if (_isUpdate)
                {
                    k3bStatus = k3bLauncherStatus.downloadingUpdate;
                }
                else
                {
                    k3bStatus = k3bLauncherStatus.downloadingGame;
                    _onlineVersion = new k3bVersion(webClient.DownloadString(k3bVersiontxt));
                }

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(k3bDownloadGameCompletedCallback);
                webClient.DownloadFileAsync(new Uri(k3bBuildzip), k3bgameZip, _onlineVersion);
            }
            catch (Exception ex)
            {
                k3bStatus = k3bLauncherStatus.failed;
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }

        private void k3bDownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string onlineVersion = ((k3bVersion)e.UserState).ToString();
                ZipFile.ExtractToDirectory(k3bgameZip, k3brootPath, true);
                File.Delete(k3bgameZip);

                File.WriteAllText(k3bversionFile, onlineVersion);

                VersionText.Text = onlineVersion;
                k3bStatus = k3bLauncherStatus.ready;
            }
            catch (Exception ex)
            {
                k3bStatus = k3bLauncherStatus.failed;
                MessageBox.Show($"Error finishing download: {ex}");
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            k3bCheckForUpdates();
            bk2CheckForUpdates();
            bk1CheckForUpdates();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(k3bgameExe) && k3bStatus == k3bLauncherStatus.ready)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(k3bgameExe);
                startInfo.WorkingDirectory = Path.Combine(k3brootPath);
                Process.Start(startInfo);

                Close();
            }
            else if (k3bStatus == k3bLauncherStatus.failed)
            {
                k3bCheckForUpdates();
            }
        }

        struct k3bVersion
        {
            internal static k3bVersion zero = new k3bVersion(0, 0, 0);

            private short major;
            private short minor;
            private short subMinor;

            internal k3bVersion(short _major, short _minor, short _subMinor)
            {
                major = _major;
                minor = _minor;
                subMinor = _subMinor;
            }
            internal k3bVersion(string _version)
            {
                string[] _versionstrings = _version.Split('.');
                if (_versionstrings.Length != 3)
                {
                    major = 0; 
                    minor = 0;
                    subMinor = 0;
                    return;
                }

                major = short.Parse(_versionstrings[0]);
                minor = short.Parse(_versionstrings[1]);
                subMinor = short.Parse(_versionstrings[2]);
            }

            internal bool IsDifferentThan(k3bVersion _otherVersion)
            {
                if (major != _otherVersion.major) { return true; }
                else if (minor != _otherVersion.minor) { return true; }
                else if (subMinor != _otherVersion.subMinor) { return true; }
                else { return false; }
            }

            public override string ToString()
            {
                return $"{major}.{minor}.{subMinor}";
            }
        }        

        private bk2LauncherStatus _bk2status;

        internal bk2LauncherStatus bk2Status
        {
            get => _bk2status;
            set
            {
                _bk2status = value;
                switch (_bk2status)
                {
                    case bk2LauncherStatus.ready:
                        bk2PlayButton.Content = "Play";
                        break;
                    case bk2LauncherStatus.failed:
                        bk2PlayButton.Content = "Failed :)";
                        break;
                    case bk2LauncherStatus.downloadingGame:
                        bk2PlayButton.Content = "Downloading Game...";
                        break;
                    case bk2LauncherStatus.downloadingUpdate:
                        bk2PlayButton.Content = "Downloading Update...";
                        break;
                    default:
                        break;
                }
            }
        }


        private void bk2CheckForUpdates()
        {
            if (File.Exists(bk2versionFile))
            {
                bk2Version localVersion = new bk2Version(File.ReadAllText(bk2versionFile));
                bk2VersionText.Text = localVersion.ToString();

                try
                {
                    WebClient webClient = new WebClient();
                    bk2Version onlineVersion = new bk2Version(webClient.DownloadString(bk2Versiontxt));

                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        bk2InstallGameFiles(true, onlineVersion);
                    }
                    else
                    {
                        bk2Status = bk2LauncherStatus.ready;
                    }
                }
                catch (Exception ex)
                {
                    bk2Status = bk2LauncherStatus.failed;
                    MessageBox.Show($"Error checking for game updates: {ex}");
                }
            }
            else
            {
                bk2InstallGameFiles(false, bk2Version.zero);
            }
        }

        private void bk2InstallGameFiles(bool _isUpdate, bk2Version _onlineVersion)
        {
            try
            {
                WebClient webClient = new WebClient();
                if (_isUpdate)
                {
                    bk2Status = bk2LauncherStatus.downloadingUpdate;
                }
                else
                {
                    bk2Status = bk2LauncherStatus.downloadingGame;
                    _onlineVersion = new bk2Version(webClient.DownloadString(bk2Versiontxt));
                }

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(bk2DownloadGameCompletedCallback);
                webClient.DownloadFileAsync(new Uri(bk2Buildzip), bk2gameZip, _onlineVersion);
            }
            catch (Exception ex)
            {
                bk2Status = bk2LauncherStatus.failed;
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }

        private void bk2DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string onlineVersion = ((bk2Version)e.UserState).ToString();
                ZipFile.ExtractToDirectory(bk2gameZip, bk2rootPath, true);
                File.Delete(bk2gameZip);

                File.WriteAllText(bk2versionFile, onlineVersion);

                bk2VersionText.Text = onlineVersion;
                bk2Status = bk2LauncherStatus.ready;
            }
            catch (Exception ex)
            {
                bk2Status = bk2LauncherStatus.failed;
                MessageBox.Show($"Error finishing download: {ex}");
            }
        }


        private void bk2PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(bk2gameExe) && bk2Status == bk2LauncherStatus.ready)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(bk2gameExe);
                startInfo.WorkingDirectory = Path.Combine(bk2rootPath);
                Process.Start(startInfo);

                Close();
            }
            else if (bk2Status == bk2LauncherStatus.failed)
            {
                bk2CheckForUpdates();
            }
        }

        struct bk2Version
        {
            internal static bk2Version zero = new bk2Version(0, 0, 0);

            private short major;
            private short minor;
            private short subMinor;

            internal bk2Version(short _major, short _minor, short _subMinor)
            {
                major = _major;
                minor = _minor;
                subMinor = _subMinor;
            }
            internal bk2Version(string _version)
            {
                string[] _versionstrings = _version.Split('.');
                if (_versionstrings.Length != 3)
                {
                    major = 0;
                    minor = 0;
                    subMinor = 0;
                    return;
                }

                major = short.Parse(_versionstrings[0]);
                minor = short.Parse(_versionstrings[1]);
                subMinor = short.Parse(_versionstrings[2]);
            }

            internal bool IsDifferentThan(bk2Version _otherVersion)
            {
                if (major != _otherVersion.major) { return true; }
                else if (minor != _otherVersion.minor) { return true; }
                else if (subMinor != _otherVersion.subMinor) { return true; }
                else { return false; }
            }

            public override string ToString()
            {
                return $"{major}.{minor}.{subMinor}";
            }
        }

        private bk1LauncherStatus _bk1status;

        internal bk1LauncherStatus bk1Status
        {
            get => _bk1status;
            set
            {
                _bk1status = value;
                switch (_bk1status)
                {
                    case bk1LauncherStatus.ready:
                        bk1PlayButton.Content = "Play";
                        break;
                    case bk1LauncherStatus.failed:
                        bk1PlayButton.Content = "Failed :)";
                        break;
                    case bk1LauncherStatus.downloadingGame:
                        bk1PlayButton.Content = "Downloading Game...";
                        break;
                    case bk1LauncherStatus.downloadingUpdate:
                        bk1PlayButton.Content = "Downloading Update...";
                        break;
                    default:
                        break;
                }
            }
        }


        private void bk1CheckForUpdates()
        {
            if (File.Exists(bk1versionFile))
            {
                bk1Version localVersion = new bk1Version(File.ReadAllText(bk1versionFile));
                bk1VersionText.Text = localVersion.ToString();

                try
                {
                    WebClient webClient = new WebClient();
                    bk1Version onlineVersion = new bk1Version(webClient.DownloadString(bk1Versiontxt));

                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        bk1InstallGameFiles(true, onlineVersion);
                    }
                    else
                    {
                        bk1Status = bk1LauncherStatus.ready;
                    }
                }
                catch (Exception ex)
                {
                    bk1Status = bk1LauncherStatus.failed;
                    MessageBox.Show($"Error checking for game updates: {ex}");
                }
            }
            else
            {
                bk1InstallGameFiles(false, bk1Version.zero);
            }
        }

        private void bk1InstallGameFiles(bool _isUpdate, bk1Version _onlineVersion)
        {
            try
            {
                WebClient webClient = new WebClient();
                if (_isUpdate)
                {
                    bk1Status = bk1LauncherStatus.downloadingUpdate;
                }
                else
                {
                    bk1Status = bk1LauncherStatus.downloadingGame;
                    _onlineVersion = new bk1Version(webClient.DownloadString(bk1Versiontxt));
                }

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(bk1DownloadGameCompletedCallback);
                webClient.DownloadFileAsync(new Uri(bk1Buildzip), bk1gameZip, _onlineVersion);
            }
            catch (Exception ex)
            {
                bk1Status = bk1LauncherStatus.failed;
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }

        private void bk1DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string onlineVersion = ((bk1Version)e.UserState).ToString();
                ZipFile.ExtractToDirectory(bk1gameZip, bk1rootPath, true);
                File.Delete(bk1gameZip);

                File.WriteAllText(bk1versionFile, onlineVersion);

                bk1VersionText.Text = onlineVersion;
                bk1Status = bk1LauncherStatus.ready;
            }
            catch (Exception ex)
            {
                bk1Status = bk1LauncherStatus.failed;
                MessageBox.Show($"Error finishing download: {ex}");
            }
        }


        private void bk1PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(bk1gameExe) && bk1Status == bk1LauncherStatus.ready)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(bk1gameExe);
                startInfo.WorkingDirectory = Path.Combine(bk1rootPath);
                Process.Start(startInfo);

                Close();
            }
            else if (bk1Status == bk1LauncherStatus.failed)
            {
                bk1CheckForUpdates();
            }
        }

        struct bk1Version
        {
            internal static bk1Version zero = new bk1Version(0, 0, 0);

            private short major;
            private short minor;
            private short subMinor;

            internal bk1Version(short _major, short _minor, short _subMinor)
            {
                major = _major;
                minor = _minor;
                subMinor = _subMinor;
            }
            internal bk1Version(string _version)
            {
                string[] _versionstrings = _version.Split('.');
                if (_versionstrings.Length != 3)
                {
                    major = 0;
                    minor = 0;
                    subMinor = 0;
                    return;
                }

                major = short.Parse(_versionstrings[0]);
                minor = short.Parse(_versionstrings[1]);
                subMinor = short.Parse(_versionstrings[2]);
            }

            internal bool IsDifferentThan(bk1Version _otherVersion)
            {
                if (major != _otherVersion.major) { return true; }
                else if (minor != _otherVersion.minor) { return true; }
                else if (subMinor != _otherVersion.subMinor) { return true; }
                else { return false; }
            }

            public override string ToString()
            {
                return $"{major}.{minor}.{subMinor}";
            }
        }

    }
}