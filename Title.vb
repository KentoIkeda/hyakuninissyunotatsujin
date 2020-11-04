Public Class Title
    Dim M_byou As Integer
    Dim M_FadeInFlag As Boolean = True

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Select Case M_FadeInFlag 'フェードインアニメーションの開始
            Case True
                If Me.Opacity = 100 Then
                    Timer1.Enabled = False
                Else
                    Me.Opacity = Me.Opacity + 0.05F '徐々に透明度を下げる
                End If
            Case Else
                If Me.Opacity = 0 Then
                    Timer1.Enabled = False
                    Me.Dispose()
                Else
                    Me.Opacity = Me.Opacity - 0.05F '徐々に透明度を上げる
                End If
        End Select
    End Sub

    Private Sub Title_Click(sender As Object, e As EventArgs) Handles Me.Click
        Timer1.Enabled = False
        Timer2.Enabled = False
        Call MainMenu.Show() 'メインメニューへ
    End Sub

    Private Sub Title_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        Timer2.Enabled = True
        Me.Opacity = 0 '不透明度を０に
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        M_byou = M_byou + 1
        If M_byou = 5 Then
            Call MainMenu.Show() 'メインメニューへ
        End If
    End Sub
End Class