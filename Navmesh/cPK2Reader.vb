Imports System.IO
Imports System.Text

Public Class cPK2Reader

    Implements IDisposable

    Dim pfile As String

    'Pserver fish
    'Dim fisch As New BlowfishNET.BlowfishECB(New Byte() {&H31, &HC8, &HD4, &H7D, &H4C, &H73}, 0, 6)

    'Orginal fish
    Dim fisch As New BlowfishNET.BlowfishECB(New Byte() {&H32, &HCE, &HDD, &H7C, &HBC, &HA8}, 0, 6)

    Dim Directory As New Generic.Dictionary(Of String, cFile)

    Public Class cFile
        Public Pos As UInt32
        Public size As UInt32
    End Class
    Structure sPk2EntryPrivate
        Dim Typ As Byte
        Dim Name As String
        Dim PosLow As UInt32
        Dim PosHigh As UInt32
        Dim Size As UInt32
        Dim NextChain As UInt32
    End Structure

    Public Sub New(ByVal Pk2Filepath As String)

        pfile = Pk2Filepath
        Dim fs As New FileStream(Pk2Filepath, FileMode.Open, FileAccess.Read)

        Dim r As New BinaryReader(fs)
        r.BaseStream.Position = 0
        Directory.Clear()
        r.ReadChars(30)
        r.ReadUInt32()
        r.ReadUInt32()
        r.BaseStream.Position += 218 '00-00...

        'first entry
        Dim buffer As Byte() = r.ReadBytes(128) '128
        fisch.DecryptRev(buffer, 0, buffer, 0, 131)

        Using rr As New BinaryReader(New MemoryStream(buffer))
            Dim typ1 As Byte = rr.ReadByte
            Dim name As String = rr.ReadChars(81)
            rr.ReadBytes(24)
            Dim posl As UInt32 = rr.ReadUInt32()
            Dim posh As UInt32 = rr.ReadUInt32()
            Dim size As UInt32 = rr.ReadUInt32()
            Dim nextc As UInt32 = rr.ReadUInt32()
        End Using


        Try


            Do While processfile(r.BaseStream.Position, r, buffer)
            Loop
        Catch ex As Exception

        End Try

        r.Close()

    End Sub

    Dim level As String = "\"

    Function processfile(ByVal startpos As UInt32, ByRef r As BinaryReader, ByRef buffer As Byte()) As Boolean

        Dim Entry As New sPk2EntryPrivate
        r.BaseStream.Position = startpos


        buffer = r.ReadBytes(128) '128
        If buffer.GetUpperBound(0) <> &H80 - 1 Then
            Return False
        End If
        fisch.DecryptRev(buffer, 0, buffer, 0, 131)

        Using rr As New BinaryReader(New MemoryStream(buffer))
            Entry.Typ = rr.ReadByte
            For i As Int32 = 1 To 81
                Dim b As Byte = rr.ReadByte()
                If b >= &H20 And b <= &H7E Then
                    Entry.Name = Entry.Name & ChrW(b)
                Else
                    rr.BaseStream.Position = 82
                    Exit For
                End If
            Next

            rr.BaseStream.Position += 24
            Entry.PosLow = rr.ReadUInt32()
            Entry.PosHigh = rr.ReadUInt32()
            Entry.Size = rr.ReadUInt32()
            Entry.NextChain = rr.ReadUInt32()
        End Using
        ' If level.StartsWith("\res\") Then
        'Console.WriteLine(Entry.PosLow & "  " & Entry.PosHigh & "  " & r.BaseStream.Position & "   " & level & Entry.Name)
        ' End If

        If Entry.Typ = 2 Then 'file
            If Entry.Size <> 0 Then
                If Entry.Name <> "" Then
                    Dim file As New cFile
                    file.Pos = Entry.PosLow
                    file.size = Entry.Size
                    If Directory.ContainsKey(level & Entry.Name) Then
                        Console.WriteLine("error")
                        Return False
                    End If
                    Directory.Add(level & Entry.Name, file)
                End If
            End If
        End If



        If Entry.Typ = 0 Then
            Return False
        End If

        If Entry.Typ = 1 And Entry.Name <> "." And Entry.Name <> ".." Then
            level = level & Entry.Name & "\"
            level = level.ToLower
            Dim CurrentPos As UInt32 = r.BaseStream.Position
            r.BaseStream.Position = Entry.PosLow
            Do While processfile(r.BaseStream.Position, r, buffer)
            Loop
            r.BaseStream.Position = CurrentPos
            level = level.Remove(Len(level) - Len(Entry.Name) - 1, Len(Entry.Name) + 1)
        End If

        If Entry.NextChain Then
            r.BaseStream.Position = Entry.NextChain
        End If
        If r.BaseStream.Position = Entry.PosLow Then
            Return False
        End If


        Return True
    End Function
    Public Function GetFile(ByVal file As cFile) As Byte()
        Dim bfile() As Byte

        Using fs2 As New FileStream(pfile, FileMode.Open, FileAccess.Read)
            Using r As New BinaryReader(fs2)
                r.BaseStream.Position = file.Pos
                bfile = r.ReadBytes(file.size)
            End Using
        End Using
        Return bfile
    End Function
    Public Function GetFile(ByVal path As String) As Byte()
        Dim bfile() As Byte
        path = path.ToLower
        'For Each s As String In Directory.Keys

        '    If s.StartsWith("\res") Then
        '        Console.WriteLine(s)
        '    End If
        'Next

        If Directory.ContainsKey(path) = False Then
            '\navmesh\nv_5c87.nvm
            Return Nothing
        End If

        Using fs2 As New FileStream(pfile, FileMode.Open, FileAccess.Read)
            Using r As New BinaryReader(fs2)
                r.BaseStream.Position = Directory(path).Pos
                bfile = r.ReadBytes(Directory(path).size)
            End Using
        End Using
        Return bfile
    End Function
    Public Function GetDirectory() As Generic.Dictionary(Of String, cFile)
        Return Directory
    End Function
    Private disposedValue As Boolean = False        ' So ermitteln Sie überflüssige Aufrufe
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: Anderen Zustand freigeben (verwaltete Objekte).
            End If
            Directory = Nothing
            fisch = Nothing
            ' TODO: Eigenen Zustand freigeben (nicht verwaltete Objekte).
            ' TODO: Große Felder auf NULL festlegen.
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(ByVal disposing As Boolean) Bereinigungscode ein.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
