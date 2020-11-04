Imports System.Media
Imports System.IO

Public Class MainMenu
    Dim M_h, M_w As Integer '画面縦横の取得変数

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        M_sound.PlayBGM("ゲーム参照\参照\BGM\都JP(メイン).wav")

        M_h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height
        M_w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width

        PictureBox4.Left = M_w - PictureBox4.Width
        PictureBox4.Top = M_h - PictureBox4.Height

        PictureBox3.Left = M_w / 2 - (PictureBox3.Width / 2)
        PictureBox3.Top = M_h / 2 - (PictureBox1.Height / 2)

        PictureBox1.Left = PictureBox3.Left - PictureBox1.Width - 50
        PictureBox1.Top = M_h / 2 - (PictureBox1.Height / 2)

        PictureBox2.Left = PictureBox3.Left + PictureBox2.Width + 50
        PictureBox2.Top = M_h / 2 - (PictureBox1.Height / 2)

    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        M_sound.StopBGM()
        M_sound.PlaySE("ゲーム参照\参照\SE\se_035和太鼓「どん からか」.wav")
        System.Threading.Thread.Sleep(2000)
        Me.Close()
        Title.Close()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        M_sound.StopBGM()
        dictionary.Show()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        M_sound.StopBGM()
        original_seisaku.Show()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        game_sentaku.Show()
    End Sub

End Class
