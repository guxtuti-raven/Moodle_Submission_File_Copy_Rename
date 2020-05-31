using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
システム動作内容

1. ユーザーがフォルダ, 拡張子を指定
2. 指定フォルダ内のフォルダを検索し、2次元配列0列目に代入
3. フォルダの頭7桁の数字を2次元配列1列目に代入
5. アクセスしたフォルダ内のファイルを(2次元配列1列目).(指定拡張子)へ変更
6. ファイルを1つ前のフォルダへ移動
*/
namespace FolderRename
{
    public partial class Form1 : Form
    {
        string Folder_path = "", extension = "";
        string[, ] Folder_File;
        public Form1()
        {
            this.MaximizeBox = false;
            InitializeComponent();
        }

        private void doing_check_Click(object sender, EventArgs e)
        {
            DialogResult Ques = MessageBox.Show("確認してください!" + "\n" + "現バージョンで行えるファイルのコピーは各学生提出フォルダにつき1つだけです!" + "\n" + "各学生が提出したファイルは1つだけですか?", "CHECK!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Ques == DialogResult.Yes)
            {
                DialogResult check = MessageBox.Show("解凍された & ファイルコピー先のフォルダ: " + Folder_path + "\nファイルの拡張子: " + textBox1.Text + "\n\n以上の設定でデータコピーを行います。\n(同じファイル名, 拡張子名のファイルは上書きされます。)\n\nよろしいですか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (check == DialogResult.Yes)
                {
                    //Phase 1
                    //入力された拡張子を変数に代入
                    extension = textBox1.Text;
                    //Phase 2
                    string[] subFolders = System.IO.Directory.GetDirectories(@Folder_path, "*", System.IO.SearchOption.AllDirectories);
                    Folder_File = new string[subFolders.Length, 2];
                    for (int i = 0; i < subFolders.Length; i++)
                    {
                        Folder_File[i, 0] = subFolders[i];
                        //Phase 3
                        Folder_File[i, 1] = subFolders[i].Substring(Folder_path.Length + 1, 7);
                    }
                    //Phase 5
                    for (int i = 0; i < subFolders.Length; i++)
                    {
                        string[] file_URI = System.IO.Directory.GetFiles(@Folder_File[i, 0], "*", System.IO.SearchOption.AllDirectories);
                        string file_newURI = file_URI[0];
                        for (int j = 0; j < 2; j++)
                        {
                            file_newURI = file_newURI.Substring(0, file_newURI.LastIndexOf("\\"));
                        }
                        file_newURI = file_newURI + "\\" + Folder_File[i, 1] + "." + extension;
                        System.IO.File.Copy(file_URI[0], @file_newURI, true);
                    }
                    MessageBox.Show("各フォルダ内のファイルコピーが完了しました。", "Finish", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(Folder_path != "" && textBox1.Text != "")
            {
                doing_check.Enabled = true;
                timer1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Phase 1
            //フォルダを指定する
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "フォルダを指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                Folder_path = fbd.SelectedPath;
            }
        }
    }
}
