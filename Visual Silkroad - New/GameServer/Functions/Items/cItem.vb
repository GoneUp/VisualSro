Imports GameServer.GameServer.GameDB, GameServer.GameServer.Functions
Public Class cInventory

    Sub New(ByVal slots As Integer)
        ReDim UserItems(slots)
        For i = 0 To UserItems.Length - 1
            UserItems(i) = New cInvItem
        Next
    End Sub

    Sub ReOrderItems(ByVal index_ As Integer)
        Dim now As ULong = DateTime.Now.Ticks
        For A = 0 To AllItems.Length - 1
            For B = 0 To UserItems.Length - 1
                If AllItems(A).OwnerCharID = UserItems(B).OwnerCharID And AllItems(A).Slot = UserItems(B).Slot Then
                    AllItems(A) = UserItems(B)
                    Exit For
                End If
            Next
        Next
        Dim past As Long = DateTime.Now.Ticks - now
        Debug.Print("[Item Reorder][Time: " & past & "ms]")

        ReDim UserItems(PlayerData(index_).MaxSlots)
        For i = 0 To UserItems.Length - 1
            UserItems(i) = New cInvItem
        Next


        For I = 0 To (AllItems.Length - 1)
            If AllItems(I).OwnerCharID = PlayerData(index_).CharacterId Then
                Me.UserItems(AllItems(I).Slot) = AllItems(I)
            End If
        Next

    End Sub

    Sub CalculateItemCount()
        ItemCount = 0
        For i = 0 To UserItems.Length - 1
            If UserItems(i).Pk2Id <> 0 Then
                ItemCount += 1
            End If
        Next
    End Sub

    Public UserItems(255) As cInvItem
    Public ItemCount As Byte

End Class

Public Class cItem


    Public ITEM_TYPE As UInteger
    Public ITEM_TYPE_NAME As String
    Public ITEM_MALL As Byte
    Public CLASS_A As Byte
    Public CLASS_B As Byte
    Public CLASS_C As Byte
    Public RACE As Byte
    Public SHOP_PRICE As ULong
    Public MIN_REPAIR As UShort
    Public MAX_REPAIR As UShort
    Public STORE_PRICE As ULong
    Public SELL_PRICE As ULong
    Public LV_REQ As Byte
    Public REQ1 As Integer
    Public REQ1_LV As Byte
    Public REQ2 As Integer
    Public REQ2_LV As Byte
    Public REQ3 As Integer
    Public REQ3_LV As Byte
    Public MAX_POSSES As Integer
    Public MAX_STACK As UShort
    Public GENDER As Byte
    Public MIN_DURA As Single
    Public MAX_DURA As Single
    Public MIN_PHYSDEF As Double
    Public MAX_PHYSDEF As Double
    Public PHYSDEF_INC As Double
    Public MIN_PARRY As Single
    Public MAX_PARRY As Single
    Public MIN_ABSORB As Double
    Public MAX_ABSORB As Double
    Public ABSORB_INC As Double
    Public MIN_BLOCK As Single
    Public MAX_BLOCK As Single
    Public MAGDEF_MIN As Double
    Public MAGDEF_MAX As Double
    Public MAGDEF_INC As Double
    Public MIN_APHYS_REINFORCE As Single
    Public MAX_APHYS_REINFORCE As Single
    Public MIN_AMAG_REINFORCE As Single
    Public MAX_AMAG_REINFORCE As Single
    Public ATTACK_DISTANCE As Single
    Public MIN_LPHYATK As Double
    Public MAX_LPHYATK As Double
    Public MIN_HPHYATK As Double
    Public MAX_HPHYATK As Double
    Public PHYATK_INC As Double
    Public MIN_LMAGATK As Double
    Public MAX_LMAGATK As Double
    Public MIN_HMAGATK As Double
    Public MAX_HMAGATK As Double
    Public MAGATK_INC As Double
    Public MIN_LPHYS_REINFORCE As Single
    Public MAX_LPHYS_REINFORCE As Single
    Public MIN_HPHYS_REINFORCE As Single
    Public MAX_HPHYS_REINFORCE As Single
    Public MIN_LMAG_REINFORCE As Single
    Public MAX_LMAG_REINFORCE As Single
    Public MIN_HMAG_REINFORCE As Single
    Public MAX_HMAG_REINFORCE As Single
    Public MIN_ATTACK_RATING As Single
    Public MAX_ATTACK_RATING As Single
    Public MIN_CRITICAL As Single
    Public MAX_CRITICAL As Single
    Public USE_TIME_HP As Integer  ' steht drin wieviel HP ein potion heilt           //*******************************
    Public USE_TIME_HP_PER As Integer ' steht drin wieviel prozent HP ein grain heilt    //* Das hier muss wahrscheinlich
    Public USE_TIME_MP As Integer ' steht drin wieviel MP ein potion heilt           //* umbenannt werden, USE_TIME
    Public USE_TIME_MP_PER As Integer ' steht drin wieviel prozent MP ein grain heilt    //* passt nicht ganz.


End Class

Public Class cInvItem

    ' Properties
    Public Property bluestatCount() As Byte
        Get
            Return Me.ibluestatCount
        End Get
        Set(ByVal value As Byte)
            Me.ibluestatCount = value
            Dim statArray As sBlueStat()
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
    Public OwnerCharID As Integer = 0
    Public Slot As Integer = 0
    Public Amount As Long = 0
    Public Durability As Integer = 30
    Private ibluestatCount As Byte
    Private ibluestats As sBlueStat()
    Public Pk2Id As Long = 0
    Public Plus As Byte = 0
    Public DatabaseID As UInteger = 0

    ' Nested Types
    Public Structure sBlueStat
        Public objectTypID As UInt32
        Public amount As UInt32
    End Structure
End Class



