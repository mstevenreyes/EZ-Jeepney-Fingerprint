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
        bool bTimeIn = false;
        bool bTimeOut = false;
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
        MySqlConnection conn;
        String myConnectionString = "server=31.220.110.151;uid=u809948736_ezjeepney_root;" + "database=u809948736_ezjeepney;password=4Qw!tKtj!VXXf$";
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
            //cmbIdx.Items.Clear();
            //int ret = zkfperrdef.ZKFP_ERR_OK;
            //if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
            //{
            //    int nCount = zkfp2.GetDeviceCount();
            //    if (nCount > 0)
            //    {
            //        for (int i = 0; i < nCount; i++)
            //        {
            //            cmbIdx.Items.Add(i.ToString());
            //        }
            //        cmbIdx.SelectedIndex = 0;
            //        bnInit.Enabled = false;
            //        bnFree.Enabled = true;
            //        bnOpen.Enabled = true;
            //    }
            //    else
            //    {
            //        zkfp2.Terminate();
            //        MessageBox.Show("No device connected!");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Initialize fail, ret=" + ret + " !");
            //}
        }

        private void bnFree_Click(object sender, EventArgs e)
        {
            //zkfp2.Terminate();
            //cbRegTmp = 0;
            //bnInit.Enabled = true;
            //bnFree.Enabled = false;
            //bnOpen.Enabled = false;
            //bnClose.Enabled = false;
            //bnEnroll.Enabled = false;
            //bnVerify.Enabled = false;
            //bnTimeIn.Enabled = false;
        }

        private void bnOpen_Click(object sender, EventArgs e)
        {
            //Init Code

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
            //============
            ret = zkfp.ZKFP_ERR_OK;

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
            bnTimeIn.Enabled = true;
            bnTimeOut.Enabled = true;

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
            textRes.Text = "Fingerprint Scanner Opened.";

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
                            if (RegisterCount > 0 && zkfp2.DBMatch(mDBHandle, CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                            {
                                textRes.Text = "Scan your same finger please.";
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
                            //Checks if finger already registered
                            if (RegisterCount < 0)
                            {
                                double registeredCheck = 0.00;
                                conn = new MySqlConnection();
                                conn.ConnectionString = myConnectionString;
                                conn.Open();
                                var checkRegistered = new MySqlCommand("SELECT fingerprint_byte FROM tb_fingerprints", conn);
                                var checkRegisterRead = checkRegistered.ExecuteReader();
                                while (checkRegisterRead.Read())
                                {
                                    var probe = new FingerprintTemplate(
                                                new FingerprintImage(File.ReadAllBytes("probe.png")));
                                    byte[] fingerByteArr = (byte[])checkRegisterRead["fingerprint_byte"];
                                    var fingerTemplate = new FingerprintTemplate(fingerByteArr);
                                    var registerMatch = new FingerprintMatcher(probe);
                                    registeredCheck = registerMatch.Match(fingerTemplate); //checks candidate fingerprint and store as similarity score
                                    if (registeredCheck >= 40.00)
                                    {
                                        MessageBox.Show("Finger already registered in the database.");
                                        return;
                                    }
                                }
                                conn.Close();
                            }
                            //Mysql Connection and Insert
                            myConnectionString = "server=31.220.110.151;uid=u809948736_ezjeepney_root;" + "database=u809948736_ezjeepney;password=4Qw!tKtj!VXXf$";
                            try
                            {
                                conn = new MySqlConnection();
                                conn.ConnectionString = myConnectionString;
                                conn.Open();
                                using (var myCommand = new MySqlCommand("INSERT INTO tb_fingerprints(emp_id, fingerprint_byte) VALUES (@emp_id, @fingerprint)", conn))
                                {
                                    myCommand.Parameters.Add("@emp_id", MySqlDbType.String).Value = textBox_emp_id.Text;
                                    myCommand.Parameters.Add("@fingerprint", MySqlDbType.VarBinary).Value = serialized;
                                    myCommand.ExecuteNonQuery();
                                }
                                conn.Close();
                            }
                            catch (MySqlException ex)
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
                                    textRes.Text = "Fingerprint Succesfully enrolled. " ;
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
                                textRes.Text = "Scan your finger " + (REGISTER_FINGER_COUNT - RegisterCount) + " more time.";
                            }
                        }
                        else
                        {
                          
                            if (bTimeIn)
                            {
                                int ret = zkfp.ZKFP_ERR_OK;
                                int fid = 0, score = 0;
                                ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                                //Mysql Query
                                
                                //Similarity Score (SourceAFIS)
                                double similarity = 0.00;
                                String candidate_id = "";
                                String candidate_name = "";           
                                //Checks fingerprint exist in db
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
                                                candidate_name = myReader2.GetString("emp_firstname") + " " + myReader2.GetString("emp_surname");
                                               
                                            break;
                                            }
                                        }
                                        conn.Close();
                                    }
                                    catch (MySqlException ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                        if (similarity >= 40.000)
                                {
                                    bTimeIn = false;
                                    conn = new MySqlConnection();
                                    conn.ConnectionString = myConnectionString;
                                    conn.Open();
                                    //Checks if user timed in already
                                    var checkTimedCmd = new MySqlCommand("SELECT * FROM tb_attendance_sheet WHERE emp_id = @emp_id AND attendance_date = @att_date", conn);
                                    checkTimedCmd.Parameters.Add("@emp_id", MySqlDbType.String).Value = candidate_id;
                                    checkTimedCmd.Parameters.Add("@att_date", MySqlDbType.String).Value = DateTime.Now.ToString("yyyy-MM-dd");
                                    var checkTimedRdr = checkTimedCmd.ExecuteReader();
                                    if (checkTimedRdr.Read())
                                    {
                                        MessageBox.Show("User already Timed-in.");
                                        conn.Close();
                                        break;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            conn = new MySqlConnection();
                                            conn.ConnectionString = myConnectionString;
                                            conn.Open();
                                            using (var timeInCommand = new MySqlCommand("INSERT INTO tb_attendance_sheet(emp_id, time_in, attendance_date) VALUES (@emp_id,  @time_in, @att_date)", conn))
                                            {
                                                timeInCommand.Parameters.Add("@emp_id", MySqlDbType.String).Value = candidate_id;
                                                timeInCommand.Parameters.Add("@time_in", MySqlDbType.String).Value = DateTime.Now.ToString("HH:mm:ss");
                                                timeInCommand.Parameters.Add("@att_date", MySqlDbType.String).Value = DateTime.Now.ToString("yyyy-MM-dd");
                                                timeInCommand.ExecuteNonQuery();
                                            }
                                            MessageBox.Show("Successful! Time-in Details: " + Environment.NewLine + "Time: " + DateTime.Now.ToString("HH:mm:ss") + Environment.NewLine + "Employee: " + candidate_name
                                           + " (" + candidate_id + ")");
                                            conn.Close();
                                        }
                                        catch (MySqlException ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    bTimeIn = false;
                                    MessageBox.Show("Fingerprint Not Registered.");
                                    
                                }
                            }
                            if (bTimeOut)
                            {
                                int ret = zkfp.ZKFP_ERR_OK;
                                //Mysql Query
                                myConnectionString = "server=31.220.110.151;uid=u809948736_ezjeepney_root;" + "database=u809948736_ezjeepney;password=4Qw!tKtj!VXXf$";
                                //Similarity Score (SourceAFIS)
                                double similarity = 0.00;
                                String candidate_id = "";
                                String candidate_name = "";
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
                                            candidate_name = myReader2.GetString("emp_firstname") + " " + myReader2.GetString("emp_surname");
                                            break;
                                        }
                                    }
                                }
                                catch (MySqlException ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                if (similarity >= 40.000)
                                {
                                    bTimeOut = false;
                                    conn = new MySqlConnection();
                                    conn.ConnectionString = myConnectionString;
                                    conn.Open();
                                    //Checks if user already timed out
                                    var checkOutCmd = new MySqlCommand("SELECT * FROM tb_attendance_sheet WHERE emp_id = @emp_id AND attendance_date = @att_date AND time_out NOT LIKE @time_out", conn);
                                    checkOutCmd.Parameters.Add("@emp_id", MySqlDbType.String).Value = candidate_id;
                                    checkOutCmd.Parameters.Add("@time_out", MySqlDbType.String).Value = "00:00:00.000000";
                                    checkOutCmd.Parameters.Add("@att_date", MySqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
                                    var checkOutRdr = checkOutCmd.ExecuteReader();
                                    if (checkOutRdr.Read())
                                    {
                                        MessageBox.Show("User already Timed-out.");
                                        conn.Close();
                                        break;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            conn = new MySqlConnection();
                                            conn.ConnectionString = myConnectionString;
                                            conn.Open();
                                            using (var timeOutCommand = new MySqlCommand("UPDATE tb_attendance_sheet SET time_out = @time_out WHERE emp_id = @emp_id AND attendance_date = @att_date", conn))
                                            {
                                                timeOutCommand.Parameters.Add("@emp_id", MySqlDbType.String).Value = candidate_id;
                                                timeOutCommand.Parameters.Add("@time_out", MySqlDbType.String).Value = DateTime.Now.ToString("HH:mm:ss");
                                                timeOutCommand.Parameters.Add("@att_date", MySqlDbType.Date).Value = DateTime.Now.ToString("yyyy-MM-dd");
                                                timeOutCommand.ExecuteNonQuery();
                                            }
                                            MessageBox.Show("Successful! Time-out Details: " + DateTime.Now.ToString("HH:mm:ss") + Environment.NewLine + "Employee: " + candidate_name
                                            + " (" + candidate_id + ")");
                                            conn.Close();
                                        }
                                        catch (MySqlException ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    bTimeOut = false;
                                    MessageBox.Show("Fingerprint Not Registered.");

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
            //Mysql Query for custom source Employee ID
            myConnectionString = "server=31.220.110.151;uid=u809948736_ezjeepney_root;" + "database=u809948736_ezjeepney;password=4Qw!tKtj!VXXf$";
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();

                var myCommand3 = new MySqlCommand("SELECT emp_id FROM tb_employee", conn);
                var myReader3 = myCommand3.ExecuteReader();
                AutoCompleteStringCollection myCollection = new AutoCompleteStringCollection();


                while (myReader3.Read())
                {
                    myCollection.Add(myReader3.GetString(0));   
                }

                textBox_emp_id.AutoCompleteCustomSource = myCollection;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            //Timer-Date Ticker
            timer1.Start();
            //Enable Open
            bnOpen.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToLongTimeString();
            date.Text = DateTime.Now.ToLongDateString();
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
            bnTimeIn.Enabled = false;
            bnTimeOut.Enabled = false;

            //Finalize Event
            zkfp2.Terminate();
            cbRegTmp = 0;
            bnInit.Enabled = true;
            bnFree.Enabled = false;
            bnOpen.Enabled = true;
            bnClose.Enabled = false;
            bnEnroll.Enabled = false;
            bnVerify.Enabled = false;
            bnTimeIn.Enabled = false;
            textRes.Text = "Fingerprint Device Terminated.";
            //=
            textBox_emp_id.Visible = false;
            empIdLabel.Visible = false;
            IsRegister = false;

        }

        private void bnEnroll_Click(object sender, EventArgs e)
        {
            if (!IsRegister)
            {
                IsRegister = true;
                RegisterCount = 0;
                cbRegTmp = 0;
                textBox_emp_id.Visible = true;
                empIdLabel.Visible = true;
                textRes.Text = "Please place your finger on the scanner";
            }
        }

        private void bnIdentify_Click(object sender, EventArgs e)
        {
            textBox_emp_id.Visible = false;
            empIdLabel.Visible = false;
            IsRegister = false;
            textRes.Text = "Scan your finger to time-in:";
            if (!bTimeIn)
            {
                bTimeIn = true;
                bTimeOut = false;
                textRes.Text = "Scan your finger to time-in:";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void bnVerify_Click(object sender, EventArgs e)
        {
            if (bTimeIn)
            {
                bTimeIn = false;
                textRes.Text = "Please press your finger!";
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bnTimeOut_Click(object sender, EventArgs e)
        {
            textBox_emp_id.Visible = false;
            empIdLabel.Visible = false;
            IsRegister = false;
            if (!bTimeOut)
            {
                bTimeOut = true;
                bTimeIn = false;
                textRes.Text = "Scan your finger to time-out:";
            }
        }

        private void textRes_TextChanged(object sender, EventArgs e)
        {

        }

        private void Time_Click(object sender, EventArgs e)
        {

        }
    }
}
