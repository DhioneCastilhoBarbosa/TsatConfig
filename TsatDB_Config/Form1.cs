using System;
using System.IO.Ports; // biblioteca para comunicação de porta serial
using System.IO;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TsatDB_Config
{
    public partial class Form1 : Form
    {
        string teste;

        string SalvarConfig;
        string RxString;
        string CaracterMsg;
        string CaracterFormato;
        string CaracterBuffer;
        string CaracterFormatoALe;
        string CaracterContagem;
        string ultimaEnviada;
        string Terminal;



        // variaveis apontamento inicio
        double T, SA, TR, B, CR, C, AR, A1, A, R1, X, X1, BR, S2, S1, S3, S4, X2,
            ER, AA, r2, E, a2;



        double Pi_180 = 0.01745329;



        double AH = 0;



        double SO = 75;









        // fim 

        public Form1()
        {
            InitializeComponent();
            AtualizaListaCOMs();
            Portas();

            menuStrip1.Enabled = true; // Alterardo para true para demostração 
            config.Enabled = true;
            Aleatorio.Enabled = true;
            RF.Enabled = true;
            status.Visible = false;
            Gps.Visible = false;
            panelTeste.Visible = false;
            terminal.Visible = false;
            apontamento.Visible = false;
            button2.Enabled = true;
            button5.Enabled = true;
            restaurar.Enabled = true;
            progressBar1.Visible = false;
            labelProgress.Visible = false;
            buttonAtualizar.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button1.Enabled = true; 



        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void Portas() // comando
        {

            // configuração do timer 1 
            timer1.Interval = 5000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;


        }

        private void button8_Click(object sender, EventArgs e)
        {
            Coordenadas();
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            AtualizaListaCOMs();

        }



        private void Data() // atualizar a data e hora 
        {
            timer2.Interval = 5000;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e) // salvar arquivo de configuração
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {

                    using (StreamWriter sw = new StreamWriter(myStream))

                    {

                        sw.Write(SalvarConfig);

                    }


                    //codigo para escrever o stream
                    myStream.Close();
                }
            }
        }

        private void button10_Click(object sender, EventArgs e) // ler arquivo
        {

            var fileContent = string.Empty;


            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog1.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();

                        String[] array = fileContent.Split(';');
                        for (int x = 0; x <= array.Length - 1; x++)
                        {

                        }

                        textBoxID.Clear();
                        textBoxIntervalo.Clear();
                        textBoxPrimeiraH.Clear();
                        textBoxIRC.Clear();


                        textBoxID.AppendText(array[0]);
                        numericUpDown1.Value = Convert.ToInt32(array[1]);
                        comboBoxTaxa.Text = array[2];
                        textBoxIntervalo.AppendText(array[3]);
                        textBoxPrimeiraH.AppendText(array[4]);
                        numericUpDownJanela.Value = Convert.ToInt32(array[5]);

                        if (array[6] == "Y")
                        {
                            comboBoxCentraMsg.Text = "Sim";
                        }
                        else
                        {
                            comboBoxCentraMsg.Text = "Não";
                        }

                        if (array[7] == "Y")
                        {
                            comboBoxBuffer.Text = "Sim";
                        }
                        else
                        {
                            comboBoxBuffer.Text = "Não";
                        }

                        if (array[8] == "A")
                        {
                            comboBoxFormato.Text = "ASCII";
                        }

                        if (array[8] == "P")
                        {
                            comboBoxFormato.Text = "Pseudo-Binário";
                        }

                        if (array[8] == "B")
                        {
                            comboBoxFormato.Text = "Binário";
                        }

                        numericUpDownCanalAle.Value = Convert.ToInt32(array[9]);
                        comboBoxTaxaAle.Text = array[10];
                        numericUpDownIntervalAle.Value = Convert.ToInt32(array[11]);
                        numericUpDownPorcentagem.Value = Convert.ToInt32(array[12]);
                        numericUpDownRepeticao.Value = Convert.ToInt32(array[13]);

                        if (array[14] == "A")
                        {
                            comboBoxFormatAle.Text = "ASCII";
                        }

                        if (array[14] == "P")
                        {
                            comboBoxFormatAle.Text = "Pseudo-Binário";
                        }

                        if (array[14] == "B")
                        {
                            comboBoxFormatAle.Text = "Binário";
                        }

                        if (array[15] == "Y")
                        {
                            comboBoxContagem.Text = "Sim";
                        }
                        else
                        {
                            comboBoxContagem.Text = "Não";
                        }

                        textBoxIRC.AppendText(array[16]);



                    }

                }
            }






        }



        private void timer2_Tick(object sender, EventArgs e)
        {
            if (TextBox1.TextLength > 0)
            {
                DadosGPSTimer();
            }

            PortaSerial.Write("time\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("Pos\n\r"); // atualizar coordenadas
            Thread.Sleep(100);
            PortaSerial.Write("gps\n\r");
            Thread.Sleep(100);
            

        }

        private void DadosGPSTimer()
        {
            // inicio Dados Time
            textBoxTimer.Clear();

            int posicaoData = teste.IndexOf("Time=");
            string dadosData = teste.Substring(posicaoData, 25);// erro aqui 
            string[] linhaData = dadosData.Split('>');


            String[] PalavrasParaRemoverData = new String[] { "Time=" };

            for (int i = 0; i <= PalavrasParaRemoverData.Length - 1; i++)
            {

                linhaData[0] = linhaData[0].Replace(PalavrasParaRemoverData[i], String.Empty);
            }

            textBoxTimer.AppendText(linhaData[0]);


            // inicio Dados do gps
            string textoGPS = teste;

            if (teste.Contains("GPS is off"))
            {

                textBoxDadosGPS.Clear();
                textBoxDadosGPS.AppendText("GPS Desligado");
            }

            if (teste.Contains("Fix Status:"))
            {
                textBoxDadosGPS.Clear();
                int posicao0 = textoGPS.IndexOf("Fix Status:");
                string dados0 = textoGPS.Substring(posicao0, 490);
                string[] linhas = dados0.Split('\n');

                for (int x = 0; x <= 18; x++)
                {
                    textBoxDadosGPS.AppendText(linhas[x] + "\n\r");
                }
            }


            TextBox1.Clear();

        }    


        private void AtualizaListaCOMs()
        {
            int i;
            bool quantDiferente; //flag para sinalizar que a quantidade de portas mudou

            i = 0;
            quantDiferente = false;

            //se a quantidade de portas mudou
            if (ComboBox1.Items.Count == SerialPort.GetPortNames().Length)
            {
                foreach (string s in SerialPort.GetPortNames())
                {
                    if (ComboBox1.Items[i++].Equals(s) == false)
                    {
                        quantDiferente = true;
                    }
                }
            }
            else
            {
                quantDiferente = true;
            }

            //Se não foi detectado diferença
            if (quantDiferente == false)
            {
                return;                     //retorna
            }

            //limpa comboBox
            ComboBox1.Items.Clear();

            //adiciona todas as COM diponíveis na lista
            foreach (string s in SerialPort.GetPortNames())
            {
                ComboBox1.Items.Add(s);
            }
            //seleciona a primeira posição da lista
            ComboBox1.SelectedIndex = 0;

        }

        private void button3_Click_1(object sender, EventArgs e) // salvar log do terminal
        {


            Stream myStream1;
            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog2.FilterIndex = 2;
            saveFileDialog2.RestoreDirectory = true;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                if ((myStream1 = saveFileDialog2.OpenFile()) != null)
                {

                    using (StreamWriter sw = new StreamWriter(myStream1))

                    {

                        sw.Write(textBoxTerminal.Text);

                    }


                    //codigo para escrever o stream
                    myStream1.Close();
                }
            }



        }

        private void button1_Click(object sender, EventArgs e) // conectar na porta serial
        {

            

            if (PortaSerial.IsOpen == false)
            {
                try
                {
                    
                    PortaSerial.PortName = ComboBox1.Items[ComboBox1.SelectedIndex].ToString();
                    PortaSerial.Close();
                    Thread.Sleep(1000);
                    PortaSerial.Open();
                    PortaSerial.DataReceived += new SerialDataReceivedEventHandler(SerialPort1_DataReceived);
                   


                }
                catch 
                {
                   
                    
                    return;

                }
             
               
                    
              
                if (PortaSerial.IsOpen)
                {
                    TextBox1.Clear(); 
                    button1.Text = "Desconectar";
                    Comandos(); // envia comandos para baixar os dados gravado no tsat
                    menuStrip1.Enabled = true;
                    config.Enabled = true;
                    Aleatorio.Enabled = true;
                    RF.Enabled = true;
                    ComboBox1.Enabled = false;
                    menuStrip1.Enabled = true;
                    config.Enabled = true;
                    button2.Enabled = true;
                    button5.Enabled = true;
                    restaurar.Enabled = true;
                    status.Enabled = true;
                    Gps.Enabled = true;
                    apontamento.Enabled = true;
                    panelTeste.Enabled = true;
                    terminal.Enabled = true;
                    timer2.Enabled = true;
                    buttonAtualizar.Enabled = true;
                    button9.Enabled = true;
                    button10.Enabled = true;








                }
            }
            else
            {

                try
                {
                    timer2.Enabled = false;
                    TextBox1.Clear();
                    PortaSerial.Close();
                    ComboBox1.Enabled = true;
                    menuStrip1.Enabled = false;
                    config.Enabled = false;
                    Aleatorio.Enabled = false;
                    RF.Enabled = false;
                    status.Enabled = false;
                    Gps.Enabled = false;
                    apontamento.Enabled = false;
                    button2.Enabled = false;
                    button5.Enabled = false;
                    restaurar.Enabled = false;
                    button1.Text = "Conectar";
                    panelTeste.Enabled = false;
                    terminal.Enabled = false;
                    buttonAtualizar.Enabled = false;
                    button9.Enabled = false;
                    button10.Enabled = false;



                }
                catch
                {
                    return;
                }

            }


            

        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            

            RxString = PortaSerial.ReadExisting(); //le o dado disponível na serial  

             this.Invoke(new EventHandler(TrataDadoRecebido));   //chama outra thread para escrever o dado no text box


        }


        private void TrataDadoRecebido(object sender, EventArgs e)
        {
            
            TextBox1.AppendText(RxString); // exibe a string na tela
            teste = TextBox1.Text;
            Terminal = string.Empty;

            Terminal = RxString;

            textBoxTerminal.AppendText(Terminal);

        }

       

        private void myTextBox_KeyPress(object sender, KeyPressEventArgs e) // função terminal 

        {

       
            if (e.KeyChar == 13)
            {
                Terminal =string.Empty;
                string Str = (textBoxTerminal.Text);


                string[] palavras = Str.Split('>'); 
                ultimaEnviada = palavras.Last(); // pega apenas a ultima palavra da textbox multiline

                if (String.IsNullOrEmpty(ultimaEnviada))
                {
                    Thread.Sleep(200);
                    PortaSerial.Write("\r");
                    Thread.Sleep(200);
                    Terminal = string.Empty;
                }
                else
                {
                    Thread.Sleep(200);
                    PortaSerial.Write(ultimaEnviada +"\r\n" );
                    Thread.Sleep(200);
                    Terminal = string.Empty;


                    String[] PalavrasParaRemoverData = new String[] { ultimaEnviada };

                    for (int i = 0; i <= PalavrasParaRemoverData.Length - 1; i++)
                    {

                        RxString = RxString.Replace(PalavrasParaRemoverData[i], String.Empty);
                    }

                }

            }



        }

        



        private void configurarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
            PortaSerial.Write("time\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("Pos\n\r"); // atualizar coordenadas
            Thread.Sleep(100);
            PortaSerial.Write("gps\n\r");
            config.Visible = true;
            Aleatorio.Visible = true;
            RF.Visible = true;
            status.Visible = false;
            Gps.Visible = false;
            apontamento.Visible = false;
            panelTeste.Visible = false;
            terminal.Visible = false;
          
        }

        private void gPSToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            timer2.Enabled = true;
            PortaSerial.Write("time\n\r"); // atualizar horario
            Thread.Sleep(100);
            PortaSerial.Write("Pos\n\r"); // atualizar coordenadas
            Thread.Sleep(100);
            PortaSerial.Write("gps\n\r");
            status.Visible = false;
            Gps.Visible = true;
            apontamento.Visible = false;
            config.Visible = false;
            Aleatorio.Visible = false;
            RF.Visible = false;
            panelTeste.Visible = false;
            terminal.Visible = false;



        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e) // aba status
        {
            timer2.Enabled = true;
            PortaSerial.Write("time\n\r"); // atualizar horario
            Thread.Sleep(100);
            PortaSerial.Write("Pos\n\r"); // atualizar coordenadas
            Thread.Sleep(100);
            PortaSerial.Write("gps\n\r");
            Gps.Visible = false;
            status.Visible = true;
            config.Visible = false;
            Aleatorio.Visible = false;
            RF.Visible = false;
            apontamento.Visible = false;
            panelTeste.Visible = false;
            terminal.Visible = false;

        }

        private void apontamentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
            PortaSerial.Write("time\n\r"); // atualizar horario
            Thread.Sleep(100);
            PortaSerial.Write("Pos\n\r"); // atualizar coordenadas
            Thread.Sleep(100);
            PortaSerial.Write("gps\n\r");
            Gps.Visible = false;
            status.Visible = false;
            config.Visible = false;
            Aleatorio.Visible = false;
            RF.Visible = false;
            apontamento.Visible = true;
            panelTeste.Visible = false;
            terminal.Visible = false;
        }


        private void testeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
            PortaSerial.Write("time\n\r"); // atualizar horario
            Thread.Sleep(100);
            PortaSerial.Write("Pos\n\r"); // atualizar coordenadas
            Thread.Sleep(100);
            PortaSerial.Write("gps\n\r");
            Gps.Visible = false;
            status.Visible = false;
            config.Visible = false;
            Aleatorio.Visible = false;
            RF.Visible = false;
            apontamento.Visible = false;
            panelTeste.Visible = true;
            panelTeste.Enabled = true;
            terminal.Visible = false;
        }
        private void terminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            TextBox1.Clear();
            textBoxTerminal.Clear();
            Terminal = " ";
            Gps.Visible = false;
            status.Visible = false;
            config.Visible = false;
            Aleatorio.Visible = false;
            RF.Visible = false;
            apontamento.Visible = false;
            panelTeste.Visible = false;
            terminal.Visible = true;
        }

        private void buttonAtualizar_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            TextBox1.Clear();
            Comandos(); // envia comandos para baixar os dados gravado no tsat
            carregarDados();
            timer2.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e) // salvar config de potencia TX
        {
            timer2.Enabled = false;
            string pot100 = textBox100.Text;
            string pot300 = textBox300.Text;
            string pot1200 = textBox1200.Text;


            PortaSerial.Write("techmode alpha\n\r");
            PortaSerial.Write("pwrlvl=" + pot100 +","+ pot300 +","+pot1200 +"\n\r");
            PortaSerial.Write("save\n\r");
            PortaSerial.Write("savecal\n\r");
            PortaSerial.Write("usermode\n\r");
            PortaSerial.Write("save\n\r");
            timer2.Enabled = true;
        }


        private void button5_Click(object sender, EventArgs e) // botão iniciar teste 
        {
           
            TesteTX();

        }
        private void TesteTX()
        {
            timer2.Enabled = false;
            string textoID = textBoxTestID.Text;              //textBoxID.Text;

            string textoTaxa = "300";           //comboBoxTaxa.Text;
            string textoIntervalo = "00:01:00:00";        //textBoxIntervalo.Text;
            string textoPHorario = "00:10:00";        //textBoxPrimeiraH.Text;
            string janela = "10";
            string texCanal = numericUpDownTestCanal.Value.ToString();             //Canal.ToString();
            CaracterMsg = "N";
            CaracterFormato = "A";
            CaracterBuffer = "N";
            string mensagem = textBoxTestMsg.Text;
            string numeroSerial = textBoxSN.Text;
            string firmware = textBoxFirware.Text;

            string msgCompleta = mensagem + numeroSerial + firmware;        
            PortaSerial.Write("NESID=" + textoID + "\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("TCH=" + texCanal + "\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("TBR=" + textoTaxa + "\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("TIN=" + textoIntervalo + "\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("FTT=" + textoPHorario + "\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("TWL=" + janela + "\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("CMSG=" + CaracterMsg + "\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("TDF=" + CaracterFormato + "\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("EBM=" + CaracterBuffer + "\n\r");
            Thread.Sleep(100);
            PortaSerial.Write("ETX \n\r");
            Thread.Sleep(300);
            PortaSerial.Write("TDT=" + msgCompleta +  "\n\r");
            timer2.Enabled = true;




        }

             
        private void TestePalavra()
        {
            string Msg = comboBoxCentraMsg.Text;
            string MsgFormato = comboBoxFormato.Text;
            string MsgBuffer = comboBoxBuffer.Text;
            string MsgFormatALe = comboBoxFormatAle.Text;
            string MsgContagem = comboBoxContagem.Text;
            if (Msg == "Sim") // inicio teste da msg da centralização 
            {
                CaracterMsg = "Y";

            }
            else
            {
                CaracterMsg = "N";
            }
            // fim 

            // inicio teste do formato de transmissão

            if (MsgFormato == "ASCII")
            {
                CaracterFormato = "A";

            }
            if (MsgFormato == "Pseudo-Binário")
            {
                CaracterFormato = "P";
            }

            if (MsgFormato == "Binário")
            {
                CaracterFormato = "B";
            }

            //fim

            // incio teste msg do buffer 
            if (MsgBuffer == "Sim") 
            {
                CaracterBuffer= "Y";

            }
            else
            {
                CaracterBuffer = "N";
            }
            // fim 


            if (MsgFormatALe == "ASCII") 
            {
                CaracterFormatoALe = "A";

            }

            if (MsgFormatALe == "Pseudo-Binário")
            {
                CaracterFormatoALe = "P";

            }

            if (MsgFormatALe == "Binário")
            {
                CaracterFormatoALe = "B";

            }

            if (MsgContagem == "Sim")
            {
                CaracterContagem = "Y";

            }
            else
            {
                CaracterContagem= "N";
            }



        }



        private void button2_Click(object sender, EventArgs e) // botão aplicar 
        {
            timer2.Enabled = false;
            // Comandos config temporizada inicio 
            TestePalavra();
            string textoID = textBoxID.Text;
            Decimal Canal = numericUpDown1.Value;
            Decimal Janela = numericUpDownJanela.Value;
            string textoTaxa = comboBoxTaxa.Text;
            string textoIntervalo = textBoxIntervalo.Text;
            string textoPHorario = textBoxPrimeiraH.Text;
            string texCanal = Canal.ToString();
            string textoJanela = Janela.ToString();
            string textoCorrGPS = textBoxIntCorrecao.Text;
            PortaSerial.Write("NESID=" + textoID + "\n\r");
            PortaSerial.Write("TCH=" + texCanal + "\n\r");
            PortaSerial.Write("TBR=" + textoTaxa + "\n\r");
            PortaSerial.Write("TIN=" + textoIntervalo + "\n\r");
            PortaSerial.Write("FTT=" + textoPHorario + "\n\r");
            PortaSerial.Write("TWL=" + textoJanela + "\n\r");
            PortaSerial.Write("CMSG=" + CaracterMsg + "\n\r");
            PortaSerial.Write("TDF=" + CaracterFormato + "\n\r");
            PortaSerial.Write("EBM=" + CaracterBuffer + "\n\r");
            PortaSerial.Write("GIN=" + textoCorrGPS +"\n\r");
            // fim 

            // comando config aleatorio inicio 

            Decimal canalALe = numericUpDownCanalAle.Value;
            string textotaxaAle = comboBoxTaxaAle.Text;
            Decimal InterALE = numericUpDownIntervalAle.Value;
            Decimal Porcent = numericUpDownPorcentagem.Value;
            Decimal repet = numericUpDownRepeticao.Value;
            string textoIRC = textBoxIRC.Text;
            string textoCanalALe = canalALe.ToString();
            string textoInterALe = InterALE.ToString();
            string textoPorcent = Porcent.ToString();
            string textoRepet = repet.ToString();
            PortaSerial.Write("RCH=" + textoCanalALe + "\n\r");
            PortaSerial.Write("RBR=" + textotaxaAle + "\n\r");
            PortaSerial.Write("RIN=" + textoInterALe + "\n\r");
            PortaSerial.Write("RPC=" + textoPorcent + "\n\r");
            PortaSerial.Write("RRC=" + textoRepet + "\n\r");
            PortaSerial.Write("RDF=" + CaracterFormatoALe + "\n\r");
            PortaSerial.Write("RMC=" + CaracterContagem + "\n\r");
            PortaSerial.Write("IRC=" + textoIRC + "\n\r");

            MessageBox.Show("Configuração enviada com sucesso.");


            timer2.Enabled = true;


        }

        private void Comandos() // lista de comandos para carregar informações do tsat
        {
            
            timer2.Enabled = false;
            labelProgress.Enabled = true;
            labelProgress.Visible = true;
            progressBar1.Visible = true;
            PortaSerial.Write("rcfg\n\r");
            progressBar1.Value = 10;
            Thread.Sleep(300);
            PortaSerial.Write("RST\n\r");
            progressBar1.Value = 20;
            Thread.Sleep(300);
            PortaSerial.Write("gps\n\r"); 
            progressBar1.Value = 30;
            Thread.Sleep(300);
            PortaSerial.Write("Pos\n\r");
            progressBar1.Value = 40;
            Thread.Sleep(300);
            PortaSerial.Write("ver\n\r");
            progressBar1.Value = 50;
            Thread.Sleep(300);
            PortaSerial.Write("time\n\r");
            progressBar1.Value = 60;
            Thread.Sleep(300);
            PortaSerial.Write("rtemp\n\r");
            progressBar1.Value = 70;
            Thread.Sleep(1000);
            PortaSerial.Write("techmode alpha\n\r");
            progressBar1.Value = 80;
            Thread.Sleep(300);
            PortaSerial.Write("PWRLVL\n\r");
            progressBar1.Value = 90;
            Thread.Sleep(300);
            PortaSerial.Write("usermode\n\r");
            Thread.Sleep(300);
            PortaSerial.Write("save\n\r");
            Thread.Sleep(1000);
            progressBar1.Value = 100;
            progressBar1.Visible = false;
            labelProgress.Visible = false;

          
            
           while (String.IsNullOrEmpty(TextBox1.Text))
           {

                    MessageBox.Show("Dados carregado com sucesso!!!");

           }

           carregarDados();
           Data();

           

        }

        

        private void Coordenadas()
        {
            textBoxLat.Clear();// limpar os campos para atualizar 
            textBoxLon.Clear();
            textBoxAlt.Clear();
            textBoxApontLong.Clear();
            textBoxApotlat.Clear();
            textBoxGPSTime.Clear();

            if (TextBox1.TextLength > 0) // se tiver dados na textbox1 
            {
                if (teste.Contains("No GPS fix"))
                {

                    textBoxGPSTime.AppendText("1996/01/01 00:02:05");
                    textBoxLat.AppendText("00.00");
                    textBoxApotlat.AppendText("00.00");
                    textBoxLon.AppendText("00.00");
                    textBoxApontLong.AppendText("00.00");
                    textBoxAlt.AppendText("00.00");
                    labelAlertaGPS.Visible = true;
                    labelAlertaGPS.Enabled = true;
                    button4.Enabled = false;
                    button8.Enabled = false;



                }
                else
                {

                    labelAlertaGPS.Visible = false;
                    labelAlertaGPS.Enabled = false;
                    button4.Enabled = true;
                    button8.Enabled = true;
                    int posicao1 = teste.IndexOf("fix:");
                    int posicao2 = teste.IndexOf("Lat:");
                    int posicao3 = teste.IndexOf("Long:");
                    int posicao4 = teste.IndexOf("Alt:");

                    string dados1 = (teste.Substring(posicao1, 24));
                    string dados2 = (teste.Substring(posicao2, 14));
                    string dados3 = (teste.Substring(posicao3, 14));
                    string dados4 = (teste.Substring(posicao4, 10));



                    String[] PalavrasParaRemover1 = new String[] { "fix:" };

                    for (int i = 0; i <= PalavrasParaRemover1.Length - 1; i++)
                    {

                        dados1 = dados1.Replace(PalavrasParaRemover1[i], String.Empty);
                    }

                    String[] PalavrasParaRemover2 = new String[] { "Lat:" };

                    for (int i = 0; i <= PalavrasParaRemover2.Length - 1; i++)
                    {

                        dados2 = dados2.Replace(PalavrasParaRemover2[i], String.Empty);
                    }


                    String[] PalavrasParaRemover3 = new String[] { "Long:" };

                    for (int i = 0; i <= PalavrasParaRemover3.Length - 1; i++)
                    {

                        dados3 = dados3.Replace(PalavrasParaRemover3[i], String.Empty);
                    }

                    String[] PalavrasParaRemover4 = new String[] { "Alt:" };

                    for (int i = 0; i <= PalavrasParaRemover4.Length - 1; i++)
                    {

                        dados4 = dados4.Replace(PalavrasParaRemover4[i], String.Empty);
                    }

                    textBoxGPSTime.AppendText(dados1);
                    textBoxLat.AppendText(dados2);
                    textBoxApotlat.AppendText(dados2);
                    textBoxLon.AppendText(dados3);
                    textBoxApontLong.AppendText(dados3);
                    textBoxAlt.AppendText(dados4);
                }
            }
        }

        private void carregarDados() // função para carregar dados que esta armazenados no tsat
        {

            if (TextBox1.TextLength > 0) // se tiver dados na textbox1 
            {
                textBoxGPSstatus.Clear();// limpar os campos para atualizar 
                textBoxSN.Clear();
                textBoxTemperatura.Clear();
                textBoxTensao.Clear();
                textBoxTimebuffer.Clear();
                textBoxTimer.Clear();
                textBoxRandoBuffer.Clear();
                textBoxHardver.Clear();
                textBoxFirware.Clear();
                textBoxFailSafe.Clear();
                textBoxFirware.Clear();
                textBoxLastTransmit.Clear();
                textBoxHabTx.Clear();
                textBoxNextTime.Clear();
                textBoxRando.Clear();
                textBoxID.Clear();
                textBoxIntervalo.Clear();
                textBoxPrimeiraH.Clear();
                textBoxIRC.Clear();
                textBox100.Clear();
                textBox1200.Clear();
                textBox300.Clear();
                textBoxHabTx.Clear();
                textBoxApontLong.Clear();
                textBoxApotlat.Clear();



                int posicao = teste.IndexOf("Transmitter:");

                int posicao5 = teste.IndexOf("GPS:");
                int posicao6 = teste.IndexOf("Number:");
                int posicao7 = teste.IndexOf("Hardware Version:");
                int posicao8 = teste.IndexOf("Firmware Version:");
                int posicao9 = teste.IndexOf("Time=");
                int posicao10 = teste.IndexOf("Timed Tx:");
                int posicao11 = teste.IndexOf("Random Tx:");
                int posicao12 = teste.IndexOf("Failsafe:");
                int posicao13 = teste.IndexOf("voltage:");
                int posicao14 = teste.IndexOf("Temp =");
                int posicao15 = teste.IndexOf("Timed Message Length:");
                int posicao16 = teste.IndexOf("Random Message Length:");
                int posicao17 = teste.IndexOf("Next Tx:");
                int posicao18 = teste.IndexOf("NESID="); // dados da temporizada 
                int posicao19 = teste.IndexOf("TCH=");
                int posicao20 = teste.IndexOf("TBR=");
                int posicao21 = teste.IndexOf("TIN=");
                int posicao22 = teste.IndexOf("FTT=");
                int posicao23 = teste.IndexOf("TWL=");
                int posicao24 = teste.IndexOf("CMSG=");
                int posicao25 = teste.IndexOf("EBM=");
                int posicao26 = teste.IndexOf("TDF=");
                // fim
                int posicao27 = teste.IndexOf("RCH=");// dados da aleatoria 
                int posicao28 = teste.IndexOf("RBR=");
                int posicao29 = teste.IndexOf("RIN=");
                int posicao30 = teste.IndexOf("RPC=");
                int posicao31 = teste.IndexOf("RRC=");
                int posicao32 = teste.IndexOf("RDF=");
                int posicao33 = teste.IndexOf("RMC=");
                int posicao34 = teste.IndexOf("IRC=");
                //fim
                int posicao35 = teste.IndexOf("PWRLVL="); // potencia tx


                string dados = (teste.Substring(posicao, 20));

                string dados5 = (teste.Substring(posicao5, 8));
                string dados6 = (teste.Substring(posicao6, 17));
                string dados7 = (teste.Substring(posicao7, 24));
                string dados8 = (teste.Substring(posicao8, 36));
                string dados9 = (teste.Substring(posicao9, 25));
                string dados10 = (teste.Substring(posicao10, 28));
                string dados11 = (teste.Substring(posicao11, 16));
                string dados12 = (teste.Substring(posicao12, 13));
                string dados13 = (teste.Substring(posicao13, 17));
                string dados14 = (teste.Substring(posicao14, 16));
                string dados15 = (teste.Substring(posicao15, 31));
                string dados16 = (teste.Substring(posicao16, 31));
                string dados17 = (teste.Substring(posicao17, 14));
                string dados18 = (teste.Substring(posicao18, 14));
                string dados19 = (teste.Substring(posicao19, 7));
                string dados20 = (teste.Substring(posicao20, 8));
                string dados21 = (teste.Substring(posicao21, 15));
                string dados22 = (teste.Substring(posicao22, 12));
                string dados23 = (teste.Substring(posicao23, 6));
                string dados24 = (teste.Substring(posicao24, 6));
                string dados25 = (teste.Substring(posicao25, 5));
                string dados26 = (teste.Substring(posicao26, 5));
                string dados27 = (teste.Substring(posicao27, 7));
                string dados28 = (teste.Substring(posicao28, 7));
                string dados29 = (teste.Substring(posicao29, 5));
                string dados30 = (teste.Substring(posicao30, 6));
                string dados31 = (teste.Substring(posicao31, 5));
                string dados32 = (teste.Substring(posicao32, 5));
                string dados33 = (teste.Substring(posicao33, 5));
                string dados34 = (teste.Substring(posicao34, 5));
                string dados35 = (teste.Substring(posicao35, 24));
                string []linhaSerial = dados6.Split('\n');
                string[] linhaT_temporizada = dados10.Split('\n');
                

                String[] PalavrasParaRemover = new String[] { "Transmitter:" };

                for (int i = 0; i <= PalavrasParaRemover.Length - 1; i++)
                {
                    dados = dados.Replace(PalavrasParaRemover[i], String.Empty);

                }



                String[] PalavrasParaRemover5 = new String[] { "GPS:" };

                for (int i = 0; i <= PalavrasParaRemover5.Length - 1; i++)
                {

                    dados5 = dados5.Replace(PalavrasParaRemover5[i], String.Empty);
                }


                String[] PalavrasParaRemover6 = new String[] { "Number:" };

                for (int i = 0; i <= PalavrasParaRemover6.Length - 1; i++)
                {
                    linhaSerial[0] = linhaSerial[0].Replace(PalavrasParaRemover6[i], String.Empty);
                    
                }

                String[] PalavrasParaRemover7 = new String[] { "Hardware Version:" };

                for (int i = 0; i <= PalavrasParaRemover7.Length - 1; i++)
                {


                    dados7 = dados7.Replace(PalavrasParaRemover7[i], String.Empty);

                }

                String[] PalavrasParaRemover8 = new String[] { "Firmware Version:" };

                for (int i = 0; i <= PalavrasParaRemover8.Length - 1; i++)
                {

                    dados8 = dados8.Replace(PalavrasParaRemover8[i], String.Empty);
                }

                String[] PalavrasParaRemover9 = new String[] { "Time=" };

                for (int i = 0; i <= PalavrasParaRemover9.Length - 1; i++)
                {

                    dados9 = dados9.Replace(PalavrasParaRemover9[i], String.Empty);
                }


                String[] PalavrasParaRemover10 = new String[] { "Timed Tx:" };

                for (int i = 0; i <= PalavrasParaRemover10.Length - 1; i++)
                {

                    linhaT_temporizada[0] = linhaT_temporizada[0].Replace(PalavrasParaRemover10[i], String.Empty);
                }

                String[] PalavrasParaRemover11 = new String[] { "Random Tx:" };

                for (int i = 0; i <= PalavrasParaRemover11.Length - 1; i++)
                {

                    dados11 = dados11.Replace(PalavrasParaRemover11[i], String.Empty);
                }


                String[] PalavrasParaRemover12 = new String[] { "Failsafe:" };

                for (int i = 0; i <= PalavrasParaRemover12.Length - 1; i++)
                {

                    dados12 = dados12.Replace(PalavrasParaRemover12[i], String.Empty);
                }

                String[] PalavrasParaRemover13 = new String[] { "voltage:" };

                for (int i = 0; i <= PalavrasParaRemover13.Length - 1; i++)
                {

                    dados13 = dados13.Replace(PalavrasParaRemover13[i], String.Empty);
                }

                String[] PalavrasParaRemover14 = new String[] { "Temp =" };

                for (int i = 0; i <= PalavrasParaRemover14.Length - 1; i++)
                {

                    dados14 = dados14.Replace(PalavrasParaRemover14[i], String.Empty);
                }

                String[] PalavrasParaRemover15 = new String[] { "Timed Message Length:" };

                for (int i = 0; i <= PalavrasParaRemover15.Length - 1; i++)
                {

                    dados15= dados15.Replace(PalavrasParaRemover15[i], String.Empty);
                }

                String[] PalavrasParaRemover16 = new String[] { "Random Message Length:" };

                for (int i = 0; i <= PalavrasParaRemover15.Length - 1; i++)
                {

                    dados16 = dados16.Replace(PalavrasParaRemover16[i], String.Empty);
                }

                String[] PalavrasParaRemover17 = new String[] { "Next Tx:" };

                for (int i = 0; i <= PalavrasParaRemover17.Length - 1; i++)
                {

                    dados17 = dados17.Replace(PalavrasParaRemover17[i], String.Empty);
                }


                String[] PalavrasParaRemover18 = new String[] { "NESID=" };

                for (int i = 0; i <= PalavrasParaRemover18.Length - 1; i++)
                {

                    dados18 = dados18.Replace(PalavrasParaRemover18[i], String.Empty);
                }

                String[] PalavrasParaRemover19 = new String[] { "TCH=" };

                for (int i = 0; i <= PalavrasParaRemover19.Length - 1; i++)
                {

                    dados19 = dados19.Replace(PalavrasParaRemover19[i], String.Empty);

                }

                String[] PalavrasParaRemover20 = new String[] { "TBR=" };

                for (int i = 0; i <= PalavrasParaRemover20.Length - 1; i++)
                {

                    dados20 = dados20.Replace(PalavrasParaRemover20[i], String.Empty);

                }

                String[] PalavrasParaRemover21 = new String[] { "TIN=" };

                for (int i = 0; i <= PalavrasParaRemover21.Length - 1; i++)
                {

                    dados21 = dados21.Replace(PalavrasParaRemover21[i], String.Empty);

                }

                String[] PalavrasParaRemover22 = new String[] { "FTT=" };

                for (int i = 0; i <= PalavrasParaRemover22.Length - 1; i++)
                {

                    dados22 = dados22.Replace(PalavrasParaRemover22[i], String.Empty);

                }

                String[] PalavrasParaRemover23 = new String[] { "TWL=" };

                for (int i = 0; i <= PalavrasParaRemover23.Length - 1; i++)
                {

                    dados23 = dados23.Replace(PalavrasParaRemover23[i], String.Empty);

                }
                String[] PalavrasParaRemover24 = new String[] { "CMSG=" };

                for (int i = 0; i <= PalavrasParaRemover24.Length - 1; i++)
                {

                    dados24 = dados24.Replace(PalavrasParaRemover24[i], String.Empty);

                    if (dados24 == "Y")
                    {
                        comboBoxCentraMsg.Text = "Sim";
                    }
                    else
                    {
                        comboBoxCentraMsg.Text = "Não";
                    }
                }


                String[] PalavrasParaRemover25 = new String[] { "EBM=" };

                for (int i = 0; i <= PalavrasParaRemover25.Length - 1; i++)
                {

                    dados25 = dados25.Replace(PalavrasParaRemover25[i], String.Empty);

                    if (dados25 == "Y")
                    {
                        comboBoxBuffer.Text = "Sim";
                    }
                    else
                    {
                        comboBoxBuffer.Text = "Não";
                    }
                }


                String[] PalavrasParaRemover26 = new String[] { "TDF=" };

                for (int i = 0; i <= PalavrasParaRemover26.Length - 1; i++)
                {

                    dados26 = dados26.Replace(PalavrasParaRemover26[i], String.Empty);

                    if (dados26 == "A")
                    {
                        comboBoxFormato.Text = "ASCII";
                    }

                    if (dados26 == "P")
                    {
                        comboBoxFormato.Text = "Pseudo-Binário";
                    }

                    if (dados26 == "B")
                    {
                        comboBoxFormato.Text = "Binário";
                    }
                }


                String[] PalavrasParaRemover27 = new String[] { "RCH=" };

                for (int i = 0; i <= PalavrasParaRemover27.Length - 1; i++)
                {

                    dados27 = dados27.Replace(PalavrasParaRemover27[i], String.Empty);

                }

                String[] PalavrasParaRemover28 = new String[] { "RBR=" };

                for (int i = 0; i <= PalavrasParaRemover28.Length - 1; i++)
                {

                    dados28 = dados28.Replace(PalavrasParaRemover28[i], String.Empty);

                }


                String[] PalavrasParaRemover29 = new String[] { "RIN=" };

                for (int i = 0; i <= PalavrasParaRemover29.Length - 1; i++)
                {

                    dados29 = dados29.Replace(PalavrasParaRemover29[i], String.Empty);

                }

                String[] PalavrasParaRemover30 = new String[] { "RPC=" };

                for (int i = 0; i <= PalavrasParaRemover30.Length - 1; i++)
                {

                    dados30 = dados30.Replace(PalavrasParaRemover30[i], String.Empty);

                }

                String[] PalavrasParaRemover31 = new String[] { "RRC=" };

                for (int i = 0; i <= PalavrasParaRemover31.Length - 1; i++)
                {

                    dados31 = dados31.Replace(PalavrasParaRemover31[i], String.Empty);

                }

                String[] PalavrasParaRemover32 = new String[] { "RDF=" };

                for (int i = 0; i <= PalavrasParaRemover32.Length - 1; i++)
                {

                    dados32 = dados32.Replace(PalavrasParaRemover32[i], String.Empty);

                    if (dados32 == "A")
                    {
                        comboBoxFormatAle.Text = "ASCII";
                    }

                    if (dados32 == "P")
                    {
                        comboBoxFormatAle.Text = "Pseudo-Binário";
                    }

                    if (dados32 == "B")
                    {
                        comboBoxFormatAle.Text = "Binário";
                    }

                }


                String[] PalavrasParaRemover33 = new String[] { "RMC=" };

                for (int i = 0; i <= PalavrasParaRemover31.Length - 1; i++)
                {

                    dados33 = dados33.Replace(PalavrasParaRemover33[i], String.Empty);

                    if (dados33 == "Y")
                    {
                        comboBoxContagem.Text = "Sim";
                    }
                    else
                    {
                        comboBoxContagem.Text = "Não";
                    }

                }

                String[] PalavrasParaRemover34 = new String[] { "IRC=" };

                for (int i = 0; i <= PalavrasParaRemover34.Length - 1; i++)
                {

                    dados34 = dados34.Replace(PalavrasParaRemover34[i], String.Empty);

                }




                String[] PalavrasParaRemover35 = new String[] { "PWRLVL=" };

                for (int i = 0; i <= PalavrasParaRemover35.Length - 1; i++)
                {
                    dados35 = dados35.Replace(PalavrasParaRemover35[i], String.Empty);



                    String[] array = dados35.Split(',');
                    for (int x = 0; x <= array.Length - 1; x++)
                    {

                    }

                    textBox100.AppendText(array[0]);
                    textBox300.AppendText(array[1]);
                    textBox1200.AppendText(array[2]);


                }


                textBoxHabTx.AppendText(dados);
                textBoxGPSstatus.AppendText(dados5);
                textBoxSN.AppendText(linhaSerial[0]);
                textBoxHardver.AppendText(dados7);
                textBoxFirware.AppendText(dados8);
                textBoxTimer.AppendText(dados9);
                textBoxNextTime.AppendText(linhaT_temporizada[0]);
                textBoxRando.AppendText(dados11);
                textBoxFailSafe.AppendText(dados12);
                textBoxTensao.AppendText(dados13);
                textBoxTemperatura.AppendText(dados14);
                textBoxTimebuffer.AppendText(dados15);
                textBoxRandoBuffer.AppendText(dados16);
                textBoxLastTransmit.AppendText(dados17);
                textBoxID.AppendText(dados18);
                numericUpDown1.Value = Convert.ToInt32(dados19);
                comboBoxTaxa.Text = dados20;
                textBoxIntervalo.AppendText(dados21);
                textBoxPrimeiraH.AppendText(dados22);
                numericUpDownJanela.Value = Convert.ToInt32(dados23);
                numericUpDownCanalAle.Value = Convert.ToInt32(dados27);
                comboBoxTaxaAle.Text = dados28;
                numericUpDownIntervalAle.Value = Convert.ToInt32(dados29);
                numericUpDownPorcentagem.Value = Convert.ToInt32(dados30);
                numericUpDownRepeticao.Value = Convert.ToInt32(dados31);
                textBoxIRC.AppendText(dados34);

                Coordenadas();



                SalvarConfig = dados18 + ";" + dados19 + ";" + dados20 + ";" + dados21 + ";" + dados22 + ";" + dados23 + ";" + dados24 + ";" + dados25 + ";" + dados26; // dados temporizados
                SalvarConfig = SalvarConfig + ";" + dados27 + ";" + dados28 + ";" + dados29 + ";" + dados30 + ";" + dados31 + ";" + dados32 + ";" + dados33 + ";" + dados34; // adicionando dados aleatorio

                String[] PalavrasParaRemover36 = new String[] { "\r\n" };

                for (int i = 0; i <= PalavrasParaRemover36.Length - 1; i++)
                {

                    SalvarConfig = SalvarConfig.Replace(PalavrasParaRemover36[i], String.Empty);

                }


            }
            else { // vai fazer nada 
            }










        }









        private void button3_Click(object sender, EventArgs e)
        {
            PortaSerial.Write("DEFAULT" + "\n\r"); // COMANDO PARA RESETAR PADRÃO DE FABRICA 
            PortaSerial.Write("SAVE" + "\n\r"); // COMANDO PARA SALVAR COAMNDO 
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBoxAzi.Clear();
            textBoxElev.Clear();
            Apontamento();

        }


        private void Apontamento() // função para apontamento da antena 
        {
            double Aa, Sa;
            float Lat = float.Parse(textBoxApotlat.Text);
            float Long = float.Parse(textBoxApontLong.Text);

            SA = Long * -1;

            AA = Lat;

            Aa = AA;
            Sa = SA;
            AH = AH * 3.3;

            A = 90 - Aa;
            T = SO - Sa;
            TR = T * Pi_180;
            BR = 90 * Pi_180;
            AR = A * Pi_180;
            X = Math.Cos(AR) * Math.Cos(BR) + Math.Sin(AR) * Math.Sin(BR) * Math.Cos(TR);
            CR = -Math.Atan(X / Math.Sqrt(-X * X + 1)) + 1.5708;

            C = CR * (1 / Pi_180);
            X1 = (Math.Sin(BR) * Math.Sin(TR)) / Math.Sin(CR);
            BR = Math.Atan(X1 / Math.Sqrt(-X1 * X1 + 1));
            B = BR * (1 / Pi_180);

            if (T < 0 && Aa > 0)
            {
                B = B + 180;
            }
             if (T < 0 && Aa < 0)
            {
                B = B * -1;
            }
            if (T > 0 && Aa < 0)
            {
                B = 360 - B;
            }
            if (T > 0 && Aa > 0)
            {
                B = B + 180;
            }
            if (T == 0 && Aa > 0)
            {
                B = 180;
            }
            if (T == 0 && Aa < 0)
            {
                B = 360;
            }
            if (Aa == 0 && T > 0)
            {
                B = 270;
            }
            if (Aa == 0 && T < 0)
            {
                B = 90;
            }

            A1 = 90 - C;
            R1 = A1 * Pi_180;
            S1 = (6378 + (AH * 0.0003048)) / Math.Sin(R1);
            S2 = 35785 + 6578 - S1;
            a2 = 180 - A1;
            r2 = a2 * Pi_180;
            S4 = Math.Sqrt(S1 * S1 - Math.Pow((6378 + AH * 0.0003048), 2));
            S3 = Math.Sqrt(Math.Pow(S4, 2) + Math.Pow(S2, 2) - 2 * S4 * S2 * Math.Cos(r2));
            X2 = (Math.Sin(r2) / S3) * S2;
            ER = Math.Atan(X2 / Math.Sqrt(-X2 * X2 + 1));
            E = ER * (1 / Pi_180);
            

       
            String Az = Convert.ToString((int) B);
            String EL = Convert.ToString((int) E);
            String Azimute = Az;
            String Elevacao = EL;

            textBoxAzi.AppendText(Azimute);
            textBoxElev.AppendText(Elevacao);




        }
    }


    /*
      // bug:

    informações dos dados do gps não limpo quando entra em modo terminal quando o gps esta fazendo a sincronização.
    desativar botão atualizar coordenadas pois se clicar quando esta sincronizando gera erro na obtenção de informação.

    

    // Adicionar:
    Botão para salvar log de informações do terminal.
    Timer e contagem de tempo da função teste.
    adicionar função de armazenamento da configuração anterior e readicionar na finalização do teste.

    OBS: quando finalizado e operacional deixa o programa resposivo  
     
     */


    // the Serial Port detection routine 
   


}
