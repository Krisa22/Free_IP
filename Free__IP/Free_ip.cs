using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Free__IP
{
    public partial class Scan : Form
    {
        public Scan()
        {
            InitializeComponent();
        }
        int i;
        string ip_S;
        List<string> Lists = new List<string>();
        List<string> List_IP = new List<string>();
        private async void ip()
        {
            Invoke((MethodInvoker)async delegate
            {
                ip_S = textBox2.Text;
                for (i = 0; i <= 254; i++)
                {
                    Lists.Add("192.168." + ip_S + "." + i); // добавляем наш ip в лист, для формирования списка
                }
                var reply = Lists.Select(async ip_L =>
                {
                    await Task.Delay(100);
                    Ping ping = new Ping();
                    byte[] packet = Encoding.ASCII.GetBytes("................................"); // наш минимальный пакет для отправки
                    PingOptions packetOpt = new PingOptions(300, true);
                    for (int b = 0; b <= 2; b++)
                    {
                        var results = await ping.SendPingAsync(ip_L, 500, packet, packetOpt);

                        if (results.Status == IPStatus.Success)
                        {
                            if (checkBox1.Checked)
                            {
                                List_IP.Add("\r\n" + ip_L);
                                List_IP = List_IP.GroupBy(x => x).Select(x => x.First()).ToList();
                                textBox1.Text = "Занятые:";
                                textBox1.Text = textBox1.Text + string.Join("", List_IP);
                            }
                        }
                        else
                        {
                            if (!checkBox1.Checked)
                            {
                                List_IP.Add("\r\n" + ip_L);
                                List_IP = List_IP.GroupBy(x => x).Select(x => x.First()).ToList();
                                textBox1.Text = "Свободные:";
                                textBox1.Text = textBox1.Text + string.Join("", List_IP);
                            }
                        }
                    }
                }).ToList();
            });

        }
        private void Scann_Click(object sender, EventArgs e)
        {
            ip_S = "";
            List_IP.Clear();
            textBox1.Text = "";
            Thread ip_Th = new Thread(ip);
            ip_Th.Start();
        }

    }
}