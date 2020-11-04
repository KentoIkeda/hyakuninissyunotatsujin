Public Class dictionary
    Dim M_stCurrentDir As String = System.IO.Directory.GetCurrentDirectory() 'カレントディレクトリ取得

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        If WebBrowser1.CanGoBack = True Then
            '前ページに戻る
            WebBrowser1.GoBack()
        ElseIf WebBrowser1.Url = New Uri(M_stCurrentDir & "\ゲーム参照\参照\html\kajin_list.html") Then
            Me.Close()
        End If
    End Sub

    Private Sub dictionary_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        M_sound.StopBGM()
        M_sound.PlayBGM("ゲーム参照\参照\BGM\都JP(メイン).wav")
    End Sub

    Private Sub dictionary_Load(sender As Object, e As EventArgs) Handles Me.Load
        M_sound.PlayBGM("ゲーム参照\参照\BGM\道草(歌人紹介).wav")
        WebBrowser1.Url = New Uri(M_stCurrentDir & "\ゲーム参照\参照\html\kajin_list.html")
    End Sub
End Class
