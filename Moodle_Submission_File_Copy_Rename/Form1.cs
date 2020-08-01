using System;
using System.Windows.Forms;

namespace Moodle_Submission_File_Copy_Rename
{
    public partial class Form1 : Form
    {
        string Folder_path = "", extension = "";
        int Try_Parse = 0, timer_count = 0, Success = 0, Failed = 0;
        public Form1()
        {
            MaximizeBox = false;
            InitializeComponent();
        }
        private void doing_check_Click(object sender, EventArgs e)
        {
            DialogResult Ques = MessageBox.Show("確認してください!" + "\n" + "現バージョンで行えるファイルのコピーは各学生提出フォルダにつき1つだけです!" + "\n" + "各学生が提出したファイルは1つだけですか?", "CHECK!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Ques == DialogResult.Yes)
            {
                DialogResult check = MessageBox.Show("解凍された & ファイルコピー先のフォルダ: " + Folder_path + "\n\n以上の設定でデータコピーを行います。\n(同じファイル名, 拡張子名のファイルは上書きされます。)\n\nよろしいですか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (check == DialogResult.Yes)
                    Copy_Data();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "フォルダを指定してください。";
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                Folder_path = fbd.SelectedPath;
                label1.Text = Folder_path;
                doing_check.Enabled = true;
            }
        }
        private void Copy_Data()
        {
            string[] subFolders = System.IO.Directory.GetDirectories(@Folder_path, "*", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < subFolders.Length; i++)
            {
                try
                {
                    string do_FOLDER = subFolders[i];
                    string[] file_URI = System.IO.Directory.GetFiles(@subFolders[i], "*", System.IO.SearchOption.AllDirectories);
                    bool all_check = true;
                    for (int j = 0; j <= 7; j++)
                    {
                        if (j == 7)
                        {
                            string space = subFolders[i].Substring(subFolders[i].LastIndexOf("\\") + 1 + j, 1);
                            if (space != " ")
                                all_check = false;
                        }
                        else
                        {
                            bool parse_check = int.TryParse(subFolders[i].Substring(subFolders[i].LastIndexOf("\\") + 1 + j, 1), out Try_Parse);
                            if (parse_check == false)
                                all_check = false;
                        }
                    }
                    if (all_check)
                    {
                        int student_number = int.Parse(subFolders[i].Substring(subFolders[i].LastIndexOf("\\") + 1, 7));
                        extension = file_URI[0].Substring(file_URI[0].LastIndexOf("."));
                        string COPY_File_URI = Folder_path + "\\" + student_number + extension;
                        System.IO.File.Copy(file_URI[0], @COPY_File_URI, true);
                        Success++;
                    }
                    else
                        Copy_Failed(i + 1, subFolders[i]);
                }
                catch
                {
                    Copy_Failed(i + 1, subFolders[i]);
                }
            }
            MessageBox.Show("ファイルコピーが完了しました。\n" +
                "成功: " + Success + "件\n" +
                "失敗: " + Failed + "件\n", "Finish", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
        private void Copy_Failed(int i, string ERROR_FOLDER)
        {
            ERROR_FOLDER = ERROR_FOLDER.Substring(Folder_path.Length + 1);
            MessageBox.Show(i + "個目のフォルダで問題が発生したため、ファイルコピーが行えませんでした。\nフォルダ名などを確認してください。\nフォルダ名の先頭が学籍番号7桁の数字になっているか, 下記フォルダの中に保存されているファイル拡張子が \".\"で区切られていることを確認してください。\n"
                + "\n" + "エラー発生フォルダ名: " + ERROR_FOLDER, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Failed++;
        }
    }
}