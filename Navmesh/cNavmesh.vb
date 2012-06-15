Imports System.IO, System.Text

Namespace Navmesh
    Public Class cSector
        Public X As Byte
        Public Y As Byte
        Public ScaleFactor As Single
        Public PictureSize As Integer = 170 '256

        Public NVM As cNVM

        'After Loading stores the Picture
        Private pPicture As Bitmap
        Public ReadOnly Property Picture() As Bitmap
            Get
                Return pPicture
            End Get
        End Property

        Public Sub New(ByVal X As Byte, ByVal Y As Byte, ByVal ScaleFactor As Single)
            Me.X = X
            Me.Y = Y
            Me.ScaleFactor = ScaleFactor

            LoadNavmesh()
            LoadObjects()
            LoadMinimap()
        End Sub

        Private Sub LoadNavmesh()
            '\navmesh\nv_11a4.nvm
            Dim buffer As Byte() = frmMain.DataPK2.GetFile("\navmesh\nv_" & Y.ToString("X2") & X.ToString("X2") & ".nvm")
            If buffer IsNot Nothing Then
                Dim ms As New MemoryStream(buffer)
                Dim br As New BinaryReader(ms)
                Dim nvm As New cNVM
                '############################################################
                'Header
                nvm.company = br.ReadChars(4)
                nvm.format = br.ReadChars(4)
                nvm.version = br.ReadChars(4)
                '############################################################
                'Entrys
                nvm.entryCount = br.ReadUInt16
                Array.Resize(nvm.entries, nvm.entryCount)
                For i = 0 To nvm.entryCount - 1
                    Dim entry As New tNVMEntity
                    entry.id = br.ReadUInt32
                    entry.x = br.ReadSingle
                    entry.y = br.ReadSingle
                    entry.z = br.ReadSingle
                    entry.uk2 = br.ReadInt16
                    entry.Angle = br.ReadSingle
                    entry.uk4 = br.ReadInt16
                    entry.uk5 = br.ReadInt16
                    entry.uk6 = br.ReadInt16
                    entry.grid = br.ReadInt16
                    entry.extraCount = br.ReadUInt16
                    Array.Resize(entry.extraArray, entry.extraCount)
                    For ii = 0 To entry.extraCount - 1
                        entry.extraArray(ii).field1 = br.ReadUInt32
                        entry.extraArray(ii).field2 = br.ReadUInt16
                    Next
                    nvm.entries(i) = entry
                Next
                '############################################################
                'Zone1
                nvm.zone1Count = br.ReadInt32
                Array.Resize(nvm.zone1s, nvm.zone1Count)
                nvm.zone1Extra = br.ReadInt32
                For i = 0 To nvm.zone1Count - 1
                    Dim zone As New tNVMZone1
                    zone.fX1 = br.ReadSingle
                    zone.fY1 = br.ReadSingle
                    zone.fX2 = br.ReadSingle
                    zone.fY2 = br.ReadSingle
                    zone.extraCount = br.ReadByte
                    Array.Resize(zone.extraArray, zone.extraCount)
                    For ii = 0 To zone.extraCount - 1
                        zone.extraArray(ii) = br.ReadUInt16
                    Next
                    nvm.zone1s(i) = zone
                Next
                '############################################################
                'Zone2
                nvm.zone2Count = br.ReadInt32
                Array.Resize(nvm.zone2s, nvm.zone2Count)
                For i = 0 To nvm.zone2Count - 1
                    Dim zone As New tNVMZone2
                    zone.fX1 = br.ReadSingle
                    zone.fY1 = br.ReadSingle
                    zone.fX2 = br.ReadSingle
                    zone.fY2 = br.ReadSingle
                    zone.s3_b1 = br.ReadByte
                    zone.s3_b2 = br.ReadByte
                    zone.s3_b3 = br.ReadByte
                    zone.s3_w2 = br.ReadUInt16
                    zone.s3_w3 = br.ReadUInt16
                    zone.s3_w4 = br.ReadUInt16
                    zone.s3_w5 = br.ReadUInt16
                    nvm.zone2s(i) = zone
                Next
                '############################################################
                'Zone3
                nvm.zone3Count = br.ReadInt32
                Array.Resize(nvm.zone3s, nvm.zone3Count)
                For i = 0 To nvm.zone3Count - 1
                    Dim zone As New tNVMZone3
                    zone.fX1 = br.ReadSingle
                    zone.fY1 = br.ReadSingle
                    zone.fX2 = br.ReadSingle
                    zone.fY2 = br.ReadSingle
                    zone.s3_b1 = br.ReadByte
                    zone.s3_b2 = br.ReadByte
                    zone.s3_b3 = br.ReadByte
                    zone.s3_w2 = br.ReadUInt16
                    zone.s3_w3 = br.ReadUInt16
                    nvm.zone3s(i) = zone
                Next
                '############################################################
                'Other
                br.ReadBytes(73728) 'Texturemap
                Array.Resize(nvm.heightmap, 9408)
                For i = 0 To 9408 - 1
                    nvm.heightmap(i) = br.ReadSingle
                Next
                Dim Rest As Integer = br.BaseStream.Length - br.BaseStream.Position
                'Rest = 3369
                'Beep()
                Me.NVM = nvm
            Else
                frmMain.AddLog("cNavmesh.LoadNavmesh -->Navmesh file not found.(" & X & "," & Y & ")")
            End If
        End Sub
        Private Sub LoadObjects()
            Dim file() As Byte = frmMain.MapPK2.GetFile("\object.ifo")
            Dim lines() As String = Encoding.UTF8.GetString(file).Split(ControlChars.Lf)
            Dim nvm As New cNVM

            If lines(0) <> "JMXVOBJI1000" Then
                frmMain.AddLog("cNavmesh.LoadObjects --> Sig Mismatch")
            End If

            Dim count As Integer = lines(1)

            Array.Resize(nvm.objectMap, count)
            For i = 2 To nvm.objectMap.Count - 1
                Dim tmpString() As String = lines(i).Split(" ")
                Dim tmp As New tObjectMap

                tmp.id = tmpString(0)
                tmp.value = tmpString(1).Replace("0x", "&H")
                tmp.name = tmpString(2).Replace(Chr(34), "") 'replace " with nothing
                nvm.objectMap(i) = tmp
            Next
            Me.NVM.objectMap = nvm.objectMap
        End Sub
        Private Sub LoadMinimap()
            Dim bitmap As Bitmap
            Dim buffer As Byte() = frmMain.MediaPK2.GetFile("\minimap\" & X & "x" & Y & ".ddj")
            If (buffer Is Nothing) Then
                bitmap = New Bitmap(256, 256, Imaging.PixelFormat.Format16bppRgb555)
            Else
                Using stream As Stream = New MemoryStream(buffer, 20, (buffer.GetUpperBound(0) - 19))
                    Dim format As FreeImageAPI.FREE_IMAGE_FORMAT = FreeImageAPI.FREE_IMAGE_FORMAT.FIF_DDS
                    Dim dib As FreeImageAPI.FIBITMAP = FreeImageAPI.FreeImage.LoadFromStream(stream, FreeImageAPI.FREE_IMAGE_LOAD_FLAGS.DEFAULT, (format))
                    bitmap = FreeImageAPI.FreeImage.GetBitmap(dib)
                    FreeImageAPI.FreeImage.UnloadEx((dib))
                End Using
            End If
            pPicture = New Bitmap(PictureSize, PictureSize)
            Dim g As Graphics = Graphics.FromImage(pPicture)
            g.DrawImage(bitmap, 0, 0, PictureSize, PictureSize)
        End Sub

#Region "BSR/BSM Stuff"
        Public Sub LoadBSR()

        End Sub

#End Region


        'Dim num22 As Single = (((Me.sectorsize * num20) + (entrys.Position.X / 10.0!)) * Me.scalefactor)
        'Dim num23 As Single = (((Me.sectorsize + (Me.sectorsize * num21)) - (entrys.Position.Z / 10.0!)) * Me.scalefactor)


    End Class
#Region "##########     Navmesh Structures      ##########"
    Public Class cNVM

        Public company As String '4 Chars
        Public format As String '4 Chars
        Public version As String '4 Chars

        Public entryCount As Short
        Public entries() As tNVMEntity

        Public zone1Count As Integer
        Public zone1Extra As Integer
        Public zone1s() As tNVMZone1

        Public zone2Count As Integer
        Public zone2s() As tNVMZone2

        Public zone3Count As Integer
        Public zone3s() As tNVMZone3

        Public texturemap(96, 96) As TTextureMapEntry
        Public heightmap(9409) As Single
        Public last3 As String
        Public last4(36) As Single

        Public objectMap() As tObjectMap
    End Class

    Public Structure tNVMEntity
        Dim id As UInteger
        Dim x As Single
        Dim y As Single
        Dim z As Single
        Dim uk2 As Short
        'Dim uk3 As Ushort --> Angle
        Dim Angle As Single
        Dim uk4 As Short
        Dim uk5 As Short
        Dim uk6 As Short
        Dim grid As Short
        Dim extraCount As UShort
        Dim extraArray() As tNVMEntityExtra
    End Structure
    Public Structure tNVMEntityExtra
        Dim field1 As UInteger
        Dim field2 As UShort
    End Structure

    Public Structure tNVMZone1
        Dim fX1 As Single
        Dim fY1 As Single
        Dim fX2 As Single
        Dim fY2 As Single
        Dim extraCount As Byte
        Dim extraArray() As UShort
    End Structure
    Public Structure tNVMZone2
        Dim fX1 As Single
        Dim fY1 As Single
        Dim fX2 As Single
        Dim fY2 As Single
        Dim s3_b1 As Byte
        Dim s3_b2 As Byte
        Dim s3_b3 As Byte
        Dim s3_w2 As UShort
        Dim s3_w3 As UShort
        Dim s3_w4 As UShort
        Dim s3_w5 As UShort
    End Structure
    Public Structure tNVMZone3
        Dim fX1 As Single
        Dim fY1 As Single
        Dim fX2 As Single
        Dim fY2 As Single
        Dim s3_b1 As Byte
        Dim s3_b2 As Byte
        Dim s3_b3 As Byte
        Dim s3_w2 As UShort
        Dim s3_w3 As UShort
    End Structure

    Public Structure tTileMap
        Dim id As ULong
        Dim value As ULong
        Dim name As String '2048 Chars
        Dim file As String '2048 Chars
        Dim extra As String '2048 Chars
    End Structure
    Public Structure tObjectMap
        Dim id As ULong
        Dim value As ULong
        Dim name As String '2048 Chars
    End Structure
    Public Structure TTextureMapEntry
        Dim w1 As UShort
        Dim w2 As UShort
        Dim w3 As UShort
        Dim w4 As UShort
    End Structure
#End Region
End Namespace
