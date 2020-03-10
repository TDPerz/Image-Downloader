using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace Aplicacion
{
    public partial class Form1 : Form
    {
        struct datosW
        {
            public string nombre;
            public string url;
        }
        List<datosW> misdatos = new List<datosW>();
        string urlanterior;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            web.ScriptErrorsSuppressed = true;
            web.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(datos);
        }

        private void datos(object sender, EventArgs e)
        {
            try
            {
                
            }catch(Exception ex)
            {
                MessageBox.Show("Error!");
                Console.WriteLine("Error!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(getUrl.Text != "" && getUrl.Text != " ")
            {
                web.Navigate(getUrl.Text);
            }
        }

        private void lista_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lista.SelectedIndex != -1)
            {
                web.Navigate(misdatos[lista.SelectedIndex].url);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> temp = new List<string>();
                List<string> tempurl = new List<string>();
                lista.Items.Clear();
                misdatos.Clear();
                foreach (HtmlElement et in web.Document.GetElementsByTagName("a"))
                {
                    temp.Add(et.InnerText);
                    Console.WriteLine(et.InnerText);
                }
                foreach (HtmlElement refe in web.Document.GetElementsByTagName("a"))
                {
                    tempurl.Add(refe.GetAttribute("href"));
                    Console.WriteLine(refe.GetAttribute("href"));
                }
                for (int i = 1; i < temp.Count-1; i++)
                {
                    datosW temporal = new datosW();
                    temporal.nombre = temp[i];
                    temporal.url = tempurl[i];
                    misdatos.Add(temporal);
                    lista.Items.Add(temp[i]);
                }
            }
            catch (Exception a)
            {
                Console.WriteLine("Error!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            urlanterior = web.Url.AbsoluteUri;
            progressBar1.Visible = true;
            label2.Visible = true;
            Console.WriteLine(urlanterior);
            int i = 0;
            //nota solo hace falta uno para que despliegue todos
            web.Navigate(misdatos[0].url);
            List<string> temp = new List<string>();
            Console.WriteLine("Urls: ");
            foreach(HtmlElement et in web.Document.GetElementsByTagName("img"))
            {
                temp.Add(et.GetAttribute("src"));
                Console.WriteLine(et.GetAttribute("src"));
            }
            Console.WriteLine("Tamaño de la lista: "+ temp.Count);
            FolderBrowserDialog sv = new FolderBrowserDialog();
            WebClient w = new WebClient();
            w.DownloadProgressChanged += new DownloadProgressChangedEventHandler(change);
            if(sv.ShowDialog() == DialogResult.OK)
            {
                for(int x = 0; x < temp.Count; x++)
                {
                    label2.Text = temp[x].Split('/')[temp[x].Split('/').Length - 1];
                    w.DownloadFile("http://www.dermatlas.net/images/"+ temp[x].Split('/')[temp[x].Split('/').Length - 1], sv.SelectedPath+@"\"+ temp[x].Split('/')[temp[x].Split('/').Length - 1]);
                    lista.SelectedIndex = x;
                }
                MessageBox.Show("Descarga Finalizada!");
            }
            else
            {
                MessageBox.Show("Descarga abortada!");
            }
            progressBar1.Visible = false;
            label2.Visible = false;
            web.Navigate(urlanterior);
        }

        private void change(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("Inicio.exe");
            this.Close();
        }
    }
}
