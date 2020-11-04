Public Class nanido_sentaku
    '変数の宣言
    Dim M_h, M_w, M_nanido As Integer

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ControlBox = False
        Me.Text = ""
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        M_h = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height
        M_w = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width
        Me.Size = New Size(M_w, M_h) '画面の最大化
        Me.Location = New Point(0, 0)
        Me.BringToFront()
        'PictureBox及びLabelの位置の設定
        PictureBox1.Location = New Point(30, 30)
        PictureBox2.Location = New Point(30, (M_h / 2) - (PictureBox2.Height) / 2)
        PictureBox3.Location = New Point(30, M_h - 30 - PictureBox3.Height)
        PictureBox4.Location = New Point(M_w - PictureBox4.Width - 20, M_h - PictureBox4.Height - 20)
        Label1.Location = New Point(PictureBox1.Location.X + PictureBox1.Width + 30, PictureBox1.Location.Y + (PictureBox1.Height - Label1.Height) / 2)
        Label2.Location = New Point(PictureBox2.Location.X + PictureBox2.Width + 30, PictureBox2.Location.Y + (PictureBox2.Height - Label2.Height) / 2)
        Label3.Location = New Point(PictureBox3.Location.X + PictureBox3.Width + 30, PictureBox3.Location.Y + (PictureBox3.Height - Label3.Height) / 2)
        For a As Integer = 1 To 4
            Controls("PictureBox" & a).Parent = Me
            Me.Controls("PictureBox" & a).BackColor = Color.Transparent
        Next
        For a As Integer = 1 To 3
            Controls("Label" & a).Parent = Me
            Me.Controls("Label" & a).BackColor = Color.Transparent
        Next
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_034和太鼓「どどん(軽)」.wav")
        M_nanido = 1
        Call game() 'gameの呼び出し
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_034和太鼓「どどん(軽)」.wav")
        M_nanido = 2
        Call game() 'gameの呼び出し
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_034和太鼓「どどん(軽)」.wav")
        M_nanido = 3
        Call game() 'gameの呼び出し
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        gamekeisiki_sentaku.M_sentaku = 2
        My.Application.ApplicationContext.MainForm = gamekeisiki_sentaku
        gamekeisiki_sentaku.Show()
        Me.Close()
    End Sub

    Sub game()
        '次の画面へのデータの引継ぎ及び表示
        game_help.M_sentaku = 2
        My.Application.ApplicationContext.MainForm = game_help
        game_cp.M_nanido = Me.M_nanido
        game_help.Show()
        Me.Close()
    End Sub
End Class