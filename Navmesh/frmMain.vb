Public Class frmMain

    Dim MyFont As Font = DefaultFont
    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        MyFont = New Font("Arial", 7.5, FontStyle.Regular, GraphicsUnit.Point, 0)
    End Sub
    Public Sub AddLog(ByVal text As String)
        txtLog.Text += text & vbNewLine
        txtLog.SelectionStart = txtLog.TextLength
        txtLog.ScrollToCaret()
    End Sub


    Private Sub cmdLoadPK2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoadPK2.Click
        LoadPK2("C:\Program Files\SilkRoad Online (RSRO) v1.009")
        cmdLoadPK2.Enabled = False
        pnlControls.Enabled = True
    End Sub

    Public MediaPK2 As cPK2Reader
    Public MapPK2 As cPK2Reader
    Public DataPK2 As cPK2Reader
    Public Sub LoadPK2(ByVal Path As String)
        AddLog("Loading Media.pk2...")
        If MediaPK2 Is Nothing Then
            MediaPK2 = New cPK2Reader(Path & "\Media.pk2")
        End If
        AddLog("Loading Data.pk2...")
        If DataPK2 Is Nothing Then
            DataPK2 = New cPK2Reader(Path & "\Data.pk2")
        End If
        AddLog("Loading Map.pk2...")
        If MapPK2 Is Nothing Then
            MapPK2 = New cPK2Reader(Path & "\Map.pk2")
        End If
        '------------------------------------------------------------
        PaintNavmesh()
    End Sub

    Private Sub cmdMoveLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMoveLeft.Click
        txtXSec.Text -= 1
        PaintNavmesh()
    End Sub
    Private Sub cmdMoveRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMoveRight.Click
        txtXSec.Text += 1
        PaintNavmesh()
    End Sub
    Private Sub cmdMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMoveUp.Click
        txtYSec.Text += 1
        PaintNavmesh()
    End Sub
    Private Sub cmdMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMoveDown.Click
        txtYSec.Text -= 1
        PaintNavmesh()
    End Sub
    Private Sub cmdRedraw_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRedraw.Click
        PaintNavmesh()
    End Sub

    Public Sub PaintNavmesh()

        Dim x As Byte = txtXSec.Text
        Dim y As Byte = txtYSec.Text
        Dim s As Single = txtScaleFactor.Text

        Dim SectorArray As New List(Of Navmesh.cSector)

        SectorArray.Add(New Navmesh.cSector(x - 1, y + 1, s)) 'TopLeft
        SectorArray.Add(New Navmesh.cSector(x, y + 1, s)) 'TopMiddle
        SectorArray.Add(New Navmesh.cSector(x + 1, y + 1, s)) 'TopRight

        SectorArray.Add(New Navmesh.cSector(x - 1, y, s)) 'MiddleLeft
        SectorArray.Add(New Navmesh.cSector(x, y, s)) 'MiddleMiddle
        SectorArray.Add(New Navmesh.cSector(x + 1, y, s)) 'MiddleRight

        SectorArray.Add(New Navmesh.cSector(x - 1, y - 1, s)) 'BottomLeft
        SectorArray.Add(New Navmesh.cSector(x, y - 1, s)) 'BottomMiddle
        SectorArray.Add(New Navmesh.cSector(x + 1, y - 1, s)) 'BottomRight

        For Each sector As Navmesh.cSector In SectorArray
            'DrawForEachSector
            Dim g As Graphics = Graphics.FromImage(sector.Picture)

            If (sector.X = x) And (sector.Y = y) Then 'If Middle
                'g.DrawArc(Pens.Red, 85, 85, 4, 4, 0, 360)
                g.DrawRectangle(Pens.Plum, New Rectangle(81, 81, 4, 4))
                'g.FillRectangle(Brushes.Red, New Rectangle(81, 81, 4, 4))
            End If

            If checkDrawImage.Checked = False Then
                g.Clear(Color.Black)
            End If

            If checkDrawXY.Checked Then
                g.DrawString(sector.X & "," & sector.Y, MyFont, Brushes.Red, 0, 0)
            End If

            g.DrawRectangle(Pens.Gray, New Rectangle(0, 0, 169, 169))

            If checkDrawObjects.Checked Then
                For Each e As Navmesh.tNVMEntity In sector.NVM.entries

                    g.DrawArc(Pens.Red, 1920 - e.x, e.y + 5, 5, 5, 0, 360)
                Next
            End If



            g = Nothing
        Next

        Dim Map As New Bitmap(512, 512)
        Dim gMap As Graphics = Graphics.FromImage(Map)

        gMap.DrawImage(SectorArray(0).Picture, 0, 0, 170, 170) 'TopLeft
        gMap.DrawImage(SectorArray(1).Picture, 170, 0, 170, 170) 'TopMiddle
        gMap.DrawImage(SectorArray(2).Picture, 340, 0, 170, 170) 'TopRight

        gMap.DrawImage(SectorArray(3).Picture, 0, 170, 170, 170) 'MiddleLeft
        gMap.DrawImage(SectorArray(4).Picture, 170, 170, 170, 170) 'MiddleMiddle
        gMap.DrawImage(SectorArray(5).Picture, 340, 170, 170, 170) 'MiddleRight

        gMap.DrawImage(SectorArray(6).Picture, 0, 340, 170, 170)
        gMap.DrawImage(SectorArray(7).Picture, 170, 340, 170, 170)
        gMap.DrawImage(SectorArray(8).Picture, 340, 340, 170, 170)

        'Set Image
        picMap.Image = Map

        AddLog("Redraw")
    End Sub

    Private Sub picHover(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles picMap.MouseMove
        Dim XSec As Byte = txtXSec.Text
        Dim YSec As Byte = txtYSec.Text

        Dim XPos As Single = e.X
        Dim YPos As Single = e.Y

        Me.Text = "X:" & XPos & "      " & "Y:" & YPos
    End Sub

    Public Function ToPacketX(ByVal XPos As Single, ByVal XSec As Byte) As Single
        Return ((XPos - ((XSec) - 135) * 192) * 10)
    End Function
    Public Function ToPacketY(ByVal YPos As Single, ByVal YSec As Byte) As Single
        Return ((YPos - ((YSec) - 92) * 192) * 10)
    End Function
    Public Function ToGameX(ByVal XPos As Single, ByVal XSec As Byte) As Single
        Return ((XSec - 135) * 192 + (XPos / 10))
    End Function
    Public Function ToGameY(ByVal YPos As Single, ByVal YSec As Byte) As Single
        Return ((YSec - 92) * 192 + (YPos / 10))
    End Function


End Class
