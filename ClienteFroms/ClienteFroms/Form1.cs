using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace ClienteFroms
{
    public partial class Form1 : Form
    {
        TcpClient Cliente;
        NetworkStream StreamCliente;
        string mensaje;
        public Form1()
        {
            InitializeComponent();
        }

        private void btConectar_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente = new TcpClient("127.0.0.1", 8888);
                StreamCliente = Cliente.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(tbNombre.Text);
                StreamCliente.Write(data, 0, data.Length);
                StreamCliente.Flush();
               mensaje = "Conectando Con el Servidor";
               Mensaje_REcivido();
               Thread Hilo = new Thread(Recivir_Mensaje);
               Hilo.Start();

            }
            catch 
            {
                MessageBox.Show("Error Al conectar");
            }

        }

        private void btEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                StreamCliente = Cliente.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(tbEnviar.Text);
                StreamCliente.Write(data, 0, data.Length);
                StreamCliente.Flush();
            }
            catch
            {
                MessageBox.Show("No pudo enviar el msj");
            }
        }
        public void Mensaje_REcivido()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Mensaje_REcivido));
            else
                tbChat.Text += "\n  =>>" + mensaje;
        }

        private void Recivir_Mensaje()
        { 
            while(true)
            {
                StreamCliente = Cliente.GetStream();
                byte[] bit = new byte[140];
                StreamCliente.Read(bit, 0, bit.Length);
                mensaje = Encoding.ASCII.GetString(bit);
                Mensaje_REcivido();

            }
        
        }
    }
}
