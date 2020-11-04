'WAVE オーディオ デバイス インターフェイス コントロール
'Copyright (C) 1998-2006, Kentaro HARA. All rights reserved.

Imports System.Runtime.InteropServices
Imports System.IO

Public Class original_seisaku
    Private Declare Function wifGetData Lib "WaveIF.ocx" Alias "wifGetDataAny" (ByVal pDest() As Byte, ByVal pSrc As Integer, ByVal nLen As Integer) As Integer
    Dim M_Buffer() As Byte ' 録音されたデータを受け取るためのバッファ
    Dim M_fileRec As Integer  ' 録音されたデータを保存する一時ファイルのファイル番号
    Dim M_strRecFileName As String ' 一時ファイルのファイル名
    Dim M_WaveSize As Integer ' 録音されたデータのサイズ
    Dim M_count As Integer = 0 '音声は録音済みか否か兼btnRecのクリックは何回目かをカウントするための変数
    Dim M_byoucount As Integer '録音制限30秒タイマー用変数
    Dim M_kamiURL As String '上の句破棄の際の上の句の格納場所
    Dim M_simoURL As String '下の句破棄の際の上の句の格納場所
    Private M_encsjis As System.Text.Encoding 'エンコード用の変数
    Dim M_OpenFileDialog1 As New OpenFileDialog() '画像参照ダイアログ用変数
    Dim M_tategaki As Graphics '縦書描写用変数A
    Dim M_tategaki2 As Graphics '縦書描写用変数B
    Dim M_tategaki3 As Graphics '縦書病者用変数C
    Dim M_Stf As New StringFormat(StringFormatFlags.DirectionVertical Or StringFormatFlags.DirectionRightToLeft) '縦書設定用変数A
    Dim M_Stf2 As New StringFormat(StringFormatFlags.DirectionVertical Or StringFormatFlags.DirectionRightToLeft) '縦書設定用変数B
    Dim M_rect As New RectangleF(10, 30, 290, 450) '縦書位置用変数A
    Dim M_rect2 As New RectangleF(0, 0, 162, 226) '縦書位置用変数B
    Dim M_rect3 As New RectangleF(0, 0, 147, 226) '縦書位置用変数C
    Dim M_fnt As New Font("@HG行書体", 60, FontStyle.Regular) '縦書フォント用変数A
    Dim M_fnt2 As New Font("@HG行書体", 30, FontStyle.Regular) '縦書フォント用変数B
    Dim M_window_h As Integer = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height '画面のサイズ取得縦
    Dim M_window_w As Integer = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width '画面のサイズ取得横
    Dim M_sound As New SoundComposer 'サウンド用変数

    ' ↓WaveInコントロール関係ここから↓

    ' OnBusyFlagChange イベントの処理
    ' Private Sub waveIn_OnBusyFlagChange(sender As System.Object, e As System.EventArgs) Handles ''wavein.OnBusyFlagChange
    'If ('wavein.IsBusy) Then
    ' 録音中であれば、
    ' [保存]、[新規作成] ボタンを無効にします。
    'btnRec.Enabled = False
    'btnSave.Enabled = False
    'btnNew.Enabled = False

    'Else
    ' 録音中でなければ、
    ' WAVE 入力デバイスを閉じます。
    '  'wavein.Close()

    ' [録音]、[保存]、[新規作成] ボタンを有効にします。

    'btnSave.Enabled = True
    'btnNew.Enabled = True

    '  End If

    ' End Sub

    ' OnDone イベントの処理
    'Private Sub waveIn_OnDone(sender As System.Object, e As AxWaveInterfaceLib._DWaveInEvents_OnDoneEvent) Handles 'wavein.OnDone
    ' データが録音されていれば、
    ' If (e.bytesRecorded > 0) Then
    ' 録音されたデータを取得します。
    ' [注意] .NET 環境では、WaveIn.GetData メソッドは使用できません。
    '  Dim pSrc As Integer
    '   pSrc = 'wavein.GetBufferAddress(e.bufferIndex)
    '  wifGetData(M_Buffer, pSrc, e.bytesRecorded)

    'If (UBound(M_Buffer) > (e.bytesRecorded - 1)) Then
    ' 無効なデータが書き込まれないように、配列のサイズを調整します。
    'ReDim Preserve M_Buffer(e.bytesRecorded - 1)

    ' データをファイルに保存します。
    ' If (M_fileRec <> 0) Then
    '    FilePut(M_fileRec, M_Buffer)
    'End If

    ' 配列のサイズを元に戻します。
    ' ReDim M_Buffer('wavein.BufferSize - 1)
    'Else
    ' データをファイルに保存します。
    'If (M_fileRec <> 0) Then
    '   FilePut(M_fileRec, M_Buffer)
    '   End If
    'End If

    ' 録音されたデータ量を保存します。
    '  M_WaveSize = M_WaveSize + e.bytesRecorded
    'End If

    ' Stop メソッドが実行されていなければ、
    'If (e.bStopped = False) Then
    ' 録音用のバッファーを追加します。
    'wavein.AddBuffer(e.bufferIndex)
    'End If
    '  End Sub

    ' 一時ファイルを開く
    Private Sub OpenTmpFile()
        ' 録音用のファイルを開いていなければ、
        If (M_fileRec = 0) Then
            ' 一時ファイル名を取得します。
            M_strRecFileName = My.Computer.FileSystem.GetTempFileName
            ' 録音のための一時ファイルを開きます。
            M_fileRec = FreeFile()
            FileOpen(M_fileRec, M_strRecFileName, OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.LockWrite)
        End If
    End Sub

    ' 一時ファイルを閉じる
    Private Sub CloseTmpFile()
        ' 録音用の一時ファイルを閉じます。
        If (M_fileRec <> 0) Then
            FileClose(M_fileRec)
            M_fileRec = 0
        End If
    End Sub

    ' 一時ファイルを削除する
    Private Sub DeleteTmpFile()
        ' 録音用の一時ファイルを削除します。
        CloseTmpFile()
        If (Dir(M_strRecFileName) <> "") Then
            Kill(M_strRecFileName)
            M_WaveSize = 0
        End If
    End Sub

    ' 初期化
    Private Sub Init()
        ' 初期化を行います。
        ' 一時ファイルを削除します。
        DeleteTmpFile()

        ' 一時ファイルを開きます。
        OpenTmpFile()


        ' [保存] ボタンを無効にします。
        'btnSave.Enabled = False

        ' 変数を初期化します。
        M_WaveSize = 0
    End Sub

    ' WAVE ファイルを保存する
    Private Sub SaveWaveFile(strFileName As String, DataFile As Integer)
        ' 録音されたデータを WAVE ファイルとして保存します。
        ' strFileName : 保存するファイル名。
        ' DataFile    : PCM データが記録されている一時ファイルのファイル番号。

        Dim M_hdr As WAVEFILEHEADER
        Dim M_file As Integer
        Dim M_DataSize As Integer
        Dim M_BytesWritten As Integer
        Dim M_BytesToWrite As Integer
        Const BufferSize = 65536    ' ファイルの読み書きに使用するバッファーのサイズ。
        Dim M_Buffer() As Byte        ' ファイルの読み書きに使用するバッファー。

        ' DataFile が開いていなければ、リターンします。
        If (DataFile = 0) Then
            Exit Sub
        End If

        ' ファイルの先頭へシークします。
        Seek(DataFile, 1)

        ' PCM データのサイズを取得します。
        M_DataSize = LOF(DataFile)

        ' 空いているファイル番号を取得します。
        M_file = FreeFile()

        ' バイナリ書き込みモードでファイルを開きます。
        FileOpen(M_file, strFileName, OpenMode.Binary, OpenAccess.Write)

        ' ファイルに書き込むヘッダ情報を初期化します。
        With M_hdr
            .ckidRIFF = "RIFF"              ' RIFF チャンクの ID。
            .ckSizeRIFF = M_DataSize + 36     ' PCM データのサイズ + 36 バイト。
            .fccType = "WAVE"               ' WAVE ファイルであることを示します。
            .ckidFmt = "fmt "               ' fmt チャンクの ID。
            .ckSizeFmt = 16                 ' fmt チャンクのサイズ。
            With .WaveFmt                   ' フォーマット情報。
                With .wf
                    .wFormatTag = WAVE_FORMAT_PCM               ' フォーマット タグ。
                    '   .nChannels = 'wavein.Channels                ' チャネル数。
                    '  .nSamplesPerSec = 'wavein.SamplesPerSec      ' サンプリング周波数。
                    '   .nAvgBytesPerSec = 'wavein.AvgBytesPerSec    ' 平均データ レート。
                    '   .nBlockAlign = 'wavein.BlockAlign            ' ブロック アラインメント。
                End With
                ' .wBitsPerSample = 'wavein.BitsPerSample          ' 量子化ビット数。
            End With
            .ckidData = "data"              ' data チャンクの ID。
            .ckSizeData = M_DataSize          ' PCM データのサイズ。
        End With

        ' ヘッダ情報を書き込みます。
        FilePut(M_file, M_hdr)

        ' ファイルの書き込みに使用するバッファーを確保します。
        ReDim M_Buffer(BufferSize - 1)

        ' PCM データを書き込みます。
        Do Until EOF(DataFile)
            M_BytesToWrite = Math.Min(BufferSize, (M_DataSize - M_BytesWritten))
            If (M_BytesToWrite = 0) Then
                Exit Do
            End If
            ReDim M_Buffer(M_BytesToWrite - 1)
            FileGet(DataFile, M_Buffer)
            FilePut(M_file, M_Buffer)
            M_BytesWritten = M_BytesWritten + M_BytesToWrite
        Loop

        ' ファイルを閉じます。
        FileClose(M_file)
    End Sub


    ' ↑WaveInコントロール関係ここまで↑


    ' ↓各Subプロシージャここから↓

    '初期化処理
    Private Sub syokika()
        M_count = 0
        Label4.Text = "タップで上の句録音"
        Label4.ForeColor = Color.Black
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        PictureBox1.ImageLocation = ""
        PictureBox2.ImageLocation = ""
        PictureBox3.BackgroundImage = My.Resources.none
        ' 'AxWindowsMediaPlayer1.currentPlaylist.clear()
        ' 'AxWindowsMediaPlayer1.Visible = False
    End Sub

    '画像参照用ダイアログボックスの処理
    Public Sub dialog()
        ' OpenFileDialog の新しいインスタンスを生成する (デザイナから追加している場合は必要ない)

        ' ダイアログのタイトルを設定する
        M_OpenFileDialog1.Title = "画像ファイルを開く"

        ' 初期表示するディレクトリを設定する
        M_OpenFileDialog1.InitialDirectory = "C:\Users\*\Pictures\"

        ' 初期表示するファイル名を設定する
        'M_OpenFileDialog1.FileName = "初期表示するファイル名をココに書く"

        ' ファイルのフィルタを設定する
        M_OpenFileDialog1.Filter = "画像ファイル|*.jpg;*.png;*.png"

        ' ファイルの種類 の初期設定を 2 番目に設定する (初期値 1)
        M_OpenFileDialog1.FilterIndex = 1

        ' ダイアログボックスを閉じる前に現在のディレクトリを復元する (初期値 False)
        M_OpenFileDialog1.RestoreDirectory = True

        ' 複数のファイルを選択可能にする (初期値 False)
        M_OpenFileDialog1.Multiselect = True

        ' [ヘルプ] ボタンを表示する (初期値 False)
        M_OpenFileDialog1.ShowHelp = True

        ' [読み取り専用] チェックボックスを表示する (初期値 False)
        M_OpenFileDialog1.ShowReadOnly = True

        ' [読み取り専用] チェックボックスをオンにする (初期値 False)
        M_OpenFileDialog1.ReadOnlyChecked = True
    End Sub

    'オリジナル書き込み用処理
    Public Sub kakikomi()
        Dim M_kami_kz As Integer = Directory.GetFiles("ゲーム参照\参照\上の句・テキスト").Length
        Dim M_simo_kz As Integer = Directory.GetFiles("ゲーム参照\参照\下の句・テキスト").Length
        Dim M_kamionsei_kz As Integer = Directory.GetFiles("ゲーム参照\参照\上の句・音声").Length
        Dim M_simoonsei_kz As Integer = Directory.GetFiles("ゲーム参照\参照\下の句・音声").Length
        Dim M_gazo_kz As Integer = Directory.GetFiles("ゲーム参照\参照\歌人").Length
        System.IO.File.Copy(TextBox3.Text, "ゲーム参照\参照\歌人\" & M_gazo_kz + 1 & "." & Strings.Right(TextBox3.Text, 3), True)
        Dim M_k_kaminoku As StreamWriter = New StreamWriter("ゲーム参照\参照\上の句・テキスト\" & M_kami_kz + 1 & ".txt", False, System.Text.Encoding.Default)
        Dim M_k_simonoku As StreamWriter = New StreamWriter("ゲーム参照\参照\下の句・テキスト\" & M_simo_kz + 1 & ".txt", False, System.Text.Encoding.Default)
        M_k_kaminoku.Write(TextBox1.Text.ToString)
        M_k_simonoku.Write(TextBox2.Text.ToString)
        M_k_kaminoku.Flush()
        M_k_simonoku.Flush()
        M_k_kaminoku.Close()
        M_k_simonoku.Close()
    End Sub

    'ひらがな入力チェック用処理
    Function Hiragana_Check()
        Return System.Text.RegularExpressions.Regex.IsMatch(TextBox2.Text, "^\p{IsHiragana}+$")
    End Function

    'ひらがな入力チェック用処理２
    Function Hiragana_Check2()
        Return System.Text.RegularExpressions.Regex.IsMatch(TextBox1.Text, "^\p{IsHiragana}+$")
    End Function

    '空白チェック
    Function Null_check()
        Return Not (TextBox1.Text.Length = 0 Or TextBox2.Text.Length = 0 Or TextBox3.Text.Length = 0)
    End Function

    '録音処理
    Private Sub Rec()

    End Sub

    '上の句録音自動ストップ用処理
    Private Sub Rec_stop_kami()
        Dim M_kamionsei_kz As Integer = Directory.GetFiles("ゲーム参照\参照\上の句・音声").Length
        M_kamiURL = "ゲーム参照\参照\上の句・音声\" & M_kamionsei_kz + 1 & ".wav"
        SaveWaveFile(M_kamiURL, M_fileRec)
        Init()
        'wavein.Stop()
        btnRec.Focus()
        'wavein.Close()
    End Sub

    '下の句録音自動ストップ用処理
    Private Sub Rec_stop_simo()
        Dim M_simoonsei_kz As Integer = Directory.GetFiles("ゲーム参照\参照\下の句・音声").Length
        M_simoURL = "ゲーム参照\参照\下の句・音声\" & M_simoonsei_kz + 1 & ".wav"
        SaveWaveFile(M_simoURL, M_fileRec)
        Init()
        'wavein.Stop()
        btnRec.Focus()
        'wavein.Close()
    End Sub

    ' ↑各Subプロシージャここまで↑


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        M_encsjis = System.Text.Encoding.GetEncoding("Shift_JIS") 'エンコード形式の設定

        '各コントロールの配置場所（Location）の確定ここから
        PictureBox1.Left = (M_window_w / 2) - 550
        PictureBox2.Left = (M_window_w / 2) + 200
        PictureBox3.Left = PictureBox1.Left + 90
        PictureBox1.Top = M_window_h / 2 / 2 - 100
        PictureBox2.Top = M_window_h / 2 / 2 - 100
        PictureBox3.Top = PictureBox2.Bottom - PictureBox3.Size.Height - 18
        Label1.Top = PictureBox1.Top - 33
        Label2.Top = PictureBox2.Top - 33
        Label1.Left = PictureBox1.Left
        Label2.Left = PictureBox2.Left
        TextBox1.Top = PictureBox1.Bottom + 20
        TextBox2.Top = PictureBox1.Bottom + 20
        TextBox3.Top = PictureBox1.Bottom + 60
        TextBox1.Left = PictureBox1.Left
        TextBox2.Left = PictureBox2.Left
        TextBox3.Left = PictureBox1.Left
        btnRec.Top = M_window_h / 2 - btnRec.Size.Height / 2
        Button2.Top = TextBox1.Top + TextBox1.Height + 5
        Button4.Top = PictureBox1.Bottom + 100
        Button5.Top = PictureBox1.Bottom + 100
        btnRec.Left = M_window_w / 2 - btnRec.Size.Width / 2
        Button2.Left = PictureBox1.Right - Button2.Size.Width
        Button4.Left = PictureBox1.Right - Button4.Size.Width
        Button5.Left = PictureBox2.Left
        Button6.Left = 1920 - Button6.Width
        kami.Left = PictureBox1.Right - kami.Width - 19
        kami.Top = PictureBox1.Top + 20
        simo.Left = kami.Left - simo.Width
        simo.Top = PictureBox1.Top + 20
        PictureBox4.Left = M_window_w - PictureBox4.Width
        PictureBox4.Top = M_window_h - PictureBox4.Height
        PictureBox6.Left = PictureBox4.Left - PictureBox6.Width
        PictureBox6.Top = M_window_h - PictureBox4.Height
        Button6.Left = 1920 - Button6.Width
        Button6.Top = 0
        ''AxWindowsMediaPlayer1.Left = PictureBox1.Right + 5
        ' 'AxWindowsMediaPlayer1.Top = btnRec.Bottom + 'AxWindowsMediaPlayer1.Height + 5
        Label3.Left = btnRec.Left
        Label3.Top = btnRec.Top - Label3.Height - 20
        Label4.Left = PictureBox1.Right
        Label4.Top = btnRec.Bottom + 10
        '各コントロールの配置場所（Location）の確定ここまで

        Me.BackgroundImage = My.Resources.背景１
        ' 一時ファイルを開きます。
        OpenTmpFile()
        ' 初期化します。
        Init()
    End Sub

    Private Sub Form1_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        If M_count = 3 Then '既に音声双方とも録音済みである場合
            System.IO.File.Delete(M_kamiURL)
            System.IO.File.Delete(M_simoURL)
        ElseIf M_count = 2 Then '上の句のみ録音である場合
            System.IO.File.Delete(M_kamiURL)
        End If
        'wavein.Stop()

        'wavein.Close()

        DeleteTmpFile()
        M_sound.StopBGM()
        M_sound.PlayBGM("ゲーム参照\参照\BGM\都JP(メイン).wav")
    End Sub

    Private Sub btnRec_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnRec.MouseClick
        Timer1.Enabled = True
        If Windows.Forms.MouseButtons.Left Then
            Select Case M_count
                Case 0 '上の句を録音していない場合にタップした場合
                    Label3.Text = "上の句" & vbCr & "録音中"
                    Label4.Text = "タップで録音停止"
                    Label4.ForeColor = Color.Red
                    Label3.Visible = True
                    Label4.Visible = True
                    Timer1.Enabled = True
                    ' 'AxWindowsMediaPlayer1.currentPlaylist.clear()
                    ' 'AxWindowsMediaPlayer1.Visible = False
                    Rec()
                    btnRec.BackgroundImage = My.Resources.録音中1
                    Button4.Enabled = False
                    Button5.Enabled = False
                    Button4.BackgroundImage = My.Resources.touroku_h
                    Button5.BackgroundImage = My.Resources.torikesu_h
                    M_count = 1
                Case 1 '上の句を録音している状態でタップした場合
                    Label4.Text = "タップで下の句録音"
                    Label4.ForeColor = Color.Black
                    Label3.Visible = False
                    M_byoucount = 0
                    btnRec.BackgroundImage = My.Resources.mike
                    Timer1.Enabled = False
                    Rec_stop_kami()
                    MsgBox("引き続き下の句の音声を録音してください。準備ができたら下のボタンを押して下さい。", MsgBoxStyle.Information)
                    M_count = 2
                Case 2 '上の句を録音し終えて待機状態でタップした場合（下の句の録音開始）
                    Timer1.Enabled = True
                    Label4.Text = "タップで録音停止"
                    Label4.ForeColor = Color.Red
                    Label3.Text = "下の句" & vbCr & "録音中"
                    Label3.Visible = True
                    Rec()
                    btnRec.BackgroundImage = My.Resources.録音中1
                    Button4.Enabled = False
                    Button5.Enabled = False
                    Button4.BackgroundImage = My.Resources.touroku_h
                    Button5.BackgroundImage = My.Resources.torikesu_h
                    M_count = 3
                Case 3 '下の句を録音し終えたた状態でタップした場合
                    btnRec.Visible = False
                    Label3.Visible = False
                    Label4.Text = "録音完了！"
                    Label4.ForeColor = Color.Black
                    btnRec.BackgroundImage = My.Resources.mike
                    Timer1.Enabled = False
                    M_byoucount = 0
                    Rec_stop_simo()
                    ''AxWindowsMediaPlayer1.Visible = True
                    ''AxWindowsMediaPlayer1.currentPlaylist.appendItem('AxWindowsMediaPlayer1.newMedia(M_kamiURL))
                    ''AxWindowsMediaPlayer1.currentPlaylist.appendItem('AxWindowsMediaPlayer1.newMedia(M_simoURL))
                    Button4.Enabled = True
                    Button5.Enabled = True
                    Button4.BackgroundImage = My.Resources.登録
                    Button5.BackgroundImage = My.Resources.取り消す
            End Select
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        dialog()
        ' ダイアログを表示し、戻り値が [OK] の場合は、選択したファイルを表示する
        If M_OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            'MessageBox.Show(M_OpenFileDialog1.FileName)
            PictureBox3.BackgroundImage = Image.FromFile(M_OpenFileDialog1.FileName)

            ' Multiselect が True の場合はこのように列挙する
            'For Each nFileName As String In M_OpenFileDialog1.FileNames
            '    MessageBox.Show(nFileName)
            'Next nFileName
            TextBox3.Text = M_OpenFileDialog1.FileName
        End If

        ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
        M_OpenFileDialog1.Dispose()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        If Hiragana_Check() = True And Hiragana_Check2() = True And Null_check() = True And M_count = 3 Then
            kakikomi()
            MsgBox("書き込みが完了しました！")
            'AxWindowsMediaPlayer1.Visible = False
            btnRec.Visible = True
            syokika()
        Else
            MsgBox("書き込みできませんでした！" & vbCr & "次の事を確認してください。" & vbCr & vbCr & "・上の句及び下の句にはひらがな以外の文字を使用していませんか？" & vbCr &
                   "・画像の未指定、未入力の部分はありませんか？" & vbCr & "・音声を録音し忘れてしませんか？")
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim M_kakunin As Integer
        M_kakunin = MsgBox("取り消しますか？音声を録音した一時ファイルは削除されます。", vbYesNo + vbQuestion, "確認")
        If M_kakunin = vbYes Then
            'AxWindowsMediaPlayer1.Visible = False
            btnRec.Visible = True
            If M_count = 3 Then
                System.IO.File.Delete(M_kamiURL)
                System.IO.File.Delete(M_simoURL)
                btnRec.Visible = True
            End If
            syokika()
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        delete.Show()
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = "ぁ" Or e.KeyChar = "ぃ" Or e.KeyChar = "ぅ" Or e.KeyChar = "ぇ" Or e.KeyChar = "ぉ" Or e.KeyChar = "ゃ" Or e.KeyChar = "ゅ" Or e.KeyChar = "ょ" Or e.KeyChar = "っ" Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        kami.Image = New Bitmap(kami.Width, kami.Height)
        M_tategaki = Graphics.FromImage(kami.Image)
        M_tategaki.DrawString(TextBox1.Text, M_fnt2, Brushes.Black, M_rect2, M_Stf)


        Dim z As String = String.Empty
        For Each s As String In Me.TextBox1.Text
            If M_encsjis.GetBytes(s).Length = 2 Then
                z = z & s
            End If
        Next
        Me.TextBox1.Text = z
    End Sub

    Private Sub TextBox1_Click(sender As Object, e As EventArgs) Handles TextBox1.Click
        Shell("explorer.exe C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe")
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = "ぁ" Or e.KeyChar = "ぃ" Or e.KeyChar = "ぅ" Or e.KeyChar = "ぇ" Or e.KeyChar = "ぉ" Or e.KeyChar = "ゃ" Or e.KeyChar = "ゅ" Or e.KeyChar = "ょ" Or e.KeyChar = "っ" Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

        PictureBox2.Image = New Bitmap(PictureBox2.Width, PictureBox2.Height)
        M_tategaki2 = Graphics.FromImage(PictureBox2.Image)
        M_tategaki2.DrawString(TextBox2.Text, M_fnt, Brushes.Black, M_rect, M_Stf)

        simo.Image = New Bitmap(simo.Width, simo.Height)
        M_tategaki3 = Graphics.FromImage(simo.Image)
        M_tategaki3.DrawString(TextBox2.Text, M_fnt2, Brushes.Black, M_rect3, M_Stf)

        Dim z As String = String.Empty
        For Each s As String In Me.TextBox2.Text
            If M_encsjis.GetBytes(s).Length = 2 Then
                z = z & s
            End If
        Next
        Me.TextBox2.Text = z
    End Sub

    Private Sub TextBox2_Click(sender As Object, e As EventArgs) Handles TextBox2.Click
        Shell("explorer.exe C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe")
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        'AxWindowsMediaPlayer1.Visible = False
        btnRec.Visible = True
        If M_count = 3 Then
            System.IO.File.Delete(M_kamiURL)
            System.IO.File.Delete(M_simoURL)
            btnRec.Visible = True
        End If
        syokika()
        Me.Close()
    End Sub

    Private Sub PictureBox6_Click(sender As Object, e As EventArgs) Handles PictureBox6.Click
        M_sound.PlaySE("ゲーム参照\参照\SE\se_025鼓システム効果音1.wav")
        original_help.Show()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        M_byoucount = M_byoucount + 1
        Select Case M_byoucount
            Case 30 And M_count = 1
                Label3.Visible = False
                M_byoucount = 0
                btnRec.BackgroundImage = My.Resources.mike
                Timer1.Enabled = False
                Rec_stop_kami()
                MsgBox("自動録音停止しました。一度に録音できる時間は30秒までです。", MsgBoxStyle.Information)
                MsgBox("引き続き下の句の音声を録音してください。準備ができたら下のボタンを押して下さい。", MsgBoxStyle.Information)
                M_count = 2
            Case 30 And M_count = 3
                Label3.Visible = False
                btnRec.BackgroundImage = My.Resources.mike
                Timer1.Enabled = False
                M_byoucount = 0
                Rec_stop_simo()
                MsgBox("自動録音停止しました。一度に録音できる時間は30秒までです。", MsgBoxStyle.Information)
                'AxWindowsMediaPlayer1.Visible = True
                'AxWindowsMediaPlayer1.currentPlaylist.appendItem('AxWindowsMediaPlayer1.newMedia(M_kamiURL))
                'AxWindowsMediaPlayer1.currentPlaylist.appendItem('AxWindowsMediaPlayer1.newMedia(M_simoURL))
                Button4.Enabled = True
                Button5.Enabled = True
                Button4.BackgroundImage = My.Resources.登録
                Button5.BackgroundImage = My.Resources.取り消す
        End Select
    End Sub

End Class