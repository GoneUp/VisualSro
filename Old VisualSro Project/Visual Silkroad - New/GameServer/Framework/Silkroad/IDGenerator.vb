Namespace Id_Gen
    Module IDGenerator
        '====================Unique ID's
        Private m_uniqueIdCounter As UInteger = 1

        Public Function GetUnqiueId() As UInteger
            Dim toreturn As UInteger = m_uniqueIdCounter
            If m_uniqueIdCounter < UInteger.MaxValue Then
                m_uniqueIdCounter += 1
            ElseIf m_uniqueIdCounter = UInteger.MaxValue Then
                m_uniqueIdCounter = 0
                Log.WriteSystemLog("Reached UniqueId Max")
            End If

            Return toreturn
        End Function

        '=================Charcater Id's
        Public Function GetNewCharId() As UInteger
            For newID As UInteger = 1 To UInteger.MaxValue
                Dim free As Boolean = True
                For c = 0 To GameDB.Chars.Count - 1
                    If GameDB.Chars(c) IsNot Nothing Then
                        If GameDB.Chars(c).CharacterId = newID Then
                            free = False
                        End If
                    End If
                Next

                If free = True Then
                    Return newID
                End If
            Next

            Throw New Exception("Cannot find a new Char ID!")
        End Function

        '======================Skill Over ID's
        Private m_skillOverIdCounter As UInteger = 10

        Public Function GetSkillOverId() As UInteger
            Dim toreturn As UInteger = m_skillOverIdCounter
            If m_skillOverIdCounter < UInteger.MaxValue Then
                m_skillOverIdCounter += 1
            ElseIf m_skillOverIdCounter = UInteger.MaxValue Then
                m_skillOverIdCounter = 0
                Log.WriteSystemLog("Reached SkillOverId Max")
            End If

            Return toreturn
        End Function

        '======================Casting ID's
        Private m_castingIdCounter As UInteger = 10

        Public Function GetCastingId() As UInteger
            Dim toreturn As UInteger = m_castingIdCounter
            If m_castingIdCounter < UInteger.MaxValue Then
                m_castingIdCounter += 1
            ElseIf m_castingIdCounter = UInteger.MaxValue Then
                m_castingIdCounter = 0
                Log.WriteSystemLog("Reached CastingID Max")
            End If

            Return toreturn
        End Function

        '======================Exchange ID's
        Private m_exchangeIdCounter As UInteger = 10

        Public Function GetExchangeId() As UInteger
            Dim toreturn As UInteger = m_exchangeIdCounter
            If m_exchangeIdCounter < UInteger.MaxValue Then
                m_exchangeIdCounter += 1
            ElseIf m_exchangeIdCounter = UInteger.MaxValue Then
                m_exchangeIdCounter = 0
                Log.WriteSystemLog("Reached ExcahngeID Max")
            End If

            Return toreturn
        End Function

        '======================Stall ID's
        Private m_stallIdCounter As UInteger = 10

        Public Function GetStallId() As UInteger
            Dim toreturn As UInteger = m_stallIdCounter
            If m_stallIdCounter < UInteger.MaxValue Then
                m_stallIdCounter += 1
            ElseIf m_stallIdCounter = UInteger.MaxValue Then
                m_stallIdCounter = 0
                Log.WriteSystemLog("Reached StallID Max")
            End If

            Return toreturn
        End Function

        '======================Party ID's
        Private m_partyIdCounter As UInteger = 10

        Public Function GetPartyId() As UInteger
            Dim toreturn As UInteger = m_partyIdCounter
            If m_partyIdCounter < UInteger.MaxValue Then
                m_partyIdCounter += 1
            ElseIf m_partyIdCounter = UInteger.MaxValue Then
                m_partyIdCounter = 0
                Log.WriteSystemLog("Reached PartyId Max")
            End If

            Return toreturn
        End Function

        '======================Item ID's
        Private m_itemIdCounter As UInt64 = 2

        Public Function GetItemId() As UInt64
            Dim toreturn As UInt64 = m_itemIdCounter
            If m_itemIdCounter < UInt64.MaxValue Then
                m_itemIdCounter += 1
            ElseIf m_itemIdCounter = UInt64.MaxValue Then
                m_itemIdCounter = 0
                Log.WriteSystemLog("Reached ItemID Max!!! FATAL")
                Server.Stop()
            End If

            Return toreturn
        End Function

        Public Sub SetItemInitalValue(ByVal id As UInt64)
            m_itemIdCounter = ID + 1
        End Sub

        '======================Skillset ID's

        Public Function GetSkillsetId() As UInt32
            Dim toreturn As UInt32 = 1
            For i = 0 To GameDB.SkillSets.Keys.Count - 1
                Dim key As UInt32 = GameDB.SkillSets.Keys(i)
                If GameDB.SkillSets(key).SetID >= toreturn Then
                    toreturn = GameDB.SkillSets(key).SetID + 1
                End If
            Next
            Return toreturn
        End Function


        '======================ChatItemLink ID's
        Private m_chatLinkIdCounter As UInt64 = 2

        Public Function GetChatLinkID() As UInt64
            Dim toreturn As UInt64 = m_chatLinkIdCounter
            If m_chatLinkIdCounter < UInt64.MaxValue Then
                m_chatLinkIdCounter += 1
            ElseIf m_chatLinkIdCounter = UInt64.MaxValue Then
                m_chatLinkIdCounter = 0
                Log.WriteSystemLog("Reached ChatLinkID Max!!!")
            End If

            Return toreturn
        End Function




    End Module
End Namespace