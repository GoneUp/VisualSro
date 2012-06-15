Namespace Navmesh
    Public Class cBMS
        Dim charCount As Integer
        Dim vertCount As Integer
        Dim boneCount As Integer
        Dim faceCount As Integer
        Dim mtlLib As String
        Dim meshGroup As String
        Dim vertices As Single(,)
        Dim normals As Single(,)
        Dim textures As Single(,)
        'char[,] bones;
        Dim faces As Short(,)

        Public Sub LoadBSM(ByVal path As String)


            Dim file() As Byte = frmMain.DataPK2.GetFile(path)
            Dim br As New IO.BinaryReader(New IO.MemoryStream(file))

            Dim filetype As String = (br.ReadChars(8)) '' Filetype ( first 9 Bytes)
            Dim Version As Integer = (br.ReadInt32) '' 110

            If Version = 110 Then

                ' Springe zur position 72 ( Nicht benötigte Header Informationen )
                br.BaseStream.Position += 60

                ' Länge des MeshGroup Namens
                charCount = br.ReadInt32()

                ' Lese Namen
                meshGroup = New String(br.ReadChars(charCount))

                ' Länge des Materiels Namen
                charCount = br.ReadInt32()

                ' Lese  Material Namen
                mtlLib = New String(br.ReadChars(charCount))

                ' Ignoierere Unbekannten Wert ( Int32 = 4bytes ) ?
                br.BaseStream.Position += 4

                ' Lese anzahl der Verticen
                vertCount = br.ReadInt32()

                ' Erstelle Arrays
                vertices = New Single(vertCount - 1, 2) {}
                normals = New Single(vertCount - 1, 2) {}
                textures = New Single(vertCount - 1, 1) {}

                ' Lese Werte
                For i As Integer = 0 To vertCount - 1
                    ' Vertices
                    For j As Integer = 0 To 2
                        vertices(i, j) = br.ReadSingle()
                    Next
                    ' Normal
                    For j As Integer = 0 To 2
                        normals(i, j) = br.ReadSingle()
                    Next
                    ' Textures
                    For j As Integer = 0 To 1
                        textures(i, j) = br.ReadSingle()
                    Next

                    ' Ignoiere 12 Bytes pro Vertices ??
                    br.BaseStream.Position += 12
                Next

                ' Lese Bones/.. (Ignoiere Bones)
                boneCount = br.ReadInt32()

                If boneCount > 0 Then
                    For i As Integer = 0 To boneCount - 1
                        ' b
                        charCount = br.ReadInt32()

                        ' Überspringe
                        br.BaseStream.Position += charCount
                    Next
                    ' Unnötiger Gaymax schrott
                    br.BaseStream.Position += vertCount * 6
                End If

                ' Lese Faces
                faceCount = br.ReadInt32()
                faces = New Short(faceCount - 1, 2) {}

                For i As Integer = 0 To faceCount - 1
                    For j As Integer = 0 To 2
                        faces(i, j) = br.ReadInt16()
                    Next
                Next

                ' Schließe Reader
                br.Close()

            Else

                Console.WriteLine("Invaild Version: {0}", Version)
            End If

        End Sub


    End Class
End Namespace
