Imports System.Runtime.InteropServices

Public Class cInvItem

    ' Properties
    Public Property bluestatCount() As Byte
        Get
            Return Me.ibluestatCount
        End Get
        Set(ByVal value As Byte)
            Me.ibluestatCount = value
            Dim statArray As sBlueStat() = DirectCast(Utils.Utils.CopyArray(DirectCast(Me.bluestats, Array), New sBlueStat(((value - 1) + 1) - 1) {}), sBlueStat())
            Me.bluestats = statArray
        End Set
    End Property

    Public Property bluestats() As sBlueStat()
        Get
            Return Me.ibluestats
        End Get
        Set(ByVal value As sBlueStat())
            Me.ibluestats = value
        End Set
    End Property


    ' Fields
    Public postition As Integer = 0
    Public amount As Integer = 0
    Public durability As Integer = 0
    Private ibluestatCount As Byte
    Private ibluestats As sBlueStat()
    Public objectTypID As Long = 0
    Public pluslevel As Byte = 0

    ' Nested Types
    <Serializable(), StructLayout(LayoutKind.Sequential)> _
    Public Structure sBlueStat
        Public objectTypID As UInt32
        Public amount As UInt32
    End Structure
End Class


