Imports System.Net.Sockets
Imports System.Text
Public Class Form1
    Dim clientSocket As New System.Net.Sockets.TcpClient()
    Dim serverStream As NetworkStream
    Dim pasta As String
    Dim last_str As String
    Dim caminho_da_pasta As String = "C:\Sistema_de_visao"
    Dim caminho_do_arquivo As String = caminho_da_pasta + "\config.cfg"
    Dim OKNG As String

    Private Sub Button1_Click(ByVal sender As System.Object,
        ByVal e As System.EventArgs) Handles Button1.Click
        ' clientSocket.Connect(IPTextBox.Text, 8500)

        Dim arquivo As System.IO.StreamWriter
        arquivo = My.Computer.FileSystem.OpenTextFileWriter(caminho_do_arquivo, False) 'True = adiciona novas linhas. False = sobrescreve todo o conteúdo
        'arquivo.WriteLine(TextBoxPasta.Text)
        Dim doc_texto As New System.Text.StringBuilder()
        doc_texto.AppendLine(TextBoxPasta.Text)
        doc_texto.AppendLine(IPTextBox.Text)
        doc_texto.AppendLine(PortTextBox.Text)
        arquivo.WriteLine(doc_texto.AppendLine)
        arquivo.Close()

        Dim config As String = System.IO.File.ReadAllLines(caminho_do_arquivo)(0)
        If config = "Insira Aqui o caminho da pasta com as imagens" Then
            MsgBox("Caminho inválido, selecione pasta correta.")
        End If

        If Button3.Text IsNot "Conectar" Then

            Dim serverStream As NetworkStream = clientSocket.GetStream()
            Dim outStream As Byte() = System.Text.Encoding.ASCII.GetBytes("T2" & vbCrLf)
            serverStream.Write(outStream, 0, outStream.Length)
            serverStream.Flush()
            Label1.Text = "Aguardando resposta: "
            msg("Aguardando resposta: ")
            Dim inStream(10024) As Byte
            'serverStream.Read(inStream, 0, CInt(clientSocket.ReceiveBufferSize))
            serverStream.Read(inStream, 0, CInt(inStream.Length))
            Dim returndata As String =
        System.Text.Encoding.ASCII.GetString(inStream)
            msg("Dados Recebidos: " + returndata)
        End If

        Try
            System.Threading.Thread.Sleep(1500)
            Dim fi As New System.IO.DirectoryInfo(TextBoxPasta.Text)
            'Dim files = fi.GetFiles.ToList
            Dim files = fi.GetFiles("*.jpeg") '.FirstOrDefault
            Dim first = (From file In files Select file Order By file.CreationTime Ascending).FirstOrDefault
            Dim last = (From file In files Select file Order By file.CreationTime Descending).FirstOrDefault
            'MsgBox(last.ToString)
            last_str = last.ToString
            PictureBox1.Image = Image.FromFile(TextBoxPasta.Text & "\" & last_str)
            GroupBox1.Text = last_str
            Dim status As String = last_str.Remove(0, 10)
            Dim Status2 As String = status.Remove(2, 21)
            GroupBox1.Text = Status2
            OKNG = Status2
            TextBoxPasta.Enabled = False
        Catch ex As Exception
            PictureBox1.Image = Nothing
            TextBoxPasta.Enabled = True
        End Try
        If OKNG = "OK" Then
            GroupBox1.BackColor = Color.LightGreen
            Label3.Text = OKNG
        ElseIf OKNG = "NG" Then
            GroupBox1.BackColor = Color.Red
            Label3.Text = OKNG
        End If

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not System.IO.Directory.Exists(caminho_da_pasta) Then
            System.IO.Directory.CreateDirectory(caminho_da_pasta)
        End If
        If Not System.IO.File.Exists(caminho_do_arquivo) Then
            System.IO.File.Create(caminho_do_arquivo).Dispose()
            Dim arquivo As System.IO.StreamWriter
            arquivo = My.Computer.FileSystem.OpenTextFileWriter(caminho_do_arquivo, False) 'True = adiciona novas linhas. False = sobrescreve todo o conteúdo
            Dim doc_texto As New System.Text.StringBuilder()
            doc_texto.AppendLine("Insira Aqui o caminho da pasta com as imagens")
            doc_texto.AppendLine("192.168.100.1")
            doc_texto.AppendLine("8500")
            doc_texto.AppendLine("000;001;006;010")
            arquivo.WriteLine(doc_texto.AppendLine)
            arquivo.Close()
            arquivo.Close()
        End If
        Dim config As String = System.IO.File.ReadAllLines(caminho_do_arquivo)(0)
        If config = "Insira Aqui o caminho da pasta com as imagens" Then
            MsgBox("Pasta Não Encontrada, inserir caminho da pasta com as imagens")
            TextBoxPasta.Text = config
            TextBoxPasta.Enabled = True
        End If
        If config <> "Insira Aqui o caminho da pasta com as imagens" Then

            TextBoxPasta.Enabled = False
            Dim pasta As String = System.IO.File.ReadAllLines(caminho_do_arquivo)(0)
            TextBoxPasta.Text = pasta
        End If
        ' clientSocket.Connect(IPTextBox.Text, PortTextBox.Text)
        PictureBox1.Image = Nothing
        Label3.Text = ""
        msg("Programa Iniciado")
        IPTextBox.Text = System.IO.File.ReadAllLines(caminho_do_arquivo)(1)
        PortTextBox.Text = System.IO.File.ReadAllLines(caminho_do_arquivo)(2)
        Label1.Text = "Aguardando conexão..."
        Try
            Dim lista = System.IO.File.ReadAllLines(caminho_do_arquivo)(3)
            Dim programa = lista.Split(";")
            For i = 0 To UBound(programa)

                ComboBox1.Items.Add(programa(i))
            Next i
        Catch ex As Exception
            MsgBox("Programas não encontrados.")
        End Try

    End Sub

    Sub msg(ByVal mesg As String)
        RichTextBox1.Text = RichTextBox1.Text + " >> " + mesg & vbCrLf
        RichTextBox1.ScrollToCaret()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object,
        ByVal e As System.EventArgs) Handles Button1.Click
        ' clientSocket.Connect(IPTextBox.Text, 8500)

        If Button3.Text IsNot "Conectar" Then
            Dim serverStream As NetworkStream = clientSocket.GetStream()
            Dim outStream As Byte() = System.Text.Encoding.ASCII.GetBytes("PW," & ComboBox1.SelectedItem & vbCrLf)
            serverStream.Write(outStream, 0, outStream.Length)
            serverStream.Flush()
            Label1.Text = "Aguardando resposta: "
            msg("Aguardando resposta: ")
            Dim inStream(10024) As Byte
            'serverStream.Read(inStream, 0, CInt(clientSocket.ReceiveBufferSize))
            serverStream.Read(inStream, 0, CInt(inStream.Length))
            Dim returndata As String =
            System.Text.Encoding.ASCII.GetString(inStream)
            msg("Dados Recebidos: " + returndata)
            System.Threading.Thread.Sleep(100)
            Dim serverStream2 As NetworkStream = clientSocket.GetStream()
            Dim outStream2 As Byte() = System.Text.Encoding.ASCII.GetBytes("T2" & vbCrLf)
            serverStream2.Write(outStream2, 0, outStream2.Length)
            serverStream2.Flush()
            msg("Enviando: " & "PW," & ComboBox1.SelectedItem & vbCrLf)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "Conectar" Then
            msg("Tentando conexão...")
            Label1.Text = "Tentando conexão..."

            Try
                clientSocket.Connect(IPTextBox.Text, 8500)

                Button3.Text = "Desconectar"
                Label1.Text = "Conectado à câmera..."
            Catch ex As Exception
                msg("Erro Ao conectar à câmera.")
                Label1.Text = "Erro ao conectar " & vbCrLf & " Verifique o IP/Porta"
                MsgBox("Erro Ao conectar à câmera.", vbOKOnly, "Erro 0x00000001")
            End Try
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Button4.Text = "Salvar" Then
            TextBoxPasta.Enabled = False
            Button4.Text = "Alterar pasta"

            Dim arquivo As System.IO.StreamWriter
            arquivo = My.Computer.FileSystem.OpenTextFileWriter(caminho_do_arquivo, False) 'True = adiciona novas linhas. False = sobrescreve todo o conteúdo
            Dim doc_texto As New System.Text.StringBuilder()
            doc_texto.AppendLine(TextBoxPasta.Text)
            doc_texto.AppendLine(IPTextBox.Text)
            doc_texto.AppendLine(PortTextBox.Text)
            arquivo.WriteLine(doc_texto.AppendLine)
            arquivo.Close()
        ElseIf Button4.Text = "Alterar pasta" Then
            TextBoxPasta.Enabled = True
            Button4.Text = "Salvar"
        End If

    End Sub
End Class
