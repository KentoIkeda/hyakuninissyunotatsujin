Public Class original_help
    Dim M_oh1 As Image = My.Resources.orihelpA 'ヘルプ画像A
    Dim M_oh2 As Image = My.Resources.orihelpB 'ヘルプ画像B

    Private Sub Form15_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PictureBox1.Left = 1920 - PictureBox1.Width
        PictureBox1.Top = 1080 - PictureBox1.Height
        PictureBox2.Image = M_oh1
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        Me.Close()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        If PictureBox2.Image Is M_oh1 Then
            PictureBox2.Image = M_oh2
            PictureBox3.BackgroundImage = My.Resources.前へ
        Else
            PictureBox2.Image = M_oh1
            PictureBox3.BackgroundImage = My.Resources.次へ
        End If
    End Sub
End Class