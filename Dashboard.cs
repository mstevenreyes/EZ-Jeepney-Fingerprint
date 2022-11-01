using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libzkfpcsharp;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using Sample;
using SourceAFIS;
using System.Drawing.Imaging;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace Demo
{
    public partial class Dashboard : Form
    {
        IntPtr mDevHandle = IntPtr.Zero;
        IntPtr mDBHandle = IntPtr.Zero;
        IntPtr FormHandle = IntPtr.Zero;
        bool bIsTimeToDie = false;
        bool IsRegister = false;
        bool bIdentify = true;
        byte[] FPBuffer;
        int RegisterCount = 0;
        const int REGISTER_FINGER_COUNT = 2;

        byte[][] RegTmps = new byte[3][];
        byte[] RegTmp = new byte[2048];
        byte[] CapTmp = new byte[2048];
        int cbCapTmp = 2048;
        int cbRegTmp = 0;
        int iFid = 1;
        Thread captureThread = null;
        //Mysql Connection
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString;
        //Employee ID

        //template fingerprinit bRegister
        FingerprintTemplate template;
        //byte array fingerprint bRegister
        byte[] serialized;
        private int mfpWidth = 0;
        private int mfpHeight = 0;

        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        public Dashboard()
        {
            InitializeComponent();
        }

        private void bnInit_Click(object sender, EventArgs e)
        {
            cmbIdx.Items.Clear();
            int ret = zkfperrdef.ZKFP_ERR_OK;
            if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
            {
                int nCount = zkfp2.GetDeviceCount();
                if (nCount > 0)
                {
                    for (int i = 0; i < nCount; i++)
                    {
                        cmbIdx.Items.Add(i.ToString());
                    }
                    cmbIdx.SelectedIndex = 0;
                    bnInit.Enabled = false;
                    bnFree.Enabled = true;
                    bnOpen.Enabled = true;
                }
                else
                {
                    zkfp2.Terminate();
                    MessageBox.Show("No device connected!");
                }
            }
            else
            {
                MessageBox.Show("Initialize fail, ret=" + ret + " !");
            }
        }

        private void bnFree_Click(object sender, EventArgs e)
        {
            zkfp2.Terminate();
            cbRegTmp = 0;
            bnInit.Enabled = true;
            bnFree.Enabled = false;
            bnOpen.Enabled = false;
            bnClose.Enabled = false;
            bnEnroll.Enabled = false;
            bnVerify.Enabled = false;
            bnIdentify.Enabled = false;
        }

        private void bnOpen_Click(object sender, EventArgs e)
        {
            int ret = zkfp.ZKFP_ERR_OK;
            if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(cmbIdx.SelectedIndex)))
            {
                MessageBox.Show("OpenDevice fail");
                return;
            }
            if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
            {
                MessageBox.Show("Init DB fail");
                zkfp2.CloseDevice(mDevHandle);
                mDevHandle = IntPtr.Zero;
                return;
            }
            bnInit.Enabled = false;
            bnFree.Enabled = true;
            bnOpen.Enabled = false;
            bnClose.Enabled = true;
            bnEnroll.Enabled = true;
            bnVerify.Enabled = true;
            bnIdentify.Enabled = true;
            RegisterCount = 0;
            cbRegTmp = 0;
            iFid = 1;
            for (int i = 0; i < 3; i++)
            {
                RegTmps[i] = new byte[2048];
            }
            byte[] paramValue = new byte[4];
            int size = 4;
            zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
            zkfp2.ByteArray2Int(paramValue, ref mfpWidth);

            size = 4;
            zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
            zkfp2.ByteArray2Int(paramValue, ref mfpHeight);

            FPBuffer = new byte[mfpWidth*mfpHeight];

            captureThread = new Thread(new ThreadStart(DoCapture));
            captureThread.IsBackground = true;
            captureThread.Start();
            bIsTimeToDie = false;
            textRes.Text = "Open success!";

        }


        private void DoCapture()
        {
            while (!bIsTimeToDie)
            {
                cbCapTmp = 2048;
                int ret = zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp);
                if (ret == zkfp.ZKFP_ERR_OK)
                {
                    SendMessage(FormHandle, MESSAGE_CAPTURED_OK, IntPtr.Zero, IntPtr.Zero);
                }
                Thread.Sleep(200);
            }
        }
        record Subject(int Id, string Name, FingerprintTemplate Template) {
          
        }
        Subject Identify(FingerprintTemplate probe, IEnumerable<Subject> candidates)
        {
            var matcher = new FingerprintMatcher(probe);
            Subject match = null;
            double max = Double.NegativeInfinity;
            foreach (var candidate in candidates)
            {
                double similarity = matcher.Match(candidate.Template);
                if (similarity > max)
                {
                    max = similarity;
                    match = candidate;
                }
            }
            double threshold = 40;
            return max >= threshold ? match : null;
        }
        
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MESSAGE_CAPTURED_OK:
                    {
                        MemoryStream ms = new MemoryStream();
                        BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
                        Bitmap bmp = new Bitmap(ms);
                        //Saves in local directory
                        bmp.Save("probe.png", ImageFormat.Png);
                        this.picFPImg.Image = bmp;
                        if (IsRegister)
                        {
                            int ret = zkfp.ZKFP_ERR_OK;
                            int fid = 0, score = 0;

                            ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                            if (zkfp.ZKFP_ERR_OK == ret)
                            {
                                textRes.Text = "This finger was already register by " + fid + "!";
                                return;
                            }
                            if (RegisterCount > 0 && zkfp2.DBMatch(mDBHandle, CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                            {
                                textRes.Text = "Please press the same finger 3 times for the enrollment";
                                return;
                            }
                            Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);

                            String strBase64 = zkfp2.BlobToBase64(CapTmp, cbCapTmp);
                            byte[] blob = zkfp2.Base64ToBlob(strBase64);
                            //Captures Fingerprint via template
                            bmp.Save("probe-" + RegisterCount.ToString() + ".png"  , ImageFormat.Png);
                            Thread.Sleep(500);
                            var image = new FingerprintImage(File.ReadAllBytes("probe-" + RegisterCount.ToString() + ".png"));                   
                            template = new FingerprintTemplate(image);
                            serialized = template.ToByteArray();
                            //Mysql Connection and Insert
                            myConnectionString = "server=localhost;uid=root;" + "database=majetsco";
                            try
                            {
                                conn = new MySqlConnection();
                                conn.ConnectionString = myConnectionString;
                                conn.Open();
                                using (var myCommand = new MySqlCommand("INSERT INTO tb_fingerprints VALUES (@emp_id, @fingerprint)", conn))
                                {
                                    myCommand.Parameters.Add("@emp_id", MySqlDbType.String).Value = textBox_emp_id.Text;
                                    myCommand.Parameters.Add("@fingerprint", MySqlDbType.VarBinary).Value = serialized;
                                    myCommand.ExecuteNonQuery();
                                }
                                //MySqlCommand myCommand = new MySqlCommand(sql, conn);
                                //MySqlDataReader myReader = myCommand.ExecuteReader();
                                conn.Close();
                                MessageBox.Show("Fingerprint Enrolled.");
                            }
                            catch (MySql.Data.MySqlClient.MySqlException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                            RegisterCount++;
                            if (RegisterCount >= REGISTER_FINGER_COUNT)
                            {
                                RegisterCount = 0;
                                if (zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBMerge(mDBHandle, RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp)) &&
                                       zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBAdd(mDBHandle, iFid, RegTmp)))
                                {
                                    iFid++;
                                    textRes.Text = "enroll success! " ;
                                }
                                else
                                {
                                    textRes.Text = "enroll fail, error code=" + ret;
                                }
                                IsRegister = false;
                                return;
                            }
                            else
                            {
                                textRes.Text = "You need to press the " + (REGISTER_FINGER_COUNT - RegisterCount) + " times fingerprint " + serialized;
                            }
                        }
                        else
                        {
                            //if (cbRegTmp <= 0)
                            //{
                            //    textRes.Text = "Please register your finger first!";
                            //    return;
                            //}
                            if (bIdentify)
                            {
                                int ret = zkfp.ZKFP_ERR_OK;
                                int fid = 0, score = 0;
                                ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                                //Mysql Query
                                myConnectionString = "server=localhost;uid=root;" + "database=majetsco";
                                //Similarity Score (SourceAFIS)
                                double similarity = 0.00;
                                String candidate_id = "";
                                try
                                {
                                    conn = new MySqlConnection();
                                    conn.ConnectionString = myConnectionString;
                                    conn.Open();

                                    var myCommand2 = new MySqlCommand("SELECT tbf.emp_id, tbf.fingerprint_byte, tbe.emp_surname, tbe.emp_firstname, " +
                                        "tbe.emp_type FROM tb_fingerprints AS tbf LEFT JOIN tb_employee AS tbe ON tbf.emp_id = tbe.emp_id", conn);
                                    var myReader2 = myCommand2.ExecuteReader();

                                    while (myReader2.Read())
                                    {
                                        var probe = new FingerprintTemplate(
                                        new FingerprintImage(File.ReadAllBytes("probe.png")));
                                        byte[] serialized2 = (byte[])myReader2["fingerprint_byte"];
                                        var template2 = new FingerprintTemplate(serialized2);
                                        var matcher = new FingerprintMatcher(probe);
                                        similarity = matcher.Match(template2); //checks candidate fingerprint and store as similarity score
                                        if (similarity >= 40.00)
                                        {
                                            candidate_id = myReader2.GetString("emp_id");
                                            break;
                                        }
                                    }
                                }
                                catch (MySqlException ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }


                                if (zkfp.ZKFP_ERR_OK == ret)
                                {
                                    textRes.Text = "Identify succ, fid= " + fid + ", score=" + score + "! Similarity Score SourceAFIS: " + similarity + "emp id: " + candidate_id  ;
                                    return;
                                }
                                else
                                {
                                    textRes.Text = "Identify fail, ret= " + ret;
                                    return;
                                }
                            }
                            else
                            {
                                int ret = zkfp2.DBMatch(mDBHandle, CapTmp, RegTmp);
                                if (0 < ret)
                                {
                                    textRes.Text = "Match finger succ, score=" + ret + "!";
                                    return;
                                }
                                else
                                {
                                    textRes.Text = "Match finger fail, ret= " + ret;
                                    return;
                                }
                            }
                        }
                    }
                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormHandle = this.Handle;
        }



        private void CloseDevice()
        {
            if (IntPtr.Zero != mDevHandle)
            {
                bIsTimeToDie = true;
                Thread.Sleep(1000);
                captureThread.Join();
                zkfp2.CloseDevice(mDevHandle);
                mDevHandle = IntPtr.Zero;
            }
        }



        private void bnClose_Click(object sender, EventArgs e)
        {
            CloseDevice();
            RegisterCount = 0;
            Thread.Sleep(1000);
            bnInit.Enabled = false;
            bnFree.Enabled = true;
            bnOpen.Enabled = true;
            bnClose.Enabled = false;
            bnEnroll.Enabled = false;
            bnVerify.Enabled = false;
            bnIdentify.Enabled = false;
        }

        private void bnEnroll_Click(object sender, EventArgs e)
        {
            if (!IsRegister)
            {
                IsRegister = true;
                RegisterCount = 0;
                cbRegTmp = 0;
                textRes.Text = "Please press your finger 3 times!";
            }
        }

        private void bnIdentify_Click(object sender, EventArgs e)
        {
            if (!bIdentify)
            {
                bIdentify = true;
                textRes.Text = "Please press your finger!";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bnVerify_Click(object sender, EventArgs e)
        {
            if (bIdentify)
            {
                bIdentify = false;
                textRes.Text = "Please press your finger!";
            }
        }
    }
}
