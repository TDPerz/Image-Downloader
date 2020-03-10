using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicacion_2
{
    public partial class Form1 : Form
    {
        struct datos
        {
            public string url;
            public string name;
        }
        List<datos> lista;
        int pros = 0;
        bool termino = false;
        int contado = 0;

        public Form1()
        {
            InitializeComponent();
            web.ScriptErrorsSuppressed = true;
            web.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(evento);
            web.Navigate("https://www.dermnetnz.org/search.html?q=psoriasis");
            lista = new List<datos>();
        }

        private void evento(object sender, EventArgs e)
        {
            foreach (HtmlElement v in web.Document.All)
            {
                if(v.GetAttribute("classname").Contains("gsc-tabHeader gsc-tabhInactive gsc-inline-block"))
                {
                    button1.Visible = true;
                    v.InvokeMember("Click");
                    
                }
            }
        }

        private void Et_Click(object sender, HtmlElementEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "" && textBox1.Text != " ")
            {

                web.Navigate("https://www.dermnetnz.org/search.html?q=" + textBox1.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = "";
            Console.WriteLine("Pros es: " + pros);
            int i = 1;
            listBox1.Items.Clear();
            lista.Clear();
            List<datos> d = new List<datos>();
            foreach (HtmlElement et in web.Document.GetElementsByTagName("a"))
            {
                if (et.GetAttribute("classname").Contains("gs-image gs-image-scalable")) // anterior gs-title
                {
                    Console.WriteLine("Enlaze " + i + " ? " + et.GetAttribute("href"));
                    datos n = new datos();
                    n.name = et.GetAttribute("href").Split('/')[et.GetAttribute("href").Split('/').Length - 2];
                    n.url = et.GetAttribute("href");
                    a = n.name;
                    d.Add(n);
                    i++;
                }
            }
            foreach (var z in d)
            {
                listBox1.Items.Add(z.name);
                lista.Add(z);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> f = new List<string>();
            progressBar1.Visible = true;
            label1.Visible = true;
            int i = 0;
            WebClient w = new WebClient();
            w.DownloadProgressChanged += new DownloadProgressChangedEventHandler(change);
            Console.WriteLine("Tamaño total: " + web.Document.GetElementsByTagName("img").Count);
            foreach (HtmlElement et in web.Document.GetElementsByTagName("img"))
            {
                if(i != 0 && web.Document.GetElementsByTagName("img").Count-5 > i)
                {
                    f.Add(et.GetAttribute("src").Split('_')[0]+ "__WatermarkedWyJXYXRlcm1hcmtlZCJd.jpg");
                    Console.WriteLine("Indice: "+i+et.GetAttribute("src").Split('_')[0] + "__WatermarkedWyJXYXRlcm1hcmtlZCJd.jpg");
                }
                i++;
            }
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if(fd.ShowDialog() == DialogResult.OK)
            {
                for(int h = 0; h < f.Count; h++)
                {
                    try
                    {
                        w.DownloadFile(f[h], fd.SelectedPath + @"\" + f[h].Split('/')[f[h].Split('/').Length - 1]);
                    }
                    catch(Exception asda)
                    {

                    }
                }
                MessageBox.Show("Descarga de imagen completa!");
            }
            progressBar1.Visible = false;
            label1.Visible = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1 && listBox1.SelectedIndex <= lista.Count)
            {
                web.Navigate(lista[listBox1.SelectedIndex].url);
            }
        }

        private void change(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("Inicio.exe");
            this.Close();
        }
    }
}
